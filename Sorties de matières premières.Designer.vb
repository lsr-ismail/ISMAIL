<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class Sorties_de_matières_premières
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
        Label21 = New Label()
        Label5 = New Label()
        txtRECHERCHE = New TextBox()
        Label4 = New Label()
        txtQuantite = New TextBox()
        Label3 = New Label()
        txtNomMat = New TextBox()
        Label2 = New Label()
        txtUnite = New TextBox()
        Label1 = New Label()
        txtStockActuel = New TextBox()
        btnActualiser = New Button()
        btnEnregistrer = New Button()
        lblNombre = New Label()
        Label6 = New Label()
        txtCodeMat = New TextBox()
        DataGridView1 = New DataGridView()
        CType(DataGridView1, ComponentModel.ISupportInitialize).BeginInit()
        SuspendLayout()
        ' 
        ' Label21
        ' 
        Label21.AutoSize = True
        Label21.Font = New Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point, CByte(0))
        Label21.Location = New Point(12, 9)
        Label21.Name = "Label21"
        Label21.Size = New Size(278, 28)
        Label21.TabIndex = 152
        Label21.Text = "Sorties de matières premières"
        ' 
        ' Label5
        ' 
        Label5.AutoSize = True
        Label5.Location = New Point(26, 100)
        Label5.Name = "Label5"
        Label5.Size = New Size(91, 20)
        Label5.TabIndex = 420
        Label5.Text = "RECHERCHE"
        ' 
        ' txtRECHERCHE
        ' 
        txtRECHERCHE.Location = New Point(129, 93)
        txtRECHERCHE.Name = "txtRECHERCHE"
        txtRECHERCHE.Size = New Size(326, 27)
        txtRECHERCHE.TabIndex = 419
        ' 
        ' Label4
        ' 
        Label4.AutoSize = True
        Label4.Location = New Point(26, 264)
        Label4.Name = "Label4"
        Label4.Size = New Size(66, 20)
        Label4.TabIndex = 418
        Label4.Text = "Quantité"
        ' 
        ' txtQuantite
        ' 
        txtQuantite.Location = New Point(130, 257)
        txtQuantite.Name = "txtQuantite"
        txtQuantite.Size = New Size(326, 27)
        txtQuantite.TabIndex = 417
        ' 
        ' Label3
        ' 
        Label3.AutoSize = True
        Label3.Location = New Point(26, 165)
        Label3.Name = "Label3"
        Label3.Size = New Size(72, 20)
        Label3.TabIndex = 416
        Label3.Text = "Nom Mat"
        ' 
        ' txtNomMat
        ' 
        txtNomMat.Location = New Point(129, 158)
        txtNomMat.Name = "txtNomMat"
        txtNomMat.ReadOnly = True
        txtNomMat.Size = New Size(326, 27)
        txtNomMat.TabIndex = 415
        ' 
        ' Label2
        ' 
        Label2.AutoSize = True
        Label2.Location = New Point(26, 198)
        Label2.Name = "Label2"
        Label2.Size = New Size(44, 20)
        Label2.TabIndex = 414
        Label2.Text = "Unité"
        ' 
        ' txtUnite
        ' 
        txtUnite.Location = New Point(129, 191)
        txtUnite.Name = "txtUnite"
        txtUnite.ReadOnly = True
        txtUnite.Size = New Size(326, 27)
        txtUnite.TabIndex = 413
        ' 
        ' Label1
        ' 
        Label1.AutoSize = True
        Label1.Location = New Point(26, 231)
        Label1.Name = "Label1"
        Label1.Size = New Size(91, 20)
        Label1.TabIndex = 412
        Label1.Text = "Stock Actuel"
        ' 
        ' txtStockActuel
        ' 
        txtStockActuel.Location = New Point(129, 224)
        txtStockActuel.Name = "txtStockActuel"
        txtStockActuel.ReadOnly = True
        txtStockActuel.Size = New Size(326, 27)
        txtStockActuel.TabIndex = 411
        ' 
        ' btnActualiser
        ' 
        btnActualiser.Location = New Point(476, 191)
        btnActualiser.Name = "btnActualiser"
        btnActualiser.Size = New Size(178, 93)
        btnActualiser.TabIndex = 410
        btnActualiser.Text = "Actualiser"
        btnActualiser.UseVisualStyleBackColor = True
        ' 
        ' btnEnregistrer
        ' 
        btnEnregistrer.Location = New Point(476, 93)
        btnEnregistrer.Name = "btnEnregistrer"
        btnEnregistrer.Size = New Size(178, 93)
        btnEnregistrer.TabIndex = 409
        btnEnregistrer.Text = "Enregistrer"
        btnEnregistrer.UseVisualStyleBackColor = True
        ' 
        ' lblNombre
        ' 
        lblNombre.AutoSize = True
        lblNombre.Enabled = False
        lblNombre.Location = New Point(223, 59)
        lblNombre.Name = "lblNombre"
        lblNombre.Size = New Size(81, 20)
        lblNombre.TabIndex = 408
        lblNombre.Text = "lblNombre"
        ' 
        ' Label6
        ' 
        Label6.AutoSize = True
        Label6.Location = New Point(26, 132)
        Label6.Name = "Label6"
        Label6.Size = New Size(74, 20)
        Label6.TabIndex = 407
        Label6.Text = "Code Mat"
        ' 
        ' txtCodeMat
        ' 
        txtCodeMat.Location = New Point(129, 125)
        txtCodeMat.Name = "txtCodeMat"
        txtCodeMat.ReadOnly = True
        txtCodeMat.Size = New Size(326, 27)
        txtCodeMat.TabIndex = 406
        ' 
        ' DataGridView1
        ' 
        DataGridView1.AllowUserToAddRows = False
        DataGridView1.Anchor = AnchorStyles.Top Or AnchorStyles.Bottom Or AnchorStyles.Left Or AnchorStyles.Right
        DataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill
        DataGridView1.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize
        DataGridView1.Location = New Point(12, 290)
        DataGridView1.MultiSelect = False
        DataGridView1.Name = "DataGridView1"
        DataGridView1.ReadOnly = True
        DataGridView1.RowHeadersWidth = 51
        DataGridView1.SelectionMode = DataGridViewSelectionMode.FullRowSelect
        DataGridView1.Size = New Size(650, 333)
        DataGridView1.TabIndex = 405
        ' 
        ' Sorties_de_matières_premières
        ' 
        AutoScaleDimensions = New SizeF(8F, 20F)
        AutoScaleMode = AutoScaleMode.Font
        ClientSize = New Size(674, 635)
        Controls.Add(Label5)
        Controls.Add(txtRECHERCHE)
        Controls.Add(Label4)
        Controls.Add(txtQuantite)
        Controls.Add(Label3)
        Controls.Add(txtNomMat)
        Controls.Add(Label2)
        Controls.Add(txtUnite)
        Controls.Add(Label1)
        Controls.Add(txtStockActuel)
        Controls.Add(btnActualiser)
        Controls.Add(btnEnregistrer)
        Controls.Add(lblNombre)
        Controls.Add(Label6)
        Controls.Add(txtCodeMat)
        Controls.Add(DataGridView1)
        Controls.Add(Label21)
        Name = "Sorties_de_matières_premières"
        Text = "Sorties de matières premières"
        CType(DataGridView1, ComponentModel.ISupportInitialize).EndInit()
        ResumeLayout(False)
        PerformLayout()
    End Sub

    Friend WithEvents Label21 As Label
    Friend WithEvents Label5 As Label
    Friend WithEvents txtRECHERCHE As TextBox
    Friend WithEvents Label4 As Label
    Friend WithEvents txtQuantite As TextBox
    Friend WithEvents Label3 As Label
    Friend WithEvents txtNomMat As TextBox
    Friend WithEvents Label2 As Label
    Friend WithEvents txtUnite As TextBox
    Friend WithEvents Label1 As Label
    Friend WithEvents txtStockActuel As TextBox
    Friend WithEvents btnActualiser As Button
    Friend WithEvents btnEnregistrer As Button
    Friend WithEvents lblNombre As Label
    Friend WithEvents Label6 As Label
    Friend WithEvents txtCodeMat As TextBox
    Friend WithEvents DataGridView1 As DataGridView
End Class
