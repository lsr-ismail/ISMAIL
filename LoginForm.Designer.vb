<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class LoginForm
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
        connecter = New Button()
        motdepasse = New TextBox()
        utilisateur = New ComboBox()
        SuspendLayout()
        ' 
        ' connecter
        ' 
        connecter.Font = New Font("Segoe UI Historic", 16.2F, FontStyle.Bold, GraphicsUnit.Point, CByte(0))
        connecter.Location = New Point(35, 92)
        connecter.Name = "connecter"
        connecter.Size = New Size(217, 93)
        connecter.TabIndex = 0
        connecter.Text = "Connecter"
        connecter.UseVisualStyleBackColor = True
        ' 
        ' motdepasse
        ' 
        motdepasse.Location = New Point(35, 59)
        motdepasse.Name = "motdepasse"
        motdepasse.Size = New Size(218, 27)
        motdepasse.TabIndex = 2
        motdepasse.UseSystemPasswordChar = True
        ' 
        ' utilisateur
        ' 
        utilisateur.DropDownStyle = ComboBoxStyle.DropDownList
        utilisateur.Location = New Point(35, 26)
        utilisateur.Name = "utilisateur"
        utilisateur.Size = New Size(218, 28)
        utilisateur.TabIndex = 1
        ' 
        ' LoginForm
        ' 
        AutoScaleDimensions = New SizeF(8F, 20F)
        AutoScaleMode = AutoScaleMode.Font
        ClientSize = New Size(288, 219)
        Controls.Add(utilisateur)
        Controls.Add(motdepasse)
        Controls.Add(connecter)
        Name = "LoginForm"
        Text = "LoginForm"
        ResumeLayout(False)
        PerformLayout()
    End Sub

    Friend WithEvents connecter As Button
    Friend WithEvents motdepasse As TextBox
    Friend WithEvents utilisateur As ComboBox
End Class
