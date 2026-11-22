namespace SimulateurAutomate;

internal static class Program
{
    /// <summary>
    /// Arguments : --port N (défaut 102) · --autostart (démarre le serveur directement)
    /// </summary>
    [STAThread]
    private static void Main(string[] args)
    {
        int port = 102; bool auto = false;
        for (int i = 0; i < args.Length; i++)
        {
            if (args[i] == "--port" && i + 1 < args.Length) port = int.Parse(args[++i]);
            else if (args[i] == "--autostart") auto = true;
        }
        ApplicationConfiguration.Initialize();
        Application.Run(new MainForm(port, auto));
    }
}
