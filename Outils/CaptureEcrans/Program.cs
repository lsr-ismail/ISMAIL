using System.Drawing.Imaging;
using System.Reflection;

namespace CaptureEcrans;

internal static class Program
{
    [STAThread]
    private static int Main(string[] args)
    {
        if (args.Length < 4) { Console.WriteLine("Usage: CaptureEcrans <central.dll> <sortie> <Clair|Sombre|Aucun> <Type...>"); return 1; }
        string dll = Path.GetFullPath(args[0]);
        string sortie = Path.GetFullPath(args[1]);
        string mode = args[2];
        Directory.CreateDirectory(sortie);
        ApplicationConfiguration.Initialize();

        // l'ERP et ses dépendances (S7.Net, SqlClient...) sont dans le dossier de central.dll
        string dossier = Path.GetDirectoryName(dll)!;
        AppDomain.CurrentDomain.AssemblyResolve += (s, e) =>
        {
            string p = Path.Combine(dossier, new AssemblyName(e.Name).Name + ".dll");
            return File.Exists(p) ? Assembly.LoadFrom(p) : null;
        };
        var asm = Assembly.LoadFrom(dll);
        var tm = asm.GetType("central.ThemeManager");
        var mAppliquer = tm.GetMethod("AppliquerFenetre");
        var mDefinir = tm.GetMethod("DefinirMode");
        var enumMode = asm.GetType("central.ModeTheme");
        if (mode != "Aucun")
            mDefinir.Invoke(null, new[] { Enum.Parse(enumMode, mode) });

        foreach (string nom in args.Skip(3))
        {
            try
            {
                var t = asm.GetType("central." + nom) ?? asm.GetType(nom);
                if (t == null) { Console.WriteLine($"[?] type introuvable : {nom}"); continue; }
                var f = (Form)Activator.CreateInstance(t)!;
                f.StartPosition = FormStartPosition.Manual;
                f.Location = new Point(10, 10);
                f.ShowInTaskbar = false;
                f.Show();
                Application.DoEvents();
                if (mode != "Aucun") mAppliquer.Invoke(null, new object[] { f });
                for (int i = 0; i < 6; i++) { Application.DoEvents(); Thread.Sleep(60); }
                using var bmp = new Bitmap(f.Width, f.Height);
                f.DrawToBitmap(bmp, new Rectangle(0, 0, f.Width, f.Height));
                string fichier = Path.Combine(sortie, $"{nom}_{mode}.png");
                bmp.Save(fichier, ImageFormat.Png);
                Console.WriteLine($"[OK] {fichier} ({f.Width}x{f.Height})");
                f.Close(); f.Dispose();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[KO] {nom} : {(ex.InnerException ?? ex).Message}");
            }
        }
        return 0;
    }
}
