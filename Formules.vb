Imports System.Drawing.Printing
Imports Microsoft.Data.SqlClient
Imports System.Data

Public Class Formules

    Dim idFormuleSelectionne As Integer = 0
    Dim idCompositionSelectionne As Integer = 0
    Dim modeEditionFormule As Boolean = False
    Dim modeEditionComposition As Boolean = False

    Private docImpression As New PrintDocument
    Private apercuImpression As New PrintPreviewDialog

    ' ═══════════════════════════════════════════════════════════════════
    '  LOAD
    ' ═══════════════════════════════════════════════════════════════════

    Private Sub Formules_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        DataGridViewFormules.RowHeadersVisible = False   ' NOUVEAU
        DataGridViewComposition.RowHeadersVisible = False   ' NOUVEAU

        ' NOUVEAU : gestion des rôles
        Select Case RoleUtilisateur
            Case "Admin", "Super"
                btnAjouterFormule.Visible = True : btnAjouterFormule.Enabled = True
                btnModifierFormule.Visible = True : btnModifierFormule.Enabled = True
                btnSupprimerFormule.Visible = True : btnSupprimerFormule.Enabled = True

                btnDupliquerFormule.Visible = True : btnDupliquerFormule.Enabled = True
                btnValiderFormule.Visible = True : btnValiderFormule.Enabled = True
                btnEnregistrerFormule.Visible = True : btnEnregistrerFormule.Enabled = True

                btnInsererComposition.Visible = True : btnInsererComposition.Enabled = False
                btnSupprimerComposition.Visible = True : btnSupprimerComposition.Enabled = False
                btnModifierComposition.Visible = True : btnModifierComposition.Enabled = False
            Case "Operateur"
                btnAjouterFormule.Visible = True : btnAjouterFormule.Enabled = False
                btnModifierFormule.Visible = True : btnModifierFormule.Enabled = False
                btnSupprimerFormule.Visible = True : btnSupprimerFormule.Enabled = False

                btnDupliquerFormule.Visible = True : btnDupliquerFormule.Enabled = False
                btnValiderFormule.Visible = True : btnValiderFormule.Enabled = False
                btnEnregistrerFormule.Visible = True : btnEnregistrerFormule.Enabled = False

                btnInsererComposition.Visible = True : btnInsererComposition.Enabled = False
                btnSupprimerComposition.Visible = True : btnSupprimerComposition.Enabled = False
                btnModifierComposition.Visible = True : btnModifierComposition.Enabled = False
            Case Else
                btnAjouterFormule.Visible = True : btnAjouterFormule.Enabled = False
                btnModifierFormule.Visible = True : btnModifierFormule.Enabled = False
                btnSupprimerFormule.Visible = True : btnSupprimerFormule.Enabled = False

                btnDupliquerFormule.Visible = True : btnDupliquerFormule.Enabled = False
                btnValiderFormule.Visible = True : btnValiderFormule.Enabled = False
                btnEnregistrerFormule.Visible = True : btnEnregistrerFormule.Enabled = False

                btnInsererComposition.Visible = True : btnInsererComposition.Enabled = False
                btnSupprimerComposition.Visible = True : btnSupprimerComposition.Enabled = False
                btnModifierComposition.Visible = True : btnModifierComposition.Enabled = False
        End Select

        cmbTypeMateriauFormule.Items.Clear()
        For Each t In {"GRANULAT", "LIANT", "EAU", "ADJUVANT", "ADDITION", "AJOUT"}
            cmbTypeMateriauFormule.Items.Add(t)
        Next

        cmbUniteFormule.Items.Clear()
        For Each u In {"kg", "L", "%", "m3"}
            cmbUniteFormule.Items.Add(u)
        Next

        ChargerClassesExposition()
        ChargerFormules()
        AddHandler docImpression.PrintPage, AddressOf docImpression_PrintPage
    End Sub

    ' ═══════════════════════════════════════════════════════════════════
    '  CLASSES EXPOSITION
    ' ═══════════════════════════════════════════════════════════════════

    Private Sub ChargerClassesExposition()
        Try
            Dim dt As DataTable = ModuleDb.Lire(
                "SELECT ClasseExposition FROM dbo.Normes ORDER BY ClasseExposition")
            cmbClasseExposition.DataSource = dt
            cmbClasseExposition.DisplayMember = "ClasseExposition"
            cmbClasseExposition.ValueMember = "ClasseExposition"
            cmbClasseExposition.SelectedIndex = -1
        Catch ex As Exception
            MessageBox.Show("Erreur chargement classes exposition : " & ex.Message)
        End Try
    End Sub

    Private Sub cmbClasseExposition_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cmbClasseExposition.SelectedIndexChanged
        ChargerNormeDansFormule(cmbClasseExposition.Text)
    End Sub

    Private Sub ChargerNormeDansFormule(classeExpo As String)
        If classeExpo.Trim() = "" Then Exit Sub
        Try
            Dim dt As DataTable = ModuleDb.Lire(
                "SELECT RapportECLimite, ClasseResistance FROM dbo.Normes WHERE ClasseExposition=@ClasseExposition",
                Sub(cmd) cmd.Parameters.Add("@ClasseExposition", SqlDbType.NVarChar, 100).Value = classeExpo)

            If dt.Rows.Count > 0 Then
                txtRapportEC.Text = dt.Rows(0)("RapportECLimite").ToString()
                cmbResistance.Text = dt.Rows(0)("ClasseResistance").ToString()
            End If
        Catch ex As Exception
            MessageBox.Show("Erreur chargement norme : " & ex.Message)
        End Try
    End Sub

    ' ═══════════════════════════════════════════════════════════════════
    '  FORMULES
    ' ═══════════════════════════════════════════════════════════════════

    Private Sub ChargerFormules()
        Try
            Dim dt As DataTable = ModuleDb.Lire(
            "SELECT IdFormule, CodeFormule, CleRecherche, NomFormule " &
            "FROM dbo.Formules ORDER BY IdFormule DESC")

            DataGridViewFormules.DataSource = dt
            DataGridViewFormules.ReadOnly = True
            DataGridViewFormules.SelectionMode = DataGridViewSelectionMode.FullRowSelect
            DataGridViewFormules.AllowUserToAddRows = False

            ' NOUVEAU : largeurs personnalisées au lieu de Fill
            DataGridViewFormules.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.None

            If DataGridViewFormules.Columns.Count > 0 Then
                DataGridViewFormules.Columns("IdFormule").Visible = False

                DataGridViewFormules.Columns("CodeFormule").HeaderText = "Code"
                DataGridViewFormules.Columns("CodeFormule").Width = 120

                DataGridViewFormules.Columns("CleRecherche").HeaderText = "Clé"
                DataGridViewFormules.Columns("CleRecherche").Width = 120

                DataGridViewFormules.Columns("NomFormule").HeaderText = "Nom"
                DataGridViewFormules.Columns("NomFormule").Width = 320
            End If
        Catch ex As Exception
            MessageBox.Show("Erreur chargement formules : " & ex.Message)
        End Try
    End Sub

    Private Sub DataGridViewFormules_CellClick(sender As Object, e As DataGridViewCellEventArgs) Handles DataGridViewFormules.CellClick
        If e.RowIndex >= 0 Then
            idFormuleSelectionne = Convert.ToInt32(DataGridViewFormules.Rows(e.RowIndex).Cells("IdFormule").Value)
            ChargerDetailsFormule(idFormuleSelectionne)
            ChargerCompositionFormule(idFormuleSelectionne)

            ' NOUVEAU : réactiver les boutons composition selon le rôle
            If RoleUtilisateur = "Admin" OrElse RoleUtilisateur = "Super" Then
                btnInsererComposition.Enabled = True
                btnSupprimerComposition.Enabled = True
                btnModifierComposition.Enabled = True
            End If
        End If
    End Sub

    Private Sub ChargerDetailsFormule(id As Integer)
        Try
            Dim dt As DataTable = ModuleDb.Lire(
                "SELECT * FROM dbo.Formules WHERE IdFormule=@IdFormule",
                Sub(cmd) cmd.Parameters.Add("@IdFormule", SqlDbType.Int).Value = id)

            If dt.Rows.Count = 0 Then Exit Sub
            Dim dr As DataRow = dt.Rows(0)

            txtCodeFormule.Text = dr("CodeFormule").ToString()
            txtCleRechercheFormule.Text = dr("CleRecherche").ToString()
            txtNomFormule.Text = dr("NomFormule").ToString()
            txtGamme.Text = dr("Gamme").ToString()
            txtTempsMalaxage.Text = dr("TempsMalaxage").ToString()
            txtMES.Text = dr("MES").ToString()
            txtVolumeAir.Text = dr("VolumeAir").ToString()
            txtTauxChlorureCalc.Text = dr("TauxChlorureCalc").ToString()
            txtMasseAlcalinCalc.Text = dr("MasseAlcalinCalc").ToString()
            cmbClasseExposition.Text = dr("ClasseExposition").ToString()
            cmbAdditionA.Text = dr("AdditionA").ToString()
            txtDesignation.Text = dr("Designation").ToString()
            cmbCertification.Text = dr("Certification").ToString()
            txtAttestation.Text = dr("Attestation").ToString()
            cmbNatureCiment.Text = dr("NatureCiment").ToString()
            cmbConsistance.Text = dr("Consistance").ToString()
            cmbGranulariteDmax.Text = dr("GranulariteDmax").ToString()
            cmbResistance.Text = dr("Resistance").ToString()
            cmbControleNorme.Text = dr("Controle").ToString()
            cmbClasseChlorure.Text = dr("ClasseChlorure").ToString()
            txtRapportEC.Text = dr("RapportEC").ToString()
            txtAdjuvants.Text = dr("Adjuvants").ToString()
            txtRemarqueNorme.Text = dr("RemarqueNorme").ToString()
        Catch ex As Exception
            MessageBox.Show("Erreur détail formule : " & ex.Message)
        End Try
    End Sub

    ' ═══════════════════════════════════════════════════════════════════
    '  ENREGISTRER FORMULE (INSERT ou UPDATE)
    ' ═══════════════════════════════════════════════════════════════════

    Private Sub btnEnregistrerFormule_Click(sender As Object, e As EventArgs) Handles btnEnregistrerFormule.Click
        If txtCodeFormule.Text.Trim() = "" Then
            MessageBox.Show("Le code formule est obligatoire.") : Exit Sub
        End If
        If txtNomFormule.Text.Trim() = "" Then
            MessageBox.Show("Le nom formule est obligatoire.") : Exit Sub
        End If
        Try
            ' ── Vérif doublon ──────────────────────────────────────
            Dim sqlVerif As String = If(Not modeEditionFormule,
                "SELECT COUNT(*) FROM dbo.Formules WHERE CodeFormule=@CodeFormule OR NomFormule=@NomFormule",
                "SELECT COUNT(*) FROM dbo.Formules WHERE (CodeFormule=@CodeFormule OR NomFormule=@NomFormule) AND IdFormule<>@IdFormule")

            Dim nb As Integer = ModuleDb.LireValeur(Of Integer)(sqlVerif,
                Sub(cmd)
                    cmd.Parameters.Add("@CodeFormule", SqlDbType.NVarChar, 50).Value = txtCodeFormule.Text.Trim()
                    cmd.Parameters.Add("@NomFormule", SqlDbType.NVarChar, 200).Value = txtNomFormule.Text.Trim()
                    If modeEditionFormule Then
                        cmd.Parameters.Add("@IdFormule", SqlDbType.Int).Value = idFormuleSelectionne
                    End If
                End Sub)

            If nb > 0 Then
                MessageBox.Show("Cette formule existe déjà (même code ou même nom).")
                Exit Sub
            End If

            If Not modeEditionFormule Then
                ' ── INSERT ─────────────────────────────────────────
                idFormuleSelectionne = ModuleDb.LireValeur(Of Integer)(
                    "INSERT INTO dbo.Formules " &
                    "(CodeFormule, CleRecherche, NomFormule, Gamme, TempsMalaxage, MES, VolumeAir, TauxChlorureCalc, MasseAlcalinCalc, " &
                    "ClasseExposition, AdditionA, Designation, Certification, Attestation, NatureCiment, Consistance, GranulariteDmax, " &
                    "Resistance, Controle, ClasseChlorure, RapportEC, Adjuvants, RemarqueNorme, DerniereModification) " &
                    "VALUES " &
                    "(@CodeFormule, @CleRecherche, @NomFormule, @Gamme, @TempsMalaxage, @MES, @VolumeAir, @TauxChlorureCalc, @MasseAlcalinCalc, " &
                    "@ClasseExposition, @AdditionA, @Designation, @Certification, @Attestation, @NatureCiment, @Consistance, @GranulariteDmax, " &
                    "@Resistance, @Controle, @ClasseChlorure, @RapportEC, @Adjuvants, @RemarqueNorme, GETDATE()); " &
                    "SELECT SCOPE_IDENTITY();",
                    AddressOf AjouterParamsFormule)

                MessageBox.Show("Formule ajoutée avec succès.")
            Else
                ' ── UPDATE ─────────────────────────────────────────
                ModuleDb.Executer(
                    "UPDATE dbo.Formules SET " &
                    "CodeFormule=@CodeFormule, CleRecherche=@CleRecherche, NomFormule=@NomFormule, " &
                    "Gamme=@Gamme, TempsMalaxage=@TempsMalaxage, MES=@MES, VolumeAir=@VolumeAir, " &
                    "TauxChlorureCalc=@TauxChlorureCalc, MasseAlcalinCalc=@MasseAlcalinCalc, " &
                    "ClasseExposition=@ClasseExposition, AdditionA=@AdditionA, Designation=@Designation, " &
                    "Certification=@Certification, Attestation=@Attestation, NatureCiment=@NatureCiment, " &
                    "Consistance=@Consistance, GranulariteDmax=@GranulariteDmax, Resistance=@Resistance, " &
                    "Controle=@Controle, ClasseChlorure=@ClasseChlorure, RapportEC=@RapportEC, " &
                    "Adjuvants=@Adjuvants, RemarqueNorme=@RemarqueNorme, DerniereModification=GETDATE() " &
                    "WHERE IdFormule=@IdFormule",
                    Sub(cmd)
                        AjouterParamsFormule(cmd)
                        cmd.Parameters.Add("@IdFormule", SqlDbType.Int).Value = idFormuleSelectionne
                    End Sub)

                MessageBox.Show("Formule modifiée avec succès.")
                modeEditionFormule = False
            End If

            ChargerFormules()

        Catch ex As Exception
            MessageBox.Show("Erreur enregistrement formule : " & ex.Message)
        End Try
    End Sub

    Private Sub btnAjouterFormule_Click(sender As Object, e As EventArgs) Handles btnAjouterFormule.Click
        ViderChampsFormule()
        txtCodeFormule.Focus()
    End Sub

    Private Sub btnModifierFormule_Click(sender As Object, e As EventArgs) Handles btnModifierFormule.Click
        If idFormuleSelectionne = 0 Then
            MessageBox.Show("Sélectionne d'abord une formule.")
            Exit Sub
        End If
        modeEditionFormule = True
        MessageBox.Show("Modifie les champs puis clique sur Enregistrer.")
    End Sub

    Private Sub btnActualiserFormule_Click(sender As Object, e As EventArgs) Handles btnActualiserFormule.Click
        ChargerFormules()
    End Sub

    ' ═══════════════════════════════════════════════════════════════════
    '  SUPPRIMER FORMULE
    ' ═══════════════════════════════════════════════════════════════════

    Private Sub btnSupprimerFormule_Click(sender As Object, e As EventArgs) Handles btnSupprimerFormule.Click
        If idFormuleSelectionne = 0 Then
            MessageBox.Show("Sélectionne d'abord une formule.")
            Exit Sub
        End If
        If MessageBox.Show("Voulez-vous supprimer cette formule ?", "Confirmation",
                           MessageBoxButtons.YesNo, MessageBoxIcon.Question) = DialogResult.No Then Exit Sub
        Try
            ModuleDb.Executer(
                "DELETE FROM dbo.FormuleComposition WHERE IdFormule=@Id",
                Sub(cmd) cmd.Parameters.Add("@Id", SqlDbType.Int).Value = idFormuleSelectionne)

            ModuleDb.Executer(
                "DELETE FROM dbo.Formules WHERE IdFormule=@Id",
                Sub(cmd) cmd.Parameters.Add("@Id", SqlDbType.Int).Value = idFormuleSelectionne)

            MessageBox.Show("Formule supprimée avec succès.")
            ViderChampsFormule()
            ChargerFormules()
        Catch ex As Exception
            MessageBox.Show("Erreur suppression formule : " & ex.Message)
        End Try
    End Sub

    ' ═══════════════════════════════════════════════════════════════════
    '  DUPLIQUER FORMULE
    ' ═══════════════════════════════════════════════════════════════════

    Private Sub btnDupliquerFormule_Click(sender As Object, e As EventArgs) Handles btnDupliquerFormule.Click
        If idFormuleSelectionne = 0 Then
            MessageBox.Show("Sélectionne d'abord une formule.")
            Exit Sub
        End If
        Try
            Dim nouveauCode As String = txtCodeFormule.Text.Trim() & "_COPIE"
            Dim nouveauNom As String = txtNomFormule.Text.Trim() & " COPIE"
            Dim i As Integer = 1

            While True
                Dim nb As Integer = ModuleDb.LireValeur(Of Integer)(
                    "SELECT COUNT(*) FROM dbo.Formules WHERE CodeFormule=@CodeFormule",
                    Sub(cmd) cmd.Parameters.Add("@CodeFormule", SqlDbType.NVarChar, 50).Value = nouveauCode)
                If nb = 0 Then Exit While
                nouveauCode = txtCodeFormule.Text.Trim() & "_COPIE" & i.ToString()
                nouveauNom = txtNomFormule.Text.Trim() & " COPIE " & i.ToString()
                i += 1
            End While

            Dim nouvelId As Integer = ModuleDb.LireValeur(Of Integer)(
                "INSERT INTO dbo.Formules " &
                "(CodeFormule, CleRecherche, NomFormule, Gamme, TempsMalaxage, MES, VolumeAir, TauxChlorureCalc, MasseAlcalinCalc, " &
                "ClasseExposition, AdditionA, Designation, Certification, Attestation, NatureCiment, Consistance, GranulariteDmax, " &
                "Resistance, Controle, ClasseChlorure, RapportEC, Adjuvants, RemarqueNorme, DerniereModification) " &
                "SELECT @NouveauCode, CleRecherche, @NouveauNom, " &
                "Gamme, TempsMalaxage, MES, VolumeAir, TauxChlorureCalc, MasseAlcalinCalc, " &
                "ClasseExposition, AdditionA, Designation, Certification, Attestation, NatureCiment, Consistance, GranulariteDmax, " &
                "Resistance, Controle, ClasseChlorure, RapportEC, Adjuvants, RemarqueNorme, GETDATE() " &
                "FROM dbo.Formules WHERE IdFormule=@IdFormule; SELECT SCOPE_IDENTITY();",
                Sub(cmd)
                    cmd.Parameters.Add("@IdFormule", SqlDbType.Int).Value = idFormuleSelectionne
                    cmd.Parameters.Add("@NouveauCode", SqlDbType.NVarChar, 50).Value = nouveauCode
                    cmd.Parameters.Add("@NouveauNom", SqlDbType.NVarChar, 200).Value = nouveauNom
                End Sub)

            ModuleDb.Executer(
                "INSERT INTO dbo.FormuleComposition " &
                "(IdFormule, IdMateriau, TypeMateriau, CodeMateriau, Quantite, Unite, Consigne, Phase, PourcentageConsigne, DecalageSecondes, Controle, AdjuvantEnPourcentDosage) " &
                "SELECT @NouvelId, IdMateriau, TypeMateriau, CodeMateriau, Quantite, Unite, Consigne, Phase, PourcentageConsigne, DecalageSecondes, Controle, AdjuvantEnPourcentDosage " &
                "FROM dbo.FormuleComposition WHERE IdFormule=@AncienId",
                Sub(cmd)
                    cmd.Parameters.Add("@NouvelId", SqlDbType.Int).Value = nouvelId
                    cmd.Parameters.Add("@AncienId", SqlDbType.Int).Value = idFormuleSelectionne
                End Sub)

            MessageBox.Show("Formule dupliquée avec succès.")
            ChargerFormules()

        Catch ex As Exception
            MessageBox.Show("Erreur duplication formule : " & ex.Message)
        End Try
    End Sub

    ' ═══════════════════════════════════════════════════════════════════
    '  COMPOSITION
    ' ═══════════════════════════════════════════════════════════════════

    Private Sub ChargerCompositionFormule(idFormule As Integer)
        Try
            Dim dt As DataTable = ModuleDb.Lire(
                "SELECT IdComposition, TypeMateriau, CodeMateriau, Quantite, Unite, Consigne, " &
                "Phase, PourcentageConsigne, DecalageSecondes, Controle, AdjuvantEnPourcentDosage " &
                "FROM dbo.FormuleComposition WHERE IdFormule=@IdFormule ORDER BY IdComposition",
                Sub(cmd) cmd.Parameters.Add("@IdFormule", SqlDbType.Int).Value = idFormule)

            DataGridViewComposition.DataSource = dt
            DataGridViewComposition.ReadOnly = True
            DataGridViewComposition.SelectionMode = DataGridViewSelectionMode.FullRowSelect
            DataGridViewComposition.AllowUserToAddRows = False
            DataGridViewComposition.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill

            If DataGridViewComposition.Columns.Count > 0 Then
                DataGridViewComposition.Columns("IdComposition").Visible = False
                DataGridViewComposition.Columns("TypeMateriau").HeaderText = "Type"
                DataGridViewComposition.Columns("CodeMateriau").HeaderText = "Matériau"
                DataGridViewComposition.Columns("Quantite").HeaderText = "Quantité"
                DataGridViewComposition.Columns("Unite").HeaderText = "Unité"
                DataGridViewComposition.Columns("Consigne").HeaderText = "Consigne"
                DataGridViewComposition.Columns("Phase").HeaderText = "Phase"
                DataGridViewComposition.Columns("PourcentageConsigne").HeaderText = "%"
                DataGridViewComposition.Columns("DecalageSecondes").HeaderText = "Décalage"
                DataGridViewComposition.Columns("Controle").HeaderText = "Contrôle"
                DataGridViewComposition.Columns("AdjuvantEnPourcentDosage").HeaderText = "Adj %"
            End If
        Catch ex As Exception
            MessageBox.Show("Erreur chargement composition : " & ex.Message)
        End Try
    End Sub

    Private Sub cmbTypeMateriauFormule_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cmbTypeMateriauFormule.SelectedIndexChanged
        ChargerMateriauxParType(cmbTypeMateriauFormule.Text)
    End Sub

    Private Sub ChargerMateriauxParType(typeMat As String)
        Try
            Dim dt As DataTable = ModuleDb.Lire(
                "SELECT IdMateriau, CodeMateriau, NomMateriau FROM dbo.Materiaux " &
                "WHERE TypeMateriau=@TypeMateriau ORDER BY NomMateriau",
                Sub(cmd) cmd.Parameters.Add("@TypeMateriau", SqlDbType.NVarChar, 50).Value = typeMat)

            cmbMateriauFormule.DataSource = dt
            cmbMateriauFormule.DisplayMember = "CodeMateriau"
            cmbMateriauFormule.ValueMember = "IdMateriau"
        Catch ex As Exception
            MessageBox.Show("Erreur chargement matériaux : " & ex.Message)
        End Try
    End Sub

    Private Sub DataGridViewComposition_CellClick(sender As Object, e As DataGridViewCellEventArgs) Handles DataGridViewComposition.CellClick
        If e.RowIndex < 0 Then Exit Sub
        ' 19/09 : valeurs NULL tolérées (ex. AdjuvantEnPourcentDosage vide → plantage DBNull)
        Dim lig As DataGridViewRow = DataGridViewComposition.Rows(e.RowIndex)
        Dim vId As Object = lig.Cells("IdComposition").Value
        If vId Is Nothing OrElse IsDBNull(vId) Then Exit Sub
        idCompositionSelectionne = Convert.ToInt32(vId)
        cmbTypeMateriauFormule.Text = CelluleTexte(lig, "TypeMateriau")
        ChargerMateriauxParType(cmbTypeMateriauFormule.Text)
        cmbMateriauFormule.Text = CelluleTexte(lig, "CodeMateriau")
        txtQuantiteFormule.Text = CelluleTexte(lig, "Quantite")
        cmbUniteFormule.Text = CelluleTexte(lig, "Unite")
        Dim vAdj As Object = lig.Cells("AdjuvantEnPourcentDosage").Value
        chkAdjuvantPourcentDosage.Checked = (vAdj IsNot Nothing AndAlso Not IsDBNull(vAdj) AndAlso Convert.ToBoolean(vAdj))
        modeEditionComposition = False
    End Sub

    Private Sub btnInsererComposition_Click(sender As Object, e As EventArgs) Handles btnInsererComposition.Click
        If idFormuleSelectionne = 0 Then MessageBox.Show("Enregistre d'abord la formule.") : Exit Sub
        If cmbTypeMateriauFormule.Text.Trim() = "" Then MessageBox.Show("Choisis le type matériau.") : Exit Sub
        If cmbMateriauFormule.Text.Trim() = "" Then MessageBox.Show("Choisis le matériau.") : Exit Sub
        If txtQuantiteFormule.Text.Trim() = "" Then MessageBox.Show("Saisis la quantité.") : Exit Sub
        If cmbMateriauFormule.SelectedValue Is Nothing Then MessageBox.Show("Choisis d'abord un matériau.") : Exit Sub
        Try
            If Not modeEditionComposition Then
                ModuleDb.Executer(
                    "INSERT INTO dbo.FormuleComposition " &
                    "(IdFormule, IdMateriau, TypeMateriau, CodeMateriau, Quantite, Unite, Consigne, " &
                    "Phase, PourcentageConsigne, DecalageSecondes, Controle, AdjuvantEnPourcentDosage) " &
                    "VALUES (@IdFormule, @IdMateriau, @TypeMateriau, @CodeMateriau, @Quantite, @Unite, @Consigne, " &
                    "@Phase, @PourcentageConsigne, @DecalageSecondes, @Controle, @AdjuvantEnPourcentDosage)",
                    AddressOf AjouterParamsComposition)
                MessageBox.Show("Ligne composition ajoutée.")
            Else
                ModuleDb.Executer(
                    "UPDATE dbo.FormuleComposition SET " &
                    "IdMateriau=@IdMateriau, TypeMateriau=@TypeMateriau, CodeMateriau=@CodeMateriau, " &
                    "Quantite=@Quantite, Unite=@Unite, Consigne=@Consigne, AdjuvantEnPourcentDosage=@AdjuvantEnPourcentDosage " &
                    "WHERE IdComposition=@IdComposition",
                    Sub(cmd)
                        AjouterParamsComposition(cmd)
                        cmd.Parameters.Add("@IdComposition", SqlDbType.Int).Value = idCompositionSelectionne
                    End Sub)
                ' 19/09 : pas de fenêtre de confirmation à la modification
                modeEditionComposition = False
            End If

            ChargerCompositionFormule(idFormuleSelectionne)
            idCompositionSelectionne = 0
            cmbTypeMateriauFormule.SelectedIndex = -1
            cmbMateriauFormule.DataSource = Nothing
            txtQuantiteFormule.Text = ""
            cmbUniteFormule.SelectedIndex = -1
            chkAdjuvantPourcentDosage.Checked = False

        Catch ex As Exception
            MessageBox.Show("Erreur composition : " & ex.Message)
        End Try
    End Sub

    Private Sub btnSupprimerComposition_Click(sender As Object, e As EventArgs) Handles btnSupprimerComposition.Click
        If idCompositionSelectionne = 0 Then
            MessageBox.Show("Sélectionne d'abord une ligne de composition.")
            Exit Sub
        End If
        If MessageBox.Show("Voulez-vous supprimer cette ligne ?", "Confirmation",
                           MessageBoxButtons.YesNo, MessageBoxIcon.Question) = DialogResult.No Then Exit Sub
        Try
            ModuleDb.Executer(
                "DELETE FROM dbo.FormuleComposition WHERE IdComposition=@IdComposition",
                Sub(cmd) cmd.Parameters.Add("@IdComposition", SqlDbType.Int).Value = idCompositionSelectionne)

            MessageBox.Show("Ligne supprimée.")
            ChargerCompositionFormule(idFormuleSelectionne)
            idCompositionSelectionne = 0
            modeEditionComposition = False
            cmbTypeMateriauFormule.SelectedIndex = -1
            cmbMateriauFormule.DataSource = Nothing
            txtQuantiteFormule.Text = ""
            cmbUniteFormule.SelectedIndex = -1
            chkAdjuvantPourcentDosage.Checked = False
        Catch ex As Exception
            MessageBox.Show("Erreur suppression composition : " & ex.Message)
        End Try
    End Sub

    ' ═══════════════════════════════════════════════════════════════════
    '  IMPRESSION
    ' ═══════════════════════════════════════════════════════════════════

    Private Sub btnImprimerFormule_Click(sender As Object, e As EventArgs) Handles btnImprimerFormule.Click
        If idFormuleSelectionne = 0 Then
            MessageBox.Show("Sélectionne d'abord une formule.")
            Exit Sub
        End If
        apercuImpression.Document = docImpression
        apercuImpression.WindowState = FormWindowState.Maximized
        apercuImpression.ShowDialog()
    End Sub

    Private Sub docImpression_PrintPage(sender As Object, e As PrintPageEventArgs)
        Dim g As Graphics = e.Graphics
        Dim titreFont As New Font("Arial", 16, FontStyle.Bold)
        Dim sousTitreFont As New Font("Arial", 11, FontStyle.Bold)
        Dim normalFont As New Font("Arial", 10, FontStyle.Regular)
        Dim x As Integer = 40 : Dim y As Integer = 40 : Dim ligne As Integer = 28

        g.DrawString("FICHE FORMULE", titreFont, Brushes.Black, x, y) : y += 40
        g.DrawString("Date impression : " & DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss"), normalFont, Brushes.Black, x, y) : y += 35

        Dim champsFormule = {
            ("Code formule :", txtCodeFormule.Text), ("Clé recherche :", txtCleRechercheFormule.Text),
            ("Nom formule :", txtNomFormule.Text), ("Gamme :", txtGamme.Text),
            ("Temps malaxage :", txtTempsMalaxage.Text), ("MES :", txtMES.Text),
            ("Volume d'air :", txtVolumeAir.Text), ("Taux chlorure :", txtTauxChlorureCalc.Text),
            ("Masse alcalin :", txtMasseAlcalinCalc.Text)}
        For Each c In champsFormule
            g.DrawString(c.Item1, sousTitreFont, Brushes.Black, x, y)
            g.DrawString(c.Item2, normalFont, Brushes.Black, x + 180, y)
            y += ligne
        Next
        y += 10 : g.DrawLine(Pens.Black, x, y, 780, y) : y += 20
        g.DrawString("NORME", titreFont, Brushes.Black, x, y) : y += 35

        Dim champsNorme = {
            ("Classe exposition :", cmbClasseExposition.Text), ("Addition :", cmbAdditionA.Text),
            ("Désignation :", txtDesignation.Text), ("Certification :", cmbCertification.Text),
            ("Attestation :", txtAttestation.Text), ("Nature ciment :", cmbNatureCiment.Text),
            ("Consistance :", cmbConsistance.Text), ("Granularité Dmax :", cmbGranulariteDmax.Text),
            ("Résistance :", cmbResistance.Text), ("Contrôle :", cmbControleNorme.Text),
            ("Classe chlorure :", cmbClasseChlorure.Text), ("Rapport E/C :", txtRapportEC.Text),
            ("Adjuvants :", txtAdjuvants.Text), ("Remarque norme :", txtRemarqueNorme.Text)}
        For Each c In champsNorme
            g.DrawString(c.Item1, sousTitreFont, Brushes.Black, x, y)
            g.DrawString(c.Item2, normalFont, Brushes.Black, x + 180, y)
            y += ligne
        Next
        y += 10 : g.DrawLine(Pens.Black, x, y, 780, y) : y += 20
        g.DrawString("COMPOSITION", titreFont, Brushes.Black, x, y) : y += 35
        g.DrawString("Matériau", sousTitreFont, Brushes.Black, x, y)
        g.DrawString("Quantité", sousTitreFont, Brushes.Black, x + 220, y)
        g.DrawString("Unité", sousTitreFont, Brushes.Black, x + 340, y)
        g.DrawString("Consigne", sousTitreFont, Brushes.Black, x + 430, y)
        y += 25 : g.DrawLine(Pens.Black, x, y, 780, y) : y += 10

        For Each row As DataGridViewRow In DataGridViewComposition.Rows
            If Not row.IsNewRow Then
                g.DrawString(row.Cells("CodeMateriau").Value.ToString(), normalFont, Brushes.Black, x, y)
                g.DrawString(row.Cells("Quantite").Value.ToString(), normalFont, Brushes.Black, x + 220, y)
                g.DrawString(row.Cells("Unite").Value.ToString(), normalFont, Brushes.Black, x + 340, y)
                g.DrawString(row.Cells("Consigne").Value.ToString(), normalFont, Brushes.Black, x + 430, y)
                y += 24
                If y > 1050 Then Exit For
            End If
        Next
    End Sub

    ' ═══════════════════════════════════════════════════════════════════
    '  HELPERS
    ' ═══════════════════════════════════════════════════════════════════

    Private Sub AjouterParamsFormule(cmd As SqlCommand)
        cmd.Parameters.Add("@CodeFormule", SqlDbType.NVarChar, 50).Value = txtCodeFormule.Text.Trim()
        cmd.Parameters.Add("@CleRecherche", SqlDbType.NVarChar, 50).Value = txtCleRechercheFormule.Text.Trim()
        cmd.Parameters.Add("@NomFormule", SqlDbType.NVarChar, 200).Value = txtNomFormule.Text.Trim()
        cmd.Parameters.Add("@Gamme", SqlDbType.NVarChar, 50).Value = txtGamme.Text.Trim()
        cmd.Parameters.Add("@TempsMalaxage", SqlDbType.Decimal).Value = ParseDecimal(txtTempsMalaxage.Text)
        cmd.Parameters.Add("@MES", SqlDbType.Decimal).Value = ParseDecimal(txtMES.Text)
        cmd.Parameters.Add("@VolumeAir", SqlDbType.Decimal).Value = ParseDecimal(txtVolumeAir.Text)
        cmd.Parameters.Add("@TauxChlorureCalc", SqlDbType.Decimal).Value = ParseDecimal(txtTauxChlorureCalc.Text)
        cmd.Parameters.Add("@MasseAlcalinCalc", SqlDbType.Decimal).Value = ParseDecimal(txtMasseAlcalinCalc.Text)
        cmd.Parameters.Add("@ClasseExposition", SqlDbType.NVarChar, 100).Value = cmbClasseExposition.Text.Trim()
        cmd.Parameters.Add("@AdditionA", SqlDbType.NVarChar, 50).Value = cmbAdditionA.Text.Trim()
        cmd.Parameters.Add("@Designation", SqlDbType.NVarChar, 200).Value = txtDesignation.Text.Trim()
        cmd.Parameters.Add("@Certification", SqlDbType.NVarChar, 100).Value = cmbCertification.Text.Trim()
        cmd.Parameters.Add("@Attestation", SqlDbType.NVarChar, 100).Value = txtAttestation.Text.Trim()
        cmd.Parameters.Add("@NatureCiment", SqlDbType.NVarChar, 100).Value = cmbNatureCiment.Text.Trim()
        cmd.Parameters.Add("@Consistance", SqlDbType.NVarChar, 50).Value = cmbConsistance.Text.Trim()
        cmd.Parameters.Add("@GranulariteDmax", SqlDbType.NVarChar, 50).Value = cmbGranulariteDmax.Text.Trim()
        cmd.Parameters.Add("@Resistance", SqlDbType.NVarChar, 50).Value = cmbResistance.Text.Trim()
        cmd.Parameters.Add("@Controle", SqlDbType.NVarChar, 50).Value = cmbControleNorme.Text.Trim()
        cmd.Parameters.Add("@ClasseChlorure", SqlDbType.NVarChar, 50).Value = cmbClasseChlorure.Text.Trim()
        cmd.Parameters.Add("@RapportEC", SqlDbType.Decimal).Value = ParseDecimal(txtRapportEC.Text)
        cmd.Parameters.Add("@Adjuvants", SqlDbType.NVarChar, 200).Value = txtAdjuvants.Text.Trim()
        cmd.Parameters.Add("@RemarqueNorme", SqlDbType.NVarChar, 500).Value = txtRemarqueNorme.Text.Trim()
    End Sub

    Private Sub AjouterParamsComposition(cmd As SqlCommand)
        cmd.Parameters.Add("@IdFormule", SqlDbType.Int).Value = idFormuleSelectionne
        cmd.Parameters.Add("@IdMateriau", SqlDbType.Int).Value = CInt(cmbMateriauFormule.SelectedValue)
        cmd.Parameters.Add("@TypeMateriau", SqlDbType.NVarChar, 50).Value = cmbTypeMateriauFormule.Text
        cmd.Parameters.Add("@CodeMateriau", SqlDbType.NVarChar, 50).Value = cmbMateriauFormule.Text
        cmd.Parameters.Add("@Quantite", SqlDbType.Decimal).Value = ParseDecimal(txtQuantiteFormule.Text)
        cmd.Parameters.Add("@Unite", SqlDbType.NVarChar, 20).Value = cmbUniteFormule.Text
        cmd.Parameters.Add("@Consigne", SqlDbType.Decimal).Value = ParseDecimal(txtQuantiteFormule.Text)
        cmd.Parameters.Add("@Phase", SqlDbType.Int).Value = 1
        cmd.Parameters.Add("@PourcentageConsigne", SqlDbType.Decimal).Value = 100
        cmd.Parameters.Add("@DecalageSecondes", SqlDbType.Int).Value = 0
        cmd.Parameters.Add("@Controle", SqlDbType.Bit).Value = False
        cmd.Parameters.Add("@AdjuvantEnPourcentDosage", SqlDbType.Bit).Value = chkAdjuvantPourcentDosage.Checked
    End Sub

    Private Function CelluleTexte(lig As DataGridViewRow, col As String) As String
        Dim v As Object = lig.Cells(col).Value
        If v Is Nothing OrElse IsDBNull(v) Then Return ""
        Return v.ToString()
    End Function

    Private Function ParseDecimal(txt As String) As Decimal
        Dim v As Decimal
        Decimal.TryParse(txt.Replace(",", "."),
            Globalization.NumberStyles.Any,
            Globalization.CultureInfo.InvariantCulture, v)
        Return v
    End Function

    Private Sub ViderChampsFormule()
        txtCodeFormule.Text = "" : txtCleRechercheFormule.Text = "" : txtNomFormule.Text = ""
        txtGamme.Text = "" : txtTempsMalaxage.Text = "" : txtMES.Text = "" : txtVolumeAir.Text = ""
        txtTauxChlorureCalc.Text = "" : txtMasseAlcalinCalc.Text = "" : txtRapportEC.Text = ""
        txtAdjuvants.Text = "" : txtRemarqueNorme.Text = "" : txtDesignation.Text = "" : txtAttestation.Text = ""
        cmbClasseExposition.SelectedIndex = -1 : cmbAdditionA.SelectedIndex = -1
        cmbCertification.SelectedIndex = -1 : cmbNatureCiment.SelectedIndex = -1
        cmbConsistance.SelectedIndex = -1 : cmbGranulariteDmax.SelectedIndex = -1
        cmbResistance.SelectedIndex = -1 : cmbControleNorme.SelectedIndex = -1 : cmbClasseChlorure.SelectedIndex = -1
        cmbTypeMateriauFormule.SelectedIndex = -1 : cmbMateriauFormule.DataSource = Nothing
        txtQuantiteFormule.Text = "" : cmbUniteFormule.SelectedIndex = -1
        chkAdjuvantPourcentDosage.Checked = False
        DataGridViewComposition.DataSource = Nothing
        idFormuleSelectionne = 0 : idCompositionSelectionne = 0
        modeEditionFormule = False : modeEditionComposition = False
    End Sub

    Private Sub DataGridViewFormules_CellContentClick(sender As Object, e As DataGridViewCellEventArgs) Handles DataGridViewFormules.CellContentClick
    End Sub

    Private Sub btnModifierComposition_Click(sender As Object, e As EventArgs) Handles btnModifierComposition.Click
        ' 19/09 : clic sur la ligne → champs remplis → modifier les champs → clic Modifier = UPDATE direct, sans confirmation
        If idCompositionSelectionne = 0 Then
            MessageBox.Show("Sélectionne d'abord une ligne de composition dans la table.", "Modifier",
                            MessageBoxButtons.OK, MessageBoxIcon.Information)
            Exit Sub
        End If
        If cmbMateriauFormule.SelectedValue Is Nothing Then MessageBox.Show("Choisis le matériau.") : Exit Sub
        If txtQuantiteFormule.Text.Trim() = "" Then MessageBox.Show("Saisis la quantité.") : Exit Sub
        Try
            ModuleDb.Executer(
                "UPDATE dbo.FormuleComposition SET " &
                "IdMateriau=@IdMateriau, TypeMateriau=@TypeMateriau, CodeMateriau=@CodeMateriau, " &
                "Quantite=@Quantite, Unite=@Unite, Consigne=@Consigne, AdjuvantEnPourcentDosage=@AdjuvantEnPourcentDosage " &
                "WHERE IdComposition=@IdComposition",
                Sub(cmd)
                    AjouterParamsComposition(cmd)
                    cmd.Parameters.Add("@IdComposition", SqlDbType.Int).Value = idCompositionSelectionne
                End Sub)
            modeEditionComposition = False
            ChargerCompositionFormule(idFormuleSelectionne)
            idCompositionSelectionne = 0
        Catch ex As Exception
            MessageBox.Show("Erreur modification composition : " & ex.Message)
        End Try
    End Sub

    Private Sub btnValiderFormule_Click(sender As Object, e As EventArgs) Handles btnValiderFormule.Click

    End Sub
End Class