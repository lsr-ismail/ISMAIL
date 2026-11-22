using System.Buffers.Binary;

namespace SimulateurAutomate;

/// <summary>Zones mémoire de l'automate simulé (DB, mémentos M, entrées I, sorties Q). Thread-safe.</summary>
public sealed class PlcMemory
{
    public const int Db1Size = 1240;      // l'ERP lit exactement 1236 octets (TAILLE_DB1)
    public const int Db71Size = 3600;     // l'ERP lit 3440 octets (TAILLE_DB71)

    private readonly object _lock = new();
    private readonly Dictionary<int, byte[]> _db = new();
    private readonly byte[] _m = new byte[1024];
    private readonly byte[] _i = new byte[256];
    private readonly byte[] _q = new byte[256];

    /// <summary>Déclenché pour chaque écriture venant d'un CLIENT (l'ERP) : db (-1 = M/I/Q), offset, longueur.</summary>
    public event Action<string, int, int, byte[]> ClientWrite;

    public PlcMemory()
    {
        _db[1] = new byte[Db1Size];
        _db[71] = new byte[Db71Size];
        foreach (int n in new[] { 8, 18, 19, 101 }) _db[n] = new byte[1024];
    }

    public IEnumerable<int> DbNumbers { get { lock (_lock) return _db.Keys.OrderBy(k => k).ToArray(); } }
    public int DbSize(int db) { lock (_lock) return _db.TryGetValue(db, out var b) ? b.Length : 0; }

    private byte[] Area(byte area, int db)
    {
        return area switch
        {
            0x84 => _db.TryGetValue(db, out var b) ? b : null,
            0x83 => _m,
            0x81 => _i,
            0x82 => _q,
            _ => null,
        };
    }

    public static string AreaName(byte area, int db) => area switch
    {
        0x84 => "DB" + db,
        0x83 => "M",
        0x81 => "I",
        0x82 => "Q",
        _ => "?" + area.ToString("X2"),
    };

    /// <summary>Lecture brute (client S7). Retourne null si hors zone.</summary>
    public byte[] Read(byte area, int db, int offset, int length)
    {
        lock (_lock)
        {
            var a = Area(area, db);
            if (a == null || offset < 0 || length < 0 || offset + length > a.Length) return null;
            var r = new byte[length];
            Array.Copy(a, offset, r, 0, length);
            return r;
        }
    }

    /// <summary>Écriture brute (client S7). Retourne false si hors zone.</summary>
    public bool WriteFromClient(byte area, int db, int offset, byte[] data)
    {
        lock (_lock)
        {
            var a = Area(area, db);
            if (a == null || offset < 0 || offset + data.Length > a.Length) return false;
            Array.Copy(data, 0, a, offset, data.Length);
        }
        ClientWrite?.Invoke(AreaName(area, db), offset, data.Length, data);
        return true;
    }

    public bool WriteBitFromClient(byte area, int db, int offset, int bit, bool value)
    {
        lock (_lock)
        {
            var a = Area(area, db);
            if (a == null || offset < 0 || offset >= a.Length) return false;
            a[offset] = value ? (byte)(a[offset] | (1 << bit)) : (byte)(a[offset] & ~(1 << bit));
        }
        ClientWrite?.Invoke(AreaName(area, db), offset, 1, new[] { (byte)(value ? 1 : 0), (byte)bit });
        return true;
    }

    // ───────── Accès « moteur / interface » (ne déclenchent pas ClientWrite) ─────────
    public byte[] Db(int n) { lock (_lock) return _db[n]; }

    public bool GetBit(int db, int octet, int bit)
    {
        lock (_lock) { var a = _db[db]; return octet < a.Length && (a[octet] & (1 << bit)) != 0; }
    }
    public void SetBit(int db, int octet, int bit, bool v)
    {
        lock (_lock)
        {
            var a = _db[db];
            if (octet >= a.Length) return;
            a[octet] = v ? (byte)(a[octet] | (1 << bit)) : (byte)(a[octet] & ~(1 << bit));
        }
    }
    public float GetReal(int db, int off)
    {
        lock (_lock) return BinaryPrimitives.ReadSingleBigEndian(_db[db].AsSpan(off, 4));
    }
    public void SetReal(int db, int off, float v)
    {
        lock (_lock) BinaryPrimitives.WriteSingleBigEndian(_db[db].AsSpan(off, 4), v);
    }
    public short GetInt(int db, int off)
    {
        lock (_lock) return BinaryPrimitives.ReadInt16BigEndian(_db[db].AsSpan(off, 2));
    }
    public void SetInt(int db, int off, short v)
    {
        lock (_lock) BinaryPrimitives.WriteInt16BigEndian(_db[db].AsSpan(off, 2), v);
    }
    public int GetDInt(int db, int off)
    {
        lock (_lock) return BinaryPrimitives.ReadInt32BigEndian(_db[db].AsSpan(off, 4));
    }
    public void SetDInt(int db, int off, int v)
    {
        lock (_lock) BinaryPrimitives.WriteInt32BigEndian(_db[db].AsSpan(off, 4), v);
    }
    public byte[] Snapshot(int db, int off, int len)
    {
        lock (_lock) { var r = new byte[len]; Array.Copy(_db[db], off, r, 0, len); return r; }
    }
    public void Clear(int db) { lock (_lock) Array.Clear(_db[db]); }
}
