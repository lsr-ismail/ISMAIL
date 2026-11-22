<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class Postes
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(Postes))
        PrintPreviewDialog1 = New PrintPreviewDialog()
        Label7 = New Label()
        dtpDerniereModification = New TextBox()
        PrintDocument1 = New Printing.PrintDocument()
        txtRECHERCHE = New TextBox()
        btnActualiser = New Button()
        btnNouveau = New Button()
        Label8 = New Label()
        Label9 = New Label()
        Label10 = New Label()
        Label11 = New Label()
        Label12 = New Label()
        Label4 = New Label()
        Label5 = New Label()
        Label6 = New Label()
        Label3 = New Label()
        Label2 = New Label()
        Label1 = New Label()
        Imprimer = New Button()
        txtTelephone = New TextBox()
        txtCodeRadio = New TextBox()
        txtChargeUtile = New TextBox()
        txtCapacite = New TextBox()
        txtChauffeur = New TextBox()
        txtEquipement = New TextBox()
        txtFamille = New TextBox()
        txtFournisseur = New TextBox()
        txtNomCamion = New TextBox()
        txtCleCamion = New TextBox()
        DataGridView1 = New DataGridView()
        txtCodeCamion = New TextBox()
        btnModifier = New Button()
        RECHERCHE = New Button()
        btnSupprimer = New Button()
        btnAjouter = New Button()
        Label21 = New Label()
        CType(DataGridView1, ComponentModel.ISupportInitialize).BeginInit()
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
        ' Label7
        ' 
        Label7.AutoSize = True
        Label7.Location = New Point(41, 497)
        Label7.Name = "Label7"
        Label7.Size = New Size(151, 20)
        Label7.TabIndex = 177
        Label7.Text = "DerniereModification"
        ' 
        ' dtpDerniereModification
        ' 
        dtpDerniereModification.Location = New Point(202, 493)
        dtpDerniereModification.Name = "dtpDerniereModification"
        dtpDerniereModification.Size = New Size(418, 27)
        dtpDerniereModification.TabIndex = 176
        ' 
        ' txtRECHERCHE
        ' 
        txtRECHERCHE.Location = New Point(305, 26)
        txtRECHERCHE.Name = "txtRECHERCHE"
        txtRECHERCHE.Size = New Size(418, 27)
        txtRECHERCHE.TabIndex = 175
        ' 
        ' btnActualiser
        ' 
        btnActualiser.Location = New Point(1153, 50)
        btnActualiser.Name = "btnActualiser"
        btnActualiser.Size = New Size(178, 61)
        btnActualiser.TabIndex = 174
        btnActualiser.Text = "Actualiser"
        btnActualiser.UseVisualStyleBackColor = True
        ' 
        ' btnNouveau
        ' 
        btnNouveau.Location = New Point(969, 50)
        btnNouveau.Name = "btnNouveau"
        btnNouveau.Size = New Size(178, 61)
        btnNouveau.TabIndex = 173
        btnNouveau.Text = "Nouveau"
        btnNouveau.UseVisualStyleBackColor = True
        ' 
        ' Label8
        ' 
        Label8.AutoSize = True
        Label8.Location = New Point(39, 464)
        Label8.Name = "Label8"
        Label8.Size = New Size(78, 20)
        Label8.TabIndex = 172
        Label8.Text = "Telephone"
        ' 
        ' Label9
        ' 
        Label9.AutoSize = True
        Label9.Location = New Point(41, 431)
        Label9.Name = "Label9"
        Label9.Size = New Size(83, 20)
        Label9.TabIndex = 171
        Label9.Text = "CodeRadio"
        ' 
        ' Label10
        ' 
        Label10.AutoSize = True
        Label10.Location = New Point(44, 398)
        Label10.Name = "Label10"
        Label10.Size = New Size(87, 20)
        Label10.TabIndex = 170
        Label10.Text = "ChargeUtile"
        ' 
        ' Label11
        ' 
        Label11.AutoSize = True
        Label11.Location = New Point(42, 365)
        Label11.Name = "Label11"
        Label11.Size = New Size(67, 20)
        Label11.TabIndex = 169
        Label11.Text = "Capacite"
        ' 
        ' Label12
        ' 
        Label12.AutoSize = True
        Label12.Location = New Point(44, 332)
        Label12.Name = "Label12"
        Label12.Size = New Size(73, 20)
        Label12.TabIndex = 168
        Label12.Text = "Chauffeur"
        ' 
        ' Label4
        ' 
        Label4.AutoSize = True
        Label4.Location = New Point(42, 299)
        Label4.Name = "Label4"
        Label4.Size = New Size(89, 20)
        Label4.TabIndex = 167
        Label4.Text = "Equipement"
        ' 
        ' Label5
        ' 
        Label5.AutoSize = True
        Label5.Location = New Point(42, 266)
        Label5.Name = "Label5"
        Label5.Size = New Size(56, 20)
        Label5.TabIndex = 166
        Label5.Text = "Famille"
        ' 
        ' Label6
        ' 
        Label6.AutoSize = True
        Label6.Location = New Point(39, 233)
        Label6.Name = "Label6"
        Label6.Size = New Size(83, 20)
        Label6.TabIndex = 165
        Label6.Text = "Fournisseur"
        ' 
        ' Label3
        ' 
        Label3.AutoSize = True
        Label3.Location = New Point(42, 200)
        Label3.Name = "Label3"
        Label3.Size = New Size(98, 20)
        Label3.TabIndex = 164
        Label3.Text = "NomCamionr"
        ' 
        ' Label2
        ' 
        Label2.AutoSize = True
        Label2.Location = New Point(42, 167)
        Label2.Name = "Label2"
        Label2.Size = New Size(81, 20)
        Label2.TabIndex = 163
        Label2.Text = "CleCamion"
        ' 
        ' Label1
        ' 
        Label1.AutoSize = True
        Label1.Location = New Point(42, 134)
        Label1.Name = "Label1"
        Label1.Size = New Size(95, 20)
        Label1.TabIndex = 162
        Label1.Text = "CodeCamion"
        ' 
        ' Imprimer
        ' 
        Imprimer.Location = New Point(785, 50)
        Imprimer.Name = "Imprimer"
        Imprimer.Size = New Size(178, 61)
        Imprimer.TabIndex = 161
        Imprimer.Text = "Imprimer"
        Imprimer.UseVisualStyleBackColor = True
        ' 
        ' txtTelephone
        ' 
        txtTelephone.Location = New Point(200, 460)
        txtTelephone.Name = "txtTelephone"
        txtTelephone.Size = New Size(418, 27)
        txtTelephone.TabIndex = 160
        ' 
        ' txtCodeRadio
        ' 
        txtCodeRadio.Location = New Point(200, 427)
        txtCodeRadio.Name = "txtCodeRadio"
        txtCodeRadio.Size = New Size(418, 27)
        txtCodeRadio.TabIndex = 159
        ' 
        ' txtChargeUtile
        ' 
        txtChargeUtile.Location = New Point(200, 394)
        txtChargeUtile.Name = "txtChargeUtile"
        txtChargeUtile.Size = New Size(418, 27)
        txtChargeUtile.TabIndex = 158
        ' 
        ' txtCapacite
        ' 
        txtCapacite.Location = New Point(200, 361)
        txtCapacite.Name = "txtCapacite"
        txtCapacite.Size = New Size(418, 27)
        txtCapacite.TabIndex = 157
        ' 
        ' txtChauffeur
        ' 
        txtChauffeur.Location = New Point(200, 328)
        txtChauffeur.Name = "txtChauffeur"
        txtChauffeur.Size = New Size(418, 27)
        txtChauffeur.TabIndex = 156
        ' 
        ' txtEquipement
        ' 
        txtEquipement.Location = New Point(200, 295)
        txtEquipement.Name = "txtEquipement"
        txtEquipement.Size = New Size(418, 27)
        txtEquipement.TabIndex = 155
        ' 
        ' txtFamille
        ' 
        txtFamille.Location = New Point(200, 262)
        txtFamille.Name = "txtFamille"
        txtFamille.Size = New Size(418, 27)
        txtFamille.TabIndex = 154
        ' 
        ' txtFournisseur
        ' 
        txtFournisseur.Location = New Point(200, 229)
        txtFournisseur.Name = "txtFournisseur"
        txtFournisseur.Size = New Size(418, 27)
        txtFournisseur.TabIndex = 153
        ' 
        ' txtNomCamion
        ' 
        txtNomCamion.Location = New Point(200, 196)
        txtNomCamion.Name = "txtNomCamion"
        txtNomCamion.Size = New Size(418, 27)
        txtNomCamion.TabIndex = 152
        ' 
        ' txtCleCamion
        ' 
        txtCleCamion.Location = New Point(202, 165)
        txtCleCamion.Name = "txtCleCamion"
        txtCleCamion.Size = New Size(418, 27)
        txtCleCamion.TabIndex = 151
        ' 
        ' DataGridView1
        ' 
        DataGridView1.AllowUserToAddRows = False
        DataGridView1.Anchor = AnchorStyles.Top Or AnchorStyles.Bottom Or AnchorStyles.Left Or AnchorStyles.Right
        DataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill
        DataGridView1.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize
        DataGridView1.Location = New Point(671, 134)
        DataGridView1.MultiSelect = False
        DataGridView1.Name = "DataGridView1"
        DataGridView1.ReadOnly = True
        DataGridView1.RowHeadersWidth = 51
        DataGridView1.SelectionMode = DataGridViewSelectionMode.FullRowSelect
        DataGridView1.Size = New Size(771, 337)
        DataGridView1.TabIndex = 150
        ' 
        ' txtCodeCamion
        ' 
        txtCodeCamion.Location = New Point(200, 130)
        txtCodeCamion.Name = "txtCodeCamion"
        txtCodeCamion.Size = New Size(418, 27)
        txtCodeCamion.TabIndex = 149
        ' 
        ' btnModifier
        ' 
        btnModifier.Location = New Point(601, 50)
        btnModifier.Name = "btnModifier"
        btnModifier.Size = New Size(178, 61)
        btnModifier.TabIndex = 148
        btnModifier.Text = "EDITE"
        btnModifier.UseVisualStyleBackColor = True
        ' 
        ' RECHERCHE
        ' 
        RECHERCHE.Location = New Point(419, 50)
        RECHERCHE.Name = "RECHERCHE"
        RECHERCHE.Size = New Size(178, 61)
        RECHERCHE.TabIndex = 147
        RECHERCHE.Text = "RECHERCHE"
        RECHERCHE.UseVisualStyleBackColor = True
        ' 
        ' btnSupprimer
        ' 
        btnSupprimer.Location = New Point(230, 50)
        btnSupprimer.Name = "btnSupprimer"
        btnSupprimer.Size = New Size(178, 61)
        btnSupprimer.TabIndex = 146
        btnSupprimer.Text = "SUPRIME"
        btnSupprimer.UseVisualStyleBackColor = True
        ' 
        ' btnAjouter
        ' 
        btnAjouter.Location = New Point(42, 50)
        btnAjouter.Name = "btnAjouter"
        btnAjouter.Size = New Size(178, 61)
        btnAjouter.TabIndex = 145
        btnAjouter.Text = "AJOUTE"
        btnAjouter.UseVisualStyleBackColor = True
        ' 
        ' Label21
        ' 
        Label21.AutoSize = True
        Label21.Font = New Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point, CByte(0))
        Label21.Location = New Point(6, 9)
        Label21.Name = "Label21"
        Label21.Size = New Size(67, 28)
        Label21.TabIndex = 178
        Label21.Text = "Postes"
        ' 
        ' Postes
        ' 
        AutoScaleDimensions = New SizeF(8F, 20F)
        AutoScaleMode = AutoScaleMode.Font
        ClientSize = New Size(1729, 996)
        Controls.Add(Label21)
        Controls.Add(Label7)
        Controls.Add(dtpDerniereModification)
        Controls.Add(txtRECHERCHE)
        Controls.Add(btnActualiser)
        Controls.Add(btnNouveau)
        Controls.Add(Label8)
        Controls.Add(Label9)
        Controls.Add(Label10)
        Controls.Add(Label11)
        Controls.Add(Label12)
        Controls.Add(Label4)
        Controls.Add(Label5)
        Controls.Add(Label6)
        Controls.Add(Label3)
        Controls.Add(Label2)
        Controls.Add(Label1)
        Controls.Add(Imprimer)
        Controls.Add(txtTelephone)
        Controls.Add(txtCodeRadio)
        Controls.Add(txtChargeUtile)
        Controls.Add(txtCapacite)
        Controls.Add(txtChauffeur)
        Controls.Add(txtEquipement)
        Controls.Add(txtFamille)
        Controls.Add(txtFournisseur)
        Controls.Add(txtNomCamion)
        Controls.Add(txtCleCamion)
        Controls.Add(DataGridView1)
        Controls.Add(txtCodeCamion)
        Controls.Add(btnModifier)
        Controls.Add(RECHERCHE)
        Controls.Add(btnSupprimer)
        Controls.Add(btnAjouter)
        Name = "Postes"
        Text = "Postes"
        CType(DataGridView1, ComponentModel.ISupportInitialize).EndInit()
        ResumeLayout(False)
        PerformLayout()
    End Sub

    Friend WithEvents PrintPreviewDialog1 As PrintPreviewDialog
    Friend WithEvents Label7 As Label
    Friend WithEvents dtpDerniereModification As TextBox
    Friend WithEvents PrintDocument1 As Printing.PrintDocument
    Friend WithEvents txtRECHERCHE As TextBox
    Friend WithEvents btnActualiser As Button
    Friend WithEvents btnNouveau As Button
    Friend WithEvents Label8 As Label
    Friend WithEvents Label9 As Label
    Friend WithEvents Label10 As Label
    Friend WithEvents Label11 As Label
    Friend WithEvents Label12 As Label
    Friend WithEvents Label4 As Label
    Friend WithEvents Label5 As Label
    Friend WithEvents Label6 As Label
    Friend WithEvents Label3 As Label
    Friend WithEvents Label2 As Label
    Friend WithEvents Label1 As Label
    Friend WithEvents Imprimer As Button
    Friend WithEvents txtTelephone As TextBox
    Friend WithEvents txtCodeRadio As TextBox
    Friend WithEvents txtChargeUtile As TextBox
    Friend WithEvents txtCapacite As TextBox
    Friend WithEvents txtChauffeur As TextBox
    Friend WithEvents txtEquipement As TextBox
    Friend WithEvents txtFamille As TextBox
    Friend WithEvents txtFournisseur As TextBox
    Friend WithEvents txtNomCamion As TextBox
    Friend WithEvents txtCleCamion As TextBox
    Friend WithEvents DataGridView1 As DataGridView
    Friend WithEvents txtCodeCamion As TextBox
    Friend WithEvents btnModifier As Button
    Friend WithEvents RECHERCHE As Button
    Friend WithEvents btnSupprimer As Button
    Friend WithEvents btnAjouter As Button
    Friend WithEvents Label21 As Label
End Class
