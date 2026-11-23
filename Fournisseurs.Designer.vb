<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class Fournisseurs
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(Fournisseurs))
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
        txtDerniereModification = New TextBox()
        txtService = New TextBox()
        txtTransport = New TextBox()
        txtMatierePremiere = New TextBox()
        txtCodePostal = New TextBox()
        txtVille = New TextBox()
        txtRue = New TextBox()
        txtNomFournisseur = New TextBox()
        txtCleFournisseur = New TextBox()
        txtCodeFournisseur = New TextBox()
        DataGridView1 = New DataGridView()
        txtIdFournisseur = New TextBox()
        btnModifier = New Button()
        RECHERCHE = New Button()
        btnSupprimer = New Button()
        btnAjouter = New Button()
        btnNouveau = New Button()
        btnActualiser = New Button()
        PrintDocument1 = New Printing.PrintDocument()
        PrintPreviewDialog1 = New PrintPreviewDialog()
        txtRECHERCHE = New TextBox()
        Label21 = New Label()
        CType(DataGridView1, ComponentModel.ISupportInitialize).BeginInit()
        SuspendLayout()
        ' 
        ' Label8
        ' 
        Label8.AutoSize = True
        Label8.Location = New Point(31, 476)
        Label8.Name = "Label8"
        Label8.Size = New Size(155, 20)
        Label8.TabIndex = 73
        Label8.Text = "Dernière Modification"
        ' 
        ' Label9
        ' 
        Label9.AutoSize = True
        Label9.Location = New Point(33, 443)
        Label9.Name = "Label9"
        Label9.Size = New Size(56, 20)
        Label9.TabIndex = 72
        Label9.Text = "Service"
        ' 
        ' Label10
        ' 
        Label10.AutoSize = True
        Label10.Location = New Point(36, 410)
        Label10.Name = "Label10"
        Label10.Size = New Size(71, 20)
        Label10.TabIndex = 71
        Label10.Text = "Transport"
        ' 
        ' Label11
        ' 
        Label11.AutoSize = True
        Label11.Location = New Point(34, 377)
        Label11.Name = "Label11"
        Label11.Size = New Size(123, 20)
        Label11.TabIndex = 70
        Label11.Text = "Matière Première"
        ' 
        ' Label12
        ' 
        Label12.AutoSize = True
        Label12.Location = New Point(36, 344)
        Label12.Name = "Label12"
        Label12.Size = New Size(83, 20)
        Label12.TabIndex = 69
        Label12.Text = "CodePostal"
        ' 
        ' Label4
        ' 
        Label4.AutoSize = True
        Label4.Location = New Point(34, 311)
        Label4.Name = "Label4"
        Label4.Size = New Size(38, 20)
        Label4.TabIndex = 68
        Label4.Text = "Ville"
        ' 
        ' Label5
        ' 
        Label5.AutoSize = True
        Label5.Location = New Point(34, 281)
        Label5.Name = "Label5"
        Label5.Size = New Size(34, 20)
        Label5.TabIndex = 67
        Label5.Text = "Rue"
        ' 
        ' Label6
        ' 
        Label6.AutoSize = True
        Label6.Location = New Point(31, 245)
        Label6.Name = "Label6"
        Label6.Size = New Size(120, 20)
        Label6.TabIndex = 66
        Label6.Text = "Nom Fournisseur"
        ' 
        ' Label3
        ' 
        Label3.AutoSize = True
        Label3.Location = New Point(34, 212)
        Label3.Name = "Label3"
        Label3.Size = New Size(108, 20)
        Label3.TabIndex = 65
        Label3.Text = "Cle Fournisseur"
        ' 
        ' Label2
        ' 
        Label2.AutoSize = True
        Label2.Location = New Point(34, 179)
        Label2.Name = "Label2"
        Label2.Size = New Size(122, 20)
        Label2.TabIndex = 64
        Label2.Text = "Code Fournisseur"
        ' 
        ' Label1
        ' 
        Label1.AutoSize = True
        Label1.Location = New Point(34, 146)
        Label1.Name = "Label1"
        Label1.Size = New Size(100, 20)
        Label1.TabIndex = 63
        Label1.Text = "Id Fournisseur"
        ' 
        ' Imprimer
        ' 
        Imprimer.Location = New Point(621, 36)
        Imprimer.Name = "Imprimer"
        Imprimer.Size = New Size(137, 61)
        Imprimer.TabIndex = 62
        Imprimer.Text = "Imprimer"
        Imprimer.UseVisualStyleBackColor = True
        ' 
        ' txtDerniereModification
        ' 
        txtDerniereModification.Location = New Point(192, 472)
        txtDerniereModification.Name = "txtDerniereModification"
        txtDerniereModification.ReadOnly = True
        txtDerniereModification.Size = New Size(292, 27)
        txtDerniereModification.TabIndex = 56
        ' 
        ' txtService
        ' 
        txtService.Location = New Point(192, 439)
        txtService.Name = "txtService"
        txtService.Size = New Size(292, 27)
        txtService.TabIndex = 55
        ' 
        ' txtTransport
        ' 
        txtTransport.Location = New Point(192, 406)
        txtTransport.Name = "txtTransport"
        txtTransport.Size = New Size(292, 27)
        txtTransport.TabIndex = 54
        ' 
        ' txtMatierePremiere
        ' 
        txtMatierePremiere.Location = New Point(192, 373)
        txtMatierePremiere.Name = "txtMatierePremiere"
        txtMatierePremiere.Size = New Size(292, 27)
        txtMatierePremiere.TabIndex = 53
        ' 
        ' txtCodePostal
        ' 
        txtCodePostal.Location = New Point(192, 340)
        txtCodePostal.Name = "txtCodePostal"
        txtCodePostal.Size = New Size(292, 27)
        txtCodePostal.TabIndex = 52
        ' 
        ' txtVille
        ' 
        txtVille.Location = New Point(192, 307)
        txtVille.Name = "txtVille"
        txtVille.Size = New Size(292, 27)
        txtVille.TabIndex = 51
        ' 
        ' txtRue
        ' 
        txtRue.Location = New Point(192, 274)
        txtRue.Name = "txtRue"
        txtRue.Size = New Size(292, 27)
        txtRue.TabIndex = 50
        ' 
        ' txtNomFournisseur
        ' 
        txtNomFournisseur.Location = New Point(192, 241)
        txtNomFournisseur.Name = "txtNomFournisseur"
        txtNomFournisseur.Size = New Size(292, 27)
        txtNomFournisseur.TabIndex = 49
        ' 
        ' txtCleFournisseur
        ' 
        txtCleFournisseur.Location = New Point(192, 208)
        txtCleFournisseur.Name = "txtCleFournisseur"
        txtCleFournisseur.Size = New Size(292, 27)
        txtCleFournisseur.TabIndex = 48
        ' 
        ' txtCodeFournisseur
        ' 
        txtCodeFournisseur.Location = New Point(194, 177)
        txtCodeFournisseur.Name = "txtCodeFournisseur"
        txtCodeFournisseur.Size = New Size(292, 27)
        txtCodeFournisseur.TabIndex = 47
        ' 
        ' DataGridView1
        ' 
        DataGridView1.AllowUserToAddRows = False
        DataGridView1.Anchor = AnchorStyles.Top Or AnchorStyles.Bottom Or AnchorStyles.Left Or AnchorStyles.Right
        DataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill
        DataGridView1.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize
        DataGridView1.Location = New Point(503, 116)
        DataGridView1.MultiSelect = False
        DataGridView1.Name = "DataGridView1"
        DataGridView1.ReadOnly = True
        DataGridView1.RowHeadersWidth = 51
        DataGridView1.SelectionMode = DataGridViewSelectionMode.FullRowSelect
        DataGridView1.Size = New Size(541, 383)
        DataGridView1.TabIndex = 46
        ' 
        ' txtIdFournisseur
        ' 
        txtIdFournisseur.Location = New Point(192, 142)
        txtIdFournisseur.Name = "txtIdFournisseur"
        txtIdFournisseur.ReadOnly = True
        txtIdFournisseur.Size = New Size(292, 27)
        txtIdFournisseur.TabIndex = 45
        ' 
        ' btnModifier
        ' 
        btnModifier.Location = New Point(478, 36)
        btnModifier.Name = "btnModifier"
        btnModifier.Size = New Size(137, 61)
        btnModifier.TabIndex = 44
        btnModifier.Text = "EDITE"
        btnModifier.UseVisualStyleBackColor = True
        ' 
        ' RECHERCHE
        ' 
        RECHERCHE.Location = New Point(335, 36)
        RECHERCHE.Name = "RECHERCHE"
        RECHERCHE.Size = New Size(137, 61)
        RECHERCHE.TabIndex = 43
        RECHERCHE.Text = "RECHERCHE"
        RECHERCHE.UseVisualStyleBackColor = True
        ' 
        ' btnSupprimer
        ' 
        btnSupprimer.Location = New Point(192, 36)
        btnSupprimer.Name = "btnSupprimer"
        btnSupprimer.Size = New Size(137, 61)
        btnSupprimer.TabIndex = 42
        btnSupprimer.Text = "SUPRIME"
        btnSupprimer.UseVisualStyleBackColor = True
        ' 
        ' btnAjouter
        ' 
        btnAjouter.Location = New Point(49, 36)
        btnAjouter.Name = "btnAjouter"
        btnAjouter.Size = New Size(137, 61)
        btnAjouter.TabIndex = 41
        btnAjouter.Text = "AJOUTER"
        btnAjouter.UseVisualStyleBackColor = True
        ' 
        ' btnNouveau
        ' 
        btnNouveau.Location = New Point(764, 36)
        btnNouveau.Name = "btnNouveau"
        btnNouveau.Size = New Size(137, 61)
        btnNouveau.TabIndex = 74
        btnNouveau.Text = "Nouveau"
        btnNouveau.UseVisualStyleBackColor = True
        ' 
        ' btnActualiser
        ' 
        btnActualiser.Location = New Point(907, 36)
        btnActualiser.Name = "btnActualiser"
        btnActualiser.Size = New Size(137, 61)
        btnActualiser.TabIndex = 75
        btnActualiser.Text = "Actualiser"
        btnActualiser.UseVisualStyleBackColor = True
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
        ' txtRECHERCHE
        ' 
        txtRECHERCHE.Location = New Point(192, 109)
        txtRECHERCHE.Name = "txtRECHERCHE"
        txtRECHERCHE.Size = New Size(292, 27)
        txtRECHERCHE.TabIndex = 76
        ' 
        ' Label21
        ' 
        Label21.AutoSize = True
        Label21.Font = New Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point, CByte(0))
        Label21.Location = New Point(12, 9)
        Label21.Name = "Label21"
        Label21.Size = New Size(120, 28)
        Label21.TabIndex = 179
        Label21.Text = "Fournisseurs"
        ' 
        ' Fournisseurs
        ' 
        AutoScaleDimensions = New SizeF(8F, 20F)
        AutoScaleMode = AutoScaleMode.Font
        ClientSize = New Size(1082, 537)
        Controls.Add(Label21)
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
        Controls.Add(txtDerniereModification)
        Controls.Add(txtService)
        Controls.Add(txtTransport)
        Controls.Add(txtMatierePremiere)
        Controls.Add(txtCodePostal)
        Controls.Add(txtVille)
        Controls.Add(txtRue)
        Controls.Add(txtNomFournisseur)
        Controls.Add(txtCleFournisseur)
        Controls.Add(txtCodeFournisseur)
        Controls.Add(DataGridView1)
        Controls.Add(txtIdFournisseur)
        Controls.Add(btnModifier)
        Controls.Add(RECHERCHE)
        Controls.Add(btnSupprimer)
        Controls.Add(btnAjouter)
        Name = "Fournisseurs"
        Text = "Fournisseurs"
        CType(DataGridView1, ComponentModel.ISupportInitialize).EndInit()
        ResumeLayout(False)
        PerformLayout()
    End Sub
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
    Friend WithEvents txtDerniereModification As TextBox
    Friend WithEvents txtService As TextBox
    Friend WithEvents txtTransport As TextBox
    Friend WithEvents txtMatierePremiere As TextBox
    Friend WithEvents txtCodePostal As TextBox
    Friend WithEvents txtVille As TextBox
    Friend WithEvents txtRue As TextBox
    Friend WithEvents txtNomFournisseur As TextBox
    Friend WithEvents txtCleFournisseur As TextBox
    Friend WithEvents txtCodeFournisseur As TextBox
    Friend WithEvents DataGridView1 As DataGridView
    Friend WithEvents txtIdFournisseur As TextBox
    Friend WithEvents btnModifier As Button
    Friend WithEvents RECHERCHE As Button
    Friend WithEvents btnSupprimer As Button
    Friend WithEvents btnAjouter As Button
    Friend WithEvents btnNouveau As Button
    Friend WithEvents btnActualiser As Button
    Friend WithEvents PrintDocument1 As Printing.PrintDocument
    Friend WithEvents PrintPreviewDialog1 As PrintPreviewDialog
    Friend WithEvents txtRECHERCHE As TextBox
    Friend WithEvents Label21 As Label
End Class
