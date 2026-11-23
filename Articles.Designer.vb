<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class Articles
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(Articles))
        Label13 = New Label()
        txtDerniereModification = New TextBox()
        txtRECHERCHE = New TextBox()
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
        txtUnite = New TextBox()
        txtServiceTransport = New TextBox()
        txtCodeExportation = New TextBox()
        txtNatureService = New TextBox()
        txtRemarques = New TextBox()
        txtDescriptionArticle = New TextBox()
        txtDesignation = New TextBox()
        txtNomArticle = New TextBox()
        txtCodeArticle = New TextBox()
        DataGridView1 = New DataGridView()
        txtIdArticle = New TextBox()
        btnModifier = New Button()
        RECHERCHE = New Button()
        btnSupprimer = New Button()
        btnAjouter = New Button()
        PrintDocument1 = New Printing.PrintDocument()
        PrintPreviewDialog1 = New PrintPreviewDialog()
        Label21 = New Label()
        CType(DataGridView1, ComponentModel.ISupportInitialize).BeginInit()
        SuspendLayout()
        ' 
        ' Label13
        ' 
        Label13.AutoSize = True
        Label13.Location = New Point(33, 470)
        Label13.Name = "Label13"
        Label13.Size = New Size(155, 20)
        Label13.TabIndex = 146
        Label13.Text = "Dernière Modification"
        ' 
        ' txtDerniereModification
        ' 
        txtDerniereModification.Location = New Point(194, 466)
        txtDerniereModification.Name = "txtDerniereModification"
        txtDerniereModification.Size = New Size(418, 27)
        txtDerniereModification.TabIndex = 145
        ' 
        ' txtRECHERCHE
        ' 
        txtRECHERCHE.Location = New Point(244, 15)
        txtRECHERCHE.Name = "txtRECHERCHE"
        txtRECHERCHE.Size = New Size(418, 27)
        txtRECHERCHE.TabIndex = 142
        ' 
        ' btnActualiser
        ' 
        btnActualiser.Location = New Point(922, 56)
        btnActualiser.Name = "btnActualiser"
        btnActualiser.Size = New Size(178, 61)
        btnActualiser.TabIndex = 141
        btnActualiser.Text = "Actualiser"
        btnActualiser.UseVisualStyleBackColor = True
        ' 
        ' btnNouveau
        ' 
        btnNouveau.Location = New Point(691, -4)
        btnNouveau.Name = "btnNouveau"
        btnNouveau.Size = New Size(178, 61)
        btnNouveau.TabIndex = 140
        btnNouveau.Text = "Nouveau"
        btnNouveau.UseVisualStyleBackColor = True
        ' 
        ' Label9
        ' 
        Label9.AutoSize = True
        Label9.Location = New Point(35, 437)
        Label9.Name = "Label9"
        Label9.Size = New Size(44, 20)
        Label9.TabIndex = 138
        Label9.Text = "Unité"
        ' 
        ' Label10
        ' 
        Label10.AutoSize = True
        Label10.Location = New Point(38, 404)
        Label10.Name = "Label10"
        Label10.Size = New Size(122, 20)
        Label10.TabIndex = 137
        Label10.Text = "Service Transport"
        ' 
        ' Label11
        ' 
        Label11.AutoSize = True
        Label11.Location = New Point(36, 371)
        Label11.Name = "Label11"
        Label11.Size = New Size(125, 20)
        Label11.TabIndex = 136
        Label11.Text = "Code Exportation"
        ' 
        ' Label12
        ' 
        Label12.AutoSize = True
        Label12.Location = New Point(38, 338)
        Label12.Name = "Label12"
        Label12.Size = New Size(105, 20)
        Label12.TabIndex = 135
        Label12.Text = "Nature Service"
        ' 
        ' Label4
        ' 
        Label4.AutoSize = True
        Label4.Location = New Point(36, 305)
        Label4.Name = "Label4"
        Label4.Size = New Size(83, 20)
        Label4.TabIndex = 134
        Label4.Text = "Remarques"
        ' 
        ' Label5
        ' 
        Label5.AutoSize = True
        Label5.Location = New Point(36, 272)
        Label5.Name = "Label5"
        Label5.Size = New Size(132, 20)
        Label5.TabIndex = 133
        Label5.Text = "Description Article"
        ' 
        ' Label6
        ' 
        Label6.AutoSize = True
        Label6.Location = New Point(33, 239)
        Label6.Name = "Label6"
        Label6.Size = New Size(89, 20)
        Label6.TabIndex = 132
        Label6.Text = "Désignation"
        ' 
        ' Label3
        ' 
        Label3.AutoSize = True
        Label3.Location = New Point(36, 206)
        Label3.Name = "Label3"
        Label3.Size = New Size(89, 20)
        Label3.TabIndex = 131
        Label3.Text = "Nom Article"
        ' 
        ' Label2
        ' 
        Label2.AutoSize = True
        Label2.Location = New Point(36, 173)
        Label2.Name = "Label2"
        Label2.Size = New Size(91, 20)
        Label2.TabIndex = 130
        Label2.Text = "Code Article"
        ' 
        ' Label1
        ' 
        Label1.AutoSize = True
        Label1.Location = New Point(36, 140)
        Label1.Name = "Label1"
        Label1.Size = New Size(69, 20)
        Label1.TabIndex = 129
        Label1.Text = "Id Article"
        ' 
        ' Imprimer
        ' 
        Imprimer.Location = New Point(779, 56)
        Imprimer.Name = "Imprimer"
        Imprimer.Size = New Size(178, 61)
        Imprimer.TabIndex = 128
        Imprimer.Text = "Imprimer"
        Imprimer.UseVisualStyleBackColor = True
        ' 
        ' txtUnite
        ' 
        txtUnite.Location = New Point(194, 433)
        txtUnite.Name = "txtUnite"
        txtUnite.Size = New Size(418, 27)
        txtUnite.TabIndex = 126
        ' 
        ' txtServiceTransport
        ' 
        txtServiceTransport.Location = New Point(194, 400)
        txtServiceTransport.Name = "txtServiceTransport"
        txtServiceTransport.Size = New Size(418, 27)
        txtServiceTransport.TabIndex = 125
        ' 
        ' txtCodeExportation
        ' 
        txtCodeExportation.Location = New Point(194, 367)
        txtCodeExportation.Name = "txtCodeExportation"
        txtCodeExportation.Size = New Size(418, 27)
        txtCodeExportation.TabIndex = 124
        ' 
        ' txtNatureService
        ' 
        txtNatureService.Location = New Point(194, 334)
        txtNatureService.Name = "txtNatureService"
        txtNatureService.Size = New Size(418, 27)
        txtNatureService.TabIndex = 123
        ' 
        ' txtRemarques
        ' 
        txtRemarques.Location = New Point(194, 301)
        txtRemarques.Name = "txtRemarques"
        txtRemarques.Size = New Size(418, 27)
        txtRemarques.TabIndex = 122
        ' 
        ' txtDescriptionArticle
        ' 
        txtDescriptionArticle.Location = New Point(194, 268)
        txtDescriptionArticle.Name = "txtDescriptionArticle"
        txtDescriptionArticle.Size = New Size(418, 27)
        txtDescriptionArticle.TabIndex = 121
        ' 
        ' txtDesignation
        ' 
        txtDesignation.Location = New Point(194, 235)
        txtDesignation.Name = "txtDesignation"
        txtDesignation.Size = New Size(418, 27)
        txtDesignation.TabIndex = 120
        ' 
        ' txtNomArticle
        ' 
        txtNomArticle.Location = New Point(194, 202)
        txtNomArticle.Name = "txtNomArticle"
        txtNomArticle.Size = New Size(418, 27)
        txtNomArticle.TabIndex = 119
        ' 
        ' txtCodeArticle
        ' 
        txtCodeArticle.Location = New Point(196, 171)
        txtCodeArticle.Name = "txtCodeArticle"
        txtCodeArticle.Size = New Size(418, 27)
        txtCodeArticle.TabIndex = 118
        ' 
        ' DataGridView1
        ' 
        DataGridView1.AllowUserToAddRows = False
        DataGridView1.Anchor = AnchorStyles.Top Or AnchorStyles.Bottom Or AnchorStyles.Left Or AnchorStyles.Right
        DataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill
        DataGridView1.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize
        DataGridView1.Location = New Point(665, 140)
        DataGridView1.MultiSelect = False
        DataGridView1.Name = "DataGridView1"
        DataGridView1.ReadOnly = True
        DataGridView1.RowHeadersWidth = 51
        DataGridView1.SelectionMode = DataGridViewSelectionMode.FullRowSelect
        DataGridView1.Size = New Size(0, 64)
        DataGridView1.TabIndex = 117
        ' 
        ' txtIdArticle
        ' 
        txtIdArticle.Location = New Point(194, 136)
        txtIdArticle.Name = "txtIdArticle"
        txtIdArticle.Size = New Size(418, 27)
        txtIdArticle.TabIndex = 116
        ' 
        ' btnModifier
        ' 
        btnModifier.Location = New Point(595, 56)
        btnModifier.Name = "btnModifier"
        btnModifier.Size = New Size(178, 61)
        btnModifier.TabIndex = 115
        btnModifier.Text = "EDITE"
        btnModifier.UseVisualStyleBackColor = True
        ' 
        ' RECHERCHE
        ' 
        RECHERCHE.Location = New Point(413, 56)
        RECHERCHE.Name = "RECHERCHE"
        RECHERCHE.Size = New Size(178, 61)
        RECHERCHE.TabIndex = 114
        RECHERCHE.Text = "RECHERCHE"
        RECHERCHE.UseVisualStyleBackColor = True
        ' 
        ' btnSupprimer
        ' 
        btnSupprimer.Location = New Point(224, 56)
        btnSupprimer.Name = "btnSupprimer"
        btnSupprimer.Size = New Size(178, 61)
        btnSupprimer.TabIndex = 113
        btnSupprimer.Text = "SUPRIME"
        btnSupprimer.UseVisualStyleBackColor = True
        ' 
        ' btnAjouter
        ' 
        btnAjouter.Location = New Point(36, 56)
        btnAjouter.Name = "btnAjouter"
        btnAjouter.Size = New Size(178, 61)
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
        Label21.Location = New Point(16, 9)
        Label21.Name = "Label21"
        Label21.Size = New Size(76, 28)
        Label21.TabIndex = 147
        Label21.Text = "Articles"
        ' 
        ' Articles
        ' 
        AutoScaleDimensions = New SizeF(8F, 20F)
        AutoScaleMode = AutoScaleMode.Font
        ClientSize = New Size(1082, 723)
        Controls.Add(Label21)
        Controls.Add(Label13)
        Controls.Add(txtDerniereModification)
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
        Controls.Add(txtUnite)
        Controls.Add(txtServiceTransport)
        Controls.Add(txtCodeExportation)
        Controls.Add(txtNatureService)
        Controls.Add(txtRemarques)
        Controls.Add(txtDescriptionArticle)
        Controls.Add(txtDesignation)
        Controls.Add(txtNomArticle)
        Controls.Add(txtCodeArticle)
        Controls.Add(DataGridView1)
        Controls.Add(txtIdArticle)
        Controls.Add(btnModifier)
        Controls.Add(RECHERCHE)
        Controls.Add(btnSupprimer)
        Controls.Add(btnAjouter)
        Name = "Articles"
        Text = "Articles"
        CType(DataGridView1, ComponentModel.ISupportInitialize).EndInit()
        ResumeLayout(False)
        PerformLayout()
    End Sub

    Friend WithEvents Label13 As Label
    Friend WithEvents txtDerniereModification As TextBox
    Friend WithEvents txtRECHERCHE As TextBox
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
    Friend WithEvents txtUnite As TextBox
    Friend WithEvents txtServiceTransport As TextBox
    Friend WithEvents txtCodeExportation As TextBox
    Friend WithEvents txtNatureService As TextBox
    Friend WithEvents txtRemarques As TextBox
    Friend WithEvents txtDescriptionArticle As TextBox
    Friend WithEvents txtDesignation As TextBox
    Friend WithEvents txtNomArticle As TextBox
    Friend WithEvents txtCodeArticle As TextBox
    Friend WithEvents DataGridView1 As DataGridView
    Friend WithEvents txtIdArticle As TextBox
    Friend WithEvents btnModifier As Button
    Friend WithEvents RECHERCHE As Button
    Friend WithEvents btnSupprimer As Button
    Friend WithEvents btnAjouter As Button
    Friend WithEvents PrintDocument1 As Printing.PrintDocument
    Friend WithEvents PrintPreviewDialog1 As PrintPreviewDialog
    Friend WithEvents Label21 As Label
End Class
