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
                // contrôle de mise en page : contrôles frères qui se chevauchent
                var chevauchements = new List<string>();
                Chercher(f, chevauchements);
                File.WriteAllLines(Path.Combine(sortie, $"{nom}_{mode}.chevauchements.txt"), chevauchements);
                Console.WriteLine($"     chevauchements : {chevauchements.Count}");
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

    private static string Nom(Control c) => $"{c.GetType().Name} '{c.Name}'" + (string.IsNullOrWhiteSpace(c.Text) || c.Text.Length > 30 ? "" : $" \"{c.Text.Replace("\r\n", " ").Replace("\n", " ")}\"");

    private static void Chercher(Control parent, List<string> sortie)
    {
        var enfants = parent.Controls.Cast<Control>().Where(c => c.Visible && c.Dock == DockStyle.None && c.Width > 3 && c.Height > 3).ToList();
        for (int i = 0; i < enfants.Count; i++)
            for (int j = i + 1; j < enfants.Count; j++)
            {
                var a = enfants[i]; var b = enfants[j];
                // un fond (panneau, image) sous un contrôle n'est pas un défaut
                if (a is PictureBox || b is PictureBox) continue;
                var r = Rectangle.Intersect(a.Bounds, b.Bounds);
                if (r.IsEmpty || r.Width * r.Height < 40) continue;
                // une étiquette dont le cadre déborde de quelques pixels n'est pas gênante : on ignore moins de 6 px de recouvrement
                if (r.Width < 6 || r.Height < 6) continue;
                sortie.Add($"{Nom(a)} {a.Bounds}  ×  {Nom(b)} {b.Bounds}  => {r.Width}x{r.Height}");
            }
        foreach (var c in parent.Controls.Cast<Control>().Where(c => c.HasChildren)) Chercher(c, sortie);
    }
}
