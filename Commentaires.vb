Imports System.Drawing.Printing
Imports Microsoft.Data.SqlClient
Imports System.Data

Public Class Commentaires

    Dim ligneSelectionnee As DataGridViewRow = Nothing

    ' ═══════════════════════════════════════════════════════════════════
    '  LOAD
    ' ═══════════════════════════════════════════════════════════════════

    Private Sub Commentaires_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        ChargerCommentaires()
        txtIdCommentaire.Visible = False
    End Sub

    ' ═══════════════════════════════════════════════════════════════════
    '  CHARGEMENT
    ' ═══════════════════════════════════════════════════════════════════

    Private Sub ChargerCommentaires()
        Try
            Dim dt As DataTable = ModuleDb.Lire(
                "SELECT * FROM Commentaires ORDER BY IdCommentaire DESC")

            DataGridView1.Columns.Clear()
            DataGridView1.AutoGenerateColumns = True
            DataGridView1.DataSource = dt
            DataGridView1.SelectionMode = DataGridViewSelectionMode.FullRowSelect
            DataGridView1.MultiSelect = False
            DataGridView1.AllowUserToAddRows = False
            DataGridView1.ReadOnly = True
            DataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill

            If DataGridView1.Columns.Contains("IdCommentaire") Then
                DataGridView1.Columns("IdCommentaire").Visible = False
            End If
            If DataGridView1.Columns.Contains("DerniereModification") Then
                DataGridView1.Columns("DerniereModification").Visible = False
            End If
            If DataGridView1.Columns.Contains("CodeCommentaire") Then
                DataGridView1.Columns("CodeCommentaire").HeaderText = "CODE"
            End If
            If DataGridView1.Columns.Contains("DescriptionCommentaire") Then
                DataGridView1.Columns("DescriptionCommentaire").HeaderText = "DESCRIPTION"
            End If

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
        txtIdCommentaire.Text = LireCell(row, "IdCommentaire")
        txtCodeCommentaire.Text = LireCell(row, "CodeCommentaire")
        txtDescriptionCommentaire.Text = LireCell(row, "DescriptionCommentaire")
        txtDerniereModification.Text = If(IsDBNull(row.Cells("DerniereModification").Value), "",
            CDate(row.Cells("DerniereModification").Value).ToString("yyyy-MM-dd"))
    End Sub

    ' ═══════════════════════════════════════════════════════════════════
    '  AJOUTER
    ' ═══════════════════════════════════════════════════════════════════

    Private Sub btnAjouter_Click(sender As Object, e As EventArgs) Handles btnAjouter.Click
        If txtCodeCommentaire.Text.Trim = "" Then
            MessageBox.Show("Le code commentaire est obligatoire.")
            Exit Sub
        End If
        Try
            ModuleDb.Executer(
                "INSERT INTO Commentaires (CodeCommentaire, DescriptionCommentaire, DerniereModification) " &
                "VALUES (@CodeCommentaire, @DescriptionCommentaire, @DerniereModification)",
                Sub(cmd)
                    cmd.Parameters.Add("@CodeCommentaire", SqlDbType.NVarChar, 50).Value = txtCodeCommentaire.Text.Trim()
                    cmd.Parameters.Add("@DescriptionCommentaire", SqlDbType.NVarChar, 500).Value = txtDescriptionCommentaire.Text.Trim()
                    cmd.Parameters.Add("@DerniereModification", SqlDbType.DateTime).Value = ParseDate(txtDerniereModification.Text)
                End Sub)

            MessageBox.Show("Commentaire ajouté avec succès.")
            ChargerCommentaires()
            ViderChampsCommentaires()

        Catch ex As Exception
            MessageBox.Show("Erreur ajout : " & ex.Message)
        End Try
    End Sub

    ' ═══════════════════════════════════════════════════════════════════
    '  MODIFIER
    ' ═══════════════════════════════════════════════════════════════════

    Private Sub btnModifier_Click(sender As Object, e As EventArgs) Handles btnModifier.Click
        If txtIdCommentaire.Text = "" Then
            MessageBox.Show("Sélectionne un commentaire.")
            Exit Sub
        End If
        Try
            ModuleDb.Executer(
                "UPDATE Commentaires SET " &
                "CodeCommentaire=@CodeCommentaire, " &
                "DescriptionCommentaire=@DescriptionCommentaire, " &
                "DerniereModification=@DerniereModification " &
                "WHERE IdCommentaire=@IdCommentaire",
                Sub(cmd)
                    cmd.Parameters.Add("@IdCommentaire", SqlDbType.Int).Value = CInt(txtIdCommentaire.Text)
                    cmd.Parameters.Add("@CodeCommentaire", SqlDbType.NVarChar, 50).Value = txtCodeCommentaire.Text.Trim()
                    cmd.Parameters.Add("@DescriptionCommentaire", SqlDbType.NVarChar, 500).Value = txtDescriptionCommentaire.Text.Trim()
                    cmd.Parameters.Add("@DerniereModification", SqlDbType.DateTime).Value = ParseDate(txtDerniereModification.Text)
                End Sub)

            MessageBox.Show("Commentaire modifié avec succès.")
            ChargerCommentaires()
            ViderChampsCommentaires()

        Catch ex As Exception
            MessageBox.Show("Erreur modification : " & ex.Message)
        End Try
    End Sub

    ' ═══════════════════════════════════════════════════════════════════
    '  SUPPRIMER
    ' ═══════════════════════════════════════════════════════════════════

    Private Sub btnSupprimer_Click(sender As Object, e As EventArgs) Handles btnSupprimer.Click
        If txtIdCommentaire.Text = "" Then
            MessageBox.Show("Sélectionne un commentaire.")
            Exit Sub
        End If
        If MessageBox.Show("Voulez-vous supprimer ce commentaire ?", "Confirmation",
                           MessageBoxButtons.YesNo, MessageBoxIcon.Question) = DialogResult.No Then Exit Sub
        Try
            ModuleDb.Executer(
                "DELETE FROM Commentaires WHERE IdCommentaire=@IdCommentaire",
                Sub(cmd)
                    cmd.Parameters.Add("@IdCommentaire", SqlDbType.Int).Value = CInt(txtIdCommentaire.Text)
                End Sub)

            MessageBox.Show("Commentaire supprimé avec succès.")
            ChargerCommentaires()
            ViderChampsCommentaires()

        Catch ex As Exception
            MessageBox.Show("Erreur suppression : " & ex.Message)
        End Try
    End Sub

    ' ═══════════════════════════════════════════════════════════════════
    '  RECHERCHE
    ' ═══════════════════════════════════════════════════════════════════

    Private Sub RECHERCHE_Click(sender As Object, e As EventArgs) Handles RECHERCHE.Click
        If txtRECHERCHE.Text.Trim() = "" Then
            ChargerCommentaires()
            Exit Sub
        End If
        Try
            Dim dt As DataTable = ModuleDb.Lire(
                "SELECT * FROM Commentaires " &
                "WHERE CodeCommentaire LIKE @mot OR DescriptionCommentaire LIKE @mot " &
                "ORDER BY IdCommentaire DESC",
                Sub(cmd)
                    cmd.Parameters.Add("@mot", SqlDbType.NVarChar, 200).Value = "%" & txtRECHERCHE.Text.Trim() & "%"
                End Sub)

            DataGridView1.Columns.Clear()
            DataGridView1.AutoGenerateColumns = True
            DataGridView1.DataSource = dt

        Catch ex As Exception
            MessageBox.Show("Erreur recherche : " & ex.Message)
        End Try
    End Sub

    ' ═══════════════════════════════════════════════════════════════════
    '  IMPRESSION
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

    Private Sub PrintDocument1_PrintPage(sender As Object, e As PrintPageEventArgs) Handles PrintDocument1.PrintPage
        Dim fontTitre As New Font("Arial", 14, FontStyle.Bold)
        Dim fontTexte As New Font("Arial", 11, FontStyle.Regular)
        Dim y As Integer = 40

        e.Graphics.DrawString("Commentaire sélectionné", fontTitre, Brushes.Black, 40, y) : y += 40
        e.Graphics.DrawString("Code : " & LireCell(ligneSelectionnee, "CodeCommentaire"), fontTexte, Brushes.Black, 40, y) : y += 30
        e.Graphics.DrawString("Description : " & LireCell(ligneSelectionnee, "DescriptionCommentaire"), fontTexte, Brushes.Black, 40, y) : y += 30
        e.Graphics.DrawString("Dernière modification : " & LireCell(ligneSelectionnee, "DerniereModification"), fontTexte, Brushes.Black, 40, y)
    End Sub

    ' ═══════════════════════════════════════════════════════════════════
    '  NOUVEAU / ACTUALISER
    ' ═══════════════════════════════════════════════════════════════════

    Private Sub btnNouveau_Click(sender As Object, e As EventArgs) Handles btnNouveau.Click
        ViderChampsCommentaires()
    End Sub

    Private Sub btnActualiser_Click(sender As Object, e As EventArgs) Handles btnActualiser.Click
        ChargerCommentaires()
    End Sub

    ' ═══════════════════════════════════════════════════════════════════
    '  HELPERS
    ' ═══════════════════════════════════════════════════════════════════

    Private Function LireCell(row As DataGridViewRow, col As String) As String
        Return If(row.Cells(col).Value Is Nothing OrElse IsDBNull(row.Cells(col).Value), "",
                  row.Cells(col).Value.ToString())
    End Function

    Private Function ParseDate(txt As String) As Object
        If txt.Trim() = "" Then Return DBNull.Value
        Dim d As Date
        Return If(Date.TryParse(txt.Trim(), d), CObj(d), DBNull.Value)
    End Function

    Private Sub ViderChampsCommentaires()
        txtIdCommentaire.Text = "" : txtCodeCommentaire.Text = ""
        txtDescriptionCommentaire.Text = "" : txtDerniereModification.Text = ""
        txtRECHERCHE.Text = ""
        ligneSelectionnee = Nothing
    End Sub

End Class