' ================================================================
'  ModuleArchivageGachees.vb
'  Gestion du cycle de dosage :
'  1. Envoyer DYC (M4.5) apres envoi formule
'  2. Surveiller Fin gachee (M64.6) → archiver dans SQL
'  3. Surveiller Fin commande (M50.3) → cloturer BL
' ================================================================

Imports S7.Net
Imports System.Windows.Forms

Public Class ModuleArchivageGachees

    ' ── Adresses PLC ─────────────────────────────────────────────
    Private Const ADR_DYC As String = "DB1.DBX0.4"   ' Depart cycle
    Private Const ADR_FIN_GACHEE As String = "DB1.DBX1082.4"  ' Fin gachee
    Private Const ADR_FIN_COMMANDE As String = "DB1.DBX914.1"  ' Fin commande

    ' ── Archivage depuis la table DB71 du CPU (matricule illimité) ──
    '  True  : chaque gâchée est archivée depuis DB71 avec son MATRICULE
    '          (compteur CPU jamais remis à zéro). Rattrape les gâchées
    '          manquées si le PC était éteint (40 dernières).
    '  False : ancien fonctionnement (bit Fin gâchée + valeurs DB1).
    Public Shared UTILISER_DB71 As Boolean = True
    Private _dernierMatricule As Long = -1        ' MATRICULE LOGICIEL (SQL) : jamais remis à zéro ; -1 = pas chargé
    Private _dernierCompteurCPU As Long = -1      ' dernier compteur CPU archivé (pour le rattrapage au démarrage)
    Private _snapDB71() As Long = Nothing         ' compteurs des 40 emplacements vus la dernière fois
    Private _rattrapageEnCours As Boolean = False
    Private _colonneMatriculeOK As Boolean = False

    ' ── Variables ────────────────────────────────────────────────
    Private _plc As Plc = Nothing
    Private _numeroBL As String = ""
    Private _idPlanning As Integer = 0
    Private _nGachee As Integer = 0
    Private _actif As Boolean = False

    ' Cumuls par camion
    Private _totalConsAGR As Double = 0
    Private _totalPoseAGR As Double = 0
    Private _totalEcartAGR As Double = 0
    Private _totalConsCIM As Double = 0
    Private _totalPoseCIM As Double = 0
    Private _totalEcartCIM As Double = 0
    Private _totalConsEAU As Double = 0
    Private _totalPoseEAU As Double = 0
    Private _totalEcartEAU As Double = 0
    Private _totalConsADJ As Double = 0
    Private _totalPoseADJ As Double = 0
    Private _totalEcartADJ As Double = 0
    ' Timer supprime - surveillance via SurveillanceCycleUI appelee depuis Synoptique.Timer2

    ' ── Proprietes publiques pour affichage Synoptique ──────────
    Public ReadOnly Property NumeroBL As String
        Get
            Return _numeroBL
        End Get
    End Property
    Public ReadOnly Property NombreGachees As Integer
        Get
            Return _nGachee
        End Get
    End Property
    Public ReadOnly Property VolumeLivre As Double
        Get
            Return CDbl(LireDBD(1, 920))
        End Get
    End Property
    Public ReadOnly Property EstActif As Boolean
        Get
            Return _actif
        End Get
    End Property
    Public ReadOnly Property IdPlanning As Integer
        Get
            Return _idPlanning
        End Get
    End Property

    ' ── Singleton ────────────────────────────────────────────────
    Private Shared _instance As ModuleArchivageGachees
    Public Shared ReadOnly Property Instance As ModuleArchivageGachees
        Get
            If _instance Is Nothing Then _instance = New ModuleArchivageGachees()
            Return _instance
        End Get
    End Property

    ' ════════════════════════════════════════════════════════════
    '  DEMARRER LE CYCLE
    '  Appeler depuis Form_ConfirmationBL apres EnvoyerVersCPU
    ' ════════════════════════════════════════════════════════════
    ' ════════════════════════════════════════════════════════════
    '  REPRISE DU BL EN COURS (18/09)
    '  Si le logiciel a été fermé / relancé pendant un chargement, le BL
    '  'EN_COURS' le plus récent est repris : les gâchées rattrapées depuis
    '  DB71 lui sont rattachées (y compris celles archivées sans BL pendant
    '  la coupure) et la fin de commande redemande l'impression du BL.
    ' ════════════════════════════════════════════════════════════
    Private _restaurationFaite As Boolean = False
    Private Sub RestaurerBLEnCours()
        _restaurationFaite = True
        If _numeroBL <> "" Then Exit Sub
        Try
            Dim dt As DataTable = ModuleDb.Lire(
                "SELECT TOP 1 NumeroBL, ISNULL(IdPlanning,0) AS IdPlanning, DateCreation " &
                "FROM dbo.BonsLivraison WHERE Statut='EN_COURS' ORDER BY DateCreation DESC")
            If dt Is Nothing OrElse dt.Rows.Count = 0 Then Exit Sub
            Dim bl As String = dt.Rows(0)("NumeroBL").ToString().Trim()
            If bl = "" Then Exit Sub
            Dim dateCrea As Date = If(IsDBNull(dt.Rows(0)("DateCreation")), Date.Today, CDate(dt.Rows(0)("DateCreation")))

            ' Gâchées archivées sans BL depuis la création de ce BL → rattachées
            ModuleDb.Executer(
                "UPDATE dbo.GacheesDetails SET NumeroBL=@bl WHERE ISNULL(NumeroBL,'')='' AND DateHeure >= @d",
                Sub(cmd)
                    cmd.Parameters.AddWithValue("@bl", bl)
                    cmd.Parameters.AddWithValue("@d", dateCrea)
                End Sub)
            ' Renumérotation des gâchées du BL dans l'ordre chronologique
            ModuleDb.Executer(
                "WITH g AS (SELECT NumeroGachee, ROW_NUMBER() OVER (ORDER BY DateHeure, MatriculeGachee) AS rn " &
                "           FROM dbo.GacheesDetails WHERE NumeroBL=@bl) UPDATE g SET NumeroGachee = rn",
                Sub(cmd) cmd.Parameters.AddWithValue("@bl", bl))

            _numeroBL = bl
            _idPlanning = CInt(dt.Rows(0)("IdPlanning"))
            _nGachee = ModuleDb.LireValeur(Of Integer)(
                "SELECT COUNT(*) FROM dbo.GacheesDetails WHERE NumeroBL=@bl",
                Sub(cmd) cmd.Parameters.AddWithValue("@bl", bl))
            _actif = True
            Synoptique.EnregistrerAction("COMMANDE", "BL en cours repris après redémarrage",
                "BL=" & bl & " gâchées déjà archivées=" & _nGachee, "", "", "AUTO")
        Catch ex As Exception
            Synoptique.EnregistrerAction("ALARME", "Reprise BL en cours : erreur", ex.Message, "", "", "ALARME")
        End Try
    End Sub

    ' ════════════════════════════════════════════════════════════
    '  FIN DE CYCLE DEMANDÉE PAR L'OPÉRATEUR (18/09) — protocole CPU
    '   clic bouton  → DB1.DBX914.2 = 1  (demande d'arrêt en fin de gâchée)
    '   CPU          → DB1.DBX1088.0 = 1 quand la gâchée en cours est finie
    '   confirmation → OUI : DB1.DBX914.3 = 1 (arrêt, puis fin de commande 914.1)
    '                  NON : DB1.DBX914.2 = 0 (le CPU continue les gâchées)
    ' ════════════════════════════════════════════════════════════
    Public Const ADR_DEMANDE_ARRET As String = "DB1.DBX914.2"
    Public Const ADR_CONFIRME_ARRET As String = "DB1.DBX914.3"
    Public Const ADR_GACHEE_FINIE As String = "DB1.DBX1088.0"
    Private _abandonDemande As Boolean = False
    Private _abandonConfirmationAffichee As Boolean = False

    Public ReadOnly Property AbandonEnCours As Boolean
        Get
            Return _abandonDemande
        End Get
    End Property

    ' Renvoie False si aucun BL n'est en cours
    ' 22/09 : 914.2 = 1 ; la confirmation OUI / NON (lbfin / finoui / finno du
    '   Synoptique) n'est affichée que lorsque la CPU met 1088.0 = 1 (fin de la
    '   gâchée en cours) → voir SurveillanceCycleUI → ConfirmerFinCycle(True/False)
    Public Function DemanderFinCycle() As Boolean
        If _numeroBL = "" Then Return False
        _abandonDemande = True
        _abandonConfirmationAffichee = False
        ModulePLCDuplex.EcrireBit(ADR_DEMANDE_ARRET, True)
        Synoptique.EnregistrerAction("COMMANDE", "Fin de cycle demandée",
            "BL=" & _numeroBL & " gâchées faites=" & _nGachee, ADR_DEMANDE_ARRET, "1", "CLIC")
        Return True
    End Function

    ' OUI → 914.3 = 1 (la CPU termine la commande → 914.1 = 1 → clôture du BL)
    ' NON → 914.2 = 0 (le cycle continue)
    Public Sub ConfirmerFinCycle(oui As Boolean)
        If Not _abandonDemande Then Exit Sub
        If oui Then
            ModulePLCDuplex.EcrireBit(ADR_CONFIRME_ARRET, True)
            Synoptique.EnregistrerAction("COMMANDE", "Fin de cycle CONFIRMÉE (OUI)",
                "BL=" & _numeroBL & " gâchées=" & _nGachee, ADR_CONFIRME_ARRET, "1", "CLIC")
        Else
            ModulePLCDuplex.EcrireBit(ADR_DEMANDE_ARRET, False)
            _abandonDemande = False : _abandonConfirmationAffichee = False
            Synoptique.EnregistrerAction("COMMANDE", "Fin de cycle ANNULÉE (NON) - le cycle continue",
                "BL=" & _numeroBL, ADR_DEMANDE_ARRET, "0", "CLIC")
        End If
    End Sub

    Public ReadOnly Property NumeroBLEnCours As String
        Get
            Return _numeroBL
        End Get
    End Property

    Public ReadOnly Property NbGacheesFaites As Integer
        Get
            Return _nGachee
        End Get
    End Property

    ' ── Clôture d'un BL resté EN_COURS alors que le CPU a terminé (18/09) ──
    '    Appelé par Form_ConfirmationBL quand 914.1 = 1 au lancement suivant.
    Public Sub CloturerBLManque(numBL As String, motif As String)
        Try
            If numBL = "" Then Exit Sub
            EnregistrerCumulsCPU(numBL)
            ModuleDb.Executer(
                "UPDATE dbo.BonsLivraison SET Statut='LIVRE', DateModification=GETDATE() WHERE NumeroBL=@bl AND Statut='EN_COURS'",
                Sub(cmd) cmd.Parameters.AddWithValue("@bl", numBL))
            If _numeroBL = numBL Then
                _numeroBL = "" : _idPlanning = 0 : _nGachee = 0 : _actif = False
            End If
            Synoptique.EnregistrerAction("COMMANDE", "BL clôturé automatiquement",
                "BL=" & numBL & " | " & motif, ADR_FIN_COMMANDE, "1", "AUTO")
        Catch ex As Exception
            Synoptique.EnregistrerAction("ALARME", "Clôture du BL manquée : erreur", ex.Message, "", "", "ALARME")
        End Try
    End Sub

    Public Sub DemarrerCycle(plc As Plc, numeroBL As String, idPlanning As Integer)
        _plc = plc
        _numeroBL = numeroBL
        _idPlanning = idPlanning
        _nGachee = 0
        _actif = True
        _totalConsAGR = 0 : _totalPoseAGR = 0 : _totalEcartAGR = 0
        _totalConsCIM = 0 : _totalPoseCIM = 0 : _totalEcartCIM = 0
        _totalConsEAU = 0 : _totalPoseEAU = 0 : _totalEcartEAU = 0
        _totalConsADJ = 0 : _totalPoseADJ = 0 : _totalEcartADJ = 0

        ' 22/09 : correction eau à 0 au lancement d'une commande
        Synoptique.RemettreCorrectionEauZero("nouvelle commande BL=" & numeroBL)

        ' 1. Envoyer bit Depart Cycle (DYC) une seule fois
        Try
            ' 21/09 : bit MAINTENU à 1 (plus d'impulsion) — la CPU le remet à 0 en fin de commande
            ModulePLCDuplex.EcrireBit(ADR_DYC, True)
            Synoptique.EnregistrerAction("CYCLE", "Départ cycle envoyé",
                "BL=" & numeroBL & " (0.4 maintenu a 1)", ADR_DYC, "1", "PLC_WRITE")
        Catch ex As Exception
            Debug.WriteLine("Erreur DYC : " & ex.Message)
        End Try

        ' Surveillance via SurveillanceCycleUI appelee depuis Synoptique.Timer2
    End Sub

    ' ════════════════════════════════════════════════════════════
    '  SURVEILLANCE — appelee depuis Synoptique.Timer2
    ' ════════════════════════════════════════════════════════════
    Public Sub SurveillanceCycleUI(plc As S7.Net.Plc)

        ' ── GARDE-FOU : liaison non active → on n'interprete RIEN ──
        ' On NE clot RIEN, on NE reset RIEN, on NE derive AUCUNE BL.
        ' Le BL en cours reste affiche "en attente" jusqu'au retour PLC.
        If plc Is Nothing OrElse Not plc.IsConnected Then
            PLCData.Connecte = False
            Exit Sub
        End If

        _plc = plc

        ' ── 18/09 : reprise du BL EN_COURS après redémarrage du logiciel ──
        If Not _restaurationFaite Then RestaurerBLEnCours()

        Try
            ' ── Gâchées : depuis la table DB71 (matricule) ───────
            If UTILISER_DB71 Then
                ' Lecture réseau (jusqu'à 17 trames) → hors thread écran
                If Not _rattrapageEnCours Then
                    _rattrapageEnCours = True
                    Threading.ThreadPool.QueueUserWorkItem(Sub(state)
                                                               Try
                                                                   RattraperDB71()
                                                               Finally
                                                                   _rattrapageEnCours = False
                                                               End Try
                                                           End Sub)
                End If
            End If

            ' ── Fin gachee (M64.6) — ancien mode (si DB71 désactivé) ──
            If _actif AndAlso Not UTILISER_DB71 Then
                Dim finGachee As Boolean
                Try
                    finGachee = CBool(ModulePLCDuplex.LireAdresse(ADR_FIN_GACHEE))
                Catch
                    ' Lecture echouee → connexion pas fiable : on attend
                    PLCData.Connecte = False
                    Exit Sub
                End Try

                If finGachee Then
                    ArchiverGachee()
                    ModulePLCDuplex.EcrireBit(ADR_FIN_GACHEE, False)
                End If
            End If

            ' ── Fin commande (M50.3) — lecture protegee ───────────
            ' 25/09 : pendant le traitement de fin de commande (arrière-plan), on ne retraite pas
            If _numeroBL <> "" AndAlso Not _finCommandeEnCours Then
                Dim finCommande As Boolean
                Try
                    finCommande = CBool(ModulePLCDuplex.LireAdresse(ADR_FIN_COMMANDE))
                Catch
                    PLCData.Connecte = False
                    Exit Sub
                End Try

                ' 22/09 : demande de fin de cycle en attente → dès que la gâchée en cours
                '         est finie (1088.0 = 1), afficher OUI / NON sur le Synoptique (une fois)
                If _abandonDemande AndAlso Not _abandonConfirmationAffichee AndAlso Not finCommande Then
                    Dim gacheeFinie As Boolean = False
                    Try
                        gacheeFinie = CBool(ModulePLCDuplex.LireAdresse(ADR_GACHEE_FINIE))
                    Catch
                    End Try
                    If gacheeFinie Then
                        _abandonConfirmationAffichee = True
                        Synoptique.MontrerConfirmationFinCycle()
                        Synoptique.EnregistrerAction("COMMANDE", "Fin de cycle : gâchée finie (1088.0) - confirmation OUI/NON affichée",
                            "BL=" & _numeroBL & " gâchées=" & _nGachee, ADR_GACHEE_FINIE, "1", "AUTO")
                    End If
                End If

                If finCommande Then
                    ' 25/09 : tout le traitement (attente DB71, SQL, cumuls) part en arrière-plan ;
                    '         la fenêtre OUI/NON/QUITTER et l'impression reviennent sur le thread écran.
                    '         Avant : Sleep jusqu'à 4,5 s + 17 trames + 6 requêtes SQL sur le thread UI → écran figé.
                    _finCommandeEnCours = True
                    Dim abandon As Boolean = _abandonDemande
                    If abandon Then
                        ModulePLCDuplex.EcrireBit(ADR_DEMANDE_ARRET, False)
                        ModulePLCDuplex.EcrireBit(ADR_CONFIRME_ARRET, False)
                    End If
                    _abandonDemande = False : _abandonConfirmationAffichee = False
                    _actif = False
                    Threading.ThreadPool.QueueUserWorkItem(Sub(state) TraiterFinCommande(abandon))
                End If
            End If

            PLCData.Connecte = True

        Catch ex As Exception
            PLCData.Connecte = False
            Debug.WriteLine("SurveillanceCycleUI : " & ex.Message)
        End Try
    End Sub

    Private _finCommandeEnCours As Boolean = False

    ' ════════════════════════════════════════════════════════════
    '  FIN DE COMMANDE (25/09) — exécutée dans le ThreadPool
    ' ════════════════════════════════════════════════════════════
    Private Sub TraiterFinCommande(abandon As Boolean)
        Try
            Dim motifFin As String = If(abandon, "Fin de cycle operateur - BL etabli avec les gachees faites", "Livraison auto M50.3")

            ' ── 16/09 : rattacher la DERNIÈRE gâchée au BL avant de le clôturer.
            If UTILISER_DB71 Then
                Dim attente As Integer = 0
                While _rattrapageEnCours AndAlso attente < 30
                    System.Threading.Thread.Sleep(100) : attente += 1
                End While
                _rattrapageEnCours = True
                Try
                    System.Threading.Thread.Sleep(1500)   ' laisser le CPU finir d'écrire DB71
                    RattraperDB71(forcer:=True)
                Finally
                    _rattrapageEnCours = False
                End Try
            End If

            ' ── 21/09 : VOLUME LIVRÉ = gâchées réellement faites × volume malaxeur
            '    quand la commande n'est pas allée au bout (abandon) ; sinon cubage camion.
            Dim nbFaites As Integer = _nGachee
            Try
                nbFaites = ModuleDb.LireValeur(Of Integer)(
                    "SELECT COUNT(*) FROM dbo.GacheesDetails WHERE NumeroBL=@bl",
                    Sub(cmd) cmd.Parameters.AddWithValue("@bl", _numeroBL))
            Catch
            End Try
            Dim volCamion As Double = 0
            Dim vMax As Double = 0
            Try
                Dim o As Object = ModuleDb.LireValeur(Of Object)(
                    "SELECT ISNULL(VolumeM3,0) FROM dbo.BonsLivraison WHERE NumeroBL=@bl",
                    Sub(cmd) cmd.Parameters.AddWithValue("@bl", _numeroBL))
                If o IsNot Nothing AndAlso Not IsDBNull(o) Then volCamion = CDbl(o)
                o = ModuleDb.LireValeur(Of Object)(
                    "SELECT ISNULL(VolumeRotationMaxi,0) FROM dbo.Planning_journalier WHERE IdPlanning=@id",
                    Sub(cmd) cmd.Parameters.AddWithValue("@id", _idPlanning))
                If o IsNot Nothing AndAlso Not IsDBNull(o) Then vMax = CDbl(o)
            Catch
            End Try
            If vMax <= 0 Then vMax = PLCData.CapMalaxeur
            Dim nbPrevu As Integer = If(vMax > 0 AndAlso volCamion > 0, CInt(Math.Ceiling(volCamion / vMax - 0.0001)), 0)
            Dim volLivre As Double
            Dim volReduit As Boolean = False
            If nbFaites > 0 AndAlso vMax > 0 AndAlso (abandon OrElse (nbPrevu > 0 AndAlso nbFaites < nbPrevu)) Then
                volLivre = Math.Round(nbFaites * vMax, 3)
                If volCamion > 0 AndAlso volLivre > volCamion Then volLivre = volCamion
                volReduit = (volCamion <= 0 OrElse volLivre < volCamion - 0.0005)
            ElseIf volCamion > 0 Then
                volLivre = volCamion
            Else
                volLivre = CDbl(LireDBD(1, 920))
            End If
            If volReduit Then
                ModuleDb.Executer(
                "UPDATE dbo.BonsLivraison SET VolumeM3=@vol WHERE NumeroBL=@bl",
                Sub(cmd)
                    cmd.Parameters.AddWithValue("@vol", volLivre)
                    cmd.Parameters.AddWithValue("@bl", _numeroBL)
                End Sub)
                Synoptique.EnregistrerAction("COMMANDE", "Volume BL réduit (fin de cycle)",
                    "BL=" & _numeroBL & " gâchées=" & nbFaites & " x " & vMax.ToString("0.00") & " m3 = " & volLivre.ToString("0.00") & " m3 (camion " & volCamion.ToString("0.00") & ")",
                    "", "", "AUTO")
            End If

            ModuleDb.Executer(
            "UPDATE dbo.Planning_journalier SET " &
            " QuantiteLivree = ISNULL(QuantiteLivree,0) + @vol, " &
            " StatutPlanning = 'Terminé', DateModification = GETDATE() " &
            "WHERE IdPlanning = @id",
            Sub(cmd)
                cmd.Parameters.AddWithValue("@vol", volLivre)
                cmd.Parameters.AddWithValue("@id", _idPlanning)
            End Sub)
            ModuleDb.Executer(
            "UPDATE dbo.BonsLivraison SET Statut='LIVRE', " &
            " QteCumulee = ISNULL((SELECT QuantiteLivree FROM dbo.Planning_journalier WHERE IdPlanning=@id), ISNULL(QteCumulee,0) + @vol) " &
            "WHERE NumeroBL=@bl",
            Sub(cmd)
                cmd.Parameters.AddWithValue("@bl", _numeroBL)
                cmd.Parameters.AddWithValue("@id", _idPlanning)
                cmd.Parameters.AddWithValue("@vol", volLivre)
            End Sub)
            Synoptique.EnregistrerAction("COMMANDE", motifFin,
            "BL=" & _numeroBL & " Vol=" & volLivre.ToString("0.00") & "m3 gâchées=" & nbFaites,
            ADR_FIN_COMMANDE, If(abandon, "abandon", "1"), "PLC_WRITE")

            ' 18/09 : cumuls REEL / THEORIQUE du camion calculés par le CPU
            EnregistrerCumulsCPU(_numeroBL)

            Dim blLivre As String = _numeroBL
            Dim gachees As Integer = nbFaites
            _numeroBL = ""
            _idPlanning = 0
            _nGachee = 0

            ' ── Fenêtre OUI / NON / QUITTER + impression : sur le thread écran
            Dim ui As Control = Synoptique.Instance
            Dim finUI As Action = Sub() ChoisirImpressionFinCommande(blLivre, gachees, volLivre)
            If ui IsNot Nothing AndAlso ui.IsHandleCreated AndAlso Not ui.IsDisposed Then
                ui.BeginInvoke(finUI)
            Else
                finUI()
            End If
        Catch ex As Exception
            Debug.WriteLine("TraiterFinCommande : " & ex.Message)
            Synoptique.EnregistrerAction("ALARME", "Fin de commande : erreur", ex.Message, "", "", "ALARME")
        Finally
            _finCommandeEnCours = False
        End Try
    End Sub

    Private Sub ChoisirImpressionFinCommande(blLivre As String, gachees As Integer, volLivre As Double)
        Try
            ' ── 16/09 : OUI = BL avec détail / NON = sans détail / QUITTER = pas d'impression
            Dim choix As DialogResult = Form_ChoixImpressionBL.Demander(blLivre, gachees, volLivre)
            If choix = DialogResult.Yes Then
                ModuleImpressionBL.ImprimerBL(blLivre, avecDetail:=True)
                Synoptique.EnregistrerAction("IMPRESSION", "BL imprimé avec détail", "BL=" & blLivre, "", "", "CLIC")
            ElseIf choix = DialogResult.No Then
                ModuleImpressionBL.ImprimerBL(blLivre, avecDetail:=False)
                Synoptique.EnregistrerAction("IMPRESSION", "BL imprimé sans détail", "BL=" & blLivre, "", "", "CLIC")
            Else
                Synoptique.EnregistrerAction("IMPRESSION", "BL non imprimé (Quitter)", "BL=" & blLivre, "", "", "CLIC")
            End If
            ' 19/09 : acquit fin de chargement (914.1 = 0) seulement après OUI ou NON (impression).
            If choix = DialogResult.Yes OrElse choix = DialogResult.No Then
                ModulePLCDuplex.EcrireBit(ADR_FIN_COMMANDE, False)
                Synoptique.EnregistrerAction("COMMANDE", "Acquit fin de chargement (après impression)", "BL=" & blLivre, ADR_FIN_COMMANDE, "0", "AUTO")
            End If
        Catch ex As Exception
            Debug.WriteLine("ChoisirImpressionFinCommande : " & ex.Message)
        End Try
    End Sub

    ' ════════════════════════════════════════════════════════════
    '  ARCHIVER UNE GACHEE
    ' ════════════════════════════════════════════════════════════
    Private Sub ArchiverGachee()
        Try
            _nGachee += 1

            ' ── Consignes (DB8) ──────────────────────────────────
            Dim consG1 As Single = LireDBD(1, 118)
            Dim consSC As Single = LireDBD(1, 122)
            Dim consSL As Single = LireDBD(1, 126)
            Dim consD2 As Single = LireDBD(1, 130)
            Dim consCIM As Single = LireDBD(1, 94)
            Dim consEAU As Single = LireDBD(18, 20)
            Dim consADJ1 As Single = LireDBD(1, 174)
            Dim consADJ2 As Single = LireDBD(1, 178)
            Dim consADJ3 As Single = LireDBD(1, 182)
            Dim consADJ4 As Single = LireDBD(1, 186)

            ' ── Poses reelles (DB18) ─────────────────────────────
            Dim poseG1 As Single = LireDBD(1, 230)
            Dim poseSC As Single = LireDBD(1, 234)
            Dim poseSL As Single = LireDBD(1, 238)
            Dim poseD2 As Single = LireDBD(1, 242)
            Dim poseS1 As Single = LireDBD(1, 206)  ' Silo 1
            Dim poseS2 As Single = LireDBD(1, 210)  ' Silo 2
            Dim poseEAU As Single = LireDBD(1, 278)
            Dim poseADJ1 As Single = LireDBD(1, 286)
            Dim poseADJ2 As Single = LireDBD(1, 290)
            Dim poseADJ3 As Single = LireDBD(1, 294)
            Dim poseADJ4 As Single = LireDBD(1, 298)

            ' ── Ecarts (DB18/DB4) ────────────────────────────────
            Dim ecarG1 As Single = LireDBD(1, 346)
            Dim ecarSC As Single = LireDBD(1, 350)
            Dim ecarSL As Single = LireDBD(1, 354)
            Dim ecarD2 As Single = LireDBD(1, 358)
            Dim ecarCIM As Single = LireDBD(1, 318)
            Dim ecarEAU As Single = LireDBD(1, 394)
            Dim ecarADJ1 As Single = LireDBD(1, 402)
            Dim ecarADJ2 As Single = LireDBD(1, 406)
            Dim ecarADJ3 As Single = LireDBD(1, 410)
            Dim ecarADJ4 As Single = LireDBD(1, 414)

            ' ── Totaux granulats ─────────────────────────────────
            Dim totalConsGran As Single = consG1 + consSC + consSL + consD2
            Dim totalPoseGran As Single = poseG1 + poseSC + poseSL + poseD2
            Dim totalEcartGran As Single = ecarG1 + ecarSC + ecarSL + ecarD2

            ' ── Cumuler les totaux par camion ────────────────────
            _totalConsAGR += CDbl(consG1 + consSC + consSL + consD2)
            _totalPoseAGR += CDbl(poseG1 + poseSC + poseSL + poseD2)
            _totalEcartAGR += CDbl(ecarG1 + ecarSC + ecarSL + ecarD2)
            _totalConsCIM += CDbl(consCIM)
            _totalPoseCIM += CDbl(poseS1 + poseS2)
            _totalEcartCIM += CDbl(ecarCIM)
            _totalConsEAU += CDbl(consEAU)
            _totalPoseEAU += CDbl(poseEAU)
            _totalEcartEAU += CDbl(ecarEAU)
            _totalConsADJ += CDbl(consADJ1 + consADJ2 + consADJ3 + consADJ4)
            _totalPoseADJ += CDbl(poseADJ1 + poseADJ2 + poseADJ3 + poseADJ4)
            _totalEcartADJ += CDbl(ecarADJ1 + ecarADJ2 + ecarADJ3 + ecarADJ4)

            ' ── INSERT dbo.GacheesDetails (Tr1/Tr2/Tr3/Tr4) ─────
            ModuleDb.Executer(
                "INSERT INTO dbo.GacheesDetails (" &
                "  DateHeure, NumeroBL, NumeroGachee, " &
                "  ConsTr1, PoseTr1, EcartTr1, " &
                "  ConsTr2, PoseTr2, EcartTr2, " &
                "  ConsTr3, PoseTr3, EcartTr3, " &
                "  ConsTr4, PoseTr4, EcartTr4, " &
                "  ConsS1,  PoseS1,  ConsS2,  PoseS2,  EcartCIM, " &
                "  ConsEAU, PoseEAU, EcartEAU, " &
                "  ConsADJ1, PoseADJ1, EcartADJ1, " &
                "  ConsADJ2, PoseADJ2, EcartADJ2, " &
                "  ConsADJ3, PoseADJ3, EcartADJ3, " &
                "  ConsADJ4, PoseADJ4, EcartADJ4, " &
                "  TotalConsTr, TotalPoseTr, TotalEcartTr, " &
                "  Operateur) " &
                "VALUES (" &
                "  GETDATE(), @bl, @ng, " &
                "  @ct1, @pt1, @et1, " &
                "  @ct2, @pt2, @et2, " &
                "  @ct3, @pt3, @et3, " &
                "  @ct4, @pt4, @et4, " &
                "  @cs1, @ps1, @cs2, @ps2, @ecim, " &
                "  @ceau, @peau, @eeau, " &
                "  @ca1, @pa1, @ea1, " &
                "  @ca2, @pa2, @ea2, " &
                "  @ca3, @pa3, @ea3, " &
                "  @ca4, @pa4, @ea4, " &
                "  @tcg, @tpg, @teg, " &
                "  @op)",
                Sub(cmd)
                    cmd.Parameters.AddWithValue("@bl", _numeroBL)
                    cmd.Parameters.AddWithValue("@ng", _nGachee)
                    cmd.Parameters.AddWithValue("@ct1", CDbl(consG1)) : cmd.Parameters.AddWithValue("@pt1", CDbl(poseG1)) : cmd.Parameters.AddWithValue("@et1", CDbl(ecarG1))
                    cmd.Parameters.AddWithValue("@ct2", CDbl(consSC)) : cmd.Parameters.AddWithValue("@pt2", CDbl(poseSC)) : cmd.Parameters.AddWithValue("@et2", CDbl(ecarSC))
                    cmd.Parameters.AddWithValue("@ct3", CDbl(consSL)) : cmd.Parameters.AddWithValue("@pt3", CDbl(poseSL)) : cmd.Parameters.AddWithValue("@et3", CDbl(ecarSL))
                    cmd.Parameters.AddWithValue("@ct4", CDbl(consD2)) : cmd.Parameters.AddWithValue("@pt4", CDbl(poseD2)) : cmd.Parameters.AddWithValue("@et4", CDbl(ecarD2))
                    cmd.Parameters.AddWithValue("@cs1", CDbl(consCIM)) : cmd.Parameters.AddWithValue("@ps1", CDbl(poseS1))
                    cmd.Parameters.AddWithValue("@cs2", CDbl(consCIM)) : cmd.Parameters.AddWithValue("@ps2", CDbl(poseS2)) : cmd.Parameters.AddWithValue("@ecim", CDbl(ecarCIM))
                    cmd.Parameters.AddWithValue("@ceau", CDbl(consEAU)) : cmd.Parameters.AddWithValue("@peau", CDbl(poseEAU)) : cmd.Parameters.AddWithValue("@eeau", CDbl(ecarEAU))
                    cmd.Parameters.AddWithValue("@ca1", CDbl(consADJ1)) : cmd.Parameters.AddWithValue("@pa1", CDbl(poseADJ1)) : cmd.Parameters.AddWithValue("@ea1", CDbl(ecarADJ1))
                    cmd.Parameters.AddWithValue("@ca2", CDbl(consADJ2)) : cmd.Parameters.AddWithValue("@pa2", CDbl(poseADJ2)) : cmd.Parameters.AddWithValue("@ea2", CDbl(ecarADJ2))
                    cmd.Parameters.AddWithValue("@ca3", CDbl(consADJ3)) : cmd.Parameters.AddWithValue("@pa3", CDbl(poseADJ3)) : cmd.Parameters.AddWithValue("@ea3", CDbl(ecarADJ3))
                    cmd.Parameters.AddWithValue("@ca4", CDbl(consADJ4)) : cmd.Parameters.AddWithValue("@pa4", CDbl(poseADJ4)) : cmd.Parameters.AddWithValue("@ea4", CDbl(ecarADJ4))
                    cmd.Parameters.AddWithValue("@tcg", CDbl(totalConsGran))
                    cmd.Parameters.AddWithValue("@tpg", CDbl(totalPoseGran))
                    cmd.Parameters.AddWithValue("@teg", CDbl(totalEcartGran))
                    cmd.Parameters.AddWithValue("@op", NomUtilisateur)
                End Sub)
            ' ── Décrémenter les stocks (quantités réelles dosées) ──
            DecrementerStocks(poseG1, poseSC, poseSL, poseD2,
                              poseS1, poseS2, poseEAU,
                              poseADJ1, poseADJ2, poseADJ3, poseADJ4)
            Synoptique.EnregistrerAction("GACHEE", "Gâchée " & _nGachee & " archivée",
                String.Format("BL={0} G{1} | G1={2} SC={3} CIM={4} EAU={5} | EcarCIM={6}",
                    _numeroBL, _nGachee, poseG1, poseSC, poseS1 + poseS2, poseEAU, ecarCIM),
                "", "", "CLIC")

        Catch ex As Exception
            Debug.WriteLine("ArchiverGachee : " & ex.Message)
        End Try
    End Sub

    ' ════════════════════════════════════════════════════════════
    '  DB71 — RATTRAPAGE : archive toutes les gâchées du CPU dont le
    '  matricule est > dernier matricule présent en SQL.
    '  Appelé en arrière-plan toutes les ~3 s (Timer2 du Synoptique).
    ' ════════════════════════════════════════════════════════════
    Public ReadOnly Property DernierMatricule As Long
        Get
            Return Math.Max(0, _dernierMatricule)
        End Get
    End Property

    Private Sub AssurerColonneMatricule()
        If _colonneMatriculeOK Then Exit Sub
        Try
            ModuleDb.Executer(
                "IF COL_LENGTH('dbo.GacheesDetails','MatriculeGachee') IS NULL " &
                "ALTER TABLE dbo.GacheesDetails ADD MatriculeGachee BIGINT NULL")
            ' 26/09 : stock PAR SILO (kg) — deux silos peuvent contenir le même matériau
            ModuleDb.Executer(
                "IF COL_LENGTH('dbo.Silos','StockKg') IS NULL " &
                "ALTER TABLE dbo.Silos ADD StockKg FLOAT NULL")
            ModuleDb.Executer(
                "IF COL_LENGTH('dbo.GacheesDetails','CompteurCPU') IS NULL " &
                "ALTER TABLE dbo.GacheesDetails ADD CompteurCPU BIGINT NULL")
            ' 22/09 : ajout d'eau manuel (DB71.DBW3360…)
            ModuleDb.Executer(
                "IF COL_LENGTH('dbo.GacheesDetails','AjoutEau') IS NULL " &
                "ALTER TABLE dbo.GacheesDetails ADD AjoutEau FLOAT NULL")
            _colonneMatriculeOK = True
        Catch ex As Exception
            Debug.WriteLine("AssurerColonneMatricule : " & ex.Message)
        End Try
    End Sub

    Private Sub ChargerDernierMatricule()
        If _dernierMatricule >= 0 Then Exit Sub
        Try
            Dim v As Object = ModuleDb.LireValeur(Of Object)(
                "SELECT ISNULL(MAX(MatriculeGachee),0) FROM dbo.GacheesDetails")
            _dernierMatricule = If(v Is Nothing OrElse IsDBNull(v), 0L, CLng(v))
            ' Dernier compteur CPU archivé (les anciennes lignes ont MatriculeGachee = compteur CPU)
            Dim c As Object = ModuleDb.LireValeur(Of Object)(
                "SELECT ISNULL(MAX(CompteurCPU), ISNULL(MAX(MatriculeGachee),0)) FROM dbo.GacheesDetails")
            _dernierCompteurCPU = If(c Is Nothing OrElse IsDBNull(c), 0L, CLng(c))
        Catch ex As Exception
            Debug.WriteLine("ChargerDernierMatricule : " & ex.Message)
            _dernierMatricule = 0 : _dernierCompteurCPU = 0
        End Try
    End Sub

    ' Dernière erreur DB71 déjà signalée dans le journal (pour ne pas le remplir)
    Private _derniereErreurDB71Signalee As String = ""

    ' ── Cumuls camion calculés par le CPU (réel / théorique), enregistrés
    '    avec le BL pour l'impression (colonnes créées automatiquement) ──
    Private _colonnesCumulsOK As Boolean = False
    Private Sub EnregistrerCumulsCPU(numBL As String)
        Try
            If numBL = "" Then Exit Sub
            Dim noms() As String = {"TR1", "TR2", "TR3", "TR4", "CIM", "EAU", "ADJ1", "ADJ2", "ADJ3", "ADJ4"}
            If Not _colonnesCumulsOK Then
                Dim sbAlter As New System.Text.StringBuilder()
                For Each n In noms
                    sbAlter.Append("IF COL_LENGTH('dbo.BonsLivraison','CpuReel" & n & "') IS NULL ALTER TABLE dbo.BonsLivraison ADD CpuReel" & n & " FLOAT NULL; ")
                    sbAlter.Append("IF COL_LENGTH('dbo.BonsLivraison','CpuTheo" & n & "') IS NULL ALTER TABLE dbo.BonsLivraison ADD CpuTheo" & n & " FLOAT NULL; ")
                Next
                ModuleDb.Executer(sbAlter.ToString())
                _colonnesCumulsOK = True
            End If
            Dim reel() As Single = {PLCData.Reel_G1, PLCData.Reel_SC, PLCData.Reel_SL, PLCData.Reel_D2, PLCData.Reel_CIM,
                                    PLCData.Reel_EAU, PLCData.Reel_ADJ1, PLCData.Reel_ADJ2, PLCData.Reel_ADJ3, PLCData.Reel_ADJ4}
            Dim theo() As Single = {PLCData.Th_G1, PLCData.Th_SC, PLCData.Th_SL, PLCData.Th_D2, PLCData.Th_CIM,
                                    PLCData.Th_EAU, PLCData.Th_ADJ1, PLCData.Th_ADJ2, PLCData.Th_ADJ3, PLCData.Th_ADJ4}
            Dim sb As New System.Text.StringBuilder("UPDATE dbo.BonsLivraison SET ")
            For i As Integer = 0 To 9
                If i > 0 Then sb.Append(", ")
                sb.Append("CpuReel" & noms(i) & "=@r" & i & ", CpuTheo" & noms(i) & "=@t" & i)
            Next
            sb.Append(" WHERE NumeroBL=@bl")
            ModuleDb.Executer(sb.ToString(),
                Sub(cmd)
                    cmd.Parameters.AddWithValue("@bl", numBL)
                    For i As Integer = 0 To 9
                        cmd.Parameters.AddWithValue("@r" & i, CDbl(reel(i)))
                        cmd.Parameters.AddWithValue("@t" & i, CDbl(theo(i)))
                    Next
                End Sub)
            Synoptique.EnregistrerAction("GACHEE", "Cumuls CPU enregistrés",
                String.Format("BL={0} réel TR={1:0}/{2:0}/{3:0}/{4:0} CIM={5:0} EAU={6:0} | theo TR={7:0}/{8:0}/{9:0}/{10:0} CIM={11:0} EAU={12:0}",
                    numBL, reel(0), reel(1), reel(2), reel(3), reel(4), reel(5), theo(0), theo(1), theo(2), theo(3), theo(4), theo(5)),
                "DB1.DBD924/1008", "", "CLIC")
        Catch ex As Exception
            Synoptique.EnregistrerAction("ALARME", "Cumuls CPU non enregistrés", ex.Message, "DB1", "", "ALARME")
        End Try
    End Sub

    ' ── Attente de gâchée COMPLÈTE (le CPU écrit DB71 en deux temps :
    '    granulats + compteur d'abord, ciment / eau / adjuvants après) ──
    Private Const STABILITE_MIN_S As Integer = 5      ' contenu identique sur 2 lectures espacées d'au moins 5 s
    Private Const ATTENTE_MAX_S As Integer = 90       ' au-delà : archivée même sans ciment/eau (gâchée sans liant, test)
    Private Class AttenteGachee
        Public PremierVu As Date
        Public DernierVu As Date
        Public Empreinte As String
    End Class
    Private _enAttente As New Dictionary(Of Integer, AttenteGachee)

    Private Function Empreinte(g As ModuleGacheesDB71.GacheeCPU) As String
        Return String.Join("|", g.Matricule, g.PoseTR1, g.PoseTR2, g.PoseTR3, g.PoseTR4, g.PoseS1, g.PoseEAU,
                           g.PoseADJ1, g.PoseADJ2, g.PoseADJ3, g.PoseADJ4,
                           g.EcartTR1, g.EcartTR2, g.EcartTR3, g.EcartTR4, g.EcartS1, g.EcartEAU,
                           g.EcartADJ1, g.EcartADJ2, g.EcartADJ3, g.EcartADJ4)
    End Function

    ' True si la gâchée peut être archivée maintenant
    Private Function GacheeComplete(g As ModuleGacheesDB71.GacheeCPU, forcer As Boolean) As Boolean
        If forcer Then Return True
        Dim emp As String = Empreinte(g)
        Dim a As AttenteGachee = Nothing
        If Not _enAttente.TryGetValue(g.Emplacement, a) OrElse a.Empreinte <> emp Then
            _enAttente(g.Emplacement) = New AttenteGachee With {.PremierVu = Now, .DernierVu = Now, .Empreinte = emp}
            Return False
        End If
        Dim stable As Boolean = (Now - a.DernierVu).TotalSeconds >= STABILITE_MIN_S
        If Not stable Then Return False
        Dim complete As Boolean = g.PoseS1 > 0.5F AndAlso g.PoseEAU > 0.5F
        If complete OrElse (Now - a.PremierVu).TotalSeconds >= ATTENTE_MAX_S Then Return True
        Return False
    End Function

    Private Sub RattraperDB71(Optional forcer As Boolean = False)
        Try
            If Not PLCData.Connecte Then Exit Sub
            AssurerColonneMatricule()
            ChargerDernierMatricule()
            If _dernierMatricule < 0 Then Exit Sub

            ModuleGacheesDB71.DerniereErreur = ""
            Dim nouvelles As List(Of ModuleGacheesDB71.GacheeCPU)

            If _snapDB71 Is Nothing Then
                ' ── DÉMARRAGE : rattrapage des gâchées manquées, seulement si le
                '    compteur CPU n'a pas été remis à zéro (sinon on ne peut pas savoir).
                Dim mx As Long = ModuleGacheesDB71.MatriculeMaxCPU()
                If mx > _dernierCompteurCPU AndAlso _dernierCompteurCPU > 0 Then
                    nouvelles = ModuleGacheesDB71.LireNouvelles(_dernierCompteurCPU)
                Else
                    nouvelles = New List(Of ModuleGacheesDB71.GacheeCPU)
                    If mx > 0 AndAlso mx < _dernierCompteurCPU Then
                        Synoptique.EnregistrerAction("GACHEE", "Compteur CPU remis a zero (" & mx & " < " & _dernierCompteurCPU & ")",
                            "Le matricule logiciel continue a " & (_dernierMatricule + 1), "DB71", mx.ToString(), "CLIC")
                    End If
                End If
                Dim snap() As Long = ModuleGacheesDB71.LireMatricules()
                If snap Is Nothing Then Exit Sub
                For Each g In nouvelles
                    If ArchiverGacheeDB71(g) Then
                        _dernierCompteurCPU = g.Matricule
                    Else
                        Exit For
                    End If
                Next
                _snapDB71 = snap
                Exit Sub
            End If

            ' ── FONCTIONNEMENT NORMAL : tout emplacement dont le compteur a changé = nouvelle gâchée
            nouvelles = ModuleGacheesDB71.LireChangements(_snapDB71)

            ' Lecture DB71 en échec → visible dans le journal (une fois par message)
            If ModuleGacheesDB71.DerniereErreur <> "" AndAlso ModuleGacheesDB71.DerniereErreur <> _derniereErreurDB71Signalee Then
                _derniereErreurDB71Signalee = ModuleGacheesDB71.DerniereErreur
                Synoptique.EnregistrerAction("ALARME", "Lecture DB71 impossible",
                    ModuleGacheesDB71.DerniereErreur, "DB71", _dernierMatricule.ToString(), "ALARME")
            End If

            For Each g In nouvelles
                ' Attendre que la ligne DB71 soit complète (ciment / eau / adjuvants
                ' écrits par le CPU après les granulats) et stable.
                If Not GacheeComplete(g, forcer) Then Continue For
                ' On ne mémorise l'emplacement QUE si l'enregistrement SQL a réussi
                ' (sinon la gâchée serait perdue sans rien dire).
                If ArchiverGacheeDB71(g) Then
                    _snapDB71(g.Emplacement - 1) = g.Matricule
                    _dernierCompteurCPU = g.Matricule
                    _enAttente.Remove(g.Emplacement)
                Else
                    Exit For
                End If
            Next
        Catch ex As Exception
            If ex.Message <> _derniereErreurDB71Signalee Then
                _derniereErreurDB71Signalee = ex.Message
                Synoptique.EnregistrerAction("ALARME", "Archivage DB71 : erreur", ex.Message, "DB71", "", "ALARME")
            End If
        End Try
    End Sub

    ' ════════════════════════════════════════════════════════════
    '  ARCHIVER UNE GACHEE LUE DANS DB71 (poses + écarts du CPU,
    '  consignes courantes, matricule CPU)
    ' ════════════════════════════════════════════════════════════
    ' Consigne d'un composant déduite de la pose et de l'écart % du CPU.
    ' Secours (pose nulle ou écart aberrant) : consigne DB1 courante.
    Private Function ConsigneDepuisCPU(pose As Single, ecartPct As Single, secours As Single) As Single
        If pose <= 0 OrElse Single.IsNaN(ecartPct) OrElse ecartPct <= -99.9 Then Return secours
        Dim c As Double = pose / (1.0 + ecartPct / 100.0)
        If c <= 0 OrElse Double.IsInfinity(c) Then Return secours
        Return CSng(Math.Round(c, 2))
    End Function

    Private Function ArchiverGacheeDB71(g As ModuleGacheesDB71.GacheeCPU) As Boolean
        Try
            ' Matricule LOGICIEL : dernier + 1, jamais remis à zéro (g.Matricule = compteur CPU)
            Dim matricule As Long = _dernierMatricule + 1
            If _actif Then _nGachee += 1
            Dim numGachee As Integer = If(_actif, _nGachee, 0)
            Dim numBL As String = _numeroBL

            ' Consignes : RECALCULÉES depuis le CPU (consigne = pose / (1 + écart% / 100)).
            ' Les consignes DB1 courantes sont déjà celles de la gâchée SUIVANTE au
            ' moment de l'archivage (décalage d'une gâchée constaté le 16/09) ;
            ' elles ne servent plus que de secours quand la pose CPU est nulle.
            Dim consG1 As Single = ConsigneDepuisCPU(g.PoseTR1, g.EcartTR1, PLCData.Cons_G1)
            Dim consSC As Single = ConsigneDepuisCPU(g.PoseTR2, g.EcartTR2, PLCData.Cons_SC)
            Dim consSL As Single = ConsigneDepuisCPU(g.PoseTR3, g.EcartTR3, PLCData.Cons_SL)
            Dim consD2 As Single = ConsigneDepuisCPU(g.PoseTR4, g.EcartTR4, PLCData.Cons_D2)
            Dim consCIM As Single = ConsigneDepuisCPU(g.PoseS1, g.EcartS1, PLCData.Cons_CIM)
            Dim consEAU As Single = ConsigneDepuisCPU(g.PoseEAU, g.EcartEAU, PLCData.Cons_EAU2)
            Dim consADJ1 As Single = ConsigneDepuisCPU(g.PoseADJ1, g.EcartADJ1, PLCData.Cons_ADJ1)
            Dim consADJ2 As Single = ConsigneDepuisCPU(g.PoseADJ2, g.EcartADJ2, PLCData.Cons_ADJ2)
            Dim consADJ3 As Single = ConsigneDepuisCPU(g.PoseADJ3, g.EcartADJ3, PLCData.Cons_ADJ3)
            Dim consADJ4 As Single = ConsigneDepuisCPU(g.PoseADJ4, g.EcartADJ4, PLCData.Cons_ADJ4)

            Dim totalConsGran As Single = consG1 + consSC + consSL + consD2
            Dim totalPoseGran As Single = g.PoseTR1 + g.PoseTR2 + g.PoseTR3 + g.PoseTR4
            Dim totalEcartGran As Single = g.EcartTR1 + g.EcartTR2 + g.EcartTR3 + g.EcartTR4

            If _actif Then
                _totalConsAGR += CDbl(totalConsGran) : _totalPoseAGR += CDbl(totalPoseGran) : _totalEcartAGR += CDbl(totalEcartGran)
                _totalConsCIM += CDbl(consCIM) : _totalPoseCIM += CDbl(g.PoseS1) : _totalEcartCIM += CDbl(g.EcartS1)
                _totalConsEAU += CDbl(consEAU) : _totalPoseEAU += CDbl(g.PoseEAU) : _totalEcartEAU += CDbl(g.EcartEAU)
                _totalConsADJ += CDbl(consADJ1 + consADJ2 + consADJ3 + consADJ4)
                _totalPoseADJ += CDbl(g.PoseADJ1 + g.PoseADJ2 + g.PoseADJ3 + g.PoseADJ4)
                _totalEcartADJ += CDbl(g.EcartADJ1 + g.EcartADJ2 + g.EcartADJ3 + g.EcartADJ4)
            End If

            ModuleDb.Executer(
                "INSERT INTO dbo.GacheesDetails (" &
                "  DateHeure, NumeroBL, NumeroGachee, MatriculeGachee, CompteurCPU, " &
                "  ConsTr1, PoseTr1, EcartTr1, ConsTr2, PoseTr2, EcartTr2, " &
                "  ConsTr3, PoseTr3, EcartTr3, ConsTr4, PoseTr4, EcartTr4, " &
                "  ConsS1, PoseS1, ConsS2, PoseS2, EcartCIM, " &
                "  ConsEAU, PoseEAU, EcartEAU, " &
                "  ConsADJ1, PoseADJ1, EcartADJ1, ConsADJ2, PoseADJ2, EcartADJ2, " &
                "  ConsADJ3, PoseADJ3, EcartADJ3, ConsADJ4, PoseADJ4, EcartADJ4, " &
                "  TotalConsTr, TotalPoseTr, TotalEcartTr, Operateur, AjoutEau) " &
                "VALUES (GETDATE(), @bl, @ng, @mat, @cpu, " &
                "  @ct1, @pt1, @et1, @ct2, @pt2, @et2, @ct3, @pt3, @et3, @ct4, @pt4, @et4, " &
                "  @cs1, @ps1, @cs2, @ps2, @ecim, @ceau, @peau, @eeau, " &
                "  @ca1, @pa1, @ea1, @ca2, @pa2, @ea2, @ca3, @pa3, @ea3, @ca4, @pa4, @ea4, " &
                "  @tcg, @tpg, @teg, @op, @ajeau)",
                Sub(cmd)
                    cmd.Parameters.AddWithValue("@bl", numBL)
                    cmd.Parameters.AddWithValue("@ng", numGachee)
                    cmd.Parameters.AddWithValue("@mat", matricule)
                    cmd.Parameters.AddWithValue("@cpu", g.Matricule)
                    cmd.Parameters.AddWithValue("@ajeau", CDbl(g.AjoutEau))
                    cmd.Parameters.AddWithValue("@ct1", CDbl(consG1)) : cmd.Parameters.AddWithValue("@pt1", CDbl(g.PoseTR1)) : cmd.Parameters.AddWithValue("@et1", CDbl(g.EcartTR1))
                    cmd.Parameters.AddWithValue("@ct2", CDbl(consSC)) : cmd.Parameters.AddWithValue("@pt2", CDbl(g.PoseTR2)) : cmd.Parameters.AddWithValue("@et2", CDbl(g.EcartTR2))
                    cmd.Parameters.AddWithValue("@ct3", CDbl(consSL)) : cmd.Parameters.AddWithValue("@pt3", CDbl(g.PoseTR3)) : cmd.Parameters.AddWithValue("@et3", CDbl(g.EcartTR3))
                    cmd.Parameters.AddWithValue("@ct4", CDbl(consD2)) : cmd.Parameters.AddWithValue("@pt4", CDbl(g.PoseTR4)) : cmd.Parameters.AddWithValue("@et4", CDbl(g.EcartTR4))
                    cmd.Parameters.AddWithValue("@cs1", CDbl(consCIM)) : cmd.Parameters.AddWithValue("@ps1", CDbl(g.PoseS1))
                    cmd.Parameters.AddWithValue("@cs2", 0.0) : cmd.Parameters.AddWithValue("@ps2", 0.0) : cmd.Parameters.AddWithValue("@ecim", CDbl(g.EcartS1))
                    cmd.Parameters.AddWithValue("@ceau", CDbl(consEAU)) : cmd.Parameters.AddWithValue("@peau", CDbl(g.PoseEAU)) : cmd.Parameters.AddWithValue("@eeau", CDbl(g.EcartEAU))
                    cmd.Parameters.AddWithValue("@ca1", CDbl(consADJ1)) : cmd.Parameters.AddWithValue("@pa1", CDbl(g.PoseADJ1)) : cmd.Parameters.AddWithValue("@ea1", CDbl(g.EcartADJ1))
                    cmd.Parameters.AddWithValue("@ca2", CDbl(consADJ2)) : cmd.Parameters.AddWithValue("@pa2", CDbl(g.PoseADJ2)) : cmd.Parameters.AddWithValue("@ea2", CDbl(g.EcartADJ2))
                    cmd.Parameters.AddWithValue("@ca3", CDbl(consADJ3)) : cmd.Parameters.AddWithValue("@pa3", CDbl(g.PoseADJ3)) : cmd.Parameters.AddWithValue("@ea3", CDbl(g.EcartADJ3))
                    cmd.Parameters.AddWithValue("@ca4", CDbl(consADJ4)) : cmd.Parameters.AddWithValue("@pa4", CDbl(g.PoseADJ4)) : cmd.Parameters.AddWithValue("@ea4", CDbl(g.EcartADJ4))
                    cmd.Parameters.AddWithValue("@tcg", CDbl(totalConsGran))
                    cmd.Parameters.AddWithValue("@tpg", CDbl(totalPoseGran))
                    cmd.Parameters.AddWithValue("@teg", CDbl(totalEcartGran))
                    cmd.Parameters.AddWithValue("@op", NomUtilisateur)
                End Sub)

            DecrementerStocks(g.PoseTR1, g.PoseTR2, g.PoseTR3, g.PoseTR4,
                              g.PoseS1, 0, g.PoseEAU,
                              g.PoseADJ1, g.PoseADJ2, g.PoseADJ3, g.PoseADJ4)

            _dernierMatricule = matricule
            Synoptique.EnregistrerAction("GACHEE", "Gâchée matricule " & matricule & " archivée (CPU " & g.Matricule & ")",
                String.Format("BL={0} G{1} empl.{2} | TR={3:0} CIM={4:0} EAU={5:0} | EcarCIM={6:0.0}",
                    numBL, numGachee, g.Emplacement, totalPoseGran, g.PoseS1, g.PoseEAU, g.EcartS1),
                "DB71", matricule.ToString(), "CLIC")

            Return True
        Catch ex As Exception
            Debug.WriteLine("ArchiverGacheeDB71 : " & ex.Message)
            Synoptique.EnregistrerAction("ALARME", "Gâchée CPU " & g.Matricule & " NON archivée",
                ex.Message, "DB71", g.Matricule.ToString(), "ALARME")
            Return False
        End Try
    End Function

    ' ================================================================
    '  PATCH ModuleArchivageGachees — DÉCRÉMENTATION DES STOCKS
    '  ----------------------------------------------------------------
    '  À chaque fin de gâchée, les quantités RÉELLEMENT dosées
    '  (lues du CPU) sont déjà archivées dans GacheesDetails.
    '  Ce patch ajoute la SOUSTRACTION AUTOMATIQUE des stocks :
    '
    '   • Materiaux.Stock  ← décrémenté pour CHAQUE élément
    '     (trémies, silos, eau, adjuvants) via la configuration
    '     (Cases/Silos/Bascules.IdMateriau), avec conversion
    '     d'unité (kg dosé → T stocké si UniteStockage='T',
    '     L dosé → m3 si 'm3')
    '
    '   • T_STOCK          ← STO_Stock décrémenté et
    '     STO_Somme_Sorties incrémenté (si la fiche stock existe,
    '     liaison par MAT_Code = CodeMateriau) — reste cohérent
    '     avec vos formulaires Entrées/Sorties MP
    '
    '   • Silos (CIMENT)   ← NiveauActuel (%) recalculé d'après
    '     les kg dosés PAR SILO (poseS1 / poseS2 lus séparément
    '     du CPU), avec trace dans HistoriqueNiveaux
    '
    '  INSTALLATION (2 étapes) dans ModuleArchivageGachees.vb :
    '
    '   ÉTAPE 1 — Dans ArchiverGachee, juste APRÈS le gros INSERT
    '   GacheesDetails (avant Synoptique.EnregistrerAction),
    '   ajouter cette ligne :
    '
    '       DecrementerStocks(poseG1, poseSC, poseSL, poseD2,
    '                         poseS1, poseS2, poseEAU,
    '                         poseADJ1, poseADJ2, poseADJ3, poseADJ4)
    '
    '   ÉTAPE 2 — Copier les deux méthodes ci-dessous dans la
    '   classe ModuleArchivageGachees (par ex. après ArchiverGachee).
    ' ================================================================


    ' ═══════════════════════════════════════════════════════════════
    '  DÉCRÉMENTATION DES STOCKS — appelée à chaque gâchée
    '  Toutes les quantités sont les POSES RÉELLES (kg / L)
    ' ═══════════════════════════════════════════════════════════════
    Private Sub DecrementerStocks(pTr1 As Single, pTr2 As Single,
                                  pTr3 As Single, pTr4 As Single,
                                  pS1 As Single, pS2 As Single,
                                  pEau As Single,
                                  pA1 As Single, pA2 As Single,
                                  pA3 As Single, pA4 As Single)
        Try
            ' ── 1. TRÉMIES : matériau affecté à chaque case ──────
            Dim posesTr() As Single = {pTr1, pTr2, pTr3, pTr4}
            For n As Integer = 1 To 4
                If posesTr(n - 1) > 0.01F Then
                    Dim idMat As Integer = ModuleDb.LireValeur(Of Integer)(
                        "SELECT ISNULL(IdMateriau, 0) FROM dbo.Cases WHERE NumeroCase = @n",
                        Sub(cmd) cmd.Parameters.AddWithValue("@n", CShort(n)))
                    If idMat > 0 Then SortieStock(idMat, CDbl(posesTr(n - 1)))
                End If
            Next

            ' ── 2. SILOS CIMENT : soustraction PAR SILO ──────────
            '     (poseS1 / poseS2 lus séparément du CPU)
            Dim posesSilo() As Single = {pS1, pS2}
            Dim dtSilos As DataTable = ModuleDb.Lire(
                "SELECT SiloID, ISNULL(IdMateriau,0) AS IdMateriau, " &
                "       ISNULL(CapaciteMax_kg,0) AS Cap " &
                "FROM dbo.Silos WHERE EstActif = 1 AND TypeElement = 'S' " &
                "ORDER BY NumeroElement, SiloID")   ' 26/09 : dbo.Silos = table unique (TR/S/ADJ/EAU)

            For i As Integer = 0 To Math.Min(1, dtSilos.Rows.Count - 1)
                Dim kg As Double = CDbl(posesSilo(i))
                If kg <= 0.01 Then Continue For

                Dim siloID As Integer = CInt(dtSilos.Rows(i)("SiloID"))
                Dim idMat As Integer = CInt(dtSilos.Rows(i)("IdMateriau"))
                Dim cap As Double = CDbl(dtSilos.Rows(i)("Cap"))

                ' a) Stock du matériau (Materiaux + T_STOCK)
                If idMat > 0 Then SortieStock(idMat, kg)

                ' 26/09 : stock du silo en kg (indépendant du matériau)
                SortieStockSilo(siloID, kg)

                ' b) Niveau du silo (%) + trace HistoriqueNiveaux
                If cap > 0 Then
                    ModuleDb.Executer(
                        "UPDATE dbo.Silos SET " &
                        "  NiveauActuel = CASE WHEN ISNULL(NiveauActuel,0) - (@kg / CapaciteMax_kg * 100) < 0 " &
                        "                 THEN 0 ELSE ISNULL(NiveauActuel,0) - (@kg / CapaciteMax_kg * 100) END, " &
                        "  DateModif = GETDATE() " &
                        "WHERE SiloID = @id; " &
                        "INSERT INTO dbo.HistoriqueNiveaux (SiloID, NiveauPct, NiveauKg, DateMesure) " &
                        "SELECT SiloID, NiveauActuel, CapaciteMax_kg * NiveauActuel / 100, GETDATE() " &
                        "FROM dbo.Silos WHERE SiloID = @id;",
                        Sub(cmd)
                            cmd.Parameters.AddWithValue("@kg", kg)
                            cmd.Parameters.AddWithValue("@id", siloID)
                        End Sub)
                End If
            Next

            ' ── 3. EAU : bascule type EAU ────────────────────────
            If pEau > 0.01F Then
                Dim idMatEau As Integer = ModuleDb.LireValeur(Of Integer)(
                    "SELECT TOP 1 ISNULL(b.IdMateriau, 0) FROM dbo.Bascules b " &
                    "JOIN dbo.TypesBascule t ON b.TypeID = t.TypeID " &
                    "WHERE t.NomType LIKE '%EAU%' AND b.Actif = 1")
                If idMatEau > 0 Then SortieStock(idMatEau, CDbl(pEau))
            End If

            ' ── 4. ADJUVANTS : bascules par ordre ────────────────
            Dim posesAdj() As Single = {pA1, pA2, pA3, pA4}
            Dim dtAdj As DataTable = ModuleDb.Lire(
                "SELECT ISNULL(b.IdMateriau,0) AS IdMateriau, " &
                "       ROW_NUMBER() OVER (ORDER BY b.Ordre, b.BasculeID) AS Rang " &
                "FROM dbo.Bascules b " &
                "JOIN dbo.TypesBascule t ON b.TypeID = t.TypeID " &
                "WHERE t.NomType LIKE '%ADJUVANT%'")
            For Each row As DataRow In dtAdj.Rows
                Dim rang As Integer = CInt(row("Rang"))
                If rang >= 1 AndAlso rang <= 4 AndAlso posesAdj(rang - 1) > 0.001F Then
                    Dim idMat As Integer = CInt(row("IdMateriau"))
                    If idMat > 0 Then SortieStock(idMat, CDbl(posesAdj(rang - 1)))
                End If
            Next

        Catch ex As Exception
            ' Ne jamais bloquer le cycle de production pour un souci de stock
            Debug.WriteLine("DecrementerStocks : " & ex.Message)
            Synoptique.EnregistrerAction("STOCK", "Erreur décrément stock",
                ex.Message, "", "", "AUTO")
        End Try
    End Sub

    ' ═══════════════════════════════════════════════════════════════
    '  SORTIE DE STOCK pour un matériau
    '  qteDosage = quantité en unité de DOSAGE (kg ou L)
    '  Conversion automatique vers l'unité de STOCKAGE :
    '    kg → T   (/1000) si UniteStockage = 'T'
    '    L  → m3  (/1000) si UniteStockage = 'm3'
    ' ═══════════════════════════════════════════════════════════════
    ' ═══════════════════════════════════════════════════════════════
    '  26/09 : STOCK PAR SILO — Silos.StockKg (kg). Entrée = kg positif, sortie = négatif.
    ' ═══════════════════════════════════════════════════════════════
    Public Sub SortieStockSilo(siloID As Integer, kg As Double)
        MouvementStockSilo(siloID, -kg)
    End Sub

    Public Sub MouvementStockSilo(siloID As Integer, deltaKg As Double)
        Try
            ModuleDb.Executer(
                "UPDATE dbo.Silos SET StockKg = ISNULL(StockKg,0) + @d, DateModif = GETDATE() WHERE SiloID = @id",
                Sub(cmd)
                    cmd.Parameters.AddWithValue("@d", deltaKg)
                    cmd.Parameters.AddWithValue("@id", siloID)
                End Sub)
        Catch ex As Exception
            Debug.WriteLine("MouvementStockSilo : " & ex.Message)
        End Try
    End Sub

    ' SiloID du silo ciment n (TypeElement 'S', NumeroElement n) — 0 si absent
    Public Shared Function SiloIdCiment(num As Integer) As Integer
        Try
            Return ModuleDb.LireValeur(Of Integer)(
                "SELECT TOP 1 SiloID FROM dbo.Silos WHERE EstActif = 1 AND TypeElement = 'S' AND NumeroElement = @n",
                Sub(cmd) cmd.Parameters.AddWithValue("@n", num))
        Catch
            Return 0
        End Try
    End Function

    Public Sub SortieStock(idMateriau As Integer, qteDosage As Double)   ' 26/09 : Public (ModuleStockManuel)
        ' 1) Materiaux.Stock (avec conversion d'unité en SQL)
        ModuleDb.Executer(
            "UPDATE dbo.Materiaux SET " &
            "  Stock = ISNULL(Stock, 0) - " &
            "    CASE WHEN UPPER(ISNULL(UniteStockage,'')) IN ('T','M3') " &
            "         THEN @q / 1000.0 ELSE @q END, " &
            "  DerniereModification = GETDATE() " &
            "WHERE IdMateriau = @id AND ISNULL(GestionStocks, 1) = 1",
            Sub(cmd)
                cmd.Parameters.AddWithValue("@q", qteDosage)
                cmd.Parameters.AddWithValue("@id", idMateriau)
            End Sub)

        ' 2) T_STOCK si la fiche existe (cohérence avec Entrées/Sorties MP)
        ModuleDb.Executer(
            "UPDATE s SET " &
            "  s.STO_Stock = s.STO_Stock - " &
            "    CASE WHEN UPPER(s.STO_Unite) IN ('T','M3') THEN @q / 1000.0 ELSE @q END, " &
            "  s.STO_Somme_Sorties = s.STO_Somme_Sorties + " &
            "    CASE WHEN UPPER(s.STO_Unite) IN ('T','M3') THEN @q / 1000.0 ELSE @q END, " &
            "  s.STO_DateModification = GETDATE() " &
            "FROM dbo.T_STOCK s " &
            "JOIN dbo.Materiaux m ON s.MAT_Code = m.CodeMateriau " &
            "WHERE m.IdMateriau = @id",
            Sub(cmd)
                cmd.Parameters.AddWithValue("@q", qteDosage)
                cmd.Parameters.AddWithValue("@id", idMateriau)
            End Sub)
    End Sub



    ' ════════════════════════════════════════════════════════════
    '  CLOTURER LA COMMANDE
    ' ════════════════════════════════════════════════════════════
    Private Sub CloturerCommande()
        Try
            ' ── Lire totaux depuis CPU (DB101=theorique, DB65=reel, DB114=ecart%) ──
            ' ── Theorique camion DB101 (mapping exact) ───────────
            ' Tr1=DBD80  Tr2=DBD84  Tr3=DBD88  Tr4=DBD92
            ' Silo1=DBD96  Silo2=DBD100  EAU=DBD104
            ' ADJ1=DBD108  ADJ2=DBD112  ADJ3=DBD116
            Dim thTr1 As Single = LireDBD(1, 1008)
            Dim thTr2 As Single = LireDBD(1, 1012)
            Dim thTr3 As Single = LireDBD(1, 1016)
            Dim thTr4 As Single = LireDBD(1, 1020)
            Dim thS1 As Single = LireDBD(1, 1024)   ' Silo1
            Dim thS2 As Single = LireDBD(1, 1028)   ' Silo2
            Dim thEAU As Single = LireDBD(1, 1032)
            Dim thADJ1 As Single = LireDBD(1, 1036)
            Dim thADJ2 As Single = LireDBD(1, 1040)
            Dim thADJ3 As Single = LireDBD(1, 1044)

            ' ── Reel camion DB65 (mapping exact) ─────────────────
            ' Tr1=DBD0  Tr2=DBD8  Tr3=DBD16  Tr4=DBD24
            ' Silo1=DBD32  Silo2=DBD48  EAU=DBD56
            ' ADJ1=DBD64  ADJ2=DBD80  ADJ3=DBD88
            Dim rlTr1 As Single = LireDBD(1, 924)
            Dim rlTr2 As Single = LireDBD(1, 928)
            Dim rlTr3 As Single = LireDBD(1, 932)
            Dim rlTr4 As Single = LireDBD(1, 936)
            Dim rlS1 As Single = LireDBD(1, 940)   ' Silo1
            Dim rlS2 As Single = LireDBD(1, 948)   ' Silo2
            Dim rlEAU As Single = LireDBD(1, 952)
            Dim rlADJ1 As Single = LireDBD(1, 956)
            Dim rlADJ2 As Single = LireDBD(1, 960)
            Dim rlADJ3 As Single = LireDBD(1, 964)

            ' ── Calcul totaux ────────────────────────────────────
            Dim consAGR As Double = CDbl(thTr1 + thTr2 + thTr3 + thTr4)
            Dim poseAGR As Double = CDbl(rlTr1 + rlTr2 + rlTr3 + rlTr4)
            Dim consCIM As Double = CDbl(thS1 + thS2)
            Dim poseCIM As Double = CDbl(rlS1 + rlS2)
            Dim consEAU As Double = CDbl(thEAU)
            Dim poseEAU As Double = CDbl(rlEAU)
            Dim consADJ As Double = CDbl(thADJ1 + thADJ2 + thADJ3)
            Dim poseADJ As Double = CDbl(rlADJ1 + rlADJ2 + rlADJ3)

            ' ── Lire temps malaxage depuis DB19 ──────────────────
            Dim tempsMlx As Single = LireDBD(19, 0)  ' A ajuster selon adresse reelle

            ' ── INSERT Production complet ─────────────────────────
            ModuleDb.Executer(
                "INSERT INTO dbo.Production " &
                "(DateHeure, Operateur, NumeroBL, NombreGachees, " &
                " ConsTr1, PoseTr1, EcartTr1, " &
                " ConsTr2, PoseTr2, EcartTr2, " &
                " ConsTr3, PoseTr3, EcartTr3, " &
                " ConsTr4, PoseTr4, EcartTr4, " &
                " ConsS1,  PoseS1,  EcartS1, " &
                " ConsS2,  PoseS2,  EcartS2, " &
                " ConsEAU, PoseEAU, EcartEAU, " &
                " ConsADJ1, PoseADJ1, EcartADJ1, " &
                " ConsADJ2, PoseADJ2, EcartADJ2, " &
                " ConsADJ3, PoseADJ3, EcartADJ3, " &
                " TotalGranCons, TotalGranPose, TotalGranEcart, " &
                " TotalCamionCons, TotalCamionPose, TempsMalaxage) " &
                "VALUES (GETDATE(), @op, @bl, @ng, " &
                " @ct1, @pt1, @et1, " &
                " @ct2, @pt2, @et2, " &
                " @ct3, @pt3, @et3, " &
                " @ct4, @pt4, @et4, " &
                " @cs1, @ps1, @es1, " &
                " @cs2, @ps2, @es2, " &
                " @ceau, @peau, @eeau, " &
                " @ca1, @pa1, @ea1, " &
                " @ca2, @pa2, @ea2, " &
                " @ca3, @pa3, @ea3, " &
                " @tgc, @tgp, @tge, " &
                " @tcc, @tcp, @tmx)",
                Sub(cmd)
                    cmd.Parameters.AddWithValue("@op", NomUtilisateur)
                    cmd.Parameters.AddWithValue("@bl", _numeroBL)
                    cmd.Parameters.AddWithValue("@ng", _nGachee)
                    ' Tremies
                    cmd.Parameters.AddWithValue("@ct1", CDbl(thTr1)) : cmd.Parameters.AddWithValue("@pt1", CDbl(rlTr1)) : cmd.Parameters.AddWithValue("@et1", CDbl(rlTr1 - thTr1))
                    cmd.Parameters.AddWithValue("@ct2", CDbl(thTr2)) : cmd.Parameters.AddWithValue("@pt2", CDbl(rlTr2)) : cmd.Parameters.AddWithValue("@et2", CDbl(rlTr2 - thTr2))
                    cmd.Parameters.AddWithValue("@ct3", CDbl(thTr3)) : cmd.Parameters.AddWithValue("@pt3", CDbl(rlTr3)) : cmd.Parameters.AddWithValue("@et3", CDbl(rlTr3 - thTr3))
                    cmd.Parameters.AddWithValue("@ct4", CDbl(thTr4)) : cmd.Parameters.AddWithValue("@pt4", CDbl(rlTr4)) : cmd.Parameters.AddWithValue("@et4", CDbl(rlTr4 - thTr4))
                    ' Silos
                    cmd.Parameters.AddWithValue("@cs1", CDbl(thS1)) : cmd.Parameters.AddWithValue("@ps1", CDbl(rlS1)) : cmd.Parameters.AddWithValue("@es1", CDbl(rlS1 - thS1))
                    cmd.Parameters.AddWithValue("@cs2", CDbl(thS2)) : cmd.Parameters.AddWithValue("@ps2", CDbl(rlS2)) : cmd.Parameters.AddWithValue("@es2", CDbl(rlS2 - thS2))
                    ' Eau
                    cmd.Parameters.AddWithValue("@ceau", CDbl(thEAU)) : cmd.Parameters.AddWithValue("@peau", CDbl(rlEAU)) : cmd.Parameters.AddWithValue("@eeau", CDbl(rlEAU - thEAU))
                    ' Adjuvants
                    cmd.Parameters.AddWithValue("@ca1", CDbl(thADJ1)) : cmd.Parameters.AddWithValue("@pa1", CDbl(rlADJ1)) : cmd.Parameters.AddWithValue("@ea1", CDbl(rlADJ1 - thADJ1))
                    cmd.Parameters.AddWithValue("@ca2", CDbl(thADJ2)) : cmd.Parameters.AddWithValue("@pa2", CDbl(rlADJ2)) : cmd.Parameters.AddWithValue("@ea2", CDbl(rlADJ2 - thADJ2))
                    cmd.Parameters.AddWithValue("@ca3", CDbl(thADJ3)) : cmd.Parameters.AddWithValue("@pa3", CDbl(rlADJ3)) : cmd.Parameters.AddWithValue("@ea3", CDbl(rlADJ3 - thADJ3))
                    ' Totaux granulats
                    cmd.Parameters.AddWithValue("@tgc", consAGR)
                    cmd.Parameters.AddWithValue("@tgp", poseAGR)
                    cmd.Parameters.AddWithValue("@tge", poseAGR - consAGR)
                    ' Total camion
                    cmd.Parameters.AddWithValue("@tcc", consAGR + consCIM + consEAU + consADJ)
                    cmd.Parameters.AddWithValue("@tcp", poseAGR + poseCIM + poseEAU + poseADJ)
                    ' Temps malaxage
                    cmd.Parameters.AddWithValue("@tmx", CDbl(tempsMlx))
                End Sub)

            ' Mettre BL en LIVRE avec poids reels
            ModuleDb.Executer(
                "UPDATE dbo.BonsLivraison SET Statut='LIVRE', DateModification=GETDATE(), " &
                " PoidsGranulat=@gran, PoidsCiment=@cim, PoidsEau=@eau " &
                "WHERE NumeroBL=@bl",
                Sub(cmd)
                    cmd.Parameters.AddWithValue("@bl", _numeroBL)
                    cmd.Parameters.AddWithValue("@gran", poseAGR)
                    cmd.Parameters.AddWithValue("@cim", poseCIM)
                    cmd.Parameters.AddWithValue("@eau", poseEAU)
                End Sub)

            ' Mettre Planning en TERMINE + mettre a jour DejaLivres
            Dim volLivre As Double = CDbl(rlTr1 + rlTr2 + rlTr3 + rlTr4 + rlS1 + rlS2 + rlEAU) / 1000.0
            ModuleDb.Executer(
                "UPDATE dbo.Planning_journalier SET StatutPlanning='Terminé', DateModification=GETDATE(), " &
                " QuantiteLivree = ISNULL(QuantiteLivree,0) + @vol " &
                "WHERE IdPlanning=@Id",
                Sub(cmd)
                    cmd.Parameters.AddWithValue("@Id", _idPlanning)
                    cmd.Parameters.AddWithValue("@vol", volLivre)
                End Sub)
            ' Mettre à jour QteCumulee dans BonsLivraison = livraisons cumulées APRES ce BL
            Dim totalLivre As Decimal = CDec(ModuleDb.LireValeur(Of Object)(
    "SELECT ISNULL(SUM(VolumeM3), 0) " &
    "FROM dbo.BonsLivraison " &
    "WHERE IdPlanning = @id AND Statut = 'LIVRE'",
    Sub(cmd) cmd.Parameters.AddWithValue("@id", _idPlanning)))

            ModuleDb.Executer(
    "UPDATE dbo.BonsLivraison " &
    "SET QteCumulee = @cumul " &
    "WHERE NumeroBL = @bl",
    Sub(cmd)
        cmd.Parameters.AddWithValue("@cumul", totalLivre)
        cmd.Parameters.AddWithValue("@bl", _numeroBL)
    End Sub)



            Synoptique.EnregistrerAction("COMMANDE", "Commande terminée",
                "BL=" & _numeroBL & " Gâchées=" & _nGachee,
                ADR_FIN_COMMANDE, "1", "CLIC")

            MessageBox.Show("✅ Commande terminée !" & vbCrLf &
                "BL N° " & _numeroBL & vbCrLf &
                _nGachee & " gâchées archivées.",
                "Fin de commande", MessageBoxButtons.OK, MessageBoxIcon.Information)

        Catch ex As Exception
            Debug.WriteLine("CloturerCommande : " & ex.Message)
        End Try
    End Sub

    ' ── Helper lecture PLC ───────────────────────────────────────
    Private Function LireDBD(db As Integer, offset As Integer) As Single
        Try
            Return CSng(ModulePLCDuplex.LireDBDReal(db, offset))
        Catch
            Return 0.0F
        End Try
    End Function

    ' ── Arreter la surveillance ──────────────────────────────────
    Public Sub Arreter()
        _actif = False
    End Sub

    ' ── Methodes Shared compatibles avec Module1 ─────────────────
    Public Shared Sub Demarrer(plc As Plc)
        ' Appel depuis Module1 au demarrage — juste stocker la reference PLC
        ' Le cycle demarre seulement via DemarrerCycle() depuis Form_ConfirmationBL
        Instance._plc = plc
    End Sub

    Public Shared Sub Arreter_Shared()
        Instance.Arreter()
    End Sub

End Class