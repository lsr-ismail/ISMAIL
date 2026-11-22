Imports Microsoft.Data.SqlClient
Imports S7.Net

Public Class Assistance___PLC

    Private plc As Plc
    Private WithEvents timerLecture As New Timer
    Private _connecte As Boolean = False
    Private _derniereAlarme As DateTime = DateTime.MinValue

    Public Property RefSynoptique As Synoptique = Nothing

    ' ═══════════════════════════════════════════════════════════════════
    '  LOAD
    ' ═══════════════════════════════════════════════════════════════════

    Private Sub Assistance___PLC_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        txtIP.Text = "192.168.0.1"
        txtRack.Text = "0"
        txtSlot.Text = "1"
        timerLecture.Interval = 1000
        btnDeconnecter.Enabled = False
        btnDepartCycle.Enabled = False
        btnArretUrgence.Enabled = False
        lblEtat.Text = "⛔ Déconnecté"
        lblEtat.ForeColor = Color.Red
        lblEtat.BackColor = Color.FromArgb(255, 220, 220)
    End Sub

    ' ═══════════════════════════════════════════════════════════════════
    '  CONNEXION
    ' ═══════════════════════════════════════════════════════════════════

    Private Sub btnConnecter_Click(sender As Object, e As EventArgs) Handles btnConnecter.Click
        Try
            Me.Cursor = Cursors.WaitCursor
            lblEtat.Text = "⏳ Connexion en cours..."
            lblEtat.ForeColor = Color.Orange
            Application.DoEvents()

            ' Utiliser PLCInstance de Module1 au lieu de créer une nouvelle connexion
            If Module1.PLCInstance IsNot Nothing AndAlso Module1.PLCInstance.IsConnected Then
                ' Réutiliser la connexion existante
                plc = Module1.PLCInstance
            Else
                ' Créer via Module1
                Module1.PLC_IP = txtIP.Text.Trim()
                Module1.PLC_Rack = CShort(txtRack.Text.Trim())
                Module1.PLC_Slot = CShort(txtSlot.Text.Trim())
                ConnecterPLCGlobal()
                plc = Module1.PLCInstance
            End If

            If plc IsNot Nothing AndAlso plc.IsConnected Then
                _connecte = True
                lblEtat.Text = "✅ Connecté — S7-1200"
                lblEtat.ForeColor = Color.Green
                lblEtat.BackColor = Color.FromArgb(220, 255, 220)
                btnConnecter.Enabled = False
                btnDeconnecter.Enabled = True
                btnDepartCycle.Enabled = True
                btnArretUrgence.Enabled = True
                timerLecture.Start()
                LogMessage("✅ PLC connecté : " & Module1.PLC_IP)
            Else
                lblEtat.Text = "❌ Connexion échouée"
                lblEtat.ForeColor = Color.Red
                lblEtat.BackColor = Color.FromArgb(255, 220, 220)
                MessageBox.Show("Connexion échouée !" & vbCrLf &
                "Vérifiez IP, câble réseau et alimentation PLC.",
                "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            End If

        Catch ex As Exception
            lblEtat.Text = "❌ Erreur"
            lblEtat.ForeColor = Color.Red
            MessageBox.Show("Erreur : " & ex.Message)
        Finally
            Me.Cursor = Cursors.Default
        End Try
    End Sub

    ' ═══════════════════════════════════════════════════════════════════
    '  DÉCONNEXION
    ' ═══════════════════════════════════════════════════════════════════

    Private Sub btnDeconnecter_Click(sender As Object, e As EventArgs) Handles btnDeconnecter.Click
        Try
            timerLecture.Stop()
            _connecte = False
            plc = Nothing   ' ← juste enlever la référence, PAS fermer
            ' NE PAS fermer Module1.PLCInstance — Synoptique l'utilise encore !
            lblEtat.Text = "⛔ Déconnecté"
            lblEtat.ForeColor = Color.Red
            lblEtat.BackColor = Color.FromArgb(255, 220, 220)
            btnConnecter.Enabled = True
            btnDeconnecter.Enabled = False
            btnDepartCycle.Enabled = False
            btnArretUrgence.Enabled = False
            LogMessage("⛔ Assistance PLC déconnectée")
        Catch ex As Exception
            MessageBox.Show("Erreur déconnexion : " & ex.Message)
        End Try
    End Sub

    ' ═══════════════════════════════════════════════════════════════════
    '  TIMER — LECTURE AUTOMATIQUE
    ' ═══════════════════════════════════════════════════════════════════

    Private Sub timerLecture_Tick(sender As Object, e As EventArgs) Handles timerLecture.Tick
        If plc Is Nothing OrElse Not plc.IsConnected Then
            timerLecture.Stop()
            _connecte = False
            lblEtat.Text = "❌ Connexion perdue"
            lblEtat.ForeColor = Color.Red
            btnConnecter.Enabled = True
            btnDeconnecter.Enabled = False
            btnDepartCycle.Enabled = False
            btnArretUrgence.Enabled = False
            LogMessage("❌ Connexion PLC perdue!")
            Exit Sub
        End If
        Try
            Dim malaxeurMarche As Boolean = CBool(ModulePLCDuplex.LireAdresse("M0.0"))
            lblMalaxeur.Text = If(malaxeurMarche, "🟢 EN MARCHE", "🔴 ARRÊT")
            lblMalaxeur.ForeColor = If(malaxeurMarche, Color.Green, Color.Red)

            Dim porteOuverte As Boolean = CBool(ModulePLCDuplex.LireAdresse("M0.1"))
            lblPorte.Text = If(porteOuverte, "🟡 OUVERTE", "🟢 FERMÉE")
            lblPorte.ForeColor = If(porteOuverte, Color.Orange, Color.Green)

            Dim tapisMarche As Boolean = CBool(ModulePLCDuplex.LireAdresse("M0.2"))
            lblTapis.Text = If(tapisMarche, "🟢 EN MARCHE", "🔴 ARRÊT")
            lblTapis.ForeColor = If(tapisMarche, Color.Green, Color.Red)

            Dim poids As Integer = CInt(plc.Read("MW10"))
            txtPoidsActuel.Text = poids.ToString("0") & " kg"
            txtPoidsActuel.BackColor = If(poids > 0, Color.FromArgb(220, 255, 220), Color.White)

            txtNiveauEau.Text = CInt(plc.Read("MW12")).ToString("0") & " L"
            txtNbGachees.Text = CInt(plc.Read("MW20")).ToString()

            Dim alarme As Boolean = CBool(ModulePLCDuplex.LireAdresse("M1.0"))
            If alarme Then
                lblAlarme.Text = "⚠️ ALARME ACTIVE"
                lblAlarme.ForeColor = Color.Red
                lblAlarme.BackColor = Color.Yellow
                EnregistrerAlarme("Alarme générale PLC")
            Else
                lblAlarme.Text = "✅ Pas d'alarme"
                lblAlarme.ForeColor = Color.Green
                lblAlarme.BackColor = Color.Transparent
            End If

            Dim cycleEnCours As Boolean = CBool(ModulePLCDuplex.LireAdresse("M2.0"))
            lblCycle.Text = If(cycleEnCours, "⚙️ CYCLE EN COURS", "⏸ EN ATTENTE")
            lblCycle.ForeColor = If(cycleEnCours, Color.Blue, Color.Gray)

            Dim bascAGR As Single = LireDBD(plc, 1, 86)
            Dim bascCIM As Single = LireDBD(plc, 1, 78)
            Dim bascEAU As Single = LireDBD(plc, 1, 82)
            Dim bascADJ As Single = LireDBD(plc, 1, 90)

            AfficherBascule(lblBascAGR, bascAGR, "kg")
            AfficherBascule(lblBascCIM, bascCIM, "kg")
            AfficherBascule(lblBascEAU, bascEAU, "L")
            AfficherBascule(lblBascADJ, bascADJ, "kg")

            If RefSynoptique IsNot Nothing AndAlso Not RefSynoptique.IsDisposed Then
                RefSynoptique.MettreAJourBascules(bascAGR, 0, bascCIM, 0, bascEAU, 0)
            End If

            lblDerniereMAJ.Text = "MAJ: " & DateTime.Now.ToString("HH:mm:ss")

        Catch
            ' Silencieux
        End Try
    End Sub

    ' ═══════════════════════════════════════════════════════════════════
    '  LECTURE DBD (REAL)
    ' ═══════════════════════════════════════════════════════════════════

    Private Function LireDBD(plc As Plc, db As Integer, dbd As Integer) As Single
        Try
            Dim result As Object = ModulePLCDuplex.LireDBDReal(db, dbd)
            If result IsNot Nothing Then Return CSng(result)
        Catch
        End Try
        Return 0.0F
    End Function

    Private Sub AfficherBascule(lbl As Label, valeur As Single, unite As String)
        lbl.Text = valeur.ToString("0.0") & " " & unite
        If valeur > 0 Then
            lbl.ForeColor = Color.Green
            lbl.BackColor = Color.FromArgb(220, 255, 220)
        Else
            lbl.ForeColor = Color.Gray
            lbl.BackColor = Color.Transparent
        End If
    End Sub

    ' ═══════════════════════════════════════════════════════════════════
    '  DÉPART CYCLE / ARRÊT URGENCE
    ' ═══════════════════════════════════════════════════════════════════

    Private Sub btnDepartCycle_Click(sender As Object, e As EventArgs) Handles btnDepartCycle.Click

        ModulePLCDuplex.EcrireBit("DB1.DBX0.5", CBool(1))


    End Sub

    Private Sub btnArretUrgence_Click(sender As Object, e As EventArgs) Handles btnArretUrgence.Click
        ModulePLCDuplex.EcrireBit("DB1.DBX0.5", CBool(0))
    End Sub

    ' ═══════════════════════════════════════════════════════════════════
    '  LECTURE / ÉCRITURE MANUELLES
    ' ═══════════════════════════════════════════════════════════════════

    Private Sub btnLire_Click(sender As Object, e As EventArgs) Handles btnLire.Click
        If Not _connecte Then MessageBox.Show("PLC non connecté.") : Exit Sub
        If txtAdresse.Text.Trim = "" Then MessageBox.Show("Entrer une adresse PLC.") : Exit Sub
        Try
            Dim valeur As Object = plc.Read(txtAdresse.Text.Trim)
            txtValeurLue.Text = valeur.ToString()
            LogMessage("📖 Lecture " & txtAdresse.Text & " = " & valeur.ToString())
        Catch ex As Exception
            MessageBox.Show("Erreur lecture : " & ex.Message)
        End Try
    End Sub

    Private Sub btnEcrire_Click(sender As Object, e As EventArgs) Handles btnEcrire.Click
        If Not _connecte OrElse txtAdresse.Text.Trim = "" OrElse txtValeurEcrire.Text.Trim = "" Then
            MessageBox.Show("PLC non connecté ou champs vides.") : Exit Sub
        End If
        Try
            Dim adresse As String = txtAdresse.Text.Trim
            Dim valeur As String = txtValeurEcrire.Text.Trim.ToUpper
            If valeur = "TRUE" OrElse valeur = "1" OrElse valeur = "OUI" Then
                plc.Write(adresse, True)
            ElseIf valeur = "FALSE" OrElse valeur = "0" OrElse valeur = "NON" Then
                plc.Write(adresse, False)
            Else
                Dim n As Integer
                If Integer.TryParse(valeur, n) Then plc.Write(adresse, CShort(n))
            End If
            LogMessage("✏️ Écriture " & adresse & " = " & valeur)
            MessageBox.Show("✅ Écriture OK → " & adresse & " = " & valeur)
        Catch ex As Exception
            MessageBox.Show("Erreur écriture : " & ex.Message)
        End Try
    End Sub

    ' ═══════════════════════════════════════════════════════════════════
    '  RESET ALARME
    ' ═══════════════════════════════════════════════════════════════════

    Private Sub btnResetAlarme_Click(sender As Object, e As EventArgs) Handles btnResetAlarme.Click
        If Not _connecte Then Exit Sub
        Try
            ModulePLCDuplex.EcrireBit("M1.0", CBool(False))
            ModulePLCDuplex.EcrireBit("DB1.DBX22.6", CBool(False))
            timerLecture.Start()
            lblEtat.Text = "✅ Connecté — S7-1200"
            lblEtat.ForeColor = Color.Green
            lblEtat.BackColor = Color.FromArgb(220, 255, 220)
            LogMessage("🔄 Alarme réinitialisée")
            MessageBox.Show("Alarme réinitialisée.")
        Catch ex As Exception
            MessageBox.Show("Erreur reset : " & ex.Message)
        End Try
    End Sub

    ' ═══════════════════════════════════════════════════════════════════
    '  ENREGISTRER ALARME — ModuleDb
    ' ═══════════════════════════════════════════════════════════════════

    Private Sub EnregistrerAlarme(message As String)
        If DateTime.Now.Subtract(_derniereAlarme).TotalSeconds < 60 Then Exit Sub
        _derniereAlarme = DateTime.Now
        Try
            ModuleDb.Executer(
                "INSERT INTO dbo.Alarmes (DateAlarme, TypeAlarme, Description, Operateur) " &
                "VALUES (GETDATE(), 'PLC', @Description, @Operateur)",
                Sub(cmd)
                    cmd.Parameters.Add("@Description", SqlDbType.NVarChar, 300).Value = message
                    cmd.Parameters.Add("@Operateur", SqlDbType.NVarChar, 100).Value = NomUtilisateur
                End Sub)
        Catch
            ' Silencieux
        End Try
    End Sub

    ' ═══════════════════════════════════════════════════════════════════
    '  LOG / FERMETURE
    ' ═══════════════════════════════════════════════════════════════════

    Private Sub LogMessage(msg As String)
        Dim ligne As String = DateTime.Now.ToString("HH:mm:ss") & " → " & msg
        lstLog.Items.Insert(0, ligne)
        If lstLog.Items.Count > 100 Then lstLog.Items.RemoveAt(lstLog.Items.Count - 1)
    End Sub

    Private Sub Assistance___PLC_FormClosing(sender As Object,
            e As FormClosingEventArgs) Handles MyBase.FormClosing
        timerLecture.Stop()
        If plc IsNot Nothing AndAlso plc.IsConnected Then plc.Close()
    End Sub

End Class