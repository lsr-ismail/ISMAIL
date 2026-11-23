<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class Éléments_à_bétonner
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
        Label13 = New Label()
        txtRECHERCHE = New TextBox()
        btnActualiser = New Button()
        btnNouveau = New Button()
        Label5 = New Label()
        Label6 = New Label()
        Label3 = New Label()
        Label2 = New Label()
        Label1 = New Label()
        Imprimer = New Button()
        txtDescriptionElement1 = New TextBox()
        txtDescriptionElement = New TextBox()
        txtNomElement = New TextBox()
        txtCodeElement = New TextBox()
        DataGridView1 = New DataGridView()
        txtIdElement = New TextBox()
        btnModifier = New Button()
        RECHERCHE = New Button()
        btnSupprimer = New Button()
        btnAjouter = New Button()
        txtDerniereModification = New TextBox()
        Label21 = New Label()
        CType(DataGridView1, ComponentModel.ISupportInitialize).BeginInit()
        SuspendLayout()
        ' 
        ' Label13
        ' 
        Label13.AutoSize = True
        Label13.Location = New Point(72, 321)
        Label13.Name = "Label13"
        Label13.Size = New Size(155, 20)
        Label13.TabIndex = 146
        Label13.Text = "Dernière Modification"
        ' 
        ' txtRECHERCHE
        ' 
        txtRECHERCHE.Location = New Point(243, 123)
        txtRECHERCHE.Name = "txtRECHERCHE"
        txtRECHERCHE.Size = New Size(418, 27)
        txtRECHERCHE.TabIndex = 142
        ' 
        ' btnActualiser
        ' 
        btnActualiser.Location = New Point(744, 40)
        btnActualiser.Name = "btnActualiser"
        btnActualiser.Size = New Size(137, 61)
        btnActualiser.TabIndex = 141
        btnActualiser.Text = "Actualiser"
        btnActualiser.UseVisualStyleBackColor = True
        ' 
        ' btnNouveau
        ' 
        btnNouveau.Location = New Point(887, 40)
        btnNouveau.Name = "btnNouveau"
        btnNouveau.Size = New Size(137, 61)
        btnNouveau.TabIndex = 140
        btnNouveau.Text = "Nouveau"
        btnNouveau.UseVisualStyleBackColor = True
        ' 
        ' Label5
        ' 
        Label5.AutoSize = True
        Label5.Location = New Point(72, 288)
        Label5.Name = "Label5"
        Label5.Size = New Size(151, 20)
        Label5.TabIndex = 133
        Label5.Text = "Description Element1"
        ' 
        ' Label6
        ' 
        Label6.AutoSize = True
        Label6.Location = New Point(72, 255)
        Label6.Name = "Label6"
        Label6.Size = New Size(143, 20)
        Label6.TabIndex = 132
        Label6.Text = "Description Élément"
        ' 
        ' Label3
        ' 
        Label3.AutoSize = True
        Label3.Location = New Point(72, 222)
        Label3.Name = "Label3"
        Label3.Size = New Size(100, 20)
        Label3.TabIndex = 131
        Label3.Text = "Nom Élément"
        ' 
        ' Label2
        ' 
        Label2.AutoSize = True
        Label2.Location = New Point(72, 191)
        Label2.Name = "Label2"
        Label2.Size = New Size(102, 20)
        Label2.TabIndex = 130
        Label2.Text = "Code Élément"
        ' 
        ' Label1
        ' 
        Label1.AutoSize = True
        Label1.Location = New Point(72, 156)
        Label1.Name = "Label1"
        Label1.Size = New Size(84, 20)
        Label1.TabIndex = 129
        Label1.Text = " Id Élément"
        ' 
        ' Imprimer
        ' 
        Imprimer.Location = New Point(601, 40)
        Imprimer.Name = "Imprimer"
        Imprimer.Size = New Size(137, 61)
        Imprimer.TabIndex = 128
        Imprimer.Text = "Imprimer"
        Imprimer.UseVisualStyleBackColor = True
        ' 
        ' txtDescriptionElement1
        ' 
        txtDescriptionElement1.Location = New Point(241, 288)
        txtDescriptionElement1.Name = "txtDescriptionElement1"
        txtDescriptionElement1.Size = New Size(418, 27)
        txtDescriptionElement1.TabIndex = 121
        ' 
        ' txtDescriptionElement
        ' 
        txtDescriptionElement.Location = New Point(241, 255)
        txtDescriptionElement.Name = "txtDescriptionElement"
        txtDescriptionElement.Size = New Size(418, 27)
        txtDescriptionElement.TabIndex = 120
        ' 
        ' txtNomElement
        ' 
        txtNomElement.Location = New Point(241, 222)
        txtNomElement.Name = "txtNomElement"
        txtNomElement.Size = New Size(418, 27)
        txtNomElement.TabIndex = 119
        ' 
        ' txtCodeElement
        ' 
        txtCodeElement.Location = New Point(243, 191)
        txtCodeElement.Name = "txtCodeElement"
        txtCodeElement.Size = New Size(418, 27)
        txtCodeElement.TabIndex = 118
        ' 
        ' DataGridView1
        ' 
        DataGridView1.AllowUserToAddRows = False
        DataGridView1.Anchor = AnchorStyles.Top Or AnchorStyles.Bottom Or AnchorStyles.Left Or AnchorStyles.Right
        DataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill
        DataGridView1.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize
        DataGridView1.Location = New Point(29, 374)
        DataGridView1.MultiSelect = False
        DataGridView1.Name = "DataGridView1"
        DataGridView1.ReadOnly = True
        DataGridView1.RowHeadersWidth = 51
        DataGridView1.SelectionMode = DataGridViewSelectionMode.FullRowSelect
        DataGridView1.Size = New Size(979, 250)
        DataGridView1.TabIndex = 117
        ' 
        ' txtIdElement
        ' 
        txtIdElement.Location = New Point(241, 156)
        txtIdElement.Name = "txtIdElement"
        txtIdElement.Size = New Size(418, 27)
        txtIdElement.TabIndex = 116
        ' 
        ' btnModifier
        ' 
        btnModifier.Location = New Point(458, 40)
        btnModifier.Name = "btnModifier"
        btnModifier.Size = New Size(137, 61)
        btnModifier.TabIndex = 115
        btnModifier.Text = "EDITE"
        btnModifier.UseVisualStyleBackColor = True
        ' 
        ' RECHERCHE
        ' 
        RECHERCHE.Location = New Point(315, 40)
        RECHERCHE.Name = "RECHERCHE"
        RECHERCHE.Size = New Size(137, 61)
        RECHERCHE.TabIndex = 114
        RECHERCHE.Text = "RECHERCHE"
        RECHERCHE.UseVisualStyleBackColor = True
        ' 
        ' btnSupprimer
        ' 
        btnSupprimer.Location = New Point(172, 40)
        btnSupprimer.Name = "btnSupprimer"
        btnSupprimer.Size = New Size(137, 61)
        btnSupprimer.TabIndex = 113
        btnSupprimer.Text = "SUPRIME"
        btnSupprimer.UseVisualStyleBackColor = True
        ' 
        ' btnAjouter
        ' 
        btnAjouter.Location = New Point(29, 40)
        btnAjouter.Name = "btnAjouter"
        btnAjouter.Size = New Size(137, 61)
        btnAjouter.TabIndex = 112
        btnAjouter.Text = "AJOUTER"
        btnAjouter.UseVisualStyleBackColor = True
        ' 
        ' txtDerniereModification
        ' 
        txtDerniereModification.Location = New Point(241, 321)
        txtDerniereModification.Name = "txtDerniereModification"
        txtDerniereModification.Size = New Size(418, 27)
        txtDerniereModification.TabIndex = 145
        ' 
        ' Label21
        ' 
        Label21.AutoSize = True
        Label21.Font = New Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point, CByte(0))
        Label21.Location = New Point(12, 9)
        Label21.Name = "Label21"
        Label21.Size = New Size(196, 28)
        Label21.TabIndex = 311
        Label21.Text = "Éléments_à_bétonner"
        ' 
        ' Éléments_à_bétonner
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
        Controls.Add(Label5)
        Controls.Add(Label6)
        Controls.Add(Label3)
        Controls.Add(Label2)
        Controls.Add(Label1)
        Controls.Add(Imprimer)
        Controls.Add(txtDescriptionElement1)
        Controls.Add(txtDescriptionElement)
        Controls.Add(txtNomElement)
        Controls.Add(txtCodeElement)
        Controls.Add(DataGridView1)
        Controls.Add(txtIdElement)
        Controls.Add(btnModifier)
        Controls.Add(RECHERCHE)
        Controls.Add(btnSupprimer)
        Controls.Add(btnAjouter)
        Name = "Éléments_à_bétonner"
        Text = "Éléments_à_bétonner"
        CType(DataGridView1, ComponentModel.ISupportInitialize).EndInit()
        ResumeLayout(False)
        PerformLayout()
    End Sub

    Friend WithEvents Label13 As Label
    Friend WithEvents txtRECHERCHE As TextBox
    Friend WithEvents btnActualiser As Button
    Friend WithEvents btnNouveau As Button
    Friend WithEvents Label5 As Label
    Friend WithEvents Label6 As Label
    Friend WithEvents Label3 As Label
    Friend WithEvents Label2 As Label
    Friend WithEvents Label1 As Label
    Friend WithEvents Imprimer As Button
    Friend WithEvents txtDescriptionElement1 As TextBox
    Friend WithEvents txtDescriptionElement As TextBox
    Friend WithEvents txtNomElement As TextBox
    Friend WithEvents txtCodeElement As TextBox
    Friend WithEvents DataGridView1 As DataGridView
    Friend WithEvents txtIdElement As TextBox
    Friend WithEvents btnModifier As Button
    Friend WithEvents RECHERCHE As Button
    Friend WithEvents btnSupprimer As Button
    Friend WithEvents btnAjouter As Button
    Friend WithEvents txtDerniereModification As TextBox
    Friend WithEvents Label21 As Label
End Class
