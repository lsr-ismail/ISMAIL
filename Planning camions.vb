Imports Microsoft.Data.SqlClient
Imports System.Data

Public Class Planning_camions

    Private _idPlanningCamionCourant As Integer = 0

    ' ═══════════════════════════════════════════════════════════════════
    '  LOAD
    ' ═══════════════════════════════════════════════════════════════════

    Private Sub Planning_camions_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        dtpDatePlanning.Value = Date.Today
        InitialiserComboStatut()
        StyleGrille()
        ChargerPlanningCamions()
    End Sub

    Private Sub StyleGrille()
        With DataGridView1
            .ReadOnly = True
            .AllowUserToAddRows = False
            .SelectionMode = DataGridViewSelectionMode.FullRowSelect
            .MultiSelect = False
            .AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.None
            .RowHeadersVisible = False
            .EnableHeadersVisualStyles = False
            .ColumnHeadersDefaultCellStyle.BackColor = Color.LightGray
            .ColumnHeadersDefaultCellStyle.ForeColor = Color.Black
            .DefaultCellStyle.SelectionBackColor = Color.RoyalBlue
            .DefaultCellStyle.SelectionForeColor = Color.White
        End With
    End Sub

    Private Sub InitialiserComboStatut()
        cmbStatut.Items.Clear()
        cmbStatut.Items.AddRange({"En attente", "En cours", "Terminé", "Annulé"})
        cmbStatut.SelectedIndex = 0
    End Sub

    ' ═══════════════════════════════════════════════════════════════════
    '  CHARGEMENT
    ' ═══════════════════════════════════════════════════════════════════

    Private Sub ChargerPlanningCamions()
        Try
            Dim dt As DataTable = ModuleDb.Lire(
                "SELECT " &
                "pc.IdPlanningCamion, " &
                "CONVERT(VARCHAR(5), pc.HeureDepart, 108) AS DEPART, " &
                "CONVERT(VARCHAR(5), pc.HeureRetour, 108) AS RETOUR, " &
                "ca.CodeCamion AS CAMION, ca.NomCamion AS NOM_CAMION, ca.Capacite AS CAPACITE, " &
                "cf.NomChauffeur AS CHAUFFEUR, " &
                "cl.NomClient AS CLIENT, ch.NomChantier AS CHANTIER, " &
                "pc.NumeroBL AS BL, pc.QuantiteM3 AS M3, pc.NumeroTour AS TOUR, " &
                "pc.Statut, pc.Observation " &
                "FROM dbo.Planning_camions pc " &
                "JOIN dbo.Camions ca ON ca.IdCamion = pc.IdCamion " &
                "LEFT JOIN dbo.Chauffeurs cf ON cf.IdChauffeur = pc.IdChauffeur " &
                "LEFT JOIN dbo.Planning_journalier pj ON pj.IdPlanning = pc.IdPlanning " &
                "LEFT JOIN dbo.Clients cl ON cl.IdClient = pj.IdClient " &
                "LEFT JOIN dbo.Chantier ch ON ch.IdChantier = pj.IdChantier " &
                "WHERE pc.DatePlanning = @DatePlanning " &
                "ORDER BY pc.HeureDepart, ca.CodeCamion",
                Sub(cmd)
                    cmd.Parameters.Add("@DatePlanning", SqlDbType.Date).Value = dtpDatePlanning.Value.Date
                End Sub)

            DataGridView1.DataSource = dt

            If DataGridView1.Columns.Contains("IdPlanningCamion") Then
                DataGridView1.Columns("IdPlanningCamion").Visible = False
            End If

            Dim entetes = New Dictionary(Of String, String) From {
                {"DEPART", "Départ"}, {"RETOUR", "Retour"},
                {"CAMION", "Code"}, {"NOM_CAMION", "Camion"},
                {"CAPACITE", "Capacité"}, {"CHAUFFEUR", "Chauffeur"},
                {"CLIENT", "Client"}, {"CHANTIER", "Chantier"},
                {"BL", "N° BL"}, {"M3", "Qté M³"},
                {"TOUR", "Tour"}, {"Statut", "Statut"}, {"Observation", "Observation"}
            }
            For Each kv In entetes
                If DataGridView1.Columns.Contains(kv.Key) Then
                    DataGridView1.Columns(kv.Key).HeaderText = kv.Value
                End If
            Next

            Dim largeurs = New Dictionary(Of String, Integer) From {
                {"DEPART", 70}, {"RETOUR", 70}, {"CAMION", 70}, {"NOM_CAMION", 120},
                {"CAPACITE", 60}, {"CHAUFFEUR", 120}, {"CLIENT", 130}, {"CHANTIER", 130},
                {"BL", 90}, {"M3", 60}, {"TOUR", 50}, {"Statut", 90}, {"Observation", 150}
            }
            For Each kv In largeurs
                If DataGridView1.Columns.Contains(kv.Key) Then
                    DataGridView1.Columns(kv.Key).Width = kv.Value
                End If
            Next

            For Each row As DataGridViewRow In DataGridView1.Rows
                If Not row.IsNewRow Then
                    Select Case LireTexte(row, "Statut").ToUpper()
                        Case "TERMINÉ", "TERMINE"
                            row.DefaultCellStyle.BackColor = Color.FromArgb(210, 255, 210)
                        Case "EN COURS"
                            row.DefaultCellStyle.BackColor = Color.FromArgb(180, 220, 255)
                        Case "ANNULÉ", "ANNULE"
                            row.DefaultCellStyle.BackColor = Color.FromArgb(255, 210, 210)
                        Case Else
                            row.DefaultCellStyle.BackColor = Color.White
                    End Select
                End If
            Next

            lblNombre.Text = "Total : " & dt.Rows.Count & " rotation(s)"

        Catch ex As Exception
            MessageBox.Show("Erreur chargement : " & ex.Message)
        End Try
    End Sub

    ' ═══════════════════════════════════════════════════════════════════
    '  CLIC GRILLE
    ' ═══════════════════════════════════════════════════════════════════

    Private Sub DataGridView1_CellClick(sender As Object, e As DataGridViewCellEventArgs) Handles DataGridView1.CellClick
        If e.RowIndex < 0 Then Exit Sub
        Try
            Dim row As DataGridViewRow = DataGridView1.Rows(e.RowIndex)
            _idPlanningCamionCourant = CInt(row.Cells("IdPlanningCamion").Value)
            txtCodeCamion.Text = LireTexte(row, "CAMION")
            txtNomCamion.Text = LireTexte(row, "NOM_CAMION")
            txtChauffeur.Text = LireTexte(row, "CHAUFFEUR")
            txtHeureDepart.Text = LireTexte(row, "DEPART")
            txtHeureRetour.Text = LireTexte(row, "RETOUR")
            txtNumeroBL.Text = LireTexte(row, "BL")
            txtQuantiteM3.Text = LireTexte(row, "M3")
            txtNumeroTour.Text = LireTexte(row, "TOUR")
            txtObservation.Text = LireTexte(row, "Observation")
            cmbStatut.Text = LireTexte(row, "Statut")
        Catch ex As Exception
            MessageBox.Show("Erreur sélection : " & ex.Message)
        End Try
    End Sub

    ' ═══════════════════════════════════════════════════════════════════
    '  AJOUTER
    ' ═══════════════════════════════════════════════════════════════════

    Private Sub btnAjouter_Click(sender As Object, e As EventArgs) Handles btnAjouter.Click
        If txtCodeCamion.Text.Trim = "" Then
            MessageBox.Show("Sélectionner un camion.")
            Exit Sub
        End If
        Try
            Dim idCamion As Integer = ModuleDb.LireValeur(Of Integer)(
                "SELECT IdCamion FROM dbo.Camions WHERE CodeCamion=@Code",
                Sub(cmd) cmd.Parameters.Add("@Code", SqlDbType.NVarChar, 50).Value = txtCodeCamion.Text.Trim())

            If idCamion = 0 Then
                MessageBox.Show("Camion introuvable.")
                Exit Sub
            End If

            Dim idChauffeur As Object = DBNull.Value
            If txtChauffeur.Text.Trim <> "" Then
                Dim id As Integer = ModuleDb.LireValeur(Of Integer)(
                    "SELECT IdChauffeur FROM dbo.Chauffeurs WHERE NomChauffeur=@Nom",
                    Sub(cmd) cmd.Parameters.Add("@Nom", SqlDbType.NVarChar, 200).Value = txtChauffeur.Text.Trim())
                If id > 0 Then idChauffeur = id
            End If

            Dim tour As Integer = 1
            Integer.TryParse(txtNumeroTour.Text, tour)

            ModuleDb.Executer(
                "INSERT INTO dbo.Planning_camions " &
                "(DatePlanning, HeureDepart, HeureRetour, IdCamion, IdChauffeur, " &
                "NumeroBL, QuantiteM3, NumeroTour, Observation, Statut) " &
                "VALUES (@DatePlanning, @HeureDepart, @HeureRetour, @IdCamion, @IdChauffeur, " &
                "@NumeroBL, @QuantiteM3, @NumeroTour, @Observation, @Statut)",
                Sub(cmd)
                    cmd.Parameters.Add("@DatePlanning", SqlDbType.Date).Value = dtpDatePlanning.Value.Date
                    cmd.Parameters.Add("@HeureDepart", SqlDbType.Time).Value = ParseHeure(txtHeureDepart.Text)
                    cmd.Parameters.Add("@HeureRetour", SqlDbType.Time).Value = ParseHeure(txtHeureRetour.Text)
                    cmd.Parameters.Add("@IdCamion", SqlDbType.Int).Value = idCamion
                    cmd.Parameters.Add("@IdChauffeur", SqlDbType.Int).Value = idChauffeur
                    cmd.Parameters.Add("@NumeroBL", SqlDbType.NVarChar, 50).Value = If(txtNumeroBL.Text.Trim = "", DBNull.Value, CObj(txtNumeroBL.Text.Trim()))
                    cmd.Parameters.Add("@QuantiteM3", SqlDbType.Decimal).Value = ParseDecimal(txtQuantiteM3.Text)
                    cmd.Parameters.Add("@NumeroTour", SqlDbType.Int).Value = tour
                    cmd.Parameters.Add("@Observation", SqlDbType.NVarChar, 300).Value = If(txtObservation.Text.Trim = "", DBNull.Value, CObj(txtObservation.Text.Trim()))
                    cmd.Parameters.Add("@Statut", SqlDbType.NVarChar, 50).Value = If(cmbStatut.Text.Trim = "", "En attente", cmbStatut.Text.Trim())
                End Sub)

            MessageBox.Show("Rotation ajoutée avec succès.")
            ChargerPlanningCamions()
            ViderChamps()

        Catch ex As Exception
            MessageBox.Show("Erreur ajout : " & ex.Message)
        End Try
    End Sub

    ' ═══════════════════════════════════════════════════════════════════
    '  MODIFIER
    ' ═══════════════════════════════════════════════════════════════════

    Private Sub btnModifier_Click(sender As Object, e As EventArgs) Handles btnModifier.Click
        If _idPlanningCamionCourant = 0 Then
            MessageBox.Show("Sélectionner une ligne à modifier.")
            Exit Sub
        End If
        Try
            Dim tour As Integer = 1
            Integer.TryParse(txtNumeroTour.Text, tour)

            ModuleDb.Executer(
                "UPDATE dbo.Planning_camions SET " &
                "HeureDepart=@HeureDepart, HeureRetour=@HeureRetour, " &
                "NumeroBL=@NumeroBL, QuantiteM3=@QuantiteM3, " &
                "NumeroTour=@NumeroTour, Observation=@Observation, Statut=@Statut " &
                "WHERE IdPlanningCamion=@Id",
                Sub(cmd)
                    cmd.Parameters.Add("@HeureDepart", SqlDbType.Time).Value = ParseHeure(txtHeureDepart.Text)
                    cmd.Parameters.Add("@HeureRetour", SqlDbType.Time).Value = ParseHeure(txtHeureRetour.Text)
                    cmd.Parameters.Add("@NumeroBL", SqlDbType.NVarChar, 50).Value = If(txtNumeroBL.Text.Trim = "", DBNull.Value, CObj(txtNumeroBL.Text.Trim()))
                    cmd.Parameters.Add("@QuantiteM3", SqlDbType.Decimal).Value = ParseDecimal(txtQuantiteM3.Text)
                    cmd.Parameters.Add("@NumeroTour", SqlDbType.Int).Value = tour
                    cmd.Parameters.Add("@Observation", SqlDbType.NVarChar, 300).Value = If(txtObservation.Text.Trim = "", DBNull.Value, CObj(txtObservation.Text.Trim()))
                    cmd.Parameters.Add("@Statut", SqlDbType.NVarChar, 50).Value = cmbStatut.Text.Trim()
                    cmd.Parameters.Add("@Id", SqlDbType.Int).Value = _idPlanningCamionCourant
                End Sub)

            MessageBox.Show("Rotation modifiée avec succès.")
            ChargerPlanningCamions()
            ViderChamps()

        Catch ex As Exception
            MessageBox.Show("Erreur modification : " & ex.Message)
        End Try
    End Sub

    ' ═══════════════════════════════════════════════════════════════════
    '  SUPPRIMER
    ' ═══════════════════════════════════════════════════════════════════

    Private Sub btnSupprimer_Click(sender As Object, e As EventArgs) Handles btnSupprimer.Click
        If _idPlanningCamionCourant = 0 Then
            MessageBox.Show("Sélectionner une ligne à supprimer.")
            Exit Sub
        End If
        If MessageBox.Show("Supprimer cette rotation ?", "Confirmation",
                           MessageBoxButtons.YesNo, MessageBoxIcon.Question) = DialogResult.No Then Exit Sub
        Try
            ModuleDb.Executer(
                "DELETE FROM dbo.Planning_camions WHERE IdPlanningCamion=@Id",
                Sub(cmd) cmd.Parameters.Add("@Id", SqlDbType.Int).Value = _idPlanningCamionCourant)

            MessageBox.Show("Rotation supprimée.")
            ChargerPlanningCamions()
            ViderChamps()

        Catch ex As Exception
            MessageBox.Show("Erreur suppression : " & ex.Message)
        End Try
    End Sub

    ' ═══════════════════════════════════════════════════════════════════
    '  STATUT RAPIDE
    ' ═══════════════════════════════════════════════════════════════════

    Private Sub btnEnCours_Click(sender As Object, e As EventArgs) Handles btnEnCours.Click
        ChangerStatut("En cours")
    End Sub

    Private Sub btnTerminer_Click(sender As Object, e As EventArgs) Handles btnTerminer.Click
        ChangerStatut("Terminé")
    End Sub

    Private Sub ChangerStatut(statut As String)
        If _idPlanningCamionCourant = 0 Then
            MessageBox.Show("Sélectionner une ligne.")
            Exit Sub
        End If
        Try
            ModuleDb.Executer(
                "UPDATE dbo.Planning_camions SET Statut=@Statut WHERE IdPlanningCamion=@Id",
                Sub(cmd)
                    cmd.Parameters.Add("@Statut", SqlDbType.NVarChar, 50).Value = statut
                    cmd.Parameters.Add("@Id", SqlDbType.Int).Value = _idPlanningCamionCourant
                End Sub)
            ChargerPlanningCamions()
        Catch ex As Exception
            MessageBox.Show("Erreur statut : " & ex.Message)
        End Try
    End Sub

    ' ═══════════════════════════════════════════════════════════════════
    '  RECHERCHE CAMION (double-clic)
    ' ═══════════════════════════════════════════════════════════════════

    Private Sub txtCodeCamion_DoubleClick(sender As Object, e As EventArgs) Handles txtCodeCamion.DoubleClick
        Dim f As New FrmRecherche
        f.Initialiser(
            "SELECT IdCamion, CodeCamion, NomCamion, Capacite FROM dbo.Camions",
            "IdCamion", "CodeCamion", "NomCamion")
        f.Text = "Recherche Camion"
        If f.ShowDialog() = DialogResult.OK Then
            txtCodeCamion.Text = f.ValeurSelectionnee
            Try
                Dim dt As DataTable = ModuleDb.Lire(
                    "SELECT NomCamion, Chauffeur FROM dbo.Camions WHERE CodeCamion=@Code",
                    Sub(cmd) cmd.Parameters.Add("@Code", SqlDbType.NVarChar, 50).Value = txtCodeCamion.Text.Trim())

                If dt.Rows.Count > 0 Then
                    txtNomCamion.Text = dt.Rows(0)("NomCamion").ToString()
                    If Not IsDBNull(dt.Rows(0)("Chauffeur")) Then
                        txtChauffeur.Text = dt.Rows(0)("Chauffeur").ToString()
                    End If
                End If
            Catch ex As Exception
                MessageBox.Show("Erreur camion : " & ex.Message)
            End Try
        End If
    End Sub

    ' ═══════════════════════════════════════════════════════════════════
    '  NAVIGATION DATE
    ' ═══════════════════════════════════════════════════════════════════

    Private Sub btnJourPlus_Click(sender As Object, e As EventArgs) Handles btnJourPlus.Click
        dtpDatePlanning.Value = dtpDatePlanning.Value.AddDays(1)
        ChargerPlanningCamions()
    End Sub

    Private Sub btnJourMoins_Click(sender As Object, e As EventArgs) Handles btnJourMoins.Click
        dtpDatePlanning.Value = dtpDatePlanning.Value.AddDays(-1)
        ChargerPlanningCamions()
    End Sub

    Private Sub btnActualiser_Click(sender As Object, e As EventArgs) Handles btnActualiser.Click
        ChargerPlanningCamions()
    End Sub

    Private Sub dtpDatePlanning_ValueChanged(sender As Object, e As EventArgs) Handles dtpDatePlanning.ValueChanged
        ChargerPlanningCamions()
    End Sub

    Private Sub btnNouveau_Click(sender As Object, e As EventArgs) Handles btnNouveau.Click
        ViderChamps()
    End Sub

    Private Sub btnImprimer_Click(sender As Object, e As EventArgs) Handles btnImprimer.Click
        MessageBox.Show("Impression en cours de développement.")
    End Sub

    ' ═══════════════════════════════════════════════════════════════════
    '  HELPERS
    ' ═══════════════════════════════════════════════════════════════════

    Private Function LireTexte(row As DataGridViewRow, col As String) As String
        Return If(IsDBNull(row.Cells(col).Value), "", row.Cells(col).Value.ToString())
    End Function

    Private Function ParseDecimal(txt As String) As Decimal
        Dim v As Decimal
        Decimal.TryParse(txt.Replace(",", "."),
            Globalization.NumberStyles.Any,
            Globalization.CultureInfo.InvariantCulture, v)
        Return v
    End Function

    Private Function ParseHeure(txt As String) As Object
        If txt.Trim = "" Then Return DBNull.Value
        Dim t As TimeSpan
        If TimeSpan.TryParse(txt.Trim, t) Then Return t
        Return DBNull.Value
    End Function

    Private Sub ViderChamps()
        _idPlanningCamionCourant = 0
        txtCodeCamion.Text = "" : txtNomCamion.Text = "" : txtChauffeur.Text = ""
        txtHeureDepart.Text = "" : txtHeureRetour.Text = ""
        txtNumeroBL.Text = "" : txtQuantiteM3.Text = ""
        txtNumeroTour.Text = "" : txtObservation.Text = ""
        cmbStatut.SelectedIndex = 0
    End Sub

End Class