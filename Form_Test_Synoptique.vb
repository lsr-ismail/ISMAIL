Imports System.Windows.Forms
Imports S7.Net

' ================================================================
'  vForm_Test_Synoptique.vb
'  Lecture directe des etats equipements depuis le CPU S7-1200
'  Remplace le test manuel par des valeurs temps reel
'  Date : 18/06/2026
' ================================================================
Public Class vForm_Test_Synoptique

    Inherits Form

    ' ── Référence synoptique ─────────────────────────────────────
    Private _syn As Synoptique

    ' ── Timer lecture CPU ────────────────────────────────────────
    Private WithEvents timerCPU As New Timer()

    ' ── Labels valeurs temps reel ────────────────────────────────
    Private lblTremie1, lblTremie2, lblTremie3, lblTremie4 As Label
    Private lblTremie5, lblTremie6, lblTremie7, lblTremie8 As Label
    Private lblTapis, lblSkip, lblMalaxeur As Label
    Private lblSilo1, lblSilo2, lblSilo3, lblSilo4 As Label
    Private lblBasculeCim, lblBasculeEau, lblBasculeAdj As Label
    Private lblPompe1, lblPompe2 As Label
    Private lblAdj1, lblAdj2, lblAdj3, lblAdj4 As Label
    Private lblPoidsAGR, lblPoidsCIM, lblPoidsEAU, lblPoidsADJ As Label
    Private lblNGachee, lblModeAuto, lblEtatPLC As Label
    Private lblStatut As Label

    ' ── Bouton synchronisation manuelle ─────────────────────────
    Private WithEvents btnSynchroniser As New Button()
    Private WithEvents btnFermer As New Button()

    ' ═══════════════════════════════════════════════════════════
    '  CONSTRUCTEUR
    ' ═══════════════════════════════════════════════════════════
    Public Sub New(synoptique As Synoptique)
        _syn = synoptique
        InitForm()
    End Sub

    ' ═══════════════════════════════════════════════════════════
    '  CONSTRUCTION FORMULAIRE
    ' ═══════════════════════════════════════════════════════════
    Private Sub InitForm()
        Me.Text = "Lecture CPU — États Équipements Temps Réel"
        Me.Size = New Size(680, 620)
        Me.StartPosition = FormStartPosition.CenterScreen
        Me.FormBorderStyle = FormBorderStyle.FixedDialog
        Me.MaximizeBox = False
        Me.BackColor = Color.FromArgb(240, 242, 245)

        ' ── Titre ──────────────────────────────────────────────
        Dim lblTitre As New Label()
        lblTitre.Text = "ÉTATS ÉQUIPEMENTS — LECTURE CPU S7-1200"
        lblTitre.Location = New Point(10, 10)
        lblTitre.Size = New Size(640, 28)
        lblTitre.Font = New Font("Segoe UI", 12, FontStyle.Bold)
        lblTitre.ForeColor = Color.FromArgb(11, 61, 92)
        Me.Controls.Add(lblTitre)

        ' ── Etat PLC ───────────────────────────────────────────
        lblEtatPLC = CreerLabel("PLC : ...", 10, 42, 300)
        lblEtatPLC.Font = New Font("Segoe UI", 9)
        lblEtatPLC.ForeColor = Color.DarkOrange

        lblModeAuto = CreerLabel("MODE : ...", 320, 42, 200)
        lblModeAuto.Font = New Font("Segoe UI", 9, FontStyle.Bold)

        ' ── SECTION BASCULES (poids temps reel) ────────────────
        AjouterTitreSec("BASCULES — POIDS TEMPS REEL (DB4)", 10, 70, Color.FromArgb(11, 61, 92))

        Dim yB As Integer = 96
        lblPoidsAGR = CreerLigneCPU("AGR (Granulats)", 10, yB)
        lblPoidsCIM = CreerLigneCPU("CIM (Ciment)", 175, yB)
        lblPoidsEAU = CreerLigneCPU("EAU (Eau)", 340, yB)
        lblPoidsADJ = CreerLigneCPU("ADJ (Adjuvant)", 505, yB)

        ' ── SECTION TREMIES ────────────────────────────────────
        AjouterTitreSec("TREMIES GRANULATS (M12.0 → M12.3)", 10, 130, Color.FromArgb(160, 100, 50))
        Dim yT As Integer = 154
        lblTremie1 = CreerLigneCPU("Trémie 1", 10, yT)
        lblTremie2 = CreerLigneCPU("Trémie 2", 175, yT)
        lblTremie3 = CreerLigneCPU("Trémie 3", 340, yT)
        lblTremie4 = CreerLigneCPU("Trémie 4", 505, yT)
        Dim yT2 As Integer = 182
        lblTremie5 = CreerLigneCPU("Trémie 5", 10, yT2)
        lblTremie6 = CreerLigneCPU("Trémie 6", 175, yT2)
        lblTremie7 = CreerLigneCPU("Trémie 7", 340, yT2)
        lblTremie8 = CreerLigneCPU("Trémie 8", 505, yT2)

        ' ── SECTION CIMENTS ────────────────────────────────────
        AjouterTitreSec("CIMENTS (M13.2 / M13.3 / M13.4)", 10, 214, Color.FromArgb(80, 80, 150))
        Dim yC As Integer = 238
        lblSilo1 = CreerLigneCPU("Vis CIM 1", 10, yC)
        lblSilo2 = CreerLigneCPU("Vis CIM 2", 175, yC)
        lblSilo3 = CreerLigneCPU("Silo 3", 340, yC)
        lblSilo4 = CreerLigneCPU("Silo 4", 505, yC)
        Dim yC2 As Integer = 266
        lblBasculeCim = CreerLigneCPU("Trappe CIM", 10, yC2)

        ' ── SECTION EAUX ───────────────────────────────────────
        AjouterTitreSec("EAUX (M13.7 / M14.1)", 10, 296, Color.FromArgb(24, 95, 165))
        Dim yE As Integer = 320
        lblBasculeEau = CreerLigneCPU("Vanne EAU", 10, yE)
        lblPompe1 = CreerLigneCPU("Pompe 1", 175, yE)
        lblPompe2 = CreerLigneCPU("Pompe 2", 340, yE)

        ' ── SECTION ADJUVANTS ──────────────────────────────────
        AjouterTitreSec("ADJUVANTS (M14.3 → M14.6)", 10, 352, Color.FromArgb(133, 79, 11))
        Dim yA As Integer = 376
        lblAdj1 = CreerLigneCPU("ADJ 1", 10, yA)
        lblAdj2 = CreerLigneCPU("ADJ 2", 175, yA)
        lblAdj3 = CreerLigneCPU("ADJ 3", 340, yA)
        lblAdj4 = CreerLigneCPU("ADJ 4", 505, yA)
        lblBasculeAdj = CreerLigneCPU("Basc.ADJ", 10, 404)

        ' ── SECTION MALAXEUR / SKIP / TAPIS ────────────────────
        AjouterTitreSec("MALAXEUR / SKIP / TAPIS", 10, 432, Color.FromArgb(15, 110, 86))
        Dim yM As Integer = 456
        lblMalaxeur = CreerLigneCPU("Malaxeur", 10, yM)
        lblTapis = CreerLigneCPU("Tapis", 175, yM)
        lblSkip = CreerLigneCPU("Skip", 340, yM)
        lblNGachee = CreerLigneCPU("N Gâchée", 505, yM)

        ' ── BOUTONS ────────────────────────────────────────────
        With btnSynchroniser
            .Text = "Synchroniser Synoptique"
            .Location = New Point(10, 498)
            .Size = New Size(220, 38)
            .Font = New Font("Segoe UI", 9, FontStyle.Bold)
            .BackColor = Color.FromArgb(100, 160, 220)
            .ForeColor = Color.White
            .FlatStyle = FlatStyle.Flat
        End With
        Me.Controls.Add(btnSynchroniser)

        With btnFermer
            .Text = "Fermer"
            .Location = New Point(560, 498)
            .Size = New Size(100, 38)
            .Font = New Font("Segoe UI", 9)
            .BackColor = Color.FromArgb(220, 220, 220)
            .FlatStyle = FlatStyle.Flat
        End With
        Me.Controls.Add(btnFermer)

        lblStatut = CreerLabel("En attente de lecture CPU...", 10, 548, 640)
        lblStatut.ForeColor = Color.Gray
    End Sub

    ' ═══════════════════════════════════════════════════════════
    '  LOAD — demarrer le timer
    ' ═══════════════════════════════════════════════════════════
    Private Sub vForm_Test_Synoptique_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        If PLCInstance IsNot Nothing AndAlso PLCInstance.IsConnected Then
            lblEtatPLC.Text = "PLC CONNECTÉ — Lecture toutes les 2s"
            lblEtatPLC.ForeColor = Color.DarkGreen
            timerCPU.Interval = 2000
            timerCPU.Start()
            ' Lecture immediate
            LireEtatsDepuisCPU()
        Else
            lblEtatPLC.Text = "PLC NON CONNECTÉ"
            lblEtatPLC.ForeColor = Color.Red
            lblStatut.Text = "Connexion PLC requise pour lire les états."
        End If
    End Sub

    ' ═══════════════════════════════════════════════════════════
    '  TIMER — lecture cyclique CPU
    ' ═══════════════════════════════════════════════════════════
    Private Sub timerCPU_Tick(sender As Object, e As EventArgs) Handles timerCPU.Tick
        LireEtatsDepuisCPU()
    End Sub

    ' ═══════════════════════════════════════════════════════════
    '  LECTURE DIRECTE CPU → affichage + synoptique
    ' ═══════════════════════════════════════════════════════════
    Private Sub LireEtatsDepuisCPU()
        If PLCInstance Is Nothing OrElse Not PLCInstance.IsConnected Then Exit Sub
        Try
            Dim plc As Plc = PLCInstance

            ' ── Bascules DB4 (poids reels) ──────────────────────
            Dim agr As Single = LireDBD(plc, 1, 86)
            Dim cim As Single = LireDBD(plc, 1, 78)
            Dim eau As Single = LireDBD(plc, 1, 82)
            Dim adj As Single = LireDBD(plc, 1, 90)
            Dim nGachee As Integer = CInt(plc.Read(DataType.DataBlock, 1, 1076, VarType.Int, 1))

            AfficherPoids(lblPoidsAGR, agr, "kg", 900)
            AfficherPoids(lblPoidsCIM, cim, "kg", 131)
            AfficherPoids(lblPoidsEAU, eau, "L", 74)
            AfficherPoids(lblPoidsADJ, adj, "kg", 10)
            lblNGachee.Text = "N:" & nGachee.ToString()
            lblNGachee.ForeColor = If(nGachee > 0, Color.Blue, Color.Gray)

            ' ── Tremies (sorties ouverture casque) ──────────────
            AfficherBool(lblTremie1, CBool(ModulePLCDuplex.LireAdresse("DB1.DBX12.0")), "Tr1")
            AfficherBool(lblTremie2, CBool(ModulePLCDuplex.LireAdresse("DB1.DBX12.2")), "Tr2")
            AfficherBool(lblTremie3, CBool(ModulePLCDuplex.LireAdresse("DB1.DBX12.4")), "Tr3")
            AfficherBool(lblTremie4, CBool(ModulePLCDuplex.LireAdresse("DB1.DBX12.6")), "Tr4")
            ' Tremies 5-8 : pas de retour direct CPU — garder 0
            AfficherBool(lblTremie5, False, "Tr5")
            AfficherBool(lblTremie6, False, "Tr6")
            AfficherBool(lblTremie7, False, "Tr7")
            AfficherBool(lblTremie8, False, "Tr8")

            ' ── Ciments ─────────────────────────────────────────
            AfficherBool(lblSilo1, CBool(ModulePLCDuplex.LireAdresse("DB1.DBX18.0")), "Vis1")  ' S VIS CIM 1
            AfficherBool(lblSilo2, CBool(ModulePLCDuplex.LireAdresse("DB1.DBX18.1")), "Vis2")  ' S VIS CIM 2
            AfficherBool(lblSilo3, False, "Silo3")
            AfficherBool(lblSilo4, False, "Silo4")
            AfficherBool(lblBasculeCim, CBool(ModulePLCDuplex.LireAdresse("DB1.DBX19.4")), "TrapCIM")  ' S OV TRAP CIM

            ' ── Eaux ─────────────────────────────────────────────
            AfficherBool(lblBasculeEau, CBool(ModulePLCDuplex.LireAdresse("DB1.DBX22.2")), "VanneEAU")  ' S DOSAGE EAU
            AfficherBool(lblPompe1, False, "Pompe1")
            AfficherBool(lblPompe2, False, "Pompe2")

            ' ── Adjuvants ────────────────────────────────────────
            AfficherBool(lblAdj1, CBool(ModulePLCDuplex.LireAdresse("DB1.DBX21.0")), "ADJ1")
            AfficherBool(lblAdj2, CBool(ModulePLCDuplex.LireAdresse("DB1.DBX21.1")), "ADJ2")
            AfficherBool(lblAdj3, CBool(ModulePLCDuplex.LireAdresse("DB1.DBX21.2")), "ADJ3")
            AfficherBool(lblAdj4, CBool(ModulePLCDuplex.LireAdresse("DB1.DBX21.3")), "ADJ4")
            AfficherBool(lblBasculeAdj, CBool(ModulePLCDuplex.LireAdresse("DB1.DBX22.1")), "BascADJ")

            ' ── Malaxeur / Skip / Tapis ──────────────────────────
            Dim malaxON As Boolean = CBool(ModulePLCDuplex.LireAdresse("DB1.DBX22.5"))   ' S MALAX MARCHE
            Dim tapisON As Boolean = CBool(ModulePLCDuplex.LireAdresse("DB1.DBX16.4"))  ' S OV BASCUE AGR
            Dim skipMonte As Boolean = CBool(ModulePLCDuplex.LireAdresse("DB1.DBX17.4"))  ' s monte skip
            Dim skipDesc As Boolean = CBool(ModulePLCDuplex.LireAdresse("DB1.DBX17.5"))  ' s desc skip

            AfficherBool(lblMalaxeur, malaxON, "MLX")
            AfficherBool(lblTapis, tapisON, "Tapis")

            ' Skip — afficher direction
            If skipMonte Then
                lblSkip.Text = "Skip ↑"
                lblSkip.ForeColor = Color.Blue
            ElseIf skipDesc Then
                lblSkip.Text = "Skip ↓"
                lblSkip.ForeColor = Color.DarkOrange
            Else
                lblSkip.Text = "Skip —"
                lblSkip.ForeColor = Color.Gray
            End If

            ' ── Mode AUTO/MANU ───────────────────────────────────
            Dim modeAuto As Boolean = CBool(ModulePLCDuplex.LireAdresse("DB1.DBX0.0"))
            Dim modeManu As Boolean = CBool(ModulePLCDuplex.LireAdresse("DB1.DBX0.1"))
            lblModeAuto.Text = If(modeAuto, "MODE AUTO", If(modeManu, "MODE MANUEL", "ARRET"))
            lblModeAuto.ForeColor = If(modeAuto, Color.Blue, If(modeManu, Color.Orange, Color.Red))

            ' ── Etats pour le synoptique ─────────────────────────
            _etatsTremies = {0, If(CBool(ModulePLCDuplex.LireAdresse("DB1.DBX12.0")), 1, 0),
                                If(CBool(ModulePLCDuplex.LireAdresse("DB1.DBX12.2")), 1, 0),
                                If(CBool(ModulePLCDuplex.LireAdresse("DB1.DBX12.4")), 1, 0),
                                If(CBool(ModulePLCDuplex.LireAdresse("DB1.DBX12.6")), 1, 0),
                                0, 0, 0, 0}

            lblStatut.Text = "Dernière lecture : " & DateTime.Now.ToString("HH:mm:ss")
            lblStatut.ForeColor = Color.DarkGreen

        Catch ex As Exception
            lblStatut.Text = "Erreur lecture : " & ex.Message
            lblStatut.ForeColor = Color.Red
        End Try
    End Sub

    Private _etatsTremies() As Integer = {0, 0, 0, 0, 0, 0, 0, 0, 0}

    ' ═══════════════════════════════════════════════════════════
    '  BOUTON SYNCHRONISER → envoyer etats vers synoptique
    ' ═══════════════════════════════════════════════════════════
    Private Sub btnSynchroniser_Click(sender As Object, e As EventArgs) Handles btnSynchroniser.Click
        If _syn Is Nothing OrElse _syn.IsDisposed Then
            lblStatut.Text = "Synoptique non disponible." : Exit Sub
        End If
        Try
            Dim etats As New Dictionary(Of String, Integer)
            etats("TREMIE_1") = _etatsTremies(1)
            etats("TREMIE_2") = _etatsTremies(2)
            etats("TREMIE_3") = _etatsTremies(3)
            etats("TREMIE_4") = _etatsTremies(4)
            etats("TREMIE_5") = _etatsTremies(5)
            etats("TREMIE_6") = _etatsTremies(6)
            etats("TREMIE_7") = _etatsTremies(7)
            etats("TREMIE_8") = _etatsTremies(8)
            etats("MALAXEUR") = If(CBool(ModulePLCDuplex.LireAdresse("DB1.DBX22.5")), 1, 0)
            etats("TAPIS") = If(CBool(ModulePLCDuplex.LireAdresse("DB1.DBX16.4")), 1, 0)
            etats("SILO_1") = If(CBool(ModulePLCDuplex.LireAdresse("DB1.DBX18.0")), 1, 0)
            etats("SILO_2") = If(CBool(ModulePLCDuplex.LireAdresse("DB1.DBX18.1")), 1, 0)
            etats("BASC_CIM") = If(CBool(ModulePLCDuplex.LireAdresse("DB1.DBX19.4")), 1, 0)
            etats("BASC_EAU") = If(CBool(ModulePLCDuplex.LireAdresse("DB1.DBX22.2")), 1, 0)
            etats("ADJ_1") = If(CBool(ModulePLCDuplex.LireAdresse("DB1.DBX21.0")), 1, 0)
            etats("ADJ_2") = If(CBool(ModulePLCDuplex.LireAdresse("DB1.DBX21.1")), 1, 0)
            etats("ADJ_3") = If(CBool(ModulePLCDuplex.LireAdresse("DB1.DBX21.2")), 1, 0)
            etats("ADJ_4") = If(CBool(ModulePLCDuplex.LireAdresse("DB1.DBX21.3")), 1, 0)

            _syn.AppliquerEtatsTest(etats)
            lblStatut.Text = "Synoptique synchronise : " & DateTime.Now.ToString("HH:mm:ss")
            lblStatut.ForeColor = Color.DarkGreen
        Catch ex As Exception
            lblStatut.Text = "Erreur sync : " & ex.Message
            lblStatut.ForeColor = Color.Red
        End Try
    End Sub

    Private Sub btnFermer_Click(sender As Object, e As EventArgs) Handles btnFermer.Click
        Me.Close()
    End Sub

    ' ═══════════════════════════════════════════════════════════
    '  HELPERS
    ' ═══════════════════════════════════════════════════════════
    Private Function LireDBD(plc As Plc, db As Integer, dbd As Integer) As Single
        Try
            Return CSng(ModulePLCDuplex.LireDBDReal(db, dbd))
        Catch
            Return 0.0F
        End Try
    End Function

    Private Sub AfficherBool(lbl As Label, actif As Boolean, nom As String)
        lbl.Text = If(actif, nom & " ●", nom & " ○")
        lbl.ForeColor = If(actif, Color.DarkGreen, Color.Gray)
        lbl.BackColor = If(actif, Color.FromArgb(220, 255, 220), Color.Transparent)
    End Sub

    Private Sub AfficherPoids(lbl As Label, valeur As Single, unite As String, consigne As Single)
        lbl.Text = valeur.ToString("0.0") & " " & unite
        If valeur <= 0 Then
            lbl.ForeColor = Color.Gray
            lbl.BackColor = Color.Transparent
        ElseIf Math.Abs(consigne - valeur) <= 3 Then
            lbl.ForeColor = Color.DarkGreen
            lbl.BackColor = Color.FromArgb(220, 255, 220)
        ElseIf valeur < consigne Then
            lbl.ForeColor = Color.Blue
            lbl.BackColor = Color.FromArgb(220, 230, 255)
        Else
            lbl.ForeColor = Color.Red
            lbl.BackColor = Color.FromArgb(255, 220, 220)
        End If
    End Sub

    Private Function CreerLabel(texte As String, x As Integer, y As Integer, w As Integer) As Label
        Dim lbl As New Label()
        lbl.Text = texte
        lbl.Location = New Point(x, y)
        lbl.Size = New Size(w, 22)
        lbl.Font = New Font("Segoe UI", 9)
        lbl.AutoSize = False
        Me.Controls.Add(lbl)
        Return lbl
    End Function

    Private Function CreerLigneCPU(nom As String, x As Integer, y As Integer) As Label
        ' Label nom
        Dim lblNom As New Label()
        lblNom.Text = nom & " :"
        lblNom.Location = New Point(x, y)
        lblNom.Size = New Size(100, 20)
        lblNom.Font = New Font("Segoe UI", 8)
        lblNom.ForeColor = Color.FromArgb(80, 80, 80)
        Me.Controls.Add(lblNom)
        ' Label valeur
        Dim lblVal As New Label()
        lblVal.Text = "..."
        lblVal.Location = New Point(x + 102, y)
        lblVal.Size = New Size(60, 20)
        lblVal.Font = New Font("Segoe UI", 8, FontStyle.Bold)
        lblVal.ForeColor = Color.Gray
        Me.Controls.Add(lblVal)
        Return lblVal
    End Function

    Private Sub AjouterTitreSec(texte As String, x As Integer, y As Integer, couleur As Color)
        Dim lbl As New Label()
        lbl.Text = texte
        lbl.Location = New Point(x, y)
        lbl.Size = New Size(650, 20)
        lbl.Font = New Font("Segoe UI", 8, FontStyle.Bold)
        lbl.ForeColor = Color.White
        lbl.BackColor = couleur
        lbl.Padding = New Padding(4, 2, 0, 0)
        Me.Controls.Add(lbl)
    End Sub

    ' ═══════════════════════════════════════════════════════════
    '  FERMETURE
    ' ═══════════════════════════════════════════════════════════
    Private Sub vForm_Test_Synoptique_FormClosing(sender As Object,
            e As FormClosingEventArgs) Handles MyBase.FormClosing
        timerCPU.Stop()
    End Sub

End Class