Imports Microsoft.Data.SqlClient
Imports System.Data

Public Class Articles

    Dim ligneSelectionnee As DataGridViewRow = Nothing

    ' ═══════════════════════════════════════════════════════════════════
    '  LOAD
    ' ═══════════════════════════════════════════════════════════════════

    Private Sub Articles_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        ChargerArticles()
        txtIdArticle.Visible = False
    End Sub

    ' ═══════════════════════════════════════════════════════════════════
    '  CHARGEMENT
    ' ═══════════════════════════════════════════════════════════════════

    Private Sub ChargerArticles()
        Try
            Dim dt As DataTable = ModuleDb.Lire(
                "SELECT * FROM Articles ORDER BY IdArticle DESC")

            DataGridView1.Columns.Clear()
            DataGridView1.AutoGenerateColumns = True
            DataGridView1.DataSource = dt
            DataGridView1.SelectionMode = DataGridViewSelectionMode.FullRowSelect
            DataGridView1.MultiSelect = False
            DataGridView1.AllowUserToAddRows = False
            DataGridView1.ReadOnly = True
            DataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill

            For Each col As String In {"IdArticle", "DescriptionArticle", "Remarques",
                                        "NatureService", "CodeExportation", "ServiceTransport",
                                        "Unite", "DerniereModification"}
                If DataGridView1.Columns.Contains(col) Then
                    DataGridView1.Columns(col).Visible = False
                End If
            Next

            If DataGridView1.Columns.Contains("CodeArticle") Then DataGridView1.Columns("CodeArticle").HeaderText = "CODE"
            If DataGridView1.Columns.Contains("NomArticle") Then DataGridView1.Columns("NomArticle").HeaderText = "NOM"
            If DataGridView1.Columns.Contains("Designation") Then DataGridView1.Columns("Designation").HeaderText = "DÉSIGNATION"

        Catch ex As Exception
            MessageBox.Show("Erreur chargement : " & ex.Message)
        End Try
    End Sub

    ' ═══════════════════════════════════════════════════════════════════
    '  CLIC GRILLE
    ' ═══════════════════════════════════════════════════════════════════

    Private Sub DataGridView1_CellClick(sender As Object, e As DataGridViewCellEventArgs) Handles DataGridView1.CellClick
        If e.RowIndex < 0 Then Exit Sub
        Dim row As DataGridViewRow = DataGridView1.Rows(e.RowIndex)
        ligneSelectionnee = row

        txtIdArticle.Text = If(IsDBNull(row.Cells("IdArticle").Value), "", row.Cells("IdArticle").Value.ToString())
        txtCodeArticle.Text = If(IsDBNull(row.Cells("CodeArticle").Value), "", row.Cells("CodeArticle").Value.ToString())
        txtNomArticle.Text = If(IsDBNull(row.Cells("NomArticle").Value), "", row.Cells("NomArticle").Value.ToString())
        txtDesignation.Text = If(IsDBNull(row.Cells("Designation").Value), "", row.Cells("Designation").Value.ToString())
        txtDescriptionArticle.Text = If(IsDBNull(row.Cells("DescriptionArticle").Value), "", row.Cells("DescriptionArticle").Value.ToString())
        txtRemarques.Text = If(IsDBNull(row.Cells("Remarques").Value), "", row.Cells("Remarques").Value.ToString())
        txtNatureService.Text = If(IsDBNull(row.Cells("NatureService").Value), "", row.Cells("NatureService").Value.ToString())
        txtCodeExportation.Text = If(IsDBNull(row.Cells("CodeExportation").Value), "", row.Cells("CodeExportation").Value.ToString())
        txtServiceTransport.Text = If(IsDBNull(row.Cells("ServiceTransport").Value), "", row.Cells("ServiceTransport").Value.ToString())
        txtUnite.Text = If(IsDBNull(row.Cells("Unite").Value), "", row.Cells("Unite").Value.ToString())
        txtDerniereModification.Text = If(IsDBNull(row.Cells("DerniereModification").Value), "",
            CDate(row.Cells("DerniereModification").Value).ToString("yyyy-MM-dd"))
    End Sub

    ' ═══════════════════════════════════════════════════════════════════
    '  AJOUTER
    ' ═══════════════════════════════════════════════════════════════════

    Private Sub btnAjouter_Click(sender As Object, e As EventArgs) Handles btnAjouter.Click
        If txtCodeArticle.Text.Trim = "" OrElse txtNomArticle.Text.Trim = "" Then
            MessageBox.Show("Code et Nom sont obligatoires.")
            Exit Sub
        End If
        Try
            ModuleDb.Executer(
                "INSERT INTO Articles " &
                "(CodeArticle, NomArticle, Designation, DescriptionArticle, Remarques, " &
                "NatureService, CodeExportation, ServiceTransport, Unite, DerniereModification) " &
                "VALUES (@CodeArticle, @NomArticle, @Designation, @DescriptionArticle, @Remarques, " &
                "@NatureService, @CodeExportation, @ServiceTransport, @Unite, @DerniereModification)",
                AddressOf AjouterParametres)

            MessageBox.Show("Article ajouté avec succès.")
            ChargerArticles()
            ViderChampsArticles()

        Catch ex As Exception
            MessageBox.Show("Erreur ajout : " & ex.Message)
        End Try
    End Sub

    ' ═══════════════════════════════════════════════════════════════════
    '  MODIFIER
    ' ═══════════════════════════════════════════════════════════════════

    Private Sub btnModifier_Click(sender As Object, e As EventArgs) Handles btnModifier.Click
        If txtIdArticle.Text = "" Then
            MessageBox.Show("Sélectionne un article.")
            Exit Sub
        End If
        Try
            ModuleDb.Executer(
                "UPDATE Articles SET " &
                "CodeArticle=@CodeArticle, NomArticle=@NomArticle, Designation=@Designation, " &
                "DescriptionArticle=@DescriptionArticle, Remarques=@Remarques, " &
                "NatureService=@NatureService, CodeExportation=@CodeExportation, " &
                "ServiceTransport=@ServiceTransport, Unite=@Unite, " &
                "DerniereModification=@DerniereModification " &
                "WHERE IdArticle=@IdArticle",
                Sub(cmd)
                    cmd.Parameters.Add("@IdArticle", SqlDbType.Int).Value = CInt(txtIdArticle.Text)
                    AjouterParametres(cmd)
                End Sub)

            MessageBox.Show("Article modifié avec succès.")
            ChargerArticles()
            ViderChampsArticles()

        Catch ex As Exception
            MessageBox.Show("Erreur modification : " & ex.Message)
        End Try
    End Sub

    ' ═══════════════════════════════════════════════════════════════════
    '  SUPPRIMER
    ' ═══════════════════════════════════════════════════════════════════

    Private Sub btnSupprimer_Click(sender As Object, e As EventArgs) Handles btnSupprimer.Click
        If txtIdArticle.Text = "" Then
            MessageBox.Show("Sélectionne un article.")
            Exit Sub
        End If
        If MessageBox.Show("Voulez-vous supprimer cet article ?", "Confirmation",
                           MessageBoxButtons.YesNo, MessageBoxIcon.Question) = DialogResult.No Then Exit Sub
        Try
            ModuleDb.Executer(
                "DELETE FROM Articles WHERE IdArticle=@IdArticle",
                Sub(cmd)
                    cmd.Parameters.Add("@IdArticle", SqlDbType.Int).Value = CInt(txtIdArticle.Text)
                End Sub)

            MessageBox.Show("Article supprimé avec succès.")
            ChargerArticles()
            ViderChampsArticles()

        Catch ex As Exception
            MessageBox.Show("Erreur suppression : " & ex.Message)
        End Try
    End Sub

    ' ═══════════════════════════════════════════════════════════════════
    '  RECHERCHE
    ' ═══════════════════════════════════════════════════════════════════

    Private Sub txtRECHERCHE_TextChanged(sender As Object, e As EventArgs) Handles txtRECHERCHE.TextChanged
        RECHERCHE.PerformClick()
    End Sub

    Private Sub RECHERCHE_Click(sender As Object, e As EventArgs) Handles RECHERCHE.Click
        If txtRECHERCHE.Text.Trim() = "" Then
            ChargerArticles()
            Exit Sub
        End If
        Try
            Dim dt As DataTable = ModuleDb.Lire(
                "SELECT * FROM Articles WHERE CodeArticle LIKE @mot " &
                "OR NomArticle LIKE @mot OR Designation LIKE @mot " &
                "ORDER BY IdArticle DESC",
                Sub(cmd)
                    cmd.Parameters.Add("@mot", SqlDbType.NVarChar, 200).Value = "%" & txtRECHERCHE.Text.Trim() & "%"
                End Sub)

            DataGridView1.DataSource = dt
            For Each col As String In {"IdArticle", "DescriptionArticle", "Remarques",
                                        "NatureService", "CodeExportation", "ServiceTransport",
                                        "Unite", "DerniereModification"}
                If DataGridView1.Columns.Contains(col) Then
                    DataGridView1.Columns(col).Visible = False
                End If
            Next

        Catch ex As Exception
            MessageBox.Show("Erreur recherche : " & ex.Message)
        End Try
    End Sub

    ' ═══════════════════════════════════════════════════════════════════
    '  NOUVEAU / ACTUALISER
    ' ═══════════════════════════════════════════════════════════════════

    Private Sub btnNouveau_Click(sender As Object, e As EventArgs) Handles btnNouveau.Click
        ViderChampsArticles()
    End Sub

    Private Sub btnActualiser_Click(sender As Object, e As EventArgs) Handles btnActualiser.Click
        ChargerArticles()
    End Sub

    ' ═══════════════════════════════════════════════════════════════════
    '  IMPRESSION (inchangée, simplifiée)
    ' ═══════════════════════════════════════════════════════════════════

    Private Sub Imprimer_Click(sender As Object, e As EventArgs) Handles Imprimer.Click
        If ligneSelectionnee Is Nothing Then
            MessageBox.Show("Sélectionne une ligne à imprimer.")
            Exit Sub
        End If
        Try
            PrintPreviewDialog1.Document = PrintDocument1
            PrintPreviewDialog1.ShowDialog()
        Catch ex As Exception
            MessageBox.Show("Erreur impression : " & ex.Message)
        End Try
    End Sub

    Private Sub PrintDocument1_PrintPage(sender As Object, e As Printing.PrintPageEventArgs) Handles PrintDocument1.PrintPage
        Dim fontTitre As New Font("Arial", 14, FontStyle.Bold)
        Dim fontTexte As New Font("Arial", 11, FontStyle.Regular)
        Dim y As Integer = 40

        e.Graphics.DrawString("Article sélectionné", fontTitre, Brushes.Black, 40, y) : y += 40

        Dim champs = {
            ("Code : ", "CodeArticle"),
            ("Nom : ", "NomArticle"),
            ("Désignation : ", "Designation"),
            ("Description : ", "DescriptionArticle"),
            ("Remarques : ", "Remarques"),
            ("Nature service : ", "NatureService"),
            ("Code exportation : ", "CodeExportation"),
            ("Service transport : ", "ServiceTransport"),
            ("Unité : ", "Unite"),
            ("Dernière modification :", "DerniereModification")
        }
        For Each c In champs
            Dim val As String = If(ligneSelectionnee.Cells(c.Item2).Value Is Nothing, "",
                                   ligneSelectionnee.Cells(c.Item2).Value.ToString())
            e.Graphics.DrawString(c.Item1 & val, fontTexte, Brushes.Black, 40, y)
            y += 30
        Next
    End Sub

    ' ═══════════════════════════════════════════════════════════════════
    '  HELPERS
    ' ═══════════════════════════════════════════════════════════════════

    Private Sub AjouterParametres(cmd As SqlCommand)
        cmd.Parameters.Add("@CodeArticle", SqlDbType.NVarChar, 50).Value = txtCodeArticle.Text.Trim()
        cmd.Parameters.Add("@NomArticle", SqlDbType.NVarChar, 200).Value = txtNomArticle.Text.Trim()
        cmd.Parameters.Add("@Designation", SqlDbType.NVarChar, 200).Value = txtDesignation.Text.Trim()
        cmd.Parameters.Add("@DescriptionArticle", SqlDbType.NVarChar, 500).Value = txtDescriptionArticle.Text.Trim()
        cmd.Parameters.Add("@Remarques", SqlDbType.NVarChar, 500).Value = txtRemarques.Text.Trim()
        cmd.Parameters.Add("@NatureService", SqlDbType.NVarChar, 100).Value = txtNatureService.Text.Trim()
        cmd.Parameters.Add("@CodeExportation", SqlDbType.NVarChar, 50).Value = txtCodeExportation.Text.Trim()
        cmd.Parameters.Add("@ServiceTransport", SqlDbType.NVarChar, 100).Value = txtServiceTransport.Text.Trim()
        cmd.Parameters.Add("@Unite", SqlDbType.NVarChar, 20).Value = txtUnite.Text.Trim()
        If txtDerniereModification.Text.Trim() = "" Then
            cmd.Parameters.Add("@DerniereModification", SqlDbType.DateTime).Value = DBNull.Value
        Else
            Dim d As Date
            cmd.Parameters.Add("@DerniereModification", SqlDbType.DateTime).Value =
                If(Date.TryParse(txtDerniereModification.Text.Trim(), d), CObj(d), DBNull.Value)
        End If
    End Sub

    Private Sub ViderChampsArticles()
        txtIdArticle.Text = "" : txtCodeArticle.Text = "" : txtNomArticle.Text = ""
        txtDesignation.Text = "" : txtDescriptionArticle.Text = "" : txtRemarques.Text = ""
        txtNatureService.Text = "" : txtCodeExportation.Text = "" : txtServiceTransport.Text = ""
        txtUnite.Text = "" : txtDerniereModification.Text = "" : txtRECHERCHE.Text = ""
        ligneSelectionnee = Nothing
    End Sub

End Class