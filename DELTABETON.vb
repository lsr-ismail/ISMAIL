Imports System.Data
Imports Microsoft.Data.SqlClient

Public Class DELTABETON

    ' ═══════════════════════════════════════════════════════════════════
    '  LOAD
    ' ═══════════════════════════════════════════════════════════════════

    Private Sub DELTABETON_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        MotDePasse.PasswordChar = "*"c   ' NOUVEAU
        RoleUtilisateu.Items.Clear()
        RoleUtilisateu.Items.AddRange({"Admin", "Super", "Operateur"})
        RoleUtilisateu.DropDownStyle = ComboBoxStyle.DropDownList
        Select Case RoleUtilisateur
            Case "Admin", "Super"
                AJOUTE.Visible = True
                EDITE.Visible = True
                SUPRIME.Visible = True
            Case "Operateur"
                AJOUTE.Visible = True
                EDITE.Visible = False
                SUPRIME.Visible = False
            Case Else
                AJOUTE.Visible = False
                EDITE.Visible = False
                SUPRIME.Visible = False
        End Select
        ChargerUtilisateurs()
    End Sub

    ' ═══════════════════════════════════════════════════════════════════
    '  CHARGEMENT
    ' ═══════════════════════════════════════════════════════════════════

    Public Sub ChargerUtilisateurs()
        Try
            Dim dt As DataTable = ModuleDb.Lire(
            "SELECT Id, NomUtilisateur, MotDePasse, RoleUtilisateur FROM Utilisateurs")
            DataGridView1.DataSource = dt

            ' NOUVEAU : cacher la colonne mot de passe dans la grille
            If DataGridView1.Columns.Contains("MotDePasse") Then
                DataGridView1.Columns("MotDePasse").Visible = False
            End If
            If DataGridView1.Columns.Contains("Id") Then
                DataGridView1.Columns("Id").Visible = False
            End If

            DataGridView1.Columns("NomUtilisateur").HeaderText = "Utilisateur"
            DataGridView1.Columns("RoleUtilisateur").HeaderText = "Rôle"
            DataGridView1.RowHeadersVisible = False   ' NOUVEAU (bonus cohérence)

        Catch ex As Exception
            MessageBox.Show(ex.Message)
        End Try
    End Sub
    ' ═══════════════════════════════════════════════════════════════════
    '  AJOUTER
    ' ═══════════════════════════════════════════════════════════════════

    Private Sub AJOUTE_Click(sender As Object, e As EventArgs) Handles AJOUTE.Click
        If NomUtilisateur.Text.Trim = "" Then
            MessageBox.Show("Saisissez un nom utilisateur.")
            Exit Sub
        End If
        Try
            ModuleDb.Executer(
                "INSERT INTO Utilisateurs(NomUtilisateur, MotDePasse, RoleUtilisateur) " &
                "VALUES(@NomUtilisateur, @MotDePasse, @RoleUtilisateur)",
                Sub(cmd)
                    cmd.Parameters.Add("@NomUtilisateur", SqlDbType.NVarChar, 100).Value = NomUtilisateur.Text.Trim()
                    cmd.Parameters.Add("@MotDePasse", SqlDbType.NVarChar, 100).Value = MotDePasse.Text.Trim()
                    cmd.Parameters.Add("@RoleUtilisateur", SqlDbType.NVarChar, 50).Value = RoleUtilisateu.Text.Trim()
                End Sub)

            MessageBox.Show("Utilisateur ajouté avec succès.")
            ChargerUtilisateurs()
            ViderChamps()

        Catch ex As Exception
            MessageBox.Show(ex.Message)
        End Try
    End Sub

    ' ═══════════════════════════════════════════════════════════════════
    '  MODIFIER
    ' ═══════════════════════════════════════════════════════════════════

    Private Sub EDITE_Click(sender As Object, e As EventArgs) Handles EDITE.Click
        If RoleUtilisateur = "Operateur" Then
            MessageBox.Show("Accès refusé.")
            Exit Sub
        End If
        If NomUtilisateur.Text.Trim = "" Then
            MessageBox.Show("Sélectionne un utilisateur.")
            Exit Sub
        End If
        Try
            ModuleDb.Executer(
                "UPDATE Utilisateurs SET MotDePasse=@MotDePasse, RoleUtilisateur=@RoleUtilisateur " &
                "WHERE NomUtilisateur=@NomUtilisateur",
                Sub(cmd)
                    cmd.Parameters.Add("@NomUtilisateur", SqlDbType.NVarChar, 100).Value = NomUtilisateur.Text.Trim()
                    cmd.Parameters.Add("@MotDePasse", SqlDbType.NVarChar, 100).Value = MotDePasse.Text.Trim()
                    cmd.Parameters.Add("@RoleUtilisateur", SqlDbType.NVarChar, 50).Value = RoleUtilisateu.Text.Trim()
                End Sub)

            MessageBox.Show("Utilisateur modifié avec succès.")
            ChargerUtilisateurs()
            ViderChamps()

        Catch ex As Exception
            MessageBox.Show(ex.Message)
        End Try
    End Sub

    ' ═══════════════════════════════════════════════════════════════════
    '  SUPPRIMER
    ' ═══════════════════════════════════════════════════════════════════

    Private Sub SUPRIME_Click(sender As Object, e As EventArgs) Handles SUPRIME.Click
        If NomUtilisateur.Text.Trim = "" Then
            MessageBox.Show("Sélectionne un utilisateur.")
            Exit Sub
        End If
        If MessageBox.Show("Voulez-vous supprimer cet utilisateur ?", "Confirmation",
                           MessageBoxButtons.YesNo, MessageBoxIcon.Question) = DialogResult.No Then Exit Sub
        Try
            ModuleDb.Executer(
                "DELETE FROM Utilisateurs WHERE NomUtilisateur=@NomUtilisateur",
                Sub(cmd)
                    cmd.Parameters.Add("@NomUtilisateur", SqlDbType.NVarChar, 100).Value = NomUtilisateur.Text.Trim()
                End Sub)

            MessageBox.Show("Utilisateur supprimé avec succès.")
            ChargerUtilisateurs()
            ViderChamps()

        Catch ex As Exception
            MessageBox.Show(ex.Message)
        End Try
    End Sub

    ' ═══════════════════════════════════════════════════════════════════
    '  CLIC GRILLE
    ' ═══════════════════════════════════════════════════════════════════

    Private Sub DataGridView1_CellClick(sender As Object, e As DataGridViewCellEventArgs) Handles DataGridView1.CellClick
        If e.RowIndex < 0 Then Exit Sub
        Dim row As DataGridViewRow = DataGridView1.Rows(e.RowIndex)

        NomUtilisateur.Text = If(IsDBNull(row.Cells("NomUtilisateur").Value), "", row.Cells("NomUtilisateur").Value.ToString())
        MotDePasse.Text = If(IsDBNull(row.Cells("MotDePasse").Value), "", row.Cells("MotDePasse").Value.ToString())

        Dim roleBD As String = If(IsDBNull(row.Cells("RoleUtilisateur").Value), "", row.Cells("RoleUtilisateur").Value.ToString().Trim())
        Dim index As Integer = RoleUtilisateu.FindStringExact(roleBD)
        If index >= 0 Then
            RoleUtilisateu.SelectedIndex = index
        Else
            RoleUtilisateu.SelectedIndex = -1
        End If
    End Sub
    ' ═══════════════════════════════════════════════════════════════════
    '  ROUTEUR / UTILITAIRES
    ' ═══════════════════════════════════════════════════════════════════

    Private Sub routeur_Click(sender As Object, e As EventArgs) Handles routeur.Click
        RoleUtilisateur = ""
        LoginForm.utilisateur.SelectedIndex = -1   ' CORRIGÉ (était .Clear())
        LoginForm.motdepasse.Clear()
        LoginForm.Show()
        Me.Close()
    End Sub

    Public Sub ViderChamps()
        NomUtilisateur.Clear()
        MotDePasse.Clear()
        RoleUtilisateu.SelectedIndex = -1   ' CORRIGÉ (était .Clear())
    End Sub

End Class