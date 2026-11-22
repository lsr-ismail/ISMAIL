Imports Microsoft.Data.SqlClient
Imports System.Data

Public Class Bons_de_Pompage

    Private _idBPCourant As Integer = 0
    Private _idClientCourant As Integer = 0
    Private _idChantierCourant As Integer = 0
    Private _idChauffeurPompe As Integer = 0

    Private Sub Bons_de_Pompage_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        dtpDate.Value = Date.Today
        InitialiserCombos()
        StyleGrille()
        ChargerBonsPompage()
        txtIdBP.Visible = False
    End Sub

    Private Sub InitialiserCombos()
        cmbStatut1.Items.Clear()
        For Each s In {"Livré", "Facturé", "Annulé"}
            cmbStatut1.Items.Add(s)
        Next
        cmbStatut1.SelectedIndex = 0
    End Sub

    Private Sub StyleGrille()
        With DataGridView1
            .ReadOnly = True : .AllowUserToAddRows = False
            .SelectionMode = DataGridViewSelectionMode.FullRowSelect
            .MultiSelect = False : .RowHeadersVisible = False
            .EnableHeadersVisualStyles = False
            .AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.None
            .ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(45, 100, 165)
            .ColumnHeadersDefaultCellStyle.ForeColor = Color.White
            .ColumnHeadersDefaultCellStyle.Font = New Font("Arial", 9, FontStyle.Bold)
            .DefaultCellStyle.SelectionBackColor = Color.RoyalBlue
            .RowTemplate.Height = 28
            .BackgroundColor = Color.White
            .AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(245, 248, 255)
        End With
    End Sub

    ' ═══════════════════════════════════════════════════════════════════
    '  CHARGEMENT
    ' ═══════════════════════════════════════════════════════════════════

    Private Sub ChargerBonsPompage()
        Try
            Dim dt As DataTable = ModuleDb.Lire(
                "SELECT IdBP, NumeroBP, " &
                "CONVERT(VARCHAR(10), DateHeure, 103) AS [Date], " &
                "NomClient, NomChantier, CodePompe, NomPompe, " &
                "VolumeM3, MontantHT, TauxTVA, MontantTTC, Statut " &
                "FROM dbo.BonsPompage " &
                "WHERE CONVERT(date, DateHeure) = @Date " &
                "ORDER BY DateHeure DESC",
                Sub(cmd)
                    cmd.Parameters.Add("@Date", SqlDbType.Date).Value = dtpDate.Value.Date
                End Sub)

            DataGridView1.DataSource = dt

            If DataGridView1.Columns.Count > 0 Then
                DataGridView1.Columns("IdBP").Visible = False
                DataGridView1.Columns("NumeroBP").HeaderText = "N° BP" : DataGridView1.Columns("NumeroBP").Width = 100
                DataGridView1.Columns("Date").HeaderText = "Date" : DataGridView1.Columns("Date").Width = 80
                DataGridView1.Columns("NomClient").HeaderText = "Client" : DataGridView1.Columns("NomClient").Width = 150
                DataGridView1.Columns("NomChantier").HeaderText = "Chantier" : DataGridView1.Columns("NomChantier").Width = 150
                DataGridView1.Columns("CodePompe").HeaderText = "Pompe" : DataGridView1.Columns("CodePompe").Width = 70
                DataGridView1.Columns("NomPompe").HeaderText = "Nom Pompe" : DataGridView1.Columns("NomPompe").Width = 100
                DataGridView1.Columns("VolumeM3").HeaderText = "Vol m³" : DataGridView1.Columns("VolumeM3").Width = 65
                DataGridView1.Columns("MontantHT").HeaderText = "HT" : DataGridView1.Columns("MontantHT").Width = 80
                DataGridView1.Columns("TauxTVA").HeaderText = "TVA%" : DataGridView1.Columns("TauxTVA").Width = 50
                DataGridView1.Columns("MontantTTC").HeaderText = "TTC" : DataGridView1.Columns("MontantTTC").Width = 80
                DataGridView1.Columns("Statut").HeaderText = "Statut" : DataGridView1.Columns("Statut").Width = 70
            End If

            lblNombre.Text = "Total : " & dt.Rows.Count & " BP"

        Catch ex As Exception
            MessageBox.Show("Erreur chargement : " & ex.Message)
        End Try
    End Sub

    Private Sub DataGridView1_CellClick(sender As Object, e As DataGridViewCellEventArgs) Handles DataGridView1.CellClick
        If e.RowIndex < 0 Then Exit Sub
        _idBPCourant = CInt(DataGridView1.Rows(e.RowIndex).Cells("IdBP").Value)
        ChargerDetailBP(_idBPCourant)
    End Sub

    Private Sub ChargerDetailBP(idBP As Integer)
        Try
            Dim dt As DataTable = ModuleDb.Lire(
                "SELECT * FROM dbo.BonsPompage WHERE IdBP=@Id",
                Sub(cmd) cmd.Parameters.Add("@Id", SqlDbType.Int).Value = idBP)

            If dt.Rows.Count = 0 Then Exit Sub
            Dim dr As DataRow = dt.Rows(0)

            txtIdBP.Text = dr("IdBP").ToString()
            txtNumeroBP.Text = dr("NumeroBP").ToString()
            txtNomClient.Text = If(IsDBNull(dr("NomClient")), "", dr("NomClient").ToString())
            txtNomChantier.Text = If(IsDBNull(dr("NomChantier")), "", dr("NomChantier").ToString())
            txtCodePompe.Text = If(IsDBNull(dr("CodePompe")), "", dr("CodePompe").ToString())
            txtNomPompe.Text = If(IsDBNull(dr("NomPompe")), "", dr("NomPompe").ToString())
            txtChauffeur.Text = If(IsDBNull(dr("NomChauffeurPompe")), "", dr("NomChauffeurPompe").ToString())
            txtVolumeM3.Text = If(IsDBNull(dr("VolumeM3")), "0.00", CDec(dr("VolumeM3")).ToString("N2"))
            txtHeureDébut.Text = If(IsDBNull(dr("HeureDébut")), "", CType(dr("HeureDébut"), TimeSpan).ToString("hh\:mm"))
            txtHeureFin.Text = If(IsDBNull(dr("HeureFin")), "", CType(dr("HeureFin"), TimeSpan).ToString("hh\:mm"))
            txtDuree.Text = If(IsDBNull(dr("DureePompageMin")), "0", dr("DureePompageMin").ToString())
            txtPUHT.Text = If(IsDBNull(dr("PUHT")), "0.00", CDec(dr("PUHT")).ToString("N2"))
            txtMontantHT.Text = If(IsDBNull(dr("MontantHT")), "0.00", CDec(dr("MontantHT")).ToString("N2"))
            txtTauxTVA.Text = If(IsDBNull(dr("TauxTVA")), "20.00", CDec(dr("TauxTVA")).ToString("N2"))
            txtMontantTTC.Text = If(IsDBNull(dr("MontantTTC")), "0.00", CDec(dr("MontantTTC")).ToString("N2"))
            txtRemarques.Text = If(IsDBNull(dr("Remarques")), "", dr("Remarques").ToString())
            cmbStatut1.Text = dr("Statut").ToString()

            _idClientCourant = If(IsDBNull(dr("IdClient")), 0, CInt(dr("IdClient")))
            _idChantierCourant = If(IsDBNull(dr("IdChantier")), 0, CInt(dr("IdChantier")))
            _idChauffeurPompe = If(IsDBNull(dr("IdChauffeurPompe")), 0, CInt(dr("IdChauffeurPompe")))

        Catch ex As Exception
            MessageBox.Show("Erreur détail BP : " & ex.Message)
        End Try
    End Sub

    ' ═══════════════════════════════════════════════════════════════════
    '  RECHERCHE CLIENT / CHANTIER / POMPE / CHAUFFEUR (double-clic)
    ' ═══════════════════════════════════════════════════════════════════

    Private Sub txtNomClient_DoubleClick(s As Object, e As EventArgs) Handles txtNomClient.DoubleClick
        Dim f As New FrmRecherche
        f.Initialiser(
            "SELECT IdClient, CodeClient, NomClient FROM dbo.Clients WHERE 1=1",
            "IdClient", "CodeClient", "NomClient")
        f.Text = "Recherche Client"
        If f.ShowDialog() = DialogResult.OK Then
            _idClientCourant = f.IdSelectionne
            txtNomClient.Text = f.ValeurSelectionnee
            _idChantierCourant = 0
            txtNomChantier.Text = ""
        End If
    End Sub

    Private Sub txtNomChantier_DoubleClick(s As Object, e As EventArgs) Handles txtNomChantier.DoubleClick
        If _idClientCourant = 0 Then
            MessageBox.Show("Sélectionnez d'abord un client.") : Exit Sub
        End If
        Dim f As New FrmRecherche
        f.Initialiser(
            "SELECT IdChantier, CodeChantier, NomChantier FROM dbo.Chantier " &
            "WHERE IdClient=" & _idClientCourant,
            "IdChantier", "CodeChantier", "NomChantier")
        f.Text = "Recherche Chantier"
        If f.ShowDialog() = DialogResult.OK Then
            _idChantierCourant = f.IdSelectionne
            txtNomChantier.Text = f.ValeurSelectionnee
        End If
    End Sub

    Private Sub txtCodePompe_DoubleClick(s As Object, e As EventArgs) Handles txtCodePompe.DoubleClick
        Dim f As New FrmRecherche
        f.Initialiser(
            "SELECT IdCamion, CodeCamion, NomCamion FROM dbo.Camions WHERE 1=1",
            "IdCamion", "CodeCamion", "NomCamion")
        f.Text = "Recherche Pompe"
        If f.ShowDialog() = DialogResult.OK Then
            Try
                Dim dt As DataTable = ModuleDb.Lire(
                    "SELECT CodeCamion, NomCamion FROM dbo.Camions WHERE IdCamion=@Id",
                    Sub(cmd) cmd.Parameters.Add("@Id", SqlDbType.Int).Value = f.IdSelectionne)
                If dt.Rows.Count > 0 Then
                    txtCodePompe.Text = dt.Rows(0)("CodeCamion").ToString()
                    txtNomPompe.Text = dt.Rows(0)("NomCamion").ToString()
                End If
            Catch ex As Exception
                MessageBox.Show("Erreur pompe : " & ex.Message)
            End Try
        End If
    End Sub

    Private Sub txtChauffeur_DoubleClick(s As Object, e As EventArgs) Handles txtChauffeur.DoubleClick
        Dim f As New FrmRecherche
        f.Initialiser(
            "SELECT IdChauffeur, CodeChauffeur, NomChauffeur " &
            "FROM dbo.Chauffeurs WHERE Actif=1 OR Actif IS NULL",
            "IdChauffeur", "CodeChauffeur", "NomChauffeur")
        f.Text = "Recherche Chauffeur"
        If f.ShowDialog() = DialogResult.OK Then
            Try
                Dim dt As DataTable = ModuleDb.Lire(
                    "SELECT NomChauffeur FROM dbo.Chauffeurs WHERE IdChauffeur=@Id",
                    Sub(cmd) cmd.Parameters.Add("@Id", SqlDbType.Int).Value = f.IdSelectionne)
                If dt.Rows.Count > 0 Then
                    txtChauffeur.Text = dt.Rows(0)("NomChauffeur").ToString()
                    _idChauffeurPompe = f.IdSelectionne
                End If
            Catch ex As Exception
                MessageBox.Show("Erreur chauffeur : " & ex.Message)
            End Try
        End If
    End Sub

    ' ═══════════════════════════════════════════════════════════════════
    '  NOUVEAU / ENREGISTRER / SUPPRIMER
    ' ═══════════════════════════════════════════════════════════════════

    Private Sub btnNouveau_Click(sender As Object, e As EventArgs) Handles btnNouveau.Click
        ViderChamps()
        Try
            Dim n As Integer = ModuleDb.LireValeur(Of Integer)(
                "SELECT ISNULL(MAX(IdBP), 0) + 1 FROM dbo.BonsPompage")
            txtNumeroBP.Text = "BP-" & Date.Today.ToString("yyyy") & "-" & n.ToString("D4")
        Catch
            txtNumeroBP.Text = "BP-" & Date.Today.ToString("yyyyMMdd")
        End Try
        txtTauxTVA.Text = "20.00"
        cmbStatut1.SelectedIndex = 0
    End Sub

    Private Sub btnEnregistrer_Click(sender As Object, e As EventArgs) Handles btnEnregistrer.Click
        If txtNumeroBP.Text.Trim = "" Then
            MessageBox.Show("Le numéro BP est obligatoire.") : Exit Sub
        End If
        If _idClientCourant = 0 Then
            MessageBox.Show("Sélectionnez un client (double-clic sur le champ Client).") : Exit Sub
        End If
        Try
            CalculerMontants()

            If txtIdBP.Text = "" Then
                ModuleDb.Executer(
                    "INSERT INTO dbo.BonsPompage " &
                    "(NumeroBP, DateHeure, IdClient, NomClient, IdChantier, NomChantier, " &
                    "CodePompe, NomPompe, IdChauffeurPompe, NomChauffeurPompe, VolumeM3, " &
                    "HeureDébut, HeureFin, DureePompageMin, " &
                    "PUHT, MontantHT, TauxTVA, MontantTTC, " &
                    "Statut, Remarques, Operateur, DateCreation) " &
                    "VALUES (@NumeroBP, @DateHeure, @IdClient, @NomClient, @IdChantier, @NomChantier, " &
                    "@CodePompe, @NomPompe, @IdChauffeur, @NomChauffeur, @VolumeM3, " &
                    "@HeureDébut, @HeureFin, @Duree, " &
                    "@PUHT, @MontantHT, @TauxTVA, @MontantTTC, " &
                    "@Statut, @Remarques, @Operateur, GETDATE())",
                    AddressOf AjouterParams)
                MessageBox.Show("✅ Bon de pompage créé.")
            Else
                ModuleDb.Executer(
                    "UPDATE dbo.BonsPompage SET " &
                    "IdClient=@IdClient, NomClient=@NomClient, " &
                    "IdChantier=@IdChantier, NomChantier=@NomChantier, " &
                    "CodePompe=@CodePompe, NomPompe=@NomPompe, " &
                    "IdChauffeurPompe=@IdChauffeur, NomChauffeurPompe=@NomChauffeur, " &
                    "VolumeM3=@VolumeM3, HeureDébut=@HeureDébut, HeureFin=@HeureFin, " &
                    "DureePompageMin=@Duree, PUHT=@PUHT, MontantHT=@MontantHT, " &
                    "TauxTVA=@TauxTVA, MontantTTC=@MontantTTC, " &
                    "Statut=@Statut, Remarques=@Remarques " &
                    "WHERE IdBP=@IdBP",
                    Sub(cmd)
                        AjouterParams(cmd)
                        cmd.Parameters.Add("@IdBP", SqlDbType.Int).Value = CInt(txtIdBP.Text)
                    End Sub)
                MessageBox.Show("✅ Bon de pompage modifié.")
            End If

            ChargerBonsPompage()
            ViderChamps()

        Catch ex As Exception
            MessageBox.Show("Erreur enregistrement : " & ex.Message)
        End Try
    End Sub

    Private Sub btnSupprimer_Click(sender As Object, e As EventArgs) Handles btnSupprimer.Click
        If txtIdBP.Text = "" Then
            MessageBox.Show("Sélectionnez d'abord un BP.") : Exit Sub
        End If
        If MessageBox.Show("Supprimer ce bon de pompage ?", "Confirmation",
                           MessageBoxButtons.YesNo, MessageBoxIcon.Question) = DialogResult.No Then Exit Sub
        Try
            ModuleDb.Executer(
                "DELETE FROM dbo.BonsPompage WHERE IdBP=@Id",
                Sub(cmd) cmd.Parameters.Add("@Id", SqlDbType.Int).Value = CInt(txtIdBP.Text))
            MessageBox.Show("BP supprimé.")
            ChargerBonsPompage()
            ViderChamps()
        Catch ex As Exception
            MessageBox.Show("Erreur suppression : " & ex.Message)
        End Try
    End Sub

    ' ═══════════════════════════════════════════════════════════════════
    '  NAVIGATION DATE
    ' ═══════════════════════════════════════════════════════════════════

    Private Sub btnJourMoins_Click(sender As Object, e As EventArgs) Handles btnJourMoins.Click
        dtpDate.Value = dtpDate.Value.AddDays(-1)
        ChargerBonsPompage()
    End Sub

    Private Sub btnJourPlus_Click(sender As Object, e As EventArgs) Handles btnJourPlus.Click
        dtpDate.Value = dtpDate.Value.AddDays(1)
        ChargerBonsPompage()
    End Sub

    Private Sub dtpDate_ValueChanged(sender As Object, e As EventArgs) Handles dtpDate.ValueChanged
        ChargerBonsPompage()
    End Sub

    Private Sub btnActualiser_Click(sender As Object, e As EventArgs) Handles btnActualiser.Click
        ChargerBonsPompage()
    End Sub

    ' ═══════════════════════════════════════════════════════════════════
    '  CALCUL MONTANTS
    ' ═══════════════════════════════════════════════════════════════════

    Private Sub CalculerMontants()
        Try
            Dim vol As Decimal = ParseDecimal(txtVolumeM3.Text)
            Dim puht As Decimal = ParseDecimal(txtPUHT.Text)
            Dim tva As Decimal = ParseDecimal(txtTauxTVA.Text)
            Dim ht As Decimal = vol * puht
            Dim ttc As Decimal = ht * (1 + tva / 100)
            txtMontantHT.Text = ht.ToString("N2")
            txtMontantTTC.Text = ttc.ToString("N2")
            Dim deb As TimeSpan
            Dim fin As TimeSpan
            If TimeSpan.TryParse(txtHeureDébut.Text, deb) AndAlso
               TimeSpan.TryParse(txtHeureFin.Text, fin) Then
                txtDuree.Text = CInt((fin - deb).TotalMinutes).ToString()
            End If
        Catch
        End Try
    End Sub

    Private Sub txtVolumeM3_Leave(s As Object, e As EventArgs) Handles txtVolumeM3.Leave
        CalculerMontants()
    End Sub

    Private Sub txtPUHT_Leave(s As Object, e As EventArgs) Handles txtPUHT.Leave
        CalculerMontants()
    End Sub

    Private Sub txtHeureFin_Leave(s As Object, e As EventArgs) Handles txtHeureFin.Leave
        CalculerMontants()
    End Sub

    ' ═══════════════════════════════════════════════════════════════════
    '  HELPERS
    ' ═══════════════════════════════════════════════════════════════════

    Private Sub AjouterParams(cmd As SqlCommand)
        cmd.Parameters.Add("@NumeroBP", SqlDbType.NVarChar, 30).Value = txtNumeroBP.Text.Trim()
        cmd.Parameters.Add("@DateHeure", SqlDbType.DateTime).Value = dtpDate.Value.Date
        cmd.Parameters.Add("@IdClient", SqlDbType.Int).Value = _idClientCourant
        cmd.Parameters.Add("@NomClient", SqlDbType.NVarChar, 150).Value = txtNomClient.Text.Trim()
        cmd.Parameters.Add("@IdChantier", SqlDbType.Int).Value = If(_idChantierCourant = 0, DBNull.Value, CObj(_idChantierCourant))
        cmd.Parameters.Add("@NomChantier", SqlDbType.NVarChar, 150).Value = txtNomChantier.Text.Trim()
        cmd.Parameters.Add("@CodePompe", SqlDbType.NVarChar, 50).Value = txtCodePompe.Text.Trim()
        cmd.Parameters.Add("@NomPompe", SqlDbType.NVarChar, 100).Value = txtNomPompe.Text.Trim()
        cmd.Parameters.Add("@IdChauffeur", SqlDbType.Int).Value = If(_idChauffeurPompe = 0, DBNull.Value, CObj(_idChauffeurPompe))
        cmd.Parameters.Add("@NomChauffeur", SqlDbType.NVarChar, 150).Value = txtChauffeur.Text.Trim()
        cmd.Parameters.Add("@VolumeM3", SqlDbType.Decimal).Value = ParseDecimal(txtVolumeM3.Text)
        cmd.Parameters.Add("@HeureDébut", SqlDbType.Time).Value = ParseHeure(txtHeureDébut.Text)
        cmd.Parameters.Add("@HeureFin", SqlDbType.Time).Value = ParseHeure(txtHeureFin.Text)
        cmd.Parameters.Add("@Duree", SqlDbType.Int).Value = ParseInt(txtDuree.Text)
        cmd.Parameters.Add("@PUHT", SqlDbType.Decimal).Value = ParseDecimal(txtPUHT.Text)
        cmd.Parameters.Add("@MontantHT", SqlDbType.Decimal).Value = ParseDecimal(txtMontantHT.Text)
        cmd.Parameters.Add("@TauxTVA", SqlDbType.Decimal).Value = ParseDecimal(txtTauxTVA.Text)
        cmd.Parameters.Add("@MontantTTC", SqlDbType.Decimal).Value = ParseDecimal(txtMontantTTC.Text)
        cmd.Parameters.Add("@Statut", SqlDbType.NVarChar, 30).Value = cmbStatut1.Text
        cmd.Parameters.Add("@Remarques", SqlDbType.NVarChar, 500).Value = txtRemarques.Text.Trim()
        cmd.Parameters.Add("@Operateur", SqlDbType.NVarChar, 100).Value = NomUtilisateur
    End Sub

    Private Function ParseDecimal(txt As String) As Decimal
        Dim v As Decimal
        Decimal.TryParse(txt.Replace(",", "."),
                         Globalization.NumberStyles.Any,
                         Globalization.CultureInfo.InvariantCulture, v)
        Return v
    End Function

    Private Function ParseInt(txt As String) As Integer
        Dim v As Integer
        Integer.TryParse(txt, v)
        Return v
    End Function

    Private Function ParseHeure(txt As String) As Object
        If txt.Trim = "" Then Return DBNull.Value
        Dim t As TimeSpan
        If TimeSpan.TryParse(txt.Trim, t) Then Return t
        Return DBNull.Value
    End Function

    Private Sub ViderChamps()
        txtIdBP.Text = "" : txtNumeroBP.Text = "" : txtNomClient.Text = ""
        txtNomChantier.Text = "" : txtCodePompe.Text = "" : txtNomPompe.Text = ""
        txtChauffeur.Text = "" : txtVolumeM3.Text = "0.00"
        txtHeureDébut.Text = "" : txtHeureFin.Text = "" : txtDuree.Text = "0"
        txtPUHT.Text = "0.00" : txtMontantHT.Text = "0.00"
        txtTauxTVA.Text = "20.00" : txtMontantTTC.Text = "0.00"
        txtRemarques.Text = "" : cmbStatut1.SelectedIndex = 0
        _idBPCourant = 0 : _idClientCourant = 0
        _idChantierCourant = 0 : _idChauffeurPompe = 0
    End Sub

End Class