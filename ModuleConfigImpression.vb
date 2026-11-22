Imports System.Data
Imports Microsoft.Data.SqlClient

' ================================================================
'  ModuleConfigImpression.vb — 19/09/2026
'  Réglages de l'écran « Configuration générale » (imprimantes,
'  copies, entêtes, formats, options OUI/NON). Stockés dans la
'  ligne unique de dbo.ConfigurationSociete ; les colonnes manquantes
'  sont créées automatiquement au premier appel de Charger().
'  Utilisé par ModuleImpressionBL (imprimante BL, copies, entête,
'  commentaires BL, sans entête).
' ================================================================
Module ModuleConfigImpression

    Public ImprimanteBL As String = ""
    Public ImprimantePeseesBPE As String = ""
    Public ImprimantePeseesPrefa As String = ""
    Public ImprimanteFacture As String = ""
    Public ImprimantePrincipale As String = ""
    Public NbreCopiesBL As Integer = 1
    Public NbreCopiesFactures As Integer = 1
    Public EnteteBL As String = ""
    Public EnteteTicket As String = ""
    Public EnteteFacture As String = ""
    Public EnteteListing As String = ""
    Public LargeurBL As Integer = 0
    Public HauteurBL As Integer = 0
    Public LargeurFacture As Integer = 0
    Public HauteurFacture As Integer = 0
    Public CommentairesBL As String = ""
    Public EnteteActive As Boolean = True     ' OUI = entête société + logo imprimés (colonne SQL SansEntete)
    Public NormeActive As Boolean = True      ' OUI = pied légal / normes imprimé (colonne SQL SansNorme)
    Public Draft As Boolean = False
    Public Temperature As Boolean = False
    Public PapierBL As Boolean = False
    Public PapierFacture As Boolean = False
    Public ModeChantier As String = "TOUJOURS"   ' PARAMETRE_CHANTIER / TOUJOURS / JAMAIS
    Public Operateur As String = ""

    Private _colonnesVerifiees As Boolean = False

    ' Colonnes ajoutées à ConfigurationSociete si absentes
    Private ReadOnly _colonnes As String() = {
        "ImprimanteBL NVARCHAR(200) NULL", "ImprimantePeseesBPE NVARCHAR(200) NULL",
        "ImprimantePeseesPrefa NVARCHAR(200) NULL", "ImprimanteFacture NVARCHAR(200) NULL",
        "ImprimantePrincipale NVARCHAR(200) NULL",
        "NbreCopiesBL INT NULL", "NbreCopiesFactures INT NULL",
        "EnteteBL NVARCHAR(250) NULL", "EnteteTicket NVARCHAR(250) NULL",
        "EnteteFacture NVARCHAR(250) NULL", "EnteteListing NVARCHAR(250) NULL",
        "LargeurBL INT NULL", "HauteurBL INT NULL", "LargeurFacture INT NULL", "HauteurFacture INT NULL",
        "CommentairesBL NVARCHAR(500) NULL",
        "SansEntete BIT NULL", "SansNorme BIT NULL", "Draft BIT NULL", "Temperature BIT NULL",
        "PapierBL BIT NULL", "PapierFacture BIT NULL",
        "ModeChantier NVARCHAR(30) NULL", "Operateur NVARCHAR(100) NULL"}

    Public Sub VerifierColonnes()
        If _colonnesVerifiees Then Exit Sub
        Try
            For Each def In _colonnes
                Dim nom As String = def.Split(" "c)(0)
                ModuleDb.Executer(
                    "IF COL_LENGTH('dbo.ConfigurationSociete','" & nom & "') IS NULL " &
                    "ALTER TABLE dbo.ConfigurationSociete ADD " & def)
            Next
            ' S'assurer qu'il existe une ligne
            Dim n As Object = ModuleDb.LireValeur(Of Object)("SELECT COUNT(*) FROM dbo.ConfigurationSociete")
            If n IsNot Nothing AndAlso Not IsDBNull(n) AndAlso CInt(n) = 0 Then
                ModuleDb.Executer("INSERT INTO dbo.ConfigurationSociete (NomSociete) VALUES ('')")
            End If
            _colonnesVerifiees = True
        Catch ex As Exception
            Debug.WriteLine("VerifierColonnes ConfigurationSociete : " & ex.Message)
        End Try
    End Sub

    Public Sub Charger()
        VerifierColonnes()
        Try
            Dim dt As DataTable = ModuleDb.Lire("SELECT TOP 1 * FROM dbo.ConfigurationSociete")
            If dt Is Nothing OrElse dt.Rows.Count = 0 Then Exit Sub
            Dim dr As DataRow = dt.Rows(0)
            ImprimanteBL = S(dr, "ImprimanteBL")
            ImprimantePeseesBPE = S(dr, "ImprimantePeseesBPE")
            ImprimantePeseesPrefa = S(dr, "ImprimantePeseesPrefa")
            ImprimanteFacture = S(dr, "ImprimanteFacture")
            ImprimantePrincipale = S(dr, "ImprimantePrincipale")
            NbreCopiesBL = Math.Max(1, I(dr, "NbreCopiesBL", 1))
            NbreCopiesFactures = Math.Max(1, I(dr, "NbreCopiesFactures", 1))
            EnteteBL = S(dr, "EnteteBL")
            EnteteTicket = S(dr, "EnteteTicket")
            EnteteFacture = S(dr, "EnteteFacture")
            EnteteListing = S(dr, "EnteteListing")
            LargeurBL = I(dr, "LargeurBL", 0)
            HauteurBL = I(dr, "HauteurBL", 0)
            LargeurFacture = I(dr, "LargeurFacture", 0)
            HauteurFacture = I(dr, "HauteurFacture", 0)
            CommentairesBL = S(dr, "CommentairesBL")
            EnteteActive = B(dr, "SansEntete", True)
            NormeActive = B(dr, "SansNorme", True)
            Draft = B(dr, "Draft")
            Temperature = B(dr, "Temperature")
            PapierBL = B(dr, "PapierBL")
            PapierFacture = B(dr, "PapierFacture")
            ModeChantier = If(S(dr, "ModeChantier") = "", "TOUJOURS", S(dr, "ModeChantier"))
            Operateur = S(dr, "Operateur")
        Catch ex As Exception
            Debug.WriteLine("ModuleConfigImpression.Charger : " & ex.Message)
        End Try
    End Sub

    ' Enregistre les variables du module dans la base
    Public Function Enregistrer() As Boolean
        VerifierColonnes()
        Try
            ModuleDb.Executer(
                "UPDATE dbo.ConfigurationSociete SET " &
                " ImprimanteBL=@iBL, ImprimantePeseesBPE=@iBPE, ImprimantePeseesPrefa=@iPre, ImprimanteFacture=@iFac, ImprimantePrincipale=@iPri, " &
                " NbreCopiesBL=@cBL, NbreCopiesFactures=@cFac, " &
                " EnteteBL=@eBL, EnteteTicket=@eTic, EnteteFacture=@eFac, EnteteListing=@eLis, " &
                " LargeurBL=@lBL, HauteurBL=@hBL, LargeurFacture=@lFac, HauteurFacture=@hFac, " &
                " CommentairesBL=@com, SansEntete=@sEnt, SansNorme=@sNor, Draft=@dra, Temperature=@tem, " &
                " PapierBL=@pBL, PapierFacture=@pFac, ModeChantier=@mode, Operateur=@op",
                Sub(cmd)
                    cmd.Parameters.AddWithValue("@iBL", ImprimanteBL)
                    cmd.Parameters.AddWithValue("@iBPE", ImprimantePeseesBPE)
                    cmd.Parameters.AddWithValue("@iPre", ImprimantePeseesPrefa)
                    cmd.Parameters.AddWithValue("@iFac", ImprimanteFacture)
                    cmd.Parameters.AddWithValue("@iPri", ImprimantePrincipale)
                    cmd.Parameters.AddWithValue("@cBL", NbreCopiesBL)
                    cmd.Parameters.AddWithValue("@cFac", NbreCopiesFactures)
                    cmd.Parameters.AddWithValue("@eBL", EnteteBL)
                    cmd.Parameters.AddWithValue("@eTic", EnteteTicket)
                    cmd.Parameters.AddWithValue("@eFac", EnteteFacture)
                    cmd.Parameters.AddWithValue("@eLis", EnteteListing)
                    cmd.Parameters.AddWithValue("@lBL", LargeurBL)
                    cmd.Parameters.AddWithValue("@hBL", HauteurBL)
                    cmd.Parameters.AddWithValue("@lFac", LargeurFacture)
                    cmd.Parameters.AddWithValue("@hFac", HauteurFacture)
                    cmd.Parameters.AddWithValue("@com", CommentairesBL)
                    cmd.Parameters.AddWithValue("@sEnt", EnteteActive)
                    cmd.Parameters.AddWithValue("@sNor", NormeActive)
                    cmd.Parameters.AddWithValue("@dra", Draft)
                    cmd.Parameters.AddWithValue("@tem", Temperature)
                    cmd.Parameters.AddWithValue("@pBL", PapierBL)
                    cmd.Parameters.AddWithValue("@pFac", PapierFacture)
                    cmd.Parameters.AddWithValue("@mode", ModeChantier)
                    cmd.Parameters.AddWithValue("@op", Operateur)
                End Sub)
            Return True
        Catch ex As Exception
            MessageBox.Show("Erreur enregistrement configuration : " & ex.Message)
            Return False
        End Try
    End Function

    ' ── Helpers ────────────────────────────────────────────────
    Private Function S(dr As DataRow, col As String) As String
        If Not dr.Table.Columns.Contains(col) OrElse IsDBNull(dr(col)) Then Return ""
        Return dr(col).ToString().Trim()
    End Function

    Private Function I(dr As DataRow, col As String, defaut As Integer) As Integer
        If Not dr.Table.Columns.Contains(col) OrElse IsDBNull(dr(col)) Then Return defaut
        Dim v As Integer
        If Integer.TryParse(dr(col).ToString(), v) Then Return v
        Return defaut
    End Function

    Private Function B(dr As DataRow, col As String, Optional defaut As Boolean = False) As Boolean
        If Not dr.Table.Columns.Contains(col) OrElse IsDBNull(dr(col)) Then Return defaut
        Try
            Return CBool(dr(col))
        Catch
            Return defaut
        End Try
    End Function

End Module
