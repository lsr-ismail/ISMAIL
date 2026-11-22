Imports Microsoft.Data.SqlClient
Imports System.Data

Public Class ListedeBL

    Private Sub ListedeBL_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        ChargerBL()
    End Sub

    ' ── Chargement de la liste ──────────────────────────────────


    Private Sub DateTimePicker1_ValueChanged(sender As Object, e As EventArgs) Handles dtpDebut.ValueChanged

    End Sub

    ' ── Mise en forme de la grille ──────────────────────────────
    Private Sub FormaterGrille()
        DataGridView1.ReadOnly = True
        DataGridView1.AllowUserToAddRows = False
        DataGridView1.SelectionMode = DataGridViewSelectionMode.FullRowSelect
        DataGridView1.MultiSelect = False
        DataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill

        If DataGridView1.Columns.Count = 0 Then Exit Sub

        Dim entetes = New Dictionary(Of String, String) From {
            {"NumeroBL", "N° BL"}, {"DateHeure", "Date"},
            {"NomClient", "Client"}, {"NomChantier", "Chantier"}, {"NumeroCommande", "N° Commande"},
            {"CodeFormule", "Formule"}, {"NomChauffeur", "Chauffeur"},
            {"CodeCamion", "Camion"}, {"VolumeM3", "Volume m³"},
            {"Statut", "Statut"}, {"Operateur", "Opérateur"}}
        For Each kv In entetes
            If DataGridView1.Columns.Contains(kv.Key) Then
                DataGridView1.Columns(kv.Key).HeaderText = kv.Value
            End If
        Next

        ' Couleur selon le statut : vert = livré, orange = en cours
        For Each row As DataGridViewRow In DataGridView1.Rows
            If row.IsNewRow Then Continue For
            Dim statut As String = If(IsDBNull(row.Cells("Statut").Value), "", row.Cells("Statut").Value.ToString())
            If statut = "LIVRE" Then
                row.DefaultCellStyle.BackColor = Color.FromArgb(220, 255, 220)
            ElseIf statut = "EN_COURS" Then
                row.DefaultCellStyle.BackColor = Color.FromArgb(255, 245, 215)
            End If
        Next
    End Sub

    ' ── Recherche en direct ─────────────────────────────────────
    Private Sub txtRECHERCHE_TextChanged(sender As Object, e As EventArgs) Handles txtRECHERCHE.TextChanged
        ChargerBL(txtRECHERCHE.Text)
    End Sub



    Private Sub btnActualiser_Click(sender As Object, e As EventArgs) Handles btnActualiser.Click
        txtRECHERCHE.Clear()
        ChargerBL()
    End Sub
    Private Sub Imprimer_Click(sender As Object, e As EventArgs) Handles Imprimer.Click
        If DataGridView1.CurrentRow Is Nothing OrElse
       IsDBNull(DataGridView1.CurrentRow.Cells("NumeroBL").Value) Then
            MessageBox.Show("Sélectionnez un BL à imprimer.")
            Exit Sub
        End If

        Dim numBL As String = DataGridView1.CurrentRow.Cells("NumeroBL").Value.ToString()

        ' Demande : avec ou sans le détail de dosage ?
        Dim rep As DialogResult = MessageBox.Show(
        "Imprimer le BL AVEC le détail des gâchées ?" & vbCrLf & vbCrLf &
        "• Oui  = avec détail" & vbCrLf &
        "• Non  = sans détail" & vbCrLf &
        "• Annuler = ne pas imprimer",
        "Impression du BL " & numBL,
        MessageBoxButtons.YesNoCancel,
        MessageBoxIcon.Question)

        Select Case rep
            Case DialogResult.Yes
                ModuleImpressionBL.ImprimerBL(numBL, avecDetail:=True)
            Case DialogResult.No
                ModuleImpressionBL.ImprimerBL(numBL, avecDetail:=False)
            Case DialogResult.Cancel
                ' on ne fait rien
        End Select
    End Sub
    ' ── Chargement avec recherche + période ─────────────────────
    Private Sub ChargerBL(Optional filtre As String = "")
        Try
            Dim sql As String =
                "SELECT NumeroBL, DateHeure, NomClient, NomChantier, " &
                "CodeFormule, NomChauffeur, CodeCamion, VolumeM3, Statut, Operateur " & "NumeroCommande " &
                "FROM dbo.BonsLivraison " &
                "WHERE CAST(DateHeure AS DATE) BETWEEN @debut AND @fin "

            If filtre.Trim <> "" Then
                sql &= "AND (NumeroBL LIKE @mot OR NomClient LIKE @mot " &
                       "OR NomChantier LIKE @mot OR CodeCamion LIKE @mot) "
            End If
            sql &= "ORDER BY DateHeure DESC"

            Dim dt As DataTable = ModuleDb.Lire(sql,
                Sub(cmd)
                    cmd.Parameters.Add("@debut", SqlDbType.Date).Value = dtpDebut.Value.Date
                    cmd.Parameters.Add("@fin", SqlDbType.Date).Value = dtpFin.Value.Date
                    If filtre.Trim <> "" Then
                        cmd.Parameters.Add("@mot", SqlDbType.NVarChar, 200).Value = "%" & filtre.Trim() & "%"
                    End If
                End Sub)

            DataGridView1.DataSource = dt
            FormaterGrille()

        Catch ex As Exception
            MessageBox.Show("Erreur chargement BL : " & ex.Message)
        End Try
    End Sub
    Private Sub RECHERCHE_Click(sender As Object, e As EventArgs) Handles RECHERCHE.Click
        ChargerBL(txtRECHERCHE.Text)
    End Sub
    Private Sub dtpDebut_ValueChanged(sender As Object, e As EventArgs) Handles dtpDebut.ValueChanged
        ChargerBL(txtRECHERCHE.Text)
    End Sub

    Private Sub dtpFin_ValueChanged(sender As Object, e As EventArgs) Handles dtpFin.ValueChanged
        ChargerBL(txtRECHERCHE.Text)
    End Sub
    ' ── Boutons date de DÉBUT ───────────────────────────────────
    Private Sub btnDebutPlus_Click(sender As Object, e As EventArgs) Handles btnDebutPlus.Click
        dtpDebut.Value = dtpDebut.Value.AddDays(1)
    End Sub

    Private Sub btnDebutMoins_Click(sender As Object, e As EventArgs) Handles btnDebutMoins.Click
        dtpDebut.Value = dtpDebut.Value.AddDays(-1)
    End Sub

    ' ── Boutons date de FIN ─────────────────────────────────────
    Private Sub btnFinPlus_Click(sender As Object, e As EventArgs) Handles btnFinPlus.Click
        dtpFin.Value = dtpFin.Value.AddDays(1)
    End Sub

    Private Sub btnFinMoins_Click(sender As Object, e As EventArgs) Handles btnFinMoins.Click
        dtpFin.Value = dtpFin.Value.AddDays(-1)
    End Sub
End Class