Imports S7.Net

' ================================================================
'  Dosage_manuel.vb — MARCHE FORCEE SIMPLIFIEE
'  Seulement : 5 boutons AUTO/FORCAGE + TOUS + ANNULATION + voyants
'  Les commandes se font par clic gauche dans le Synoptique
'  Date : 19/06/2026
' ================================================================
Public Class Dosage_manuel

    ' ═══ MODES FORCAGE (Shared → accessibles depuis Synoptique) ════
    Public Shared ModeForceGranulats As Boolean = False
    Public Shared ModeForceCiments As Boolean = False
    Public Shared ModeForceEaux As Boolean = False
    Public Shared ModeForceAdjuvants As Boolean = False
    Public Shared ModeForceMalaxeurs As Boolean = False

    ' ═══ CONTROLES ══════════════════════════════════════════════════
    Private WithEvents timerEtat As New Timer()
    Private WithEvents btnForceGR As New Button()
    Private WithEvents btnForceCIM As New Button()
    Private WithEvents btnForceEAU As New Button()
    Private WithEvents btnForceADJ As New Button()
    Private WithEvents btnForceMLX As New Button()
    Private WithEvents btnTousForcages As New Button()
    Private WithEvents btnAnnulation As New Button()

    ' Voyants etat CPU
    Private voyantMalax As New Label()
    Private voyantVis1 As New Label()
    Private voyantVis2 As New Label()
    Private voyantTrapCIM As New Label()
    Private voyantTrapEAU As New Label()
    Private voyantTrapMLX As New Label()
    Private voyantSkip As New Label()
    Private voyantDefaut As New Label()

    ' Labels info
    Private lblMode As New Label()
    Private lblEtat As New Label()

    ' ═══ LOAD ════════════════════════════════════════════════════════
    Private Sub Dosage_manuel_Load(s As Object, e As EventArgs) Handles MyBase.Load
        Me.Text = "MARCHE FORCEE"
        Me.Size = New Size(620, 195)
        Me.BackColor = Color.FromArgb(240, 242, 245)
        Me.FormBorderStyle = FormBorderStyle.FixedDialog
        Me.MaximizeBox = False
        ' Label21.Text = "MARCHE FORCEE — COMMANDE MANUELLE"
        'Label21.Font = New Font("Segoe UI", 13, FontStyle.Bold)
        ' Label21.ForeColor = Color.FromArgb(11, 61, 92)
        ' Label21.Size = New Size(640, 30)
        ConstruireInterface()
        timerEtat.Interval = 500
        timerEtat.Start()
    End Sub

    ' ═══ CONSTRUCTION INTERFACE ══════════════════════════════════════
    Private Sub ConstruireInterface()

        ' ── Info mode PLC ──────────────────────────────────────────
        '  lblEtat.Location = New Point(10, 45)
        '   lblEtat.Size = New Size(300, 22)
        '  lblEtat.Font = New Font("Segoe UI", 9)
        '' lblEtat.Padding = New Padding(4)
        '  lblEtat.Text = If(PLCInstance?.IsConnected, "PLC CONNECTE", "PLC NON CONNECTE")
        ' lblEtat.ForeColor = If(PLCInstance?.IsConnected, Color.DarkGreen, Color.Red)
        ' lblEtat.BackColor = If(PLCInstance?.IsConnected, Color.FromArgb(220, 255, 220), Color.FromArgb(255, 220, 220))
        ' Me.Controls.Add(lblEtat)

        ' lblMode.Location = New Point(320, 45)
        ' lblMode.Size = New Size(200, 22)
        ' lblMode.Font = New Font("Segoe UI", 9, FontStyle.Bold)
        ' lblMode.Text = "MODE : ..."
        ' Me.Controls.Add(lblMode)

        ' ── 5 boutons AUTO/FORCAGE ─────────────────────────────────
        Dim y0 As Integer = 5
        CreerBtnForce(btnForceGR, "AUTO GRANULATS", 10, y0)

        CreerBtnForce(btnForceCIM, "AUTO CIMENTS", 130, y0)
        CreerBtnForce(btnForceEAU, "AUTO EAUX", 250, y0)
        CreerBtnForce(btnForceADJ, "AUTO ADJUVANTS", 370, y0)
        CreerBtnForce(btnForceMLX, "AUTO MALAXEURS", 490, y0)

        ' ── Explication ────────────────────────────────────────────
        ' Dim lblInfo As New Label()
        ' lblInfo.Text = "Activez un groupe, puis cliquez directement sur l element dans le Synoptique"
        '  lblInfo.Location = New Point(10, 135)
        '  lblInfo.Size = New Size(650, 22)
        ' lblInfo.Font = New Font("Segoe UI", 9, FontStyle.Italic)
        ' lblInfo.ForeColor = Color.FromArgb(80, 80, 80)
        ' Me.Controls.Add(lblInfo)

        ' ── Voyants etat CPU ───────────────────────────────────────
        Dim yV As Integer = 55
        '  Me.Controls.Add(CreerTitre("ETATS CPU TEMPS REEL", 10, yV - 22))
        CreerVoyant(voyantVis1, "Vis CIM 1", 15, yV)
        CreerVoyant(voyantVis2, "Vis CIM 2", 88, yV)
        CreerVoyant(voyantTrapCIM, "Trap.CIM", 161, yV)
        CreerVoyant(voyantTrapEAU, "Trap.EAU", 234, yV)
        CreerVoyant(voyantTrapMLX, "Trap.MLX", 307, yV)
        CreerVoyant(voyantMalax, "Malaxeur", 380, yV)
        CreerVoyant(voyantSkip, "Skip", 453, yV)
        CreerVoyant(voyantDefaut, "DEFAUT", 526, yV)

        ' ── Boutons globaux ────────────────────────────────────────
        Dim yG As Integer = 100
        ' 21/09 : les deux boutons centrés dans la page (180 + 10 + 200 = 390 px)
        Dim xG As Integer = Math.Max(0, (Me.ClientSize.Width - 390) \ 2)
        With btnTousForcages
            .Text = "TOUS LES FORÇAGES"
            .Location = New Point(xG, yG)
            .Size = New Size(180, 45)
            .Font = New Font("Segoe UI", 10, FontStyle.Bold)
            .BackColor = Color.FromArgb(255, 200, 100)
            .FlatStyle = FlatStyle.Flat
        End With
        Me.Controls.Add(btnTousForcages)

        With btnAnnulation
            .Text = "ANNULATION FORÇAGES"
            .Location = New Point(xG + 190, yG)
            .Size = New Size(200, 45)
            .Font = New Font("Segoe UI", 10, FontStyle.Bold)
            .BackColor = Color.FromArgb(255, 150, 150)
            .FlatStyle = FlatStyle.Flat
        End With
        Me.Controls.Add(btnAnnulation)



    End Sub

    ' ═══ HELPERS CREATION CONTROLES ══════════════════════════════════
    Private Sub CreerBtnForce(btn As Button, texte As String, x As Integer, y As Integer)
        btn.Text = texte
        btn.Location = New Point(x, y)
        btn.Size = New Size(110, 45)
        btn.Font = New Font("Segoe UI", 8, FontStyle.Bold)
        btn.BackColor = Color.FromArgb(200, 230, 200)
        btn.ForeColor = Color.DarkGreen
        btn.FlatStyle = FlatStyle.Flat
        btn.FlatAppearance.BorderSize = 2
        Me.Controls.Add(btn)
    End Sub

    Private Sub CreerVoyant(lbl As Label, nom As String, x As Integer, y As Integer)
        lbl.Text = nom
        lbl.Location = New Point(x, y)
        lbl.Size = New Size(70, 40)
        lbl.TextAlign = ContentAlignment.MiddleCenter
        lbl.Font = New Font("Segoe UI", 7.5, FontStyle.Bold)
        lbl.BackColor = Color.FromArgb(200, 200, 200)
        lbl.ForeColor = Color.Gray
        lbl.BorderStyle = BorderStyle.Fixed3D
        Me.Controls.Add(lbl)
    End Sub

    Private Function CreerTitre(texte As String, x As Integer, y As Integer) As Label
        Dim lbl As New Label()
        lbl.Text = texte
        lbl.Location = New Point(x, y)
        lbl.Size = New Size(650, 18)
        lbl.Font = New Font("Segoe UI", 8, FontStyle.Bold)
        lbl.ForeColor = Color.White
        lbl.BackColor = Color.FromArgb(11, 61, 92)
        lbl.Padding = New Padding(3, 1, 0, 0)
        Return lbl
    End Function


    ' ═══ TIMER LECTURE ETAT CPU ══════════════════════════════════════
    Private Sub timerEtat_Tick(s As Object, e As EventArgs) Handles timerEtat.Tick
        ' Passe par le module thread-safe → plus de conflit avec le Synoptique
        Try
            SetVoyant(voyantVis1, CBool(ModulePLCDuplex.LireAdresse("DB1.DBX18.0")), "Vis CIM 1")
            SetVoyant(voyantVis2, CBool(ModulePLCDuplex.LireAdresse("DB1.DBX18.1")), "Vis CIM 2")
            SetVoyant(voyantTrapCIM, CBool(ModulePLCDuplex.LireAdresse("DB1.DBX19.4")), "Trap.CIM")
            SetVoyant(voyantTrapEAU, CBool(ModulePLCDuplex.LireAdresse("DB1.DBX22.2")), "Trap.EAU")
            SetVoyant(voyantTrapMLX, CBool(ModulePLCDuplex.LireAdresse("DB1.DBX23.0")), "Trap.MLX")
            SetVoyant(voyantMalax, CBool(ModulePLCDuplex.LireAdresse("DB1.DBX22.5")), "Malaxeur")

            Dim skipON As Boolean = CBool(ModulePLCDuplex.LireAdresse("DB1.DBX17.4")) _
                             OrElse CBool(ModulePLCDuplex.LireAdresse("DB1.DBX17.5"))
            SetVoyant(voyantSkip, skipON, "Skip")

            SetVoyant(voyantDefaut, CBool(ModulePLCDuplex.LireAdresse("DB1.DBX1082.0")), "DEFAUT")

            Dim modeAuto As Boolean = CBool(ModulePLCDuplex.LireAdresse("DB1.DBX0.0"))
            lblMode.Text = "MODE : " & If(modeAuto, "AUTO", "MANUEL")
            lblMode.ForeColor = If(modeAuto, Color.Blue, Color.Orange)
        Catch
        End Try
    End Sub

    Private Sub SetVoyant(lbl As Label, actif As Boolean, nom As String)
        lbl.Text = nom
        lbl.BackColor = If(actif, Color.FromArgb(100, 220, 100), Color.FromArgb(200, 200, 200))
        lbl.ForeColor = If(actif, Color.DarkGreen, Color.Gray)
    End Sub

    ' ═══ BOUTONS FORCAGE ══════════════════════════════════════════════
    Private Sub btnForceGR_Click(s As Object, e As EventArgs) Handles btnForceGR.Click
        ModeForceGranulats = Not ModeForceGranulats
        MAJBtnForce(btnForceGR, ModeForceGranulats, "GRANULATS")
        MAJBoutonsSynoptique()
        Log(If(ModeForceGranulats, "FORCAGE GRANULATS active", "GRANULATS -> AUTO"))
    End Sub
    Private Sub btnForceCIM_Click(s As Object, e As EventArgs) Handles btnForceCIM.Click
        ModeForceCiments = Not ModeForceCiments
        MAJBtnForce(btnForceCIM, ModeForceCiments, "CIMENTS")
        MAJBoutonsSynoptique()
        Log(If(ModeForceCiments, "FORCAGE CIMENTS active", "CIMENTS -> AUTO"))
    End Sub
    Private Sub btnForceEAU_Click(s As Object, e As EventArgs) Handles btnForceEAU.Click
        ModeForceEaux = Not ModeForceEaux
        MAJBtnForce(btnForceEAU, ModeForceEaux, "EAUX")
        MAJBoutonsSynoptique()
        Log(If(ModeForceEaux, "FORCAGE EAUX active", "EAUX -> AUTO"))
    End Sub
    Private Sub btnForceADJ_Click(s As Object, e As EventArgs) Handles btnForceADJ.Click
        ModeForceAdjuvants = Not ModeForceAdjuvants
        MAJBtnForce(btnForceADJ, ModeForceAdjuvants, "ADJUVANTS")
        MAJBoutonsSynoptique()
        Log(If(ModeForceAdjuvants, "FORCAGE ADJUVANTS active", "ADJUVANTS -> AUTO"))
    End Sub
    Private Sub btnForceMLX_Click(s As Object, e As EventArgs) Handles btnForceMLX.Click
        ModeForceMalaxeurs = Not ModeForceMalaxeurs
        MAJBtnForce(btnForceMLX, ModeForceMalaxeurs, "MALAXEURS")
        MAJBoutonsSynoptique()
        Log(If(ModeForceMalaxeurs, "FORCAGE MALAXEURS active", "MALAXEURS -> AUTO"))
    End Sub

    Private Sub MAJBtnForce(btn As Button, force As Boolean, nom As String)
        btn.Text = If(force, "FORCAGE " & nom, "AUTO " & nom)
        btn.BackColor = If(force, Color.FromArgb(255, 200, 100), Color.FromArgb(200, 230, 200))
        btn.ForeColor = If(force, Color.DarkBlue, Color.DarkGreen)
        btn.FlatAppearance.BorderColor = If(force, Color.Gold, Color.Green)
    End Sub

    ' ═══ TOUS FORCAGES / ANNULATION / ABANDON ═════════════════════════
    Private Sub btnTousForcages_Click(s As Object, e As EventArgs) Handles btnTousForcages.Click
        If MessageBox.Show("Activer TOUS les forçages ?", "Confirmation",
            MessageBoxButtons.YesNo, MessageBoxIcon.Warning) = DialogResult.No Then Exit Sub
        ModeForceGranulats = True : MAJBtnForce(btnForceGR, True, "GRANULATS")
        ModeForceCiments = True : MAJBtnForce(btnForceCIM, True, "CIMENTS")
        ModeForceEaux = True : MAJBtnForce(btnForceEAU, True, "EAUX")
        ModeForceAdjuvants = True : MAJBtnForce(btnForceADJ, True, "ADJUVANTS")
        ModeForceMalaxeurs = True : MAJBtnForce(btnForceMLX, True, "MALAXEURS")
        MAJBoutonsSynoptique()
        MAJBoutonsSynoptique()
        Log("TOUS LES FORCAGES actives")
        Synoptique.EnregistrerAction("FORCAGE", "TOUS LES FORÇAGES actives", "", "", "", "FORCAGE")
    End Sub

    Private Sub btnAnnulation_Click(s As Object, e As EventArgs) Handles btnAnnulation.Click
        ModeForceGranulats = False : MAJBtnForce(btnForceGR, False, "GRANULATS")
        ModeForceCiments = False : MAJBtnForce(btnForceCIM, False, "CIMENTS")
        ModeForceEaux = False : MAJBtnForce(btnForceEAU, False, "EAUX")
        ModeForceAdjuvants = False : MAJBtnForce(btnForceADJ, False, "ADJUVANTS")
        ModeForceMalaxeurs = False : MAJBtnForce(btnForceMLX, False, "MALAXEURS")
        ' Arreter tout
        If PLCInstance?.IsConnected Then
            ModulePLCDuplex.EcrireBit("DB1.DBX5.1", CBool(False)) : ModulePLCDuplex.EcrireBit("DB1.DBX5.2", CBool(False))
            ModulePLCDuplex.EcrireBit("M30.4", CBool(False)) : ModulePLCDuplex.EcrireBit("DB1.DBX9.3", CBool(False))
            ModulePLCDuplex.EcrireBit("DB1.DBX1088.2", CBool(False))
        End If
        MAJBoutonsSynoptique()
        MAJBoutonsSynoptique()
        Log("ANNULATION TOUS FORCAGES")
        Synoptique.EnregistrerAction("FORCAGE", "ANNULATION TOUS FORÇAGES", "", "", "", "FORCAGE")
    End Sub

    ' 21/09 : bouton ABANDON DE CYCLE supprimé (fin de cycle gérée par le Synoptique : 914.2 / 1088.0 / 914.3)


    ' Appel direct vers Synoptique — instantane, pas de Timer
    ' Reference directe vers le Synoptique
    Public Shared RefSynoptique As Synoptique = Nothing

    Private Sub MAJBoutonsSynoptique()
        If RefSynoptique IsNot Nothing AndAlso Not RefSynoptique.IsDisposed Then
            RefSynoptique.MettreAJourVisibiliteBoutons()
        End If
    End Sub


    ' ═══ METHODES SHARED (appelees depuis Synoptique) ════════════════
    Public Shared Sub CommanderTremie(num As Integer, ouvrir As Boolean)
        If Not ModeForceGranulats Then Exit Sub
        Dim adresses() As String = {"DB1.DBX0.5", "DB1.DBX0.6", "DB1.DBX0.7", "DB1.DBX3.5", "DB1.DBX0.5", "DB1.DBX0.6", "DB1.DBX0.7", "DB1.DBX3.5"}
        If num >= 1 AndAlso num <= 8 Then EcrirePLC(adresses(num - 1), ouvrir)
    End Sub
    Public Shared Sub CommanderVisCiment(num As Integer, marche As Boolean)
        If Not ModeForceCiments Then Exit Sub
        EcrirePLC(If(num = 1, "DB1.DBX5.1", "DB1.DBX5.2"), marche)
    End Sub
    Public Shared Sub CommanderBasculeCiment(ouvrir As Boolean)
        If Not ModeForceCiments Then Exit Sub
        EcrirePLC("DB1.DBX6.5", ouvrir)
    End Sub
    Public Shared Sub CommanderVanneEau(ouvrir As Boolean)
        If Not ModeForceEaux Then Exit Sub
        EcrirePLC("DB1.DBX9.3", ouvrir)
    End Sub
    Public Shared Sub CommanderBasculeEau(ouvrir As Boolean)
        If Not ModeForceEaux Then Exit Sub
        EcrirePLC("DB1.DBX9.5", ouvrir)
    End Sub
    Public Shared Sub CommanderAdjuvant(num As Integer, ouvrir As Boolean)
        If Not ModeForceAdjuvants Then Exit Sub
        Dim adresses() As String = {"DB1.DBX8.1", "DB1.DBX8.2", "DB1.DBX8.3", "DB1.DBX8.4"}
        If num >= 1 AndAlso num <= 4 Then EcrirePLC(adresses(num - 1), ouvrir)
    End Sub
    Public Shared Sub CommanderMalaxeur(demarrer As Boolean)
        If Not ModeForceMalaxeurs Then Exit Sub
        EcrirePLC("DB1.DBX1088.2", demarrer)
    End Sub
    Public Shared Sub CommanderTapisPeseur(marche As Boolean)
        If Not ModeForceGranulats Then Exit Sub
        EcrirePLC("M30.4", marche)
    End Sub
    Public Shared Sub CommanderSkip(monter As Boolean)
        If Not ModeForceGranulats Then Exit Sub
        ' Couper le sens opposé puis impulsion 300 ms sans geler l'écran
        EcrirePLC(If(monter, "DB1.DBX4.6", "DB1.DBX4.5"), False)
        ModulePLCDuplex.EcrireImpulsion(If(monter, "DB1.DBX4.5", "DB1.DBX4.6"), 300)
    End Sub

    ' ═══ HELPER PLC ══════════════════════════════════════════════════
    Private Shared Sub EcrirePLC(adresse As String, valeur As Boolean)
        Try
            If PLCInstance IsNot Nothing AndAlso PLCInstance.IsConnected Then
                PLCInstance.Write(adresse, valeur)
            End If
        Catch ex As Exception
            Debug.WriteLine("PLC " & adresse & " : " & ex.Message)
        End Try
    End Sub
    ' Lecture booléenne sûre — jamais d'erreur même si PLC occupé
    Private Function LireBit(adresse As String) As Boolean
        Dim v As Object = ModulePLCDuplex.LireAdresse(adresse)
        If v Is Nothing Then Return False
        Return CBool(v)
    End Function
    ' ═══ LOG ══════════════════════════════════════════════════════════
    Private Sub Log(msg As String)
        ' Ecriture fichier log uniquement (lstLog supprime)
        Try
            System.IO.File.AppendAllText("marche_forcee.log",
                DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") & " | " & NomUtilisateur & " | " & msg & Environment.NewLine)
        Catch : End Try
        Debug.WriteLine(DateTime.Now.ToString("HH:mm:ss") & " > " & msg)
    End Sub

    ' ═══ FERMETURE ════════════════════════════════════════════════════
    Private Sub Dosage_manuel_FormClosing(s As Object, e As FormClosingEventArgs) Handles MyBase.FormClosing
        timerEtat.Stop()
        ' Desactiver tous les forcages a la fermeture
        ModeForceGranulats = False
        ModeForceCiments = False
        ModeForceEaux = False
        ModeForceAdjuvants = False
        ModeForceMalaxeurs = False
        ' Cacher tous les boutons dans le Synoptique
        MAJBoutonsSynoptique()
        ' Arreter tous les sorties PLC actives
        If PLCInstance IsNot Nothing AndAlso PLCInstance.IsConnected Then
            ModulePLCDuplex.EcrireBit("DB1.DBX5.1", CBool(False)) : ModulePLCDuplex.EcrireBit("DB1.DBX5.2", CBool(False))
            ModulePLCDuplex.EcrireBit("M30.4", CBool(False)) : ModulePLCDuplex.EcrireBit("DB1.DBX9.3", CBool(False))
            ModulePLCDuplex.EcrireBit("DB1.DBX1088.2", CBool(False))
        End If
        Log("Fermeture Marche Forcee — tous forcages desactives")
    End Sub

End Class