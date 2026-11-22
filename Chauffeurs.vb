Imports System.Drawing.Printing
Imports Microsoft.Data.SqlClient
Imports System.Data

Public Class Chauffeurs

    Private _idChauffeurCourant As Integer = 0

    ' ═══════════════════════════════════════════════════════════════════
    '  LOAD
    ' ═══════════════════════════════════════════════════════════════════

    Private Sub Chauffeurs_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        DataGridView1.RowHeadersVisible = False   ' NOUVEAU

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

        ChargerChauffeurs()
        DataGridView1.SelectionMode = DataGridViewSelectionMode.FullRowSelect
        DataGridView1.MultiSelect = False
        DataGridView1.ReadOnly = True
        DataGridView1.AllowUserToAddRows = False
    End Sub

    ' ═══════════════════════════════════════════════════════════════════
    '  CHARGEMENT
    ' ═══════════════════════════════════════════════════════════════════

    Private Sub ChargerChauffeurs()
        Try
            Dim dt As DataTable = ModuleDb.Lire(
            "SELECT IdChauffeur, CodeChauffeur, NomChauffeur, Adresse, " &
            "CodePostal, Telephone, Email, Remarques, Actif, DerniereModification " &
            "FROM dbo.Chauffeurs ORDER BY NomChauffeur")

            DataGridView1.DataSource = dt
            DataGridView1.ReadOnly = True
            DataGridView1.AllowUserToAddRows = False

            ' NOUVEAU : cacher toutes les colonnes puis ne montrer que l'essentiel
            For Each col As DataGridViewColumn In DataGridView1.Columns
                col.Visible = False
            Next
            DataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.None

            If DataGridView1.Columns.Contains("CodeChauffeur") Then
                DataGridView1.Columns("CodeChauffeur").Visible = True
                DataGridView1.Columns("CodeChauffeur").HeaderText = "Code"
                DataGridView1.Columns("CodeChauffeur").Width = 90
            End If
            If DataGridView1.Columns.Contains("NomChauffeur") Then
                DataGridView1.Columns("NomChauffeur").Visible = True
                DataGridView1.Columns("NomChauffeur").HeaderText = "Nom"
                DataGridView1.Columns("NomChauffeur").Width = 225
            End If
            If DataGridView1.Columns.Contains("Telephone") Then
                DataGridView1.Columns("Telephone").Visible = True
                DataGridView1.Columns("Telephone").HeaderText = "Téléphone"
                DataGridView1.Columns("Telephone").Width = 100
            End If
            If DataGridView1.Columns.Contains("Actif") Then
                DataGridView1.Columns("Actif").Visible = True
                DataGridView1.Columns("Actif").HeaderText = "Actif"
                DataGridView1.Columns("Actif").Width = 40
            End If

            lblNombre.Text = "Total : " & dt.Rows.Count.ToString() & " chauffeur(s)"

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
            _idChauffeurCourant = CInt(row.Cells("IdChauffeur").Value)

            txtidChauffeur.Text = _idChauffeurCourant.ToString()   ' NOUVEAU

            txtCodeChauffeur.Text = LireTexte(row, "CodeChauffeur")
            txtNomChauffeur.Text = LireTexte(row, "NomChauffeur")
            txtAdresse.Text = LireTexte(row, "Adresse")
            txtCodePostal.Text = LireTexte(row, "CodePostal")
            txtTelephone.Text = LireTexte(row, "Telephone")
            txtEmail.Text = LireTexte(row, "Email")
            txtRemarques.Text = LireTexte(row, "Remarques")

            chkActif.Checked = Not IsDBNull(row.Cells("Actif").Value) AndAlso
                            Convert.ToBoolean(row.Cells("Actif").Value)

            dtpDerniereModification.Text = If(Not IsDBNull(row.Cells("DerniereModification").Value),
            Convert.ToDateTime(row.Cells("DerniereModification").Value).ToString("dd/MM/yyyy"),
            Date.Today.ToString("dd/MM/yyyy"))

        Catch ex As Exception
            MessageBox.Show("Erreur sélection : " & ex.Message)
        End Try
    End Sub
    ' ═══════════════════════════════════════════════════════════════════
    '  AJOUTER
    ' ═══════════════════════════════════════════════════════════════════

    Private Sub btnAjouter_Click(sender As Object, e As EventArgs) Handles btnAjouter.Click
        If txtCodeChauffeur.Text.Trim = "" OrElse txtNomChauffeur.Text.Trim = "" Then
            MessageBox.Show("Code et Nom sont obligatoires.")
            Exit Sub
        End If
        Try
            ModuleDb.Executer(
                "INSERT INTO dbo.Chauffeurs " &
                "(CodeChauffeur, NomChauffeur, Adresse, CodePostal, Telephone, " &
                "Email, Remarques, Actif, DerniereModification) " &
                "VALUES (@CodeChauffeur, @NomChauffeur, @Adresse, @CodePostal, @Telephone, " &
                "@Email, @Remarques, @Actif, @DerniereModification)",
                AddressOf AjouterParametres)

            MessageBox.Show("Chauffeur ajouté avec succès.")
            ChargerChauffeurs()
            ViderChamps()

        Catch ex As Exception
            MessageBox.Show("Erreur ajout : " & ex.Message)
        End Try
    End Sub

    ' ═══════════════════════════════════════════════════════════════════
    '  MODIFIER
    ' ═══════════════════════════════════════════════════════════════════

    Private Sub btnModifier_Click(sender As Object, e As EventArgs) Handles btnModifier.Click
        If _idChauffeurCourant = 0 Then
            MessageBox.Show("Sélectionner un chauffeur à modifier.")
            Exit Sub
        End If
        Try
            ModuleDb.Executer(
                "UPDATE dbo.Chauffeurs SET " &
                "CodeChauffeur=@CodeChauffeur, NomChauffeur=@NomChauffeur, " &
                "Adresse=@Adresse, CodePostal=@CodePostal, Telephone=@Telephone, " &
                "Email=@Email, Remarques=@Remarques, Actif=@Actif, " &
                "DerniereModification=@DerniereModification " &
                "WHERE IdChauffeur=@IdChauffeur",
                Sub(cmd)
                    cmd.Parameters.Add("@IdChauffeur", SqlDbType.Int).Value = _idChauffeurCourant
                    AjouterParametres(cmd)
                End Sub)

            MessageBox.Show("Chauffeur modifié avec succès.")
            ChargerChauffeurs()
            ViderChamps()

        Catch ex As Exception
            MessageBox.Show("Erreur modification : " & ex.Message)
        End Try
    End Sub

    ' ═══════════════════════════════════════════════════════════════════
    '  SUPPRIMER
    ' ═══════════════════════════════════════════════════════════════════

    Private Sub btnSupprimer_Click(sender As Object, e As EventArgs) Handles btnSupprimer.Click
        If _idChauffeurCourant = 0 Then
            MessageBox.Show("Sélectionner un chauffeur à supprimer.")
            Exit Sub
        End If
        If MessageBox.Show("Voulez-vous supprimer ce chauffeur ?", "Confirmation",
                           MessageBoxButtons.YesNo, MessageBoxIcon.Question) = DialogResult.No Then
            Exit Sub
        End If
        Try
            ModuleDb.Executer(
                "DELETE FROM dbo.Chauffeurs WHERE IdChauffeur=@IdChauffeur",
                Sub(cmd)
                    cmd.Parameters.Add("@IdChauffeur", SqlDbType.Int).Value = _idChauffeurCourant
                End Sub)

            MessageBox.Show("Chauffeur supprimé avec succès.")
            ChargerChauffeurs()
            ViderChamps()

        Catch ex As Exception
            MessageBox.Show("Erreur suppression : " & ex.Message)
        End Try
    End Sub

    ' ═══════════════════════════════════════════════════════════════════
    '  RECHERCHE (temps réel)
    ' ═══════════════════════════════════════════════════════════════════

    Private Sub txtRECHERCHE_TextChanged(sender As Object, e As EventArgs) Handles txtRECHERCHE.TextChanged
        RECHERCHE.PerformClick()
    End Sub

    Private Sub RECHERCHE_Click(sender As Object, e As EventArgs) Handles RECHERCHE.Click
        Try
            Dim dt As DataTable = ModuleDb.Lire(
                "SELECT IdChauffeur, CodeChauffeur, NomChauffeur, Adresse, " &
                "CodePostal, Telephone, Email, Remarques, Actif, DerniereModification " &
                "FROM dbo.Chauffeurs WHERE CodeChauffeur LIKE @mot OR NomChauffeur LIKE @mot " &
                "ORDER BY NomChauffeur",
                Sub(cmd)
                    cmd.Parameters.Add("@mot", SqlDbType.NVarChar, 200).Value = "%" & txtRECHERCHE.Text.Trim & "%"
                End Sub)

            DataGridView1.DataSource = dt

            For Each col As DataGridViewColumn In DataGridView1.Columns
                col.Visible = False
            Next
            DataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.None

            If DataGridView1.Columns.Contains("CodeChauffeur") Then
                DataGridView1.Columns("CodeChauffeur").Visible = True
                DataGridView1.Columns("CodeChauffeur").HeaderText = "Code"
                DataGridView1.Columns("CodeChauffeur").Width = 100
            End If
            If DataGridView1.Columns.Contains("NomChauffeur") Then
                DataGridView1.Columns("NomChauffeur").Visible = True
                DataGridView1.Columns("NomChauffeur").HeaderText = "Nom"
                DataGridView1.Columns("NomChauffeur").Width = 250
            End If
            If DataGridView1.Columns.Contains("Telephone") Then
                DataGridView1.Columns("Telephone").Visible = True
                DataGridView1.Columns("Telephone").HeaderText = "Téléphone"
                DataGridView1.Columns("Telephone").Width = 150
            End If
            If DataGridView1.Columns.Contains("Actif") Then
                DataGridView1.Columns("Actif").Visible = True
                DataGridView1.Columns("Actif").HeaderText = "Actif"
                DataGridView1.Columns("Actif").Width = 70
            End If

            lblNombre.Text = "Total : " & dt.Rows.Count.ToString() & " chauffeur(s)"

        Catch ex As Exception
            MessageBox.Show("Erreur recherche : " & ex.Message)
        End Try
    End Sub

    ' ═══════════════════════════════════════════════════════════════════
    '  NOUVEAU / ACTUALISER
    ' ═══════════════════════════════════════════════════════════════════

    Private Sub btnNouveau_Click(sender As Object, e As EventArgs) Handles btnNouveau.Click
        ViderChamps()
    End Sub

    Private Sub btnActualiser_Click(sender As Object, e As EventArgs) Handles btnActualiser.Click
        ChargerChauffeurs()
    End Sub

    ' ═══════════════════════════════════════════════════════════════════
    '  IMPRESSION (inchangée)
    ' ═══════════════════════════════════════════════════════════════════

    Private Sub Imprimer_Click(sender As Object, e As EventArgs) Handles Imprimer.Click
        If _idChauffeurCourant = 0 Then
            MessageBox.Show("Sélectionner un chauffeur à imprimer.")
            Exit Sub
        End If
        PrintPreviewDialog1.Document = PrintDocument1
        PrintPreviewDialog1.ShowDialog()
    End Sub

    Private Sub PrintDocument1_PrintPage(sender As Object, e As PrintPageEventArgs) Handles PrintDocument1.PrintPage
        Dim fTitre As New Font("Arial", 16, FontStyle.Bold)
        Dim fLabel As New Font("Arial", 11, FontStyle.Bold)
        Dim fTxt As New Font("Arial", 11)
        Dim x As Integer = 50
        Dim y As Integer = 50
        Dim esp As Integer = 28

        e.Graphics.DrawString("FICHE CHAUFFEUR", fTitre, Brushes.Black, x, y)
        e.Graphics.DrawString("Date : " & DateTime.Now.ToString("dd/MM/yyyy HH:mm"),
                              New Font("Arial", 9, FontStyle.Italic), Brushes.Gray, x, y + 35)
        y += 80
        e.Graphics.DrawLine(Pens.Black, x, y, 700, y)
        y += 15

        Dim champs = {
            ("Code :", txtCodeChauffeur.Text),
            ("Nom :", txtNomChauffeur.Text),
            ("Adresse :", txtAdresse.Text),
            ("Code Postal :", txtCodePostal.Text),
            ("Téléphone :", txtTelephone.Text),
            ("Email :", txtEmail.Text),
            ("Remarques :", txtRemarques.Text),
            ("Actif :", If(chkActif.Checked, "Oui", "Non"))
        }
        For Each c In champs
            e.Graphics.DrawString(c.Item1, fLabel, Brushes.Black, x, y)
            e.Graphics.DrawString(c.Item2, fTxt, Brushes.Black, x + 160, y)
            y += esp
        Next

        e.Graphics.DrawString("Centrale à Béton — Gestion des chauffeurs",
                              New Font("Arial", 8, FontStyle.Italic), Brushes.Gray, x, y + 20)
    End Sub

    ' ═══════════════════════════════════════════════════════════════════
    '  HELPERS
    ' ═══════════════════════════════════════════════════════════════════

    Private Sub AjouterParametres(cmd As SqlCommand)
        cmd.Parameters.Add("@CodeChauffeur", SqlDbType.NVarChar, 50).Value = ValTexte(txtCodeChauffeur.Text)
        cmd.Parameters.Add("@NomChauffeur", SqlDbType.NVarChar, 200).Value = ValTexte(txtNomChauffeur.Text)
        cmd.Parameters.Add("@Adresse", SqlDbType.NVarChar, 300).Value = ValTexte(txtAdresse.Text)
        cmd.Parameters.Add("@CodePostal", SqlDbType.NVarChar, 20).Value = ValTexte(txtCodePostal.Text)
        cmd.Parameters.Add("@Telephone", SqlDbType.NVarChar, 30).Value = ValTexte(txtTelephone.Text)
        cmd.Parameters.Add("@Email", SqlDbType.NVarChar, 100).Value = ValTexte(txtEmail.Text)
        cmd.Parameters.Add("@Remarques", SqlDbType.NVarChar, 500).Value = ValTexte(txtRemarques.Text)
        cmd.Parameters.Add("@Actif", SqlDbType.Bit).Value = chkActif.Checked
        cmd.Parameters.Add("@DerniereModification", SqlDbType.Date).Value = Date.Today
    End Sub

    Private Function LireTexte(row As DataGridViewRow, col As String) As String
        Return If(IsDBNull(row.Cells(col).Value), "", row.Cells(col).Value.ToString())
    End Function

    Private Function ValTexte(txt As String) As Object
        Return If(txt.Trim = "", DBNull.Value, CObj(txt.Trim))
    End Function

    Private Sub ViderChamps()
        _idChauffeurCourant = 0
        txtidChauffeur.Text = ""   ' NOUVEAU
        txtCodeChauffeur.Text = "" : txtNomChauffeur.Text = "" : txtAdresse.Text = ""
        txtCodePostal.Text = "" : txtTelephone.Text = "" : txtEmail.Text = ""
        txtRemarques.Text = ""
        chkActif.Checked = True
        dtpDerniereModification.Text = Date.Today.ToString("dd/MM/yyyy")
        txtCodeChauffeur.Focus()
    End Sub

    Private Sub DataGridView1_CellContentClick(sender As Object, e As DataGridViewCellEventArgs) Handles DataGridView1.CellContentClick

    End Sub
End Class