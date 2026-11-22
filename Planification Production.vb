Imports Microsoft.Data.SqlClient
Imports System.Data

Public Class Planification_Production

    ' ═══════════════════════════════════════════════════════════════════
    '  LOAD
    ' ═══════════════════════════════════════════════════════════════════

    Private Sub Planification_Production_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        InitialiserFiltres()
        StyleGrille()
        ChargerProduction()
    End Sub

    ' ═══════════════════════════════════════════════════════════════════
    '  INITIALISATION
    ' ═══════════════════════════════════════════════════════════════════

    Private Sub InitialiserFiltres()
        dtpDateDebut.Value = Date.Today
        dtpDateFin.Value = Date.Today.AddDays(7)

        cmbStatut.Items.Clear()
        cmbStatut.Items.Add("Tous")
        cmbStatut.Items.Add("Créé")
        cmbStatut.Items.Add("En cours")
        cmbStatut.Items.Add("Terminé")
        cmbStatut.SelectedIndex = 0
    End Sub

    Private Sub StyleGrille()
        With DataGridViewProduction
            .ReadOnly = True
            .AllowUserToAddRows = False
            .AllowUserToDeleteRows = False
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
            .DefaultCellStyle.Font = New Font("Arial", 9)
            .RowTemplate.Height = 28
            .BackgroundColor = Color.White
            .BorderStyle = BorderStyle.None
            .AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(245, 248, 255)
        End With
    End Sub

    ' ═══════════════════════════════════════════════════════════════════
    '  CHARGEMENT PRINCIPAL
    ' ═══════════════════════════════════════════════════════════════════

    Private Sub ChargerProduction()
        Try
            Dim sqlStatut As String = ""
            If cmbStatut.Text <> "Tous" AndAlso cmbStatut.Text.Trim <> "" Then
                sqlStatut = " AND StatutPlanning = @Statut"
            End If

            Dim dt As DataTable = ModuleDb.Lire(
                "SELECT " &
                "IdPlanning AS [N°], " &
                "CONVERT(VARCHAR(10), DatePlanning, 103) AS [DATE], " &
                "CONVERT(VARCHAR(5), HeureFabrication, 108) AS [FAB], " &
                "CONVERT(VARCHAR(5), HeureLivraison, 108) AS [LIV], " &
                "NomClient AS [CLIENT], " &
                "NomChantier AS [CHANTIER], " &
                "CodeFormule AS [FORMULE], " &
                "QuantiteTotale AS [QTE], " &
                "QuantiteLivree AS [LIVRÉ], " &
                "Reste AS [RESTE], " &
                "CASE WHEN Complement=1 THEN 'OUI' ELSE '' END AS [PL], " &
                "CASE WHEN AConfirmer=1 THEN 'OUI' ELSE '' END AS [AC], " &
                "StatutPlanning AS [STATUT], " &
                "ISNULL(Remarque,'') AS [REMARQUE] " &
                "FROM dbo.vw_ChargePlanning " &
                "WHERE DatePlanning BETWEEN @DateDebut AND @DateFin" &
                sqlStatut & " " &
                "ORDER BY DatePlanning, HeureFabrication, IdPlanning",
                Sub(cmd)
                    cmd.Parameters.Add("@DateDebut", SqlDbType.Date).Value = dtpDateDebut.Value.Date
                    cmd.Parameters.Add("@DateFin", SqlDbType.Date).Value = dtpDateFin.Value.Date
                    If sqlStatut <> "" Then
                        cmd.Parameters.Add("@Statut", SqlDbType.NVarChar, 30).Value = cmbStatut.Text.Trim()
                    End If
                End Sub)

            DataGridViewProduction.DataSource = dt
            AppliquerColonnes()
            MettreAJourTotaux(dt)
            Me.BeginInvoke(New Action(Sub() ColorerLignes()))

        Catch ex As Exception
            MessageBox.Show("Erreur chargement production : " & ex.Message)
        End Try
    End Sub

    Private Sub AppliquerColonnes()
        If DataGridViewProduction.Columns.Count = 0 Then Exit Sub
        Dim largeurs = New Dictionary(Of String, Integer) From {
            {"N°", 45}, {"DATE", 90}, {"FAB", 55}, {"LIV", 55},
            {"CLIENT", 160}, {"CHANTIER", 160}, {"FORMULE", 80},
            {"QTE", 65}, {"LIVRÉ", 65}, {"RESTE", 65},
            {"PL", 40}, {"AC", 40}, {"STATUT", 90}, {"REMARQUE", 130}
        }
        For Each kvp In largeurs
            If DataGridViewProduction.Columns.Contains(kvp.Key) Then
                DataGridViewProduction.Columns(kvp.Key).Width = kvp.Value
            End If
        Next
    End Sub

    Private Sub MettreAJourTotaux(dt As DataTable)
        Dim totalQte As Decimal = 0D
        Dim totalLivre As Decimal = 0D
        Dim totalReste As Decimal = 0D

        For Each row As DataRow In dt.Rows
            If Not IsDBNull(row("QTE")) Then totalQte += Convert.ToDecimal(row("QTE"))
            If Not IsDBNull(row("LIVRÉ")) Then totalLivre += Convert.ToDecimal(row("LIVRÉ"))
            If Not IsDBNull(row("RESTE")) Then totalReste += Convert.ToDecimal(row("RESTE"))
        Next

        lblNbPlannings.Text = "Plannings : " & dt.Rows.Count
        lblTotalQte.Text = "Total QTE : " & totalQte.ToString("0.00") & " m³"
        lblTotalLivre.Text = "Livré : " & totalLivre.ToString("0.00") & " m³"
        lblTotalReste.Text = "Reste : " & totalReste.ToString("0.00") & " m³"
    End Sub

    ' ═══════════════════════════════════════════════════════════════════
    '  COLORATION LIGNES PAR STATUT
    ' ═══════════════════════════════════════════════════════════════════

    Private Sub ColorerLignes()
        If DataGridViewProduction.Columns.Count = 0 Then Exit Sub
        If Not DataGridViewProduction.Columns.Contains("STATUT") Then Exit Sub

        For Each row As DataGridViewRow In DataGridViewProduction.Rows
            If row.IsNewRow Then Continue For
            Dim statut As String = If(row.Cells("STATUT").Value, "").ToString().ToUpper()
            Dim reste As Decimal = 0D
            Decimal.TryParse(If(row.Cells("RESTE").Value, "0").ToString(), reste)

            Dim couleur As Color
            Select Case statut
                Case "TERMINÉ", "TERMINE"
                    couleur = Color.FromArgb(210, 255, 210)   ' Vert
                Case "EN COURS"
                    couleur = Color.FromArgb(180, 220, 255)   ' Bleu
                Case "CRÉÉ", "CREE"
                    couleur = If(reste > 0,
                        Color.FromArgb(255, 245, 180),        ' Jaune
                        Color.White)
                Case Else
                    couleur = Color.White
            End Select

            row.DefaultCellStyle.BackColor = couleur
            row.DefaultCellStyle.ForeColor = Color.Black
        Next
        DataGridViewProduction.Refresh()
    End Sub

    ' ═══════════════════════════════════════════════════════════════════
    '  NAVIGATION / FILTRES
    ' ═══════════════════════════════════════════════════════════════════

    Private Sub btnActualiser_Click(sender As Object, e As EventArgs) Handles btnActualiser.Click
        ChargerProduction()
    End Sub

    Private Sub btnSemainePrecedente_Click(sender As Object, e As EventArgs) Handles btnSemainePrecedente.Click
        dtpDateDebut.Value = dtpDateDebut.Value.AddDays(-7)
        dtpDateFin.Value = dtpDateFin.Value.AddDays(-7)
        ChargerProduction()
    End Sub

    Private Sub btnSemaineSuivante_Click(sender As Object, e As EventArgs) Handles btnSemaineSuivante.Click
        dtpDateDebut.Value = dtpDateDebut.Value.AddDays(7)
        dtpDateFin.Value = dtpDateFin.Value.AddDays(7)
        ChargerProduction()
    End Sub

    Private Sub btnAujourdhui_Click(sender As Object, e As EventArgs) Handles btnAujourdhui.Click
        dtpDateDebut.Value = Date.Today
        dtpDateFin.Value = Date.Today.AddDays(7)
        ChargerProduction()
    End Sub

    Private Sub cmbStatut_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cmbStatut.SelectedIndexChanged
        ChargerProduction()
    End Sub

    Private Sub dtpDateDebut_ValueChanged(sender As Object, e As EventArgs) Handles dtpDateDebut.ValueChanged
        ChargerProduction()
    End Sub

    Private Sub dtpDateFin_ValueChanged(sender As Object, e As EventArgs) Handles dtpDateFin.ValueChanged
        ChargerProduction()
    End Sub

    ' ═══════════════════════════════════════════════════════════════════
    '  ACTIONS SUR PLANNING SÉLECTIONNÉ
    ' ═══════════════════════════════════════════════════════════════════

    Private Sub btnOuvrir_Click(sender As Object, e As EventArgs) Handles btnOuvrir.Click
        If Not VerifierSelection() Then Exit Sub
        Dim f As New Planification_Demands
        f.IdPlanningEnCours = LireIdSelectionne()
        f.ShowDialog()
        ChargerProduction()
    End Sub

    Private Sub btnDemarrer_Click(sender As Object, e As EventArgs) Handles btnDemarrer.Click
        If Not VerifierSelection() Then Exit Sub
        Dim id As Integer = LireIdSelectionne()
        If MessageBox.Show("Démarrer la fabrication pour ce planning ?", "Confirmation",
                           MessageBoxButtons.YesNo, MessageBoxIcon.Question) = DialogResult.No Then Exit Sub
        Try
            Dim statut As String = ModuleDb.LireValeur(Of String)(
                "SELECT StatutPlanning FROM dbo.Planning_journalier WHERE IdPlanning=@Id",
                Sub(cmd) cmd.Parameters.Add("@Id", SqlDbType.Int).Value = id)

            If statut IsNot Nothing AndAlso
               (statut.ToUpper = "TERMINÉ" OrElse statut.ToUpper = "TERMINE") Then
                MessageBox.Show("Ce planning est déjà terminé.")
                Exit Sub
            End If

            ModuleDb.Executer(
                "UPDATE dbo.Planning_journalier SET " &
                "StatutPlanning='En cours', DateModification=GETDATE() " &
                "WHERE IdPlanning=@Id",
                Sub(cmd) cmd.Parameters.Add("@Id", SqlDbType.Int).Value = id)

            MessageBox.Show("✅ Planning N° " & id & " → En cours.")
            ChargerProduction()

        Catch ex As Exception
            MessageBox.Show("Erreur : " & ex.Message)
        End Try
    End Sub

    Private Sub btnTerminer_Click(sender As Object, e As EventArgs) Handles btnTerminer.Click
        If Not VerifierSelection() Then Exit Sub
        Dim id As Integer = LireIdSelectionne()
        If MessageBox.Show("Marquer ce planning comme Terminé ?", "Confirmation",
                           MessageBoxButtons.YesNo, MessageBoxIcon.Question) = DialogResult.No Then Exit Sub
        Try
            ModuleDb.Executer(
                "UPDATE dbo.Planning_journalier SET " &
                "StatutPlanning='Terminé', DateModification=GETDATE() " &
                "WHERE IdPlanning=@Id",
                Sub(cmd) cmd.Parameters.Add("@Id", SqlDbType.Int).Value = id)

            MessageBox.Show("✅ Planning N° " & id & " → Terminé.")
            ChargerProduction()

        Catch ex As Exception
            MessageBox.Show("Erreur : " & ex.Message)
        End Try
    End Sub

    Private Sub btnSupprimer_Click(sender As Object, e As EventArgs)
        If Not VerifierSelection() Then Exit Sub
        Dim id = LireIdSelectionne()
        If MessageBox.Show("Supprimer ce planning ?", "Confirmation",
                           MessageBoxButtons.YesNo, MessageBoxIcon.Question) = DialogResult.No Then Exit Sub
        Try
            Executer(
                "DELETE FROM dbo.Planning_ProduitsServices WHERE IdPlanning=@Id",
                Sub(cmd) cmd.Parameters.Add("@Id", SqlDbType.Int).Value = id)
            Executer(
                "DELETE FROM dbo.Planning_journalier WHERE IdPlanning=@Id",
                Sub(cmd) cmd.Parameters.Add("@Id", SqlDbType.Int).Value = id)

            MessageBox.Show("Planning supprimé.")
            ChargerProduction()

        Catch ex As Exception
            MessageBox.Show("Erreur suppression : " & ex.Message)
        End Try
    End Sub

    Private Sub btnNouveauPlanning_Click(sender As Object, e As EventArgs) Handles btnNouveauPlanning.Click
        Dim f As New Planification_Demands
        f.ShowDialog()
        ChargerProduction()
    End Sub

    Private Sub DataGridViewProduction_CellDoubleClick(sender As Object, e As DataGridViewCellEventArgs) Handles DataGridViewProduction.CellDoubleClick
        If e.RowIndex < 0 Then Exit Sub
        Dim f As New Planification_Demands
        f.IdPlanningEnCours = LireIdSelectionne()
        f.ShowDialog()
        ChargerProduction()
    End Sub

    ' ═══════════════════════════════════════════════════════════════════
    '  HELPERS
    ' ═══════════════════════════════════════════════════════════════════

    Private Function VerifierSelection() As Boolean
        If DataGridViewProduction.CurrentRow Is Nothing Then
            MessageBox.Show("Sélectionnez d'abord un planning.")
            Return False
        End If
        Return True
    End Function

    Private Function LireIdSelectionne() As Integer
        Return CInt(DataGridViewProduction.CurrentRow.Cells("N°").Value)
    End Function


End Class