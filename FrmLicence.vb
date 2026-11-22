Imports Microsoft.Data.SqlClient
Imports System.Data

Public Class FrmLicence

    Public LicenceValide As Boolean = False

    Private Sub FrmLicence_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Me.StartPosition = FormStartPosition.CenterScreen
        txtCodeMachine.ReadOnly = True
        txtCodeMachine.PasswordChar = Nothing
        txtCodeMachine.UseSystemPasswordChar = False
        txtCodeMachine.Font = New Font("Consolas", 20, FontStyle.Bold)

        AfficherEtatLicence()
    End Sub

    Private Sub AfficherEtatLicence()
        Try
            Dim dt As DataTable = ModuleDb.Lire(
                "SELECT TOP 1 IdCertification, CodeGranulat, Designation, DateControle, Disponible, " &
                "CertificatQualite, Conforme " &
                "FROM dbo.GranulatsCertification WHERE Disponible = 1 ORDER BY DateControle DESC, IdCertification DESC")

            txtCodeMachine.Text = ObtenirCodeAffiche()

            If dt.Rows.Count = 0 Then
                lblStatut.Text = "Aucune licence trouvée."
                lblStatut.ForeColor = Color.Red
                lblClient.Text = ""
                lblExpiration.Text = ""
                Exit Sub
            End If

            Dim nomClient As String = dt.Rows(0)("Designation").ToString()
            Dim dateControle As Date = Convert.ToDateTime(dt.Rows(0)("DateControle"))
            Dim certificatEnregistre As String = If(IsDBNull(dt.Rows(0)("CertificatQualite")), "", dt.Rows(0)("CertificatQualite").ToString())
            Dim conforme As Boolean = Convert.ToBoolean(dt.Rows(0)("Conforme"))
            Dim certificatActuel As String = ObtenirIdentifiantMachine()

            lblClient.Text = "Client : " & nomClient
            lblExpiration.Text = "Licence liée à cette machine (sans date d'expiration)"   ' 24/09

            If certificatEnregistre = certificatActuel AndAlso conforme Then
                lblStatut.Text = "Licence VALIDE"
                lblStatut.ForeColor = Color.Green
                LicenceValide = True
            Else
                lblStatut.Text = "Licence NON ACTIVÉE sur ce poste"
                lblStatut.ForeColor = Color.OrangeRed
            End If

        Catch ex As Exception
            lblStatut.Text = "Erreur de connexion à la base."
            lblStatut.ForeColor = Color.Red
            MessageBox.Show(ex.Message)
        End Try
    End Sub

    Private Sub btnActiver_Click(sender As Object, e As EventArgs) Handles btnActiver.Click
        Try
            Dim dt As DataTable = ModuleDb.Lire(
                "SELECT TOP 1 IdCertification FROM dbo.GranulatsCertification WHERE Disponible = 1 ORDER BY DateControle DESC, IdCertification DESC")

            If dt.Rows.Count = 0 Then
                MessageBox.Show("Aucune licence à activer.")
                Exit Sub
            End If

            Dim idCertification As Integer = Convert.ToInt32(dt.Rows(0)("IdCertification"))
            Dim certificatActuel As String = ObtenirIdentifiantMachine()
            Dim codeNumeriqueMachine As String = ObtenirCodeNumeriqueMachine()
            Dim codeAttendu As String = GenererCodeActivationNumerique(codeNumeriqueMachine)

            If txtCodeActivation.Text.Trim() = codeAttendu Then
                ModuleDb.Executer(
                    "UPDATE dbo.GranulatsCertification SET CertificatQualite = @Certificat, Conforme = 1 WHERE IdCertification = @IdCertification",
                    Sub(cmd)
                        cmd.Parameters.Add("@Certificat", SqlDbType.NVarChar, 200).Value = certificatActuel
                        cmd.Parameters.Add("@IdCertification", SqlDbType.Int).Value = idCertification
                    End Sub)
                ' 24/09 : une seule ligne active (les doublons faisaient redemander le code)
                ModuleDb.Executer(
                    "UPDATE dbo.GranulatsCertification SET Disponible = 0 WHERE Disponible = 1 AND IdCertification <> @IdCertification",
                    Sub(cmd) cmd.Parameters.Add("@IdCertification", SqlDbType.Int).Value = idCertification)

                MessageBox.Show("Licence activée avec succès !", "Activation réussie", MessageBoxButtons.OK, MessageBoxIcon.Information)
                LicenceValide = True
                Me.Close()
            Else
                MessageBox.Show("Code d'activation incorrect. Vérifiez le code envoyé par le support.",
                    "Échec de l'activation", MessageBoxButtons.OK, MessageBoxIcon.Error)
            End If

        Catch ex As Exception
            MessageBox.Show("Erreur activation : " & ex.Message)
        End Try
    End Sub

    Private Sub btnQuitter_Click(sender As Object, e As EventArgs) Handles btnQuitter.Click
        Me.Close()
    End Sub

End Class
