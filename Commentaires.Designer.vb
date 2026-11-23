<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class Commentaires
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(Commentaires))
        txtDerniereModification = New TextBox()
        txtRECHERCHE = New TextBox()
        btnActualiser = New Button()
        btnNouveau = New Button()
        Label3 = New Label()
        Label2 = New Label()
        Label1 = New Label()
        Imprimer = New Button()
        txtDescriptionCommentaire = New TextBox()
        txtCodeCommentaire = New TextBox()
        DataGridView1 = New DataGridView()
        txtIdCommentaire = New TextBox()
        btnModifier = New Button()
        RECHERCHE = New Button()
        btnSupprimer = New Button()
        btnAjouter = New Button()
        Label13 = New Label()
        PrintDialog1 = New PrintDialog()
        PrintPreviewDialog1 = New PrintPreviewDialog()
        PrintDocument1 = New Printing.PrintDocument()
        Label4 = New Label()
        CType(DataGridView1, ComponentModel.ISupportInitialize).BeginInit()
        SuspendLayout()
        ' 
        ' txtDerniereModification
        ' 
        txtDerniereModification.Location = New Point(197, 292)
        txtDerniereModification.Name = "txtDerniereModification"
        txtDerniereModification.Size = New Size(379, 27)
        txtDerniereModification.TabIndex = 145
        ' 
        ' txtRECHERCHE
        ' 
        txtRECHERCHE.Location = New Point(197, 151)
        txtRECHERCHE.Name = "txtRECHERCHE"
        txtRECHERCHE.Size = New Size(377, 27)
        txtRECHERCHE.TabIndex = 142
        ' 
        ' btnActualiser
        ' 
        btnActualiser.Location = New Point(906, 52)
        btnActualiser.Name = "btnActualiser"
        btnActualiser.Size = New Size(141, 61)
        btnActualiser.TabIndex = 141
        btnActualiser.Text = "Actualiser"
        btnActualiser.UseVisualStyleBackColor = True
        ' 
        ' btnNouveau
        ' 
        btnNouveau.Location = New Point(759, 52)
        btnNouveau.Name = "btnNouveau"
        btnNouveau.Size = New Size(141, 61)
        btnNouveau.TabIndex = 140
        btnNouveau.Text = "Nouveau"
        btnNouveau.UseVisualStyleBackColor = True
        ' 
        ' Label3
        ' 
        Label3.AutoSize = True
        Label3.Location = New Point(14, 262)
        Label3.Name = "Label3"
        Label3.Size = New Size(179, 20)
        Label3.TabIndex = 131
        Label3.Text = "Description Commentaire"
        ' 
        ' Label2
        ' 
        Label2.AutoSize = True
        Label2.Location = New Point(14, 229)
        Label2.Name = "Label2"
        Label2.Size = New Size(138, 20)
        Label2.TabIndex = 130
        Label2.Text = "Code Commentaire"
        ' 
        ' Label1
        ' 
        Label1.AutoSize = True
        Label1.Location = New Point(14, 196)
        Label1.Name = "Label1"
        Label1.Size = New Size(116, 20)
        Label1.TabIndex = 129
        Label1.Text = "Id Commentaire"
        ' 
        ' Imprimer
        ' 
        Imprimer.Location = New Point(612, 52)
        Imprimer.Name = "Imprimer"
        Imprimer.Size = New Size(141, 61)
        Imprimer.TabIndex = 128
        Imprimer.Text = "Imprimer"
        Imprimer.UseVisualStyleBackColor = True
        ' 
        ' txtDescriptionCommentaire
        ' 
        txtDescriptionCommentaire.Location = New Point(195, 259)
        txtDescriptionCommentaire.Name = "txtDescriptionCommentaire"
        txtDescriptionCommentaire.Size = New Size(379, 27)
        txtDescriptionCommentaire.TabIndex = 119
        ' 
        ' txtCodeCommentaire
        ' 
        txtCodeCommentaire.Location = New Point(197, 228)
        txtCodeCommentaire.Name = "txtCodeCommentaire"
        txtCodeCommentaire.Size = New Size(379, 27)
        txtCodeCommentaire.TabIndex = 118
        ' 
        ' DataGridView1
        ' 
        DataGridView1.AllowUserToAddRows = False
        DataGridView1.Anchor = AnchorStyles.Top Or AnchorStyles.Bottom Or AnchorStyles.Left Or AnchorStyles.Right
        DataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill
        DataGridView1.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize
        DataGridView1.Location = New Point(597, 127)
        DataGridView1.MultiSelect = False
        DataGridView1.Name = "DataGridView1"
        DataGridView1.ReadOnly = True
        DataGridView1.RowHeadersWidth = 51
        DataGridView1.SelectionMode = DataGridViewSelectionMode.FullRowSelect
        DataGridView1.Size = New Size(461, 419)
        DataGridView1.TabIndex = 117
        ' 
        ' txtIdCommentaire
        ' 
        txtIdCommentaire.Location = New Point(195, 193)
        txtIdCommentaire.Name = "txtIdCommentaire"
        txtIdCommentaire.Size = New Size(379, 27)
        txtIdCommentaire.TabIndex = 116
        ' 
        ' btnModifier
        ' 
        btnModifier.Location = New Point(465, 52)
        btnModifier.Name = "btnModifier"
        btnModifier.Size = New Size(141, 61)
        btnModifier.TabIndex = 115
        btnModifier.Text = "EDITE"
        btnModifier.UseVisualStyleBackColor = True
        ' 
        ' RECHERCHE
        ' 
        RECHERCHE.Location = New Point(318, 52)
        RECHERCHE.Name = "RECHERCHE"
        RECHERCHE.Size = New Size(141, 61)
        RECHERCHE.TabIndex = 114
        RECHERCHE.Text = "RECHERCHE"
        RECHERCHE.UseVisualStyleBackColor = True
        ' 
        ' btnSupprimer
        ' 
        btnSupprimer.Location = New Point(171, 52)
        btnSupprimer.Name = "btnSupprimer"
        btnSupprimer.Size = New Size(141, 61)
        btnSupprimer.TabIndex = 113
        btnSupprimer.Text = "SUPRIME"
        btnSupprimer.UseVisualStyleBackColor = True
        ' 
        ' btnAjouter
        ' 
        btnAjouter.Location = New Point(27, 52)
        btnAjouter.Name = "btnAjouter"
        btnAjouter.Size = New Size(141, 61)
        btnAjouter.TabIndex = 112
        btnAjouter.Text = "AJOUTER"
        btnAjouter.UseVisualStyleBackColor = True
        ' 
        ' Label13
        ' 
        Label13.AutoSize = True
        Label13.Location = New Point(13, 295)
        Label13.Name = "Label13"
        Label13.Size = New Size(155, 20)
        Label13.TabIndex = 146
        Label13.Text = "Dernière Modification"
        ' 
        ' PrintDialog1
        ' 
        PrintDialog1.UseEXDialog = True
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
        ' PrintDocument1
        ' 
        ' 
        ' Label4
        ' 
        Label4.AutoSize = True
        Label4.Font = New Font("Segoe UI", 13.8F, FontStyle.Regular, GraphicsUnit.Point, CByte(0))
        Label4.Location = New Point(27, 9)
        Label4.Name = "Label4"
        Label4.Size = New Size(162, 31)
        Label4.TabIndex = 252
        Label4.Text = "Commentaires"
        ' 
        ' Commentaires
        ' 
        AutoScaleDimensions = New SizeF(8F, 20F)
        AutoScaleMode = AutoScaleMode.Font
        ClientSize = New Size(1082, 723)
        Controls.Add(Label4)
        Controls.Add(Label13)
        Controls.Add(txtDerniereModification)
        Controls.Add(txtRECHERCHE)
        Controls.Add(btnActualiser)
        Controls.Add(btnNouveau)
        Controls.Add(Label3)
        Controls.Add(Label2)
        Controls.Add(Label1)
        Controls.Add(Imprimer)
        Controls.Add(txtDescriptionCommentaire)
        Controls.Add(txtCodeCommentaire)
        Controls.Add(DataGridView1)
        Controls.Add(txtIdCommentaire)
        Controls.Add(btnModifier)
        Controls.Add(RECHERCHE)
        Controls.Add(btnSupprimer)
        Controls.Add(btnAjouter)
        Name = "Commentaires"
        Text = "Commentaires"
        CType(DataGridView1, ComponentModel.ISupportInitialize).EndInit()
        ResumeLayout(False)
        PerformLayout()
    End Sub
    Friend WithEvents txtDerniereModification As TextBox
    Friend WithEvents txtRECHERCHE As TextBox
    Friend WithEvents btnActualiser As Button
    Friend WithEvents btnNouveau As Button
    Friend WithEvents Label3 As Label
    Friend WithEvents Label2 As Label
    Friend WithEvents Label1 As Label
    Friend WithEvents Imprimer As Button
    Friend WithEvents txtDescriptionCommentaire As TextBox
    Friend WithEvents txtCodeCommentaire As TextBox
    Friend WithEvents DataGridView1 As DataGridView
    Friend WithEvents txtIdCommentaire As TextBox
    Friend WithEvents btnModifier As Button
    Friend WithEvents RECHERCHE As Button
    Friend WithEvents btnSupprimer As Button
    Friend WithEvents btnAjouter As Button
    Friend WithEvents Label13 As Label
    Friend WithEvents PrintDialog1 As PrintDialog
    Friend WithEvents PrintPreviewDialog1 As PrintPreviewDialog
    Friend WithEvents PrintDocument1 As Printing.PrintDocument
    Friend WithEvents Label4 As Label
End Class
