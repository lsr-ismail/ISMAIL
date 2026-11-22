Imports Microsoft.Data.SqlClient
Imports System.Data

Public Class Éléments_à_bétonner

    Dim ligneSelectionnee As DataGridViewRow = Nothing

    ' ═══════════════════════════════════════════════════════════════════
    '  LOAD
    ' ═══════════════════════════════════════════════════════════════════

    Private Sub ElementsBetonner_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        ChargerElementsBetonner()
        txtIdElement.Visible = False
    End Sub

    ' ═══════════════════════════════════════════════════════════════════
    '  CHARGEMENT
    ' ═══════════════════════════════════════════════════════════════════

    Private Sub ChargerElementsBetonner()
        Try
            Dim dt As DataTable = ModuleDb.Lire(
                "SELECT * FROM ElementsBetonner ORDER BY IdElement DESC")

            DataGridView1.Columns.Clear()
            DataGridView1.AutoGenerateColumns = True
            DataGridView1.DataSource = dt
            DataGridView1.SelectionMode = DataGridViewSelectionMode.FullRowSelect
            DataGridView1.MultiSelect = False
            DataGridView1.AllowUserToAddRows = False
            DataGridView1.ReadOnly = True
            DataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill

            If DataGridView1.Columns.Contains("IdElement") Then
                DataGridView1.Columns("IdElement").Visible = False
            End If
            If DataGridView1.Columns.Contains("DerniereModification") Then
                DataGridView1.Columns("DerniereModification").Visible = False
            End If
            If DataGridView1.Columns.Contains("CodeElement") Then
                DataGridView1.Columns("CodeElement").HeaderText = "CODE"
            End If
            If DataGridView1.Columns.Contains("NomElement") Then
                DataGridView1.Columns("NomElement").HeaderText = "NOM"
            End If
            If DataGridView1.Columns.Contains("DescriptionElement") Then
                DataGridView1.Columns("DescriptionElement").HeaderText = "DESCRIPTION"
            End If
            If DataGridView1.Columns.Contains("DescriptionElement1") Then
                DataGridView1.Columns("DescriptionElement1").HeaderText = "DESCRIPTION 1"
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
        txtIdElement.Text = LireCell(row, "IdElement")
        txtCodeElement.Text = LireCell(row, "CodeElement")
        txtNomElement.Text = LireCell(row, "NomElement")
        txtDescriptionElement.Text = LireCell(row, "DescriptionElement")
        txtDescriptionElement1.Text = LireCell(row, "DescriptionElement1")
        txtDerniereModification.Text = If(IsDBNull(row.Cells("DerniereModification").Value), "",
            CDate(row.Cells("DerniereModification").Value).ToString("yyyy-MM-dd"))
    End Sub

    ' ═══════════════════════════════════════════════════════════════════
    '  AJOUTER
    ' ═══════════════════════════════════════════════════════════════════

    Private Sub btnAjouter_Click(sender As Object, e As EventArgs) Handles btnAjouter.Click
        If txtCodeElement.Text.Trim = "" Then
            MessageBox.Show("Le code élément est obligatoire.")
            Exit Sub
        End If
        Try
            ModuleDb.Executer(
                "INSERT INTO ElementsBetonner " &
                "(CodeElement, NomElement, DescriptionElement, DescriptionElement1, DerniereModification) " &
                "VALUES (@CodeElement, @NomElement, @DescriptionElement, @DescriptionElement1, @DerniereModification)",
                Sub(cmd)
                    cmd.Parameters.Add("@CodeElement", SqlDbType.NVarChar, 50).Value = txtCodeElement.Text.Trim()
                    cmd.Parameters.Add("@NomElement", SqlDbType.NVarChar, 200).Value = txtNomElement.Text.Trim()
                    cmd.Parameters.Add("@DescriptionElement", SqlDbType.NVarChar, 500).Value = txtDescriptionElement.Text.Trim()
                    cmd.Parameters.Add("@DescriptionElement1", SqlDbType.NVarChar, 500).Value = txtDescriptionElement1.Text.Trim()
                    cmd.Parameters.Add("@DerniereModification", SqlDbType.DateTime).Value = ParseDate(txtDerniereModification.Text)
                End Sub)

            MessageBox.Show("Élément ajouté avec succès.")
            ChargerElementsBetonner()
            ViderChampsElements()

        Catch ex As Exception
            MessageBox.Show("Erreur ajout : " & ex.Message)
        End Try
    End Sub

    ' ═══════════════════════════════════════════════════════════════════
    '  MODIFIER
    ' ═══════════════════════════════════════════════════════════════════

    Private Sub btnModifier_Click(sender As Object, e As EventArgs) Handles btnModifier.Click
        If txtIdElement.Text = "" Then
            MessageBox.Show("Sélectionne un élément.")
            Exit Sub
        End If
        Try
            ModuleDb.Executer(
                "UPDATE ElementsBetonner SET " &
                "CodeElement=@CodeElement, NomElement=@NomElement, " &
                "DescriptionElement=@DescriptionElement, DescriptionElement1=@DescriptionElement1, " &
                "DerniereModification=@DerniereModification " &
                "WHERE IdElement=@IdElement",
                Sub(cmd)
                    cmd.Parameters.Add("@IdElement", SqlDbType.Int).Value = CInt(txtIdElement.Text)
                    cmd.Parameters.Add("@CodeElement", SqlDbType.NVarChar, 50).Value = txtCodeElement.Text.Trim()
                    cmd.Parameters.Add("@NomElement", SqlDbType.NVarChar, 200).Value = txtNomElement.Text.Trim()
                    cmd.Parameters.Add("@DescriptionElement", SqlDbType.NVarChar, 500).Value = txtDescriptionElement.Text.Trim()
                    cmd.Parameters.Add("@DescriptionElement1", SqlDbType.NVarChar, 500).Value = txtDescriptionElement1.Text.Trim()
                    cmd.Parameters.Add("@DerniereModification", SqlDbType.DateTime).Value = ParseDate(txtDerniereModification.Text)
                End Sub)

            MessageBox.Show("Élément modifié avec succès.")
            ChargerElementsBetonner()
            ViderChampsElements()

        Catch ex As Exception
            MessageBox.Show("Erreur modification : " & ex.Message)
        End Try
    End Sub

    ' ═══════════════════════════════════════════════════════════════════
    '  SUPPRIMER
    ' ═══════════════════════════════════════════════════════════════════

    Private Sub btnSupprimer_Click(sender As Object, e As EventArgs) Handles btnSupprimer.Click
        If txtIdElement.Text = "" Then
            MessageBox.Show("Sélectionne un élément.")
            Exit Sub
        End If
        If MessageBox.Show("Voulez-vous supprimer cet élément ?", "Confirmation",
                           MessageBoxButtons.YesNo, MessageBoxIcon.Question) = DialogResult.No Then Exit Sub
        Try
            ModuleDb.Executer(
                "DELETE FROM ElementsBetonner WHERE IdElement=@IdElement",
                Sub(cmd)
                    cmd.Parameters.Add("@IdElement", SqlDbType.Int).Value = CInt(txtIdElement.Text)
                End Sub)

            MessageBox.Show("Élément supprimé avec succès.")
            ChargerElementsBetonner()
            ViderChampsElements()

        Catch ex As Exception
            MessageBox.Show("Erreur suppression : " & ex.Message)
        End Try
    End Sub

    ' ═══════════════════════════════════════════════════════════════════
    '  RECHERCHE
    ' ═══════════════════════════════════════════════════════════════════

    Private Sub RECHERCHE_Click(sender As Object, e As EventArgs) Handles RECHERCHE.Click
        If txtRECHERCHE.Text.Trim() = "" Then
            ChargerElementsBetonner()
            Exit Sub
        End If
        Try
            Dim dt As DataTable = ModuleDb.Lire(
                "SELECT * FROM ElementsBetonner " &
                "WHERE CodeElement LIKE @mot OR NomElement LIKE @mot " &
                "OR DescriptionElement LIKE @mot " &
                "ORDER BY IdElement DESC",
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
    '  NOUVEAU / ACTUALISER
    ' ═══════════════════════════════════════════════════════════════════

    Private Sub btnNouveau_Click(sender As Object, e As EventArgs) Handles btnNouveau.Click
        ViderChampsElements()
    End Sub

    Private Sub btnActualiser_Click(sender As Object, e As EventArgs) Handles btnActualiser.Click
        ChargerElementsBetonner()
    End Sub

    ' ═══════════════════════════════════════════════════════════════════
    '  HELPERS
    ' ═══════════════════════════════════════════════════════════════════

    Private Function LireCell(row As DataGridViewRow, col As String) As String
        Return If(IsDBNull(row.Cells(col).Value), "", row.Cells(col).Value.ToString())
    End Function

    Private Function ParseDate(txt As String) As Object
        If txt.Trim() = "" Then Return DBNull.Value
        Dim d As Date
        Return If(Date.TryParse(txt.Trim(), d), CObj(d), DBNull.Value)
    End Function

    Private Sub ViderChampsElements()
        txtIdElement.Text = "" : txtCodeElement.Text = "" : txtNomElement.Text = ""
        txtDescriptionElement.Text = "" : txtDescriptionElement1.Text = ""
        txtDerniereModification.Text = "" : txtRECHERCHE.Text = ""
        ligneSelectionnee = Nothing
    End Sub

End Class