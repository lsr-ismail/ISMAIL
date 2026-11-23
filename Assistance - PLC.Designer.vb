<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class Assistance___PLC
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
        Label7 = New Label()
        Label8 = New Label()
        Label9 = New Label()
        Label10 = New Label()
        Label11 = New Label()
        Label12 = New Label()
        Label4 = New Label()
        lblTapis = New Label()
        lblEtat = New Label()
        Label3 = New Label()
        Label2 = New Label()
        Label1 = New Label()
        btnEcrire = New Button()
        CheckBoxVerrouille = New CheckBox()
        CheckBoxControleCredit = New CheckBox()
        CheckBoxAutorisationDosage = New CheckBox()
        txtValeurEcrire = New TextBox()
        txtValeurLue = New TextBox()
        txtAdresse = New TextBox()
        txtNbGachees = New TextBox()
        txtNiveauEau = New TextBox()
        txtPoidsActuel = New TextBox()
        txtSlot = New TextBox()
        txtRack = New TextBox()
        txtIP = New TextBox()
        btnArretUrgence = New Button()
        btnDepartCycle = New Button()
        btnDeconnecter = New Button()
        btnConnecter = New Button()
        lblMalaxeur = New Label()
        lblPorte = New Label()
        lblAlarme = New Label()
        lblCycle = New Label()
        lblDerniereMAJ = New Label()
        btnLire = New Button()
        btnResetAlarme = New Button()
        lstLog = New ListBox()
        lblBascEAU = New Label()
        lblBascCIM = New Label()
        lblBascAGR = New Label()
        lblBascADJ = New Label()
        SuspendLayout()
        ' 
        ' Label21
        ' 
        Label21.AutoSize = True
        Label21.Font = New Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point, CByte(0))
        Label21.Location = New Point(12, 9)
        Label21.Name = "Label21"
        Label21.Size = New Size(156, 28)
        Label21.TabIndex = 149
        Label21.Text = "Assistance - PLC"
        ' 
        ' Label7
        ' 
        Label7.AutoSize = True
        Label7.Location = New Point(36, 498)
        Label7.Name = "Label7"
        Label7.Size = New Size(49, 20)
        Label7.TabIndex = 285
        Label7.Text = "lstLog"
        ' 
        ' Label8
        ' 
        Label8.AutoSize = True
        Label8.Location = New Point(36, 461)
        Label8.Name = "Label8"
        Label8.Size = New Size(104, 20)
        Label8.TabIndex = 284
        Label8.Text = "Valeur à écrire"
        ' 
        ' Label9
        ' 
        Label9.AutoSize = True
        Label9.Location = New Point(38, 428)
        Label9.Name = "Label9"
        Label9.Size = New Size(90, 20)
        Label9.TabIndex = 283
        Label9.Text = "Valeur lue"
        ' 
        ' Label10
        ' 
        Label10.AutoSize = True
        Label10.Location = New Point(41, 395)
        Label10.Name = "Label10"
        Label10.Size = New Size(78, 20)
        Label10.TabIndex = 282
        Label10.Text = "Adresse"
        ' 
        ' Label11
        ' 
        Label11.AutoSize = True
        Label11.Location = New Point(39, 362)
        Label11.Name = "Label11"
        Label11.Size = New Size(101, 20)
        Label11.TabIndex = 281
        Label11.Text = "Nb gâchées"
        ' 
        ' Label12
        ' 
        Label12.AutoSize = True
        Label12.Location = New Point(41, 329)
        Label12.Name = "Label12"
        Label12.Size = New Size(96, 20)
        Label12.TabIndex = 280
        Label12.Text = "Niveau eau"
        ' 
        ' Label4
        ' 
        Label4.AutoSize = True
        Label4.Location = New Point(39, 296)
        Label4.Name = "Label4"
        Label4.Size = New Size(103, 20)
        Label4.TabIndex = 279
        Label4.Text = "Poids actuel"
        ' 
        ' lblTapis
        ' 
        lblTapis.AutoSize = True
        lblTapis.Location = New Point(621, 220)
        lblTapis.Name = "lblTapis"
        lblTapis.Size = New Size(59, 20)
        lblTapis.TabIndex = 278
        lblTapis.Text = "—"
        ' 
        ' lblEtat
        ' 
        lblEtat.AutoSize = True
        lblEtat.Location = New Point(621, 144)
        lblEtat.Name = "lblEtat"
        lblEtat.Size = New Size(52, 20)
        lblEtat.TabIndex = 277
        lblEtat.Text = "—"
        ' 
        ' Label3
        ' 
        Label3.AutoSize = True
        Label3.Location = New Point(39, 197)
        Label3.Name = "Label3"
        Label3.Size = New Size(52, 20)
        Label3.TabIndex = 276
        Label3.Text = "Slot"
        ' 
        ' Label2
        ' 
        Label2.AutoSize = True
        Label2.Location = New Point(39, 164)
        Label2.Name = "Label2"
        Label2.Size = New Size(57, 20)
        Label2.TabIndex = 275
        Label2.Text = "Rack"
        ' 
        ' Label1
        ' 
        Label1.AutoSize = True
        Label1.Location = New Point(39, 131)
        Label1.Name = "Label1"
        Label1.Size = New Size(38, 20)
        Label1.TabIndex = 274
        Label1.Text = "Adresse IP"
        ' 
        ' btnEcrire
        ' 
        btnEcrire.Location = New Point(872, 60)
        btnEcrire.Name = "btnEcrire"
        btnEcrire.Size = New Size(166, 61)
        btnEcrire.TabIndex = 273
        btnEcrire.Text = "Écrire"
        btnEcrire.UseVisualStyleBackColor = True
        ' 
        ' CheckBoxVerrouille
        ' 
        CheckBoxVerrouille.AutoSize = True
        CheckBoxVerrouille.Location = New Point(347, 625)
        CheckBoxVerrouille.Name = "CheckBoxVerrouille"
        CheckBoxVerrouille.Size = New Size(94, 24)
        CheckBoxVerrouille.TabIndex = 272
        CheckBoxVerrouille.Text = "Verrouillé"
        CheckBoxVerrouille.UseVisualStyleBackColor = True
        ' 
        ' CheckBoxControleCredit
        ' 
        CheckBoxControleCredit.AutoSize = True
        CheckBoxControleCredit.Location = New Point(347, 595)
        CheckBoxControleCredit.Name = "CheckBoxControleCredit"
        CheckBoxControleCredit.Size = New Size(130, 24)
        CheckBoxControleCredit.TabIndex = 271
        CheckBoxControleCredit.Text = "Contrôle crédit"
        CheckBoxControleCredit.UseVisualStyleBackColor = True
        ' 
        ' CheckBoxAutorisationDosage
        ' 
        CheckBoxAutorisationDosage.AutoSize = True
        CheckBoxAutorisationDosage.Location = New Point(347, 565)
        CheckBoxAutorisationDosage.Name = "CheckBoxAutorisationDosage"
        CheckBoxAutorisationDosage.Size = New Size(165, 24)
        CheckBoxAutorisationDosage.TabIndex = 270
        CheckBoxAutorisationDosage.Text = "Autorisation dosage"
        CheckBoxAutorisationDosage.UseVisualStyleBackColor = True
        ' 
        ' txtValeurEcrire
        ' 
        txtValeurEcrire.Location = New Point(155, 457)
        txtValeurEcrire.Name = "txtValeurEcrire"
        txtValeurEcrire.Size = New Size(346, 27)
        txtValeurEcrire.TabIndex = 267
        ' 
        ' txtValeurLue
        ' 
        txtValeurLue.Location = New Point(155, 424)
        txtValeurLue.Name = "txtValeurLue"
        txtValeurLue.Size = New Size(346, 27)
        txtValeurLue.TabIndex = 266
        ' 
        ' txtAdresse
        ' 
        txtAdresse.Location = New Point(155, 391)
        txtAdresse.Name = "txtAdresse"
        txtAdresse.Size = New Size(346, 27)
        txtAdresse.TabIndex = 265
        ' 
        ' txtNbGachees
        ' 
        txtNbGachees.Location = New Point(155, 358)
        txtNbGachees.Name = "txtNbGachees"
        txtNbGachees.Size = New Size(346, 27)
        txtNbGachees.TabIndex = 264
        ' 
        ' txtNiveauEau
        ' 
        txtNiveauEau.Location = New Point(155, 325)
        txtNiveauEau.Name = "txtNiveauEau"
        txtNiveauEau.Size = New Size(346, 27)
        txtNiveauEau.TabIndex = 263
        ' 
        ' txtPoidsActuel
        ' 
        txtPoidsActuel.Location = New Point(155, 292)
        txtPoidsActuel.Name = "txtPoidsActuel"
        txtPoidsActuel.Size = New Size(346, 27)
        txtPoidsActuel.TabIndex = 262
        ' 
        ' txtSlot
        ' 
        txtSlot.Location = New Point(155, 193)
        txtSlot.Name = "txtSlot"
        txtSlot.Size = New Size(346, 27)
        txtSlot.TabIndex = 259
        ' 
        ' txtRack
        ' 
        txtRack.Location = New Point(155, 160)
        txtRack.Name = "txtRack"
        txtRack.Size = New Size(346, 27)
        txtRack.TabIndex = 258
        ' 
        ' txtIP
        ' 
        txtIP.Location = New Point(155, 127)
        txtIP.Name = "txtIP"
        txtIP.Size = New Size(346, 27)
        txtIP.TabIndex = 256
        ' 
        ' btnArretUrgence
        ' 
        btnArretUrgence.Location = New Point(528, 60)
        btnArretUrgence.Name = "btnArretUrgence"
        btnArretUrgence.Size = New Size(166, 61)
        btnArretUrgence.TabIndex = 255
        btnArretUrgence.Text = "Arrêt d'urgence"
        btnArretUrgence.UseVisualStyleBackColor = True
        ' 
        ' btnDepartCycle
        ' 
        btnDepartCycle.Location = New Point(356, 60)
        btnDepartCycle.Name = "btnDepartCycle"
        btnDepartCycle.Size = New Size(166, 61)
        btnDepartCycle.TabIndex = 254
        btnDepartCycle.Text = "Départ cycle"
        btnDepartCycle.UseVisualStyleBackColor = True
        ' 
        ' btnDeconnecter
        ' 
        btnDeconnecter.Location = New Point(184, 60)
        btnDeconnecter.Name = "btnDeconnecter"
        btnDeconnecter.Size = New Size(166, 61)
        btnDeconnecter.TabIndex = 253
        btnDeconnecter.Text = "Déconnecter"
        btnDeconnecter.UseVisualStyleBackColor = True
        ' 
        ' btnConnecter
        ' 
        btnConnecter.Location = New Point(12, 60)
        btnConnecter.Name = "btnConnecter"
        btnConnecter.Size = New Size(166, 61)
        btnConnecter.TabIndex = 252
        btnConnecter.Text = "Connecter"
        btnConnecter.UseVisualStyleBackColor = True
        ' 
        ' lblMalaxeur
        ' 
        lblMalaxeur.AutoSize = True
        lblMalaxeur.Location = New Point(621, 164)
        lblMalaxeur.Name = "lblMalaxeur"
        lblMalaxeur.Size = New Size(87, 20)
        lblMalaxeur.TabIndex = 287
        lblMalaxeur.Text = "—"
        ' 
        ' lblPorte
        ' 
        lblPorte.AutoSize = True
        lblPorte.Location = New Point(622, 193)
        lblPorte.Name = "lblPorte"
        lblPorte.Size = New Size(60, 20)
        lblPorte.TabIndex = 288
        lblPorte.Text = "—"
        ' 
        ' lblAlarme
        ' 
        lblAlarme.AutoSize = True
        lblAlarme.Location = New Point(622, 250)
        lblAlarme.Name = "lblAlarme"
        lblAlarme.Size = New Size(74, 20)
        lblAlarme.TabIndex = 289
        lblAlarme.Text = "—"
        ' 
        ' lblCycle
        ' 
        lblCycle.AutoSize = True
        lblCycle.Location = New Point(621, 279)
        lblCycle.Name = "lblCycle"
        lblCycle.Size = New Size(61, 20)
        lblCycle.TabIndex = 290
        lblCycle.Text = "—"
        ' 
        ' lblDerniereMAJ
        ' 
        lblDerniereMAJ.AutoSize = True
        lblDerniereMAJ.Location = New Point(621, 313)
        lblDerniereMAJ.Name = "lblDerniereMAJ"
        lblDerniereMAJ.Size = New Size(112, 20)
        lblDerniereMAJ.TabIndex = 291
        lblDerniereMAJ.Text = "—"
        ' 
        ' btnLire
        ' 
        btnLire.Location = New Point(700, 60)
        btnLire.Name = "btnLire"
        btnLire.Size = New Size(166, 61)
        btnLire.TabIndex = 292
        btnLire.Text = "Lire"
        btnLire.UseVisualStyleBackColor = True
        ' 
        ' btnResetAlarme
        ' 
        btnResetAlarme.Location = New Point(841, -4)
        btnResetAlarme.Name = "btnResetAlarme"
        btnResetAlarme.Size = New Size(166, 61)
        btnResetAlarme.TabIndex = 293
        btnResetAlarme.Text = "Reset alarme"
        btnResetAlarme.UseVisualStyleBackColor = True
        ' 
        ' lstLog
        ' 
        lstLog.FormattingEnabled = True
        lstLog.Location = New Point(155, 498)
        lstLog.Name = "lstLog"
        lstLog.Size = New Size(150, 104)
        lstLog.TabIndex = 294
        ' 
        ' lblBascEAU
        ' 
        lblBascEAU.AutoSize = True
        lblBascEAU.Location = New Point(634, 471)
        lblBascEAU.Name = "lblBascEAU"
        lblBascEAU.Size = New Size(84, 20)
        lblBascEAU.TabIndex = 295
        lblBascEAU.Text = "—"
        ' 
        ' lblBascCIM
        ' 
        lblBascCIM.AutoSize = True
        lblBascCIM.Location = New Point(634, 451)
        lblBascCIM.Name = "lblBascCIM"
        lblBascCIM.Size = New Size(82, 20)
        lblBascCIM.TabIndex = 296
        lblBascCIM.Text = "—"
        ' 
        ' lblBascAGR
        ' 
        lblBascAGR.AutoSize = True
        lblBascAGR.Location = New Point(634, 431)
        lblBascAGR.Name = "lblBascAGR"
        lblBascAGR.Size = New Size(85, 20)
        lblBascAGR.TabIndex = 297
        lblBascAGR.Text = "—"
        ' 
        ' lblBascADJ
        ' 
        lblBascADJ.AutoSize = True
        lblBascADJ.Location = New Point(635, 491)
        lblBascADJ.Name = "lblBascADJ"
        lblBascADJ.Size = New Size(82, 20)
        lblBascADJ.TabIndex = 298
        lblBascADJ.Text = "—"
        ' 
        ' Assistance___PLC
        ' 
        AutoScaleDimensions = New SizeF(8F, 20F)
        AutoScaleMode = AutoScaleMode.Font
        ClientSize = New Size(1082, 723)
        Controls.Add(lblBascADJ)
        Controls.Add(lblBascAGR)
        Controls.Add(lblBascCIM)
        Controls.Add(lblBascEAU)
        Controls.Add(lstLog)
        Controls.Add(btnResetAlarme)
        Controls.Add(btnLire)
        Controls.Add(lblDerniereMAJ)
        Controls.Add(lblCycle)
        Controls.Add(lblAlarme)
        Controls.Add(lblPorte)
        Controls.Add(lblMalaxeur)
        Controls.Add(Label7)
        Controls.Add(Label8)
        Controls.Add(Label9)
        Controls.Add(Label10)
        Controls.Add(Label11)
        Controls.Add(Label12)
        Controls.Add(Label4)
        Controls.Add(lblTapis)
        Controls.Add(lblEtat)
        Controls.Add(Label3)
        Controls.Add(Label2)
        Controls.Add(Label1)
        Controls.Add(btnEcrire)
        Controls.Add(CheckBoxVerrouille)
        Controls.Add(CheckBoxControleCredit)
        Controls.Add(CheckBoxAutorisationDosage)
        Controls.Add(txtValeurEcrire)
        Controls.Add(txtValeurLue)
        Controls.Add(txtAdresse)
        Controls.Add(txtNbGachees)
        Controls.Add(txtNiveauEau)
        Controls.Add(txtPoidsActuel)
        Controls.Add(txtSlot)
        Controls.Add(txtRack)
        Controls.Add(txtIP)
        Controls.Add(btnArretUrgence)
        Controls.Add(btnDepartCycle)
        Controls.Add(btnDeconnecter)
        Controls.Add(btnConnecter)
        Controls.Add(Label21)
        Name = "Assistance___PLC"
        Text = "Assistance PLC"
        ResumeLayout(False)
        PerformLayout()
    End Sub

    Friend WithEvents Label21 As Label
    Friend WithEvents Label7 As Label
    Friend WithEvents Label8 As Label
    Friend WithEvents Label9 As Label
    Friend WithEvents Label10 As Label
    Friend WithEvents Label11 As Label
    Friend WithEvents Label12 As Label
    Friend WithEvents Label4 As Label
    Friend WithEvents lblTapis As Label
    Friend WithEvents lblEtat As Label
    Friend WithEvents Label3 As Label
    Friend WithEvents Label2 As Label
    Friend WithEvents Label1 As Label
    Friend WithEvents btnEcrire As Button
    Friend WithEvents CheckBoxVerrouille As CheckBox
    Friend WithEvents CheckBoxControleCredit As CheckBox
    Friend WithEvents CheckBoxAutorisationDosage As CheckBox
    Friend WithEvents txtValeurEcrire As TextBox
    Friend WithEvents txtValeurLue As TextBox
    Friend WithEvents txtAdresse As TextBox
    Friend WithEvents txtNbGachees As TextBox
    Friend WithEvents txtNiveauEau As TextBox
    Friend WithEvents txtPoidsActuel As TextBox
    Friend WithEvents txtSlot As TextBox
    Friend WithEvents txtRack As TextBox
    Friend WithEvents txtIP As TextBox
    Friend WithEvents btnArretUrgence As Button
    Friend WithEvents btnDepartCycle As Button
    Friend WithEvents btnDeconnecter As Button
    Friend WithEvents btnConnecter As Button
    Friend WithEvents lblMalaxeur As Label
    Friend WithEvents lblPorte As Label
    Friend WithEvents lblAlarme As Label
    Friend WithEvents lblCycle As Label
    Friend WithEvents lblDerniereMAJ As Label
    Friend WithEvents btnLire As Button
    Friend WithEvents btnResetAlarme As Button
    Friend WithEvents lstLog As ListBox
    Friend WithEvents lblBascEAU As Label
    Friend WithEvents lblBascCIM As Label
    Friend WithEvents lblBascAGR As Label
    Friend WithEvents lblBascADJ As Label
End Class
