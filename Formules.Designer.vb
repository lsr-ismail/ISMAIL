<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class Formules
    Inherits System.Windows.Forms.Form

    'Form remplace la méthode Dispose pour nettoyer la liste des composants.
    <System.Diagnostics.DebuggerNonUserCode()> _
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        Try
            If disposing AndAlso components IsNot Nothing Then
                components.Dispose()
            End If
        Finally
            MyBase.Dispose(disposing)
        End Try
    End Sub

    'Requise par le Concepteur Windows Form
    Private components As System.ComponentModel.IContainer

    'REMARQUE : la procédure suivante est requise par le Concepteur Windows Form
    'Elle peut être modifiée à l'aide du Concepteur Windows Form.  
    'Ne la modifiez pas à l'aide de l'éditeur de code.
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(Formules))
        PrintDocument1 = New Printing.PrintDocument()
        PrintPreviewDialog1 = New PrintPreviewDialog()
        DataGridViewFormules = New DataGridView()
        btnModifierComposition = New Button()
        btnSupprimerComposition = New Button()
        btnInsererComposition = New Button()
        Label13 = New Label()
        TabControl1 = New TabControl()
        TabPage1 = New TabPage()
        chkAdjuvantPourcentDosage = New CheckBox()
        cmbTypeMateriauFormule = New ComboBox()
        txtQuantiteFormule = New TextBox()
        Label4 = New Label()
        Label69 = New Label()
        cmbMateriauFormule = New ComboBox()
        Label67 = New Label()
        Label66 = New Label()
        Label65 = New Label()
        DataGridViewComposition = New DataGridView()
        TabPage2 = New TabPage()
        cmbControleNorme = New ComboBox()
        cmbGranulariteDmax = New ComboBox()
        cmbNatureCiment = New ComboBox()
        txtDesignation = New ComboBox()
        cmbAdditionA = New ComboBox()
        cmbClasseExposition = New ComboBox()
        Label58 = New Label()
        txtRapportEC = New TextBox()
        txtAttestation = New TextBox()
        txtRemarqueNorme = New ListBox()
        chkDosageMiniOuReel = New Button()
        Label71 = New Label()
        TextBox2 = New TextBox()
        Label70 = New Label()
        Label11 = New Label()
        Label12 = New Label()
        Label17 = New Label()
        cmbConsistance = New ComboBox()
        cmbResistance = New ComboBox()
        cmbClasseChlorure = New ComboBox()
        cmbCertification = New ComboBox()
        Label14 = New Label()
        txtAd = New Label()
        Label18 = New Label()
        Label19 = New Label()
        Label20 = New Label()
        Label21 = New Label()
        Label22 = New Label()
        Label23 = New Label()
        Label24 = New Label()
        Label25 = New Label()
        txtAdjuvants = New TextBox()
        TabPage3 = New TabPage()
        Label30 = New Label()
        txtTarif2MontantFormule = New TextBox()
        dtpTarif2DateFormule = New TextBox()
        TextBox1 = New TextBox()
        TextBox4 = New TextBox()
        TextBox5 = New TextBox()
        Label16 = New Label()
        Label26 = New Label()
        Label27 = New Label()
        Label28 = New Label()
        Label29 = New Label()
        Label32 = New Label()
        Label33 = New Label()
        Label34 = New Label()
        Label35 = New Label()
        Label36 = New Label()
        Label37 = New Label()
        txtPrixRevientFormule = New TextBox()
        txtTarif1MontantFormule = New TextBox()
        dtpTarif1DateFormule = New TextBox()
        txtTaxe2Formule = New TextBox()
        txtTaxe1Formule = New TextBox()
        txtTVAFormule = New TextBox()
        txtNomProduitFormule = New TextBox()
        txtCleProduitFormule = New TextBox()
        txtCodeProduitFormule = New TextBox()
        TabPage4 = New TabPage()
        cmbDestinationAdjuvantPese = New ComboBox()
        txtNiveauPerformance = New TextBox()
        txtReductionChargeMaxiTremie = New TextBox()
        ListBox3 = New ListBox()
        chkGestionAjoutEauCompte = New Button()
        chkFormuleInactive = New Button()
        chkValidationPresenceBenne = New Button()
        chkQteLivreeEgaleFabriquee = New Button()
        chkImpressionBLPoidsTonnes = New Button()
        ListBox2 = New ListBox()
        chkGacheeSeche = New Button()
        Label38 = New Label()
        Label39 = New Label()
        Label40 = New Label()
        Label42 = New Label()
        Label43 = New Label()
        Label44 = New Label()
        Label45 = New Label()
        Label46 = New Label()
        Label47 = New Label()
        Label48 = New Label()
        Label49 = New Label()
        TabPage5 = New TabPage()
        Label5 = New Label()
        Label6 = New Label()
        txtDefautPlasticiteMax = New TextBox()
        txtDefautPlasticiteMin = New TextBox()
        Label7 = New Label()
        Label8 = New Label()
        txtPourcentageEauPesee = New TextBox()
        txtEauPlasticite = New TextBox()
        Label59 = New Label()
        Label60 = New Label()
        txtTempsHygrometre = New TextBox()
        txtEauHygrometre = New TextBox()
        Label55 = New Label()
        Label56 = New Label()
        Label57 = New Label()
        txtPhaseHygrometre = New TextBox()
        txtNumeroProgrammeHygrometre = New TextBox()
        txtToleranceEauMax = New TextBox()
        Label52 = New Label()
        Label53 = New Label()
        Label54 = New Label()
        txtToleranceEauMin = New TextBox()
        txtTempsMalaxagePlasticite = New TextBox()
        txtTempsDecalagePlasticite = New TextBox()
        Label31 = New Label()
        Label51 = New Label()
        Label50 = New Label()
        txtPuissanceVisee = New TextBox()
        txtPasPlasticite = New TextBox()
        txtPhaseEauPlasticite = New TextBox()
        txtCleRechercheFormule = New TextBox()
        Label3 = New Label()
        Label2 = New Label()
        Label1 = New Label()
        txtFamilleBeton = New TextBox()
        txtNomFormule = New TextBox()
        txtCodeFormule = New TextBox()
        Label63 = New Label()
        Label64 = New Label()
        txtTempsMalaxage = New TextBox()
        txtMES = New TextBox()
        txtVolumeAir = New TextBox()
        Label62 = New Label()
        txtGamme = New TextBox()
        btnImprimerFormule = New Button()
        btnSupprimerFormule = New Button()
        btnDupliquerFormule = New Button()
        btnModifierFormule = New Button()
        btnAjouterFormule = New Button()
        Label9 = New Label()
        Label10 = New Label()
        Label41 = New Label()
        Label61 = New Label()
        txtTauxChlorureCalc = New TextBox()
        txtMasseAlcalinCalc = New TextBox()
        btnActualiserFormule = New Button()
        btnAbandonnerFormule = New Button()
        btnValiderFormule = New Button()
        TextBox3 = New TextBox()
        cmbUniteFormule = New ComboBox()
        Label15 = New Label()
        btnEnregistrerFormule = New Button()
        CType(DataGridViewFormules, ComponentModel.ISupportInitialize).BeginInit()
        TabControl1.SuspendLayout()
        TabPage1.SuspendLayout()
        CType(DataGridViewComposition, ComponentModel.ISupportInitialize).BeginInit()
        TabPage2.SuspendLayout()
        TabPage3.SuspendLayout()
        TabPage4.SuspendLayout()
        TabPage5.SuspendLayout()
        SuspendLayout()
        ' 
        ' PrintPreviewDialog1
        ' 
        PrintPreviewDialog1.AutoScrollMargin = New Size(0, 0)
        PrintPreviewDialog1.AutoScrollMinSize = New Size(0, 0)
        PrintPreviewDialog1.ClientSize = New Size(400, 300)
        PrintPreviewDialog1.Enabled = True
        PrintPreviewDialog1.Icon = CType(resources.GetObject("PrintPreviewDialog1.Icon"), Icon)
        PrintPreviewDialog1.Name = "PrintPreviewDialog1"
        PrintPreviewDialog1.Visible = False
        ' 
        ' DataGridViewFormules
        ' 
        DataGridViewFormules.AllowUserToAddRows = False
        DataGridViewFormules.Anchor = AnchorStyles.Top Or AnchorStyles.Bottom Or AnchorStyles.Left Or AnchorStyles.Right
        DataGridViewFormules.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill
        DataGridViewFormules.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize
        DataGridViewFormules.Location = New Point(898, 329)
        DataGridViewFormules.MultiSelect = False
        DataGridViewFormules.Name = "DataGridViewFormules"
        DataGridViewFormules.ReadOnly = True
        DataGridViewFormules.RowHeadersWidth = 51
        DataGridViewFormules.SelectionMode = DataGridViewSelectionMode.FullRowSelect
        DataGridViewFormules.Size = New Size(804, 337)
        DataGridViewFormules.TabIndex = 150
        ' 
        ' btnModifierComposition
        ' 
        btnModifierComposition.Location = New Point(393, 59)
        btnModifierComposition.Name = "btnModifierComposition"
        btnModifierComposition.Size = New Size(136, 35)
        btnModifierComposition.TabIndex = 148
        btnModifierComposition.Text = "EDITE"
        btnModifierComposition.UseVisualStyleBackColor = True
        ' 
        ' btnSupprimerComposition
        ' 
        btnSupprimerComposition.Location = New Point(217, 59)
        btnSupprimerComposition.Name = "btnSupprimerComposition"
        btnSupprimerComposition.Size = New Size(136, 35)
        btnSupprimerComposition.TabIndex = 146
        btnSupprimerComposition.Text = "SUPRIME"
        btnSupprimerComposition.UseVisualStyleBackColor = True
        ' 
        ' btnInsererComposition
        ' 
        btnInsererComposition.Location = New Point(29, 59)
        btnInsererComposition.Name = "btnInsererComposition"
        btnInsererComposition.Size = New Size(136, 35)
        btnInsererComposition.TabIndex = 145
        btnInsererComposition.Text = "AJOUTER"
        btnInsererComposition.UseVisualStyleBackColor = True
        ' 
        ' Label13
        ' 
        Label13.AutoSize = True
        Label13.Location = New Point(20, 9)
        Label13.Name = "Label13"
        Label13.Size = New Size(69, 20)
        Label13.TabIndex = 178
        Label13.Text = "Formules"
        ' 
        ' TabControl1
        ' 
        TabControl1.Controls.Add(TabPage1)
        TabControl1.Controls.Add(TabPage2)
        TabControl1.Controls.Add(TabPage3)
        TabControl1.Controls.Add(TabPage4)
        TabControl1.Controls.Add(TabPage5)
        TabControl1.Location = New Point(20, 296)
        TabControl1.Name = "TabControl1"
        TabControl1.SelectedIndex = 0
        TabControl1.Size = New Size(872, 636)
        TabControl1.TabIndex = 179
        ' 
        ' TabPage1
        ' 
        TabPage1.Controls.Add(chkAdjuvantPourcentDosage)
        TabPage1.Controls.Add(cmbTypeMateriauFormule)
        TabPage1.Controls.Add(txtQuantiteFormule)
        TabPage1.Controls.Add(Label4)
        TabPage1.Controls.Add(Label69)
        TabPage1.Controls.Add(cmbMateriauFormule)
        TabPage1.Controls.Add(Label67)
        TabPage1.Controls.Add(Label66)
        TabPage1.Controls.Add(Label65)
        TabPage1.Controls.Add(DataGridViewComposition)
        TabPage1.Controls.Add(btnModifierComposition)
        TabPage1.Controls.Add(btnSupprimerComposition)
        TabPage1.Controls.Add(btnInsererComposition)
        TabPage1.Location = New Point(4, 29)
        TabPage1.Name = "TabPage1"
        TabPage1.Padding = New Padding(3)
        TabPage1.Size = New Size(864, 603)
        TabPage1.TabIndex = 0
        TabPage1.Text = "Composition"
        TabPage1.UseVisualStyleBackColor = True
        ' 
        ' chkAdjuvantPourcentDosage
        ' 
        chkAdjuvantPourcentDosage.AutoSize = True
        chkAdjuvantPourcentDosage.Location = New Point(756, 32)
        chkAdjuvantPourcentDosage.Name = "chkAdjuvantPourcentDosage"
        chkAdjuvantPourcentDosage.Size = New Size(18, 17)
        chkAdjuvantPourcentDosage.TabIndex = 284
        chkAdjuvantPourcentDosage.UseVisualStyleBackColor = True
        ' 
        ' cmbTypeMateriauFormule
        ' 
        cmbTypeMateriauFormule.FormattingEnabled = True
        cmbTypeMateriauFormule.Location = New Point(19, 26)
        cmbTypeMateriauFormule.Name = "cmbTypeMateriauFormule"
        cmbTypeMateriauFormule.Size = New Size(161, 28)
        cmbTypeMateriauFormule.TabIndex = 283
        ' 
        ' txtQuantiteFormule
        ' 
        txtQuantiteFormule.Location = New Point(353, 26)
        txtQuantiteFormule.Name = "txtQuantiteFormule"
        txtQuantiteFormule.Size = New Size(118, 27)
        txtQuantiteFormule.TabIndex = 282
        ' 
        ' Label4
        ' 
        Label4.AutoSize = True
        Label4.Location = New Point(429, 517)
        Label4.Name = "Label4"
        Label4.Size = New Size(83, 20)
        Label4.TabIndex = 214
        Label4.Text = "Fournisseur"
        ' 
        ' Label69
        ' 
        Label69.AutoSize = True
        Label69.Location = New Point(570, 32)
        Label69.Name = "Label69"
        Label69.Size = New Size(184, 20)
        Label69.TabIndex = 213
        Label69.Text = "Adjuvant Pourcent Dosage"
        ' 
        ' cmbMateriauFormule
        ' 
        cmbMateriauFormule.FormattingEnabled = True
        cmbMateriauFormule.Location = New Point(190, 25)
        cmbMateriauFormule.Name = "cmbMateriauFormule"
        cmbMateriauFormule.Size = New Size(151, 28)
        cmbMateriauFormule.TabIndex = 208
        ' 
        ' Label67
        ' 
        Label67.AutoSize = True
        Label67.Location = New Point(347, 4)
        Label67.Name = "Label67"
        Label67.Size = New Size(124, 20)
        Label67.TabIndex = 206
        Label67.Text = "Quantité Formule"
        ' 
        ' Label66
        ' 
        Label66.AutoSize = True
        Label66.Location = New Point(192, 4)
        Label66.Name = "Label66"
        Label66.Size = New Size(126, 20)
        Label66.TabIndex = 205
        Label66.Text = "Materiau Formule"
        ' 
        ' Label65
        ' 
        Label65.AutoSize = True
        Label65.Location = New Point(19, 3)
        Label65.Name = "Label65"
        Label65.Size = New Size(161, 20)
        Label65.TabIndex = 204
        Label65.Text = "Type Materiau Formule"
        ' 
        ' DataGridViewComposition
        ' 
        DataGridViewComposition.AllowUserToAddRows = False
        DataGridViewComposition.Anchor = AnchorStyles.Top Or AnchorStyles.Bottom Or AnchorStyles.Left Or AnchorStyles.Right
        DataGridViewComposition.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill
        DataGridViewComposition.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize
        DataGridViewComposition.Location = New Point(29, 128)
        DataGridViewComposition.MultiSelect = False
        DataGridViewComposition.Name = "DataGridViewComposition"
        DataGridViewComposition.ReadOnly = True
        DataGridViewComposition.RowHeadersWidth = 51
        DataGridViewComposition.SelectionMode = DataGridViewSelectionMode.FullRowSelect
        DataGridViewComposition.Size = New Size(813, 386)
        DataGridViewComposition.TabIndex = 203
        ' 
        ' TabPage2
        ' 
        TabPage2.Controls.Add(cmbControleNorme)
        TabPage2.Controls.Add(cmbGranulariteDmax)
        TabPage2.Controls.Add(cmbNatureCiment)
        TabPage2.Controls.Add(txtDesignation)
        TabPage2.Controls.Add(cmbAdditionA)
        TabPage2.Controls.Add(cmbClasseExposition)
        TabPage2.Controls.Add(Label58)
        TabPage2.Controls.Add(txtRapportEC)
        TabPage2.Controls.Add(txtAttestation)
        TabPage2.Controls.Add(txtRemarqueNorme)
        TabPage2.Controls.Add(chkDosageMiniOuReel)
        TabPage2.Controls.Add(Label71)
        TabPage2.Controls.Add(TextBox2)
        TabPage2.Controls.Add(Label70)
        TabPage2.Controls.Add(Label11)
        TabPage2.Controls.Add(Label12)
        TabPage2.Controls.Add(Label17)
        TabPage2.Controls.Add(cmbConsistance)
        TabPage2.Controls.Add(cmbResistance)
        TabPage2.Controls.Add(cmbClasseChlorure)
        TabPage2.Controls.Add(cmbCertification)
        TabPage2.Controls.Add(Label14)
        TabPage2.Controls.Add(txtAd)
        TabPage2.Controls.Add(Label18)
        TabPage2.Controls.Add(Label19)
        TabPage2.Controls.Add(Label20)
        TabPage2.Controls.Add(Label21)
        TabPage2.Controls.Add(Label22)
        TabPage2.Controls.Add(Label23)
        TabPage2.Controls.Add(Label24)
        TabPage2.Controls.Add(Label25)
        TabPage2.Controls.Add(txtAdjuvants)
        TabPage2.Location = New Point(4, 29)
        TabPage2.Name = "TabPage2"
        TabPage2.Padding = New Padding(3)
        TabPage2.Size = New Size(864, 603)
        TabPage2.TabIndex = 1
        TabPage2.Text = "Norme"
        TabPage2.UseVisualStyleBackColor = True
        ' 
        ' cmbControleNorme
        ' 
        cmbControleNorme.FormattingEnabled = True
        cmbControleNorme.Location = New Point(522, 264)
        cmbControleNorme.Name = "cmbControleNorme"
        cmbControleNorme.Size = New Size(167, 28)
        cmbControleNorme.TabIndex = 254
        ' 
        ' cmbGranulariteDmax
        ' 
        cmbGranulariteDmax.FormattingEnabled = True
        cmbGranulariteDmax.Location = New Point(522, 234)
        cmbGranulariteDmax.Name = "cmbGranulariteDmax"
        cmbGranulariteDmax.Size = New Size(167, 28)
        cmbGranulariteDmax.TabIndex = 253
        ' 
        ' cmbNatureCiment
        ' 
        cmbNatureCiment.FormattingEnabled = True
        cmbNatureCiment.Location = New Point(184, 195)
        cmbNatureCiment.Name = "cmbNatureCiment"
        cmbNatureCiment.Size = New Size(506, 28)
        cmbNatureCiment.TabIndex = 252
        ' 
        ' txtDesignation
        ' 
        txtDesignation.FormattingEnabled = True
        txtDesignation.Location = New Point(183, 124)
        txtDesignation.Name = "txtDesignation"
        txtDesignation.Size = New Size(506, 28)
        txtDesignation.TabIndex = 251
        ' 
        ' cmbAdditionA
        ' 
        cmbAdditionA.FormattingEnabled = True
        cmbAdditionA.Location = New Point(183, 90)
        cmbAdditionA.Name = "cmbAdditionA"
        cmbAdditionA.Size = New Size(506, 28)
        cmbAdditionA.TabIndex = 250
        ' 
        ' cmbClasseExposition
        ' 
        cmbClasseExposition.FormattingEnabled = True
        cmbClasseExposition.Location = New Point(181, 57)
        cmbClasseExposition.Name = "cmbClasseExposition"
        cmbClasseExposition.Size = New Size(506, 28)
        cmbClasseExposition.TabIndex = 249
        ' 
        ' Label58
        ' 
        Label58.AutoSize = True
        Label58.Location = New Point(39, 374)
        Label58.Name = "Label58"
        Label58.Size = New Size(149, 20)
        Label58.TabIndex = 248
        Label58.Text = "Dosage Mini Ou Réel"
        ' 
        ' txtRapportEC
        ' 
        txtRapportEC.Location = New Point(525, 299)
        txtRapportEC.Name = "txtRapportEC"
        txtRapportEC.Size = New Size(166, 27)
        txtRapportEC.TabIndex = 247
        ' 
        ' txtAttestation
        ' 
        txtAttestation.Location = New Point(524, 161)
        txtAttestation.Name = "txtAttestation"
        txtAttestation.Size = New Size(166, 27)
        txtAttestation.TabIndex = 244
        ' 
        ' txtRemarqueNorme
        ' 
        txtRemarqueNorme.FormattingEnabled = True
        txtRemarqueNorme.Location = New Point(297, 489)
        txtRemarqueNorme.Name = "txtRemarqueNorme"
        txtRemarqueNorme.Size = New Size(409, 84)
        txtRemarqueNorme.TabIndex = 243
        ' 
        ' chkDosageMiniOuReel
        ' 
        chkDosageMiniOuReel.Location = New Point(204, 367)
        chkDosageMiniOuReel.Name = "chkDosageMiniOuReel"
        chkDosageMiniOuReel.Size = New Size(74, 35)
        chkDosageMiniOuReel.TabIndex = 242
        chkDosageMiniOuReel.UseVisualStyleBackColor = True
        ' 
        ' Label71
        ' 
        Label71.AutoSize = True
        Label71.Location = New Point(366, 378)
        Label71.Name = "Label71"
        Label71.Size = New Size(149, 20)
        Label71.TabIndex = 241
        Label71.Text = "Dosage Mini Ou Réel"
        ' 
        ' TextBox2
        ' 
        TextBox2.Location = New Point(525, 371)
        TextBox2.Name = "TextBox2"
        TextBox2.Size = New Size(133, 27)
        TextBox2.TabIndex = 240
        ' 
        ' Label70
        ' 
        Label70.AutoSize = True
        Label70.Location = New Point(364, 162)
        Label70.Name = "Label70"
        Label70.Size = New Size(82, 20)
        Label70.TabIndex = 238
        Label70.Text = "Attestation"
        ' 
        ' Label11
        ' 
        Label11.AutoSize = True
        Label11.Location = New Point(366, 300)
        Label11.Name = "Label11"
        Label11.Size = New Size(84, 20)
        Label11.TabIndex = 234
        Label11.Text = "Rapport EC"
        ' 
        ' Label12
        ' 
        Label12.AutoSize = True
        Label12.Location = New Point(368, 267)
        Label12.Name = "Label12"
        Label12.Size = New Size(116, 20)
        Label12.TabIndex = 233
        Label12.Text = "Contrôle Norme"
        ' 
        ' Label17
        ' 
        Label17.AutoSize = True
        Label17.Location = New Point(366, 234)
        Label17.Name = "Label17"
        Label17.Size = New Size(125, 20)
        Label17.TabIndex = 232
        Label17.Text = "Granularité Dmax"
        ' 
        ' cmbConsistance
        ' 
        cmbConsistance.FormattingEnabled = True
        cmbConsistance.Location = New Point(183, 228)
        cmbConsistance.Name = "cmbConsistance"
        cmbConsistance.Size = New Size(167, 28)
        cmbConsistance.TabIndex = 231
        ' 
        ' cmbResistance
        ' 
        cmbResistance.FormattingEnabled = True
        cmbResistance.Location = New Point(184, 264)
        cmbResistance.Name = "cmbResistance"
        cmbResistance.Size = New Size(167, 28)
        cmbResistance.TabIndex = 230
        ' 
        ' cmbClasseChlorure
        ' 
        cmbClasseChlorure.FormattingEnabled = True
        cmbClasseChlorure.Location = New Point(183, 298)
        cmbClasseChlorure.Name = "cmbClasseChlorure"
        cmbClasseChlorure.Size = New Size(167, 28)
        cmbClasseChlorure.TabIndex = 229
        ' 
        ' cmbCertification
        ' 
        cmbCertification.FormattingEnabled = True
        cmbCertification.Location = New Point(183, 160)
        cmbCertification.Name = "cmbCertification"
        cmbCertification.Size = New Size(167, 28)
        cmbCertification.TabIndex = 228
        ' 
        ' Label14
        ' 
        Label14.AutoSize = True
        Label14.Location = New Point(131, 489)
        Label14.Name = "Label14"
        Label14.Size = New Size(127, 20)
        Label14.TabIndex = 227
        Label14.Text = "Remarque Norme"
        ' 
        ' txtAd
        ' 
        txtAd.AutoSize = True
        txtAd.Location = New Point(204, 449)
        txtAd.Name = "txtAd"
        txtAd.Size = New Size(74, 20)
        txtAd.TabIndex = 224
        txtAd.Text = "Adjuvants"
        ' 
        ' Label18
        ' 
        Label18.AutoSize = True
        Label18.Location = New Point(25, 297)
        Label18.Name = "Label18"
        Label18.Size = New Size(110, 20)
        Label18.TabIndex = 221
        Label18.Text = "Classe Chlorure"
        ' 
        ' Label19
        ' 
        Label19.AutoSize = True
        Label19.Location = New Point(27, 264)
        Label19.Name = "Label19"
        Label19.Size = New Size(78, 20)
        Label19.TabIndex = 220
        Label19.Text = "Résistance"
        ' 
        ' Label20
        ' 
        Label20.AutoSize = True
        Label20.Location = New Point(25, 231)
        Label20.Name = "Label20"
        Label20.Size = New Size(87, 20)
        Label20.TabIndex = 219
        Label20.Text = "Consistance"
        ' 
        ' Label21
        ' 
        Label21.AutoSize = True
        Label21.Location = New Point(25, 198)
        Label21.Name = "Label21"
        Label21.Size = New Size(105, 20)
        Label21.TabIndex = 218
        Label21.Text = "Nature Ciment"
        ' 
        ' Label22
        ' 
        Label22.AutoSize = True
        Label22.Location = New Point(22, 165)
        Label22.Name = "Label22"
        Label22.Size = New Size(90, 20)
        Label22.TabIndex = 217
        Label22.Text = "Certification"
        ' 
        ' Label23
        ' 
        Label23.AutoSize = True
        Label23.Location = New Point(25, 132)
        Label23.Name = "Label23"
        Label23.Size = New Size(89, 20)
        Label23.TabIndex = 216
        Label23.Text = "Désignation"
        ' 
        ' Label24
        ' 
        Label24.AutoSize = True
        Label24.Location = New Point(25, 99)
        Label24.Name = "Label24"
        Label24.Size = New Size(81, 20)
        Label24.TabIndex = 215
        Label24.Text = "Addition A"
        ' 
        ' Label25
        ' 
        Label25.AutoSize = True
        Label25.Location = New Point(26, 66)
        Label25.Name = "Label25"
        Label25.Size = New Size(123, 20)
        Label25.TabIndex = 214
        Label25.Text = "Classe Exposition"
        ' 
        ' txtAdjuvants
        ' 
        txtAdjuvants.Location = New Point(288, 446)
        txtAdjuvants.Name = "txtAdjuvants"
        txtAdjuvants.Size = New Size(418, 27)
        txtAdjuvants.TabIndex = 213
        ' 
        ' TabPage3
        ' 
        TabPage3.Controls.Add(Label30)
        TabPage3.Controls.Add(txtTarif2MontantFormule)
        TabPage3.Controls.Add(dtpTarif2DateFormule)
        TabPage3.Controls.Add(TextBox1)
        TabPage3.Controls.Add(TextBox4)
        TabPage3.Controls.Add(TextBox5)
        TabPage3.Controls.Add(Label16)
        TabPage3.Controls.Add(Label26)
        TabPage3.Controls.Add(Label27)
        TabPage3.Controls.Add(Label28)
        TabPage3.Controls.Add(Label29)
        TabPage3.Controls.Add(Label32)
        TabPage3.Controls.Add(Label33)
        TabPage3.Controls.Add(Label34)
        TabPage3.Controls.Add(Label35)
        TabPage3.Controls.Add(Label36)
        TabPage3.Controls.Add(Label37)
        TabPage3.Controls.Add(txtPrixRevientFormule)
        TabPage3.Controls.Add(txtTarif1MontantFormule)
        TabPage3.Controls.Add(dtpTarif1DateFormule)
        TabPage3.Controls.Add(txtTaxe2Formule)
        TabPage3.Controls.Add(txtTaxe1Formule)
        TabPage3.Controls.Add(txtTVAFormule)
        TabPage3.Controls.Add(txtNomProduitFormule)
        TabPage3.Controls.Add(txtCleProduitFormule)
        TabPage3.Controls.Add(txtCodeProduitFormule)
        TabPage3.Location = New Point(4, 29)
        TabPage3.Name = "TabPage3"
        TabPage3.Padding = New Padding(3)
        TabPage3.Size = New Size(864, 603)
        TabPage3.TabIndex = 2
        TabPage3.Text = "Facturation"
        TabPage3.UseVisualStyleBackColor = True
        ' 
        ' Label30
        ' 
        Label30.AutoSize = True
        Label30.Location = New Point(405, 305)
        Label30.Name = "Label30"
        Label30.Size = New Size(37, 20)
        Label30.TabIndex = 235
        Label30.Text = "Tarif"
        ' 
        ' txtTarif2MontantFormule
        ' 
        txtTarif2MontantFormule.Location = New Point(368, 365)
        txtTarif2MontantFormule.Name = "txtTarif2MontantFormule"
        txtTarif2MontantFormule.Size = New Size(120, 27)
        txtTarif2MontantFormule.TabIndex = 233
        ' 
        ' dtpTarif2DateFormule
        ' 
        dtpTarif2DateFormule.Location = New Point(368, 332)
        dtpTarif2DateFormule.Name = "dtpTarif2DateFormule"
        dtpTarif2DateFormule.Size = New Size(120, 27)
        dtpTarif2DateFormule.TabIndex = 232
        ' 
        ' TextBox1
        ' 
        TextBox1.Location = New Point(423, 231)
        TextBox1.Name = "TextBox1"
        TextBox1.Size = New Size(190, 27)
        TextBox1.TabIndex = 231
        ' 
        ' TextBox4
        ' 
        TextBox4.Location = New Point(423, 198)
        TextBox4.Name = "TextBox4"
        TextBox4.Size = New Size(191, 27)
        TextBox4.TabIndex = 230
        ' 
        ' TextBox5
        ' 
        TextBox5.Location = New Point(423, 165)
        TextBox5.Name = "TextBox5"
        TextBox5.Size = New Size(190, 27)
        TextBox5.TabIndex = 229
        ' 
        ' Label16
        ' 
        Label16.AutoSize = True
        Label16.Location = New Point(422, 70)
        Label16.Name = "Label16"
        Label16.Size = New Size(99, 20)
        Label16.TabIndex = 228
        Label16.Text = "Code Camion"
        ' 
        ' Label26
        ' 
        Label26.AutoSize = True
        Label26.Location = New Point(260, 305)
        Label26.Name = "Label26"
        Label26.Size = New Size(37, 20)
        Label26.TabIndex = 227
        Label26.Text = "Tarif"
        ' 
        ' Label27
        ' 
        Label27.AutoSize = True
        Label27.Location = New Point(34, 401)
        Label27.Name = "Label27"
        Label27.Size = New Size(78, 20)
        Label27.TabIndex = 224
        Label27.Text = "Téléphone"
        ' 
        ' Label28
        ' 
        Label28.AutoSize = True
        Label28.Location = New Point(36, 368)
        Label28.Name = "Label28"
        Label28.Size = New Size(167, 20)
        Label28.TabIndex = 223
        Label28.Text = "Tarif 1 Montant Formule"
        ' 
        ' Label29
        ' 
        Label29.AutoSize = True
        Label29.Location = New Point(39, 335)
        Label29.Name = "Label29"
        Label29.Size = New Size(99, 20)
        Label29.TabIndex = 222
        Label29.Text = "Date Formule"
        ' 
        ' Label32
        ' 
        Label32.AutoSize = True
        Label32.Location = New Point(37, 236)
        Label32.Name = "Label32"
        Label32.Size = New Size(108, 20)
        Label32.TabIndex = 219
        Label32.Text = "Taxe 2 Formule"
        ' 
        ' Label33
        ' 
        Label33.AutoSize = True
        Label33.Location = New Point(37, 203)
        Label33.Name = "Label33"
        Label33.Size = New Size(108, 20)
        Label33.TabIndex = 218
        Label33.Text = "Taxe 1 Formule"
        ' 
        ' Label34
        ' 
        Label34.AutoSize = True
        Label34.Location = New Point(34, 170)
        Label34.Name = "Label34"
        Label34.Size = New Size(93, 20)
        Label34.TabIndex = 217
        Label34.Text = "TVA Formule"
        ' 
        ' Label35
        ' 
        Label35.AutoSize = True
        Label35.Location = New Point(37, 137)
        Label35.Name = "Label35"
        Label35.Size = New Size(152, 20)
        Label35.TabIndex = 216
        Label35.Text = "Nom Produit Formule"
        ' 
        ' Label36
        ' 
        Label36.AutoSize = True
        Label36.Location = New Point(37, 104)
        Label36.Name = "Label36"
        Label36.Size = New Size(140, 20)
        Label36.TabIndex = 215
        Label36.Text = "Cle Produit Formule"
        ' 
        ' Label37
        ' 
        Label37.AutoSize = True
        Label37.Location = New Point(40, 70)
        Label37.Name = "Label37"
        Label37.Size = New Size(154, 20)
        Label37.TabIndex = 214
        Label37.Text = "Code Produit Formule"
        ' 
        ' txtPrixRevientFormule
        ' 
        txtPrixRevientFormule.Location = New Point(217, 394)
        txtPrixRevientFormule.Name = "txtPrixRevientFormule"
        txtPrixRevientFormule.Size = New Size(120, 27)
        txtPrixRevientFormule.TabIndex = 213
        ' 
        ' txtTarif1MontantFormule
        ' 
        txtTarif1MontantFormule.Location = New Point(217, 361)
        txtTarif1MontantFormule.Name = "txtTarif1MontantFormule"
        txtTarif1MontantFormule.Size = New Size(120, 27)
        txtTarif1MontantFormule.TabIndex = 212
        ' 
        ' dtpTarif1DateFormule
        ' 
        dtpTarif1DateFormule.Location = New Point(217, 328)
        dtpTarif1DateFormule.Name = "dtpTarif1DateFormule"
        dtpTarif1DateFormule.Size = New Size(120, 27)
        dtpTarif1DateFormule.TabIndex = 211
        ' 
        ' txtTaxe2Formule
        ' 
        txtTaxe2Formule.Location = New Point(196, 232)
        txtTaxe2Formule.Name = "txtTaxe2Formule"
        txtTaxe2Formule.Size = New Size(190, 27)
        txtTaxe2Formule.TabIndex = 208
        ' 
        ' txtTaxe1Formule
        ' 
        txtTaxe1Formule.Location = New Point(196, 199)
        txtTaxe1Formule.Name = "txtTaxe1Formule"
        txtTaxe1Formule.Size = New Size(191, 27)
        txtTaxe1Formule.TabIndex = 207
        ' 
        ' txtTVAFormule
        ' 
        txtTVAFormule.Location = New Point(196, 166)
        txtTVAFormule.Name = "txtTVAFormule"
        txtTVAFormule.Size = New Size(190, 27)
        txtTVAFormule.TabIndex = 206
        ' 
        ' txtNomProduitFormule
        ' 
        txtNomProduitFormule.Location = New Point(195, 133)
        txtNomProduitFormule.Name = "txtNomProduitFormule"
        txtNomProduitFormule.Size = New Size(418, 27)
        txtNomProduitFormule.TabIndex = 205
        ' 
        ' txtCleProduitFormule
        ' 
        txtCleProduitFormule.Location = New Point(197, 102)
        txtCleProduitFormule.Name = "txtCleProduitFormule"
        txtCleProduitFormule.Size = New Size(418, 27)
        txtCleProduitFormule.TabIndex = 204
        ' 
        ' txtCodeProduitFormule
        ' 
        txtCodeProduitFormule.Location = New Point(196, 67)
        txtCodeProduitFormule.Name = "txtCodeProduitFormule"
        txtCodeProduitFormule.Size = New Size(220, 27)
        txtCodeProduitFormule.TabIndex = 203
        ' 
        ' TabPage4
        ' 
        TabPage4.Controls.Add(cmbDestinationAdjuvantPese)
        TabPage4.Controls.Add(txtNiveauPerformance)
        TabPage4.Controls.Add(txtReductionChargeMaxiTremie)
        TabPage4.Controls.Add(ListBox3)
        TabPage4.Controls.Add(chkGestionAjoutEauCompte)
        TabPage4.Controls.Add(chkFormuleInactive)
        TabPage4.Controls.Add(chkValidationPresenceBenne)
        TabPage4.Controls.Add(chkQteLivreeEgaleFabriquee)
        TabPage4.Controls.Add(chkImpressionBLPoidsTonnes)
        TabPage4.Controls.Add(ListBox2)
        TabPage4.Controls.Add(chkGacheeSeche)
        TabPage4.Controls.Add(Label38)
        TabPage4.Controls.Add(Label39)
        TabPage4.Controls.Add(Label40)
        TabPage4.Controls.Add(Label42)
        TabPage4.Controls.Add(Label43)
        TabPage4.Controls.Add(Label44)
        TabPage4.Controls.Add(Label45)
        TabPage4.Controls.Add(Label46)
        TabPage4.Controls.Add(Label47)
        TabPage4.Controls.Add(Label48)
        TabPage4.Controls.Add(Label49)
        TabPage4.Location = New Point(4, 29)
        TabPage4.Name = "TabPage4"
        TabPage4.Size = New Size(864, 603)
        TabPage4.TabIndex = 3
        TabPage4.Text = "Divers"
        TabPage4.UseVisualStyleBackColor = True
        ' 
        ' cmbDestinationAdjuvantPese
        ' 
        cmbDestinationAdjuvantPese.FormattingEnabled = True
        cmbDestinationAdjuvantPese.Location = New Point(330, 337)
        cmbDestinationAdjuvantPese.Name = "cmbDestinationAdjuvantPese"
        cmbDestinationAdjuvantPese.Size = New Size(235, 28)
        cmbDestinationAdjuvantPese.TabIndex = 262
        ' 
        ' txtNiveauPerformance
        ' 
        txtNiveauPerformance.Location = New Point(183, 272)
        txtNiveauPerformance.Name = "txtNiveauPerformance"
        txtNiveauPerformance.Size = New Size(418, 27)
        txtNiveauPerformance.TabIndex = 261
        ' 
        ' txtReductionChargeMaxiTremie
        ' 
        txtReductionChargeMaxiTremie.Location = New Point(183, 243)
        txtReductionChargeMaxiTremie.Name = "txtReductionChargeMaxiTremie"
        txtReductionChargeMaxiTremie.Size = New Size(418, 27)
        txtReductionChargeMaxiTremie.TabIndex = 260
        ' 
        ' ListBox3
        ' 
        ListBox3.FormattingEnabled = True
        ListBox3.Location = New Point(183, 385)
        ListBox3.Name = "ListBox3"
        ListBox3.Size = New Size(409, 84)
        ListBox3.TabIndex = 259
        ' 
        ' chkGestionAjoutEauCompte
        ' 
        chkGestionAjoutEauCompte.Location = New Point(256, 169)
        chkGestionAjoutEauCompte.Name = "chkGestionAjoutEauCompte"
        chkGestionAjoutEauCompte.Size = New Size(74, 31)
        chkGestionAjoutEauCompte.TabIndex = 258
        chkGestionAjoutEauCompte.UseVisualStyleBackColor = True
        ' 
        ' chkFormuleInactive
        ' 
        chkFormuleInactive.Location = New Point(256, 202)
        chkFormuleInactive.Name = "chkFormuleInactive"
        chkFormuleInactive.Size = New Size(74, 31)
        chkFormuleInactive.TabIndex = 257
        chkFormuleInactive.UseVisualStyleBackColor = True
        ' 
        ' chkValidationPresenceBenne
        ' 
        chkValidationPresenceBenne.Location = New Point(256, 136)
        chkValidationPresenceBenne.Name = "chkValidationPresenceBenne"
        chkValidationPresenceBenne.Size = New Size(74, 31)
        chkValidationPresenceBenne.TabIndex = 256
        chkValidationPresenceBenne.UseVisualStyleBackColor = True
        ' 
        ' chkQteLivreeEgaleFabriquee
        ' 
        chkQteLivreeEgaleFabriquee.Location = New Point(256, 70)
        chkQteLivreeEgaleFabriquee.Name = "chkQteLivreeEgaleFabriquee"
        chkQteLivreeEgaleFabriquee.Size = New Size(74, 31)
        chkQteLivreeEgaleFabriquee.TabIndex = 252
        chkQteLivreeEgaleFabriquee.UseVisualStyleBackColor = True
        ' 
        ' chkImpressionBLPoidsTonnes
        ' 
        chkImpressionBLPoidsTonnes.Location = New Point(256, 103)
        chkImpressionBLPoidsTonnes.Name = "chkImpressionBLPoidsTonnes"
        chkImpressionBLPoidsTonnes.Size = New Size(74, 31)
        chkImpressionBLPoidsTonnes.TabIndex = 251
        chkImpressionBLPoidsTonnes.UseVisualStyleBackColor = True
        ' 
        ' ListBox2
        ' 
        ListBox2.FormattingEnabled = True
        ListBox2.Location = New Point(183, 493)
        ListBox2.Name = "ListBox2"
        ListBox2.Size = New Size(409, 84)
        ListBox2.TabIndex = 245
        ' 
        ' chkGacheeSeche
        ' 
        chkGacheeSeche.Location = New Point(256, 37)
        chkGacheeSeche.Name = "chkGacheeSeche"
        chkGacheeSeche.Size = New Size(74, 31)
        chkGacheeSeche.TabIndex = 244
        chkGacheeSeche.UseVisualStyleBackColor = True
        ' 
        ' Label38
        ' 
        Label38.AutoSize = True
        Label38.Location = New Point(26, 479)
        Label38.Name = "Label38"
        Label38.Size = New Size(149, 20)
        Label38.TabIndex = 227
        Label38.Text = "Remarque centrale"
        ' 
        ' Label39
        ' 
        Label39.AutoSize = True
        Label39.Location = New Point(29, 385)
        Label39.Name = "Label39"
        Label39.Size = New Size(171, 20)
        Label39.TabIndex = 224
        Label39.Text = "Remarque laboratoire"
        ' 
        ' Label40
        ' 
        Label40.AutoSize = True
        Label40.Location = New Point(24, 345)
        Label40.Name = "Label40"
        Label40.Size = New Size(202, 20)
        Label40.TabIndex = 223
        Label40.Text = "Destination adjuvant pesé"
        ' 
        ' Label42
        ' 
        Label42.AutoSize = True
        Label42.Location = New Point(25, 279)
        Label42.Name = "Label42"
        Label42.Size = New Size(155, 20)
        Label42.TabIndex = 221
        Label42.Text = "Niveau performance"
        ' 
        ' Label43
        ' 
        Label43.AutoSize = True
        Label43.Location = New Point(27, 246)
        Label43.Name = "Label43"
        Label43.Size = New Size(217, 20)
        Label43.TabIndex = 220
        Label43.Text = "Réduction charge maxi trémie"
        ' 
        ' Label44
        ' 
        Label44.AutoSize = True
        Label44.Location = New Point(25, 213)
        Label44.Name = "Label44"
        Label44.Size = New Size(136, 20)
        Label44.TabIndex = 219
        Label44.Text = "Formule inactive"
        ' 
        ' Label45
        ' 
        Label45.AutoSize = True
        Label45.Location = New Point(25, 180)
        Label45.Name = "Label45"
        Label45.Size = New Size(184, 20)
        Label45.TabIndex = 218
        Label45.Text = "Gestion Ajout Eau Compte"
        ' 
        ' Label46
        ' 
        Label46.AutoSize = True
        Label46.Location = New Point(22, 147)
        Label46.Name = "Label46"
        Label46.Size = New Size(183, 20)
        Label46.TabIndex = 217
        Label46.Text = "Validation Presence Benne"
        ' 
        ' Label47
        ' 
        Label47.AutoSize = True
        Label47.Location = New Point(25, 114)
        Label47.Name = "Label47"
        Label47.Size = New Size(190, 20)
        Label47.TabIndex = 216
        Label47.Text = "Impression BL Poids Tonnes"
        ' 
        ' Label48
        ' 
        Label48.AutoSize = True
        Label48.Location = New Point(25, 81)
        Label48.Name = "Label48"
        Label48.Size = New Size(186, 20)
        Label48.TabIndex = 215
        Label48.Text = "Qté Livrée Égale Fabriquée"
        ' 
        ' Label49
        ' 
        Label49.AutoSize = True
        Label49.Location = New Point(25, 48)
        Label49.Name = "Label49"
        Label49.Size = New Size(101, 20)
        Label49.TabIndex = 214
        Label49.Text = "Gâchée Sèche"
        ' 
        ' TabPage5
        ' 
        TabPage5.Controls.Add(Label5)
        TabPage5.Controls.Add(Label6)
        TabPage5.Controls.Add(txtDefautPlasticiteMax)
        TabPage5.Controls.Add(txtDefautPlasticiteMin)
        TabPage5.Controls.Add(Label7)
        TabPage5.Controls.Add(Label8)
        TabPage5.Controls.Add(txtPourcentageEauPesee)
        TabPage5.Controls.Add(txtEauPlasticite)
        TabPage5.Controls.Add(Label59)
        TabPage5.Controls.Add(Label60)
        TabPage5.Controls.Add(txtTempsHygrometre)
        TabPage5.Controls.Add(txtEauHygrometre)
        TabPage5.Controls.Add(Label55)
        TabPage5.Controls.Add(Label56)
        TabPage5.Controls.Add(Label57)
        TabPage5.Controls.Add(txtPhaseHygrometre)
        TabPage5.Controls.Add(txtNumeroProgrammeHygrometre)
        TabPage5.Controls.Add(txtToleranceEauMax)
        TabPage5.Controls.Add(Label52)
        TabPage5.Controls.Add(Label53)
        TabPage5.Controls.Add(Label54)
        TabPage5.Controls.Add(txtToleranceEauMin)
        TabPage5.Controls.Add(txtTempsMalaxagePlasticite)
        TabPage5.Controls.Add(txtTempsDecalagePlasticite)
        TabPage5.Controls.Add(Label31)
        TabPage5.Controls.Add(Label51)
        TabPage5.Controls.Add(Label50)
        TabPage5.Controls.Add(txtPuissanceVisee)
        TabPage5.Controls.Add(txtPasPlasticite)
        TabPage5.Controls.Add(txtPhaseEauPlasticite)
        TabPage5.Location = New Point(4, 29)
        TabPage5.Name = "TabPage5"
        TabPage5.Size = New Size(864, 603)
        TabPage5.TabIndex = 4
        TabPage5.Text = "Gestion Plasticité"
        TabPage5.UseVisualStyleBackColor = True
        ' 
        ' Label5
        ' 
        Label5.AutoSize = True
        Label5.Location = New Point(465, 294)
        Label5.Name = "Label5"
        Label5.Size = New Size(158, 20)
        Label5.TabIndex = 274
        Label5.Text = "Défaut plasticité max"
        ' 
        ' Label6
        ' 
        Label6.AutoSize = True
        Label6.Location = New Point(462, 261)
        Label6.Name = "Label6"
        Label6.Size = New Size(155, 20)
        Label6.TabIndex = 273
        Label6.Text = "Défaut plasticité min"
        ' 
        ' txtDefautPlasticiteMax
        ' 
        txtDefautPlasticiteMax.Location = New Point(624, 290)
        txtDefautPlasticiteMax.Name = "txtDefautPlasticiteMax"
        txtDefautPlasticiteMax.Size = New Size(222, 27)
        txtDefautPlasticiteMax.TabIndex = 272
        ' 
        ' txtDefautPlasticiteMin
        ' 
        txtDefautPlasticiteMin.Location = New Point(624, 257)
        txtDefautPlasticiteMin.Name = "txtDefautPlasticiteMin"
        txtDefautPlasticiteMin.Size = New Size(221, 27)
        txtDefautPlasticiteMin.TabIndex = 271
        ' 
        ' Label7
        ' 
        Label7.AutoSize = True
        Label7.Location = New Point(465, 147)
        Label7.Name = "Label7"
        Label7.Size = New Size(169, 20)
        Label7.TabIndex = 270
        Label7.Text = "Pourcentage eau pesée"
        ' 
        ' Label8
        ' 
        Label8.AutoSize = True
        Label8.Location = New Point(465, 114)
        Label8.Name = "Label8"
        Label8.Size = New Size(109, 20)
        Label8.TabIndex = 269
        Label8.Text = "Eau plasticité"
        ' 
        ' txtPourcentageEauPesee
        ' 
        txtPourcentageEauPesee.Location = New Point(624, 143)
        txtPourcentageEauPesee.Name = "txtPourcentageEauPesee"
        txtPourcentageEauPesee.Size = New Size(221, 27)
        txtPourcentageEauPesee.TabIndex = 268
        ' 
        ' txtEauPlasticite
        ' 
        txtEauPlasticite.Location = New Point(624, 110)
        txtEauPlasticite.Name = "txtEauPlasticite"
        txtEauPlasticite.Size = New Size(222, 27)
        txtEauPlasticite.TabIndex = 267
        ' 
        ' Label59
        ' 
        Label59.AutoSize = True
        Label59.Location = New Point(66, 395)
        Label59.Name = "Label59"
        Label59.Size = New Size(149, 20)
        Label59.TabIndex = 266
        Label59.Text = "Temps hygromètre"
        ' 
        ' Label60
        ' 
        Label60.AutoSize = True
        Label60.Location = New Point(63, 362)
        Label60.Name = "Label60"
        Label60.Size = New Size(130, 20)
        Label60.TabIndex = 265
        Label60.Text = "Eau hygromètre"
        ' 
        ' txtTempsHygrometre
        ' 
        txtTempsHygrometre.Location = New Point(225, 391)
        txtTempsHygrometre.Name = "txtTempsHygrometre"
        txtTempsHygrometre.Size = New Size(222, 27)
        txtTempsHygrometre.TabIndex = 263
        ' 
        ' txtEauHygrometre
        ' 
        txtEauHygrometre.Location = New Point(225, 358)
        txtEauHygrometre.Name = "txtEauHygrometre"
        txtEauHygrometre.Size = New Size(221, 27)
        txtEauHygrometre.TabIndex = 262
        ' 
        ' Label55
        ' 
        Label55.AutoSize = True
        Label55.Location = New Point(66, 330)
        Label55.Name = "Label55"
        Label55.Size = New Size(144, 20)
        Label55.TabIndex = 261
        Label55.Text = "Phase hygromètre"
        ' 
        ' Label56
        ' 
        Label56.AutoSize = True
        Label56.Location = New Point(66, 297)
        Label56.Name = "Label56"
        Label56.Size = New Size(238, 20)
        Label56.TabIndex = 260
        Label56.Text = "N° programme hygromètre"
        ' 
        ' Label57
        ' 
        Label57.AutoSize = True
        Label57.Location = New Point(63, 264)
        Label57.Name = "Label57"
        Label57.Size = New Size(142, 20)
        Label57.TabIndex = 259
        Label57.Text = "Tolérance eau max"
        ' 
        ' txtPhaseHygrometre
        ' 
        txtPhaseHygrometre.Location = New Point(225, 326)
        txtPhaseHygrometre.Name = "txtPhaseHygrometre"
        txtPhaseHygrometre.Size = New Size(221, 27)
        txtPhaseHygrometre.TabIndex = 258
        ' 
        ' txtNumeroProgrammeHygrometre
        ' 
        txtNumeroProgrammeHygrometre.Location = New Point(225, 293)
        txtNumeroProgrammeHygrometre.Name = "txtNumeroProgrammeHygrometre"
        txtNumeroProgrammeHygrometre.Size = New Size(222, 27)
        txtNumeroProgrammeHygrometre.TabIndex = 257
        ' 
        ' txtToleranceEauMax
        ' 
        txtToleranceEauMax.Location = New Point(225, 260)
        txtToleranceEauMax.Name = "txtToleranceEauMax"
        txtToleranceEauMax.Size = New Size(221, 27)
        txtToleranceEauMax.TabIndex = 256
        ' 
        ' Label52
        ' 
        Label52.AutoSize = True
        Label52.Location = New Point(66, 229)
        Label52.Name = "Label52"
        Label52.Size = New Size(219, 20)
        Label52.TabIndex = 255
        Label52.Text = "EquipementtxtToleranceEauMin"
        ' 
        ' Label53
        ' 
        Label53.AutoSize = True
        Label53.Location = New Point(66, 196)
        Label53.Name = "Label53"
        Label53.Size = New Size(193, 20)
        Label53.TabIndex = 254
        Label53.Text = "Temps malaxage plasticité"
        ' 
        ' Label54
        ' 
        Label54.AutoSize = True
        Label54.Location = New Point(63, 163)
        Label54.Name = "Label54"
        Label54.Size = New Size(191, 20)
        Label54.TabIndex = 253
        Label54.Text = "Temps décalage plasticité"
        ' 
        ' txtToleranceEauMin
        ' 
        txtToleranceEauMin.Location = New Point(225, 225)
        txtToleranceEauMin.Name = "txtToleranceEauMin"
        txtToleranceEauMin.Size = New Size(221, 27)
        txtToleranceEauMin.TabIndex = 252
        ' 
        ' txtTempsMalaxagePlasticite
        ' 
        txtTempsMalaxagePlasticite.Location = New Point(225, 192)
        txtTempsMalaxagePlasticite.Name = "txtTempsMalaxagePlasticite"
        txtTempsMalaxagePlasticite.Size = New Size(222, 27)
        txtTempsMalaxagePlasticite.TabIndex = 251
        ' 
        ' txtTempsDecalagePlasticite
        ' 
        txtTempsDecalagePlasticite.Location = New Point(225, 159)
        txtTempsDecalagePlasticite.Name = "txtTempsDecalagePlasticite"
        txtTempsDecalagePlasticite.Size = New Size(221, 27)
        txtTempsDecalagePlasticite.TabIndex = 250
        ' 
        ' Label31
        ' 
        Label31.AutoSize = True
        Label31.Location = New Point(63, 126)
        Label31.Name = "Label31"
        Label31.Size = New Size(147, 20)
        Label31.TabIndex = 249
        Label31.Text = "Phase eau plasticité"
        ' 
        ' Label51
        ' 
        Label51.AutoSize = True
        Label51.Location = New Point(60, 60)
        Label51.Name = "Label51"
        Label51.Size = New Size(124, 20)
        Label51.TabIndex = 247
        Label51.Text = "Puissance visée"
        ' 
        ' Label50
        ' 
        Label50.AutoSize = True
        Label50.Location = New Point(63, 93)
        Label50.Name = "Label50"
        Label50.Size = New Size(106, 20)
        Label50.TabIndex = 248
        Label50.Text = "Pas plasticité"
        ' 
        ' txtPuissanceVisee
        ' 
        txtPuissanceVisee.Location = New Point(222, 56)
        txtPuissanceVisee.Name = "txtPuissanceVisee"
        txtPuissanceVisee.Size = New Size(221, 27)
        txtPuissanceVisee.TabIndex = 244
        ' 
        ' txtPasPlasticite
        ' 
        txtPasPlasticite.Location = New Point(222, 89)
        txtPasPlasticite.Name = "txtPasPlasticite"
        txtPasPlasticite.Size = New Size(222, 27)
        txtPasPlasticite.TabIndex = 245
        ' 
        ' txtPhaseEauPlasticite
        ' 
        txtPhaseEauPlasticite.Location = New Point(222, 122)
        txtPhaseEauPlasticite.Name = "txtPhaseEauPlasticite"
        txtPhaseEauPlasticite.Size = New Size(221, 27)
        txtPhaseEauPlasticite.TabIndex = 246
        ' 
        ' txtCleRechercheFormule
        ' 
        txtCleRechercheFormule.Location = New Point(241, 125)
        txtCleRechercheFormule.Name = "txtCleRechercheFormule"
        txtCleRechercheFormule.Size = New Size(391, 27)
        txtCleRechercheFormule.TabIndex = 200
        ' 
        ' Label3
        ' 
        Label3.AutoSize = True
        Label3.Location = New Point(84, 223)
        Label3.Name = "Label3"
        Label3.Size = New Size(99, 20)
        Label3.TabIndex = 191
        Label3.Text = "Famille Béton"
        ' 
        ' Label2
        ' 
        Label2.AutoSize = True
        Label2.Location = New Point(84, 190)
        Label2.Name = "Label2"
        Label2.Size = New Size(100, 20)
        Label2.TabIndex = 190
        Label2.Text = "Nom Formule"
        ' 
        ' Label1
        ' 
        Label1.AutoSize = True
        Label1.Location = New Point(80, 157)
        Label1.Name = "Label1"
        Label1.Size = New Size(102, 20)
        Label1.TabIndex = 189
        Label1.Text = "Code Formule"
        ' 
        ' txtFamilleBeton
        ' 
        txtFamilleBeton.Location = New Point(241, 227)
        txtFamilleBeton.Name = "txtFamilleBeton"
        txtFamilleBeton.Size = New Size(391, 27)
        txtFamilleBeton.TabIndex = 180
        ' 
        ' txtNomFormule
        ' 
        txtNomFormule.Location = New Point(241, 194)
        txtNomFormule.Name = "txtNomFormule"
        txtNomFormule.Size = New Size(391, 27)
        txtNomFormule.TabIndex = 179
        ' 
        ' txtCodeFormule
        ' 
        txtCodeFormule.Location = New Point(241, 161)
        txtCodeFormule.Name = "txtCodeFormule"
        txtCodeFormule.Size = New Size(391, 27)
        txtCodeFormule.TabIndex = 178
        ' 
        ' Label63
        ' 
        Label63.AutoSize = True
        Label63.Location = New Point(667, 164)
        Label63.Name = "Label63"
        Label63.Size = New Size(121, 20)
        Label63.TabIndex = 204
        Label63.Text = "Temps Malaxage"
        ' 
        ' Label64
        ' 
        Label64.AutoSize = True
        Label64.Location = New Point(667, 197)
        Label64.Name = "Label64"
        Label64.Size = New Size(38, 20)
        Label64.TabIndex = 205
        Label64.Text = "MES"
        ' 
        ' txtTempsMalaxage
        ' 
        txtTempsMalaxage.Location = New Point(800, 161)
        txtTempsMalaxage.Name = "txtTempsMalaxage"
        txtTempsMalaxage.Size = New Size(378, 27)
        txtTempsMalaxage.TabIndex = 201
        ' 
        ' txtMES
        ' 
        txtMES.Location = New Point(800, 194)
        txtMES.Name = "txtMES"
        txtMES.Size = New Size(378, 27)
        txtMES.TabIndex = 202
        ' 
        ' txtVolumeAir
        ' 
        txtVolumeAir.Location = New Point(800, 227)
        txtVolumeAir.Name = "txtVolumeAir"
        txtVolumeAir.Size = New Size(378, 27)
        txtVolumeAir.TabIndex = 203
        ' 
        ' Label62
        ' 
        Label62.AutoSize = True
        Label62.Location = New Point(667, 230)
        Label62.Name = "Label62"
        Label62.Size = New Size(82, 20)
        Label62.TabIndex = 206
        Label62.Text = "Volume Air"
        ' 
        ' txtGamme
        ' 
        txtGamme.Location = New Point(800, 128)
        txtGamme.Name = "txtGamme"
        txtGamme.Size = New Size(378, 27)
        txtGamme.TabIndex = 207
        ' 
        ' btnImprimerFormule
        ' 
        btnImprimerFormule.Location = New Point(684, 32)
        btnImprimerFormule.Name = "btnImprimerFormule"
        btnImprimerFormule.Size = New Size(140, 61)
        btnImprimerFormule.TabIndex = 214
        btnImprimerFormule.Text = "Imprimer"
        btnImprimerFormule.UseVisualStyleBackColor = True
        ' 
        ' btnSupprimerFormule
        ' 
        btnSupprimerFormule.Location = New Point(538, 32)
        btnSupprimerFormule.Name = "btnSupprimerFormule"
        btnSupprimerFormule.Size = New Size(140, 61)
        btnSupprimerFormule.TabIndex = 211
        btnSupprimerFormule.Text = "Supprimer"
        btnSupprimerFormule.UseVisualStyleBackColor = True
        ' 
        ' btnDupliquerFormule
        ' 
        btnDupliquerFormule.Location = New Point(392, 32)
        btnDupliquerFormule.Name = "btnDupliquerFormule"
        btnDupliquerFormule.Size = New Size(140, 61)
        btnDupliquerFormule.TabIndex = 210
        btnDupliquerFormule.Text = "Dupliquer"
        btnDupliquerFormule.UseVisualStyleBackColor = True
        ' 
        ' btnModifierFormule
        ' 
        btnModifierFormule.Location = New Point(246, 32)
        btnModifierFormule.Name = "btnModifierFormule"
        btnModifierFormule.Size = New Size(140, 61)
        btnModifierFormule.TabIndex = 209
        btnModifierFormule.Text = "Modifier"
        btnModifierFormule.UseVisualStyleBackColor = True
        ' 
        ' btnAjouterFormule
        ' 
        btnAjouterFormule.Location = New Point(84, 32)
        btnAjouterFormule.Name = "btnAjouterFormule"
        btnAjouterFormule.Size = New Size(140, 61)
        btnAjouterFormule.TabIndex = 208
        btnAjouterFormule.Text = "Ajouter"
        btnAjouterFormule.UseVisualStyleBackColor = True
        ' 
        ' Label9
        ' 
        Label9.AutoSize = True
        Label9.Location = New Point(80, 128)
        Label9.Name = "Label9"
        Label9.Size = New Size(160, 20)
        Label9.TabIndex = 215
        Label9.Text = "Cle Recherche Formule"
        ' 
        ' Label10
        ' 
        Label10.AutoSize = True
        Label10.Location = New Point(667, 135)
        Label10.Name = "Label10"
        Label10.Size = New Size(61, 20)
        Label10.TabIndex = 216
        Label10.Text = "Gamme"
        ' 
        ' Label41
        ' 
        Label41.AutoSize = True
        Label41.Location = New Point(919, 267)
        Label41.Name = "Label41"
        Label41.Size = New Size(130, 20)
        Label41.TabIndex = 277
        Label41.Text = "Taux Chlorure Calc"
        ' 
        ' Label61
        ' 
        Label61.AutoSize = True
        Label61.Location = New Point(922, 300)
        Label61.Name = "Label61"
        Label61.Size = New Size(131, 20)
        Label61.TabIndex = 278
        Label61.Text = "Masse Alcalin Calc"
        ' 
        ' txtTauxChlorureCalc
        ' 
        txtTauxChlorureCalc.Location = New Point(1081, 263)
        txtTauxChlorureCalc.Name = "txtTauxChlorureCalc"
        txtTauxChlorureCalc.Size = New Size(221, 27)
        txtTauxChlorureCalc.TabIndex = 275
        ' 
        ' txtMasseAlcalinCalc
        ' 
        txtMasseAlcalinCalc.Location = New Point(1081, 296)
        txtMasseAlcalinCalc.Name = "txtMasseAlcalinCalc"
        txtMasseAlcalinCalc.Size = New Size(222, 27)
        txtMasseAlcalinCalc.TabIndex = 276
        ' 
        ' btnActualiserFormule
        ' 
        btnActualiserFormule.Location = New Point(1122, 32)
        btnActualiserFormule.Name = "btnActualiserFormule"
        btnActualiserFormule.Size = New Size(140, 61)
        btnActualiserFormule.TabIndex = 281
        btnActualiserFormule.Text = "Actualiser"
        btnActualiserFormule.UseVisualStyleBackColor = True
        ' 
        ' btnAbandonnerFormule
        ' 
        btnAbandonnerFormule.Location = New Point(976, 32)
        btnAbandonnerFormule.Name = "btnAbandonnerFormule"
        btnAbandonnerFormule.Size = New Size(140, 61)
        btnAbandonnerFormule.TabIndex = 280
        btnAbandonnerFormule.Text = "Abandonner"
        btnAbandonnerFormule.UseVisualStyleBackColor = True
        ' 
        ' btnValiderFormule
        ' 
        btnValiderFormule.Location = New Point(830, 32)
        btnValiderFormule.Name = "btnValiderFormule"
        btnValiderFormule.Size = New Size(140, 61)
        btnValiderFormule.TabIndex = 279
        btnValiderFormule.Text = "Valider"
        btnValiderFormule.UseVisualStyleBackColor = True
        ' 
        ' TextBox3
        ' 
        TextBox3.Location = New Point(1197, 216)
        TextBox3.Name = "TextBox3"
        TextBox3.Size = New Size(166, 27)
        TextBox3.TabIndex = 249
        ' 
        ' cmbUniteFormule
        ' 
        cmbUniteFormule.FormattingEnabled = True
        cmbUniteFormule.Location = New Point(699, 283)
        cmbUniteFormule.Name = "cmbUniteFormule"
        cmbUniteFormule.Size = New Size(167, 28)
        cmbUniteFormule.TabIndex = 249
        ' 
        ' Label15
        ' 
        Label15.AutoSize = True
        Label15.Location = New Point(566, 291)
        Label15.Name = "Label15"
        Label15.Size = New Size(127, 20)
        Label15.TabIndex = 282
        Label15.Text = "Unité formule"
        ' 
        ' btnEnregistrerFormule
        ' 
        btnEnregistrerFormule.Location = New Point(1268, 32)
        btnEnregistrerFormule.Name = "btnEnregistrerFormule"
        btnEnregistrerFormule.Size = New Size(140, 61)
        btnEnregistrerFormule.TabIndex = 283
        btnEnregistrerFormule.Text = "Enregistrer"
        btnEnregistrerFormule.UseVisualStyleBackColor = True
        ' 
        ' Formules
        ' 
        AutoScaleDimensions = New SizeF(8F, 20F)
        AutoScaleMode = AutoScaleMode.Font
        ClientSize = New Size(1729, 996)
        Controls.Add(btnEnregistrerFormule)
        Controls.Add(Label15)
        Controls.Add(cmbUniteFormule)
        Controls.Add(TextBox3)
        Controls.Add(btnActualiserFormule)
        Controls.Add(btnAbandonnerFormule)
        Controls.Add(btnValiderFormule)
        Controls.Add(Label41)
        Controls.Add(Label10)
        Controls.Add(Label61)
        Controls.Add(Label9)
        Controls.Add(txtTauxChlorureCalc)
        Controls.Add(txtMasseAlcalinCalc)
        Controls.Add(btnImprimerFormule)
        Controls.Add(btnSupprimerFormule)
        Controls.Add(btnDupliquerFormule)
        Controls.Add(btnModifierFormule)
        Controls.Add(btnAjouterFormule)
        Controls.Add(txtGamme)
        Controls.Add(Label62)
        Controls.Add(Label63)
        Controls.Add(Label64)
        Controls.Add(txtTempsMalaxage)
        Controls.Add(txtMES)
        Controls.Add(txtVolumeAir)
        Controls.Add(TabControl1)
        Controls.Add(Label13)
        Controls.Add(txtCleRechercheFormule)
        Controls.Add(DataGridViewFormules)
        Controls.Add(Label3)
        Controls.Add(Label1)
        Controls.Add(Label2)
        Controls.Add(txtCodeFormule)
        Controls.Add(txtNomFormule)
        Controls.Add(txtFamilleBeton)
        Name = "Formules"
        Text = "Formules"
        CType(DataGridViewFormules, ComponentModel.ISupportInitialize).EndInit()
        TabControl1.ResumeLayout(False)
        TabPage1.ResumeLayout(False)
        TabPage1.PerformLayout()
        CType(DataGridViewComposition, ComponentModel.ISupportInitialize).EndInit()
        TabPage2.ResumeLayout(False)
        TabPage2.PerformLayout()
        TabPage3.ResumeLayout(False)
        TabPage3.PerformLayout()
        TabPage4.ResumeLayout(False)
        TabPage4.PerformLayout()
        TabPage5.ResumeLayout(False)
        TabPage5.PerformLayout()
        ResumeLayout(False)
        PerformLayout()
    End Sub

    Friend WithEvents PrintDocument1 As Printing.PrintDocument
    Friend WithEvents PrintPreviewDialog1 As PrintPreviewDialog
    Friend WithEvents DataGridViewFormules As DataGridView
    Friend WithEvents btnModifier As Button
    Friend WithEvents btnSupprimer As Button
    Friend WithEvents btnAjouter As Button
    Friend WithEvents Label13 As Label
    Friend WithEvents TabControl1 As TabControl
    Friend WithEvents TabPage1 As TabPage
    Friend WithEvents txtCleRechercheFormule As TextBox
    Friend WithEvents Label3 As Label
    Friend WithEvents Label2 As Label
    Friend WithEvents Label1 As Label
    Friend WithEvents txtFamilleBeton As TextBox
    Friend WithEvents txtNomFormule As TextBox
    Friend WithEvents txtCodeFormule As TextBox
    Friend WithEvents TabPage2 As TabPage
    Friend WithEvents TabPage3 As TabPage
    Friend WithEvents TabPage4 As TabPage
    Friend WithEvents TabPage5 As TabPage
    Friend WithEvents DataGridView2 As DataGridView
    Friend WithEvents Label14 As Label
    Friend WithEvents Label15 As Label
    Friend WithEvents Label18 As Label
    Friend WithEvents Label19 As Label
    Friend WithEvents Label20 As Label
    Friend WithEvents Label21 As Label
    Friend WithEvents Label22 As Label
    Friend WithEvents Label23 As Label
    Friend WithEvents Label24 As Label
    Friend WithEvents Label25 As Label
    Friend WithEvents txtReductionChargeMaxiTremie As TextBox
    Friend WithEvents TextBox9 As TextBox
    Friend WithEvents TextBox11 As TextBox
    Friend WithEvents TextBox12 As TextBox
    Friend WithEvents TextBox13 As TextBox
    Friend WithEvents Label26 As Label
    Friend WithEvents Label27 As Label
    Friend WithEvents Label28 As Label
    Friend WithEvents Label29 As Label
    Friend WithEvents Label32 As Label
    Friend WithEvents Label33 As Label
    Friend WithEvents Label34 As Label
    Friend WithEvents Label35 As Label
    Friend WithEvents Label36 As Label
    Friend WithEvents Label37 As Label
    Friend WithEvents txtPrixRevientFormule As TextBox
    Friend WithEvents txtTarif1MontantFormule As TextBox
    Friend WithEvents dtpTarif1DateFormule As TextBox
    Friend WithEvents txtTaxe2Formule As TextBox
    Friend WithEvents txtTaxe1Formule As TextBox
    Friend WithEvents txtTVAFormule As TextBox
    Friend WithEvents txtNomProduitFormule As TextBox
    Friend WithEvents txtCleProduitFormule As TextBox
    Friend WithEvents txtCodeProduitFormule As TextBox
    Friend WithEvents Label38 As Label
    Friend WithEvents Label39 As Label
    Friend WithEvents Label40 As Label
    Friend WithEvents Label42 As Label
    Friend WithEvents Label43 As Label
    Friend WithEvents Label44 As Label
    Friend WithEvents Label45 As Label
    Friend WithEvents Label46 As Label
    Friend WithEvents Label47 As Label
    Friend WithEvents Label48 As Label
    Friend WithEvents Label49 As Label
    Friend WithEvents TextBox29 As TextBox
    Friend WithEvents ComboBox3 As ComboBox
    Friend WithEvents cmbControleNorme As ComboBox
    Friend WithEvents cmbMateriauFormule As ComboBox
    Friend WithEvents Label67 As Label
    Friend WithEvents Label66 As Label
    Friend WithEvents Label65 As Label
    Friend WithEvents Label63 As Label
    Friend WithEvents Label64 As Label
    Friend WithEvents txtTempsMalaxage As TextBox
    Friend WithEvents txtMES As TextBox
    Friend WithEvents txtVolumeAir As TextBox
    Friend WithEvents Label69 As Label
    Friend WithEvents Label4 As Label
    Friend WithEvents ComboBox18 As ComboBox
    Friend WithEvents Label70 As Label
    Friend WithEvents ComboBox15 As ComboBox
    Friend WithEvents ComboBox16 As ComboBox
    Friend WithEvents ComboBox17 As ComboBox
    Friend WithEvents Label11 As Label
    Friend WithEvents Label12 As Label
    Friend WithEvents Label17 As Label
    Friend WithEvents ComboBox8 As ComboBox
    Friend WithEvents ComboBox7 As ComboBox
    Friend WithEvents ComboBox6 As ComboBox
    Friend WithEvents cmbDestinationAdjuvantPese As ComboBox
    Friend WithEvents txtRemarqueNorme As ListBox
    Friend WithEvents btnActualiserFormule As Button
    Friend WithEvents Label71 As Label
    Friend WithEvents TextBox2 As TextBox
    Friend WithEvents TextBox1 As TextBox
    Friend WithEvents TextBox4 As TextBox
    Friend WithEvents TextBox5 As TextBox
    Friend WithEvents Label16 As Label
    Friend WithEvents Label30 As Label
    Friend WithEvents txtTarif2MontantFormule As TextBox
    Friend WithEvents dtpTarif2DateFormule As TextBox
    Friend WithEvents chkQteLivreeEgaleFabriquee As Button
    Friend WithEvents chkImpressionBLPoidsTonnes As Button
    Friend WithEvents ListBox2 As ListBox
    Friend WithEvents chkGacheeSeche As Button
    Friend WithEvents ListBox3 As ListBox
    Friend WithEvents chkGestionAjoutEauCompte As Button
    Friend WithEvents chkFormuleInactive As Button
    Friend WithEvents chkValidationPresenceBenne As Button
    Friend WithEvents Label5 As Label
    Friend WithEvents Label6 As Label
    Friend WithEvents txtDefautPlasticiteMax As TextBox
    Friend WithEvents txtDefautPlasticiteMin As TextBox
    Friend WithEvents Label7 As Label
    Friend WithEvents Label8 As Label
    Friend WithEvents txtPourcentageEauPesee As TextBox
    Friend WithEvents txtEauPlasticite As TextBox
    Friend WithEvents Label59 As Label
    Friend WithEvents Label60 As Label
    Friend WithEvents txtTempsHygrometre As TextBox
    Friend WithEvents txtEauHygrometre As TextBox
    Friend WithEvents Label55 As Label
    Friend WithEvents Label56 As Label
    Friend WithEvents Label57 As Label
    Friend WithEvents txtPhaseHygrometre As TextBox
    Friend WithEvents txtNumeroProgrammeHygrometre As TextBox
    Friend WithEvents txtToleranceEauMax As TextBox
    Friend WithEvents Label52 As Label
    Friend WithEvents Label53 As Label
    Friend WithEvents Label54 As Label
    Friend WithEvents txtToleranceEauMin As TextBox
    Friend WithEvents txtTempsMalaxagePlasticite As TextBox
    Friend WithEvents txtTempsDecalagePlasticite As TextBox
    Friend WithEvents Label31 As Label
    Friend WithEvents Label51 As Label
    Friend WithEvents Label50 As Label
    Friend WithEvents txtPuissanceVisee As TextBox
    Friend WithEvents txtPasPlasticite As TextBox
    Friend WithEvents txtPhaseEauPlasticite As TextBox
    Friend WithEvents Label62 As Label
    Friend WithEvents txtGamme As TextBox
    Friend WithEvents btnImprimerFormule As Button
    Friend WithEvents btnSupprimerFormule As Button
    Friend WithEvents btnDupliquerFormule As Button
    Friend WithEvents btnModifierFormule As Button
    Friend WithEvents btnAjouterFormule As Button
    Friend WithEvents Label9 As Label
    Friend WithEvents Label10 As Label
    Friend WithEvents btnModifierComposition As Button
    Friend WithEvents btnSupprimerComposition As Button
    Friend WithEvents btnInsererComposition As Button
    Friend WithEvents DataGridViewComposition As DataGridView
    Friend WithEvents Label58 As Label
    Friend WithEvents txtRapportEC As TextBox
    Friend WithEvents txtAttestation As TextBox
    Friend WithEvents chkDosageMiniOuReel As Button
    Friend WithEvents cmbConsistance As ComboBox
    Friend WithEvents cmbResistance As ComboBox
    Friend WithEvents cmbClasseChlorure As ComboBox
    Friend WithEvents cmbCertification As ComboBox
    Friend WithEvents txtAd As Label
    Friend WithEvents txtAdjuvants As TextBox
    Friend WithEvents txtNiveauPerformance As TextBox
    Friend WithEvents Label41 As Label
    Friend WithEvents Label61 As Label
    Friend WithEvents txtTauxChlorureCalc As TextBox
    Friend WithEvents txtMasseAlcalinCalc As TextBox
    Friend WithEvents btnAbandonnerFormule As Button
    Friend WithEvents btnValiderFormule As Button
    Friend WithEvents txtQuantiteFormule As TextBox
    Friend WithEvents TextBox3 As TextBox
    Friend WithEvents cmbTypeMateriauFormule As ComboBox
    Friend WithEvents chkAdjuvantPourcentDosage As CheckBox
    Friend WithEvents cmbUniteFormule As ComboBox
    Friend WithEvents btnEnregistrerFormule As Button
    Friend WithEvents txtDesignation As ComboBox
    Friend WithEvents cmbAdditionA As ComboBox
    Friend WithEvents cmbClasseExposition As ComboBox
    Friend WithEvents cmbNatureCiment As ComboBox
    Friend WithEvents cmbGranulariteDmax As ComboBox
End Class
