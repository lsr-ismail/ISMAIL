<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class Zones
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(Zones))
        txtRECHERCHE = New TextBox()
        btnActualiser = New Button()
        btnNouveau = New Button()
        Label11 = New Label()
        Label12 = New Label()
        Label4 = New Label()
        Label5 = New Label()
        Label6 = New Label()
        Label3 = New Label()
        Label2 = New Label()
        Label1 = New Label()
        Imprimer = New Button()
        txtDerniereModification = New TextBox()
        txtNumeroZone = New TextBox()
        txtCodePostal = New TextBox()
        txtTypeZone = New TextBox()
        txtNomZone = New TextBox()
        txtCleZone = New TextBox()
        txtCodeZone = New TextBox()
        DataGridView1 = New DataGridView()
        txtIdZone = New TextBox()
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
        ' txtRECHERCHE
        ' 
        txtRECHERCHE.Location = New Point(229, 138)
        txtRECHERCHE.Name = "txtRECHERCHE"
        txtRECHERCHE.Size = New Size(280, 27)
        txtRECHERCHE.TabIndex = 142
        ' 
        ' btnActualiser
        ' 
        btnActualiser.Location = New Point(923, 54)
        btnActualiser.Name = "btnActualiser"
        btnActualiser.Size = New Size(145, 61)
        btnActualiser.TabIndex = 141
        btnActualiser.Text = "Actualiser"
        btnActualiser.UseVisualStyleBackColor = True
        ' 
        ' btnNouveau
        ' 
        btnNouveau.Location = New Point(772, 54)
        btnNouveau.Name = "btnNouveau"
        btnNouveau.Size = New Size(145, 61)
        btnNouveau.TabIndex = 140
        btnNouveau.Text = "Nouveau"
        btnNouveau.UseVisualStyleBackColor = True
        ' 
        ' Label11
        ' 
        Label11.AutoSize = True
        Label11.Location = New Point(71, 405)
        Label11.Name = "Label11"
        Label11.Size = New Size(155, 20)
        Label11.TabIndex = 136
        Label11.Text = "Derniere Modification"
        ' 
        ' Label12
        ' 
        Label12.AutoSize = True
        Label12.Location = New Point(73, 372)
        Label12.Name = "Label12"
        Label12.Size = New Size(101, 20)
        Label12.TabIndex = 135
        Label12.Text = "Numero Zone"
        ' 
        ' Label4
        ' 
        Label4.AutoSize = True
        Label4.Location = New Point(71, 339)
        Label4.Name = "Label4"
        Label4.Size = New Size(87, 20)
        Label4.TabIndex = 134
        Label4.Text = "Code Postal"
        ' 
        ' Label5
        ' 
        Label5.AutoSize = True
        Label5.Location = New Point(71, 306)
        Label5.Name = "Label5"
        Label5.Size = New Size(78, 20)
        Label5.TabIndex = 133
        Label5.Text = "Type Zone"
        ' 
        ' Label6
        ' 
        Label6.AutoSize = True
        Label6.Location = New Point(68, 273)
        Label6.Name = "Label6"
        Label6.Size = New Size(80, 20)
        Label6.TabIndex = 132
        Label6.Text = "Nom Zone"
        ' 
        ' Label3
        ' 
        Label3.AutoSize = True
        Label3.Location = New Point(71, 240)
        Label3.Name = "Label3"
        Label3.Size = New Size(68, 20)
        Label3.TabIndex = 131
        Label3.Text = "Cle Zone"
        ' 
        ' Label2
        ' 
        Label2.AutoSize = True
        Label2.Location = New Point(71, 207)
        Label2.Name = "Label2"
        Label2.Size = New Size(82, 20)
        Label2.TabIndex = 130
        Label2.Text = "Code Zone"
        ' 
        ' Label1
        ' 
        Label1.AutoSize = True
        Label1.Location = New Point(71, 174)
        Label1.Name = "Label1"
        Label1.Size = New Size(60, 20)
        Label1.TabIndex = 129
        Label1.Text = "Id Zone"
        ' 
        ' Imprimer
        ' 
        Imprimer.Location = New Point(622, 54)
        Imprimer.Name = "Imprimer"
        Imprimer.Size = New Size(145, 61)
        Imprimer.TabIndex = 128
        Imprimer.Text = "Imprimer"
        Imprimer.UseVisualStyleBackColor = True
        ' 
        ' txtDerniereModification
        ' 
        txtDerniereModification.Location = New Point(229, 401)
        txtDerniereModification.Name = "txtDerniereModification"
        txtDerniereModification.ReadOnly = True
        txtDerniereModification.Size = New Size(280, 27)
        txtDerniereModification.TabIndex = 124
        ' 
        ' txtNumeroZone
        ' 
        txtNumeroZone.Location = New Point(229, 368)
        txtNumeroZone.Name = "txtNumeroZone"
        txtNumeroZone.Size = New Size(280, 27)
        txtNumeroZone.TabIndex = 123
        ' 
        ' txtCodePostal
        ' 
        txtCodePostal.Location = New Point(229, 335)
        txtCodePostal.Name = "txtCodePostal"
        txtCodePostal.Size = New Size(280, 27)
        txtCodePostal.TabIndex = 122
        ' 
        ' txtTypeZone
        ' 
        txtTypeZone.Location = New Point(229, 302)
        txtTypeZone.Name = "txtTypeZone"
        txtTypeZone.Size = New Size(280, 27)
        txtTypeZone.TabIndex = 121
        ' 
        ' txtNomZone
        ' 
        txtNomZone.Location = New Point(229, 269)
        txtNomZone.Name = "txtNomZone"
        txtNomZone.Size = New Size(280, 27)
        txtNomZone.TabIndex = 120
        ' 
        ' txtCleZone
        ' 
        txtCleZone.Location = New Point(229, 236)
        txtCleZone.Name = "txtCleZone"
        txtCleZone.Size = New Size(280, 27)
        txtCleZone.TabIndex = 119
        ' 
        ' txtCodeZone
        ' 
        txtCodeZone.Location = New Point(231, 205)
        txtCodeZone.Name = "txtCodeZone"
        txtCodeZone.Size = New Size(280, 27)
        txtCodeZone.TabIndex = 118
        ' 
        ' DataGridView1
        ' 
        DataGridView1.AllowUserToAddRows = False
        DataGridView1.Anchor = AnchorStyles.Top Or AnchorStyles.Bottom Or AnchorStyles.Left Or AnchorStyles.Right
        DataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill
        DataGridView1.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize
        DataGridView1.Location = New Point(524, 138)
        DataGridView1.MultiSelect = False
        DataGridView1.Name = "DataGridView1"
        DataGridView1.ReadOnly = True
        DataGridView1.RowHeadersWidth = 51
        DataGridView1.SelectionMode = DataGridViewSelectionMode.FullRowSelect
        DataGridView1.Size = New Size(546, 455)
        DataGridView1.TabIndex = 117
        ' 
        ' txtIdZone
        ' 
        txtIdZone.Location = New Point(229, 170)
        txtIdZone.Name = "txtIdZone"
        txtIdZone.ReadOnly = True
        txtIdZone.Size = New Size(280, 27)
        txtIdZone.TabIndex = 116
        ' 
        ' btnModifier
        ' 
        btnModifier.Location = New Point(472, 54)
        btnModifier.Name = "btnModifier"
        btnModifier.Size = New Size(145, 61)
        btnModifier.TabIndex = 115
        btnModifier.Text = "EDITE"
        btnModifier.UseVisualStyleBackColor = True
        ' 
        ' RECHERCHE
        ' 
        RECHERCHE.Location = New Point(321, 54)
        RECHERCHE.Name = "RECHERCHE"
        RECHERCHE.Size = New Size(145, 61)
        RECHERCHE.TabIndex = 114
        RECHERCHE.Text = "RECHERCHE"
        RECHERCHE.UseVisualStyleBackColor = True
        ' 
        ' btnSupprimer
        ' 
        btnSupprimer.Location = New Point(171, 54)
        btnSupprimer.Name = "btnSupprimer"
        btnSupprimer.Size = New Size(145, 61)
        btnSupprimer.TabIndex = 113
        btnSupprimer.Text = "SUPRIME"
        btnSupprimer.UseVisualStyleBackColor = True
        ' 
        ' btnAjouter
        ' 
        btnAjouter.Location = New Point(21, 54)
        btnAjouter.Name = "btnAjouter"
        btnAjouter.Size = New Size(145, 61)
        btnAjouter.TabIndex = 112
        btnAjouter.Text = "AJOUTE"
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
        Label21.Location = New Point(12, 9)
        Label21.Name = "Label21"
        Label21.Size = New Size(64, 28)
        Label21.TabIndex = 150
        Label21.Text = "Zones"
        ' 
        ' Zones
        ' 
        AutoScaleDimensions = New SizeF(8F, 20F)
        AutoScaleMode = AutoScaleMode.Font
        ClientSize = New Size(1082, 723)
        Controls.Add(Label21)
        Controls.Add(txtRECHERCHE)
        Controls.Add(btnActualiser)
        Controls.Add(btnNouveau)
        Controls.Add(Label11)
        Controls.Add(Label12)
        Controls.Add(Label4)
        Controls.Add(Label5)
        Controls.Add(Label6)
        Controls.Add(Label3)
        Controls.Add(Label2)
        Controls.Add(Label1)
        Controls.Add(Imprimer)
        Controls.Add(txtDerniereModification)
        Controls.Add(txtNumeroZone)
        Controls.Add(txtCodePostal)
        Controls.Add(txtTypeZone)
        Controls.Add(txtNomZone)
        Controls.Add(txtCleZone)
        Controls.Add(txtCodeZone)
        Controls.Add(DataGridView1)
        Controls.Add(txtIdZone)
        Controls.Add(btnModifier)
        Controls.Add(RECHERCHE)
        Controls.Add(btnSupprimer)
        Controls.Add(btnAjouter)
        Name = "Zones"
        Text = "Zones"
        CType(DataGridView1, ComponentModel.ISupportInitialize).EndInit()
        ResumeLayout(False)
        PerformLayout()
    End Sub
    Friend WithEvents txtRECHERCHE As TextBox
    Friend WithEvents btnActualiser As Button
    Friend WithEvents btnNouveau As Button
    Friend WithEvents Label11 As Label
    Friend WithEvents Label12 As Label
    Friend WithEvents Label4 As Label
    Friend WithEvents Label5 As Label
    Friend WithEvents Label6 As Label
    Friend WithEvents Label3 As Label
    Friend WithEvents Label2 As Label
    Friend WithEvents Label1 As Label
    Friend WithEvents Imprimer As Button
    Friend WithEvents txtDerniereModification As TextBox
    Friend WithEvents txtNumeroZone As TextBox
    Friend WithEvents txtCodePostal As TextBox
    Friend WithEvents txtTypeZone As TextBox
    Friend WithEvents txtNomZone As TextBox
    Friend WithEvents txtCleZone As TextBox
    Friend WithEvents txtCodeZone As TextBox
    Friend WithEvents DataGridView1 As DataGridView
    Friend WithEvents txtIdZone As TextBox
    Friend WithEvents btnModifier As Button
    Friend WithEvents RECHERCHE As Button
    Friend WithEvents btnSupprimer As Button
    Friend WithEvents btnAjouter As Button
    Friend WithEvents PrintDocument1 As Printing.PrintDocument
    Friend WithEvents PrintPreviewDialog1 As PrintPreviewDialog
    Friend WithEvents Label21 As Label
End Class
