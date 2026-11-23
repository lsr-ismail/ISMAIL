<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class Chauffeurs
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(Chauffeurs))
        btnActualiser = New Button()
        btnNouveau = New Button()
        Label9 = New Label()
        Label10 = New Label()
        Label11 = New Label()
        Label12 = New Label()
        Label4 = New Label()
        Label5 = New Label()
        Label6 = New Label()
        Label3 = New Label()
        Label2 = New Label()
        Label1 = New Label()
        Imprimer = New Button()
        txtRemarques = New TextBox()
        txtEmail = New TextBox()
        txtTelephone = New TextBox()
        txtCodePostal = New TextBox()
        txtAdresse = New TextBox()
        txtNomChauffeur = New TextBox()
        txtCodeChauffeur = New TextBox()
        DataGridView1 = New DataGridView()
        lblNombre = New TextBox()
        btnModifier = New Button()
        RECHERCHE = New Button()
        btnSupprimer = New Button()
        btnAjouter = New Button()
        chkActif = New CheckBox()
        PrintDocument1 = New Printing.PrintDocument()
        PrintPreviewDialog1 = New PrintPreviewDialog()
        txtRECHERCHE = New TextBox()
        dtpDerniereModification = New TextBox()
        txtidChauffeur = New TextBox()
        CType(DataGridView1, ComponentModel.ISupportInitialize).BeginInit()
        SuspendLayout()
        ' 
        ' btnActualiser
        ' 
        btnActualiser.Location = New Point(857, 21)
        btnActualiser.Name = "btnActualiser"
        btnActualiser.Size = New Size(132, 61)
        btnActualiser.TabIndex = 141
        btnActualiser.Text = "Actualiser"
        btnActualiser.UseVisualStyleBackColor = True
        ' 
        ' btnNouveau
        ' 
        btnNouveau.Location = New Point(719, 21)
        btnNouveau.Name = "btnNouveau"
        btnNouveau.Size = New Size(132, 61)
        btnNouveau.TabIndex = 140
        btnNouveau.Text = "Nouveau"
        btnNouveau.UseVisualStyleBackColor = True
        ' 
        ' Label9
        ' 
        Label9.AutoSize = True
        Label9.Location = New Point(28, 457)
        Label9.Name = "Label9"
        Label9.Size = New Size(155, 20)
        Label9.TabIndex = 138
        Label9.Text = "Dernière Modification"
        ' 
        ' Label10
        ' 
        Label10.AutoSize = True
        Label10.Location = New Point(31, 424)
        Label10.Name = "Label10"
        Label10.Size = New Size(40, 20)
        Label10.TabIndex = 137
        Label10.Text = "Actif"
        ' 
        ' Label11
        ' 
        Label11.AutoSize = True
        Label11.Location = New Point(29, 391)
        Label11.Name = "Label11"
        Label11.Size = New Size(83, 20)
        Label11.TabIndex = 136
        Label11.Text = "Remarques"
        ' 
        ' Label12
        ' 
        Label12.AutoSize = True
        Label12.Location = New Point(31, 358)
        Label12.Name = "Label12"
        Label12.Size = New Size(46, 20)
        Label12.TabIndex = 135
        Label12.Text = "Email"
        ' 
        ' Label4
        ' 
        Label4.AutoSize = True
        Label4.Location = New Point(29, 325)
        Label4.Name = "Label4"
        Label4.Size = New Size(78, 20)
        Label4.TabIndex = 134
        Label4.Text = "Téléphone"
        ' 
        ' Label5
        ' 
        Label5.AutoSize = True
        Label5.Location = New Point(29, 292)
        Label5.Name = "Label5"
        Label5.Size = New Size(87, 20)
        Label5.TabIndex = 133
        Label5.Text = "Code Postal"
        ' 
        ' Label6
        ' 
        Label6.AutoSize = True
        Label6.Location = New Point(26, 259)
        Label6.Name = "Label6"
        Label6.Size = New Size(61, 20)
        Label6.TabIndex = 132
        Label6.Text = "Adresse"
        ' 
        ' Label3
        ' 
        Label3.AutoSize = True
        Label3.Location = New Point(29, 226)
        Label3.Name = "Label3"
        Label3.Size = New Size(110, 20)
        Label3.TabIndex = 131
        Label3.Text = "Nom Chauffeur"
        ' 
        ' Label2
        ' 
        Label2.AutoSize = True
        Label2.Location = New Point(29, 193)
        Label2.Name = "Label2"
        Label2.Size = New Size(112, 20)
        Label2.TabIndex = 130
        Label2.Text = "Code Chauffeur"
        ' 
        ' Label1
        ' 
        Label1.AutoSize = True
        Label1.Location = New Point(29, 160)
        Label1.Name = "Label1"
        Label1.Size = New Size(88, 20)
        Label1.TabIndex = 129
        Label1.Text = "Id chauffeur"
        ' 
        ' Imprimer
        ' 
        Imprimer.Location = New Point(581, 21)
        Imprimer.Name = "Imprimer"
        Imprimer.Size = New Size(132, 61)
        Imprimer.TabIndex = 128
        Imprimer.Text = "Imprimer"
        Imprimer.UseVisualStyleBackColor = True
        ' 
        ' txtRemarques
        ' 
        txtRemarques.Location = New Point(187, 387)
        txtRemarques.Name = "txtRemarques"
        txtRemarques.Size = New Size(341, 27)
        txtRemarques.TabIndex = 124
        ' 
        ' txtEmail
        ' 
        txtEmail.Location = New Point(187, 354)
        txtEmail.Name = "txtEmail"
        txtEmail.Size = New Size(341, 27)
        txtEmail.TabIndex = 123
        ' 
        ' txtTelephone
        ' 
        txtTelephone.Location = New Point(187, 321)
        txtTelephone.Name = "txtTelephone"
        txtTelephone.Size = New Size(341, 27)
        txtTelephone.TabIndex = 122
        ' 
        ' txtCodePostal
        ' 
        txtCodePostal.Location = New Point(187, 288)
        txtCodePostal.Name = "txtCodePostal"
        txtCodePostal.Size = New Size(341, 27)
        txtCodePostal.TabIndex = 121
        ' 
        ' txtAdresse
        ' 
        txtAdresse.Location = New Point(187, 255)
        txtAdresse.Name = "txtAdresse"
        txtAdresse.Size = New Size(341, 27)
        txtAdresse.TabIndex = 120
        ' 
        ' txtNomChauffeur
        ' 
        txtNomChauffeur.Location = New Point(187, 222)
        txtNomChauffeur.Name = "txtNomChauffeur"
        txtNomChauffeur.Size = New Size(341, 27)
        txtNomChauffeur.TabIndex = 119
        ' 
        ' txtCodeChauffeur
        ' 
        txtCodeChauffeur.Location = New Point(189, 191)
        txtCodeChauffeur.Name = "txtCodeChauffeur"
        txtCodeChauffeur.Size = New Size(341, 27)
        txtCodeChauffeur.TabIndex = 118
        ' 
        ' DataGridView1
        ' 
        DataGridView1.AllowUserToAddRows = False
        DataGridView1.Anchor = AnchorStyles.Top Or AnchorStyles.Bottom Or AnchorStyles.Left Or AnchorStyles.Right
        DataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill
        DataGridView1.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize
        DataGridView1.Location = New Point(534, 105)
        DataGridView1.MultiSelect = False
        DataGridView1.Name = "DataGridView1"
        DataGridView1.ReadOnly = True
        DataGridView1.RowHeadersWidth = 51
        DataGridView1.SelectionMode = DataGridViewSelectionMode.FullRowSelect
        DataGridView1.Size = New Size(527, 496)
        DataGridView1.TabIndex = 117
        ' 
        ' lblNombre
        ' 
        lblNombre.Location = New Point(272, 90)
        lblNombre.Name = "lblNombre"
        lblNombre.ReadOnly = True
        lblNombre.Size = New Size(165, 27)
        lblNombre.TabIndex = 116
        lblNombre.TextAlign = HorizontalAlignment.Center
        ' 
        ' btnModifier
        ' 
        btnModifier.Location = New Point(443, 21)
        btnModifier.Name = "btnModifier"
        btnModifier.Size = New Size(132, 61)
        btnModifier.TabIndex = 115
        btnModifier.Text = "EDITE"
        btnModifier.UseVisualStyleBackColor = True
        ' 
        ' RECHERCHE
        ' 
        RECHERCHE.Location = New Point(305, 21)
        RECHERCHE.Name = "RECHERCHE"
        RECHERCHE.Size = New Size(132, 61)
        RECHERCHE.TabIndex = 114
        RECHERCHE.Text = "RECHERCHE"
        RECHERCHE.UseVisualStyleBackColor = True
        ' 
        ' btnSupprimer
        ' 
        btnSupprimer.Location = New Point(167, 21)
        btnSupprimer.Name = "btnSupprimer"
        btnSupprimer.Size = New Size(132, 61)
        btnSupprimer.TabIndex = 113
        btnSupprimer.Text = "SUPRIME"
        btnSupprimer.UseVisualStyleBackColor = True
        ' 
        ' btnAjouter
        ' 
        btnAjouter.Location = New Point(29, 21)
        btnAjouter.Name = "btnAjouter"
        btnAjouter.Size = New Size(132, 61)
        btnAjouter.TabIndex = 112
        btnAjouter.Text = "AJOUTER"
        btnAjouter.UseVisualStyleBackColor = True
        ' 
        ' chkActif
        ' 
        chkActif.AutoSize = True
        chkActif.Location = New Point(361, 420)
        chkActif.Name = "chkActif"
        chkActif.Size = New Size(35, 24)
        chkActif.TabIndex = 143
        chkActif.Text = " "
        chkActif.UseVisualStyleBackColor = True
        ' 
        ' PrintDocument1
        ' 
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
        txtRECHERCHE.Location = New Point(189, 123)
        txtRECHERCHE.Name = "txtRECHERCHE"
        txtRECHERCHE.Size = New Size(341, 27)
        txtRECHERCHE.TabIndex = 142
        ' 
        ' dtpDerniereModification
        ' 
        dtpDerniereModification.Location = New Point(187, 453)
        dtpDerniereModification.Name = "dtpDerniereModification"
        dtpDerniereModification.Size = New Size(341, 27)
        dtpDerniereModification.TabIndex = 126
        ' 
        ' txtidChauffeur
        ' 
        txtidChauffeur.Location = New Point(189, 158)
        txtidChauffeur.Name = "txtidChauffeur"
        txtidChauffeur.ReadOnly = True
        txtidChauffeur.Size = New Size(341, 27)
        txtidChauffeur.TabIndex = 144
        ' 
        ' Chauffeurs
        ' 
        AutoScaleDimensions = New SizeF(8F, 20F)
        AutoScaleMode = AutoScaleMode.Font
        ClientSize = New Size(1082, 723)
        Controls.Add(txtidChauffeur)
        Controls.Add(chkActif)
        Controls.Add(txtRECHERCHE)
        Controls.Add(btnActualiser)
        Controls.Add(btnNouveau)
        Controls.Add(Label9)
        Controls.Add(Label10)
        Controls.Add(Label11)
        Controls.Add(Label12)
        Controls.Add(Label4)
        Controls.Add(Label5)
        Controls.Add(Label6)
        Controls.Add(Label3)
        Controls.Add(Label2)
        Controls.Add(Label1)
        Controls.Add(Imprimer)
        Controls.Add(dtpDerniereModification)
        Controls.Add(txtRemarques)
        Controls.Add(txtEmail)
        Controls.Add(txtTelephone)
        Controls.Add(txtCodePostal)
        Controls.Add(txtAdresse)
        Controls.Add(txtNomChauffeur)
        Controls.Add(txtCodeChauffeur)
        Controls.Add(DataGridView1)
        Controls.Add(lblNombre)
        Controls.Add(btnModifier)
        Controls.Add(RECHERCHE)
        Controls.Add(btnSupprimer)
        Controls.Add(btnAjouter)
        Name = "Chauffeurs"
        Text = "Chauffeurs"
        CType(DataGridView1, ComponentModel.ISupportInitialize).EndInit()
        ResumeLayout(False)
        PerformLayout()
    End Sub
    Friend WithEvents btnActualiser As Button
    Friend WithEvents btnNouveau As Button
    Friend WithEvents Label9 As Label
    Friend WithEvents Label10 As Label
    Friend WithEvents Label11 As Label
    Friend WithEvents Label12 As Label
    Friend WithEvents Label4 As Label
    Friend WithEvents Label5 As Label
    Friend WithEvents Label6 As Label
    Friend WithEvents Label3 As Label
    Friend WithEvents Label2 As Label
    Friend WithEvents Label1 As Label
    Friend WithEvents Imprimer As Button
    Friend WithEvents txtRemarques As TextBox
    Friend WithEvents txtEmail As TextBox
    Friend WithEvents txtTelephone As TextBox
    Friend WithEvents txtCodePostal As TextBox
    Friend WithEvents txtAdresse As TextBox
    Friend WithEvents txtNomChauffeur As TextBox
    Friend WithEvents txtCodeChauffeur As TextBox
    Friend WithEvents DataGridView1 As DataGridView
    Friend WithEvents lblNombre As TextBox
    Friend WithEvents btnModifier As Button
    Friend WithEvents RECHERCHE As Button
    Friend WithEvents btnSupprimer As Button
    Friend WithEvents btnAjouter As Button
    Friend WithEvents chkActif As CheckBox
    Friend WithEvents PrintDocument1 As Printing.PrintDocument
    Friend WithEvents PrintPreviewDialog1 As PrintPreviewDialog
    Friend WithEvents txtRECHERCHE As TextBox
    Friend WithEvents dtpDerniereModification As TextBox
    Friend WithEvents txtidChauffeur As TextBox
End Class
