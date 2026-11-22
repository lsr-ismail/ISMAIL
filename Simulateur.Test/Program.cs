using System.Buffers.Binary;
using S7.Net;
using SimulateurAutomate;

// Usage : dotnet run --project Simulateur.Test [port]
int port = args.Length > 0 ? int.Parse(args[0]) : 1102;
int echecs = 0;
void Check(string nom, bool ok, string detail = "")
{
    Console.WriteLine($"[{(ok ? "OK " : "KO ")}] {nom} {detail}");
    if (!ok) echecs++;
}

var mem = new PlcMemory();
var srv = new S7Server(mem);
var cpu = new CpuEngine(mem) { Vitesse = 50 };
cpu.Log += s => Console.WriteLine("      cpu> " + s);
srv.Start(port);
cpu.Start();

using var plc = new Plc(CpuType.S71200, "127.0.0.1", port, 0, 1);
plc.Open();
Check("Connexion S7", plc.IsConnected);

static float Real(byte[] b, int o) => BinaryPrimitives.ReadSingleBigEndian(b.AsSpan(o, 4));
void WReal(int db, int off, float v) { var b = new byte[4]; BinaryPrimitives.WriteSingleBigEndian(b, v); plc.WriteBytes(DataType.DataBlock, db, off, b); }
void SetBit(int off, int bit, bool v)   // même méthode que l'ERP : lecture-modification-écriture de l'octet
{
    var b = plc.ReadBytes(DataType.DataBlock, 1, off, 1);
    b[0] = v ? (byte)(b[0] | (1 << bit)) : (byte)(b[0] & ~(1 << bit));
    plc.WriteBytes(DataType.DataBlock, 1, off, b);
}
bool GetBit(int off, int bit) => (plc.ReadBytes(DataType.DataBlock, 1, off, 1)[0] & (1 << bit)) != 0;

var db1 = plc.ReadBytes(DataType.DataBlock, 1, 0, 1236);
Check("Lecture DB1 (1236 octets)", db1.Length == 1236, $"({db1.Length})");
var db71 = plc.ReadBytes(DataType.DataBlock, 71, 0, 3440);
Check("Lecture DB71 (3440 octets)", db71.Length == 3440, $"({db71.Length})");

plc.Write("DB1.DBW52", (short)7);
Check("Écriture / relecture DBW52", Convert.ToInt32(plc.Read("DB1.DBW52")) == 7);
plc.Write("DB1.DBX0.5", true);
Check("Écriture bit DBX0.5 (Write bool)", GetBit(0, 5));
plc.Write("DB1.DBX0.5", false);
Check("Remise à 0 du bit DBX0.5", !GetBit(0, 5));

// ── Scénario ERP : envoi formule + départ cycle ──
WReal(1, 538, 800); WReal(1, 542, 400);   // TR1, TR2 (kg/m³)
WReal(1, 530, 350);                      // ciment
WReal(1, 586, 180);                      // eau
WReal(1, 594, 3);                        // adjuvant 1
WReal(1, 916, 1.0f); WReal(1, 920, 2.0f); // malaxeur 1 m³, camion 2 m³ → 2 gâchées
Check("Relecture consigne ciment", Math.Abs(Real(plc.ReadBytes(DataType.DataBlock, 1, 530, 4), 0) - 350) < 0.01);

SetBit(0, 0, true);   // mode auto
SetBit(0, 4, true);   // DYC
var t0 = DateTime.Now;
bool fin = false;
while ((DateTime.Now - t0).TotalSeconds < 90)
{
    if (GetBit(914, 1)) { fin = true; break; }
    Thread.Sleep(200);
}
Check("Fin de commande (914.1) atteinte", fin, $"en {(DateTime.Now - t0).TotalSeconds:0.0}s");
Check("DYC remis à 0 par le CPU", !GetBit(0, 4));
db71 = plc.ReadBytes(DataType.DataBlock, 71, 0, 3440);
int m1 = BinaryPrimitives.ReadInt32BigEndian(db71.AsSpan(3200, 4));
int m2 = BinaryPrimitives.ReadInt32BigEndian(db71.AsSpan(3204, 4));
Check("Matricules DB71", m1 == 1 && m2 == 2, $"({m1},{m2})");
float poseCim = Real(db71, 640), poseTr1 = Real(db71, 0);
Check("Pose ciment gâchée 1 ≈ 350", Math.Abs(poseCim - 350) < 350 * 0.02, $"({poseCim:0.0})");
Check("Pose TR1 gâchée 1 ≈ 800", Math.Abs(poseTr1 - 800) < 800 * 0.02, $"({poseTr1:0.0})");

// ── Panne de liaison ──
srv.LiaisonCoupee = true; srv.DropAll();
bool erreur = false;
try { plc.ReadBytes(DataType.DataBlock, 1, 0, 10); } catch { erreur = true; }
Check("Coupure de liaison détectée côté client", erreur);

srv.Stop(); cpu.Stop();
Console.WriteLine(echecs == 0 ? "\nTOUS LES TESTS PASSENT" : $"\n{echecs} TEST(S) EN ÉCHEC");
return echecs == 0 ? 0 : 1;
