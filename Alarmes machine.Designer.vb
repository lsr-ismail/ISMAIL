<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class Alarmes_machine
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
        Dim DataGridViewCellStyle1 As DataGridViewCellStyle = New DataGridViewCellStyle()
        Dim DataGridViewCellStyle2 As DataGridViewCellStyle = New DataGridViewCellStyle()
        btnDebutMoins = New Button()
        btnDebutPlus = New Button()
        dtDebut = New DateTimePicker()
        btnActualiser = New Button()
        DataGridView1 = New DataGridView()
        btnInstantane = New Button()
        btnFinMoins = New Button()
        btnFinPlus = New Button()
        dtFin = New DateTimePicker()
        btnImprimerTout = New Button()
        lblNombre = New Label()
        txtedite = New TextBox()
        btnModifier = New Button()
        btexpcsv = New Button()
        CType(DataGridView1, ComponentModel.ISupportInitialize).BeginInit()
        SuspendLayout()
        ' 
        ' btnDebutMoins
        ' 
        btnDebutMoins.Font = New Font("Segoe UI", 13F)
        btnDebutMoins.Location = New Point(711, 19)
        btnDebutMoins.Name = "btnDebutMoins"
        btnDebutMoins.Size = New Size(55, 41)
        btnDebutMoins.TabIndex = 342
        btnDebutMoins.Text = "▶"
        btnDebutMoins.UseVisualStyleBackColor = True
        ' 
        ' btnDebutPlus
        ' 
        btnDebutPlus.Font = New Font("Segoe UI", 13F)
        btnDebutPlus.Location = New Point(422, 16)
        btnDebutPlus.Name = "btnDebutPlus"
        btnDebutPlus.Size = New Size(55, 41)
        btnDebutPlus.TabIndex = 341
        btnDebutPlus.Text = "◀"
        btnDebutPlus.UseVisualStyleBackColor = True
        ' 
        ' dtDebut
        ' 
        dtDebut.Font = New Font("Segoe UI", 13F)
        dtDebut.Location = New Point(483, 21)
        dtDebut.Name = "dtDebut"
        dtDebut.Size = New Size(222, 36)
        dtDebut.TabIndex = 340
        ' 
        ' btnActualiser
        ' 
        btnActualiser.Font = New Font("Segoe UI", 13F)
        btnActualiser.Location = New Point(133, 87)
        btnActualiser.Name = "btnActualiser"
        btnActualiser.Size = New Size(115, 61)
        btnActualiser.TabIndex = 339
        btnActualiser.Text = "Actualiser"
        btnActualiser.UseVisualStyleBackColor = True
        ' 
        ' DataGridView1
        ' 
        DataGridView1.AllowUserToAddRows = False
        DataGridView1.Anchor = AnchorStyles.Top Or AnchorStyles.Bottom Or AnchorStyles.Left Or AnchorStyles.Right
        DataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill
        DataGridViewCellStyle1.Alignment = DataGridViewContentAlignment.MiddleLeft
        DataGridViewCellStyle1.BackColor = SystemColors.Control
        DataGridViewCellStyle1.Font = New Font("Segoe UI", 9F)
        DataGridViewCellStyle1.ForeColor = SystemColors.WindowText
        DataGridViewCellStyle1.SelectionBackColor = SystemColors.Highlight
        DataGridViewCellStyle1.SelectionForeColor = SystemColors.HighlightText
        DataGridViewCellStyle1.WrapMode = DataGridViewTriState.True
        DataGridView1.ColumnHeadersDefaultCellStyle = DataGridViewCellStyle1
        DataGridView1.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize
        DataGridViewCellStyle2.Alignment = DataGridViewContentAlignment.MiddleLeft
        DataGridViewCellStyle2.BackColor = SystemColors.Window
        DataGridViewCellStyle2.Font = New Font("Segoe UI", 9F)
        DataGridViewCellStyle2.ForeColor = SystemColors.ControlText
        DataGridViewCellStyle2.SelectionBackColor = SystemColors.Highlight
        DataGridViewCellStyle2.SelectionForeColor = SystemColors.HighlightText
        DataGridViewCellStyle2.WrapMode = DataGridViewTriState.False
        DataGridView1.DefaultCellStyle = DataGridViewCellStyle2
        DataGridView1.Location = New Point(12, 162)
        DataGridView1.MultiSelect = False
        DataGridView1.Name = "DataGridView1"
        DataGridView1.ReadOnly = True
        DataGridView1.RowHeadersWidth = 51
        DataGridView1.SelectionMode = DataGridViewSelectionMode.FullRowSelect
        DataGridView1.Size = New Size(880, 549)
        DataGridView1.TabIndex = 338
        ' 
        ' btnInstantane
        ' 
        btnInstantane.Font = New Font("Segoe UI", 13F)
        btnInstantane.Location = New Point(254, 87)
        btnInstantane.Name = "btnInstantane"
        btnInstantane.Size = New Size(141, 61)
        btnInstantane.TabIndex = 337
        btnInstantane.Text = " Aujourd'hui"
        btnInstantane.UseVisualStyleBackColor = True
        ' 
        ' btnFinMoins
        ' 
        btnFinMoins.Font = New Font("Segoe UI", 13F)
        btnFinMoins.Location = New Point(361, 16)
        btnFinMoins.Name = "btnFinMoins"
        btnFinMoins.Size = New Size(55, 41)
        btnFinMoins.TabIndex = 345
        btnFinMoins.Text = "▶"
        btnFinMoins.UseVisualStyleBackColor = True
        ' 
        ' btnFinPlus
        ' 
        btnFinPlus.Font = New Font("Segoe UI", 13F)
        btnFinPlus.Location = New Point(72, 16)
        btnFinPlus.Name = "btnFinPlus"
        btnFinPlus.Size = New Size(55, 41)
        btnFinPlus.TabIndex = 344
        btnFinPlus.Text = "◀"
        btnFinPlus.UseVisualStyleBackColor = True
        ' 
        ' dtFin
        ' 
        dtFin.Font = New Font("Segoe UI", 13F)
        dtFin.Location = New Point(133, 21)
        dtFin.Name = "dtFin"
        dtFin.Size = New Size(222, 36)
        dtFin.TabIndex = 343
        ' 
        ' btnImprimerTout
        ' 
        btnImprimerTout.Font = New Font("Segoe UI", 13F)
        btnImprimerTout.Location = New Point(12, 87)
        btnImprimerTout.Name = "btnImprimerTout"
        btnImprimerTout.Size = New Size(115, 61)
        btnImprimerTout.TabIndex = 346
        btnImprimerTout.Text = "Imprimer"
        btnImprimerTout.UseVisualStyleBackColor = True
        ' 
        ' lblNombre
        ' 
        lblNombre.AutoSize = True
        lblNombre.Location = New Point(847, 9)
        lblNombre.Name = "lblNombre"
        lblNombre.Size = New Size(45, 20)
        lblNombre.TabIndex = 347
        lblNombre.Text = "Label"
        ' 
        ' txtedite
        ' 
        txtedite.Font = New Font("Segoe UI", 13F)
        txtedite.Location = New Point(410, 69)
        txtedite.Name = "txtedite"
        txtedite.Size = New Size(413, 36)
        txtedite.TabIndex = 349
        ' 
        ' btnModifier
        ' 
        btnModifier.Font = New Font("Segoe UI", 13F)
        btnModifier.Location = New Point(649, 111)
        btnModifier.Name = "btnModifier"
        btnModifier.Size = New Size(178, 37)
        btnModifier.TabIndex = 348
        btnModifier.Text = "EDITE"
        btnModifier.UseVisualStyleBackColor = True
        ' 
        ' btexpcsv
        ' 
        btexpcsv.Font = New Font("Segoe UI", 13F)
        btexpcsv.Location = New Point(410, 111)
        btexpcsv.Name = "btexpcsv"
        btexpcsv.Size = New Size(178, 37)
        btexpcsv.TabIndex = 350
        btexpcsv.Text = "EXPORT CSV"
        btexpcsv.UseVisualStyleBackColor = True
        ' 
        ' Alarmes_machine
        ' 
        AutoScaleDimensions = New SizeF(8F, 20F)
        AutoScaleMode = AutoScaleMode.Font
        ClientSize = New Size(904, 723)
        Controls.Add(btexpcsv)
        Controls.Add(txtedite)
        Controls.Add(btnModifier)
        Controls.Add(lblNombre)
        Controls.Add(btnImprimerTout)
        Controls.Add(btnFinMoins)
        Controls.Add(btnFinPlus)
        Controls.Add(dtFin)
        Controls.Add(btnDebutMoins)
        Controls.Add(btnDebutPlus)
        Controls.Add(dtDebut)
        Controls.Add(btnActualiser)
        Controls.Add(DataGridView1)
        Controls.Add(btnInstantane)
        Name = "Alarmes_machine"
        Text = "Alarmes machine"
        CType(DataGridView1, ComponentModel.ISupportInitialize).EndInit()
        ResumeLayout(False)
        PerformLayout()
    End Sub

    Friend WithEvents btnDebutMoins As Button
    Friend WithEvents btnDebutPlus As Button
    Friend WithEvents dtDebut As DateTimePicker
    Friend WithEvents btnActualiser As Button
    Friend WithEvents DataGridView1 As DataGridView
    Friend WithEvents btnInstantane As Button
    Friend WithEvents btnFinMoins As Button
    Friend WithEvents btnFinPlus As Button
    Friend WithEvents dtFin As DateTimePicker
    Friend WithEvents btnImprimerTout As Button
    Friend WithEvents lblNombre As Label
    Friend WithEvents txtedite As TextBox
    Friend WithEvents btnModifier As Button
    Friend WithEvents btexpcsv As Button
End Class
