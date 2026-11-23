Imports System.Drawing
Imports System.IO
Imports Microsoft.Data.SqlClient
Imports System.Data
Imports S7.Net

Public Class Synoptique
    ' ── Variables config éléments (chargées depuis SQL) ──────────
    Private _nomTR(4) As String
    Private _codeMatTR(4) As String      ' 26/09 : code matériau configuré (en-têtes du tableau de poids)
    Private _nomS(2) As String
    Private _nomADJ(4) As String : Private _nomEAU As String
    ' ═══════════════════════════════════════════════════════════════
    '  LABELS DYNAMIQUES — 1 label par élément (trémie/silo/adj/eau)
    '  Clé = code élément  ex: "TR1", "TR2", "S1", "S2", "ADJ1", "EAU"
    ' ═══════════════════════════════════════════════════════════════
    Private _labelsStock As New Dictionary(Of String, Label)()
    Private _labelsNom As New Dictionary(Of String, Label)()
    ' ═══ BITS CAPTEURS DENSITÉ M26 / M25 / M6 / M7 / M5 ════════════
    Private _bitDens1 As Boolean = False  ' M26.0 → Picture1
    Private _bitDens2 As Boolean = False  ' M26.1 → Picture2
    Private _bitDens3 As Boolean = False  ' M26.2 → Picture3
    Private _bitDens4 As Boolean = False  ' M26.3 → Picture4
    Private _bitDens5 As Boolean = False  ' M25.4 → Picture5
    Private _bitDens6 As Boolean = False  ' M25.2 → Picture6
    Private _bitDens7 As Boolean = False  ' M25.1 → Picture7
    Private _bitDens8 As Boolean = False  ' M25.0 → Picture8
    Private _bitDens9 As Boolean = False  ' M25.3 → Picture9
    Private _bitDens10 As Boolean = False ' M6.7  → Picture10
    Private _bitDens11 As Boolean = False ' M7.0  → Picture11
    Private _bitDens12 As Boolean = False ' M5.3  → Picture12
    Private _visEnEcriture As Boolean = False
    '  VARIABLES ETAT
    Private _etatTremie(8) As Integer
    Private _etatTapis As Integer
    Private _etatSkip As Integer
    Private _etatMalaxeur As Integer
    Private _etatSilo(4) As Integer
    Private _etatBasculeCim As Integer
    Private _etatPompe(2) As Integer
    Private _etatBasculeEau As Integer
    Private _etatAdj(4) As Integer
    Private _etatBasculeAdj As Integer

    '  VARIABLES PLC TEMPS REEL
    Friend _plc As Plc = Nothing
    Friend _plcConnecte As Boolean = False
    Private _tickCount As Integer = 0
    Private _poids_AGR As Single = 0
    Private _poids_CIM As Single = 0
    Private _poids_EAU As Single = 0
    Private _poids_ADJ As Single = 0
    Private _nGachee As Integer = 0
    Private _malaxeurON As Boolean = False
    Private _cycleEnCours As Boolean = False
    Private _arretUrg As Boolean = False
    Private _modeAuto As Boolean = False      ' M9.1
    Private _defautActif As Boolean = False   ' M5.0
    Private _finChargement As Boolean = False ' M50.3
    Private _vidangeMlx As Boolean = False    ' M58.3
    Private _consigneTapis As Single = 0  ' DB8 DBD126
    Private _poidsTapis As Single = 0     ' DB4 DBD0
    Private _tapisPlein As Boolean = False
    Private _skipPlein As Boolean = False   ' M57.1
    Private _skipHaut As Boolean = False    ' M25.0
    Private _skipAttente As Boolean = False ' M25.1
    Private _skipBas As Boolean = False     ' M25.2
    Private _malaxeurPlein As Boolean = False   ' M57.0
    ' ═══════════════════════════════════════════════════════════════
    '  VARIABLES TABLEAU POIDS — MAPPING CPU REEL
    '  CONSIGNES : DB8 / DB18 / DB78
    '  POSES     : DB18
    '  ECARTS    : DB18 / DB4  (lus depuis CPU, pas recalcules)
    ' ═══════════════════════════════════════════════════════════════

    ' --- Consignes ---
    Private _cons_G1 As Single = 0   ' DB8  DBD0
    Private _cons_SC As Single = 0   ' DB8  DBD4
    Private _cons_SL As Single = 0   ' DB8  DBD8
    Private _cons_D2 As Single = 0   ' DB8  DBD12
    Private _cons_CIM As Single = 0   ' DB8  DBD16  (commun Silo1 et Silo2)
    Private _cons_EAU2 As Single = 0   ' DB18 DBD20
    Private _cons_ADJ1 As Single = 0   ' DB8  DBD216
    Private _cons_ADJ2 As Single = 0   ' DB8  DBD228
    Private _cons_ADJ3 As Single = 0   ' DB78 DBD0
    Private _cons_ADJ4 As Single = 0   ' DB78 DBD4

    ' --- Poses (poids reels) ---
    Private _pose_G1 As Single = 0   ' DB18 DBD2
    Private _pose_SC As Single = 0   ' DB18 DBD6
    Private _pose_SL As Single = 0   ' DB18 DBD10
    Private _pose_D2 As Single = 0   ' DB18 DBD14
    Private _pose_CIM_S1 As Single = 0   ' DB18 DBD18  Silo 1
    Private _pose_CIM_S2 As Single = 0   ' DB18 DBD58  Silo 2
    Private _pose_EAU2 As Single = 0   ' DB18 DBD22
    Private _pose_ADJ1 As Single = 0   ' DB18 DBD62
    Private _pose_ADJ2 As Single = 0   ' DB18 DBD66
    Private _pose_ADJ3 As Single = 0   ' DB18 DBD70
    Private _pose_ADJ4 As Single = 0   ' DB18 DBD74

    ' --- Ecarts lus depuis CPU ---
    Private _ecar_G1 As Single = 0   ' DB18 DBD26
    Private _ecar_SC As Single = 0   ' DB18 DBD30
    Private _ecar_SL As Single = 0   ' DB18 DBD34
    Private _ecar_D2 As Single = 0   ' DB18 DBD38
    Private _ecar_CIM As Single = 0   ' DB18 DBD42  (commun Silo1 et Silo2)
    Private _ecar_EAU2 As Single = 0   ' DB18 DBD46
    Private _ecar_ADJ1 As Single = 0   ' DB4  DBD98
    Private _ecar_ADJ2 As Single = 0   ' DB4  DBD102
    Private _ecar_ADJ3 As Single = 0   ' DB4  DBD114
    Private _ecar_ADJ4 As Single = 0   ' DB4  DBD118

    ' --- Ciment : 1 seul silo actif a la fois ---
    Private _siloCimentActif As Integer = 1   ' 1 ou 2

    ' --- Capacite malaxeur et camion ---

    ' --- Variables CPU supplementaires ---
    Private _capMalaxeur As Single = 0   ' DB19  DBD58
    Private _capCamion As Single = 0   ' DB19  DBD66

    ' Dosage theorique camion (DB101 DBD80-116) — calcule par CPU
    Private _th_G1 As Single = 0   ' DB101 DBD80
    Private _th_SC As Single = 0   ' DB101 DBD84
    Private _th_SL As Single = 0   ' DB101 DBD88
    Private _th_D2 As Single = 0   ' DB101 DBD92
    Private _th_CIM As Single = 0   ' DB101 DBD96
    Private _th_EAU As Single = 0   ' DB101 DBD100
    Private _th_ADJ1 As Single = 0   ' DB101 DBD104
    Private _th_ADJ2 As Single = 0   ' DB101 DBD108
    Private _th_ADJ3 As Single = 0   ' DB101 DBD112
    Private _th_ADJ4 As Single = 0   ' DB101 DBD116

    ' Dosage reel camion (DB65)
    Private _reel_G1 As Single = 0   ' DB65 DBD0
    Private _reel_SC As Single = 0   ' DB65 DBD8
    Private _reel_SL As Single = 0   ' DB65 DBD16
    Private _reel_D2 As Single = 0   ' DB65 DBD24
    Private _reel_CIM As Single = 0   ' DB65 DBD32
    Private _reel_EAU As Single = 0   ' DB65 DBD48
    Private _reel_ADJ1 As Single = 0   ' DB65 DBD56
    Private _reel_ADJ2 As Single = 0   ' DB65 DBD64
    Private _reel_ADJ3 As Single = 0   ' DB65 DBD80
    Private _reel_ADJ4 As Single = 0   ' DB65 DBD88

    ' Ecart % camion (DB114)
    Private _pctEcar_G1 As Single = 0   ' DB114 DBD0
    Private _pctEcar_SC As Single = 0   ' DB114 DBD4
    Private _pctEcar_SL As Single = 0   ' DB114 DBD8
    Private _pctEcar_D2 As Single = 0   ' DB114 DBD12
    Private _pctEcar_CIM As Single = 0   ' DB114 DBD16
    Private _pctEcar_EAU As Single = 0   ' DB114 DBD20
    Private _pctEcar_ADJ1 As Single = 0   ' DB114 DBD24
    Private _pctEcar_ADJ2 As Single = 0   ' DB114 DBD28
    Private _pctEcar_ADJ3 As Single = 0   ' DB114 DBD32
    Private _pctEcar_ADJ4 As Single = 0   ' DB114 DBD36
    ' ── Clignotement fleche ──────────────────────────────────────
    Private _flecheVisible As Boolean = True
    Private _bitTr1 As Boolean = False   ' M12.0
    Private _bitTr2 As Boolean = False   ' M12.1
    Private _bitTr3 As Boolean = False   ' M12.2
    Private _bitTr4 As Boolean = False   ' M12.3
    Private _bitMx As Boolean = False    ' M15.1
    Private _bitMxVert As Boolean = False   ' DB1.DBX30.1
    Private _bitMxRouge As Boolean = False  ' DB1.DBX914.6
    Private _bitS1 As Boolean = False    ' M13.2
    Private _bitS2 As Boolean = False    ' M13.3
    Private _bitSl As Boolean = False    ' M13.4
    Private _bitEa1 As Boolean = False   ' M13.7
    Private _bitEau As Boolean = False   ' M14.1
    Private _bitAdj1 As Boolean = False  ' M14.3
    Private _bitAdj2 As Boolean = False  ' M14.4
    Private _bitAdj3 As Boolean = False  ' M14.5
    Private _bitAdj4 As Boolean = False  ' M14.6
    Private _bitEaadjj As Boolean = False ' M19.6

    ' --- Tolerances ---
    Private Const TOL_KG As Single = 2.0F
    Private Const TOL_L As Single = 1.0F
    Private Const TOL_ADJ As Single = 0.05F

    ' --- 10/11 : Tolérances % lues dans la CPU (DB8, REAL) ---
    '  index : 0=G1 (DBD24) 1=SC (28) 2=SL (32) 3=D2 (36) 4=CIM (40) 5=EAU (44)
    '          6..9=ADJ1..4 (DBD334/338/342/346)
    '  Rouge = pose > consigne + consigne × tolérance%.
    '  Si la lecture échoue ou si la tolérance vaut 0 → valeurs fixes TOL_KG / TOL_L / TOL_ADJ.
    Private _tolPct(9) As Single
    Private _tolPctLue As Boolean = False

    Private Sub LireTolerancesCPU()
        Try
            If Not PLCData.Connecte Then Exit Sub
            Dim a() As Byte = ModulePLCDuplex.LireBytesDB(8, 24, 24)    ' DB8.DBD24 .. DBD44
            Dim b() As Byte = ModulePLCDuplex.LireBytesDB(8, 334, 16)   ' DB8.DBD334 .. DBD346
            If a Is Nothing OrElse a.Length < 24 OrElse b Is Nothing OrElse b.Length < 16 Then Exit Sub
            Dim t(9) As Single
            For i As Integer = 0 To 5 : t(i) = ReelTolBE(a, i * 4) : Next
            For i As Integer = 0 To 3 : t(6 + i) = ReelTolBE(b, i * 4) : Next
            _tolPct = t
            _tolPctLue = True
        Catch ex As Exception
            Debug.WriteLine("LireTolerancesCPU : " & ex.Message)
        End Try
    End Sub

    Private Shared Function ReelTolBE(data() As Byte, off As Integer) As Single
        Dim inv() As Byte = {data(off + 3), data(off + 2), data(off + 1), data(off)}
        Dim v As Single = BitConverter.ToSingle(inv, 0)
        If Single.IsNaN(v) OrElse Single.IsInfinity(v) Then Return 0
        Return v
    End Function

    ' Tolérance absolue (kg / L) = consigne × tolérance% CPU ; sinon valeur fixe
    Private Function TolAbs(index As Integer, consigne As Single, tolFixe As Single) As Single
        If Not _tolPctLue OrElse index < 0 OrElse index > 9 Then Return tolFixe
        Dim pct As Single = _tolPct(index)
        If pct <= 0 OrElse pct > 50 Then Return tolFixe
        Return Math.Abs(consigne) * pct / 100.0F
    End Function

    ' --- DataGridViews ---
    Private WithEvents timerHistorique As New Timer()
    Private WithEvents Timer2 As New Timer()   ' Timer SQL 500ms

    Private DossierImages As String = ""
    Private _cacheImages As New Dictionary(Of String, Image)()

    Private Sub InitDossier()
        DossierImages = Application.StartupPath & "\images\Synoptique\"
    End Sub

    ' Charger image avec cache — evite relecture disque a chaque tick
    Private Function ChargerImageCachee(chemin As String) As Image
        If _cacheImages.ContainsKey(chemin) Then
            Return _cacheImages(chemin)
        End If
        If File.Exists(chemin) Then
            Try
                Dim ms As New MemoryStream(File.ReadAllBytes(chemin))
                Dim img As Image = Image.FromStream(ms)
                _cacheImages(chemin) = img
                Return img
            Catch
            End Try
        End If
        Return Nothing
    End Function
    ' ═══════════════════════════════════════════════════════════════
    '  CLIGNOTEMENT FLECHE — M=1 affiche flech.png, M=0 cache
    ' ═══════════════════════════════════════════════════════════════
    Private Sub AppliquerFleche(pb As PictureBox, bitActif As Boolean)
        ' 22/09 : cache d'état dans Tag → pas de réaffectation d'image à chaque cycle
        Dim etat As String = If(bitActif, "on", "off")
        If String.Equals(TryCast(pb.Tag, String), etat) Then Exit Sub
        pb.Tag = etat
        If bitActif Then
            Dim chemin As String = DossierImages & "flech.jpg"
            Dim img As Image = ChargerImageCachee(chemin)
            If img IsNot Nothing Then
                pb.Image = img
                pb.SizeMode = PictureBoxSizeMode.StretchImage
                pb.BackColor = Color.Transparent
            Else
                pb.Image = Nothing
                pb.BackColor = Color.LimeGreen
            End If
            pb.Visible = True
        Else
            pb.Visible = False
            pb.Image = Nothing
        End If
    End Sub
    ' 19/09 : flèche VERTE (bitVert) ou ROUGE (bitRouge), clignotante (~0,5 s) ;
    '         rouge prioritaire ; aucune → cachée. Images flechvert / flechrouge (.png ou .jpg)
    Private Sub AppliquerFlecheCouleur(pb As PictureBox, bitVert As Boolean, bitRouge As Boolean)
        Dim nom As String = If(bitRouge, "flechrouge", If(bitVert, "flechvert", ""))
        If nom = "" Then
            pb.Visible = False : pb.Image = Nothing : pb.Tag = Nothing
            Exit Sub
        End If
        If Not String.Equals(TryCast(pb.Tag, String), nom) Then
            Dim img As Image = ChargerImageCachee(DossierImages & nom & ".png")
            If img Is Nothing Then img = ChargerImageCachee(DossierImages & nom & ".jpg")
            If img IsNot Nothing Then
                pb.Image = img
                pb.SizeMode = PictureBoxSizeMode.StretchImage
                pb.BackColor = Color.Transparent
            Else
                pb.Image = Nothing
                pb.BackColor = If(bitRouge, Color.Red, Color.LimeGreen)
            End If
            pb.Tag = nom
        End If
        ' Clignotement : visible 300 ms / caché 300 ms
        pb.Visible = ((Environment.TickCount \ 300) Mod 2 = 0)
    End Sub

    ' ═══════════════════════════════════════════════════════════════
    '  LOAD
    ' ═══════════════════════════════════════════════════════════════
    ' 22/09 : référence partagée vers le Synoptique affiché (Application.OpenForms ne
    '         contient pas les formulaires hébergés dans panelmain avec TopLevel = False)
    Public Shared Instance As Synoptique = Nothing
    Private Shared Function TrouverInstance() As Synoptique
        If Instance IsNot Nothing AndAlso Not Instance.IsDisposed Then Return Instance
        For Each frm As Form In Application.OpenForms
            If TypeOf frm Is Synoptique Then Return CType(frm, Synoptique)
        Next
        Return Nothing
    End Function

    Private Sub Form_Synoptique_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Instance = Me
        AfficherConfirmationFinCycle(False)   ' 21/09 : lbfin / finoui / finno cachés au démarrage
        CreerCurseurEau()                     ' 21/09 : curseur correction eau TrackBarEAU (DB1.DBW1194, Int, -20..+20)
        CreerGrilleDefauts()                  ' 22/09 : grille des défauts CPU actifs (cachée sans défaut)
        ModulePLCDuplex.DemarrerDuplex()
        AddHandler ModulePLCDuplex.NouvellesValeurs, AddressOf SurNouvellesValeursPLC
        ' ── Uniquement l interface — pas de SQL ni PLC ici ────────
        InitDossier()
        InitMenusClicDroit()
        InitClicsGaucheForcage()
        CreerJaugesSilo()                     ' 25/09 : jauges stock ciment (stock logiciel)
        InitBoutonsForcage()
        InitTableauPoids()
        InitBoutonsOperateur()       ' UNE SEULE FOIS (corrige)
        InitHistoriqueGachees()      ' 25/09 : mise en forme seule (SQL dans Shown)
        InitJournalActions()         ' 25/09 : mise en forme seule (SQL dans Shown)
        ' 25/09 : InitLabelsDepuisConfig / MettreAJourLabelsConfig / ModuleDefautsCPU.Initialiser
        '         déplacés dans Form_Synoptique_Shown (SQL hors du Load → fenêtre affichée tout de suite)
        ComboBoxvis.Items.Add("Vis 1")
        ComboBoxvis.Items.Add("Vis 2")
        ComboBoxvis.DropDownStyle = ComboBoxStyle.DropDownList
        AddHandler ComboBoxvis.SelectedIndexChanged, AddressOf ComboBoxvis_Changed

    End Sub
    ' ═══════════════════════════════════════════════════════════════
    '  RAFRAÎCHISSEMENT PILOTÉ PAR LE DUPLEX (~300 ms)
    '  Remplace Timer1 — cadences gérées par compteur :
    '   • chaque événement (~300ms) : tableau poids + labels bascules
    '   • 1 sur 3 (~1s)  : images synoptique + boutons + labels config
    '   • 1 sur 33 (~10s): archivage SQL des tags
    ' ═══════════════════════════════════════════════════════════════
    Private _majEnCours As Boolean = False
    Private _compteurDuplex As Long = 0

    Private _dernierAffichageMs As Integer = 0
    Private Const CADENCE_UI_MS As Integer = 200     ' 25/09 : 5 rafraîchissements/s maximum

    Private Sub SurNouvellesValeursPLC()
        If _majEnCours Then Exit Sub
        If Not Me.IsHandleCreated OrElse Me.IsDisposed Then Exit Sub
        ' 25/09 : limitation de cadence — le CPU envoie 5-10 évènements/s, l'écran n'en traite que 5 max
        If Environment.TickCount - _dernierAffichageMs < CADENCE_UI_MS Then Exit Sub
        _dernierAffichageMs = Environment.TickCount
        _majEnCours = True     ' 25/09 : posé AVANT le BeginInvoke → plus d'empilement quand l'écran est occupé

        Try
            Me.BeginInvoke(Sub()
                               Try
                                   _compteurDuplex += 1
                                   _plcConnecte = PLCData.Connecte
                                   _plc = Module1.PLCInstance

                                   ' ── RAPIDE : chaque cycle (~300 ms) ──
                                   SynchroniserDepuisPLCData()
                                   MettreAJourTableauPoids()
                                   MettreAJourLabels()

                                   ' ── MOYEN : ~1 seconde (1 cycle sur 3) ──
                                   If _compteurDuplex Mod 3 = 0 Then
                                       MettreAJourSynoptique()
                                       MettreAJourBoutonsOperateur()
                                       AfficherLabelsContinu()
                                   End If

                                   ' 22/09 : ArchiverTagsSQL retiré d'ici (doublon, et SQL sur le thread UI) :
                                   '         il reste Timer1 → ThreadPool toutes les 10 s
                               Finally
                                   _majEnCours = False
                               End Try
                           End Sub)
        Catch
            _majEnCours = False
        End Try
    End Sub
    ' ═══════════════════════════════════════════════════════════════
    '  MÉTHODE PUBLIQUE — appelée depuis Configuration_silos
    '  après chaque sauvegarde → recharge config + affiche labels
    ' ═══════════════════════════════════════════════════════════════
    ' ═══════════════════════════════════════════════════════════════
    '  NOMS TRÉMIES / SILOS DEPUIS LA BASE
    '  Appelée au Load ET par Configuration_silos après sauvegarde.
    '  Met à jour les en-têtes des colonnes 0-5 du tableau de poids :
    '    col 0-3 = TR1..TR4 (Cases.CodeCase)
    '    col 4-5 = Silos S1/S2 (Silos.NomSilo)
    ' ═══════════════════════════════════════════════════════════════
    Public Sub RechercherConfigEtMettreAJourLabels()
        Try
            ' ── Trémies : CodeCase de dbo.Cases → colonnes 0 à 3 ────
            ' 26/09 : dbo.Silos (table unique) — en-tête = code matériau configuré, sinon nom de l'élément
            Dim dtCases As DataTable = ModuleDb.Lire(
                "SELECT s.NumeroElement AS NumeroCase, ISNULL(NULLIF(m.CodeMateriau,''), s.NomSilo) AS CodeCase " &
                "FROM dbo.Silos s LEFT JOIN dbo.Materiaux m ON m.IdMateriau = s.IdMateriau " &
                "WHERE s.EstActif = 1 AND s.TypeElement = 'TR' AND s.NumeroElement BETWEEN 1 AND 4 ORDER BY s.NumeroElement")

            For Each row As DataRow In dtCases.Rows
                Dim num As Integer = CInt(row("NumeroCase"))
                Dim code As String = row("CodeCase").ToString().Trim()
                If num >= 1 AndAlso num <= 4 AndAlso code <> "" Then
                    ' colonne = NumeroCase - 1  (TR1 → col 0, TR4 → col 3)
                    DataGridViewdomend.Columns(num - 1).HeaderText = code
                End If
            Next

            ' ── Silo ciment : une seule colonne (4) = "CIM" ─────────
            '    (une seule bascule ciment, 21/09/2026)
            If DataGridViewdomend.ColumnCount > 4 Then
                DataGridViewdomend.Columns(4).HeaderText = "CIM"
            End If

            ' Rafraîchir l'affichage de la grille
            DataGridViewdomend.Invalidate()

        Catch ex As Exception
            Debug.WriteLine("RechercherConfigEtMettreAJourLabels : " & ex.Message)
        End Try
    End Sub
    ' ═══════════════════════════════════════════════════════════════
    '  REMPLIR LABELS DEPUIS CONFIGURATION
    '  Appeler dans : Synoptique_Load  ET  Timer2_Tick
    ' ═══════════════════════════════════════════════════════════════
    Private Sub MettreAJourLabelsConfig()
        Try

            ' ════════════════════════════════════════════════════
            '  TRÉMIES — depuis dbo.Cases
            ' ════════════════════════════════════════════════════
            Dim dtCases As DataTable = ModuleDb.Lire(
            "SELECT NumeroCase, NomCase, CodeMateriau, " &
            "       NiveauKg, CapaciteKg, SeuilAlerteKg " &
            "FROM dbo.Cases WHERE Actif = 1 ORDER BY NumeroCase")

            Dim lblsNomTR() As Label = {lblNomTR1, lblNomTR2, lblNomTR3, lblNomTR4}
            Dim lblsStockTR() As Label = {lblStockTR1, lblStockTR2, lblStockTR3, lblStockTR4}

            Dim i As Integer = 0
            For Each row As DataRow In dtCases.Rows
                If i >= 4 Then Exit For

                Dim nomCase As String = row("NomCase").ToString()
                Dim codeMat As String = row("CodeMateriau").ToString()
                Dim niveauKg As Decimal = If(IsDBNull(row("NiveauKg")), 0, CDec(row("NiveauKg")))
                Dim capaciteKg As Decimal = If(IsDBNull(row("CapaciteKg")), 1, CDec(row("CapaciteKg")))
                Dim seuilKg As Decimal = If(IsDBNull(row("SeuilAlerteKg")), 0, CDec(row("SeuilAlerteKg")))
                Dim pct As Integer = If(capaciteKg > 0, CInt(niveauKg / capaciteKg * 100), 0)
                pct = Math.Max(0, Math.Min(100, pct))

                ' ── Nom produit
                lblsNomTR(i).Text = codeMat & " - " & nomCase
                lblsNomTR(i).ForeColor = Color.DarkBlue

                ' ── Stock + niveau %
                lblsStockTR(i).Text = niveauKg.ToString("N0") & " kg  " & pct & "%"
                lblsStockTR(i).ForeColor = If(niveauKg < seuilKg, Color.Red,
                                        If(pct < 40, Color.OrangeRed,
                                        Color.DarkGreen))
                i += 1
            Next

            ' ════════════════════════════════════════════════════
            '  SILOS CIMENT — depuis dbo.Silos
            ' ════════════════════════════════════════════════════
            Dim dtSilos As DataTable = ModuleDb.Lire(
            "SELECT s.NomSilo, s.NiveauActuel, s.NiveauMini, " &
            "       s.CapaciteMax_kg, p.Reference AS ProduitRef " &
            "FROM dbo.Silos s " &
            "LEFT JOIN dbo.Produits p ON s.ProduitID = p.ProduitID " &
            "WHERE s.EstActif = 1 AND s.TypeElement = 'S' ORDER BY s.NumeroElement, s.SiloID")

            Dim lblsNomS() As Label = {lblNomS1, lblNomS2}
            Dim lblsNiveauS() As Label = {lblNiveauS1, lblNiveauS2}

            Dim j As Integer = 0
            For Each row As DataRow In dtSilos.Rows
                If j >= 2 Then Exit For

                Dim nomSilo As String = row("NomSilo").ToString()
                Dim produit As String = If(IsDBNull(row("ProduitRef")), "CIM", row("ProduitRef").ToString())
                Dim niveauPct As Decimal = If(IsDBNull(row("NiveauActuel")), 0, CDec(row("NiveauActuel")))
                Dim miniPct As Decimal = If(IsDBNull(row("NiveauMini")), 0, CDec(row("NiveauMini")))
                Dim capKg As Decimal = If(IsDBNull(row("CapaciteMax_kg")), 0, CDec(row("CapaciteMax_kg")))
                Dim niveauKg As Decimal = capKg * niveauPct / 100

                ' ── Nom produit
                lblsNomS(j).Text = produit & " - " & nomSilo
                lblsNomS(j).ForeColor = Color.DarkSlateBlue

                ' ── Niveau %  + kg
                _seuilSiloPct(j + 1) = CSng(miniPct)      ' 25/09 : seuil rouge de la jauge
                If Not _niveauSiloLu(j + 1) Then          ' 25/09 : remplacé par la valeur CPU dès qu'elle est lue
                    lblsNiveauS(j).Text = niveauKg.ToString("N0") & " kg  " &
                                       niveauPct.ToString("0.0") & "%"
                    lblsNiveauS(j).ForeColor = If(niveauPct < miniPct, Color.Red,
                                             If(niveauPct < miniPct + 10, Color.OrangeRed,
                                             Color.DarkGreen))
                End If
                j += 1
            Next

            ' ════════════════════════════════════════════════════
            '  ADJUVANTS — depuis dbo.Bascules
            ' ════════════════════════════════════════════════════
            Dim dtAdj As DataTable = ModuleDb.Lire(
            "SELECT b.NomBascule " &
            "FROM dbo.Bascules b " &
            "JOIN dbo.TypesBascule t ON b.TypeID = t.TypeID " &
            "WHERE t.NomType LIKE '%ADJUVANT%' AND b.Actif = 1 " &
            "ORDER BY b.BasculeID")

            Dim lblsAdj() As Label = {lblNomADJ1, lblNomADJ2, lblNomADJ3, lblNomADJ4}

            Dim k As Integer = 0
            For Each row As DataRow In dtAdj.Rows
                If k >= 4 Then Exit For
                lblsAdj(k).Text = row("NomBascule").ToString()
                lblsAdj(k).ForeColor = Color.DarkViolet
                k += 1
            Next

            ' Vider les adjuvants non configurés
            For m As Integer = k To 3
                lblsAdj(m).Text = "-"
                lblsAdj(m).ForeColor = Color.Gray
            Next

            ' ════════════════════════════════════════════════════
            '  EAU — depuis dbo.Bascules
            ' ════════════════════════════════════════════════════
            Dim dtEau As DataTable = ModuleDb.Lire(
            "SELECT TOP 1 b.NomBascule " &
            "FROM dbo.Bascules b " &
            "JOIN dbo.TypesBascule t ON b.TypeID = t.TypeID " &
            "WHERE t.NomType LIKE '%EAU%' AND b.Actif = 1")

            If dtEau IsNot Nothing AndAlso dtEau.Rows.Count > 0 Then
                lblNomEAU.Text = dtEau.Rows(0)("NomBascule").ToString()
                lblNomEAU.ForeColor = Color.DarkCyan
            Else
                lblNomEAU.Text = "EAU"
                lblNomEAU.ForeColor = Color.Gray
            End If

        Catch ex As Exception
            Debug.WriteLine("MettreAJourLabelsConfig : " & ex.Message)
        End Try
    End Sub
    ' ═══════════════════════════════════════════════════════════════
    '  INITIALISER LES LABELS DYNAMIQUES DEPUIS LA CONFIGURATION
    '  Appelé une seule fois dans Synoptique_Load
    ' ═══════════════════════════════════════════════════════════════
    Private Sub InitLabelsDepuisConfig()
        Try
            ' ── Supprimer les anciens labels dynamiques si rechargement ──
            For Each lbl As Label In _labelsStock.Values
                Me.Controls.Remove(lbl)
            Next
            For Each lbl As Label In _labelsNom.Values
                Me.Controls.Remove(lbl)
            Next
            _labelsStock.Clear()
            _labelsNom.Clear()

            ' ════════════════════════════════════════════════════
            '  1. TRÉMIES — depuis dbo.Cases
            ' ════════════════════════════════════════════════════
            Dim dtCases As DataTable = ModuleDb.Lire(
            "SELECT NumeroCase, NomCase, CodeMateriau, " &
            "       NiveauKg, CapaciteKg, SeuilAlerteKg, Actif " &
            "FROM dbo.Cases WHERE Actif = 1 " &
            "ORDER BY NumeroCase")

            ' Position X des 4 trémies sur le synoptique
            ' (basé sur PictureBox1=82, PB2=249, PB3=415, PB4=583)
            Dim posXTremies() As Integer = {82, 249, 415, 583}
            Dim posYTremie As Integer = 350   ' au-dessus des PictureBox (y=378)

            Dim numTremie As Integer = 0
            For Each row As DataRow In dtCases.Rows
                If numTremie >= 4 Then Exit For   ' max 4 trémies visibles

                Dim num As Integer = numTremie + 1
                Dim cle As String = "TR" & num
                Dim xPos As Integer = posXTremies(numTremie)
                Dim nomMat As String = row("CodeMateriau").ToString()
                Dim nomCase As String = row("NomCase").ToString()
                Dim niveauKg As Decimal = If(IsDBNull(row("NiveauKg")), 0, CDec(row("NiveauKg")))
                Dim capaciteKg As Decimal = If(IsDBNull(row("CapaciteKg")), 1, CDec(row("CapaciteKg")))
                Dim pct As Integer = If(capaciteKg > 0, CInt(niveauKg / capaciteKg * 100), 0)
                pct = Math.Max(0, Math.Min(100, pct))

                ' Label NOM produit (ex: "G1 / Gravier")
                Dim lblNom As New Label() With {
                .Name = "dynNom_" & cle,
                .Text = nomMat,
                .Location = New Point(xPos + 5, posYTremie),
                .Size = New Size(130, 20),
                .Font = New Font("Segoe UI", 8, FontStyle.Bold),
                .ForeColor = Color.DarkBlue,
                .BackColor = Color.Transparent,
                .TextAlign = ContentAlignment.MiddleCenter
            }

                ' Label STOCK (ex: "45 000 kg  47%")
                Dim couleur As Color = If(pct < 20, Color.Red,
                                   If(pct < 40, Color.OrangeRed, Color.DarkGreen))
                Dim lblStock As New Label() With {
                .Name = "dynStock_" & cle,
                .Text = niveauKg.ToString("N0") & " kg" & vbCrLf & pct & "%",
                .Location = New Point(xPos + 5, posYTremie + 22),
                .Size = New Size(130, 36),
                .Font = New Font("Segoe UI", 8, FontStyle.Regular),
                .ForeColor = couleur,
                .BackColor = Color.Transparent,
                .TextAlign = ContentAlignment.MiddleCenter
            }

                Me.Controls.Add(lblNom)
                Me.Controls.Add(lblStock)
                lblNom.BringToFront()
                lblStock.BringToFront()

                _labelsNom(cle) = lblNom
                _labelsStock(cle) = lblStock

                numTremie += 1
            Next

            ' ════════════════════════════════════════════════════
            '  2. SILOS CIMENT — depuis dbo.Silos
            ' ════════════════════════════════════════════════════
            Dim dtSilos As DataTable = ModuleDb.Lire(
            "SELECT s.SiloID, s.NomSilo, s.NiveauActuel, " &
            "       s.NiveauMini, s.CapaciteMax_kg, " &
            "       p.Reference AS ProduitRef, p.NomProduit " &
            "FROM dbo.Silos s " &
            "LEFT JOIN dbo.Produits p ON s.ProduitID = p.ProduitID " &
            "WHERE s.EstActif = 1 AND s.TypeElement = 'S' ORDER BY s.NumeroElement, s.SiloID")

            ' Position des silos ciment
            ' PictureBox12 = (1007, 67)   PictureBox13 = (1130, 67)
            Dim posXSilos() As Integer = {1007, 1130}
            Dim posYSilo As Integer = 40

            Dim numSilo As Integer = 0
            For Each row As DataRow In dtSilos.Rows
                If numSilo >= 2 Then Exit For

                Dim cle As String = "S" & (numSilo + 1)
                Dim xPos As Integer = posXSilos(numSilo)
                Dim niveauPct As Decimal = If(IsDBNull(row("NiveauActuel")), 0, CDec(row("NiveauActuel")))
                Dim miniPct As Decimal = If(IsDBNull(row("NiveauMini")), 0, CDec(row("NiveauMini")))
                Dim capKg As Decimal = If(IsDBNull(row("CapaciteMax_kg")), 0, CDec(row("CapaciteMax_kg")))
                Dim niveauKg As Decimal = capKg * niveauPct / 100
                Dim nomProduit As String = If(IsDBNull(row("ProduitRef")), "CIM", row("ProduitRef").ToString())
                Dim nomSilo As String = row("NomSilo").ToString()

                Dim couleur As Color = If(niveauPct < miniPct, Color.Red,
                                    If(niveauPct < miniPct + 10, Color.OrangeRed,
                                    Color.DarkGreen))

                Dim lblNom As New Label() With {
                .Name = "dynNom_" & cle,
                .Text = nomProduit,
                .Location = New Point(xPos + 5, posYSilo),
                .Size = New Size(115, 20),
                .Font = New Font("Segoe UI", 8, FontStyle.Bold),
                .ForeColor = Color.DarkSlateBlue,
                .BackColor = Color.Transparent,
                .TextAlign = ContentAlignment.MiddleCenter
            }

                Dim lblStock As New Label() With {
                .Name = "dynStock_" & cle,
                .Text = niveauKg.ToString("N0") & " kg" & vbCrLf &
                        niveauPct.ToString("0.0") & "%",
                .Location = New Point(xPos + 5, posYSilo + 22),
                .Size = New Size(115, 36),
                .Font = New Font("Segoe UI", 8, FontStyle.Regular),
                .ForeColor = couleur,
                .BackColor = Color.Transparent,
                .TextAlign = ContentAlignment.MiddleCenter
            }

                Me.Controls.Add(lblNom)
                Me.Controls.Add(lblStock)
                lblNom.BringToFront()
                lblStock.BringToFront()

                _labelsNom(cle) = lblNom
                _labelsStock(cle) = lblStock

                numSilo += 1
            Next

            ' ════════════════════════════════════════════════════
            '  3. ADJUVANTS — depuis dbo.Bascules (type ADJUVANT)
            ' ════════════════════════════════════════════════════
            Dim dtAdj As DataTable = ModuleDb.Lire(
            "SELECT b.BasculeID, b.NomBascule, b.Actif " &
            "FROM dbo.Bascules b " &
            "JOIN dbo.TypesBascule t ON b.TypeID = t.TypeID " &
            "WHERE t.NomType LIKE '%ADJUVANT%' AND b.Actif = 1 " &
            "ORDER BY b.BasculeID")

            ' Position des adjuvants
            ' PictureBoxadj1=(1465,174) PictureBoxadj2=(1516,174)
            ' PictureBoxadj3=(1568,174) PictureBoxadj4=(1620,174)
            Dim posXAdj() As Integer = {1452, 1504, 1556, 1608}
            Dim posYAdj As Integer = 148

            Dim numAdj As Integer = 0
            For Each row As DataRow In dtAdj.Rows
                If numAdj >= 4 Then Exit For

                Dim cle As String = "ADJ" & (numAdj + 1)
                Dim xPos As Integer = posXAdj(numAdj)
                Dim nom As String = row("NomBascule").ToString()
                ' Nom court : garder max 6 caractères pour tenir dans 30px
                Dim nomCourt As String = If(nom.Length > 6, nom.Substring(0, 6), nom)

                Dim lblNom As New Label() With {
                .Name = "dynNom_" & cle,
                .Text = nomCourt,
                .Location = New Point(xPos, posYAdj),
                .Size = New Size(44, 20),
                .Font = New Font("Segoe UI", 7, FontStyle.Bold),
                .ForeColor = Color.DarkViolet,
                .BackColor = Color.Transparent,
                .TextAlign = ContentAlignment.MiddleCenter
            }

                Me.Controls.Add(lblNom)
                lblNom.BringToFront()
                _labelsNom(cle) = lblNom

                numAdj += 1
            Next

            ' ════════════════════════════════════════════════════
            '  4. EAU — depuis dbo.Bascules (type EAU)
            ' ════════════════════════════════════════════════════
            Dim dtEau As DataTable = ModuleDb.Lire(
            "SELECT TOP 1 b.NomBascule FROM dbo.Bascules b " &
            "JOIN dbo.TypesBascule t ON b.TypeID = t.TypeID " &
            "WHERE t.NomType LIKE '%EAU%' AND b.Actif = 1")

            If dtEau IsNot Nothing AndAlso dtEau.Rows.Count > 0 Then
                ' PictureBoxeau position ≈ (1328, 102)
                Dim lblEau As New Label() With {
                .Name = "dynNom_EAU",
                .Text = "EAU",
                .Location = New Point(1330, 80),
                .Size = New Size(60, 18),
                .Font = New Font("Segoe UI", 7, FontStyle.Bold),
                .ForeColor = Color.DarkCyan,
                .BackColor = Color.Transparent,
                .TextAlign = ContentAlignment.MiddleCenter
            }
                Me.Controls.Add(lblEau)
                lblEau.BringToFront()
                _labelsNom("EAU") = lblEau
            End If

        Catch ex As Exception
            Debug.WriteLine("InitLabelsDepuisConfig : " & ex.Message)
        End Try
    End Sub
    Private Sub ComboBoxvis_Changed(sender As Object, e As EventArgs)
        If Not _plcConnecte Then Exit Sub
        Try
            _visEnEcriture = True
            Dim valeur As Short = CShort(ComboBoxvis.SelectedIndex)
            ModulePLCDuplex.EcrireInt("DB1.DBW52", valeur)
            EnregistrerAction("CIMENT", "Choix Vis", "Vis " & (valeur + 1), "DB1.DBW52", valeur.ToString(), "PLC_WRITE")
            ' Bloquer la relecture pendant 1s sans geler l'UI
            Dim t As New Timer() : t.Interval = 1000
            AddHandler t.Tick, Sub()
                                   _visEnEcriture = False : t.Stop() : t.Dispose()
                               End Sub
            t.Start()
        Catch ex As Exception
            _visEnEcriture = False
        End Try
    End Sub
    ' ── Shown : appele une fois apres affichage complet ──────────────
    Private Sub Form_Synoptique_Shown(sender As Object, e As EventArgs) Handles MyBase.Shown
        ' Lancer dans un thread pour ne pas bloquer l ecran
        Dim t As New System.Threading.Thread(Sub()
                                                 Try
                                                     ' 1. Creer table journal si absente (SQL rapide)
                                                     CreerTableJournalSiAbsente()

                                                     ' 2. Charger etats BDD + défauts CPU (SQL, arrière-plan)
                                                     ChargerEtatsBDD()
                                                     Try
                                                         ModuleDefautsCPU.Initialiser()
                                                     Catch ex2 As Exception
                                                         Debug.WriteLine("Initialiser defauts : " & ex2.Message)
                                                     End Try

                                                     ' 3. Labels config (SQL court, la fenêtre est déjà visible),
                                                     '    connecter PLC + demarrer timers sur UI thread
                                                     Me.BeginInvoke(Sub()
                                                                        InitLabelsDepuisConfig()
                                                                        MettreAJourLabelsConfig()
                                                                        MettreAJourSynoptique()
                                                                        ConnecterPLC()
                                                                        Timer1.Interval = 1000
                                                                        Timer1.Start()
                                                                        Timer2.Interval = 3000
                                                                        Timer2.Start()
                                                                    End Sub)
                                                     ' 4. Journal + historique (SQL en arrière-plan, affichage par BeginInvoke)
                                                     Dim dtJ As DataTable = LireJournalActions()
                                                     If dtJ IsNot Nothing AndAlso Me.IsHandleCreated Then Me.BeginInvoke(Sub() AfficherJournalActions(dtJ))
                                                     ChargerHistoriqueGachees()

                                                 Catch ex As Exception
                                                     Debug.WriteLine("Erreur Shown : " & ex.Message)
                                                 End Try
                                             End Sub)
        t.IsBackground = True
        t.Start()
    End Sub

    ' ═══════════════════════════════════════════════════════════════
    '  JOURNAL ACTIONS
    ' ═══════════════════════════════════════════════════════════════
    Private Sub CreerTableJournalSiAbsente()
        Try
            ModuleDb.Executer(
            "IF NOT EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME='JournalActions') " &
            "BEGIN CREATE TABLE dbo.JournalActions (" &
            "Id INT IDENTITY(1,1) PRIMARY KEY," &
            "DateHeure DATETIME NOT NULL DEFAULT GETDATE()," &
            "Operateur NVARCHAR(100) NULL, Module NVARCHAR(50) NULL," &
            "Action NVARCHAR(200) NULL, Detail NVARCHAR(500) NULL," &
            "Adresse_PLC NVARCHAR(50) NULL, Valeur_PLC NVARCHAR(50) NULL," &
            "TypeAction NVARCHAR(30) NULL) END")
        Catch ex As Exception
            Debug.WriteLine("Erreur creation table journal : " & ex.Message)
        End Try
    End Sub

    ' File d attente pour INSERT asynchrone — evite de bloquer l UI
    Private Shared _queueJournal As New System.Collections.Concurrent.ConcurrentQueue(Of String())
    Private Shared _threadJournal As System.Threading.Thread = Nothing
    Private Shared _stopJournal As Boolean = False

    Public Shared Sub EnregistrerAction(
        module_ As String, action As String,
        Optional detail As String = "",
        Optional adressePLC As String = "",
        Optional valeurPLC As String = "",
        Optional typeAction As String = "CLIC")

        ' Mettre en file d attente — retour immediat sans bloquer
        _queueJournal.Enqueue({NomUtilisateur, module_, action,
                                detail, adressePLC, valeurPLC, typeAction})

        ' Demarrer le thread de vidage si pas encore lance
        If _threadJournal Is Nothing OrElse Not _threadJournal.IsAlive Then
            _stopJournal = False   ' 22/09 : sinon plus aucun journal après une fermeture du Synoptique
            _threadJournal = New System.Threading.Thread(AddressOf ViderQueueJournal)
            _threadJournal.IsBackground = True
            _threadJournal.Name = "ThreadJournal"
            _threadJournal.Start()
        End If
    End Sub

    ' Thread qui vide la file et insere en SQL — 1 connexion pour N inserts
    Private Shared Sub ViderQueueJournal()
        Do While Not _stopJournal
            Try
                Dim item() As String = Nothing
                Do While _queueJournal.TryDequeue(item)
                    ModuleDb.Executer(
                    "INSERT INTO dbo.JournalActions " &
                    "(DateHeure,Operateur,Module,Action,Detail,Adresse_PLC,Valeur_PLC,TypeAction) " &
                    "VALUES (GETDATE(),@op,@mod,@act,@det,@adr,@val,@typ)",
                    Sub(cmd)
                        cmd.Parameters.AddWithValue("@op", item(0))
                        cmd.Parameters.AddWithValue("@mod", item(1))
                        cmd.Parameters.AddWithValue("@act", item(2))
                        cmd.Parameters.AddWithValue("@det", item(3))
                        cmd.Parameters.AddWithValue("@adr", item(4))
                        cmd.Parameters.AddWithValue("@val", item(5))
                        cmd.Parameters.AddWithValue("@typ", item(6))
                    End Sub)
                Loop
            Catch ex As Exception
                Debug.WriteLine("Erreur thread journal : " & ex.Message)
            End Try
            ' Attendre 200ms avant de revider
            System.Threading.Thread.Sleep(200)
        Loop
    End Sub

    Private Sub InitJournalActions()
        DataGridViewJournal.Font = New Font("Consolas", 7.5)
        DataGridViewJournal.RowHeadersVisible = False
        DataGridViewJournal.AllowUserToAddRows = False
        DataGridViewJournal.AllowUserToDeleteRows = False
        DataGridViewJournal.ReadOnly = True
        DataGridViewJournal.SelectionMode = DataGridViewSelectionMode.FullRowSelect
        DataGridViewJournal.BackgroundColor = Color.FromArgb(15, 15, 25)
        DataGridViewJournal.GridColor = Color.FromArgb(50, 50, 70)
        DataGridViewJournal.DefaultCellStyle.BackColor = Color.FromArgb(20, 20, 35)
        DataGridViewJournal.DefaultCellStyle.ForeColor = Color.FromArgb(180, 220, 180)
        DataGridViewJournal.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(11, 61, 92)
        DataGridViewJournal.ColumnHeadersDefaultCellStyle.ForeColor = Color.White
        DataGridViewJournal.ColumnHeadersDefaultCellStyle.Font = New Font("Segoe UI", 7.5, FontStyle.Bold)
        DataGridViewJournal.EnableHeadersVisualStyles = False
        DataGridViewJournal.BorderStyle = BorderStyle.Fixed3D

        Dim colonnesJ() As (String, Integer) = {
            ("Heure", 60), ("Operateur", 80), ("Module", 90),
            ("Action", 200), ("Detail", 250), ("PLC Adr", 70),
            ("PLC Val", 60), ("Type", 60)}

        Dim cj As (String, Integer)
        For Each cj In colonnesJ
            Dim col As New DataGridViewTextBoxColumn()
            col.HeaderText = cj.Item1
            col.Width = cj.Item2
            col.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft
            DataGridViewJournal.Columns.Add(col)
        Next

        ' Couleurs appliquees directement dans ChargerJournalActions (chargement initial : Shown)
    End Sub

    ' Memorise la derniere ligne pour eviter rechargement inutile
    Private _dernierHeureJournal As String = ""

    ' 22/09 : lecture SQL (thread de fond) — renvoie Nothing si rien de nouveau
    Private Function LireJournalActions() As DataTable
        Try
            Dim dt As DataTable = ModuleDb.Lire(
            "SELECT TOP 50 " &
            "  CONVERT(VARCHAR(8), DateHeure, 108)  AS Heure," &
            "  ISNULL(Operateur,'-')               AS Operateur," &
            "  ISNULL(Module,'-')                  AS Module," &
            "  ISNULL(Action,'-')                  AS Action," &
            "  ISNULL(Detail,'')                   AS Detail," &
            "  ISNULL(Adresse_PLC,'')              AS Adresse_PLC," &
            "  ISNULL(Valeur_PLC,'')               AS Valeur_PLC," &
            "  ISNULL(TypeAction,'')               AS TypeAction" &
            " FROM dbo.V_JournalRecent")
            If dt Is Nothing OrElse dt.Rows.Count = 0 Then Return Nothing
            Dim cle As String = dt.Rows(0)("Heure").ToString() & dt.Rows(0)("Action").ToString()
            If cle = _dernierHeureJournal Then Return Nothing
            _dernierHeureJournal = cle
            Return dt
        Catch ex As Exception
            Debug.WriteLine("Erreur lecture journal : " & ex.Message)
            Return Nothing
        End Try
    End Function

    ' Affichage (thread UI)
    Private Sub AfficherJournalActions(dt As DataTable)
        Try
            DataGridViewJournal.SuspendLayout()
            DataGridViewJournal.Rows.Clear()
            Dim rowJ As DataRow
            For Each rowJ In dt.Rows
                Dim idx As Integer = DataGridViewJournal.Rows.Add(
                    rowJ("Heure").ToString(), rowJ("Operateur").ToString(),
                    rowJ("Module").ToString(), rowJ("Action").ToString(),
                    rowJ("Detail").ToString(), rowJ("Adresse_PLC").ToString(),
                    rowJ("Valeur_PLC").ToString(), rowJ("TypeAction").ToString())
                Dim typeAct As String = rowJ("TypeAction").ToString().ToUpper()
                Select Case typeAct
                    Case "PLC_WRITE" : DataGridViewJournal.Rows(idx).DefaultCellStyle.ForeColor = Color.FromArgb(255, 200, 100)
                    Case "FORCAGE" : DataGridViewJournal.Rows(idx).DefaultCellStyle.ForeColor = Color.FromArgb(255, 150, 50)
                    Case "ALARME" : DataGridViewJournal.Rows(idx).DefaultCellStyle.ForeColor = Color.FromArgb(255, 80, 80)
                    Case "CONNEXION" : DataGridViewJournal.Rows(idx).DefaultCellStyle.ForeColor = Color.FromArgb(100, 255, 100)
                End Select
            Next
            DataGridViewJournal.ResumeLayout()
        Catch ex As Exception
            Debug.WriteLine("Erreur affichage journal : " & ex.Message)
        End Try
    End Sub

    ' Compatibilité (appel direct depuis le thread UI)
    Private Sub ChargerJournalActions()
        Dim dt As DataTable = LireJournalActions()
        If dt IsNot Nothing Then AfficherJournalActions(dt)
    End Sub

    ' ═══════════════════════════════════════════════════════════════
    '  HISTORIQUE GACHEES
    ' ═══════════════════════════════════════════════════════════════
    Private Sub InitHistoriqueGachees()
        DataGridViewHistorique.Font = New Font("Segoe UI", 8)
        DataGridViewHistorique.RowHeadersVisible = False
        DataGridViewHistorique.AllowUserToAddRows = False
        DataGridViewHistorique.AllowUserToDeleteRows = False
        DataGridViewHistorique.ReadOnly = True
        DataGridViewHistorique.SelectionMode = DataGridViewSelectionMode.FullRowSelect
        DataGridViewHistorique.BackgroundColor = Color.White
        DataGridViewHistorique.GridColor = Color.LightGray
        DataGridViewHistorique.BorderStyle = BorderStyle.Fixed3D
        DataGridViewHistorique.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize
        DataGridViewHistorique.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(11, 61, 92)
        DataGridViewHistorique.ColumnHeadersDefaultCellStyle.ForeColor = Color.White
        DataGridViewHistorique.ColumnHeadersDefaultCellStyle.Font = New Font("Segoe UI", 8, FontStyle.Bold)
        DataGridViewHistorique.EnableHeadersVisualStyles = False
        DataGridViewHistorique.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(245, 248, 252)

        Dim colonnesH() As (String, Integer, String) = {
            ("Heure", 68, "DateHeure"),
            ("N BL", 95, "NumeroBL"),
            ("Client", 130, "NomClient"),
            ("Chantier", 120, "NomChantier"),
            ("Formule", 95, "NomFormule"),
            ("Camion", 65, "CodeCamion"),
            ("m3", 40, "VolumeM3"),
            ("Statut", 75, "Statut")}

        Dim ch As (String, Integer, String)
        For Each ch In colonnesH
            Dim c As New DataGridViewTextBoxColumn()
            c.HeaderText = ch.Item1
            c.Width = ch.Item2
            c.Name = ch.Item3
            c.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter
            DataGridViewHistorique.Columns.Add(c)
        Next

        timerHistorique.Interval = 5000
        AddHandler timerHistorique.Tick, AddressOf timerHistorique_Tick
        timerHistorique.Start()
    End Sub

    Private _histOccupe As Boolean = False
    Private Shared ReadOnly _fontBarre As New Font("Segoe UI", 8, FontStyle.Strikeout)
    Private Shared ReadOnly _fontGras As New Font("Segoe UI", 8, FontStyle.Bold)
    Private Sub timerHistorique_Tick(sender As Object, e As EventArgs)
        ' 22/09 : SQL en arrière-plan, grille mise à jour par BeginInvoke
        If _histOccupe Then Exit Sub
        _histOccupe = True
        Threading.ThreadPool.QueueUserWorkItem(Sub(state)
                                                   Try
                                                       ChargerHistoriqueGachees()
                                                   Finally
                                                       _histOccupe = False
                                                   End Try
                                               End Sub)
    End Sub

    ' Memorise le dernier BL pour eviter rechargement inutile
    Private _dernierNumeroBL As String = ""

    Private Sub ChargerHistoriqueGachees()
        Try
            Dim dt As DataTable = ModuleDb.Lire(
            "SELECT TOP 20 " &
            "  CONVERT(VARCHAR(8),DateHeure,108)    AS DateHeure," &
            "  ISNULL(NumeroBL,'---')               AS NumeroBL," &
            "  ISNULL(NomClient,'---')              AS NomClient," &
            "  ISNULL(NomChantier,'---')            AS NomChantier," &
            "  ISNULL(NomFormule,'---')             AS NomFormule," &
            "  ISNULL(CodeCamion,'---')             AS CodeCamion," &
            "  ISNULL(CAST(VolumeM3 AS VARCHAR),'0') AS VolumeM3," &
            "  Statut " &
            "FROM dbo.BonsLivraison ORDER BY DateCreation DESC")
            If dt Is Nothing Then Exit Sub
            If dt.Rows.Count = 0 Then Exit Sub

            ' Ne recharger que si le premier BL ou son statut a changé (19/09 : EN_COURS → LIVRE)
            Dim premierBL As String = dt.Rows(0)("NumeroBL").ToString() & "|" & dt.Rows(0)("Statut").ToString()
            If premierBL = _dernierNumeroBL Then Exit Sub
            _dernierNumeroBL = premierBL

            Dim dtH As DataTable = dt
            If Me.IsHandleCreated AndAlso Not Me.IsDisposed Then
                Me.BeginInvoke(Sub()
                                   DataGridViewHistorique.SuspendLayout()
                                   DataGridViewHistorique.Rows.Clear()
                                   Dim rowH As DataRow
                                   For Each rowH In dtH.Rows
                                       Dim idx As Integer = DataGridViewHistorique.Rows.Add(
                                      rowH("DateHeure").ToString(),
                                      rowH("NumeroBL").ToString(),
                                      rowH("NomClient").ToString(),
                                      rowH("NomChantier").ToString(),
                                      rowH("NomFormule").ToString(),
                                      rowH("CodeCamion").ToString(),
                                      rowH("VolumeM3").ToString(),
                                      rowH("Statut").ToString())
                                       Dim statut As String = rowH("Statut").ToString()
                                       Select Case statut.ToUpper()
                                           Case "LIVRE"
                                               DataGridViewHistorique.Rows(idx).DefaultCellStyle.ForeColor = Color.DarkGreen
                                           Case "EN_COURS"
                                               DataGridViewHistorique.Rows(idx).DefaultCellStyle.BackColor = Color.FromArgb(255, 255, 200)
                                               DataGridViewHistorique.Rows(idx).DefaultCellStyle.ForeColor = Color.DarkOrange
                                           Case "ANNULE"
                                               DataGridViewHistorique.Rows(idx).DefaultCellStyle.ForeColor = Color.Red
                                               DataGridViewHistorique.Rows(idx).DefaultCellStyle.Font = _fontBarre
                                       End Select
                                   Next
                                   If DataGridViewHistorique.Rows.Count > 0 Then
                                       DataGridViewHistorique.Rows(0).DefaultCellStyle.BackColor = Color.FromArgb(220, 255, 220)
                                       DataGridViewHistorique.Rows(0).DefaultCellStyle.Font = _fontGras
                                   End If
                                   DataGridViewHistorique.ResumeLayout()
                               End Sub)
            End If
        Catch ex As Exception
            Debug.WriteLine("Erreur chargement historique : " & ex.Message)
        End Try
    End Sub

    Private Sub DataGridViewHistorique_CellDoubleClick(sender As Object, e As DataGridViewCellEventArgs) Handles DataGridViewHistorique.CellDoubleClick
        If e.RowIndex < 0 Then Exit Sub
        Dim row As DataGridViewRow = DataGridViewHistorique.Rows(e.RowIndex)
        Dim msg As String =
            "BL      : " & row.Cells("NumeroBL").Value.ToString() & vbCrLf &
            "Client  : " & row.Cells("NomClient").Value.ToString() & vbCrLf &
            "Chantier: " & row.Cells("NomChantier").Value.ToString() & vbCrLf &
            "Formule : " & row.Cells("NomFormule").Value.ToString() & vbCrLf &
            "Camion  : " & row.Cells("CodeCamion").Value.ToString() & vbCrLf &
            "Volume  : " & row.Cells("VolumeM3").Value.ToString() & " m3" & vbCrLf &
            "Statut  : " & row.Cells("Statut").Value.ToString()
        MessageBox.Show(msg, "Détails Gachée", MessageBoxButtons.OK, MessageBoxIcon.Information)
    End Sub

    ' ═══════════════════════════════════════════════════════════════
    '  TABLEAU POIDS TEMPS REEL
    ' ═══════════════════════════════════════════════════════════════
    Private Sub InitTableauPoids()
        ' ── 10 colonnes — mapping exact CPU (21/09/2026 : une seule
        '    bascule ciment → une seule colonne CIM, lecteur DB18 DBD18)
        '  0=G1  1=SC  2=SL  3=D2  4=CIM
        '  5=EAU  6=ADJ1  7=ADJ2  8=ADJ3  9=ADJ4
        Dim cols() As String = {"G1", "SC", "SL", "D2",
                                "CIM",
                                "EAU",
                                "ADJ1", "ADJ2", "ADJ3", "ADJ4"}
        Dim colW As Integer = 60

        ' AllowUserToAddRows=False AVANT RowCount — sinon ligne vide ajoutee
        DataGridViewdomend.AllowUserToAddRows = False
        DataGridViewdomend.AllowUserToDeleteRows = False
        DataGridViewdomend.ReadOnly = True

        DataGridViewdomend.ColumnCount = cols.Length   ' 10
        DataGridViewdomend.RowCount = 6                ' 6 lignes

        For i As Integer = 0 To cols.Length - 1
            DataGridViewdomend.Columns(i).HeaderText = cols(i)
            DataGridViewdomend.Columns(i).Width = colW
            DataGridViewdomend.Columns(i).SortMode = DataGridViewColumnSortMode.NotSortable
            DataGridViewdomend.Columns(i).DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter
            DataGridViewdomend.Columns(i).DefaultCellStyle.Font = New Font("Consolas", 9.0F, FontStyle.Bold)
        Next

        ' En-tête CIM mis en valeur (22/09 : une fois, à l'init)
        DataGridViewdomend.Columns(4).HeaderCell.Style.ForeColor = Color.Yellow
        DataGridViewdomend.Columns(4).HeaderCell.Style.BackColor = Color.FromArgb(80, 50, 0)
        DataGridViewdomend.Columns(5).DefaultCellStyle.Font = New Font("Consolas", 9.0F, FontStyle.Bold)
        DataGridViewdomend.RowHeadersWidth = 120
        DataGridViewdomend.Rows(0).HeaderCell.Value = "Consigne"
        DataGridViewdomend.Rows(1).HeaderCell.Value = "Pose"
        DataGridViewdomend.Rows(2).HeaderCell.Value = "Ecart"
        DataGridViewdomend.Rows(3).HeaderCell.Value = "Th.Cam kg"
        DataGridViewdomend.Rows(4).HeaderCell.Value = "Reel Cam kg"
        DataGridViewdomend.Rows(5).HeaderCell.Value = "Ecar% Cam"

        DataGridViewdomend.MultiSelect = False
        DataGridViewdomend.SelectionMode = DataGridViewSelectionMode.CellSelect
        DataGridViewdomend.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.None
        DataGridViewdomend.ColumnHeadersHeight = 26
        DataGridViewdomend.RowTemplate.Height = 24
        DataGridViewdomend.ScrollBars = ScrollBars.None
        DataGridViewdomend.EnableHeadersVisualStyles = False

        DataGridViewdomend.BackgroundColor = Color.FromArgb(25, 25, 25)
        DataGridViewdomend.GridColor = Color.FromArgb(70, 70, 70)
        DataGridViewdomend.DefaultCellStyle.BackColor = Color.FromArgb(40, 40, 40)
        DataGridViewdomend.DefaultCellStyle.ForeColor = Color.White
        DataGridViewdomend.DefaultCellStyle.SelectionBackColor = Color.FromArgb(60, 60, 80)
        DataGridViewdomend.DefaultCellStyle.SelectionForeColor = Color.White
        DataGridViewdomend.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(45, 45, 45)
        DataGridViewdomend.ColumnHeadersDefaultCellStyle.ForeColor = Color.Yellow
        DataGridViewdomend.ColumnHeadersDefaultCellStyle.Font = New Font("Consolas", 9.0F, FontStyle.Bold)
        DataGridViewdomend.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter
        DataGridViewdomend.RowHeadersDefaultCellStyle.BackColor = Color.FromArgb(45, 45, 45)
        DataGridViewdomend.RowHeadersDefaultCellStyle.ForeColor = Color.White
        DataGridViewdomend.RowHeadersDefaultCellStyle.Font = New Font("Consolas", 8.5F, FontStyle.Bold)

        ' Taille exacte : 120 + 10x60 + 4 = 724px | 26 + 6x24 + 2 = 172px
        DataGridViewdomend.Width = 120 + cols.Length * colW + 4
        DataGridViewdomend.Height = 26 + 6 * 24 + 2

        MettreAJourTableauPoids()
    End Sub

    ' ── Choisir silo ciment actif (clic droit en-tete CIM S1/S2) ──
    Public Sub ChoisirSiloCiment(numSilo As Integer)
        If numSilo <> 1 AndAlso numSilo <> 2 Then Exit Sub
        _siloCimentActif = numSilo
        AfficherInfoForcage("Silo CIM " & numSilo & " ACTIF")
        EnregistrerAction("CIMENT", "Silo actif", "Silo " & numSilo, "", "", "CLIC")
    End Sub

    Private Sub MettreAJourTableauPoids()
        If DataGridViewdomend Is Nothing Then Exit Sub
        If DataGridViewdomend.ColumnCount < 10 Then Exit Sub
        If DataGridViewdomend.RowCount < 6 Then Exit Sub

        Try
            DataGridViewdomend.SuspendLayout()

            ' ════════════════════════════════════════════════════
            '  MAPPING EXACT CPU — 10 colonnes (une seule bascule ciment)
            '  0=G1  1=SC  2=SL  3=D2  4=CIM
            '  5=EAU  6=ADJ1  7=ADJ2  8=ADJ3  9=ADJ4
            ' ════════════════════════════════════════════════════

            ' LIGNE 0 : CONSIGNE
            ' DB8 DBD0/4/8/12/16/16 | DB18 DBD20 | DB8 DBD216/228 | DB78 DBD0/4
            Dim arrCons() As Single = {
                _cons_G1, _cons_SC, _cons_SL, _cons_D2,
                _cons_CIM,
                _cons_EAU2,
                _cons_ADJ1, _cons_ADJ2, _cons_ADJ3, _cons_ADJ4}

            ' LIGNE 1 : POSE
            ' DB18 DBD2/6/10/14/18/58/22/62/66/70/74
            Dim arrPose() As Single = {
                _pose_G1, _pose_SC, _pose_SL, _pose_D2,
                _pose_CIM_S1,
                _pose_EAU2,
                _pose_ADJ1, _pose_ADJ2, _pose_ADJ3, _pose_ADJ4}

            ' LIGNE 2 : ECART
            ' DB18 DBD26/30/34/38/42/42 | DB18 DBD46 | DB4 DBD98/102/114/118
            Dim arrEcar() As Single = {
                _ecar_G1, _ecar_SC, _ecar_SL, _ecar_D2,
                _ecar_CIM,
                _ecar_EAU2,
                _ecar_ADJ1, _ecar_ADJ2, _ecar_ADJ3, _ecar_ADJ4}

            ' LIGNE 3 : DOSAGE THEORIQUE CAMION
            ' DB101 DBD80/84/88/92/96/100/104/108/112/116
            Dim arrTh() As Single = {
                _th_G1, _th_SC, _th_SL, _th_D2,
                _th_CIM,
                _th_EAU,
                _th_ADJ1, _th_ADJ2, _th_ADJ3, _th_ADJ4}

            ' LIGNE 4 : DOSAGE REEL CAMION
            ' DB65 DBD0/8/16/24/32/48/56/64/80/88
            Dim arrReel() As Single = {
                _reel_G1, _reel_SC, _reel_SL, _reel_D2,
                _reel_CIM,
                _reel_EAU,
                _reel_ADJ1, _reel_ADJ2, _reel_ADJ3, _reel_ADJ4}

            ' LIGNE 5 : ECART % CAMION
            ' DB114 DBD0/4/8/12/16/20/24/28/32/36
            Dim arrPct() As Single = {
                _pctEcar_G1, _pctEcar_SC, _pctEcar_SL, _pctEcar_D2,
                _pctEcar_CIM,
                _pctEcar_EAU,
                _pctEcar_ADJ1, _pctEcar_ADJ2, _pctEcar_ADJ3, _pctEcar_ADJ4}

            Dim arrTol() As Single = {
                TOL_KG, TOL_KG, TOL_KG, TOL_KG,
                TOL_KG,
                TOL_L,
                TOL_ADJ, TOL_ADJ, TOL_ADJ, TOL_ADJ}

            Dim arrFmt() As String = {
                "0.0", "0.0", "0.0", "0.0",
                "0.0",
                "0.0",
                "0.000", "0.000", "0.000", "0.000"}

            For col As Integer = 0 To 9

                ' Une seule bascule ciment : plus de colonne inactive
                Dim siloInactif As Boolean = False
                Dim pasDeDonnCamion As Boolean = False


                Dim cons As Single = arrCons(col)
                Dim pose As Single = arrPose(col)
                Dim ecar As Single = arrEcar(col)
                Dim tol As Single = TolAbs(col, cons, arrTol(col))   ' 10/11 : tolérance % CPU
                Dim fmt As String = arrFmt(col)

                ' ── LIGNE 0 : Consigne ───────────────────────────
                Cel(col, 0, If(siloInactif, "---", cons.ToString(fmt)), Color.FromArgb(40, 40, 40),
                    If(siloInactif, Color.FromArgb(65, 65, 65), Color.Yellow))

                ' ── LIGNE 1 : Pose ───────────────────────────────
                Dim fond1 As Color, texte1 As Color
                If siloInactif Then
                    fond1 = Color.FromArgb(35, 35, 35) : texte1 = Color.FromArgb(65, 65, 65)
                ElseIf cons <= 0 AndAlso pose <= 0 Then
                    fond1 = Color.FromArgb(40, 40, 40) : texte1 = Color.FromArgb(80, 80, 80)
                ElseIf pose >= cons AndAlso Math.Abs(ecar) <= tol Then
                    fond1 = Color.FromArgb(0, 110, 0) : texte1 = Color.White
                ElseIf pose < cons Then
                    fond1 = Color.FromArgb(0, 55, 120) : texte1 = Color.White
                Else
                    fond1 = Color.FromArgb(170, 0, 0) : texte1 = Color.White
                End If
                Cel(col, 1, If(siloInactif, "---", pose.ToString(fmt)), fond1, texte1)

                ' ── LIGNE 2 : Ecart ──────────────────────────────
                Dim signe As String = If(ecar >= 0, "+", "")
                Dim texte2 As Color
                If siloInactif Then
                    texte2 = Color.FromArgb(65, 65, 65)
                ElseIf cons <= 0 AndAlso pose <= 0 Then
                    texte2 = Color.FromArgb(70, 70, 70)
                ElseIf Math.Abs(ecar) <= tol Then
                    texte2 = Color.FromArgb(80, 200, 80)
                ElseIf ecar < 0 Then
                    texte2 = Color.Orange
                Else
                    texte2 = Color.Tomato
                End If
                Cel(col, 2, If(siloInactif, "---", signe & ecar.ToString(fmt)), Color.FromArgb(28, 28, 28), texte2)

                ' ── LIGNE 3 : Dosage theorique camion ────────────
                Dim thV As Single = arrTh(col)
                Cel(col, 3, If(pasDeDonnCamion, "---", thV.ToString(fmt)), Color.FromArgb(35, 35, 35),
                    If(pasDeDonnCamion, Color.FromArgb(65, 65, 65), Color.White))

                ' ── LIGNE 4 : Dosage reel camion ─────────────────
                Dim reelV As Single = arrReel(col)
                Dim texte4 As Color
                If pasDeDonnCamion Then
                    texte4 = Color.FromArgb(65, 65, 65)
                ElseIf thV <= 0 AndAlso reelV <= 0 Then
                    texte4 = Color.FromArgb(80, 80, 80)
                ElseIf Math.Abs(reelV - thV) <= TolAbs(col, thV, arrTol(col)) Then
                    texte4 = Color.FromArgb(80, 200, 80)
                Else
                    texte4 = Color.Orange
                End If
                Cel(col, 4, If(pasDeDonnCamion, "---", reelV.ToString(fmt)), Color.FromArgb(35, 35, 35), texte4)

                ' ── LIGNE 5 : Ecart % camion ─────────────────────
                Dim pctV As Single = arrPct(col)
                Dim val5 As String, texte5 As Color
                If pasDeDonnCamion Then
                    val5 = "---" : texte5 = Color.FromArgb(65, 65, 65)
                ElseIf Math.Abs(pctV) < 0.05F Then
                    val5 = "0.0 %" : texte5 = Color.FromArgb(80, 80, 80)
                ElseIf Math.Abs(pctV) <= 2.0F Then
                    val5 = pctV.ToString("+0.0;-0.0") & " %" : texte5 = Color.FromArgb(80, 200, 80)
                ElseIf Math.Abs(pctV) <= 5.0F Then
                    val5 = pctV.ToString("+0.0;-0.0") & " %" : texte5 = Color.Orange
                Else
                    val5 = pctV.ToString("+0.0;-0.0") & " %" : texte5 = Color.Tomato
                End If
                Cel(col, 5, val5, Color.FromArgb(30, 30, 30), texte5)

            Next col

            DataGridViewdomend.ResumeLayout()

        Catch ex As Exception
            Debug.WriteLine("Erreur MettreAJourTableauPoids : " & ex.Message)
            DataGridViewdomend.ResumeLayout()
        End Try
    End Sub

    ' 25/09 : écrit une cellule du tableau de poids SEULEMENT si sa valeur ou ses couleurs changent
    '         (affecter Value invalide la cellule même si la valeur est identique → 60 repaints par cycle)
    Private Sub Cel(col As Integer, row As Integer, valeur As String, fond As Color, texte As Color)
        Dim c As DataGridViewCell = DataGridViewdomend(col, row)
        If Not String.Equals(TryCast(c.Value, String), valeur) Then c.Value = valeur
        If c.Style.BackColor <> fond Then c.Style.BackColor = fond
        If c.Style.ForeColor <> texte Then c.Style.ForeColor = texte
    End Sub

    Private Sub InitClicsGaucheForcage()
        ' 25/09 : plus aucune action au clic (simple ou double) sur les images du Synoptique.
        '         Le mode manuel se fait par Dosage manuel (boutons de groupe) ; les boutons de
        '         commande apparaissent quand le groupe est en forçage. Clic droit = Stock / Paramètres.
    End Sub

    ' ═══════════════════════════════════════════════════════════════
    '  JAUGES STOCK CIMENT (25-26/09/2026) — niveau = stock logiciel (T_STOCK du matériau du silo, en kg)
    '  Dessiné dans PictureBoxSS1 / PictureBoxSS2 (Designer) : barre verticale vert/rouge ; texte lblNiveauS1/S2 idem.
    ' ═══════════════════════════════════════════════════════════════
    Private Const CAP_SILO_KG As Single = 50000          ' capacité silo : 50 t
    Private ReadOnly _niveauSiloKg(2) As Single
    Private ReadOnly _niveauSiloLu(2) As Boolean
    Private ReadOnly _seuilSiloPct(2) As Single          ' NiveauMini (%) depuis dbo.Silos, défaut 20
    Private _lectureSiloEnCours As Boolean = False

    Private Sub CreerJaugesSilo()
        ' 25/09 : niveau dessiné dans PictureBoxSS1 / PictureBoxSS2 (Designer) :
        '         barre verticale qui monte du bas, verte si niveau ≥ seuil mini, rouge sinon.
        Dim pbs() As PictureBox = {PictureBoxSS1, PictureBoxSS2}
        For i As Integer = 1 To 2
            _seuilSiloPct(i) = 20
            Dim numSilo As Integer = i
            Dim pb As PictureBox = pbs(i - 1)
            AddHandler pb.Paint, Sub(s, ev) DessinerJaugeSilo(numSilo, ev.Graphics, pb.ClientSize)
        Next
    End Sub

    Private Shared ReadOnly _brosseVert As New SolidBrush(Color.FromArgb(0, 170, 0))
    Private Shared ReadOnly _brosseRouge As New SolidBrush(Color.Red)
    Private Shared ReadOnly _brosseGris As New SolidBrush(Color.LightGray)

    Private Sub DessinerJaugeSilo(num As Integer, g As Graphics, taille As Size)
        Dim pct As Single = Math.Max(0, Math.Min(100, _niveauSiloKg(num) / CAP_SILO_KG * 100))
        Dim h As Integer = CInt(taille.Height * pct / 100)
        Dim couleur As Color = If(pct < _seuilSiloPct(num), Color.Red, Color.FromArgb(0, 170, 0))
        If Not _niveauSiloLu(num) Then couleur = Color.LightGray
        Dim b As SolidBrush = If(couleur = Color.Red, _brosseRouge, If(couleur = Color.LightGray, _brosseGris, _brosseVert))
        g.FillRectangle(b, 0, taille.Height - h, taille.Width, h)
        For k As Integer = 1 To 3              ' graduations 25 / 50 / 75 %
            Dim y As Integer = taille.Height - CInt(taille.Height * k / 4)
            g.DrawLine(Pens.DimGray, 0, y, 4, y)
        Next
    End Sub

    Private Sub LireNiveauxSilo()
        ' 26/09 : le niveau des silos vient de la GESTION DE STOCK du logiciel (T_STOCK du matériau
        '         affecté au silo, converti en kg), plus du CPU. Lecture SQL en arrière-plan (Timer2, 3 s).
        If _lectureSiloEnCours Then Exit Sub
        _lectureSiloEnCours = True
        Threading.ThreadPool.QueueUserWorkItem(
            Sub(state)
                Dim v(2) As Single
                Dim ok As Boolean = False
                Try
                    ' 26/09 : stock PAR SILO (Silos.StockKg, kg) — deux silos peuvent avoir le même matériau
                    Dim dt As DataTable = ModuleDb.Lire(
                        "SELECT ISNULL(s.StockKg,0) AS StockKg, ISNULL(s.NiveauMini,20) AS NiveauMini " &
                        "FROM dbo.Silos s WHERE s.EstActif = 1 AND s.TypeElement = 'S' ORDER BY s.NumeroElement, s.SiloID")
                    For i As Integer = 0 To Math.Min(1, dt.Rows.Count - 1)
                        v(i) = CSng(dt.Rows(i)("StockKg"))
                        _seuilSiloPct(i + 1) = CSng(dt.Rows(i)("NiveauMini"))
                    Next
                    ok = True
                Catch
                End Try
                Try
                    If ok AndAlso Me.IsHandleCreated AndAlso Not Me.IsDisposed Then
                        Me.BeginInvoke(Sub() AfficherNiveauxSilo(v(0), v(1)))
                    End If
                Catch
                End Try
                _lectureSiloEnCours = False
            End Sub)
    End Sub

    Private Sub AfficherNiveauxSilo(v1 As Single, v2 As Single)
        Dim valeurs() As Single = {v1, v2}
        Dim lbls() As Label = {lblNiveauS1, lblNiveauS2}
        For i As Integer = 1 To 2
            _niveauSiloKg(i) = valeurs(i - 1) : _niveauSiloLu(i) = True
            Dim pct As Single = Math.Max(0, Math.Min(100, valeurs(i - 1) / CAP_SILO_KG * 100))
            Dim txt As String = valeurs(i - 1).ToString("N0") & " kg"      ' 25/09 : poids restant seul, sans %
            If lbls(i - 1).Text <> txt Then
                lbls(i - 1).Text = txt
                lbls(i - 1).ForeColor = If(pct < _seuilSiloPct(i), Color.Red, Color.DarkGreen)
            End If
            If i = 1 Then PictureBoxSS1.Invalidate() Else PictureBoxSS2.Invalidate()
        Next
    End Sub

    ' 22/09 : plus de Timer jetable (fuite : un timer par clic) — simple trace
    Private Sub AfficherInfoForcage(msg As String)
        AfficherForcage(msg)
    End Sub

    Public Sub MettreAJourVisibiliteBoutons()
        If Dosage_manuel.ModeForceGranulats Then AfficherBoutonsGroupe("GRANULATS") Else CacherBoutonsGroupe("GRANULATS")
        If Dosage_manuel.ModeForceCiments Then AfficherBoutonsGroupe("CIMENTS") Else CacherBoutonsGroupe("CIMENTS")
        If Dosage_manuel.ModeForceEaux Then AfficherBoutonsGroupe("EAUX") Else CacherBoutonsGroupe("EAUX")
        If Dosage_manuel.ModeForceAdjuvants Then AfficherBoutonsGroupe("ADJUVANTS") Else CacherBoutonsGroupe("ADJUVANTS")
        If Dosage_manuel.ModeForceMalaxeurs Then AfficherBoutonsGroupe("MALAXEURS") Else CacherBoutonsGroupe("MALAXEURS")
    End Sub

    Private Sub CacherBoutonsGroupe(groupe As String)
        Dim btn As Button
        For Each btn In GetBoutonsGroupe(groupe)
            btn.Visible = False
        Next
    End Sub

    Private Sub AfficherBoutonsGroupe(groupe As String)
        Dim btn As Button
        For Each btn In GetBoutonsGroupe(groupe)
            btn.Visible = True
        Next
    End Sub

    Private Function GetBoutonsGroupe(groupe As String) As List(Of Button)
        Dim liste As New List(Of Button)
        Select Case groupe.ToUpper()
            Case "GRANULATS" : liste.AddRange({Buttontr1, Buttontr2, Buttontr3, Buttontr4, Buttontpa, Buttontpm, Buttonskh, Buttonska, Buttonskb})
            Case "CIMENTS" : liste.AddRange({Buttonsl1, Buttonsl2, Buttonsltr})
            Case "EAUX" : liste.AddRange({Buttoneau1, Buttoneau2, Buttoneautr})
            Case "ADJUVANTS" : liste.AddRange({Buttonadj1, Buttonadj2, Buttonadj3, Buttonadj4, Buttonadjtr})
            Case "MALAXEURS" : liste.AddRange({Buttonmxm, Buttonmxa, Buttonmxov, Buttonmxfr})
        End Select
        Return liste
    End Function

    ' ═══════════════════════════════════════════════════════════════
    '  CONNEXION PLC
    ' ═══════════════════════════════════════════════════════════════
    Private Sub ConnecterPLC()
        Try
            ' Utiliser PLCInstance de Module1 au lieu de créer une nouvelle connexion
            If Module1.PLCInstance Is Nothing OrElse Not Module1.PLCInstance.IsConnected Then
                ConnecterPLCGlobal()   ' ← Module1 crée la connexion
            End If

            ' Synoptique utilise la même connexion
            _plc = Module1.PLCInstance
            _plcConnecte = (_plc IsNot Nothing AndAlso _plc.IsConnected)

            If _plcConnecte Then
                'lblEtatPLC.Text = "PLC : Connecté " & PLC_IP
                ' lblEtatPLC.ForeColor = Color.Green
                ' lblEtatPLC.BackColor = Color.FromArgb(220, 255, 220)
                EnregistrerAction("SYNOPTIQUE", "PLC connecté", "IP " & PLC_IP, "", "", "CONNEXION")
            Else
                'lblEtatPLC.Text = "PLC : Déconnecté"
                '  lblEtatPLC.ForeColor = Color.Red
                '  lblEtatPLC.BackColor = Color.FromArgb(255, 220, 220)
            End If

        Catch ex As Exception
            _plcConnecte = False

            Debug.WriteLine("Synoptique ConnecterPLC : " & ex.Message)
        End Try
    End Sub

    ' ═══════════════════════════════════════════════════════════════
    '  TIMER
    ' ═══════════════════════════════════════════════════════════════
    ' Timer1 1000ms — lecture PLC + mise a jour affichage
    Private _lectureEnCours As Boolean = False

    Private Sub Timer1_Tick(sender As Object, e As EventArgs) Handles Timer1.Tick
        If _lectureEnCours Then Exit Sub
        _lectureEnCours = True
        Try
            _plc = Module1.PLCInstance
            ' Images / boutons / labels : déjà rafraîchis par SurNouvellesValeursPLC (duplex)
            _tickCount += 1
            If _tickCount Mod 10 = 0 AndAlso _plcConnecte Then
                Threading.ThreadPool.QueueUserWorkItem(Sub(state) ArchiverTagsSQL())
            End If
        Finally
            _lectureEnCours = False
        End Try
    End Sub


    ' Timer2 3000 ms — 22/09 : le SQL est exécuté en arrière-plan (ThreadPool),
    ' seule la mise à jour des contrôles revient sur le thread UI (BeginInvoke).
    ' ChargerConfigElements (noms des trémies/silos) : toutes les 60 s seulement.
    Private _timer2Occupe As Boolean = False
    Private _timer2Compteur As Integer = 0
    Private Sub Timer2_Tick(sender As Object, e As EventArgs) Handles Timer2.Tick
        ' Revalider la connexion reelle a chaque tick (pas confiance au flag seul)
        _plcConnecte = (_plc IsNot Nothing AndAlso _plc.IsConnected)

        If Not _timer2Occupe Then
            _timer2Occupe = True
            _timer2Compteur += 1
            Dim chargerConfig As Boolean = (_timer2Compteur Mod 20 = 1)
            Dim lireTol As Boolean = (_timer2Compteur Mod 10 = 1)   ' 10/11 : tolérances CPU ~30 s
            Threading.ThreadPool.QueueUserWorkItem(
                Sub(state)
                    Try
                        ChargerEtatsBDD()
                        If chargerConfig Then ChargerConfigElements()
                        If lireTol Then LireTolerancesCPU()
                        Dim dtJ As DataTable = LireJournalActions()
                        If dtJ IsNot Nothing AndAlso Me.IsHandleCreated AndAlso Not Me.IsDisposed Then
                            Me.BeginInvoke(Sub() AfficherJournalActions(dtJ))
                        End If
                    Catch ex As Exception
                        Debug.WriteLine("Timer2 fond : " & ex.Message)
                    Finally
                        _timer2Occupe = False
                    End Try
                End Sub)
        End If

        If _plcConnecte Then
            ModuleArchivageGachees.Instance.SurveillanceCycleUI(_plc)
        End If
        LireNiveauxSilo()                     ' 26/09 : stock logiciel des silos (SQL, arrière-plan, même sans CPU)
    End Sub

    ' ═══════════════════════════════════════════════════════════════
    '  MISE A JOUR LABELS
    ' ═══════════════════════════════════════════════════════════════
    Private Sub MettreAJourLabels()
        ' 22/09 : consignes réelles de la formule (plus de valeurs en dur 900/131/74/10)
        ' 10/11 : tolérance % CPU (AGR = tolérance G1, ADJ = tolérance ADJ1)
        Dim consAGR As Single = _cons_G1 + _cons_SC + _cons_SL + _cons_D2
        Dim consADJ As Single = _cons_ADJ1 + _cons_ADJ2 + _cons_ADJ3 + _cons_ADJ4
        AfficherPoids(lblPoidsAGR, _poids_AGR, "kg", consAGR, TolAbs(0, consAGR, TOL_KG))
        AfficherPoids(lblPoidsCIM, _poids_CIM, "kg", _cons_CIM, TolAbs(4, _cons_CIM, TOL_KG))
        AfficherPoids(lblPoidsEAU, _poids_EAU, "L", _cons_EAU2, TolAbs(5, _cons_EAU2, TOL_L))
        AfficherPoids(lblPoidsADJ, _poids_ADJ, "kg", consADJ, TolAbs(6, consADJ, TOL_ADJ))

        Label1.Text = _cons_G1.ToString("0.0")   ' DB8 DBD0
        Label2.Text = _cons_SC.ToString("0.0")   ' DB8 DBD4
        Label3.Text = _cons_SL.ToString("0.0")   ' DB8 DBD8
        Label4.Text = _cons_D2.ToString("0.0")   ' DB8 DBD12

        lblMalaxeur.Text = If(_malaxeurON, "MALAXEUR : MARCHE", "MALAXEUR : ARRET")
        lblMalaxeur.ForeColor = If(_malaxeurON, Color.Green, Color.Gray)
        lblCycle.Text = If(_cycleEnCours, "CYCLE EN COURS", "FIN CYCLE")
        lblCycle.ForeColor = If(_cycleEnCours, Color.Blue, Color.Gray)
        MettreAJourCompteursGachees()   ' 22/09 : DB1.DBD1220…1232 (instantané)
        SuivreCorrectionEauCPU()        ' 22/09 : curseur + case suivent la valeur CPU (DB1.DBW1194)
        If _compteurDuplex Mod 3 = 0 Then MettreAJourDefautsCPU()   ' 22/09 défauts CPU ; 25/09 : 1 cycle sur 3
        ' ══ Voyants capteurs densité ══
        AppliquerVoyant(Picture1, _bitDens1)
        AppliquerVoyant(Picture2, _bitDens2)
        AppliquerVoyant(Picture3, _bitDens3)
        AppliquerVoyant(Picture4, _bitDens4)
        AppliquerVoyant(Picture5, _bitDens5)
        AppliquerVoyant(Picture6, _bitDens6)
        AppliquerVoyant(Picture7, _bitDens7)
        AppliquerVoyant(Picture8, _bitDens8)
        AppliquerVoyant(Picture9, _bitDens9)
        AppliquerVoyant(Picture10, _bitDens10)
        AppliquerVoyant(Picture11, _bitDens11)
        AppliquerVoyant(Picture12, _bitDens12)
        ' Afficher etat BL en cours et volume livre
        Dim arch = ModuleArchivageGachees.Instance
        Dim infoLivre As String = ""
        If arch.NumeroBL <> "" Then
            ' 10/11 : gâchées faites / nombre demandé (CPU)
            Dim nbDemande As Integer = CInt(Math.Round(PLCData.NbGacheeCalc))
            Dim txtGach As String = arch.NombreGachees.ToString() &
                If(nbDemande > 0, "/" & nbDemande.ToString(), "")
            infoLivre = "BL: " & arch.NumeroBL & " | Gachees: " & txtGach &
                " | Vol livre: " & arch.VolumeLivre.ToString("0.00") & " m3"
            If Not _plcConnecte Then
                infoLivre &= "   ⚠ EN ATTENTE DE COMMUNICATION PLC"
            End If
        End If
        lblInfoLivraison.Text = infoLivre
        lblInfoLivraison.ForeColor = If(_plcConnecte, Color.Black, Color.OrangeRed)

        If _arretUrg Then
            lblAlarme.Text = "ARRÊT D'URGENCE !"
            lblAlarme.ForeColor = Color.White
            lblAlarme.BackColor = Color.Red
        Else
            lblAlarme.Text = ""
            lblAlarme.BackColor = Color.Transparent
        End If
    End Sub
    ' ═══════════════════════════════════════════════════════════════
    '  VOYANT CAPTEUR DENSITÉ — VERT si bit=1, ROUGE si bit=0
    '  Utilise image disque si disponible, sinon couleur BackColor
    ' ═══════════════════════════════════════════════════════════════
    Private Sub AppliquerVoyant(pb As PictureBox, bitActif As Boolean)
        ' Éviter redessinage inutile
        Dim cleTag As String = If(bitActif, "vert", "rouge")
        If pb.Tag IsNot Nothing AndAlso pb.Tag.ToString() = cleTag Then Exit Sub
        pb.Tag = cleTag

        ' Essayer image disque : voyant_vert.png / voyant_rouge.png
        Dim fichier As String = DossierImages & If(bitActif, "voyant_vert.png", "voyant_rouge.png")
        Dim img As Image = ChargerImageCachee(fichier)

        If img IsNot Nothing Then
            pb.Image = img
            pb.SizeMode = PictureBoxSizeMode.StretchImage
            pb.BackColor = Color.Transparent
        Else
            ' Pas d'image → fond coloré pur
            pb.Image = Nothing
            pb.BackColor = If(bitActif,
            Color.FromArgb(80, 200, 80),    ' VERT
            Color.FromArgb(220, 50, 50))    ' ROUGE
        End If
    End Sub
    Private Sub AfficherPoids(lbl As Label, valeur As Single, unite As String, consigne As Single,
                              Optional tol As Single = 2.0F)
        lbl.Text = valeur.ToString("0.0") & " " & unite
        Dim ecart As Single = consigne - valeur
        If valeur <= 0 Then
            lbl.ForeColor = Color.Gray
            lbl.BackColor = Color.Transparent
        ElseIf Math.Abs(ecart) <= tol Then
            lbl.ForeColor = Color.Green
            lbl.BackColor = Color.FromArgb(220, 255, 220)
        ElseIf valeur < consigne Then
            lbl.ForeColor = Color.Blue
            lbl.BackColor = Color.FromArgb(220, 230, 255)
        Else
            lbl.ForeColor = Color.Red
            lbl.BackColor = Color.FromArgb(255, 220, 220)
        End If
        Dim tip As String = "Consigne : " & consigne.ToString("0") & " " & unite &
                    " | Ecart : " & ecart.ToString("+0.0;-0.0") & " " & unite
        If Not String.Equals(TryCast(lbl.Tag, String), tip) Then   ' 22/09 : seulement si changement
            lbl.Tag = tip
            ToolTip1.SetToolTip(lbl, tip)
        End If
    End Sub

    ' ═══════════════════════════════════════════════════════════════
    '  ARCHIVAGE SQL
    ' ═══════════════════════════════════════════════════════════════
    Private Sub ArchiverTagsSQL()
        Try
            Dim tags() As (String, Single) = {
                ("OUT SCAL AGR", _poids_AGR),
                ("OUT SCAL CIM", _poids_CIM),
                ("OUT SCAL EAU", _poids_EAU),
                ("BASCULE ADJ", _poids_ADJ)}
            Dim item As (String, Single)
            For Each item In tags
                ModuleDb.Executer(
                    "UPDATE dbo.T_PLC_TAGS SET TAG_VALEUR_REAL=@v,TAG_DATEUPDATE=GETDATE() WHERE TAG_NOM=@n AND TAG_ACTIF=1",
                    Sub(cmd)
                        cmd.Parameters.AddWithValue("@v", item.Item2)
                        cmd.Parameters.AddWithValue("@n", item.Item1)
                    End Sub)
            Next
            ModuleDb.Executer(
                "UPDATE dbo.T_PLC_TAGS SET TAG_VALEUR_BOOL=@v,TAG_DATEUPDATE=GETDATE() WHERE TAG_NOM=@n AND TAG_ACTIF=1",
                Sub(cmd)
                    cmd.Parameters.AddWithValue("@v", _malaxeurON)
                    cmd.Parameters.AddWithValue("@n", "def malaxeur")
                End Sub)
            ModuleDb.Executer(
                "UPDATE dbo.Synoptique_Etats SET SYN_Etat=@v, SYN_DateMAJ=GETDATE() WHERE SYN_Code='N_GACHEE'",
                Sub(cmd) cmd.Parameters.AddWithValue("@v", _nGachee))
        Catch
        End Try
    End Sub

    Public Sub MettreAJourBascules(valAGR As Single, etatAGR As Integer,
                                   valCIM As Single, etatCIM As Integer,
                                   valEAU As Single, etatEAU As Integer)

    End Sub

    ' ═══════════════════════════════════════════════════════════════
    '  CHARGER ETATS BDD
    ' ═══════════════════════════════════════════════════════════════
    Private Sub ChargerEtatsBDD()
        Try
            Dim dt As DataTable = ModuleDb.Lire(
                "SELECT SYN_Code, SYN_Etat FROM dbo.Synoptique_Etats")
            If dt Is Nothing Then Exit Sub
            Dim row As DataRow
            For Each row In dt.Rows
                Dim code As String = row("SYN_Code").ToString().ToUpper()
                If IsDBNull(row("SYN_Etat")) Then Continue For   ' 22/09 : NULL → ligne ignorée (avant : sortie de boucle)
                Dim etat As Integer = Convert.ToInt32(row("SYN_Etat"))
                Select Case code

                    ' ══ Éléments pilotés par le CPU ══════════════════
                    '  BDD utilisée SEULEMENT si PLC déconnecté (secours)
                    Case "TREMIE_1" : If Not _plcConnecte Then _etatTremie(1) = etat
                    Case "TREMIE_2" : If Not _plcConnecte Then _etatTremie(2) = etat
                    Case "TREMIE_3" : If Not _plcConnecte Then _etatTremie(3) = etat
                    Case "TREMIE_4" : If Not _plcConnecte Then _etatTremie(4) = etat
                    Case "TAPIS" : If Not _plcConnecte Then _etatTapis = etat
                    Case "SKIP" : If Not _plcConnecte Then _etatSkip = etat
                    Case "MALAXEUR" : If Not _plcConnecte Then _etatMalaxeur = etat
                    Case "SILO_1" : If Not _plcConnecte Then _etatSilo(1) = etat
                    Case "SILO_2" : If Not _plcConnecte Then _etatSilo(2) = etat
                    Case "BASC_CIM" : If Not _plcConnecte Then _etatBasculeCim = etat
                    Case "BASC_EAU" : If Not _plcConnecte Then _etatBasculeEau = etat
                    Case "BASC_ADJ" : If Not _plcConnecte Then _etatBasculeAdj = etat
                    Case "ADJ_1" : If Not _plcConnecte Then _etatAdj(1) = etat
                    Case "ADJ_2" : If Not _plcConnecte Then _etatAdj(2) = etat
                    Case "ADJ_3" : If Not _plcConnecte Then _etatAdj(3) = etat
                    Case "ADJ_4" : If Not _plcConnecte Then _etatAdj(4) = etat

                    ' ══ Éléments SANS pilotage CPU ═══════════════════
                    '  BDD = seule source, toujours appliquée
                    Case "TREMIE_5" : _etatTremie(5) = etat
                    Case "TREMIE_6" : _etatTremie(6) = etat
                    Case "TREMIE_7" : _etatTremie(7) = etat
                    Case "TREMIE_8" : _etatTremie(8) = etat
                    Case "SILO_3" : _etatSilo(3) = etat
                    Case "SILO_4" : _etatSilo(4) = etat
                    Case "POMPE_1" : _etatPompe(1) = etat
                    Case "POMPE_2" : _etatPompe(2) = etat
                End Select
            Next
        Catch ex As Exception
            Debug.WriteLine("Erreur BDD Synoptique : " & ex.Message)
        End Try
    End Sub

    ' ═══════════════════════════════════════════════════════════════
    '  MENUS CLIC DROIT
    ' ═══════════════════════════════════════════════════════════════
    Private Sub InitMenusClicDroit()
        ' 25/09 : clic droit = nom + [Stock] + Paramètres (petite fenêtre Frm_ParamEquipement)
        CreerMenuEquipement(PictureBox1, "TREMIE", 1, True)
        CreerMenuEquipement(PictureBox2, "TREMIE", 2, True)
        CreerMenuEquipement(PictureBox3, "TREMIE", 3, True)
        CreerMenuEquipement(PictureBox4, "TREMIE", 4, True)
        CreerMenuEquipement(PictureBox12, "SILO", 1, True)
        CreerMenuEquipement(PictureBox13, "SILO", 2, True)
        CreerMenuEquipement(PictureBox9, "TAPIS", 0, False)
        CreerMenuEquipement(PictureBox10, "SKIP", 0, False)
        CreerMenuEquipement(PictureBox11, "MALAXEUR", 0, False)
        CreerMenuEquipement(PictureBox16, "BASC_CIM", 0, False)
        CreerMenuEquipement(PictureBox19, "BASC_EAU", 0, False)
        CreerMenuEquipement(PictureBox24, "BASC_ADJ", 0, False)
        CreerMenuEquipement(PictureBox20, "ADJ", 1, False)
        CreerMenuEquipement(PictureBox21, "ADJ", 2, False)
        CreerMenuEquipement(PictureBox22, "ADJ", 3, False)
        CreerMenuEquipement(PictureBox23, "ADJ", 4, False)
    End Sub

    ' Titre affiché dans le menu / la fenêtre Paramètres
    Private Shared Function TitreEquipement(typeEquip As String, num As Integer) As String
        Select Case typeEquip
            Case "TREMIE" : Return "TR" & num
            Case "SILO" : Return "Silo CIM " & num
            Case "TAPIS" : Return "Tapis peseur"
            Case "SKIP" : Return "Skip"
            Case "MALAXEUR" : Return "Malaxeur"
            Case "BASC_CIM" : Return "Bascule ciment"
            Case "BASC_EAU" : Return "Bascule eau"
            Case "BASC_ADJ" : Return "Bascule adjuvants"
            Case "ADJ" : Return "Adjuvant " & num
            Case Else : Return typeEquip
        End Select
    End Function

    ' Paramètres CPU de chaque équipement — mêmes adresses que Configuration centrale
    '   (REAL = kg / % ; TIME = DINT ms affiché en secondes)
    Private Shared Function ParametresEquipement(typeEquip As String, num As Integer) As List(Of Frm_ParamEquipement.ParamDef)
        Dim l As New List(Of Frm_ParamEquipement.ParamDef)
        Select Case typeEquip
            Case "TREMIE"
                Dim oPetit() As Integer = {72, 76, 80, 84}, oTol() As Integer = {24, 28, 32, 36}
                Dim oOn() As Integer = {138, 142, 146, 150}, oOff() As Integer = {162, 166, 170, 174}
                l.Add(New Frm_ParamEquipement.ParamDef("Petite début", 8, oPetit(num - 1), False, "kg"))
                l.Add(New Frm_ParamEquipement.ParamDef("Tolérance", 8, oTol(num - 1), False, "%"))
                l.Add(New Frm_ParamEquipement.ParamDef("Temps On", 8, oOn(num - 1), True, "s"))
                l.Add(New Frm_ParamEquipement.ParamDef("Temps Off", 8, oOff(num - 1), True, "s"))
            Case "SILO"
                l.Add(New Frm_ParamEquipement.ParamDef("Petite début", 8, 88, False, "kg"))
                l.Add(New Frm_ParamEquipement.ParamDef("Tolérance", 8, 40, False, "%"))
                l.Add(New Frm_ParamEquipement.ParamDef("Temps On", 8, If(num = 1, 154, 158), True, "s"))
                l.Add(New Frm_ParamEquipement.ParamDef("Temps Off", 8, If(num = 1, 178, 182), True, "s"))
            Case "TAPIS"
                l.Add(New Frm_ParamEquipement.ParamDef("Zéro bascule AGR", 1, 1052, False, "kg"))
            Case "SKIP"
                l.Add(New Frm_ParamEquipement.ParamDef("Temps vidange skip", 8, 264, True, "s"))
                l.Add(New Frm_ParamEquipement.ParamDef("Position attente skip", 19, 106, True, "s"))
            Case "MALAXEUR"
                l.Add(New Frm_ParamEquipement.ParamDef("Temps malaxage IMD", 8, 256, True, "s"))
                l.Add(New Frm_ParamEquipement.ParamDef("Temps vidange malaxeur", 8, 260, True, "s"))
            Case "BASC_CIM"
                l.Add(New Frm_ParamEquipement.ParamDef("Zéro bascule CIM", 1, 1056, False, "kg"))
                l.Add(New Frm_ParamEquipement.ParamDef("Temps retard vidange CIM", 8, 248, True, "s"))
            Case "BASC_EAU"
                l.Add(New Frm_ParamEquipement.ParamDef("Zéro bascule EAU", 1, 1060, False, "kg"))
                l.Add(New Frm_ParamEquipement.ParamDef("Temps retard vidange EAU", 8, 252, True, "s"))
                l.Add(New Frm_ParamEquipement.ParamDef("Tolérance eau", 8, 44, False, "%"))
            Case "BASC_ADJ"
                l.Add(New Frm_ParamEquipement.ParamDef("Zéro bascule ADJ", 1, 1064, False, "kg"))
                l.Add(New Frm_ParamEquipement.ParamDef("Temps vidange bascule ADJ", 90, 28, True, "s"))
            Case "ADJ"
                l.Add(New Frm_ParamEquipement.ParamDef("Tolérance adj " & num, 8, 330 + 4 * num, False, "%"))   ' 334 / 338 / 342 / 346
        End Select
        Return l
    End Function

    Private Sub CreerMenuEquipement(pb As PictureBox, typeEquip As String, num As Integer, avecStock As Boolean)
        Dim menu As New ContextMenuStrip()
        Dim itNom As New ToolStripMenuItem("...") With {.Font = New Font(menu.Font, FontStyle.Bold)}
        Dim codeMat As String = ""
        Dim siloId As Integer = 0
        Dim titre As String = TitreEquipement(typeEquip, num)
        menu.Items.Add(itNom)
        menu.Items.Add(New ToolStripSeparator())
        Dim itStock As ToolStripMenuItem = Nothing
        If avecStock Then
            itStock = New ToolStripMenuItem("Stock")
            menu.Items.Add(itStock)
            AddHandler itStock.Click,
                Sub(s, ev)
                    ' 26/09 : fiche stock du matériau (Entrée stock / Actualiser), sans la table
                    Try
                        If typeEquip = "SILO" AndAlso siloId > 0 Then
                            ' 26/09 : fiche du SILO (stock par silo)
                            Using f As New Frm_StockElement(siloId, codeMat, titre)
                                f.ShowDialog(Me)
                            End Using
                            Exit Sub
                        End If
                        If codeMat = "" Then
                            MessageBox.Show("Aucun matériau affecté à cet équipement (Configuration silos).", "Stock",
                                            MessageBoxButtons.OK, MessageBoxIcon.Information)
                            Exit Sub
                        End If
                        Using f As New Frm_StockElement(codeMat, titre)
                            f.ShowDialog(Me)
                        End Using
                    Catch ex As Exception
                        AfficherForcage("Stock : " & ex.Message)
                    End Try
                End Sub
        End If
        menu.Items.Add("Paramètres", Nothing,
            Sub(s, ev)
                Try
                    Using f As New Frm_ParamEquipement(titre, ParametresEquipement(typeEquip, num))
                        f.ShowDialog(Me)
                    End Using
                Catch ex As Exception
                    AfficherForcage("Paramètres : " & ex.Message)
                End Try
            End Sub)
        AddHandler menu.Opening,
            Sub(s, ev)
                titre = TitreEquipement(typeEquip, num)
                codeMat = "" : siloId = 0
                If avecStock Then
                    Try
                        ' 26/09 : dbo.Silos est la table unique de configuration (TypeElement TR/S, NumeroElement)
                        Dim dt As DataTable = ModuleDb.Lire(
                            "SELECT TOP 1 s.SiloID, s.NomSilo, m.CodeMateriau, m.NomMateriau, st.STO_Stock, st.STO_Unite, ISNULL(s.StockKg,0) AS StockKg " &
                            "FROM dbo.Silos s " &
                            "LEFT JOIN dbo.Materiaux m ON m.IdMateriau = s.IdMateriau " &
                            "LEFT JOIN dbo.T_STOCK st ON st.MAT_Code = LEFT(m.CodeMateriau,15) " &
                            "WHERE s.EstActif = 1 AND s.TypeElement = @type AND s.NumeroElement = @num",
                            Sub(cmd)
                                cmd.Parameters.Add("@num", SqlDbType.Int).Value = num
                                cmd.Parameters.Add("@type", SqlDbType.NVarChar, 10).Value = If(typeEquip = "SILO", "S", "TR")
                            End Sub)
                        If dt.Rows.Count > 0 Then
                            Dim r As DataRow = dt.Rows(0)
                            codeMat = If(IsDBNull(r("CodeMateriau")), "", r("CodeMateriau").ToString())
                            siloId = CInt(r("SiloID"))
                            Dim nom As String = If(IsDBNull(r("NomMateriau")), If(IsDBNull(r("NomSilo")), "", r("NomSilo").ToString()), r("NomMateriau").ToString())
                            titre &= " : " & codeMat & " - " & nom
                            ' 26/09 : valeur du stock affichée dans le menu (clic → fiche stock)
                            If itStock IsNot Nothing Then
                                If typeEquip = "SILO" Then
                                    itStock.Text = "Stock silo : " & CDbl(r("StockKg")).ToString("N0") & " kg"
                                ElseIf IsDBNull(r("STO_Stock")) Then
                                    itStock.Text = "Stock : non trouvé (code " & codeMat & ")"
                                Else
                                    itStock.Text = "Stock : " & CDec(r("STO_Stock")).ToString("N2") & " " & If(IsDBNull(r("STO_Unite")), "", r("STO_Unite").ToString())
                                End If
                            End If
                        End If
                    Catch
                    End Try
                End If
                itNom.Text = titre
            End Sub
        pb.ContextMenuStrip = menu
    End Sub

    Private Sub ChangerEtat(code As String, etat As Integer, pb As PictureBox, typeEquip As String)
        Try
            ModuleDb.Executer(
                "UPDATE dbo.Synoptique_Etats SET SYN_Etat=@etat, SYN_DateMAJ=GETDATE() WHERE SYN_Code=@code",
                Sub(cmd)
                    cmd.Parameters.Add("@etat", SqlDbType.Int).Value = etat
                    cmd.Parameters.Add("@code", SqlDbType.NVarChar, 50).Value = code
                End Sub)
            AppliquerImage(pb, typeEquip, etat)
        Catch ex As Exception
            MessageBox.Show("Erreur : " & ex.Message)
        End Try
    End Sub

    Private Sub VoirStock(code As String)
        MessageBox.Show("Stock de " & code, "Stock", MessageBoxButtons.OK, MessageBoxIcon.Information)
    End Sub

    Private Sub VoirNiveauSilo(num As Integer)
        Dim niveau = ModuleDb.LireValeur(Of Single)(
            "SELECT TOP 1 TAG_VALEUR_REAL FROM dbo.T_PLC_TAGS WHERE TAG_NOM LIKE '%SILO%" & num & "%'")
        MessageBox.Show("Niveau Silo " & num & " : " & If(niveau > 0, niveau.ToString("0.0") & " %", "N/A"),
                        "Niveau Silo", MessageBoxButtons.OK, MessageBoxIcon.Information)
    End Sub

    Private Sub DemarrerRemplissage(num As Integer)
        If MessageBox.Show("Demarrer remplissage Silo " & num & " ?", "Confirmation",
                           MessageBoxButtons.YesNo, MessageBoxIcon.Question) = DialogResult.Yes Then
            MessageBox.Show("Ordre remplissage envoyé.")
        End If
    End Sub

    ' ═══════════════════════════════════════════════════════════════
    '  SYNOPTIQUE PICTUREBOX
    ' ═══════════════════════════════════════════════════════════════
    Private Sub MettreAJourSynoptique()
        AppliquerImage(PictureBox1, "tremie", _etatTremie(1))
        AppliquerImage(PictureBox2, "tremie", _etatTremie(2))
        AppliquerImage(PictureBox3, "tremie", _etatTremie(3))
        AppliquerImage(PictureBox4, "tremie", _etatTremie(4))
        AppliquerImage(PictureBox9, "tapis", _etatTapis)
        AppliquerImage(PictureBox10, "skip", _etatSkip)
        AppliquerImage(PictureBox11, "malaxeur", _etatMalaxeur)
        AppliquerImage(PictureBox12, "silo", _etatSilo(1))
        AppliquerImage(PictureBox13, "silo", _etatSilo(2))
        AppliquerImage(PictureBox16, "bascule", _etatBasculeCim)
        AppliquerImage(PictureBox17, "pompe", _etatPompe(1))
        AppliquerImage(PictureBox18, "pompe", _etatPompe(2))
        AppliquerImage(PictureBox19, "bascule", _etatBasculeEau)
        AppliquerImage(PictureBox20, "adjuvant", _etatAdj(1))
        AppliquerImage(PictureBox21, "adjuvant", _etatAdj(2))
        AppliquerImage(PictureBox22, "adjuvant", _etatAdj(3))
        AppliquerImage(PictureBox23, "adjuvant", _etatAdj(4))
        AppliquerImage(PictureBox24, "bascule", _etatBasculeAdj)

        ' ── Fleche clignotante selon bit PLC ─────────────────────
        AppliquerFleche(PictureBoxtr1, _bitTr1)
        AppliquerFleche(PictureBoxtr2, _bitTr2)
        AppliquerFleche(PictureBoxtr3, _bitTr3)
        AppliquerFleche(PictureBoxtr4, _bitTr4)
        AppliquerFlecheCouleur(PictureBoxmx, _bitMxVert, _bitMxRouge)   ' 19/09 : verte 30.1 / rouge 914.6, clignotantes
        ' 24/09 : skip — flèche verte clignotante : monte (DB1.DBX17.4) / descend (DB1.DBX17.5)
        AppliquerFlecheCouleur(PictureBoxskph, PLCData.SkipMonte, False)
        AppliquerFlecheCouleur(PictureBoxskpb, PLCData.SkipDesc, False)
        AppliquerFleche(PictureBoxs1, _bitS1)
        AppliquerFleche(PictureBoxs2, _bitS2)
        AppliquerFleche(PictureBoxsl, _bitSl)
        AppliquerFleche(PictureBoxea1, _bitEa1)
        AppliquerFleche(PictureBoxeau, _bitEau)
        AppliquerFleche(PictureBoxadj1, _bitAdj1)
        AppliquerFleche(PictureBoxadj2, _bitAdj2)
        AppliquerFleche(PictureBoxadj3, _bitAdj3)
        AppliquerFleche(PictureBoxadj4, _bitAdj4)
        AppliquerFleche(PictureBoxeaadjj, _bitEaadjj)


    End Sub

    ' Tag = dernier etat affiche pour eviter rechargement inutile
    Private Sub AppliquerImage(pb As PictureBox, typeEquip As String, etat As Integer)
        ' Ne recharger que si l etat a change
        Dim cleEtat As String = typeEquip & "_" & etat.ToString()
        If pb.Tag IsNot Nothing AndAlso pb.Tag.ToString() = cleEtat Then Exit Sub
        pb.Tag = cleEtat

        Dim chemin As String = GetImagePath(typeEquip, etat)
        pb.SizeMode = PictureBoxSizeMode.StretchImage

        ' Essayer .png d abord puis .gif si pas trouve
        Dim img As Image = ChargerImageCachee(chemin)
        If img Is Nothing Then
            Dim cheminGif As String = chemin.Replace(".png", ".gif")
            img = ChargerImageCachee(cheminGif)
        End If

        If img IsNot Nothing Then
            pb.Image = img
            pb.BackColor = Color.Transparent
        Else
            pb.Image = Nothing
            pb.BackColor = CouleurSecours(etat)
            Debug.WriteLine("Image non trouvee : " & chemin)
        End If
        ToolTip1.SetToolTip(pb, typeEquip.ToUpper & " - " & NomEtat(typeEquip, etat))
    End Sub

    Private Function GetImagePath(typeEquip As String, etat As Integer) As String
        Dim fichier As String = ""
        Dim lst() As String
        Select Case typeEquip.ToLower()
            Case "tremie"
                lst = New String() {"tremie_arret", "tremie_marche", "tremie_alarme", "tremie_defaut"}
                fichier = lst(Math.Min(etat, 3)) & ".png"
            Case "tapis"
                lst = New String() {"tapis_arret", "tapis_arret_sable",
                        "tapis_marche_sable", "tapis_marche_vide"}
                fichier = lst(Math.Min(etat, 3)) & ".gif"
            Case "skip"
                lst = New String() {"skip_vide_bas", "skip_vide_attente", "skip_vide_haut",
                        "skip_plein_bas", "skip_plein_attente", "skip_plein_haut",
                        "skip_monte", "skip_descend"}
                fichier = lst(Math.Min(etat, 7)) & ".gif"
            Case "malaxeur"
                lst = New String() {"malaxeur_arret", "malaxeur_marche", "malaxeur_vide"}
                fichier = lst(Math.Min(etat, 2)) & ".gif"
            Case "silo"
                lst = New String() {"silo_arret", "silo_marche", "silo_alarme", "silo_defaut"}
                fichier = lst(Math.Min(etat, 3)) & ".png"
            Case "bascule"
                lst = New String() {"bascule_arret", "bascule_marche", "bascule_alarme", "bascule_defaut"}
                fichier = lst(Math.Min(etat, 3)) & ".png"
            Case "pompe"
                lst = New String() {"pompe_arret", "pompe_marche", "pompe_alarme", "pompe_defaut"}
                fichier = lst(Math.Min(etat, 3)) & ".png"
            Case "adjuvant"
                lst = New String() {"adjuvant_arret", "adjuvant_marche", "adjuvant_alarme", "adjuvant_defaut"}
                fichier = lst(Math.Min(etat, 3)) & ".png"
        End Select
        Return DossierImages & fichier
    End Function

    Private Function CouleurSecours(etat As Integer) As Color
        Select Case etat
            Case 1 : Return Color.FromArgb(180, 255, 180)
            Case 2 : Return Color.FromArgb(255, 255, 140)
            Case 3 : Return Color.FromArgb(255, 160, 160)
            Case Else : Return Color.FromArgb(210, 210, 210)
        End Select
    End Function

    Private Function NomEtat(typeEquip As String, etat As Integer) As String
        Select Case typeEquip.ToLower()
            Case "tapis"
                Dim lst() As String = {"Arret", "Arret sable", "Marche sable", "Marche vide"}
                Return lst(Math.Min(etat, 3))
            Case "skip"
                Dim lst() As String = {"Vide bas", "Vide attente", "Vide haut",
                                       "Plein bas", "Plein attente", "Plein haut", "Monte", "Descend"}
                Return lst(Math.Min(etat, 7))
            Case "malaxeur"
                Dim lst() As String = {"Arret", "Marche", "Vide"}
                Return lst(Math.Min(etat, 2))
            Case Else
                Dim lst() As String = {"Arret", "Marche", "Alarme", "Defaut"}
                Return lst(Math.Min(etat, 3))
        End Select
    End Function

    Public Sub AppliquerEtatsTest(etats As Dictionary(Of String, Integer))
        If etats.ContainsKey("TREMIE_1") Then _etatTremie(1) = etats("TREMIE_1")
        If etats.ContainsKey("TREMIE_2") Then _etatTremie(2) = etats("TREMIE_2")
        If etats.ContainsKey("TREMIE_3") Then _etatTremie(3) = etats("TREMIE_3")
        If etats.ContainsKey("TREMIE_4") Then _etatTremie(4) = etats("TREMIE_4")
        If etats.ContainsKey("TAPIS") Then _etatTapis = etats("TAPIS")
        If etats.ContainsKey("SKIP") Then _etatSkip = etats("SKIP")
        If etats.ContainsKey("MALAXEUR") Then _etatMalaxeur = etats("MALAXEUR")
        If etats.ContainsKey("SILO_1") Then _etatSilo(1) = etats("SILO_1")
        If etats.ContainsKey("SILO_2") Then _etatSilo(2) = etats("SILO_2")
        If etats.ContainsKey("BASC_CIM") Then _etatBasculeCim = etats("BASC_CIM")
        If etats.ContainsKey("POMPE_1") Then _etatPompe(1) = etats("POMPE_1")
        If etats.ContainsKey("POMPE_2") Then _etatPompe(2) = etats("POMPE_2")
        If etats.ContainsKey("BASC_EAU") Then _etatBasculeEau = etats("BASC_EAU")
        If etats.ContainsKey("ADJ_1") Then _etatAdj(1) = etats("ADJ_1")
        If etats.ContainsKey("ADJ_2") Then _etatAdj(2) = etats("ADJ_2")
        If etats.ContainsKey("ADJ_3") Then _etatAdj(3) = etats("ADJ_3")
        If etats.ContainsKey("ADJ_4") Then _etatAdj(4) = etats("ADJ_4")
        If etats.ContainsKey("BASC_ADJ") Then _etatBasculeAdj = etats("BASC_ADJ")
        MettreAJourSynoptique()
    End Sub

    ' ═══════════════════════════════════════════════════════════════
    '  BOUTONS
    ' ═══════════════════════════════════════════════════════════════
    Private Sub Form_Synoptique_FormClosing(sender As Object, e As FormClosingEventArgs) Handles MyBase.FormClosing
        If Instance Is Me Then Instance = Nothing
        Timer1.Stop()
        Timer2.Stop()
        RemoveHandler ModulePLCDuplex.NouvellesValeurs, AddressOf SurNouvellesValeursPLC
        ' Arreter thread journal et vider la file avant fermeture
        _stopJournal = True
        If _threadJournal IsNot Nothing AndAlso _threadJournal.IsAlive Then
            _threadJournal.Join(1000)
        End If
        ' NE PAS fermer _plc — c'est la connexion partagée Module1.PLCInstance
    End Sub

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        For Each frm As Form In Application.OpenForms
            If TypeOf frm Is Dosage_manuel Then frm.Close() : Exit For
        Next
        Dim f As New Dosage_manuel
        Dosage_manuel.RefSynoptique = Me
        f.Owner = Me
        f.TopMost = True
        f.Show()
    End Sub

    ' ═══════════════════════════════════════════════════════════════
    '  BOUTONS FORCAGE
    ' ═══════════════════════════════════════════════════════════════
    Private Sub InitBoutonsForcage()
        AjouterPoussoir(Buttontr1, "DB1.DBX0.5", "GRANULATS", "Tremie 1", stockSource:="TR1")
        AjouterPoussoir(Buttontr2, "DB1.DBX0.6", "GRANULATS", "Tremie 2", stockSource:="TR2")
        AjouterPoussoir(Buttontr3, "DB1.DBX0.7", "GRANULATS", "Tremie 3", stockSource:="TR3")
        AjouterPoussoir(Buttontr4, "DB1.DBX1.0", "GRANULATS", "Tremie 4", stockSource:="TR4")   ' 19/09 : ex DB1.DBX3.5
        StyleBoutonMarche(Buttontpa) : StyleBoutonArret(Buttontpm)
        AddHandler Buttontpa.Click, Sub(s, e)
                                        If Not Dosage_manuel.ModeForceGranulats Then MessageBox.Show("Activez FORÇAGE GRANULATS !") : Exit Sub
                                        ModulePLCDuplex.EcrireBit("DB1.DBX3.5", CBool(1)) : MAJBoutonActif(Buttontpa, True) : MAJBoutonActif(Buttontpm, False)
                                        ModuleStockManuel.DebutVidange("AGR")   ' 26/09 : stock manuel — poids AGR avant vidange
                                        AfficherForcage("Tapis MARCHE") : EnregistrerAction("FORÇAGE", "Tapis marche", DetailBascules("GRANULATS"), "DB1.DBX3.5", "1", "FORCAGE")
                                    End Sub
        AddHandler Buttontpm.Click, Sub(s, e)
                                        If Not Dosage_manuel.ModeForceGranulats Then MessageBox.Show("Activez FORÇAGE GRANULATS !") : Exit Sub
                                        ModulePLCDuplex.EcrireBit("DB1.DBX3.5", CBool(0)) : MAJBoutonActif(Buttontpm, True) : MAJBoutonActif(Buttontpa, False)
                                        BloquerGroupeStabilisation("GRANULATS", Sub() ModuleStockManuel.FinVidange("AGR"))   ' 26/09
                                        AfficherForcage("Tapis ARRET") : EnregistrerAction("FORÇAGE", "Tapis arrêt", DetailBascules("GRANULATS"), "DB1.DBX3.5", "0", "FORCAGE")
                                    End Sub
        StyleBoutonMarche(Buttonskh) : StyleBoutonMarche(Buttonskb) : StyleBoutonArret(Buttonska)
        AddHandler Buttonskh.Click, Sub(s, e)
                                        If Not Dosage_manuel.ModeForceGranulats Then MessageBox.Show("Activez FORÇAGE GRANULATS !") : Exit Sub
                                        ModulePLCDuplex.EcrireBit("DB1.DBX4.5", CBool(1)) : ModulePLCDuplex.EcrireBit("DB1.DBX4.6", CBool(0))
                                        MAJBoutonActif(Buttonskh, True) : MAJBoutonActif(Buttonskb, False) : MAJBoutonActif(Buttonska, False)
                                        AfficherForcage("Skip MONTEE") : EnregistrerAction("FORÇAGE", "Skip montee", DetailBascules("GRANULATS"), "DB1.DBX4.5", "1", "FORCAGE")
                                    End Sub
        AddHandler Buttonskb.Click, Sub(s, e)
                                        If Not Dosage_manuel.ModeForceGranulats Then MessageBox.Show("Activez FORÇAGE GRANULATS !") : Exit Sub
                                        ModulePLCDuplex.EcrireBit("DB1.DBX4.6", CBool(1)) : ModulePLCDuplex.EcrireBit("DB1.DBX4.5", CBool(0))
                                        MAJBoutonActif(Buttonskb, True) : MAJBoutonActif(Buttonskh, False) : MAJBoutonActif(Buttonska, False)
                                        AfficherForcage("Skip DESCENTE") : EnregistrerAction("FORÇAGE", "Skip descente", DetailBascules("GRANULATS"), "DB1.DBX4.6", "1", "FORCAGE")
                                    End Sub
        AddHandler Buttonska.Click, Sub(s, e)
                                        If Not Dosage_manuel.ModeForceGranulats Then MessageBox.Show("Activez FORÇAGE GRANULATS !") : Exit Sub
                                        ModulePLCDuplex.EcrireBit("DB1.DBX4.5", CBool(0)) : ModulePLCDuplex.EcrireBit("DB1.DBX4.6", CBool(0))
                                        MAJBoutonActif(Buttonska, True) : MAJBoutonActif(Buttonskh, False) : MAJBoutonActif(Buttonskb, False)
                                        AfficherForcage("Skip ARRET") : EnregistrerAction("FORÇAGE", "Skip arrêt", DetailBascules("GRANULATS"), "DB1.DBX4.5+DB1.DBX4.6", "0", "FORCAGE")
                                    End Sub
        AjouterPoussoir(Buttonsl1, "DB1.DBX5.1", "CIMENTS", "Vis CIM 1", stockSource:="S1")
        AjouterPoussoir(Buttonsl2, "DB1.DBX5.2", "CIMENTS", "Vis CIM 2", stockSource:="S2")
        AjouterPoussoir(Buttonsltr, "DB1.DBX6.5", "CIMENTS", "Trappe CIM", stockVidange:="CIM")
        AjouterPoussoir(Buttoneau1, "DB1.DBX9.3", "EAUX", "Vanne EAU Ouvrir", stockSource:="EAU")
        AddHandler Buttoneau2.MouseDown, Sub(s, e)
                                             If Not Dosage_manuel.ModeForceEaux Then MessageBox.Show("Activez FORÇAGE EAUX !") : Exit Sub
                                             EcrirePLC("DB1.DBX9.3", False) : MAJBoutonActif(Buttoneau2, True)
                                             AfficherForcage("Vanne EAU FERMER") : EnregistrerAction("FORÇAGE", "Vanne EAU fermer", DetailBascules("EAUX"), "DB1.DBX9.3", "False", "FORCAGE")
                                         End Sub
        AddHandler Buttoneau2.MouseUp, Sub(s, e) MAJBoutonActif(Buttoneau2, False)
        AjouterPoussoir(Buttoneautr, "DB1.DBX9.5", "EAUX", "Trappe EAU", stockVidange:="EAU")
        AjouterPoussoir(Buttonadj1, "DB1.DBX8.1", "ADJUVANTS", "ADJ 1", stockSource:="ADJ1")
        AjouterPoussoir(Buttonadj2, "DB1.DBX8.2", "ADJUVANTS", "ADJ 2", stockSource:="ADJ2")
        AjouterPoussoir(Buttonadj3, "DB1.DBX8.3", "ADJUVANTS", "ADJ 3", stockSource:="ADJ3")
        AjouterPoussoir(Buttonadj4, "DB1.DBX8.4", "ADJUVANTS", "ADJ 4", stockSource:="ADJ4")
        AjouterPoussoir(Buttonadjtr, "DB1.DBX9.2", "ADJUVANTS", "Trappe ADJ", stockVidange:="ADJ")
        StyleBoutonMarche(Buttonmxm) : StyleBoutonArret(Buttonmxa)
        AddHandler Buttonmxm.Click, Sub(s, e)
                                        If Not Dosage_manuel.ModeForceMalaxeurs Then MessageBox.Show("Activez FORÇAGE MALAXEURS !") : Exit Sub
                                        If MessageBox.Show("Demarrer le malaxeur ?", "Confirmation",
                                            MessageBoxButtons.YesNo, MessageBoxIcon.Warning) = DialogResult.No Then Exit Sub
                                        ModulePLCDuplex.EcrireBit("DB1.DBX1088.2", CBool(1)) : MAJBoutonActif(Buttonmxm, True) : MAJBoutonActif(Buttonmxa, False)
                                        AfficherForcage("Malaxeur MARCHE") : EnregistrerAction("FORÇAGE", "Malaxeur marche", DetailBascules("MALAXEURS"), "DB1.DBX1088.2", "1", "FORCAGE")
                                    End Sub
        AddHandler Buttonmxa.Click, Sub(s, e)
                                        If Not Dosage_manuel.ModeForceMalaxeurs Then MessageBox.Show("Activez FORÇAGE MALAXEURS !") : Exit Sub
                                        ModulePLCDuplex.EcrireBit("DB1.DBX1088.2", CBool(0)) : MAJBoutonActif(Buttonmxa, True) : MAJBoutonActif(Buttonmxm, False)
                                        AfficherForcage("Malaxeur ARRET") : EnregistrerAction("FORÇAGE", "Malaxeur arrêt", DetailBascules("MALAXEURS"), "DB1.DBX1088.2", "0", "FORCAGE")
                                    End Sub
        AjouterPoussoir(Buttonmxov, "DB1.DBX10.0", "MALAXEURS", "Trappe MLX Ouvrir")
        AjouterPoussoir(Buttonmxfr, "DB1.DBX10.2", "MALAXEURS", "Trappe MLX Fermer")

        ' Masquer tous les boutons au demarrage — visibles uniquement en mode forcage
        CacherBoutonsGroupe("GRANULATS")
        CacherBoutonsGroupe("CIMENTS")
        CacherBoutonsGroupe("EAUX")
        CacherBoutonsGroupe("ADJUVANTS")
        CacherBoutonsGroupe("MALAXEURS")
    End Sub

    ' Genere le detail bascules selon le groupe actif
    Private Function DetailBascules(groupe As String) As String
        Select Case groupe.ToUpper()
            Case "GRANULATS"
                Return String.Format("AGR={0:0}kg | Cons={1:0}kg | Écart={2:+0;-0}kg",
                    _poids_AGR, _cons_G1 + _cons_SC + _cons_SL + _cons_D2,
                    _ecar_G1 + _ecar_SC + _ecar_SL + _ecar_D2)
            Case "CIMENTS"
                Dim poseCIM As Single = If(_siloCimentActif = 1, _pose_CIM_S1, _pose_CIM_S2)
                Return String.Format("CIM={0:0}kg | Cons={1:0}kg | Écart={2:+0;-0}kg | Silo={3}",
                    _poids_CIM, _cons_CIM, _ecar_CIM, _siloCimentActif)
            Case "EAUX"
                Return String.Format("EAU={0:0}L | Cons={1:0}L | Écart={2:+0;-0}L",
                    _poids_EAU, _cons_EAU2, _ecar_EAU2)
            Case "ADJUVANTS"
                Return String.Format("ADJ={0:0.000}kg | Cons={1:0.000}kg | A1={2:0.000} A2={3:0.000} A3={4:0.000} A4={5:0.000}",
                    _poids_ADJ,
                    _cons_ADJ1 + _cons_ADJ2 + _cons_ADJ3 + _cons_ADJ4,
                    _pose_ADJ1, _pose_ADJ2, _pose_ADJ3, _pose_ADJ4)
            Case "MALAXEURS"
                Return String.Format("Malaxeur={0} | Cycle={1} | Gâchée={2}",
                    If(_malaxeurON, "MARCHE", "ARRÊT"),
                    If(_cycleEnCours, "EN COURS", "ARRÊT"),
                    _nGachee)
            Case Else
                Return ""
        End Select
    End Function

    ' 26/09 : stockSource = équipement qui REMPLIT une bascule (TR1..4, S1/S2, EAU, ADJ1..4)
    '         stockVidange = trappe qui VIDE une bascule (CIM / EAU / ADJ) → ModuleStockManuel
    Private Sub AjouterPoussoir(btn As Button, adresse As String, groupe As String, nom As String,
                                Optional stockSource As String = "", Optional stockVidange As String = "")
        AddHandler btn.MouseDown, Sub(s, e)
                                      Dim ok As Boolean = False
                                      Select Case groupe.ToUpper()
                                          Case "GRANULATS" : ok = Dosage_manuel.ModeForceGranulats
                                          Case "CIMENTS" : ok = Dosage_manuel.ModeForceCiments
                                          Case "EAUX" : ok = Dosage_manuel.ModeForceEaux
                                          Case "ADJUVANTS" : ok = Dosage_manuel.ModeForceAdjuvants
                                          Case "MALAXEURS" : ok = Dosage_manuel.ModeForceMalaxeurs
                                      End Select
                                      If Not ok Then
                                          MessageBox.Show("Activez d'abord FORÇAGE " & groupe & " !",
                                              "Forçage requis", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                                          Exit Sub
                                      End If
                                      ' 26/09 : stock manuel — poids de la bascule AVANT l'action
                                      If stockSource <> "" Then ModuleStockManuel.DebutRemplissage(stockSource)
                                      If stockVidange <> "" Then ModuleStockManuel.DebutVidange(stockVidange)
                                      ' 22/09 : écriture via la file du module (verrouillée, jamais Nothing)
                                      ModulePLCDuplex.EcrireBit(adresse, True)
                                      btn.Tag = "appuye"
                                      MAJBoutonActif(CType(s, Button), True)
                                      AfficherForcage(nom & " ACTIVE")
                                      EnregistrerAction("FORCAGE", nom & " active",
                                          DetailBascules(groupe), adresse, "1", "FORÇAGE")
                                  End Sub
        Dim relacher = Sub(s As Object, e As EventArgs)
                           If Not String.Equals(TryCast(btn.Tag, String), "appuye") Then Exit Sub
                           btn.Tag = Nothing
                           ModulePLCDuplex.EcrireBit(adresse, False)
                           MAJBoutonActif(btn, False)
                           ' 26/09 : stock manuel — boutons du groupe bloqués 0,5 s, puis lecture du poids
                           If stockSource <> "" Then BloquerGroupeStabilisation(groupe, Sub() ModuleStockManuel.FinRemplissage(stockSource))
                           If stockVidange <> "" Then BloquerGroupeStabilisation(groupe, Sub() ModuleStockManuel.FinVidange(stockVidange))
                           EnregistrerAction("FORCAGE", nom & " désactivé",
                               DetailBascules(groupe), adresse, "0", "FORÇAGE")
                       End Sub
        AddHandler btn.MouseUp, Sub(s, e) relacher(s, e)
        AddHandler btn.MouseLeave, relacher   ' 22/09 : souris sortie du bouton → bit remis à 0
    End Sub

    ' 21/09 : boutons MARCHE toujours verts, boutons ARRÊT toujours rouges
    '         (couleur vive = état actif, couleur pâle = inactif) ; autres poussoirs : vert actif / gris inactif
    Private Function EstBoutonArret(btn As Button) As Boolean
        Return btn Is Buttontpm OrElse btn Is Buttonska OrElse btn Is Buttonmxa OrElse btn Is Buttoneau2
    End Function

    Private Function EstBoutonMarche(btn As Button) As Boolean
        Return btn Is Buttontpa OrElse btn Is Buttonskh OrElse btn Is Buttonskb OrElse btn Is Buttonmxm
    End Function

    ' 26/09 : bloque les boutons d'un groupe pendant DELAI_STABILISATION_MS (bascule stable),
    '         puis exécute l'action (lecture du poids → panier / sortie de stock) et débloque.
    Private Sub BloquerGroupeStabilisation(groupe As String, action As Action)
        Dim boutons As List(Of Button) = GetBoutonsGroupe(groupe)
        For Each b In boutons : b.Enabled = False : Next
        Dim tmr As New Timer() With {.Interval = ModuleStockManuel.DELAI_STABILISATION_MS}
        AddHandler tmr.Tick, Sub()
                                 tmr.Stop() : tmr.Dispose()
                                 For Each b In boutons : b.Enabled = True : Next
                                 Try
                                     action()
                                 Catch ex As Exception
                                     Debug.WriteLine("Stock manuel : " & ex.Message)
                                 End Try
                             End Sub
        tmr.Start()
    End Sub

    Private Sub MAJBoutonActif(btn As Button, actif As Boolean)
        If btn Is Nothing OrElse Not btn.Visible Then Exit Sub
        If EstBoutonArret(btn) Then
            btn.BackColor = If(actif, Color.FromArgb(220, 40, 40), Color.FromArgb(240, 150, 150))
            btn.ForeColor = Color.White
        ElseIf EstBoutonMarche(btn) Then
            btn.BackColor = If(actif, Color.FromArgb(40, 190, 40), Color.FromArgb(150, 230, 150))
            btn.ForeColor = Color.White
        Else
            btn.BackColor = If(actif, Color.FromArgb(60, 190, 60), Color.FromArgb(220, 220, 220))
            btn.ForeColor = If(actif, Color.White, Color.Black)
        End If
    End Sub

    Private Sub StyleBoutonMarche(btn As Button)
        If btn Is Nothing Then Exit Sub
        btn.BackColor = Color.FromArgb(100, 220, 100)
        btn.ForeColor = Color.White
        btn.FlatAppearance.BorderColor = Color.DarkGreen
    End Sub

    Private Sub StyleBoutonArret(btn As Button)
        If btn Is Nothing Then Exit Sub
        btn.BackColor = Color.FromArgb(220, 60, 60)
        btn.ForeColor = Color.White
        btn.FlatAppearance.BorderColor = Color.DarkRed
    End Sub

    Private Sub EcrirePLC(adresse As String, valeur As Boolean)
        Try
            If _plc IsNot Nothing AndAlso _plc.IsConnected Then
                _plc.Write(adresse, If(valeur, 1, 0))
            End If
        Catch ex As Exception
            Debug.WriteLine("Erreur PLC " & adresse & " : " & ex.Message)
        End Try
    End Sub

    ' 22/09 : plus de Timer jetable ; message conservé dans _dernierMessageForcage (Debug)
    Private _dernierMessageForcage As String = ""
    Private Sub AfficherForcage(msg As String)
        _dernierMessageForcage = DateTime.Now.ToString("HH:mm:ss") & " " & msg
        Debug.WriteLine("FORCAGE : " & _dernierMessageForcage)
    End Sub

    ' ═══════════════════════════════════════════════════════════════
    '  BOUTONS OPERATEUR — UN SEUL HANDLER PAR BOUTON (CORRIGE)
    '  Appele UNE SEULE FOIS depuis Form_Synoptique_Load
    ' ═══════════════════════════════════════════════════════════════
    Private Sub InitBoutonsOperateur()

        ' ── Style commun ─────────────────────────────────────────
        For Each btn As Button In {btnModeAuto, btnDepartCycle, btnRAZ,
                                   btnRAZDefaut, btnFinChargement, btnVidangeBeton}
            btn.Size = New Size(140, 38)
            btn.Font = New Font("Segoe UI", 9, FontStyle.Bold)
            btn.FlatStyle = FlatStyle.Flat
            btn.Cursor = Cursors.Hand
        Next

        ' ── Textes fixes ─────────────────────────────────────────
        btnDepartCycle.Text = "▶  DÉPART CYCLE"
        btnRAZ.Text = "↺  RAZ"
        btnRAZDefaut.Text = "⚠  REPRISE DÉFAUT"
        btnFinChargement.Text = "✔  FIN CHARGEMENT"

        ' ════════════════════════════════════════════════════════
        '  btnModeAuto — Bascule M9.1 (AUTO / MANUEL)
        '  CORRIGE : un seul handler, lit CPU avant d ecrire
        ' ════════════════════════════════════════════════════════
        AddHandler btnModeAuto.Click, Sub(s, e)
                                          If Not _plcConnecte Then
                                              MessageBox.Show("PLC non connecté.", "Mode", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                                              Exit Sub
                                          End If
                                          Try
                                              Dim modeActuel As Boolean = CBool(ModulePLCDuplex.LireAdresse("DB1.DBX0.0"))
                                              Dim nouveauMode As Boolean = Not modeActuel
                                              ModulePLCDuplex.EcrireBit("DB1.DBX0.0", CBool(If(nouveauMode, 1, 0)))
                                              _modeAuto = nouveauMode
                                              MettreAJourBoutonsOperateur()
                                              AfficherForcage("Mode " & If(nouveauMode, "AUTOMATIQUE", "MANUEL"))
                                              EnregistrerAction("COMMANDE", "Mode " & If(nouveauMode, "AUTO", "MANUEL"),
                                                  "DB1.DBX0.0=" & If(nouveauMode, "1", "0"), "DB1.DBX0.0",
                                                  If(nouveauMode, "1", "0"), "PLC_WRITE")
                                          Catch ex As Exception
                                              MessageBox.Show("Erreur Mode AUTO/MANU : " & ex.Message)
                                          End Try
                                      End Sub

        ' ════════════════════════════════════════════════════════
        '  btnDepartCycle — DB1.DBX0.4 MAINTENU (21/09/2026)
        '  0.4 = 1 → commande en cours : refus. 0.4 = 0 → écrit 1,
        '  la CPU le remet à 0 en fin de commande.
        ' ════════════════════════════════════════════════════════
        AddHandler btnDepartCycle.Click, Sub(s, e)
                                             If Not _plcConnecte Then
                                                 MessageBox.Show("PLC non connecté.", "Départ Cycle", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                                                 Exit Sub
                                             End If
                                             Try
                                                 If CBool(ModulePLCDuplex.LireAdresse("DB1.DBX0.4")) Then
                                                     MessageBox.Show("Commande en cours (DB1.DBX0.4 = 1) : attendez la fin de la commande.",
                                                                     "Départ Cycle", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                                                     Exit Sub
                                                 End If
                                                 ModulePLCDuplex.EcrireBit("DB1.DBX0.4", True)
                                                 AfficherForcage("Départ Cycle envoyé")
                                                 EnregistrerAction("COMMANDE", "Départ Cycle", "0.4=0->1 (maintenu)", "DB1.DBX0.4", "1", "PLC_WRITE")
                                             Catch ex As Exception
                                                 MessageBox.Show("Erreur Départ Cycle : " & ex.Message)
                                             End Try
                                         End Sub

        ' ════════════════════════════════════════════════════════
        '  btnRAZ — Impulsion M8.2
        ' ════════════════════════════════════════════════════════
        AddHandler btnRAZ.Click, Sub(s, e)
                                     If Not _plcConnecte Then
                                         MessageBox.Show("PLC non connecté.", "RAZ", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                                         Exit Sub
                                     End If
                                     Try
                                         ModulePLCDuplex.EcrireImpulsion("DB1.DBX914.4", 200)
                                         AfficherForcage("RAZ envoyé")
                                         EnregistrerAction("COMMANDE", "RAZ", "DB1.DBX914.4=1->0", "DB1.DBX914.4", "1->0", "PLC_WRITE")
                                     Catch ex As Exception
                                         MessageBox.Show("Erreur RAZ : " & ex.Message)
                                     End Try
                                 End Sub

        ' ════════════════════════════════════════════════════════
        '  btnRAZDefaut — Impulsion M23.7
        ' ════════════════════════════════════════════════════════
        AddHandler btnRAZDefaut.Click, Sub(s, e)
                                           If Not _plcConnecte Then Exit Sub
                                           Try
                                               ModulePLCDuplex.EcrireImpulsion("DB1.DBX914.0", 200)
                                               AfficherForcage("Reprise Défaut envoyée")
                                               EnregistrerAction("COMMANDE", "Reprise Défaut", "DB1.DBX914.0=1->0", "DB1.DBX914.0", "1->0", "PLC_WRITE")
                                           Catch ex As Exception
                                               MessageBox.Show("Erreur Reprise Défaut : " & ex.Message)
                                           End Try
                                       End Sub

        ' ════════════════════════════════════════════════════════
        '  btnFinChargement — Remet M50.3 a 0
        ' ════════════════════════════════════════════════════════
        AddHandler btnFinChargement.Click, Sub(s, e)
                                               If Not _plcConnecte Then
                                                   MessageBox.Show("PLC non connecté.", "Fin Chargement", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                                                   Exit Sub
                                               End If
                                               Try
                                                   ModulePLCDuplex.EcrireBit("DB1.DBX914.1", CBool(0))
                                                   AfficherForcage("Fin Chargement confirmé")
                                                   EnregistrerAction("COMMANDE", "Fin Chargement", "DB1.DBX914.1=0", "DB1.DBX914.1", "0", "PLC_WRITE")
                                               Catch ex As Exception
                                                   MessageBox.Show("Erreur Fin Chargement : " & ex.Message)
                                               End Try
                                           End Sub

        ' ════════════════════════════════════════════════════════
        '  btnVidangeBeton — Bascule M58.3
        '  CORRIGE : adresse unique M58.3 (etait M14.2 dans un handler)
        ' ════════════════════════════════════════════════════════
        AddHandler btnVidangeBeton.Click, Sub(s, e)
                                              If Not _plcConnecte Then
                                                  MessageBox.Show("PLC non connecté.", "Vidange", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                                                  Exit Sub
                                              End If
                                              Try
                                                  Dim vidangeActuelle As Boolean = CBool(ModulePLCDuplex.LireAdresse("DB1.DBX914.5"))
                                                  Dim nouvelleVidange As Boolean = Not vidangeActuelle
                                                  ModulePLCDuplex.EcrireBit("DB1.DBX914.5", CBool(If(nouvelleVidange, 1, 0)))
                                                  _vidangeMlx = nouvelleVidange
                                                  MettreAJourBoutonsOperateur()
                                                  AfficherForcage("Vidange béton " & If(nouvelleVidange, "AUTORISEE", "INTERDITE"))
                                                  EnregistrerAction("COMMANDE", "Vidange béton " & If(nouvelleVidange, "AUTORISEE", "INTERDITE"),
                                                      "DB1.DBX914.5=" & If(nouvelleVidange, "1", "0"), "DB1.DBX914.5",
                                                      If(nouvelleVidange, "1", "0"), "PLC_WRITE")
                                              Catch ex As Exception
                                                  MessageBox.Show("Erreur Vidange : " & ex.Message)
                                              End Try
                                          End Sub

        ' ── Visibilite initiale ──────────────────────────────────
        btnDepartCycle.Visible = False
        btnRAZ.Visible = False
        btnRAZDefaut.Visible = False
        btnFinChargement.Visible = False

        MettreAJourBoutonsOperateur()
    End Sub

    ' ── Mise a jour affichage boutons selon etat CPU ──────────────
    Public Sub MettreAJourBoutonsOperateur()
        If Not _plcConnecte Then
            btnDepartCycle.Visible = False
            btnRAZ.Visible = False
            btnRAZDefaut.Visible = False
            btnFinChargement.Visible = False
            btnModeAuto.Text = "MODE : ?"
            btnModeAuto.BackColor = Color.Gray
            btnVidangeBeton.Text = "VIDANGE : ?"
            btnVidangeBeton.BackColor = Color.Gray
            Exit Sub
        End If

        ' ── AUTO / MANU ──────────────────────────────────────────
        If _modeAuto Then
            btnModeAuto.Text = "⚙  AUTOMATIQUE"
            btnModeAuto.BackColor = Color.FromArgb(0, 140, 0)
            btnModeAuto.ForeColor = Color.White
        Else
            btnModeAuto.Text = "✋  MANUEL"
            btnModeAuto.BackColor = Color.FromArgb(180, 100, 0)
            btnModeAuto.ForeColor = Color.White
        End If

        ' ── Depart Cycle — visible seulement si AUTO ─────────────
        btnDepartCycle.Visible = _modeAuto
        If _modeAuto Then
            btnDepartCycle.BackColor = Color.FromArgb(0, 140, 0)
            btnDepartCycle.ForeColor = Color.White
        End If

        ' ── RAZ — visible seulement si MANUEL ────────────────────
        btnRAZ.Visible = Not _modeAuto
        If Not _modeAuto Then
            btnRAZ.BackColor = Color.FromArgb(180, 100, 0)
            btnRAZ.ForeColor = Color.White
        End If

        ' ── Reprise Defaut — visible seulement si defaut (M5.0=1) 
        btnRAZDefaut.Visible = _defautActif
        If _defautActif Then
            btnRAZDefaut.BackColor = If(DateTime.Now.Second Mod 2 = 0,
                Color.FromArgb(200, 50, 0), Color.FromArgb(255, 100, 50))
            btnRAZDefaut.ForeColor = Color.White
        End If

        ' ── Fin Chargement — visible seulement si M50.3=1 ────────
        btnFinChargement.Visible = _finChargement
        ' 21/09 : confirmation fin de cycle cachée dès que la demande n'est plus en attente
        Try
            If lbfin.Visible AndAlso Not ModuleArchivageGachees.Instance.AbandonEnCours Then AfficherConfirmationFinCycle(False)
            If _btnAbandonTexte <> "" AndAlso Not ModuleArchivageGachees.Instance.AbandonEnCours AndAlso btnabbondomennt.Text <> _btnAbandonTexte Then MAJBoutonAbandon(False)
        Catch
        End Try
        If _finChargement Then
            btnFinChargement.BackColor = Color.FromArgb(10, 50, 150)
            btnFinChargement.ForeColor = Color.White
        End If

        ' ── Vidange Beton (M58.3) — toujours visible ─────────────
        If _vidangeMlx Then
            btnVidangeBeton.Text = "🔓  VIDANGE ON"
            btnVidangeBeton.BackColor = Color.FromArgb(0, 120, 200)
            btnVidangeBeton.ForeColor = Color.White
        Else
            btnVidangeBeton.Text = "🔒  VIDANGE OFF"
            btnVidangeBeton.BackColor = Color.FromArgb(90, 90, 90)
            btnVidangeBeton.ForeColor = Color.White
        End If
    End Sub

    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click
        Planning_journalier.Show()
    End Sub
    ' ═══════════════════════════════════════════════════════════════
    '  CHARGER CONFIG ELEMENTS DEPUIS SQL
    '  Appeler dans Timer2_Tick (toutes les 2s)
    ' ═══════════════════════════════════════════════════════════════
    Private Sub ChargerConfigElements()
        ' 26/09 : noms affichés = noms CONFIGURÉS dans l'écran Configuration (dbo.Silos, table unique :
        '         TypeElement TR / S / ADJ / EAU, NumeroElement, NomSilo) — plus dbo.Cases / Bascules / Produits.
        Try
            Dim dt As DataTable = ModuleDb.Lire(
                "SELECT s.TypeElement, s.NumeroElement, s.NomSilo, m.CodeMateriau, m.NomMateriau " &
                "FROM dbo.Silos s LEFT JOIN dbo.Materiaux m ON m.IdMateriau = s.IdMateriau " &
                "WHERE s.EstActif = 1 ORDER BY s.TypeElement, s.NumeroElement, s.SiloID")
            Dim kAdj As Integer = 0
            For Each row As DataRow In dt.Rows
                Dim typ As String = row("TypeElement").ToString().Trim().ToUpper()
                Dim num As Integer = If(IsDBNull(row("NumeroElement")), 0, CInt(row("NumeroElement")))
                Dim nom As String = If(IsDBNull(row("NomSilo")), "", row("NomSilo").ToString().Trim())
                Dim codeMat As String = If(IsDBNull(row("CodeMateriau")), "", row("CodeMateriau").ToString().Trim())
                If nom = "" Then nom = If(codeMat <> "", codeMat, typ & num)
                Select Case typ
                    Case "TR" : If num >= 1 AndAlso num <= 4 Then _nomTR(num) = nom : _codeMatTR(num) = codeMat
                    Case "S" : If num >= 1 AndAlso num <= 2 Then _nomS(num) = nom
                    Case "ADJ"
                        kAdj += 1
                        Dim idx As Integer = If(num >= 1 AndAlso num <= 4, num, kAdj)
                        If idx >= 1 AndAlso idx <= 4 Then _nomADJ(idx) = nom
                    Case "EAU" : _nomEAU = nom
                End Select
            Next
            For m As Integer = 1 To 4
                If String.IsNullOrEmpty(_nomADJ(m)) Then _nomADJ(m) = "-"
            Next
        Catch ex As Exception
            Debug.WriteLine("ChargerConfigElements : " & ex.Message)
        End Try
    End Sub
    ' ═══════════════════════════════════════════════════════════════
    '  COULEUR SELON ECART POSE / CONSIGNE — factorisée
    '  Gris = pas de donnée | Vert = dosage atteint (dans tolérance)
    '  Bleu = en cours (pose < consigne) | Rouge = dépassement
    ' ═══════════════════════════════════════════════════════════════
    Private Function CouleurEcart(pose As Single, consigne As Single, tolerance As Single) As Color
        If pose <= 0 AndAlso consigne <= 0 Then Return Color.Gray
        Dim ecart As Single = consigne - pose
        If Math.Abs(ecart) <= tolerance Then Return Color.DarkGreen
        If pose < consigne Then Return Color.Blue
        Return Color.Red
    End Function
    ' ═══════════════════════════════════════════════════════════════
    '  AFFICHER LABELS EN CONTINU
    '  Mapping DB18 :
    '   lblStockTR1=DBD2  lblStockTR2=DBD6  lblStockTR3=DBD10  lblStockTR4=DBD14
    '   lblNiveauS1=DBD18 lblNiveauS2=DBD58 lblNomEAU=DBD22
    '   lblNomADJ1=DBD62  lblNomADJ2=DBD66  lblNomADJ3=DBD70   lblNomADJ4=DBD74
    ' ═══════════════════════════════════════════════════════════════
    Private Sub AfficherLabelsContinu()
        Try
            ' ── TRÉMIES — chaque trémie a SA propre pose (DB18) ──
            Dim lblsNomTR() As Label = {Nothing, lblNomTR1, lblNomTR2, lblNomTR3, lblNomTR4}
            Dim lblsStockTR() As Label = {Nothing, lblStockTR1, lblStockTR2, lblStockTR3, lblStockTR4}
            Dim posTR() As Single = {0, _pose_G1, _pose_SC, _pose_SL, _pose_D2}
            Dim consTR() As Single = {0, _cons_G1, _cons_SC, _cons_SL, _cons_D2}

            For i As Integer = 1 To 4
                If lblsNomTR(i) Is Nothing Then Continue For

                ' Nom produit
                lblsNomTR(i).Text = If(_nomTR(i) <> "", _nomTR(i), "TR" & i)
                lblsNomTR(i).ForeColor = Color.DarkBlue

                ' Pose réelle uniquement (DB18 DBD2/6/10/14)
                lblsStockTR(i).Text = posTR(i).ToString("0.0") & " kg"
                lblsStockTR(i).ForeColor = CouleurEcart(posTR(i), consTR(i), TolAbs(i - 1, consTR(i), TOL_KG))
            Next

            ' ── SILOS CIMENT : nom seulement ─────────────────────
            ' 25/09 : lblNiveauS1/S2 = niveau de stock (gestion de stock logiciel, AfficherNiveauxSilo),
            '         plus la pose ciment (déjà visible sur la bascule et dans le tableau de poids).
            Dim lblsNomS() As Label = {Nothing, lblNomS1, lblNomS2}
            For j As Integer = 1 To 2
                If lblsNomS(j) Is Nothing Then Continue For
                lblsNomS(j).Text = If(_nomS(j) <> "", _nomS(j), "SILO" & j)
                lblsNomS(j).ForeColor = Color.DarkSlateBlue
            Next

            ' ── ADJUVANTS (DB18 DBD62/66/70/74) ──────────────────
            Dim lblsAdj() As Label = {Nothing, lblNomADJ1, lblNomADJ2, lblNomADJ3, lblNomADJ4}
            Dim posAdj() As Single = {0, _pose_ADJ1, _pose_ADJ2, _pose_ADJ3, _pose_ADJ4}
            Dim consAdj() As Single = {0, _cons_ADJ1, _cons_ADJ2, _cons_ADJ3, _cons_ADJ4}

            For k As Integer = 1 To 4
                If lblsAdj(k) Is Nothing Then Continue For
                lblsAdj(k).Text = If(_nomADJ(k) <> "", _nomADJ(k), "ADJ" & k) & vbCrLf &
                              posAdj(k).ToString("0.000") & " kg"
                lblsAdj(k).ForeColor = CouleurEcart(posAdj(k), consAdj(k), TolAbs(5 + k, consAdj(k), TOL_ADJ))
            Next

            ' ── EAU (DB18 DBD22) ──────────────────────────────────
            If lblNomEAU IsNot Nothing Then
                lblNomEAU.Text = If(_nomEAU <> "", _nomEAU, "EAU") & vbCrLf &
                             _pose_EAU2.ToString("0.0") & " L"
                lblNomEAU.ForeColor = CouleurEcart(_pose_EAU2, _cons_EAU2, TolAbs(5, _cons_EAU2, TOL_L))
            End If

        Catch ex As Exception
            Debug.WriteLine("AfficherLabelsContinu : " & ex.Message)
        End Try
    End Sub
    ' ═══ SYNCHRONISATION depuis ModulePLCDuplex (0 appel réseau) ═══
    Private Sub SynchroniserDepuisPLCData()
        _poids_AGR = PLCData.Poids_AGR : _poids_CIM = PLCData.Poids_CIM
        _poids_EAU = PLCData.Poids_EAU : _poids_ADJ = PLCData.Poids_ADJ
        _poidsTapis = PLCData.PoidsTapis : _consigneTapis = PLCData.ConsigneTapis
        _ecar_ADJ1 = PLCData.Ecar_ADJ1 : _ecar_ADJ2 = PLCData.Ecar_ADJ2
        _ecar_ADJ3 = PLCData.Ecar_ADJ3 : _ecar_ADJ4 = PLCData.Ecar_ADJ4

        _cons_G1 = PLCData.Cons_G1 : _cons_SC = PLCData.Cons_SC
        _cons_SL = PLCData.Cons_SL : _cons_D2 = PLCData.Cons_D2
        _cons_CIM = PLCData.Cons_CIM : _cons_EAU2 = PLCData.Cons_EAU2
        _cons_ADJ1 = PLCData.Cons_ADJ1 : _cons_ADJ2 = PLCData.Cons_ADJ2
        _cons_ADJ3 = PLCData.Cons_ADJ3 : _cons_ADJ4 = PLCData.Cons_ADJ4

        _nGachee = PLCData.NGachee
        _pose_G1 = PLCData.Pose_G1 : _pose_SC = PLCData.Pose_SC
        _pose_SL = PLCData.Pose_SL : _pose_D2 = PLCData.Pose_D2
        _pose_CIM_S1 = PLCData.Pose_CIM_S1 : _pose_CIM_S2 = PLCData.Pose_CIM_S2
        _pose_EAU2 = PLCData.Pose_EAU2
        _pose_ADJ1 = PLCData.Pose_ADJ1 : _pose_ADJ2 = PLCData.Pose_ADJ2
        _pose_ADJ3 = PLCData.Pose_ADJ3 : _pose_ADJ4 = PLCData.Pose_ADJ4
        _ecar_G1 = PLCData.Ecar_G1 : _ecar_SC = PLCData.Ecar_SC
        _ecar_SL = PLCData.Ecar_SL : _ecar_D2 = PLCData.Ecar_D2
        _ecar_CIM = PLCData.Ecar_CIM : _ecar_EAU2 = PLCData.Ecar_EAU2

        _capMalaxeur = PLCData.CapMalaxeur : _capCamion = PLCData.CapCamion

        _th_G1 = PLCData.Th_G1 : _th_SC = PLCData.Th_SC : _th_SL = PLCData.Th_SL
        _th_D2 = PLCData.Th_D2 : _th_CIM = PLCData.Th_CIM : _th_EAU = PLCData.Th_EAU
        _th_ADJ1 = PLCData.Th_ADJ1 : _th_ADJ2 = PLCData.Th_ADJ2
        _th_ADJ3 = PLCData.Th_ADJ3 : _th_ADJ4 = PLCData.Th_ADJ4

        _reel_G1 = PLCData.Reel_G1 : _reel_SC = PLCData.Reel_SC
        _reel_SL = PLCData.Reel_SL : _reel_D2 = PLCData.Reel_D2
        _reel_CIM = PLCData.Reel_CIM : _reel_EAU = PLCData.Reel_EAU
        _reel_ADJ1 = PLCData.Reel_ADJ1 : _reel_ADJ2 = PLCData.Reel_ADJ2
        _reel_ADJ3 = PLCData.Reel_ADJ3 : _reel_ADJ4 = PLCData.Reel_ADJ4

        _pctEcar_G1 = PLCData.PctEcar_G1 : _pctEcar_SC = PLCData.PctEcar_SC
        _pctEcar_SL = PLCData.PctEcar_SL : _pctEcar_D2 = PLCData.PctEcar_D2
        _pctEcar_CIM = PLCData.PctEcar_CIM : _pctEcar_EAU = PLCData.PctEcar_EAU
        _pctEcar_ADJ1 = PLCData.PctEcar_ADJ1 : _pctEcar_ADJ2 = PLCData.PctEcar_ADJ2
        _pctEcar_ADJ3 = PLCData.PctEcar_ADJ3 : _pctEcar_ADJ4 = PLCData.PctEcar_ADJ4

        _bitTr1 = PLCData.BitTr1 : _bitTr2 = PLCData.BitTr2
        _bitTr3 = PLCData.BitTr3 : _bitTr4 = PLCData.BitTr4
        _bitMx = PLCData.BitMx : _bitS1 = PLCData.BitS1 : _bitS2 = PLCData.BitS2
        ' 19/09 : flèche VERTE = DB1.DBX23.0 = 1 ET DB1.DBX30.1 = 0 ; ROUGE = DB1.DBX914.6 = 1
        _bitMxVert = PLCData.BitMx AndAlso PLCData.BitMxVert : _bitMxRouge = PLCData.BitMxRouge
        _bitSl = PLCData.BitSl : _bitEa1 = PLCData.BitEa1 : _bitEau = PLCData.BitEau
        _bitAdj1 = PLCData.BitAdj1 : _bitAdj2 = PLCData.BitAdj2
        _bitAdj3 = PLCData.BitAdj3 : _bitAdj4 = PLCData.BitAdj4
        _bitEaadjj = PLCData.BitEaadjj
        _malaxeurON = PLCData.MalaxeurON : _malaxeurPlein = PLCData.MalaxeurPlein
        _cycleEnCours = PLCData.CycleEnCours : _arretUrg = PLCData.ArretUrg
        _skipPlein = PLCData.SkipPlein : _skipHaut = PLCData.SkipHaut
        _skipAttente = PLCData.SkipAttente : _skipBas = PLCData.SkipBas
        _modeAuto = PLCData.ModeAuto : _defautActif = PLCData.DefautActif
        _finChargement = PLCData.FinChargement : _vidangeMlx = PLCData.VidangeMlx

        _bitDens1 = PLCData.BitDens(1) : _bitDens2 = PLCData.BitDens(2)
        _bitDens3 = PLCData.BitDens(3) : _bitDens4 = PLCData.BitDens(4)
        _bitDens5 = PLCData.BitDens(5) : _bitDens6 = PLCData.BitDens(6)
        _bitDens7 = PLCData.BitDens(7) : _bitDens8 = PLCData.BitDens(8)
        _bitDens9 = PLCData.BitDens(9) : _bitDens10 = PLCData.BitDens(10)
        _bitDens11 = PLCData.BitDens(11) : _bitDens12 = PLCData.BitDens(12)

        ' Vis ciment (DB8 DBW324)
        If Not _visEnEcriture AndAlso ComboBoxvis.Items.Count >= 2 Then
            If PLCData.VisActif = 0 Then
                If ComboBoxvis.SelectedIndex <> 0 Then ComboBoxvis.SelectedIndex = 0
            ElseIf PLCData.VisActif = 1 Then
                If ComboBoxvis.SelectedIndex <> 1 Then ComboBoxvis.SelectedIndex = 1
            End If
        End If

        ' ── États calculés (identique à avant) ──
        _etatTremie(1) = If(_bitTr1, 1, 0) : _etatTremie(2) = If(_bitTr2, 1, 0)
        _etatTremie(3) = If(_bitTr3, 1, 0) : _etatTremie(4) = If(_bitTr4, 1, 0)
        _etatSilo(1) = If(_bitS1, 1, 0) : _etatSilo(2) = If(_bitS2, 1, 0)
        _etatBasculeCim = If(_bitSl, 1, 0) : _etatBasculeEau = If(_bitEa1, 1, 0)
        _etatAdj(1) = If(_bitAdj1, 1, 0) : _etatAdj(2) = If(_bitAdj2, 1, 0)
        _etatAdj(3) = If(_bitAdj3, 1, 0) : _etatAdj(4) = If(_bitAdj4, 1, 0)
        _etatBasculeAdj = If(_poids_ADJ > 0.5, 1, 0)
        ' Hystérésis : passe à PLEIN au-dessus de consigne,
        ' ne repasse à VIDE qu'en dessous de consigne - 5 kg
        If _consigneTapis > 0 Then
            If _poidsTapis >= _consigneTapis Then
                _tapisPlein = True
            ElseIf _poidsTapis < _consigneTapis - 5.0F Then
                _tapisPlein = False
            End If
            ' entre les deux : garder l'état précédent
        Else
            _tapisPlein = False
        End If

        If _arretUrg Then : _etatMalaxeur = 0
        ElseIf Not _malaxeurON Then : _etatMalaxeur = 0
        ElseIf _malaxeurPlein Then : _etatMalaxeur = 1
        Else : _etatMalaxeur = 2
        End If

        If PLCData.TapisMarche Then
            _etatTapis = If(_tapisPlein, 2, 3)
        Else
            _etatTapis = If(_tapisPlein, 1, 0)
        End If

        If PLCData.SkipMonte Then : _etatSkip = 6
        ElseIf PLCData.SkipDesc Then : _etatSkip = 7
        ElseIf _skipBas Then : _etatSkip = If(_skipPlein, 3, 0)
        ElseIf _skipAttente Then : _etatSkip = If(_skipPlein, 4, 1)
        ElseIf _skipHaut Then : _etatSkip = If(_skipPlein, 5, 2)
        Else : _etatSkip = 0
        End If
    End Sub

    ' ── FIN DE CYCLE (abandon) 21/09/2026 — confirmation sur le Synoptique :
    '    btnabbondomennt → 914.2 = 1 + affichage lbfin / finoui / finno
    '    finoui → 914.3 = 1     finno → 914.2 = 0
    ' 22/09 : couleur du bouton fin de cycle pendant la demande
    Private _btnAbandonCouleur As Color = Color.Empty
    Private _btnAbandonTexte As String = ""
    Private Sub MAJBoutonAbandon(demandeEnCours As Boolean)
        Try
            If _btnAbandonTexte = "" Then
                _btnAbandonCouleur = btnabbondomennt.BackColor
                _btnAbandonTexte = btnabbondomennt.Text
            End If
            If demandeEnCours Then
                btnabbondomennt.BackColor = Color.Orange
                btnabbondomennt.ForeColor = Color.Black
                btnabbondomennt.Text = "Annuler" & vbCrLf & "arrêt en fin"
            Else
                btnabbondomennt.BackColor = _btnAbandonCouleur
                btnabbondomennt.UseVisualStyleBackColor = True
                btnabbondomennt.ForeColor = SystemColors.ControlText
                btnabbondomennt.Text = _btnAbandonTexte
            End If
        Catch
        End Try
    End Sub

    Private Sub AfficherConfirmationFinCycle(visible As Boolean)
        Try
            lbfin.Visible = visible
            finoui.Visible = visible
            finno.Visible = visible
            If visible Then
                finoui.BackColor = Color.FromArgb(0, 150, 60) : finoui.ForeColor = Color.White
                finno.BackColor = Color.FromArgb(200, 40, 40) : finno.ForeColor = Color.White
                lbfin.BringToFront() : finoui.BringToFront() : finno.BringToFront()
            End If
        Catch
        End Try
    End Sub

    ' 22/09 : bouton BASCULE (comme WinCC)
    '   « arret en fin »        → clic → 914.2 = 1, bouton orange « Annule arret en fin »
    '   « Annule arret en fin » → clic → 914.2 = 0, retour « arret en fin »
    '   OUI / NON (finoui / finno) apparaissent quand 1088.0 = 1
    Private Sub btnabbondomennt_Click(sender As Object, e As EventArgs) Handles btnabbondomennt.Click
        If Not _plcConnecte Then
            MessageBox.Show("PLC non connecté.", "Fin de cycle", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Exit Sub
        End If
        If ModuleArchivageGachees.Instance.AbandonEnCours Then
            ' Annulation de la demande
            ModuleArchivageGachees.Instance.ConfirmerFinCycle(False)
            AfficherConfirmationFinCycle(False)
            MAJBoutonAbandon(False)
            AfficherForcage("Arrêt en fin ANNULÉ (914.2 = 0)")
            Exit Sub
        End If
        If ModuleArchivageGachees.Instance.DemanderFinCycle() Then
            MAJBoutonAbandon(True)
            AfficherForcage("Arrêt en fin demandé (914.2 = 1) - attente fin de la gâchée")
        Else
            MessageBox.Show("Aucune commande en cours : rien à arrêter.", "Fin de cycle",
                            MessageBoxButtons.OK, MessageBoxIcon.Warning)
        End If
    End Sub

    ' 22/09 : appelée par ModuleArchivageGachees quand 1088.0 = 1 (fin de la gâchée en cours)
    Public Shared Sub MontrerConfirmationFinCycle()
        Try
            Dim syn As Synoptique = TrouverInstance()
            If syn Is Nothing Then Exit Sub
            If syn.InvokeRequired Then
                syn.BeginInvoke(Sub() syn.AfficherConfirmationFinCycle(True))
            Else
                syn.AfficherConfirmationFinCycle(True)
            End If
        Catch
        End Try
    End Sub

    Private Sub finoui_Click(sender As Object, e As EventArgs) Handles finoui.Click
        ModuleArchivageGachees.Instance.ConfirmerFinCycle(True)
        AfficherConfirmationFinCycle(False)
        AfficherForcage("Fin de cycle confirmée (914.3 = 1)")
    End Sub

    Private Sub finno_Click(sender As Object, e As EventArgs) Handles finno.Click
        ModuleArchivageGachees.Instance.ConfirmerFinCycle(False)
        AfficherConfirmationFinCycle(False)
        MAJBoutonAbandon(False)
        AfficherForcage("Fin de cycle annulée (914.2 = 0)")
    End Sub
    ' ═══════════════════════════════════════════════════════════════
    '  CURSEUR CORRECTION EAU (21/09/2026) — TrackBarEAU du Designer
    '  −20 … +20 → DB1.DBW1194 (Int, 16 bits)
    '  Graduations, titre et case de saisie ajoutés par le code autour
    '  du curseur (Entrée dans la case = envoi ; relâcher le curseur = envoi)
    ' ═══════════════════════════════════════════════════════════════
    Private Const ADR_CORR_EAU As String = "DB1.DBW1194"
    Private Const OFFSET_CORR_EAU As Integer = 1194
    Private Const CORR_EAU_MIN As Integer = -20
    Private Const CORR_EAU_MAX As Integer = 20
    Private _txtEauCorr As TextBox
    Private _lblEauGrad As Label
    Private _eauCorrEnCours As Boolean = False

    Private Sub CreerCurseurEau()
        If _txtEauCorr IsNot Nothing Then Exit Sub
        Try
            With TrackBarEAU
                .Minimum = CORR_EAU_MIN : .Maximum = CORR_EAU_MAX
                .TickFrequency = 5 : .SmallChange = 1 : .LargeChange = 5
                .TickStyle = TickStyle.TopLeft
                .Value = 0
            End With
            AddHandler TrackBarEAU.Scroll, AddressOf TbEau_Scroll
            AddHandler TrackBarEAU.MouseUp, Sub(o, ev) EnvoyerCorrectionEau(TrackBarEAU.Value)
            AddHandler TrackBarEAU.MouseDown, Sub(o, ev) TrackBarEAU.Focus()
            ' 22/09 : clavier — ↑/↓ = ±5, →/← = ±1 (envoi immédiat à la CPU)
            AddHandler TrackBarEAU.PreviewKeyDown, Sub(o, ev)
                                                       If ev.KeyCode = Keys.Up OrElse ev.KeyCode = Keys.Down OrElse
                                                          ev.KeyCode = Keys.Left OrElse ev.KeyCode = Keys.Right Then ev.IsInputKey = True
                                                   End Sub
            AddHandler TrackBarEAU.KeyDown, AddressOf ClavierCorrectionEau

            Dim parent As Control = TrackBarEAU.Parent
            Dim r As Rectangle = TrackBarEAU.Bounds

            ' Titre « eau » au-dessus
            Dim lblTitre As New Label With {
                .Text = "eau", .AutoSize = False, .Size = New Size(r.Width + 40, 16),
                .Location = New Point(r.X - 30, r.Y - 18), .TextAlign = ContentAlignment.MiddleCenter,
                .ForeColor = Color.Blue, .BackColor = Color.Transparent, .Font = New Font("Segoe UI", 9, FontStyle.Bold)}

            ' Graduations +20 … −20 à gauche
            _lblEauGrad = New Label With {
                .Location = New Point(r.X - 30, r.Y), .Size = New Size(30, r.Height), .BackColor = Color.Transparent}
            AddHandler _lblEauGrad.Paint, AddressOf DessinerGraduationsEau

            ' Case de saisie sous le curseur, centrée
            _txtEauCorr = New TextBox With {
                .Location = New Point(r.X + r.Width \ 2 - 23, r.Bottom + 4), .Size = New Size(46, 24), .Text = "0",
                .TextAlign = HorizontalAlignment.Center, .Font = New Font("Segoe UI", 9, FontStyle.Bold)}
            AddHandler _txtEauCorr.KeyDown, AddressOf TxtEauCorr_KeyDown

            parent.Controls.AddRange({lblTitre, _lblEauGrad, _txtEauCorr})
            lblTitre.BringToFront() : _lblEauGrad.BringToFront() : _txtEauCorr.BringToFront()

            LireCorrectionEau()
        Catch ex As Exception
            Debug.WriteLine("CreerCurseurEau : " & ex.Message)
        End Try
    End Sub

    Private Sub DessinerGraduationsEau(sender As Object, e As PaintEventArgs)
        Dim lbl As Label = CType(sender, Label)
        Using f As New Font("Segoe UI", 7)
            ' Course utile du curseur : ~10 px de marge en haut et en bas
            Dim haut As Single = 10, bas As Single = lbl.Height - 10
            Dim plage As Single = CORR_EAU_MAX - CORR_EAU_MIN
            For v As Integer = CORR_EAU_MAX To CORR_EAU_MIN Step -5
                Dim y As Single = bas - (v - CORR_EAU_MIN) / plage * (bas - haut)
                Dim txt As String = v.ToString()
                Dim w As Single = e.Graphics.MeasureString(txt, f).Width
                e.Graphics.DrawString(txt, f, Brushes.Black, lbl.Width - w - 1, y - 6)
            Next
        End Using
    End Sub

    Private Sub TbEau_Scroll(sender As Object, e As EventArgs)
        If _eauCorrEnCours Then Exit Sub
        _txtEauCorr.Text = TrackBarEAU.Value.ToString()
    End Sub

    ' 22/09 : interception des flèches AVANT le formulaire parent (panelmain) :
    '         curseur ou case eau avec le focus → ↑/↓ = ±5, →/← = ±1
    Protected Overrides Function ProcessCmdKey(ByRef msg As Message, keyData As Keys) As Boolean
        Try
            Dim k As Keys = keyData And Keys.KeyCode
            If (k = Keys.Up OrElse k = Keys.Down OrElse k = Keys.Left OrElse k = Keys.Right) AndAlso
               _txtEauCorr IsNot Nothing AndAlso
               (TrackBarEAU.Focused OrElse _txtEauCorr.Focused OrElse
                (Me.ActiveControl IsNot Nothing AndAlso (Me.ActiveControl Is TrackBarEAU OrElse Me.ActiveControl Is _txtEauCorr))) Then
                Dim pas As Integer = If(k = Keys.Up, 5, If(k = Keys.Down, -5, If(k = Keys.Right, 1, -1)))
                Dim v As Integer = Math.Max(CORR_EAU_MIN, Math.Min(CORR_EAU_MAX, TrackBarEAU.Value + pas))
                AfficherCorrectionEau(v)
                EnvoyerCorrectionEau(v)
                Return True
            End If
        Catch
        End Try
        Return MyBase.ProcessCmdKey(msg, keyData)
    End Function

    ' 22/09 : ↑/↓ = ±5, →/← = ±1 sur le curseur ou dans la case
    Private Sub ClavierCorrectionEau(sender As Object, e As KeyEventArgs)
        Dim pas As Integer = 0
        Select Case e.KeyCode
            Case Keys.Up : pas = 5
            Case Keys.Down : pas = -5
            Case Keys.Right : pas = 1
            Case Keys.Left : pas = -1
            Case Else : Exit Sub
        End Select
        e.Handled = True : e.SuppressKeyPress = True
        Dim v As Integer = Math.Max(CORR_EAU_MIN, Math.Min(CORR_EAU_MAX, TrackBarEAU.Value + pas))
        AfficherCorrectionEau(v)
        EnvoyerCorrectionEau(v)
    End Sub

    Private Sub TxtEauCorr_KeyDown(sender As Object, e As KeyEventArgs)
        If e.KeyCode = Keys.Up OrElse e.KeyCode = Keys.Down OrElse e.KeyCode = Keys.Left OrElse e.KeyCode = Keys.Right Then
            ClavierCorrectionEau(sender, e) : Exit Sub
        End If
        If e.KeyCode <> Keys.Enter Then Exit Sub
        e.SuppressKeyPress = True
        Dim v As Integer
        If Integer.TryParse(_txtEauCorr.Text.Trim(), v) Then
            v = Math.Max(CORR_EAU_MIN, Math.Min(CORR_EAU_MAX, v))
            AfficherCorrectionEau(v)
            EnvoyerCorrectionEau(v)
        Else
            _txtEauCorr.Text = TrackBarEAU.Value.ToString()
        End If
    End Sub

    Private Sub AfficherCorrectionEau(v As Integer)
        _eauCorrEnCours = True
        Try
            TrackBarEAU.Value = Math.Max(TrackBarEAU.Minimum, Math.Min(TrackBarEAU.Maximum, v))
            _txtEauCorr.Text = v.ToString()
        Finally
            _eauCorrEnCours = False
        End Try
    End Sub

    ' Lecture de la valeur actuelle du CPU (Int big endian) au chargement
    ' 22/09 : la case et le curseur affichent en permanence la valeur lue dans la CPU.
    '         Pas de mise à jour pendant une saisie (case avec le focus) ou un déplacement du curseur.
    Private _corrEauCpuAffichee As Integer = Integer.MinValue
    Private Sub SuivreCorrectionEauCPU()
        Try
            If _txtEauCorr Is Nothing OrElse Not PLCData.Connecte Then Exit Sub
            If _txtEauCorr.Focused OrElse TrackBarEAU.Capture Then Exit Sub
            Dim v As Integer = PLCData.CorrEau
            If v = _corrEauCpuAffichee Then Exit Sub
            _corrEauCpuAffichee = v
            _corrEauRelue = True
            AfficherCorrectionEau(Math.Max(CORR_EAU_MIN, Math.Min(CORR_EAU_MAX, v)))
            If v < CORR_EAU_MIN OrElse v > CORR_EAU_MAX Then _txtEauCorr.Text = v.ToString()
        Catch
        End Try
    End Sub

    Private _corrEauRelue As Boolean = False
    Private Sub LireCorrectionEau()
        ' 22/09 : valeur de l'instantané (DB1.DBW1194) ; relue une fois dès que le PLC est connecté
        Try
            If Not PLCData.Connecte Then Exit Sub
            Dim v As Integer = PLCData.CorrEau
            If v >= CORR_EAU_MIN AndAlso v <= CORR_EAU_MAX Then AfficherCorrectionEau(v)
            _corrEauRelue = True
        Catch
        End Try
    End Sub

    ' 22/09 : remise à zéro à chaque nouvelle gâchée / nouvelle commande
    '   (appelée par ModuleArchivageGachees, depuis n'importe quel thread)
    Public Shared Sub RemettreCorrectionEauZero(motif As String)
        Try
            Dim syn As Synoptique = TrouverInstance()
            Try
                ModulePLCDuplex.EcrireInt(ADR_CORR_EAU, 0)
            Catch
            End Try
            If syn IsNot Nothing AndAlso syn._txtEauCorr IsNot Nothing Then
                If syn.InvokeRequired Then
                    syn.BeginInvoke(Sub() syn.AfficherCorrectionEau(0))
                Else
                    syn.AfficherCorrectionEau(0)
                End If
            End If
            EnregistrerAction("COMMANDE", "Correction eau remise a 0", motif, ADR_CORR_EAU, "0", "AUTO")
        Catch
        End Try
    End Sub

    Private Sub EnvoyerCorrectionEau(valeur As Integer)
        If Not _plcConnecte Then
            AfficherForcage("PLC non connecté : correction eau non envoyée")
            Exit Sub
        End If
        Try
            If ModulePLCDuplex.EcrireInt(ADR_CORR_EAU, valeur) Then
                AfficherForcage("Correction eau = " & valeur)
                EnregistrerAction("COMMANDE", "Correction eau", valeur.ToString(), ADR_CORR_EAU, valeur.ToString(), "PLC_WRITE")
            Else
                AfficherForcage("Erreur écriture correction eau (" & ADR_CORR_EAU & ")")
            End If
        Catch ex As Exception
            AfficherForcage("Erreur correction eau : " & ex.Message)
        End Try
    End Sub
    ' ═══════════════════════════════════════════════════════════════
    '  COMPTEURS GÂCHÉES (22/09/2026) — lecture DB1.DBD1220…1232
    '    1220 Real  nombre de gâchées calculé      → lbgache
    '    1224 DInt  compteur gâchée en cours AGR   → lbcmpgachearg
    '    1228 DInt  compteur gâchée en cours CIM/EAU/ADJ → lbcmpgachcim
    '    1232 Real  reste compteur gâchée          → lbcmpgacherst
    ' ═══════════════════════════════════════════════════════════════
    Private Sub MettreAJourCompteursGachees()
        ' 22/09 : valeurs lues dans l'instantané DB1 (PLCData), plus de lecture réseau
        Try
            Dim t1 As String = PLCData.NbGacheeCalc.ToString("0")   ' 25/09 : entier, sans virgule
            Dim t2 As String = PLCData.CptGacheeAgr.ToString()
            Dim t3 As String = PLCData.CptGacheeCim.ToString()
            Dim t4 As String = PLCData.ResteGachee.ToString("0")    ' 25/09 : entier, sans virgule
            'If lbgache.Text <> t1 Then lbgache.Text = t1
            If lbcmpgachearg.Text <> t2 Then lbgache.Text = t2
            If lbcmpgachearg.Text <> t2 Then lbcmpgachearg.Text = t2
            If lbcmpgachcim.Text <> t3 Then lbcmpgachcim.Text = t3
            If lbcmpgacherst.Text <> t4 Then lbcmpgacherst.Text = t4
            ' 25/09 : compteurs trémies 1-4 (DB1.DBW1196…1202 Int) → cmptr1…cmptr4
            Dim lblsCpt() As Label = {cmptr1, cmptr2, cmptr3, cmptr4}
            For k As Integer = 1 To 4
                Dim tk As String = PLCData.CptTremie(k).ToString()
                If lblsCpt(k - 1).Text <> tk Then lblsCpt(k - 1).Text = tk
            Next
        Catch ex As Exception
            Debug.WriteLine("MettreAJourCompteursGachees : " & ex.Message)
        End Try
    End Sub
    ' ═══════════════════════════════════════════════════════════════
    '  DÉFAUTS CPU ACTIFS (22/09/2026) — grille créée par le code,
    '  visible seulement quand au moins un défaut est présent.
    '  Colonnes : Date / DB (adresse du bit) / Commentaire (AlarmesConfig)
    ' ═══════════════════════════════════════════════════════════════
    Private _gridDefauts As DataGridView
    Private _cleDefauts As String = ""

    ' 22/09 : DataGridViewDefauts du Designer (position / taille conservées)
    Private Sub CreerGrilleDefauts()
        If _gridDefauts IsNot Nothing Then Exit Sub
        Try
            _gridDefauts = DataGridViewDefauts
            With _gridDefauts
                .Visible = False
                .ReadOnly = True : .AllowUserToAddRows = False : .AllowUserToDeleteRows = False
                .RowHeadersVisible = False : .MultiSelect = False
                .SelectionMode = DataGridViewSelectionMode.FullRowSelect
                .BackgroundColor = Color.FromArgb(255, 235, 235) : .GridColor = Color.LightCoral
                .BorderStyle = BorderStyle.FixedSingle : .EnableHeadersVisualStyles = False
                .ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing
                .ColumnHeadersHeight = 22 : .ScrollBars = ScrollBars.Vertical
                .AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.None
                .RowTemplate.Height = 20
                .ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(180, 30, 30)
                .ColumnHeadersDefaultCellStyle.ForeColor = Color.White
                .ColumnHeadersDefaultCellStyle.Font = New Font("Segoe UI", 8, FontStyle.Bold)
                .DefaultCellStyle.BackColor = Color.FromArgb(255, 235, 235)
                .DefaultCellStyle.ForeColor = Color.DarkRed
                .DefaultCellStyle.Font = New Font("Segoe UI", 8)
                .DefaultCellStyle.SelectionBackColor = Color.FromArgb(255, 200, 200)
                .DefaultCellStyle.SelectionForeColor = Color.DarkRed
                .DataSource = Nothing
                .Columns.Clear()
                .Columns.Add("Date", "Date")
                .Columns.Add("DB", "DB")
                .Columns.Add("Commentaire", "Commentaire")
                .Columns(0).Width = 110
                .Columns(1).Visible = False          ' 22/09 : colonne DB masquée (Date + Commentaire seulement)
                .Columns(2).AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill
            End With
            For Each c As DataGridViewColumn In _gridDefauts.Columns
                c.SortMode = DataGridViewColumnSortMode.NotSortable
            Next
            ' 25/09 : ModuleDefautsCPU.Initialiser() (SQL) → Form_Synoptique_Shown, en arrière-plan
        Catch ex As Exception
            Debug.WriteLine("CreerGrilleDefauts : " & ex.Message)
        End Try
    End Sub

    ' Clic sur un défaut → écran Alarmes machine (historique + édition du commentaire)
    Private Sub DataGridViewDefauts_CellContentClick(sender As Object, e As DataGridViewCellEventArgs) Handles DataGridViewDefauts.CellContentClick
        Try
            Dim f As New Alarmes_machine()
            f.Show()
        Catch ex As Exception
            Debug.WriteLine("Ouverture Alarmes machine : " & ex.Message)
        End Try
    End Sub

    ' 22/09 : la table suit le bouton REPRISE DEFAUT (btnRAZDefaut) :
    '         visible seulement quand un défaut CPU est actif (DB1.DBX1082.0 = 1)
    Private Sub MettreAJourDefautsCPU()
        Try
            ModuleDefautsCPU.Scruter()
            If _gridDefauts Is Nothing Then Exit Sub
            If Not _defautActif Then
                If _gridDefauts.Visible Then _gridDefauts.Visible = False
                Exit Sub
            End If
            Dim actifs = ModuleDefautsCPU.Actifs()
            Dim cle As New System.Text.StringBuilder()
            For Each d In actifs : cle.Append(d.Adresse).Append("|").Append(d.Texte).Append(";") : Next
            If cle.ToString() <> _cleDefauts Then
                _cleDefauts = cle.ToString()
                _gridDefauts.SuspendLayout()
                _gridDefauts.Rows.Clear()
                For Each d In actifs
                    _gridDefauts.Rows.Add(d.DateApparition.ToString("dd/MM HH:mm:ss"), d.Adresse, d.Texte)
                Next
                _gridDefauts.ResumeLayout()
            End If
            If Not _gridDefauts.Visible Then _gridDefauts.Visible = True : _gridDefauts.BringToFront()
        Catch ex As Exception
            Debug.WriteLine("MettreAJourDefautsCPU : " & ex.Message)
        End Try
    End Sub
End Class