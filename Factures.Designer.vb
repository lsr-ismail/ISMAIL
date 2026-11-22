<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class Factures
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(Factures))
        Label7 = New Label()
        Label9 = New Label()
        Label10 = New Label()
        AdresseClient = New Label()
        Label5 = New Label()
        Label6 = New Label()
        txtNomClient = New TextBox()
        txtICE = New TextBox()
        txtIdFacture = New TextBox()
        DataGridViewFactures = New DataGridView()
        btnSupprimer = New Button()
        btnNouveau = New Button()
        PrintDocument1 = New Printing.PrintDocument()
        PrintPreviewDialog1 = New PrintPreviewDialog()
        Label13 = New Label()
        DataGridViewLignes = New DataGridView()
        dtpDateFacture = New DateTimePicker()
        dtpDateEcheance = New DateTimePicker()
        Label1 = New Label()
        Label2 = New Label()
        txtMontantHT = New TextBox()
        Label3 = New Label()
        Label11 = New Label()
        txtVolumeTotal = New TextBox()
        txtNombreBL = New TextBox()
        Label16 = New Label()
        txtRemise = New TextBox()
        btnActualiser = New Button()
        btnMarquerRegle = New Button()
        Label4 = New Label()
        Label8 = New Label()
        Label12 = New Label()
        Label14 = New Label()
        Label18 = New Label()
        Label20 = New Label()
        Label21 = New Label()
        Label22 = New Label()
        Label24 = New Label()
        txtRemarques = New TextBox()
        txtMontantTTC = New TextBox()
        txtTauxTVA = New TextBox()
        Label25 = New Label()
        Label15 = New Label()
        txtNumeroFacture = New TextBox()
        txtAdresseClient = New TextBox()
        TextBox11 = New TextBox()
        dtpPeriodeAu = New DateTimePicker()
        dtpPeriodeDu = New DateTimePicker()
        Label23 = New Label()
        txtMontantTVA = New TextBox()
        Label26 = New Label()
        txtMontantHTNet = New TextBox()
        cmbStatut = New ComboBox()
        cmbModeReglement = New ComboBox()
        Label17 = New Label()
        lblTotalTTC = New Label()
        lblTotalRestant = New Label()
        lblTotalRegle = New Label()
        lblTotalHT = New Label()
        lblNbFactures = New Label()
        Label19 = New Label()
        txtMontantRegle = New TextBox()
        Label27 = New Label()
        txtMontantRestant = New TextBox()
        btnEnregistrer = New Button()
        btnValider = New Button()
        btnImprimer = New Button()
        CType(DataGridViewFactures, ComponentModel.ISupportInitialize).BeginInit()
        CType(DataGridViewLignes, ComponentModel.ISupportInitialize).BeginInit()
        SuspendLayout()
        ' 
        ' Label7
        ' 
        Label7.AutoSize = True
        Label7.Location = New Point(75, 361)
        Label7.Name = "Label7"
        Label7.Size = New Size(81, 20)
        Label7.TabIndex = 309
        Label7.Text = "Periode Au"
        ' 
        ' Label9
        ' 
        Label9.AutoSize = True
        Label9.Location = New Point(77, 328)
        Label9.Name = "Label9"
        Label9.Size = New Size(86, 20)
        Label9.TabIndex = 303
        Label9.Text = "Periode Du "
        ' 
        ' Label10
        ' 
        Label10.AutoSize = True
        Label10.Location = New Point(80, 295)
        Label10.Name = "Label10"
        Label10.Size = New Size(103, 20)
        Label10.TabIndex = 302
        Label10.Text = "Adresse Client"
        ' 
        ' AdresseClient
        ' 
        AdresseClient.AutoSize = True
        AdresseClient.Location = New Point(80, 229)
        AdresseClient.Name = "AdresseClient"
        AdresseClient.Size = New Size(84, 20)
        AdresseClient.TabIndex = 300
        AdresseClient.Text = "Nom Client"
        ' 
        ' Label5
        ' 
        Label5.AutoSize = True
        Label5.Location = New Point(82, 163)
        Label5.Name = "Label5"
        Label5.Size = New Size(92, 20)
        Label5.TabIndex = 298
        Label5.Text = "Date Facture"
        ' 
        ' Label6
        ' 
        Label6.AutoSize = True
        Label6.Location = New Point(88, 97)
        Label6.Name = "Label6"
        Label6.Size = New Size(73, 20)
        Label6.TabIndex = 297
        Label6.Text = "Id Facture"
        ' 
        ' txtNomClient
        ' 
        txtNomClient.Location = New Point(229, 226)
        txtNomClient.Name = "txtNomClient"
        txtNomClient.Size = New Size(326, 27)
        txtNomClient.TabIndex = 289
        ' 
        ' txtICE
        ' 
        txtICE.Location = New Point(192, 211)
        txtICE.Name = "txtICE"
        txtICE.Size = New Size(418, 27)
        txtICE.TabIndex = 288
        ' 
        ' txtIdFacture
        ' 
        txtIdFacture.Location = New Point(229, 94)
        txtIdFacture.Name = "txtIdFacture"
        txtIdFacture.Size = New Size(326, 27)
        txtIdFacture.TabIndex = 285
        ' 
        ' DataGridViewFactures
        ' 
        DataGridViewFactures.AllowUserToAddRows = False
        DataGridViewFactures.Anchor = AnchorStyles.Top Or AnchorStyles.Bottom Or AnchorStyles.Left Or AnchorStyles.Right
        DataGridViewFactures.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill
        DataGridViewFactures.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize
        DataGridViewFactures.Location = New Point(614, 566)
        DataGridViewFactures.MultiSelect = False
        DataGridViewFactures.Name = "DataGridViewFactures"
        DataGridViewFactures.ReadOnly = True
        DataGridViewFactures.RowHeadersWidth = 51
        DataGridViewFactures.SelectionMode = DataGridViewSelectionMode.FullRowSelect
        DataGridViewFactures.Size = New Size(454, 210)
        DataGridViewFactures.TabIndex = 282
        ' 
        ' btnSupprimer
        ' 
        btnSupprimer.Location = New Point(304, 12)
        btnSupprimer.Name = "btnSupprimer"
        btnSupprimer.Size = New Size(178, 37)
        btnSupprimer.TabIndex = 278
        btnSupprimer.Text = "SUPRIME"
        btnSupprimer.UseVisualStyleBackColor = True
        ' 
        ' btnNouveau
        ' 
        btnNouveau.Location = New Point(116, 12)
        btnNouveau.Name = "btnNouveau"
        btnNouveau.Size = New Size(178, 37)
        btnNouveau.TabIndex = 277
        btnNouveau.Text = "Nouveau"
        btnNouveau.UseVisualStyleBackColor = True
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
        ' Label13
        ' 
        Label13.AutoSize = True
        Label13.Location = New Point(48, 9)
        Label13.Name = "Label13"
        Label13.Size = New Size(62, 20)
        Label13.TabIndex = 310
        Label13.Text = "Factures"
        ' 
        ' DataGridViewLignes
        ' 
        DataGridViewLignes.AllowUserToAddRows = False
        DataGridViewLignes.Anchor = AnchorStyles.Top Or AnchorStyles.Bottom Or AnchorStyles.Left Or AnchorStyles.Right
        DataGridViewLignes.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill
        DataGridViewLignes.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize
        DataGridViewLignes.Location = New Point(117, 566)
        DataGridViewLignes.MultiSelect = False
        DataGridViewLignes.Name = "DataGridViewLignes"
        DataGridViewLignes.ReadOnly = True
        DataGridViewLignes.RowHeadersWidth = 51
        DataGridViewLignes.SelectionMode = DataGridViewSelectionMode.FullRowSelect
        DataGridViewLignes.Size = New Size(454, 210)
        DataGridViewLignes.TabIndex = 311
        ' 
        ' dtpDateFacture
        ' 
        dtpDateFacture.Location = New Point(276, 160)
        dtpDateFacture.Name = "dtpDateFacture"
        dtpDateFacture.Size = New Size(250, 27)
        dtpDateFacture.TabIndex = 312
        ' 
        ' dtpDateEcheance
        ' 
        dtpDateEcheance.Location = New Point(276, 193)
        dtpDateEcheance.Name = "dtpDateEcheance"
        dtpDateEcheance.Size = New Size(250, 27)
        dtpDateEcheance.TabIndex = 313
        ' 
        ' Label1
        ' 
        Label1.AutoSize = True
        Label1.Location = New Point(80, 262)
        Label1.Name = "Label1"
        Label1.Size = New Size(47, 20)
        Label1.TabIndex = 316
        Label1.Text = "txtICE"
        ' 
        ' Label2
        ' 
        Label2.AutoSize = True
        Label2.Location = New Point(75, 460)
        Label2.Name = "Label2"
        Label2.Size = New Size(88, 20)
        Label2.TabIndex = 322
        Label2.Text = "Montant HT"
        ' 
        ' txtMontantHT
        ' 
        txtMontantHT.Location = New Point(229, 453)
        txtMontantHT.Name = "txtMontantHT"
        txtMontantHT.Size = New Size(326, 27)
        txtMontantHT.TabIndex = 321
        ' 
        ' Label3
        ' 
        Label3.AutoSize = True
        Label3.Location = New Point(73, 427)
        Label3.Name = "Label3"
        Label3.Size = New Size(96, 20)
        Label3.TabIndex = 320
        Label3.Text = "Volume Total"
        ' 
        ' Label11
        ' 
        Label11.AutoSize = True
        Label11.Location = New Point(75, 394)
        Label11.Name = "Label11"
        Label11.Size = New Size(84, 20)
        Label11.TabIndex = 319
        Label11.Text = "Nombre BL"
        ' 
        ' txtVolumeTotal
        ' 
        txtVolumeTotal.Location = New Point(227, 420)
        txtVolumeTotal.Name = "txtVolumeTotal"
        txtVolumeTotal.Size = New Size(326, 27)
        txtVolumeTotal.TabIndex = 318
        ' 
        ' txtNombreBL
        ' 
        txtNombreBL.Location = New Point(227, 387)
        txtNombreBL.Name = "txtNombreBL"
        txtNombreBL.Size = New Size(326, 27)
        txtNombreBL.TabIndex = 317
        ' 
        ' Label16
        ' 
        Label16.AutoSize = True
        Label16.Location = New Point(76, 493)
        Label16.Name = "Label16"
        Label16.Size = New Size(57, 20)
        Label16.TabIndex = 324
        Label16.Text = "Remise"
        ' 
        ' txtRemise
        ' 
        txtRemise.Location = New Point(229, 486)
        txtRemise.Name = "txtRemise"
        txtRemise.Size = New Size(326, 27)
        txtRemise.TabIndex = 323
        ' 
        ' btnActualiser
        ' 
        btnActualiser.Location = New Point(676, 51)
        btnActualiser.Name = "btnActualiser"
        btnActualiser.Size = New Size(178, 37)
        btnActualiser.TabIndex = 334
        btnActualiser.Text = "Actualiser"
        btnActualiser.UseVisualStyleBackColor = True
        ' 
        ' btnMarquerRegle
        ' 
        btnMarquerRegle.Location = New Point(494, 51)
        btnMarquerRegle.Name = "btnMarquerRegle"
        btnMarquerRegle.Size = New Size(178, 37)
        btnMarquerRegle.TabIndex = 333
        btnMarquerRegle.Text = "MarquerRegle"
        btnMarquerRegle.UseVisualStyleBackColor = True
        ' 
        ' Label4
        ' 
        Label4.AutoSize = True
        Label4.Location = New Point(841, 478)
        Label4.Name = "Label4"
        Label4.Size = New Size(95, 20)
        Label4.TabIndex = 360
        Label4.Text = "Total Restant"
        ' 
        ' Label8
        ' 
        Label8.AutoSize = True
        Label8.Location = New Point(841, 453)
        Label8.Name = "Label8"
        Label8.Size = New Size(84, 20)
        Label8.TabIndex = 358
        Label8.Text = "Total Regle"
        ' 
        ' Label12
        ' 
        Label12.AutoSize = True
        Label12.Location = New Point(587, 478)
        Label12.Name = "Label12"
        Label12.Size = New Size(65, 20)
        Label12.TabIndex = 356
        Label12.Text = "Total HT"
        ' 
        ' Label14
        ' 
        Label14.AutoSize = True
        Label14.Location = New Point(588, 453)
        Label14.Name = "Label14"
        Label14.Size = New Size(82, 20)
        Label14.TabIndex = 354
        Label14.Text = "NbFactures"
        ' 
        ' Label18
        ' 
        Label18.AutoSize = True
        Label18.Location = New Point(591, 290)
        Label18.Name = "Label18"
        Label18.Size = New Size(48, 20)
        Label18.TabIndex = 350
        Label18.Text = "Statut"
        ' 
        ' Label20
        ' 
        Label20.AutoSize = True
        Label20.Location = New Point(588, 356)
        Label20.Name = "Label20"
        Label20.Size = New Size(83, 20)
        Label20.TabIndex = 343
        Label20.Text = "Remarques"
        ' 
        ' Label21
        ' 
        Label21.AutoSize = True
        Label21.Location = New Point(591, 323)
        Label21.Name = "Label21"
        Label21.Size = New Size(124, 20)
        Label21.TabIndex = 342
        Label21.Text = "Mode Reglement"
        ' 
        ' Label22
        ' 
        Label22.AutoSize = True
        Label22.Location = New Point(591, 159)
        Label22.Name = "Label22"
        Label22.Size = New Size(93, 20)
        Label22.TabIndex = 341
        Label22.Text = "Montant TTC"
        ' 
        ' Label24
        ' 
        Label24.AutoSize = True
        Label24.Location = New Point(87, 529)
        Label24.Name = "Label24"
        Label24.Size = New Size(68, 20)
        Label24.TabIndex = 339
        Label24.Text = "Taux TVA"
        ' 
        ' txtRemarques
        ' 
        txtRemarques.Location = New Point(740, 349)
        txtRemarques.Multiline = True
        txtRemarques.Name = "txtRemarques"
        txtRemarques.Size = New Size(326, 95)
        txtRemarques.TabIndex = 338
        ' 
        ' txtMontantTTC
        ' 
        txtMontantTTC.Location = New Point(740, 156)
        txtMontantTTC.Name = "txtMontantTTC"
        txtMontantTTC.Size = New Size(326, 27)
        txtMontantTTC.TabIndex = 337
        ' 
        ' txtTauxTVA
        ' 
        txtTauxTVA.Location = New Point(227, 519)
        txtTauxTVA.Name = "txtTauxTVA"
        txtTauxTVA.Size = New Size(326, 27)
        txtTauxTVA.TabIndex = 336
        ' 
        ' Label25
        ' 
        Label25.AutoSize = True
        Label25.Location = New Point(82, 193)
        Label25.Name = "Label25"
        Label25.Size = New Size(107, 20)
        Label25.TabIndex = 361
        Label25.Text = "Date Echeance"
        ' 
        ' Label15
        ' 
        Label15.AutoSize = True
        Label15.Location = New Point(86, 126)
        Label15.Name = "Label15"
        Label15.Size = New Size(114, 20)
        Label15.TabIndex = 363
        Label15.Text = "Numero Facture"
        ' 
        ' txtNumeroFacture
        ' 
        txtNumeroFacture.Location = New Point(227, 123)
        txtNumeroFacture.Name = "txtNumeroFacture"
        txtNumeroFacture.Size = New Size(326, 27)
        txtNumeroFacture.TabIndex = 362
        ' 
        ' txtAdresseClient
        ' 
        txtAdresseClient.Location = New Point(229, 290)
        txtAdresseClient.Name = "txtAdresseClient"
        txtAdresseClient.Size = New Size(326, 27)
        txtAdresseClient.TabIndex = 365
        ' 
        ' TextBox11
        ' 
        TextBox11.Location = New Point(229, 257)
        TextBox11.Name = "TextBox11"
        TextBox11.Size = New Size(326, 27)
        TextBox11.TabIndex = 364
        ' 
        ' dtpPeriodeAu
        ' 
        dtpPeriodeAu.Location = New Point(260, 356)
        dtpPeriodeAu.Name = "dtpPeriodeAu"
        dtpPeriodeAu.Size = New Size(250, 27)
        dtpPeriodeAu.TabIndex = 367
        ' 
        ' dtpPeriodeDu
        ' 
        dtpPeriodeDu.Location = New Point(260, 323)
        dtpPeriodeDu.Name = "dtpPeriodeDu"
        dtpPeriodeDu.Size = New Size(250, 27)
        dtpPeriodeDu.TabIndex = 366
        ' 
        ' Label23
        ' 
        Label23.AutoSize = True
        Label23.Location = New Point(586, 130)
        Label23.Name = "Label23"
        Label23.Size = New Size(95, 20)
        Label23.TabIndex = 371
        Label23.Text = "Montant TVA"
        ' 
        ' txtMontantTVA
        ' 
        txtMontantTVA.Location = New Point(740, 123)
        txtMontantTVA.Name = "txtMontantTVA"
        txtMontantTVA.Size = New Size(326, 27)
        txtMontantTVA.TabIndex = 370
        ' 
        ' Label26
        ' 
        Label26.AutoSize = True
        Label26.Location = New Point(588, 97)
        Label26.Name = "Label26"
        Label26.Size = New Size(120, 20)
        Label26.TabIndex = 369
        Label26.Text = "Montant H T Net"
        ' 
        ' txtMontantHTNet
        ' 
        txtMontantHTNet.Location = New Point(740, 90)
        txtMontantHTNet.Name = "txtMontantHTNet"
        txtMontantHTNet.Size = New Size(326, 27)
        txtMontantHTNet.TabIndex = 368
        ' 
        ' cmbStatut
        ' 
        cmbStatut.FormattingEnabled = True
        cmbStatut.Location = New Point(754, 286)
        cmbStatut.Name = "cmbStatut"
        cmbStatut.Size = New Size(219, 28)
        cmbStatut.TabIndex = 372
        ' 
        ' cmbModeReglement
        ' 
        cmbModeReglement.FormattingEnabled = True
        cmbModeReglement.Location = New Point(754, 317)
        cmbModeReglement.Name = "cmbModeReglement"
        cmbModeReglement.Size = New Size(219, 28)
        cmbModeReglement.TabIndex = 373
        ' 
        ' Label17
        ' 
        Label17.AutoSize = True
        Label17.Location = New Point(589, 503)
        Label17.Name = "Label17"
        Label17.Size = New Size(70, 20)
        Label17.TabIndex = 374
        Label17.Text = "Total TTC"
        ' 
        ' lblTotalTTC
        ' 
        lblTotalTTC.AutoSize = True
        lblTotalTTC.Location = New Point(699, 499)
        lblTotalTTC.Name = "lblTotalTTC"
        lblTotalTTC.Size = New Size(95, 20)
        lblTotalTTC.TabIndex = 379
        lblTotalTTC.Text = "Montant TVA"
        ' 
        ' lblTotalRestant
        ' 
        lblTotalRestant.AutoSize = True
        lblTotalRestant.Location = New Point(951, 474)
        lblTotalRestant.Name = "lblTotalRestant"
        lblTotalRestant.Size = New Size(93, 20)
        lblTotalRestant.TabIndex = 378
        lblTotalRestant.Text = "Montant TTC"
        ' 
        ' lblTotalRegle
        ' 
        lblTotalRegle.AutoSize = True
        lblTotalRegle.Location = New Point(951, 449)
        lblTotalRegle.Name = "lblTotalRegle"
        lblTotalRegle.Size = New Size(95, 20)
        lblTotalRegle.TabIndex = 377
        lblTotalRegle.Text = "Montant TVA"
        ' 
        ' lblTotalHT
        ' 
        lblTotalHT.AutoSize = True
        lblTotalHT.Location = New Point(697, 474)
        lblTotalHT.Name = "lblTotalHT"
        lblTotalHT.Size = New Size(116, 20)
        lblTotalHT.TabIndex = 376
        lblTotalHT.Text = "Montant HT Net"
        ' 
        ' lblNbFactures
        ' 
        lblNbFactures.AutoSize = True
        lblNbFactures.Location = New Point(698, 449)
        lblNbFactures.Name = "lblNbFactures"
        lblNbFactures.Size = New Size(68, 20)
        lblNbFactures.TabIndex = 375
        lblNbFactures.Text = "Taux TVA"
        ' 
        ' Label19
        ' 
        Label19.AutoSize = True
        Label19.Location = New Point(587, 196)
        Label19.Name = "Label19"
        Label19.Size = New Size(107, 20)
        Label19.TabIndex = 383
        Label19.Text = "Montant Regle"
        ' 
        ' txtMontantRegle
        ' 
        txtMontantRegle.Location = New Point(740, 189)
        txtMontantRegle.Name = "txtMontantRegle"
        txtMontantRegle.Size = New Size(326, 27)
        txtMontantRegle.TabIndex = 382
        ' 
        ' Label27
        ' 
        Label27.AutoSize = True
        Label27.Location = New Point(592, 225)
        Label27.Name = "Label27"
        Label27.Size = New Size(118, 20)
        Label27.TabIndex = 381
        Label27.Text = "Montant Restant"
        ' 
        ' txtMontantRestant
        ' 
        txtMontantRestant.Location = New Point(740, 222)
        txtMontantRestant.Name = "txtMontantRestant"
        txtMontantRestant.Size = New Size(326, 27)
        txtMontantRestant.TabIndex = 380
        ' 
        ' btnEnregistrer
        ' 
        btnEnregistrer.Location = New Point(116, 51)
        btnEnregistrer.Name = "btnEnregistrer"
        btnEnregistrer.Size = New Size(178, 37)
        btnEnregistrer.TabIndex = 384
        btnEnregistrer.Text = "Enregistrer"
        btnEnregistrer.UseVisualStyleBackColor = True
        ' 
        ' btnValider
        ' 
        btnValider.Location = New Point(494, 12)
        btnValider.Name = "btnValider"
        btnValider.Size = New Size(178, 37)
        btnValider.TabIndex = 385
        btnValider.Text = "Valider"
        btnValider.UseVisualStyleBackColor = True
        ' 
        ' btnImprimer
        ' 
        btnImprimer.Location = New Point(676, 12)
        btnImprimer.Name = "btnImprimer"
        btnImprimer.Size = New Size(178, 37)
        btnImprimer.TabIndex = 386
        btnImprimer.Text = "Imprimer"
        btnImprimer.UseVisualStyleBackColor = True
        ' 
        ' Factures
        ' 
        AutoScaleDimensions = New SizeF(8F, 20F)
        AutoScaleMode = AutoScaleMode.Font
        ClientSize = New Size(1082, 788)
        Controls.Add(btnImprimer)
        Controls.Add(btnValider)
        Controls.Add(btnEnregistrer)
        Controls.Add(Label19)
        Controls.Add(txtMontantRegle)
        Controls.Add(Label27)
        Controls.Add(txtMontantRestant)
        Controls.Add(lblTotalTTC)
        Controls.Add(lblTotalRestant)
        Controls.Add(lblTotalRegle)
        Controls.Add(lblTotalHT)
        Controls.Add(lblNbFactures)
        Controls.Add(Label17)
        Controls.Add(cmbModeReglement)
        Controls.Add(cmbStatut)
        Controls.Add(Label23)
        Controls.Add(txtMontantTVA)
        Controls.Add(Label26)
        Controls.Add(txtMontantHTNet)
        Controls.Add(dtpPeriodeAu)
        Controls.Add(dtpPeriodeDu)
        Controls.Add(txtAdresseClient)
        Controls.Add(TextBox11)
        Controls.Add(Label15)
        Controls.Add(txtNumeroFacture)
        Controls.Add(Label25)
        Controls.Add(Label4)
        Controls.Add(Label8)
        Controls.Add(Label12)
        Controls.Add(Label14)
        Controls.Add(Label18)
        Controls.Add(Label20)
        Controls.Add(Label21)
        Controls.Add(Label22)
        Controls.Add(Label24)
        Controls.Add(txtRemarques)
        Controls.Add(txtMontantTTC)
        Controls.Add(txtTauxTVA)
        Controls.Add(btnActualiser)
        Controls.Add(btnMarquerRegle)
        Controls.Add(Label16)
        Controls.Add(txtRemise)
        Controls.Add(Label2)
        Controls.Add(txtMontantHT)
        Controls.Add(Label3)
        Controls.Add(Label11)
        Controls.Add(txtVolumeTotal)
        Controls.Add(txtNombreBL)
        Controls.Add(Label1)
        Controls.Add(dtpDateEcheance)
        Controls.Add(dtpDateFacture)
        Controls.Add(DataGridViewLignes)
        Controls.Add(Label13)
        Controls.Add(Label7)
        Controls.Add(Label9)
        Controls.Add(Label10)
        Controls.Add(AdresseClient)
        Controls.Add(Label5)
        Controls.Add(Label6)
        Controls.Add(txtNomClient)
        Controls.Add(txtIdFacture)
        Controls.Add(DataGridViewFactures)
        Controls.Add(btnSupprimer)
        Controls.Add(btnNouveau)
        Name = "Factures"
        Text = "Factures"
        CType(DataGridViewFactures, ComponentModel.ISupportInitialize).EndInit()
        CType(DataGridViewLignes, ComponentModel.ISupportInitialize).EndInit()
        ResumeLayout(False)
        PerformLayout()
    End Sub

    Friend WithEvents Label7 As Label
    Friend WithEvents txtRECHERCHE As TextBox
    Friend WithEvents btnActualiser As Button
    Friend WithEvents btnNouveau As Button
    Friend WithEvents Label9 As Label
    Friend WithEvents Label10 As Label
    Friend WithEvents AdresseClient As Label
    Friend WithEvents Label5 As Label
    Friend WithEvents Label6 As Label
    Friend WithEvents Imprimer As Button
    Friend WithEvents txtTelephone As TextBox
    Friend WithEvents txtChargeUtile As TextBox
    Friend WithEvents txtNomClient As TextBox
    Friend WithEvents txtICE As TextBox

    Friend WithEvents txtIdFacture As TextBox
    Friend WithEvents txtNomCamion As TextBox
    Friend WithEvents DataGridViewFactures As DataGridView
    Friend WithEvents btnModifier As Button
    Friend WithEvents btnSupprimer As Button
    Friend WithEvents btnAjouter As Button
    Friend WithEvents RECHERCHE As Button
    Friend WithEvents PrintDocument1 As Printing.PrintDocument
    Friend WithEvents PrintPreviewDialog1 As PrintPreviewDialog
    Friend WithEvents Label13 As Label
    Friend WithEvents DataGridViewLignes As DataGridView
    Friend WithEvents dtpDateFacture As DateTimePicker
    Friend WithEvents dtpDateEcheance As DateTimePicker
    Friend WithEvents Label1 As Label
    Friend WithEvents Label2 As Label
    Friend WithEvents txtMontantHT As TextBox
    Friend WithEvents Label3 As Label
    Friend WithEvents Label11 As Label
    Friend WithEvents txtVolumeTotal As TextBox
    Friend WithEvents txtNombreBL As TextBox
    Friend WithEvents Label16 As Label
    Friend WithEvents txtRemise As TextBox
    Friend WithEvents Button1 As Button
    Friend WithEvents Button2 As Button
    Friend WithEvents Button3 As Button
    Friend WithEvents Button4 As Button
    Friend WithEvents btnMarquerRegle As Button
    Friend WithEvents Label4 As Label
    Friend WithEvents Label8 As Label
    Friend WithEvents Label12 As Label
    Friend WithEvents TextBox3 As TextBox
    Friend WithEvents Label14 As Label
    Friend WithEvents TextBox4 As TextBox
    Friend WithEvents TextBox5 As TextBox
    Friend WithEvents Label18 As Label
    Friend WithEvents DateTimePicker1 As DateTimePicker
    Friend WithEvents DateTimePicker2 As DateTimePicker
    Friend WithEvents DateTimePicker3 As DateTimePicker
    Friend WithEvents DateTimePicker4 As DateTimePicker
    Friend WithEvents lblTotalTTC As Label
    Friend WithEvents TextBox6 As TextBox
    Friend WithEvents Label20 As Label
    Friend WithEvents Label21 As Label
    Friend WithEvents Label22 As Label
    Friend WithEvents Label24 As Label
    Friend WithEvents txtRemarques As TextBox
    Friend WithEvents txtMontantTTC As TextBox
    Friend WithEvents txtTauxTVA As TextBox
    Friend WithEvents Label25 As Label
    Friend WithEvents Label15 As Label
    Friend WithEvents txtNumeroFacture As TextBox
    Friend WithEvents txtAdresseClient As TextBox
    Friend WithEvents TextBox11 As TextBox
    Friend WithEvents dtpPeriodeAu As DateTimePicker
    Friend WithEvents dtpPeriodeDu As DateTimePicker
    Friend WithEvents Label23 As Label
    Friend WithEvents txtMontantTVA As TextBox
    Friend WithEvents Label26 As Label
    Friend WithEvents txtMontantHTNet As TextBox
    Friend WithEvents cmbStatut As ComboBox
    Friend WithEvents cmbModeReglement As ComboBox
    Friend WithEvents Label17 As Label
    Friend WithEvents lblTotalRestant As Label
    Friend WithEvents lblTotalRegle As Label
    Friend WithEvents lblTotalHT As Label
    Friend WithEvents lblNbFactures As Label
    Friend WithEvents Label19 As Label
    Friend WithEvents txtMontantRegle As TextBox
    Friend WithEvents Label27 As Label
    Friend WithEvents txtMontantRestant As TextBox
    Friend WithEvents btnEnregistrer As Button
    Friend WithEvents btnValider As Button
    Friend WithEvents btnImprimer As Button
End Class
