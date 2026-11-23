<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class Transporteurs
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
        txtValeur2 = New TextBox()
        txtValeur1 = New TextBox()
        txtService3 = New TextBox()
        txtService2 = New TextBox()
        txtService1 = New TextBox()
        txtEquipement = New TextBox()
        txtFamille = New TextBox()
        txtNomTransporteurs = New TextBox()
        txtCleTransporteurs = New TextBox()
        txtCodeTransporteurs = New TextBox()
        DataGridView1 = New DataGridView()
        txtIdTransporteurs = New TextBox()
        btnModifier = New Button()
        RECHERCHE = New Button()
        btnSupprimer = New Button()
        btnAjouter = New Button()
        Label7 = New Label()
        txtValeur3 = New TextBox()
        Label13 = New Label()
        txtDerniereModification = New TextBox()
        Label21 = New Label()
        CType(DataGridView1, ComponentModel.ISupportInitialize).BeginInit()
        SuspendLayout()
        ' 
        ' txtRECHERCHE
        ' 
        txtRECHERCHE.Location = New Point(197, 135)
        txtRECHERCHE.Name = "txtRECHERCHE"
        txtRECHERCHE.Size = New Size(338, 27)
        txtRECHERCHE.TabIndex = 107
        ' 
        ' btnActualiser
        ' 
        btnActualiser.Location = New Point(925, 45)
        btnActualiser.Name = "btnActualiser"
        btnActualiser.Size = New Size(140, 60)
        btnActualiser.TabIndex = 106
        btnActualiser.Text = "Actualiser"
        btnActualiser.UseVisualStyleBackColor = True
        ' 
        ' btnNouveau
        ' 
        btnNouveau.Location = New Point(779, 45)
        btnNouveau.Name = "btnNouveau"
        btnNouveau.Size = New Size(140, 60)
        btnNouveau.TabIndex = 105
        btnNouveau.Text = "Nouveau"
        btnNouveau.UseVisualStyleBackColor = True
        ' 
        ' Label8
        ' 
        Label8.AutoSize = True
        Label8.Location = New Point(36, 502)
        Label8.Name = "Label8"
        Label8.Size = New Size(62, 20)
        Label8.TabIndex = 104
        Label8.Text = " Valeur2"
        ' 
        ' Label9
        ' 
        Label9.AutoSize = True
        Label9.Location = New Point(38, 469)
        Label9.Name = "Label9"
        Label9.Size = New Size(58, 20)
        Label9.TabIndex = 103
        Label9.Text = "Valeur1"
        ' 
        ' Label10
        ' 
        Label10.AutoSize = True
        Label10.Location = New Point(41, 436)
        Label10.Name = "Label10"
        Label10.Size = New Size(64, 20)
        Label10.TabIndex = 102
        Label10.Text = "Service3"
        ' 
        ' Label11
        ' 
        Label11.AutoSize = True
        Label11.Location = New Point(39, 403)
        Label11.Name = "Label11"
        Label11.Size = New Size(64, 20)
        Label11.TabIndex = 101
        Label11.Text = "Service2"
        ' 
        ' Label12
        ' 
        Label12.AutoSize = True
        Label12.Location = New Point(41, 370)
        Label12.Name = "Label12"
        Label12.Size = New Size(64, 20)
        Label12.TabIndex = 100
        Label12.Text = "Service1"
        ' 
        ' Label4
        ' 
        Label4.AutoSize = True
        Label4.Location = New Point(39, 337)
        Label4.Name = "Label4"
        Label4.Size = New Size(89, 20)
        Label4.TabIndex = 99
        Label4.Text = "Équipement"
        ' 
        ' Label5
        ' 
        Label5.AutoSize = True
        Label5.Location = New Point(39, 304)
        Label5.Name = "Label5"
        Label5.Size = New Size(56, 20)
        Label5.TabIndex = 98
        Label5.Text = "Famille"
        ' 
        ' Label6
        ' 
        Label6.AutoSize = True
        Label6.Location = New Point(36, 271)
        Label6.Name = "Label6"
        Label6.Size = New Size(135, 20)
        Label6.TabIndex = 97
        Label6.Text = "Nom Transporteurs"
        ' 
        ' Label3
        ' 
        Label3.AutoSize = True
        Label3.Location = New Point(39, 238)
        Label3.Name = "Label3"
        Label3.Size = New Size(123, 20)
        Label3.TabIndex = 96
        Label3.Text = "Cle Transporteurs"
        ' 
        ' Label2
        ' 
        Label2.AutoSize = True
        Label2.Location = New Point(39, 205)
        Label2.Name = "Label2"
        Label2.Size = New Size(137, 20)
        Label2.TabIndex = 95
        Label2.Text = "Code Transporteurs"
        ' 
        ' Label1
        ' 
        Label1.AutoSize = True
        Label1.Location = New Point(39, 172)
        Label1.Name = "Label1"
        Label1.Size = New Size(115, 20)
        Label1.TabIndex = 94
        Label1.Text = "Id Transporteurs"
        ' 
        ' Imprimer
        ' 
        Imprimer.Location = New Point(633, 45)
        Imprimer.Name = "Imprimer"
        Imprimer.Size = New Size(140, 60)
        Imprimer.TabIndex = 93
        Imprimer.Text = "Imprimer"
        Imprimer.UseVisualStyleBackColor = True
        ' 
        ' txtValeur2
        ' 
        txtValeur2.Location = New Point(197, 498)
        txtValeur2.Name = "txtValeur2"
        txtValeur2.Size = New Size(338, 27)
        txtValeur2.TabIndex = 92
        ' 
        ' txtValeur1
        ' 
        txtValeur1.Location = New Point(197, 465)
        txtValeur1.Name = "txtValeur1"
        txtValeur1.Size = New Size(338, 27)
        txtValeur1.TabIndex = 91
        ' 
        ' txtService3
        ' 
        txtService3.Location = New Point(197, 432)
        txtService3.Name = "txtService3"
        txtService3.Size = New Size(338, 27)
        txtService3.TabIndex = 90
        ' 
        ' txtService2
        ' 
        txtService2.Location = New Point(197, 399)
        txtService2.Name = "txtService2"
        txtService2.Size = New Size(338, 27)
        txtService2.TabIndex = 89
        ' 
        ' txtService1
        ' 
        txtService1.Location = New Point(197, 366)
        txtService1.Name = "txtService1"
        txtService1.Size = New Size(338, 27)
        txtService1.TabIndex = 88
        ' 
        ' txtEquipement
        ' 
        txtEquipement.Location = New Point(197, 333)
        txtEquipement.Name = "txtEquipement"
        txtEquipement.Size = New Size(338, 27)
        txtEquipement.TabIndex = 87
        ' 
        ' txtFamille
        ' 
        txtFamille.Location = New Point(197, 300)
        txtFamille.Name = "txtFamille"
        txtFamille.Size = New Size(338, 27)
        txtFamille.TabIndex = 86
        ' 
        ' txtNomTransporteurs
        ' 
        txtNomTransporteurs.Location = New Point(197, 267)
        txtNomTransporteurs.Name = "txtNomTransporteurs"
        txtNomTransporteurs.Size = New Size(338, 27)
        txtNomTransporteurs.TabIndex = 85
        ' 
        ' txtCleTransporteurs
        ' 
        txtCleTransporteurs.Location = New Point(197, 234)
        txtCleTransporteurs.Name = "txtCleTransporteurs"
        txtCleTransporteurs.Size = New Size(338, 27)
        txtCleTransporteurs.TabIndex = 84
        ' 
        ' txtCodeTransporteurs
        ' 
        txtCodeTransporteurs.Location = New Point(199, 203)
        txtCodeTransporteurs.Name = "txtCodeTransporteurs"
        txtCodeTransporteurs.Size = New Size(338, 27)
        txtCodeTransporteurs.TabIndex = 83
        ' 
        ' DataGridView1
        ' 
        DataGridView1.AllowUserToAddRows = False
        DataGridView1.Anchor = AnchorStyles.Top Or AnchorStyles.Bottom Or AnchorStyles.Left Or AnchorStyles.Right
        DataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill
        DataGridView1.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize
        DataGridView1.Location = New Point(543, 135)
        DataGridView1.MultiSelect = False
        DataGridView1.Name = "DataGridView1"
        DataGridView1.ReadOnly = True
        DataGridView1.RowHeadersWidth = 51
        DataGridView1.SelectionMode = DataGridViewSelectionMode.FullRowSelect
        DataGridView1.Size = New Size(526, 456)
        DataGridView1.TabIndex = 82
        ' 
        ' txtIdTransporteurs
        ' 
        txtIdTransporteurs.Location = New Point(197, 168)
        txtIdTransporteurs.Name = "txtIdTransporteurs"
        txtIdTransporteurs.ReadOnly = True
        txtIdTransporteurs.Size = New Size(338, 27)
        txtIdTransporteurs.TabIndex = 81
        ' 
        ' btnModifier
        ' 
        btnModifier.Location = New Point(487, 45)
        btnModifier.Name = "btnModifier"
        btnModifier.Size = New Size(140, 60)
        btnModifier.TabIndex = 80
        btnModifier.Text = "EDITE"
        btnModifier.UseVisualStyleBackColor = True
        ' 
        ' RECHERCHE
        ' 
        RECHERCHE.Location = New Point(342, 45)
        RECHERCHE.Name = "RECHERCHE"
        RECHERCHE.Size = New Size(140, 60)
        RECHERCHE.TabIndex = 79
        RECHERCHE.Text = "RECHERCHE"
        RECHERCHE.UseVisualStyleBackColor = True
        ' 
        ' btnSupprimer
        ' 
        btnSupprimer.Location = New Point(197, 45)
        btnSupprimer.Name = "btnSupprimer"
        btnSupprimer.Size = New Size(140, 60)
        btnSupprimer.TabIndex = 78
        btnSupprimer.Text = "SUPRIME"
        btnSupprimer.UseVisualStyleBackColor = True
        ' 
        ' btnAjouter
        ' 
        btnAjouter.Location = New Point(51, 45)
        btnAjouter.Name = "btnAjouter"
        btnAjouter.Size = New Size(140, 60)
        btnAjouter.TabIndex = 77
        btnAjouter.Text = "AJOUTER"
        btnAjouter.UseVisualStyleBackColor = True
        ' 
        ' Label7
        ' 
        Label7.AutoSize = True
        Label7.Location = New Point(38, 535)
        Label7.Name = "Label7"
        Label7.Size = New Size(62, 20)
        Label7.TabIndex = 109
        Label7.Text = " Valeur3"
        ' 
        ' txtValeur3
        ' 
        txtValeur3.Location = New Point(199, 531)
        txtValeur3.Name = "txtValeur3"
        txtValeur3.Size = New Size(338, 27)
        txtValeur3.TabIndex = 108
        ' 
        ' Label13
        ' 
        Label13.AutoSize = True
        Label13.Location = New Point(41, 568)
        Label13.Name = "Label13"
        Label13.Size = New Size(155, 20)
        Label13.TabIndex = 111
        Label13.Text = "Dernière Modification"
        ' 
        ' txtDerniereModification
        ' 
        txtDerniereModification.Location = New Point(202, 564)
        txtDerniereModification.Name = "txtDerniereModification"
        txtDerniereModification.ReadOnly = True
        txtDerniereModification.Size = New Size(338, 27)
        txtDerniereModification.TabIndex = 110
        ' 
        ' Label21
        ' 
        Label21.AutoSize = True
        Label21.Font = New Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point, CByte(0))
        Label21.Location = New Point(12, 9)
        Label21.Name = "Label21"
        Label21.Size = New Size(130, 28)
        Label21.TabIndex = 150
        Label21.Text = "Transporteurs"
        ' 
        ' Transporteurs
        ' 
        AutoScaleDimensions = New SizeF(8F, 20F)
        AutoScaleMode = AutoScaleMode.Font
        ClientSize = New Size(1099, 621)
        Controls.Add(Label21)
        Controls.Add(Label13)
        Controls.Add(txtDerniereModification)
        Controls.Add(Label7)
        Controls.Add(txtValeur3)
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
        Controls.Add(txtValeur2)
        Controls.Add(txtValeur1)
        Controls.Add(txtService3)
        Controls.Add(txtService2)
        Controls.Add(txtService1)
        Controls.Add(txtEquipement)
        Controls.Add(txtFamille)
        Controls.Add(txtNomTransporteurs)
        Controls.Add(txtCleTransporteurs)
        Controls.Add(txtCodeTransporteurs)
        Controls.Add(DataGridView1)
        Controls.Add(txtIdTransporteurs)
        Controls.Add(btnModifier)
        Controls.Add(RECHERCHE)
        Controls.Add(btnSupprimer)
        Controls.Add(btnAjouter)
        Name = "Transporteurs"
        Text = "Transporteurs"
        CType(DataGridView1, ComponentModel.ISupportInitialize).EndInit()
        ResumeLayout(False)
        PerformLayout()
    End Sub

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
    Friend WithEvents txtValeur2 As TextBox
    Friend WithEvents txtValeur1 As TextBox
    Friend WithEvents txtService3 As TextBox
    Friend WithEvents txtService2 As TextBox
    Friend WithEvents txtService1 As TextBox
    Friend WithEvents txtEquipement As TextBox
    Friend WithEvents txtFamille As TextBox
    Friend WithEvents txtNomTransporteurs As TextBox
    Friend WithEvents txtCleTransporteurs As TextBox
    Friend WithEvents txtCodeTransporteurs As TextBox
    Friend WithEvents DataGridView1 As DataGridView
    Friend WithEvents txtIdTransporteurs As TextBox
    Friend WithEvents btnModifier As Button
    Friend WithEvents RECHERCHE As Button
    Friend WithEvents btnSupprimer As Button
    Friend WithEvents btnAjouter As Button
    Friend WithEvents Label7 As Label
    Friend WithEvents txtValeur3 As TextBox
    Friend WithEvents Label13 As Label
    Friend WithEvents txtDerniereModification As TextBox
    Friend WithEvents Label21 As Label
End Class
