<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class taxes
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(taxes))
        PrintDocument1 = New Printing.PrintDocument()
        PrintPreviewDialog1 = New PrintPreviewDialog()
        btnActualiser = New Button()
        btnNouveau = New Button()
        Label5 = New Label()
        Label6 = New Label()
        Label3 = New Label()
        Label2 = New Label()
        Label1 = New Label()
        Imprimer = New Button()
        txtCompteComptable = New TextBox()
        txtTaux = New TextBox()
        txtNomTaxe = New TextBox()
        DataGridView1 = New DataGridView()
        txtCodeTaxe = New TextBox()
        btnModifier = New Button()
        RECHERCHE = New Button()
        btnSupprimer = New Button()
        btnAjouter = New Button()
        Label21 = New Label()
        chkActif = New CheckBox()
        Label4 = New Label()
        dtpDerniereModification = New TextBox()
        txtRECHERCHE = New TextBox()
        CType(DataGridView1, ComponentModel.ISupportInitialize).BeginInit()
        SuspendLayout()
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
        ' btnActualiser
        ' 
        btnActualiser.Location = New Point(822, 43)
        btnActualiser.Name = "btnActualiser"
        btnActualiser.Size = New Size(129, 61)
        btnActualiser.TabIndex = 174
        btnActualiser.Text = "Actualiser"
        btnActualiser.UseVisualStyleBackColor = True
        ' 
        ' btnNouveau
        ' 
        btnNouveau.Location = New Point(687, 43)
        btnNouveau.Name = "btnNouveau"
        btnNouveau.Size = New Size(129, 61)
        btnNouveau.TabIndex = 173
        btnNouveau.Text = "Nouveau"
        btnNouveau.UseVisualStyleBackColor = True
        ' 
        ' Label5
        ' 
        Label5.AutoSize = True
        Label5.Location = New Point(31, 290)
        Label5.Name = "Label5"
        Label5.Size = New Size(40, 20)
        Label5.TabIndex = 166
        Label5.Text = "Actif"
        ' 
        ' Label6
        ' 
        Label6.AutoSize = True
        Label6.Location = New Point(28, 257)
        Label6.Name = "Label6"
        Label6.Size = New Size(140, 20)
        Label6.TabIndex = 165
        Label6.Text = "Compte Comptable"
        ' 
        ' Label3
        ' 
        Label3.AutoSize = True
        Label3.Location = New Point(31, 224)
        Label3.Name = "Label3"
        Label3.Size = New Size(38, 20)
        Label3.TabIndex = 164
        Label3.Text = "Taux"
        ' 
        ' Label2
        ' 
        Label2.AutoSize = True
        Label2.Location = New Point(31, 191)
        Label2.Name = "Label2"
        Label2.Size = New Size(75, 20)
        Label2.TabIndex = 163
        Label2.Text = "Nom Taxe"
        ' 
        ' Label1
        ' 
        Label1.AutoSize = True
        Label1.Location = New Point(31, 158)
        Label1.Name = "Label1"
        Label1.Size = New Size(77, 20)
        Label1.TabIndex = 162
        Label1.Text = "Code Taxe"
        ' 
        ' Imprimer
        ' 
        Imprimer.Location = New Point(552, 43)
        Imprimer.Name = "Imprimer"
        Imprimer.Size = New Size(129, 61)
        Imprimer.TabIndex = 161
        Imprimer.Text = "Imprimer"
        Imprimer.UseVisualStyleBackColor = True
        ' 
        ' txtCompteComptable
        ' 
        txtCompteComptable.Location = New Point(200, 250)
        txtCompteComptable.Name = "txtCompteComptable"
        txtCompteComptable.Size = New Size(235, 27)
        txtCompteComptable.TabIndex = 153
        ' 
        ' txtTaux
        ' 
        txtTaux.Location = New Point(200, 217)
        txtTaux.Name = "txtTaux"
        txtTaux.Size = New Size(235, 27)
        txtTaux.TabIndex = 152
        ' 
        ' txtNomTaxe
        ' 
        txtNomTaxe.Location = New Point(202, 186)
        txtNomTaxe.Name = "txtNomTaxe"
        txtNomTaxe.Size = New Size(235, 27)
        txtNomTaxe.TabIndex = 151
        ' 
        ' DataGridView1
        ' 
        DataGridView1.AllowUserToAddRows = False
        DataGridView1.Anchor = AnchorStyles.Top Or AnchorStyles.Bottom Or AnchorStyles.Left Or AnchorStyles.Right
        DataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill
        DataGridView1.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize
        DataGridView1.Location = New Point(441, 118)
        DataGridView1.MultiSelect = False
        DataGridView1.Name = "DataGridView1"
        DataGridView1.ReadOnly = True
        DataGridView1.RowHeadersWidth = 51
        DataGridView1.SelectionMode = DataGridViewSelectionMode.FullRowSelect
        DataGridView1.Size = New Size(539, 256)
        DataGridView1.TabIndex = 150
        ' 
        ' txtCodeTaxe
        ' 
        txtCodeTaxe.Location = New Point(200, 151)
        txtCodeTaxe.Name = "txtCodeTaxe"
        txtCodeTaxe.ReadOnly = True
        txtCodeTaxe.Size = New Size(235, 27)
        txtCodeTaxe.TabIndex = 149
        ' 
        ' btnModifier
        ' 
        btnModifier.Location = New Point(417, 43)
        btnModifier.Name = "btnModifier"
        btnModifier.Size = New Size(129, 61)
        btnModifier.TabIndex = 148
        btnModifier.Text = "EDITE"
        btnModifier.UseVisualStyleBackColor = True
        ' 
        ' RECHERCHE
        ' 
        RECHERCHE.Location = New Point(282, 43)
        RECHERCHE.Name = "RECHERCHE"
        RECHERCHE.Size = New Size(129, 61)
        RECHERCHE.TabIndex = 147
        RECHERCHE.Text = "RECHERCHE"
        RECHERCHE.UseVisualStyleBackColor = True
        ' 
        ' btnSupprimer
        ' 
        btnSupprimer.Location = New Point(147, 43)
        btnSupprimer.Name = "btnSupprimer"
        btnSupprimer.Size = New Size(129, 61)
        btnSupprimer.TabIndex = 146
        btnSupprimer.Text = "SUPRIME"
        btnSupprimer.UseVisualStyleBackColor = True
        ' 
        ' btnAjouter
        ' 
        btnAjouter.Location = New Point(12, 43)
        btnAjouter.Name = "btnAjouter"
        btnAjouter.Size = New Size(129, 61)
        btnAjouter.TabIndex = 145
        btnAjouter.Text = "AJOUTER"
        btnAjouter.UseVisualStyleBackColor = True
        ' 
        ' Label21
        ' 
        Label21.AutoSize = True
        Label21.Font = New Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point, CByte(0))
        Label21.Location = New Point(12, 12)
        Label21.Name = "Label21"
        Label21.Size = New Size(56, 28)
        Label21.TabIndex = 178
        Label21.Text = "taxes"
        ' 
        ' chkActif
        ' 
        chkActif.AutoSize = True
        chkActif.Location = New Point(305, 290)
        chkActif.Name = "chkActif"
        chkActif.Size = New Size(18, 17)
        chkActif.TabIndex = 179
        chkActif.UseVisualStyleBackColor = True
        ' 
        ' Label4
        ' 
        Label4.AutoSize = True
        Label4.Location = New Point(30, 316)
        Label4.Name = "Label4"
        Label4.Size = New Size(155, 20)
        Label4.TabIndex = 181
        Label4.Text = "Dernière Modification"
        ' 
        ' dtpDerniereModification
        ' 
        dtpDerniereModification.Location = New Point(202, 309)
        dtpDerniereModification.Name = "dtpDerniereModification"
        dtpDerniereModification.Size = New Size(235, 27)
        dtpDerniereModification.TabIndex = 180
        ' 
        ' txtRECHERCHE
        ' 
        txtRECHERCHE.Location = New Point(200, 118)
        txtRECHERCHE.Name = "txtRECHERCHE"
        txtRECHERCHE.Size = New Size(235, 27)
        txtRECHERCHE.TabIndex = 182
        ' 
        ' taxes
        ' 
        AutoScaleDimensions = New SizeF(8F, 20F)
        AutoScaleMode = AutoScaleMode.Font
        ClientSize = New Size(987, 386)
        Controls.Add(txtRECHERCHE)
        Controls.Add(Label4)
        Controls.Add(dtpDerniereModification)
        Controls.Add(chkActif)
        Controls.Add(Label21)
        Controls.Add(btnActualiser)
        Controls.Add(btnNouveau)
        Controls.Add(Label5)
        Controls.Add(Label6)
        Controls.Add(Label3)
        Controls.Add(Label2)
        Controls.Add(Label1)
        Controls.Add(Imprimer)
        Controls.Add(txtCompteComptable)
        Controls.Add(txtTaux)
        Controls.Add(txtNomTaxe)
        Controls.Add(DataGridView1)
        Controls.Add(txtCodeTaxe)
        Controls.Add(btnModifier)
        Controls.Add(RECHERCHE)
        Controls.Add(btnSupprimer)
        Controls.Add(btnAjouter)
        Name = "taxes"
        Text = "taxes"
        CType(DataGridView1, ComponentModel.ISupportInitialize).EndInit()
        ResumeLayout(False)
        PerformLayout()
    End Sub

    Friend WithEvents PrintDocument1 As Printing.PrintDocument
    Friend WithEvents PrintPreviewDialog1 As PrintPreviewDialog
    Friend WithEvents btnActualiser As Button
    Friend WithEvents btnNouveau As Button
    Friend WithEvents Label5 As Label
    Friend WithEvents Label6 As Label
    Friend WithEvents Label3 As Label
    Friend WithEvents Label2 As Label
    Friend WithEvents Label1 As Label
    Friend WithEvents Imprimer As Button
    Friend WithEvents txtCompteComptable As TextBox
    Friend WithEvents txtTaux As TextBox
    Friend WithEvents txtNomTaxe As TextBox
    Friend WithEvents DataGridView1 As DataGridView
    Friend WithEvents txtCodeTaxe As TextBox
    Friend WithEvents btnModifier As Button
    Friend WithEvents RECHERCHE As Button
    Friend WithEvents btnSupprimer As Button
    Friend WithEvents btnAjouter As Button
    Friend WithEvents Label21 As Label
    Friend WithEvents chkActif As CheckBox
    Friend WithEvents Label4 As Label
    Friend WithEvents dtpDerniereModification As TextBox
    Friend WithEvents txtRECHERCHE As TextBox
End Class
