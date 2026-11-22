Imports System.Data
Imports Microsoft.Data.SqlClient
Imports System.Text
Imports System.Windows.Forms

' ================================================================
'  Historique_Gachees.vb — TABLE DES GÂCHÉES ENREGISTRÉES (SQL)
'  16/09/2026
'  ─────────────────────────────────────────────────────────────
'  Affiche dbo.GacheesDetails : matricule CPU (DB71), date/heure,
'  N° BL, N° gâchée, poses et écarts de chaque composant.
'  Filtres : période, N° BL, matricule. Export CSV (Excel).
'  Suivi auto : la table se rafraîchit toute seule quand une
'  nouvelle gâchée est archivée (ModuleArchivageGachees).
'
'  Formulaire construit entièrement en code : aucun Designer.
'  Ouverture : PanelMain → menu Production → "Historique gâchées"
' ================================================================
Public Class Historique_Gachees
    Inherits Form

    ' ── Filtres ──────────────────────────────────────────────────
    Private dtpDu As New DateTimePicker()
    Private dtpAu As New DateTimePicker()
    Private txtBL As New TextBox()
    Private txtMatricule As New TextBox()
    Private chkSuiviAuto As New CheckBox()
    Private WithEvents btnActualiser As New Button()
    Private WithEvents btnAujourdhui As New Button()
    Private WithEvents btnExporter As New Button()
    Private WithEvents btnFermer As New Button()
    Private WithEvents btnDiag As New Button()

    ' ── Grille + état ────────────────────────────────────────────
    Private WithEvents grid As New DataGridView()
    Private lblEtat As New Label()
    Private lblTotaux As New Label()
    Private WithEvents timerSuivi As New Timer()
    Private _derniereCle As String = ""

    ' ═══════════════════════════════════════════════════════════
    '  CONSTRUCTEUR
    ' ═══════════════════════════════════════════════════════════
    Public Sub New()
        Me.Text = "Historique des gâchées"
        Me.Size = New Size(1380, 720)
        Me.StartPosition = FormStartPosition.CenterScreen
        Me.Font = New Font("Segoe UI", 9)
        Me.BackColor = Color.FromArgb(245, 248, 252)
        ConstruireInterface()
        ConstruireColonnes()
    End Sub

    Private Sub Historique_Gachees_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        dtpDu.Value = Date.Today.AddDays(-7)
        dtpAu.Value = Date.Today
        Charger()
        timerSuivi.Interval = 10000
        timerSuivi.Start()
    End Sub

    ' ═══════════════════════════════════════════════════════════
    '  INTERFACE
    ' ═══════════════════════════════════════════════════════════
    Private Sub ConstruireInterface()
        Dim pnlHaut As New Panel With {.Dock = DockStyle.Top, .Height = 58, .BackColor = Color.FromArgb(11, 61, 92)}

        Dim titre As New Label With {
            .Text = "HISTORIQUE DES GÂCHÉES", .ForeColor = Color.White,
            .Font = New Font("Segoe UI", 13, FontStyle.Bold), .AutoSize = True, .Location = New Point(12, 16)}
        pnlHaut.Controls.Add(titre)

        Dim x As Integer = 300
        pnlHaut.Controls.Add(Etiquette("Du :", x, 20)) : x += 32
        dtpDu.Format = DateTimePickerFormat.Short : dtpDu.Width = 105 : dtpDu.Location = New Point(x, 17)
        pnlHaut.Controls.Add(dtpDu) : x += 115
        pnlHaut.Controls.Add(Etiquette("Au :", x, 20)) : x += 32
        dtpAu.Format = DateTimePickerFormat.Short : dtpAu.Width = 105 : dtpAu.Location = New Point(x, 17)
        pnlHaut.Controls.Add(dtpAu) : x += 120

        pnlHaut.Controls.Add(Etiquette("N° BL :", x, 20)) : x += 52
        txtBL.Width = 90 : txtBL.Location = New Point(x, 17)
        pnlHaut.Controls.Add(txtBL) : x += 100

        pnlHaut.Controls.Add(Etiquette("Matricule :", x, 20)) : x += 74
        txtMatricule.Width = 80 : txtMatricule.Location = New Point(x, 17)
        pnlHaut.Controls.Add(txtMatricule) : x += 95

        btnActualiser.Text = "Actualiser" : btnActualiser.Location = New Point(x, 14) : btnActualiser.Size = New Size(95, 30)
        StyleBouton(btnActualiser, Color.FromArgb(0, 120, 215)) : pnlHaut.Controls.Add(btnActualiser) : x += 100
        btnAujourdhui.Text = "Aujourd'hui" : btnAujourdhui.Location = New Point(x, 14) : btnAujourdhui.Size = New Size(95, 30)
        StyleBouton(btnAujourdhui, Color.FromArgb(0, 150, 100)) : pnlHaut.Controls.Add(btnAujourdhui) : x += 100
        btnExporter.Text = "Exporter CSV" : btnExporter.Location = New Point(x, 14) : btnExporter.Size = New Size(105, 30)
        StyleBouton(btnExporter, Color.FromArgb(90, 90, 90)) : pnlHaut.Controls.Add(btnExporter) : x += 110

        chkSuiviAuto.Text = "Suivi auto" : chkSuiviAuto.Checked = True : chkSuiviAuto.ForeColor = Color.White
        chkSuiviAuto.AutoSize = True : chkSuiviAuto.Location = New Point(x, 20)
        pnlHaut.Controls.Add(chkSuiviAuto) : x += 95

        btnFermer.Text = "Fermer" : btnFermer.Location = New Point(x, 14) : btnFermer.Size = New Size(80, 30)
        StyleBouton(btnFermer, Color.FromArgb(180, 50, 50)) : pnlHaut.Controls.Add(btnFermer) : x += 85
        btnDiag.Text = "Diag DB71" : btnDiag.Location = New Point(x, 14) : btnDiag.Size = New Size(90, 30)
        StyleBouton(btnDiag, Color.FromArgb(120, 80, 160)) : pnlHaut.Controls.Add(btnDiag)

        Dim pnlBas As New Panel With {.Dock = DockStyle.Bottom, .Height = 30, .BackColor = Color.FromArgb(230, 235, 240)}
        lblEtat.AutoSize = True : lblEtat.Location = New Point(10, 7) : lblEtat.Text = "…"
        lblTotaux.AutoSize = True : lblTotaux.Location = New Point(420, 7) : lblTotaux.Font = New Font("Segoe UI", 9, FontStyle.Bold)
        pnlBas.Controls.Add(lblEtat) : pnlBas.Controls.Add(lblTotaux)

        grid.Dock = DockStyle.Fill
        grid.Font = New Font("Segoe UI", 8)
        grid.RowHeadersVisible = False
        grid.AllowUserToAddRows = False
        grid.AllowUserToDeleteRows = False
        grid.ReadOnly = True
        grid.SelectionMode = DataGridViewSelectionMode.FullRowSelect
        grid.MultiSelect = False
        grid.BackgroundColor = Color.White
        grid.GridColor = Color.LightGray
        grid.BorderStyle = BorderStyle.None
        grid.AutoGenerateColumns = False
        grid.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize
        grid.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(11, 61, 92)
        grid.ColumnHeadersDefaultCellStyle.ForeColor = Color.White
        grid.ColumnHeadersDefaultCellStyle.Font = New Font("Segoe UI", 8, FontStyle.Bold)
        grid.ColumnHeadersDefaultCellStyle.WrapMode = DataGridViewTriState.True
        grid.EnableHeadersVisualStyles = False
        grid.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(245, 248, 252)
        grid.RowTemplate.Height = 22

        Me.Controls.Add(grid)
        Me.Controls.Add(pnlBas)
        Me.Controls.Add(pnlHaut)
        grid.BringToFront()
    End Sub

    Private Function Etiquette(texte As String, x As Integer, y As Integer) As Label
        Return New Label With {.Text = texte, .ForeColor = Color.White, .AutoSize = True, .Location = New Point(x, y)}
    End Function

    Private Sub StyleBouton(b As Button, fond As Color)
        b.FlatStyle = FlatStyle.Flat : b.FlatAppearance.BorderSize = 0
        b.BackColor = fond : b.ForeColor = Color.White
        b.Font = New Font("Segoe UI", 9, FontStyle.Bold) : b.Cursor = Cursors.Hand
    End Sub

    ' Colonnes : (Titre, Largeur, Champ SQL, Format)
    Private Sub ConstruireColonnes()
        Dim cols() As (String, Integer, String, String) = {
            ("Matricule", 70, "MatriculeGachee", "N0"),
            ("N°" & vbLf & "CPU", 48, "CompteurCPU", "N0"),
            ("Date", 80, "DateJour", ""),
            ("Heure", 58, "HeureStr", ""),
            ("N° BL", 85, "NumeroBL", ""),
            ("N°" & vbLf & "gâchée", 48, "NumeroGachee", ""),
            ("Pose" & vbLf & "TR1", 58, "PoseTr1", "N0"), ("Écart %" & vbLf & "TR1", 56, "EcartTr1", "N2"),
            ("Pose" & vbLf & "TR2", 58, "PoseTr2", "N0"), ("Écart %" & vbLf & "TR2", 56, "EcartTr2", "N2"),
            ("Pose" & vbLf & "TR3", 58, "PoseTr3", "N0"), ("Écart %" & vbLf & "TR3", 56, "EcartTr3", "N2"),
            ("Pose" & vbLf & "TR4", 58, "PoseTr4", "N0"), ("Écart %" & vbLf & "TR4", 56, "EcartTr4", "N2"),
            ("Total" & vbLf & "granulats", 65, "TotalPoseTr", "N0"),
            ("Pose" & vbLf & "CIM", 58, "PoseS1", "N0"), ("Écart %" & vbLf & "CIM", 56, "EcartCIM", "N2"),
            ("Pose" & vbLf & "EAU", 58, "PoseEAU", "N1"), ("Écart %" & vbLf & "EAU", 56, "EcartEAU", "N2"),
            ("Pose" & vbLf & "ADJ1", 58, "PoseADJ1", "N2"), ("Écart %" & vbLf & "ADJ1", 56, "EcartADJ1", "N2"),
            ("Pose" & vbLf & "ADJ2", 58, "PoseADJ2", "N2"), ("Écart %" & vbLf & "ADJ2", 56, "EcartADJ2", "N2"),
            ("Pose" & vbLf & "ADJ3", 58, "PoseADJ3", "N2"), ("Écart %" & vbLf & "ADJ3", 56, "EcartADJ3", "N2"),
            ("Pose" & vbLf & "ADJ4", 58, "PoseADJ4", "N2"), ("Écart %" & vbLf & "ADJ4", 56, "EcartADJ4", "N2"),
            ("Opérateur", 90, "Operateur", "")}

        For Each c In cols
            Dim col As New DataGridViewTextBoxColumn()
            col.HeaderText = c.Item1
            col.Width = c.Item2
            col.Name = c.Item3
            col.DataPropertyName = c.Item3
            col.SortMode = DataGridViewColumnSortMode.Automatic
            col.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter
            If c.Item4 <> "" Then col.DefaultCellStyle.Format = c.Item4
            If c.Item3.StartsWith("Ecart") Then col.DefaultCellStyle.BackColor = Color.FromArgb(255, 250, 235)
            If c.Item3 = "MatriculeGachee" Then col.DefaultCellStyle.Font = New Font("Segoe UI", 8, FontStyle.Bold)
            grid.Columns.Add(col)
        Next
    End Sub

    ' ═══════════════════════════════════════════════════════════
    '  CHARGEMENT SQL
    ' ═══════════════════════════════════════════════════════════
    Private Function ConstruireSQL(ByRef param As Action(Of SqlCommand)) As String
        Dim du As Date = dtpDu.Value.Date
        Dim au As Date = dtpAu.Value.Date.AddDays(1)
        Dim bl As String = txtBL.Text.Trim()
        Dim mat As Long = 0
        Long.TryParse(txtMatricule.Text.Trim(), mat)

        Dim sb As New StringBuilder()
        sb.Append("SELECT TOP 2000 ")
        sb.Append("  MatriculeGachee, CompteurCPU, DateHeure, ")
        sb.Append("  CONVERT(VARCHAR(10), DateHeure, 103) AS DateJour, ")
        sb.Append("  CONVERT(VARCHAR(8),  DateHeure, 108) AS HeureStr, ")
        sb.Append("  ISNULL(NumeroBL,'---') AS NumeroBL, NumeroGachee, ")
        sb.Append("  PoseTr1, EcartTr1, PoseTr2, EcartTr2, PoseTr3, EcartTr3, PoseTr4, EcartTr4, TotalPoseTr, ")
        sb.Append("  PoseS1, EcartCIM, PoseEAU, EcartEAU, ")
        sb.Append("  PoseADJ1, EcartADJ1, PoseADJ2, EcartADJ2, PoseADJ3, EcartADJ3, PoseADJ4, EcartADJ4, ")
        sb.Append("  ISNULL(Operateur,'') AS Operateur ")
        sb.Append("FROM dbo.GacheesDetails WHERE 1=1 ")
        If mat > 0 Then
            sb.Append("AND MatriculeGachee = @mat ")
        Else
            sb.Append("AND DateHeure >= @du AND DateHeure < @au ")
            If bl <> "" Then sb.Append("AND NumeroBL LIKE @bl ")
        End If
        sb.Append("ORDER BY DateHeure DESC, MatriculeGachee DESC")

        param = Sub(cmd)
                    cmd.Parameters.AddWithValue("@mat", mat)
                    cmd.Parameters.AddWithValue("@du", du)
                    cmd.Parameters.AddWithValue("@au", au)
                    cmd.Parameters.AddWithValue("@bl", "%" & bl & "%")
                End Sub
        Return sb.ToString()
    End Function

    Private Sub Charger()
        Try
            AssurerColonneMatricule()
            Dim p As Action(Of SqlCommand) = Nothing
            Dim sql As String = ConstruireSQL(p)
            Dim dt As DataTable = ModuleDb.Lire(sql, p)
            If dt Is Nothing Then Exit Sub

            grid.SuspendLayout()
            grid.DataSource = dt
            grid.ResumeLayout()

            ' Totaux
            Dim tG As Double = 0, tC As Double = 0, tE As Double = 0
            For Each r As DataRow In dt.Rows
                tG += D(r("TotalPoseTr")) : tC += D(r("PoseS1")) : tE += D(r("PoseEAU"))
            Next
            lblEtat.Text = dt.Rows.Count & " gâchée(s)  —  " & dtpDu.Value.ToShortDateString() & " → " & dtpAu.Value.ToShortDateString() &
                           "   (actualisé " & Now.ToString("HH:mm:ss") & ")"
            lblTotaux.Text = "Total granulats : " & tG.ToString("N0") & " kg    Ciment : " & tC.ToString("N0") & " kg    Eau : " & tE.ToString("N0") & " L"
            _derniereCle = CleActuelle()
        Catch ex As Exception
            lblEtat.Text = "Erreur SQL : " & ex.Message
        End Try
    End Sub

    ' Signature légère "dernière gâchée + nombre" pour ne recharger que si nécessaire
    Private Function CleActuelle() As String
        Try
            Dim v As Object = ModuleDb.LireValeur(Of Object)(
                "SELECT CAST(COUNT(*) AS VARCHAR) + '|' + CAST(ISNULL(MAX(MatriculeGachee),0) AS VARCHAR) " &
                "FROM dbo.GacheesDetails WHERE DateHeure >= @du",
                Sub(cmd) cmd.Parameters.AddWithValue("@du", dtpDu.Value.Date))
            Return If(v Is Nothing OrElse IsDBNull(v), "", v.ToString())
        Catch
            Return ""
        End Try
    End Function

    Private Sub AssurerColonneMatricule()
        Try
            ModuleDb.Executer(
                "IF COL_LENGTH('dbo.GacheesDetails','MatriculeGachee') IS NULL " &
                "ALTER TABLE dbo.GacheesDetails ADD MatriculeGachee BIGINT NULL")
            ModuleDb.Executer(
                "IF COL_LENGTH('dbo.GacheesDetails','CompteurCPU') IS NULL " &
                "ALTER TABLE dbo.GacheesDetails ADD CompteurCPU BIGINT NULL")
        Catch
        End Try
    End Sub

    ' ═══════════════════════════════════════════════════════════
    '  ÉVÉNEMENTS
    ' ═══════════════════════════════════════════════════════════
    Private Sub btnActualiser_Click(sender As Object, e As EventArgs) Handles btnActualiser.Click
        Charger()
    End Sub

    Private Sub btnAujourdhui_Click(sender As Object, e As EventArgs) Handles btnAujourdhui.Click
        dtpDu.Value = Date.Today : dtpAu.Value = Date.Today
        txtBL.Text = "" : txtMatricule.Text = ""
        Charger()
    End Sub

    Private Sub btnFermer_Click(sender As Object, e As EventArgs) Handles btnFermer.Click
        Me.Close()
    End Sub

    ' Diagnostic DB71 : valeurs brutes des blocs (vérification des offsets)
    Private Sub btnDiag_Click(sender As Object, e As EventArgs) Handles btnDiag.Click
        Dim txt As String = ModuleGacheesDB71.Diagnostic(3)
        Dim f As New Form With {.Text = "Diagnostic DB71 — valeurs brutes", .Size = New Size(900, 650), .StartPosition = FormStartPosition.CenterParent}
        Dim tb As New TextBox With {.Multiline = True, .ReadOnly = True, .ScrollBars = ScrollBars.Both, .WordWrap = False,
                                    .Dock = DockStyle.Fill, .Font = New Font("Consolas", 10), .Text = txt}
        f.Controls.Add(tb)
        f.ShowDialog(Me)
    End Sub

    Private Sub timerSuivi_Tick(sender As Object, e As EventArgs) Handles timerSuivi.Tick
        If Not chkSuiviAuto.Checked Then Exit Sub
        If txtMatricule.Text.Trim() <> "" Then Exit Sub
        ' Ne pas perdre la sélection de l'utilisateur pour rien
        If CleActuelle() <> _derniereCle Then Charger()
    End Sub

    ' Écarts (% CPU) en rouge si > 3 %
    Private Sub grid_CellFormatting(sender As Object, e As DataGridViewCellFormattingEventArgs) Handles grid.CellFormatting
        If e.RowIndex < 0 OrElse e.Value Is Nothing OrElse IsDBNull(e.Value) Then Exit Sub
        Dim nom As String = grid.Columns(e.ColumnIndex).Name
        If Not nom.StartsWith("Ecart") Then Exit Sub
        ' Écart CPU en % : rouge au-delà de ±3 %
        Dim ecart As Double = D(e.Value)
        If Math.Abs(ecart) > 3 Then
            e.CellStyle.ForeColor = Color.Red
            e.CellStyle.Font = New Font("Segoe UI", 8, FontStyle.Bold)
        End If
    End Sub

    ' Double-clic : fiche détaillée de la gâchée
    Private Sub grid_CellDoubleClick(sender As Object, e As DataGridViewCellEventArgs) Handles grid.CellDoubleClick
        If e.RowIndex < 0 Then Exit Sub
        Dim r As DataGridViewRow = grid.Rows(e.RowIndex)
        Dim sb As New StringBuilder()
        sb.AppendLine("Matricule : " & V(r, "MatriculeGachee"))
        sb.AppendLine("Date      : " & V(r, "DateJour") & " " & V(r, "HeureStr"))
        sb.AppendLine("N° BL     : " & V(r, "NumeroBL") & "   Gâchée n° " & V(r, "NumeroGachee"))
        sb.AppendLine("")
        sb.AppendLine("           Pose      Écart")
        For Each c In {("TR1", "PoseTr1", "EcartTr1"), ("TR2", "PoseTr2", "EcartTr2"), ("TR3", "PoseTr3", "EcartTr3"), ("TR4", "PoseTr4", "EcartTr4"),
                       ("CIM", "PoseS1", "EcartCIM"), ("EAU", "PoseEAU", "EcartEAU"),
                       ("ADJ1", "PoseADJ1", "EcartADJ1"), ("ADJ2", "PoseADJ2", "EcartADJ2"), ("ADJ3", "PoseADJ3", "EcartADJ3"), ("ADJ4", "PoseADJ4", "EcartADJ4")}
            sb.AppendLine(c.Item1.PadRight(6) & V(r, c.Item2).PadLeft(10) & V(r, c.Item3).PadLeft(10))
        Next
        sb.AppendLine("")
        sb.AppendLine("Total granulats : " & V(r, "TotalPoseTr") & " kg")
        sb.AppendLine("Opérateur : " & V(r, "Operateur"))
        MessageBox.Show(sb.ToString(), "Gâchée " & V(r, "MatriculeGachee"), MessageBoxButtons.OK, MessageBoxIcon.Information)
    End Sub

    Private Function D(o As Object) As Double
        If o Is Nothing OrElse IsDBNull(o) Then Return 0
        Dim valeur As Double = 0
        If Double.TryParse(o.ToString(), valeur) Then Return valeur
        Return 0
    End Function

    Private Function V(r As DataGridViewRow, col As String) As String
        Dim o As Object = r.Cells(col).Value
        If o Is Nothing OrElse IsDBNull(o) Then Return "-"
        If TypeOf o Is Double OrElse TypeOf o Is Single OrElse TypeOf o Is Decimal Then Return CDbl(o).ToString("N1")
        Return o.ToString()
    End Function

    ' ═══════════════════════════════════════════════════════════
    '  EXPORT CSV (séparateur ; → s'ouvre directement dans Excel)
    ' ═══════════════════════════════════════════════════════════
    Private Sub btnExporter_Click(sender As Object, e As EventArgs) Handles btnExporter.Click
        If grid.Rows.Count = 0 Then
            MessageBox.Show("Aucune gâchée à exporter.", "Export", MessageBoxButtons.OK, MessageBoxIcon.Information)
            Exit Sub
        End If
        Using dlg As New SaveFileDialog()
            dlg.Filter = "Fichier CSV (*.csv)|*.csv"
            dlg.FileName = "Gachees_" & dtpDu.Value.ToString("yyyyMMdd") & "_" & dtpAu.Value.ToString("yyyyMMdd") & ".csv"
            If dlg.ShowDialog() <> DialogResult.OK Then Exit Sub
            Try
                Dim sb As New StringBuilder()
                Dim entetes As New List(Of String)
                For Each c As DataGridViewColumn In grid.Columns
                    entetes.Add(c.HeaderText.Replace(vbLf, " "))
                Next
                sb.AppendLine(String.Join(";", entetes))
                For Each r As DataGridViewRow In grid.Rows
                    Dim vals As New List(Of String)
                    For Each c As DataGridViewColumn In grid.Columns
                        Dim o As Object = r.Cells(c.Index).Value
                        vals.Add(If(o Is Nothing OrElse IsDBNull(o), "", o.ToString().Replace(";", ",")))
                    Next
                    sb.AppendLine(String.Join(";", vals))
                Next
                IO.File.WriteAllText(dlg.FileName, sb.ToString(), Encoding.UTF8)
                MessageBox.Show(grid.Rows.Count & " gâchée(s) exportée(s) :" & vbCrLf & dlg.FileName, "Export", MessageBoxButtons.OK, MessageBoxIcon.Information)
            Catch ex As Exception
                MessageBox.Show("Erreur export : " & ex.Message, "Export", MessageBoxButtons.OK, MessageBoxIcon.Error)
            End Try
        End Using
    End Sub

End Class
