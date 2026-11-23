<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class Planning_camions
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
        Label21 = New Label()
        lblNombre = New Label()
        btnImprimer = New Button()
        btnNouveau = New Button()
        Label19 = New Label()
        txtNumeroBL = New TextBox()
        Label11 = New Label()
        Label12 = New Label()
        Label4 = New Label()
        Label5 = New Label()
        Label6 = New Label()
        Label3 = New Label()
        Label2 = New Label()
        Label1 = New Label()
        txtObservation = New TextBox()
        txtNumeroTour = New TextBox()
        txtQuantiteM3 = New TextBox()
        txtHeureRetour = New TextBox()
        txtHeureDepart = New TextBox()
        txtChauffeur = New TextBox()
        txtNomCamion = New TextBox()
        txtCodeCamion = New TextBox()
        btnActualiser = New Button()
        txtResteTotal = New TextBox()
        lblNombreEnregistrements = New TextBox()
        btnJourMoins = New Button()
        btnJourPlus = New Button()
        dtpDatePlanning = New DateTimePicker()
        DataGridView1 = New DataGridView()
        btnModifier = New Button()
        btnSupprimer = New Button()
        btnAjouter = New Button()
        cmbStatut = New ComboBox()
        Label7 = New Label()
        btnEnCours = New Button()
        btnTerminer = New Button()
        CType(DataGridView1, ComponentModel.ISupportInitialize).BeginInit()
        SuspendLayout()
        ' 
        ' Label21
        ' 
        Label21.AutoSize = True
        Label21.Font = New Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point, CByte(0))
        Label21.Location = New Point(12, 9)
        Label21.Name = "Label21"
        Label21.Size = New Size(168, 28)
        Label21.TabIndex = 154
        Label21.Text = "Planning_camions"
        ' 
        ' lblNombre
        ' 
        lblNombre.AutoSize = True
        lblNombre.Location = New Point(248, 121)
        lblNombre.Name = "lblNombre"
        lblNombre.Size = New Size(81, 20)
        lblNombre.TabIndex = 403
        lblNombre.Text = "lblNombre"
        ' 
        ' btnImprimer
        ' 
        btnImprimer.Location = New Point(575, 28)
        btnImprimer.Name = "btnImprimer"
        btnImprimer.Size = New Size(178, 61)
        btnImprimer.TabIndex = 402
        btnImprimer.Text = "Imprimer"
        btnImprimer.UseVisualStyleBackColor = True
        ' 
        ' btnNouveau
        ' 
        btnNouveau.Location = New Point(773, 28)
        btnNouveau.Name = "btnNouveau"
        btnNouveau.Size = New Size(146, 61)
        btnNouveau.TabIndex = 401
        btnNouveau.Text = "btnNouveau"
        btnNouveau.UseVisualStyleBackColor = True
        ' 
        ' Label19
        ' 
        Label19.AutoSize = True
        Label19.Location = New Point(141, 313)
        Label19.Name = "Label19"
        Label19.Size = New Size(96, 20)
        Label19.TabIndex = 400
        Label19.Text = "txtNumeroBL"
        ' 
        ' txtNumeroBL
        ' 
        txtNumeroBL.Location = New Point(255, 306)
        txtNumeroBL.Name = "txtNumeroBL"
        txtNumeroBL.Size = New Size(418, 27)
        txtNumeroBL.TabIndex = 399
        ' 
        ' Label11
        ' 
        Label11.AutoSize = True
        Label11.Location = New Point(139, 409)
        Label11.Name = "Label11"
        Label11.Size = New Size(106, 20)
        Label11.TabIndex = 397
        Label11.Text = "txtObservation"
        ' 
        ' Label12
        ' 
        Label12.AutoSize = True
        Label12.Location = New Point(142, 369)
        Label12.Name = "Label12"
        Label12.Size = New Size(109, 20)
        Label12.TabIndex = 396
        Label12.Text = "txtNumeroTour"
        ' 
        ' Label4
        ' 
        Label4.AutoSize = True
        Label4.Location = New Point(139, 343)
        Label4.Name = "Label4"
        Label4.Size = New Size(104, 20)
        Label4.TabIndex = 395
        Label4.Text = "txtQuantiteM3"
        ' 
        ' Label5
        ' 
        Label5.AutoSize = True
        Label5.Location = New Point(139, 280)
        Label5.Name = "Label5"
        Label5.Size = New Size(110, 20)
        Label5.TabIndex = 394
        Label5.Text = "txtHeureRetour"
        ' 
        ' Label6
        ' 
        Label6.AutoSize = True
        Label6.Location = New Point(136, 247)
        Label6.Name = "Label6"
        Label6.Size = New Size(112, 20)
        Label6.TabIndex = 393
        Label6.Text = "txtHeureDepart"
        ' 
        ' Label3
        ' 
        Label3.AutoSize = True
        Label3.Location = New Point(139, 214)
        Label3.Name = "Label3"
        Label3.Size = New Size(90, 20)
        Label3.TabIndex = 392
        Label3.Text = "txtChauffeur"
        ' 
        ' Label2
        ' 
        Label2.AutoSize = True
        Label2.Location = New Point(139, 181)
        Label2.Name = "Label2"
        Label2.Size = New Size(110, 20)
        Label2.TabIndex = 391
        Label2.Text = "txtNomCamion"
        ' 
        ' Label1
        ' 
        Label1.AutoSize = True
        Label1.Location = New Point(139, 148)
        Label1.Name = "Label1"
        Label1.Size = New Size(112, 20)
        Label1.TabIndex = 390
        Label1.Text = "txtCodeCamion"
        ' 
        ' txtObservation
        ' 
        txtObservation.Location = New Point(255, 405)
        txtObservation.Name = "txtObservation"
        txtObservation.Size = New Size(418, 27)
        txtObservation.TabIndex = 389
        ' 
        ' txtNumeroTour
        ' 
        txtNumeroTour.Location = New Point(255, 372)
        txtNumeroTour.Name = "txtNumeroTour"
        txtNumeroTour.Size = New Size(418, 27)
        txtNumeroTour.TabIndex = 388
        ' 
        ' txtQuantiteM3
        ' 
        txtQuantiteM3.Location = New Point(255, 339)
        txtQuantiteM3.Name = "txtQuantiteM3"
        txtQuantiteM3.Size = New Size(418, 27)
        txtQuantiteM3.TabIndex = 387
        ' 
        ' txtHeureRetour
        ' 
        txtHeureRetour.Location = New Point(255, 276)
        txtHeureRetour.Name = "txtHeureRetour"
        txtHeureRetour.Size = New Size(418, 27)
        txtHeureRetour.TabIndex = 386
        ' 
        ' txtHeureDepart
        ' 
        txtHeureDepart.Location = New Point(255, 243)
        txtHeureDepart.Name = "txtHeureDepart"
        txtHeureDepart.Size = New Size(418, 27)
        txtHeureDepart.TabIndex = 385
        ' 
        ' txtChauffeur
        ' 
        txtChauffeur.Location = New Point(255, 210)
        txtChauffeur.Name = "txtChauffeur"
        txtChauffeur.Size = New Size(418, 27)
        txtChauffeur.TabIndex = 384
        ' 
        ' txtNomCamion
        ' 
        txtNomCamion.Location = New Point(255, 177)
        txtNomCamion.Name = "txtNomCamion"
        txtNomCamion.Size = New Size(418, 27)
        txtNomCamion.TabIndex = 383
        ' 
        ' txtCodeCamion
        ' 
        txtCodeCamion.Location = New Point(255, 144)
        txtCodeCamion.Name = "txtCodeCamion"
        txtCodeCamion.Size = New Size(418, 27)
        txtCodeCamion.TabIndex = 382
        ' 
        ' btnActualiser
        ' 
        btnActualiser.Location = New Point(1541, 42)
        btnActualiser.Name = "btnActualiser"
        btnActualiser.Size = New Size(178, 61)
        btnActualiser.TabIndex = 381
        btnActualiser.Text = "Actualiser"
        btnActualiser.UseVisualStyleBackColor = True
        ' 
        ' txtResteTotal
        ' 
        txtResteTotal.Location = New Point(1190, 16)
        txtResteTotal.Name = "txtResteTotal"
        txtResteTotal.Size = New Size(110, 27)
        txtResteTotal.TabIndex = 380
        ' 
        ' lblNombreEnregistrements
        ' 
        lblNombreEnregistrements.Location = New Point(1306, 16)
        lblNombreEnregistrements.Name = "lblNombreEnregistrements"
        lblNombreEnregistrements.Size = New Size(110, 27)
        lblNombreEnregistrements.TabIndex = 379
        ' 
        ' btnJourMoins
        ' 
        btnJourMoins.Location = New Point(1510, 55)
        btnJourMoins.Name = "btnJourMoins"
        btnJourMoins.Size = New Size(25, 34)
        btnJourMoins.TabIndex = 378
        btnJourMoins.Text = "-"
        btnJourMoins.UseVisualStyleBackColor = True
        ' 
        ' btnJourPlus
        ' 
        btnJourPlus.Location = New Point(1259, 53)
        btnJourPlus.Name = "btnJourPlus"
        btnJourPlus.Size = New Size(25, 34)
        btnJourPlus.TabIndex = 377
        btnJourPlus.Text = "+"
        btnJourPlus.UseVisualStyleBackColor = True
        ' 
        ' dtpDatePlanning
        ' 
        dtpDatePlanning.Location = New Point(1290, 60)
        dtpDatePlanning.Name = "dtpDatePlanning"
        dtpDatePlanning.Size = New Size(214, 27)
        dtpDatePlanning.TabIndex = 376
        ' 
        ' DataGridView1
        ' 
        DataGridView1.AllowUserToAddRows = False
        DataGridView1.AllowUserToDeleteRows = False
        DataGridView1.Anchor = AnchorStyles.Top Or AnchorStyles.Bottom Or AnchorStyles.Left Or AnchorStyles.Right
        DataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill
        DataGridView1.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize
        DataGridView1.Location = New Point(773, 109)
        DataGridView1.MultiSelect = False
        DataGridView1.Name = "DataGridView1"
        DataGridView1.ReadOnly = True
        DataGridView1.RowHeadersVisible = False
        DataGridView1.RowHeadersWidth = 51
        DataGridView1.SelectionMode = DataGridViewSelectionMode.FullRowSelect
        DataGridView1.Size = New Size(943, 879)
        DataGridView1.TabIndex = 375
        ' 
        ' btnModifier
        ' 
        btnModifier.Location = New Point(194, 42)
        btnModifier.Name = "btnModifier"
        btnModifier.Size = New Size(178, 61)
        btnModifier.TabIndex = 374
        btnModifier.Text = "EDITE"
        btnModifier.UseVisualStyleBackColor = True
        ' 
        ' btnSupprimer
        ' 
        btnSupprimer.Location = New Point(378, 42)
        btnSupprimer.Name = "btnSupprimer"
        btnSupprimer.Size = New Size(178, 61)
        btnSupprimer.TabIndex = 373
        btnSupprimer.Text = "SUPRIME"
        btnSupprimer.UseVisualStyleBackColor = True
        ' 
        ' btnAjouter
        ' 
        btnAjouter.Location = New Point(10, 42)
        btnAjouter.Name = "btnAjouter"
        btnAjouter.Size = New Size(178, 61)
        btnAjouter.TabIndex = 372
        btnAjouter.Text = "AJOUTER"
        btnAjouter.UseVisualStyleBackColor = True
        ' 
        ' cmbStatut
        ' 
        cmbStatut.FormattingEnabled = True
        cmbStatut.Location = New Point(255, 438)
        cmbStatut.Name = "cmbStatut"
        cmbStatut.Size = New Size(418, 28)
        cmbStatut.TabIndex = 404
        ' 
        ' Label7
        ' 
        Label7.AutoSize = True
        Label7.Location = New Point(144, 441)
        Label7.Name = "Label7"
        Label7.Size = New Size(77, 20)
        Label7.TabIndex = 405
        Label7.Text = "cmbStatut"
        ' 
        ' btnEnCours
        ' 
        btnEnCours.Location = New Point(1077, 28)
        btnEnCours.Name = "btnEnCours"
        btnEnCours.Size = New Size(146, 61)
        btnEnCours.TabIndex = 406
        btnEnCours.Text = "btnEnCours"
        btnEnCours.UseVisualStyleBackColor = True
        ' 
        ' btnTerminer
        ' 
        btnTerminer.Location = New Point(925, 28)
        btnTerminer.Name = "btnTerminer"
        btnTerminer.Size = New Size(146, 61)
        btnTerminer.TabIndex = 407
        btnTerminer.Text = "btnTerminer"
        btnTerminer.UseVisualStyleBackColor = True
        ' 
        ' Planning_camions
        ' 
        AutoScaleDimensions = New SizeF(8F, 20F)
        AutoScaleMode = AutoScaleMode.Font
        ClientSize = New Size(1729, 996)
        Controls.Add(btnTerminer)
        Controls.Add(btnEnCours)
        Controls.Add(Label7)
        Controls.Add(cmbStatut)
        Controls.Add(lblNombre)
        Controls.Add(btnImprimer)
        Controls.Add(btnNouveau)
        Controls.Add(Label19)
        Controls.Add(txtNumeroBL)
        Controls.Add(Label11)
        Controls.Add(Label12)
        Controls.Add(Label4)
        Controls.Add(Label5)
        Controls.Add(Label6)
        Controls.Add(Label3)
        Controls.Add(Label2)
        Controls.Add(Label1)
        Controls.Add(txtObservation)
        Controls.Add(txtNumeroTour)
        Controls.Add(txtQuantiteM3)
        Controls.Add(txtHeureRetour)
        Controls.Add(txtHeureDepart)
        Controls.Add(txtChauffeur)
        Controls.Add(txtNomCamion)
        Controls.Add(txtCodeCamion)
        Controls.Add(btnActualiser)
        Controls.Add(txtResteTotal)
        Controls.Add(lblNombreEnregistrements)
        Controls.Add(btnJourMoins)
        Controls.Add(btnJourPlus)
        Controls.Add(dtpDatePlanning)
        Controls.Add(DataGridView1)
        Controls.Add(btnModifier)
        Controls.Add(btnSupprimer)
        Controls.Add(btnAjouter)
        Controls.Add(Label21)
        Name = "Planning_camions"
        Text = "Planning_camions"
        CType(DataGridView1, ComponentModel.ISupportInitialize).EndInit()
        ResumeLayout(False)
        PerformLayout()
    End Sub

    Friend WithEvents Label21 As Label
    Friend WithEvents lblNombre As Label
    Friend WithEvents btnImprimer As Button
    Friend WithEvents btnNouveau As Button
    Friend WithEvents Label19 As Label
    Friend WithEvents txtNumeroBL As TextBox
    Friend WithEvents Label11 As Label
    Friend WithEvents Label12 As Label
    Friend WithEvents Label4 As Label
    Friend WithEvents Label5 As Label
    Friend WithEvents Label6 As Label
    Friend WithEvents Label3 As Label
    Friend WithEvents Label2 As Label
    Friend WithEvents Label1 As Label
    Friend WithEvents txtObservation As TextBox
    Friend WithEvents txtNumeroTour As TextBox
    Friend WithEvents txtQuantiteM3 As TextBox
    Friend WithEvents txtHeureRetour As TextBox
    Friend WithEvents txtHeureDepart As TextBox
    Friend WithEvents txtChauffeur As TextBox
    Friend WithEvents txtNomCamion As TextBox
    Friend WithEvents txtCodeCamion As TextBox
    Friend WithEvents btnActualiser As Button
    Friend WithEvents txtResteTotal As TextBox
    Friend WithEvents lblNombreEnregistrements As TextBox
    Friend WithEvents btnJourMoins As Button
    Friend WithEvents btnJourPlus As Button
    Friend WithEvents dtpDatePlanning As DateTimePicker
    Friend WithEvents DataGridView1 As DataGridView
    Friend WithEvents btnModifier As Button
    Friend WithEvents btnSupprimer As Button
    Friend WithEvents btnAjouter As Button
    Friend WithEvents cmbStatut As ComboBox
    Friend WithEvents Label7 As Label
    Friend WithEvents btnEnCours As Button
    Friend WithEvents btnTerminer As Button
End Class
