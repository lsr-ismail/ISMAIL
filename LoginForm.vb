Imports Microsoft.Data.SqlClient
Imports System.Data

Public Class LoginForm

    Private Sub LoginForm_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Me.StartPosition = FormStartPosition.Manual
        Dim zoneEcran As Rectangle = Screen.PrimaryScreen.WorkingArea
        Me.Location = New Point(
        zoneEcran.Left + (zoneEcran.Width - Me.Width) \ 2,
        zoneEcran.Top + (zoneEcran.Height - Me.Height) \ 2)

        AssurerStructureLicence()
        Dim erreur As String = ""
        If Not VerifierLicence(erreur) Then
            Dim frmLic As New FrmLicence()
            frmLic.ShowDialog()
            If Not frmLic.LicenceValide Then
                Application.Exit()
                Exit Sub
            End If
        End If

        motdepasse.PasswordChar = "*"
        ChargerSociete()
        ChargerUtilisateursCombo()
        ' Afficher nom societe dans le titre
        If SocieteNom <> "" Then
            Me.Text = "Connexion — " & SocieteNom
        End If
    End Sub
    ' ═══════════════════════════════════════════════════════════════
    '  CHARGER LA COMBOBOX UTILISATEUR
    ' ═══════════════════════════════════════════════════════════════
    Private Sub ChargerUtilisateursCombo()
        Try
            Dim dt As DataTable = ModuleDb.Lire(
            "SELECT NomUtilisateur FROM dbo.Utilisateurs ORDER BY NomUtilisateur")

            utilisateur.DataSource = dt
            utilisateur.DisplayMember = "NomUtilisateur"
            utilisateur.ValueMember = "NomUtilisateur"
            utilisateur.SelectedIndex = -1
        Catch ex As Exception
            MessageBox.Show("Erreur chargement utilisateurs : " & ex.Message,
                        "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub
    ' ═══════════════════════════════════════════════════════════════
    '  CONNEXION
    ' ═══════════════════════════════════════════════════════════════
    Private Sub connecter_Click(sender As Object, e As EventArgs) Handles connecter.Click
        If utilisateur.SelectedIndex = -1 Then
            MessageBox.Show("Sélectionnez un utilisateur.")
            utilisateur.Focus()
            Exit Sub
        End If
        If motdepasse.Text.Trim = "" Then
            MessageBox.Show("Entrez votre mot de passe.")
            motdepasse.Focus()
            Exit Sub
        End If

        Try
            Me.Cursor = Cursors.WaitCursor

            Dim role As String = ModuleDb.LireValeur(Of String)(
            "SELECT RoleUtilisateur FROM dbo.Utilisateurs " &
            "WHERE NomUtilisateur=@u AND MotDePasse=@p",
            Sub(cmd)
                cmd.Parameters.Add("@u", SqlDbType.NVarChar, 100).Value = utilisateur.Text.Trim()
                cmd.Parameters.Add("@p", SqlDbType.NVarChar, 100).Value = motdepasse.Text.Trim()
            End Sub)

            If role IsNot Nothing AndAlso role <> "" Then
                RoleUtilisateur = role
                NomUtilisateur = utilisateur.Text.Trim()
                InitialiserApplication()
                Synoptique.EnregistrerAction("SECURITE", "Connexion réussie", "Rôle=" & role, "", "", "LOGIN")
                Dim f As New PanelMain
                f.Show()
                Me.Hide()
            Else
                MessageBox.Show("Mot de passe incorrect.",
                "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                Synoptique.EnregistrerAction("SECURITE", "Echec de connexion", "Utilisateur saisi : " & utilisateur.Text.Trim(), "", "", "LOGIN_ECHEC")
                motdepasse.Clear()
                motdepasse.Focus()
            End If

        Catch ex As Exception
            MessageBox.Show("Erreur connexion : " & ex.Message,
                        "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Error)
        Finally
            Me.Cursor = Cursors.Default
        End Try
    End Sub
    Private Sub motdepasse_KeyPress(sender As Object, e As KeyPressEventArgs) Handles motdepasse.KeyPress
        If e.KeyChar = Chr(13) Then connecter_Click(sender, e)
    End Sub

    ' BOUTON BYPASS — DEVELOPPEMENT UNIQUEMENT (supprimer avant production)
    Private Sub Button1_Click_1(sender As Object, e As EventArgs)
        RoleUtilisateur = "Admin"
        NomUtilisateur = "Oper"
        InitialiserApplication()
        Hide()
        PanelMain.Show()
    End Sub

    ' Bouton test connexion SQL
    Private Sub Button2_Click(sender As Object, e As EventArgs)
        Try
            Dim nb = LireValeur(Of Integer)("SELECT COUNT(*) FROM dbo.Clients")
            MessageBox.Show("Connexion SQL OK — " & nb & " clients dans la base.",
                            "Test SQL", MessageBoxButtons.OK, MessageBoxIcon.Information)
        Catch ex As Exception
            MessageBox.Show("Erreur SQL : " & ex.Message,
                            "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub


End Class