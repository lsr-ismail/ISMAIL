Imports System.Drawing.Printing
Imports Microsoft.Data.SqlClient
Imports System.Data

Public Class Fournisseurs

    Private _idFournisseurCourant As Integer = 0
    Private _ligneCourante As Integer = 0

    ' ═══════════════════════════════════════════════════════════════════
    '  LOAD
    ' ═══════════════════════════════════════════════════════════════════

    Private Sub Fournisseurs_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        DataGridView1.RowHeadersVisible = False   ' NOUVEAU
        'txtIdFournisseur.Visible = False

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

        ChargerFournisseurs()
    End Sub

    ' ═══════════════════════════════════════════════════════════════════
    '  CHARGEMENT
    ' ═══════════════════════════════════════════════════════════════════

    Private Sub ChargerFournisseurs()
        Try
            Dim dt As DataTable = ModuleDb.Lire(
            "SELECT IdFournisseur, CodeFournisseur, CleFournisseur, NomFournisseur, " &
            "Rue, Ville, CodePostal, MatierePremiere, Transport, Service, DerniereModification " &
            "FROM dbo.Fournisseurs ORDER BY NomFournisseur")

            DataGridView1.DataSource = dt
            DataGridView1.SelectionMode = DataGridViewSelectionMode.FullRowSelect
            DataGridView1.MultiSelect = False
            DataGridView1.AllowUserToAddRows = False
            DataGridView1.ReadOnly = True

            ' NOUVEAU : cacher toutes les colonnes puis ne montrer que l'essentiel
            For Each col As DataGridViewColumn In DataGridView1.Columns
                col.Visible = False
            Next
            DataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.None

            If DataGridView1.Columns.Contains("CodeFournisseur") Then
                DataGridView1.Columns("CodeFournisseur").Visible = True
                DataGridView1.Columns("CodeFournisseur").HeaderText = "Code"
                DataGridView1.Columns("CodeFournisseur").Width = 60
            End If
            If DataGridView1.Columns.Contains("NomFournisseur") Then
                DataGridView1.Columns("NomFournisseur").Visible = True
                DataGridView1.Columns("NomFournisseur").HeaderText = "Nom"
                DataGridView1.Columns("NomFournisseur").Width = 250
            End If
            If DataGridView1.Columns.Contains("Ville") Then
                DataGridView1.Columns("Ville").Visible = True
                DataGridView1.Columns("Ville").HeaderText = "Ville"
                DataGridView1.Columns("Ville").Width = 150
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
        Try
            Dim row As DataGridViewRow = DataGridView1.Rows(e.RowIndex)
            _idFournisseurCourant = CInt(row.Cells("IdFournisseur").Value)

            txtIdFournisseur.Text = _idFournisseurCourant.ToString()
            txtCodeFournisseur.Text = LireTexte(row, "CodeFournisseur")
            txtCleFournisseur.Text = LireTexte(row, "CleFournisseur")
            txtNomFournisseur.Text = LireTexte(row, "NomFournisseur")
            txtRue.Text = LireTexte(row, "Rue")
            txtVille.Text = LireTexte(row, "Ville")
            txtCodePostal.Text = LireTexte(row, "CodePostal")
            txtMatierePremiere.Text = LireTexte(row, "MatierePremiere")
            txtTransport.Text = LireTexte(row, "Transport")
            txtService.Text = LireTexte(row, "Service")
            txtDerniereModification.Text = If(IsDBNull(row.Cells("DerniereModification").Value), "",
                Convert.ToDateTime(row.Cells("DerniereModification").Value).ToString("yyyy-MM-dd"))

        Catch ex As Exception
            MessageBox.Show("Erreur sélection : " & ex.Message)
        End Try
    End Sub

    ' ═══════════════════════════════════════════════════════════════════
    '  AJOUTER
    ' ═══════════════════════════════════════════════════════════════════

    Private Sub btnAjouter_Click(sender As Object, e As EventArgs) Handles btnAjouter.Click
        If txtCodeFournisseur.Text.Trim = "" OrElse txtNomFournisseur.Text.Trim = "" Then
            MessageBox.Show("Code et Nom sont obligatoires.")
            Exit Sub
        End If
        Try
            ModuleDb.Executer(
                "INSERT INTO dbo.Fournisseurs " &
                "(CodeFournisseur, CleFournisseur, NomFournisseur, Rue, Ville, " &
                "CodePostal, MatierePremiere, Transport, Service, DerniereModification) " &
                "VALUES (@CodeFournisseur, @CleFournisseur, @NomFournisseur, @Rue, @Ville, " &
                "@CodePostal, @MatierePremiere, @Transport, @Service, @DerniereModification)",
                AddressOf AjouterParametres)

            MessageBox.Show("Fournisseur ajouté avec succès.")
            ChargerFournisseurs()
            ViderChampsFournisseur()

        Catch ex As Exception
            MessageBox.Show("Erreur ajout : " & ex.Message)
        End Try
    End Sub

    ' ═══════════════════════════════════════════════════════════════════
    '  MODIFIER
    ' ═══════════════════════════════════════════════════════════════════

    Private Sub btnModifier_Click(sender As Object, e As EventArgs) Handles btnModifier.Click
        If _idFournisseurCourant = 0 Then
            MessageBox.Show("Sélectionner un fournisseur à modifier.")
            Exit Sub
        End If
        Try
            ModuleDb.Executer(
                "UPDATE dbo.Fournisseurs SET " &
                "CodeFournisseur=@CodeFournisseur, CleFournisseur=@CleFournisseur, " &
                "NomFournisseur=@NomFournisseur, Rue=@Rue, Ville=@Ville, " &
                "CodePostal=@CodePostal, MatierePremiere=@MatierePremiere, " &
                "Transport=@Transport, Service=@Service, DerniereModification=@DerniereModification " &
                "WHERE IdFournisseur=@IdFournisseur",
                Sub(cmd)
                    cmd.Parameters.Add("@IdFournisseur", SqlDbType.Int).Value = _idFournisseurCourant
                    AjouterParametres(cmd)
                End Sub)

            MessageBox.Show("Fournisseur modifié avec succès.")
            ChargerFournisseurs()
            ViderChampsFournisseur()

        Catch ex As Exception
            MessageBox.Show("Erreur modification : " & ex.Message)
        End Try
    End Sub

    ' ═══════════════════════════════════════════════════════════════════
    '  SUPPRIMER
    ' ═══════════════════════════════════════════════════════════════════

    Private Sub btnSupprimer_Click(sender As Object, e As EventArgs) Handles btnSupprimer.Click
        If _idFournisseurCourant = 0 Then
            MessageBox.Show("Sélectionner un fournisseur à supprimer.")
            Exit Sub
        End If
        If MessageBox.Show("Voulez-vous supprimer ce fournisseur ?", "Confirmation",
                           MessageBoxButtons.YesNo, MessageBoxIcon.Question) = DialogResult.No Then
            Exit Sub
        End If
        Try
            ModuleDb.Executer(
                "DELETE FROM dbo.Fournisseurs WHERE IdFournisseur=@IdFournisseur",
                Sub(cmd)
                    cmd.Parameters.Add("@IdFournisseur", SqlDbType.Int).Value = _idFournisseurCourant
                End Sub)

            MessageBox.Show("Fournisseur supprimé avec succès.")
            ChargerFournisseurs()
            ViderChampsFournisseur()

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
        If txtRECHERCHE.Text.Trim = "" Then
            ChargerFournisseurs()
            Exit Sub
        End If
        Try
            Dim dt As DataTable = ModuleDb.Lire(
                "SELECT IdFournisseur, CodeFournisseur, CleFournisseur, NomFournisseur, " &
                "Rue, Ville, CodePostal, MatierePremiere, Transport, Service, DerniereModification " &
                "FROM dbo.Fournisseurs WHERE CodeFournisseur LIKE @mot " &
                "OR NomFournisseur LIKE @mot OR Ville LIKE @mot ORDER BY NomFournisseur",
                Sub(cmd)
                    cmd.Parameters.Add("@mot", SqlDbType.NVarChar, 200).Value = "%" & txtRECHERCHE.Text.Trim & "%"
                End Sub)

            DataGridView1.DataSource = dt

            For Each col As DataGridViewColumn In DataGridView1.Columns
                col.Visible = False
            Next
            DataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.None

            If DataGridView1.Columns.Contains("CodeFournisseur") Then
                DataGridView1.Columns("CodeFournisseur").Visible = True
                DataGridView1.Columns("CodeFournisseur").HeaderText = "Code"
                DataGridView1.Columns("CodeFournisseur").Width = 100
            End If
            If DataGridView1.Columns.Contains("NomFournisseur") Then
                DataGridView1.Columns("NomFournisseur").Visible = True
                DataGridView1.Columns("NomFournisseur").HeaderText = "Nom"
                DataGridView1.Columns("NomFournisseur").Width = 250
            End If
            If DataGridView1.Columns.Contains("Ville") Then
                DataGridView1.Columns("Ville").Visible = True
                DataGridView1.Columns("Ville").HeaderText = "Ville"
                DataGridView1.Columns("Ville").Width = 150
            End If
            If DataGridView1.Columns.Contains("IdFournisseur") Then
                DataGridView1.Columns("IdFournisseur").Visible = False
            End If

        Catch ex As Exception
            MessageBox.Show("Erreur recherche : " & ex.Message)
        End Try
    End Sub

    ' ═══════════════════════════════════════════════════════════════════
    '  ACTUALISER / NOUVEAU
    ' ═══════════════════════════════════════════════════════════════════

    Private Sub btnNouveau_Click(sender As Object, e As EventArgs) Handles btnNouveau.Click
        ViderChampsFournisseur()
    End Sub

    Private Sub btnActualiser_Click(sender As Object, e As EventArgs) Handles btnActualiser.Click
        ChargerFournisseurs()
    End Sub

    ' ═══════════════════════════════════════════════════════════════════
    '  IMPRESSION (inchangée)
    ' ═══════════════════════════════════════════════════════════════════

    Private Sub Imprimer_Click(sender As Object, e As EventArgs) Handles Imprimer.Click
        _ligneCourante = 0
        PrintPreviewDialog1.Document = PrintDocument1
        PrintPreviewDialog1.ShowDialog()
    End Sub

    Private Sub PrintDocument1_PrintPage(sender As Object, e As PrintPageEventArgs) Handles PrintDocument1.PrintPage
        Dim fTitre As New Font("Arial", 14, FontStyle.Bold)
        Dim fEntete As New Font("Arial", 9, FontStyle.Bold)
        Dim fTexte As New Font("Arial", 9)
        Dim x As Integer = 40
        Dim y As Integer = 40
        Dim colW As Integer = 110

        e.Graphics.DrawString("LISTE DES FOURNISSEURS", fTitre, Brushes.Black, x, y)
        e.Graphics.DrawString("Date : " & DateTime.Now.ToString("dd/MM/yyyy"),
                              New Font("Arial", 8), Brushes.Gray, x, y + 25)
        y += 55

        Dim colonnes = {"Code", "Nom", "Ville", "Téléphone", "Matière 1ère"}
        Dim nomsCols = {"CodeFournisseur", "NomFournisseur", "Ville", "Transport", "MatierePremiere"}
        Dim px As Integer = x
        For Each h In colonnes
            e.Graphics.FillRectangle(Brushes.LightGray, px, y, colW, 20)
            e.Graphics.DrawRectangle(Pens.Black, px, y, colW, 20)
            e.Graphics.DrawString(h, fEntete, Brushes.Black, px + 2, y + 3)
            px += colW
        Next
        y += 20

        While _ligneCourante < DataGridView1.Rows.Count
            Dim row As DataGridViewRow = DataGridView1.Rows(_ligneCourante)
            If Not row.IsNewRow Then
                px = x
                For Each col In nomsCols
                    Dim val As String = If(DataGridView1.Columns.Contains(col) AndAlso
                        Not IsDBNull(row.Cells(col).Value), row.Cells(col).Value.ToString(), "")
                    e.Graphics.DrawRectangle(Pens.LightGray, px, y, colW, 20)
                    e.Graphics.DrawString(val, fTexte, Brushes.Black, px + 2, y + 3)
                    px += colW
                Next
                y += 20
            End If
            _ligneCourante += 1
            If y > e.MarginBounds.Bottom - 40 Then
                e.HasMorePages = True
                Exit Sub
            End If
        End While

        e.HasMorePages = False
        _ligneCourante = 0
    End Sub

    ' ═══════════════════════════════════════════════════════════════════
    '  HELPERS
    ' ═══════════════════════════════════════════════════════════════════

    Private Sub AjouterParametres(cmd As SqlCommand)
        cmd.Parameters.Add("@CodeFournisseur", SqlDbType.NVarChar, 50).Value = ValTexte(txtCodeFournisseur.Text)
        cmd.Parameters.Add("@CleFournisseur", SqlDbType.NVarChar, 50).Value = ValTexte(txtCleFournisseur.Text)
        cmd.Parameters.Add("@NomFournisseur", SqlDbType.NVarChar, 200).Value = ValTexte(txtNomFournisseur.Text)
        cmd.Parameters.Add("@Rue", SqlDbType.NVarChar, 200).Value = ValTexte(txtRue.Text)
        cmd.Parameters.Add("@Ville", SqlDbType.NVarChar, 100).Value = ValTexte(txtVille.Text)
        cmd.Parameters.Add("@CodePostal", SqlDbType.NVarChar, 20).Value = ValTexte(txtCodePostal.Text)
        cmd.Parameters.Add("@MatierePremiere", SqlDbType.NVarChar, 100).Value = ValTexte(txtMatierePremiere.Text)
        cmd.Parameters.Add("@Transport", SqlDbType.NVarChar, 100).Value = ValTexte(txtTransport.Text)
        cmd.Parameters.Add("@Service", SqlDbType.NVarChar, 100).Value = ValTexte(txtService.Text)
        cmd.Parameters.Add("@DerniereModification", SqlDbType.Date).Value = Date.Today
    End Sub

    Private Function LireTexte(row As DataGridViewRow, col As String) As String
        Return If(IsDBNull(row.Cells(col).Value), "", row.Cells(col).Value.ToString())
    End Function

    Private Function ValTexte(txt As String) As Object
        Return If(txt.Trim = "", DBNull.Value, CObj(txt.Trim))
    End Function

    Private Sub ViderChampsFournisseur()
        _idFournisseurCourant = 0
        txtIdFournisseur.Text = "" : txtCodeFournisseur.Text = "" : txtCleFournisseur.Text = ""
        txtNomFournisseur.Text = "" : txtRue.Text = "" : txtVille.Text = ""
        txtCodePostal.Text = "" : txtMatierePremiere.Text = "" : txtTransport.Text = ""
        txtService.Text = "" : txtDerniereModification.Text = ""
        txtCodeFournisseur.Focus()
    End Sub

End Class