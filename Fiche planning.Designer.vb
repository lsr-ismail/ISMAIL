<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class Fiche_planning
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
        Label21 = New Label()
        btnActualiser = New Button()
        btnExportCSV = New Button()
        lblPourcent = New Label()
        btnImprimer = New Button()
        DataGridView1 = New DataGridView()
        btnModifier = New Button()
        RECHERCHE = New Button()
        btnSupprimer = New Button()
        btnAujourdhui = New Button()
        dtpDate = New DateTimePicker()
        lblTitreFiche = New Label()
        btnJourMoins = New Button()
        btnJourPlus = New Button()
        ProgressBar1 = New ProgressBar()
        lblTotalQte = New Label()
        lblNbPlannings = New Label()
        lblTotalReste = New Label()
        lblCree = New Label()
        lblTotalLivre = New Label()
        lblTermine = New Label()
        lblEnCours = New Label()
        CType(DataGridView1, ComponentModel.ISupportInitialize).BeginInit()
        SuspendLayout()
        ' 
        ' Label21
        ' 
        Label21.AutoSize = True
        Label21.Font = New Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point, CByte(0))
        Label21.Location = New Point(12, 9)
        Label21.Name = "Label21"
        Label21.Size = New Size(142, 28)
        Label21.TabIndex = 311
        Label21.Text = "Fiche planning"
        ' 
        ' btnActualiser
        ' 
        btnActualiser.Location = New Point(767, 85)
        btnActualiser.Name = "btnActualiser"
        btnActualiser.Size = New Size(137, 61)
        btnActualiser.TabIndex = 329
        btnActualiser.Text = "Actualiser"
        btnActualiser.UseVisualStyleBackColor = True
        ' 
        ' btnExportCSV
        ' 
        btnExportCSV.Location = New Point(910, 85)
        btnExportCSV.Name = "btnExportCSV"
        btnExportCSV.Size = New Size(137, 61)
        btnExportCSV.TabIndex = 328
        btnExportCSV.Text = "💾 Export CSV"
        btnExportCSV.UseVisualStyleBackColor = True
        ' 
        ' lblPourcent
        ' 
        lblPourcent.AutoSize = True
        lblPourcent.Location = New Point(22, 175)
        lblPourcent.Name = "lblPourcent"
        lblPourcent.Size = New Size(45, 20)
        lblPourcent.TabIndex = 323
        lblPourcent.Text = "Label"
        ' 
        ' btnImprimer
        ' 
        btnImprimer.Location = New Point(624, 85)
        btnImprimer.Name = "btnImprimer"
        btnImprimer.Size = New Size(137, 61)
        btnImprimer.TabIndex = 322
        btnImprimer.Text = "Imprimer"
        btnImprimer.UseVisualStyleBackColor = True
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
        DataGridView1.Location = New Point(110, 175)
        DataGridView1.MultiSelect = False
        DataGridView1.Name = "DataGridView1"
        DataGridView1.ReadOnly = True
        DataGridView1.RowHeadersWidth = 51
        DataGridView1.SelectionMode = DataGridViewSelectionMode.FullRowSelect
        DataGridView1.Size = New Size(921, 494)
        DataGridView1.TabIndex = 317
        ' 
        ' btnModifier
        ' 
        btnModifier.Location = New Point(481, 85)
        btnModifier.Name = "btnModifier"
        btnModifier.Size = New Size(137, 61)
        btnModifier.TabIndex = 315
        btnModifier.Text = "EDITE"
        btnModifier.UseVisualStyleBackColor = True
        ' 
        ' RECHERCHE
        ' 
        RECHERCHE.Location = New Point(338, 85)
        RECHERCHE.Name = "RECHERCHE"
        RECHERCHE.Size = New Size(137, 61)
        RECHERCHE.TabIndex = 314
        RECHERCHE.Text = "RECHERCHE"
        RECHERCHE.UseVisualStyleBackColor = True
        ' 
        ' btnSupprimer
        ' 
        btnSupprimer.Location = New Point(195, 85)
        btnSupprimer.Name = "btnSupprimer"
        btnSupprimer.Size = New Size(137, 61)
        btnSupprimer.TabIndex = 313
        btnSupprimer.Text = "SUPRIME"
        btnSupprimer.UseVisualStyleBackColor = True
        ' 
        ' btnAujourdhui
        ' 
        btnAujourdhui.Location = New Point(52, 85)
        btnAujourdhui.Name = "btnAujourdhui"
        btnAujourdhui.Size = New Size(137, 61)
        btnAujourdhui.TabIndex = 312
        btnAujourdhui.Text = " Aujourd'hui"
        btnAujourdhui.UseVisualStyleBackColor = True
        ' 
        ' dtpDate
        ' 
        dtpDate.Location = New Point(338, 12)
        dtpDate.Name = "dtpDate"
        dtpDate.Size = New Size(250, 27)
        dtpDate.TabIndex = 333
        ' 
        ' lblTitreFiche
        ' 
        lblTitreFiche.AutoSize = True
        lblTitreFiche.Font = New Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point, CByte(0))
        lblTitreFiche.Location = New Point(190, 9)
        lblTitreFiche.Name = "lblTitreFiche"
        lblTitreFiche.Size = New Size(93, 28)
        lblTitreFiche.TabIndex = 334
        lblTitreFiche.Text = "titreFiche"
        ' 
        ' btnJourMoins
        ' 
        btnJourMoins.Location = New Point(277, 12)
        btnJourMoins.Name = "btnJourMoins"
        btnJourMoins.Size = New Size(55, 41)
        btnJourMoins.TabIndex = 335
        btnJourMoins.Text = "◀"
        btnJourMoins.UseVisualStyleBackColor = True
        ' 
        ' btnJourPlus
        ' 
        btnJourPlus.Location = New Point(594, 12)
        btnJourPlus.Name = "btnJourPlus"
        btnJourPlus.Size = New Size(55, 41)
        btnJourPlus.TabIndex = 336
        btnJourPlus.Text = "▶"
        btnJourPlus.UseVisualStyleBackColor = True
        ' 
        ' ProgressBar1
        ' 
        ProgressBar1.Location = New Point(350, 50)
        ProgressBar1.Name = "ProgressBar1"
        ProgressBar1.Size = New Size(220, 29)
        ProgressBar1.TabIndex = 337
        ' 
        ' lblTotalQte
        ' 
        lblTotalQte.AutoSize = True
        lblTotalQte.Location = New Point(22, 215)
        lblTotalQte.Name = "lblTotalQte"
        lblTotalQte.Size = New Size(45, 20)
        lblTotalQte.TabIndex = 338
        lblTotalQte.Text = "Label"
        ' 
        ' lblNbPlannings
        ' 
        lblNbPlannings.AutoSize = True
        lblNbPlannings.Location = New Point(22, 195)
        lblNbPlannings.Name = "lblNbPlannings"
        lblNbPlannings.Size = New Size(45, 20)
        lblNbPlannings.TabIndex = 339
        lblNbPlannings.Text = "Label"
        ' 
        ' lblTotalReste
        ' 
        lblTotalReste.AutoSize = True
        lblTotalReste.Location = New Point(22, 255)
        lblTotalReste.Name = "lblTotalReste"
        lblTotalReste.Size = New Size(45, 20)
        lblTotalReste.TabIndex = 342
        lblTotalReste.Text = "Label"
        ' 
        ' lblCree
        ' 
        lblCree.AutoSize = True
        lblCree.Location = New Point(22, 275)
        lblCree.Name = "lblCree"
        lblCree.Size = New Size(45, 20)
        lblCree.TabIndex = 341
        lblCree.Text = "Label"
        ' 
        ' lblTotalLivre
        ' 
        lblTotalLivre.AutoSize = True
        lblTotalLivre.Location = New Point(22, 235)
        lblTotalLivre.Name = "lblTotalLivre"
        lblTotalLivre.Size = New Size(45, 20)
        lblTotalLivre.TabIndex = 340
        lblTotalLivre.Text = "Label"
        ' 
        ' lblTermine
        ' 
        lblTermine.AutoSize = True
        lblTermine.Location = New Point(22, 315)
        lblTermine.Name = "lblTermine"
        lblTermine.Size = New Size(45, 20)
        lblTermine.TabIndex = 345
        lblTermine.Text = "Label"
        ' 
        ' lblEnCours
        ' 
        lblEnCours.AutoSize = True
        lblEnCours.Location = New Point(22, 295)
        lblEnCours.Name = "lblEnCours"
        lblEnCours.Size = New Size(45, 20)
        lblEnCours.TabIndex = 343
        lblEnCours.Text = "Label"
        ' 
        ' Fiche_planning
        ' 
        AutoScaleDimensions = New SizeF(8F, 20F)
        AutoScaleMode = AutoScaleMode.Font
        ClientSize = New Size(1082, 723)
        Controls.Add(lblTermine)
        Controls.Add(lblEnCours)
        Controls.Add(lblTotalReste)
        Controls.Add(lblCree)
        Controls.Add(lblTotalLivre)
        Controls.Add(lblNbPlannings)
        Controls.Add(lblTotalQte)
        Controls.Add(ProgressBar1)
        Controls.Add(btnJourPlus)
        Controls.Add(btnJourMoins)
        Controls.Add(lblTitreFiche)
        Controls.Add(dtpDate)
        Controls.Add(btnActualiser)
        Controls.Add(btnExportCSV)
        Controls.Add(lblPourcent)
        Controls.Add(btnImprimer)
        Controls.Add(DataGridView1)
        Controls.Add(btnModifier)
        Controls.Add(RECHERCHE)
        Controls.Add(btnSupprimer)
        Controls.Add(btnAujourdhui)
        Controls.Add(Label21)
        Name = "Fiche_planning"
        Text = "Fiche planning"
        CType(DataGridView1, ComponentModel.ISupportInitialize).EndInit()
        ResumeLayout(False)
        PerformLayout()
    End Sub

    Friend WithEvents Label21 As Label
    Friend WithEvents btnActualiser As Button
    Friend WithEvents btnExportCSV As Button
    Friend WithEvents lblPourcent As Label
    Friend WithEvents btnImprimer As Button
    Friend WithEvents DataGridView1 As DataGridView
    Friend WithEvents btnModifier As Button
    Friend WithEvents RECHERCHE As Button
    Friend WithEvents btnSupprimer As Button
    Friend WithEvents btnAujourdhui As Button
    Friend WithEvents dtpDate As DateTimePicker
    Friend WithEvents lblTitreFiche As Label
    Friend WithEvents btnJourMoins As Button
    Friend WithEvents btnJourPlus As Button
    Friend WithEvents ProgressBar1 As ProgressBar
    Friend WithEvents lblTotalQte As Label
    Friend WithEvents lblNbPlannings As Label
    Friend WithEvents lblTotalReste As Label
    Friend WithEvents lblCree As Label
    Friend WithEvents lblTotalLivre As Label
    Friend WithEvents lblTermine As Label
    Friend WithEvents lblEnCours As Label
End Class
