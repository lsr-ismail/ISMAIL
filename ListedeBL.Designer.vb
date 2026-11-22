<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class ListedeBL
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(ListedeBL))
        txtRECHERCHE = New TextBox()
        btnActualiser = New Button()
        Label1 = New Label()
        Imprimer = New Button()
        DataGridView1 = New DataGridView()
        txtCodeCamion = New TextBox()
        PrintDocument1 = New Printing.PrintDocument()
        PrintPreviewDialog1 = New PrintPreviewDialog()
        RECHERCHE = New Button()
        btnFinMoins = New Button()
        btnFinPlus = New Button()
        dtpFin = New DateTimePicker()
        btnDebutMoins = New Button()
        btnDebutPlus = New Button()
        dtpDebut = New DateTimePicker()
        CType(DataGridView1, ComponentModel.ISupportInitialize).BeginInit()
        SuspendLayout()
        ' 
        ' txtRECHERCHE
        ' 
        txtRECHERCHE.Location = New Point(633, 138)
        txtRECHERCHE.Name = "txtRECHERCHE"
        txtRECHERCHE.Size = New Size(418, 27)
        txtRECHERCHE.TabIndex = 307
        ' 
        ' btnActualiser
        ' 
        btnActualiser.Location = New Point(689, 71)
        btnActualiser.Name = "btnActualiser"
        btnActualiser.Size = New Size(178, 61)
        btnActualiser.TabIndex = 306
        btnActualiser.Text = "Actualiser"
        btnActualiser.UseVisualStyleBackColor = True
        ' 
        ' Label1
        ' 
        Label1.AutoSize = True
        Label1.Location = New Point(60, 145)
        Label1.Name = "Label1"
        Label1.Size = New Size(44, 20)
        Label1.TabIndex = 294
        Label1.Text = "Code"
        ' 
        ' Imprimer
        ' 
        Imprimer.Location = New Point(873, 71)
        Imprimer.Name = "Imprimer"
        Imprimer.Size = New Size(178, 61)
        Imprimer.TabIndex = 293
        Imprimer.Text = "Imprimer"
        Imprimer.UseVisualStyleBackColor = True
        ' 
        ' DataGridView1
        ' 
        DataGridView1.AllowUserToAddRows = False
        DataGridView1.Anchor = AnchorStyles.Top Or AnchorStyles.Bottom Or AnchorStyles.Left Or AnchorStyles.Right
        DataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill
        DataGridView1.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize
        DataGridView1.Location = New Point(21, 198)
        DataGridView1.MultiSelect = False
        DataGridView1.Name = "DataGridView1"
        DataGridView1.ReadOnly = True
        DataGridView1.RowHeadersWidth = 51
        DataGridView1.SelectionMode = DataGridViewSelectionMode.FullRowSelect
        DataGridView1.Size = New Size(1052, 496)
        DataGridView1.TabIndex = 282
        ' 
        ' txtCodeCamion
        ' 
        txtCodeCamion.Location = New Point(209, 138)
        txtCodeCamion.Name = "txtCodeCamion"
        txtCodeCamion.Size = New Size(418, 27)
        txtCodeCamion.TabIndex = 281
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
        ' RECHERCHE
        ' 
        RECHERCHE.Location = New Point(505, 71)
        RECHERCHE.Name = "RECHERCHE"
        RECHERCHE.Size = New Size(178, 61)
        RECHERCHE.TabIndex = 279
        RECHERCHE.Text = "RECHERCHE"
        RECHERCHE.UseVisualStyleBackColor = True
        ' 
        ' btnFinMoins
        ' 
        btnFinMoins.Location = New Point(311, 101)
        btnFinMoins.Name = "btnFinMoins"
        btnFinMoins.Size = New Size(34, 34)
        btnFinMoins.TabIndex = 322
        btnFinMoins.Text = "-"
        btnFinMoins.UseVisualStyleBackColor = True
        ' 
        ' btnFinPlus
        ' 
        btnFinPlus.Location = New Point(60, 101)
        btnFinPlus.Name = "btnFinPlus"
        btnFinPlus.Size = New Size(34, 34)
        btnFinPlus.TabIndex = 321
        btnFinPlus.Text = "+"
        btnFinPlus.UseVisualStyleBackColor = True
        ' 
        ' dtpFin
        ' 
        dtpFin.Location = New Point(91, 105)
        dtpFin.Name = "dtpFin"
        dtpFin.Size = New Size(223, 27)
        dtpFin.TabIndex = 320
        ' 
        ' btnDebutMoins
        ' 
        btnDebutMoins.Location = New Point(311, 64)
        btnDebutMoins.Name = "btnDebutMoins"
        btnDebutMoins.Size = New Size(34, 34)
        btnDebutMoins.TabIndex = 325
        btnDebutMoins.Text = "-"
        btnDebutMoins.UseVisualStyleBackColor = True
        ' 
        ' btnDebutPlus
        ' 
        btnDebutPlus.Location = New Point(60, 64)
        btnDebutPlus.Name = "btnDebutPlus"
        btnDebutPlus.Size = New Size(34, 34)
        btnDebutPlus.TabIndex = 324
        btnDebutPlus.Text = "+"
        btnDebutPlus.UseVisualStyleBackColor = True
        ' 
        ' dtpDebut
        ' 
        dtpDebut.Location = New Point(91, 68)
        dtpDebut.Name = "dtpDebut"
        dtpDebut.Size = New Size(223, 27)
        dtpDebut.TabIndex = 323
        ' 
        ' ListedeBL
        ' 
        AutoScaleDimensions = New SizeF(8F, 20F)
        AutoScaleMode = AutoScaleMode.Font
        ClientSize = New Size(1082, 723)
        Controls.Add(btnDebutMoins)
        Controls.Add(btnDebutPlus)
        Controls.Add(dtpDebut)
        Controls.Add(btnFinMoins)
        Controls.Add(btnFinPlus)
        Controls.Add(dtpFin)
        Controls.Add(txtRECHERCHE)
        Controls.Add(btnActualiser)
        Controls.Add(Label1)
        Controls.Add(Imprimer)
        Controls.Add(DataGridView1)
        Controls.Add(txtCodeCamion)
        Controls.Add(RECHERCHE)
        Name = "ListedeBL"
        Text = "ListedeBL"
        CType(DataGridView1, ComponentModel.ISupportInitialize).EndInit()
        ResumeLayout(False)
        PerformLayout()
    End Sub
    Friend WithEvents txtRECHERCHE As TextBox
    Friend WithEvents btnActualiser As Button
    Friend WithEvents Label1 As Label
    Friend WithEvents Imprimer As Button
    Friend WithEvents DataGridView1 As DataGridView
    Friend WithEvents txtCodeCamion As TextBox
    Friend WithEvents PrintDocument1 As Printing.PrintDocument
    Friend WithEvents PrintPreviewDialog1 As PrintPreviewDialog
    Friend WithEvents RECHERCHE As Button
    Friend WithEvents btnFinMoins As Button
    Friend WithEvents btnFinPlus As Button
    Friend WithEvents dtpFin As DateTimePicker
    Friend WithEvents btnDebutMoins As Button
    Friend WithEvents btnDebutPlus As Button
    Friend WithEvents dtpDebut As DateTimePicker
End Class
