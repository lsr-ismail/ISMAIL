<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class Planification_Demands
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
        GroupBox1 = New GroupBox()
        txtNomClient = New TextBox()
        txtCodeClient = New TextBox()
        txtIdClient = New TextBox()
        Label11 = New Label()
        Label2 = New Label()
        Label1 = New Label()
        GroupBox2 = New GroupBox()
        txtNomChantier = New TextBox()
        Label5 = New Label()
        txtCodeChantier = New TextBox()
        txtIdChantier = New TextBox()
        Label6 = New Label()
        Label8 = New Label()
        GroupBox3 = New GroupBox()
        txtNomFormule = New TextBox()
        txtCodeFormule = New TextBox()
        DataGridViewCompositionPlanning = New DataGridView()
        Label21 = New Label()
        Label20 = New Label()
        Label19 = New Label()
        Label18 = New Label()
        Label17 = New Label()
        txtMasseAlcalinCalc = New TextBox()
        txtTempsMalaxage = New TextBox()
        txtMES = New TextBox()
        txtVolumeAir = New TextBox()
        txtTauxChlorureCalc = New TextBox()
        txtGamme = New TextBox()
        Label9 = New Label()
        Label12 = New Label()
        Label13 = New Label()
        GroupBox5 = New GroupBox()
        txtNomChauffeur = New TextBox()
        txtCodeChauffeur = New TextBox()
        txtNomCamion = New TextBox()
        txtCodeCamion = New TextBox()
        Label38 = New Label()
        Label37 = New Label()
        Label33 = New Label()
        Label7 = New Label()
        Label27 = New Label()
        txtCapaciteCamion = New TextBox()
        Label28 = New Label()
        Label25 = New Label()
        Label24 = New Label()
        txtCadence = New TextBox()
        txtDejaLivres = New TextBox()
        Label16 = New Label()
        Label15 = New Label()
        Label14 = New Label()
        txtTotalChantier = New TextBox()
        chkComplement = New CheckBox()
        chkAConfirmer = New CheckBox()
        dtpDateLivraison = New DateTimePicker()
        txtVolumeRotationMaxi = New TextBox()
        GroupBox4 = New GroupBox()
        btnMoinsMinuteLivraison = New Button()
        Button2 = New Button()
        btnJourPlus = New Button()
        btnPlusMinuteLivraison = New Button()
        btnMoinsMinuteFabrication = New Button()
        btnPlusMinuteFabrication = New Button()
        btnMoinsHeureLivraison = New Button()
        btnMoinsHeureFabrication = New Button()
        btnPlusHeureFabrication = New Button()
        btnPlusHeureLivraison = New Button()
        txtHeureFabrication = New TextBox()
        Label23 = New Label()
        txtHeureLivraison = New TextBox()
        Label22 = New Label()
        GroupBox7 = New GroupBox()
        txtNomPS1 = New TextBox()
        txtNomPS3 = New TextBox()
        txtNomPS2 = New TextBox()
        txtNomPS5 = New TextBox()
        txtNomPS4 = New TextBox()
        txtNomPS7 = New TextBox()
        txtNomPS6 = New TextBox()
        txtCodePS1 = New TextBox()
        txtCodePS3 = New TextBox()
        txtCodePS2 = New TextBox()
        txtCodePS5 = New TextBox()
        txtCodePS4 = New TextBox()
        txtCodePS7 = New TextBox()
        txtCodePS6 = New TextBox()
        Abandonner = New Button()
        Valider = New Button()
        Label32 = New Label()
        Label31 = New Label()
        Label36 = New Label()
        cmbUnitePS7 = New ComboBox()
        txtQtePS7 = New TextBox()
        cmbUnitePS6 = New ComboBox()
        txtQtePS6 = New TextBox()
        cmbUnitePS5 = New ComboBox()
        txtQtePS5 = New TextBox()
        cmbUnitePS4 = New ComboBox()
        txtQtePS4 = New TextBox()
        cmbUnitePS3 = New ComboBox()
        txtQtePS3 = New TextBox()
        cmbUnitePS2 = New ComboBox()
        txtQtePS2 = New TextBox()
        cmbUnitePS1 = New ComboBox()
        txtQtePS1 = New TextBox()
        Label30 = New Label()
        Label35 = New Label()
        Label4 = New Label()
        Label3 = New Label()
        Label29 = New Label()
        txtQuantiteService = New TextBox()
        Label34 = New Label()
        txtCodeEquipementService = New TextBox()
        txtNomEquipementService = New TextBox()
        GroupBox6 = New GroupBox()
        GroupBox1.SuspendLayout()
        GroupBox2.SuspendLayout()
        GroupBox3.SuspendLayout()
        CType(DataGridViewCompositionPlanning, ComponentModel.ISupportInitialize).BeginInit()
        GroupBox5.SuspendLayout()
        GroupBox4.SuspendLayout()
        GroupBox7.SuspendLayout()
        GroupBox6.SuspendLayout()
        SuspendLayout()
        ' 
        ' GroupBox1
        ' 
        GroupBox1.Controls.Add(txtNomClient)
        GroupBox1.Controls.Add(txtCodeClient)
        GroupBox1.Controls.Add(txtIdClient)
        GroupBox1.Controls.Add(Label11)
        GroupBox1.Controls.Add(Label2)
        GroupBox1.Controls.Add(Label1)
        GroupBox1.Location = New Point(4, 12)
        GroupBox1.Name = "GroupBox1"
        GroupBox1.Size = New Size(1041, 57)
        GroupBox1.TabIndex = 233
        GroupBox1.TabStop = False
        GroupBox1.Text = "Client"
        ' 
        ' txtNomClient
        ' 
        txtNomClient.Location = New Point(714, 18)
        txtNomClient.Name = "txtNomClient"
        txtNomClient.Size = New Size(322, 27)
        txtNomClient.TabIndex = 269
        ' 
        ' txtCodeClient
        ' 
        txtCodeClient.Location = New Point(334, 20)
        txtCodeClient.Name = "txtCodeClient"
        txtCodeClient.Size = New Size(267, 27)
        txtCodeClient.TabIndex = 268
        ' 
        ' txtIdClient
        ' 
        txtIdClient.Location = New Point(109, 22)
        txtIdClient.Name = "txtIdClient"
        txtIdClient.Size = New Size(114, 27)
        txtIdClient.TabIndex = 267
        ' 
        ' Label11
        ' 
        Label11.AutoSize = True
        Label11.Location = New Point(607, 23)
        Label11.Name = "Label11"
        Label11.Size = New Size(88, 20)
        Label11.TabIndex = 252
        Label11.Text = " Nom Client"
        ' 
        ' Label2
        ' 
        Label2.AutoSize = True
        Label2.Location = New Point(242, 25)
        Label2.Name = "Label2"
        Label2.Size = New Size(86, 20)
        Label2.TabIndex = 251
        Label2.Text = "Code Client"
        ' 
        ' Label1
        ' 
        Label1.AutoSize = True
        Label1.Location = New Point(20, 25)
        Label1.Name = "Label1"
        Label1.Size = New Size(64, 20)
        Label1.TabIndex = 250
        Label1.Text = "id Client"
        ' 
        ' GroupBox2
        ' 
        GroupBox2.Controls.Add(txtNomChantier)
        GroupBox2.Controls.Add(Label5)
        GroupBox2.Controls.Add(txtCodeChantier)
        GroupBox2.Controls.Add(txtIdChantier)
        GroupBox2.Controls.Add(Label6)
        GroupBox2.Controls.Add(Label8)
        GroupBox2.Location = New Point(4, 68)
        GroupBox2.Name = "GroupBox2"
        GroupBox2.Size = New Size(1041, 57)
        GroupBox2.TabIndex = 234
        GroupBox2.TabStop = False
        GroupBox2.Text = "Chantier"
        ' 
        ' txtNomChantier
        ' 
        txtNomChantier.Location = New Point(714, 16)
        txtNomChantier.Name = "txtNomChantier"
        txtNomChantier.Size = New Size(322, 27)
        txtNomChantier.TabIndex = 272
        ' 
        ' Label5
        ' 
        Label5.AutoSize = True
        Label5.Location = New Point(607, 19)
        Label5.Name = "Label5"
        Label5.Size = New Size(101, 20)
        Label5.TabIndex = 255
        Label5.Text = "Nom Chantier"
        ' 
        ' txtCodeChantier
        ' 
        txtCodeChantier.Location = New Point(334, 16)
        txtCodeChantier.Name = "txtCodeChantier"
        txtCodeChantier.Size = New Size(267, 27)
        txtCodeChantier.TabIndex = 271
        ' 
        ' txtIdChantier
        ' 
        txtIdChantier.Location = New Point(96, 16)
        txtIdChantier.Name = "txtIdChantier"
        txtIdChantier.Size = New Size(114, 27)
        txtIdChantier.TabIndex = 270
        ' 
        ' Label6
        ' 
        Label6.AutoSize = True
        Label6.Location = New Point(225, 19)
        Label6.Name = "Label6"
        Label6.Size = New Size(103, 20)
        Label6.TabIndex = 254
        Label6.Text = "Code Chantier"
        ' 
        ' Label8
        ' 
        Label8.AutoSize = True
        Label8.Location = New Point(14, 23)
        Label8.Name = "Label8"
        Label8.Size = New Size(81, 20)
        Label8.TabIndex = 253
        Label8.Text = "Id Chantier"
        ' 
        ' GroupBox3
        ' 
        GroupBox3.Controls.Add(txtNomFormule)
        GroupBox3.Controls.Add(txtCodeFormule)
        GroupBox3.Controls.Add(DataGridViewCompositionPlanning)
        GroupBox3.Controls.Add(Label21)
        GroupBox3.Controls.Add(Label20)
        GroupBox3.Controls.Add(Label19)
        GroupBox3.Controls.Add(Label18)
        GroupBox3.Controls.Add(Label17)
        GroupBox3.Controls.Add(txtMasseAlcalinCalc)
        GroupBox3.Controls.Add(txtTempsMalaxage)
        GroupBox3.Controls.Add(txtMES)
        GroupBox3.Controls.Add(txtVolumeAir)
        GroupBox3.Controls.Add(txtTauxChlorureCalc)
        GroupBox3.Controls.Add(txtGamme)
        GroupBox3.Controls.Add(Label9)
        GroupBox3.Controls.Add(Label12)
        GroupBox3.Controls.Add(Label13)
        GroupBox3.Location = New Point(4, 125)
        GroupBox3.Name = "GroupBox3"
        GroupBox3.Size = New Size(1041, 164)
        GroupBox3.TabIndex = 256
        GroupBox3.TabStop = False
        GroupBox3.Text = "Formule"
        ' 
        ' txtNomFormule
        ' 
        txtNomFormule.Location = New Point(349, 18)
        txtNomFormule.Name = "txtNomFormule"
        txtNomFormule.Size = New Size(267, 27)
        txtNomFormule.TabIndex = 273
        ' 
        ' txtCodeFormule
        ' 
        txtCodeFormule.Location = New Point(120, 18)
        txtCodeFormule.Name = "txtCodeFormule"
        txtCodeFormule.Size = New Size(114, 27)
        txtCodeFormule.TabIndex = 268
        ' 
        ' DataGridViewCompositionPlanning
        ' 
        DataGridViewCompositionPlanning.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize
        DataGridViewCompositionPlanning.Location = New Point(637, 19)
        DataGridViewCompositionPlanning.Name = "DataGridViewCompositionPlanning"
        DataGridViewCompositionPlanning.RowHeadersWidth = 51
        DataGridViewCompositionPlanning.Size = New Size(382, 130)
        DataGridViewCompositionPlanning.TabIndex = 257
        ' 
        ' Label21
        ' 
        Label21.AutoSize = True
        Label21.Location = New Point(14, 54)
        Label21.Name = "Label21"
        Label21.Size = New Size(42, 20)
        Label21.TabIndex = 266
        Label21.Text = " MES"
        ' 
        ' Label20
        ' 
        Label20.AutoSize = True
        Label20.Location = New Point(14, 87)
        Label20.Name = "Label20"
        Label20.Size = New Size(86, 20)
        Label20.TabIndex = 265
        Label20.Text = " Volume Air"
        ' 
        ' Label19
        ' 
        Label19.AutoSize = True
        Label19.Location = New Point(243, 54)
        Label19.Name = "Label19"
        Label19.Size = New Size(134, 20)
        Label19.TabIndex = 264
        Label19.Text = " Taux Chlorure Calc"
        ' 
        ' Label18
        ' 
        Label18.AutoSize = True
        Label18.Location = New Point(252, 120)
        Label18.Name = "Label18"
        Label18.Size = New Size(135, 20)
        Label18.TabIndex = 263
        Label18.Text = " Masse Alcalin Calc"
        ' 
        ' Label17
        ' 
        Label17.AutoSize = True
        Label17.Location = New Point(243, 87)
        Label17.Name = "Label17"
        Label17.Size = New Size(125, 20)
        Label17.TabIndex = 262
        Label17.Text = " Temps Malaxage"
        ' 
        ' txtMasseAlcalinCalc
        ' 
        txtMasseAlcalinCalc.Location = New Point(455, 117)
        txtMasseAlcalinCalc.Name = "txtMasseAlcalinCalc"
        txtMasseAlcalinCalc.Size = New Size(114, 27)
        txtMasseAlcalinCalc.TabIndex = 261
        ' 
        ' txtTempsMalaxage
        ' 
        txtTempsMalaxage.Location = New Point(455, 84)
        txtTempsMalaxage.Name = "txtTempsMalaxage"
        txtTempsMalaxage.Size = New Size(114, 27)
        txtTempsMalaxage.TabIndex = 260
        ' 
        ' txtMES
        ' 
        txtMES.Location = New Point(120, 51)
        txtMES.Name = "txtMES"
        txtMES.Size = New Size(114, 27)
        txtMES.TabIndex = 259
        ' 
        ' txtVolumeAir
        ' 
        txtVolumeAir.Location = New Point(120, 84)
        txtVolumeAir.Name = "txtVolumeAir"
        txtVolumeAir.Size = New Size(114, 27)
        txtVolumeAir.TabIndex = 258
        ' 
        ' txtTauxChlorureCalc
        ' 
        txtTauxChlorureCalc.Location = New Point(120, 117)
        txtTauxChlorureCalc.Name = "txtTauxChlorureCalc"
        txtTauxChlorureCalc.Size = New Size(114, 27)
        txtTauxChlorureCalc.TabIndex = 257
        ' 
        ' txtGamme
        ' 
        txtGamme.Location = New Point(455, 51)
        txtGamme.Name = "txtGamme"
        txtGamme.Size = New Size(114, 27)
        txtGamme.TabIndex = 256
        ' 
        ' Label9
        ' 
        Label9.AutoSize = True
        Label9.Location = New Point(20, 120)
        Label9.Name = "Label9"
        Label9.Size = New Size(61, 20)
        Label9.TabIndex = 255
        Label9.Text = "Gamme"
        ' 
        ' Label12
        ' 
        Label12.AutoSize = True
        Label12.Location = New Point(243, 22)
        Label12.Name = "Label12"
        Label12.Size = New Size(100, 20)
        Label12.TabIndex = 254
        Label12.Text = "Nom Formule"
        ' 
        ' Label13
        ' 
        Label13.AutoSize = True
        Label13.Location = New Point(12, 22)
        Label13.Name = "Label13"
        Label13.Size = New Size(102, 20)
        Label13.TabIndex = 253
        Label13.Text = "Code Formule"
        ' 
        ' GroupBox5
        ' 
        GroupBox5.Controls.Add(txtNomChauffeur)
        GroupBox5.Controls.Add(txtCodeChauffeur)
        GroupBox5.Controls.Add(txtNomCamion)
        GroupBox5.Controls.Add(txtCodeCamion)
        GroupBox5.Controls.Add(Label38)
        GroupBox5.Controls.Add(Label37)
        GroupBox5.Controls.Add(Label33)
        GroupBox5.Controls.Add(Label7)
        GroupBox5.Controls.Add(Label27)
        GroupBox5.Controls.Add(txtCapaciteCamion)
        GroupBox5.Controls.Add(Label28)
        GroupBox5.Location = New Point(4, 394)
        GroupBox5.Name = "GroupBox5"
        GroupBox5.Size = New Size(1041, 64)
        GroupBox5.TabIndex = 253
        GroupBox5.TabStop = False
        GroupBox5.Text = "Famille camion"
        ' 
        ' txtNomChauffeur
        ' 
        txtNomChauffeur.Location = New Point(819, 26)
        txtNomChauffeur.Name = "txtNomChauffeur"
        txtNomChauffeur.Size = New Size(217, 27)
        txtNomChauffeur.TabIndex = 295
        ' 
        ' txtCodeChauffeur
        ' 
        txtCodeChauffeur.Location = New Point(692, 26)
        txtCodeChauffeur.Name = "txtCodeChauffeur"
        txtCodeChauffeur.Size = New Size(114, 27)
        txtCodeChauffeur.TabIndex = 294
        ' 
        ' txtNomCamion
        ' 
        txtNomCamion.Location = New Point(314, 25)
        txtNomCamion.Name = "txtNomCamion"
        txtNomCamion.Size = New Size(217, 27)
        txtNomCamion.TabIndex = 293
        ' 
        ' txtCodeCamion
        ' 
        txtCodeCamion.Location = New Point(99, 23)
        txtCodeCamion.Name = "txtCodeCamion"
        txtCodeCamion.Size = New Size(114, 27)
        txtCodeCamion.TabIndex = 274
        ' 
        ' Label38
        ' 
        Label38.AutoSize = True
        Label38.Location = New Point(857, -2)
        Label38.Name = "Label38"
        Label38.Size = New Size(114, 20)
        Label38.TabIndex = 291
        Label38.Text = "Nom Chauffeur "
        ' 
        ' Label37
        ' 
        Label37.AutoSize = True
        Label37.Location = New Point(695, 0)
        Label37.Name = "Label37"
        Label37.Size = New Size(116, 20)
        Label37.TabIndex = 289
        Label37.Text = "Code Chauffeur "
        ' 
        ' Label33
        ' 
        Label33.AutoSize = True
        Label33.Location = New Point(547, 0)
        Label33.Name = "Label33"
        Label33.Size = New Size(69, 20)
        Label33.TabIndex = 285
        Label33.Text = "capacité "
        ' 
        ' Label7
        ' 
        Label7.AutoSize = True
        Label7.Location = New Point(640, 29)
        Label7.Name = "Label7"
        Label7.Size = New Size(34, 20)
        Label7.TabIndex = 285
        Label7.Text = " M3"
        ' 
        ' Label27
        ' 
        Label27.AutoSize = True
        Label27.Location = New Point(209, 28)
        Label27.Name = "Label27"
        Label27.Size = New Size(99, 20)
        Label27.TabIndex = 252
        Label27.Text = " Nom camion"
        ' 
        ' txtCapaciteCamion
        ' 
        txtCapaciteCamion.Location = New Point(544, 23)
        txtCapaciteCamion.Name = "txtCapaciteCamion"
        txtCapaciteCamion.Size = New Size(90, 27)
        txtCapaciteCamion.TabIndex = 286
        ' 
        ' Label28
        ' 
        Label28.AutoSize = True
        Label28.Location = New Point(6, 28)
        Label28.Name = "Label28"
        Label28.Size = New Size(97, 20)
        Label28.TabIndex = 251
        Label28.Text = "Code camion"
        ' 
        ' Label25
        ' 
        Label25.AutoSize = True
        Label25.Location = New Point(169, 3)
        Label25.Name = "Label25"
        Label25.Size = New Size(77, 20)
        Label25.TabIndex = 254
        Label25.Text = "DejaLivres"
        ' 
        ' Label24
        ' 
        Label24.AutoSize = True
        Label24.Location = New Point(20, 65)
        Label24.Name = "Label24"
        Label24.Size = New Size(99, 20)
        Label24.TabIndex = 255
        Label24.Text = "DateLivraison"
        ' 
        ' txtCadence
        ' 
        txtCadence.Location = New Point(660, 16)
        txtCadence.Name = "txtCadence"
        txtCadence.Size = New Size(86, 27)
        txtCadence.TabIndex = 258
        ' 
        ' txtDejaLivres
        ' 
        txtDejaLivres.Location = New Point(160, 20)
        txtDejaLivres.Name = "txtDejaLivres"
        txtDejaLivres.Size = New Size(90, 27)
        txtDejaLivres.TabIndex = 259
        ' 
        ' Label16
        ' 
        Label16.AutoSize = True
        Label16.Location = New Point(281, 22)
        Label16.Name = "Label16"
        Label16.Size = New Size(152, 20)
        Label16.TabIndex = 264
        Label16.Text = "Volume rotation maxi"
        ' 
        ' Label15
        ' 
        Label15.AutoSize = True
        Label15.Location = New Point(585, 20)
        Label15.Name = "Label15"
        Label15.Size = New Size(64, 20)
        Label15.TabIndex = 265
        Label15.Text = "cadence"
        ' 
        ' Label14
        ' 
        Label14.AutoSize = True
        Label14.Location = New Point(120, 23)
        Label14.Name = "Label14"
        Label14.Size = New Size(34, 20)
        Label14.TabIndex = 266
        Label14.Text = " M3"
        ' 
        ' txtTotalChantier
        ' 
        txtTotalChantier.Location = New Point(24, 20)
        txtTotalChantier.Name = "txtTotalChantier"
        txtTotalChantier.Size = New Size(90, 27)
        txtTotalChantier.TabIndex = 267
        ' 
        ' chkComplement
        ' 
        chkComplement.AutoSize = True
        chkComplement.Location = New Point(778, 21)
        chkComplement.Name = "chkComplement"
        chkComplement.Size = New Size(117, 24)
        chkComplement.TabIndex = 268
        chkComplement.Text = "Complement"
        chkComplement.UseVisualStyleBackColor = True
        ' 
        ' chkAConfirmer
        ' 
        chkAConfirmer.AutoSize = True
        chkAConfirmer.Location = New Point(912, 19)
        chkAConfirmer.Name = "chkAConfirmer"
        chkAConfirmer.Size = New Size(107, 24)
        chkAConfirmer.TabIndex = 269
        chkAConfirmer.Text = "AConfirmer"
        chkAConfirmer.UseVisualStyleBackColor = True
        ' 
        ' dtpDateLivraison
        ' 
        dtpDateLivraison.Location = New Point(150, 62)
        dtpDateLivraison.Name = "dtpDateLivraison"
        dtpDateLivraison.Size = New Size(214, 27)
        dtpDateLivraison.TabIndex = 268
        ' 
        ' txtVolumeRotationMaxi
        ' 
        txtVolumeRotationMaxi.Location = New Point(455, 16)
        txtVolumeRotationMaxi.Name = "txtVolumeRotationMaxi"
        txtVolumeRotationMaxi.Size = New Size(90, 27)
        txtVolumeRotationMaxi.TabIndex = 270
        ' 
        ' GroupBox4
        ' 
        GroupBox4.Controls.Add(btnMoinsMinuteLivraison)
        GroupBox4.Controls.Add(Button2)
        GroupBox4.Controls.Add(btnJourPlus)
        GroupBox4.Controls.Add(btnPlusMinuteLivraison)
        GroupBox4.Controls.Add(btnMoinsMinuteFabrication)
        GroupBox4.Controls.Add(txtVolumeRotationMaxi)
        GroupBox4.Controls.Add(btnPlusMinuteFabrication)
        GroupBox4.Controls.Add(btnMoinsHeureLivraison)
        GroupBox4.Controls.Add(chkAConfirmer)
        GroupBox4.Controls.Add(btnMoinsHeureFabrication)
        GroupBox4.Controls.Add(dtpDateLivraison)
        GroupBox4.Controls.Add(chkComplement)
        GroupBox4.Controls.Add(Label24)
        GroupBox4.Controls.Add(btnPlusHeureFabrication)
        GroupBox4.Controls.Add(btnPlusHeureLivraison)
        GroupBox4.Controls.Add(txtTotalChantier)
        GroupBox4.Controls.Add(txtHeureFabrication)
        GroupBox4.Controls.Add(Label14)
        GroupBox4.Controls.Add(Label15)
        GroupBox4.Controls.Add(Label23)
        GroupBox4.Controls.Add(Label16)
        GroupBox4.Controls.Add(txtDejaLivres)
        GroupBox4.Controls.Add(txtCadence)
        GroupBox4.Controls.Add(Label25)
        GroupBox4.Controls.Add(txtHeureLivraison)
        GroupBox4.Controls.Add(Label22)
        GroupBox4.Location = New Point(4, 289)
        GroupBox4.Name = "GroupBox4"
        GroupBox4.Size = New Size(1041, 105)
        GroupBox4.TabIndex = 267
        GroupBox4.TabStop = False
        GroupBox4.Text = "Totale Chantiers"
        ' 
        ' btnMoinsMinuteLivraison
        ' 
        btnMoinsMinuteLivraison.Font = New Font("Segoe UI", 7.20000029F, FontStyle.Regular, GraphicsUnit.Point, CByte(0))
        btnMoinsMinuteLivraison.Location = New Point(660, 80)
        btnMoinsMinuteLivraison.Name = "btnMoinsMinuteLivraison"
        btnMoinsMinuteLivraison.Size = New Size(26, 20)
        btnMoinsMinuteLivraison.TabIndex = 284
        btnMoinsMinuteLivraison.Text = "-"
        btnMoinsMinuteLivraison.TextAlign = ContentAlignment.TopCenter
        btnMoinsMinuteLivraison.UseVisualStyleBackColor = True
        ' 
        ' Button2
        ' 
        Button2.Location = New Point(370, 61)
        Button2.Name = "Button2"
        Button2.Size = New Size(25, 34)
        Button2.TabIndex = 274
        Button2.Text = "-"
        Button2.UseVisualStyleBackColor = True
        ' 
        ' btnJourPlus
        ' 
        btnJourPlus.Location = New Point(120, 57)
        btnJourPlus.Name = "btnJourPlus"
        btnJourPlus.Size = New Size(25, 34)
        btnJourPlus.TabIndex = 273
        btnJourPlus.Text = "+"
        btnJourPlus.UseVisualStyleBackColor = True
        ' 
        ' btnPlusMinuteLivraison
        ' 
        btnPlusMinuteLivraison.Font = New Font("Segoe UI", 7.20000029F, FontStyle.Regular, GraphicsUnit.Point, CByte(0))
        btnPlusMinuteLivraison.Location = New Point(660, 60)
        btnPlusMinuteLivraison.Name = "btnPlusMinuteLivraison"
        btnPlusMinuteLivraison.Size = New Size(26, 20)
        btnPlusMinuteLivraison.TabIndex = 283
        btnPlusMinuteLivraison.Text = "+"
        btnPlusMinuteLivraison.TextAlign = ContentAlignment.TopRight
        btnPlusMinuteLivraison.UseVisualStyleBackColor = True
        ' 
        ' btnMoinsMinuteFabrication
        ' 
        btnMoinsMinuteFabrication.Font = New Font("Segoe UI", 7.20000029F, FontStyle.Regular, GraphicsUnit.Point, CByte(0))
        btnMoinsMinuteFabrication.Location = New Point(980, 81)
        btnMoinsMinuteFabrication.Name = "btnMoinsMinuteFabrication"
        btnMoinsMinuteFabrication.Size = New Size(26, 20)
        btnMoinsMinuteFabrication.TabIndex = 280
        btnMoinsMinuteFabrication.Text = "-"
        btnMoinsMinuteFabrication.TextAlign = ContentAlignment.TopCenter
        btnMoinsMinuteFabrication.UseVisualStyleBackColor = True
        ' 
        ' btnPlusMinuteFabrication
        ' 
        btnPlusMinuteFabrication.Font = New Font("Segoe UI", 7.20000029F, FontStyle.Regular, GraphicsUnit.Point, CByte(0))
        btnPlusMinuteFabrication.Location = New Point(980, 58)
        btnPlusMinuteFabrication.Name = "btnPlusMinuteFabrication"
        btnPlusMinuteFabrication.Size = New Size(26, 20)
        btnPlusMinuteFabrication.TabIndex = 279
        btnPlusMinuteFabrication.Text = "+"
        btnPlusMinuteFabrication.TextAlign = ContentAlignment.TopRight
        btnPlusMinuteFabrication.UseVisualStyleBackColor = True
        ' 
        ' btnMoinsHeureLivraison
        ' 
        btnMoinsHeureLivraison.Font = New Font("Segoe UI", 7.20000029F, FontStyle.Regular, GraphicsUnit.Point, CByte(0))
        btnMoinsHeureLivraison.Location = New Point(532, 77)
        btnMoinsHeureLivraison.Name = "btnMoinsHeureLivraison"
        btnMoinsHeureLivraison.Size = New Size(26, 20)
        btnMoinsHeureLivraison.TabIndex = 282
        btnMoinsHeureLivraison.Text = "-"
        btnMoinsHeureLivraison.TextAlign = ContentAlignment.TopCenter
        btnMoinsHeureLivraison.UseVisualStyleBackColor = True
        ' 
        ' btnMoinsHeureFabrication
        ' 
        btnMoinsHeureFabrication.Font = New Font("Segoe UI", 7.20000029F, FontStyle.Regular, GraphicsUnit.Point, CByte(0))
        btnMoinsHeureFabrication.Location = New Point(854, 77)
        btnMoinsHeureFabrication.Name = "btnMoinsHeureFabrication"
        btnMoinsHeureFabrication.Size = New Size(26, 20)
        btnMoinsHeureFabrication.TabIndex = 278
        btnMoinsHeureFabrication.Text = "-"
        btnMoinsHeureFabrication.TextAlign = ContentAlignment.TopCenter
        btnMoinsHeureFabrication.UseVisualStyleBackColor = True
        ' 
        ' btnPlusHeureFabrication
        ' 
        btnPlusHeureFabrication.Font = New Font("Segoe UI", 7.20000029F, FontStyle.Regular, GraphicsUnit.Point, CByte(0))
        btnPlusHeureFabrication.Location = New Point(854, 57)
        btnPlusHeureFabrication.Name = "btnPlusHeureFabrication"
        btnPlusHeureFabrication.Size = New Size(26, 20)
        btnPlusHeureFabrication.TabIndex = 277
        btnPlusHeureFabrication.Text = "+"
        btnPlusHeureFabrication.TextAlign = ContentAlignment.TopRight
        btnPlusHeureFabrication.UseVisualStyleBackColor = True
        ' 
        ' btnPlusHeureLivraison
        ' 
        btnPlusHeureLivraison.Font = New Font("Segoe UI", 7.20000029F, FontStyle.Regular, GraphicsUnit.Point, CByte(0))
        btnPlusHeureLivraison.Location = New Point(532, 57)
        btnPlusHeureLivraison.Name = "btnPlusHeureLivraison"
        btnPlusHeureLivraison.Size = New Size(26, 20)
        btnPlusHeureLivraison.TabIndex = 281
        btnPlusHeureLivraison.Text = "+"
        btnPlusHeureLivraison.TextAlign = ContentAlignment.TopRight
        btnPlusHeureLivraison.UseVisualStyleBackColor = True
        ' 
        ' txtHeureFabrication
        ' 
        txtHeureFabrication.Location = New Point(887, 64)
        txtHeureFabrication.Name = "txtHeureFabrication"
        txtHeureFabrication.Size = New Size(90, 27)
        txtHeureFabrication.TabIndex = 275
        ' 
        ' Label23
        ' 
        Label23.AutoSize = True
        Label23.Location = New Point(709, 65)
        Label23.Name = "Label23"
        Label23.Size = New Size(126, 20)
        Label23.TabIndex = 262
        Label23.Text = "Heure Fabrication"
        ' 
        ' txtHeureLivraison
        ' 
        txtHeureLivraison.Location = New Point(564, 65)
        txtHeureLivraison.Name = "txtHeureLivraison"
        txtHeureLivraison.Size = New Size(90, 27)
        txtHeureLivraison.TabIndex = 276
        ' 
        ' Label22
        ' 
        Label22.AutoSize = True
        Label22.Location = New Point(406, 65)
        Label22.Name = "Label22"
        Label22.Size = New Size(111, 20)
        Label22.TabIndex = 263
        Label22.Text = "Heure Livraison"
        ' 
        ' GroupBox7
        ' 
        GroupBox7.Controls.Add(txtNomPS1)
        GroupBox7.Controls.Add(txtNomPS3)
        GroupBox7.Controls.Add(txtNomPS2)
        GroupBox7.Controls.Add(txtNomPS5)
        GroupBox7.Controls.Add(txtNomPS4)
        GroupBox7.Controls.Add(txtNomPS7)
        GroupBox7.Controls.Add(txtNomPS6)
        GroupBox7.Controls.Add(GroupBox6)
        GroupBox7.Controls.Add(txtCodePS1)
        GroupBox7.Controls.Add(txtCodePS3)
        GroupBox7.Controls.Add(txtCodePS2)
        GroupBox7.Controls.Add(txtCodePS5)
        GroupBox7.Controls.Add(txtCodePS4)
        GroupBox7.Controls.Add(txtCodePS7)
        GroupBox7.Controls.Add(txtCodePS6)
        GroupBox7.Controls.Add(Label32)
        GroupBox7.Controls.Add(Label31)
        GroupBox7.Controls.Add(Label36)
        GroupBox7.Controls.Add(cmbUnitePS7)
        GroupBox7.Controls.Add(txtQtePS7)
        GroupBox7.Controls.Add(cmbUnitePS6)
        GroupBox7.Controls.Add(txtQtePS6)
        GroupBox7.Controls.Add(cmbUnitePS5)
        GroupBox7.Controls.Add(txtQtePS5)
        GroupBox7.Controls.Add(cmbUnitePS4)
        GroupBox7.Controls.Add(txtQtePS4)
        GroupBox7.Controls.Add(cmbUnitePS3)
        GroupBox7.Controls.Add(txtQtePS3)
        GroupBox7.Controls.Add(cmbUnitePS2)
        GroupBox7.Controls.Add(txtQtePS2)
        GroupBox7.Controls.Add(cmbUnitePS1)
        GroupBox7.Controls.Add(txtQtePS1)
        GroupBox7.Controls.Add(Label30)
        GroupBox7.Location = New Point(4, 458)
        GroupBox7.Name = "GroupBox7"
        GroupBox7.Size = New Size(649, 254)
        GroupBox7.TabIndex = 273
        GroupBox7.TabStop = False
        GroupBox7.Text = "Produits et services"
        ' 
        ' txtNomPS1
        ' 
        txtNomPS1.Location = New Point(192, 40)
        txtNomPS1.Name = "txtNomPS1"
        txtNomPS1.Size = New Size(171, 27)
        txtNomPS1.TabIndex = 316
        ' 
        ' txtNomPS3
        ' 
        txtNomPS3.Location = New Point(193, 99)
        txtNomPS3.Name = "txtNomPS3"
        txtNomPS3.Size = New Size(171, 27)
        txtNomPS3.TabIndex = 315
        ' 
        ' txtNomPS2
        ' 
        txtNomPS2.Location = New Point(193, 70)
        txtNomPS2.Name = "txtNomPS2"
        txtNomPS2.Size = New Size(171, 27)
        txtNomPS2.TabIndex = 314
        ' 
        ' txtNomPS5
        ' 
        txtNomPS5.Location = New Point(193, 157)
        txtNomPS5.Name = "txtNomPS5"
        txtNomPS5.Size = New Size(171, 27)
        txtNomPS5.TabIndex = 313
        ' 
        ' txtNomPS4
        ' 
        txtNomPS4.Location = New Point(193, 129)
        txtNomPS4.Name = "txtNomPS4"
        txtNomPS4.Size = New Size(171, 27)
        txtNomPS4.TabIndex = 312
        ' 
        ' txtNomPS7
        ' 
        txtNomPS7.Location = New Point(193, 213)
        txtNomPS7.Name = "txtNomPS7"
        txtNomPS7.Size = New Size(171, 27)
        txtNomPS7.TabIndex = 311
        ' 
        ' txtNomPS6
        ' 
        txtNomPS6.Location = New Point(193, 185)
        txtNomPS6.Name = "txtNomPS6"
        txtNomPS6.Size = New Size(171, 27)
        txtNomPS6.TabIndex = 310
        ' 
        ' txtCodePS1
        ' 
        txtCodePS1.Location = New Point(12, 41)
        txtCodePS1.Name = "txtCodePS1"
        txtCodePS1.Size = New Size(171, 27)
        txtCodePS1.TabIndex = 309
        ' 
        ' txtCodePS3
        ' 
        txtCodePS3.Location = New Point(12, 100)
        txtCodePS3.Name = "txtCodePS3"
        txtCodePS3.Size = New Size(171, 27)
        txtCodePS3.TabIndex = 307
        ' 
        ' txtCodePS2
        ' 
        txtCodePS2.Location = New Point(12, 71)
        txtCodePS2.Name = "txtCodePS2"
        txtCodePS2.Size = New Size(171, 27)
        txtCodePS2.TabIndex = 306
        ' 
        ' txtCodePS5
        ' 
        txtCodePS5.Location = New Point(12, 158)
        txtCodePS5.Name = "txtCodePS5"
        txtCodePS5.Size = New Size(171, 27)
        txtCodePS5.TabIndex = 305
        ' 
        ' txtCodePS4
        ' 
        txtCodePS4.Location = New Point(12, 130)
        txtCodePS4.Name = "txtCodePS4"
        txtCodePS4.Size = New Size(171, 27)
        txtCodePS4.TabIndex = 304
        ' 
        ' txtCodePS7
        ' 
        txtCodePS7.Location = New Point(12, 214)
        txtCodePS7.Name = "txtCodePS7"
        txtCodePS7.Size = New Size(171, 27)
        txtCodePS7.TabIndex = 303
        ' 
        ' txtCodePS6
        ' 
        txtCodePS6.Location = New Point(12, 186)
        txtCodePS6.Name = "txtCodePS6"
        txtCodePS6.Size = New Size(171, 27)
        txtCodePS6.TabIndex = 298
        ' 
        ' Abandonner
        ' 
        Abandonner.Font = New Font("Segoe UI", 16.2F, FontStyle.Regular, GraphicsUnit.Point, CByte(0))
        Abandonner.Location = New Point(752, 572)
        Abandonner.Name = "Abandonner"
        Abandonner.Size = New Size(180, 100)
        Abandonner.TabIndex = 302
        Abandonner.Text = "Abandonner"
        Abandonner.UseVisualStyleBackColor = True
        ' 
        ' Valider
        ' 
        Valider.Font = New Font("Segoe UI", 16.2F, FontStyle.Regular, GraphicsUnit.Point, CByte(0))
        Valider.Location = New Point(752, 464)
        Valider.Name = "Valider"
        Valider.Size = New Size(180, 100)
        Valider.TabIndex = 301
        Valider.Text = "Valider"
        Valider.UseVisualStyleBackColor = True
        ' 
        ' Label32
        ' 
        Label32.AutoSize = True
        Label32.Location = New Point(247, 16)
        Label32.Name = "Label32"
        Label32.Size = New Size(50, 20)
        Label32.TabIndex = 289
        Label32.Text = " Nom "
        ' 
        ' Label31
        ' 
        Label31.AutoSize = True
        Label31.Location = New Point(63, 18)
        Label31.Name = "Label31"
        Label31.Size = New Size(44, 20)
        Label31.TabIndex = 289
        Label31.Text = "Code"
        ' 
        ' Label36
        ' 
        Label36.AutoSize = True
        Label36.Location = New Point(533, 16)
        Label36.Name = "Label36"
        Label36.Size = New Size(50, 20)
        Label36.TabIndex = 300
        Label36.Text = "UNITÉ"
        ' 
        ' cmbUnitePS7
        ' 
        cmbUnitePS7.AutoCompleteMode = AutoCompleteMode.SuggestAppend
        cmbUnitePS7.AutoCompleteSource = AutoCompleteSource.ListItems
        cmbUnitePS7.FormattingEnabled = True
        cmbUnitePS7.Location = New Point(466, 215)
        cmbUnitePS7.Name = "cmbUnitePS7"
        cmbUnitePS7.Size = New Size(171, 28)
        cmbUnitePS7.TabIndex = 297
        ' 
        ' txtQtePS7
        ' 
        txtQtePS7.Location = New Point(374, 215)
        txtQtePS7.Name = "txtQtePS7"
        txtQtePS7.Size = New Size(86, 27)
        txtQtePS7.TabIndex = 296
        ' 
        ' cmbUnitePS6
        ' 
        cmbUnitePS6.AutoCompleteMode = AutoCompleteMode.SuggestAppend
        cmbUnitePS6.AutoCompleteSource = AutoCompleteSource.ListItems
        cmbUnitePS6.FormattingEnabled = True
        cmbUnitePS6.Location = New Point(466, 186)
        cmbUnitePS6.Name = "cmbUnitePS6"
        cmbUnitePS6.Size = New Size(171, 28)
        cmbUnitePS6.TabIndex = 293
        ' 
        ' txtQtePS6
        ' 
        txtQtePS6.Location = New Point(374, 186)
        txtQtePS6.Name = "txtQtePS6"
        txtQtePS6.Size = New Size(86, 27)
        txtQtePS6.TabIndex = 292
        ' 
        ' cmbUnitePS5
        ' 
        cmbUnitePS5.AutoCompleteMode = AutoCompleteMode.SuggestAppend
        cmbUnitePS5.AutoCompleteSource = AutoCompleteSource.ListItems
        cmbUnitePS5.FormattingEnabled = True
        cmbUnitePS5.Location = New Point(466, 157)
        cmbUnitePS5.Name = "cmbUnitePS5"
        cmbUnitePS5.Size = New Size(171, 28)
        cmbUnitePS5.TabIndex = 289
        ' 
        ' txtQtePS5
        ' 
        txtQtePS5.Location = New Point(374, 157)
        txtQtePS5.Name = "txtQtePS5"
        txtQtePS5.Size = New Size(86, 27)
        txtQtePS5.TabIndex = 288
        ' 
        ' cmbUnitePS4
        ' 
        cmbUnitePS4.AutoCompleteMode = AutoCompleteMode.SuggestAppend
        cmbUnitePS4.AutoCompleteSource = AutoCompleteSource.ListItems
        cmbUnitePS4.FormattingEnabled = True
        cmbUnitePS4.Location = New Point(466, 128)
        cmbUnitePS4.Name = "cmbUnitePS4"
        cmbUnitePS4.Size = New Size(171, 28)
        cmbUnitePS4.TabIndex = 285
        ' 
        ' txtQtePS4
        ' 
        txtQtePS4.Location = New Point(374, 128)
        txtQtePS4.Name = "txtQtePS4"
        txtQtePS4.Size = New Size(86, 27)
        txtQtePS4.TabIndex = 284
        ' 
        ' cmbUnitePS3
        ' 
        cmbUnitePS3.AutoCompleteMode = AutoCompleteMode.SuggestAppend
        cmbUnitePS3.AutoCompleteSource = AutoCompleteSource.ListItems
        cmbUnitePS3.FormattingEnabled = True
        cmbUnitePS3.Location = New Point(466, 99)
        cmbUnitePS3.Name = "cmbUnitePS3"
        cmbUnitePS3.Size = New Size(171, 28)
        cmbUnitePS3.TabIndex = 281
        ' 
        ' txtQtePS3
        ' 
        txtQtePS3.Location = New Point(374, 99)
        txtQtePS3.Name = "txtQtePS3"
        txtQtePS3.Size = New Size(86, 27)
        txtQtePS3.TabIndex = 280
        ' 
        ' cmbUnitePS2
        ' 
        cmbUnitePS2.AutoCompleteMode = AutoCompleteMode.SuggestAppend
        cmbUnitePS2.AutoCompleteSource = AutoCompleteSource.ListItems
        cmbUnitePS2.FormattingEnabled = True
        cmbUnitePS2.Location = New Point(466, 69)
        cmbUnitePS2.Name = "cmbUnitePS2"
        cmbUnitePS2.Size = New Size(171, 28)
        cmbUnitePS2.TabIndex = 277
        ' 
        ' txtQtePS2
        ' 
        txtQtePS2.Location = New Point(374, 69)
        txtQtePS2.Name = "txtQtePS2"
        txtQtePS2.Size = New Size(86, 27)
        txtQtePS2.TabIndex = 276
        ' 
        ' cmbUnitePS1
        ' 
        cmbUnitePS1.AutoCompleteMode = AutoCompleteMode.SuggestAppend
        cmbUnitePS1.AutoCompleteSource = AutoCompleteSource.ListItems
        cmbUnitePS1.FormattingEnabled = True
        cmbUnitePS1.Location = New Point(466, 40)
        cmbUnitePS1.Name = "cmbUnitePS1"
        cmbUnitePS1.Size = New Size(171, 28)
        cmbUnitePS1.TabIndex = 272
        ' 
        ' txtQtePS1
        ' 
        txtQtePS1.Location = New Point(374, 40)
        txtQtePS1.Name = "txtQtePS1"
        txtQtePS1.Size = New Size(86, 27)
        txtQtePS1.TabIndex = 271
        ' 
        ' Label30
        ' 
        Label30.AutoSize = True
        Label30.Location = New Point(404, 16)
        Label30.Name = "Label30"
        Label30.Size = New Size(35, 20)
        Label30.TabIndex = 252
        Label30.Text = "QTÉ"
        ' 
        ' Label35
        ' 
        Label35.AutoSize = True
        Label35.Font = New Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point, CByte(0))
        Label35.Location = New Point(113, -6)
        Label35.Name = "Label35"
        Label35.Size = New Size(209, 28)
        Label35.TabIndex = 274
        Label35.Text = "Planification_Demands"
        ' 
        ' Label4
        ' 
        Label4.AutoSize = True
        Label4.Location = New Point(17, 20)
        Label4.Name = "Label4"
        Label4.Size = New Size(44, 20)
        Label4.TabIndex = 251
        Label4.Text = "Code"
        ' 
        ' Label3
        ' 
        Label3.AutoSize = True
        Label3.Location = New Point(253, 17)
        Label3.Name = "Label3"
        Label3.Size = New Size(50, 20)
        Label3.TabIndex = 252
        Label3.Text = " Nom "
        ' 
        ' Label29
        ' 
        Label29.AutoSize = True
        Label29.Location = New Point(662, 40)
        Label29.Name = "Label29"
        Label29.Size = New Size(34, 20)
        Label29.TabIndex = 287
        Label29.Text = " M3"
        ' 
        ' txtQuantiteService
        ' 
        txtQuantiteService.Location = New Point(566, 37)
        txtQuantiteService.Name = "txtQuantiteService"
        txtQuantiteService.Size = New Size(90, 27)
        txtQuantiteService.TabIndex = 288
        ' 
        ' Label34
        ' 
        Label34.AutoSize = True
        Label34.Location = New Point(481, 40)
        Label34.Name = "Label34"
        Label34.Size = New Size(64, 20)
        Label34.TabIndex = 287
        Label34.Text = "quantité"
        ' 
        ' txtCodeEquipementService
        ' 
        txtCodeEquipementService.Location = New Point(17, 49)
        txtCodeEquipementService.Name = "txtCodeEquipementService"
        txtCodeEquipementService.Size = New Size(217, 27)
        txtCodeEquipementService.TabIndex = 296
        ' 
        ' txtNomEquipementService
        ' 
        txtNomEquipementService.Location = New Point(253, 49)
        txtNomEquipementService.Name = "txtNomEquipementService"
        txtNomEquipementService.Size = New Size(217, 27)
        txtNomEquipementService.TabIndex = 297
        ' 
        ' GroupBox6
        ' 
        GroupBox6.Controls.Add(txtNomEquipementService)
        GroupBox6.Controls.Add(txtCodeEquipementService)
        GroupBox6.Controls.Add(Label34)
        GroupBox6.Controls.Add(Label29)
        GroupBox6.Controls.Add(Label3)
        GroupBox6.Controls.Add(Label4)
        GroupBox6.Controls.Add(txtQuantiteService)
        GroupBox6.Location = New Point(819, 77)
        GroupBox6.Name = "GroupBox6"
        GroupBox6.Size = New Size(125, 46)
        GroupBox6.TabIndex = 272
        GroupBox6.TabStop = False
        GroupBox6.Text = "Équipement du camion service"
        ' 
        ' Planification_Demands
        ' 
        AutoScaleMode = AutoScaleMode.None
        AutoScroll = True
        ClientSize = New Size(1082, 723)
        Controls.Add(Valider)
        Controls.Add(Label35)
        Controls.Add(GroupBox7)
        Controls.Add(GroupBox5)
        Controls.Add(GroupBox4)
        Controls.Add(GroupBox3)
        Controls.Add(GroupBox2)
        Controls.Add(GroupBox1)
        Controls.Add(Abandonner)
        Name = "Planification_Demands"
        StartPosition = FormStartPosition.CenterScreen
        Text = "Planification_Demands"
        GroupBox1.ResumeLayout(False)
        GroupBox1.PerformLayout()
        GroupBox2.ResumeLayout(False)
        GroupBox2.PerformLayout()
        GroupBox3.ResumeLayout(False)
        GroupBox3.PerformLayout()
        CType(DataGridViewCompositionPlanning, ComponentModel.ISupportInitialize).EndInit()
        GroupBox5.ResumeLayout(False)
        GroupBox5.PerformLayout()
        GroupBox4.ResumeLayout(False)
        GroupBox4.PerformLayout()
        GroupBox7.ResumeLayout(False)
        GroupBox7.PerformLayout()
        GroupBox6.ResumeLayout(False)
        GroupBox6.PerformLayout()
        ResumeLayout(False)
        PerformLayout()
    End Sub
    Friend WithEvents GroupBox1 As GroupBox
    Friend WithEvents GroupBox2 As GroupBox
    Friend WithEvents Label11 As Label
    Friend WithEvents Label2 As Label
    Friend WithEvents Label1 As Label
    Friend WithEvents Label5 As Label
    Friend WithEvents Label6 As Label
    Friend WithEvents Label8 As Label
    Friend WithEvents GroupBox3 As GroupBox
    Friend WithEvents Label9 As Label
    Friend WithEvents Label13 As Label
    Friend WithEvents Label21 As Label
    Friend WithEvents Label20 As Label
    Friend WithEvents Label19 As Label
    Friend WithEvents Label18 As Label
    Friend WithEvents Label17 As Label
    Friend WithEvents txtMasseAlcalinCalc As TextBox
    Friend WithEvents txtTempsMalaxage As TextBox
    Friend WithEvents txtMES As TextBox
    Friend WithEvents txtVolumeAir As TextBox
    Friend WithEvents txtTauxChlorureCalc As TextBox
    Friend WithEvents txtGamme As TextBox
    Friend WithEvents DataGridViewCompositionPlanning As DataGridView
    Friend WithEvents GroupBox5 As GroupBox
    Friend WithEvents Label27 As Label
    Friend WithEvents Label28 As Label
    Friend WithEvents Label25 As Label
    Friend WithEvents Label24 As Label
    Friend WithEvents txtCadence As TextBox
    Friend WithEvents txtDejaLivres As TextBox
    Friend WithEvents Label16 As Label
    Friend WithEvents Label15 As Label
    Friend WithEvents Label14 As Label
    Friend WithEvents txtTotalChantier As TextBox
    Friend WithEvents chkComplement As CheckBox
    Friend WithEvents chkAConfirmer As CheckBox
    Friend WithEvents dtpDateLivraison As DateTimePicker
    Friend WithEvents txtVolumeRotationMaxi As TextBox
    Friend WithEvents GroupBox4 As GroupBox
    Friend WithEvents Label7 As Label
    Friend WithEvents GroupBox7 As GroupBox
    Friend WithEvents TextBox17 As TextBox
    Friend WithEvents cmbUnitePS7 As ComboBox
    Friend WithEvents txtQtePS7 As TextBox
    Friend WithEvents cmbUnitePS6 As ComboBox
    Friend WithEvents txtQtePS6 As TextBox
    Friend WithEvents cmbUnitePS5 As ComboBox
    Friend WithEvents txtQtePS5 As TextBox
    Friend WithEvents cmbUnitePS4 As ComboBox
    Friend WithEvents txtQtePS4 As TextBox
    Friend WithEvents cmbUnitePS3 As ComboBox
    Friend WithEvents txtQtePS3 As TextBox
    Friend WithEvents cmbUnitePS2 As ComboBox
    Friend WithEvents txtQtePS2 As TextBox
    Friend WithEvents cmbUnitePS1 As ComboBox
    Friend WithEvents txtQtePS1 As TextBox
    Friend WithEvents Label30 As Label
    Friend WithEvents Label22 As Label
    Friend WithEvents Label23 As Label
    Friend WithEvents Button2 As Button
    Friend WithEvents btnJourPlus As Button
    Friend WithEvents btnPlusHeureFabrication As Button
    Friend WithEvents txtHeureLivraison As TextBox
    Friend WithEvents txtHeureFabrication As TextBox
    Friend WithEvents btnMoinsMinuteLivraison As Button
    Friend WithEvents btnPlusMinuteLivraison As Button
    Friend WithEvents btnMoinsHeureLivraison As Button
    Friend WithEvents btnPlusHeureLivraison As Button
    Friend WithEvents btnMoinsMinuteFabrication As Button
    Friend WithEvents btnPlusMinuteFabrication As Button
    Friend WithEvents btnMoinsHeureFabrication As Button
    Friend WithEvents Label33 As Label
    Friend WithEvents txtCapaciteCamion As TextBox
    Friend WithEvents Label35 As Label
    Friend WithEvents Label36 As Label
    Friend WithEvents Label37 As Label
    Friend WithEvents Label38 As Label
    Friend WithEvents Label32 As Label
    Friend WithEvents Label31 As Label
    Friend WithEvents Abandonner As Button
    Friend WithEvents Valider As Button
    Friend WithEvents txtIdClient As TextBox
    Friend WithEvents txtNomClient As TextBox
    Friend WithEvents txtCodeClient As TextBox
    Friend WithEvents txtNomChantier As TextBox
    Friend WithEvents txtCodeChantier As TextBox
    Friend WithEvents txtIdChantier As TextBox
    Friend WithEvents txtNomFormule As TextBox
    Friend WithEvents txtCodeFormule As TextBox
    Friend WithEvents txtNomCamion As TextBox
    Friend WithEvents txtCodeCamion As TextBox
    Friend WithEvents txtNomChauffeur As TextBox
    Friend WithEvents txtCodeChauffeur As TextBox
    Friend WithEvents txtCodePS1 As TextBox
    Friend WithEvents txtCodePS3 As TextBox
    Friend WithEvents txtCodePS2 As TextBox
    Friend WithEvents txtCodePS5 As TextBox
    Friend WithEvents txtCodePS4 As TextBox
    Friend WithEvents txtCodePS7 As TextBox
    Friend WithEvents txtCodePS6 As TextBox
    Friend WithEvents Label12 As Label
    Friend WithEvents txtNomPS1 As TextBox
    Friend WithEvents txtNomPS3 As TextBox
    Friend WithEvents txtNomPS2 As TextBox
    Friend WithEvents txtNomPS5 As TextBox
    Friend WithEvents txtNomPS4 As TextBox
    Friend WithEvents txtNomPS7 As TextBox
    Friend WithEvents txtNomPS6 As TextBox
    Friend WithEvents Label4 As Label
    Friend WithEvents Label3 As Label
    Friend WithEvents Label29 As Label
    Friend WithEvents txtQuantiteService As TextBox
    Friend WithEvents Label34 As Label
    Friend WithEvents txtCodeEquipementService As TextBox
    Friend WithEvents txtNomEquipementService As TextBox
    Friend WithEvents GroupBox6 As GroupBox
End Class
