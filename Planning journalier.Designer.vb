<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class Planning_journalier
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
        Imprimer = New Button()
        DataGridViewPlanning = New DataGridView()
        btnModifier = New Button()
        btnSupprimer = New Button()
        btnAjouter = New Button()
        Duplique = New Button()
        Exécute = New Button()
        btnJourMoins = New Button()
        btnJourPlus = New Button()
        dtpDatePlanning = New DateTimePicker()
        lblNombreEnregistrements = New TextBox()
        txtResteTotal = New TextBox()
        btnActualiser = New Button()
        CType(DataGridViewPlanning, ComponentModel.ISupportInitialize).BeginInit()
        SuspendLayout()
        ' 
        ' Label21
        ' 
        Label21.AutoSize = True
        Label21.Font = New Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point, CByte(0))
        Label21.Location = New Point(12, 3)
        Label21.Name = "Label21"
        Label21.Size = New Size(179, 28)
        Label21.TabIndex = 154
        Label21.Text = "Planning_journalier"
        ' 
        ' Imprimer
        ' 
        Imprimer.Location = New Point(470, 38)
        Imprimer.Name = "Imprimer"
        Imprimer.Size = New Size(147, 61)
        Imprimer.TabIndex = 312
        Imprimer.Text = "Imprimer"
        Imprimer.UseVisualStyleBackColor = True
        ' 
        ' DataGridViewPlanning
        ' 
        DataGridViewPlanning.AllowUserToAddRows = False
        DataGridViewPlanning.AllowUserToDeleteRows = False
        DataGridViewPlanning.Anchor = AnchorStyles.Top Or AnchorStyles.Bottom Or AnchorStyles.Left Or AnchorStyles.Right
        DataGridViewPlanning.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill
        DataGridViewPlanning.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize
        DataGridViewPlanning.Location = New Point(11, 141)
        DataGridViewPlanning.MultiSelect = False
        DataGridViewPlanning.Name = "DataGridViewPlanning"
        DataGridViewPlanning.ReadOnly = True
        DataGridViewPlanning.RowHeadersVisible = False
        DataGridViewPlanning.RowHeadersWidth = 51
        DataGridViewPlanning.SelectionMode = DataGridViewSelectionMode.FullRowSelect
        DataGridViewPlanning.Size = New Size(1059, 570)
        DataGridViewPlanning.TabIndex = 311
        ' 
        ' btnModifier
        ' 
        btnModifier.Location = New Point(164, 38)
        btnModifier.Name = "btnModifier"
        btnModifier.Size = New Size(147, 61)
        btnModifier.TabIndex = 310
        btnModifier.Text = "EDITE"
        btnModifier.UseVisualStyleBackColor = True
        ' 
        ' btnSupprimer
        ' 
        btnSupprimer.Location = New Point(317, 38)
        btnSupprimer.Name = "btnSupprimer"
        btnSupprimer.Size = New Size(147, 61)
        btnSupprimer.TabIndex = 308
        btnSupprimer.Text = "SUPRIME"
        btnSupprimer.UseVisualStyleBackColor = True
        ' 
        ' btnAjouter
        ' 
        btnAjouter.Location = New Point(11, 38)
        btnAjouter.Name = "btnAjouter"
        btnAjouter.Size = New Size(147, 61)
        btnAjouter.TabIndex = 307
        btnAjouter.Text = "AJOUTE"
        btnAjouter.UseVisualStyleBackColor = True
        ' 
        ' Duplique
        ' 
        Duplique.Location = New Point(776, 38)
        Duplique.Name = "Duplique"
        Duplique.Size = New Size(147, 61)
        Duplique.TabIndex = 315
        Duplique.Text = "Duplique"
        Duplique.UseVisualStyleBackColor = True
        ' 
        ' Exécute
        ' 
        Exécute.Location = New Point(623, 38)
        Exécute.Name = "Exécute"
        Exécute.Size = New Size(147, 61)
        Exécute.TabIndex = 316
        Exécute.Text = "Exécute"
        Exécute.UseVisualStyleBackColor = True
        ' 
        ' btnJourMoins
        ' 
        btnJourMoins.Location = New Point(485, 3)
        btnJourMoins.Name = "btnJourMoins"
        btnJourMoins.Size = New Size(34, 34)
        btnJourMoins.TabIndex = 319
        btnJourMoins.Text = "-"
        btnJourMoins.UseVisualStyleBackColor = True
        ' 
        ' btnJourPlus
        ' 
        btnJourPlus.Location = New Point(234, 3)
        btnJourPlus.Name = "btnJourPlus"
        btnJourPlus.Size = New Size(34, 34)
        btnJourPlus.TabIndex = 318
        btnJourPlus.Text = "+"
        btnJourPlus.UseVisualStyleBackColor = True
        ' 
        ' dtpDatePlanning
        ' 
        dtpDatePlanning.Location = New Point(265, 7)
        dtpDatePlanning.Name = "dtpDatePlanning"
        dtpDatePlanning.Size = New Size(223, 27)
        dtpDatePlanning.TabIndex = 317
        ' 
        ' lblNombreEnregistrements
        ' 
        lblNombreEnregistrements.Location = New Point(747, 7)
        lblNombreEnregistrements.Name = "lblNombreEnregistrements"
        lblNombreEnregistrements.Size = New Size(119, 27)
        lblNombreEnregistrements.TabIndex = 320
        ' 
        ' txtResteTotal
        ' 
        txtResteTotal.Location = New Point(563, 7)
        txtResteTotal.Name = "txtResteTotal"
        txtResteTotal.Size = New Size(119, 27)
        txtResteTotal.TabIndex = 321
        ' 
        ' btnActualiser
        ' 
        btnActualiser.Location = New Point(929, 38)
        btnActualiser.Name = "btnActualiser"
        btnActualiser.Size = New Size(147, 61)
        btnActualiser.TabIndex = 322
        btnActualiser.Text = "Actualiser"
        btnActualiser.UseVisualStyleBackColor = True
        ' 
        ' Planning_journalier
        ' 
        AutoScaleDimensions = New SizeF(8F, 20F)
        AutoScaleMode = AutoScaleMode.Font
        ClientSize = New Size(1082, 723)
        Controls.Add(btnActualiser)
        Controls.Add(txtResteTotal)
        Controls.Add(lblNombreEnregistrements)
        Controls.Add(btnJourMoins)
        Controls.Add(btnJourPlus)
        Controls.Add(dtpDatePlanning)
        Controls.Add(Exécute)
        Controls.Add(Duplique)
        Controls.Add(Imprimer)
        Controls.Add(DataGridViewPlanning)
        Controls.Add(btnModifier)
        Controls.Add(btnSupprimer)
        Controls.Add(btnAjouter)
        Controls.Add(Label21)
        Name = "Planning_journalier"
        Text = "Planning_journalier"
        CType(DataGridViewPlanning, ComponentModel.ISupportInitialize).EndInit()
        ResumeLayout(False)
        PerformLayout()
    End Sub

    Friend WithEvents Label21 As Label
    Friend WithEvents Imprimer As Button
    Friend WithEvents DataGridViewPlanning As DataGridView
    Friend WithEvents btnModifier As Button
    Friend WithEvents btnSupprimer As Button
    Friend WithEvents btnAjouter As Button
    Friend WithEvents Duplique As Button
    Friend WithEvents Exécute As Button
    Friend WithEvents btnJourMoins As Button
    Friend WithEvents btnJourPlus As Button
    Friend WithEvents dtpDatePlanning As DateTimePicker
    Friend WithEvents lblNombreEnregistrements As TextBox
    Friend WithEvents txtResteTotal As TextBox
    Friend WithEvents btnActualiser As Button
End Class
