Imports System.Drawing.Printing
Imports Microsoft.Data.SqlClient
Imports System.Data

Public Class Planning_journalier

    Public IdPlanningEnCours As Integer = 0

    Private Sub Planning_journalier_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        dtpDatePlanning.Value = Date.Today
        StyleGrillePlanning()
        AvancerCommandesComplement()   ' ← AJOUT
        ChargerPlanningJournalier()
    End Sub

    Private Sub StyleGrillePlanning()
        With DataGridViewPlanning
            .ReadOnly = True : .AllowUserToAddRows = False : .AllowUserToDeleteRows = False
            .SelectionMode = DataGridViewSelectionMode.FullRowSelect : .MultiSelect = False
            .AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.None : .RowHeadersVisible = False
            .EnableHeadersVisualStyles = False
            .ColumnHeadersDefaultCellStyle.BackColor = Color.LightGray
            .ColumnHeadersDefaultCellStyle.ForeColor = Color.Black
            .DefaultCellStyle.SelectionBackColor = Color.RoyalBlue
            .DefaultCellStyle.SelectionForeColor = Color.White
            .DefaultCellStyle.BackColor = Color.White
            .DefaultCellStyle.ForeColor = Color.Black
        End With
    End Sub
    ' ================================================================
    '  AVANCEMENT AUTOMATIQUE — Commandes avec Complement=1
    '  Au chargement : si DatePlanning < aujourd'hui et Complement=1,
    '  on met à jour DatePlanning = aujourd'hui (la ligne suit le jour)
    '  QuantiteLivree et StatutPlanning sont remis à zéro / 'Créé'
    ' ================================================================
    Private Sub AvancerCommandesComplement()
        Try
            ModuleDb.Executer(
        "UPDATE dbo.Planning_journalier " &
        "SET DatePlanning      = CAST(GETDATE() AS DATE), " &
        "    StatutPlanning    = CASE WHEN ISNULL(QuantiteLivree,0) >= ISNULL(QuantiteTotale,0) " &
        "                             THEN 'Terminé' ELSE 'Créé' END, " &
        "    DateModification  = GETDATE() " &
        "WHERE Complement = 1 " &
        "  AND DatePlanning < CAST(GETDATE() AS DATE)",
        Sub(cmd)
        End Sub)
        Catch ex As Exception
            MessageBox.Show("Erreur avancement commandes complément : " & ex.Message)
        End Try
    End Sub
    Private Sub ChargerPlanningJournalier()
        Try
            Dim dt As DataTable = ModuleDb.Lire(
    "SELECT IdPlanning AS [N°], " &
    "CONVERT(VARCHAR(5), HeureFabrication, 108) AS FAB, " &
    "CONVERT(VARCHAR(5), HeureLivraison, 108) AS LIV, " &
    "NomClient AS CLIENT, NomChantier AS CHANTIER, " &
    "CodeFormule AS FORMULE, QuantiteTotale AS QTE, " &
    "CASE WHEN Complement=1 THEN 'OUI' ELSE '' END AS PL, " &
    "CASE WHEN AConfirmer=1 THEN 'OUI' ELSE '' END AS AC, " &
    "QuantiteLivree AS LIVRE, Reste AS RESTE, " &
    "ISNULL(Remarque,'') AS REMARQUE, " &
    "ISNULL((SELECT TOP 1 Commande FROM dbo.Chantier WHERE CodeChantier=v.CodeChantier),'') AS COMMANDE " &
    "FROM dbo.vw_ChargePlanning v " &
    "WHERE DatePlanning=@DatePlanning " &
    "ORDER BY HeureFabrication, IdPlanning",
    Sub(cmd)
        cmd.Parameters.Add("@DatePlanning", SqlDbType.Date).Value = dtpDatePlanning.Value.Date
    End Sub)

            DataGridViewPlanning.DataSource = Nothing
            DataGridViewPlanning.DataSource = dt
            AppliquerColonnes()
            MettreAJourTotaux(dt)
            Me.BeginInvoke(New Action(Sub() ColorerLignesPlanning()))
        Catch ex As Exception
            MessageBox.Show("Erreur chargement planning : " & ex.Message)
        End Try
    End Sub

    Private Sub AppliquerColonnes()
        If DataGridViewPlanning.Columns.Count = 0 Then Exit Sub
        Dim largeurs = New Dictionary(Of String, Integer) From {
        {"N°", 45}, {"FAB", 60}, {"LIV", 60}, {"CLIENT", 180}, {"CHANTIER", 180},
        {"FORMULE", 80}, {"QTE", 70}, {"PL", 45}, {"AC", 45},
        {"LIVRE", 70}, {"RESTE", 70}, {"REMARQUE", 150},
        {"COMMANDE", 120}}   ' ← NOUVEAU
        For Each kvp In largeurs
            If DataGridViewPlanning.Columns.Contains(kvp.Key) Then
                DataGridViewPlanning.Columns(kvp.Key).Width = kvp.Value
            End If
        Next
    End Sub

    Private Sub MettreAJourTotaux(dt As DataTable)
        lblNombreEnregistrements.Text = dt.Rows.Count.ToString()
        Dim totalReste As Decimal = 0D
        For Each row As DataRow In dt.Rows
            If Not IsDBNull(row("RESTE")) Then totalReste += Convert.ToDecimal(row("RESTE"))
        Next
        txtResteTotal.Text = totalReste.ToString("0.00")
    End Sub

    Private Sub ColorerLignesPlanning()
        If DataGridViewPlanning.Columns.Count = 0 OrElse
           Not DataGridViewPlanning.Columns.Contains("PL") Then Exit Sub
        For Each row As DataGridViewRow In DataGridViewPlanning.Rows
            If row.IsNewRow Then Continue For
            Dim pl As String = If(row.Cells("PL").Value, "").ToString().Trim().ToUpper()
            Dim qte As Decimal = LireCellule(row, "QTE")
            Dim livre As Decimal = LireCellule(row, "LIVRE")
            Dim reste As Decimal = LireCellule(row, "RESTE")
            Dim couleur As Color
            If pl = "" OrElse pl = "NON" Then
                couleur = Color.FromArgb(255, 210, 210)
            ElseIf livre >= qte AndAlso qte > 0 Then
                couleur = Color.FromArgb(210, 255, 210)
            ElseIf reste > 0 Then
                couleur = Color.FromArgb(255, 245, 180)
            Else
                couleur = Color.White
            End If
            row.DefaultCellStyle.BackColor = couleur
            row.DefaultCellStyle.ForeColor = Color.Black
        Next
        DataGridViewPlanning.Refresh()
    End Sub

    Private Function LireCellule(row As DataGridViewRow, col As String) As Decimal
        Dim v As Decimal
        Decimal.TryParse(If(row.Cells(col).Value, "0").ToString(), v)
        Return v
    End Function

    Private Sub btnJourPlus_Click(sender As Object, e As EventArgs) Handles btnJourPlus.Click
        dtpDatePlanning.Value = dtpDatePlanning.Value.AddDays(1) : ChargerPlanningJournalier()
    End Sub
    Private Sub btnJourMoins_Click(sender As Object, e As EventArgs) Handles btnJourMoins.Click
        dtpDatePlanning.Value = dtpDatePlanning.Value.AddDays(-1) : ChargerPlanningJournalier()
    End Sub
    Private Sub btnActualiser_Click_1(sender As Object, e As EventArgs) Handles btnActualiser.Click
        ChargerPlanningJournalier()
    End Sub

    Private Sub btnAjouter_Click(sender As Object, e As EventArgs) Handles btnAjouter.Click
        Dim f As New Planification_Demands : f.ShowDialog() : ChargerPlanningJournalier()
    End Sub

    Private Sub btnModifier_Click(sender As Object, e As EventArgs) Handles btnModifier.Click
        If Not VerifierSelectionLigne() Then Exit Sub
        Try
            Dim f As New Planification_Demands
            f.IdPlanningEnCours = LireIdLigneSelectionnee()
            f.ShowDialog()
            ChargerPlanningJournalier()
        Catch ex As Exception
            MessageBox.Show("Erreur modification : " & ex.Message)
        End Try
    End Sub

    Private Sub btnSupprimer_Click(sender As Object, e As EventArgs) Handles btnSupprimer.Click
        If Not VerifierSelectionLigne() Then Exit Sub
        Dim idPlanning As Integer = LireIdLigneSelectionnee()
        If MessageBox.Show("Supprimer ce planning ?", "Confirmation",
                           MessageBoxButtons.YesNo, MessageBoxIcon.Question) = DialogResult.No Then Exit Sub
        Try
            ModuleDb.Executer(
                "DELETE FROM dbo.Planning_journalier WHERE IdPlanning=@IdPlanning",
                Sub(cmd) cmd.Parameters.Add("@IdPlanning", SqlDbType.Int).Value = idPlanning)
            MessageBox.Show("Planning supprimé.")
            ChargerPlanningJournalier()
        Catch ex As Exception
            MessageBox.Show("Erreur suppression : " & ex.Message)
        End Try
    End Sub

    Private Sub Duplique_Click(sender As Object, e As EventArgs) Handles Duplique.Click
        If Not VerifierSelectionLigne() Then Exit Sub
        Dim idAncien As Integer = LireIdLigneSelectionnee()
        Try
            Dim idNouveau As Integer = ModuleDb.LireValeur(Of Integer)(
                "INSERT INTO dbo.Planning_journalier " &
                "(IdClient, IdChantier, IdFormule, DatePlanning, HeureFabrication, HeureLivraison, " &
                "QuantiteTotale, QuantiteLivree, Complement, AConfirmer, CadenceMinutes, VolumeRotationMaxi, " &
                "IdCamion, IdChauffeur, IdEquipementService, QuantiteService, Remarque, StatutPlanning, DateCreation) " &
                "SELECT IdClient, IdChantier, IdFormule, @DatePlanning, HeureFabrication, HeureLivraison, " &
                "QuantiteTotale, 0, Complement, AConfirmer, CadenceMinutes, VolumeRotationMaxi, " &
                "IdCamion, IdChauffeur, IdEquipementService, QuantiteService, Remarque, 'Créé', GETDATE() " &
                "FROM dbo.Planning_journalier WHERE IdPlanning=@IdPlanning; " &
                "SELECT CAST(SCOPE_IDENTITY() AS INT);",
                Sub(cmd)
                    cmd.Parameters.Add("@IdPlanning", SqlDbType.Int).Value = idAncien
                    cmd.Parameters.Add("@DatePlanning", SqlDbType.Date).Value = Date.Today
                End Sub)

            ModuleDb.Executer(
                "INSERT INTO dbo.Planning_ProduitsServices " &
                "(IdPlanning, NumeroLigne, IdProduit, Quantite, Unite) " &
                "SELECT @IdNouv, NumeroLigne, IdProduit, Quantite, Unite " &
                "FROM dbo.Planning_ProduitsServices WHERE IdPlanning=@IdAnc",
                Sub(cmd)
                    cmd.Parameters.Add("@IdNouv", SqlDbType.Int).Value = idNouveau
                    cmd.Parameters.Add("@IdAnc", SqlDbType.Int).Value = idAncien
                End Sub)

            MessageBox.Show("Planning dupliqué avec succès.")
            dtpDatePlanning.Value = Date.Today
            ChargerPlanningJournalier()
        Catch ex As Exception
            MessageBox.Show("Erreur duplication : " & ex.Message)
        End Try
    End Sub

    ' ================================================================
    '  PATCH — Public_Class_Planning_journalier.vb
    '  Modifier Exécute_Click pour ouvrir confirmation + envoi CPU
    ' ================================================================
    '
    '  REMPLACER le contenu de Exécute_Click par ce code :
    '
    ' ================================================================

    Private Sub Exécute_Click(sender As Object, e As EventArgs) Handles Exécute.Click
        If Not VerifierSelectionLigne() Then Exit Sub
        Dim idPlanning As Integer = LireIdLigneSelectionnee()

        If MessageBox.Show("Démarrer la fabrication pour ce planning ?",
                           "Confirmation Exécution",
                           MessageBoxButtons.YesNo,
                           MessageBoxIcon.Question) = DialogResult.No Then Exit Sub
        Try
            ' 1. Verifier statut
            Dim statut As String = ModuleDb.LireValeur(Of String)(
                "SELECT StatutPlanning FROM dbo.Planning_journalier WHERE IdPlanning=@Id",
                Sub(cmd) cmd.Parameters.Add("@Id", SqlDbType.Int).Value = idPlanning)

            If statut IsNot Nothing AndAlso
   (statut.ToUpper = "TERMINÉ" OrElse statut.ToUpper = "TERMINE") Then

                ' Lire complement, quantite totale et livree
                Dim complement As Boolean = CBool(ModuleDb.LireValeur(Of Object)(
        "SELECT ISNULL(Complement,0) FROM dbo.Planning_journalier WHERE IdPlanning=@Id",
        Sub(cmd) cmd.Parameters.Add("@Id", SqlDbType.Int).Value = idPlanning))
                Dim qteTotal As Decimal = CDec(ModuleDb.LireValeur(Of Object)(
        "SELECT ISNULL(QuantiteTotale,0) FROM dbo.Planning_journalier WHERE IdPlanning=@Id",
        Sub(cmd) cmd.Parameters.Add("@Id", SqlDbType.Int).Value = idPlanning))
                Dim qteLivre As Decimal = CDec(ModuleDb.LireValeur(Of Object)(
        "SELECT ISNULL(QuantiteLivree,0) FROM dbo.Planning_journalier WHERE IdPlanning=@Id",
        Sub(cmd) cmd.Parameters.Add("@Id", SqlDbType.Int).Value = idPlanning))

                ' Bloquer seulement si beton totalement livre ET complement pas coche
                If qteLivre >= qteTotal AndAlso Not complement Then
                    MessageBox.Show("Ce planning est déjà terminé et totalement livré!",
            "Information", MessageBoxButtons.OK, MessageBoxIcon.Information)
                    Exit Sub
                End If
                ' Sinon continuer — complement autorise ou reste a livrer
            End If

            ' 2. Passer en cours
            ModuleDb.Executer(
                "UPDATE dbo.Planning_journalier SET StatutPlanning='En cours', " &
                "DateModification=GETDATE() WHERE IdPlanning=@Id",
                Sub(cmd) cmd.Parameters.Add("@Id", SqlDbType.Int).Value = idPlanning)

            MessageBox.Show("✅ Fabrication démarrée!" & vbCrLf &
                "Planning N° " & idPlanning & " → En cours",
                "Exécution", MessageBoxButtons.OK, MessageBoxIcon.Information)

            ' 3. Lire infos planning pour la confirmation
            Dim dtPlan As DataTable = ModuleDb.Lire(
                "SELECT pj.IdPlanning, pj.IdClient, cl.NomClient, " &
                "       pj.IdChantier, ch.NomChantier, " &
                "       pj.IdFormule, f.CodeFormule, f.NomFormule, " &
                "       pj.QuantiteTotale, pj.VolumeRotationMaxi, " &
               "       ISNULL(ca.CodeCamion,'')     AS CodeCamion, " &
"       ISNULL(ca.NomCamion,'')      AS NomCamion, " &
"       ISNULL(ca.Capacite,0)        AS Capacite, " &
"       ISNULL(ch2.NomChauffeur,'')  AS NomChauffeur " &
"FROM dbo.Planning_journalier pj " &
"LEFT JOIN dbo.Clients   cl  ON cl.IdClient      = pj.IdClient " &
"LEFT JOIN dbo.Chantier  ch  ON ch.IdChantier    = pj.IdChantier " &
"LEFT JOIN dbo.Formules  f   ON f.IdFormule      = pj.IdFormule " &
"LEFT JOIN dbo.Camions   ca  ON ca.IdCamion      = pj.IdCamion " &
"LEFT JOIN dbo.Chauffeurs ch2 ON ch2.IdChauffeur = pj.IdChauffeur " &
                "WHERE pj.IdPlanning = @Id",
                Sub(cmd) cmd.Parameters.Add("@Id", SqlDbType.Int).Value = idPlanning)

            If dtPlan IsNot Nothing AndAlso dtPlan.Rows.Count > 0 Then
                Dim row As DataRow = dtPlan.Rows(0)

                ' 4. Ouvrir fenetre de confirmation + envoi CPU
                Dim fConfirm As New Form_ConfirmationBL()
                fConfirm.IdPlanning = idPlanning
                fConfirm.NomClient = row("NomClient").ToString()
                fConfirm.NomChantier = row("NomChantier").ToString()
                fConfirm.CodeFormule = row("CodeFormule").ToString()
                fConfirm.NomFormule = row("NomFormule").ToString()
                fConfirm.IdFormule = CInt(If(IsDBNull(row("IdFormule")), 0, row("IdFormule")))
                fConfirm.VolumeTotalM3 = CDec(If(IsDBNull(row("QuantiteTotale")), 0, row("QuantiteTotale")))
                fConfirm.CapaciteMalaxeur = CDec(If(IsDBNull(row("VolumeRotationMaxi")), 1, row("VolumeRotationMaxi")))
                fConfirm.CodeCamion = row("CodeCamion").ToString()
                fConfirm.NomCamion = row("NomCamion").ToString()
                fConfirm.CapaciteCamion = CDec(If(IsDBNull(row("Capacite")), 0, row("Capacite")))
                fConfirm.NomChauffeur = row("NomChauffeur").ToString()
                fConfirm.ShowDialog(Me)

                ' 22/09 : BL envoyé au CPU → fermer aussi le planning, retour au Synoptique
                If fConfirm.BLConfirme Then
                    Try
                        For Each frm As Form In Application.OpenForms
                            If TypeOf frm Is Synoptique Then frm.Activate() : frm.BringToFront() : Exit For
                        Next
                    Catch
                    End Try
                    Me.Close()
                    Exit Sub
                End If
            End If

            ChargerPlanningJournalier()

        Catch ex As Exception
            MessageBox.Show("Erreur exécution : " & ex.Message)
        End Try
    End Sub
    Private Sub DataGridViewPlanning_CellDoubleClick(sender As Object, e As DataGridViewCellEventArgs) Handles DataGridViewPlanning.CellDoubleClick
        If e.RowIndex < 0 Then Exit Sub
        Try
            Dim f As New Planification_Demands
            f.IdPlanningEnCours = LireIdLigneSelectionnee()
            f.ShowDialog()
            ChargerPlanningJournalier()
        Catch ex As Exception
            MessageBox.Show("Erreur modification : " & ex.Message)
        End Try
    End Sub

    Private Sub DataGridViewPlanning_CellContentClick(sender As Object, e As DataGridViewCellEventArgs) Handles DataGridViewPlanning.CellContentClick
    End Sub

    ' ═══════════════════════════════════════════════════════════════════
    '  IMPRESSION
    ' ═══════════════════════════════════════════════════════════════════

    Private WithEvents pdPlanning As New PrintDocument
    Private _impClient As String = "" : Private _impChantier As String = ""
    Private _impFormule As String = "" : Private _impGamme As String = ""
    Private _impTempsMalaxage As String = "" : Private _impMES As String = ""
    Private _impVolumeAir As String = "" : Private _impTauxChlorure As String = ""
    Private _impMasseAlcalin As String = "" : Private _impDateLivraison As String = ""
    Private _impHeureFabrication As String = "" : Private _impHeureLivraison As String = ""
    Private _impQuantiteTotale As String = "" : Private _impQuantiteLivree As String = ""
    Private _impReste As String = "" : Private _impCadence As String = ""
    Private _impVolumeRotation As String = "" : Private _impComplement As String = ""
    Private _impAConfirmer As String = "" : Private _impCamion As String = ""
    Private _impChauffeur As String = "" : Private _impCapacite As String = ""
    Private _impRemarque As String = ""
    Private _impLignesPS As New List(Of String())

    Private Sub Imprimer_Click(sender As Object, e As EventArgs) Handles Imprimer.Click
        If Not VerifierSelectionLigne() Then Exit Sub
        Try
            ChargerDonneesImpression(LireIdLigneSelectionnee())
            Dim apercu As New PrintPreviewDialog
            apercu.Document = pdPlanning
            apercu.WindowState = FormWindowState.Maximized
            apercu.ShowDialog()
        Catch ex As Exception
            MessageBox.Show("Erreur impression : " & ex.Message)
        End Try
    End Sub

    Private Sub ChargerDonneesImpression(idPlanning As Integer)
        Try
            Dim dt As DataTable = ModuleDb.Lire(
                "SELECT p.*, cl.CodeClient, cl.NomClient, ch.CodeChantier, ch.NomChantier, " &
                "f.CodeFormule, f.NomFormule, f.Gamme, f.TempsMalaxage, " &
                "f.MES, f.VolumeAir, f.TauxChlorureCalc, f.MasseAlcalinCalc, " &
                "ca.CodeCamion, ca.NomCamion, ca.Capacite, cf.CodeChauffeur, cf.NomChauffeur, " &
                "(p.QuantiteTotale - p.QuantiteLivree) AS Reste " &
                "FROM dbo.Planning_journalier p " &
                "LEFT JOIN dbo.Clients cl ON cl.IdClient = p.IdClient " &
                "LEFT JOIN dbo.Chantier ch ON ch.IdChantier = p.IdChantier " &
                "LEFT JOIN dbo.Formules f ON f.IdFormule = p.IdFormule " &
                "LEFT JOIN dbo.Camions ca ON ca.IdCamion = p.IdCamion " &
                "LEFT JOIN dbo.Chauffeurs cf ON cf.IdChauffeur = p.IdChauffeur " &
                "WHERE p.IdPlanning=@IdPlanning",
                Sub(cmd) cmd.Parameters.Add("@IdPlanning", SqlDbType.Int).Value = idPlanning)

            If dt.Rows.Count > 0 Then
                Dim dr As DataRow = dt.Rows(0)
                _impClient = LireDR(dr, "CodeClient") & " - " & LireDR(dr, "NomClient")
                _impChantier = LireDR(dr, "CodeChantier") & " - " & LireDR(dr, "NomChantier")
                _impFormule = LireDR(dr, "CodeFormule") : _impGamme = LireDR(dr, "Gamme")
                _impTempsMalaxage = LireDR(dr, "TempsMalaxage") : _impMES = LireDR(dr, "MES")
                _impVolumeAir = LireDR(dr, "VolumeAir") : _impTauxChlorure = LireDR(dr, "TauxChlorureCalc")
                _impMasseAlcalin = LireDR(dr, "MasseAlcalinCalc")
                _impDateLivraison = If(IsDBNull(dr("DatePlanning")), "", CDate(dr("DatePlanning")).ToString("dd/MM/yyyy"))
                _impHeureFabrication = If(IsDBNull(dr("HeureFabrication")), "", CType(dr("HeureFabrication"), TimeSpan).ToString("hh\:mm"))
                _impHeureLivraison = If(IsDBNull(dr("HeureLivraison")), "", CType(dr("HeureLivraison"), TimeSpan).ToString("hh\:mm"))
                _impQuantiteTotale = If(IsDBNull(dr("QuantiteTotale")), "0.00", Convert.ToDecimal(dr("QuantiteTotale")).ToString("0.00"))
                _impQuantiteLivree = If(IsDBNull(dr("QuantiteLivree")), "0.00", Convert.ToDecimal(dr("QuantiteLivree")).ToString("0.00"))
                _impReste = If(IsDBNull(dr("Reste")), "0.00", Convert.ToDecimal(dr("Reste")).ToString("0.00"))
                _impCadence = LireDR(dr, "CadenceMinutes")
                _impVolumeRotation = If(IsDBNull(dr("VolumeRotationMaxi")), "0.00", Convert.ToDecimal(dr("VolumeRotationMaxi")).ToString("0.00"))
                _impComplement = If(Not IsDBNull(dr("Complement")) AndAlso Convert.ToBoolean(dr("Complement")), "OUI", "NON")
                _impAConfirmer = If(Not IsDBNull(dr("AConfirmer")) AndAlso Convert.ToBoolean(dr("AConfirmer")), "OUI", "NON")
                _impCamion = LireDR(dr, "CodeCamion") & " - " & LireDR(dr, "NomCamion")
                _impChauffeur = LireDR(dr, "CodeChauffeur") & " - " & LireDR(dr, "NomChauffeur")
                _impCapacite = If(IsDBNull(dr("Capacite")), "", Convert.ToDecimal(dr("Capacite")).ToString("0.00"))
                _impRemarque = LireDR(dr, "Remarque")
            End If

            _impLignesPS.Clear()
            Dim dtPS As DataTable = ModuleDb.Lire(
                "SELECT ps.CodeProduit, ps.NomProduit, pps.Quantite, pps.Unite " &
                "FROM dbo.Planning_ProduitsServices pps " &
                "JOIN dbo.ProduitsServices ps ON ps.IdProduit = pps.IdProduit " &
                "WHERE pps.IdPlanning=@IdPlanning ORDER BY pps.NumeroLigne",
                Sub(cmd) cmd.Parameters.Add("@IdPlanning", SqlDbType.Int).Value = idPlanning)
            For Each row As DataRow In dtPS.Rows
                _impLignesPS.Add({row("CodeProduit").ToString(), row("NomProduit").ToString(),
                                  row("Quantite").ToString(), row("Unite").ToString()})
            Next
        Catch ex As Exception
            MessageBox.Show("Erreur chargement impression : " & ex.Message)
        End Try
    End Sub

    Private Sub pdPlanning_PrintPage(sender As Object, e As PrintPageEventArgs) Handles pdPlanning.PrintPage
        Dim fontTitre As New Font("Arial", 16, FontStyle.Bold)
        Dim fontSousTitre As New Font("Arial", 9, FontStyle.Italic)
        Dim fontLabel As New Font("Arial", 9, FontStyle.Bold)
        Dim fontValeur As New Font("Arial", 9, FontStyle.Regular)
        Dim fontEntete As New Font("Arial", 10, FontStyle.Bold)
        Dim g As Graphics = e.Graphics
        Dim x As Integer = 40 : Dim y As Integer = 30
        Dim col1 As Integer = 160 : Dim col2 As Integer = 430 : Dim col3 As Integer = 570
        Dim espace As Integer = 22

        g.FillRectangle(New SolidBrush(Color.FromArgb(10, 50, 120)), x, y, 720, 75)
        g.DrawString(SocieteNom, New Font("Arial", 13, FontStyle.Bold), Brushes.White, x + 10, y + 5)
        g.DrawString(SocieteAdresse, New Font("Arial", 8), Brushes.LightGray, x + 10, y + 25)
        g.DrawString("Tél : " & SocieteTelephone & "   Fax : " & SocieteFax, New Font("Arial", 8), Brushes.LightGray, x + 10, y + 40)
        g.DrawString("ICE : " & SocieteICE, New Font("Arial", 8), Brushes.LightGray, x + 10, y + 55)
        g.DrawString("RC : " & SocieteRC, New Font("Arial", 8), Brushes.LightGray, x + 450, y + 25)
        g.DrawString("IF : " & SocieteIF, New Font("Arial", 8), Brushes.LightGray, x + 450, y + 40)
        g.DrawString("Patente : " & SocietePatente, New Font("Arial", 8), Brushes.LightGray, x + 450, y + 55)
        y += 85

        g.DrawString("BON DE LIVRAISON", fontTitre, Brushes.DarkBlue, 200, y) : y += 28
        g.DrawString("Imprimé le : " & DateTime.Now.ToString("dd/MM/yyyy HH:mm"), fontSousTitre, Brushes.Gray, x, y) : y += 20
        g.DrawLine(New Pen(Color.DarkBlue, 2), x, y, 760, y) : y += 12

        DessinerEntete(g, fontEntete, "CLIENT / CHANTIER", x, y, 720) : y += 22
        g.DrawString("Client :", fontLabel, Brushes.Black, x + 5, y)
        g.DrawString(_impClient, fontValeur, Brushes.Black, col1, y) : y += espace
        g.DrawString("Chantier :", fontLabel, Brushes.Black, x + 5, y)
        g.DrawString(_impChantier, fontValeur, Brushes.Black, col1, y) : y += 28

        DessinerEntete(g, fontEntete, "FORMULE", x, y, 720) : y += 22
        g.DrawString("Code :", fontLabel, Brushes.Black, x + 5, y)
        g.DrawString(_impFormule, fontValeur, Brushes.Black, col1, y)
        g.DrawString("Gamme :", fontLabel, Brushes.Black, col2, y)
        g.DrawString(_impGamme, fontValeur, Brushes.Black, col3, y) : y += espace
        g.DrawString("Temps Malaxage :", fontLabel, Brushes.Black, x + 5, y)
        g.DrawString(_impTempsMalaxage, fontValeur, Brushes.Black, col1, y)
        g.DrawString("Volume Air :", fontLabel, Brushes.Black, col2, y)
        g.DrawString(_impVolumeAir, fontValeur, Brushes.Black, col3, y) : y += espace
        g.DrawString("MES :", fontLabel, Brushes.Black, x + 5, y)
        g.DrawString(_impMES, fontValeur, Brushes.Black, col1, y)
        g.DrawString("Taux Chlorure :", fontLabel, Brushes.Black, col2, y)
        g.DrawString(_impTauxChlorure, fontValeur, Brushes.Black, col3, y) : y += 28

        DessinerEntete(g, fontEntete, "PLANNING", x, y, 720) : y += 22
        g.DrawString("Date Livraison :", fontLabel, Brushes.Black, x + 5, y)
        g.DrawString(_impDateLivraison, fontValeur, Brushes.Black, col1, y)
        g.DrawString("Heure Fabrication :", fontLabel, Brushes.Black, col2, y)
        g.DrawString(_impHeureFabrication, fontValeur, Brushes.Black, col3, y) : y += espace
        g.DrawString("Quantité Totale :", fontLabel, Brushes.Black, x + 5, y)
        g.DrawString(_impQuantiteTotale & " M3", fontValeur, Brushes.Black, col1, y)
        g.DrawString("Heure Livraison :", fontLabel, Brushes.Black, col2, y)
        g.DrawString(_impHeureLivraison, fontValeur, Brushes.Black, col3, y) : y += espace
        g.DrawString("Déjà Livré :", fontLabel, Brushes.Black, x + 5, y)
        g.DrawString(_impQuantiteLivree & " M3", fontValeur, Brushes.Black, col1, y)
        g.DrawString("Reste :", fontLabel, Brushes.Black, col2, y)
        g.DrawString(_impReste & " M3", fontValeur, Brushes.Black, col3, y) : y += espace
        g.DrawString("Cadence :", fontLabel, Brushes.Black, x + 5, y)
        g.DrawString(_impCadence & " min", fontValeur, Brushes.Black, col1, y)
        g.DrawString("Vol. Rotation Maxi :", fontLabel, Brushes.Black, col2, y)
        g.DrawString(_impVolumeRotation & " M3", fontValeur, Brushes.Black, col3, y) : y += espace
        g.DrawString("Complément :", fontLabel, Brushes.Black, x + 5, y)
        g.DrawString(_impComplement, fontValeur, Brushes.Black, col1, y)
        g.DrawString("A Confirmer :", fontLabel, Brushes.Black, col2, y)
        g.DrawString(_impAConfirmer, fontValeur, Brushes.Black, col3, y) : y += 28

        DessinerEntete(g, fontEntete, "CAMION / CHAUFFEUR", x, y, 720) : y += 22
        g.DrawString("Camion :", fontLabel, Brushes.Black, x + 5, y)
        g.DrawString(_impCamion & "  (" & _impCapacite & " M3)", fontValeur, Brushes.Black, col1, y) : y += espace
        g.DrawString("Chauffeur :", fontLabel, Brushes.Black, x + 5, y)
        g.DrawString(_impChauffeur, fontValeur, Brushes.Black, col1, y) : y += 28

        If _impLignesPS.Count > 0 Then
            DessinerEntete(g, fontEntete, "PRODUITS / SERVICES", x, y, 720) : y += 22
            g.DrawString("Code", fontLabel, Brushes.Black, x + 5, y)
            g.DrawString("Nom", fontLabel, Brushes.Black, x + 120, y)
            g.DrawString("Quantité", fontLabel, Brushes.Black, x + 480, y)
            g.DrawString("Unité", fontLabel, Brushes.Black, x + 580, y) : y += espace
            g.DrawLine(Pens.LightGray, x, y - 5, 760, y - 5)
            For Each ligne In _impLignesPS
                g.DrawString(ligne(0), fontValeur, Brushes.Black, x + 5, y)
                g.DrawString(ligne(1), fontValeur, Brushes.Black, x + 120, y)
                g.DrawString(ligne(2), fontValeur, Brushes.Black, x + 480, y)
                g.DrawString(ligne(3), fontValeur, Brushes.Black, x + 580, y)
                y += espace
            Next : y += 10
        End If

        If _impRemarque <> "" Then
            DessinerEntete(g, fontEntete, "REMARQUE", x, y, 720) : y += 22
            g.DrawString(_impRemarque, fontValeur, Brushes.Black, x + 5, y) : y += 25
        End If

        y += 20
        g.DrawString("Préparé par : ___________________", fontLabel, Brushes.Black, x, y)
        g.DrawString("Approuvé par : ___________________", fontLabel, Brushes.Black, 430, y)
        g.DrawLine(New Pen(Color.FromArgb(10, 50, 120), 2), x, 790, 760, 790)
        g.FillRectangle(New SolidBrush(Color.FromArgb(10, 50, 120)), x, 792, 720, 16)
        g.DrawString(SocieteNom & "  |  " & SocieteAdresse & "  |  Tél : " & SocieteTelephone & "  |  ICE : " & SocieteICE,
                     New Font("Arial", 7), Brushes.White, x + 5, 795)
    End Sub

    Private Sub DessinerEntete(g As Graphics, font As Font, titre As String, x As Integer, y As Integer, largeur As Integer)
        g.FillRectangle(New SolidBrush(Color.FromArgb(200, 215, 255)), x, y, largeur, 20)
        g.DrawRectangle(Pens.DarkBlue, x, y, largeur, 20)
        g.DrawString(titre, font, Brushes.DarkBlue, x + 5, y + 2)
    End Sub

    Private Function VerifierSelectionLigne() As Boolean
        If DataGridViewPlanning.CurrentRow Is Nothing Then
            MessageBox.Show("Sélectionner une ligne.") : Return False
        End If
        Return True
    End Function

    Private Function LireIdLigneSelectionnee() As Integer
        Return CInt(DataGridViewPlanning.CurrentRow.Cells("N°").Value)
    End Function

    Private Function LireDR(dr As DataRow, col As String) As String
        Return If(IsDBNull(dr(col)), "", dr(col).ToString())
    End Function

End Class