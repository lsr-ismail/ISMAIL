<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class Bons_de_Pompage
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(Bons_de_Pompage))
        Label13 = New Label()
        btnEnregistrer = New Button()
        Label19 = New Label()
        txtMontantTTC = New TextBox()
        cmbStatut1 = New ComboBox()
        Label23 = New Label()
        txtMontantHT = New TextBox()
        Label26 = New Label()
        txtDuree = New TextBox()
        txtNomChantier = New TextBox()
        txtNomClient = New TextBox()
        lblNombre = New Label()
        Label14 = New Label()
        Label18 = New Label()
        PrintPreviewDialog1 = New PrintPreviewDialog()
        PrintDocument1 = New Printing.PrintDocument()
        Label21 = New Label()
        Label22 = New Label()
        Label24 = New Label()
        txtRemarques = New TextBox()
        txtTauxTVA = New TextBox()
        txtHeureFin = New TextBox()
        btnActualiser = New Button()
        Label16 = New Label()
        txtHeureDébut = New TextBox()
        Label2 = New Label()
        txtPUHT = New TextBox()
        Label3 = New Label()
        Label11 = New Label()
        txtVolumeM3 = New TextBox()
        txtChauffeur = New TextBox()
        Label1 = New Label()
        dtpDate = New DateTimePicker()
        DataGridView1 = New DataGridView()
        Label7 = New Label()
        Label9 = New Label()
        Label10 = New Label()
        AdresseClient = New Label()
        Label6 = New Label()
        txtIdBP = New TextBox()
        btnSupprimer = New Button()
        btnNouveau = New Button()
        btnJourMoins = New Button()
        btnJourPlus = New Button()
        txtNomPompe = New TextBox()
        txtCodePompe = New TextBox()
        txtNumeroBP = New TextBox()
        Label5 = New Label()
        CType(DataGridView1, ComponentModel.ISupportInitialize).BeginInit()
        SuspendLayout()
        ' 
        ' Label13
        ' 
        Label13.AutoSize = True
        Label13.Font = New Font("Segoe UI", 13.8F, FontStyle.Regular, GraphicsUnit.Point, CByte(0))
        Label13.Location = New Point(12, 9)
        Label13.Name = "Label13"
        Label13.Size = New Size(206, 31)
        Label13.TabIndex = 253
        Label13.Text = "Bons_de_Pompage"
        ' 
        ' btnEnregistrer
        ' 
        btnEnregistrer.Location = New Point(764, 42)
        btnEnregistrer.Name = "btnEnregistrer"
        btnEnregistrer.Size = New Size(178, 37)
        btnEnregistrer.TabIndex = 449
        btnEnregistrer.Text = "Enregistrer"
        btnEnregistrer.UseVisualStyleBackColor = True
        ' 
        ' Label19
        ' 
        Label19.AutoSize = True
        Label19.Location = New Point(531, 298)
        Label19.Name = "Label19"
        Label19.Size = New Size(93, 20)
        Label19.TabIndex = 448
        Label19.Text = "Montant TTC"
        ' 
        ' txtMontantTTC
        ' 
        txtMontantTTC.Location = New Point(684, 291)
        txtMontantTTC.Name = "txtMontantTTC"
        txtMontantTTC.Size = New Size(326, 27)
        txtMontantTTC.TabIndex = 447
        ' 
        ' cmbStatut1
        ' 
        cmbStatut1.FormattingEnabled = True
        cmbStatut1.Location = New Point(712, 424)
        cmbStatut1.Name = "cmbStatut1"
        cmbStatut1.Size = New Size(219, 28)
        cmbStatut1.TabIndex = 438
        ' 
        ' Label23
        ' 
        Label23.AutoSize = True
        Label23.Location = New Point(530, 232)
        Label23.Name = "Label23"
        Label23.Size = New Size(88, 20)
        Label23.TabIndex = 436
        Label23.Text = "Montant HT"
        ' 
        ' txtMontantHT
        ' 
        txtMontantHT.Location = New Point(684, 225)
        txtMontantHT.Name = "txtMontantHT"
        txtMontantHT.Size = New Size(326, 27)
        txtMontantHT.TabIndex = 435
        ' 
        ' Label26
        ' 
        Label26.AutoSize = True
        Label26.Location = New Point(532, 199)
        Label26.Name = "Label26"
        Label26.Size = New Size(49, 20)
        Label26.TabIndex = 434
        Label26.Text = "Durée"
        ' 
        ' txtDuree
        ' 
        txtDuree.Location = New Point(684, 192)
        txtDuree.Name = "txtDuree"
        txtDuree.Size = New Size(326, 27)
        txtDuree.TabIndex = 433
        ' 
        ' txtNomChantier
        ' 
        txtNomChantier.Location = New Point(178, 286)
        txtNomChantier.Name = "txtNomChantier"
        txtNomChantier.Size = New Size(326, 27)
        txtNomChantier.TabIndex = 430
        ' 
        ' txtNomClient
        ' 
        txtNomClient.Location = New Point(178, 253)
        txtNomClient.Name = "txtNomClient"
        txtNomClient.Size = New Size(326, 27)
        txtNomClient.TabIndex = 429
        ' 
        ' lblNombre
        ' 
        lblNombre.AutoSize = True
        lblNombre.Location = New Point(693, 465)
        lblNombre.Name = "lblNombre"
        lblNombre.Size = New Size(65, 20)
        lblNombre.TabIndex = 423
        lblNombre.Text = "Total HT"
        ' 
        ' Label14
        ' 
        Label14.AutoSize = True
        Label14.Location = New Point(580, 465)
        Label14.Name = "Label14"
        Label14.Size = New Size(64, 20)
        Label14.TabIndex = 422
        Label14.Text = "Nombre"
        ' 
        ' Label18
        ' 
        Label18.AutoSize = True
        Label18.Location = New Point(533, 328)
        Label18.Name = "Label18"
        Label18.Size = New Size(83, 20)
        Label18.TabIndex = 421
        Label18.Text = "Remarques"
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
        ' Label21
        ' 
        Label21.AutoSize = True
        Label21.Location = New Point(535, 425)
        Label21.Name = "Label21"
        Label21.Size = New Size(48, 20)
        Label21.TabIndex = 419
        Label21.Text = "Statut"
        ' 
        ' Label22
        ' 
        Label22.AutoSize = True
        Label22.Location = New Point(535, 261)
        Label22.Name = "Label22"
        Label22.Size = New Size(68, 20)
        Label22.TabIndex = 418
        Label22.Text = "Taux TVA"
        ' 
        ' Label24
        ' 
        Label24.AutoSize = True
        Label24.Location = New Point(546, 155)
        Label24.Name = "Label24"
        Label24.Size = New Size(72, 20)
        Label24.TabIndex = 417
        Label24.Text = "Heure Fin"
        ' 
        ' txtRemarques
        ' 
        txtRemarques.Location = New Point(684, 322)
        txtRemarques.Multiline = True
        txtRemarques.Name = "txtRemarques"
        txtRemarques.Size = New Size(326, 95)
        txtRemarques.TabIndex = 416
        ' 
        ' txtTauxTVA
        ' 
        txtTauxTVA.Location = New Point(684, 258)
        txtTauxTVA.Name = "txtTauxTVA"
        txtTauxTVA.Size = New Size(326, 27)
        txtTauxTVA.TabIndex = 415
        ' 
        ' txtHeureFin
        ' 
        txtHeureFin.Location = New Point(686, 145)
        txtHeureFin.Name = "txtHeureFin"
        txtHeureFin.Size = New Size(326, 27)
        txtHeureFin.TabIndex = 414
        ' 
        ' btnActualiser
        ' 
        btnActualiser.Location = New Point(580, 42)
        btnActualiser.Name = "btnActualiser"
        btnActualiser.Size = New Size(178, 37)
        btnActualiser.TabIndex = 413
        btnActualiser.Text = "Actualiser"
        btnActualiser.UseVisualStyleBackColor = True
        ' 
        ' Label16
        ' 
        Label16.AutoSize = True
        Label16.Location = New Point(535, 119)
        Label16.Name = "Label16"
        Label16.Size = New Size(94, 20)
        Label16.TabIndex = 411
        Label16.Text = "Heure Début"
        ' 
        ' txtHeureDébut
        ' 
        txtHeureDébut.Location = New Point(688, 112)
        txtHeureDébut.Name = "txtHeureDébut"
        txtHeureDébut.Size = New Size(326, 27)
        txtHeureDébut.TabIndex = 410
        ' 
        ' Label2
        ' 
        Label2.AutoSize = True
        Label2.Location = New Point(24, 456)
        Label2.Name = "Label2"
        Label2.Size = New Size(46, 20)
        Label2.TabIndex = 409
        Label2.Text = "PUHT"
        ' 
        ' txtPUHT
        ' 
        txtPUHT.Location = New Point(178, 449)
        txtPUHT.Name = "txtPUHT"
        txtPUHT.Size = New Size(326, 27)
        txtPUHT.TabIndex = 408
        ' 
        ' Label3
        ' 
        Label3.AutoSize = True
        Label3.Location = New Point(22, 423)
        Label3.Name = "Label3"
        Label3.Size = New Size(84, 20)
        Label3.TabIndex = 407
        Label3.Text = "Volume M3"
        ' 
        ' Label11
        ' 
        Label11.AutoSize = True
        Label11.Location = New Point(24, 390)
        Label11.Name = "Label11"
        Label11.Size = New Size(73, 20)
        Label11.TabIndex = 406
        Label11.Text = "Chauffeur"
        ' 
        ' txtVolumeM3
        ' 
        txtVolumeM3.Location = New Point(176, 416)
        txtVolumeM3.Name = "txtVolumeM3"
        txtVolumeM3.Size = New Size(326, 27)
        txtVolumeM3.TabIndex = 405
        ' 
        ' txtChauffeur
        ' 
        txtChauffeur.Location = New Point(176, 383)
        txtChauffeur.Name = "txtChauffeur"
        txtChauffeur.Size = New Size(326, 27)
        txtChauffeur.TabIndex = 404
        ' 
        ' Label1
        ' 
        Label1.AutoSize = True
        Label1.Location = New Point(29, 258)
        Label1.Name = "Label1"
        Label1.Size = New Size(84, 20)
        Label1.TabIndex = 403
        Label1.Text = "Nom Client"
        ' 
        ' dtpDate
        ' 
        dtpDate.Location = New Point(225, 156)
        dtpDate.Name = "dtpDate"
        dtpDate.Size = New Size(250, 27)
        dtpDate.TabIndex = 401
        ' 
        ' DataGridView1
        ' 
        DataGridView1.AllowUserToAddRows = False
        DataGridView1.Anchor = AnchorStyles.Top Or AnchorStyles.Bottom Or AnchorStyles.Left Or AnchorStyles.Right
        DataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill
        DataGridView1.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize
        DataGridView1.Location = New Point(66, 502)
        DataGridView1.MultiSelect = False
        DataGridView1.Name = "DataGridView1"
        DataGridView1.ReadOnly = True
        DataGridView1.RowHeadersWidth = 51
        DataGridView1.SelectionMode = DataGridViewSelectionMode.FullRowSelect
        DataGridView1.Size = New Size(833, 270)
        DataGridView1.TabIndex = 400
        ' 
        ' Label7
        ' 
        Label7.AutoSize = True
        Label7.Location = New Point(24, 357)
        Label7.Name = "Label7"
        Label7.Size = New Size(92, 20)
        Label7.TabIndex = 398
        Label7.Text = "Nom Pompe"
        ' 
        ' Label9
        ' 
        Label9.AutoSize = True
        Label9.Location = New Point(26, 324)
        Label9.Name = "Label9"
        Label9.Size = New Size(94, 20)
        Label9.TabIndex = 397
        Label9.Text = "Code Pompe"
        ' 
        ' Label10
        ' 
        Label10.AutoSize = True
        Label10.Location = New Point(29, 291)
        Label10.Name = "Label10"
        Label10.Size = New Size(101, 20)
        Label10.TabIndex = 396
        Label10.Text = "Nom Chantier"
        ' 
        ' AdresseClient
        ' 
        AdresseClient.AutoSize = True
        AdresseClient.Location = New Point(29, 195)
        AdresseClient.Name = "AdresseClient"
        AdresseClient.Size = New Size(43, 20)
        AdresseClient.TabIndex = 395
        AdresseClient.Text = "Id BP"
        ' 
        ' Label6
        ' 
        Label6.AutoSize = True
        Label6.Location = New Point(31, 159)
        Label6.Name = "Label6"
        Label6.Size = New Size(92, 20)
        Label6.TabIndex = 394
        Label6.Text = "Date Facture"
        ' 
        ' txtIdBP
        ' 
        txtIdBP.Location = New Point(178, 192)
        txtIdBP.Name = "txtIdBP"
        txtIdBP.Size = New Size(326, 27)
        txtIdBP.TabIndex = 392
        ' 
        ' btnSupprimer
        ' 
        btnSupprimer.Location = New Point(396, 42)
        btnSupprimer.Name = "btnSupprimer"
        btnSupprimer.Size = New Size(178, 37)
        btnSupprimer.TabIndex = 388
        btnSupprimer.Text = "SUPRIME"
        btnSupprimer.UseVisualStyleBackColor = True
        ' 
        ' btnNouveau
        ' 
        btnNouveau.Location = New Point(208, 42)
        btnNouveau.Name = "btnNouveau"
        btnNouveau.Size = New Size(178, 37)
        btnNouveau.TabIndex = 387
        btnNouveau.Text = "Nouveau"
        btnNouveau.UseVisualStyleBackColor = True
        ' 
        ' btnJourMoins
        ' 
        btnJourMoins.Location = New Point(225, 119)
        btnJourMoins.Name = "btnJourMoins"
        btnJourMoins.Size = New Size(82, 37)
        btnJourMoins.TabIndex = 451
        btnJourMoins.Text = "-"
        btnJourMoins.UseVisualStyleBackColor = True
        ' 
        ' btnJourPlus
        ' 
        btnJourPlus.Location = New Point(379, 119)
        btnJourPlus.Name = "btnJourPlus"
        btnJourPlus.Size = New Size(96, 37)
        btnJourPlus.TabIndex = 452
        btnJourPlus.Text = "+"
        btnJourPlus.UseVisualStyleBackColor = True
        ' 
        ' txtNomPompe
        ' 
        txtNomPompe.Location = New Point(178, 352)
        txtNomPompe.Name = "txtNomPompe"
        txtNomPompe.Size = New Size(326, 27)
        txtNomPompe.TabIndex = 454
        ' 
        ' txtCodePompe
        ' 
        txtCodePompe.Location = New Point(178, 319)
        txtCodePompe.Name = "txtCodePompe"
        txtCodePompe.Size = New Size(326, 27)
        txtCodePompe.TabIndex = 453
        ' 
        ' txtNumeroBP
        ' 
        txtNumeroBP.Location = New Point(178, 222)
        txtNumeroBP.Name = "txtNumeroBP"
        txtNumeroBP.Size = New Size(326, 27)
        txtNumeroBP.TabIndex = 456
        ' 
        ' Label5
        ' 
        Label5.AutoSize = True
        Label5.Location = New Point(29, 227)
        Label5.Name = "Label5"
        Label5.Size = New Size(84, 20)
        Label5.TabIndex = 455
        Label5.Text = "Numéro BP"
        ' 
        ' Bons_de_Pompage
        ' 
        AutoScaleDimensions = New SizeF(8F, 20F)
        AutoScaleMode = AutoScaleMode.Font
        ClientSize = New Size(1082, 723)
        Controls.Add(txtNumeroBP)
        Controls.Add(Label5)
        Controls.Add(txtNomPompe)
        Controls.Add(txtCodePompe)
        Controls.Add(btnJourPlus)
        Controls.Add(btnJourMoins)
        Controls.Add(btnEnregistrer)
        Controls.Add(Label19)
        Controls.Add(txtMontantTTC)
        Controls.Add(cmbStatut1)
        Controls.Add(Label23)
        Controls.Add(txtMontantHT)
        Controls.Add(Label26)
        Controls.Add(txtDuree)
        Controls.Add(txtNomChantier)
        Controls.Add(txtNomClient)
        Controls.Add(lblNombre)
        Controls.Add(Label14)
        Controls.Add(Label18)
        Controls.Add(Label21)
        Controls.Add(Label22)
        Controls.Add(Label24)
        Controls.Add(txtRemarques)
        Controls.Add(txtTauxTVA)
        Controls.Add(txtHeureFin)
        Controls.Add(btnActualiser)
        Controls.Add(Label16)
        Controls.Add(txtHeureDébut)
        Controls.Add(Label2)
        Controls.Add(txtPUHT)
        Controls.Add(Label3)
        Controls.Add(Label11)
        Controls.Add(txtVolumeM3)
        Controls.Add(txtChauffeur)
        Controls.Add(Label1)
        Controls.Add(dtpDate)
        Controls.Add(DataGridView1)
        Controls.Add(Label7)
        Controls.Add(Label9)
        Controls.Add(Label10)
        Controls.Add(AdresseClient)
        Controls.Add(Label6)
        Controls.Add(txtIdBP)
        Controls.Add(btnSupprimer)
        Controls.Add(btnNouveau)
        Controls.Add(Label13)
        Name = "Bons_de_Pompage"
        Text = "Bons_de_Pompage"
        CType(DataGridView1, ComponentModel.ISupportInitialize).EndInit()
        ResumeLayout(False)
        PerformLayout()
    End Sub

    Friend WithEvents Label13 As Label
    Friend WithEvents btnImprimer As Button
    Friend WithEvents btnValider As Button
    Friend WithEvents btnEnregistrer As Button
    Friend WithEvents Label19 As Label
    Friend WithEvents txtMontantTTC As TextBox
    Friend WithEvents Label27 As Label
    Friend WithEvents txtMontantRestant As TextBox
    Friend WithEvents lblTotalTTC As Label
    Friend WithEvents lblTotalRestant As Label
    Friend WithEvents lblTotalRegle As Label
    Friend WithEvents lblTotalHT As Label
    Friend WithEvents lblNbFactures As Label
    Friend WithEvents Label17 As Label
    Friend WithEvents cmbStatut1 As ComboBox
    Friend WithEvents cmbStatut As ComboBox
    Friend WithEvents Label23 As Label
    Friend WithEvents txtMontantHT As TextBox
    Friend WithEvents Label26 As Label
    Friend WithEvents txtDuree As TextBox
    Friend WithEvents dtpPeriodeAu As DateTimePicker
    Friend WithEvents dtpPeriodeDu As DateTimePicker
    Friend WithEvents txtNomChantier As TextBox
    Friend WithEvents txtNomClient As TextBox
    Friend WithEvents Label15 As Label
    Friend WithEvents txtNumeroFacture As TextBox
    Friend WithEvents Label25 As Label
    Friend WithEvents Label4 As Label
    Friend WithEvents Label8 As Label
    Friend WithEvents lblNombre As Label
    Friend WithEvents Label14 As Label
    Friend WithEvents Label18 As Label
    Friend WithEvents PrintPreviewDialog1 As PrintPreviewDialog
    Friend WithEvents txtICE As TextBox
    Friend WithEvents PrintDocument1 As Printing.PrintDocument
    Friend WithEvents Label20 As Label
    Friend WithEvents Label21 As Label
    Friend WithEvents Label22 As Label
    Friend WithEvents Label24 As Label
    Friend WithEvents txtRemarques As TextBox
    Friend WithEvents txtTauxTVA As TextBox
    Friend WithEvents txtHeureFin As TextBox
    Friend WithEvents btnActualiser As Button
    Friend WithEvents btnMarquerRegle As Button
    Friend WithEvents Label16 As Label
    Friend WithEvents txtHeureDébut As TextBox
    Friend WithEvents Label2 As Label
    Friend WithEvents txtPUHT As TextBox
    Friend WithEvents Label3 As Label
    Friend WithEvents Label11 As Label
    Friend WithEvents txtVolumeM3 As TextBox
    Friend WithEvents txtChauffeur As TextBox
    Friend WithEvents Label1 As Label
    Friend WithEvents dtpDateEcheance As DateTimePicker
    Friend WithEvents dtpDate As DateTimePicker
    Friend WithEvents DataGridView1 As DataGridView
    Friend WithEvents Label7 As Label
    Friend WithEvents Label9 As Label
    Friend WithEvents Label10 As Label
    Friend WithEvents AdresseClient As Label
    Friend WithEvents Label6 As Label
    Friend WithEvents Label28 As Label
    Friend WithEvents txtIdBP As TextBox
    Friend WithEvents txtIdFacture As TextBox
    Friend WithEvents DataGridViewFactures As DataGridView
    Friend WithEvents btnSupprimer As Button
    Friend WithEvents btnNouveau As Button
    Friend WithEvents btnJourMoins As Button
    Friend WithEvents btnJourPlus As Button
    Friend WithEvents txtNomPompe As TextBox
    Friend WithEvents txtCodePompe As TextBox
    Friend WithEvents txtNumeroBP As TextBox
    Friend WithEvents Label5 As Label
End Class
