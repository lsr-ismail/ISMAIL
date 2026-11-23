<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class Assistance___registre_des_occurrences
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
        RECHERCHE = New Button()
        txtRECHERCHE = New TextBox()
        DataGridView1 = New DataGridView()
        btnFinMoins = New Button()
        btnfinPlus = New Button()
        dtpFin = New DateTimePicker()
        btnDebutMoins = New Button()
        btnDebutPlus = New Button()
        dtpDebut = New DateTimePicker()
        CType(DataGridView1, ComponentModel.ISupportInitialize).BeginInit()
        SuspendLayout()
        ' 
        ' Label21
        ' 
        Label21.AutoSize = True
        Label21.Font = New Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point, 0)
        Label21.Location = New Point(12, 9)
        Label21.Name = "Label21"
        Label21.Size = New Size(353, 28)
        Label21.TabIndex = 149
        Label21.Text = "Assistance - registre des occurrences"
        ' 
        ' RECHERCHE
        ' 
        RECHERCHE.Location = New Point(677, 9)
        RECHERCHE.Name = "RECHERCHE"
        RECHERCHE.Size = New Size(123, 45)
        RECHERCHE.TabIndex = 150
        RECHERCHE.Text = "RECHERCHE"
        RECHERCHE.UseVisualStyleBackColor = True
        ' 
        ' txtRECHERCHE
        ' 
        txtRECHERCHE.Location = New Point(381, 13)
        txtRECHERCHE.Name = "txtRECHERCHE"
        txtRECHERCHE.Size = New Size(278, 27)
        txtRECHERCHE.TabIndex = 151
        ' 
        ' DataGridView1
        ' 
        DataGridView1.AllowUserToAddRows = False
        DataGridView1.Anchor = AnchorStyles.Top Or AnchorStyles.Bottom Or AnchorStyles.Left Or AnchorStyles.Right
        DataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill
        DataGridView1.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize
        DataGridView1.Location = New Point(95, 104)
        DataGridView1.MultiSelect = False
        DataGridView1.Name = "DataGridView1"
        DataGridView1.ReadOnly = True
        DataGridView1.RowHeadersWidth = 51
        DataGridView1.SelectionMode = DataGridViewSelectionMode.FullRowSelect
        DataGridView1.Size = New Size(923, 607)
        DataGridView1.TabIndex = 218
        ' 
        ' btnFinMoins
        ' 
        btnFinMoins.Location = New Point(415, 57)
        btnFinMoins.Name = "btnFinMoins"
        btnFinMoins.Size = New Size(25, 34)
        btnFinMoins.TabIndex = 336
        btnFinMoins.Text = "-"
        btnFinMoins.UseVisualStyleBackColor = True
        ' 
        ' btnfinPlus
        ' 
        btnfinPlus.Location = New Point(164, 55)
        btnfinPlus.Name = "btnfinPlus"
        btnfinPlus.Size = New Size(25, 34)
        btnfinPlus.TabIndex = 335
        btnfinPlus.Text = "+"
        btnfinPlus.UseVisualStyleBackColor = True
        ' 
        ' dtpFin
        ' 
        dtpFin.Location = New Point(195, 62)
        dtpFin.Name = "dtpFin"
        dtpFin.Size = New Size(214, 27)
        dtpFin.TabIndex = 334
        ' 
        ' btnDebutMoins
        ' 
        btnDebutMoins.Location = New Point(702, 59)
        btnDebutMoins.Name = "btnDebutMoins"
        btnDebutMoins.Size = New Size(25, 34)
        btnDebutMoins.TabIndex = 339
        btnDebutMoins.Text = "-"
        btnDebutMoins.UseVisualStyleBackColor = True
        ' 
        ' btnDebutPlus
        ' 
        btnDebutPlus.Location = New Point(451, 57)
        btnDebutPlus.Name = "btnDebutPlus"
        btnDebutPlus.Size = New Size(25, 34)
        btnDebutPlus.TabIndex = 338
        btnDebutPlus.Text = "+"
        btnDebutPlus.UseVisualStyleBackColor = True
        ' 
        ' dtpDebut
        ' 
        dtpDebut.Location = New Point(482, 64)
        dtpDebut.Name = "dtpDebut"
        dtpDebut.Size = New Size(214, 27)
        dtpDebut.TabIndex = 337
        ' 
        ' Assistance___registre_des_occurrences
        ' 
        AutoScaleDimensions = New SizeF(8F, 20F)
        AutoScaleMode = AutoScaleMode.Font
        ClientSize = New Size(1082, 723)
        Controls.Add(btnDebutMoins)
        Controls.Add(btnDebutPlus)
        Controls.Add(dtpDebut)
        Controls.Add(btnFinMoins)
        Controls.Add(btnfinPlus)
        Controls.Add(dtpFin)
        Controls.Add(DataGridView1)
        Controls.Add(txtRECHERCHE)
        Controls.Add(RECHERCHE)
        Controls.Add(Label21)
        Name = "Assistance___registre_des_occurrences"
        Text = "Assistance - registre des occurrences"
        CType(DataGridView1, ComponentModel.ISupportInitialize).EndInit()
        ResumeLayout(False)
        PerformLayout()
    End Sub

    Friend WithEvents Label21 As Label
    Friend WithEvents RECHERCHE As Button
    Friend WithEvents txtRECHERCHE As TextBox
    Friend WithEvents DataGridView1 As DataGridView
    Friend WithEvents btnFinMoins As Button
    Friend WithEvents btnfinPlus As Button
    Friend WithEvents dtpFin As DateTimePicker
    Friend WithEvents btnDebutMoins As Button
    Friend WithEvents btnDebutPlus As Button
    Friend WithEvents dtpDebut As DateTimePicker
End Class
