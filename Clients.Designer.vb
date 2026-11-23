<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class Clients
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
        SUPRIME = New Button()
        RECHERCHE = New Button()
        AJOUTE = New Button()
        EDITE = New Button()
        TextBoxCode = New TextBox()
        DataGridView1 = New DataGridView()
        TextBoxNom = New TextBox()
        TextBoxCodePostal = New TextBox()
        TextBoxAdresse = New TextBox()
        TextBoxFax = New TextBox()
        TextBoxTelephone = New TextBox()
        TextBoxTVA = New TextBox()
        TextBoxPays = New TextBox()
        TextBoxRemarques = New TextBox()
        TextBoxContacts = New TextBox()
        TextBoxRadial = New TextBox()
        TextBoxZone = New TextBox()
        TextBoxEmail = New TextBox()
        CheckBoxAutorisationDosage = New CheckBox()
        CheckBoxControleCredit = New CheckBox()
        CheckBoxVerrouille = New CheckBox()
        Imprimer = New Button()
        Label1 = New Label()
        Label2 = New Label()
        Label3 = New Label()
        Label4 = New Label()
        Label5 = New Label()
        Label6 = New Label()
        Label7 = New Label()
        Label8 = New Label()
        Label9 = New Label()
        Label10 = New Label()
        Label11 = New Label()
        Label12 = New Label()
        Label18 = New Label()
        Label13 = New Label()
        BTN_CHANTIER = New Button()
        code = New Label()
        TextBoxId = New TextBox()
        CType(DataGridView1, ComponentModel.ISupportInitialize).BeginInit()
        SuspendLayout()
        ' 
        ' SUPRIME
        ' 
        SUPRIME.Location = New Point(714, 38)
        SUPRIME.Name = "SUPRIME"
        SUPRIME.Size = New Size(160, 61)
        SUPRIME.TabIndex = 1
        SUPRIME.Text = "SUPRIME"
        SUPRIME.UseVisualStyleBackColor = True
        ' 
        ' RECHERCHE
        ' 
        RECHERCHE.Location = New Point(540, 38)
        RECHERCHE.Name = "RECHERCHE"
        RECHERCHE.Size = New Size(160, 61)
        RECHERCHE.TabIndex = 2
        RECHERCHE.Text = "RECHERCHE"
        RECHERCHE.UseVisualStyleBackColor = True
        ' 
        ' AJOUTE
        ' 
        AJOUTE.Location = New Point(36, 38)
        AJOUTE.Name = "AJOUTE"
        AJOUTE.Size = New Size(160, 61)
        AJOUTE.TabIndex = 0
        AJOUTE.Text = "AJOUTER"
        AJOUTE.UseVisualStyleBackColor = True
        ' 
        ' EDITE
        ' 
        EDITE.Location = New Point(204, 38)
        EDITE.Name = "EDITE"
        EDITE.Size = New Size(160, 61)
        EDITE.TabIndex = 3
        EDITE.Text = "EDITE"
        EDITE.UseVisualStyleBackColor = True
        ' 
        ' TextBoxCode
        ' 
        TextBoxCode.Location = New Point(166, 167)
        TextBoxCode.Name = "TextBoxCode"
        TextBoxCode.Size = New Size(346, 27)
        TextBoxCode.TabIndex = 4
        ' 
        ' DataGridView1
        ' 
        DataGridView1.AllowUserToAddRows = False
        DataGridView1.Anchor = AnchorStyles.Top Or AnchorStyles.Bottom Or AnchorStyles.Left Or AnchorStyles.Right
        DataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill
        DataGridView1.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize
        DataGridView1.Location = New Point(540, 109)
        DataGridView1.MultiSelect = False
        DataGridView1.Name = "DataGridView1"
        DataGridView1.ReadOnly = True
        DataGridView1.RowHeadersWidth = 51
        DataGridView1.SelectionMode = DataGridViewSelectionMode.FullRowSelect
        DataGridView1.Size = New Size(530, 571)
        DataGridView1.TabIndex = 5
        ' 
        ' TextBoxNom
        ' 
        TextBoxNom.Location = New Point(166, 200)
        TextBoxNom.Name = "TextBoxNom"
        TextBoxNom.Size = New Size(346, 27)
        TextBoxNom.TabIndex = 6
        ' 
        ' TextBoxCodePostal
        ' 
        TextBoxCodePostal.Location = New Point(166, 266)
        TextBoxCodePostal.Name = "TextBoxCodePostal"
        TextBoxCodePostal.Size = New Size(346, 27)
        TextBoxCodePostal.TabIndex = 8
        ' 
        ' TextBoxAdresse
        ' 
        TextBoxAdresse.Location = New Point(166, 233)
        TextBoxAdresse.Name = "TextBoxAdresse"
        TextBoxAdresse.Size = New Size(346, 27)
        TextBoxAdresse.TabIndex = 7
        ' 
        ' TextBoxFax
        ' 
        TextBoxFax.Location = New Point(166, 398)
        TextBoxFax.Name = "TextBoxFax"
        TextBoxFax.Size = New Size(346, 27)
        TextBoxFax.TabIndex = 12
        ' 
        ' TextBoxTelephone
        ' 
        TextBoxTelephone.Location = New Point(166, 365)
        TextBoxTelephone.Name = "TextBoxTelephone"
        TextBoxTelephone.Size = New Size(346, 27)
        TextBoxTelephone.TabIndex = 11
        ' 
        ' TextBoxTVA
        ' 
        TextBoxTVA.Location = New Point(166, 332)
        TextBoxTVA.Name = "TextBoxTVA"
        TextBoxTVA.Size = New Size(346, 27)
        TextBoxTVA.TabIndex = 10
        ' 
        ' TextBoxPays
        ' 
        TextBoxPays.Location = New Point(166, 299)
        TextBoxPays.Name = "TextBoxPays"
        TextBoxPays.Size = New Size(346, 27)
        TextBoxPays.TabIndex = 9
        ' 
        ' TextBoxRemarques
        ' 
        TextBoxRemarques.Location = New Point(166, 563)
        TextBoxRemarques.Name = "TextBoxRemarques"
        TextBoxRemarques.Size = New Size(346, 27)
        TextBoxRemarques.TabIndex = 17
        ' 
        ' TextBoxContacts
        ' 
        TextBoxContacts.Location = New Point(166, 530)
        TextBoxContacts.Name = "TextBoxContacts"
        TextBoxContacts.Size = New Size(346, 27)
        TextBoxContacts.TabIndex = 16
        ' 
        ' TextBoxRadial
        ' 
        TextBoxRadial.Location = New Point(166, 497)
        TextBoxRadial.Name = "TextBoxRadial"
        TextBoxRadial.Size = New Size(346, 27)
        TextBoxRadial.TabIndex = 15
        ' 
        ' TextBoxZone
        ' 
        TextBoxZone.Location = New Point(166, 464)
        TextBoxZone.Name = "TextBoxZone"
        TextBoxZone.Size = New Size(346, 27)
        TextBoxZone.TabIndex = 14
        ' 
        ' TextBoxEmail
        ' 
        TextBoxEmail.Location = New Point(166, 431)
        TextBoxEmail.Name = "TextBoxEmail"
        TextBoxEmail.Size = New Size(346, 27)
        TextBoxEmail.TabIndex = 13
        ' 
        ' CheckBoxAutorisationDosage
        ' 
        CheckBoxAutorisationDosage.AutoSize = True
        CheckBoxAutorisationDosage.Location = New Point(243, 606)
        CheckBoxAutorisationDosage.Name = "CheckBoxAutorisationDosage"
        CheckBoxAutorisationDosage.Size = New Size(165, 24)
        CheckBoxAutorisationDosage.TabIndex = 23
        CheckBoxAutorisationDosage.Text = "Autorisation dosage"
        CheckBoxAutorisationDosage.UseVisualStyleBackColor = True
        ' 
        ' CheckBoxControleCredit
        ' 
        CheckBoxControleCredit.AutoSize = True
        CheckBoxControleCredit.Location = New Point(243, 636)
        CheckBoxControleCredit.Name = "CheckBoxControleCredit"
        CheckBoxControleCredit.Size = New Size(130, 24)
        CheckBoxControleCredit.TabIndex = 24
        CheckBoxControleCredit.Text = "Contrôle crédit"
        CheckBoxControleCredit.UseVisualStyleBackColor = True
        ' 
        ' CheckBoxVerrouille
        ' 
        CheckBoxVerrouille.AutoSize = True
        CheckBoxVerrouille.Location = New Point(243, 666)
        CheckBoxVerrouille.Name = "CheckBoxVerrouille"
        CheckBoxVerrouille.Size = New Size(94, 24)
        CheckBoxVerrouille.TabIndex = 25
        CheckBoxVerrouille.Text = "Verrouillé"
        CheckBoxVerrouille.UseVisualStyleBackColor = True
        ' 
        ' Imprimer
        ' 
        Imprimer.Location = New Point(884, 38)
        Imprimer.Name = "Imprimer"
        Imprimer.Size = New Size(160, 61)
        Imprimer.TabIndex = 26
        Imprimer.Text = "Imprimer"
        Imprimer.UseVisualStyleBackColor = True
        ' 
        ' Label1
        ' 
        Label1.AutoSize = True
        Label1.Location = New Point(50, 171)
        Label1.Name = "Label1"
        Label1.Size = New Size(44, 20)
        Label1.TabIndex = 27
        Label1.Text = "Code"
        ' 
        ' Label2
        ' 
        Label2.AutoSize = True
        Label2.Location = New Point(50, 204)
        Label2.Name = "Label2"
        Label2.Size = New Size(42, 20)
        Label2.TabIndex = 28
        Label2.Text = "Nom"
        ' 
        ' Label3
        ' 
        Label3.AutoSize = True
        Label3.Location = New Point(50, 237)
        Label3.Name = "Label3"
        Label3.Size = New Size(61, 20)
        Label3.TabIndex = 29
        Label3.Text = "Adresse"
        ' 
        ' Label4
        ' 
        Label4.AutoSize = True
        Label4.Location = New Point(50, 336)
        Label4.Name = "Label4"
        Label4.Size = New Size(30, 20)
        Label4.TabIndex = 32
        Label4.Text = "ICE"
        ' 
        ' Label5
        ' 
        Label5.AutoSize = True
        Label5.Location = New Point(50, 303)
        Label5.Name = "Label5"
        Label5.Size = New Size(37, 20)
        Label5.TabIndex = 31
        Label5.Text = "Pays"
        ' 
        ' Label6
        ' 
        Label6.AutoSize = True
        Label6.Location = New Point(47, 270)
        Label6.Name = "Label6"
        Label6.Size = New Size(83, 20)
        Label6.TabIndex = 30
        Label6.Text = "CodePostal"
        ' 
        ' Label7
        ' 
        Label7.AutoSize = True
        Label7.Location = New Point(47, 538)
        Label7.Name = "Label7"
        Label7.Size = New Size(66, 20)
        Label7.TabIndex = 38
        Label7.Text = "Contacts"
        ' 
        ' Label8
        ' 
        Label8.AutoSize = True
        Label8.Location = New Point(47, 501)
        Label8.Name = "Label8"
        Label8.Size = New Size(51, 20)
        Label8.TabIndex = 37
        Label8.Text = "Radial"
        ' 
        ' Label9
        ' 
        Label9.AutoSize = True
        Label9.Location = New Point(49, 468)
        Label9.Name = "Label9"
        Label9.Size = New Size(43, 20)
        Label9.TabIndex = 36
        Label9.Text = "Zone"
        ' 
        ' Label10
        ' 
        Label10.AutoSize = True
        Label10.Location = New Point(52, 435)
        Label10.Name = "Label10"
        Label10.Size = New Size(46, 20)
        Label10.TabIndex = 35
        Label10.Text = "Email"
        ' 
        ' Label11
        ' 
        Label11.AutoSize = True
        Label11.Location = New Point(50, 402)
        Label11.Name = "Label11"
        Label11.Size = New Size(30, 20)
        Label11.TabIndex = 34
        Label11.Text = "Fax"
        ' 
        ' Label12
        ' 
        Label12.AutoSize = True
        Label12.Location = New Point(52, 369)
        Label12.Name = "Label12"
        Label12.Size = New Size(78, 20)
        Label12.TabIndex = 33
        Label12.Text = "Téléphone"
        ' 
        ' Label18
        ' 
        Label18.AutoSize = True
        Label18.Location = New Point(47, 570)
        Label18.Name = "Label18"
        Label18.Size = New Size(83, 20)
        Label18.TabIndex = 39
        Label18.Text = "Remarques"
        ' 
        ' Label13
        ' 
        Label13.AutoSize = True
        Label13.Font = New Font("Segoe UI", 13.8F, FontStyle.Regular, GraphicsUnit.Point, CByte(0))
        Label13.Location = New Point(58, -1)
        Label13.Name = "Label13"
        Label13.Size = New Size(73, 31)
        Label13.TabIndex = 251
        Label13.Text = "Client"
        ' 
        ' BTN_CHANTIER
        ' 
        BTN_CHANTIER.Location = New Point(370, 38)
        BTN_CHANTIER.Name = "BTN_CHANTIER"
        BTN_CHANTIER.Size = New Size(160, 61)
        BTN_CHANTIER.TabIndex = 252
        BTN_CHANTIER.Text = "CHANTIERS"
        BTN_CHANTIER.UseVisualStyleBackColor = True
        ' 
        ' code
        ' 
        code.AutoSize = True
        code.Location = New Point(50, 138)
        code.Name = "code"
        code.Size = New Size(24, 20)
        code.TabIndex = 254
        code.Text = "ID"
        ' 
        ' TextBoxId
        ' 
        TextBoxId.Location = New Point(166, 134)
        TextBoxId.Name = "TextBoxId"
        TextBoxId.ReadOnly = True
        TextBoxId.Size = New Size(346, 27)
        TextBoxId.TabIndex = 253
        ' 
        ' Clients
        ' 
        AutoScaleDimensions = New SizeF(8F, 20F)
        AutoScaleMode = AutoScaleMode.Font
        ClientSize = New Size(1082, 723)
        Controls.Add(code)
        Controls.Add(TextBoxId)
        Controls.Add(BTN_CHANTIER)
        Controls.Add(Label13)
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
        Controls.Add(TextBoxRemarques)
        Controls.Add(TextBoxContacts)
        Controls.Add(TextBoxRadial)
        Controls.Add(TextBoxZone)
        Controls.Add(TextBoxEmail)
        Controls.Add(TextBoxFax)
        Controls.Add(TextBoxTelephone)
        Controls.Add(TextBoxTVA)
        Controls.Add(TextBoxPays)
        Controls.Add(TextBoxCodePostal)
        Controls.Add(TextBoxAdresse)
        Controls.Add(TextBoxNom)
        Controls.Add(DataGridView1)
        Controls.Add(TextBoxCode)
        Controls.Add(EDITE)
        Controls.Add(RECHERCHE)
        Controls.Add(SUPRIME)
        Controls.Add(AJOUTE)
        Name = "Clients"
        Text = "Clients"
        CType(DataGridView1, ComponentModel.ISupportInitialize).EndInit()
        ResumeLayout(False)
        PerformLayout()
    End Sub
    Friend WithEvents SUPRIME As Button
    Friend WithEvents RECHERCHE As Button
    Friend WithEvents AJOUTE As Button
    Friend WithEvents EDITE As Button
    Friend WithEvents TextBoxCode As TextBox
    Friend WithEvents DataGridView1 As DataGridView
    Friend WithEvents TextBoxNom As TextBox
    Friend WithEvents TextBoxCodePostal As TextBox
    Friend WithEvents TextBoxAdresse As TextBox
    Friend WithEvents TextBoxFax As TextBox
    Friend WithEvents TextBoxTelephone As TextBox
    Friend WithEvents TextBoxTVA As TextBox
    Friend WithEvents TextBoxPays As TextBox
    Friend WithEvents TextBoxValeurActuelle As TextBox
    Friend WithEvents TextBoxRemarques As TextBox
    Friend WithEvents TextBoxContacts As TextBox
    Friend WithEvents TextBoxRadial As TextBox
    Friend WithEvents TextBoxZone As TextBox
    Friend WithEvents TextBoxEmail As TextBox
    Friend WithEvents CheckBoxAutorisationDosage As CheckBox
    Friend WithEvents CheckBoxControleCredit As CheckBox
    Friend WithEvents CheckBoxVerrouille As CheckBox
    Friend WithEvents Imprimer As Button
    Friend WithEvents Label1 As Label
    Friend WithEvents Label2 As Label
    Friend WithEvents Label3 As Label
    Friend WithEvents Label4 As Label
    Friend WithEvents Label5 As Label
    Friend WithEvents Label6 As Label
    Friend WithEvents Label7 As Label
    Friend WithEvents Label8 As Label
    Friend WithEvents Label9 As Label
    Friend WithEvents Label10 As Label
    Friend WithEvents Label11 As Label
    Friend WithEvents Label12 As Label
    Friend WithEvents Label18 As Label
    Friend WithEvents Label13 As Label
    Friend WithEvents BTN_CHANTIER As Button
    Friend WithEvents code As Label
    Friend WithEvents TextBoxId As TextBox
End Class
