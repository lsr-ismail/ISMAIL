<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class Produits
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(Produits))
        Label21 = New Label()
        chkServiceTransport = New CheckBox()
        PrintDocument1 = New Printing.PrintDocument()
        PrintPreviewDialog1 = New PrintPreviewDialog()
        txtRECHERCHE = New TextBox()
        btnActualiserTable = New Button()
        btnNouveauTable = New Button()
        lblCodeExport = New Label()
        lblServiceAssocie3 = New Label()
        lblServiceAssocie2 = New Label()
        lblServiceAssocie1 = New Label()
        lblNomElement = New Label()
        lblCodeElement = New Label()
        Imprimer = New Button()
        txtNatureService = New TextBox()
        txtTVA = New TextBox()
        txtCodeFacturation = New TextBox()
        txtNomProduit = New TextBox()
        txtCleProduit = New TextBox()
        DataGridViewTables = New DataGridView()
        btnEditeTable = New Button()
        btnSupprimeTable = New Button()
        btnAjouteTable = New Button()
        Label1 = New Label()
        v = New Label()
        Label4 = New Label()
        Label6 = New Label()
        Label7 = New Label()
        txtMontantUnitaire2 = New TextBox()
        cmbUnite1 = New ComboBox()
        Label5 = New Label()
        cmbUnite2 = New ComboBox()
        Label3 = New Label()
        txtIdProduit = New TextBox()
        dtpDateEntree1 = New DateTimePicker()
        txtMontantUnitaire1 = New TextBox()
        txtCodeProduit = New TextBox()
        chkActif = New CheckBox()
        dtpDateEntree2 = New DateTimePicker()
        ComboBox1 = New ComboBox()
        Label8 = New Label()
        Label9 = New Label()
        Label10 = New Label()
        CType(DataGridViewTables, ComponentModel.ISupportInitialize).BeginInit()
        SuspendLayout()
        ' 
        ' Label21
        ' 
        Label21.AutoSize = True
        Label21.Font = New Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point, CByte(0))
        Label21.Location = New Point(12, 9)
        Label21.Name = "Label21"
        Label21.Size = New Size(85, 28)
        Label21.TabIndex = 153
        Label21.Text = "Produits"
        ' 
        ' chkServiceTransport
        ' 
        chkServiceTransport.AutoSize = True
        chkServiceTransport.Location = New Point(418, 416)
        chkServiceTransport.Name = "chkServiceTransport"
        chkServiceTransport.Size = New Size(18, 17)
        chkServiceTransport.TabIndex = 279
        chkServiceTransport.UseVisualStyleBackColor = True
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
        txtRECHERCHE.Location = New Point(216, 120)
        txtRECHERCHE.Name = "txtRECHERCHE"
        txtRECHERCHE.Size = New Size(295, 27)
        txtRECHERCHE.TabIndex = 277
        ' 
        ' btnActualiserTable
        ' 
        btnActualiserTable.Location = New Point(770, 50)
        btnActualiserTable.Name = "btnActualiserTable"
        btnActualiserTable.Size = New Size(140, 61)
        btnActualiserTable.TabIndex = 276
        btnActualiserTable.Text = "Actualiser"
        btnActualiserTable.UseVisualStyleBackColor = True
        ' 
        ' btnNouveauTable
        ' 
        btnNouveauTable.Location = New Point(624, 50)
        btnNouveauTable.Name = "btnNouveauTable"
        btnNouveauTable.Size = New Size(140, 61)
        btnNouveauTable.TabIndex = 275
        btnNouveauTable.Text = "Nouveau"
        btnNouveauTable.UseVisualStyleBackColor = True
        ' 
        ' lblCodeExport
        ' 
        lblCodeExport.AutoSize = True
        lblCodeExport.Location = New Point(63, 385)
        lblCodeExport.Name = "lblCodeExport"
        lblCodeExport.Size = New Size(105, 20)
        lblCodeExport.TabIndex = 273
        lblCodeExport.Text = "Nature Service"
        ' 
        ' lblServiceAssocie3
        ' 
        lblServiceAssocie3.AutoSize = True
        lblServiceAssocie3.Location = New Point(61, 352)
        lblServiceAssocie3.Name = "lblServiceAssocie3"
        lblServiceAssocie3.Size = New Size(35, 20)
        lblServiceAssocie3.TabIndex = 272
        lblServiceAssocie3.Text = "TVA"
        ' 
        ' lblServiceAssocie2
        ' 
        lblServiceAssocie2.AutoSize = True
        lblServiceAssocie2.Location = New Point(61, 319)
        lblServiceAssocie2.Name = "lblServiceAssocie2"
        lblServiceAssocie2.Size = New Size(121, 20)
        lblServiceAssocie2.TabIndex = 271
        lblServiceAssocie2.Text = "Code Facturation"
        ' 
        ' lblServiceAssocie1
        ' 
        lblServiceAssocie1.AutoSize = True
        lblServiceAssocie1.Location = New Point(58, 286)
        lblServiceAssocie1.Name = "lblServiceAssocie1"
        lblServiceAssocie1.Size = New Size(92, 20)
        lblServiceAssocie1.TabIndex = 270
        lblServiceAssocie1.Text = "Type Produit"
        ' 
        ' lblNomElement
        ' 
        lblNomElement.AutoSize = True
        lblNomElement.Location = New Point(61, 253)
        lblNomElement.Name = "lblNomElement"
        lblNomElement.Size = New Size(94, 20)
        lblNomElement.TabIndex = 269
        lblNomElement.Text = "Nom Produit"
        ' 
        ' lblCodeElement
        ' 
        lblCodeElement.AutoSize = True
        lblCodeElement.Location = New Point(61, 220)
        lblCodeElement.Name = "lblCodeElement"
        lblCodeElement.Size = New Size(82, 20)
        lblCodeElement.TabIndex = 268
        lblCodeElement.Text = "Cle Produit"
        ' 
        ' Imprimer
        ' 
        Imprimer.Location = New Point(478, 50)
        Imprimer.Name = "Imprimer"
        Imprimer.Size = New Size(140, 61)
        Imprimer.TabIndex = 266
        Imprimer.Text = "Imprimer"
        Imprimer.UseVisualStyleBackColor = True
        ' 
        ' txtNatureService
        ' 
        txtNatureService.Location = New Point(219, 380)
        txtNatureService.Name = "txtNatureService"
        txtNatureService.Size = New Size(295, 27)
        txtNatureService.TabIndex = 265
        ' 
        ' txtTVA
        ' 
        txtTVA.Location = New Point(219, 348)
        txtTVA.Name = "txtTVA"
        txtTVA.Size = New Size(295, 27)
        txtTVA.TabIndex = 264
        ' 
        ' txtCodeFacturation
        ' 
        txtCodeFacturation.Location = New Point(219, 315)
        txtCodeFacturation.Name = "txtCodeFacturation"
        txtCodeFacturation.Size = New Size(295, 27)
        txtCodeFacturation.TabIndex = 263
        ' 
        ' txtNomProduit
        ' 
        txtNomProduit.Location = New Point(219, 249)
        txtNomProduit.Name = "txtNomProduit"
        txtNomProduit.Size = New Size(295, 27)
        txtNomProduit.TabIndex = 261
        ' 
        ' txtCleProduit
        ' 
        txtCleProduit.Location = New Point(219, 215)
        txtCleProduit.Name = "txtCleProduit"
        txtCleProduit.Size = New Size(295, 27)
        txtCleProduit.TabIndex = 260
        ' 
        ' DataGridViewTables
        ' 
        DataGridViewTables.AllowUserToAddRows = False
        DataGridViewTables.Anchor = AnchorStyles.Top Or AnchorStyles.Bottom Or AnchorStyles.Left Or AnchorStyles.Right
        DataGridViewTables.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill
        DataGridViewTables.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize
        DataGridViewTables.Location = New Point(520, 135)
        DataGridViewTables.MultiSelect = False
        DataGridViewTables.Name = "DataGridViewTables"
        DataGridViewTables.ReadOnly = True
        DataGridViewTables.RowHeadersWidth = 51
        DataGridViewTables.SelectionMode = DataGridViewSelectionMode.FullRowSelect
        DataGridViewTables.Size = New Size(536, 500)
        DataGridViewTables.TabIndex = 259
        ' 
        ' btnEditeTable
        ' 
        btnEditeTable.Location = New Point(332, 50)
        btnEditeTable.Name = "btnEditeTable"
        btnEditeTable.Size = New Size(140, 61)
        btnEditeTable.TabIndex = 258
        btnEditeTable.Text = "EDITE"
        btnEditeTable.UseVisualStyleBackColor = True
        ' 
        ' btnSupprimeTable
        ' 
        btnSupprimeTable.Location = New Point(186, 50)
        btnSupprimeTable.Name = "btnSupprimeTable"
        btnSupprimeTable.Size = New Size(140, 61)
        btnSupprimeTable.TabIndex = 256
        btnSupprimeTable.Text = "SUPRIME"
        btnSupprimeTable.UseVisualStyleBackColor = True
        ' 
        ' btnAjouteTable
        ' 
        btnAjouteTable.Location = New Point(40, 50)
        btnAjouteTable.Name = "btnAjouteTable"
        btnAjouteTable.Size = New Size(140, 61)
        btnAjouteTable.TabIndex = 255
        btnAjouteTable.Text = "AJOUTER"
        btnAjouteTable.UseVisualStyleBackColor = True
        ' 
        ' Label1
        ' 
        Label1.AutoSize = True
        Label1.Location = New Point(61, 413)
        Label1.Name = "Label1"
        Label1.Size = New Size(122, 20)
        Label1.TabIndex = 283
        Label1.Text = "Service Transport"
        ' 
        ' v
        ' 
        v.AutoSize = True
        v.Location = New Point(63, 578)
        v.Name = "v"
        v.Size = New Size(56, 20)
        v.TabIndex = 295
        v.Text = "Unité 2"
        ' 
        ' Label4
        ' 
        Label4.AutoSize = True
        Label4.Location = New Point(61, 546)
        Label4.Name = "Label4"
        Label4.Size = New Size(133, 20)
        Label4.TabIndex = 292
        Label4.Text = "Montan tUnitaire 2"
        ' 
        ' Label6
        ' 
        Label6.AutoSize = True
        Label6.Location = New Point(58, 480)
        Label6.Name = "Label6"
        Label6.Size = New Size(133, 20)
        Label6.TabIndex = 290
        Label6.Text = "Montant Unitaire 1"
        ' 
        ' Label7
        ' 
        Label7.AutoSize = True
        Label7.Location = New Point(61, 447)
        Label7.Name = "Label7"
        Label7.Size = New Size(99, 20)
        Label7.TabIndex = 289
        Label7.Text = "Date Entrée 1"
        ' 
        ' txtMontantUnitaire2
        ' 
        txtMontantUnitaire2.Location = New Point(219, 542)
        txtMontantUnitaire2.Name = "txtMontantUnitaire2"
        txtMontantUnitaire2.Size = New Size(295, 27)
        txtMontantUnitaire2.TabIndex = 287
        ' 
        ' cmbUnite1
        ' 
        cmbUnite1.FormattingEnabled = True
        cmbUnite1.Location = New Point(219, 509)
        cmbUnite1.Name = "cmbUnite1"
        cmbUnite1.Size = New Size(292, 28)
        cmbUnite1.TabIndex = 297
        ' 
        ' Label5
        ' 
        Label5.AutoSize = True
        Label5.Location = New Point(61, 514)
        Label5.Name = "Label5"
        Label5.Size = New Size(56, 20)
        Label5.TabIndex = 296
        Label5.Text = "Unité 1"
        ' 
        ' cmbUnite2
        ' 
        cmbUnite2.FormattingEnabled = True
        cmbUnite2.Location = New Point(219, 575)
        cmbUnite2.Name = "cmbUnite2"
        cmbUnite2.Size = New Size(292, 28)
        cmbUnite2.TabIndex = 298
        ' 
        ' Label3
        ' 
        Label3.AutoSize = True
        Label3.Location = New Point(58, 159)
        Label3.Name = "Label3"
        Label3.Size = New Size(74, 20)
        Label3.TabIndex = 300
        Label3.Text = "Id Produit"
        ' 
        ' txtIdProduit
        ' 
        txtIdProduit.Location = New Point(216, 153)
        txtIdProduit.Name = "txtIdProduit"
        txtIdProduit.ShortcutsEnabled = False
        txtIdProduit.Size = New Size(295, 27)
        txtIdProduit.TabIndex = 299
        ' 
        ' dtpDateEntree1
        ' 
        dtpDateEntree1.Location = New Point(216, 442)
        dtpDateEntree1.Name = "dtpDateEntree1"
        dtpDateEntree1.Size = New Size(250, 27)
        dtpDateEntree1.TabIndex = 301
        ' 
        ' txtMontantUnitaire1
        ' 
        txtMontantUnitaire1.Location = New Point(219, 476)
        txtMontantUnitaire1.Name = "txtMontantUnitaire1"
        txtMontantUnitaire1.Size = New Size(295, 27)
        txtMontantUnitaire1.TabIndex = 285
        ' 
        ' txtCodeProduit
        ' 
        txtCodeProduit.Location = New Point(218, 184)
        txtCodeProduit.Name = "txtCodeProduit"
        txtCodeProduit.Size = New Size(149, 27)
        txtCodeProduit.TabIndex = 303
        ' 
        ' chkActif
        ' 
        chkActif.AutoSize = True
        chkActif.Location = New Point(453, 194)
        chkActif.Name = "chkActif"
        chkActif.Size = New Size(18, 17)
        chkActif.TabIndex = 305
        chkActif.UseVisualStyleBackColor = True
        ' 
        ' dtpDateEntree2
        ' 
        dtpDateEntree2.Location = New Point(219, 613)
        dtpDateEntree2.Name = "dtpDateEntree2"
        dtpDateEntree2.Size = New Size(250, 27)
        dtpDateEntree2.TabIndex = 307
        ' 
        ' ComboBox1
        ' 
        ComboBox1.FormattingEnabled = True
        ComboBox1.Location = New Point(219, 281)
        ComboBox1.Name = "ComboBox1"
        ComboBox1.Size = New Size(292, 28)
        ComboBox1.TabIndex = 308
        ' 
        ' Label8
        ' 
        Label8.AutoSize = True
        Label8.Location = New Point(376, 191)
        Label8.Name = "Label8"
        Label8.Size = New Size(62, 20)
        Label8.TabIndex = 309
        Label8.Text = "Actif"
        ' 
        ' Label9
        ' 
        Label9.AutoSize = True
        Label9.Location = New Point(66, 184)
        Label9.Name = "Label9"
        Label9.Size = New Size(96, 20)
        Label9.TabIndex = 310
        Label9.Text = "Code Produit"
        ' 
        ' Label10
        ' 
        Label10.AutoSize = True
        Label10.Location = New Point(64, 618)
        Label10.Name = "Label10"
        Label10.Size = New Size(99, 20)
        Label10.TabIndex = 312
        Label10.Text = "Date Entrée 2"
        ' 
        ' Produits
        ' 
        AutoScaleDimensions = New SizeF(8F, 20F)
        AutoScaleMode = AutoScaleMode.Font
        ClientSize = New Size(1082, 653)
        Controls.Add(Label10)
        Controls.Add(Label9)
        Controls.Add(Label8)
        Controls.Add(ComboBox1)
        Controls.Add(dtpDateEntree2)
        Controls.Add(chkActif)
        Controls.Add(txtCodeProduit)
        Controls.Add(dtpDateEntree1)
        Controls.Add(Label3)
        Controls.Add(txtIdProduit)
        Controls.Add(cmbUnite2)
        Controls.Add(cmbUnite1)
        Controls.Add(Label5)
        Controls.Add(v)
        Controls.Add(Label4)
        Controls.Add(Label6)
        Controls.Add(Label7)
        Controls.Add(txtMontantUnitaire2)
        Controls.Add(txtMontantUnitaire1)
        Controls.Add(Label1)
        Controls.Add(chkServiceTransport)
        Controls.Add(txtRECHERCHE)
        Controls.Add(btnActualiserTable)
        Controls.Add(btnNouveauTable)
        Controls.Add(lblCodeExport)
        Controls.Add(lblServiceAssocie3)
        Controls.Add(lblServiceAssocie2)
        Controls.Add(lblServiceAssocie1)
        Controls.Add(lblNomElement)
        Controls.Add(lblCodeElement)
        Controls.Add(Imprimer)
        Controls.Add(txtNatureService)
        Controls.Add(txtTVA)
        Controls.Add(txtCodeFacturation)
        Controls.Add(txtNomProduit)
        Controls.Add(txtCleProduit)
        Controls.Add(DataGridViewTables)
        Controls.Add(btnEditeTable)
        Controls.Add(btnSupprimeTable)
        Controls.Add(btnAjouteTable)
        Controls.Add(Label21)
        Name = "Produits"
        Text = "Produits"
        CType(DataGridViewTables, ComponentModel.ISupportInitialize).EndInit()
        ResumeLayout(False)
        PerformLayout()
    End Sub

    Friend WithEvents Label21 As Label
    Friend WithEvents txtImpression As TextBox
    Friend WithEvents chkServiceTransport As CheckBox
    Friend WithEvents PrintDocument1 As Printing.PrintDocument
    Friend WithEvents PrintPreviewDialog1 As PrintPreviewDialog
    Friend WithEvents lblImpression As Label
    Friend WithEvents txtRECHERCHE As TextBox
    Friend WithEvents btnActualiserTable As Button
    Friend WithEvents btnNouveauTable As Button
    Friend WithEvents lblCodeExport As Label
    Friend WithEvents lblServiceAssocie3 As Label
    Friend WithEvents lblServiceAssocie2 As Label
    Friend WithEvents lblServiceAssocie1 As Label
    Friend WithEvents lblNomElement As Label
    Friend WithEvents lblCodeElement As Label
    Friend WithEvents Imprimer As Button
    Friend WithEvents txtNatureService As TextBox
    Friend WithEvents txtTVA As TextBox
    Friend WithEvents txtCodeFacturation As TextBox
    Friend WithEvents cmbTypeProduit As TextBox
    Friend WithEvents txtNomProduit As TextBox
    Friend WithEvents txtCleProduit As TextBox
    Friend WithEvents DataGridViewTables As DataGridView
    Friend WithEvents btnEditeTable As Button
    Friend WithEvents btnSupprimeTable As Button
    Friend WithEvents btnAjouteTable As Button
    Friend WithEvents Label1 As Label
    Friend WithEvents v As Label
    Friend WithEvents Label4 As Label
    Friend WithEvents Label6 As Label
    Friend WithEvents Label7 As Label
    Friend WithEvents txtMontantUnitaire2 As TextBox
    Friend WithEvents TextBox5 As TextBox
    Friend WithEvents cmbUnite1 As ComboBox
    Friend WithEvents Label5 As Label
    Friend WithEvents cmbUnite2 As ComboBox
    Friend WithEvents Label3 As Label
    Friend WithEvents txtIdProduit As TextBox
    Friend WithEvents dtpDateEntree1 As DateTimePicker
    Friend WithEvents txtMontantUnitaire1 As TextBox
    Friend WithEvents txtCodeProduit As TextBox
    Friend WithEvents chkActif As CheckBox
    Friend WithEvents dtpDateEntree2 As DateTimePicker
    Friend WithEvents ComboBox1 As ComboBox
    Friend WithEvents Label8 As Label
    Friend WithEvents Label9 As Label
    Friend WithEvents Label10 As Label


End Class
