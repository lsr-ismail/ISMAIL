Imports Microsoft.Data.SqlClient
Imports System.Drawing
Imports System.Data

' ================================================================
'  Configuration_silos — VERSION 3 « TABLE UNIQUE »
'  ----------------------------------------------------------------
'  Tous les éléments (TR1-4, Silos ciment, ADJ1-4, EAU) vivent
'  dans dbo.Silos (TypeElement + NumeroElement).
'  À chaque sauvegarde, le formulaire synchronise aussi :
'    • Cases    (paramètres automate des trémies)
'    • Bascules (IdMateriau des adjuvants / eau)
'  → les patchs BL / envoi CPU / stocks fonctionnent sans change.
'  PRÉREQUIS : Migration_Silos_TableUnique.sql exécutée.
' ================================================================
Public Class Configuration_silos
    Inherits System.Windows.Forms.Form

#Region "CONTRÔLES"
    Private dgvElements As DataGridView
    Private pnlDetail As Panel
    Private lblTitreDetail As Label
    Private cmbMateriau As ComboBox
    Private lblMateriau As Label
    Private txtNomElement As TextBox
    Private nudNiveauMini As NumericUpDown
    Private nudCapacite As NumericUpDown
    Private cmbImageGIF As ComboBox
    Private pbPreview As PictureBox
    Private lblCouleurInfo As Label
    Private lblInfoMat As Label
    Private btnSauver As Button

    Private _siloID As Integer = -1
    Private _typeElement As String = ""
    Private _numeroElement As Integer = 0
    Private _enModification As Boolean = False

    Private Structure MateriauItem
        Public IdMateriau As Integer
        Public CodeMateriau As String
        Public NomMateriau As String
        Public TypeMateriau As String
        Public CodeMatierePremiere As String
        Public Overrides Function ToString() As String
            If IdMateriau = 0 Then Return "(vide)"
            Return CodeMateriau & " — " & NomMateriau
        End Function
    End Structure

    Private ReadOnly _couleursGIF As New Dictionary(Of String, String) From {
        {"silo_vert", "Vert"}, {"silo_orange", "Orange"}, {"silo_jaune", "Jaune"},
        {"silo_rouge", "Rouge"}, {"silo_bleu", "Bleu"}, {"silo_saphir", "Saphir"},
        {"silo_turquoise", "Turquoise"}, {"silo_violet", "Violet"},
        {"silo_rose", "Rose"}, {"silo_mauve", "Mauve"},
        {"silo_or", "Or"}, {"silo_platine", "Platine"}}
#End Region

#Region "INITIALISATION"
    Public Sub New()
        CreerInterface()
        ChargerElements()
    End Sub

    Private Sub CreerInterface()
        Me.Text = "Configuration — Tous les éléments (table unique)"
        Me.Size = New Size(1180, 730)
        Me.StartPosition = FormStartPosition.CenterScreen
        Me.BackColor = Color.FromArgb(235, 238, 245)
        Me.Font = New Font("Arial", 9)

        Dim pnlTitre As New Panel() With {
            .Dock = DockStyle.Top, .Height = 45,
            .BackColor = Color.FromArgb(45, 100, 165)}
        pnlTitre.Controls.Add(New Label() With {
            .Text = "⚙  CONFIGURATION — TRÉMIES / SILOS / ADJUVANTS / EAU",
            .Font = New Font("Arial", 12, FontStyle.Bold),
            .ForeColor = Color.White, .AutoSize = True, .Location = New Point(12, 10)})
        Me.Controls.Add(pnlTitre)

        ' ── Liste (gauche) ───────────────────────────────────────────
        Dim pnlListe As New Panel() With {
            .Bounds = New Rectangle(10, 55, 580, 585),
            .BackColor = Color.White, .BorderStyle = BorderStyle.FixedSingle}
        pnlListe.Controls.Add(New Label() With {
            .Text = "Éléments (dbo.Silos — table unique)",
            .Font = New Font("Arial", 9, FontStyle.Bold), .ForeColor = Color.White,
            .Dock = DockStyle.Top, .Height = 28,
            .TextAlign = ContentAlignment.MiddleLeft,
            .Padding = New Padding(5, 0, 0, 0),
            .BackColor = Color.FromArgb(45, 100, 165)})

        dgvElements = New DataGridView() With {
            .Dock = DockStyle.Fill, .ReadOnly = True,
            .SelectionMode = DataGridViewSelectionMode.FullRowSelect,
            .MultiSelect = False, .AllowUserToAddRows = False,
            .AllowUserToDeleteRows = False, .RowHeadersVisible = False,
            .BackgroundColor = Color.White, .BorderStyle = BorderStyle.None,
            .AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
            .EnableHeadersVisualStyles = False}
        dgvElements.RowTemplate.Height = 30
        dgvElements.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(45, 100, 165)
        dgvElements.ColumnHeadersDefaultCellStyle.ForeColor = Color.White
        dgvElements.ColumnHeadersDefaultCellStyle.Font = New Font("Arial", 8, FontStyle.Bold)
        dgvElements.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(245, 247, 252)
        AddHandler dgvElements.SelectionChanged, AddressOf DGV_SelectionChanged

        dgvElements.Columns.Add(New DataGridViewTextBoxColumn() With {.Name = "colSiloID", .Visible = False})
        dgvElements.Columns.Add(New DataGridViewTextBoxColumn() With {.Name = "colType", .Visible = False})
        dgvElements.Columns.Add(New DataGridViewTextBoxColumn() With {.Name = "colNum", .Visible = False})
        dgvElements.Columns.Add(New DataGridViewTextBoxColumn() With {.Name = "colRefID", .Visible = False})
        dgvElements.Columns.Add(New DataGridViewTextBoxColumn() With {.Name = "colGIF", .Visible = False})
        dgvElements.Columns.Add(New DataGridViewTextBoxColumn() With {.Name = "colCode", .HeaderText = "Code", .FillWeight = 50})
        dgvElements.Columns.Add(New DataGridViewTextBoxColumn() With {.Name = "colNom", .HeaderText = "Nom élément", .FillWeight = 110})
        dgvElements.Columns.Add(New DataGridViewTextBoxColumn() With {.Name = "colAffecte", .HeaderText = "Matériau affecté", .FillWeight = 130})
        dgvElements.Columns.Add(New DataGridViewTextBoxColumn() With {.Name = "colCapacite", .HeaderText = "Capacité (kg)", .FillWeight = 75})
        dgvElements.Columns.Add(New DataGridViewTextBoxColumn() With {.Name = "colMini", .HeaderText = "Mini (%)", .FillWeight = 50})

        pnlListe.Controls.Add(dgvElements)
        Me.Controls.Add(pnlListe)

        ' ── Détail (droite) ──────────────────────────────────────────
        pnlDetail = New Panel() With {
            .Bounds = New Rectangle(600, 55, 560, 585),
            .BackColor = Color.White, .BorderStyle = BorderStyle.FixedSingle}
        lblTitreDetail = New Label() With {
            .Text = "Sélectionnez un élément",
            .Font = New Font("Arial", 9, FontStyle.Bold),
            .ForeColor = Color.White, .Dock = DockStyle.Top, .Height = 28,
            .TextAlign = ContentAlignment.MiddleLeft,
            .Padding = New Padding(5, 0, 0, 0),
            .BackColor = Color.FromArgb(45, 100, 165)}
        pnlDetail.Controls.Add(lblTitreDetail)

        Dim fy As Integer = 45

        AjouterLabel("Nom :", pnlDetail, fy)
        txtNomElement = New TextBox() With {.Bounds = New Rectangle(180, fy, 355, 26)}
        pnlDetail.Controls.Add(txtNomElement) : fy += 40

        lblMateriau = New Label() With {
            .Text = "Matériau :", .Font = New Font("Arial", 9),
            .ForeColor = Color.FromArgb(50, 60, 80),
            .Bounds = New Rectangle(15, fy + 3, 163, 20)}
        pnlDetail.Controls.Add(lblMateriau)
        cmbMateriau = New ComboBox() With {
            .Bounds = New Rectangle(180, fy, 355, 26),
            .DropDownStyle = ComboBoxStyle.DropDownList}
        AddHandler cmbMateriau.SelectedIndexChanged, AddressOf CmbMateriau_Changed
        pnlDetail.Controls.Add(cmbMateriau) : fy += 28

        lblInfoMat = New Label() With {
            .Bounds = New Rectangle(180, fy, 355, 18),
            .Font = New Font("Arial", 8, FontStyle.Italic),
            .ForeColor = Color.FromArgb(80, 100, 160)}
        pnlDetail.Controls.Add(lblInfoMat) : fy += 28

        AjouterLabel("Couleur GIF :", pnlDetail, fy)
        cmbImageGIF = New ComboBox() With {
            .Bounds = New Rectangle(180, fy, 200, 26),
            .DropDownStyle = ComboBoxStyle.DropDownList}
        For Each kvp In _couleursGIF : cmbImageGIF.Items.Add(kvp.Key) : Next
        AddHandler cmbImageGIF.SelectedIndexChanged, AddressOf CmbImageGIF_Changed
        pnlDetail.Controls.Add(cmbImageGIF)
        lblCouleurInfo = New Label() With {
            .Bounds = New Rectangle(390, fy + 4, 150, 20),
            .Font = New Font("Arial", 8, FontStyle.Italic),
            .ForeColor = Color.FromArgb(80, 80, 120)}
        pnlDetail.Controls.Add(lblCouleurInfo) : fy += 40

        AjouterLabel("Capacité max (kg) :", pnlDetail, fy)
        nudCapacite = New NumericUpDown() With {
            .Bounds = New Rectangle(180, fy, 160, 26),
            .Maximum = 999999, .DecimalPlaces = 0}
        pnlDetail.Controls.Add(nudCapacite) : fy += 40

        AjouterLabel("Niveau mini (%) :", pnlDetail, fy)
        nudNiveauMini = New NumericUpDown() With {
            .Bounds = New Rectangle(180, fy, 160, 26),
            .Maximum = 99, .Minimum = 0}
        pnlDetail.Controls.Add(nudNiveauMini) : fy += 50

        pnlDetail.Controls.Add(New Label() With {
            .Text = "Aperçu :", .Font = New Font("Arial", 9, FontStyle.Bold),
            .ForeColor = Color.FromArgb(45, 100, 165),
            .Bounds = New Rectangle(15, fy, 160, 20)})
        pbPreview = New PictureBox() With {
            .Bounds = New Rectangle(180, fy, 90, 170),
            .SizeMode = PictureBoxSizeMode.StretchImage,
            .BorderStyle = BorderStyle.FixedSingle,
            .BackColor = Color.FromArgb(220, 225, 235)}
        pnlDetail.Controls.Add(pbPreview)
        Me.Controls.Add(pnlDetail)

        btnSauver = New Button() With {
            .Text = "💾 Sauvegarder",
            .Bounds = New Rectangle(600, 648, 180, 34),
            .BackColor = Color.FromArgb(45, 100, 165),
            .ForeColor = Color.White, .FlatStyle = FlatStyle.Flat,
            .Font = New Font("Arial", 10, FontStyle.Bold)}
        AddHandler btnSauver.Click, AddressOf BtnSauver_Click
        Me.Controls.Add(btnSauver)

        Dim btnFermer As New Button() With {
            .Text = "Fermer",
            .Bounds = New Rectangle(1040, 648, 120, 34),
            .BackColor = Color.FromArgb(120, 120, 120),
            .ForeColor = Color.White, .FlatStyle = FlatStyle.Flat}
        AddHandler btnFermer.Click, Sub() Me.Close()
        Me.Controls.Add(btnFermer)
    End Sub

    Private Sub AjouterLabel(texte As String, parent As Panel, y As Integer)
        parent.Controls.Add(New Label() With {
            .Text = texte, .Font = New Font("Arial", 9),
            .ForeColor = Color.FromArgb(50, 60, 80),
            .Bounds = New Rectangle(15, y + 3, 163, 20)})
    End Sub
#End Region

#Region "CHARGEMENT — UNE SEULE REQUÊTE"
    Private Sub ChargerElements()
        dgvElements.Rows.Clear()
        Try
            Dim dt As DataTable = ModuleDb.Lire(
                "SELECT s.SiloID, s.TypeElement, s.NumeroElement, s.NomSilo, " &
                "       s.CapaciteMax_kg, s.NiveauMini, s.IdMateriau, " &
                "       ISNULL(s.CouleurGIF,'silo_saphir') AS CouleurGIF, " &
                "       ISNULL(m.CodeMateriau + ' — ' + m.NomMateriau, '(non affecté)') AS Affectation " &
                "FROM dbo.Silos s " &
                "LEFT JOIN dbo.Materiaux m ON s.IdMateriau = m.IdMateriau " &
                "WHERE s.EstActif = 1 " &
                "ORDER BY CASE s.TypeElement WHEN 'TR' THEN 1 WHEN 'S' THEN 2 " &
                "         WHEN 'ADJ' THEN 3 ELSE 4 END, s.NumeroElement")

            For Each row As DataRow In dt.Rows
                Dim typ As String = row("TypeElement").ToString()
                Dim num As Integer = CInt(row("NumeroElement"))
                Dim code As String = If(typ = "EAU", "EAU", typ & num)
                Dim idMat As Integer = If(IsDBNull(row("IdMateriau")), 0, CInt(row("IdMateriau")))
                Dim cap As String = If(IsDBNull(row("CapaciteMax_kg")), "0",
                                       CDec(row("CapaciteMax_kg")).ToString("N0"))
                Dim mini As String = If(IsDBNull(row("NiveauMini")), "0",
                                        CDec(row("NiveauMini")).ToString("0"))

                Dim i = dgvElements.Rows.Add(
                    CInt(row("SiloID")), typ, num, idMat,
                    row("CouleurGIF").ToString(), code,
                    row("NomSilo").ToString(), row("Affectation").ToString(),
                    cap, mini)

                ' Couleur de ligne par type
                Select Case typ
                    Case "TR"
                        dgvElements.Rows(i).DefaultCellStyle.BackColor = Color.FromArgb(225, 235, 255)
                        dgvElements.Rows(i).DefaultCellStyle.ForeColor = Color.FromArgb(20, 40, 120)
                    Case "S"
                        dgvElements.Rows(i).DefaultCellStyle.BackColor = Color.FromArgb(255, 240, 210)
                        dgvElements.Rows(i).DefaultCellStyle.ForeColor = Color.FromArgb(120, 60, 0)
                    Case "ADJ"
                        dgvElements.Rows(i).DefaultCellStyle.BackColor = Color.FromArgb(240, 225, 255)
                        dgvElements.Rows(i).DefaultCellStyle.ForeColor = Color.FromArgb(80, 0, 120)
                    Case "EAU"
                        dgvElements.Rows(i).DefaultCellStyle.BackColor = Color.FromArgb(210, 240, 255)
                        dgvElements.Rows(i).DefaultCellStyle.ForeColor = Color.FromArgb(0, 60, 130)
                End Select
            Next

        Catch ex As Exception
            MessageBox.Show("Erreur chargement : " & ex.Message,
                "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    Private Sub ChargerMateriaux(filtre As String)
        cmbMateriau.Items.Clear()
        cmbMateriau.Items.Add(New MateriauItem() With {
            .IdMateriau = 0, .CodeMateriau = "(vide)", .NomMateriau = "",
            .TypeMateriau = "", .CodeMatierePremiere = ""})
        Try
            Dim dt As DataTable = ModuleDb.Lire(
                "SELECT IdMateriau, CodeMateriau, NomMateriau, TypeMateriau, " &
                "ISNULL(CodeMatierePremiere,'') AS CodeMatierePremiere " &
                "FROM dbo.Materiaux WHERE TypeMateriau = @t " &
                "ORDER BY CodeMateriau",
                Sub(cmd) cmd.Parameters.Add("@t", SqlDbType.NVarChar, 50).Value = filtre)
            For Each row As DataRow In dt.Rows
                cmbMateriau.Items.Add(New MateriauItem() With {
                    .IdMateriau = CInt(row("IdMateriau")),
                    .CodeMateriau = row("CodeMateriau").ToString(),
                    .NomMateriau = row("NomMateriau").ToString(),
                    .TypeMateriau = row("TypeMateriau").ToString(),
                    .CodeMatierePremiere = row("CodeMatierePremiere").ToString()})
            Next
        Catch ex As Exception
            MessageBox.Show("Erreur chargement matériaux : " & ex.Message)
        End Try
    End Sub

    Private Function FiltreParType(typ As String) As String
        Select Case typ
            Case "TR" : Return "GRANULAT"
            Case "S" : Return "LIANT"
            Case "ADJ" : Return "ADJUVANT"
            Case "EAU" : Return "EAU"
            Case Else : Return "GRANULAT"
        End Select
    End Function
#End Region

#Region "SÉLECTION"
    Private Sub DGV_SelectionChanged(sender As Object, e As EventArgs)
        If dgvElements.SelectedRows.Count = 0 Then Return
        Dim row As DataGridViewRow = dgvElements.SelectedRows(0)

        _siloID = CInt(row.Cells("colSiloID").Value)
        _typeElement = row.Cells("colType").Value.ToString()
        _numeroElement = CInt(row.Cells("colNum").Value)
        Dim refID As Integer = CInt(row.Cells("colRefID").Value)

        lblTitreDetail.Text = "Élément : " & row.Cells("colCode").Value.ToString() &
                              " — " & row.Cells("colNom").Value.ToString()
        _enModification = True

        txtNomElement.Text = row.Cells("colNom").Value.ToString()

        Dim capVal As Decimal = 0 : Dim miniVal As Decimal = 0
        Decimal.TryParse(row.Cells("colCapacite").Value.ToString().Replace(" ", "").Replace(",", ""), capVal)
        Decimal.TryParse(row.Cells("colMini").Value.ToString(), miniVal)
        nudCapacite.Value = Math.Min(capVal, nudCapacite.Maximum)
        nudNiveauMini.Value = Math.Min(miniVal, nudNiveauMini.Maximum)
        nudNiveauMini.Enabled = (_typeElement = "TR" OrElse _typeElement = "S")

        ChargerMateriaux(FiltreParType(_typeElement))
        SelectionnerMateriau(refID)

        Dim gif As String = row.Cells("colGIF").Value.ToString()
        Dim idx As Integer = cmbImageGIF.Items.IndexOf(gif)
        cmbImageGIF.SelectedIndex = If(idx >= 0, idx, 0)

        _enModification = False
        MettreAJourPreview()
        MettreAJourInfoMat()
    End Sub

    Private Sub SelectionnerMateriau(idMat As Integer)
        For Each item As Object In cmbMateriau.Items
            If CType(item, MateriauItem).IdMateriau = idMat Then
                cmbMateriau.SelectedItem = item : Return
            End If
        Next
        If cmbMateriau.Items.Count > 0 Then cmbMateriau.SelectedIndex = 0
    End Sub
#End Region

#Region "ÉVÉNEMENTS"
    Private Sub CmbMateriau_Changed(sender As Object, e As EventArgs)
        If _enModification Then Return
        MettreAJourInfoMat() : MettreAJourPreview()
    End Sub

    Private Sub CmbImageGIF_Changed(sender As Object, e As EventArgs)
        If _enModification Then Return
        MettreAJourPreview()
    End Sub

    Private Sub MettreAJourInfoMat()
        If cmbMateriau.SelectedItem Is Nothing Then lblInfoMat.Text = "" : Return
        Dim m = CType(cmbMateriau.SelectedItem, MateriauItem)
        If m.IdMateriau = 0 Then lblInfoMat.Text = "" : Return
        Dim s As String = "Type : " & m.TypeMateriau
        If m.CodeMatierePremiere <> "" Then s &= "   Code automate : " & m.CodeMatierePremiere
        lblInfoMat.Text = s
    End Sub

    Private Sub MettreAJourPreview()
        If cmbImageGIF.SelectedItem Is Nothing Then Return
        Dim gifNom As String = cmbImageGIF.SelectedItem.ToString()
        If _couleursGIF.ContainsKey(gifNom) Then lblCouleurInfo.Text = "→ " & _couleursGIF(gifNom)
        Try
            Dim ch As String = IO.Path.Combine(Application.StartupPath, "images", gifNom & ".gif")
            If IO.File.Exists(ch) Then
                pbPreview.Image = Image.FromFile(ch)
            Else
                pbPreview.Image = Nothing
                pbPreview.BackColor = CouleurDepuisNom(gifNom)
            End If
        Catch
            pbPreview.BackColor = Color.LightGray
        End Try
    End Sub
#End Region

#Region "SAUVEGARDE — Silos maître + synchronisation"
    Private Sub BtnSauver_Click(sender As Object, e As EventArgs)
        If _siloID <= 0 Then
            MessageBox.Show("Sélectionnez d'abord un élément.") : Return
        End If
        If String.IsNullOrWhiteSpace(txtNomElement.Text) Then
            MessageBox.Show("Nom obligatoire.", "Validation",
                MessageBoxButtons.OK, MessageBoxIcon.Warning) : Return
        End If

        Dim mat As New MateriauItem()
        If cmbMateriau.SelectedItem IsNot Nothing Then
            mat = CType(cmbMateriau.SelectedItem, MateriauItem)
        End If
        Dim gif As String = If(cmbImageGIF.SelectedItem IsNot Nothing,
            cmbImageGIF.SelectedItem.ToString(), "silo_saphir")

        Try
            ' ═══ 1. TABLE MAÎTRE : dbo.Silos ═════════════════════
            ModuleDb.Executer(
                "UPDATE dbo.Silos SET " &
                "  NomSilo = @nom, IdMateriau = @idmat, " &
                "  CapaciteMax_kg = @capa, NiveauMini = @mini, " &
                "  CouleurGIF = @gif, DateModif = GETDATE() " &
                "WHERE SiloID = @id",
                Sub(cmd)
                    cmd.Parameters.Add("@nom", SqlDbType.NVarChar, 100).Value = txtNomElement.Text.Trim()
                    cmd.Parameters.Add("@idmat", SqlDbType.Int).Value = If(mat.IdMateriau = 0, DBNull.Value, CObj(mat.IdMateriau))
                    cmd.Parameters.Add("@capa", SqlDbType.Decimal).Value = nudCapacite.Value
                    cmd.Parameters.Add("@mini", SqlDbType.Decimal).Value = nudNiveauMini.Value
                    cmd.Parameters.Add("@gif", SqlDbType.NVarChar, 30).Value = gif
                    cmd.Parameters.Add("@id", SqlDbType.Int).Value = _siloID
                End Sub)

            ' ═══ 2. SYNCHRONISATION vers les tables automate ═════
            Select Case _typeElement
                Case "TR"
                    ' Cases : paramètres lus par le PLC + patchs BL/stocks
                    Dim codeMatAuto As Byte = 0
                    Byte.TryParse(mat.CodeMatierePremiere, codeMatAuto)
                    ModuleDb.Executer(
                        "UPDATE dbo.Cases SET " &
                        "  CodeCase = @nom, CapaciteCase = @cap, " &
                        "  SeuilAlerte = @seuil, IdMateriau = @idmat, " &
                        "  CodeMateriau = @codemat, TypeMateriau = 0 " &
                        "WHERE NumeroCase = @num",
                        Sub(cmd)
                            cmd.Parameters.Add("@nom", SqlDbType.NVarChar, 20).Value = txtNomElement.Text.Trim()
                            cmd.Parameters.Add("@cap", SqlDbType.Decimal).Value = nudCapacite.Value
                            cmd.Parameters.Add("@seuil", SqlDbType.Decimal).Value = nudNiveauMini.Value
                            cmd.Parameters.Add("@idmat", SqlDbType.Int).Value = If(mat.IdMateriau = 0, DBNull.Value, CObj(mat.IdMateriau))
                            cmd.Parameters.Add("@codemat", SqlDbType.TinyInt).Value = codeMatAuto
                            cmd.Parameters.Add("@num", SqlDbType.SmallInt).Value = CShort(_numeroElement)
                        End Sub)

                Case "ADJ", "EAU"
                    ' Bascules : nom, capacité, matériau (via BasculeID du silo)
                    ModuleDb.Executer(
                        "UPDATE b SET " &
                        "  b.NomBascule = @nom, b.CapaciteMax_kg = @capa, " &
                        "  b.IdMateriau = @idmat, b.DateModif = GETDATE() " &
                        "FROM dbo.Bascules b " &
                        "JOIN dbo.Silos s ON s.BasculeID = b.BasculeID " &
                        "WHERE s.SiloID = @id",
                        Sub(cmd)
                            cmd.Parameters.Add("@nom", SqlDbType.NVarChar, 100).Value = txtNomElement.Text.Trim()
                            cmd.Parameters.Add("@capa", SqlDbType.Decimal).Value = nudCapacite.Value
                            cmd.Parameters.Add("@idmat", SqlDbType.Int).Value = If(mat.IdMateriau = 0, DBNull.Value, CObj(mat.IdMateriau))
                            cmd.Parameters.Add("@id", SqlDbType.Int).Value = _siloID
                        End Sub)
            End Select

            ' ═══ 3. Rafraîchir le Synoptique ═════════════════════
            MettreAJourLabelsSynoptique()
            ChargerElements()
            MessageBox.Show("✅ Élément mis à jour (table unique + synchro automate)",
                "Sauvegarde", MessageBoxButtons.OK, MessageBoxIcon.Information)

        Catch ex As Exception
            MessageBox.Show("Erreur sauvegarde : " & ex.Message,
                "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub
#End Region

#Region "SYNOPTIQUE / UTILITAIRES"
    Private Sub MettreAJourLabelsSynoptique()
        Try
            For Each frm As Form In Application.OpenForms
                If TypeOf frm Is Synoptique Then
                    CType(frm, Synoptique).RechercherConfigEtMettreAJourLabels()
                    Exit For
                End If
            Next
        Catch ex As Exception
            Debug.WriteLine("MettreAJourLabelsSynoptique : " & ex.Message)
        End Try
    End Sub

    Private Function CouleurDepuisNom(nom As String) As Color
        Select Case nom
            Case "silo_vert" : Return Color.FromArgb(0, 150, 60)
            Case "silo_orange" : Return Color.FromArgb(210, 110, 0)
            Case "silo_jaune" : Return Color.FromArgb(180, 160, 0)
            Case "silo_rouge" : Return Color.FromArgb(180, 30, 30)
            Case "silo_bleu" : Return Color.FromArgb(30, 100, 190)
            Case "silo_saphir" : Return Color.FromArgb(20, 60, 160)
            Case "silo_turquoise" : Return Color.FromArgb(0, 150, 150)
            Case "silo_violet" : Return Color.FromArgb(120, 30, 160)
            Case "silo_rose" : Return Color.FromArgb(200, 60, 120)
            Case "silo_mauve" : Return Color.FromArgb(160, 80, 180)
            Case "silo_or" : Return Color.FromArgb(180, 140, 0)
            Case Else : Return Color.FromArgb(130, 140, 160)
        End Select
    End Function

    Private Sub Configuration_silos_Load(sender As Object, e As EventArgs) Handles MyBase.Load
    End Sub

    Private Sub TabPage4_Click(sender As Object, e As EventArgs) Handles TabPage4.Click

    End Sub

    Private Sub Label58_Click(sender As Object, e As EventArgs) Handles Label58.Click

    End Sub
#End Region

End Class