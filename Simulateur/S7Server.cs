using System.Net;
using System.Net.Sockets;

namespace SimulateurAutomate;

/// <summary>
/// Serveur S7comm minimal (ISO-on-TCP, port 102 par défaut) suffisant pour S7.Net :
/// connexion COTP, négociation PDU, lecture/écriture d'octets et de bits (DB, M, I, Q).
/// Ne reproduit PAS les fonctions de service (SZL, programmation, etc.).
/// </summary>
public sealed class S7Server
{
    private readonly PlcMemory _mem;
    private TcpListener _listener;
    private CancellationTokenSource _cts;
    private readonly List<TcpClient> _clients = new();
    private readonly object _lock = new();

    public int Port { get; private set; } = 102;
    public bool Running => _listener != null;
    /// <summary>Si true : les connexions entrantes sont refusées / coupées (simulation câble débranché).</summary>
    public volatile bool LiaisonCoupee;
    /// <summary>Latence artificielle ajoutée à chaque réponse (ms).</summary>
    public volatile int LatenceMs;
    /// <summary>Si true : le serveur lit les requêtes mais ne répond pas (simulation CPU figé).</summary>
    public volatile bool NePasRepondre;

    public long NbRequetes;
    public int NbClients { get { lock (_lock) return _clients.Count(c => c.Connected); } }
    public event Action<string> Log;

    public S7Server(PlcMemory mem) { _mem = mem; }

    public void Start(int port)
    {
        if (_listener != null) return;
        Port = port;
        _listener = new TcpListener(IPAddress.Any, port);
        _listener.Start();
        _cts = new CancellationTokenSource();
        var token = _cts.Token;
        Task.Run(() => AcceptLoop(token));
        Log?.Invoke($"Serveur S7 démarré sur le port {port}");
    }

    public void Stop()
    {
        if (_listener == null) return;
        _cts.Cancel();
        try { _listener.Stop(); } catch { }
        _listener = null;
        DropAll();
        Log?.Invoke("Serveur S7 arrêté");
    }

    public void DropAll()
    {
        lock (_lock)
        {
            foreach (var c in _clients) { try { c.Close(); } catch { } }
            _clients.Clear();
        }
    }

    private async Task AcceptLoop(CancellationToken token)
    {
        while (!token.IsCancellationRequested)
        {
            TcpClient c;
            try { c = await _listener.AcceptTcpClientAsync(token); }
            catch { break; }
            if (LiaisonCoupee) { try { c.Close(); } catch { } continue; }
            c.NoDelay = true;
            lock (_lock) _clients.Add(c);
            Log?.Invoke("Client connecté : " + c.Client.RemoteEndPoint);
            _ = Task.Run(() => ClientLoop(c, token));
        }
    }

    private static bool ReadExact(NetworkStream s, byte[] buf, int n)
    {
        int got = 0;
        while (got < n)
        {
            int r = s.Read(buf, got, n - got);
            if (r <= 0) return false;
            got += r;
        }
        return true;
    }

    private void ClientLoop(TcpClient c, CancellationToken token)
    {
        string who = "?";
        try { who = c.Client.RemoteEndPoint.ToString(); } catch { }
        try
        {
            var s = c.GetStream();
            var head = new byte[4];
            while (!token.IsCancellationRequested)
            {
                if (!ReadExact(s, head, 4)) break;
                if (head[0] != 3) break;
                int len = (head[2] << 8) | head[3];
                if (len < 7) break;
                var body = new byte[len - 4];
                if (!ReadExact(s, body, body.Length)) break;
                if (LiaisonCoupee) break;
                Interlocked.Increment(ref NbRequetes);
                if (NePasRepondre) continue;
                if (LatenceMs > 0) Thread.Sleep(LatenceMs);

                byte[] resp = Handle(body);
                if (resp != null) s.Write(resp, 0, resp.Length);
            }
        }
        catch { }
        finally
        {
            try { c.Close(); } catch { }
            lock (_lock) _clients.Remove(c);
            Log?.Invoke("Client déconnecté : " + who);
        }
    }

    // ───────────────────────── Protocole ─────────────────────────
    private static byte[] Tpkt(byte[] payload)
    {
        var r = new byte[payload.Length + 4];
        r[0] = 3; r[1] = 0; r[2] = (byte)(r.Length >> 8); r[3] = (byte)r.Length;
        Array.Copy(payload, 0, r, 4, payload.Length);
        return r;
    }

    private byte[] Handle(byte[] body)
    {
        // body = COTP + S7
        int cotpLen = body[0];
        byte pduType = body[1];
        if (pduType == 0xE0) // Connection Request → Connection Confirm
        {
            // On renvoie les paramètres du client (TSAP, taille TPDU) tels quels.
            // Structure CR : LI, 0xE0, dst(2), src(2), class(1), params...
            var cc = new List<byte> { 0x00, 0xD0, body[4], body[5], 0x00, 0x01, 0x00 };
            int paramsStart = 7;
            for (int i = paramsStart; i < 1 + cotpLen && i < body.Length; i++) cc.Add(body[i]);
            cc[0] = (byte)(cc.Count - 1);
            return Tpkt(cc.ToArray());
        }
        if (pduType != 0xF0 || body.Length < 3 + 10) return null; // Data TPDU attendu

        var s7 = body.AsSpan(3).ToArray();
        if (s7[0] != 0x32 || s7[1] != 0x01) return null;          // Job uniquement
        int pduRef = (s7[4] << 8) | s7[5];
        int paramLen = (s7[6] << 8) | s7[7];
        int dataLen = (s7[8] << 8) | s7[9];
        var prm = s7.AsSpan(10, paramLen).ToArray();
        var dat = s7.AsSpan(10 + paramLen, Math.Min(dataLen, s7.Length - 10 - paramLen)).ToArray();

        switch (prm[0])
        {
            case 0xF0: return SetupComm(pduRef, prm);
            case 0x04: return ReadVar(pduRef, prm);
            case 0x05: return WriteVar(pduRef, prm, dat);
            default: return AckData(pduRef, new byte[] { prm[0] }, Array.Empty<byte>(), 0x81, 0x04); // fonction non supportée
        }
    }

    private static byte[] AckData(int pduRef, byte[] prm, byte[] dat, byte errClass = 0, byte errCode = 0)
    {
        var p = new List<byte> { 0x02, 0xF0, 0x80 };
        p.AddRange(new byte[] { 0x32, 0x03, 0, 0, (byte)(pduRef >> 8), (byte)pduRef,
            (byte)(prm.Length >> 8), (byte)prm.Length, (byte)(dat.Length >> 8), (byte)dat.Length, errClass, errCode });
        p.AddRange(prm);
        p.AddRange(dat);
        return Tpkt(p.ToArray());
    }

    private static byte[] SetupComm(int pduRef, byte[] prm)
    {
        int req = prm.Length >= 8 ? (prm[6] << 8) | prm[7] : 240;
        int pdu = Math.Min(Math.Max(req, 240), 960);
        var r = new byte[] { 0xF0, 0x00, 0x00, 0x01, 0x00, 0x01, (byte)(pdu >> 8), (byte)pdu };
        return AckData(pduRef, r, Array.Empty<byte>());
    }

    private sealed class Item
    {
        public byte Transport; public int Count; public int Db; public byte Area; public int Addr;
        public int ByteOff => Addr >> 3;
        public int BitOff => Addr & 7;
    }

    private static List<Item> ParseItems(byte[] prm)
    {
        var l = new List<Item>();
        int n = prm[1];
        int p = 2;
        for (int k = 0; k < n && p + 12 <= prm.Length; k++, p += 12)
        {
            l.Add(new Item
            {
                Transport = prm[p + 3],
                Count = (prm[p + 4] << 8) | prm[p + 5],
                Db = (prm[p + 6] << 8) | prm[p + 7],
                Area = prm[p + 8],
                Addr = (prm[p + 9] << 16) | (prm[p + 10] << 8) | prm[p + 11],
            });
        }
        return l;
    }

    private byte[] ReadVar(int pduRef, byte[] prm)
    {
        var items = ParseItems(prm);
        var dat = new List<byte>();
        for (int k = 0; k < items.Count; k++)
        {
            var it = items[k];
            bool bit = it.Transport == 0x01;
            int len = bit ? 1 : it.Count;
            var data = _mem.Read(it.Area, it.Db, it.ByteOff, len);
            if (data == null)
            {
                dat.AddRange(new byte[] { 0x05, 0x00, 0x00, 0x00 }); // adresse invalide
                continue;
            }
            if (bit)
            {
                dat.AddRange(new byte[] { 0xFF, 0x03, 0x00, 0x01, (byte)((data[0] >> it.BitOff) & 1) });
            }
            else
            {
                int bits = len * 8;
                dat.AddRange(new byte[] { 0xFF, 0x04, (byte)(bits >> 8), (byte)bits });
                dat.AddRange(data);
            }
            if (dat.Count % 2 == 1 && k < items.Count - 1) dat.Add(0);
        }
        return AckData(pduRef, new byte[] { 0x04, (byte)items.Count }, dat.ToArray());
    }

    private byte[] WriteVar(int pduRef, byte[] prm, byte[] dat)
    {
        var items = ParseItems(prm);
        var ret = new byte[items.Count];
        int p = 0;
        for (int k = 0; k < items.Count; k++)
        {
            var it = items[k];
            if (p + 4 > dat.Length) { ret[k] = 0x05; continue; }
            byte transport = dat[p + 1];
            int lenField = (dat[p + 2] << 8) | dat[p + 3];
            int nbytes = transport == 0x03 ? 1 : (transport == 0x09 ? lenField : (lenField + 7) / 8);
            if (p + 4 + nbytes > dat.Length) { ret[k] = 0x05; break; }
            var data = dat.AsSpan(p + 4, nbytes).ToArray();
            bool ok;
            if (it.Transport == 0x01 || transport == 0x03)
                ok = _mem.WriteBitFromClient(it.Area, it.Db, it.ByteOff, it.BitOff, (data[0] & 1) != 0);
            else
                ok = _mem.WriteFromClient(it.Area, it.Db, it.ByteOff, data);
            ret[k] = ok ? (byte)0xFF : (byte)0x05;
            p += 4 + nbytes;
            if (nbytes % 2 == 1) p++; // octet de remplissage
        }
        return AckData(pduRef, new byte[] { 0x05, (byte)items.Count }, ret);
    }
}
