Imports Microsoft.Data.SqlClient
Imports System.Data

Public Class Zones

    Dim ligneSelectionnee As DataGridViewRow = Nothing

    ' ═══════════════════════════════════════════════════════════════════
    '  LOAD
    ' ═══════════════════════════════════════════════════════════════════

    Private Sub Zones_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        DataGridView1.RowHeadersVisible = False   ' NOUVEAU
        ' txtIdZone.Visible = False

        ' NOUVEAU : gestion des rôles
        Select Case RoleUtilisateur
            Case "Admin", "Super"
                btnAjouter.Visible = True : btnAjouter.Enabled = True
                btnModifier.Visible = True : btnModifier.Enabled = True
                btnSupprimer.Visible = True : btnSupprimer.Enabled = True
            Case "Operateur"
                btnAjouter.Visible = True : btnAjouter.Enabled = False
                btnModifier.Visible = True : btnModifier.Enabled = False
                btnSupprimer.Visible = True : btnSupprimer.Enabled = False
            Case Else
                btnAjouter.Visible = True : btnAjouter.Enabled = False
                btnModifier.Visible = True : btnModifier.Enabled = False
                btnSupprimer.Visible = True : btnSupprimer.Enabled = False
        End Select

        ChargerZones()
    End Sub

    ' ═══════════════════════════════════════════════════════════════════
    '  CHARGEMENT
    ' ═══════════════════════════════════════════════════════════════════

    Private Sub ChargerZones()
        Try
            Dim dt As DataTable = ModuleDb.Lire(
                "SELECT * FROM Zones ORDER BY IdZone DESC")

            DataGridView1.Columns.Clear()
            DataGridView1.AutoGenerateColumns = True
            DataGridView1.DataSource = dt
            DataGridView1.SelectionMode = DataGridViewSelectionMode.FullRowSelect
            DataGridView1.MultiSelect = False
            DataGridView1.AllowUserToAddRows = False
            DataGridView1.ReadOnly = True
            DataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill

            For Each col As String In {"IdZone", "TypeZone", "CodePostal", "NumeroZone", "DerniereModification"}
                If DataGridView1.Columns.Contains(col) Then
                    DataGridView1.Columns(col).Visible = False
                End If
            Next

            If DataGridView1.Columns.Contains("CodeZone") Then DataGridView1.Columns("CodeZone").HeaderText = "CODE"
            If DataGridView1.Columns.Contains("CleZone") Then DataGridView1.Columns("CleZone").HeaderText = "CLÉ"
            If DataGridView1.Columns.Contains("NomZone") Then DataGridView1.Columns("NomZone").HeaderText = "NOM"

        Catch ex As Exception
            MessageBox.Show("Erreur chargement : " & ex.Message)
        End Try
    End Sub

    ' ═══════════════════════════════════════════════════════════════════
    '  CLIC GRILLE
    ' ═══════════════════════════════════════════════════════════════════

    Private Sub DataGridView1_CellClick(sender As Object, e As DataGridViewCellEventArgs) Handles DataGridView1.CellClick
        If e.RowIndex < 0 Then Exit Sub
        ligneSelectionnee = DataGridView1.Rows(e.RowIndex)
        Dim row As DataGridViewRow = ligneSelectionnee

        txtIdZone.Text = If(IsDBNull(row.Cells("IdZone").Value), "", row.Cells("IdZone").Value.ToString())
        txtCodeZone.Text = If(IsDBNull(row.Cells("CodeZone").Value), "", row.Cells("CodeZone").Value.ToString())
        txtCleZone.Text = If(IsDBNull(row.Cells("CleZone").Value), "", row.Cells("CleZone").Value.ToString())
        txtNomZone.Text = If(IsDBNull(row.Cells("NomZone").Value), "", row.Cells("NomZone").Value.ToString())
        txtTypeZone.Text = If(IsDBNull(row.Cells("TypeZone").Value), "", row.Cells("TypeZone").Value.ToString())
        txtCodePostal.Text = If(IsDBNull(row.Cells("CodePostal").Value), "", row.Cells("CodePostal").Value.ToString())
        txtNumeroZone.Text = If(IsDBNull(row.Cells("NumeroZone").Value), "", row.Cells("NumeroZone").Value.ToString())
        txtDerniereModification.Text = If(IsDBNull(row.Cells("DerniereModification").Value), "",
            CDate(row.Cells("DerniereModification").Value).ToString("yyyy-MM-dd"))
    End Sub

    ' ═══════════════════════════════════════════════════════════════════
    '  AJOUTER
    ' ═══════════════════════════════════════════════════════════════════

    Private Sub btnAjouter_Click(sender As Object, e As EventArgs) Handles btnAjouter.Click
        If txtCodeZone.Text.Trim = "" OrElse txtNomZone.Text.Trim = "" Then
            MessageBox.Show("Code et Nom sont obligatoires.")
            Exit Sub
        End If
        Try
            ModuleDb.Executer(
                "INSERT INTO Zones (CodeZone, CleZone, NomZone, TypeZone, CodePostal, NumeroZone, DerniereModification) " &
                "VALUES (@CodeZone, @CleZone, @NomZone, @TypeZone, @CodePostal, @NumeroZone, @DerniereModification)",
                Sub(cmd) AjouterParametres(cmd))

            MessageBox.Show("Zone ajoutée avec succès.")
            ChargerZones()
            ViderChampsZones()

        Catch ex As Exception
            MessageBox.Show("Erreur ajout : " & ex.Message)
        End Try
    End Sub

    ' ═══════════════════════════════════════════════════════════════════
    '  MODIFIER
    ' ═══════════════════════════════════════════════════════════════════

    Private Sub btnModifier_Click(sender As Object, e As EventArgs) Handles btnModifier.Click
        If txtIdZone.Text = "" Then
            MessageBox.Show("Sélectionne une zone.")
            Exit Sub
        End If
        Try
            ModuleDb.Executer(
                "UPDATE Zones SET " &
                "CodeZone=@CodeZone, CleZone=@CleZone, NomZone=@NomZone, " &
                "TypeZone=@TypeZone, CodePostal=@CodePostal, NumeroZone=@NumeroZone, " &
                "DerniereModification=@DerniereModification " &
                "WHERE IdZone=@IdZone",
                Sub(cmd)
                    cmd.Parameters.Add("@IdZone", SqlDbType.Int).Value = CInt(txtIdZone.Text)
                    AjouterParametres(cmd)
                End Sub)

            MessageBox.Show("Zone modifiée avec succès.")
            ChargerZones()
            ViderChampsZones()

        Catch ex As Exception
            MessageBox.Show("Erreur modification : " & ex.Message)
        End Try
    End Sub

    ' ═══════════════════════════════════════════════════════════════════
    '  SUPPRIMER
    ' ═══════════════════════════════════════════════════════════════════

    Private Sub btnSupprimer_Click(sender As Object, e As EventArgs) Handles btnSupprimer.Click
        If txtIdZone.Text = "" Then
            MessageBox.Show("Sélectionne une zone.")
            Exit Sub
        End If
        If MessageBox.Show("Voulez-vous supprimer cette zone ?", "Confirmation",
                           MessageBoxButtons.YesNo, MessageBoxIcon.Question) = DialogResult.No Then Exit Sub
        Try
            ModuleDb.Executer(
                "DELETE FROM Zones WHERE IdZone=@IdZone",
                Sub(cmd)
                    cmd.Parameters.Add("@IdZone", SqlDbType.Int).Value = CInt(txtIdZone.Text)
                End Sub)

            MessageBox.Show("Zone supprimée avec succès.")
            ChargerZones()
            ViderChampsZones()

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
            ChargerZones()
            Exit Sub
        End If
        Try
            Dim dt As DataTable = ModuleDb.Lire(
                "SELECT IdZone, CodeZone, CleZone, NomZone, TypeZone, " &
                "CodePostal, NumeroZone, DerniereModification " &
                "FROM dbo.Zones WHERE CodeZone LIKE @mot " &
                "OR CleZone LIKE @mot OR NomZone LIKE @mot ORDER BY NomZone",
                Sub(cmd)
                    cmd.Parameters.Add("@mot", SqlDbType.NVarChar, 200).Value = "%" & txtRECHERCHE.Text.Trim() & "%"
                End Sub)

            DataGridView1.DataSource = dt
            For Each col As String In {"IdZone", "TypeZone", "CodePostal", "NumeroZone", "DerniereModification"}
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
        ViderChampsZones()
    End Sub

    Private Sub btnActualiser_Click(sender As Object, e As EventArgs) Handles btnActualiser.Click
        ChargerZones()
    End Sub

    ' ═══════════════════════════════════════════════════════════════════
    '  IMPRESSION (inchangée)
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

        e.Graphics.DrawString("Zone sélectionnée", fontTitre, Brushes.Black, 40, y) : y += 40

        Dim champs = {
            ("Code : ", "CodeZone"),
            ("Clé : ", "CleZone"),
            ("Nom : ", "NomZone"),
            ("Type zone : ", "TypeZone"),
            ("Code postal : ", "CodePostal"),
            ("Numéro zone : ", "NumeroZone"),
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
        cmd.Parameters.Add("@CodeZone", SqlDbType.NVarChar, 50).Value = txtCodeZone.Text.Trim()
        cmd.Parameters.Add("@CleZone", SqlDbType.NVarChar, 50).Value = txtCleZone.Text.Trim()
        cmd.Parameters.Add("@NomZone", SqlDbType.NVarChar, 200).Value = txtNomZone.Text.Trim()
        cmd.Parameters.Add("@TypeZone", SqlDbType.NVarChar, 50).Value = txtTypeZone.Text.Trim()
        cmd.Parameters.Add("@CodePostal", SqlDbType.NVarChar, 20).Value = txtCodePostal.Text.Trim()
        Dim numeroZone As Integer = 0
        Integer.TryParse(txtNumeroZone.Text, numeroZone)
        cmd.Parameters.Add("@NumeroZone", SqlDbType.Int).Value = numeroZone
        If txtDerniereModification.Text.Trim() = "" Then
            cmd.Parameters.Add("@DerniereModification", SqlDbType.DateTime).Value = DBNull.Value
        Else
            Dim d As Date
            cmd.Parameters.Add("@DerniereModification", SqlDbType.DateTime).Value =
                If(Date.TryParse(txtDerniereModification.Text.Trim(), d), CObj(d), DBNull.Value)
        End If
    End Sub

    Private Sub ViderChampsZones()
        txtIdZone.Text = "" : txtCodeZone.Text = "" : txtCleZone.Text = ""
        txtNomZone.Text = "" : txtTypeZone.Text = "" : txtCodePostal.Text = ""
        txtNumeroZone.Text = "" : txtDerniereModification.Text = ""
        txtRECHERCHE.Text = ""
        ligneSelectionnee = Nothing
    End Sub

End Class