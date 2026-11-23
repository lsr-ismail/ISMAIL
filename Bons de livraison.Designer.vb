<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class Bons_de_livraison
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
        btnActualiser = New Button()
        btnJourMoins = New Button()
        btnJourPlus = New Button()
        dtpDateBL = New DateTimePicker()
        DataGridView1 = New DataGridView()
        btnModifier = New Button()
        btnSupprimer = New Button()
        btnAjouter = New Button()
        Label19 = New Label()
        txtOperateur = New TextBox()
        lblTotalM3 = New Label()
        Label11 = New Label()
        Label12 = New Label()
        Label4 = New Label()
        Label5 = New Label()
        Label6 = New Label()
        Label3 = New Label()
        Label2 = New Label()
        Label1 = New Label()
        txtRECHERCHE = New TextBox()
        txtPlanningSelectionne = New TextBox()
        txtObservation = New TextBox()
        txtCumulM3 = New TextBox()
        txtQuantiteM3 = New TextBox()
        txtVehicule = New TextBox()
        txtNumeroTour = New TextBox()
        txtNumeroBL = New TextBox()
        btnImprimer = New Button()
        btnChoisirPlanning = New Button()
        lblNombre = New Label()
        Label7 = New Label()
        Label10 = New Label()
        CType(DataGridView1, ComponentModel.ISupportInitialize).BeginInit()
        SuspendLayout()
        ' 
        ' Label21
        ' 
        Label21.AutoSize = True
        Label21.Font = New Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point, CByte(0))
        Label21.Location = New Point(12, 9)
        Label21.Name = "Label21"
        Label21.Size = New Size(165, 28)
        Label21.TabIndex = 94
        Label21.Text = "Bons de livraison"
        ' 
        ' btnActualiser
        ' 
        btnActualiser.Location = New Point(855, 45)
        btnActualiser.Name = "btnActualiser"
        btnActualiser.Size = New Size(138, 61)
        btnActualiser.TabIndex = 336
        btnActualiser.Text = "Actualiser"
        btnActualiser.UseVisualStyleBackColor = True
        ' 
        ' btnJourMoins
        ' 
        btnJourMoins.Location = New Point(567, 5)
        btnJourMoins.Name = "btnJourMoins"
        btnJourMoins.Size = New Size(25, 34)
        btnJourMoins.TabIndex = 333
        btnJourMoins.Text = "-"
        btnJourMoins.UseVisualStyleBackColor = True
        ' 
        ' btnJourPlus
        ' 
        btnJourPlus.Location = New Point(316, 3)
        btnJourPlus.Name = "btnJourPlus"
        btnJourPlus.Size = New Size(25, 34)
        btnJourPlus.TabIndex = 332
        btnJourPlus.Text = "+"
        btnJourPlus.UseVisualStyleBackColor = True
        ' 
        ' dtpDateBL
        ' 
        dtpDateBL.Location = New Point(347, 10)
        dtpDateBL.Name = "dtpDateBL"
        dtpDateBL.Size = New Size(214, 27)
        dtpDateBL.TabIndex = 331
        ' 
        ' DataGridView1
        ' 
        DataGridView1.AllowUserToAddRows = False
        DataGridView1.AllowUserToDeleteRows = False
        DataGridView1.Anchor = AnchorStyles.Top Or AnchorStyles.Bottom Or AnchorStyles.Left Or AnchorStyles.Right
        DataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill
        DataGridView1.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize
        DataGridView1.Location = New Point(572, 125)
        DataGridView1.MultiSelect = False
        DataGridView1.Name = "DataGridView1"
        DataGridView1.ReadOnly = True
        DataGridView1.RowHeadersVisible = False
        DataGridView1.RowHeadersWidth = 51
        DataGridView1.SelectionMode = DataGridViewSelectionMode.FullRowSelect
        DataGridView1.Size = New Size(498, 591)
        DataGridView1.TabIndex = 327
        ' 
        ' btnModifier
        ' 
        btnModifier.Location = New Point(212, 43)
        btnModifier.Name = "btnModifier"
        btnModifier.Size = New Size(138, 61)
        btnModifier.TabIndex = 326
        btnModifier.Text = "EDITE"
        btnModifier.UseVisualStyleBackColor = True
        ' 
        ' btnSupprimer
        ' 
        btnSupprimer.Location = New Point(377, 43)
        btnSupprimer.Name = "btnSupprimer"
        btnSupprimer.Size = New Size(138, 61)
        btnSupprimer.TabIndex = 325
        btnSupprimer.Text = "SUPRIME"
        btnSupprimer.UseVisualStyleBackColor = True
        ' 
        ' btnAjouter
        ' 
        btnAjouter.Location = New Point(50, 43)
        btnAjouter.Name = "btnAjouter"
        btnAjouter.Size = New Size(138, 61)
        btnAjouter.TabIndex = 324
        btnAjouter.Text = "AJOUTER"
        btnAjouter.UseVisualStyleBackColor = True
        ' 
        ' Label19
        ' 
        Label19.AutoSize = True
        Label19.Location = New Point(24, 317)
        Label19.Name = "Label19"
        Label19.Size = New Size(76, 20)
        Label19.TabIndex = 367
        Label19.Text = "Opérateur"
        ' 
        ' txtOperateur
        ' 
        txtOperateur.Location = New Point(138, 310)
        txtOperateur.Name = "txtOperateur"
        txtOperateur.Size = New Size(418, 27)
        txtOperateur.TabIndex = 366
        ' 
        ' lblTotalM3
        ' 
        lblTotalM3.AutoSize = True
        lblTotalM3.Location = New Point(347, 125)
        lblTotalM3.Name = "lblTotalM3"
        lblTotalM3.Size = New Size(80, 20)
        lblTotalM3.TabIndex = 358
        lblTotalM3.Text = "lblTotalM3"
        ' 
        ' Label11
        ' 
        Label11.AutoSize = True
        Label11.Location = New Point(22, 413)
        Label11.Name = "Label11"
        Label11.Size = New Size(91, 20)
        Label11.TabIndex = 357
        Label11.Text = "RECHERCHE"
        ' 
        ' Label12
        ' 
        Label12.AutoSize = True
        Label12.Location = New Point(24, 370)
        Label12.Name = "Label12"
        Label12.Size = New Size(86, 40)
        Label12.TabIndex = 356
        Label12.Text = "Planning" & vbCrLf & "Sélectionné"
        ' 
        ' Label4
        ' 
        Label4.AutoSize = True
        Label4.Location = New Point(22, 347)
        Label4.Name = "Label4"
        Label4.Size = New Size(89, 20)
        Label4.TabIndex = 355
        Label4.Text = "Observation"
        ' 
        ' Label5
        ' 
        Label5.AutoSize = True
        Label5.Location = New Point(22, 284)
        Label5.Name = "Label5"
        Label5.Size = New Size(76, 20)
        Label5.TabIndex = 354
        Label5.Text = "Cumul M3"
        ' 
        ' Label6
        ' 
        Label6.AutoSize = True
        Label6.Location = New Point(19, 251)
        Label6.Name = "Label6"
        Label6.Size = New Size(91, 20)
        Label6.TabIndex = 353
        Label6.Text = "Quantité M3"
        ' 
        ' Label3
        ' 
        Label3.AutoSize = True
        Label3.Location = New Point(22, 218)
        Label3.Name = "Label3"
        Label3.Size = New Size(64, 20)
        Label3.TabIndex = 352
        Label3.Text = "Véhicule"
        ' 
        ' Label2
        ' 
        Label2.AutoSize = True
        Label2.Location = New Point(22, 185)
        Label2.Name = "Label2"
        Label2.Size = New Size(96, 20)
        Label2.TabIndex = 351
        Label2.Text = "Numéro Tour"
        ' 
        ' Label1
        ' 
        Label1.AutoSize = True
        Label1.Location = New Point(22, 152)
        Label1.Name = "Label1"
        Label1.Size = New Size(83, 20)
        Label1.TabIndex = 350
        Label1.Text = "Numéro BL"
        ' 
        ' txtRECHERCHE
        ' 
        txtRECHERCHE.Location = New Point(138, 409)
        txtRECHERCHE.Name = "txtRECHERCHE"
        txtRECHERCHE.Size = New Size(418, 27)
        txtRECHERCHE.TabIndex = 344
        ' 
        ' txtPlanningSelectionne
        ' 
        txtPlanningSelectionne.Location = New Point(138, 376)
        txtPlanningSelectionne.Name = "txtPlanningSelectionne"
        txtPlanningSelectionne.Size = New Size(418, 27)
        txtPlanningSelectionne.TabIndex = 343
        ' 
        ' txtObservation
        ' 
        txtObservation.Location = New Point(138, 343)
        txtObservation.Name = "txtObservation"
        txtObservation.Size = New Size(418, 27)
        txtObservation.TabIndex = 342
        ' 
        ' txtCumulM3
        ' 
        txtCumulM3.Location = New Point(138, 280)
        txtCumulM3.Name = "txtCumulM3"
        txtCumulM3.Size = New Size(418, 27)
        txtCumulM3.TabIndex = 341
        ' 
        ' txtQuantiteM3
        ' 
        txtQuantiteM3.Location = New Point(138, 247)
        txtQuantiteM3.Name = "txtQuantiteM3"
        txtQuantiteM3.Size = New Size(418, 27)
        txtQuantiteM3.TabIndex = 340
        ' 
        ' txtVehicule
        ' 
        txtVehicule.Location = New Point(138, 214)
        txtVehicule.Name = "txtVehicule"
        txtVehicule.Size = New Size(418, 27)
        txtVehicule.TabIndex = 339
        ' 
        ' txtNumeroTour
        ' 
        txtNumeroTour.Location = New Point(138, 181)
        txtNumeroTour.Name = "txtNumeroTour"
        txtNumeroTour.Size = New Size(418, 27)
        txtNumeroTour.TabIndex = 338
        ' 
        ' txtNumeroBL
        ' 
        txtNumeroBL.Location = New Point(138, 148)
        txtNumeroBL.Name = "txtNumeroBL"
        txtNumeroBL.Size = New Size(418, 27)
        txtNumeroBL.TabIndex = 337
        ' 
        ' btnImprimer
        ' 
        btnImprimer.Location = New Point(538, 45)
        btnImprimer.Name = "btnImprimer"
        btnImprimer.Size = New Size(138, 61)
        btnImprimer.TabIndex = 369
        btnImprimer.Text = "Imprimer"
        btnImprimer.UseVisualStyleBackColor = True
        ' 
        ' btnChoisirPlanning
        ' 
        btnChoisirPlanning.Location = New Point(695, 45)
        btnChoisirPlanning.Name = "btnChoisirPlanning"
        btnChoisirPlanning.Size = New Size(138, 61)
        btnChoisirPlanning.TabIndex = 368
        btnChoisirPlanning.Text = "Choisir Planning"
        btnChoisirPlanning.UseVisualStyleBackColor = True
        ' 
        ' lblNombre
        ' 
        lblNombre.AutoSize = True
        lblNombre.Location = New Point(167, 125)
        lblNombre.Name = "lblNombre"
        lblNombre.Size = New Size(81, 20)
        lblNombre.TabIndex = 370
        lblNombre.Text = "lblNombre"
        ' 
        ' Label7
        ' 
        Label7.AutoSize = True
        Label7.Location = New Point(433, 125)
        Label7.Name = "Label7"
        Label7.Size = New Size(30, 20)
        Label7.TabIndex = 371
        Label7.Text = "M3"
        ' 
        ' Label10
        ' 
        Label10.AutoSize = True
        Label10.Location = New Point(138, 125)
        Label10.Name = "Label10"
        Label10.Size = New Size(23, 20)
        Label10.TabIndex = 372
        Label10.Text = "N:"
        ' 
        ' Bons_de_livraison
        ' 
        AutoScaleDimensions = New SizeF(8F, 20F)
        AutoScaleMode = AutoScaleMode.Font
        ClientSize = New Size(1082, 723)
        Controls.Add(Label10)
        Controls.Add(Label7)
        Controls.Add(lblNombre)
        Controls.Add(btnImprimer)
        Controls.Add(btnChoisirPlanning)
        Controls.Add(Label19)
        Controls.Add(txtOperateur)
        Controls.Add(lblTotalM3)
        Controls.Add(Label11)
        Controls.Add(Label12)
        Controls.Add(Label4)
        Controls.Add(Label5)
        Controls.Add(Label6)
        Controls.Add(Label3)
        Controls.Add(Label2)
        Controls.Add(Label1)
        Controls.Add(txtRECHERCHE)
        Controls.Add(txtPlanningSelectionne)
        Controls.Add(txtObservation)
        Controls.Add(txtCumulM3)
        Controls.Add(txtQuantiteM3)
        Controls.Add(txtVehicule)
        Controls.Add(txtNumeroTour)
        Controls.Add(txtNumeroBL)
        Controls.Add(btnActualiser)
        Controls.Add(btnJourMoins)
        Controls.Add(btnJourPlus)
        Controls.Add(dtpDateBL)
        Controls.Add(DataGridView1)
        Controls.Add(btnModifier)
        Controls.Add(btnSupprimer)
        Controls.Add(btnAjouter)
        Controls.Add(Label21)
        Name = "Bons_de_livraison"
        Text = "Bons de livraison"
        CType(DataGridView1, ComponentModel.ISupportInitialize).EndInit()
        ResumeLayout(False)
        PerformLayout()
    End Sub

    Friend WithEvents Label21 As Label
    Friend WithEvents btnActualiser As Button
    Friend WithEvents btnJourMoins As Button
    Friend WithEvents btnJourPlus As Button
    Friend WithEvents dtpDateBL As DateTimePicker
    Friend WithEvents Exécute As Button
    Friend WithEvents Duplique As Button
    Friend WithEvents Imprimer As Button
    Friend WithEvents DataGridView1 As DataGridView
    Friend WithEvents btnModifier As Button
    Friend WithEvents btnSupprimer As Button
    Friend WithEvents btnAjouter As Button
    Friend WithEvents Label20 As Label
    Friend WithEvents txtTypeChantier As TextBox
    Friend WithEvents Label19 As Label
    Friend WithEvents txtOperateur As TextBox
    Friend WithEvents Label17 As Label
    Friend WithEvents txtCreationChantierBL As TextBox
    Friend WithEvents txtImpressionPeses As TextBox
    Friend WithEvents Label18 As Label
    Friend WithEvents lblNombre As Label
    Friend WithEvents Label8 As Label
    Friend WithEvents Label9 As Label
    Friend WithEvents lblTotalM3 As Label
    Friend WithEvents Label11 As Label
    Friend WithEvents Label12 As Label
    Friend WithEvents Label4 As Label
    Friend WithEvents Label5 As Label
    Friend WithEvents Label6 As Label
    Friend WithEvents Label3 As Label
    Friend WithEvents Label2 As Label
    Friend WithEvents Label1 As Label
    Friend WithEvents txtMarcheWattmetre As TextBox
    Friend WithEvents txtNatureChantier As TextBox
    Friend WithEvents txtPlusValue As TextBox
    Friend WithEvents txtZoneTransport As TextBox
    Friend WithEvents txtZoneFacturation As TextBox
    Friend WithEvents txtRECHERCHE As TextBox
    Friend WithEvents txtPlanningSelectionne As TextBox
    Friend WithEvents txtObservation As TextBox
    Friend WithEvents txtCumulM3 As TextBox
    Friend WithEvents txtQuantiteM3 As TextBox
    Friend WithEvents txtVehicule As TextBox
    Friend WithEvents txtNumeroTour As TextBox
    Friend WithEvents txtNumeroBL As TextBox
    Friend WithEvents btnImprimer As Button
    Friend WithEvents btnChoisirPlanning As Button
    Friend WithEvents Label7 As Label
    Friend WithEvents Label10 As Label
End Class
