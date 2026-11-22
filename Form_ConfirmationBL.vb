' ================================================================
'  Form_ConfirmationBL.vb — VERSION CORRIGEE
'  MAJ 15/09/2026 — NOUVEL ADRESSAGE DB1 :
'   • EnvoyerVersCPU passe par ModulePLCDuplex.EcrireReal, qui traduit
'     DB101.DBD0→36 vers DB1.DBD538/542/546/550/530/586/594/598/602/606
'     et DB19.DBD58/66 vers DB1.DBD916/920.
'   • Plus de connexion S7 « locale » créée à la volée : on utilise
'     la connexion partagée Module1.PLCInstance (verrou commun).
'   • Méthode privée EcrireReal(plc, …) SUPPRIMÉE (contournait DB1).
' ================================================================

Imports System.Drawing
Imports System.Drawing.Printing
Imports System.Windows.Forms
Imports S7.Net
Imports Microsoft.Data.SqlClient

Public Class Form_ConfirmationBL
    ' TEST 18/09 : False = conditions CPU (914.1, mode auto, défaut, attente fin de cycle) désactivées
    Private Const CONDITIONS_CPU As Boolean = False

    Public Property IdPlanning As Integer = 0
    Public Property IdFormule As Integer = 0
    Public Property NomClient As String = ""
    Public Property NomChantier As String = ""
    Public Property CodeFormule As String = ""
    Public Property NomFormule As String = ""
    Public Property VolumeTotalM3 As Decimal = 0
    Public Property CapaciteMalaxeur As Decimal = 1
    Public Property CodeCamion As String = ""
    Public Property NomCamion As String = ""
    Public Property CapaciteCamion As Decimal = 0
    Public Property NomChauffeur As String = ""

    Private _numeroBL As String = ""
    Private _blConfirme As Boolean = False
    ' 22/09 : lu par Planning journalier pour se fermer aussi après l'envoi
    Public ReadOnly Property BLConfirme As Boolean
        Get
            Return _blConfirme
        End Get
    End Property
    Private WithEvents pdBL As New PrintDocument()
    Private WithEvents pdBL_Detail As New PrintDocument()

    ' Donnees impression
    Private _dtGachees As System.Data.DataTable = Nothing
    Private _codeFormule2 As String = ""
    Private _nomFormule2 As String = ""
    Private _designation As String = ""
    Private _classeResistance As String = ""
    Private _classeExposition As String = ""
    Private _classeConsistance As String = ""
    Private _classeChlorure As String = ""
    Private _dmax As String = ""
    Private _typeCiment As String = ""
    Private _typeAdjuvant As String = ""
    Private _massVolumique As String = ""

    Private lblCLIENT As New Label()
    Private lblCHANTIER As New Label()
    Private lblFORMULE As New Label()
    Private lblVOLUME As New Label()
    Private txtCODECAMION As New TextBox()
    Private txtNOMCAMION As New TextBox()
    Private txtCAPACITE As New TextBox()
    Private txtCHAUFFEUR As New TextBox()
    Private lblNUMEROBL As New Label()
    Private lblSTATUTCPU As New Label()
    Private btnCONFIRMER As New Button()
    Private btnIMPRIMER As New Button()
    Private btnFERMER As New Button()
    Private pnlSTATUT As New Panel()

    Private Sub Form_ConfirmationBL_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Me.Text = "Confirmation Bon de Livraison"
        Me.Size = New Size(620, 520)
        Me.BackColor = Color.FromArgb(245, 247, 250)
        Me.StartPosition = FormStartPosition.CenterParent
        Me.FormBorderStyle = FormBorderStyle.FixedDialog
        Me.MaximizeBox = False
        ConstruireInterface()
        RemplirChamps()
    End Sub

    Private Sub ConstruireInterface()
        Dim lblTitre As New Label()
        lblTitre.Text = "CONFIRMATION BON DE LIVRAISON"
        lblTitre.Font = New Font("Arial", 13, FontStyle.Bold)
        lblTitre.ForeColor = Color.FromArgb(10, 50, 120)
        lblTitre.Location = New Point(15, 12)
        lblTitre.AutoSize = True
        Me.Controls.Add(lblTitre)

        Dim pnlRecap As New GroupBox()
        pnlRecap.Text = "Récapitulatif Planning"
        pnlRecap.Location = New Point(15, 45)
        pnlRecap.Size = New Size(575, 140)
        pnlRecap.Font = New Font("Arial", 9, FontStyle.Bold)
        Me.Controls.Add(pnlRecap)

        Dim y As Integer = 22
        For Each item In {("Client :", lblCLIENT), ("Chantier :", lblCHANTIER),
                          ("Formule :", lblFORMULE), ("Volume :", lblVOLUME)}
            Dim lTxt As New Label()
            lTxt.Text = item.Item1
            lTxt.Font = New Font("Arial", 9, FontStyle.Bold)
            lTxt.Location = New Point(10, y)
            lTxt.Size = New Size(90, 22)
            pnlRecap.Controls.Add(lTxt)
            item.Item2.Font = New Font("Arial", 9)
            item.Item2.Location = New Point(105, y)
            item.Item2.Size = New Size(450, 22)
            item.Item2.ForeColor = Color.FromArgb(10, 50, 120)
            pnlRecap.Controls.Add(item.Item2)
            y += 26
        Next

        Dim pnlCamion As New GroupBox()
        pnlCamion.Text = "Camion"
        pnlCamion.Location = New Point(15, 195)
        pnlCamion.Size = New Size(575, 120)
        pnlCamion.Font = New Font("Arial", 9, FontStyle.Bold)
        Me.Controls.Add(pnlCamion)

        For Each c In {("Code camion :", txtCODECAMION, 10, 22),
                       ("Nom camion :", txtNOMCAMION, 200, 22),
                       ("Capacité m³ :", txtCAPACITE, 420, 22),
                       ("Chauffeur :", txtCHAUFFEUR, 10, 65)}
            Dim lbl As New Label()
            lbl.Text = c.Item1
            lbl.Font = New Font("Arial", 8, FontStyle.Bold)
            lbl.Location = New Point(c.Item3, c.Item4)
            lbl.AutoSize = True
            pnlCamion.Controls.Add(lbl)
            c.Item2.Location = New Point(c.Item3, c.Item4 + 18)
            c.Item2.Size = New Size(If(c.Item3 = 420, 140,
                                        If(c.Item3 = 10 AndAlso c.Item4 = 65, 540, 175)), 27)
            c.Item2.Font = New Font("Arial", 9)
            pnlCamion.Controls.Add(c.Item2)
        Next

        Dim btnRech As New Button()
        btnRech.Text = "..."
        btnRech.Location = New Point(390, 40)
        btnRech.Size = New Size(25, 27)
        AddHandler btnRech.Click, AddressOf RechercherCamion
        pnlCamion.Controls.Add(btnRech)

        pnlSTATUT.Location = New Point(15, 325)
        pnlSTATUT.Size = New Size(575, 60)
        pnlSTATUT.BackColor = Color.FromArgb(230, 240, 255)
        pnlSTATUT.BorderStyle = BorderStyle.FixedSingle
        Me.Controls.Add(pnlSTATUT)

        lblNUMEROBL.Text = "En attente de confirmation..."
        lblNUMEROBL.Font = New Font("Consolas", 10, FontStyle.Bold)
        lblNUMEROBL.ForeColor = Color.FromArgb(10, 50, 120)
        lblNUMEROBL.Location = New Point(10, 8)
        lblNUMEROBL.AutoSize = True
        pnlSTATUT.Controls.Add(lblNUMEROBL)

        lblSTATUTCPU.Font = New Font("Arial", 9)
        lblSTATUTCPU.Location = New Point(10, 32)
        lblSTATUTCPU.AutoSize = True
        pnlSTATUT.Controls.Add(lblSTATUTCPU)

        btnCONFIRMER.Text = "✅  CONFIRMER ET ENVOYER AU CPU"
        btnCONFIRMER.Location = New Point(15, 400)
        btnCONFIRMER.Size = New Size(280, 55)
        btnCONFIRMER.BackColor = Color.FromArgb(0, 120, 0)
        btnCONFIRMER.ForeColor = Color.White
        btnCONFIRMER.Font = New Font("Arial", 10, FontStyle.Bold)
        btnCONFIRMER.FlatStyle = FlatStyle.Flat
        AddHandler btnCONFIRMER.Click, AddressOf Confirmer_Click
        Me.Controls.Add(btnCONFIRMER)

        btnIMPRIMER.Text = "🖨  IMPRIMER BL"
        btnIMPRIMER.Location = New Point(305, 400)
        btnIMPRIMER.Size = New Size(160, 55)
        btnIMPRIMER.BackColor = Color.FromArgb(10, 50, 120)
        btnIMPRIMER.ForeColor = Color.White
        btnIMPRIMER.Font = New Font("Arial", 10, FontStyle.Bold)
        btnIMPRIMER.FlatStyle = FlatStyle.Flat
        btnIMPRIMER.Enabled = False
        AddHandler btnIMPRIMER.Click, AddressOf Imprimer_Click
        Me.Controls.Add(btnIMPRIMER)

        btnFERMER.Text = "Fermer"
        btnFERMER.Location = New Point(475, 400)
        btnFERMER.Size = New Size(115, 55)
        btnFERMER.BackColor = Color.FromArgb(180, 60, 60)
        btnFERMER.ForeColor = Color.White
        btnFERMER.Font = New Font("Arial", 10, FontStyle.Bold)
        btnFERMER.FlatStyle = FlatStyle.Flat
        AddHandler btnFERMER.Click, Sub(s, ev) Me.Close()
        Me.Controls.Add(btnFERMER)
    End Sub

    Private Sub RemplirChamps()
        lblCLIENT.Text = NomClient
        lblCHANTIER.Text = NomChantier
        lblFORMULE.Text = CodeFormule & " — " & NomFormule
        lblVOLUME.Text = VolumeTotalM3.ToString("0.00") & " m³"
        txtCODECAMION.Text = CodeCamion
        txtNOMCAMION.Text = NomCamion
        txtCAPACITE.Text = CapaciteCamion.ToString("0.00")
        txtCHAUFFEUR.Text = NomChauffeur
    End Sub

    Private Sub RechercherCamion(sender As Object, e As EventArgs)
        Try
            Dim f As New FrmRecherche()
            f.Initialiser(
                "SELECT IdCamion, CodeCamion, NomCamion FROM dbo.Camions ORDER BY CodeCamion",
                "IdCamion", "CodeCamion", "NomCamion")
            If f.ShowDialog() = DialogResult.OK Then
                Dim dt As DataTable = ModuleDb.Lire(
                    "SELECT CodeCamion, NomCamion, Capacite FROM dbo.Camions WHERE IdCamion=@Id",
                    Sub(cmd) cmd.Parameters.AddWithValue("@Id", f.IdSelectionne))
                If dt IsNot Nothing AndAlso dt.Rows.Count > 0 Then
                    txtCODECAMION.Text = dt.Rows(0)("CodeCamion").ToString()
                    txtNOMCAMION.Text = dt.Rows(0)("NomCamion").ToString()
                    txtCAPACITE.Text = dt.Rows(0)("Capacite").ToString()
                End If
            End If
        Catch ex As Exception
            Debug.WriteLine("Recherche camion : " & ex.Message)
        End Try
    End Sub

    Private Function GenererNumeroBL() As String
        Try
            Dim dernier As Object = ModuleDb.LireValeur(Of String)(
                "SELECT TOP 1 NumeroBL FROM dbo.BonsLivraison " &
                "WHERE NumeroBL LIKE 'BL-" & Date.Today.ToString("yyyyMMdd") & "-%' " &
                "ORDER BY NumeroBL DESC")
            Dim num As Integer = 1
            If dernier IsNot Nothing Then
                Dim parts() As String = dernier.ToString().Split("-"c)
                If parts.Length >= 3 Then
                    Integer.TryParse(parts(parts.Length - 1), num)
                    num += 1
                End If
            End If
            Return "BL-" & Date.Today.ToString("yyyyMMdd") & "-" & num.ToString("D3")
        Catch
            Return "BL-" & Date.Today.ToString("yyyyMMdd") & "-001"
        End Try
    End Function

    Private Sub CreerBLSQL(numeroBL As String, capCamion As Decimal)

        ' ── 1. Lire infos Planning + Client + Chantier + Formule ────
        Dim dt As DataTable = ModuleDb.Lire(
        "SELECT pj.IdClient, cl.NomClient, cl.CodeClient, " &
        "       pj.IdChantier, ch.NomChantier, ch.CodeChantier, " &
        "       pj.IdFormule, f.CodeFormule, f.NomFormule, " &
        "       pj.QuantiteTotale, pj.QuantiteLivree, " &
        "       ISNULL(ch.Commande,'') AS Commande " &
        "FROM dbo.Planning_journalier pj " &
        "LEFT JOIN dbo.Clients  cl ON cl.IdClient   = pj.IdClient " &
        "LEFT JOIN dbo.Chantier ch ON ch.IdChantier = pj.IdChantier " &
        "LEFT JOIN dbo.Formules f  ON f.IdFormule   = pj.IdFormule " &
        "WHERE pj.IdPlanning = @Id",
        Sub(cmd) cmd.Parameters.AddWithValue("@Id", IdPlanning))

        Dim idClient As Integer = 0
        Dim sNomClient As String = NomClient
        Dim codeClient As String = ""
        Dim idChantier As Integer = 0
        Dim sNomChantier As String = NomChantier
        Dim codeChantier As String = ""
        Dim sCodeFormule As String = CodeFormule
        Dim sNomFormule As String = NomFormule
        Dim sCommande As String = ""
        Dim qteCommandee As Decimal = 0
        Dim qteCumulee As Decimal = 0

        If dt IsNot Nothing AndAlso dt.Rows.Count > 0 Then
            Dim r As DataRow = dt.Rows(0)
            idClient = CInt(If(IsDBNull(r("IdClient")), 0, r("IdClient")))
            sNomClient = r("NomClient").ToString()
            codeClient = r("CodeClient").ToString()
            idChantier = CInt(If(IsDBNull(r("IdChantier")), 0, r("IdChantier")))
            sNomChantier = r("NomChantier").ToString()
            codeChantier = r("CodeChantier").ToString()
            sCodeFormule = r("CodeFormule").ToString()
            sNomFormule = r("NomFormule").ToString()
            sCommande = r("Commande").ToString()
            qteCommandee = CDec(If(IsDBNull(r("QuantiteTotale")), 0, r("QuantiteTotale")))
            qteCumulee = CDec(If(IsDBNull(r("QuantiteLivree")), 0, r("QuantiteLivree")))
        End If

        ' ── 2. INSERT BonsLivraison ──────────────────────────────────
        ModuleDb.Executer(
        "INSERT INTO dbo.BonsLivraison " &
        "(NumeroBL, DateHeure, " &
        " IdClient, CodeClient, NomClient, " &
        " IdChantier, CodeChantier, NomChantier, " &
        " IdFormule, CodeFormule, NomFormule, " &
        " CodeCamion, NomChauffeur, " &
        " VolumeM3, Statut, Imprime, Operateur, DateCreation, " &
        " IdPlanning, NumeroCommande, QteCommandee, QteCumulee) " &
        "VALUES " &
        "(@bl, GETDATE(), " &
        " @idCl, @codeCl, @nomCl, " &
        " @idCh, @codeCh, @nomCh, " &
        " @idF, @codeF, @nomF, " &
        " @cam, @chauf, " &
        " @vol, 'EN_COURS', 0, @op, GETDATE(), " &
        " @idPlan, @comm, @qteCmd, @qteCum)",
        Sub(cmd)
            cmd.Parameters.AddWithValue("@bl", numeroBL)
            ' CORRECTION 15/09/2026 : 0 → NULL (sinon violation des clés étrangères FK_BL_Client / FK_BL_Chantier / FK_BL_Formule)
            cmd.Parameters.AddWithValue("@idCl", If(idClient > 0, CObj(idClient), DBNull.Value))
            cmd.Parameters.AddWithValue("@codeCl", codeClient)
            cmd.Parameters.AddWithValue("@nomCl", sNomClient)
            cmd.Parameters.AddWithValue("@idCh", If(idChantier > 0, CObj(idChantier), DBNull.Value))
            cmd.Parameters.AddWithValue("@codeCh", codeChantier)
            cmd.Parameters.AddWithValue("@nomCh", sNomChantier)
            cmd.Parameters.AddWithValue("@idF", If(IdFormule > 0, CObj(IdFormule), DBNull.Value))
            cmd.Parameters.AddWithValue("@codeF", sCodeFormule)
            cmd.Parameters.AddWithValue("@nomF", sNomFormule)
            cmd.Parameters.AddWithValue("@cam", txtCODECAMION.Text)
            cmd.Parameters.AddWithValue("@chauf", txtCHAUFFEUR.Text)
            cmd.Parameters.AddWithValue("@vol", capCamion)
            cmd.Parameters.AddWithValue("@op", NomUtilisateur)
            cmd.Parameters.AddWithValue("@idPlan", IdPlanning)
            cmd.Parameters.AddWithValue("@comm", sCommande)
            cmd.Parameters.AddWithValue("@qteCmd", qteCommandee)
            cmd.Parameters.AddWithValue("@qteCum", qteCumulee)
        End Sub)

    End Sub

    Private Sub Imprimer_Click(sender As Object, e As EventArgs)
        If Not _blConfirme Then
            MessageBox.Show("Confirmez d'abord le BL avant d'imprimer.")
            Exit Sub
        End If
        Dim choix As DialogResult = MessageBox.Show(
            "Imprimer BL avec détail des gâchées ?",
            "Type d'impression",
            MessageBoxButtons.YesNoCancel,
            MessageBoxIcon.Question)
        If choix = DialogResult.Cancel Then Exit Sub
        ' Nouveau modele unique ModuleImpressionBL
        ModuleImpressionBL.ImprimerBL(_numeroBL, avecDetail:=(choix = DialogResult.Yes))
    End Sub

    Private Sub Form_ConfirmationBL_FormClosing(sender As Object, e As FormClosingEventArgs) Handles MyBase.FormClosing
        If Not _blConfirme Then
            If MessageBox.Show("BL non confirmé. Fermer quand même ?",
                "Attention", MessageBoxButtons.YesNo,
                MessageBoxIcon.Warning) = DialogResult.No Then
                e.Cancel = True
            End If
        End If
    End Sub

    ' ── Impression BL apres livraison (appelee depuis Synoptique) ─
    Public Sub ImprimerBLApresLivraison(numeroBL As String)
        Try
            ModuleImpressionBL.ImprimerBL(numeroBL, avecDetail:=True)
        Catch ex As Exception
            Debug.WriteLine("ImprimerBLApresLivraison : " & ex.Message)
        End Try
    End Sub

    ' ═══════════════════════════════════════════════════════════════
    '  A) CONFIRMER — avec validation bloquante AVANT tout
    ' ═══════════════════════════════════════════════════════════════
    Private Sub Confirmer_Click(sender As Object, e As EventArgs)
        If txtCODECAMION.Text.Trim = "" Then
            MessageBox.Show("Veuillez saisir le camion.", "Champ requis",
                MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Exit Sub
        End If

        ' Conversion capacite camion - supporte "8,00" et "8.00"
        Dim capCamion As Decimal = 0
        If Not Decimal.TryParse(txtCAPACITE.Text,
                                Globalization.NumberStyles.Any,
                                Globalization.CultureInfo.CurrentCulture,
                                capCamion) Then
            Decimal.TryParse(txtCAPACITE.Text.Replace(",", "."),
                             Globalization.NumberStyles.Any,
                             Globalization.CultureInfo.InvariantCulture,
                             capCamion)
        End If
        If capCamion <= 0 Then
            MessageBox.Show("Capacité camion invalide.", "Erreur",
                MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Exit Sub
        End If

        ' ═══════════════════════════════════════════════════════════
        '  ★ CONDITIONS DE TRANSFERT (18/09) ★
        ' ═══════════════════════════════════════════════════════════
        ' ─── TEST 18/09 : CONDITIONS_CPU = False → on saute 914.1 / mode auto / défaut / attente fin de cycle
        '     (remettre True pour la production)
        If CapaciteMalaxeur <= 0D OrElse CapaciteMalaxeur > 2D Then
            MessageBox.Show("🛑 LANCEMENT BLOQUÉ" & vbCrLf & vbCrLf &
                "Volume malaxeur = " & CapaciteMalaxeur.ToString("0.00") & " m³ : il doit être compris entre 0 et 2 m³." & vbCrLf &
                "Corrigez « Volume rotation maxi » dans le planning.",
                "Volume malaxeur invalide", MessageBoxButtons.OK, MessageBoxIcon.Stop)
            Exit Sub
        End If
        ' 2. CPU connecté (indispensable pour vérifier l'état du cycle)
        If Not PLCData.Connecte Then
            MessageBox.Show("🛑 LANCEMENT BLOQUÉ" & vbCrLf & vbCrLf &
                "L'automate n'est pas connecté : impossible de transférer la formule." & vbCrLf &
                "Dernière erreur : " & PLCData.DerniereErreur,
                "CPU non connecté", MessageBoxButtons.OK, MessageBoxIcon.Stop)
            Exit Sub
        End If
        ' 3. 21/09 : DÉPART CYCLE DB1.DBX0.4 maintenu par le logiciel, remis à 0 par la CPU
        '    en fin de commande → 0.4 = 1 signifie « commande précédente pas terminée » : blocage.
        '    (condition TOUJOURS active, indépendante de CONDITIONS_CPU)
        Dim dyc04 As Boolean = False
        Try
            dyc04 = CBool(ModulePLCDuplex.LireAdresse("DB1.DBX0.4"))
        Catch
        End Try
        If dyc04 Then
            MessageBox.Show("🛑 LANCEMENT BLOQUÉ" & vbCrLf & vbCrLf &
                "Une commande est encore en cours dans l'automate (Départ cycle DB1.DBX0.4 = 1)." & vbCrLf &
                "Attendez la fin de la commande (la CPU remet le bit à 0), puis relancez.",
                "Commande en cours", MessageBoxButtons.OK, MessageBoxIcon.Stop)
            Synoptique.EnregistrerAction("COMMANDE", "Lancement refuse : 0.4 = 1", "commande precedente non terminee", "DB1.DBX0.4", "1", "CLIC")
            Exit Sub
        End If
        ' 3a. Une commande est encore en cours (cycle CPU ou BL EN_COURS) → attendre la fin ou abandonner
        Dim blEnCours As String = ""
        Try
            Dim o As Object = ModuleDb.LireValeur(Of Object)(
                "SELECT TOP 1 NumeroBL FROM dbo.BonsLivraison WHERE Statut='EN_COURS' ORDER BY DateCreation DESC")
            If o IsNot Nothing AndAlso Not IsDBNull(o) Then blEnCours = o.ToString()
        Catch
        End Try
        '    Référence = bit FIN DE CHARGEMENT DB1.DBX914.1, sens confirmé le 18/09 :
        '      914.1 = 1 → fin de chargement pas encore acquittée (fenêtre OUI/NON/QUITTER) : blocage
        '      914.1 = 0 → libre : un BL resté EN_COURS dans la base est clôturé automatiquement
        ' 18/09 : plus de blocage sur 914.1 → remis à 0 automatiquement avant le transfert (étape 4b)
        If blEnCours <> "" Then
            ModuleArchivageGachees.Instance.CloturerBLManque(blEnCours, "BL reste EN_COURS alors que 914.1 = 0 au lancement suivant")
            blEnCours = ""
        End If
        ' 3b. Automate en mode AUTOMATIQUE et sans défaut actif
        If CONDITIONS_CPU AndAlso Not PLCData.ModeAuto Then
            MessageBox.Show("🛑 LANCEMENT BLOQUÉ" & vbCrLf & vbCrLf &
                "L'automate n'est pas en mode AUTOMATIQUE (DB1.DBX0.0)." & vbCrLf &
                "Passez en automatique sur le Synoptique, puis relancez.",
                "Mode manuel", MessageBoxButtons.OK, MessageBoxIcon.Stop)
            Exit Sub
        End If
        If CONDITIONS_CPU AndAlso PLCData.DefautActif Then
            MessageBox.Show("🛑 LANCEMENT BLOQUÉ" & vbCrLf & vbCrLf &
                "Un DÉFAUT est actif dans l'automate (DB1.DBX1082.0)." & vbCrLf &
                "Acquittez le défaut (Reprise défaut), puis relancez.",
                "Défaut actif", MessageBoxButtons.OK, MessageBoxIcon.Stop)
            Exit Sub
        End If
        ' 3c. Cubage du camion ≤ reste à livrer du planning (avertissement, confirmable)
        Try
            If IdPlanning > 0 Then
                Dim dtQ As System.Data.DataTable = ModuleDb.Lire(
                    "SELECT ISNULL(QuantiteTotale,0) AS T, ISNULL(QuantiteLivree,0) AS L FROM dbo.Planning_journalier WHERE IdPlanning=@id",
                    Sub(cmd) cmd.Parameters.Add("@id", System.Data.SqlDbType.Int).Value = IdPlanning)
                If dtQ.Rows.Count > 0 Then
                    Dim reste As Decimal = CDec(dtQ.Rows(0)("T")) - CDec(dtQ.Rows(0)("L"))
                    If CDec(dtQ.Rows(0)("T")) > 0 AndAlso capCamion > reste + 0.01D Then
                        If MessageBox.Show("⚠️ Le cubage du camion (" & capCamion.ToString("0.00") & " m³) dépasse le reste à livrer" & vbCrLf &
                                           "de la commande (" & reste.ToString("0.00") & " m³)." & vbCrLf & vbCrLf & "Continuer quand même ?",
                                           "Quantité commandée dépassée", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) <> DialogResult.Yes Then
                            Exit Sub
                        End If
                    End If
                End If
            End If
        Catch
        End Try

        ' ═══════════════════════════════════════════════════════════
        '  ★ CONTRÔLE BLOQUANT : formule ↔ configuration trémies ★
        ' ═══════════════════════════════════════════════════════════
        Dim erreurs As List(Of String) = ValiderFormuleContreConfiguration()
        If erreurs.Count > 0 Then
            MessageBox.Show(
                "🛑 LANCEMENT BLOQUÉ" & vbCrLf & vbCrLf &
                "La formule """ & NomFormule & """ contient des matériaux" & vbCrLf &
                "NON AFFECTÉS dans la configuration des trémies/silos/adjuvants :" & vbCrLf & vbCrLf &
                "  • " & String.Join(vbCrLf & "  • ", erreurs) & vbCrLf & vbCrLf &
                "→ Ouvrez « Configuration silos » et affectez ces matériaux" & vbCrLf &
                "   à une trémie, un silo ou une bascule avant de relancer.",
                "Formule incompatible avec la configuration",
                MessageBoxButtons.OK, MessageBoxIcon.Stop)
            Exit Sub   ' ← STOP : pas de BL, pas d'envoi CPU
        End If

        btnCONFIRMER.Enabled = False
        lblSTATUTCPU.Text = "Envoi en cours..."
        lblSTATUTCPU.ForeColor = Color.Orange

        ' Capturer les valeurs avant le thread
        Dim capCamionVal As Decimal = capCamion
        Dim codeCamionVal As String = txtCODECAMION.Text
        Dim chauffeurVal As String = txtCHAUFFEUR.Text

        ' Lancer dans un thread pour ne pas bloquer l UI
        Dim t As New System.Threading.Thread(Sub()
                                                 Try
                                                     ' 4. Attendre la FIN DE CYCLE avant le transfert (sécurité, max 60 s) :
                                                     '    cycle en cours (1088.1) sans fin de commande (914.1)
                                                     Dim attente As Integer = 0
                                                     While CONDITIONS_CPU AndAlso PLCData.CycleEnCours AndAlso attente < 600
                                                         System.Threading.Thread.Sleep(100) : attente += 1
                                                     End While
                                                     If (CONDITIONS_CPU AndAlso PLCData.CycleEnCours) OrElse Not PLCData.Connecte Then
                                                         Me.BeginInvoke(Sub()
                                                                            btnCONFIRMER.Enabled = True
                                                                            lblSTATUTCPU.Text = "Transfert annulé : cycle encore en cours / CPU déconnecté"
                                                                            lblSTATUTCPU.ForeColor = Color.Red
                                                                        End Sub)
                                                         Exit Sub
                                                     End If

                                                     ' 4b. 18/09 : AVANT transfert, remise à 0 de la fin de chargement (914.1), toujours,
                                                     '     puis vérification jusqu'à 3 s que le CPU l'a bien acceptée
                                                     Dim avant914 As Boolean = PLCData.FinChargement
                                                     ModulePLCDuplex.EcrireBit("DB1.DBX914.1", False)
                                                     Dim n914 As Integer = 0
                                                     Dim apres914 As Boolean = True
                                                     Do
                                                         System.Threading.Thread.Sleep(250) : n914 += 1
                                                         Try
                                                             apres914 = CBool(ModulePLCDuplex.LireAdresse("DB1.DBX914.1"))
                                                         Catch
                                                             apres914 = PLCData.FinChargement
                                                         End Try
                                                     Loop While apres914 AndAlso n914 < 12
                                                     Dim trace914 As String = "avant=" & If(avant914, "1", "0") & " apres=" & If(apres914, "1", "0") & " (" & (n914 * 250) & " ms)"
                                                     Me.BeginInvoke(Sub()
                                                                        Synoptique.EnregistrerAction("COMMANDE", "Acquit 914.1 avant transfert", trace914, "DB1.DBX914.1", "0", "PLC_WRITE")
                                                                        If apres914 Then lblSTATUTCPU.Text = "⚠️ 914.1 reste à 1 (le CPU le maintient) — transfert quand même..."
                                                                    End Sub)

                                                     ' 5. Transfert de la formule + capacités ; si échec → pas de BL, pas de départ
                                                     Dim cpuOK As Boolean = EnvoyerVersCPU(capCamionVal)
                                                     ' 5b. Relecture de contrôle : DB1.DBD530…606 doivent contenir la formule envoyée
                                                     Dim ecartRelecture As String = ""
                                                     If cpuOK Then
                                                         Me.BeginInvoke(Sub() lblSTATUTCPU.Text = "Vérification du transfert...")
                                                         cpuOK = VerifierTransfert(ecartRelecture)
                                                         If Not cpuOK Then PLCData.DerniereErreur = "relecture différente : " & ecartRelecture
                                                     End If
                                                     If Not cpuOK Then
                                                         Me.BeginInvoke(Sub()
                                                                            btnCONFIRMER.Enabled = True
                                                                            lblSTATUTCPU.Text = "🛑 Transfert CPU échoué : BL non créé. " & PLCData.DerniereErreur
                                                                            lblSTATUTCPU.ForeColor = Color.Red
                                                                            Synoptique.EnregistrerAction("ALARME", "Transfert formule CPU echoue",
                                                                                PLCData.DerniereErreur, "DB101", "", "ALARME")
                                                                        End Sub)
                                                         Exit Sub
                                                     End If

                                                     ' 6. BL créé, puis DÉPART CYCLE (DemarrerCycle, plus bas)
                                                     Dim numBL As String = GenererNumeroBL()
                                                     CreerBLSQL(numBL, capCamionVal)

                                                     Me.BeginInvoke(Sub()
                                                                        _numeroBL = numBL
                                                                        _blConfirme = True
                                                                        lblNUMEROBL.Text = "BL N° " & numBL & " créé ✅"
                                                                        lblNUMEROBL.ForeColor = Color.DarkGreen
                                                                        pnlSTATUT.BackColor = Color.FromArgb(220, 255, 220)
                                                                        lblSTATUTCPU.Text = If(cpuOK,
                                                                            "CPU mis à jour ✅",
                                                                            "⚠️ BL créé mais PLC non connecté")
                                                                        lblSTATUTCPU.ForeColor = If(cpuOK, Color.DarkGreen, Color.OrangeRed)
                                                                        btnIMPRIMER.Enabled = True
                                                                        btnCONFIRMER.Enabled = False
                                                                        Synoptique.EnregistrerAction("BL", "BL confirme",
                                                                            "BL=" & numBL & " Cam=" & codeCamionVal & " Cap=" & capCamionVal & "m3",
                                                                            "", "", "CLIC")

                                                                        ' Demarrer surveillance cycle + archivage gachees
                                                                        Dim synForm As Synoptique = Nothing
                                                                        For Each frm As Form In Application.OpenForms
                                                                            If TypeOf frm Is Synoptique Then
                                                                                synForm = CType(frm, Synoptique)
                                                                                Exit For
                                                                            End If
                                                                        Next
                                                                        If synForm IsNot Nothing Then
                                                                            ModuleArchivageGachees.Instance.DemarrerCycle(
                                                                                synForm._plc, numBL, IdPlanning)
                                                                        End If
                                                                        ' 22/09 : BL créé + cycle lancé → fermeture automatique (retour au Synoptique) après 1,5 s
                                                                        Dim tFerme As New System.Windows.Forms.Timer() With {.Interval = 1500}
                                                                        AddHandler tFerme.Tick, Sub()
                                                                                                   tFerme.Stop() : tFerme.Dispose()
                                                                                                   Try
                                                                                                       Me.DialogResult = DialogResult.OK
                                                                                                       Me.Close()
                                                                                                   Catch
                                                                                                   End Try
                                                                                               End Sub
                                                                        tFerme.Start()
                                                                        ' 7. Vérifier que le CPU a bien démarré le cycle (≤ 8 s)
                                                                        System.Threading.ThreadPool.QueueUserWorkItem(
                                                                            Sub(state)
                                                                                Dim n As Integer = 0
                                                                                While Not PLCData.CycleEnCours AndAlso n < 80
                                                                                    System.Threading.Thread.Sleep(100) : n += 1
                                                                                End While
                                                                                Dim demarre As Boolean = PLCData.CycleEnCours
                                                                                Try
                                                                                    Me.BeginInvoke(Sub()
                                                                                                       If demarre Then
                                                                                                           lblSTATUTCPU.Text = "CPU mis à jour ✅ — cycle démarré"
                                                                                                           lblSTATUTCPU.ForeColor = Color.DarkGreen
                                                                                                       Else
                                                                                                           lblSTATUTCPU.Text = "⚠️ Le CPU n'a pas démarré le cycle (vérifier mode auto / défaut / skip)"
                                                                                                           lblSTATUTCPU.ForeColor = Color.OrangeRed
                                                                                                           Synoptique.EnregistrerAction("ALARME", "Depart cycle sans reponse du CPU",
                                                                                                               "BL=" & numBL, "DB1.DBX0.4", "", "ALARME")
                                                                                                       End If
                                                                                                   End Sub)
                                                                                Catch
                                                                                End Try
                                                                            End Sub)
                                                                    End Sub)

                                                 Catch ex As Exception
                                                     Me.BeginInvoke(Sub()
                                                                        btnCONFIRMER.Enabled = True
                                                                        lblSTATUTCPU.Text = "Erreur : " & ex.Message
                                                                        lblSTATUTCPU.ForeColor = Color.Red
                                                                    End Sub)
                                                 End Try
                                             End Sub)
        t.IsBackground = True
        t.Start()
    End Sub

    ' ═══════════════════════════════════════════════════════════════
    '  C) VALIDATION : chaque matériau de la formule doit être
    '     affecté dans la configuration (Cases / Silos / Bascules)
    ' ═══════════════════════════════════════════════════════════════
    Private Function ValiderFormuleContreConfiguration() As List(Of String)
        Dim erreurs As New List(Of String)
        Try
            Dim dt As System.Data.DataTable = ModuleDb.Lire(
                "SELECT fc.IdMateriau, fc.Quantite, " &
                "       ISNULL(m.CodeMateriau, fc.CodeMateriau) AS Code, " &
                "       ISNULL(m.NomMateriau, '') AS Nom, " &
                "       CASE WHEN EXISTS (SELECT 1 FROM dbo.Cases    c WHERE c.IdMateriau = fc.IdMateriau) THEN 1 " &
                "            WHEN EXISTS (SELECT 1 FROM dbo.Silos    s WHERE s.IdMateriau = fc.IdMateriau AND s.EstActif = 1) THEN 1 " &
                "            WHEN EXISTS (SELECT 1 FROM dbo.Bascules b WHERE b.IdMateriau = fc.IdMateriau) THEN 1 " &
                "            ELSE 0 END AS EstAffecte " &
                "FROM dbo.FormuleComposition fc " &
                "LEFT JOIN dbo.Materiaux m ON fc.IdMateriau = m.IdMateriau " &
                "WHERE fc.IdFormule = @id AND ISNULL(fc.Quantite, 0) > 0",
                Sub(cmd) cmd.Parameters.Add("@id", System.Data.SqlDbType.Int).Value = IdFormule)

            If dt Is Nothing OrElse dt.Rows.Count = 0 Then
                erreurs.Add("La formule est vide (aucun composant avec quantité > 0).")
                Return erreurs
            End If

            For Each row As System.Data.DataRow In dt.Rows
                Dim code As String = row("Code").ToString().Trim()
                Dim nom As String = row("Nom").ToString().Trim()
                Dim libelle As String = If(nom <> "", code & " — " & nom, code)

                If IsDBNull(row("IdMateriau")) Then
                    erreurs.Add(libelle & "  (composant non lié à la table Matériaux — rééditez la formule)")
                ElseIf CInt(row("EstAffecte")) = 0 Then
                    erreurs.Add(libelle)
                End If
            Next

        Catch ex As Exception
            erreurs.Add("Erreur de vérification : " & ex.Message)
        End Try
        Return erreurs
    End Function

    ' ═══════════════════════════════════════════════════════════════
    '  B) ENVOI CPU — mapping basé sur la CONFIGURATION réelle
    '
    '     Offsets « historiques » DB101 (traduits vers DB1 par
    '     ModulePLCDuplex.EcrireReal — table _mapRealDB1) :
    '       Trémie 1..4  → 0, 4, 8, 12    → DB1.DBD538/542/546/550
    '       Silo ciment  → 16             → DB1.DBD530
    '       Eau          → 20             → DB1.DBD586
    '       Adjuvant 1..4→ 24, 28, 32, 36 → DB1.DBD594/598/602/606
    '     Capacités DB19.DBD58/66          → DB1.DBD916/920
    ' ═══════════════════════════════════════════════════════════════
    ' Consignes envoyées lors du dernier transfert (offset DB101 → kg/m3), pour la relecture
    Private _dernieresConsignes As Dictionary(Of Integer, Single) = Nothing

    ' ── Relecture de contrôle du transfert (18/09) ─────────────────
    '    DB101.DBD0/4/8/12/16/20/24…36 → DB1.DBD538/542/546/550/530/586/594…606
    '    On attend le rafraîchissement de la zone lente DB1 (530→610, ~3 s).
    Private Function VerifierTransfert(ByRef detail As String) As Boolean
        detail = ""
        If _dernieresConsignes Is Nothing Then Return True
        Dim versDB1 As New Dictionary(Of Integer, Integer) From {
            {0, 538}, {4, 542}, {8, 546}, {12, 550}, {16, 530}, {20, 586}, {24, 594}, {28, 598}, {32, 602}, {36, 606}}
        Dim noms As New Dictionary(Of Integer, String) From {
            {0, "TR1"}, {4, "TR2"}, {8, "TR3"}, {12, "TR4"}, {16, "CIM"}, {20, "EAU"}, {24, "ADJ1"}, {28, "ADJ2"}, {32, "ADJ3"}, {36, "ADJ4"}}
        For essai As Integer = 1 To 4
            System.Threading.Thread.Sleep(1500)
            detail = ""
            For Each kvp In _dernieresConsignes
                Dim lu As Single = ModulePLCDuplex.LireDBDReal(1, versDB1(kvp.Key))
                If Math.Abs(lu - kvp.Value) > 0.05F Then
                    detail &= noms(kvp.Key) & " envoyé " & kvp.Value.ToString("0.00") & " lu " & lu.ToString("0.00") & " ; "
                End If
            Next
            If detail = "" Then Return True
        Next
        Return False
    End Function

    Private Function EnvoyerVersCPU(capCamion As Decimal) As Boolean
        Try
            ' ── 1. Charger la composition (liée par IdMateriau) ─────
            Dim dtCompo As System.Data.DataTable = ModuleDb.Lire(
                "SELECT IdMateriau, Quantite FROM dbo.FormuleComposition " &
                "WHERE IdFormule=@id AND IdMateriau IS NOT NULL " &
                "ORDER BY Phase, IdComposition",
                Sub(cmd) cmd.Parameters.Add("@id", System.Data.SqlDbType.Int).Value = IdFormule)
            If dtCompo Is Nothing OrElse dtCompo.Rows.Count = 0 Then Return False

            ' Quantités par IdMateriau (cumul si doublon)
            Dim qteParMat As New Dictionary(Of Integer, Single)
            For Each row As System.Data.DataRow In dtCompo.Rows
                Dim idm As Integer = CInt(row("IdMateriau"))
                Dim q As Single = CSng(If(IsDBNull(row("Quantite")), 0, row("Quantite")))
                If qteParMat.ContainsKey(idm) Then
                    qteParMat(idm) += q
                Else
                    qteParMat(idm) = q
                End If
            Next

            ' ── 2. Mapping IdMateriau → offset (depuis la CONFIGURATION)
            Dim offsetParMat As New Dictionary(Of Integer, Integer)

            ' Trémies : NumeroCase 1..4 → offsets 0,4,8,12
            Dim dtTr As System.Data.DataTable = ModuleDb.Lire(
                "SELECT NumeroCase, IdMateriau FROM dbo.Cases " &
                "WHERE IdMateriau IS NOT NULL AND NumeroCase BETWEEN 1 AND 4 " &
                "ORDER BY NumeroCase")
            For Each row As System.Data.DataRow In dtTr.Rows
                Dim idm As Integer = CInt(row("IdMateriau"))
                If Not offsetParMat.ContainsKey(idm) Then
                    offsetParMat(idm) = (CInt(row("NumeroCase")) - 1) * 4
                End If
            Next

            ' Silos ciment (TypeElement = 'S' uniquement) → offset 16
            Dim dtSi As System.Data.DataTable = ModuleDb.Lire(
                "SELECT IdMateriau FROM dbo.Silos " &
                "WHERE IdMateriau IS NOT NULL AND EstActif = 1 AND TypeElement = 'S'")
            For Each row As System.Data.DataRow In dtSi.Rows
                Dim idm As Integer = CInt(row("IdMateriau"))
                If Not offsetParMat.ContainsKey(idm) Then offsetParMat(idm) = 16
            Next

            ' Eau : bascule type EAU → offset 20
            Dim dtEau As System.Data.DataTable = ModuleDb.Lire(
                "SELECT b.IdMateriau FROM dbo.Bascules b " &
                "JOIN dbo.TypesBascule t ON b.TypeID = t.TypeID " &
                "WHERE t.NomType LIKE '%EAU%' AND b.IdMateriau IS NOT NULL")
            For Each row As System.Data.DataRow In dtEau.Rows
                Dim idm As Integer = CInt(row("IdMateriau"))
                If Not offsetParMat.ContainsKey(idm) Then offsetParMat(idm) = 20
            Next

            ' Adjuvants : par Ordre → offsets 24,28,32,36
            Dim dtAdj As System.Data.DataTable = ModuleDb.Lire(
                "SELECT b.IdMateriau, ROW_NUMBER() OVER (ORDER BY b.Ordre, b.BasculeID) AS Rang " &
                "FROM dbo.Bascules b " &
                "JOIN dbo.TypesBascule t ON b.TypeID = t.TypeID " &
                "WHERE t.NomType LIKE '%ADJUVANT%' AND b.IdMateriau IS NOT NULL")
            For Each row As System.Data.DataRow In dtAdj.Rows
                Dim idm As Integer = CInt(row("IdMateriau"))
                Dim rang As Integer = CInt(row("Rang"))
                If rang <= 4 AndAlso Not offsetParMat.ContainsKey(idm) Then
                    offsetParMat(idm) = 20 + rang * 4   ' 24,28,32,36
                End If
            Next

            ' ── 3. Consignes par offset (0 par défaut partout) ──────
            Dim consignes As New Dictionary(Of Integer, Single)
            For Each off In {0, 4, 8, 12, 16, 20, 24, 28, 32, 36}
                consignes(off) = 0.0F
            Next
            _dernieresConsignes = consignes
            For Each kvp In qteParMat
                If offsetParMat.ContainsKey(kvp.Key) Then
                    consignes(offsetParMat(kvp.Key)) += kvp.Value
                End If
            Next

            ' ── 4. Liaison PLC : connexion partagée du programme ────
            If Module1.PLCInstance Is Nothing OrElse Not Module1.PLCInstance.IsConnected Then
                Try
                    ConnecterPLCGlobal()   ' Module1
                Catch
                End Try
            End If
            If Module1.PLCInstance Is Nothing OrElse Not Module1.PLCInstance.IsConnected Then
                Debug.WriteLine("EnvoyerVersCPU : PLC non connecté")
                Return False
            End If

            ' ── 5. Écriture des consignes — traduites vers DB1 ──────
            Dim tousOk As Boolean = True
            For Each kvp In consignes
                If Not ModulePLCDuplex.EcrireReal(101, kvp.Key, kvp.Value) Then tousOk = False
            Next
            If Not ModulePLCDuplex.EcrireReal(1, 916, CSng(CapaciteMalaxeur)) Then tousOk = False
            If Not ModulePLCDuplex.EcrireReal(1, 920, CSng(capCamion)) Then tousOk = False

            If Not tousOk Then
                Debug.WriteLine("EnvoyerVersCPU : au moins une consigne NON écrite")
                Return False
            End If

            Synoptique.EnregistrerAction("COMMANDE", "Formule envoyée au CPU",
                consignes.Count & " consignes + capacités", "DB1 (ex DB101/DB19)", "", "PLC_WRITE")
            Return True

        Catch ex As Exception
            Debug.WriteLine("EnvoyerVersCPU : " & ex.Message)
            Return False
        End Try
    End Function

End Class