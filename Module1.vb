Imports System.Data
Imports S7.Net

' ================================================================
'  Module1.vb — Module global de l application CentraleBeton
'  Variables globales + fonctions utilitaires partagees
'  Date : 17/06/2026
' ================================================================
Module Module1
    ' ═══════════════════════════════════════════════════════════════════
    '  FERMER TOUTES LES FENETRES SAUF PanelMain AVANT D'EN OUVRIR UNE
    ' ═══════════════════════════════════════════════════════════════════


    Public Sub FermerAutresFormulaires()
        Dim liste As New List(Of Form)
        For Each f As Form In Application.OpenForms
            If Not (TypeOf f Is PanelMain) AndAlso Not (TypeOf f Is LoginForm) AndAlso Not (TypeOf f Is Synoptique) Then
                liste.Add(f)
            End If
        Next
        For Each f As Form In liste
            f.Close()
        Next
    End Sub

    ' ═══════════════════════════════════════════════════════════════
    '  INFORMATIONS SOCIETE (chargees au Login)
    ' ═══════════════════════════════════════════════════════════════
    Public SocieteNom As String = ""
    Public SocieteAdresse As String = ""
    Public SocieteVille As String = ""
    Public SocieteTelephone As String = ""
    Public SocieteFax As String = ""
    Public SocieteEmail As String = ""
    Public SocieteRC As String = ""
    Public SocieteIF As String = ""
    Public SocieteICE As String = ""
    Public SocietePatente As String = ""
    Public SocieteLogoPath As String = ""

    ' ═══════════════════════════════════════════════════════════════
    '  CONFIGURATION PLC (chargee au Login)
    ' ═══════════════════════════════════════════════════════════════
    Public PLC_IP As String = "192.168.0.1"
    Public PLC_Rack As Short = 0
    Public PLC_Slot As Short = 1
    Public PLC_Connecte As Boolean = False

    ' Instance PLC partagee entre tous les modules
    Public PLCInstance As Plc = Nothing

    ' ═══════════════════════════════════════════════════════════════
    '  ETAT PRODUCTION EN TEMPS REEL
    ' ═══════════════════════════════════════════════════════════════
    Public Poids_AGR As Single = 0
    Public Poids_CIM As Single = 0
    Public Poids_EAU As Single = 0
    Public Poids_ADJ As Single = 0
    Public N_Gachee As Integer = 0
    Public Cycle_EnCours As Boolean = False
    Public Arret_Urgence As Boolean = False

    ' ═══════════════════════════════════════════════════════════════
    '  PLANIFICATION ACTIVE (commande en cours)
    ' ═══════════════════════════════════════════════════════════════
    Public Plan_IdDemande As Integer = 0
    Public Plan_NomClient As String = ""
    Public Plan_NomChantier As String = ""
    Public Plan_NomFormule As String = ""
    Public Plan_CodeCamion As String = ""
    Public Plan_VolumeM3 As Decimal = 0

    ' ═══════════════════════════════════════════════════════════════
    '  CHARGEMENT SOCIETE (appele dans LoginForm)
    ' ═══════════════════════════════════════════════════════════════
    Public Sub ChargerSociete()
        Try
            Dim dt As DataTable = ModuleDb.Lire(
                "SELECT TOP 1 NomSociete, Adresse, Ville, Telephone, Fax, " &
                "Email, NumeroRC, NumeroIF, NumeroICE, NumeroPatente, LogoPath " &
                "FROM dbo.ConfigurationSociete")

            If dt Is Nothing OrElse dt.Rows.Count = 0 Then Exit Sub
            Dim dr As DataRow = dt.Rows(0)

            SocieteNom = LireDR(dr, "NomSociete")
            SocieteAdresse = LireDR(dr, "Adresse")
            SocieteVille = LireDR(dr, "Ville")
            SocieteTelephone = LireDR(dr, "Telephone")
            SocieteFax = LireDR(dr, "Fax")
            SocieteEmail = LireDR(dr, "Email")
            SocieteRC = LireDR(dr, "NumeroRC")
            SocieteIF = LireDR(dr, "NumeroIF")
            SocieteICE = LireDR(dr, "NumeroICE")
            SocietePatente = LireDR(dr, "NumeroPatente")
            SocieteLogoPath = LireDR(dr, "LogoPath")

        Catch ex As Exception
            MessageBox.Show("Erreur chargement societe : " & ex.Message,
                            "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Warning)
        End Try
    End Sub

    ' ═══════════════════════════════════════════════════════════════
    '  CHARGEMENT CONFIG PLC (appele dans LoginForm)
    ' ═══════════════════════════════════════════════════════════════
    Public Sub ChargerConfigPLC()
        Try
            ' Lire IP PLC depuis la table Machines
            Dim dt As DataTable = ModuleDb.Lire(
                "SELECT TOP 1 Adresse_IP_PLC FROM dbo.Machines WHERE Actif=1")
            If dt IsNot Nothing AndAlso dt.Rows.Count > 0 Then
                Dim ip = dt.Rows(0)("Adresse_IP_PLC").ToString()
                If ip <> "" Then PLC_IP = ip
            End If
        Catch
            ' Garder IP par defaut 192.168.0.1
        End Try
    End Sub

    ' ═══════════════════════════════════════════════════════════════
    '  CONNEXION PLC GLOBALE (appele dans PanelMain_Load)
    ' ═══════════════════════════════════════════════════════════════
    Public Sub ConnecterPLCGlobal()
        Try
            PLCInstance = New Plc(CpuType.S71200, PLC_IP, PLC_Rack, PLC_Slot)
            PLCInstance.Open()
            If PLCInstance.IsConnected Then
                PLC_Connecte = True
                ' Demarrer l archivage automatique des gachees
                ModuleArchivageGachees.Demarrer(PLCInstance)
            End If
        Catch ex As Exception
            PLC_Connecte = False
            Debug.WriteLine("Erreur connexion PLC global : " & ex.Message)
        End Try
    End Sub

    Public Sub DeconnecterPLCGlobal()
        Try
            ModuleArchivageGachees.Instance.Arreter()
            If PLCInstance IsNot Nothing AndAlso PLCInstance.IsConnected Then
                PLCInstance.Close()
            End If
            PLC_Connecte = False
        Catch
        End Try
    End Sub

    ' ═══════════════════════════════════════════════════════════════
    '  CHARGEMENT PLANIFICATION ACTIVE
    ' ═══════════════════════════════════════════════════════════════
    Public Sub ChargerPlanificationActive()
        Try
            Dim dt As DataTable = ModuleDb.Lire(
                "SELECT TOP 1 d.IdDemande, c.NomClient, ch.NomChantier, " &
                "f.NomFormule, cam.CodeCamion, d.VolumeM3 " &
                "FROM dbo.DemandesDosageBeton d " &
                "LEFT JOIN dbo.Clients c      ON c.IdClient    = d.IdClient " &
                "LEFT JOIN dbo.Chantiers ch   ON ch.IdChantier = d.IdChantier " &
                "LEFT JOIN dbo.Formules f     ON f.IdFormule   = d.IdFormule " &
                "LEFT JOIN dbo.Camions cam    ON cam.IdCamion  = d.IdCamion " &
                "WHERE d.StatutDemande = 'EN_COURS' " &
                "ORDER BY d.DateDemande DESC")

            If dt IsNot Nothing AndAlso dt.Rows.Count > 0 Then
                Dim row = dt.Rows(0)
                Plan_IdDemande = CInt(row("IdDemande"))
                Plan_NomClient = row("NomClient").ToString()
                Plan_NomChantier = row("NomChantier").ToString()
                Plan_NomFormule = row("NomFormule").ToString()
                Plan_CodeCamion = row("CodeCamion").ToString()
                Plan_VolumeM3 = CDec(row("VolumeM3"))
            End If
        Catch
        End Try
    End Sub

    ' ═══════════════════════════════════════════════════════════════
    '  INITIALISATION COMPLETE AU LOGIN
    '  Appeler depuis LoginForm apres authentification reussie
    ' ═══════════════════════════════════════════════════════════════
    Public Sub InitialiserApplication()
        ChargerSociete()
        ChargerConfigPLC()
        ChargerPlanificationActive()
        ConnecterPLCGlobal()
    End Sub

    ' ═══════════════════════════════════════════════════════════════
    '  GENERATION NUMERO BL
    ' ═══════════════════════════════════════════════════════════════
    Public Function GenererNumeroBL() As String
        Try
            Dim dernier As Integer = ModuleDb.LireValeur(Of Integer)(
                "SELECT COUNT(*) FROM dbo.BonsLivraison " &
                "WHERE CAST(DateHeure AS DATE) = CAST(GETDATE() AS DATE)")
            Return "BL-" & DateTime.Now.ToString("yyyyMMdd") & "-" &
                   (dernier + 1).ToString("0000")
        Catch
            Return "BL-" & DateTime.Now.ToString("yyyyMMddHHmmss")
        End Try
    End Function

    ' ═══════════════════════════════════════════════════════════════
    '  GENERATION NUMERO FACTURE
    ' ═══════════════════════════════════════════════════════════════
    Public Function GenererNumeroFacture() As String
        Try
            Dim annee As String = DateTime.Now.Year.ToString()
            Dim dernier As Integer = ModuleDb.LireValeur(Of Integer)(
                "SELECT COUNT(*) FROM dbo.Factures " &
                "WHERE YEAR(DateFacture) = " & annee)
            Return "FAC-" & annee & "-" & (dernier + 1).ToString("00000")
        Catch
            Return "FAC-" & DateTime.Now.ToString("yyyyMMddHHmmss")
        End Try
    End Function

    ' ═══════════════════════════════════════════════════════════════
    '  UTILITAIRES
    ' ═══════════════════════════════════════════════════════════════
    Private Function LireDR(dr As DataRow, col As String) As String
        Return If(IsDBNull(dr(col)), "", dr(col).ToString())
    End Function

    Public Function FormatPoids(valeur As Single) As String
        Return valeur.ToString("0.0") & " kg"
    End Function

    Public Function FormatVolume(valeur As Decimal) As String
        Return valeur.ToString("0.00") & " m³"
    End Function

    Public Function FormatMontant(valeur As Decimal) As String
        Return valeur.ToString("#,##0.00") & " DH"
    End Function

    Public Function FormatDate(d As DateTime) As String
        Return d.ToString("dd/MM/yyyy")
    End Function

    Public Function FormatDateHeure(d As DateTime) As String
        Return d.ToString("dd/MM/yyyy HH:mm")
    End Function

End Module