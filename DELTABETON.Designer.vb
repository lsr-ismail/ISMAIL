<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class DELTABETON
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
        Label3 = New Label()
        Label2 = New Label()
        Label1 = New Label()
        MotDePasse = New TextBox()
        NomUtilisateur = New TextBox()
        EDITE = New Button()
        SUPRIME = New Button()
        AJOUTE = New Button()
        DataGridView1 = New DataGridView()
        Label21 = New Label()
        RoleUtilisateu = New ComboBox()
        CType(DataGridView1, ComponentModel.ISupportInitialize).BeginInit()
        SuspendLayout()
        ' 
        ' Label3
        ' 
        Label3.AutoSize = True
        Label3.Location = New Point(47, 191)
        Label3.Name = "Label3"
        Label3.Size = New Size(110, 20)
        Label3.TabIndex = 39
        Label3.Text = "Rôle Utilisateur"
        ' 
        ' Label2
        ' 
        Label2.AutoSize = True
        Label2.Location = New Point(47, 158)
        Label2.Name = "Label2"
        Label2.Size = New Size(98, 20)
        Label2.TabIndex = 38
        Label2.Text = "Mot De Passe"
        ' 
        ' Label1
        ' 
        Label1.AutoSize = True
        Label1.Location = New Point(47, 125)
        Label1.Name = "Label1"
        Label1.Size = New Size(113, 20)
        Label1.TabIndex = 37
        Label1.Text = "Nom Utilisateur"
        ' 
        ' MotDePasse
        ' 
        MotDePasse.Location = New Point(163, 154)
        MotDePasse.Name = "MotDePasse"
        MotDePasse.Size = New Size(418, 27)
        MotDePasse.TabIndex = 35
        ' 
        ' NomUtilisateur
        ' 
        NomUtilisateur.Location = New Point(163, 121)
        NomUtilisateur.Name = "NomUtilisateur"
        NomUtilisateur.Size = New Size(418, 27)
        NomUtilisateur.TabIndex = 33
        ' 
        ' EDITE
        ' 
        EDITE.Location = New Point(409, 41)
        EDITE.Name = "EDITE"
        EDITE.Size = New Size(178, 61)
        EDITE.TabIndex = 32
        EDITE.Text = "EDITE"
        EDITE.UseVisualStyleBackColor = True
        ' 
        ' SUPRIME
        ' 
        SUPRIME.Location = New Point(225, 41)
        SUPRIME.Name = "SUPRIME"
        SUPRIME.Size = New Size(178, 61)
        SUPRIME.TabIndex = 31
        SUPRIME.Text = "SUPRIME"
        SUPRIME.UseVisualStyleBackColor = True
        ' 
        ' AJOUTE
        ' 
        AJOUTE.Location = New Point(37, 41)
        AJOUTE.Name = "AJOUTE"
        AJOUTE.Size = New Size(178, 61)
        AJOUTE.TabIndex = 30
        AJOUTE.Text = "AJOUTER"
        AJOUTE.UseVisualStyleBackColor = True
        ' 
        ' DataGridView1
        ' 
        DataGridView1.AllowUserToAddRows = False
        DataGridView1.Anchor = AnchorStyles.Top Or AnchorStyles.Bottom Or AnchorStyles.Left Or AnchorStyles.Right
        DataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill
        DataGridView1.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize
        DataGridView1.Location = New Point(26, 220)
        DataGridView1.MultiSelect = False
        DataGridView1.Name = "DataGridView1"
        DataGridView1.ReadOnly = True
        DataGridView1.RowHeadersWidth = 51
        DataGridView1.SelectionMode = DataGridViewSelectionMode.FullRowSelect
        DataGridView1.Size = New Size(555, 390)
        DataGridView1.TabIndex = 40
        ' 
        ' Label21
        ' 
        Label21.AutoSize = True
        Label21.Font = New Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point, CByte(0))
        Label21.Location = New Point(12, 9)
        Label21.Name = "Label21"
        Label21.Size = New Size(126, 28)
        Label21.TabIndex = 312
        Label21.Text = "DELTABETON"
        ' 
        ' RoleUtilisateu
        ' 
        RoleUtilisateu.DropDownStyle = ComboBoxStyle.DropDownList
        RoleUtilisateu.FormattingEnabled = True
        RoleUtilisateu.Location = New Point(163, 186)
        RoleUtilisateu.Name = "RoleUtilisateu"
        RoleUtilisateu.Size = New Size(418, 28)
        RoleUtilisateu.TabIndex = 313
        ' 
        ' DELTABETON
        ' 
        AutoScaleDimensions = New SizeF(8F, 20F)
        AutoScaleMode = AutoScaleMode.Font
        ClientSize = New Size(616, 622)
        Controls.Add(RoleUtilisateu)
        Controls.Add(Label21)
        Controls.Add(DataGridView1)
        Controls.Add(Label3)
        Controls.Add(Label2)
        Controls.Add(Label1)
        Controls.Add(MotDePasse)
        Controls.Add(NomUtilisateur)
        Controls.Add(EDITE)
        Controls.Add(SUPRIME)
        Controls.Add(AJOUTE)
        Name = "DELTABETON"
        Text = "DELTABETON"
        CType(DataGridView1, ComponentModel.ISupportInitialize).EndInit()
        ResumeLayout(False)
        PerformLayout()
    End Sub

    Friend WithEvents Label3 As Label
    Friend WithEvents Label2 As Label
    Friend WithEvents Label1 As Label
    Friend WithEvents MotDePasse As TextBox
    Friend WithEvents NomUtilisateur As TextBox
    Friend WithEvents EDITE As Button
    Friend WithEvents SUPRIME As Button
    Friend WithEvents AJOUTE As Button
    Friend WithEvents DataGridView1 As DataGridView
    Friend WithEvents routeur As Button
    Friend WithEvents Label21 As Label
    Friend WithEvents RoleUtilisateu As ComboBox
End Class
