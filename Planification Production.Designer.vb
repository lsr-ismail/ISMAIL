<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class Planification_Production
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
        btnActualiser = New Button()
        lblNombreEnregistrements = New TextBox()
        btnSemainePrecedente = New Button()
        dtpDateDebut = New DateTimePicker()
        btnTerminer = New Button()
        btnNouveauPlanning = New Button()
        Imprimer = New Button()
        DataGridViewProduction = New DataGridView()
        btnOuvrir = New Button()
        btnSupprimer = New Button()
        btnDemarrer = New Button()
        dtpDateFin = New DateTimePicker()
        cmbStatut = New ComboBox()
        btnAujourdhui = New Button()
        lblNbPlannings = New Label()
        lblTotalQte = New Label()
        lblTotalLivre = New Label()
        Label3 = New Label()
        lblTotalReste = New Label()
        Label1 = New Label()
        Label2 = New Label()
        Label4 = New Label()
        btnSemaineSuivante = New Button()
        CType(DataGridViewProduction, ComponentModel.ISupportInitialize).BeginInit()
        SuspendLayout()
        ' 
        ' Label21
        ' 
        Label21.AutoSize = True
        Label21.Font = New Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point, CByte(0))
        Label21.Location = New Point(12, 9)
        Label21.Name = "Label21"
        Label21.Size = New Size(224, 28)
        Label21.TabIndex = 154
        Label21.Text = "Planification Production"
        ' 
        ' btnActualiser
        ' 
        btnActualiser.Location = New Point(927, 42)
        btnActualiser.Name = "btnActualiser"
        btnActualiser.Size = New Size(147, 61)
        btnActualiser.TabIndex = 336
        btnActualiser.Text = "Actualiser"
        btnActualiser.UseVisualStyleBackColor = True
        ' 
        ' lblNombreEnregistrements
        ' 
        lblNombreEnregistrements.Location = New Point(745, 11)
        lblNombreEnregistrements.Name = "lblNombreEnregistrements"
        lblNombreEnregistrements.Size = New Size(119, 27)
        lblNombreEnregistrements.TabIndex = 334
        ' 
        ' btnSemainePrecedente
        ' 
        btnSemainePrecedente.Location = New Point(-1, 166)
        btnSemainePrecedente.Name = "btnSemainePrecedente"
        btnSemainePrecedente.Size = New Size(109, 34)
        btnSemainePrecedente.TabIndex = 332
        btnSemainePrecedente.Text = "◀ Semaine"
        btnSemainePrecedente.UseVisualStyleBackColor = True
        ' 
        ' dtpDateDebut
        ' 
        dtpDateDebut.Location = New Point(114, 168)
        dtpDateDebut.Name = "dtpDateDebut"
        dtpDateDebut.Size = New Size(223, 27)
        dtpDateDebut.TabIndex = 331
        ' 
        ' btnTerminer
        ' 
        btnTerminer.Location = New Point(621, 42)
        btnTerminer.Name = "btnTerminer"
        btnTerminer.Size = New Size(147, 61)
        btnTerminer.TabIndex = 330
        btnTerminer.Text = "→ Terminé"
        btnTerminer.UseVisualStyleBackColor = True
        ' 
        ' btnNouveauPlanning
        ' 
        btnNouveauPlanning.Location = New Point(774, 42)
        btnNouveauPlanning.Name = "btnNouveauPlanning"
        btnNouveauPlanning.Size = New Size(147, 61)
        btnNouveauPlanning.TabIndex = 329
        btnNouveauPlanning.Text = "Nouveau"
        btnNouveauPlanning.UseVisualStyleBackColor = True
        ' 
        ' Imprimer
        ' 
        Imprimer.Location = New Point(468, 42)
        Imprimer.Name = "Imprimer"
        Imprimer.Size = New Size(147, 61)
        Imprimer.TabIndex = 328
        Imprimer.Text = "Imprimer"
        Imprimer.UseVisualStyleBackColor = True
        ' 
        ' DataGridViewProduction
        ' 
        DataGridViewProduction.AllowUserToAddRows = False
        DataGridViewProduction.AllowUserToDeleteRows = False
        DataGridViewProduction.Anchor = AnchorStyles.Top Or AnchorStyles.Bottom Or AnchorStyles.Left Or AnchorStyles.Right
        DataGridViewProduction.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill
        DataGridViewProduction.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.None
        DataGridViewProduction.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize
        DataGridViewProduction.Location = New Point(446, 126)
        DataGridViewProduction.MultiSelect = False
        DataGridViewProduction.Name = "DataGridViewProduction"
        DataGridViewProduction.ReadOnly = True
        DataGridViewProduction.RowHeadersVisible = False
        DataGridViewProduction.RowHeadersWidth = 51
        DataGridViewProduction.SelectionMode = DataGridViewSelectionMode.FullRowSelect
        DataGridViewProduction.Size = New Size(600, 570)
        DataGridViewProduction.TabIndex = 327
        ' 
        ' btnOuvrir
        ' 
        btnOuvrir.Location = New Point(162, 42)
        btnOuvrir.Name = "btnOuvrir"
        btnOuvrir.Size = New Size(147, 61)
        btnOuvrir.TabIndex = 326
        btnOuvrir.Text = "Ouvrir planning"
        btnOuvrir.UseVisualStyleBackColor = True
        ' 
        ' btnSupprimer
        ' 
        btnSupprimer.Location = New Point(315, 42)
        btnSupprimer.Name = "btnSupprimer"
        btnSupprimer.Size = New Size(147, 61)
        btnSupprimer.TabIndex = 325
        btnSupprimer.Text = "SUPRIME"
        btnSupprimer.UseVisualStyleBackColor = True
        ' 
        ' btnDemarrer
        ' 
        btnDemarrer.Location = New Point(9, 42)
        btnDemarrer.Name = "btnDemarrer"
        btnDemarrer.Size = New Size(147, 61)
        btnDemarrer.TabIndex = 324
        btnDemarrer.Text = "→ En cours"
        btnDemarrer.UseVisualStyleBackColor = True
        ' 
        ' dtpDateFin
        ' 
        dtpDateFin.Location = New Point(162, 128)
        dtpDateFin.Name = "dtpDateFin"
        dtpDateFin.Size = New Size(223, 27)
        dtpDateFin.TabIndex = 337
        ' 
        ' cmbStatut
        ' 
        cmbStatut.FormattingEnabled = True
        cmbStatut.Location = New Point(537, 7)
        cmbStatut.Name = "cmbStatut"
        cmbStatut.Size = New Size(189, 28)
        cmbStatut.TabIndex = 338
        ' 
        ' btnAujourdhui
        ' 
        btnAujourdhui.Location = New Point(30, 126)
        btnAujourdhui.Name = "btnAujourdhui"
        btnAujourdhui.Size = New Size(126, 34)
        btnAujourdhui.TabIndex = 339
        btnAujourdhui.Text = "Aujourd'hui"
        btnAujourdhui.UseVisualStyleBackColor = True
        ' 
        ' lblNbPlannings
        ' 
        lblNbPlannings.AutoSize = True
        lblNbPlannings.Font = New Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point, CByte(0))
        lblNbPlannings.Location = New Point(242, 231)
        lblNbPlannings.Name = "lblNbPlannings"
        lblNbPlannings.Size = New Size(100, 28)
        lblNbPlannings.TabIndex = 340
        lblNbPlannings.Text = "Compteur"
        ' 
        ' lblTotalQte
        ' 
        lblTotalQte.AutoSize = True
        lblTotalQte.Font = New Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point, CByte(0))
        lblTotalQte.Location = New Point(242, 269)
        lblTotalQte.Name = "lblTotalQte"
        lblTotalQte.Size = New Size(93, 28)
        lblTotalQte.TabIndex = 341
        lblTotalQte.Text = "Total QTÉ"
        ' 
        ' lblTotalLivre
        ' 
        lblTotalLivre.AutoSize = True
        lblTotalLivre.Font = New Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point, CByte(0))
        lblTotalLivre.Location = New Point(242, 309)
        lblTotalLivre.Name = "lblTotalLivre"
        lblTotalLivre.Size = New Size(96, 28)
        lblTotalLivre.TabIndex = 342
        lblTotalLivre.Text = "Total livré"
        ' 
        ' Label3
        ' 
        Label3.AutoSize = True
        Label3.Font = New Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point, CByte(0))
        Label3.Location = New Point(136, 231)
        Label3.Name = "Label3"
        Label3.Size = New Size(100, 28)
        Label3.TabIndex = 343
        Label3.Text = "Compteur"
        ' 
        ' lblTotalReste
        ' 
        lblTotalReste.AutoSize = True
        lblTotalReste.Font = New Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point, CByte(0))
        lblTotalReste.Location = New Point(239, 337)
        lblTotalReste.Name = "lblTotalReste"
        lblTotalReste.Size = New Size(101, 28)
        lblTotalReste.TabIndex = 344
        lblTotalReste.Text = "Total reste"
        ' 
        ' Label1
        ' 
        Label1.AutoSize = True
        Label1.Font = New Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point, CByte(0))
        Label1.Location = New Point(140, 337)
        Label1.Name = "Label1"
        Label1.Size = New Size(101, 28)
        Label1.TabIndex = 347
        Label1.Text = "Total reste"
        ' 
        ' Label2
        ' 
        Label2.AutoSize = True
        Label2.Font = New Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point, CByte(0))
        Label2.Location = New Point(143, 309)
        Label2.Name = "Label2"
        Label2.Size = New Size(96, 28)
        Label2.TabIndex = 346
        Label2.Text = "Total livré"
        ' 
        ' Label4
        ' 
        Label4.AutoSize = True
        Label4.Font = New Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point, CByte(0))
        Label4.Location = New Point(143, 269)
        Label4.Name = "Label4"
        Label4.Size = New Size(93, 28)
        Label4.TabIndex = 345
        Label4.Text = "Total QTÉ"
        ' 
        ' btnSemaineSuivante
        ' 
        btnSemaineSuivante.Location = New Point(343, 166)
        btnSemaineSuivante.Name = "btnSemaineSuivante"
        btnSemaineSuivante.Size = New Size(109, 34)
        btnSemaineSuivante.TabIndex = 348
        btnSemaineSuivante.Text = "◀ Semaine"
        btnSemaineSuivante.UseVisualStyleBackColor = True
        ' 
        ' Planification_Production
        ' 
        AutoScaleDimensions = New SizeF(8F, 20F)
        AutoScaleMode = AutoScaleMode.Font
        ClientSize = New Size(1082, 723)
        Controls.Add(btnSemaineSuivante)
        Controls.Add(Label1)
        Controls.Add(Label2)
        Controls.Add(Label4)
        Controls.Add(lblTotalReste)
        Controls.Add(Label3)
        Controls.Add(lblTotalLivre)
        Controls.Add(lblTotalQte)
        Controls.Add(lblNbPlannings)
        Controls.Add(btnAujourdhui)
        Controls.Add(cmbStatut)
        Controls.Add(dtpDateFin)
        Controls.Add(btnActualiser)
        Controls.Add(lblNombreEnregistrements)
        Controls.Add(btnSemainePrecedente)
        Controls.Add(dtpDateDebut)
        Controls.Add(btnTerminer)
        Controls.Add(btnNouveauPlanning)
        Controls.Add(Imprimer)
        Controls.Add(DataGridViewProduction)
        Controls.Add(btnOuvrir)
        Controls.Add(btnSupprimer)
        Controls.Add(btnDemarrer)
        Controls.Add(Label21)
        Name = "Planification_Production"
        Text = "Planification Production"
        CType(DataGridViewProduction, ComponentModel.ISupportInitialize).EndInit()
        ResumeLayout(False)
        PerformLayout()
    End Sub

    Friend WithEvents Label21 As Label
    Friend WithEvents btnActualiser As Button
    Friend WithEvents txtResteTotal As TextBox
    Friend WithEvents lblNombreEnregistrements As TextBox
    Friend WithEvents btnJourMoins As Button
    Friend WithEvents btnSemainePrecedente As Button
    Friend WithEvents dtpDateDebut As DateTimePicker
    Friend WithEvents btnTerminer As Button
    Friend WithEvents btnNouveauPlanning As Button
    Friend WithEvents Imprimer As Button
    Friend WithEvents DataGridViewProduction As DataGridView
    Friend WithEvents btnOuvrir As Button
    Friend WithEvents btnSupprimer As Button
    Friend WithEvents btnDemarrer As Button
    Friend WithEvents dtpDateFin As DateTimePicker
    Friend WithEvents cmbStatut As ComboBox
    Friend WithEvents btnAujourdhui As Button
    Friend WithEvents lblNbPlannings As Label
    Friend WithEvents lblTotalQte As Label
    Friend WithEvents lblTotalLivre As Label
    Friend WithEvents Label3 As Label
    Friend WithEvents lblTotalReste As Label
    Friend WithEvents Label1 As Label
    Friend WithEvents Label2 As Label
    Friend WithEvents Label4 As Label
    Friend WithEvents btnSemaineSuivante As Button
End Class
