Imports Microsoft.Data.SqlClient
Imports System.Data
Imports System.Drawing.Printing

Public Class Factures

    Private _idFactureCourant As Integer = 0
    Private _modeEdition As Boolean = False
    Private WithEvents pdFacture As New PrintDocument

    ' ═══════════════════════════════════════════════════════════════════
    '  LOAD
    ' ═══════════════════════════════════════════════════════════════════

    Private Sub Factures_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        InitialiserCombos()
        StyleGrille()
        ChargerFactures()
        txtIdFacture.Visible = False
    End Sub

    Private Sub InitialiserCombos()
        cmbStatut.Items.Clear()
        For Each s In {"Brouillon", "Validée", "Envoyée", "Réglée", "Annulée"}
            cmbStatut.Items.Add(s)
        Next
        cmbStatut.SelectedIndex = 0

        cmbModeReglement.Items.Clear()
        For Each m In {"Virement", "Chèque", "Espèces", "Effet", "Compensation"}
            cmbModeReglement.Items.Add(m)
        Next
    End Sub

    Private Sub StyleGrille()
        With DataGridViewFactures
            .ReadOnly = True
            .AllowUserToAddRows = False
            .SelectionMode = DataGridViewSelectionMode.FullRowSelect
            .MultiSelect = False
            .RowHeadersVisible = False
            .EnableHeadersVisualStyles = False
            .AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.None
            .ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(45, 100, 165)
            .ColumnHeadersDefaultCellStyle.ForeColor = Color.White
            .ColumnHeadersDefaultCellStyle.Font = New Font("Arial", 9, FontStyle.Bold)
            .DefaultCellStyle.SelectionBackColor = Color.RoyalBlue
            .DefaultCellStyle.SelectionForeColor = Color.White
            .RowTemplate.Height = 28
            .BackgroundColor = Color.White
            .AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(245, 248, 255)
        End With

        With DataGridViewLignes
            .ReadOnly = True
            .AllowUserToAddRows = False
            .SelectionMode = DataGridViewSelectionMode.FullRowSelect
            .RowHeadersVisible = False
            .EnableHeadersVisualStyles = False
            .AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill
            .ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(80, 130, 80)
            .ColumnHeadersDefaultCellStyle.ForeColor = Color.White
            .BackgroundColor = Color.White
        End With
    End Sub

    ' ═══════════════════════════════════════════════════════════════════
    '  CHARGEMENT LISTE FACTURES
    ' ═══════════════════════════════════════════════════════════════════

    Private Sub ChargerFactures()
        Try
            Dim dt As DataTable = ModuleDb.Lire(
                "SELECT IdFacture, NumeroFacture, DateFacture, NomClient, " &
                "MontantHT, MontantTVA, MontantTTC, MontantRegle, MontantRestant, " &
                "Statut, NombreBL, VolumeTotal " &
                "FROM dbo.Factures ORDER BY IdFacture DESC")

            DataGridViewFactures.DataSource = dt

            If DataGridViewFactures.Columns.Count > 0 Then
                DataGridViewFactures.Columns("IdFacture").Visible = False
                DataGridViewFactures.Columns("NumeroFacture").HeaderText = "N° Facture" : DataGridViewFactures.Columns("NumeroFacture").Width = 110
                DataGridViewFactures.Columns("DateFacture").HeaderText = "Date" : DataGridViewFactures.Columns("DateFacture").Width = 85
                DataGridViewFactures.Columns("NomClient").HeaderText = "Client" : DataGridViewFactures.Columns("NomClient").Width = 160
                DataGridViewFactures.Columns("MontantHT").HeaderText = "HT" : DataGridViewFactures.Columns("MontantHT").Width = 85
                DataGridViewFactures.Columns("MontantTVA").HeaderText = "TVA" : DataGridViewFactures.Columns("MontantTVA").Width = 75
                DataGridViewFactures.Columns("MontantTTC").HeaderText = "TTC" : DataGridViewFactures.Columns("MontantTTC").Width = 90
                DataGridViewFactures.Columns("MontantRegle").HeaderText = "Réglé" : DataGridViewFactures.Columns("MontantRegle").Width = 80
                DataGridViewFactures.Columns("MontantRestant").HeaderText = "Restant" : DataGridViewFactures.Columns("MontantRestant").Width = 80
                DataGridViewFactures.Columns("Statut").HeaderText = "Statut" : DataGridViewFactures.Columns("Statut").Width = 80
                DataGridViewFactures.Columns("NombreBL").HeaderText = "NB BL" : DataGridViewFactures.Columns("NombreBL").Width = 55
                DataGridViewFactures.Columns("VolumeTotal").HeaderText = "Vol m³" : DataGridViewFactures.Columns("VolumeTotal").Width = 65
            End If

            Me.BeginInvoke(New Action(Sub() ColorerLignesFactures()))
            MettreAJourTotaux(dt)

        Catch ex As Exception
            MessageBox.Show("Erreur chargement factures : " & ex.Message)
        End Try
    End Sub

    Private Sub ColorerLignesFactures()
        For Each row As DataGridViewRow In DataGridViewFactures.Rows
            If row.IsNewRow Then Continue For
            Select Case row.Cells("Statut").Value.ToString().ToUpper()
                Case "RÉGLÉE" : row.DefaultCellStyle.BackColor = Color.FromArgb(210, 255, 210)
                Case "VALIDÉE" : row.DefaultCellStyle.BackColor = Color.FromArgb(210, 235, 255)
                Case "ENVOYÉE" : row.DefaultCellStyle.BackColor = Color.FromArgb(255, 245, 180)
                Case "ANNULÉE" : row.DefaultCellStyle.BackColor = Color.FromArgb(255, 210, 210)
                Case Else : row.DefaultCellStyle.BackColor = Color.White
            End Select
        Next
        DataGridViewFactures.Refresh()
    End Sub

    Private Sub MettreAJourTotaux(dt As DataTable)
        Dim totalHT As Decimal = 0D
        Dim totalTTC As Decimal = 0D
        Dim totalRegle As Decimal = 0D
        Dim totalRestant As Decimal = 0D

        For Each row As DataRow In dt.Rows
            If Not IsDBNull(row("MontantHT")) Then totalHT += CDec(row("MontantHT"))
            If Not IsDBNull(row("MontantTTC")) Then totalTTC += CDec(row("MontantTTC"))
            If Not IsDBNull(row("MontantRegle")) Then totalRegle += CDec(row("MontantRegle"))
            If Not IsDBNull(row("MontantRestant")) Then totalRestant += CDec(row("MontantRestant"))
        Next

        lblNbFactures.Text = "Factures : " & dt.Rows.Count
        lblTotalHT.Text = "Total HT : " & totalHT.ToString("N2") & " MAD"
        lblTotalTTC.Text = "Total TTC : " & totalTTC.ToString("N2") & " MAD"
        lblTotalRegle.Text = "Réglé : " & totalRegle.ToString("N2") & " MAD"
        lblTotalRestant.Text = "Restant : " & totalRestant.ToString("N2") & " MAD"
    End Sub

    ' ═══════════════════════════════════════════════════════════════════
    '  CLIC GRILLE FACTURES
    ' ═══════════════════════════════════════════════════════════════════

    Private Sub DataGridViewFactures_CellClick(sender As Object, e As DataGridViewCellEventArgs) Handles DataGridViewFactures.CellClick
        If e.RowIndex < 0 Then Exit Sub
        _idFactureCourant = CInt(DataGridViewFactures.Rows(e.RowIndex).Cells("IdFacture").Value)
        ChargerDetailFacture(_idFactureCourant)
        ChargerLignesFacture(_idFactureCourant)
    End Sub

    ' ═══════════════════════════════════════════════════════════════════
    '  CHARGEMENT DÉTAIL FACTURE
    ' ═══════════════════════════════════════════════════════════════════

    Private Sub ChargerDetailFacture(idFacture As Integer)
        Try
            Dim dt As DataTable = ModuleDb.Lire(
                "SELECT * FROM dbo.Factures WHERE IdFacture=@Id",
                Sub(cmd) cmd.Parameters.Add("@Id", SqlDbType.Int).Value = idFacture)

            If dt.Rows.Count = 0 Then Exit Sub
            Dim dr As DataRow = dt.Rows(0)

            txtIdFacture.Text = dr("IdFacture").ToString()
            txtNumeroFacture.Text = dr("NumeroFacture").ToString()
            dtpDateFacture.Value = If(IsDBNull(dr("DateFacture")), Date.Today, CDate(dr("DateFacture")))
            If Not IsDBNull(dr("DateEcheance")) Then dtpDateEcheance.Value = CDate(dr("DateEcheance"))
            txtNomClient.Text = If(IsDBNull(dr("NomClient")), "", dr("NomClient").ToString())
            txtICE.Text = If(IsDBNull(dr("ICE")), "", dr("ICE").ToString())
            txtAdresseClient.Text = If(IsDBNull(dr("AdresseClient")), "", dr("AdresseClient").ToString())
            txtMontantHT.Text = If(IsDBNull(dr("MontantHT")), "0.00", CDec(dr("MontantHT")).ToString("N2"))
            txtRemise.Text = If(IsDBNull(dr("RemisePercent")), "0.00", CDec(dr("RemisePercent")).ToString("N2"))
            txtMontantHTNet.Text = If(IsDBNull(dr("MontantHT_Net")), "0.00", CDec(dr("MontantHT_Net")).ToString("N2"))
            txtTauxTVA.Text = If(IsDBNull(dr("TauxTVA")), "20.00", CDec(dr("TauxTVA")).ToString("N2"))
            txtMontantTVA.Text = If(IsDBNull(dr("MontantTVA")), "0.00", CDec(dr("MontantTVA")).ToString("N2"))
            txtMontantTTC.Text = If(IsDBNull(dr("MontantTTC")), "0.00", CDec(dr("MontantTTC")).ToString("N2"))
            txtMontantRegle.Text = If(IsDBNull(dr("MontantRegle")), "0.00", CDec(dr("MontantRegle")).ToString("N2"))
            txtMontantRestant.Text = If(IsDBNull(dr("MontantRestant")), "0.00", CDec(dr("MontantRestant")).ToString("N2"))
            cmbStatut.Text = dr("Statut").ToString()
            cmbModeReglement.Text = If(IsDBNull(dr("ModeReglement")), "", dr("ModeReglement").ToString())
            txtRemarques.Text = If(IsDBNull(dr("Remarques")), "", dr("Remarques").ToString())
            txtNombreBL.Text = If(IsDBNull(dr("NombreBL")), "0", dr("NombreBL").ToString())
            txtVolumeTotal.Text = If(IsDBNull(dr("VolumeTotal")), "0.00", CDec(dr("VolumeTotal")).ToString("N2"))

            ' Période
            If Not IsDBNull(dr("PeriodeDu")) Then dtpPeriodeDu.Value = CDate(dr("PeriodeDu"))
            If Not IsDBNull(dr("PeriodeAu")) Then dtpPeriodeAu.Value = CDate(dr("PeriodeAu"))

        Catch ex As Exception
            MessageBox.Show("Erreur chargement détail : " & ex.Message)
        End Try
    End Sub

    ' ═══════════════════════════════════════════════════════════════════
    '  CHARGEMENT LIGNES FACTURE
    ' ═══════════════════════════════════════════════════════════════════

    Private Sub ChargerLignesFacture(idFacture As Integer)
        Try
            Dim dt As DataTable = ModuleDb.Lire(
                "SELECT NumeroLigne, CodeProduit, Designation, Unite, " &
                "Quantite, PUHT, RemisePercent, MontantHT, TauxTVA, MontantTTC, NumeroBL " &
                "FROM dbo.FacturesLignes WHERE IdFacture=@Id ORDER BY NumeroLigne",
                Sub(cmd) cmd.Parameters.Add("@Id", SqlDbType.Int).Value = idFacture)

            DataGridViewLignes.DataSource = dt

            If DataGridViewLignes.Columns.Count > 0 Then
                DataGridViewLignes.Columns("NumeroLigne").HeaderText = "N°"
                DataGridViewLignes.Columns("CodeProduit").HeaderText = "Code"
                DataGridViewLignes.Columns("Designation").HeaderText = "Désignation"
                DataGridViewLignes.Columns("Unite").HeaderText = "Unité"
                DataGridViewLignes.Columns("Quantite").HeaderText = "Qté"
                DataGridViewLignes.Columns("PUHT").HeaderText = "PU HT"
                DataGridViewLignes.Columns("RemisePercent").HeaderText = "Remise %"
                DataGridViewLignes.Columns("MontantHT").HeaderText = "Montant HT"
                DataGridViewLignes.Columns("TauxTVA").HeaderText = "TVA %"
                DataGridViewLignes.Columns("MontantTTC").HeaderText = "Montant TTC"
                DataGridViewLignes.Columns("NumeroBL").HeaderText = "N° BL"
            End If

        Catch ex As Exception
            MessageBox.Show("Erreur chargement lignes : " & ex.Message)
        End Try
    End Sub

    ' ═══════════════════════════════════════════════════════════════════
    '  CALCUL AUTOMATIQUE MONTANTS
    ' ═══════════════════════════════════════════════════════════════════

    Private Sub CalculerMontants()
        Try
            Dim ht As Decimal = ParseDecimal(txtMontantHT.Text)
            Dim remise As Decimal = ParseDecimal(txtRemise.Text)
            Dim tva As Decimal = ParseDecimal(txtTauxTVA.Text)

            Dim remiseMontant As Decimal = ht * remise / 100
            Dim htNet As Decimal = ht - remiseMontant
            Dim montantTVA As Decimal = htNet * tva / 100
            Dim ttc As Decimal = htNet + montantTVA

            txtMontantHTNet.Text = htNet.ToString("N2")
            txtMontantTVA.Text = montantTVA.ToString("N2")
            txtMontantTTC.Text = ttc.ToString("N2")

            Dim regle As Decimal = ParseDecimal(txtMontantRegle.Text)
            txtMontantRestant.Text = (ttc - regle).ToString("N2")
        Catch
        End Try
    End Sub

    Private Sub txtMontantHT_Leave(s As Object, e As EventArgs) Handles txtMontantHT.Leave
        CalculerMontants()
    End Sub
    Private Sub txtRemise_Leave(s As Object, e As EventArgs) Handles txtRemise.Leave
        CalculerMontants()
    End Sub
    Private Sub txtTauxTVA_Leave(s As Object, e As EventArgs) Handles txtTauxTVA.Leave
        CalculerMontants()
    End Sub
    Private Sub txtMontantRegle_Leave(s As Object, e As EventArgs) Handles txtMontantRegle.Leave
        CalculerMontants()
    End Sub

    ' ═══════════════════════════════════════════════════════════════════
    '  NOUVELLE FACTURE
    ' ═══════════════════════════════════════════════════════════════════

    Private Sub btnNouveau_Click(sender As Object, e As EventArgs) Handles btnNouveau.Click
        ViderChamps()
        ' Générer numéro automatique
        Try
            Dim n As Integer = ModuleDb.LireValeur(Of Integer)(
                "SELECT ISNULL(MAX(IdFacture), 0) + 1 FROM dbo.Factures")
            txtNumeroFacture.Text = "FAC-" & Date.Today.ToString("yyyy") & "-" & n.ToString("D4")
        Catch
            txtNumeroFacture.Text = "FAC-" & Date.Today.ToString("yyyyMMdd")
        End Try
        dtpDateFacture.Value = Date.Today
        dtpDateEcheance.Value = Date.Today.AddDays(30)
        txtTauxTVA.Text = "20.00"
        cmbStatut.SelectedIndex = 0
        txtNomClient.Focus()
    End Sub

    ' ═══════════════════════════════════════════════════════════════════
    '  ENREGISTRER FACTURE
    ' ═══════════════════════════════════════════════════════════════════

    Private Sub btnEnregistrer_Click(sender As Object, e As EventArgs) Handles btnEnregistrer.Click
        If txtNumeroFacture.Text.Trim = "" Then
            MessageBox.Show("Le numéro de facture est obligatoire.") : Exit Sub
        End If
        If txtNomClient.Text.Trim = "" Then
            MessageBox.Show("Le client est obligatoire.") : Exit Sub
        End If

        Try
            If txtIdFacture.Text = "" Then
                ' INSERT
                ModuleDb.Executer(
                    "INSERT INTO dbo.Factures " &
                    "(NumeroFacture, DateFacture, DateEcheance, NomClient, ICE, AdresseClient, " &
                    "MontantHT, RemisePercent, RemiseMontant, MontantHT_Net, TauxTVA, MontantTVA, MontantTTC, " &
                    "MontantRegle, PeriodeDu, PeriodeAu, NombreBL, VolumeTotal, " &
                    "Statut, ModeReglement, Remarques, Operateur, DateCreation) " &
                    "VALUES (@NumeroFacture, @DateFacture, @DateEcheance, @NomClient, @ICE, @AdresseClient, " &
                    "@MontantHT, @RemisePercent, @RemiseMontant, @MontantHT_Net, @TauxTVA, @MontantTVA, @MontantTTC, " &
                    "@MontantRegle, @PeriodeDu, @PeriodeAu, @NombreBL, @VolumeTotal, " &
                    "@Statut, @ModeReglement, @Remarques, @Operateur, GETDATE())",
                    AddressOf AjouterParams)
                MessageBox.Show("✅ Facture créée avec succès.")
            Else
                ' UPDATE
                ModuleDb.Executer(
                    "UPDATE dbo.Factures SET " &
                    "NumeroFacture=@NumeroFacture, DateFacture=@DateFacture, DateEcheance=@DateEcheance, " &
                    "NomClient=@NomClient, ICE=@ICE, AdresseClient=@AdresseClient, " &
                    "MontantHT=@MontantHT, RemisePercent=@RemisePercent, RemiseMontant=@RemiseMontant, " &
                    "MontantHT_Net=@MontantHT_Net, TauxTVA=@TauxTVA, MontantTVA=@MontantTVA, MontantTTC=@MontantTTC, " &
                    "MontantRegle=@MontantRegle, PeriodeDu=@PeriodeDu, PeriodeAu=@PeriodeAu, " &
                    "NombreBL=@NombreBL, VolumeTotal=@VolumeTotal, " &
                    "Statut=@Statut, ModeReglement=@ModeReglement, Remarques=@Remarques, " &
                    "DerniereModification=GETDATE() WHERE IdFacture=@IdFacture",
                    Sub(cmd)
                        AjouterParams(cmd)
                        cmd.Parameters.Add("@IdFacture", SqlDbType.Int).Value = CInt(txtIdFacture.Text)
                    End Sub)
                MessageBox.Show("✅ Facture modifiée avec succès.")
            End If

            ChargerFactures()

        Catch ex As Exception
            MessageBox.Show("Erreur enregistrement : " & ex.Message)
        End Try
    End Sub

    ' ═══════════════════════════════════════════════════════════════════
    '  SUPPRIMER FACTURE
    ' ═══════════════════════════════════════════════════════════════════

    Private Sub btnSupprimer_Click(sender As Object, e As EventArgs) Handles btnSupprimer.Click
        If txtIdFacture.Text = "" Then
            MessageBox.Show("Sélectionnez d'abord une facture.") : Exit Sub
        End If
        If MessageBox.Show("Supprimer cette facture ?", "Confirmation",
                           MessageBoxButtons.YesNo, MessageBoxIcon.Question) = DialogResult.No Then Exit Sub
        Try
            ModuleDb.Executer(
                "DELETE FROM dbo.FacturesLignes WHERE IdFacture=@Id",
                Sub(cmd) cmd.Parameters.Add("@Id", SqlDbType.Int).Value = CInt(txtIdFacture.Text))
            ModuleDb.Executer(
                "DELETE FROM dbo.Factures WHERE IdFacture=@Id",
                Sub(cmd) cmd.Parameters.Add("@Id", SqlDbType.Int).Value = CInt(txtIdFacture.Text))
            MessageBox.Show("Facture supprimée.")
            ViderChamps()
            ChargerFactures()
        Catch ex As Exception
            MessageBox.Show("Erreur suppression : " & ex.Message)
        End Try
    End Sub

    ' ═══════════════════════════════════════════════════════════════════
    '  CHANGER STATUT RAPIDE
    ' ═══════════════════════════════════════════════════════════════════

    Private Sub btnValider_Click(sender As Object, e As EventArgs) Handles btnValider.Click
        ChangerStatut("Validée")
    End Sub
    Private Sub btnMarquerRegle_Click(sender As Object, e As EventArgs) Handles btnMarquerRegle.Click
        ChangerStatut("Réglée")
    End Sub

    Private Sub ChangerStatut(statut As String)
        If txtIdFacture.Text = "" Then
            MessageBox.Show("Sélectionnez d'abord une facture.") : Exit Sub
        End If
        Try
            ModuleDb.Executer(
                "UPDATE dbo.Factures SET Statut=@Statut, DerniereModification=GETDATE() WHERE IdFacture=@Id",
                Sub(cmd)
                    cmd.Parameters.Add("@Statut", SqlDbType.NVarChar, 30).Value = statut
                    cmd.Parameters.Add("@Id", SqlDbType.Int).Value = CInt(txtIdFacture.Text)
                End Sub)
            cmbStatut.Text = statut
            ChargerFactures()
            MessageBox.Show("Statut → " & statut)
        Catch ex As Exception
            MessageBox.Show("Erreur statut : " & ex.Message)
        End Try
    End Sub

    ' ═══════════════════════════════════════════════════════════════════
    '  RECHERCHE CLIENT (double-clic)
    ' ═══════════════════════════════════════════════════════════════════

    Private Sub txtNomClient_DoubleClick(s As Object, e As EventArgs) Handles txtNomClient.DoubleClick
        Dim f As New FrmRecherche
        f.Initialiser(
            "SELECT IdClient, CodeClient, NomClient FROM dbo.Clients WHERE Actif=1",
            "IdClient", "CodeClient", "NomClient")
        f.Text = "Recherche Client"
        If f.ShowDialog() = DialogResult.OK Then
            Try
                Dim dt As DataTable = ModuleDb.Lire(
    "SELECT NomClient, Adresse, ICE FROM dbo.Clients WHERE IdClient=@Id",
    Sub(cmd) cmd.Parameters.Add("@Id", SqlDbType.Int).Value = f.IdSelectionne)
                If dt.Rows.Count > 0 Then
                    txtNomClient.Text = dt.Rows(0)("NomClient").ToString()
                    txtAdresseClient.Text = If(IsDBNull(dt.Rows(0)("Adresse")), "", dt.Rows(0)("Adresse").ToString())
                    txtICE.Text = If(IsDBNull(dt.Rows(0)("ICE")), "", dt.Rows(0)("ICE").ToString())
                End If
            Catch ex As Exception
                MessageBox.Show("Erreur client : " & ex.Message)
            End Try
        End If
    End Sub

    ' ═══════════════════════════════════════════════════════════════════
    '  IMPRESSION
    ' ═══════════════════════════════════════════════════════════════════

    Private Sub btnImprimer_Click(sender As Object, e As EventArgs) Handles btnImprimer.Click
        If txtIdFacture.Text = "" Then
            MessageBox.Show("Sélectionnez d'abord une facture.") : Exit Sub
        End If
        Try
            Dim apercu As New PrintPreviewDialog
            apercu.Document = pdFacture
            apercu.WindowState = FormWindowState.Maximized
            apercu.ShowDialog()
        Catch ex As Exception
            MessageBox.Show("Erreur impression : " & ex.Message)
        End Try
    End Sub

    Private Sub pdFacture_PrintPage(sender As Object, e As PrintPageEventArgs) Handles pdFacture.PrintPage
        Dim g As Graphics = e.Graphics
        Dim fontTitre As New Font("Arial", 16, FontStyle.Bold)
        Dim fontLabel As New Font("Arial", 9, FontStyle.Bold)
        Dim fontVal As New Font("Arial", 9)
        Dim fontEntete As New Font("Arial", 10, FontStyle.Bold)
        Dim x As Integer = 30 : Dim y As Integer = 30

        ' ── En-tête société ─────────────────────────────────────
        g.FillRectangle(New SolidBrush(Color.FromArgb(10, 50, 120)), x, y, 740, 65)
        g.DrawString(SocieteNom, New Font("Arial", 13, FontStyle.Bold), Brushes.White, x + 10, y + 5)
        g.DrawString(SocieteAdresse, New Font("Arial", 8), Brushes.LightGray, x + 10, y + 28)
        g.DrawString("Tél: " & SocieteTelephone & "  Fax: " & SocieteFax, New Font("Arial", 8), Brushes.LightGray, x + 10, y + 43)
        g.DrawString("ICE: " & SocieteICE & "  IF: " & SocieteIF & "  RC: " & SocieteRC, New Font("Arial", 8), Brushes.LightGray, x + 400, y + 28)
        g.DrawString("Patente: " & SocietePatente, New Font("Arial", 8), Brushes.LightGray, x + 400, y + 43)
        y += 80

        ' ── Titre FACTURE ───────────────────────────────────────
        g.FillRectangle(New SolidBrush(Color.FromArgb(45, 100, 165)), x, y, 740, 35)
        g.DrawString("FACTURE N° " & txtNumeroFacture.Text,
                     New Font("Arial", 14, FontStyle.Bold), Brushes.White, x + 10, y + 8)
        g.DrawString("Date : " & dtpDateFacture.Value.ToString("dd/MM/yyyy") &
                     "   Échéance : " & dtpDateEcheance.Value.ToString("dd/MM/yyyy"),
                     fontVal, Brushes.White, x + 450, y + 12)
        y += 45

        ' ── Client ──────────────────────────────────────────────
        g.FillRectangle(New SolidBrush(Color.FromArgb(230, 235, 250)), x, y, 360, 70)
        g.DrawRectangle(Pens.DarkBlue, x, y, 360, 70)
        g.DrawString("CLIENT", fontEntete, Brushes.DarkBlue, x + 10, y + 5)
        g.DrawString(txtNomClient.Text, fontLabel, Brushes.Black, x + 10, y + 25)
        g.DrawString(txtAdresseClient.Text, fontVal, Brushes.Black, x + 10, y + 42)
        g.DrawString("ICE : " & txtICE.Text, fontVal, Brushes.Black, x + 10, y + 57)

        ' ── Période ─────────────────────────────────────────────
        g.FillRectangle(New SolidBrush(Color.FromArgb(230, 235, 250)), x + 380, y, 360, 70)
        g.DrawRectangle(Pens.DarkBlue, x + 380, y, 360, 70)
        g.DrawString("PÉRIODE / INFORMATIONS", fontEntete, Brushes.DarkBlue, x + 390, y + 5)
        g.DrawString("Période : " & dtpPeriodeDu.Value.ToString("dd/MM/yyyy") &
                     " au " & dtpPeriodeAu.Value.ToString("dd/MM/yyyy"),
                     fontVal, Brushes.Black, x + 390, y + 25)
        g.DrawString("Nombre BL : " & txtNombreBL.Text & "   Volume : " & txtVolumeTotal.Text & " m³",
                     fontVal, Brushes.Black, x + 390, y + 42)
        g.DrawString("Mode règlement : " & cmbModeReglement.Text, fontVal, Brushes.Black, x + 390, y + 57)
        y += 85

        ' ── Lignes facture ──────────────────────────────────────
        Dim cols = {"N°", "Code", "Désignation", "Unité", "Quantité", "PU HT", "Remise%", "Montant HT", "TVA%", "Montant TTC"}
        Dim xCols = {0, 20, 60, 220, 255, 295, 355, 405, 475, 520}
        g.FillRectangle(New SolidBrush(Color.FromArgb(45, 100, 165)), x, y, 740, 22)
        For i As Integer = 0 To cols.Length - 1
            g.DrawString(cols(i), New Font("Arial", 8, FontStyle.Bold), Brushes.White, x + xCols(i) + 2, y + 4)
        Next
        y += 22

        Dim dt As DataTable = TryCast(DataGridViewLignes.DataSource, DataTable)
        If dt IsNot Nothing Then
            Dim pair As Boolean = False
            For Each row As DataRow In dt.Rows
                g.FillRectangle(New SolidBrush(If(pair, Color.FromArgb(245, 248, 255), Color.White)), x, y, 740, 20)
                Dim vals = {
                    row("NumeroLigne").ToString(), row("CodeProduit").ToString(),
                    TronquerTexte(row("Designation").ToString(), 22),
                    row("Unite").ToString(), CDec(row("Quantite")).ToString("N3"),
                    CDec(row("PUHT")).ToString("N2"), CDec(row("RemisePercent")).ToString("N1") & "%",
                    CDec(row("MontantHT")).ToString("N2"), CDec(row("TauxTVA")).ToString("N1") & "%",
                    CDec(row("MontantTTC")).ToString("N2")}
                For i As Integer = 0 To vals.Length - 1
                    g.DrawString(vals(i), fontVal, Brushes.Black, x + xCols(i) + 2, y + 3)
                Next
                g.DrawLine(Pens.LightGray, x, y + 20, x + 740, y + 20)
                y += 20 : pair = Not pair
            Next
        End If
        y += 15

        ' ── Totaux ──────────────────────────────────────────────
        g.DrawLine(New Pen(Color.DarkBlue, 1), x + 450, y, x + 740, y) : y += 5
        Dim totaux = {
            ("Montant HT :", txtMontantHT.Text & " MAD"),
            ("Remise (" & txtRemise.Text & "%) :", (ParseDecimal(txtMontantHT.Text) * ParseDecimal(txtRemise.Text) / 100).ToString("N2") & " MAD"),
            ("Montant HT Net :", txtMontantHTNet.Text & " MAD"),
            ("TVA (" & txtTauxTVA.Text & "%) :", txtMontantTVA.Text & " MAD"),
            ("MONTANT TTC :", txtMontantTTC.Text & " MAD"),
            ("Déjà réglé :", txtMontantRegle.Text & " MAD"),
            ("RESTE À PAYER :", txtMontantRestant.Text & " MAD")}

        For Each t In totaux
            Dim isBold As Boolean = t.Item1.StartsWith("MONTANT") OrElse t.Item1.StartsWith("RESTE")
            Dim fnt As Font = If(isBold, New Font("Arial", 10, FontStyle.Bold), fontVal)
            If isBold Then g.FillRectangle(New SolidBrush(Color.FromArgb(45, 100, 165)), x + 450, y, 290, 22)
            g.DrawString(t.Item1, fnt, If(isBold, Brushes.White, Brushes.Black), x + 455, y + 3)
            g.DrawString(t.Item2, fnt, If(isBold, Brushes.White, Brushes.Black), x + 630, y + 3)
            y += If(isBold, 22, 18)
        Next
        y += 15

        ' ── Remarques ───────────────────────────────────────────
        If txtRemarques.Text.Trim <> "" Then
            g.DrawString("Remarques : " & txtRemarques.Text, fontVal, Brushes.Gray, x, y) : y += 20
        End If

        ' ── Signatures ──────────────────────────────────────────
        y += 10
        g.DrawString("Signature client : ___________________", fontLabel, Brushes.Black, x, y)
        g.DrawString("Cachet & Signature : ___________________", fontLabel, Brushes.Black, x + 430, y)

        ' ── Pied de page ────────────────────────────────────────
        g.FillRectangle(New SolidBrush(Color.FromArgb(10, 50, 120)), x, 800, 740, 15)
        g.DrawString(SocieteNom & "  |  " & SocieteAdresse & "  |  ICE: " & SocieteICE &
                     "  |  Imprimé le " & DateTime.Now.ToString("dd/MM/yyyy HH:mm"),
                     New Font("Arial", 7), Brushes.White, x + 5, 803)
    End Sub

    ' ═══════════════════════════════════════════════════════════════════
    '  ACTUALISER
    ' ═══════════════════════════════════════════════════════════════════

    Private Sub btnActualiser_Click(sender As Object, e As EventArgs) Handles btnActualiser.Click
        ChargerFactures()
    End Sub

    ' ═══════════════════════════════════════════════════════════════════
    '  HELPERS
    ' ═══════════════════════════════════════════════════════════════════

    Private Sub AjouterParams(cmd As SqlCommand)
        cmd.Parameters.Add("@NumeroFacture", SqlDbType.NVarChar, 30).Value = txtNumeroFacture.Text.Trim()
        cmd.Parameters.Add("@DateFacture", SqlDbType.Date).Value = dtpDateFacture.Value.Date
        cmd.Parameters.Add("@DateEcheance", SqlDbType.Date).Value = dtpDateEcheance.Value.Date
        cmd.Parameters.Add("@NomClient", SqlDbType.NVarChar, 150).Value = txtNomClient.Text.Trim()
        cmd.Parameters.Add("@ICE", SqlDbType.NVarChar, 50).Value = txtICE.Text.Trim()
        cmd.Parameters.Add("@AdresseClient", SqlDbType.NVarChar, 250).Value = txtAdresseClient.Text.Trim()
        cmd.Parameters.Add("@MontantHT", SqlDbType.Decimal).Value = ParseDecimal(txtMontantHT.Text)
        cmd.Parameters.Add("@RemisePercent", SqlDbType.Decimal).Value = ParseDecimal(txtRemise.Text)
        cmd.Parameters.Add("@RemiseMontant", SqlDbType.Decimal).Value = ParseDecimal(txtMontantHT.Text) * ParseDecimal(txtRemise.Text) / 100
        cmd.Parameters.Add("@MontantHT_Net", SqlDbType.Decimal).Value = ParseDecimal(txtMontantHTNet.Text)
        cmd.Parameters.Add("@TauxTVA", SqlDbType.Decimal).Value = ParseDecimal(txtTauxTVA.Text)
        cmd.Parameters.Add("@MontantTVA", SqlDbType.Decimal).Value = ParseDecimal(txtMontantTVA.Text)
        cmd.Parameters.Add("@MontantTTC", SqlDbType.Decimal).Value = ParseDecimal(txtMontantTTC.Text)
        cmd.Parameters.Add("@MontantRegle", SqlDbType.Decimal).Value = ParseDecimal(txtMontantRegle.Text)
        cmd.Parameters.Add("@PeriodeDu", SqlDbType.Date).Value = dtpPeriodeDu.Value.Date
        cmd.Parameters.Add("@PeriodeAu", SqlDbType.Date).Value = dtpPeriodeAu.Value.Date
        cmd.Parameters.Add("@NombreBL", SqlDbType.Int).Value = ParseInt(txtNombreBL.Text)
        cmd.Parameters.Add("@VolumeTotal", SqlDbType.Decimal).Value = ParseDecimal(txtVolumeTotal.Text)
        cmd.Parameters.Add("@Statut", SqlDbType.NVarChar, 30).Value = cmbStatut.Text
        cmd.Parameters.Add("@ModeReglement", SqlDbType.NVarChar, 50).Value = cmbModeReglement.Text
        cmd.Parameters.Add("@Remarques", SqlDbType.NVarChar, 500).Value = txtRemarques.Text.Trim()
        cmd.Parameters.Add("@Operateur", SqlDbType.NVarChar, 100).Value = NomUtilisateur
    End Sub

    Private Function ParseDecimal(txt As String) As Decimal
        Dim v As Decimal
        Decimal.TryParse(txt.Replace(",", ".").Replace(" ", ""),
            Globalization.NumberStyles.Any,
            Globalization.CultureInfo.InvariantCulture, v)
        Return v
    End Function

    Private Function ParseInt(txt As String) As Integer
        Dim v As Integer : Integer.TryParse(txt, v) : Return v
    End Function

    Private Function TronquerTexte(txt As String, max As Integer) As String
        Return If(txt.Length <= max, txt, txt.Substring(0, max - 1) & "…")
    End Function

    Private Sub ViderChamps()
        txtIdFacture.Text = "" : txtNumeroFacture.Text = "" : txtNomClient.Text = ""
        txtICE.Text = "" : txtAdresseClient.Text = "" : txtRemarques.Text = ""
        txtMontantHT.Text = "0.00" : txtRemise.Text = "0.00"
        txtMontantHTNet.Text = "0.00" : txtTauxTVA.Text = "20.00"
        txtMontantTVA.Text = "0.00" : txtMontantTTC.Text = "0.00"
        txtMontantRegle.Text = "0.00" : txtMontantRestant.Text = "0.00"
        txtNombreBL.Text = "0" : txtVolumeTotal.Text = "0.00"
        cmbStatut.SelectedIndex = 0 : cmbModeReglement.SelectedIndex = -1
        DataGridViewLignes.DataSource = Nothing
        _idFactureCourant = 0
    End Sub

End Class