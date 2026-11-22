using System.Diagnostics;

namespace SimulateurAutomate;

/// <summary>
/// Reproduit (de façon SIMPLIFIÉE) le comportement du programme automate tel que l'ERP l'attend.
/// Toutes les adresses viennent de l'ERP (ModulePLCDuplex.vb, ModuleArchivageGachees.vb, Synoptique.vb,
/// ModuleGacheesDB71.vb). Le programme automate réel n'a PAS été fourni : les durées, débits et la
/// logique d'enchaînement sont des HYPOTHÈSES réglables dans l'interface, pas la vérité terrain.
/// </summary>
public sealed class CpuEngine
{
    private readonly PlcMemory _m;
    private Thread _th;
    private volatile bool _run;

    public event Action<string> Log;

    // ─── Réglages (modifiables depuis l'interface) ───
    public double Vitesse = 1.0;             // multiplicateur de temps
    public double DebitAgrKgS = 80;
    public double DebitCimKgS = 30;
    public double DebitEauLS = 15;
    public double DebitAdjKgS = 0.5;
    public double TempsVidangeS = 2;
    public double TempsMalaxageS = 5;
    public double TempsOuvertureS = 3;
    public double ErreurPct = 1.0;           // dispersion aléatoire de la pose (± %)
    public bool ExigerModeAuto = false;      // hypothèse : sans preuve que le CPU l'exige
    public bool PauseSurDefaut = true;

    public enum Phase { Repos, Dosage, Vidange, Malaxage, Ouverture, FinGachee, AttenteConfirmation, FinCommande }
    public Phase Etat { get; private set; } = Phase.Repos;
    public int GacheeCourante { get; private set; }
    public int NbGachees { get; private set; }
    public int Matricule { get; private set; }

    // Adresses DB1 (source : ERP)
    private static class A
    {
        public const int ModeAuto_o = 0, ModeAuto_b = 0, Dyc_o = 0, Dyc_b = 4;
        public static readonly (int o, int b)[] ForceTr = { (0, 5), (0, 6), (0, 7), (1, 0) };
        public static readonly (int o, int b)[] StTr = { (12, 0), (12, 2), (12, 4), (12, 6) };
        public static readonly (int o, int b)[] ForceVis = { (5, 1), (5, 2) };
        public static readonly (int o, int b)[] ForceAdj = { (8, 1), (8, 2), (8, 3), (8, 4) };
        public static readonly (int o, int b)[] StAdj = { (21, 0), (21, 1), (21, 2), (21, 3) };
        public const int FinCmd_o = 914, FinCmd_b = 1, DemArret_b = 2, ConfArret_b = 3, Raz_b = 4, Vidange_b = 5, FlecheRouge_b = 6, RepriseDef_b = 0;
        public const int Etat_o = 1082;   // .0 défaut .1 AU .2 mlx plein .3 skip plein .4 fin gâchée
        public const int Cyc_o = 1088;    // .0 gâchée finie .1 cycle en cours .2 forçage malaxeur
        public const int PoidsCim = 78, PoidsEau = 82, PoidsAgr = 86, PoidsAdj = 90;
        public const int CapMlx = 916, CapCam = 920;
        public static readonly int[] FormTr = { 538, 542, 546, 550 };
        public const int FormCim = 530, FormEau = 586;
        public static readonly int[] FormAdj = { 594, 598, 602, 606 };
        public static readonly int[] ConsTr = { 118, 122, 126, 130 };
        public static readonly int[] PoseTr = { 230, 234, 238, 242 };
        public static readonly int[] EcarTr = { 346, 350, 354, 358 };
        public static readonly int[] ReelTr = { 924, 928, 932, 936 };
        public static readonly int[] ThTr = { 1008, 1012, 1016, 1020 };
        public const int ConsCim = 94, PoseCimS1 = 206, PoseCimS2 = 210, EcarCim = 318, ReelCim = 940, ThCim = 1024;
        public const int ConsEau = 166, PoseEau = 278, EcarEau = 394, ReelEau = 948, ThEau = 1028;
        public static readonly int[] ConsAdj = { 174, 178, 182, 186 };
        public static readonly int[] PoseAdj = { 286, 290, 294, 298 };
        public static readonly int[] EcarAdj = { 402, 406, 410, 414 };
        public static readonly int[] ReelAdj = { 952, 956, 960, 964 };
        public static readonly int[] ThAdj = { 1032, 1036, 1040, 1044 };
        public const int NGachee = 1076, NGacheeTotal = 1078, NGacheeReste = 1048, NbGacheeCalc = 1220, ResteGachee = 1232;
        public const int CptGacheeAgr = 1224, CptGacheeCim = 1228;
        // DB71 : blocs de 40 REAL (160 octets) par colonne
        public static readonly int[] P71 = { 0, 160, 320, 480, 640, 800, 960, 1120, 1280, 1440 };     // TR1-4, S1, EAU, ADJ1-4
        public static readonly int[] E71 = { 1600, 1760, 1920, 2080, 3040, 2880, 2240, 2400, 2560, 2720 };
        public const int Compteur71 = 3200, AjoutEau71 = 3360;
    }

    public CpuEngine(PlcMemory m) { _m = m; }

    // ─── Données de la gâchée en cours ───
    private readonly double[] _cons = new double[10];   // TR1-4, CIM, EAU, ADJ1-4
    private readonly double[] _pose = new double[10];
    private readonly double[] _cible = new double[10];
    private double _phaseT;
    private bool _prevDyc, _prevRaz, _prevRepDef;
    private readonly Random _rnd = new();
    private double _tFinGachee;

    private static readonly string[] Noms = { "TR1", "TR2", "TR3", "TR4", "CIM", "EAU", "ADJ1", "ADJ2", "ADJ3", "ADJ4" };

    public void Start()
    {
        if (_run) return;
        _run = true;
        InitialiserEtatRepos();
        _th = new Thread(Boucle) { IsBackground = true, Name = "CpuEngine" };
        _th.Start();
    }
    public void Stop() { _run = false; }

    public void InitialiserEtatRepos()
    {
        // valeurs par défaut pour que l'IHM affiche quelque chose de cohérent
        _m.SetBit(1, 30, 1, true);  // flèche verte = 0 (l'ERP inverse) → 1 = pas de flèche verte
        if (_m.GetReal(1, A.CapMlx) == 0) _m.SetReal(1, A.CapMlx, 1.0f);
        if (_m.GetReal(1, A.CapCam) == 0) _m.SetReal(1, A.CapCam, 8.0f);
    }

    private bool B(int o, int b) => _m.GetBit(1, o, b);
    private void S(int o, int b, bool v) => _m.SetBit(1, o, b, v);

    private void Boucle()
    {
        var sw = Stopwatch.StartNew();
        double last = 0;
        while (_run)
        {
            Thread.Sleep(50);
            double now = sw.Elapsed.TotalSeconds;
            double dt = (now - last) * Vitesse;
            last = now;
            try { Tick(dt); } catch (Exception ex) { Log?.Invoke("Moteur CPU : " + ex.Message); }
        }
    }

    private bool Defaut => B(A.Etat_o, 0) || B(A.Etat_o, 1);

    private void Tick(double dt)
    {
        // Fronts montants
        bool dyc = B(A.Dyc_o, A.Dyc_b);
        bool raz = B(A.FinCmd_o, A.Raz_b);
        bool repDef = B(A.FinCmd_o, A.RepriseDef_b);

        if (repDef && !_prevRepDef) ReprisedeDefaut();
        if (raz && !_prevRaz) { Log?.Invoke("RAZ reçue → cycle annulé"); AnnulerCycle(); }
        if (dyc && !_prevDyc && Etat == Phase.Repos) DemarrerCycle();
        _prevDyc = dyc; _prevRaz = raz; _prevRepDef = repDef;

        // Défaut global = au moins un défaut TIMEOUT/DISJ injecté
        bool anyDef = false;
        for (int o = 32; o <= 51 && !anyDef; o++) if (_m.Db(1)[o] != 0) anyDef = true;
        S(A.Etat_o, 0, anyDef);

        bool bloque = PauseSurDefaut && Defaut;
        if (Etat == Phase.Repos) ForcagesManuels(dt);
        else if (!bloque) AvancerCycle(dt);

        S(A.Cyc_o, 1, Etat != Phase.Repos && Etat != Phase.FinCommande);
        S(A.FinCmd_o, A.FlecheRouge_b, bloque);
    }

    // ───────────────────────── Cycle automatique ─────────────────────────
    private void DemarrerCycle()
    {
        if (ExigerModeAuto && !B(A.ModeAuto_o, A.ModeAuto_b)) { Log?.Invoke("DYC ignoré : mode MANUEL (option 'exiger auto')"); return; }
        if (Defaut && PauseSurDefaut) { Log?.Invoke("DYC ignoré : défaut / arrêt d'urgence actif"); return; }
        float capMlx = _m.GetReal(1, A.CapMlx), capCam = _m.GetReal(1, A.CapCam);
        if (capMlx <= 0.001f) capMlx = 1f;
        NbGachees = Math.Max(1, (int)Math.Ceiling(capCam / capMlx - 1e-6));
        GacheeCourante = 0;
        S(A.FinCmd_o, A.FinCmd_b, false);
        _m.SetReal(1, A.NbGacheeCalc, NbGachees);
        _m.SetReal(1, A.NGacheeReste, NbGachees);
        Log?.Invoke($"DYC reçu → cycle démarré : {NbGachees} gâchée(s), capacité malaxeur {capMlx} m³, camion {capCam} m³");
        NouvelleGachee();
    }

    private void NouvelleGachee()
    {
        GacheeCourante++;
        float capMlx = _m.GetReal(1, A.CapMlx), capCam = _m.GetReal(1, A.CapCam);
        if (capMlx <= 0.001f) capMlx = 1f;
        double reste = capCam - (GacheeCourante - 1) * capMlx;
        double vol = Math.Max(0.01, Math.Min(capMlx, reste));
        // Les consignes de formule envoyées par l'ERP sont en kg/m³ (Form_ConfirmationBL.vb l.703)
        for (int i = 0; i < 4; i++) _cons[i] = _m.GetReal(1, A.FormTr[i]) * vol;
        _cons[4] = _m.GetReal(1, A.FormCim) * vol;
        _cons[5] = _m.GetReal(1, A.FormEau) * vol;
        for (int i = 0; i < 4; i++) _cons[6 + i] = _m.GetReal(1, A.FormAdj[i]) * vol;
        for (int i = 0; i < 10; i++)
        {
            double err = 1.0 + (_rnd.NextDouble() * 2 - 1) * ErreurPct / 100.0;
            _cible[i] = _cons[i] * err;
            _pose[i] = 0;
        }
        for (int i = 0; i < 4; i++) _m.SetReal(1, A.ConsTr[i], (float)_cons[i]);
        _m.SetReal(1, A.ConsCim, (float)_cons[4]);
        _m.SetReal(1, A.ConsEau, (float)_cons[5]);
        for (int i = 0; i < 4; i++) _m.SetReal(1, A.ConsAdj[i], (float)_cons[6 + i]);
        _m.SetInt(1, A.NGachee, (short)GacheeCourante);
        _m.SetReal(1, A.NGacheeReste, NbGachees - GacheeCourante + 1);
        _m.SetReal(1, A.ResteGachee, (float)Math.Max(0, reste - vol));
        S(A.Cyc_o, 0, false);
        S(A.Etat_o, 4, false);
        Etat = Phase.Dosage; _phaseT = 0;
        Log?.Invoke($"Gâchée {GacheeCourante}/{NbGachees} : dosage ({vol:0.00} m³)");
    }

    private void AvancerCycle(double dt)
    {
        _phaseT += dt;
        switch (Etat)
        {
            case Phase.Dosage: Dosage(dt); break;
            case Phase.Vidange:
                {
                    double f = Math.Max(0, 1 - _phaseT / TempsVidangeS);
                    SetPoids((float)(_pose[0] + _pose[1] + _pose[2] + _pose[3]) * (float)f, (float)_pose[4] * (float)f,
                             (float)_pose[5] * (float)f, (float)(_pose[6] + _pose[7] + _pose[8] + _pose[9]) * (float)f);
                    S(17, 5, true); S(25, 7, false); S(22, 5, true);
                    if (_phaseT >= TempsVidangeS) { S(17, 5, false); Etat = Phase.Malaxage; _phaseT = 0; SetPoids(0, 0, 0, 0); }
                    break;
                }
            case Phase.Malaxage:
                S(22, 5, true);
                if (_phaseT >= TempsMalaxageS) { Etat = Phase.Ouverture; _phaseT = 0; S(23, 0, true); }
                break;
            case Phase.Ouverture:
                if (_phaseT >= TempsOuvertureS) { S(23, 0, false); S(22, 5, GacheeCourante < NbGachees); TerminerGachee(); }
                break;
            case Phase.FinGachee:
                _tFinGachee += dt;
                if (_tFinGachee >= 1.5) { S(A.Etat_o, 4, false); ApresGachee(); }
                break;
            case Phase.AttenteConfirmation:
                // l'ERP a demandé l'arrêt (914.2) : il confirme avec 914.3 ou annule en remettant 914.2 à 0
                if (!B(A.FinCmd_o, A.DemArret_b)) { S(A.Cyc_o, 0, false); Log?.Invoke("Arrêt annulé → on continue"); ApresGachee(true); }
                else if (B(A.FinCmd_o, A.ConfArret_b)) { S(A.Cyc_o, 0, false); Log?.Invoke("Arrêt confirmé → fin de commande"); TerminerCommande(); }
                break;
        }
    }

    private void Dosage(double dt)
    {
        // Agrégats : TR1..4 l'un après l'autre sur la bascule agrégats
        double cumulAgr = 0; bool agrOk = true;
        for (int i = 0; i < 4; i++)
        {
            bool prec = i == 0 || _pose[i - 1] >= _cible[i - 1] - 1e-6;
            bool actif = prec && _pose[i] < _cible[i] - 1e-6 && _cible[i] > 0;
            if (actif) _pose[i] = Math.Min(_cible[i], _pose[i] + DebitAgrKgS * dt);
            if (_pose[i] < _cible[i] - 1e-6) agrOk = false;
            S(A.StTr[i].o, A.StTr[i].b, actif);
            cumulAgr += _pose[i];
        }
        S(16, 4, !agrOk);   // tapis en marche pendant le dosage agrégats
        bool cimActif = _pose[4] < _cible[4] - 1e-6 && _cible[4] > 0;
        if (cimActif) _pose[4] = Math.Min(_cible[4], _pose[4] + DebitCimKgS * dt);
        S(18, 0, cimActif);
        bool eauActif = _pose[5] < _cible[5] - 1e-6 && _cible[5] > 0;
        if (eauActif) _pose[5] = Math.Min(_cible[5], _pose[5] + DebitEauLS * dt);
        S(22, 4, eauActif);
        double cumulAdj = 0; bool adjOk = true;
        for (int i = 6; i < 10; i++)
        {
            bool actif = _pose[i] < _cible[i] - 1e-6 && _cible[i] > 0;
            if (actif) _pose[i] = Math.Min(_cible[i], _pose[i] + DebitAdjKgS * dt);
            if (_pose[i] < _cible[i] - 1e-6) adjOk = false;
            S(A.StAdj[i - 6].o, A.StAdj[i - 6].b, actif);
            cumulAdj += _pose[i];
        }
        SetPoids((float)cumulAgr, (float)_pose[4], (float)_pose[5], (float)cumulAdj);
        if (agrOk && !cimActif && !eauActif && adjOk)
        {
            S(16, 4, false);
            Etat = Phase.Vidange; _phaseT = 0;
            Log?.Invoke($"Gâchée {GacheeCourante} : dosage terminé → vidange dans le malaxeur");
        }
    }

    private void SetPoids(float agr, float cim, float eau, float adj)
    {
        _m.SetReal(1, A.PoidsAgr, agr); _m.SetReal(1, A.PoidsCim, cim);
        _m.SetReal(1, A.PoidsEau, eau); _m.SetReal(1, A.PoidsAdj, adj);
    }

    private void TerminerGachee()
    {
        Matricule++;
        int k = (Matricule - 1) % 40;
        // Pose / écart dans DB71 (colonne par colonne)
        double[] poses = _pose;
        int[] idxVers71 = { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9 }; // TR1-4, CIM(S1), EAU, ADJ1-4 (même ordre que _pose)
        for (int i = 0; i < 10; i++)
        {
            _m.SetReal(71, A.P71[i] + 4 * k, (float)poses[i]);
            _m.SetReal(71, A.E71[i] + 4 * k, (float)(poses[i] - _cons[i]));
        }
        _m.SetInt(71, A.AjoutEau71 + 2 * k, 0);
        _m.SetDInt(71, A.Compteur71 + 4 * k, Matricule);   // écrit en dernier : c'est le déclencheur côté ERP

        // Valeurs « live » DB1 (poses, écarts, réels, théoriques)
        for (int i = 0; i < 4; i++)
        {
            _m.SetReal(1, A.PoseTr[i], (float)_pose[i]); _m.SetReal(1, A.EcarTr[i], (float)(_pose[i] - _cons[i]));
            _m.SetReal(1, A.ReelTr[i], (float)_pose[i]); _m.SetReal(1, A.ThTr[i], (float)_cons[i]);
        }
        _m.SetReal(1, A.PoseCimS1, (float)_pose[4]); _m.SetReal(1, A.EcarCim, (float)(_pose[4] - _cons[4]));
        _m.SetReal(1, A.ReelCim, (float)_pose[4]); _m.SetReal(1, A.ThCim, (float)_cons[4]);
        _m.SetReal(1, A.PoseEau, (float)_pose[5]); _m.SetReal(1, A.EcarEau, (float)(_pose[5] - _cons[5]));
        _m.SetReal(1, A.ReelEau, (float)_pose[5]); _m.SetReal(1, A.ThEau, (float)_cons[5]);
        for (int i = 0; i < 4; i++)
        {
            _m.SetReal(1, A.PoseAdj[i], (float)_pose[6 + i]); _m.SetReal(1, A.EcarAdj[i], (float)(_pose[6 + i] - _cons[6 + i]));
            _m.SetReal(1, A.ReelAdj[i], (float)_pose[6 + i]); _m.SetReal(1, A.ThAdj[i], (float)_cons[6 + i]);
        }
        _m.SetReal(1, A.NGacheeTotal, Matricule);
        _m.SetDInt(1, A.CptGacheeAgr, Matricule); _m.SetDInt(1, A.CptGacheeCim, Matricule);

        S(A.Etat_o, 4, true);     // fin gâchée (l'ERP ne la remet à 0 qu'en mode non-DB71 → on la relâche nous-mêmes)
        _tFinGachee = 0;
        Etat = Phase.FinGachee;
        Log?.Invoke($"Gâchée {GacheeCourante}/{NbGachees} terminée (matricule CPU {Matricule}, emplacement DB71 n°{k + 1})");
    }

    private void ApresGachee(bool apresAnnulation = false)
    {
        bool derniere = GacheeCourante >= NbGachees;
        bool demande = B(A.FinCmd_o, A.DemArret_b);
        if (!apresAnnulation && demande && !derniere)
        {
            S(A.Cyc_o, 0, true);   // 1088.0 : gâchée en cours finie → l'ERP affiche OUI/NON
            Etat = Phase.AttenteConfirmation;
            Log?.Invoke("Arrêt demandé (914.2) : gâchée finie, en attente de confirmation (914.3)");
            return;
        }
        if (derniere) TerminerCommande(); else NouvelleGachee();
    }

    private void TerminerCommande()
    {
        S(A.FinCmd_o, A.FinCmd_b, true);        // 914.1 : fin de commande
        S(A.Dyc_o, A.Dyc_b, false);             // le CPU remet DYC à 0 (Synoptique.vb l.2482)
        S(A.Cyc_o, 1, false);
        _prevDyc = false;
        Etat = Phase.Repos;
        Log?.Invoke("Commande terminée (914.1 = 1, DYC remis à 0)");
    }

    private void AnnulerCycle()
    {
        Etat = Phase.Repos;
        for (int i = 0; i < 4; i++) { S(A.StTr[i].o, A.StTr[i].b, false); S(A.StAdj[i].o, A.StAdj[i].b, false); }
        S(16, 4, false); S(18, 0, false); S(22, 4, false); S(22, 5, false); S(23, 0, false);
        S(A.Dyc_o, A.Dyc_b, false); S(A.Cyc_o, 0, false); S(A.Cyc_o, 1, false); S(A.Etat_o, 4, false);
        SetPoids(0, 0, 0, 0);
        _prevDyc = false;
    }

    private void ReprisedeDefaut()
    {
        Array.Clear(_m.Db(1), 32, 20);   // TIMEOUT + DISJ (octets 32..51)
        S(A.Etat_o, 0, false); S(A.Etat_o, 1, false);
        Log?.Invoke("Reprise défaut (914.0) : défauts effacés");
    }

    // ───────────────────────── Forçages manuels ─────────────────────────
    private void ForcagesManuels(double dt)
    {
        float agr = _m.GetReal(1, A.PoidsAgr), cim = _m.GetReal(1, A.PoidsCim), eau = _m.GetReal(1, A.PoidsEau), adj = _m.GetReal(1, A.PoidsAdj);
        for (int i = 0; i < 4; i++)
        {
            bool f = B(A.ForceTr[i].o, A.ForceTr[i].b);
            S(A.StTr[i].o, A.StTr[i].b, f);
            if (f) agr += (float)(DebitAgrKgS * dt);
        }
        bool tapis = B(3, 5);
        S(16, 4, tapis);
        if (tapis) agr += (float)(DebitAgrKgS * dt * 0.5);
        bool v1 = B(5, 1), v2 = B(5, 2);
        S(18, 0, v1); S(18, 1, v2);
        if (v1 || v2) cim += (float)(DebitCimKgS * dt);
        if (B(6, 5)) cim = Math.Max(0, cim - (float)(DebitCimKgS * 3 * dt));
        bool vanne = B(9, 3);
        S(22, 4, vanne);
        if (vanne) eau += (float)(DebitEauLS * dt);
        if (B(9, 5)) eau = Math.Max(0, eau - (float)(DebitEauLS * 3 * dt));
        for (int i = 0; i < 4; i++)
        {
            bool f = B(A.ForceAdj[i].o, A.ForceAdj[i].b);
            S(A.StAdj[i].o, A.StAdj[i].b, f);
            if (f) adj += (float)(DebitAdjKgS * dt);
        }
        if (B(9, 2)) adj = Math.Max(0, adj - (float)(DebitAdjKgS * 3 * dt));
        // skip
        bool monte = B(4, 5), desc = B(4, 6);
        S(17, 4, monte); S(17, 5, desc);
        S(25, 5, monte); S(25, 7, desc);
        if (B(0, 5) || B(0, 6) || B(0, 7) || B(1, 0)) { /* trémies forcées : déjà traité */ }
        // malaxeur forcé (1088.2) / trappe malaxeur
        S(22, 5, B(A.Cyc_o, 2));
        if (B(10, 0)) S(23, 0, true);
        if (B(10, 2)) S(23, 0, false);
        if (agr > 0 && !tapis && !B(0, 5) && !B(0, 6) && !B(0, 7) && !B(1, 0) && !(monte || desc)) { /* poids retenu */ }
        SetPoids(agr, cim, eau, adj);
    }

    // ───────────────────────── Aide pour l'interface ─────────────────────────
    public void SetDefaut(int octet, int bit, bool v)
    {
        S(octet, bit, v);
        Log?.Invoke($"Défaut DB1.DBX{octet}.{bit} = {(v ? 1 : 0)}");
    }
    public void ArretUrgence(bool v) { S(A.Etat_o, 1, v); Log?.Invoke("Arrêt d'urgence " + (v ? "ACTIVÉ" : "relâché")); }
    public void Reinitialiser()
    {
        AnnulerCycle();
        Matricule = 0; GacheeCourante = 0; NbGachees = 0;
        _m.Clear(1); _m.Clear(71);
        InitialiserEtatRepos();
        Log?.Invoke("Mémoire DB1 / DB71 remise à zéro");
    }
}
