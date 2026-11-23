<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class camions
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(camions))
        Label7 = New Label()
        dtpDerniereModification = New TextBox()
        txtRECHERCHE = New TextBox()
        btnActualiser = New Button()
        btnNouveau = New Button()
        Label8 = New Label()
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
        txtTelephone = New TextBox()
        txtCodeRadio = New TextBox()
        txtChargeUtile = New TextBox()
        txtCapacite = New TextBox()
        txtChauffeur = New TextBox()
        txtEquipement = New TextBox()
        txtFamille = New TextBox()
        txtFournisseur = New TextBox()
        txtNomCamion = New TextBox()
        txtCleCamion = New TextBox()
        DataGridView1 = New DataGridView()
        txtCodeCamion = New TextBox()
        btnModifier = New Button()
        RECHERCHE = New Button()
        btnSupprimer = New Button()
        btnAjouter = New Button()
        PrintDocument1 = New Printing.PrintDocument()
        PrintPreviewDialog1 = New PrintPreviewDialog()
        Label21 = New Label()
        Label13 = New Label()
        txtMatricule = New TextBox()
        CType(DataGridView1, ComponentModel.ISupportInitialize).BeginInit()
        SuspendLayout()
        ' 
        ' Label7
        ' 
        Label7.AutoSize = True
        Label7.Location = New Point(33, 540)
        Label7.Name = "Label7"
        Label7.Size = New Size(155, 20)
        Label7.TabIndex = 144
        Label7.Text = "Dernière Modification"
        ' 
        ' dtpDerniereModification
        ' 
        dtpDerniereModification.Location = New Point(194, 536)
        dtpDerniereModification.Name = "dtpDerniereModification"
        dtpDerniereModification.Size = New Size(355, 27)
        dtpDerniereModification.TabIndex = 143
        ' 
        ' txtRECHERCHE
        ' 
        txtRECHERCHE.Location = New Point(192, 107)
        txtRECHERCHE.Name = "txtRECHERCHE"
        txtRECHERCHE.Size = New Size(355, 27)
        txtRECHERCHE.TabIndex = 142
        ' 
        ' btnActualiser
        ' 
        btnActualiser.Location = New Point(768, 39)
        btnActualiser.Name = "btnActualiser"
        btnActualiser.Size = New Size(142, 61)
        btnActualiser.TabIndex = 141
        btnActualiser.Text = "Actualiser"
        btnActualiser.UseVisualStyleBackColor = True
        ' 
        ' btnNouveau
        ' 
        btnNouveau.Location = New Point(916, 39)
        btnNouveau.Name = "btnNouveau"
        btnNouveau.Size = New Size(142, 61)
        btnNouveau.TabIndex = 140
        btnNouveau.Text = "Nouveau"
        btnNouveau.UseVisualStyleBackColor = True
        ' 
        ' Label8
        ' 
        Label8.AutoSize = True
        Label8.Location = New Point(31, 507)
        Label8.Name = "Label8"
        Label8.Size = New Size(78, 20)
        Label8.TabIndex = 139
        Label8.Text = "Téléphone"
        ' 
        ' Label9
        ' 
        Label9.AutoSize = True
        Label9.Location = New Point(33, 474)
        Label9.Name = "Label9"
        Label9.Size = New Size(83, 20)
        Label9.TabIndex = 138
        Label9.Text = "CodeRadio"
        ' 
        ' Label10
        ' 
        Label10.AutoSize = True
        Label10.Location = New Point(36, 441)
        Label10.Name = "Label10"
        Label10.Size = New Size(87, 20)
        Label10.TabIndex = 137
        Label10.Text = "ChargeUtile"
        ' 
        ' Label11
        ' 
        Label11.AutoSize = True
        Label11.Location = New Point(34, 408)
        Label11.Name = "Label11"
        Label11.Size = New Size(67, 20)
        Label11.TabIndex = 136
        Label11.Text = "Capacité"
        ' 
        ' Label12
        ' 
        Label12.AutoSize = True
        Label12.Location = New Point(36, 375)
        Label12.Name = "Label12"
        Label12.Size = New Size(73, 20)
        Label12.TabIndex = 135
        Label12.Text = "Chauffeur"
        ' 
        ' Label4
        ' 
        Label4.AutoSize = True
        Label4.Location = New Point(34, 342)
        Label4.Name = "Label4"
        Label4.Size = New Size(89, 20)
        Label4.TabIndex = 134
        Label4.Text = "Équipement"
        ' 
        ' Label5
        ' 
        Label5.AutoSize = True
        Label5.Location = New Point(34, 309)
        Label5.Name = "Label5"
        Label5.Size = New Size(56, 20)
        Label5.TabIndex = 133
        Label5.Text = "Famille"
        ' 
        ' Label6
        ' 
        Label6.AutoSize = True
        Label6.Location = New Point(31, 276)
        Label6.Name = "Label6"
        Label6.Size = New Size(83, 20)
        Label6.TabIndex = 132
        Label6.Text = "Fournisseur"
        ' 
        ' Label3
        ' 
        Label3.AutoSize = True
        Label3.Location = New Point(34, 210)
        Label3.Name = "Label3"
        Label3.Size = New Size(102, 20)
        Label3.TabIndex = 131
        Label3.Text = "Nom Camionr"
        ' 
        ' Label2
        ' 
        Label2.AutoSize = True
        Label2.Location = New Point(34, 177)
        Label2.Name = "Label2"
        Label2.Size = New Size(85, 20)
        Label2.TabIndex = 130
        Label2.Text = "Cle Camion"
        ' 
        ' Label1
        ' 
        Label1.AutoSize = True
        Label1.Location = New Point(34, 144)
        Label1.Name = "Label1"
        Label1.Size = New Size(99, 20)
        Label1.TabIndex = 129
        Label1.Text = "Code Camion"
        ' 
        ' Imprimer
        ' 
        Imprimer.Location = New Point(472, 40)
        Imprimer.Name = "Imprimer"
        Imprimer.Size = New Size(142, 61)
        Imprimer.TabIndex = 128
        Imprimer.Text = "Imprimer"
        Imprimer.UseVisualStyleBackColor = True
        ' 
        ' txtTelephone
        ' 
        txtTelephone.Location = New Point(192, 503)
        txtTelephone.Name = "txtTelephone"
        txtTelephone.Size = New Size(355, 27)
        txtTelephone.TabIndex = 127
        ' 
        ' txtCodeRadio
        ' 
        txtCodeRadio.Location = New Point(192, 470)
        txtCodeRadio.Name = "txtCodeRadio"
        txtCodeRadio.Size = New Size(355, 27)
        txtCodeRadio.TabIndex = 126
        ' 
        ' txtChargeUtile
        ' 
        txtChargeUtile.Location = New Point(192, 437)
        txtChargeUtile.Name = "txtChargeUtile"
        txtChargeUtile.Size = New Size(355, 27)
        txtChargeUtile.TabIndex = 125
        ' 
        ' txtCapacite
        ' 
        txtCapacite.Location = New Point(192, 404)
        txtCapacite.Name = "txtCapacite"
        txtCapacite.Size = New Size(355, 27)
        txtCapacite.TabIndex = 124
        ' 
        ' txtChauffeur
        ' 
        txtChauffeur.Location = New Point(192, 371)
        txtChauffeur.Name = "txtChauffeur"
        txtChauffeur.Size = New Size(355, 27)
        txtChauffeur.TabIndex = 123
        ' 
        ' txtEquipement
        ' 
        txtEquipement.Location = New Point(192, 338)
        txtEquipement.Name = "txtEquipement"
        txtEquipement.Size = New Size(355, 27)
        txtEquipement.TabIndex = 122
        ' 
        ' txtFamille
        ' 
        txtFamille.Location = New Point(192, 305)
        txtFamille.Name = "txtFamille"
        txtFamille.Size = New Size(355, 27)
        txtFamille.TabIndex = 121
        ' 
        ' txtFournisseur
        ' 
        txtFournisseur.Location = New Point(192, 272)
        txtFournisseur.Name = "txtFournisseur"
        txtFournisseur.Size = New Size(355, 27)
        txtFournisseur.TabIndex = 120
        ' 
        ' txtNomCamion
        ' 
        txtNomCamion.Location = New Point(192, 206)
        txtNomCamion.Name = "txtNomCamion"
        txtNomCamion.Size = New Size(355, 27)
        txtNomCamion.TabIndex = 119
        ' 
        ' txtCleCamion
        ' 
        txtCleCamion.Location = New Point(194, 175)
        txtCleCamion.Name = "txtCleCamion"
        txtCleCamion.Size = New Size(355, 27)
        txtCleCamion.TabIndex = 118
        ' 
        ' DataGridView1
        ' 
        DataGridView1.AllowUserToAddRows = False
        DataGridView1.Anchor = AnchorStyles.Top Or AnchorStyles.Bottom Or AnchorStyles.Left Or AnchorStyles.Right
        DataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill
        DataGridView1.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize
        DataGridView1.Location = New Point(553, 119)
        DataGridView1.MultiSelect = False
        DataGridView1.Name = "DataGridView1"
        DataGridView1.ReadOnly = True
        DataGridView1.RowHeadersVisible = False
        DataGridView1.RowHeadersWidth = 51
        DataGridView1.SelectionMode = DataGridViewSelectionMode.FullRowSelect
        DataGridView1.Size = New Size(517, 428)
        DataGridView1.TabIndex = 117
        ' 
        ' txtCodeCamion
        ' 
        txtCodeCamion.Location = New Point(192, 140)
        txtCodeCamion.Name = "txtCodeCamion"
        txtCodeCamion.Size = New Size(355, 27)
        txtCodeCamion.TabIndex = 116
        ' 
        ' btnModifier
        ' 
        btnModifier.Location = New Point(620, 39)
        btnModifier.Name = "btnModifier"
        btnModifier.Size = New Size(142, 61)
        btnModifier.TabIndex = 115
        btnModifier.Text = "EDITE"
        btnModifier.UseVisualStyleBackColor = True
        ' 
        ' RECHERCHE
        ' 
        RECHERCHE.Location = New Point(323, 39)
        RECHERCHE.Name = "RECHERCHE"
        RECHERCHE.Size = New Size(142, 61)
        RECHERCHE.TabIndex = 114
        RECHERCHE.Text = "RECHERCHE"
        RECHERCHE.UseVisualStyleBackColor = True
        ' 
        ' btnSupprimer
        ' 
        btnSupprimer.Location = New Point(175, 39)
        btnSupprimer.Name = "btnSupprimer"
        btnSupprimer.Size = New Size(142, 61)
        btnSupprimer.TabIndex = 113
        btnSupprimer.Text = "SUPRIME"
        btnSupprimer.UseVisualStyleBackColor = True
        ' 
        ' btnAjouter
        ' 
        btnAjouter.Location = New Point(27, 40)
        btnAjouter.Name = "btnAjouter"
        btnAjouter.Size = New Size(142, 61)
        btnAjouter.TabIndex = 112
        btnAjouter.Text = "AJOUTER"
        btnAjouter.UseVisualStyleBackColor = True
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
        ' Label21
        ' 
        Label21.AutoSize = True
        Label21.Font = New Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point, CByte(0))
        Label21.Location = New Point(47, 9)
        Label21.Name = "Label21"
        Label21.Size = New Size(87, 28)
        Label21.TabIndex = 145
        Label21.Text = "Camions"
        ' 
        ' Label13
        ' 
        Label13.AutoSize = True
        Label13.Location = New Point(31, 243)
        Label13.Name = "Label13"
        Label13.Size = New Size(71, 20)
        Label13.TabIndex = 147
        Label13.Text = "Matricule"
        ' 
        ' txtMatricule
        ' 
        txtMatricule.Location = New Point(192, 239)
        txtMatricule.Name = "txtMatricule"
        txtMatricule.Size = New Size(355, 27)
        txtMatricule.TabIndex = 146
        ' 
        ' camions
        ' 
        AutoScaleDimensions = New SizeF(8F, 20F)
        AutoScaleMode = AutoScaleMode.Font
        ClientSize = New Size(1082, 723)
        Controls.Add(Label13)
        Controls.Add(txtMatricule)
        Controls.Add(Label21)
        Controls.Add(Label7)
        Controls.Add(dtpDerniereModification)
        Controls.Add(txtRECHERCHE)
        Controls.Add(btnActualiser)
        Controls.Add(btnNouveau)
        Controls.Add(Label8)
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
        Controls.Add(txtTelephone)
        Controls.Add(txtCodeRadio)
        Controls.Add(txtChargeUtile)
        Controls.Add(txtCapacite)
        Controls.Add(txtChauffeur)
        Controls.Add(txtEquipement)
        Controls.Add(txtFamille)
        Controls.Add(txtFournisseur)
        Controls.Add(txtNomCamion)
        Controls.Add(txtCleCamion)
        Controls.Add(DataGridView1)
        Controls.Add(txtCodeCamion)
        Controls.Add(btnModifier)
        Controls.Add(RECHERCHE)
        Controls.Add(btnSupprimer)
        Controls.Add(btnAjouter)
        Name = "camions"
        Text = "camions"
        CType(DataGridView1, ComponentModel.ISupportInitialize).EndInit()
        ResumeLayout(False)
        PerformLayout()
    End Sub
    Friend WithEvents Label7 As Label
    Friend WithEvents dtpDerniereModification As TextBox
    Friend WithEvents txtRECHERCHE As TextBox
    Friend WithEvents btnActualiser As Button
    Friend WithEvents btnNouveau As Button
    Friend WithEvents Label8 As Label
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
    Friend WithEvents txtTelephone As TextBox
    Friend WithEvents txtCodeRadio As TextBox
    Friend WithEvents txtChargeUtile As TextBox
    Friend WithEvents txtCapacite As TextBox
    Friend WithEvents txtChauffeur As TextBox
    Friend WithEvents txtEquipement As TextBox
    Friend WithEvents txtFamille As TextBox
    Friend WithEvents txtFournisseur As TextBox
    Friend WithEvents txtNomCamion As TextBox
    Friend WithEvents txtCleCamion As TextBox
    Friend WithEvents DataGridView1 As DataGridView
    Friend WithEvents txtCodeCamion As TextBox
    Friend WithEvents btnModifier As Button
    Friend WithEvents RECHERCHE As Button
    Friend WithEvents btnSupprimer As Button
    Friend WithEvents btnAjouter As Button
    Friend WithEvents PrintDocument1 As Printing.PrintDocument
    Friend WithEvents PrintPreviewDialog1 As PrintPreviewDialog
    Friend WithEvents Label21 As Label
    Friend WithEvents Label13 As Label
    Friend WithEvents txtMatricule As TextBox
End Class
