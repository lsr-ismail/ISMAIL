using System.Globalization;

namespace SimulateurAutomate;

public sealed class MainForm : Form
{
    private readonly PlcMemory _mem = new();
    private readonly S7Server _srv;
    private readonly CpuEngine _cpu;

    // Tableau de bord
    private NumericUpDown _numPort;
    private Button _btnStart, _btnLiaison, _btnFiger, _btnDeco, _btnReset;
    private NumericUpDown _numLatence;
    private Label _lblSrv, _lblClients, _lblReq, _lblPhase, _lblGachee, _lblMat;
    private Label _pAgr, _pCim, _pEau, _pAdj;
    private readonly Dictionary<string, Label> _led = new();
    private CheckBox _chkAU;
    // Variables
    private DataGridView _grid;
    private readonly List<Tag> _rows = new();
    // Défauts
    private CheckedListBox _lstDef;
    private ComboBox _cmbDefFiltre;
    private readonly List<(int o, int b, string cls)> _defItems = new();
    private bool _majDef;
    // Journal
    private RichTextBox _log;
    private System.Windows.Forms.Timer _timer;

    public MainForm(int port, bool autoStart)
    {
        Text = "Simulateur d'automate S7 — ERP Centrale à béton";
        Width = 1100; Height = 760; StartPosition = FormStartPosition.CenterScreen;
        Font = new Font("Segoe UI", 9f);

        _srv = new S7Server(_mem);
        _cpu = new CpuEngine(_mem);
        _srv.Log += s => Journaliser(s);
        _cpu.Log += s => Journaliser(s);
        _mem.ClientWrite += OnClientWrite;

        var tabs = new TabControl { Dock = DockStyle.Fill };
        tabs.TabPages.Add(OngletTableauDeBord(port));
        tabs.TabPages.Add(OngletVariables());
        tabs.TabPages.Add(OngletDefauts());
        tabs.TabPages.Add(OngletReglages());
        tabs.TabPages.Add(OngletJournal());
        Controls.Add(tabs);

        _timer = new System.Windows.Forms.Timer { Interval = 250 };
        _timer.Tick += (s, e) => Rafraichir();
        _timer.Start();

        FormClosing += (s, e) => { _cpu.Stop(); _srv.Stop(); };
        Shown += (s, e) => { if (autoStart) DemarrerArreter(); };
    }

    // ───────────────────────── Onglets ─────────────────────────
    private TabPage OngletTableauDeBord(int port)
    {
        var tp = new TabPage("Tableau de bord");
        var top = new FlowLayoutPanel { Dock = DockStyle.Top, Height = 78, Padding = new Padding(8), WrapContents = true };
        top.Controls.Add(new Label { Text = "Port TCP :", AutoSize = true, Margin = new Padding(3, 8, 3, 3) });
        _numPort = new NumericUpDown { Minimum = 1, Maximum = 65535, Value = port, Width = 70 };
        top.Controls.Add(_numPort);
        _btnStart = Btn("▶ Démarrer le simulateur", (s, e) => DemarrerArreter(), 190);
        top.Controls.Add(_btnStart);
        _btnLiaison = Btn("✂ Couper la liaison", (s, e) => ToggleLiaison(), 160);
        top.Controls.Add(_btnLiaison);
        _btnFiger = Btn("❄ Figer le CPU (pas de réponse)", (s, e) => ToggleFiger(), 215);
        top.Controls.Add(_btnFiger);
        _btnDeco = Btn("⏏ Déconnecter les clients", (s, e) => _srv.DropAll(), 185);
        top.Controls.Add(_btnDeco);
        top.Controls.Add(new Label { Text = "Latence (ms) :", AutoSize = true, Margin = new Padding(12, 8, 3, 3) });
        _numLatence = new NumericUpDown { Minimum = 0, Maximum = 5000, Width = 60 };
        _numLatence.ValueChanged += (s, e) => _srv.LatenceMs = (int)_numLatence.Value;
        top.Controls.Add(_numLatence);
        _btnReset = Btn("⟲ Réinitialiser la mémoire", (s, e) => { if (MessageBox.Show("Remettre DB1 et DB71 à zéro ?", "Confirmation", MessageBoxButtons.YesNo) == DialogResult.Yes) _cpu.Reinitialiser(); }, 185);
        top.Controls.Add(_btnReset);

        var info = new TableLayoutPanel { Dock = DockStyle.Top, Height = 70, ColumnCount = 6, Padding = new Padding(8) };
        for (int i = 0; i < 6; i++) info.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 16.6f));
        _lblSrv = Info(info, 0, "Serveur : arrêté");
        _lblClients = Info(info, 1, "Clients : 0");
        _lblReq = Info(info, 2, "Requêtes : 0");
        _lblPhase = Info(info, 3, "Phase : repos");
        _lblGachee = Info(info, 4, "Gâchée : -");
        _lblMat = Info(info, 5, "Matricule CPU : 0");

        var poids = new GroupBox { Text = "Pesées (live)", Dock = DockStyle.Top, Height = 110 };
        var pl = new TableLayoutPanel { Dock = DockStyle.Fill, ColumnCount = 4 };
        for (int i = 0; i < 4; i++) pl.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 25));
        _pAgr = Gros(pl, 0, "Agrégats"); _pCim = Gros(pl, 1, "Ciment"); _pEau = Gros(pl, 2, "Eau"); _pAdj = Gros(pl, 3, "Adjuvants");
        poids.Controls.Add(pl);

        var leds = new GroupBox { Text = "États (mis à jour en direct)", Dock = DockStyle.Top, Height = 170 };
        var fl = new FlowLayoutPanel { Dock = DockStyle.Fill, Padding = new Padding(6) };
        foreach (var t in Tags.All.Where(t => t.Groupe == "États" || t.Groupe == "Commandes" && t.Type == TagType.Bit))
        {
            var lb = new Label { Text = t.Nom, AutoSize = false, Width = 175, Height = 22, TextAlign = ContentAlignment.MiddleCenter, BackColor = Color.LightGray, Margin = new Padding(3) };
            _led[t.Adresse] = lb; fl.Controls.Add(lb);
        }
        leds.Controls.Add(fl);

        var cmd = new GroupBox { Text = "Simuler l'opérateur sur le terrain", Dock = DockStyle.Top, Height = 56 };
        var cf = new FlowLayoutPanel { Dock = DockStyle.Fill, Padding = new Padding(6) };
        _chkAU = new CheckBox { Text = "Arrêt d'urgence enfoncé", AutoSize = true };
        _chkAU.CheckedChanged += (s, e) => _cpu.ArretUrgence(_chkAU.Checked);
        cf.Controls.Add(_chkAU);
        cf.Controls.Add(Btn("Effacer tous les défauts", (s, e) => { Array.Clear(_mem.Db(1), 32, 20); SynchroDefauts(); Journaliser("Défauts effacés (manuel)"); }, 175));
        cmd.Controls.Add(cf);

        tp.Controls.Add(cmd); tp.Controls.Add(leds); tp.Controls.Add(poids); tp.Controls.Add(info); tp.Controls.Add(top);
        return tp;
    }

    private TabPage OngletVariables()
    {
        var tp = new TabPage("Variables (lecture / écriture)");
        _grid = new DataGridView
        {
            Dock = DockStyle.Fill, AllowUserToAddRows = false, AllowUserToDeleteRows = false, RowHeadersVisible = false,
            AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill, SelectionMode = DataGridViewSelectionMode.FullRowSelect,
        };
        _grid.Columns.Add("g", "Groupe"); _grid.Columns.Add("n", "Nom"); _grid.Columns.Add("a", "Adresse"); _grid.Columns.Add("t", "Type"); _grid.Columns.Add("v", "Valeur (modifiable)");
        foreach (DataGridViewColumn c in _grid.Columns) c.ReadOnly = c.Name != "v";
        foreach (var t in Tags.All) { _rows.Add(t); _grid.Rows.Add(t.Groupe, t.Nom, t.Adresse, t.Type.ToString(), ""); }
        _grid.CellEndEdit += (s, e) => EcrireCellule(e.RowIndex);
        var help = new Label { Dock = DockStyle.Top, Height = 38, Text = "Bits : tapez 0/1. Réels : 123.4. Les modifications sont écrites dans la mémoire simulée ; l'ERP les verra à sa prochaine lecture.", Padding = new Padding(6) };
        tp.Controls.Add(_grid); tp.Controls.Add(help);
        return tp;
    }

    private TabPage OngletDefauts()
    {
        var tp = new TabPage("Défauts / détecteurs");
        var top = new FlowLayoutPanel { Dock = DockStyle.Top, Height = 40, Padding = new Padding(6) };
        top.Controls.Add(new Label { Text = "Groupe :", AutoSize = true, Margin = new Padding(3, 7, 3, 3) });
        _cmbDefFiltre = new ComboBox { DropDownStyle = ComboBoxStyle.DropDownList, Width = 260 };
        _cmbDefFiltre.Items.AddRange(new object[] { "TIMEOUT / tolérances (DBX32.0 – 42.3)", "DISJ - disjoncteurs (DBX44.0 – 51.5)", "DET - détecteurs (DBX24.0 – 30.2)" });
        _cmbDefFiltre.SelectedIndex = 0;
        _cmbDefFiltre.SelectedIndexChanged += (s, e) => RemplirDefauts();
        top.Controls.Add(_cmbDefFiltre);
        top.Controls.Add(new Label { Text = "(Libellés exacts : voir ModuleDefautsCPU.vb — ici seules les adresses sont données.)", AutoSize = true, Margin = new Padding(12, 7, 3, 3) });
        _lstDef = new CheckedListBox { Dock = DockStyle.Fill, CheckOnClick = true };
        _lstDef.ItemCheck += (s, e) =>
        {
            if (_majDef) return;
            var it = _defItems[e.Index];
            _cpu.SetDefaut(it.o, it.b, e.NewValue == CheckState.Checked);
        };
        tp.Controls.Add(_lstDef); tp.Controls.Add(top);
        RemplirDefauts();
        return tp;
    }

    private void RemplirDefauts()
    {
        _majDef = true;
        _lstDef.Items.Clear(); _defItems.Clear();
        int g = _cmbDefFiltre.SelectedIndex;
        (int, int, int, int, string) rg = g switch { 0 => (32, 0, 42, 3, "TIMEOUT"), 1 => (44, 0, 51, 5, "DISJ"), _ => (24, 0, 30, 2, "DET") };
        for (int o = rg.Item1; o <= rg.Item3; o++)
            for (int b = 0; b < 8; b++)
            {
                if (o == rg.Item3 && b > rg.Item4) break;
                _defItems.Add((o, b, rg.Item5));
                _lstDef.Items.Add($"DB1.DBX{o}.{b}   [{rg.Item5}]", _mem.GetBit(1, o, b));
            }
        _majDef = false;
    }

    private void SynchroDefauts()
    {
        _majDef = true;
        for (int i = 0; i < _defItems.Count; i++) _lstDef.SetItemChecked(i, _mem.GetBit(1, _defItems[i].o, _defItems[i].b));
        _majDef = false;
    }

    private TabPage OngletReglages()
    {
        var tp = new TabPage("Réglages du process");
        var fl = new FlowLayoutPanel { Dock = DockStyle.Fill, FlowDirection = FlowDirection.TopDown, Padding = new Padding(12), WrapContents = false };
        fl.Controls.Add(new Label { AutoSize = true, Text = "Ces valeurs sont des HYPOTHÈSES de simulation (le programme automate réel n'est pas disponible).\r\nElles ne modifient jamais le code de l'ERP.", ForeColor = Color.DarkRed });
        void Num(string nom, double v, double min, double max, int dec, Action<double> set)
        {
            var p = new FlowLayoutPanel { AutoSize = true };
            p.Controls.Add(new Label { Text = nom, Width = 300, Margin = new Padding(3, 7, 3, 3) });
            var n = new NumericUpDown { Minimum = (decimal)min, Maximum = (decimal)max, DecimalPlaces = dec, Value = (decimal)v, Width = 90, Increment = dec > 0 ? 0.5m : 1 };
            n.ValueChanged += (s, e) => set((double)n.Value);
            p.Controls.Add(n); fl.Controls.Add(p);
        }
        Num("Vitesse du temps (×)  — 10 = 10 fois plus vite", _cpu.Vitesse, 0.1, 100, 1, v => _cpu.Vitesse = v);
        Num("Débit agrégats (kg/s)", _cpu.DebitAgrKgS, 1, 1000, 1, v => _cpu.DebitAgrKgS = v);
        Num("Débit ciment (kg/s)", _cpu.DebitCimKgS, 1, 1000, 1, v => _cpu.DebitCimKgS = v);
        Num("Débit eau (L/s)", _cpu.DebitEauLS, 1, 1000, 1, v => _cpu.DebitEauLS = v);
        Num("Débit adjuvant (kg/s)", _cpu.DebitAdjKgS, 0.01, 100, 2, v => _cpu.DebitAdjKgS = v);
        Num("Durée vidange vers malaxeur (s)", _cpu.TempsVidangeS, 0.5, 120, 1, v => _cpu.TempsVidangeS = v);
        Num("Durée de malaxage (s)", _cpu.TempsMalaxageS, 0.5, 600, 1, v => _cpu.TempsMalaxageS = v);
        Num("Durée d'ouverture trappe malaxeur (s)", _cpu.TempsOuvertureS, 0.5, 120, 1, v => _cpu.TempsOuvertureS = v);
        Num("Dispersion de pose (± %)", _cpu.ErreurPct, 0, 50, 1, v => _cpu.ErreurPct = v);
        var c1 = new CheckBox { Text = "Exiger le mode AUTO pour accepter un départ cycle (hypothèse)", AutoSize = true, Checked = _cpu.ExigerModeAuto };
        c1.CheckedChanged += (s, e) => _cpu.ExigerModeAuto = c1.Checked;
        var c2 = new CheckBox { Text = "Suspendre le cycle en cas de défaut / arrêt d'urgence", AutoSize = true, Checked = _cpu.PauseSurDefaut };
        c2.CheckedChanged += (s, e) => _cpu.PauseSurDefaut = c2.Checked;
        fl.Controls.Add(c1); fl.Controls.Add(c2);
        tp.Controls.Add(fl);
        return tp;
    }

    private TabPage OngletJournal()
    {
        var tp = new TabPage("Journal (échanges ERP ↔ simulateur)");
        _log = new RichTextBox { Dock = DockStyle.Fill, ReadOnly = true, Font = new Font("Consolas", 9f), BackColor = Color.White };
        var b = Btn("Effacer le journal", (s, e) => _log.Clear(), 140);
        var p = new Panel { Dock = DockStyle.Top, Height = 36 }; p.Controls.Add(b);
        tp.Controls.Add(_log); tp.Controls.Add(p);
        return tp;
    }

    // ───────────────────────── Actions ─────────────────────────
    private void DemarrerArreter()
    {
        if (_srv.Running)
        {
            _cpu.Stop(); _srv.Stop();
            _btnStart.Text = "▶ Démarrer le simulateur"; _numPort.Enabled = true;
            return;
        }
        try
        {
            _srv.Start((int)_numPort.Value);
            _cpu.Start();
            _btnStart.Text = "■ Arrêter le simulateur"; _numPort.Enabled = false;
        }
        catch (Exception ex)
        {
            MessageBox.Show("Impossible d'ouvrir le port " + _numPort.Value + " :\r\n" + ex.Message +
                "\r\n\r\nLe port 102 peut être occupé par un service Siemens (S7DOS Help Service). Essayez un autre port, ou arrêtez ce service.",
                "Port indisponible", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            Journaliser("Échec ouverture port : " + ex.Message);
            try { _srv.Stop(); } catch { }
        }
    }

    private void ToggleLiaison()
    {
        _srv.LiaisonCoupee = !_srv.LiaisonCoupee;
        if (_srv.LiaisonCoupee) _srv.DropAll();
        _btnLiaison.Text = _srv.LiaisonCoupee ? "🔌 Rétablir la liaison" : "✂ Couper la liaison";
        _btnLiaison.BackColor = _srv.LiaisonCoupee ? Color.Salmon : SystemColors.Control;
        Journaliser(_srv.LiaisonCoupee ? "LIAISON COUPÉE (simulation câble débranché)" : "Liaison rétablie");
    }

    private void ToggleFiger()
    {
        _srv.NePasRepondre = !_srv.NePasRepondre;
        _btnFiger.Text = _srv.NePasRepondre ? "▶ Dégeler le CPU" : "❄ Figer le CPU (pas de réponse)";
        _btnFiger.BackColor = _srv.NePasRepondre ? Color.LightSkyBlue : SystemColors.Control;
        Journaliser(_srv.NePasRepondre ? "CPU FIGÉ : les requêtes ne reçoivent plus de réponse" : "CPU dégelé");
    }

    private void EcrireCellule(int row)
    {
        var t = _rows[row];
        string s = Convert.ToString(_grid.Rows[row].Cells["v"].Value) ?? "";
        s = s.Trim().Replace(',', '.');
        try
        {
            switch (t.Type)
            {
                case TagType.Bit: _mem.SetBit(t.Db, t.Offset, t.Bit, s == "1" || s.Equals("true", StringComparison.OrdinalIgnoreCase)); break;
                case TagType.Real: _mem.SetReal(t.Db, t.Offset, float.Parse(s, CultureInfo.InvariantCulture)); break;
                case TagType.Int: _mem.SetInt(t.Db, t.Offset, short.Parse(s, CultureInfo.InvariantCulture)); break;
                case TagType.DInt: _mem.SetDInt(t.Db, t.Offset, int.Parse(s, CultureInfo.InvariantCulture)); break;
            }
            Journaliser($"[Simulateur] {t.Adresse} ({t.Nom}) = {s}");
        }
        catch (Exception ex) { MessageBox.Show("Valeur invalide : " + ex.Message); }
    }

    private string Lire(Tag t) => t.Type switch
    {
        TagType.Bit => _mem.GetBit(t.Db, t.Offset, t.Bit) ? "1" : "0",
        TagType.Real => _mem.GetReal(t.Db, t.Offset).ToString("0.###", CultureInfo.InvariantCulture),
        TagType.Int => _mem.GetInt(t.Db, t.Offset).ToString(),
        _ => _mem.GetDInt(t.Db, t.Offset).ToString(),
    };

    private void Rafraichir()
    {
        _lblSrv.Text = "Serveur : " + (_srv.Running ? $"actif (port {_srv.Port})" : "arrêté");
        _lblSrv.ForeColor = _srv.Running ? Color.DarkGreen : Color.DarkRed;
        _lblClients.Text = "Clients : " + _srv.NbClients;
        _lblReq.Text = "Requêtes : " + _srv.NbRequetes;
        _lblPhase.Text = "Phase : " + _cpu.Etat;
        _lblGachee.Text = _cpu.NbGachees > 0 ? $"Gâchée : {_cpu.GacheeCourante}/{_cpu.NbGachees}" : "Gâchée : -";
        _lblMat.Text = "Matricule CPU : " + _cpu.Matricule;
        _pAgr.Text = _mem.GetReal(1, 86).ToString("0.0") + " kg";
        _pCim.Text = _mem.GetReal(1, 78).ToString("0.0") + " kg";
        _pEau.Text = _mem.GetReal(1, 82).ToString("0.0") + " L";
        _pAdj.Text = _mem.GetReal(1, 90).ToString("0.000") + " kg";

        foreach (var t in Tags.All)
            if (t.Type == TagType.Bit && _led.TryGetValue(t.Adresse, out var lb))
            {
                bool on = _mem.GetBit(t.Db, t.Offset, t.Bit);
                lb.BackColor = on ? (t.Nom.Contains("Défaut") || t.Nom.Contains("urgence") ? Color.Tomato : Color.LightGreen) : Color.LightGray;
            }

        if (_grid.CurrentCell == null || !_grid.IsCurrentCellInEditMode)
            for (int i = 0; i < _rows.Count; i++)
            {
                var c = _grid.Rows[i].Cells["v"];
                if (_grid.IsCurrentCellInEditMode && _grid.CurrentCell == c) continue;
                string v = Lire(_rows[i]);
                if (!Equals(c.Value, v)) c.Value = v;
            }
    }

    // ───────────────────────── Journal ─────────────────────────
    private void OnClientWrite(string zone, int offset, int len, byte[] data)
    {
        string txt;
        if (zone == "DB1")
        {
            var names = Tags.All.Where(t => t.Db == 1 && t.Offset >= offset && t.Offset < offset + Math.Max(len, 1) && (t.Type == TagType.Bit || t.Offset + 4 <= offset + len + 3))
                               .Select(t => t.Nom).Take(3);
            string n = string.Join(", ", names);
            txt = len == 4 ? $"ERP → {zone}.DBD{offset} = {BitConverter.ToSingle(new[] { data[3], data[2], data[1], data[0] }, 0):0.###}"
                : len == 1 ? $"ERP → {zone}.DBB{offset} = 0x{data[0]:X2}"
                : $"ERP → {zone} [{offset}..{offset + len - 1}] ({len} octets)";
            if (n != "") txt += "   (" + n + ")";
        }
        else txt = $"ERP → {zone} offset {offset} ({len} octets)";
        Journaliser(txt);
    }

    private void Journaliser(string s)
    {
        if (_log == null || _log.IsDisposed) return;
        void Add()
        {
            if (_log.TextLength > 200000) _log.Clear();
            _log.AppendText(DateTime.Now.ToString("HH:mm:ss.fff") + "  " + s + "\r\n");
            _log.ScrollToCaret();
        }
        try { if (_log.InvokeRequired) _log.BeginInvoke((Action)Add); else Add(); } catch { }
    }

    // ───────────────────────── Aides UI ─────────────────────────
    private static Button Btn(string t, EventHandler h, int w)
    {
        var b = new Button { Text = t, Width = w, Height = 30 };
        b.Click += h; return b;
    }
    private static Label Info(TableLayoutPanel p, int col, string t)
    {
        var l = new Label { Text = t, Dock = DockStyle.Fill, Font = new Font("Segoe UI", 10f, FontStyle.Bold), TextAlign = ContentAlignment.MiddleLeft };
        p.Controls.Add(l, col, 0); return l;
    }
    private static Label Gros(TableLayoutPanel p, int col, string titre)
    {
        var panel = new Panel { Dock = DockStyle.Fill };
        panel.Controls.Add(new Label { Text = titre, Dock = DockStyle.Top, Height = 20, TextAlign = ContentAlignment.MiddleCenter });
        var v = new Label { Text = "0", Dock = DockStyle.Fill, Font = new Font("Segoe UI", 18f, FontStyle.Bold), TextAlign = ContentAlignment.MiddleCenter };
        panel.Controls.Add(v); v.BringToFront();
        p.Controls.Add(panel, col, 0); return v;
    }
}
