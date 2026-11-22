Imports Microsoft.Data.SqlClient
Imports System.Data

Public Class Planification_Demands

    Public IdPlanningEnCours As Integer = 0
    Private chargementFormule As Boolean = False

    Private Sub FormClient_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        ViderTout()
    End Sub

    Private Sub Planification_Demands_Shown(sender As Object, e As EventArgs) Handles Me.Shown
        ChargerPageApresAffichage()
    End Sub

    Private Sub ChargerPageApresAffichage()
        Try
            Me.Cursor = Cursors.WaitCursor
            ChargerUnitesPS()
            If IdPlanningEnCours > 0 Then ChargerPlanning(IdPlanningEnCours)
        Catch ex As Exception
            MessageBox.Show("Erreur chargement : " & ex.Message)
        Finally
            Me.Cursor = Cursors.Default
        End Try
    End Sub

    Private Sub ViderTout()
        txtIdClient.Text = "" : txtCodeClient.Text = "" : txtNomClient.Text = ""
        txtIdChantier.Text = "" : txtCodeChantier.Text = "" : txtNomChantier.Text = ""
        txtCodeFormule.Text = "" : txtNomFormule.Text = ""
        ViderDetailsFormule()
        txtCodeCamion.Text = "" : txtNomCamion.Text = "" : txtCapaciteCamion.Text = ""
        txtCodeChauffeur.Text = "" : txtNomChauffeur.Text = ""
        txtCodeEquipementService.Text = "" : txtNomEquipementService.Text = "" : txtQuantiteService.Text = ""
        txtTotalChantier.Text = "" : txtDejaLivres.Text = "" : txtDejaLivres.ReadOnly = True
        txtCadence.Text = "" : txtVolumeRotationMaxi.Text = ""
        chkComplement.Checked = False : chkAConfirmer.Checked = False
        dtpDateLivraison.Format = DateTimePickerFormat.Short
        dtpDateLivraison.Value = Date.Today
        ViderLignesPS()
    End Sub

    Private Sub ViderDetailsFormule()
        txtGamme.Clear() : txtTempsMalaxage.Clear() : txtMES.Clear()
        txtVolumeAir.Clear() : txtTauxChlorureCalc.Clear() : txtMasseAlcalinCalc.Clear()
        DataGridViewCompositionPlanning.DataSource = Nothing
    End Sub

    Private Sub ViderLignesPS()
        For Each t In {txtCodePS1, txtCodePS2, txtCodePS3, txtCodePS4, txtCodePS5, txtCodePS6, txtCodePS7}
            t.Text = ""
        Next
        For Each t In {txtNomPS1, txtNomPS2, txtNomPS3, txtNomPS4, txtNomPS5, txtNomPS6, txtNomPS7}
            t.Text = ""
        Next
        For Each t In {txtQtePS1, txtQtePS2, txtQtePS3, txtQtePS4, txtQtePS5, txtQtePS6, txtQtePS7}
            t.Text = ""
        Next
        For Each c In {cmbUnitePS1, cmbUnitePS2, cmbUnitePS3, cmbUnitePS4, cmbUnitePS5, cmbUnitePS6, cmbUnitePS7}
            c.Text = ""
        Next
    End Sub

    Private Sub ChargerUnitesPS()
        Try
            Dim dt As DataTable = ModuleDb.Lire("SELECT NomUnite FROM dbo.UnitesPS ORDER BY NomUnite")
            For Each cmb As ComboBox In {cmbUnitePS1, cmbUnitePS2, cmbUnitePS3, cmbUnitePS4, cmbUnitePS5, cmbUnitePS6, cmbUnitePS7}
                cmb.DataSource = dt.Copy()
                cmb.DisplayMember = "NomUnite"
                cmb.ValueMember = "NomUnite"
                cmb.SelectedIndex = -1
            Next
        Catch ex As Exception
            MessageBox.Show("Erreur chargement unités : " & ex.Message)
        End Try
    End Sub

    Private Sub txtIdClient_Leave(sender As Object, e As EventArgs) Handles txtIdClient.Leave
        If txtIdClient.Text.Trim = "" Then
            txtCodeClient.Text = "" : txtNomClient.Text = ""
            txtIdChantier.Text = "" : txtCodeChantier.Text = "" : txtNomChantier.Text = ""
            Exit Sub
        End If
        Dim id As Integer
        If Not Integer.TryParse(txtIdClient.Text.Trim, id) Then Exit Sub
        Try
            Dim dt As DataTable = ModuleDb.Lire(
                "SELECT CodeClient, NomClient FROM dbo.Clients WHERE IdClient=@Id",
                Sub(cmd) cmd.Parameters.Add("@Id", SqlDbType.Int).Value = id)
            If dt.Rows.Count > 0 Then
                txtCodeClient.Text = dt.Rows(0)("CodeClient").ToString()
                txtNomClient.Text = dt.Rows(0)("NomClient").ToString()
            Else
                MessageBox.Show("Client introuvable.")
                txtCodeClient.Text = "" : txtNomClient.Text = ""
            End If
        Catch ex As Exception
            MessageBox.Show("Erreur client : " & ex.Message)
        End Try
    End Sub

    Private Sub txtIdChantier_Leave(sender As Object, e As EventArgs) Handles txtIdChantier.Leave
        If txtIdChantier.Text.Trim = "" Then
            txtCodeChantier.Text = "" : txtNomChantier.Text = "" : Exit Sub
        End If
        Dim id As Integer
        If Not Integer.TryParse(txtIdChantier.Text.Trim, id) Then Exit Sub
        Try
            Dim dt As DataTable = ModuleDb.Lire(
                "SELECT CodeChantier, NomChantier FROM dbo.Chantier WHERE IdChantier=@Id",
                Sub(cmd) cmd.Parameters.Add("@Id", SqlDbType.Int).Value = id)
            If dt.Rows.Count > 0 Then
                txtCodeChantier.Text = dt.Rows(0)("CodeChantier").ToString()
                txtNomChantier.Text = dt.Rows(0)("NomChantier").ToString()
            Else
                MessageBox.Show("Chantier introuvable.")
                txtCodeChantier.Text = "" : txtNomChantier.Text = ""
            End If
        Catch ex As Exception
            MessageBox.Show("Erreur chantier : " & ex.Message)
        End Try
    End Sub

    Private Sub txtCodeFormule_Leave(sender As Object, e As EventArgs) Handles txtCodeFormule.Leave
        If txtCodeFormule.Text.Trim = "" Then
            txtNomFormule.Text = "" : ViderDetailsFormule() : Exit Sub
        End If
        RechercherFormuleParCode(txtCodeFormule.Text.Trim)
    End Sub

    Private Sub RechercherFormuleParCode(code As String)
        Try
            Dim dt As DataTable = ModuleDb.Lire(
                "SELECT IdFormule, NomFormule, Gamme, TempsMalaxage, MES, " &
                "VolumeAir, TauxChlorureCalc, MasseAlcalinCalc " &
                "FROM dbo.Formules WHERE CodeFormule=@Code",
                Sub(cmd) cmd.Parameters.Add("@Code", SqlDbType.NVarChar, 50).Value = code)
            If dt.Rows.Count > 0 Then
                Dim dr As DataRow = dt.Rows(0)
                txtNomFormule.Text = dr("NomFormule").ToString()
                txtGamme.Text = dr("Gamme").ToString()
                txtTempsMalaxage.Text = dr("TempsMalaxage").ToString()
                txtMES.Text = dr("MES").ToString()
                txtVolumeAir.Text = dr("VolumeAir").ToString()
                txtTauxChlorureCalc.Text = dr("TauxChlorureCalc").ToString()
                txtMasseAlcalinCalc.Text = dr("MasseAlcalinCalc").ToString()
                ChargerCompositionFormule(CInt(dr("IdFormule")))
            Else
                MessageBox.Show("Formule introuvable : " & code)
                txtNomFormule.Text = "" : ViderDetailsFormule()
            End If
        Catch ex As Exception
            MessageBox.Show("Erreur formule : " & ex.Message)
        End Try
    End Sub

    Private Sub ChargerCompositionFormule(idFormule As Integer)
        Try
            Dim dt As DataTable = ModuleDb.Lire(
                "SELECT TypeMateriau, CodeMateriau, Quantite, Unite, Consigne " &
                "FROM dbo.FormuleComposition WHERE IdFormule=@IdFormule ORDER BY IdComposition",
                Sub(cmd) cmd.Parameters.Add("@IdFormule", SqlDbType.Int).Value = idFormule)
            With DataGridViewCompositionPlanning
                .DataSource = dt
                .ReadOnly = True
                .AllowUserToAddRows = False
                .SelectionMode = DataGridViewSelectionMode.FullRowSelect
                .AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill
                If .Columns.Count > 0 Then
                    .Columns("TypeMateriau").HeaderText = "Type"
                    .Columns("CodeMateriau").HeaderText = "Matériau"
                    .Columns("Quantite").HeaderText = "Quantité"
                    .Columns("Unite").HeaderText = "Unité"
                    .Columns("Consigne").HeaderText = "Consigne"
                End If
            End With
        Catch ex As Exception
            MessageBox.Show("Erreur composition : " & ex.Message)
        End Try
    End Sub

    Private Sub txtCodeCamion_Leave(sender As Object, e As EventArgs) Handles txtCodeCamion.Leave
        If txtCodeCamion.Text.Trim = "" Then
            txtNomCamion.Text = "" : txtCapaciteCamion.Text = "" : Exit Sub
        End If
        Try
            Dim dt As DataTable = ModuleDb.Lire(
                "SELECT NomCamion, Capacite FROM dbo.Camions WHERE CodeCamion=@Code",
                Sub(cmd) cmd.Parameters.Add("@Code", SqlDbType.NVarChar, 50).Value = txtCodeCamion.Text.Trim())
            If dt.Rows.Count > 0 Then
                txtNomCamion.Text = dt.Rows(0)("NomCamion").ToString()
                txtCapaciteCamion.Text = Convert.ToDecimal(dt.Rows(0)("Capacite")).ToString("0.00")
            Else
                MessageBox.Show("Camion introuvable : " & txtCodeCamion.Text)
                txtNomCamion.Text = "" : txtCapaciteCamion.Text = ""
            End If
        Catch ex As Exception
            MessageBox.Show("Erreur camion : " & ex.Message)
        End Try
    End Sub

    Private Sub txtCodeChauffeur_Leave(sender As Object, e As EventArgs) Handles txtCodeChauffeur.Leave
        If txtCodeChauffeur.Text.Trim = "" Then txtNomChauffeur.Text = "" : Exit Sub
        Try
            Dim dt As DataTable = ModuleDb.Lire(
                "SELECT NomChauffeur FROM dbo.Chauffeurs WHERE CodeChauffeur=@Code",
                Sub(cmd) cmd.Parameters.Add("@Code", SqlDbType.NVarChar, 50).Value = txtCodeChauffeur.Text.Trim())
            If dt.Rows.Count > 0 Then
                txtNomChauffeur.Text = dt.Rows(0)("NomChauffeur").ToString()
            Else
                MessageBox.Show("Chauffeur introuvable : " & txtCodeChauffeur.Text)
                txtNomChauffeur.Text = ""
            End If
        Catch ex As Exception
            MessageBox.Show("Erreur chauffeur : " & ex.Message)
        End Try
    End Sub

    Private Sub txtCodeEquipementService_Leave(sender As Object, e As EventArgs) Handles txtCodeEquipementService.Leave
        If txtCodeEquipementService.Text.Trim = "" Then txtNomEquipementService.Text = "" : Exit Sub
        Try
            Dim dt As DataTable = ModuleDb.Lire(
                "SELECT NomProduit FROM dbo.ProduitsServices WHERE CodeProduit=@Code",
                Sub(cmd) cmd.Parameters.Add("@Code", SqlDbType.NVarChar, 50).Value = txtCodeEquipementService.Text.Trim())
            txtNomEquipementService.Text = If(dt.Rows.Count > 0, dt.Rows(0)("NomProduit").ToString(), "")
        Catch ex As Exception
            MessageBox.Show("Erreur équipement : " & ex.Message)
        End Try
    End Sub

    Private Sub RechercherPSParCode(txtCode As TextBox, txtNom As TextBox)
        If txtCode.Text.Trim = "" Then txtNom.Text = "" : Exit Sub
        Try
            Dim dt As DataTable = ModuleDb.Lire(
                "SELECT NomProduit FROM dbo.ProduitsServices WHERE CodeProduit=@Code",
                Sub(cmd) cmd.Parameters.Add("@Code", SqlDbType.NVarChar, 50).Value = txtCode.Text.Trim())
            If dt.Rows.Count > 0 Then
                txtNom.Text = dt.Rows(0)("NomProduit").ToString()
            Else
                MessageBox.Show("Produit introuvable : " & txtCode.Text)
                txtNom.Text = ""
            End If
        Catch ex As Exception
            MessageBox.Show("Erreur produit/service : " & ex.Message)
        End Try
    End Sub

    Private Sub ChargerPlanning(idPlanning As Integer)
        Try
            Dim dt As DataTable = ModuleDb.Lire(
                "SELECT p.IdClient, p.IdChantier, p.IdFormule, p.IdCamion, p.IdChauffeur, " &
                "p.QuantiteTotale, p.QuantiteLivree, p.Complement, p.AConfirmer, " &
                "p.DatePlanning, p.HeureFabrication, p.HeureLivraison, " &
                "p.CadenceMinutes, p.VolumeRotationMaxi, " &
                "cl.CodeClient, cl.NomClient, ch.CodeChantier, ch.NomChantier, " &
                "f.CodeFormule, f.NomFormule, f.Gamme, f.TempsMalaxage, f.MES, " &
                "f.VolumeAir, f.TauxChlorureCalc, f.MasseAlcalinCalc, " &
                "ca.CodeCamion, ca.NomCamion, ca.Capacite, " &
                "cf.CodeChauffeur, cf.NomChauffeur " &
                "FROM dbo.Planning_journalier p " &
                "LEFT JOIN dbo.Clients cl ON cl.IdClient = p.IdClient " &
                "LEFT JOIN dbo.Chantier ch ON ch.IdChantier = p.IdChantier " &
                "LEFT JOIN dbo.Formules f ON f.IdFormule = p.IdFormule " &
                "LEFT JOIN dbo.Camions ca ON ca.IdCamion = p.IdCamion " &
                "LEFT JOIN dbo.Chauffeurs cf ON cf.IdChauffeur = p.IdChauffeur " &
                "WHERE p.IdPlanning=@IdPlanning",
                Sub(cmd) cmd.Parameters.Add("@IdPlanning", SqlDbType.Int).Value = idPlanning)

            If dt.Rows.Count = 0 Then Exit Sub
            Dim dr As DataRow = dt.Rows(0)

            txtIdClient.Text = LireDR(dr, "IdClient")
            txtCodeClient.Text = LireDR(dr, "CodeClient")
            txtNomClient.Text = LireDR(dr, "NomClient")
            txtIdChantier.Text = LireDR(dr, "IdChantier")
            txtCodeChantier.Text = LireDR(dr, "CodeChantier")
            txtNomChantier.Text = LireDR(dr, "NomChantier")
            txtCodeFormule.Text = LireDR(dr, "CodeFormule")
            txtNomFormule.Text = LireDR(dr, "NomFormule")
            txtGamme.Text = LireDR(dr, "Gamme")
            txtTempsMalaxage.Text = LireDR(dr, "TempsMalaxage")
            txtMES.Text = LireDR(dr, "MES")
            txtVolumeAir.Text = LireDR(dr, "VolumeAir")
            txtTauxChlorureCalc.Text = LireDR(dr, "TauxChlorureCalc")
            txtMasseAlcalinCalc.Text = LireDR(dr, "MasseAlcalinCalc")
            txtCodeCamion.Text = LireDR(dr, "CodeCamion")
            txtNomCamion.Text = LireDR(dr, "NomCamion")
            txtCapaciteCamion.Text = If(IsDBNull(dr("Capacite")), "", Convert.ToDecimal(dr("Capacite")).ToString("0.00"))
            txtCodeChauffeur.Text = LireDR(dr, "CodeChauffeur")
            txtNomChauffeur.Text = LireDR(dr, "NomChauffeur")
            txtTotalChantier.Text = If(IsDBNull(dr("QuantiteTotale")), "0.00", Convert.ToDecimal(dr("QuantiteTotale")).ToString("0.00"))
            ' DejaLivres — lecture seule depuis SQL, jamais ecrase par Valider
            txtDejaLivres.Text = If(IsDBNull(dr("QuantiteLivree")), "0.00", Convert.ToDecimal(dr("QuantiteLivree")).ToString("0.00"))
            chkComplement.Checked = Not IsDBNull(dr("Complement")) AndAlso Convert.ToBoolean(dr("Complement"))
            chkAConfirmer.Checked = Not IsDBNull(dr("AConfirmer")) AndAlso Convert.ToBoolean(dr("AConfirmer"))
            If Not IsDBNull(dr("DatePlanning")) Then dtpDateLivraison.Value = Convert.ToDateTime(dr("DatePlanning"))
            txtHeureFabrication.Text = If(IsDBNull(dr("HeureFabrication")), "", CType(dr("HeureFabrication"), TimeSpan).ToString("hh\:mm"))
            txtHeureLivraison.Text = If(IsDBNull(dr("HeureLivraison")), "", CType(dr("HeureLivraison"), TimeSpan).ToString("hh\:mm"))
            txtCadence.Text = LireDR(dr, "CadenceMinutes")
            txtVolumeRotationMaxi.Text = If(IsDBNull(dr("VolumeRotationMaxi")), "0.00", Convert.ToDecimal(dr("VolumeRotationMaxi")).ToString("0.00"))

            Dim idFormule As Integer = If(IsDBNull(dr("IdFormule")), 0, CInt(dr("IdFormule")))
            If idFormule > 0 Then ChargerCompositionFormule(idFormule)
            ChargerLignesPS(idPlanning)
        Catch ex As Exception
            MessageBox.Show("Erreur chargement planning : " & ex.Message)
        End Try
    End Sub

    Private Sub ChargerLignesPS(idPlanning As Integer)
        Try
            Dim dt As DataTable = ModuleDb.Lire(
                "SELECT pps.NumeroLigne, ps.CodeProduit, ps.NomProduit, pps.Quantite, pps.Unite " &
                "FROM dbo.Planning_ProduitsServices pps " &
                "JOIN dbo.ProduitsServices ps ON ps.IdProduit = pps.IdProduit " &
                "WHERE pps.IdPlanning=@IdPlanning ORDER BY pps.NumeroLigne",
                Sub(cmd) cmd.Parameters.Add("@IdPlanning", SqlDbType.Int).Value = idPlanning)

            ViderLignesPS()
            Dim codes = {txtCodePS1, txtCodePS2, txtCodePS3, txtCodePS4, txtCodePS5, txtCodePS6, txtCodePS7}
            Dim noms = {txtNomPS1, txtNomPS2, txtNomPS3, txtNomPS4, txtNomPS5, txtNomPS6, txtNomPS7}
            Dim qtes = {txtQtePS1, txtQtePS2, txtQtePS3, txtQtePS4, txtQtePS5, txtQtePS6, txtQtePS7}
            Dim unites = {cmbUnitePS1, cmbUnitePS2, cmbUnitePS3, cmbUnitePS4, cmbUnitePS5, cmbUnitePS6, cmbUnitePS7}

            For Each row As DataRow In dt.Rows
                Dim n As Integer = CInt(row("NumeroLigne")) - 1
                If n >= 0 AndAlso n <= 6 Then
                    codes(n).Text = row("CodeProduit").ToString()
                    noms(n).Text = row("NomProduit").ToString()
                    qtes(n).Text = row("Quantite").ToString()
                    unites(n).Text = row("Unite").ToString()
                End If
            Next
        Catch ex As Exception
            MessageBox.Show("Erreur lignes PS : " & ex.Message)
        End Try
    End Sub

    Private Sub Valider_Click(sender As Object, e As EventArgs) Handles Valider.Click
        If txtIdClient.Text.Trim = "" Then MessageBox.Show("Choisir un client.") : Exit Sub
        If txtIdChantier.Text.Trim = "" Then MessageBox.Show("Choisir un chantier.") : Exit Sub
        If txtCodeFormule.Text.Trim = "" Then MessageBox.Show("Choisir une formule.") : Exit Sub
        Try
            Dim idFormule As Integer = ModuleDb.LireValeur(Of Integer)(
                "SELECT IdFormule FROM dbo.Formules WHERE CodeFormule=@Code",
                Sub(cmd) cmd.Parameters.Add("@Code", SqlDbType.NVarChar, 50).Value = txtCodeFormule.Text.Trim())
            If idFormule = 0 Then MessageBox.Show("Formule invalide.") : Exit Sub

            Dim idCamion As Object = DBNull.Value
            If txtCodeCamion.Text.Trim <> "" Then
                Dim id As Integer = ModuleDb.LireValeur(Of Integer)(
                    "SELECT IdCamion FROM dbo.Camions WHERE CodeCamion=@Code",
                    Sub(cmd) cmd.Parameters.Add("@Code", SqlDbType.NVarChar, 50).Value = txtCodeCamion.Text.Trim())
                If id > 0 Then idCamion = id
            End If

            Dim idChauffeur As Object = DBNull.Value
            If txtCodeChauffeur.Text.Trim <> "" Then
                Dim id As Integer = ModuleDb.LireValeur(Of Integer)(
                    "SELECT IdChauffeur FROM dbo.Chauffeurs WHERE CodeChauffeur=@Code",
                    Sub(cmd) cmd.Parameters.Add("@Code", SqlDbType.NVarChar, 50).Value = txtCodeChauffeur.Text.Trim())
                If id > 0 Then idChauffeur = id
            End If

            Dim idPlanning As Integer = IdPlanningEnCours

            If IdPlanningEnCours = 0 Then
                ' ── NOUVEAU PLANNING — QuantiteLivree = 0 par defaut ──
                idPlanning = ModuleDb.LireValeur(Of Integer)(
                    "INSERT INTO dbo.Planning_journalier " &
                    "(IdClient, IdChantier, IdFormule, DatePlanning, HeureFabrication, HeureLivraison, " &
                    "QuantiteTotale, QuantiteLivree, Complement, AConfirmer, CadenceMinutes, VolumeRotationMaxi, " &
                    "IdCamion, IdChauffeur, QuantiteService, DateCreation) " &
                    "VALUES (@IdClient, @IdChantier, @IdFormule, @DatePlanning, @HeureFabrication, @HeureLivraison, " &
                    "@QuantiteTotale, 0, @Complement, @AConfirmer, @CadenceMinutes, @VolumeRotationMaxi, " &
                    "@IdCamion, @IdChauffeur, @QuantiteService, GETDATE()); SELECT CAST(SCOPE_IDENTITY() AS INT);",
                    Sub(cmd) RemplirParamsPlanning(cmd, idFormule, idCamion, idChauffeur))
            Else
                ' ── MODIFICATION — Ne jamais toucher QuantiteLivree ──
                ' QuantiteLivree est mis a jour uniquement par ModuleArchivageGachees.CloturerCommande()
                ModuleDb.Executer(
                    "UPDATE dbo.Planning_journalier SET " &
                    "IdClient=@IdClient, IdChantier=@IdChantier, IdFormule=@IdFormule, " &
                    "DatePlanning=@DatePlanning, HeureFabrication=@HeureFabrication, HeureLivraison=@HeureLivraison, " &
                    "QuantiteTotale=@QuantiteTotale, " &
                    "Complement=@Complement, AConfirmer=@AConfirmer, " &
                    "CadenceMinutes=@CadenceMinutes, VolumeRotationMaxi=@VolumeRotationMaxi, " &
                    "IdCamion=@IdCamion, IdChauffeur=@IdChauffeur, QuantiteService=@QuantiteService, " &
                    "DateModification=GETDATE() WHERE IdPlanning=@IdPlanning",
                    Sub(cmd)
                        RemplirParamsPlanning(cmd, idFormule, idCamion, idChauffeur)
                        cmd.Parameters.Add("@IdPlanning", SqlDbType.Int).Value = IdPlanningEnCours
                    End Sub)

                ModuleDb.Executer(
                    "DELETE FROM dbo.Planning_ProduitsServices WHERE IdPlanning=@Id",
                    Sub(cmd) cmd.Parameters.Add("@Id", SqlDbType.Int).Value = IdPlanningEnCours)
            End If

            Dim codes = {txtCodePS1, txtCodePS2, txtCodePS3, txtCodePS4, txtCodePS5, txtCodePS6, txtCodePS7}
            Dim qtes = {txtQtePS1, txtQtePS2, txtQtePS3, txtQtePS4, txtQtePS5, txtQtePS6, txtQtePS7}
            Dim unites = {cmbUnitePS1, cmbUnitePS2, cmbUnitePS3, cmbUnitePS4, cmbUnitePS5, cmbUnitePS6, cmbUnitePS7}

            For i As Integer = 0 To 6
                EnregistrerLignePS(idPlanning, i + 1, codes(i), qtes(i), unites(i))
            Next

            MessageBox.Show(If(IdPlanningEnCours = 0, "Planning enregistré avec succès.", "Planning modifié avec succès."))
            Me.Close()

        Catch ex As Exception
            MessageBox.Show("Erreur Valider : " & ex.Message)
        End Try
    End Sub

    ' RemplirParamsPlanning — sans @QuantiteLivree (gere par CloturerCommande)
    Private Sub RemplirParamsPlanning(cmd As SqlCommand, idFormule As Integer, idCamion As Object, idChauffeur As Object)
        cmd.Parameters.Add("@IdClient", SqlDbType.Int).Value = CInt(txtIdClient.Text)
        cmd.Parameters.Add("@IdChantier", SqlDbType.Int).Value = CInt(txtIdChantier.Text)
        cmd.Parameters.Add("@IdFormule", SqlDbType.Int).Value = idFormule
        cmd.Parameters.Add("@DatePlanning", SqlDbType.Date).Value = dtpDateLivraison.Value.Date
        cmd.Parameters.Add("@HeureFabrication", SqlDbType.Time).Value = ParseHeure(txtHeureFabrication.Text)
        cmd.Parameters.Add("@HeureLivraison", SqlDbType.Time).Value = ParseHeure(txtHeureLivraison.Text)
        cmd.Parameters.Add("@QuantiteTotale", SqlDbType.Decimal).Value = ParseDecimal(txtTotalChantier.Text)
        cmd.Parameters.Add("@Complement", SqlDbType.Bit).Value = chkComplement.Checked
        cmd.Parameters.Add("@AConfirmer", SqlDbType.Bit).Value = chkAConfirmer.Checked
        cmd.Parameters.Add("@CadenceMinutes", SqlDbType.Int).Value = ParseInt(txtCadence.Text)
        cmd.Parameters.Add("@VolumeRotationMaxi", SqlDbType.Decimal).Value = ParseDecimal(txtVolumeRotationMaxi.Text)
        cmd.Parameters.Add("@IdCamion", SqlDbType.Int).Value = idCamion
        cmd.Parameters.Add("@IdChauffeur", SqlDbType.Int).Value = idChauffeur
        cmd.Parameters.Add("@QuantiteService", SqlDbType.Decimal).Value = ParseDecimal(txtQuantiteService.Text)
    End Sub

    Private Sub EnregistrerLignePS(idPlanning As Integer, noLigne As Integer,
                                    txtCode As TextBox, txtQte As TextBox, cmbUnite As ComboBox)
        If txtCode.Text.Trim = "" Then Exit Sub
        Try
            Dim idProduit As Integer = ModuleDb.LireValeur(Of Integer)(
                "SELECT IdProduit FROM dbo.ProduitsServices WHERE CodeProduit=@Code",
                Sub(cmd) cmd.Parameters.Add("@Code", SqlDbType.NVarChar, 50).Value = txtCode.Text.Trim())
            If idProduit = 0 Then Exit Sub
            ModuleDb.Executer(
                "INSERT INTO dbo.Planning_ProduitsServices " &
                "(IdPlanning, NumeroLigne, IdProduit, Quantite, Unite) " &
                "VALUES (@IdPlanning, @NumeroLigne, @IdProduit, @Quantite, @Unite)",
                Sub(cmd)
                    cmd.Parameters.Add("@IdPlanning", SqlDbType.Int).Value = idPlanning
                    cmd.Parameters.Add("@NumeroLigne", SqlDbType.Int).Value = noLigne
                    cmd.Parameters.Add("@IdProduit", SqlDbType.Int).Value = idProduit
                    cmd.Parameters.Add("@Quantite", SqlDbType.Decimal).Value = ParseDecimal(txtQte.Text)
                    cmd.Parameters.Add("@Unite", SqlDbType.NVarChar, 50).Value = If(cmbUnite.Text.Trim = "", DBNull.Value, CObj(cmbUnite.Text.Trim()))
                End Sub)
        Catch ex As Exception
            MessageBox.Show("Erreur ligne PS : " & ex.Message)
        End Try
    End Sub

    Private Sub Abandonner_Click(sender As Object, e As EventArgs) Handles Abandonner.Click
        Me.Close()
    End Sub

    Private Sub btnJourPlus_Click(sender As Object, e As EventArgs) Handles btnJourPlus.Click
        dtpDateLivraison.Value = dtpDateLivraison.Value.AddDays(1)
    End Sub
    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click
        dtpDateLivraison.Value = dtpDateLivraison.Value.AddDays(-1)
    End Sub

    Private Function LireHeure(txt As TextBox) As TimeSpan
        Dim t As TimeSpan
        TimeSpan.TryParse(txt.Text, t)
        Return t
    End Function
    Private Sub AfficherHeure(txt As TextBox, t As TimeSpan)
        txt.Text = t.ToString("hh\:mm")
    End Sub

    Private Sub btnPlusHeureFabrication_Click(s As Object, e As EventArgs) Handles btnPlusHeureFabrication.Click
        Dim t = LireHeure(txtHeureFabrication).Add(New TimeSpan(1, 0, 0))
        AfficherHeure(txtHeureFabrication, If(t.TotalHours >= 24, t.Subtract(New TimeSpan(24, 0, 0)), t))
    End Sub
    Private Sub btnMoinsHeureFabrication_Click(s As Object, e As EventArgs) Handles btnMoinsHeureFabrication.Click
        Dim t = LireHeure(txtHeureFabrication).Subtract(New TimeSpan(1, 0, 0))
        AfficherHeure(txtHeureFabrication, If(t.TotalMinutes < 0, t.Add(New TimeSpan(24, 0, 0)), t))
    End Sub
    Private Sub btnPlusMinuteFabrication_Click(s As Object, e As EventArgs) Handles btnPlusMinuteFabrication.Click
        Dim t = LireHeure(txtHeureFabrication).Add(New TimeSpan(0, 1, 0))
        AfficherHeure(txtHeureFabrication, If(t.TotalHours >= 24, t.Subtract(New TimeSpan(24, 0, 0)), t))
    End Sub
    Private Sub btnMoinsMinuteFabrication_Click(s As Object, e As EventArgs) Handles btnMoinsMinuteFabrication.Click
        Dim t = LireHeure(txtHeureFabrication).Subtract(New TimeSpan(0, 1, 0))
        AfficherHeure(txtHeureFabrication, If(t.TotalMinutes < 0, t.Add(New TimeSpan(24, 0, 0)), t))
    End Sub
    Private Sub btnPlusHeureLivraison_Click(s As Object, e As EventArgs) Handles btnPlusHeureLivraison.Click
        Dim t = LireHeure(txtHeureLivraison).Add(New TimeSpan(1, 0, 0))
        AfficherHeure(txtHeureLivraison, If(t.TotalHours >= 24, t.Subtract(New TimeSpan(24, 0, 0)), t))
    End Sub
    Private Sub btnMoinsHeureLivraison_Click(s As Object, e As EventArgs) Handles btnMoinsHeureLivraison.Click
        Dim t = LireHeure(txtHeureLivraison).Subtract(New TimeSpan(1, 0, 0))
        AfficherHeure(txtHeureLivraison, If(t.TotalMinutes < 0, t.Add(New TimeSpan(24, 0, 0)), t))
    End Sub
    Private Sub btnPlusMinuteLivraison_Click(s As Object, e As EventArgs) Handles btnPlusMinuteLivraison.Click
        Dim t = LireHeure(txtHeureLivraison).Add(New TimeSpan(0, 1, 0))
        AfficherHeure(txtHeureLivraison, If(t.TotalHours >= 24, t.Subtract(New TimeSpan(24, 0, 0)), t))
    End Sub
    Private Sub btnMoinsMinuteLivraison_Click(s As Object, e As EventArgs) Handles btnMoinsMinuteLivraison.Click
        Dim t = LireHeure(txtHeureLivraison).Subtract(New TimeSpan(0, 1, 0))
        AfficherHeure(txtHeureLivraison, If(t.TotalMinutes < 0, t.Add(New TimeSpan(24, 0, 0)), t))
    End Sub
    Private Sub txtHeureFabrication_KeyPress(sender As Object, e As KeyPressEventArgs) Handles txtHeureFabrication.KeyPress, txtHeureLivraison.KeyPress
        If Not Char.IsControl(e.KeyChar) AndAlso Not Char.IsDigit(e.KeyChar) AndAlso e.KeyChar <> ":"c Then
            e.Handled = True
        End If
    End Sub

    Private Sub txtIdClient_DoubleClick(s As Object, e As EventArgs) Handles txtIdClient.DoubleClick, txtCodeClient.DoubleClick, txtNomClient.DoubleClick
        RechercheClient()
    End Sub
    Private Sub RechercheClient()
        Dim f As New FrmRecherche
        f.Initialiser("SELECT IdClient, CodeClient, NomClient, Telephone FROM dbo.Clients WHERE 1=1", "IdClient", "CodeClient", "NomClient")
        f.Text = "Recherche Client"
        If f.ShowDialog() = DialogResult.OK Then
            txtIdClient.Text = f.IdSelectionne.ToString()
            txtIdClient_Leave(Nothing, Nothing)
        End If
    End Sub

    Private Sub txtIdChantier_DoubleClick(s As Object, e As EventArgs) Handles txtIdChantier.DoubleClick, txtCodeChantier.DoubleClick, txtNomChantier.DoubleClick
        RechercheChantier()
    End Sub
    Private Sub RechercheChantier()
        If txtIdClient.Text.Trim = "" Then MessageBox.Show("Choisir d'abord un client.") : Exit Sub
        Dim f As New FrmRecherche
        f.Initialiser("SELECT IdChantier, CodeChantier, NomChantier FROM dbo.Chantier WHERE IdClient=" & txtIdClient.Text.Trim, "IdChantier", "CodeChantier", "NomChantier")
        f.Text = "Recherche Chantier"
        If f.ShowDialog() = DialogResult.OK Then
            txtIdChantier.Text = f.IdSelectionne.ToString()
            txtIdChantier_Leave(Nothing, Nothing)
        End If
    End Sub

    Private Sub txtCodeFormule_DoubleClick(s As Object, e As EventArgs) Handles txtCodeFormule.DoubleClick, txtNomFormule.DoubleClick
        RechercheFormule()
    End Sub
    Private Sub RechercheFormule()
        Dim f As New FrmRecherche
        f.Initialiser("SELECT IdFormule, CodeFormule, NomFormule FROM dbo.Formules WHERE FormuleInactive = 0 OR FormuleInactive IS NULL", "IdFormule", "CodeFormule", "NomFormule")
        f.Text = "Recherche Formule"
        If f.ShowDialog() = DialogResult.OK Then
            txtCodeFormule.Text = f.ValeurSelectionnee
            txtCodeFormule_Leave(Nothing, Nothing)
        End If
    End Sub

    Private Sub txtCodeCamion_DoubleClick(s As Object, e As EventArgs) Handles txtCodeCamion.DoubleClick, txtNomCamion.DoubleClick
        RechercheCamion()
    End Sub
    Private Sub RechercheCamion()
        Dim f As New FrmRecherche
        f.Initialiser("SELECT IdCamion, CodeCamion, NomCamion, Capacite FROM dbo.Camions WHERE 1=1", "IdCamion", "CodeCamion", "NomCamion")
        f.Text = "Recherche Camion"
        If f.ShowDialog() = DialogResult.OK Then
            txtCodeCamion.Text = f.ValeurSelectionnee
            txtCodeCamion_Leave(Nothing, Nothing)
        End If
    End Sub

    Private Sub txtCodeChauffeur_DoubleClick(s As Object, e As EventArgs) Handles txtCodeChauffeur.DoubleClick, txtNomChauffeur.DoubleClick
        RechercheChauffeur()
    End Sub
    Private Sub RechercheChauffeur()
        Dim f As New FrmRecherche
        f.Initialiser("SELECT IdChauffeur, CodeChauffeur, NomChauffeur FROM dbo.Chauffeurs WHERE Actif = 1 OR Actif IS NULL", "IdChauffeur", "CodeChauffeur", "NomChauffeur")
        f.Text = "Recherche Chauffeur"
        If f.ShowDialog() = DialogResult.OK Then
            txtCodeChauffeur.Text = f.ValeurSelectionnee
            txtCodeChauffeur_Leave(Nothing, Nothing)
        End If
    End Sub

    Private Sub txtCodeEquipementService_DoubleClick(s As Object, e As EventArgs) Handles txtCodeEquipementService.DoubleClick, txtNomEquipementService.DoubleClick
        RechercheEquipement()
    End Sub
    Private Sub RechercheEquipement()
        Dim f As New FrmRecherche
        f.Initialiser("SELECT IdProduit, CodeProduit, NomProduit FROM dbo.ProduitsServices WHERE Actif = 1 OR Actif IS NULL", "IdProduit", "CodeProduit", "NomProduit")
        f.Text = "Recherche Equipement Service"
        If f.ShowDialog() = DialogResult.OK Then
            txtCodeEquipementService.Text = f.ValeurSelectionnee
            txtCodeEquipementService_Leave(Nothing, Nothing)
        End If
    End Sub

    Private Sub RecherchePS(txtCode As TextBox, txtNom As TextBox)
        Dim f As New FrmRecherche
        f.Initialiser("SELECT IdProduit, CodeProduit, NomProduit FROM dbo.ProduitsServices WHERE Actif = 1 OR Actif IS NULL", "IdProduit", "CodeProduit", "NomProduit")
        f.Text = "Recherche Produit/Service"
        If f.ShowDialog() = DialogResult.OK Then
            txtCode.Text = f.ValeurSelectionnee
            RechercherPSParCode(txtCode, txtNom)
        End If
    End Sub

    Private Sub txtCodePS1_Leave(s As Object, e As EventArgs) Handles txtCodePS1.Leave
        RechercherPSParCode(txtCodePS1, txtNomPS1)
    End Sub
    Private Sub txtCodePS2_Leave(s As Object, e As EventArgs) Handles txtCodePS2.Leave
        RechercherPSParCode(txtCodePS2, txtNomPS2)
    End Sub
    Private Sub txtCodePS3_Leave(s As Object, e As EventArgs) Handles txtCodePS3.Leave
        RechercherPSParCode(txtCodePS3, txtNomPS3)
    End Sub
    Private Sub txtCodePS4_Leave(s As Object, e As EventArgs) Handles txtCodePS4.Leave
        RechercherPSParCode(txtCodePS4, txtNomPS4)
    End Sub
    Private Sub txtCodePS5_Leave(s As Object, e As EventArgs) Handles txtCodePS5.Leave
        RechercherPSParCode(txtCodePS5, txtNomPS5)
    End Sub
    Private Sub txtCodePS6_Leave(s As Object, e As EventArgs) Handles txtCodePS6.Leave
        RechercherPSParCode(txtCodePS6, txtNomPS6)
    End Sub
    Private Sub txtCodePS7_Leave(s As Object, e As EventArgs) Handles txtCodePS7.Leave
        RechercherPSParCode(txtCodePS7, txtNomPS7)
    End Sub

    Private Sub txtCodePS1_DoubleClick(s As Object, e As EventArgs) Handles txtCodePS1.DoubleClick, txtNomPS1.DoubleClick
        RecherchePS(txtCodePS1, txtNomPS1)
    End Sub
    Private Sub txtCodePS2_DoubleClick(s As Object, e As EventArgs) Handles txtCodePS2.DoubleClick, txtNomPS2.DoubleClick
        RecherchePS(txtCodePS2, txtNomPS2)
    End Sub
    Private Sub txtCodePS3_DoubleClick(s As Object, e As EventArgs) Handles txtCodePS3.DoubleClick, txtNomPS3.DoubleClick
        RecherchePS(txtCodePS3, txtNomPS3)
    End Sub
    Private Sub txtCodePS4_DoubleClick(s As Object, e As EventArgs) Handles txtCodePS4.DoubleClick, txtNomPS4.DoubleClick
        RecherchePS(txtCodePS4, txtNomPS4)
    End Sub
    Private Sub txtCodePS5_DoubleClick(s As Object, e As EventArgs) Handles txtCodePS5.DoubleClick, txtNomPS5.DoubleClick
        RecherchePS(txtCodePS5, txtNomPS5)
    End Sub
    Private Sub txtCodePS6_DoubleClick(s As Object, e As EventArgs) Handles txtCodePS6.DoubleClick, txtNomPS6.DoubleClick
        RecherchePS(txtCodePS6, txtNomPS6)
    End Sub
    Private Sub txtCodePS7_DoubleClick(s As Object, e As EventArgs) Handles txtCodePS7.DoubleClick, txtNomPS7.DoubleClick
        RecherchePS(txtCodePS7, txtNomPS7)
    End Sub

    Private Function ParseDecimal(txt As String) As Decimal
        Dim v As Decimal
        Decimal.TryParse(txt.Replace(",", "."), Globalization.NumberStyles.Any, Globalization.CultureInfo.InvariantCulture, v)
        Return v
    End Function
    Private Function ParseInt(txt As String) As Integer
        Dim v As Integer : Integer.TryParse(txt, v) : Return v
    End Function
    Private Function ParseHeure(txt As String) As Object
        If txt.Trim = "" Then Return DBNull.Value
        Dim t As TimeSpan
        If TimeSpan.TryParse(txt.Trim, t) Then Return t
        Return DBNull.Value
    End Function
    Private Function LireDR(dr As DataRow, col As String) As String
        Return If(IsDBNull(dr(col)), "", dr(col).ToString())
    End Function

    Private Sub txtChantier_TextChanged(s As Object, e As EventArgs) Handles txtIdChantier.TextChanged
    End Sub
    Private Sub GroupBox1_Enter(s As Object, e As EventArgs) Handles GroupBox1.Enter
    End Sub

End Class