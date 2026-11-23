<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class Chantiers
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
        txtValeurActuelle = New TextBox()
        Label18 = New Label()
        Label7 = New Label()
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
        CheckBoxVerrouille = New CheckBox()
        CheckBoxControleCredit = New CheckBox()
        CheckBoxAutorisationDosage = New CheckBox()
        txtMarcheWattmetre = New TextBox()
        txtNatureChantier = New TextBox()
        txtPlusValue = New TextBox()
        txtZoneTransport = New TextBox()
        txtZoneFacturation = New TextBox()
        txtTelephone = New TextBox()
        txtPays = New TextBox()
        txtCodePostal = New TextBox()
        txtRue = New TextBox()
        txtNomChantier = New TextBox()
        txtCleChantier = New TextBox()
        txtCodeChantier = New TextBox()
        DataGridView1 = New DataGridView()
        txtIdChantier = New TextBox()
        btnModifier = New Button()
        btnActualiser = New Button()
        btnSupprimer = New Button()
        btnAjouter = New Button()
        Label13 = New Label()
        Label14 = New Label()
        Label15 = New Label()
        Label16 = New Label()
        Label17 = New Label()
        txtDerniereModification = New TextBox()
        txtCommande = New TextBox()
        txtCommandeObligatoire = New TextBox()
        txtCreationChantierBL = New TextBox()
        txtImpressionPeses = New TextBox()
        Label19 = New Label()
        txtVille = New TextBox()
        Label20 = New Label()
        txtTypeChantier = New TextBox()
        btnNouveau = New Button()
        Label21 = New Label()
        Label22 = New Label()
        Label23 = New Label()
        Label24 = New Label()
        Label25 = New Label()
        GroupBox1 = New GroupBox()
        cmdNomClient = New ComboBox()
        cmdCodeClient = New ComboBox()
        cmdIdClient = New ComboBox()
        CType(DataGridView1, ComponentModel.ISupportInitialize).BeginInit()
        GroupBox1.SuspendLayout()
        SuspendLayout()
        ' 
        ' txtValeurActuelle
        ' 
        txtValeurActuelle.Location = New Point(112, 684)
        txtValeurActuelle.Name = "txtValeurActuelle"
        txtValeurActuelle.Size = New Size(418, 27)
        txtValeurActuelle.TabIndex = 76
        ' 
        ' Label18
        ' 
        Label18.AutoSize = True
        Label18.Location = New Point(34, 500)
        Label18.Name = "Label18"
        Label18.Size = New Size(132, 20)
        Label18.TabIndex = 75
        Label18.Text = "Marche Wattmetre"
        ' 
        ' Label7
        ' 
        Label7.AutoSize = True
        Label7.Location = New Point(34, 435)
        Label7.Name = "Label7"
        Label7.Size = New Size(113, 20)
        Label7.TabIndex = 74
        Label7.Text = "Nature Chantier"
        ' 
        ' Label8
        ' 
        Label8.AutoSize = True
        Label8.Location = New Point(34, 398)
        Label8.Name = "Label8"
        Label8.Size = New Size(75, 20)
        Label8.TabIndex = 73
        Label8.Text = "Plus Value"
        ' 
        ' Label9
        ' 
        Label9.AutoSize = True
        Label9.ImageAlign = ContentAlignment.BottomLeft
        Label9.Location = New Point(36, 365)
        Label9.Name = "Label9"
        Label9.Size = New Size(109, 20)
        Label9.TabIndex = 72
        Label9.Text = "Zone Transport"
        ' 
        ' Label10
        ' 
        Label10.AutoSize = True
        Label10.Location = New Point(39, 332)
        Label10.Name = "Label10"
        Label10.Size = New Size(120, 20)
        Label10.TabIndex = 71
        Label10.Text = "Zone Facturation"
        ' 
        ' Label11
        ' 
        Label11.AutoSize = True
        Label11.Location = New Point(37, 299)
        Label11.Name = "Label11"
        Label11.Size = New Size(78, 20)
        Label11.TabIndex = 70
        Label11.Text = "Téléphone"
        ' 
        ' Label12
        ' 
        Label12.AutoSize = True
        Label12.Location = New Point(39, 266)
        Label12.Name = "Label12"
        Label12.Size = New Size(37, 20)
        Label12.TabIndex = 69
        Label12.Text = "Pays"
        ' 
        ' Label4
        ' 
        Label4.AutoSize = True
        Label4.Location = New Point(37, 233)
        Label4.Name = "Label4"
        Label4.Size = New Size(87, 20)
        Label4.TabIndex = 68
        Label4.Text = "Code Postal"
        ' 
        ' Label5
        ' 
        Label5.AutoSize = True
        Label5.Location = New Point(37, 170)
        Label5.Name = "Label5"
        Label5.Size = New Size(34, 20)
        Label5.TabIndex = 67
        Label5.Text = "Rue"
        ' 
        ' Label6
        ' 
        Label6.AutoSize = True
        Label6.Location = New Point(34, 137)
        Label6.Name = "Label6"
        Label6.Size = New Size(101, 20)
        Label6.TabIndex = 66
        Label6.Text = "Nom Chantier"
        ' 
        ' Label3
        ' 
        Label3.AutoSize = True
        Label3.Location = New Point(382, 97)
        Label3.Name = "Label3"
        Label3.Size = New Size(89, 20)
        Label3.TabIndex = 65
        Label3.Text = "Cle Chantier"
        ' 
        ' Label2
        ' 
        Label2.AutoSize = True
        Label2.Location = New Point(199, 97)
        Label2.Name = "Label2"
        Label2.Size = New Size(103, 20)
        Label2.TabIndex = 64
        Label2.Text = "Code Chantier"
        ' 
        ' Label1
        ' 
        Label1.AutoSize = True
        Label1.Location = New Point(34, 97)
        Label1.Name = "Label1"
        Label1.Size = New Size(81, 20)
        Label1.TabIndex = 63
        Label1.Text = "Id Chantier"
        ' 
        ' Imprimer
        ' 
        Imprimer.Location = New Point(694, 26)
        Imprimer.Name = "Imprimer"
        Imprimer.Size = New Size(140, 61)
        Imprimer.TabIndex = 62
        Imprimer.Text = "Imprimer"
        Imprimer.UseVisualStyleBackColor = True
        ' 
        ' CheckBoxVerrouille
        ' 
        CheckBoxVerrouille.AutoSize = True
        CheckBoxVerrouille.Location = New Point(188, 658)
        CheckBoxVerrouille.Name = "CheckBoxVerrouille"
        CheckBoxVerrouille.Size = New Size(94, 24)
        CheckBoxVerrouille.TabIndex = 61
        CheckBoxVerrouille.Text = "Verrouillé"
        CheckBoxVerrouille.UseVisualStyleBackColor = True
        ' 
        ' CheckBoxControleCredit
        ' 
        CheckBoxControleCredit.AutoSize = True
        CheckBoxControleCredit.Location = New Point(52, 655)
        CheckBoxControleCredit.Name = "CheckBoxControleCredit"
        CheckBoxControleCredit.Size = New Size(130, 24)
        CheckBoxControleCredit.TabIndex = 60
        CheckBoxControleCredit.Text = "Contrôle crédit"
        CheckBoxControleCredit.UseVisualStyleBackColor = True
        ' 
        ' CheckBoxAutorisationDosage
        ' 
        CheckBoxAutorisationDosage.AutoSize = True
        CheckBoxAutorisationDosage.Location = New Point(288, 655)
        CheckBoxAutorisationDosage.Name = "CheckBoxAutorisationDosage"
        CheckBoxAutorisationDosage.Size = New Size(165, 24)
        CheckBoxAutorisationDosage.TabIndex = 59
        CheckBoxAutorisationDosage.Text = "Autorisation dosage"
        CheckBoxAutorisationDosage.UseVisualStyleBackColor = True
        ' 
        ' txtMarcheWattmetre
        ' 
        txtMarcheWattmetre.Location = New Point(204, 490)
        txtMarcheWattmetre.Name = "txtMarcheWattmetre"
        txtMarcheWattmetre.Size = New Size(349, 27)
        txtMarcheWattmetre.TabIndex = 58
        ' 
        ' txtNatureChantier
        ' 
        txtNatureChantier.Location = New Point(204, 424)
        txtNatureChantier.Name = "txtNatureChantier"
        txtNatureChantier.Size = New Size(349, 27)
        txtNatureChantier.TabIndex = 57
        ' 
        ' txtPlusValue
        ' 
        txtPlusValue.Location = New Point(204, 391)
        txtPlusValue.Name = "txtPlusValue"
        txtPlusValue.Size = New Size(349, 27)
        txtPlusValue.TabIndex = 56
        ' 
        ' txtZoneTransport
        ' 
        txtZoneTransport.Location = New Point(204, 358)
        txtZoneTransport.Name = "txtZoneTransport"
        txtZoneTransport.Size = New Size(349, 27)
        txtZoneTransport.TabIndex = 55
        ' 
        ' txtZoneFacturation
        ' 
        txtZoneFacturation.Location = New Point(204, 325)
        txtZoneFacturation.Name = "txtZoneFacturation"
        txtZoneFacturation.Size = New Size(349, 27)
        txtZoneFacturation.TabIndex = 54
        ' 
        ' txtTelephone
        ' 
        txtTelephone.Location = New Point(204, 292)
        txtTelephone.Name = "txtTelephone"
        txtTelephone.Size = New Size(349, 27)
        txtTelephone.TabIndex = 53
        ' 
        ' txtPays
        ' 
        txtPays.Location = New Point(204, 259)
        txtPays.Name = "txtPays"
        txtPays.Size = New Size(349, 27)
        txtPays.TabIndex = 52
        ' 
        ' txtCodePostal
        ' 
        txtCodePostal.Location = New Point(204, 226)
        txtCodePostal.Name = "txtCodePostal"
        txtCodePostal.Size = New Size(349, 27)
        txtCodePostal.TabIndex = 51
        ' 
        ' txtRue
        ' 
        txtRue.Location = New Point(204, 163)
        txtRue.Name = "txtRue"
        txtRue.Size = New Size(349, 27)
        txtRue.TabIndex = 50
        ' 
        ' txtNomChantier
        ' 
        txtNomChantier.Location = New Point(204, 130)
        txtNomChantier.Name = "txtNomChantier"
        txtNomChantier.Size = New Size(349, 27)
        txtNomChantier.TabIndex = 49
        ' 
        ' txtCleChantier
        ' 
        txtCleChantier.Location = New Point(471, 93)
        txtCleChantier.Name = "txtCleChantier"
        txtCleChantier.Size = New Size(86, 27)
        txtCleChantier.TabIndex = 48
        ' 
        ' txtCodeChantier
        ' 
        txtCodeChantier.Location = New Point(297, 93)
        txtCodeChantier.Name = "txtCodeChantier"
        txtCodeChantier.Size = New Size(86, 27)
        txtCodeChantier.TabIndex = 47
        ' 
        ' DataGridView1
        ' 
        DataGridView1.AllowUserToAddRows = False
        DataGridView1.Anchor = AnchorStyles.Top Or AnchorStyles.Bottom Or AnchorStyles.Left Or AnchorStyles.Right
        DataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill
        DataGridView1.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize
        DataGridView1.Location = New Point(613, 89)
        DataGridView1.MultiSelect = False
        DataGridView1.Name = "DataGridView1"
        DataGridView1.ReadOnly = True
        DataGridView1.RowHeadersWidth = 51
        DataGridView1.SelectionMode = DataGridViewSelectionMode.FullRowSelect
        DataGridView1.Size = New Size(613, 724)
        DataGridView1.TabIndex = 46
        ' 
        ' txtIdChantier
        ' 
        txtIdChantier.Location = New Point(117, 93)
        txtIdChantier.Name = "txtIdChantier"
        txtIdChantier.Size = New Size(86, 27)
        txtIdChantier.TabIndex = 45
        ' 
        ' btnModifier
        ' 
        btnModifier.Location = New Point(548, 26)
        btnModifier.Name = "btnModifier"
        btnModifier.Size = New Size(140, 61)
        btnModifier.TabIndex = 44
        btnModifier.Text = "EDITE"
        btnModifier.UseVisualStyleBackColor = True
        ' 
        ' btnActualiser
        ' 
        btnActualiser.Location = New Point(402, 26)
        btnActualiser.Name = "btnActualiser"
        btnActualiser.Size = New Size(140, 61)
        btnActualiser.TabIndex = 43
        btnActualiser.Text = "Actualiser"
        btnActualiser.UseVisualStyleBackColor = True
        ' 
        ' btnSupprimer
        ' 
        btnSupprimer.Location = New Point(256, 26)
        btnSupprimer.Name = "btnSupprimer"
        btnSupprimer.Size = New Size(140, 61)
        btnSupprimer.TabIndex = 42
        btnSupprimer.Text = "SUPRIME"
        btnSupprimer.UseVisualStyleBackColor = True
        ' 
        ' btnAjouter
        ' 
        btnAjouter.Location = New Point(110, 26)
        btnAjouter.Name = "btnAjouter"
        btnAjouter.Size = New Size(140, 61)
        btnAjouter.TabIndex = 41
        btnAjouter.Text = "AJOUTER"
        btnAjouter.UseVisualStyleBackColor = True
        ' 
        ' Label13
        ' 
        Label13.AutoSize = True
        Label13.Location = New Point(34, 632)
        Label13.Name = "Label13"
        Label13.Size = New Size(168, 20)
        Label13.TabIndex = 86
        Label13.Text = "txtDerniereModification"
        ' 
        ' Label14
        ' 
        Label14.AutoSize = True
        Label14.Location = New Point(34, 600)
        Label14.Name = "Label14"
        Label14.Size = New Size(86, 20)
        Label14.TabIndex = 85
        Label14.Text = "Commande"
        ' 
        ' Label15
        ' 
        Label15.AutoSize = True
        Label15.Location = New Point(308, 564)
        Label15.Name = "Label15"
        Label15.Size = New Size(166, 20)
        Label15.TabIndex = 84
        Label15.Text = "Commande Obligatoire"
        ' 
        ' Label16
        ' 
        Label16.AutoSize = True
        Label16.ImageAlign = ContentAlignment.BottomLeft
        Label16.Location = New Point(36, 564)
        Label16.Name = "Label16"
        Label16.Size = New Size(144, 20)
        Label16.TabIndex = 83
        Label16.Text = "Création Chantier BL"
        ' 
        ' Label17
        ' 
        Label17.AutoSize = True
        Label17.Location = New Point(34, 527)
        Label17.Name = "Label17"
        Label17.Size = New Size(120, 20)
        Label17.TabIndex = 82
        Label17.Text = "Impression Peses"
        ' 
        ' txtDerniereModification
        ' 
        txtDerniereModification.Location = New Point(204, 622)
        txtDerniereModification.Name = "txtDerniereModification"
        txtDerniereModification.Size = New Size(349, 27)
        txtDerniereModification.TabIndex = 81
        ' 
        ' txtCommande
        ' 
        txtCommande.Location = New Point(204, 589)
        txtCommande.Name = "txtCommande"
        txtCommande.Size = New Size(349, 27)
        txtCommande.TabIndex = 80
        ' 
        ' txtCommandeObligatoire
        ' 
        txtCommandeObligatoire.Location = New Point(478, 557)
        txtCommandeObligatoire.Name = "txtCommandeObligatoire"
        txtCommandeObligatoire.Size = New Size(78, 27)
        txtCommandeObligatoire.TabIndex = 79
        ' 
        ' txtCreationChantierBL
        ' 
        txtCreationChantierBL.Location = New Point(204, 557)
        txtCreationChantierBL.Name = "txtCreationChantierBL"
        txtCreationChantierBL.Size = New Size(78, 27)
        txtCreationChantierBL.TabIndex = 78
        ' 
        ' txtImpressionPeses
        ' 
        txtImpressionPeses.Location = New Point(204, 524)
        txtImpressionPeses.Name = "txtImpressionPeses"
        txtImpressionPeses.Size = New Size(349, 27)
        txtImpressionPeses.TabIndex = 77
        ' 
        ' Label19
        ' 
        Label19.AutoSize = True
        Label19.Location = New Point(39, 203)
        Label19.Name = "Label19"
        Label19.Size = New Size(38, 20)
        Label19.TabIndex = 88
        Label19.Text = "Ville"
        ' 
        ' txtVille
        ' 
        txtVille.Location = New Point(204, 193)
        txtVille.Name = "txtVille"
        txtVille.Size = New Size(349, 27)
        txtVille.TabIndex = 87
        ' 
        ' Label20
        ' 
        Label20.AutoSize = True
        Label20.Location = New Point(36, 467)
        Label20.Name = "Label20"
        Label20.Size = New Size(99, 20)
        Label20.TabIndex = 90
        Label20.Text = "Type Chantier"
        ' 
        ' txtTypeChantier
        ' 
        txtTypeChantier.Location = New Point(204, 457)
        txtTypeChantier.Name = "txtTypeChantier"
        txtTypeChantier.Size = New Size(349, 27)
        txtTypeChantier.TabIndex = 89
        ' 
        ' btnNouveau
        ' 
        btnNouveau.DialogResult = DialogResult.Continue
        btnNouveau.Location = New Point(840, 26)
        btnNouveau.Name = "btnNouveau"
        btnNouveau.Size = New Size(140, 61)
        btnNouveau.TabIndex = 91
        btnNouveau.Text = "VIDE"
        btnNouveau.UseVisualStyleBackColor = True
        ' 
        ' Label21
        ' 
        Label21.AutoSize = True
        Label21.Font = New Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point, CByte(0))
        Label21.Location = New Point(56, -2)
        Label21.Name = "Label21"
        Label21.Size = New Size(85, 28)
        Label21.TabIndex = 92
        Label21.Text = "Chantier"
        ' 
        ' Label22
        ' 
        Label22.AutoSize = True
        Label22.Font = New Font("Segoe UI", 13.8F, FontStyle.Regular, GraphicsUnit.Point, CByte(0))
        Label22.Location = New Point(635, 568)
        Label22.Name = "Label22"
        Label22.Size = New Size(0, 31)
        Label22.TabIndex = 256
        ' 
        ' Label23
        ' 
        Label23.AutoSize = True
        Label23.Location = New Point(149, 58)
        Label23.Name = "Label23"
        Label23.Size = New Size(42, 20)
        Label23.TabIndex = 255
        Label23.Text = "Nom"
        ' 
        ' Label24
        ' 
        Label24.AutoSize = True
        Label24.Location = New Point(258, 24)
        Label24.Name = "Label24"
        Label24.Size = New Size(44, 20)
        Label24.TabIndex = 254
        Label24.Text = "Code"
        ' 
        ' Label25
        ' 
        Label25.AutoSize = True
        Label25.Location = New Point(18, 24)
        Label25.Name = "Label25"
        Label25.Size = New Size(42, 20)
        Label25.TabIndex = 257
        Label25.Text = "Nom"
        ' 
        ' GroupBox1
        ' 
        GroupBox1.Controls.Add(cmdNomClient)
        GroupBox1.Controls.Add(cmdCodeClient)
        GroupBox1.Controls.Add(cmdIdClient)
        GroupBox1.Controls.Add(Label25)
        GroupBox1.Controls.Add(Label23)
        GroupBox1.Controls.Add(Label24)
        GroupBox1.Location = New Point(39, 706)
        GroupBox1.Name = "GroupBox1"
        GroupBox1.Size = New Size(514, 107)
        GroupBox1.TabIndex = 259
        GroupBox1.TabStop = False
        GroupBox1.Text = "Client"
        ' 
        ' cmdNomClient
        ' 
        cmdNomClient.AutoCompleteMode = AutoCompleteMode.SuggestAppend
        cmdNomClient.AutoCompleteSource = AutoCompleteSource.ListItems
        cmdNomClient.FormattingEnabled = True
        cmdNomClient.Location = New Point(219, 55)
        cmdNomClient.Name = "cmdNomClient"
        cmdNomClient.Size = New Size(171, 28)
        cmdNomClient.TabIndex = 262
        ' 
        ' cmdCodeClient
        ' 
        cmdCodeClient.AutoCompleteMode = AutoCompleteMode.SuggestAppend
        cmdCodeClient.AutoCompleteSource = AutoCompleteSource.ListItems
        cmdCodeClient.FormattingEnabled = True
        cmdCodeClient.Location = New Point(320, 21)
        cmdCodeClient.Name = "cmdCodeClient"
        cmdCodeClient.Size = New Size(171, 28)
        cmdCodeClient.TabIndex = 261
        ' 
        ' cmdIdClient
        ' 
        cmdIdClient.AutoCompleteMode = AutoCompleteMode.SuggestAppend
        cmdIdClient.AutoCompleteSource = AutoCompleteSource.ListItems
        cmdIdClient.FormattingEnabled = True
        cmdIdClient.Location = New Point(66, 21)
        cmdIdClient.Name = "cmdIdClient"
        cmdIdClient.Size = New Size(171, 28)
        cmdIdClient.TabIndex = 260
        ' 
        ' Chantiers
        ' 
        AutoScaleDimensions = New SizeF(8F, 20F)
        AutoScaleMode = AutoScaleMode.Font
        ClientSize = New Size(1238, 828)
        Controls.Add(GroupBox1)
        Controls.Add(Label22)
        Controls.Add(Label21)
        Controls.Add(btnNouveau)
        Controls.Add(Label20)
        Controls.Add(txtTypeChantier)
        Controls.Add(Label19)
        Controls.Add(txtVille)
        Controls.Add(Label13)
        Controls.Add(Label14)
        Controls.Add(Label15)
        Controls.Add(Label16)
        Controls.Add(Label17)
        Controls.Add(txtDerniereModification)
        Controls.Add(txtCommande)
        Controls.Add(txtCommandeObligatoire)
        Controls.Add(txtCreationChantierBL)
        Controls.Add(txtImpressionPeses)
        Controls.Add(txtValeurActuelle)
        Controls.Add(Label18)
        Controls.Add(Label7)
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
        Controls.Add(CheckBoxVerrouille)
        Controls.Add(CheckBoxControleCredit)
        Controls.Add(CheckBoxAutorisationDosage)
        Controls.Add(txtMarcheWattmetre)
        Controls.Add(txtNatureChantier)
        Controls.Add(txtPlusValue)
        Controls.Add(txtZoneTransport)
        Controls.Add(txtZoneFacturation)
        Controls.Add(txtTelephone)
        Controls.Add(txtPays)
        Controls.Add(txtCodePostal)
        Controls.Add(txtRue)
        Controls.Add(txtNomChantier)
        Controls.Add(txtCleChantier)
        Controls.Add(txtCodeChantier)
        Controls.Add(DataGridView1)
        Controls.Add(txtIdChantier)
        Controls.Add(btnModifier)
        Controls.Add(btnActualiser)
        Controls.Add(btnSupprimer)
        Controls.Add(btnAjouter)
        Name = "Chantiers"
        Text = "Chantiers"
        CType(DataGridView1, ComponentModel.ISupportInitialize).EndInit()
        GroupBox1.ResumeLayout(False)
        GroupBox1.PerformLayout()
        ResumeLayout(False)
        PerformLayout()
    End Sub

    Friend WithEvents txtValeurActuelle As TextBox
    Friend WithEvents Label18 As Label
    Friend WithEvents Label7 As Label
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
    Friend WithEvents CheckBoxVerrouille As CheckBox
    Friend WithEvents CheckBoxControleCredit As CheckBox
    Friend WithEvents CheckBoxAutorisationDosage As CheckBox
    Friend WithEvents txtMarcheWattmetre As TextBox
    Friend WithEvents txtNatureChantier As TextBox
    Friend WithEvents txtPlusValue As TextBox
    Friend WithEvents txtZoneTransport As TextBox
    Friend WithEvents txtZoneFacturation As TextBox
    Friend WithEvents txtTelephone As TextBox
    Friend WithEvents txtPays As TextBox
    Friend WithEvents txtCodePostal As TextBox
    Friend WithEvents txtRue As TextBox
    Friend WithEvents txtNomChantier As TextBox
    Friend WithEvents txtCleChantier As TextBox
    Friend WithEvents txtCodeChantier As TextBox
    Friend WithEvents DataGridView1 As DataGridView
    Friend WithEvents txtIdChantier As TextBox
    Friend WithEvents btnModifier As Button
    Friend WithEvents btnActualiser As Button
    Friend WithEvents btnSupprimer As Button
    Friend WithEvents btnAjouter As Button
    Friend WithEvents Label13 As Label
    Friend WithEvents Label14 As Label
    Friend WithEvents Label15 As Label
    Friend WithEvents Label16 As Label
    Friend WithEvents Label17 As Label
    Friend WithEvents txtDerniereModification As TextBox
    Friend WithEvents txtCommande As TextBox
    Friend WithEvents txtCommandeObligatoire As TextBox
    Friend WithEvents txtCreationChantierBL As TextBox
    Friend WithEvents txtImpressionPeses As TextBox
    Friend WithEvents Label19 As Label
    Friend WithEvents txtVille As TextBox
    Friend WithEvents Label20 As Label
    Friend WithEvents txtTypeChantier As TextBox
    Friend WithEvents btnNouveau As Button
    Friend WithEvents Label21 As Label
    Friend WithEvents Label22 As Label
    Friend WithEvents Label23 As Label
    Friend WithEvents Label24 As Label
    Friend WithEvents Label25 As Label
    Friend WithEvents GroupBox1 As GroupBox
    Friend WithEvents cmdNomClient As ComboBox
    Friend WithEvents cmdCodeClient As ComboBox
    Friend WithEvents cmdIdClient As ComboBox
End Class
