<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class Prévisions_de_Matières_Premières
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
        Label516 = New Label()
        Label518 = New Label()
        btnEnregPesees = New Button()
        Label11 = New Label()
        btnBasculeHorsLigne = New Button()
        Label541 = New Label()
        btnHygroMoyenne = New Button()
        Label1 = New Label()
        Label14 = New Label()
        Label20 = New Label()
        Label25 = New Label()
        txtEcartADJ = New TextBox()
        txtEcartEAU = New TextBox()
        Label26 = New Label()
        txtDensiteFines = New TextBox()
        txtMasseEauMin = New TextBox()
        Label351 = New Label()
        cmbCondLancement = New ComboBox()
        txtEcartL = New TextBox()
        txtEcartGR = New TextBox()
        Label2 = New Label()
        btnMarcheSelectif = New Button()
        Label3 = New Label()
        btnDeuxiemeCourbe = New Button()
        Label4 = New Label()
        Label5 = New Label()
        btnManuelAutomate = New Button()
        btnVoyants3 = New Button()
        Label6 = New Label()
        btnRapportEC = New Button()
        Label7 = New Label()
        txtTempsSignalFin = New TextBox()
        Label8 = New Label()
        rbDensite = New RadioButton()
        rbTauxMassique = New RadioButton()
        SuspendLayout()
        ' 
        ' Label21
        ' 
        Label21.AutoSize = True
        Label21.Font = New Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point, CByte(0))
        Label21.Location = New Point(12, 9)
        Label21.Name = "Label21"
        Label21.Size = New Size(305, 28)
        Label21.TabIndex = 153
        Label21.Text = "Prévisions de Matières Premières"
        ' 
        ' Label516
        ' 
        Label516.AutoSize = True
        Label516.Font = New Font("Segoe UI", 10.2F, FontStyle.Regular, GraphicsUnit.Point, CByte(0))
        Label516.Location = New Point(45, 454)
        Label516.Name = "Label516"
        Label516.Size = New Size(242, 23)
        Label516.TabIndex = 629
        Label516.Text = "Gestion hygrométrie moyenne"
        ' 
        ' Label518
        ' 
        Label518.AutoSize = True
        Label518.Font = New Font("Segoe UI", 10.2F, FontStyle.Regular, GraphicsUnit.Point, CByte(0))
        Label518.Location = New Point(45, 365)
        Label518.Name = "Label518"
        Label518.Size = New Size(138, 23)
        Label518.TabIndex = 628
        Label518.Text = "Densité des fines"
        ' 
        ' btnEnregPesees
        ' 
        btnEnregPesees.Location = New Point(412, 81)
        btnEnregPesees.Name = "btnEnregPesees"
        btnEnregPesees.Size = New Size(94, 29)
        btnEnregPesees.TabIndex = 719
        btnEnregPesees.Text = "NON"
        btnEnregPesees.UseVisualStyleBackColor = True
        ' 
        ' Label11
        ' 
        Label11.AutoSize = True
        Label11.Font = New Font("Segoe UI", 10.2F, FontStyle.Regular, GraphicsUnit.Point, CByte(0))
        Label11.Location = New Point(45, 58)
        Label11.Name = "Label11"
        Label11.Size = New Size(218, 23)
        Label11.TabIndex = 717
        Label11.Text = "Contrôle Bascule hors ligne"
        ' 
        ' btnBasculeHorsLigne
        ' 
        btnBasculeHorsLigne.Location = New Point(412, 52)
        btnBasculeHorsLigne.Name = "btnBasculeHorsLigne"
        btnBasculeHorsLigne.Size = New Size(94, 29)
        btnBasculeHorsLigne.TabIndex = 716
        btnBasculeHorsLigne.Text = "NON"
        btnBasculeHorsLigne.UseVisualStyleBackColor = True
        ' 
        ' Label541
        ' 
        Label541.AutoSize = True
        Label541.Font = New Font("Segoe UI", 10.2F, FontStyle.Regular, GraphicsUnit.Point, CByte(0))
        Label541.Location = New Point(45, 332)
        Label541.Name = "Label541"
        Label541.Size = New Size(246, 23)
        Label541.TabIndex = 722
        Label541.Text = "Masse eau claire minimum (kg)"
        ' 
        ' btnHygroMoyenne
        ' 
        btnHygroMoyenne.Location = New Point(482, 454)
        btnHygroMoyenne.Name = "btnHygroMoyenne"
        btnHygroMoyenne.Size = New Size(94, 29)
        btnHygroMoyenne.TabIndex = 733
        btnHygroMoyenne.Text = "NON"
        btnHygroMoyenne.UseVisualStyleBackColor = True
        ' 
        ' Label1
        ' 
        Label1.AutoSize = True
        Label1.Font = New Font("Segoe UI", 10.2F, FontStyle.Regular, GraphicsUnit.Point, CByte(0))
        Label1.Location = New Point(45, 87)
        Label1.Name = "Label1"
        Label1.Size = New Size(344, 23)
        Label1.TabIndex = 745
        Label1.Text = "Enregistrement pesées sur ouverture trappe"
        ' 
        ' Label14
        ' 
        Label14.AutoSize = True
        Label14.Font = New Font("Segoe UI", 10.2F, FontStyle.Regular, GraphicsUnit.Point, CByte(0))
        Label14.Location = New Point(47, 216)
        Label14.Name = "Label14"
        Label14.Size = New Size(156, 23)
        Label14.TabIndex = 785
        Label14.Text = "Écart max. liant (%)"
        ' 
        ' Label20
        ' 
        Label20.AutoSize = True
        Label20.Font = New Font("Segoe UI", 10.2F, FontStyle.Regular, GraphicsUnit.Point, CByte(0))
        Label20.Location = New Point(45, 187)
        Label20.Name = "Label20"
        Label20.Size = New Size(187, 23)
        Label20.TabIndex = 784
        Label20.Text = "Écart max. granulat (%)"
        ' 
        ' Label25
        ' 
        Label25.AutoSize = True
        Label25.Font = New Font("Segoe UI", 10.2F, FontStyle.Regular, GraphicsUnit.Point, CByte(0))
        Label25.Location = New Point(44, 275)
        Label25.Name = "Label25"
        Label25.Size = New Size(189, 23)
        Label25.TabIndex = 790
        Label25.Text = "Écart max. adjuvant (%)"
        ' 
        ' txtEcartADJ
        ' 
        txtEcartADJ.Location = New Point(480, 275)
        txtEcartADJ.Name = "txtEcartADJ"
        txtEcartADJ.Size = New Size(96, 27)
        txtEcartADJ.TabIndex = 788
        ' 
        ' txtEcartEAU
        ' 
        txtEcartEAU.Location = New Point(480, 246)
        txtEcartEAU.Name = "txtEcartEAU"
        txtEcartEAU.Size = New Size(96, 27)
        txtEcartEAU.TabIndex = 787
        ' 
        ' Label26
        ' 
        Label26.AutoSize = True
        Label26.Font = New Font("Segoe UI", 10.2F, FontStyle.Regular, GraphicsUnit.Point, CByte(0))
        Label26.Location = New Point(47, 246)
        Label26.Name = "Label26"
        Label26.Size = New Size(151, 23)
        Label26.TabIndex = 786
        Label26.Text = "Écart max. eau (%)"
        ' 
        ' txtDensiteFines
        ' 
        txtDensiteFines.Location = New Point(481, 361)
        txtDensiteFines.Name = "txtDensiteFines"
        txtDensiteFines.Size = New Size(96, 27)
        txtDensiteFines.TabIndex = 792
        ' 
        ' txtMasseEauMin
        ' 
        txtMasseEauMin.Location = New Point(481, 332)
        txtMasseEauMin.Name = "txtMasseEauMin"
        txtMasseEauMin.Size = New Size(96, 27)
        txtMasseEauMin.TabIndex = 791
        ' 
        ' Label351
        ' 
        Label351.AutoSize = True
        Label351.Font = New Font("Segoe UI", 10.2F, FontStyle.Regular, GraphicsUnit.Point, CByte(0))
        Label351.Location = New Point(45, 543)
        Label351.Name = "Label351"
        Label351.Size = New Size(278, 23)
        Label351.TabIndex = 794
        Label351.Text = "Condition lancement appels postes"
        ' 
        ' cmbCondLancement
        ' 
        cmbCondLancement.FormattingEnabled = True
        cmbCondLancement.Location = New Point(392, 543)
        cmbCondLancement.Name = "cmbCondLancement"
        cmbCondLancement.Size = New Size(182, 28)
        cmbCondLancement.TabIndex = 793
        ' 
        ' txtEcartL
        ' 
        txtEcartL.Location = New Point(480, 216)
        txtEcartL.Name = "txtEcartL"
        txtEcartL.Size = New Size(96, 27)
        txtEcartL.TabIndex = 798
        ' 
        ' txtEcartGR
        ' 
        txtEcartGR.Location = New Point(480, 187)
        txtEcartGR.Name = "txtEcartGR"
        txtEcartGR.Size = New Size(96, 27)
        txtEcartGR.TabIndex = 797
        ' 
        ' Label2
        ' 
        Label2.AutoSize = True
        Label2.Font = New Font("Segoe UI", 10.2F, FontStyle.Regular, GraphicsUnit.Point, CByte(0))
        Label2.Location = New Point(45, 135)
        Label2.Name = "Label2"
        Label2.Size = New Size(188, 23)
        Label2.TabIndex = 800
        Label2.Text = "Marche forcée sélective"
        ' 
        ' btnMarcheSelectif
        ' 
        btnMarcheSelectif.Location = New Point(412, 129)
        btnMarcheSelectif.Name = "btnMarcheSelectif"
        btnMarcheSelectif.Size = New Size(94, 29)
        btnMarcheSelectif.TabIndex = 799
        btnMarcheSelectif.Text = "NON"
        btnMarcheSelectif.UseVisualStyleBackColor = True
        ' 
        ' Label3
        ' 
        Label3.AutoSize = True
        Label3.Font = New Font("Segoe UI", 10.2F, FontStyle.Regular, GraphicsUnit.Point, CByte(0))
        Label3.Location = New Point(599, 118)
        Label3.Name = "Label3"
        Label3.Size = New Size(312, 23)
        Label3.TabIndex = 806
        Label3.Text = "Gestion deuxième courbe wattmétrique"
        ' 
        ' btnDeuxiemeCourbe
        ' 
        btnDeuxiemeCourbe.Location = New Point(966, 116)
        btnDeuxiemeCourbe.Name = "btnDeuxiemeCourbe"
        btnDeuxiemeCourbe.Size = New Size(94, 29)
        btnDeuxiemeCourbe.TabIndex = 805
        btnDeuxiemeCourbe.Text = "NON"
        btnDeuxiemeCourbe.UseVisualStyleBackColor = True
        ' 
        ' Label4
        ' 
        Label4.AutoSize = True
        Label4.Font = New Font("Segoe UI", 10.2F, FontStyle.Regular, GraphicsUnit.Point, CByte(0))
        Label4.Location = New Point(599, 87)
        Label4.Name = "Label4"
        Label4.Size = New Size(266, 23)
        Label4.TabIndex = 804
        Label4.Text = "Temps signal fin de production (s)"
        ' 
        ' Label5
        ' 
        Label5.AutoSize = True
        Label5.Font = New Font("Segoe UI", 10.2F, FontStyle.Regular, GraphicsUnit.Point, CByte(0))
        Label5.Location = New Point(599, 58)
        Label5.Name = "Label5"
        Label5.Size = New Size(215, 23)
        Label5.TabIndex = 802
        Label5.Text = "Manuel géré par automate"
        ' 
        ' btnManuelAutomate
        ' 
        btnManuelAutomate.Location = New Point(966, 52)
        btnManuelAutomate.Name = "btnManuelAutomate"
        btnManuelAutomate.Size = New Size(94, 29)
        btnManuelAutomate.TabIndex = 801
        btnManuelAutomate.Text = "NON"
        btnManuelAutomate.UseVisualStyleBackColor = True
        ' 
        ' btnVoyants3
        ' 
        btnVoyants3.Location = New Point(480, 508)
        btnVoyants3.Name = "btnVoyants3"
        btnVoyants3.Size = New Size(94, 29)
        btnVoyants3.TabIndex = 808
        btnVoyants3.Text = "NON"
        btnVoyants3.UseVisualStyleBackColor = True
        ' 
        ' Label6
        ' 
        Label6.AutoSize = True
        Label6.Font = New Font("Segoe UI", 10.2F, FontStyle.Regular, GraphicsUnit.Point, CByte(0))
        Label6.Location = New Point(43, 508)
        Label6.Name = "Label6"
        Label6.Size = New Size(238, 23)
        Label6.TabIndex = 807
        Label6.Text = "Gestion 3 voyants mode préfa"
        ' 
        ' btnRapportEC
        ' 
        btnRapportEC.Location = New Point(480, 577)
        btnRapportEC.Name = "btnRapportEC"
        btnRapportEC.Size = New Size(94, 29)
        btnRapportEC.TabIndex = 810
        btnRapportEC.Text = "NON"
        btnRapportEC.UseVisualStyleBackColor = True
        ' 
        ' Label7
        ' 
        Label7.AutoSize = True
        Label7.Font = New Font("Segoe UI", 10.2F, FontStyle.Regular, GraphicsUnit.Point, CByte(0))
        Label7.Location = New Point(43, 577)
        Label7.Name = "Label7"
        Label7.Size = New Size(292, 23)
        Label7.TabIndex = 809
        Label7.Text = "Affichage rapport E/C sur synoptique"
        ' 
        ' txtTempsSignalFin
        ' 
        txtTempsSignalFin.Location = New Point(966, 87)
        txtTempsSignalFin.Name = "txtTempsSignalFin"
        txtTempsSignalFin.Size = New Size(96, 27)
        txtTempsSignalFin.TabIndex = 811
        ' 
        ' Label8
        ' 
        Label8.AutoSize = True
        Label8.Font = New Font("Segoe UI", 10.2F, FontStyle.Regular, GraphicsUnit.Point, CByte(0))
        Label8.Location = New Point(47, 398)
        Label8.Name = "Label8"
        Label8.Size = New Size(166, 23)
        Label8.TabIndex = 812
        Label8.Text = "Gestion sonde MES :"
        ' 
        ' rbDensite
        ' 
        rbDensite.AutoSize = True
        rbDensite.Location = New Point(499, 424)
        rbDensite.Name = "rbDensite"
        rbDensite.Size = New Size(80, 24)
        rbDensite.TabIndex = 813
        rbDensite.TabStop = True
        rbDensite.Text = "Densité"
        rbDensite.UseVisualStyleBackColor = True
        ' 
        ' rbTauxMassique
        ' 
        rbTauxMassique.AutoSize = True
        rbTauxMassique.Location = New Point(499, 397)
        rbTauxMassique.Name = "rbTauxMassique"
        rbTauxMassique.Size = New Size(125, 24)
        rbTauxMassique.TabIndex = 814
        rbTauxMassique.TabStop = True
        rbTauxMassique.Text = "Taux massique"
        rbTauxMassique.UseVisualStyleBackColor = True
        ' 
        ' Prévisions_de_Matières_Premières
        ' 
        AutoScaleDimensions = New SizeF(8F, 20F)
        AutoScaleMode = AutoScaleMode.Font
        ClientSize = New Size(1069, 719)
        Controls.Add(rbTauxMassique)
        Controls.Add(rbDensite)
        Controls.Add(Label8)
        Controls.Add(txtTempsSignalFin)
        Controls.Add(btnRapportEC)
        Controls.Add(Label7)
        Controls.Add(btnVoyants3)
        Controls.Add(Label6)
        Controls.Add(Label3)
        Controls.Add(btnDeuxiemeCourbe)
        Controls.Add(Label4)
        Controls.Add(Label5)
        Controls.Add(btnManuelAutomate)
        Controls.Add(Label2)
        Controls.Add(btnMarcheSelectif)
        Controls.Add(txtEcartL)
        Controls.Add(txtEcartGR)
        Controls.Add(Label351)
        Controls.Add(cmbCondLancement)
        Controls.Add(txtDensiteFines)
        Controls.Add(txtMasseEauMin)
        Controls.Add(Label25)
        Controls.Add(txtEcartADJ)
        Controls.Add(txtEcartEAU)
        Controls.Add(Label26)
        Controls.Add(Label14)
        Controls.Add(Label20)
        Controls.Add(Label1)
        Controls.Add(btnHygroMoyenne)
        Controls.Add(Label541)
        Controls.Add(btnEnregPesees)
        Controls.Add(Label11)
        Controls.Add(btnBasculeHorsLigne)
        Controls.Add(Label516)
        Controls.Add(Label518)
        Controls.Add(Label21)
        Name = "Prévisions_de_Matières_Premières"
        Text = "Prévisions de Matières Premières"
        ResumeLayout(False)
        PerformLayout()
    End Sub

    Friend WithEvents Label21 As Label
    Friend WithEvents Button17 As Button
    Friend WithEvents Label535 As Label
    Friend WithEvents Button18 As Button
    Friend WithEvents Label536 As Label
    Friend WithEvents Button19 As Button
    Friend WithEvents Button20 As Button
    Friend WithEvents Button21 As Button
    Friend WithEvents Label516 As Label
    Friend WithEvents Label518 As Label
    Friend WithEvents btnEnregPesees As Button
    Friend WithEvents Label11 As Label
    Friend WithEvents btnBasculeHorsLigne As Button
    Friend WithEvents Label541 As Label
    Friend WithEvents btnHygroMoyenne As Button
    Friend WithEvents Label1 As Label
    Friend WithEvents Label14 As Label
    Friend WithEvents Label20 As Label
    Friend WithEvents Label25 As Label
    Friend WithEvents txtEcartADJ As TextBox
    Friend WithEvents txtEcartEAU As TextBox
    Friend WithEvents Label26 As Label
    Friend WithEvents txtDensiteFines As TextBox
    Friend WithEvents txtMasseEauMin As TextBox
    Friend WithEvents Label351 As Label
    Friend WithEvents cmbCondLancement As ComboBox
    Friend WithEvents txtEcartL As TextBox
    Friend WithEvents txtEcartGR As TextBox
    Friend WithEvents Label297 As Label
    Friend WithEvents txtMalPuissVide As TextBox
    Friend WithEvents btnMalMarcheImp As Button
    Friend WithEvents Label2 As Label
    Friend WithEvents btnMarcheSelectif As Button
    Friend WithEvents Label3 As Label
    Friend WithEvents btnDeuxiemeCourbe As Button
    Friend WithEvents Label4 As Label
    Friend WithEvents Label5 As Label
    Friend WithEvents btnManuelAutomate As Button
    Friend WithEvents btnVoyants3 As Button
    Friend WithEvents Label6 As Label
    Friend WithEvents btnRapportEC As Button
    Friend WithEvents Label7 As Label
    Friend WithEvents txtTempsSignalFin As TextBox
    Friend WithEvents Label8 As Label
    Friend WithEvents rbDensite As RadioButton
    Friend WithEvents rbTauxMassique As RadioButton
End Class
