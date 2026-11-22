<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class TVA
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(TVA))
        PrintDocument1 = New Printing.PrintDocument()
        txtRECHERCHE = New TextBox()
        btnActualiser = New Button()
        btnNouveau = New Button()
        Label10 = New Label()
        dtpDateTarif2 = New Label()
        Label4 = New Label()
        Label5 = New Label()
        Label6 = New Label()
        Label3 = New Label()
        Label2 = New Label()
        Label1 = New Label()
        PrintPreviewDialog1 = New PrintPreviewDialog()
        txtTVA_Montant2 = New TextBox()
        txtTVA_Montant1 = New TextBox()
        txtTVA_Nom = New TextBox()
        txtTVA_Cle = New TextBox()
        txtTVA_Code = New TextBox()
        DataGridView1 = New DataGridView()
        txtTVA_ID = New TextBox()
        btnModifier = New Button()
        btnSupprimer = New Button()
        btnAjouter = New Button()
        Label21 = New Label()
        Label12 = New Label()
        DateTimePicker1 = New DateTimePicker()
        chkInterdiction = New CheckBox()
        DateTimePicker2 = New DateTimePicker()
        lblNombre = New Label()
        CType(DataGridView1, ComponentModel.ISupportInitialize).BeginInit()
        SuspendLayout()
        ' 
        ' txtRECHERCHE
        ' 
        txtRECHERCHE.Location = New Point(187, 133)
        txtRECHERCHE.Name = "txtRECHERCHE"
        txtRECHERCHE.ReadOnly = True
        txtRECHERCHE.Size = New Size(250, 27)
        txtRECHERCHE.TabIndex = 208
        ' 
        ' btnActualiser
        ' 
        btnActualiser.Location = New Point(765, 38)
        btnActualiser.Name = "btnActualiser"
        btnActualiser.Size = New Size(169, 61)
        btnActualiser.TabIndex = 207
        btnActualiser.Text = "Actualiser"
        btnActualiser.UseVisualStyleBackColor = True
        ' 
        ' btnNouveau
        ' 
        btnNouveau.Location = New Point(581, 38)
        btnNouveau.Name = "btnNouveau"
        btnNouveau.Size = New Size(178, 61)
        btnNouveau.TabIndex = 206
        btnNouveau.Text = "Nouveau"
        btnNouveau.UseVisualStyleBackColor = True
        ' 
        ' Label10
        ' 
        Label10.AutoSize = True
        Label10.Location = New Point(52, 434)
        Label10.Name = "Label10"
        Label10.Size = New Size(85, 20)
        Label10.TabIndex = 203
        Label10.Text = "Interdiction"
        ' 
        ' dtpDateTarif2
        ' 
        dtpDateTarif2.AutoSize = True
        dtpDateTarif2.Location = New Point(50, 401)
        dtpDateTarif2.Name = "dtpDateTarif2"
        dtpDateTarif2.Size = New Size(85, 20)
        dtpDateTarif2.TabIndex = 202
        dtpDateTarif2.Text = "Date Tarif 2"
        ' 
        ' Label4
        ' 
        Label4.AutoSize = True
        Label4.Location = New Point(50, 335)
        Label4.Name = "Label4"
        Label4.Size = New Size(107, 20)
        Label4.TabIndex = 200
        Label4.Text = "TVA Montant 2"
        ' 
        ' Label5
        ' 
        Label5.AutoSize = True
        Label5.Location = New Point(50, 302)
        Label5.Name = "Label5"
        Label5.Size = New Size(107, 20)
        Label5.TabIndex = 199
        Label5.Text = "TVA Montant 1"
        ' 
        ' Label6
        ' 
        Label6.AutoSize = True
        Label6.Location = New Point(47, 269)
        Label6.Name = "Label6"
        Label6.Size = New Size(72, 20)
        Label6.TabIndex = 198
        Label6.Text = "Nom TVA"
        ' 
        ' Label3
        ' 
        Label3.AutoSize = True
        Label3.Location = New Point(50, 236)
        Label3.Name = "Label3"
        Label3.Size = New Size(60, 20)
        Label3.TabIndex = 197
        Label3.Text = "Cle TVA"
        ' 
        ' Label2
        ' 
        Label2.AutoSize = True
        Label2.Location = New Point(50, 203)
        Label2.Name = "Label2"
        Label2.Size = New Size(74, 20)
        Label2.TabIndex = 196
        Label2.Text = "Code TVA"
        ' 
        ' Label1
        ' 
        Label1.AutoSize = True
        Label1.Location = New Point(50, 170)
        Label1.Name = "Label1"
        Label1.Size = New Size(54, 20)
        Label1.TabIndex = 195
        Label1.Text = "ID TVA"
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
        ' txtTVA_Montant2
        ' 
        txtTVA_Montant2.Location = New Point(187, 331)
        txtTVA_Montant2.Name = "txtTVA_Montant2"
        txtTVA_Montant2.Size = New Size(250, 27)
        txtTVA_Montant2.TabIndex = 188
        ' 
        ' txtTVA_Montant1
        ' 
        txtTVA_Montant1.Location = New Point(187, 298)
        txtTVA_Montant1.Name = "txtTVA_Montant1"
        txtTVA_Montant1.Size = New Size(250, 27)
        txtTVA_Montant1.TabIndex = 187
        ' 
        ' txtTVA_Nom
        ' 
        txtTVA_Nom.Location = New Point(187, 265)
        txtTVA_Nom.Name = "txtTVA_Nom"
        txtTVA_Nom.Size = New Size(250, 27)
        txtTVA_Nom.TabIndex = 186
        ' 
        ' txtTVA_Cle
        ' 
        txtTVA_Cle.Location = New Point(187, 232)
        txtTVA_Cle.Name = "txtTVA_Cle"
        txtTVA_Cle.Size = New Size(250, 27)
        txtTVA_Cle.TabIndex = 185
        ' 
        ' txtTVA_Code
        ' 
        txtTVA_Code.Location = New Point(189, 201)
        txtTVA_Code.Name = "txtTVA_Code"
        txtTVA_Code.Size = New Size(250, 27)
        txtTVA_Code.TabIndex = 184
        ' 
        ' DataGridView1
        ' 
        DataGridView1.AllowUserToAddRows = False
        DataGridView1.Anchor = AnchorStyles.Top Or AnchorStyles.Bottom Or AnchorStyles.Left Or AnchorStyles.Right
        DataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill
        DataGridView1.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize
        DataGridView1.Location = New Point(454, 113)
        DataGridView1.MultiSelect = False
        DataGridView1.Name = "DataGridView1"
        DataGridView1.ReadOnly = True
        DataGridView1.RowHeadersWidth = 51
        DataGridView1.SelectionMode = DataGridViewSelectionMode.FullRowSelect
        DataGridView1.Size = New Size(519, 428)
        DataGridView1.TabIndex = 183
        ' 
        ' txtTVA_ID
        ' 
        txtTVA_ID.Location = New Point(187, 166)
        txtTVA_ID.Name = "txtTVA_ID"
        txtTVA_ID.Size = New Size(250, 27)
        txtTVA_ID.TabIndex = 182
        ' 
        ' btnModifier
        ' 
        btnModifier.Location = New Point(213, 38)
        btnModifier.Name = "btnModifier"
        btnModifier.Size = New Size(178, 61)
        btnModifier.TabIndex = 181
        btnModifier.Text = "EDITE"
        btnModifier.UseVisualStyleBackColor = True
        ' 
        ' btnSupprimer
        ' 
        btnSupprimer.Location = New Point(397, 38)
        btnSupprimer.Name = "btnSupprimer"
        btnSupprimer.Size = New Size(178, 61)
        btnSupprimer.TabIndex = 179
        btnSupprimer.Text = "SUPRIME"
        btnSupprimer.UseVisualStyleBackColor = True
        ' 
        ' btnAjouter
        ' 
        btnAjouter.Location = New Point(29, 38)
        btnAjouter.Name = "btnAjouter"
        btnAjouter.Size = New Size(178, 61)
        btnAjouter.TabIndex = 178
        btnAjouter.Text = "AJOUTE"
        btnAjouter.UseVisualStyleBackColor = True
        ' 
        ' Label21
        ' 
        Label21.AutoSize = True
        Label21.Font = New Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point, CByte(0))
        Label21.Location = New Point(12, 9)
        Label21.Name = "Label21"
        Label21.Size = New Size(46, 28)
        Label21.TabIndex = 211
        Label21.Text = "TVA"
        ' 
        ' Label12
        ' 
        Label12.AutoSize = True
        Label12.Location = New Point(52, 372)
        Label12.Name = "Label12"
        Label12.Size = New Size(85, 20)
        Label12.TabIndex = 212
        Label12.Text = "Date Tarif 1"
        ' 
        ' DateTimePicker1
        ' 
        DateTimePicker1.Location = New Point(187, 401)
        DateTimePicker1.Name = "DateTimePicker1"
        DateTimePicker1.Size = New Size(250, 27)
        DateTimePicker1.TabIndex = 213
        ' 
        ' chkInterdiction
        ' 
        chkInterdiction.AutoSize = True
        chkInterdiction.Location = New Point(281, 434)
        chkInterdiction.Name = "chkInterdiction"
        chkInterdiction.Size = New Size(18, 17)
        chkInterdiction.TabIndex = 216
        chkInterdiction.UseVisualStyleBackColor = True
        ' 
        ' DateTimePicker2
        ' 
        DateTimePicker2.Location = New Point(187, 364)
        DateTimePicker2.Name = "DateTimePicker2"
        DateTimePicker2.Size = New Size(250, 27)
        DateTimePicker2.TabIndex = 217
        ' 
        ' lblNombre
        ' 
        lblNombre.AutoSize = True
        lblNombre.Location = New Point(239, 110)
        lblNombre.Name = "lblNombre"
        lblNombre.Size = New Size(73, 20)
        lblNombre.TabIndex = 218
        lblNombre.Text = "txtTVA_ID"
        ' 
        ' TVA
        ' 
        AutoScaleDimensions = New SizeF(8F, 20F)
        AutoScaleMode = AutoScaleMode.Font
        ClientSize = New Size(985, 553)
        Controls.Add(lblNombre)
        Controls.Add(DateTimePicker2)
        Controls.Add(chkInterdiction)
        Controls.Add(DateTimePicker1)
        Controls.Add(Label12)
        Controls.Add(Label21)
        Controls.Add(txtRECHERCHE)
        Controls.Add(btnActualiser)
        Controls.Add(btnNouveau)
        Controls.Add(Label10)
        Controls.Add(dtpDateTarif2)
        Controls.Add(Label4)
        Controls.Add(Label5)
        Controls.Add(Label6)
        Controls.Add(Label3)
        Controls.Add(Label2)
        Controls.Add(Label1)
        Controls.Add(txtTVA_Montant2)
        Controls.Add(txtTVA_Montant1)
        Controls.Add(txtTVA_Nom)
        Controls.Add(txtTVA_Cle)
        Controls.Add(txtTVA_Code)
        Controls.Add(DataGridView1)
        Controls.Add(txtTVA_ID)
        Controls.Add(btnModifier)
        Controls.Add(btnSupprimer)
        Controls.Add(btnAjouter)
        Name = "TVA"
        Text = "TVA"
        CType(DataGridView1, ComponentModel.ISupportInitialize).EndInit()
        ResumeLayout(False)
        PerformLayout()
    End Sub

    Friend WithEvents PrintDocument1 As Printing.PrintDocument
    Friend WithEvents Label7 As Label
    Friend WithEvents dtpDerniereModification As TextBox
    Friend WithEvents txtRECHERCHE As TextBox
    Friend WithEvents btnActualiser As Button
    Friend WithEvents btnNouveau As Button
    Friend WithEvents Label8 As Label
    Friend WithEvents Label9 As Label
    Friend WithEvents Label10 As Label
    Friend WithEvents dtpDateTarif2 As Label
    Friend WithEvents dtpDateTarif1 As Label
    Friend WithEvents Label4 As Label
    Friend WithEvents Label5 As Label
    Friend WithEvents Label6 As Label
    Friend WithEvents Label3 As Label
    Friend WithEvents Label2 As Label
    Friend WithEvents Label1 As Label
    Friend WithEvents PrintPreviewDialog1 As PrintPreviewDialog
    Friend WithEvents txtTelephone As TextBox
    Friend WithEvents txtCodeRadio As TextBox
    Friend WithEvents txtChargeUtile As TextBox
    Friend WithEvents txtCapacite As TextBox
    Friend WithEvents txtTVA_Montant2 As TextBox
    Friend WithEvents txtTVA_Montant1 As TextBox
    Friend WithEvents txtTVA_Nom As TextBox
    Friend WithEvents txtTVA_Cle As TextBox
    Friend WithEvents txtTVA_Code As TextBox
    Friend WithEvents DataGridView1 As DataGridView
    Friend WithEvents txtTVA_ID As TextBox
    Friend WithEvents btnModifier As Button
    Friend WithEvents btnSupprimer As Button
    Friend WithEvents btnAjouter As Button
    Friend WithEvents Label21 As Label
    Friend WithEvents Label12 As Label
    Friend WithEvents DateTimePicker1 As DateTimePicker

    Friend WithEvents chkInterdiction As CheckBox
    Friend WithEvents DateTimePicker2 As DateTimePicker
    Friend WithEvents lblNombre As Label
End Class
