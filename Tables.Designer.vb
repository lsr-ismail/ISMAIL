<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class Tables
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(Tables))
        txtRECHERCHE = New TextBox()
        btnActualiserTable = New Button()
        btnNouveauTable = New Button()
        lblNormee = New Label()
        lblCodeExport = New Label()
        lblServiceAssocie3 = New Label()
        lblServiceAssocie2 = New Label()
        lblServiceAssocie1 = New Label()
        lblNomElement = New Label()
        lblCodeElement = New Label()
        lblNomTable = New Label()
        Imprimer = New Button()
        txtCodeExport = New TextBox()
        txtServiceAssocie3 = New TextBox()
        txtServiceAssocie2 = New TextBox()
        txtServiceAssocie1 = New TextBox()
        txtNomElement = New TextBox()
        txtCodeElement = New TextBox()
        DataGridViewTables = New DataGridView()
        btnEditeTable = New Button()
        btnRechercheTable = New Button()
        btnSupprimeTable = New Button()
        btnAjouteTable = New Button()
        PrintPreviewDialog1 = New PrintPreviewDialog()
        PrintDocument1 = New Printing.PrintDocument()
        cmbNomTable = New ComboBox()
        chkNormee = New CheckBox()
        txtImpression = New TextBox()
        lblImpression = New Label()
        Label21 = New Label()
        CType(DataGridViewTables, ComponentModel.ISupportInitialize).BeginInit()
        SuspendLayout()
        ' 
        ' txtRECHERCHE
        ' 
        txtRECHERCHE.Location = New Point(304, 14)
        txtRECHERCHE.Name = "txtRECHERCHE"
        txtRECHERCHE.Size = New Size(418, 27)
        txtRECHERCHE.TabIndex = 208
        ' 
        ' btnActualiserTable
        ' 
        btnActualiserTable.Location = New Point(1152, 38)
        btnActualiserTable.Name = "btnActualiserTable"
        btnActualiserTable.Size = New Size(178, 61)
        btnActualiserTable.TabIndex = 207
        btnActualiserTable.Text = "Actualiser"
        btnActualiserTable.UseVisualStyleBackColor = True
        ' 
        ' btnNouveauTable
        ' 
        btnNouveauTable.Location = New Point(968, 38)
        btnNouveauTable.Name = "btnNouveauTable"
        btnNouveauTable.Size = New Size(178, 61)
        btnNouveauTable.TabIndex = 206
        btnNouveauTable.Text = "Nouveau"
        btnNouveauTable.UseVisualStyleBackColor = True
        ' 
        ' lblNormee
        ' 
        lblNormee.AutoSize = True
        lblNormee.Location = New Point(41, 353)
        lblNormee.Name = "lblNormee"
        lblNormee.Size = New Size(63, 20)
        lblNormee.TabIndex = 202
        lblNormee.Text = "Normee"
        ' 
        ' lblCodeExport
        ' 
        lblCodeExport.AutoSize = True
        lblCodeExport.Location = New Point(43, 320)
        lblCodeExport.Name = "lblCodeExport"
        lblCodeExport.Size = New Size(91, 20)
        lblCodeExport.TabIndex = 201
        lblCodeExport.Text = "Code Export"
        ' 
        ' lblServiceAssocie3
        ' 
        lblServiceAssocie3.AutoSize = True
        lblServiceAssocie3.Location = New Point(41, 287)
        lblServiceAssocie3.Name = "lblServiceAssocie3"
        lblServiceAssocie3.Size = New Size(122, 20)
        lblServiceAssocie3.TabIndex = 200
        lblServiceAssocie3.Text = "Service Associe 3"
        ' 
        ' lblServiceAssocie2
        ' 
        lblServiceAssocie2.AutoSize = True
        lblServiceAssocie2.Location = New Point(41, 254)
        lblServiceAssocie2.Name = "lblServiceAssocie2"
        lblServiceAssocie2.Size = New Size(122, 20)
        lblServiceAssocie2.TabIndex = 199
        lblServiceAssocie2.Text = "Service Associe 2"
        ' 
        ' lblServiceAssocie1
        ' 
        lblServiceAssocie1.AutoSize = True
        lblServiceAssocie1.Location = New Point(38, 221)
        lblServiceAssocie1.Name = "lblServiceAssocie1"
        lblServiceAssocie1.Size = New Size(122, 20)
        lblServiceAssocie1.TabIndex = 198
        lblServiceAssocie1.Text = "Service Associe 1"
        ' 
        ' lblNomElement
        ' 
        lblNomElement.AutoSize = True
        lblNomElement.Location = New Point(41, 188)
        lblNomElement.Name = "lblNomElement"
        lblNomElement.Size = New Size(100, 20)
        lblNomElement.TabIndex = 197
        lblNomElement.Text = "Nom Élément"
        ' 
        ' lblCodeElement
        ' 
        lblCodeElement.AutoSize = True
        lblCodeElement.Location = New Point(41, 155)
        lblCodeElement.Name = "lblCodeElement"
        lblCodeElement.Size = New Size(102, 20)
        lblCodeElement.TabIndex = 196
        lblCodeElement.Text = "Code Élément"
        ' 
        ' lblNomTable
        ' 
        lblNomTable.AutoSize = True
        lblNomTable.Location = New Point(41, 122)
        lblNomTable.Name = "lblNomTable"
        lblNomTable.Size = New Size(79, 20)
        lblNomTable.TabIndex = 195
        lblNomTable.Text = "Type Table"
        ' 
        ' Imprimer
        ' 
        Imprimer.Location = New Point(784, 38)
        Imprimer.Name = "Imprimer"
        Imprimer.Size = New Size(178, 61)
        Imprimer.TabIndex = 194
        Imprimer.Text = "Imprimer"
        Imprimer.UseVisualStyleBackColor = True
        ' 
        ' txtCodeExport
        ' 
        txtCodeExport.Location = New Point(199, 315)
        txtCodeExport.Name = "txtCodeExport"
        txtCodeExport.Size = New Size(418, 27)
        txtCodeExport.TabIndex = 189
        ' 
        ' txtServiceAssocie3
        ' 
        txtServiceAssocie3.Location = New Point(199, 283)
        txtServiceAssocie3.Name = "txtServiceAssocie3"
        txtServiceAssocie3.Size = New Size(418, 27)
        txtServiceAssocie3.TabIndex = 188
        ' 
        ' txtServiceAssocie2
        ' 
        txtServiceAssocie2.Location = New Point(199, 250)
        txtServiceAssocie2.Name = "txtServiceAssocie2"
        txtServiceAssocie2.Size = New Size(418, 27)
        txtServiceAssocie2.TabIndex = 187
        ' 
        ' txtServiceAssocie1
        ' 
        txtServiceAssocie1.Location = New Point(199, 217)
        txtServiceAssocie1.Name = "txtServiceAssocie1"
        txtServiceAssocie1.Size = New Size(418, 27)
        txtServiceAssocie1.TabIndex = 186
        ' 
        ' txtNomElement
        ' 
        txtNomElement.Location = New Point(199, 184)
        txtNomElement.Name = "txtNomElement"
        txtNomElement.Size = New Size(418, 27)
        txtNomElement.TabIndex = 185
        ' 
        ' txtCodeElement
        ' 
        txtCodeElement.Location = New Point(201, 153)
        txtCodeElement.Name = "txtCodeElement"
        txtCodeElement.Size = New Size(418, 27)
        txtCodeElement.TabIndex = 184
        ' 
        ' DataGridViewTables
        ' 
        DataGridViewTables.AllowUserToAddRows = False
        DataGridViewTables.Anchor = AnchorStyles.Top Or AnchorStyles.Bottom Or AnchorStyles.Left Or AnchorStyles.Right
        DataGridViewTables.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill
        DataGridViewTables.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize
        DataGridViewTables.Location = New Point(670, 122)
        DataGridViewTables.MultiSelect = False
        DataGridViewTables.Name = "DataGridViewTables"
        DataGridViewTables.ReadOnly = True
        DataGridViewTables.RowHeadersWidth = 51
        DataGridViewTables.SelectionMode = DataGridViewSelectionMode.FullRowSelect
        DataGridViewTables.Size = New Size(771, 337)
        DataGridViewTables.TabIndex = 183
        ' 
        ' btnEditeTable
        ' 
        btnEditeTable.Location = New Point(600, 38)
        btnEditeTable.Name = "btnEditeTable"
        btnEditeTable.Size = New Size(178, 61)
        btnEditeTable.TabIndex = 181
        btnEditeTable.Text = "EDITE"
        btnEditeTable.UseVisualStyleBackColor = True
        ' 
        ' btnRechercheTable
        ' 
        btnRechercheTable.Location = New Point(418, 38)
        btnRechercheTable.Name = "btnRechercheTable"
        btnRechercheTable.Size = New Size(178, 61)
        btnRechercheTable.TabIndex = 180
        btnRechercheTable.Text = "RECHERCHE"
        btnRechercheTable.UseVisualStyleBackColor = True
        ' 
        ' btnSupprimeTable
        ' 
        btnSupprimeTable.Location = New Point(229, 38)
        btnSupprimeTable.Name = "btnSupprimeTable"
        btnSupprimeTable.Size = New Size(178, 61)
        btnSupprimeTable.TabIndex = 179
        btnSupprimeTable.Text = "SUPRIME"
        btnSupprimeTable.UseVisualStyleBackColor = True
        ' 
        ' btnAjouteTable
        ' 
        btnAjouteTable.Location = New Point(41, 38)
        btnAjouteTable.Name = "btnAjouteTable"
        btnAjouteTable.Size = New Size(178, 61)
        btnAjouteTable.TabIndex = 178
        btnAjouteTable.Text = "AJOUTER"
        btnAjouteTable.UseVisualStyleBackColor = True
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
        ' cmbNomTable
        ' 
        cmbNomTable.FormattingEnabled = True
        cmbNomTable.Location = New Point(199, 119)
        cmbNomTable.Name = "cmbNomTable"
        cmbNomTable.Size = New Size(292, 28)
        cmbNomTable.TabIndex = 250
        ' 
        ' chkNormee
        ' 
        chkNormee.AutoSize = True
        chkNormee.Location = New Point(389, 352)
        chkNormee.Name = "chkNormee"
        chkNormee.Size = New Size(18, 17)
        chkNormee.TabIndex = 251
        chkNormee.UseVisualStyleBackColor = True
        ' 
        ' txtImpression
        ' 
        txtImpression.Location = New Point(199, 375)
        txtImpression.Name = "txtImpression"
        txtImpression.Size = New Size(418, 27)
        txtImpression.TabIndex = 252
        ' 
        ' lblImpression
        ' 
        lblImpression.AutoSize = True
        lblImpression.Location = New Point(41, 382)
        lblImpression.Name = "lblImpression"
        lblImpression.Size = New Size(81, 20)
        lblImpression.TabIndex = 253
        lblImpression.Text = "Impression"
        ' 
        ' Label21
        ' 
        Label21.AutoSize = True
        Label21.Font = New Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point, CByte(0))
        Label21.Location = New Point(12, 7)
        Label21.Name = "Label21"
        Label21.Size = New Size(65, 28)
        Label21.TabIndex = 254
        Label21.Text = "Tables"
        ' 
        ' Tables
        ' 
        AutoScaleDimensions = New SizeF(8F, 20F)
        AutoScaleMode = AutoScaleMode.Font
        ClientSize = New Size(1729, 996)
        Controls.Add(Label21)
        Controls.Add(lblImpression)
        Controls.Add(txtImpression)
        Controls.Add(chkNormee)
        Controls.Add(cmbNomTable)
        Controls.Add(txtRECHERCHE)
        Controls.Add(btnActualiserTable)
        Controls.Add(btnNouveauTable)
        Controls.Add(lblNormee)
        Controls.Add(lblCodeExport)
        Controls.Add(lblServiceAssocie3)
        Controls.Add(lblServiceAssocie2)
        Controls.Add(lblServiceAssocie1)
        Controls.Add(lblNomElement)
        Controls.Add(lblCodeElement)
        Controls.Add(lblNomTable)
        Controls.Add(Imprimer)
        Controls.Add(txtCodeExport)
        Controls.Add(txtServiceAssocie3)
        Controls.Add(txtServiceAssocie2)
        Controls.Add(txtServiceAssocie1)
        Controls.Add(txtNomElement)
        Controls.Add(txtCodeElement)
        Controls.Add(DataGridViewTables)
        Controls.Add(btnEditeTable)
        Controls.Add(btnRechercheTable)
        Controls.Add(btnSupprimeTable)
        Controls.Add(btnAjouteTable)
        Name = "Tables"
        Text = "Tables"
        CType(DataGridViewTables, ComponentModel.ISupportInitialize).EndInit()
        ResumeLayout(False)
        PerformLayout()
    End Sub

    Friend WithEvents Label7 As Label
    Friend WithEvents dtpDerniereModification As TextBox
    Friend WithEvents txtRECHERCHE As TextBox
    Friend WithEvents btnActualiserTable As Button
    Friend WithEvents btnNouveauTable As Button
    Friend WithEvents Label8 As Label
    Friend WithEvents Label9 As Label
    Friend WithEvents Label10 As Label
    Friend WithEvents lblNormee As Label
    Friend WithEvents lblCodeExport As Label
    Friend WithEvents lblServiceAssocie3 As Label
    Friend WithEvents lblServiceAssocie2 As Label
    Friend WithEvents lblServiceAssocie1 As Label
    Friend WithEvents lblNomElement As Label
    Friend WithEvents lblCodeElement As Label
    Friend WithEvents lblNomTable As Label
    Friend WithEvents Imprimer As Button
    Friend WithEvents txtTelephone As TextBox
    Friend WithEvents txtCodeRadio As TextBox
    Friend WithEvents txtCodeExport As TextBox
    Friend WithEvents txtServiceAssocie3 As TextBox
    Friend WithEvents txtServiceAssocie2 As TextBox
    Friend WithEvents txtServiceAssocie1 As TextBox
    Friend WithEvents txtNomElement As TextBox
    Friend WithEvents txtCodeElement As TextBox
    Friend WithEvents DataGridView1 As DataGridView
    Friend WithEvents txtCodeCamion As TextBox
    Friend WithEvents btnEditeTable As Button
    Friend WithEvents btnRechercheTable As Button
    Friend WithEvents btnSupprimeTable As Button
    Friend WithEvents btnAjouteTable As Button
    Friend WithEvents PrintPreviewDialog1 As PrintPreviewDialog
    Friend WithEvents PrintDocument1 As Printing.PrintDocument
    Friend WithEvents DataGridViewTables As DataGridView
    Friend WithEvents cmbNomTable As ComboBox
    Friend WithEvents chkNormee As CheckBox
    Friend WithEvents txtImpression As TextBox
    Friend WithEvents lblImpression As Label
    Friend WithEvents TextBox1 As TextBox
    Friend WithEvents TextBox2 As TextBox
    Friend WithEvents TextBox3 As TextBox
    Friend WithEvents Label21 As Label

End Class
