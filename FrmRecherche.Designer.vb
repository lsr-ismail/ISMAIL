<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class FrmRecherche
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
        btnSelectionner = New Button()
        btnAnnuler = New Button()
        txtRechercheCode = New TextBox()
        DataGridView1 = New DataGridView()
        code = New Label()
        Label1 = New Label()
        lblNombre = New Label()
        txtRechercheNom = New TextBox()
        CType(DataGridView1, ComponentModel.ISupportInitialize).BeginInit()
        SuspendLayout()
        ' 
        ' btnSelectionner
        ' 
        btnSelectionner.Location = New Point(393, 78)
        btnSelectionner.Name = "btnSelectionner"
        btnSelectionner.Size = New Size(137, 36)
        btnSelectionner.TabIndex = 0
        btnSelectionner.Text = "Selectionner"
        btnSelectionner.UseVisualStyleBackColor = True
        ' 
        ' btnAnnuler
        ' 
        btnAnnuler.Location = New Point(536, 78)
        btnAnnuler.Name = "btnAnnuler"
        btnAnnuler.Size = New Size(137, 36)
        btnAnnuler.TabIndex = 1
        btnAnnuler.Text = "Annuler"
        btnAnnuler.UseVisualStyleBackColor = True
        ' 
        ' txtRechercheCode
        ' 
        txtRechercheCode.Location = New Point(373, 22)
        txtRechercheCode.Name = "txtRechercheCode"
        txtRechercheCode.Size = New Size(307, 27)
        txtRechercheCode.TabIndex = 2
        ' 
        ' DataGridView1
        ' 
        DataGridView1.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize
        DataGridView1.Location = New Point(141, 127)
        DataGridView1.Name = "DataGridView1"
        DataGridView1.RowHeadersWidth = 51
        DataGridView1.Size = New Size(511, 514)
        DataGridView1.TabIndex = 3
        ' 
        ' code
        ' 
        code.AutoSize = True
        code.Location = New Point(206, 22)
        code.Name = "code"
        code.Size = New Size(42, 20)
        code.TabIndex = 4
        code.Text = "code"
        ' 
        ' Label1
        ' 
        Label1.AutoSize = True
        Label1.Location = New Point(206, 52)
        Label1.Name = "Label1"
        Label1.Size = New Size(42, 20)
        Label1.TabIndex = 5
        Label1.Text = "Nom"
        ' 
        ' lblNombre
        ' 
        lblNombre.AutoSize = True
        lblNombre.Location = New Point(206, 86)
        lblNombre.Name = "lblNombre"
        lblNombre.Size = New Size(137, 20)
        lblNombre.TabIndex = 6
        lblNombre.Text = "0 enregistrement(s)"
        ' 
        ' txtRechercheNom
        ' 
        txtRechercheNom.Location = New Point(373, 49)
        txtRechercheNom.Name = "txtRechercheNom"
        txtRechercheNom.Size = New Size(307, 27)
        txtRechercheNom.TabIndex = 7
        ' 
        ' FrmRecherche
        ' 
        AutoScaleDimensions = New SizeF(8F, 20F)
        AutoScaleMode = AutoScaleMode.Font
        ClientSize = New Size(1082, 723)
        Controls.Add(txtRechercheNom)
        Controls.Add(lblNombre)
        Controls.Add(Label1)
        Controls.Add(code)
        Controls.Add(DataGridView1)
        Controls.Add(txtRechercheCode)
        Controls.Add(btnAnnuler)
        Controls.Add(btnSelectionner)
        Name = "FrmRecherche"
        Text = "FrmRecherche"
        CType(DataGridView1, ComponentModel.ISupportInitialize).EndInit()
        ResumeLayout(False)
        PerformLayout()
    End Sub

    Friend WithEvents btnSelectionner As Button
    Friend WithEvents btnAnnuler As Button
    Friend WithEvents txtRechercheCode As TextBox
    Friend WithEvents DataGridView1 As DataGridView
    Friend WithEvents code As Label
    Friend WithEvents Label1 As Label
    Friend WithEvents lblNombre As Label
    Friend WithEvents txtRechercheNom As TextBox
End Class
