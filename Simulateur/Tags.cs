namespace SimulateurAutomate;

public enum TagType { Bit, Real, Int, DInt }

public sealed record Tag(string Groupe, string Nom, int Db, int Offset, int Bit, TagType Type)
{
    public string Adresse => Type switch
    {
        TagType.Bit => $"DB{Db}.DBX{Offset}.{Bit}",
        TagType.Real => $"DB{Db}.DBD{Offset}",
        TagType.DInt => $"DB{Db}.DBD{Offset}",
        _ => $"DB{Db}.DBW{Offset}",
    };
}

/// <summary>Variables connues de l'ERP (source : ModulePLCDuplex.vb, classe PLCData, et formulaires).</summary>
public static class Tags
{
    public static readonly List<Tag> All = Build();

    private static List<Tag> Build()
    {
        var l = new List<Tag>();
        void Bit(string g, string n, int o, int b) => l.Add(new Tag(g, n, 1, o, b, TagType.Bit));
        void Real(string g, string n, int o) => l.Add(new Tag(g, n, 1, o, 0, TagType.Real));
        void Int(string g, string n, int o) => l.Add(new Tag(g, n, 1, o, 0, TagType.Int));
        void DInt(string g, string n, int o) => l.Add(new Tag(g, n, 1, o, 0, TagType.DInt));

        // Commandes PC → CPU
        Bit("Commandes", "Mode AUTO (1) / MANUEL (0)", 0, 0);
        Bit("Commandes", "DYC - Départ cycle", 0, 4);
        Bit("Commandes", "Reprise défaut (impulsion)", 914, 0);
        Bit("Commandes", "Fin de commande (CPU→PC)", 914, 1);
        Bit("Commandes", "Demande arrêt fin de gâchée", 914, 2);
        Bit("Commandes", "Confirmation arrêt", 914, 3);
        Bit("Commandes", "RAZ (impulsion)", 914, 4);
        Bit("Commandes", "Vidange béton autorisée", 914, 5);
        Bit("Commandes", "Flèche rouge malaxeur / blocage", 914, 6);
        Int("Commandes", "VisActif (DBW52)", 52);
        Int("Commandes", "Correction eau (DBW1194)", 1194);
        // Forçages
        Bit("Forçages", "Trémie 1", 0, 5); Bit("Forçages", "Trémie 2", 0, 6); Bit("Forçages", "Trémie 3", 0, 7); Bit("Forçages", "Trémie 4", 1, 0);
        Bit("Forçages", "Tapis", 3, 5);
        Bit("Forçages", "Skip montée", 4, 5); Bit("Forçages", "Skip descente", 4, 6);
        Bit("Forçages", "Vis ciment 1", 5, 1); Bit("Forçages", "Vis ciment 2", 5, 2); Bit("Forçages", "Trappe ciment", 6, 5);
        Bit("Forçages", "Adjuvant 1", 8, 1); Bit("Forçages", "Adjuvant 2", 8, 2); Bit("Forçages", "Adjuvant 3", 8, 3); Bit("Forçages", "Adjuvant 4", 8, 4);
        Bit("Forçages", "Trappe adjuvant", 9, 2); Bit("Forçages", "Vanne eau", 9, 3); Bit("Forçages", "Trappe eau", 9, 5);
        Bit("Forçages", "Trappe malaxeur ouvrir", 10, 0); Bit("Forçages", "Trappe malaxeur fermer", 10, 2);
        Bit("Forçages", "Malaxeur forcé", 1088, 2);
        // États CPU → PC
        Bit("États", "Trémie 1 active", 12, 0); Bit("États", "Trémie 2 active", 12, 2); Bit("États", "Trémie 3 active", 12, 4); Bit("États", "Trémie 4 active", 12, 6);
        Bit("États", "Tapis en marche", 16, 4);
        Bit("États", "Skip monte", 17, 4); Bit("États", "Skip descend", 17, 5);
        Bit("États", "Vis ciment 1", 18, 0); Bit("États", "Vis ciment 2", 18, 1);
        Bit("États", "Adjuvant 1 actif", 21, 0); Bit("États", "Adjuvant 2 actif", 21, 1); Bit("États", "Adjuvant 3 actif", 21, 2); Bit("États", "Adjuvant 4 actif", 21, 3);
        Bit("États", "Eau (vanne)", 22, 4); Bit("États", "Malaxeur ON", 22, 5); Bit("États", "Trappe malaxeur ouverte", 23, 0);
        Bit("États", "Skip haut", 25, 5); Bit("États", "Skip attente", 25, 6); Bit("États", "Skip bas", 25, 7);
        Bit("États", "Flèche verte (0 = allumée)", 30, 1);
        Bit("États", "Défaut actif", 1082, 0); Bit("États", "Arrêt d'urgence", 1082, 1);
        Bit("États", "Malaxeur plein", 1082, 2); Bit("États", "Skip plein", 1082, 3); Bit("États", "Fin de gâchée", 1082, 4);
        Bit("États", "Gâchée en cours finie", 1088, 0); Bit("États", "Cycle en cours", 1088, 1);
        // Pesée
        Real("Poids", "Poids ciment", 78); Real("Poids", "Poids eau", 82); Real("Poids", "Poids agrégats", 86); Real("Poids", "Poids adjuvants", 90);
        Real("Capacités", "Capacité malaxeur (m³)", 916); Real("Capacités", "Capacité camion (m³)", 920);
        Int("Compteurs", "N° gâchée courante", 1076); Real("Compteurs", "Total gâchées", 1078);
        Real("Compteurs", "Gâchées restantes", 1048); Real("Compteurs", "Nb gâchées calculé", 1220);
        // Formule (envoyée par l'ERP, kg/m³)
        string[] fn = { "TR1", "TR2", "TR3", "TR4" }; int[] fo = { 538, 542, 546, 550 };
        for (int i = 0; i < 4; i++) Real("Formule (kg/m³)", fn[i], fo[i]);
        Real("Formule (kg/m³)", "CIM", 530); Real("Formule (kg/m³)", "EAU", 586);
        for (int i = 0; i < 4; i++) Real("Formule (kg/m³)", "ADJ" + (i + 1), 594 + 4 * i);
        // Consignes de gâchée / poses / écarts
        string[] cn = { "TR1(G1)", "TR2(SC)", "TR3(SL)", "TR4(D2)" };
        for (int i = 0; i < 4; i++) { Real("Consigne gâchée", cn[i], 118 + 4 * i); Real("Pose", cn[i], 230 + 4 * i); Real("Écart", cn[i], 346 + 4 * i); }
        Real("Consigne gâchée", "CIM", 94); Real("Consigne gâchée", "EAU", 166);
        Real("Pose", "CIM S1", 206); Real("Pose", "CIM S2", 210); Real("Pose", "EAU", 278);
        Real("Écart", "CIM", 318); Real("Écart", "EAU", 394);
        for (int i = 0; i < 4; i++) { Real("Consigne gâchée", "ADJ" + (i + 1), 174 + 4 * i); Real("Pose", "ADJ" + (i + 1), 286 + 4 * i); Real("Écart", "ADJ" + (i + 1), 402 + 4 * i); }
        return l;
    }
}
