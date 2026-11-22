<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class FrmLicence
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
        txtCodeActivation = New TextBox()
        btnActiver = New Button()
        btnQuitter = New Button()
        lblStatut = New Label()
        lblClient = New Label()
        lblExpiration = New Label()
        txtCodeMachine = New TextBox()
        SuspendLayout()
        ' 
        ' txtCodeActivation
        ' 
        txtCodeActivation.Location = New Point(98, 152)
        txtCodeActivation.Name = "txtCodeActivation"
        txtCodeActivation.Size = New Size(218, 27)
        txtCodeActivation.TabIndex = 5
        txtCodeActivation.UseSystemPasswordChar = True
        ' 
        ' btnActiver
        ' 
        btnActiver.Font = New Font("Segoe UI Historic", 16.2F, FontStyle.Bold, GraphicsUnit.Point, CByte(0))
        btnActiver.Location = New Point(12, 227)
        btnActiver.Name = "btnActiver"
        btnActiver.Size = New Size(161, 74)
        btnActiver.TabIndex = 3
        btnActiver.Text = "Activer"
        btnActiver.UseVisualStyleBackColor = True
        ' 
        ' btnQuitter
        ' 
        btnQuitter.Font = New Font("Segoe UI Historic", 16.2F, FontStyle.Bold, GraphicsUnit.Point, CByte(0))
        btnQuitter.Location = New Point(241, 227)
        btnQuitter.Name = "btnQuitter"
        btnQuitter.Size = New Size(161, 74)
        btnQuitter.TabIndex = 6
        btnQuitter.Text = "Quitter"
        btnQuitter.UseVisualStyleBackColor = True
        ' 
        ' lblStatut
        ' 
        lblStatut.AutoSize = True
        lblStatut.Location = New Point(98, 9)
        lblStatut.Name = "lblStatut"
        lblStatut.Size = New Size(53, 20)
        lblStatut.TabIndex = 7
        lblStatut.Text = "Label1"
        ' 
        ' lblClient
        ' 
        lblClient.AutoSize = True
        lblClient.Location = New Point(98, 41)
        lblClient.Name = "lblClient"
        lblClient.Size = New Size(53, 20)
        lblClient.TabIndex = 8
        lblClient.Text = "Label2"
        ' 
        ' lblExpiration
        ' 
        lblExpiration.AutoSize = True
        lblExpiration.Location = New Point(98, 70)
        lblExpiration.Name = "lblExpiration"
        lblExpiration.Size = New Size(53, 20)
        lblExpiration.TabIndex = 9
        lblExpiration.Text = "Label3"
        ' 
        ' txtCodeMachine
        ' 
        txtCodeMachine.Location = New Point(98, 103)
        txtCodeMachine.Name = "txtCodeMachine"
        txtCodeMachine.ReadOnly = True
        txtCodeMachine.Size = New Size(218, 27)
        txtCodeMachine.TabIndex = 10
        txtCodeMachine.UseSystemPasswordChar = True
        ' 
        ' FrmLicence
        ' 
        AutoScaleDimensions = New SizeF(8F, 20F)
        AutoScaleMode = AutoScaleMode.Font
        ClientSize = New Size(414, 339)
        Controls.Add(txtCodeMachine)
        Controls.Add(lblExpiration)
        Controls.Add(lblClient)
        Controls.Add(lblStatut)
        Controls.Add(btnQuitter)
        Controls.Add(txtCodeActivation)
        Controls.Add(btnActiver)
        Name = "FrmLicence"
        Text = "FrmLicence"
        ResumeLayout(False)
        PerformLayout()
    End Sub

    Friend WithEvents txtCodeActivation As TextBox
    Friend WithEvents btnActiver As Button
    Friend WithEvents btnQuitter As Button
    Friend WithEvents lblStatut As Label
    Friend WithEvents lblClient As Label
    Friend WithEvents lblExpiration As Label
    Friend WithEvents txtCodeMachine As TextBox
End Class
