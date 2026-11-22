Imports System.Windows.Forms
Imports Microsoft.Data.SqlClient
Imports System.Data

Public Class Frm_Config_Automate
    Inherits Form

    Private _numeroCas As Integer
    Private _typeEquip As String

    Private tabs As TabControl
    Private tabCases As TabPage
    Private tabBascules As TabPage
    Private tabSilos As TabPage
    Private tabPLC As TabPage
    Private tabGeneral As TabPage

    Private dgvCases As DataGridView
    Private dtCases As New DataTable()
    Private txtCNom As TextBox
    Private cmbCType As ComboBox
    Private txtCCapacite As TextBox
    Private txtCNiveau As TextBox
    Private txtCHygro As TextBox
    Private txtCSeuil As TextBox
    Private txtCTempsDosage As TextBox
    Private txtCSeuilPV As TextBox
    Private txtCSeuilVibreur As TextBox
    Private chkCDosageValide As CheckBox
    Private chkCActif As CheckBox
    Private btnCSauvegarder As Button
    Private btnCNouveau As Button
    Private btnCSupprimer As Button
    Private lblCStatut As Label
    Private _caseID_selectionne As Integer = 0

    Private dgvBascules As DataGridView
    Private dgvSilos As DataGridView

    ' ═══════════════════════════════════════════════════════════════════
    '  CONSTRUCTEUR
    ' ═══════════════════════════════════════════════════════════════════

    Public Sub New(numeroCas As Integer, typeEquip As String)
        _numeroCas = numeroCas
        _typeEquip = typeEquip.ToUpper()
        InitialiserFormulaire()
    End Sub

    ' ═══════════════════════════════════════════════════════════════════
    '  INITIALISATION UI
    ' ═══════════════════════════════════════════════════════════════════

    Private Sub InitialiserFormulaire()
        Me.Text = "Configuration Automate — Centrale à Béton"
        Me.Size = New Size(900, 640)
        Me.StartPosition = FormStartPosition.CenterParent
        Me.FormBorderStyle = FormBorderStyle.Sizable
        Me.MinimumSize = New Size(820, 560)
        Me.BackColor = Color.FromArgb(240, 242, 245)

        tabs = New TabControl() With {.Dock = DockStyle.Fill, .Font = New Font("Arial", 9, FontStyle.Bold)}
        Me.Controls.Add(tabs)

        tabCases = New TabPage("🏗  CASES / TRÉMIES")
        tabBascules = New TabPage("⚖  BASCULES")
        tabSilos = New TabPage("🏭  SILOS CIMENT")
        tabPLC = New TabPage("📡  PLC / AUTOMATE")
        tabGeneral = New TabPage("⚙  GÉNÉRAL")

        tabs.TabPages.AddRange({tabCases, tabBascules, tabSilos, tabPLC, tabGeneral})

        BuildTabCases()
        BuildTabBascules()
        BuildTabSilos()
        BuildTabPLC()
        BuildTabGeneral()

        tabs.SelectedTab = If(_typeEquip = "SILO", tabSilos, tabCases)
        AddHandler tabs.SelectedIndexChanged, AddressOf Tabs_Changed
    End Sub

    Private Sub Frm_Config_Automate_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        If _typeEquip = "SILO" Then ChargerSilos() Else ChargerCases()
    End Sub

    ' ═══════════════════════════════════════════════════════════════════
    '  BUILD ONGLETS
    ' ═══════════════════════════════════════════════════════════════════

    Private Sub BuildTabCases()
        tabCases.BackColor = Color.FromArgb(248, 250, 252)
        Dim split As New SplitContainer() With {
            .Dock = DockStyle.Fill, .Panel1MinSize = 250,
            .Panel2MinSize = 100, .SplitterDistance = 370}
        tabCases.Controls.Add(split)

        split.Panel1.Controls.Add(New Label() With {
            .Text = "Liste des cases", .Dock = DockStyle.Top, .Height = 24,
            .Font = New Font("Arial", 9, FontStyle.Bold),
            .BackColor = Color.FromArgb(60, 130, 90), .ForeColor = Color.White,
            .TextAlign = ContentAlignment.MiddleCenter})

        dgvCases = New DataGridView() With {
            .Dock = DockStyle.Fill, .ReadOnly = True, .AllowUserToAddRows = False,
            .AllowUserToDeleteRows = False, .SelectionMode = DataGridViewSelectionMode.FullRowSelect,
            .MultiSelect = False, .RowHeadersVisible = False,
            .AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
            .BackgroundColor = Color.White, .BorderStyle = BorderStyle.None,
            .Font = New Font("Arial", 8)}
        dgvCases.ColumnHeadersDefaultCellStyle.Font = New Font("Arial", 8, FontStyle.Bold)
        dgvCases.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(220, 235, 220)
        dgvCases.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(245, 250, 245)
        split.Panel1.Controls.Add(dgvCases)

        Dim pnlFiche As New Panel() With {.Dock = DockStyle.Fill, .Padding = New Padding(10)}
        split.Panel2.Controls.Add(pnlFiche)
        pnlFiche.Controls.Add(New Label() With {
            .Text = "Détail de la case", .Dock = DockStyle.Top, .Height = 24,
            .Font = New Font("Arial", 9, FontStyle.Bold),
            .BackColor = Color.FromArgb(60, 130, 90), .ForeColor = Color.White,
            .TextAlign = ContentAlignment.MiddleCenter})

        Dim pnlChamps As New Panel() With {.Location = New Point(8, 32), .Size = New Size(480, 460)}
        pnlFiche.Controls.Add(pnlChamps)

        Dim yF As Integer = 0
        AjouterLabelFiche(pnlChamps, "Nom de la case :", 0, yF)
        txtCNom = New TextBox() With {.Location = New Point(150, yF), .Size = New Size(280, 22)}
        pnlChamps.Controls.Add(txtCNom) : yF += 30

        AjouterLabelFiche(pnlChamps, "Type matériau :", 0, yF)
        cmbCType = New ComboBox() With {.Location = New Point(150, yF), .Size = New Size(160, 22), .DropDownStyle = ComboBoxStyle.DropDownList}
        cmbCType.Items.AddRange({"GRANULAT", "SABLE", "CIMENT", "AUTRE"}) : cmbCType.SelectedIndex = 0
        pnlChamps.Controls.Add(cmbCType) : yF += 30

        For Each t In {
            ("Capacité maxi (kg) :", "80000", 130),
            ("Niveau actuel (kg) :", "0", 130),
            ("Hygrométrie (%) :", "0", 100),
            ("Seuil alerte (kg) :", "0", 130),
            ("Temps dosage max (s) :", "90", 100),
            ("Seuil petite vitesse (kg) :", "400", 130),
            ("Seuil débit vibreur (kg/s) :", "59", 130)}
            AjouterLabelFiche(pnlChamps, t.Item1, 0, yF)
            Dim txt As New TextBox() With {.Location = New Point(150, yF), .Size = New Size(t.Item3, 22), .Text = t.Item2}
            pnlChamps.Controls.Add(txt)
            Select Case t.Item1
                Case "Capacité maxi (kg) :" : txtCCapacite = txt
                Case "Niveau actuel (kg) :" : txtCNiveau = txt
                Case "Hygrométrie (%) :" : txtCHygro = txt
                Case "Seuil alerte (kg) :" : txtCSeuil = txt
                Case "Temps dosage max (s) :" : txtCTempsDosage = txt
                Case "Seuil petite vitesse (kg) :" : txtCSeuilPV = txt
                Case "Seuil débit vibreur (kg/s) :" : txtCSeuilVibreur = txt
            End Select
            yF += 30
        Next

        chkCDosageValide = New CheckBox() With {.Text = "Dosage validé", .Location = New Point(0, yF), .Size = New Size(150, 22)}
        pnlChamps.Controls.Add(chkCDosageValide)
        chkCActif = New CheckBox() With {.Text = "Case active", .Location = New Point(160, yF), .Size = New Size(130, 22), .Checked = True}
        pnlChamps.Controls.Add(chkCActif) : yF += 36

        btnCNouveau = New Button() With {.Text = "➕ Nouveau", .Location = New Point(0, yF), .Size = New Size(110, 30), .BackColor = Color.FromArgb(200, 230, 200)}
        pnlChamps.Controls.Add(btnCNouveau)
        btnCSauvegarder = New Button() With {.Text = "💾 Sauvegarder", .Location = New Point(120, yF), .Size = New Size(130, 30), .BackColor = Color.FromArgb(100, 160, 220), .ForeColor = Color.White, .Font = New Font("Arial", 9, FontStyle.Bold)}
        pnlChamps.Controls.Add(btnCSauvegarder)
        btnCSupprimer = New Button() With {.Text = "🗑 Supprimer", .Location = New Point(260, yF), .Size = New Size(110, 30), .BackColor = Color.FromArgb(240, 180, 180)}
        pnlChamps.Controls.Add(btnCSupprimer) : yF += 40

        lblCStatut = New Label() With {.Location = New Point(0, yF), .Size = New Size(430, 20), .Font = New Font("Arial", 8, FontStyle.Italic)}
        pnlChamps.Controls.Add(lblCStatut)

        AddHandler dgvCases.SelectionChanged, AddressOf DgvCases_SelectionChanged
        AddHandler btnCNouveau.Click, AddressOf BtnCNouveau_Click
        AddHandler btnCSauvegarder.Click, AddressOf BtnCSauvegarder_Click
        AddHandler btnCSupprimer.Click, AddressOf BtnCSupprimer_Click
    End Sub

    Private Sub BuildTabBascules()
        tabBascules.BackColor = Color.FromArgb(248, 250, 252)
        tabBascules.Controls.Add(New Label() With {
            .Text = "Liste des bascules", .Dock = DockStyle.Top, .Height = 24,
            .Font = New Font("Arial", 9, FontStyle.Bold),
            .BackColor = Color.FromArgb(60, 100, 160), .ForeColor = Color.White,
            .TextAlign = ContentAlignment.MiddleCenter})
        dgvBascules = New DataGridView() With {
            .Dock = DockStyle.Fill, .ReadOnly = True, .AllowUserToAddRows = False,
            .RowHeadersVisible = False, .AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
            .BackgroundColor = Color.White, .BorderStyle = BorderStyle.None,
            .Font = New Font("Arial", 8), .SelectionMode = DataGridViewSelectionMode.FullRowSelect}
        dgvBascules.ColumnHeadersDefaultCellStyle.Font = New Font("Arial", 8, FontStyle.Bold)
        dgvBascules.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(210, 220, 240)
        dgvBascules.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(240, 245, 255)
        tabBascules.Controls.Add(dgvBascules)
    End Sub

    Private Sub BuildTabSilos()
        tabSilos.BackColor = Color.FromArgb(248, 250, 252)
        tabSilos.Controls.Add(New Label() With {
            .Text = "Liste des silos ciment", .Dock = DockStyle.Top, .Height = 24,
            .Font = New Font("Arial", 9, FontStyle.Bold),
            .BackColor = Color.FromArgb(100, 80, 160), .ForeColor = Color.White,
            .TextAlign = ContentAlignment.MiddleCenter})
        dgvSilos = New DataGridView() With {
            .Dock = DockStyle.Fill, .ReadOnly = True, .AllowUserToAddRows = False,
            .RowHeadersVisible = False, .AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
            .BackgroundColor = Color.White, .BorderStyle = BorderStyle.None,
            .Font = New Font("Arial", 8), .SelectionMode = DataGridViewSelectionMode.FullRowSelect}
        dgvSilos.ColumnHeadersDefaultCellStyle.Font = New Font("Arial", 8, FontStyle.Bold)
        dgvSilos.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(225, 215, 245)
        dgvSilos.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(245, 240, 255)
        tabSilos.Controls.Add(dgvSilos)
    End Sub

    Private Sub BuildTabPLC()
        tabPLC.BackColor = Color.FromArgb(248, 250, 252)
        tabPLC.Controls.Add(New Label() With {
            .Text = "📡 Paramètres PLC / Automate Siemens S7" & vbCrLf & vbCrLf &
                    "Cette section sera disponible après la connexion PLC.",
            .Font = New Font("Arial", 10), .ForeColor = Color.Gray,
            .TextAlign = ContentAlignment.MiddleCenter, .Dock = DockStyle.Fill})
    End Sub

    Private Sub BuildTabGeneral()
        tabGeneral.BackColor = Color.FromArgb(248, 250, 252)
        tabGeneral.Controls.Add(New Label() With {
            .Text = "⚙ Paramètres généraux" & vbCrLf & vbCrLf &
                    "Délais, modes de dosage, paramètres malaxeur, etc." & vbCrLf & "(À compléter)",
            .Font = New Font("Arial", 10), .ForeColor = Color.Gray,
            .TextAlign = ContentAlignment.MiddleCenter, .Dock = DockStyle.Fill})
    End Sub

    ' ═══════════════════════════════════════════════════════════════════
    '  CHANGEMENT ONGLET
    ' ═══════════════════════════════════════════════════════════════════

    Private Sub Tabs_Changed(sender As Object, e As EventArgs)
        Select Case tabs.SelectedTab.Text
            Case tabCases.Text : If dtCases.Rows.Count = 0 Then ChargerCases()
            Case tabBascules.Text : If dgvBascules.Rows.Count = 0 Then ChargerBascules()
            Case tabSilos.Text : If dgvSilos.Rows.Count = 0 Then ChargerSilos()
        End Select
    End Sub

    ' ═══════════════════════════════════════════════════════════════════
    '  CHARGEMENT DONNÉES
    ' ═══════════════════════════════════════════════════════════════════

    Private Sub ChargerCases()
        Try
            Dim dt As DataTable = ModuleDb.Lire(
                "SELECT CaseID, NumeroCase, TypeMateriau, NomCase, " &
                "CapaciteKg, NiveauKg, Hygrometrie, SeuilAlerteKg, TempsDosageMaxS, " &
                "SeuilPetiteVitesseKg, SeuilDebitVibreurKgS, DosageValide, Actif, DateModif " &
                "FROM dbo.Cases ORDER BY NumeroCase, TypeMateriau")

            dtCases.Clear()
            For Each row As DataRow In dt.Rows
                dtCases.ImportRow(row)
            Next

            Dim dtVue As New DataTable()
            dtVue.Columns.Add("N°", GetType(Integer))
            dtVue.Columns.Add("Type", GetType(String))
            dtVue.Columns.Add("Nom", GetType(String))
            dtVue.Columns.Add("Capacité kg", GetType(Decimal))
            dtVue.Columns.Add("Niveau kg", GetType(Decimal))
            dtVue.Columns.Add("Hygro %", GetType(Decimal))
            dtVue.Columns.Add("Validé", GetType(Boolean))
            dtVue.Columns.Add("Actif", GetType(Boolean))

            For Each row As DataRow In dtCases.Rows
                dtVue.Rows.Add(row("NumeroCase"), row("TypeMateriau"), row("NomCase"),
                               row("CapaciteKg"), row("NiveauKg"), row("Hygrometrie"),
                               row("DosageValide"), row("Actif"))
            Next

            dgvCases.DataSource = dtVue

            Dim typeCible As String = If(_typeEquip = "SILO", "CIMENT", "GRANULAT")
            For i As Integer = 0 To dgvCases.Rows.Count - 1
                If Convert.ToInt32(dgvCases.Rows(i).Cells(0).Value) = _numeroCas AndAlso
                   dgvCases.Rows(i).Cells(1).Value.ToString().ToUpper() = typeCible Then
                    dgvCases.Rows(i).Selected = True
                    dgvCases.FirstDisplayedScrollingRowIndex = i
                    Exit For
                End If
            Next

        Catch ex As Exception
            MessageBox.Show("Erreur chargement Cases : " & ex.Message, "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    Private Sub ChargerBascules()
        Try
            Dim dt As DataTable = ModuleDb.Lire(
                "SELECT b.BasculeID, b.NomBascule, b.CapaciteMax_kg, b.DB_PLC, " &
                "b.Offset_Pose, b.Offset_Consigne, b.Ordre, b.Actif, b.DateModif " &
                "FROM dbo.Bascules b ORDER BY b.Ordre, b.BasculeID")
            dgvBascules.DataSource = dt
        Catch ex As Exception
            MessageBox.Show("Erreur chargement Bascules : " & ex.Message, "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    Private Sub ChargerSilos()
        Try
            Dim dt As DataTable = ModuleDb.Lire(
                "SELECT s.SiloID, s.NomSilo, s.CapaciteMax_kg, " &
                "s.NiveauActuel AS [Niveau %], s.NiveauMini AS [Seuil Mini %], " &
                "s.DB_PLC, s.Offset_Niveau, m.NomMateriau AS [Matériau], " &
                "s.EstActif, s.DateModif " &
                "FROM dbo.Silos s " &
                "LEFT JOIN dbo.Materiaux m ON m.IdMateriau = s.IdMateriau " &
                "ORDER BY s.Position, s.SiloID")
            dgvSilos.DataSource = dt
        Catch ex As Exception
            MessageBox.Show("Erreur chargement Silos : " & ex.Message, "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    ' ═══════════════════════════════════════════════════════════════════
    '  SÉLECTION GRILLE
    ' ═══════════════════════════════════════════════════════════════════

    Private Sub DgvCases_SelectionChanged(sender As Object, e As EventArgs)
        If dgvCases.SelectedRows.Count = 0 Then Return
        Dim idx As Integer = dgvCases.SelectedRows(0).Index
        If idx < 0 OrElse idx >= dtCases.Rows.Count Then Return
        Dim row As DataRow = dtCases.Rows(idx)
        _caseID_selectionne = Convert.ToInt32(row("CaseID"))
        txtCNom.Text = row("NomCase").ToString()
        cmbCType.Text = row("TypeMateriau").ToString()
        txtCCapacite.Text = row("CapaciteKg").ToString()
        txtCNiveau.Text = row("NiveauKg").ToString()
        txtCHygro.Text = row("Hygrometrie").ToString()
        txtCSeuil.Text = row("SeuilAlerteKg").ToString()
        txtCTempsDosage.Text = row("TempsDosageMaxS").ToString()
        txtCSeuilPV.Text = row("SeuilPetiteVitesseKg").ToString()
        txtCSeuilVibreur.Text = row("SeuilDebitVibreurKgS").ToString()
        chkCDosageValide.Checked = Convert.ToBoolean(row("DosageValide"))
        chkCActif.Checked = Convert.ToBoolean(row("Actif"))
        lblCStatut.Text = "Case #" & _caseID_selectionne & " sélectionnée."
        lblCStatut.ForeColor = Color.Gray
    End Sub

    ' ═══════════════════════════════════════════════════════════════════
    '  CRUD CASES
    ' ═══════════════════════════════════════════════════════════════════

    Private Sub BtnCNouveau_Click(sender As Object, e As EventArgs)
        _caseID_selectionne = 0
        txtCNom.Text = "Case " & (dtCases.Rows.Count + 1)
        cmbCType.SelectedIndex = 0
        txtCCapacite.Text = "80000" : txtCNiveau.Text = "0" : txtCHygro.Text = "0"
        txtCSeuil.Text = "5000" : txtCTempsDosage.Text = "90"
        txtCSeuilPV.Text = "400" : txtCSeuilVibreur.Text = "59"
        chkCDosageValide.Checked = False : chkCActif.Checked = True
        lblCStatut.Text = "Nouvelle case — remplir et sauvegarder."
        lblCStatut.ForeColor = Color.DarkBlue
        txtCNom.Focus()
    End Sub

    Private Sub BtnCSauvegarder_Click(sender As Object, e As EventArgs)
        If txtCNom.Text.Trim() = "" Then
            MessageBox.Show("Le nom de la case est obligatoire.", "Validation",
                            MessageBoxButtons.OK, MessageBoxIcon.Warning) : Return
        End If
        Try
            If _caseID_selectionne = 0 Then
                Dim numMax As Integer = 0
                For Each row As DataRow In dtCases.Rows
                    Dim n As Integer = Convert.ToInt32(row("NumeroCase"))
                    If n > numMax Then numMax = n
                Next
                ModuleDb.Executer(
                    "INSERT INTO dbo.Cases " &
                    "(NumeroCase, TypeMateriau, NomCase, CapaciteKg, NiveauKg, " &
                    "Hygrometrie, SeuilAlerteKg, TempsDosageMaxS, " &
                    "SeuilPetiteVitesseKg, SeuilDebitVibreurKgS, DosageValide, Actif) " &
                    "VALUES (@num, @type, @nom, @cap, @niv, @hygro, @seuil, @temps, @pv, @vib, @valide, @actif)",
                    Sub(cmd) RemplirParamsCase(cmd, numMax + 1))
            Else
                ModuleDb.Executer(
                    "UPDATE dbo.Cases SET NomCase=@nom, TypeMateriau=@type, " &
                    "CapaciteKg=@cap, NiveauKg=@niv, Hygrometrie=@hygro, " &
                    "SeuilAlerteKg=@seuil, TempsDosageMaxS=@temps, " &
                    "SeuilPetiteVitesseKg=@pv, SeuilDebitVibreurKgS=@vib, " &
                    "DosageValide=@valide, Actif=@actif, DateModif=GETDATE() " &
                    "WHERE CaseID=@id",
                    Sub(cmd)
                        RemplirParamsCase(cmd, 0)
                        cmd.Parameters.Add("@id", SqlDbType.Int).Value = _caseID_selectionne
                    End Sub)
            End If
            lblCStatut.Text = "✓ Sauvegardé à " & DateTime.Now.ToString("HH:mm:ss")
            lblCStatut.ForeColor = Color.Green
            ChargerCases()
        Catch ex As Exception
            lblCStatut.Text = "✗ Erreur : " & ex.Message
            lblCStatut.ForeColor = Color.Red
            MessageBox.Show("Erreur : " & ex.Message, "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    Private Sub BtnCSupprimer_Click(sender As Object, e As EventArgs)
        If _caseID_selectionne = 0 Then
            MessageBox.Show("Sélectionnez d'abord une case.", "Info",
                            MessageBoxButtons.OK, MessageBoxIcon.Information) : Return
        End If
        If MessageBox.Show("Supprimer cette case ? Cette action est irréversible.",
                           "Confirmation", MessageBoxButtons.YesNo,
                           MessageBoxIcon.Warning) = DialogResult.No Then Return
        Try
            ModuleDb.Executer(
                "DELETE FROM dbo.Cases WHERE CaseID=@id",
                Sub(cmd) cmd.Parameters.Add("@id", SqlDbType.Int).Value = _caseID_selectionne)
            _caseID_selectionne = 0
            lblCStatut.Text = "✓ Case supprimée."
            lblCStatut.ForeColor = Color.DarkOrange
            ChargerCases()
        Catch ex As Exception
            MessageBox.Show("Erreur : " & ex.Message, "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    ' ═══════════════════════════════════════════════════════════════════
    '  HELPERS
    ' ═══════════════════════════════════════════════════════════════════

    Private Sub RemplirParamsCase(cmd As SqlCommand, numMax As Integer)
        If numMax > 0 Then cmd.Parameters.Add("@num", SqlDbType.Int).Value = numMax
        cmd.Parameters.Add("@type", SqlDbType.NVarChar, 50).Value = cmbCType.Text
        cmd.Parameters.Add("@nom", SqlDbType.NVarChar, 100).Value = txtCNom.Text.Trim()
        cmd.Parameters.Add("@cap", SqlDbType.Decimal).Value = ToDecimal(txtCCapacite.Text)
        cmd.Parameters.Add("@niv", SqlDbType.Decimal).Value = ToDecimal(txtCNiveau.Text)
        cmd.Parameters.Add("@hygro", SqlDbType.Decimal).Value = ToDecimal(txtCHygro.Text)
        cmd.Parameters.Add("@seuil", SqlDbType.Decimal).Value = ToDecimal(txtCSeuil.Text)
        cmd.Parameters.Add("@temps", SqlDbType.Decimal).Value = ToDecimal(txtCTempsDosage.Text)
        cmd.Parameters.Add("@pv", SqlDbType.Decimal).Value = ToDecimal(txtCSeuilPV.Text)
        cmd.Parameters.Add("@vib", SqlDbType.Decimal).Value = ToDecimal(txtCSeuilVibreur.Text)
        cmd.Parameters.Add("@valide", SqlDbType.Bit).Value = chkCDosageValide.Checked
        cmd.Parameters.Add("@actif", SqlDbType.Bit).Value = chkCActif.Checked
    End Sub

    Private Sub AjouterLabelFiche(parent As Panel, texte As String, x As Integer, y As Integer)
        parent.Controls.Add(New Label() With {
            .Text = texte, .Location = New Point(x, y + 3),
            .Size = New Size(148, 18), .Font = New Font("Arial", 8),
            .ForeColor = Color.DimGray})
    End Sub

    Private Function ToDecimal(texte As String) As Decimal
        Dim v As Decimal = 0
        Decimal.TryParse(texte.Trim().Replace(",", "."),
                         Globalization.NumberStyles.Any,
                         Globalization.CultureInfo.InvariantCulture, v)
        Return v
    End Function

End Class