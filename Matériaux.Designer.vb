<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class Matériaux
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(Matériaux))
        PrintPreviewDialog1 = New PrintPreviewDialog()
        PrintDocument1 = New Printing.PrintDocument()
        txtRECHERCHE = New TextBox()
        btnActualiser = New Button()
        btnNouveau = New Button()
        Label5 = New Label()
        Label6 = New Label()
        Label3 = New Label()
        Label2 = New Label()
        Label1 = New Label()
        Imprimer = New Button()
        txtCodeMatierePremiere = New TextBox()
        txtNomMateriau = New TextBox()
        txtCleRecherche = New TextBox()
        DataGridView1 = New DataGridView()
        txtCodeMateriau = New TextBox()
        btnModifier = New Button()
        RECHERCHE = New Button()
        btnSupprimer = New Button()
        btnAjouter = New Button()
        txtMateriauEquivalent = New TextBox()
        txtPrixRevient = New TextBox()
        txtDensite = New TextBox()
        txtStock = New TextBox()
        txtDosageMax = New TextBox()
        Label24 = New Label()
        Label23 = New Label()
        Label22 = New Label()
        Label21 = New Label()
        Label20 = New Label()
        Label25 = New Label()
        Label26 = New Label()
        Label27 = New Label()
        Label28 = New Label()
        Label29 = New Label()
        txtAbsorptionCapillaire = New TextBox()
        txtGestionStock = New TextBox()
        txtTauxAlcalin = New TextBox()
        txtTauxChlorure = New TextBox()
        txtTolerance = New TextBox()
        Label30 = New Label()
        txtSable = New TextBox()
        Label4 = New Label()
        txtDosageMiniBL = New TextBox()
        Label7 = New Label()
        Label8 = New Label()
        Label9 = New Label()
        Label10 = New Label()
        Label11 = New Label()
        txtUnite = New TextBox()
        txtMontantUnitaire = New TextBox()
        txtTarif2 = New TextBox()
        txtTarif1 = New TextBox()
        txtTVA = New TextBox()
        Label12 = New Label()
        Label13 = New Label()
        Label14 = New Label()
        txtNomProduit = New TextBox()
        txtCleRechercheProduit = New TextBox()
        txtCodeProduit = New TextBox()
        Label15 = New Label()
        txtDosageMaxiBL = New TextBox()
        cmbTypeMateriau = New ComboBox()
        Label16 = New Label()
        lblNombre = New Label()
        CType(DataGridView1, ComponentModel.ISupportInitialize).BeginInit()
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
        ' txtRECHERCHE
        ' 
        txtRECHERCHE.Location = New Point(211, 106)
        txtRECHERCHE.Name = "txtRECHERCHE"
        txtRECHERCHE.Size = New Size(310, 27)
        txtRECHERCHE.TabIndex = 175
        ' 
        ' btnActualiser
        ' 
        btnActualiser.Location = New Point(951, 27)
        btnActualiser.Name = "btnActualiser"
        btnActualiser.Size = New Size(143, 61)
        btnActualiser.TabIndex = 174
        btnActualiser.Text = "Actualiser"
        btnActualiser.UseVisualStyleBackColor = True
        ' 
        ' btnNouveau
        ' 
        btnNouveau.Location = New Point(802, 27)
        btnNouveau.Name = "btnNouveau"
        btnNouveau.Size = New Size(143, 61)
        btnNouveau.TabIndex = 173
        btnNouveau.Text = "Nouveau"
        btnNouveau.UseVisualStyleBackColor = True
        ' 
        ' Label5
        ' 
        Label5.AutoSize = True
        Label5.Location = New Point(41, 275)
        Label5.Name = "Label5"
        Label5.Size = New Size(163, 20)
        Label5.TabIndex = 166
        Label5.Text = "Code matière première"
        ' 
        ' Label6
        ' 
        Label6.AutoSize = True
        Label6.Location = New Point(38, 242)
        Label6.Name = "Label6"
        Label6.Size = New Size(103, 20)
        Label6.TabIndex = 165
        Label6.Text = "Type matériau"
        ' 
        ' Label3
        ' 
        Label3.AutoSize = True
        Label3.Location = New Point(41, 209)
        Label3.Name = "Label3"
        Label3.Size = New Size(105, 20)
        Label3.TabIndex = 164
        Label3.Text = "Nom matériau"
        ' 
        ' Label2
        ' 
        Label2.AutoSize = True
        Label2.Location = New Point(41, 176)
        Label2.Name = "Label2"
        Label2.Size = New Size(98, 20)
        Label2.TabIndex = 163
        Label2.Text = "Clé recherche"
        ' 
        ' Label1
        ' 
        Label1.AutoSize = True
        Label1.Location = New Point(41, 143)
        Label1.Name = "Label1"
        Label1.Size = New Size(107, 20)
        Label1.TabIndex = 162
        Label1.Text = "Code matériau"
        ' 
        ' Imprimer
        ' 
        Imprimer.Location = New Point(653, 27)
        Imprimer.Name = "Imprimer"
        Imprimer.Size = New Size(143, 61)
        Imprimer.TabIndex = 161
        Imprimer.Text = "Imprimer"
        Imprimer.UseVisualStyleBackColor = True
        ' 
        ' txtCodeMatierePremiere
        ' 
        txtCodeMatierePremiere.Location = New Point(209, 271)
        txtCodeMatierePremiere.Name = "txtCodeMatierePremiere"
        txtCodeMatierePremiere.Size = New Size(310, 27)
        txtCodeMatierePremiere.TabIndex = 154
        ' 
        ' txtNomMateriau
        ' 
        txtNomMateriau.Location = New Point(209, 205)
        txtNomMateriau.Name = "txtNomMateriau"
        txtNomMateriau.Size = New Size(310, 27)
        txtNomMateriau.TabIndex = 152
        ' 
        ' txtCleRecherche
        ' 
        txtCleRecherche.Location = New Point(211, 174)
        txtCleRecherche.Name = "txtCleRecherche"
        txtCleRecherche.Size = New Size(310, 27)
        txtCleRecherche.TabIndex = 151
        ' 
        ' DataGridView1
        ' 
        DataGridView1.AllowUserToAddRows = False
        DataGridView1.Anchor = AnchorStyles.Top Or AnchorStyles.Bottom Or AnchorStyles.Left Or AnchorStyles.Right
        DataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill
        DataGridView1.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize
        DataGridView1.Location = New Point(533, 411)
        DataGridView1.MultiSelect = False
        DataGridView1.Name = "DataGridView1"
        DataGridView1.ReadOnly = True
        DataGridView1.RowHeadersWidth = 51
        DataGridView1.SelectionMode = DataGridViewSelectionMode.FullRowSelect
        DataGridView1.Size = New Size(562, 321)
        DataGridView1.TabIndex = 150
        ' 
        ' txtCodeMateriau
        ' 
        txtCodeMateriau.Location = New Point(209, 139)
        txtCodeMateriau.Name = "txtCodeMateriau"
        txtCodeMateriau.Size = New Size(310, 27)
        txtCodeMateriau.TabIndex = 149
        ' 
        ' btnModifier
        ' 
        btnModifier.Location = New Point(504, 27)
        btnModifier.Name = "btnModifier"
        btnModifier.Size = New Size(143, 61)
        btnModifier.TabIndex = 148
        btnModifier.Text = "EDITE"
        btnModifier.UseVisualStyleBackColor = True
        ' 
        ' RECHERCHE
        ' 
        RECHERCHE.Location = New Point(355, 27)
        RECHERCHE.Name = "RECHERCHE"
        RECHERCHE.Size = New Size(143, 61)
        RECHERCHE.TabIndex = 147
        RECHERCHE.Text = "RECHERCHE"
        RECHERCHE.UseVisualStyleBackColor = True
        ' 
        ' btnSupprimer
        ' 
        btnSupprimer.Location = New Point(206, 27)
        btnSupprimer.Name = "btnSupprimer"
        btnSupprimer.Size = New Size(143, 61)
        btnSupprimer.TabIndex = 146
        btnSupprimer.Text = "SUPRIME"
        btnSupprimer.UseVisualStyleBackColor = True
        ' 
        ' btnAjouter
        ' 
        btnAjouter.Location = New Point(57, 27)
        btnAjouter.Name = "btnAjouter"
        btnAjouter.Size = New Size(143, 61)
        btnAjouter.TabIndex = 145
        btnAjouter.Text = "AJOUTER"
        btnAjouter.UseVisualStyleBackColor = True
        ' 
        ' txtMateriauEquivalent
        ' 
        txtMateriauEquivalent.Location = New Point(210, 305)
        txtMateriauEquivalent.Name = "txtMateriauEquivalent"
        txtMateriauEquivalent.Size = New Size(310, 27)
        txtMateriauEquivalent.TabIndex = 179
        ' 
        ' txtPrixRevient
        ' 
        txtPrixRevient.Location = New Point(212, 340)
        txtPrixRevient.Name = "txtPrixRevient"
        txtPrixRevient.Size = New Size(310, 27)
        txtPrixRevient.TabIndex = 180
        ' 
        ' txtDensite
        ' 
        txtDensite.Location = New Point(210, 371)
        txtDensite.Name = "txtDensite"
        txtDensite.Size = New Size(310, 27)
        txtDensite.TabIndex = 181
        ' 
        ' txtStock
        ' 
        txtStock.Location = New Point(210, 404)
        txtStock.Name = "txtStock"
        txtStock.Size = New Size(310, 27)
        txtStock.TabIndex = 182
        ' 
        ' txtDosageMax
        ' 
        txtDosageMax.Location = New Point(210, 437)
        txtDosageMax.Name = "txtDosageMax"
        txtDosageMax.Size = New Size(310, 27)
        txtDosageMax.TabIndex = 183
        ' 
        ' Label24
        ' 
        Label24.AutoSize = True
        Label24.Location = New Point(42, 309)
        Label24.Name = "Label24"
        Label24.Size = New Size(141, 20)
        Label24.TabIndex = 184
        Label24.Text = "Matériau équivalent"
        ' 
        ' Label23
        ' 
        Label23.AutoSize = True
        Label23.Location = New Point(42, 342)
        Label23.Name = "Label23"
        Label23.Size = New Size(82, 20)
        Label23.TabIndex = 185
        Label23.Text = "Prix revient"
        ' 
        ' Label22
        ' 
        Label22.AutoSize = True
        Label22.Location = New Point(42, 375)
        Label22.Name = "Label22"
        Label22.Size = New Size(59, 20)
        Label22.TabIndex = 186
        Label22.Text = "Densité"
        ' 
        ' Label21
        ' 
        Label21.AutoSize = True
        Label21.Location = New Point(44, 411)
        Label21.Name = "Label21"
        Label21.Size = New Size(45, 20)
        Label21.TabIndex = 187
        Label21.Text = "Stock"
        ' 
        ' Label20
        ' 
        Label20.AutoSize = True
        Label20.Location = New Point(42, 441)
        Label20.Name = "Label20"
        Label20.Size = New Size(92, 20)
        Label20.TabIndex = 188
        Label20.Text = "Dosage max"
        ' 
        ' Label25
        ' 
        Label25.AutoSize = True
        Label25.Location = New Point(43, 610)
        Label25.Name = "Label25"
        Label25.Size = New Size(148, 20)
        Label25.TabIndex = 198
        Label25.Text = "Absorption capillaire"
        ' 
        ' Label26
        ' 
        Label26.AutoSize = True
        Label26.Location = New Point(40, 577)
        Label26.Name = "Label26"
        Label26.Size = New Size(97, 20)
        Label26.TabIndex = 197
        Label26.Text = "Gestion stock"
        ' 
        ' Label27
        ' 
        Label27.AutoSize = True
        Label27.Location = New Point(43, 544)
        Label27.Name = "Label27"
        Label27.Size = New Size(85, 20)
        Label27.TabIndex = 196
        Label27.Text = "Taux alcalin"
        ' 
        ' Label28
        ' 
        Label28.AutoSize = True
        Label28.Location = New Point(43, 511)
        Label28.Name = "Label28"
        Label28.Size = New Size(96, 20)
        Label28.TabIndex = 195
        Label28.Text = "Taux chlorure"
        ' 
        ' Label29
        ' 
        Label29.AutoSize = True
        Label29.Location = New Point(43, 478)
        Label29.Name = "Label29"
        Label29.Size = New Size(73, 20)
        Label29.TabIndex = 194
        Label29.Text = "Tolérance"
        ' 
        ' txtAbsorptionCapillaire
        ' 
        txtAbsorptionCapillaire.Location = New Point(211, 606)
        txtAbsorptionCapillaire.Name = "txtAbsorptionCapillaire"
        txtAbsorptionCapillaire.Size = New Size(310, 27)
        txtAbsorptionCapillaire.TabIndex = 193
        ' 
        ' txtGestionStock
        ' 
        txtGestionStock.Location = New Point(211, 573)
        txtGestionStock.Name = "txtGestionStock"
        txtGestionStock.Size = New Size(310, 27)
        txtGestionStock.TabIndex = 192
        ' 
        ' txtTauxAlcalin
        ' 
        txtTauxAlcalin.Location = New Point(211, 540)
        txtTauxAlcalin.Name = "txtTauxAlcalin"
        txtTauxAlcalin.Size = New Size(310, 27)
        txtTauxAlcalin.TabIndex = 191
        ' 
        ' txtTauxChlorure
        ' 
        txtTauxChlorure.Location = New Point(213, 509)
        txtTauxChlorure.Name = "txtTauxChlorure"
        txtTauxChlorure.Size = New Size(310, 27)
        txtTauxChlorure.TabIndex = 190
        ' 
        ' txtTolerance
        ' 
        txtTolerance.Location = New Point(211, 474)
        txtTolerance.Name = "txtTolerance"
        txtTolerance.Size = New Size(310, 27)
        txtTolerance.TabIndex = 189
        ' 
        ' Label30
        ' 
        Label30.AutoSize = True
        Label30.Location = New Point(43, 642)
        Label30.Name = "Label30"
        Label30.Size = New Size(46, 20)
        Label30.TabIndex = 200
        Label30.Text = "Sable"
        ' 
        ' txtSable
        ' 
        txtSable.Location = New Point(208, 639)
        txtSable.Name = "txtSable"
        txtSable.Size = New Size(310, 27)
        txtSable.TabIndex = 199
        ' 
        ' Label4
        ' 
        Label4.AutoSize = True
        Label4.Location = New Point(552, 337)
        Label4.Name = "Label4"
        Label4.Size = New Size(113, 20)
        Label4.TabIndex = 222
        Label4.Text = "Dosage mini BL"
        ' 
        ' txtDosageMiniBL
        ' 
        txtDosageMiniBL.Location = New Point(710, 333)
        txtDosageMiniBL.Name = "txtDosageMiniBL"
        txtDosageMiniBL.Size = New Size(310, 27)
        txtDosageMiniBL.TabIndex = 221
        ' 
        ' Label7
        ' 
        Label7.AutoSize = True
        Label7.Location = New Point(555, 304)
        Label7.Name = "Label7"
        Label7.Size = New Size(44, 20)
        Label7.TabIndex = 220
        Label7.Text = "Unité"
        ' 
        ' Label8
        ' 
        Label8.AutoSize = True
        Label8.Location = New Point(552, 271)
        Label8.Name = "Label8"
        Label8.Size = New Size(119, 20)
        Label8.TabIndex = 219
        Label8.Text = "Montant unitaire"
        ' 
        ' Label9
        ' 
        Label9.AutoSize = True
        Label9.Location = New Point(555, 238)
        Label9.Name = "Label9"
        Label9.Size = New Size(49, 20)
        Label9.TabIndex = 218
        Label9.Text = "Tarif 2"
        ' 
        ' Label10
        ' 
        Label10.AutoSize = True
        Label10.Location = New Point(555, 205)
        Label10.Name = "Label10"
        Label10.Size = New Size(49, 20)
        Label10.TabIndex = 217
        Label10.Text = "Tarif 1"
        ' 
        ' Label11
        ' 
        Label11.AutoSize = True
        Label11.Location = New Point(555, 172)
        Label11.Name = "Label11"
        Label11.Size = New Size(35, 20)
        Label11.TabIndex = 216
        Label11.Text = "TVA"
        ' 
        ' txtUnite
        ' 
        txtUnite.Location = New Point(713, 300)
        txtUnite.Name = "txtUnite"
        txtUnite.Size = New Size(310, 27)
        txtUnite.TabIndex = 215
        ' 
        ' txtMontantUnitaire
        ' 
        txtMontantUnitaire.Location = New Point(713, 267)
        txtMontantUnitaire.Name = "txtMontantUnitaire"
        txtMontantUnitaire.Size = New Size(310, 27)
        txtMontantUnitaire.TabIndex = 214
        ' 
        ' txtTarif2
        ' 
        txtTarif2.Location = New Point(713, 234)
        txtTarif2.Name = "txtTarif2"
        txtTarif2.Size = New Size(310, 27)
        txtTarif2.TabIndex = 213
        ' 
        ' txtTarif1
        ' 
        txtTarif1.Location = New Point(715, 203)
        txtTarif1.Name = "txtTarif1"
        txtTarif1.Size = New Size(310, 27)
        txtTarif1.TabIndex = 212
        ' 
        ' txtTVA
        ' 
        txtTVA.Location = New Point(713, 168)
        txtTVA.Name = "txtTVA"
        txtTVA.Size = New Size(310, 27)
        txtTVA.TabIndex = 211
        ' 
        ' Label12
        ' 
        Label12.AutoSize = True
        Label12.Location = New Point(554, 135)
        Label12.Name = "Label12"
        Label12.Size = New Size(95, 20)
        Label12.TabIndex = 210
        Label12.Text = "Nom produit"
        ' 
        ' Label13
        ' 
        Label13.AutoSize = True
        Label13.Location = New Point(37, 709)
        Label13.Name = "Label13"
        Label13.Size = New Size(98, 20)
        Label13.TabIndex = 209
        Label13.Text = "Clé recherche"
        ' 
        ' Label14
        ' 
        Label14.AutoSize = True
        Label14.Location = New Point(40, 676)
        Label14.Name = "Label14"
        Label14.Size = New Size(97, 20)
        Label14.TabIndex = 208
        Label14.Text = "Code produit"
        ' 
        ' txtNomProduit
        ' 
        txtNomProduit.Location = New Point(712, 131)
        txtNomProduit.Name = "txtNomProduit"
        txtNomProduit.Size = New Size(310, 27)
        txtNomProduit.TabIndex = 205
        ' 
        ' txtCleRechercheProduit
        ' 
        txtCleRechercheProduit.Location = New Point(208, 705)
        txtCleRechercheProduit.Name = "txtCleRechercheProduit"
        txtCleRechercheProduit.Size = New Size(310, 27)
        txtCleRechercheProduit.TabIndex = 204
        ' 
        ' txtCodeProduit
        ' 
        txtCodeProduit.Location = New Point(208, 672)
        txtCodeProduit.Name = "txtCodeProduit"
        txtCodeProduit.Size = New Size(310, 27)
        txtCodeProduit.TabIndex = 203
        ' 
        ' Label15
        ' 
        Label15.AutoSize = True
        Label15.Location = New Point(552, 370)
        Label15.Name = "Label15"
        Label15.Size = New Size(116, 20)
        Label15.TabIndex = 224
        Label15.Text = "Dosage maxi BL"
        ' 
        ' txtDosageMaxiBL
        ' 
        txtDosageMaxiBL.Location = New Point(710, 366)
        txtDosageMaxiBL.Name = "txtDosageMaxiBL"
        txtDosageMaxiBL.Size = New Size(310, 27)
        txtDosageMaxiBL.TabIndex = 223
        ' 
        ' cmbTypeMateriau
        ' 
        cmbTypeMateriau.FormattingEnabled = True
        cmbTypeMateriau.Location = New Point(209, 234)
        cmbTypeMateriau.Name = "cmbTypeMateriau"
        cmbTypeMateriau.Size = New Size(312, 28)
        cmbTypeMateriau.TabIndex = 225
        ' 
        ' Label16
        ' 
        Label16.AutoSize = True
        Label16.Font = New Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point, CByte(0))
        Label16.Location = New Point(48, -4)
        Label16.Name = "Label16"
        Label16.Size = New Size(99, 28)
        Label16.TabIndex = 226
        Label16.Text = "Matériaux"
        ' 
        ' lblNombre
        ' 
        lblNombre.AutoSize = True
        lblNombre.Location = New Point(802, 101)
        lblNombre.Name = "lblNombre"
        lblNombre.Size = New Size(64, 20)
        lblNombre.TabIndex = 227
        lblNombre.Text = "Nombre"
        lblNombre.TextAlign = ContentAlignment.MiddleCenter
        ' 
        ' Matériaux
        ' 
        AutoScaleDimensions = New SizeF(8F, 20F)
        AutoScaleMode = AutoScaleMode.Font
        ClientSize = New Size(1109, 782)
        Controls.Add(lblNombre)
        Controls.Add(Label16)
        Controls.Add(cmbTypeMateriau)
        Controls.Add(Label15)
        Controls.Add(txtDosageMaxiBL)
        Controls.Add(Label4)
        Controls.Add(txtDosageMiniBL)
        Controls.Add(Label7)
        Controls.Add(Label8)
        Controls.Add(Label9)
        Controls.Add(Label10)
        Controls.Add(Label11)
        Controls.Add(txtUnite)
        Controls.Add(txtMontantUnitaire)
        Controls.Add(txtTarif2)
        Controls.Add(txtTarif1)
        Controls.Add(txtTVA)
        Controls.Add(Label12)
        Controls.Add(Label13)
        Controls.Add(Label14)
        Controls.Add(txtNomProduit)
        Controls.Add(txtCleRechercheProduit)
        Controls.Add(txtCodeProduit)
        Controls.Add(Label30)
        Controls.Add(txtSable)
        Controls.Add(Label25)
        Controls.Add(Label26)
        Controls.Add(Label27)
        Controls.Add(Label28)
        Controls.Add(Label29)
        Controls.Add(txtAbsorptionCapillaire)
        Controls.Add(txtGestionStock)
        Controls.Add(txtTauxAlcalin)
        Controls.Add(txtTauxChlorure)
        Controls.Add(txtTolerance)
        Controls.Add(Label20)
        Controls.Add(Label21)
        Controls.Add(Label22)
        Controls.Add(Label23)
        Controls.Add(Label24)
        Controls.Add(txtDosageMax)
        Controls.Add(txtStock)
        Controls.Add(txtDensite)
        Controls.Add(txtPrixRevient)
        Controls.Add(txtMateriauEquivalent)
        Controls.Add(txtRECHERCHE)
        Controls.Add(btnActualiser)
        Controls.Add(btnNouveau)
        Controls.Add(Label5)
        Controls.Add(Label6)
        Controls.Add(Label3)
        Controls.Add(Label2)
        Controls.Add(Label1)
        Controls.Add(Imprimer)
        Controls.Add(txtCodeMatierePremiere)
        Controls.Add(txtNomMateriau)
        Controls.Add(txtCleRecherche)
        Controls.Add(DataGridView1)
        Controls.Add(txtCodeMateriau)
        Controls.Add(btnModifier)
        Controls.Add(RECHERCHE)
        Controls.Add(btnSupprimer)
        Controls.Add(btnAjouter)
        Name = "Matériaux"
        Text = "Matériaux"
        CType(DataGridView1, ComponentModel.ISupportInitialize).EndInit()
        ResumeLayout(False)
        PerformLayout()
    End Sub

    Friend WithEvents PrintPreviewDialog1 As PrintPreviewDialog
    Friend WithEvents PrintDocument1 As Printing.PrintDocument
    Friend WithEvents txtRECHERCHE As TextBox
    Friend WithEvents btnActualiser As Button
    Friend WithEvents btnNouveau As Button
    Friend WithEvents Label5 As Label
    Friend WithEvents Label6 As Label
    Friend WithEvents Label3 As Label
    Friend WithEvents Label2 As Label
    Friend WithEvents Label1 As Label
    Friend WithEvents Imprimer As Button
    Friend WithEvents txtCodeMatierePremiere As TextBox
    Friend WithEvents txtLibelleSilo As TextBox
    Friend WithEvents txtNomMateriau As TextBox
    Friend WithEvents txtCleRecherche As TextBox
    Friend WithEvents DataGridView1 As DataGridView
    Friend WithEvents txtCodeMateriau As TextBox
    Friend WithEvents btnModifier As Button
    Friend WithEvents RECHERCHE As Button
    Friend WithEvents btnSupprimer As Button
    Friend WithEvents btnAjouter As Button
    Friend WithEvents txtMateriauEquivalent As TextBox
    Friend WithEvents txtPrixRevient As TextBox
    Friend WithEvents txtDensite As TextBox
    Friend WithEvents txtStock As TextBox
    Friend WithEvents txtDosageMax As TextBox
    Friend WithEvents Label24 As Label
    Friend WithEvents Label23 As Label
    Friend WithEvents Label22 As Label
    Friend WithEvents Label21 As Label
    Friend WithEvents Label20 As Label
    Friend WithEvents Label25 As Label
    Friend WithEvents Label26 As Label
    Friend WithEvents Label27 As Label
    Friend WithEvents Label28 As Label
    Friend WithEvents Label29 As Label
    Friend WithEvents txtAbsorptionCapillaire As TextBox
    Friend WithEvents txtGestionStock As TextBox
    Friend WithEvents txtTauxAlcalin As TextBox
    Friend WithEvents txtTauxChlorure As TextBox
    Friend WithEvents txtTolerance As TextBox
    Friend WithEvents Label30 As Label
    Friend WithEvents txtSable As TextBox
    Friend WithEvents Label4 As Label
    Friend WithEvents txtDosageMiniBL As TextBox
    Friend WithEvents Label7 As Label
    Friend WithEvents Label8 As Label
    Friend WithEvents Label9 As Label
    Friend WithEvents Label10 As Label
    Friend WithEvents Label11 As Label
    Friend WithEvents txtUnite As TextBox
    Friend WithEvents txtMontantUnitaire As TextBox
    Friend WithEvents txtTarif2 As TextBox
    Friend WithEvents txtTarif1 As TextBox
    Friend WithEvents txtTVA As TextBox
    Friend WithEvents Label12 As Label
    Friend WithEvents Label13 As Label
    Friend WithEvents Label14 As Label
    Friend WithEvents txtNomProduit As TextBox
    Friend WithEvents txtCleRechercheProduit As TextBox
    Friend WithEvents txtCodeProduit As TextBox
    Friend WithEvents Label15 As Label
    Friend WithEvents txtDosageMaxiBL As TextBox
    Friend WithEvents ComboBox1 As ComboBox
    Friend WithEvents cmbTypeMateriau As ComboBox
    Friend WithEvents Label16 As Label
    Friend WithEvents lblNombre As Label
End Class
