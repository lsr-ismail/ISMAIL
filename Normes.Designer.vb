<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class Normes
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(Normes))
        PrintPreviewDialog1 = New PrintPreviewDialog()
        btnAjouteNorme = New Button()
        btnSupprimeNorme = New Button()
        btnEditeNorme = New Button()
        txtClasseExposition = New TextBox()
        DataGridViewNormes = New DataGridView()
        txtRapportECLimite = New TextBox()
        txtClasseResistance = New TextBox()
        txtTeneurMiniLiant = New TextBox()
        txtCoefD = New TextBox()
        txtCoefL = New TextBox()
        txtCoefS = New TextBox()
        txtCoefU = New TextBox()
        txtCoefV = New TextBox()
        btnImprimerNorme = New Button()
        PrintDocument1 = New Printing.PrintDocument()
        Label1 = New Label()
        Label2 = New Label()
        Label3 = New Label()
        Label6 = New Label()
        Label5 = New Label()
        Label4 = New Label()
        Label12 = New Label()
        Label11 = New Label()
        Label10 = New Label()
        btnNouveauNorme = New Button()
        btnActualiserNormeme = New Button()
        Label21 = New Label()
        btnRechercheNorme = New Button()
        CType(DataGridViewNormes, ComponentModel.ISupportInitialize).BeginInit()
        SuspendLayout()
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
        ' btnAjouteNorme
        ' 
        btnAjouteNorme.Location = New Point(35, 41)
        btnAjouteNorme.Name = "btnAjouteNorme"
        btnAjouteNorme.Size = New Size(178, 61)
        btnAjouteNorme.TabIndex = 211
        btnAjouteNorme.Text = "AJOUTER"
        btnAjouteNorme.UseVisualStyleBackColor = True
        ' 
        ' btnSupprimeNorme
        ' 
        btnSupprimeNorme.Location = New Point(223, 41)
        btnSupprimeNorme.Name = "btnSupprimeNorme"
        btnSupprimeNorme.Size = New Size(178, 61)
        btnSupprimeNorme.TabIndex = 212
        btnSupprimeNorme.Text = "SUPRIME"
        btnSupprimeNorme.UseVisualStyleBackColor = True
        ' 
        ' btnEditeNorme
        ' 
        btnEditeNorme.Location = New Point(594, 41)
        btnEditeNorme.Name = "btnEditeNorme"
        btnEditeNorme.Size = New Size(178, 61)
        btnEditeNorme.TabIndex = 214
        btnEditeNorme.Text = "EDITE"
        btnEditeNorme.UseVisualStyleBackColor = True
        ' 
        ' txtClasseExposition
        ' 
        txtClasseExposition.Location = New Point(193, 121)
        txtClasseExposition.Name = "txtClasseExposition"
        txtClasseExposition.Size = New Size(418, 27)
        txtClasseExposition.TabIndex = 215
        ' 
        ' DataGridViewNormes
        ' 
        DataGridViewNormes.AllowUserToAddRows = False
        DataGridViewNormes.Anchor = AnchorStyles.Top Or AnchorStyles.Bottom Or AnchorStyles.Left Or AnchorStyles.Right
        DataGridViewNormes.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill
        DataGridViewNormes.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize
        DataGridViewNormes.Location = New Point(617, 413)
        DataGridViewNormes.MultiSelect = False
        DataGridViewNormes.Name = "DataGridViewNormes"
        DataGridViewNormes.ReadOnly = True
        DataGridViewNormes.RowHeadersWidth = 51
        DataGridViewNormes.SelectionMode = DataGridViewSelectionMode.FullRowSelect
        DataGridViewNormes.Size = New Size(1053, 283)
        DataGridViewNormes.TabIndex = 216
        ' 
        ' txtRapportECLimite
        ' 
        txtRapportECLimite.Location = New Point(195, 156)
        txtRapportECLimite.Name = "txtRapportECLimite"
        txtRapportECLimite.Size = New Size(418, 27)
        txtRapportECLimite.TabIndex = 217
        ' 
        ' txtClasseResistance
        ' 
        txtClasseResistance.Location = New Point(193, 187)
        txtClasseResistance.Name = "txtClasseResistance"
        txtClasseResistance.Size = New Size(418, 27)
        txtClasseResistance.TabIndex = 218
        ' 
        ' txtTeneurMiniLiant
        ' 
        txtTeneurMiniLiant.Location = New Point(193, 220)
        txtTeneurMiniLiant.Name = "txtTeneurMiniLiant"
        txtTeneurMiniLiant.Size = New Size(418, 27)
        txtTeneurMiniLiant.TabIndex = 219
        ' 
        ' txtCoefD
        ' 
        txtCoefD.Location = New Point(193, 253)
        txtCoefD.Name = "txtCoefD"
        txtCoefD.Size = New Size(418, 27)
        txtCoefD.TabIndex = 220
        ' 
        ' txtCoefL
        ' 
        txtCoefL.Location = New Point(193, 286)
        txtCoefL.Name = "txtCoefL"
        txtCoefL.Size = New Size(418, 27)
        txtCoefL.TabIndex = 221
        ' 
        ' txtCoefS
        ' 
        txtCoefS.Location = New Point(193, 319)
        txtCoefS.Name = "txtCoefS"
        txtCoefS.Size = New Size(418, 27)
        txtCoefS.TabIndex = 222
        ' 
        ' txtCoefU
        ' 
        txtCoefU.Location = New Point(193, 352)
        txtCoefU.Name = "txtCoefU"
        txtCoefU.Size = New Size(418, 27)
        txtCoefU.TabIndex = 223
        ' 
        ' txtCoefV
        ' 
        txtCoefV.Location = New Point(193, 385)
        txtCoefV.Name = "txtCoefV"
        txtCoefV.Size = New Size(418, 27)
        txtCoefV.TabIndex = 224
        ' 
        ' btnImprimerNorme
        ' 
        btnImprimerNorme.Location = New Point(633, 108)
        btnImprimerNorme.Name = "btnImprimerNorme"
        btnImprimerNorme.Size = New Size(178, 61)
        btnImprimerNorme.TabIndex = 227
        btnImprimerNorme.Text = "Imprimer"
        btnImprimerNorme.UseVisualStyleBackColor = True
        ' 
        ' Label1
        ' 
        Label1.AutoSize = True
        Label1.Location = New Point(35, 125)
        Label1.Name = "Label1"
        Label1.Size = New Size(136, 20)
        Label1.TabIndex = 228
        Label1.Text = "Classe exposition"
        ' 
        ' Label2
        ' 
        Label2.AutoSize = True
        Label2.Location = New Point(35, 158)
        Label2.Name = "Label2"
        Label2.Size = New Size(138, 20)
        Label2.TabIndex = 229
        Label2.Text = "Rapport E/C limite"
        ' 
        ' Label3
        ' 
        Label3.AutoSize = True
        Label3.Location = New Point(35, 191)
        Label3.Name = "Label3"
        Label3.Size = New Size(136, 20)
        Label3.TabIndex = 230
        Label3.Text = "Classe résistance"
        ' 
        ' Label6
        ' 
        Label6.AutoSize = True
        Label6.Location = New Point(32, 224)
        Label6.Name = "Label6"
        Label6.Size = New Size(131, 20)
        Label6.TabIndex = 231
        Label6.Text = "Teneur mini liant"
        ' 
        ' Label5
        ' 
        Label5.AutoSize = True
        Label5.Location = New Point(35, 257)
        Label5.Name = "Label5"
        Label5.Size = New Size(68, 20)
        Label5.TabIndex = 232
        Label5.Text = "Coef D"
        ' 
        ' Label4
        ' 
        Label4.AutoSize = True
        Label4.Location = New Point(35, 290)
        Label4.Name = "Label4"
        Label4.Size = New Size(64, 20)
        Label4.TabIndex = 233
        Label4.Text = "Coef L"
        ' 
        ' Label12
        ' 
        Label12.AutoSize = True
        Label12.Location = New Point(37, 323)
        Label12.Name = "Label12"
        Label12.Size = New Size(65, 20)
        Label12.TabIndex = 234
        Label12.Text = "Coef S"
        ' 
        ' Label11
        ' 
        Label11.AutoSize = True
        Label11.Location = New Point(35, 356)
        Label11.Name = "Label11"
        Label11.Size = New Size(67, 20)
        Label11.TabIndex = 235
        Label11.Text = "Coef U"
        ' 
        ' Label10
        ' 
        Label10.AutoSize = True
        Label10.Location = New Point(37, 389)
        Label10.Name = "Label10"
        Label10.Size = New Size(66, 20)
        Label10.TabIndex = 236
        Label10.Text = "Coef V"
        Label10.TextAlign = ContentAlignment.TopCenter
        ' 
        ' btnNouveauNorme
        ' 
        btnNouveauNorme.Location = New Point(633, 175)
        btnNouveauNorme.Name = "btnNouveauNorme"
        btnNouveauNorme.Size = New Size(178, 61)
        btnNouveauNorme.TabIndex = 239
        btnNouveauNorme.Text = "Nouveau"
        btnNouveauNorme.UseVisualStyleBackColor = True
        ' 
        ' btnActualiserNormeme
        ' 
        btnActualiserNormeme.Location = New Point(633, 242)
        btnActualiserNormeme.Name = "btnActualiserNormeme"
        btnActualiserNormeme.Size = New Size(178, 61)
        btnActualiserNormeme.TabIndex = 240
        btnActualiserNormeme.Text = "Actualiser"
        btnActualiserNormeme.UseVisualStyleBackColor = True
        ' 
        ' Label21
        ' 
        Label21.AutoSize = True
        Label21.Font = New Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point, CByte(0))
        Label21.Location = New Point(12, 9)
        Label21.Name = "Label21"
        Label21.Size = New Size(81, 28)
        Label21.TabIndex = 241
        Label21.Text = "Normes"
        ' 
        ' btnRechercheNorme
        ' 
        btnRechercheNorme.Location = New Point(845, 175)
        btnRechercheNorme.Name = "btnRechercheNorme"
        btnRechercheNorme.Size = New Size(178, 61)
        btnRechercheNorme.TabIndex = 242
        btnRechercheNorme.Text = "Recherche"
        btnRechercheNorme.UseVisualStyleBackColor = True
        ' 
        ' Normes
        ' 
        AutoScaleDimensions = New SizeF(8F, 20F)
        AutoScaleMode = AutoScaleMode.Font
        ClientSize = New Size(1082, 723)
        Controls.Add(btnRechercheNorme)
        Controls.Add(Label21)
        Controls.Add(btnActualiserNormeme)
        Controls.Add(btnNouveauNorme)
        Controls.Add(Label10)
        Controls.Add(Label11)
        Controls.Add(Label12)
        Controls.Add(Label4)
        Controls.Add(Label5)
        Controls.Add(Label6)
        Controls.Add(Label3)
        Controls.Add(Label2)
        Controls.Add(Label1)
        Controls.Add(btnImprimerNorme)
        Controls.Add(txtCoefV)
        Controls.Add(txtCoefU)
        Controls.Add(txtCoefS)
        Controls.Add(txtCoefL)
        Controls.Add(txtCoefD)
        Controls.Add(txtTeneurMiniLiant)
        Controls.Add(txtClasseResistance)
        Controls.Add(txtRapportECLimite)
        Controls.Add(DataGridViewNormes)
        Controls.Add(txtClasseExposition)
        Controls.Add(btnEditeNorme)
        Controls.Add(btnSupprimeNorme)
        Controls.Add(btnAjouteNorme)
        Name = "Normes"
        Text = "Normes"
        CType(DataGridViewNormes, ComponentModel.ISupportInitialize).EndInit()
        ResumeLayout(False)
        PerformLayout()
    End Sub
    Friend WithEvents PrintPreviewDialog1 As PrintPreviewDialog
    Friend WithEvents btnAjouteNorme As Button
    Friend WithEvents btnSupprimeNorme As Button
    Friend WithEvents btnEditeNorme As Button
    Friend WithEvents txtClasseExposition As TextBox
    Friend WithEvents DataGridViewNormes As DataGridView
    Friend WithEvents txtRapportECLimite As TextBox
    Friend WithEvents txtClasseResistance As TextBox
    Friend WithEvents txtTeneurMiniLiant As TextBox
    Friend WithEvents txtCoefD As TextBox
    Friend WithEvents txtCoefL As TextBox
    Friend WithEvents txtCoefS As TextBox
    Friend WithEvents txtCoefU As TextBox
    Friend WithEvents txtCoefV As TextBox
    Friend WithEvents btnImprimerNorme As Button
    Friend WithEvents PrintDocument1 As Printing.PrintDocument
    Friend WithEvents Label1 As Label
    Friend WithEvents Label2 As Label
    Friend WithEvents Label3 As Label
    Friend WithEvents Label6 As Label
    Friend WithEvents Label5 As Label
    Friend WithEvents Label4 As Label
    Friend WithEvents Label12 As Label
    Friend WithEvents Label11 As Label
    Friend WithEvents Label10 As Label
    Friend WithEvents btnNouveauNorme As Button
    Friend WithEvents btnActualiserNormeme As Button
    Friend WithEvents dtpDerniereModification As TextBox
    Friend WithEvents Label7 As Label
    Friend WithEvents Label21 As Label
    Friend WithEvents btnRechercheNorme As Button
End Class
