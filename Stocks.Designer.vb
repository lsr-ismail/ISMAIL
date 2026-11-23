<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class Stocks
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(Stocks))
        Label21 = New Label()
        txtQteEntree = New TextBox()
        txtSTO_Stock = New TextBox()
        txtSTO_Somme_Corrections = New TextBox()
        txtSTO_Somme_Sorties = New TextBox()
        txtSTO_Somme_Entrees = New TextBox()
        txtSTO_Stock_Initial = New TextBox()
        txtMAT_Nom = New TextBox()
        DataGridView1 = New DataGridView()
        txtMAT_Code = New TextBox()
        btnCorrection = New Button()
        PrintPreviewDialog1 = New PrintPreviewDialog()
        PrintDocument1 = New Printing.PrintDocument()
        txtRECHERCHE = New TextBox()
        btnEntree = New Button()
        lblNombre = New Label()
        Label11 = New Label()
        Label12 = New Label()
        Label4 = New Label()
        Label5 = New Label()
        Label6 = New Label()
        Label3 = New Label()
        Label2 = New Label()
        Label13 = New Label()
        btnActualiser = New Button()
        Label8 = New Label()
        dtpSTO_Date = New DateTimePicker()
        txtSTO_Unite = New TextBox()
        Label1 = New Label()
        btnMiseAJour = New Button()
        btnImprimer = New Button()
        CType(DataGridView1, ComponentModel.ISupportInitialize).BeginInit()
        SuspendLayout()
        ' 
        ' Label21
        ' 
        Label21.AutoSize = True
        Label21.Font = New Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point, CByte(0))
        Label21.Location = New Point(12, 9)
        Label21.Name = "Label21"
        Label21.Size = New Size(68, 28)
        Label21.TabIndex = 150
        Label21.Text = "Stocks"
        ' 
        ' txtQteEntree
        ' 
        txtQteEntree.Location = New Point(205, 437)
        txtQteEntree.Name = "txtQteEntree"
        txtQteEntree.Size = New Size(252, 27)
        txtQteEntree.TabIndex = 224
        ' 
        ' txtSTO_Stock
        ' 
        txtSTO_Stock.Location = New Point(205, 404)
        txtSTO_Stock.Name = "txtSTO_Stock"
        txtSTO_Stock.Size = New Size(252, 27)
        txtSTO_Stock.TabIndex = 223
        ' 
        ' txtSTO_Somme_Corrections
        ' 
        txtSTO_Somme_Corrections.Location = New Point(205, 371)
        txtSTO_Somme_Corrections.Name = "txtSTO_Somme_Corrections"
        txtSTO_Somme_Corrections.Size = New Size(252, 27)
        txtSTO_Somme_Corrections.TabIndex = 222
        ' 
        ' txtSTO_Somme_Sorties
        ' 
        txtSTO_Somme_Sorties.Location = New Point(205, 338)
        txtSTO_Somme_Sorties.Name = "txtSTO_Somme_Sorties"
        txtSTO_Somme_Sorties.Size = New Size(252, 27)
        txtSTO_Somme_Sorties.TabIndex = 221
        ' 
        ' txtSTO_Somme_Entrees
        ' 
        txtSTO_Somme_Entrees.Location = New Point(205, 305)
        txtSTO_Somme_Entrees.Name = "txtSTO_Somme_Entrees"
        txtSTO_Somme_Entrees.Size = New Size(252, 27)
        txtSTO_Somme_Entrees.TabIndex = 220
        ' 
        ' txtSTO_Stock_Initial
        ' 
        txtSTO_Stock_Initial.Location = New Point(205, 272)
        txtSTO_Stock_Initial.Name = "txtSTO_Stock_Initial"
        txtSTO_Stock_Initial.Size = New Size(252, 27)
        txtSTO_Stock_Initial.TabIndex = 219
        ' 
        ' txtMAT_Nom
        ' 
        txtMAT_Nom.Location = New Point(203, 206)
        txtMAT_Nom.Name = "txtMAT_Nom"
        txtMAT_Nom.Size = New Size(252, 27)
        txtMAT_Nom.TabIndex = 218
        ' 
        ' DataGridView1
        ' 
        DataGridView1.AllowUserToAddRows = False
        DataGridView1.Anchor = AnchorStyles.Top Or AnchorStyles.Bottom Or AnchorStyles.Left Or AnchorStyles.Right
        DataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill
        DataGridView1.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize
        DataGridView1.Location = New Point(473, 122)
        DataGridView1.MultiSelect = False
        DataGridView1.Name = "DataGridView1"
        DataGridView1.ReadOnly = True
        DataGridView1.RowHeadersWidth = 51
        DataGridView1.SelectionMode = DataGridViewSelectionMode.FullRowSelect
        DataGridView1.Size = New Size(597, 426)
        DataGridView1.TabIndex = 217
        ' 
        ' txtMAT_Code
        ' 
        txtMAT_Code.Location = New Point(203, 173)
        txtMAT_Code.Name = "txtMAT_Code"
        txtMAT_Code.Size = New Size(252, 27)
        txtMAT_Code.TabIndex = 216
        ' 
        ' btnCorrection
        ' 
        btnCorrection.Location = New Point(476, 36)
        btnCorrection.Name = "btnCorrection"
        btnCorrection.Size = New Size(178, 61)
        btnCorrection.TabIndex = 214
        btnCorrection.Text = "Correction"
        btnCorrection.UseVisualStyleBackColor = True
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
        txtRECHERCHE.Location = New Point(203, 140)
        txtRECHERCHE.Name = "txtRECHERCHE"
        txtRECHERCHE.Size = New Size(252, 27)
        txtRECHERCHE.TabIndex = 242
        ' 
        ' btnEntree
        ' 
        btnEntree.Location = New Point(292, 36)
        btnEntree.Name = "btnEntree"
        btnEntree.Size = New Size(178, 61)
        btnEntree.TabIndex = 241
        btnEntree.Text = "Entrée"
        btnEntree.UseVisualStyleBackColor = True
        ' 
        ' lblNombre
        ' 
        lblNombre.AutoSize = True
        lblNombre.Location = New Point(285, 117)
        lblNombre.Name = "lblNombre"
        lblNombre.Size = New Size(81, 20)
        lblNombre.TabIndex = 238
        lblNombre.Text = "lblNombre"
        ' 
        ' Label11
        ' 
        Label11.AutoSize = True
        Label11.Location = New Point(26, 441)
        Label11.Name = "Label11"
        Label11.Size = New Size(79, 20)
        Label11.TabIndex = 236
        Label11.Text = "Qté Entrée"
        ' 
        ' Label12
        ' 
        Label12.AutoSize = True
        Label12.Location = New Point(28, 408)
        Label12.Name = "Label12"
        Label12.Size = New Size(75, 20)
        Label12.TabIndex = 235
        Label12.Text = "STO Stock"
        ' 
        ' Label4
        ' 
        Label4.AutoSize = True
        Label4.Location = New Point(26, 375)
        Label4.Name = "Label4"
        Label4.Size = New Size(169, 20)
        Label4.TabIndex = 234
        Label4.Text = "STO Somme Corrections"
        ' 
        ' Label5
        ' 
        Label5.AutoSize = True
        Label5.Location = New Point(26, 342)
        Label5.Name = "Label5"
        Label5.Size = New Size(131, 20)
        Label5.TabIndex = 233
        Label5.Text = "TO Somme Sorties"
        ' 
        ' Label6
        ' 
        Label6.AutoSize = True
        Label6.Location = New Point(23, 309)
        Label6.Name = "Label6"
        Label6.Size = New Size(142, 20)
        Label6.TabIndex = 232
        Label6.Text = "STO Somme Entrées"
        ' 
        ' Label3
        ' 
        Label3.AutoSize = True
        Label3.Location = New Point(26, 276)
        Label3.Name = "Label3"
        Label3.Size = New Size(116, 20)
        Label3.TabIndex = 231
        Label3.Text = "STO Stock Initial"
        ' 
        ' Label2
        ' 
        Label2.AutoSize = True
        Label2.Location = New Point(22, 208)
        Label2.Name = "Label2"
        Label2.Size = New Size(76, 20)
        Label2.TabIndex = 230
        Label2.Text = "MAT Nom"
        ' 
        ' Label13
        ' 
        Label13.AutoSize = True
        Label13.Location = New Point(24, 177)
        Label13.Name = "Label13"
        Label13.Size = New Size(78, 20)
        Label13.TabIndex = 229
        Label13.Text = "MAT Code"
        ' 
        ' btnActualiser
        ' 
        btnActualiser.Location = New Point(108, 36)
        btnActualiser.Name = "btnActualiser"
        btnActualiser.Size = New Size(178, 61)
        btnActualiser.TabIndex = 212
        btnActualiser.Text = "Actualiser"
        btnActualiser.UseVisualStyleBackColor = True
        ' 
        ' Label8
        ' 
        Label8.AutoSize = True
        Label8.Location = New Point(28, 479)
        Label8.Name = "Label8"
        Label8.Size = New Size(121, 20)
        Label8.TabIndex = 239
        Label8.Text = "Date Time Picker"
        ' 
        ' dtpSTO_Date
        ' 
        dtpSTO_Date.Location = New Point(207, 474)
        dtpSTO_Date.Name = "dtpSTO_Date"
        dtpSTO_Date.Size = New Size(250, 27)
        dtpSTO_Date.TabIndex = 243
        ' 
        ' txtSTO_Unite
        ' 
        txtSTO_Unite.Location = New Point(205, 239)
        txtSTO_Unite.Name = "txtSTO_Unite"
        txtSTO_Unite.Size = New Size(252, 27)
        txtSTO_Unite.TabIndex = 244
        ' 
        ' Label1
        ' 
        Label1.AutoSize = True
        Label1.Location = New Point(24, 241)
        Label1.Name = "Label1"
        Label1.Size = New Size(74, 20)
        Label1.TabIndex = 245
        Label1.Text = "STO Unité"
        ' 
        ' btnMiseAJour
        ' 
        btnMiseAJour.Location = New Point(660, 36)
        btnMiseAJour.Name = "btnMiseAJour"
        btnMiseAJour.Size = New Size(178, 61)
        btnMiseAJour.TabIndex = 246
        btnMiseAJour.Text = "Mise A Jour"
        btnMiseAJour.UseVisualStyleBackColor = True
        ' 
        ' btnImprimer
        ' 
        btnImprimer.Location = New Point(844, 36)
        btnImprimer.Name = "btnImprimer"
        btnImprimer.Size = New Size(178, 61)
        btnImprimer.TabIndex = 247
        btnImprimer.Text = "Imprimer"
        btnImprimer.UseVisualStyleBackColor = True
        ' 
        ' Stocks
        ' 
        AutoScaleDimensions = New SizeF(8F, 20F)
        AutoScaleMode = AutoScaleMode.Font
        ClientSize = New Size(1082, 560)
        Controls.Add(btnImprimer)
        Controls.Add(btnMiseAJour)
        Controls.Add(txtSTO_Unite)
        Controls.Add(Label1)
        Controls.Add(dtpSTO_Date)
        Controls.Add(txtQteEntree)
        Controls.Add(txtSTO_Stock)
        Controls.Add(txtSTO_Somme_Corrections)
        Controls.Add(txtSTO_Somme_Sorties)
        Controls.Add(txtSTO_Somme_Entrees)
        Controls.Add(txtSTO_Stock_Initial)
        Controls.Add(txtMAT_Nom)
        Controls.Add(DataGridView1)
        Controls.Add(txtMAT_Code)
        Controls.Add(btnCorrection)
        Controls.Add(txtRECHERCHE)
        Controls.Add(btnEntree)
        Controls.Add(Label8)
        Controls.Add(lblNombre)
        Controls.Add(Label11)
        Controls.Add(Label12)
        Controls.Add(Label4)
        Controls.Add(Label5)
        Controls.Add(Label6)
        Controls.Add(Label3)
        Controls.Add(Label2)
        Controls.Add(Label13)
        Controls.Add(btnActualiser)
        Controls.Add(Label21)
        Name = "Stocks"
        Text = "Stocks"
        CType(DataGridView1, ComponentModel.ISupportInitialize).EndInit()
        ResumeLayout(False)
        PerformLayout()
    End Sub

    Friend WithEvents Label21 As Label
    Friend WithEvents txtQteEntree As TextBox
    Friend WithEvents txtSTO_Stock As TextBox
    Friend WithEvents txtSTO_Somme_Corrections As TextBox
    Friend WithEvents txtSTO_Somme_Sorties As TextBox
    Friend WithEvents txtSTO_Somme_Entrees As TextBox
    Friend WithEvents txtSTO_Stock_Initial As TextBox
    Friend WithEvents txtMAT_Nom As TextBox
    Friend WithEvents DataGridView1 As DataGridView
    Friend WithEvents txtMAT_Code As TextBox
    Friend WithEvents btnCorrection As Button
    Friend WithEvents PrintPreviewDialog1 As PrintPreviewDialog
    Friend WithEvents PrintDocument1 As Printing.PrintDocument
    Friend WithEvents txtRECHERCHE As TextBox
    Friend WithEvents btnEntree As Button
    Friend WithEvents lblNombre As Label
    Friend WithEvents Label11 As Label
    Friend WithEvents Label12 As Label
    Friend WithEvents Label4 As Label
    Friend WithEvents Label5 As Label
    Friend WithEvents Label6 As Label
    Friend WithEvents Label3 As Label
    Friend WithEvents Label2 As Label
    Friend WithEvents Label13 As Label
    Friend WithEvents btnActualiser As Button
    Friend WithEvents Label8 As Label
    Friend WithEvents dtpSTO_Date As DateTimePicker
    Friend WithEvents txtSTO_Unite As TextBox
    Friend WithEvents Label1 As Label
    Friend WithEvents btnMiseAJour As Button
    Friend WithEvents btnImprimer As Button
End Class
