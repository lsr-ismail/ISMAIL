Imports Microsoft.Data.SqlClient
Imports System.Data
Imports System.Management
Imports System.Security.Cryptography
Imports System.Text

Module ModuleLicence

    Private Const FACTEUR_MULTIPLICATION As Long = 18
    Private Const CONSTANTE_AJOUT As Long = 6
    Private Const MODULO As Long = 19941985

    Public Sub AssurerStructureLicence()
        Try
            Dim dt As DataTable = ModuleDb.Lire(
                "SELECT COLUMN_NAME FROM INFORMATION_SCHEMA.COLUMNS " &
                "WHERE TABLE_NAME = 'GranulatsCertification' AND COLUMN_NAME = 'Conforme'")

            If dt.Rows.Count = 0 Then
                ModuleDb.Executer(
                    "ALTER TABLE dbo.GranulatsCertification ADD Conforme BIT NOT NULL DEFAULT 0",
                    Sub(cmd)
                    End Sub)
            End If

        Catch ex As Exception
            MessageBox.Show("Erreur vérification structure : " & ex.Message)
        End Try
    End Sub

    Public Function ObtenirCodeNumeriqueMachine() As String
        Dim hashComplet As String = ObtenirIdentifiantMachine()
        Dim somme As Long = 0
        For Each c As Char In hashComplet
            somme += AscW(c)
        Next
        Dim code As Long = (somme Mod 900000L) + 100000L
        Return code.ToString()
    End Function

    Public Function ObtenirCodeAffiche() As String
        Dim codeOriginal As String = ObtenirCodeNumeriqueMachine()
        Return InverserChiffres(codeOriginal)
    End Function

    Private Function InverserChiffres(texte As String) As String
        Dim tableau As Char() = texte.ToCharArray()
        Array.Reverse(tableau)
        Return New String(tableau)
    End Function

    Public Function GenererCodeActivationNumerique(codeMachineNumerique As String) As String
        Dim n As Long
        If Not Long.TryParse(codeMachineNumerique, n) Then Return "000000"
        Dim resultat As Long = (n * FACTEUR_MULTIPLICATION + CONSTANTE_AJOUT) Mod MODULO
        Return resultat.ToString("000000")
    End Function

    ' 24/09 : empreinte = CARTE MÈRE + PROCESSEUR uniquement (éléments fixes,
    '         identiques à chaque redémarrage) ; plus de disque dur (l'ordre des
    '         disques peut changer → le code d'activation était redemandé).
    Private _identifiantMachine As String = Nothing
    Public Function ObtenirIdentifiantMachine() As String
        If Not String.IsNullOrEmpty(_identifiantMachine) Then Return _identifiantMachine
        Try
            Dim serieCarteMere As String = Nettoyer(LireProprieteWMI("Win32_BaseBoard", "SerialNumber"))
            Dim idProcesseur As String = Nettoyer(LireProprieteWMI("Win32_Processor", "ProcessorId"))
            If serieCarteMere = "" Then serieCarteMere = Nettoyer(LireProprieteWMI("Win32_BIOS", "SerialNumber"))
            If serieCarteMere = "" Then serieCarteMere = "NA"
            If idProcesseur = "" Then idProcesseur = "NA"

            Dim combinaison As String = serieCarteMere & "|" & idProcesseur

            Using sha As SHA256 = SHA256.Create()
                Dim octets As Byte() = sha.ComputeHash(Encoding.UTF8.GetBytes(combinaison))
                _identifiantMachine = Convert.ToBase64String(octets)
                Return _identifiantMachine
            End Using

        Catch ex As Exception
            Return "ERREUR-" & Environment.MachineName
        End Try
    End Function

    ' Valeurs WMI vides ou génériques → "" (majuscules, sans espaces)
    Private Function Nettoyer(v As String) As String
        If v Is Nothing Then Return ""
        Dim t As String = v.Trim().ToUpperInvariant().Replace(" ", "")
        Select Case t
            Case "", "NONE", "DEFAULTSTRING", "TOBEFILLEDBYO.E.M.", "SYSTEMSERIALNUMBER", "0", "NOTAPPLICABLE", "N/A"
                Return ""
        End Select
        Return t
    End Function

    Private Function LireProprieteWMI(classeWMI As String, propriete As String) As String
        Try
            Dim resultat As String = ""
            Using searcher As New ManagementObjectSearcher("SELECT " & propriete & " FROM " & classeWMI)
                For Each obj As ManagementObject In searcher.Get()
                    If obj(propriete) IsNot Nothing Then
                        resultat = obj(propriete).ToString().Trim()
                    End If
                    Exit For
                Next
            End Using
            Return resultat
        Catch
            Return ""
        End Try
    End Function

    Public Sub ReinitialiserLicence(codeClient As String, nomClient As String, dureeAnnees As Integer, nombrePostes As Integer)
        Try
            ModuleDb.Executer(
                "INSERT INTO dbo.GranulatsCertification (CodeGranulat, Designation, DateControle, Granulometrie, Disponible, Conforme) " &
                "VALUES (@CodeGranulat, @Designation, @DateControle, @Granulometrie, 1, 0)",
                Sub(cmd)
                    cmd.Parameters.Add("@CodeGranulat", SqlDbType.NVarChar, 50).Value = codeClient
                    cmd.Parameters.Add("@Designation", SqlDbType.NVarChar, 150).Value = nomClient
                    cmd.Parameters.Add("@DateControle", SqlDbType.Date).Value = Date.Today.AddYears(dureeAnnees)
                    cmd.Parameters.Add("@Granulometrie", SqlDbType.Int).Value = nombrePostes
                End Sub)

        Catch ex As Exception
            MessageBox.Show("Erreur création : " & ex.Message)
        End Try
    End Sub

    Public Function VerifierLicence(ByRef messageErreur As String) As Boolean
        Try
            Dim dt As DataTable = ModuleDb.Lire(
                "SELECT TOP 1 IdCertification, CodeGranulat, Designation, DateControle, Disponible, " &
                "CertificatQualite, Conforme " &
                "FROM dbo.GranulatsCertification WHERE Disponible = 1 ORDER BY DateControle DESC, IdCertification DESC")

            If dt.Rows.Count = 0 Then
                ReinitialiserLicence("CLI-TEST", "Client Test Centrale", 1, 5)
                messageErreur = "Confirmez l'activation avec le code fourni par le support."
                Return False
            End If

            Dim idCertification As Integer = Convert.ToInt32(dt.Rows(0)("IdCertification"))
            Dim dateControle As Date = Convert.ToDateTime(dt.Rows(0)("DateControle"))
            Dim certificatEnregistre As String = If(IsDBNull(dt.Rows(0)("CertificatQualite")), "", dt.Rows(0)("CertificatQualite").ToString())
            Dim conforme As Boolean = Convert.ToBoolean(dt.Rows(0)("Conforme"))
            Dim certificatActuel As String = ObtenirIdentifiantMachine()

            ' 24/09 : plus de date d'expiration — licence liée uniquement à la machine
            '         (carte mère + processeur). DateControle n'est plus contrôlée.

            If certificatEnregistre <> "" AndAlso certificatEnregistre = certificatActuel Then
                If conforme Then
                    Return True
                Else
                    messageErreur = "Confirmez l'activation avec le code fourni par le support."
                    Return False
                End If
            End If

            ModuleDb.Executer(
                "UPDATE dbo.GranulatsCertification SET CertificatQualite = @Certificat, Conforme = 0 WHERE IdCertification = @IdCertification",
                Sub(cmd)
                    cmd.Parameters.Add("@Certificat", SqlDbType.NVarChar, 200).Value = certificatActuel
                    cmd.Parameters.Add("@IdCertification", SqlDbType.Int).Value = idCertification
                End Sub)

            messageErreur = "Confirmez l'activation avec le code fourni par le support."
            Return False

        Catch ex As Exception
            messageErreur = "Erreur : " & ex.Message
            Return False
        End Try
    End Function

End Module