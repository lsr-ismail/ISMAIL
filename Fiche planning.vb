Imports System.Data
Imports System.Drawing.Printing
Imports System.Windows.Forms.VisualStyles.VisualStyleElement
Imports Microsoft.Data.SqlClient

Public Class Fiche_planning

    Private _date As Date = Date.Today
    Private WithEvents pdFiche As New PrintDocument

    ' ═══════════════════════════════════════════════════════════════════
    '  LOAD
    ' ═══════════════════════════════════════════════════════════════════

    Private Sub Fiche_planning_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        dtpDate.Value = Date.Today
        StyleGrille()
        ChargerFichePlanning()
    End Sub

    ' ═══════════════════════════════════════════════════════════════════
    '  STYLE GRILLE
    ' ═══════════════════════════════════════════════════════════════════

    Private Sub StyleGrille()
        With DataGridView1
            .ReadOnly = True
            .AllowUserToAddRows = False
            .SelectionMode = DataGridViewSelectionMode.FullRowSelect
            .MultiSelect = False
            .RowHeadersVisible = False
            .EnableHeadersVisualStyles = False
            .AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.None
            .ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(45, 100, 165)
            .ColumnHeadersDefaultCellStyle.ForeColor = Color.White
            .ColumnHeadersDefaultCellStyle.Font = New Font("Arial", 9, FontStyle.Bold)
            .DefaultCellStyle.SelectionBackColor = Color.RoyalBlue
            .DefaultCellStyle.SelectionForeColor = Color.White
            .RowTemplate.Height = 28
            .BackgroundColor = Color.White
            .AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(245, 248, 255)
        End With
    End Sub

    ' ═══════════════════════════════════════════════════════════════════
    '  CHARGEMENT
    ' ═══════════════════════════════════════════════════════════════════

    Private Sub ChargerFichePlanning()
        Try
            _date = dtpDate.Value.Date

            Dim dt As DataTable = ModuleDb.Lire(
                "SELECT " &
                "IdPlanning AS [N°], " &
                "CONVERT(VARCHAR(5), HeureFabrication, 108) AS [H.Fab], " &
                "CONVERT(VARCHAR(5), HeureLivraison, 108) AS [H.Liv], " &
                "NomClient AS [Client], " &
                "NomChantier AS [Chantier], " &
                "CodeFormule AS [Formule], " &
                "QuantiteTotale AS [Qté m³], " &
                "QuantiteLivree AS [Livré], " &
                "Reste AS [Reste], " &
                "CASE WHEN Complement=1 THEN '✔' ELSE '' END AS [PL], " &
                "CASE WHEN AConfirmer=1 THEN '✔' ELSE '' END AS [AC], " &
                "StatutPlanning AS [Statut], " &
                "NomCamion AS [Camion], " &
                "NomChauffeur AS [Chauffeur] " &
                "FROM dbo.vw_ChargePlanning " &
                "WHERE DatePlanning=@DatePlanning " &
                "ORDER BY HeureFabrication, IdPlanning",
                Sub(cmd)
                    cmd.Parameters.Add("@DatePlanning", SqlDbType.Date).Value = _date
                End Sub)

            DataGridView1.DataSource = dt
            AppliquerColonnes()
            MettreAJourTotaux(dt)
            Me.BeginInvoke(New Action(Sub() ColorerLignes()))

        Catch ex As Exception
            MessageBox.Show("Erreur chargement fiche planning : " & ex.Message)
        End Try
    End Sub

    Private Sub AppliquerColonnes()
        If DataGridView1.Columns.Count = 0 Then Exit Sub
        Dim largeurs = New Dictionary(Of String, Integer) From {
            {"N°", 40}, {"H.Fab", 55}, {"H.Liv", 55},
            {"Client", 150}, {"Chantier", 150}, {"Formule", 75},
            {"Qté m³", 65}, {"Livré", 65}, {"Reste", 65},
            {"PL", 35}, {"AC", 35}, {"Statut", 85},
            {"Camion", 90}, {"Chauffeur", 100}
        }
        For Each kvp In largeurs
            If DataGridView1.Columns.Contains(kvp.Key) Then
                DataGridView1.Columns(kvp.Key).Width = kvp.Value
            End If
        Next
    End Sub

    Private Sub MettreAJourTotaux(dt As DataTable)
        Dim totalQte As Decimal = 0D
        Dim totalLivre As Decimal = 0D
        Dim totalReste As Decimal = 0D
        Dim nbCree As Integer = 0
        Dim nbEnCours As Integer = 0
        Dim nbTermine As Integer = 0

        For Each row As DataRow In dt.Rows
            If Not IsDBNull(row("Qté m³")) Then totalQte += Convert.ToDecimal(row("Qté m³"))
            If Not IsDBNull(row("Livré")) Then totalLivre += Convert.ToDecimal(row("Livré"))
            If Not IsDBNull(row("Reste")) Then totalReste += Convert.ToDecimal(row("Reste"))
            Select Case row("Statut").ToString().ToUpper()
                Case "CRÉÉ", "CREE" : nbCree += 1
                Case "EN COURS" : nbEnCours += 1
                Case "TERMINÉ", "TERMINE" : nbTermine += 1
            End Select
        Next

        ' Totaux
        lblNbPlannings.Text = "Plannings : " & dt.Rows.Count
        lblTotalQte.Text = "Total : " & totalQte.ToString("0.00") & " m³"
        lblTotalLivre.Text = "Livré : " & totalLivre.ToString("0.00") & " m³"
        lblTotalReste.Text = "Reste : " & totalReste.ToString("0.00") & " m³"

        ' Statuts
        lblCree.Text = "🔵 Créé : " & nbCree
        lblEnCours.Text = "🟡 En cours : " & nbEnCours
        lblTermine.Text = "🟢 Terminé : " & nbTermine

        ' Barre de progression
        If totalQte > 0 Then
            Dim pct As Integer = CInt((totalLivre / totalQte) * 100)
            progressBar1.Value = Math.Min(100, pct)
            lblPourcent.Text = pct.ToString() & " % livré"
        Else
            progressBar1.Value = 0
            lblPourcent.Text = "0 %"
        End If

        ' Titre avec date
        lblTitreFiche.Text = "📋 FICHE JOURNALIÈRE — " & _date.ToString("dddd dd MMMM yyyy").ToUpper()
    End Sub

    ' ═══════════════════════════════════════════════════════════════════
    '  COLORATION LIGNES
    ' ═══════════════════════════════════════════════════════════════════

    Private Sub ColorerLignes()
        If DataGridView1.Columns.Count = 0 Then Exit Sub
        For Each row As DataGridViewRow In DataGridView1.Rows
            If row.IsNewRow Then Continue For
            Select Case row.Cells("Statut").Value.ToString().ToUpper()
                Case "TERMINÉ", "TERMINE"
                    row.DefaultCellStyle.BackColor = Color.FromArgb(210, 255, 210)
                Case "EN COURS"
                    row.DefaultCellStyle.BackColor = Color.FromArgb(255, 245, 180)
                Case Else
                    row.DefaultCellStyle.BackColor = Color.White
            End Select
        Next
        DataGridView1.Refresh()
    End Sub

    ' ═══════════════════════════════════════════════════════════════════
    '  NAVIGATION DATE
    ' ═══════════════════════════════════════════════════════════════════

    Private Sub btnJourMoins_Click(sender As Object, e As EventArgs) Handles btnJourMoins.Click
        dtpDate.Value = dtpDate.Value.AddDays(-1)
        ChargerFichePlanning()
    End Sub

    Private Sub btnJourPlus_Click(sender As Object, e As EventArgs) Handles btnJourPlus.Click
        dtpDate.Value = dtpDate.Value.AddDays(1)
        ChargerFichePlanning()
    End Sub

    Private Sub btnAujourdhui_Click(sender As Object, e As EventArgs) Handles btnAujourdhui.Click
        dtpDate.Value = Date.Today
        ChargerFichePlanning()
    End Sub

    Private Sub dtpDate_ValueChanged(sender As Object, e As EventArgs) Handles dtpDate.ValueChanged
        ChargerFichePlanning()
    End Sub

    Private Sub btnActualiser_Click(sender As Object, e As EventArgs)
        ChargerFichePlanning()
    End Sub

    ' ═══════════════════════════════════════════════════════════════════
    '  IMPRESSION
    ' ═══════════════════════════════════════════════════════════════════

    Private Sub btnImprimer_Click(sender As Object, e As EventArgs) Handles btnImprimer.Click
        Try
            Dim apercu As New PrintPreviewDialog
            apercu.Document = pdFiche
            apercu.WindowState = FormWindowState.Maximized
            apercu.ShowDialog()
        Catch ex As Exception
            MessageBox.Show("Erreur impression : " & ex.Message)
        End Try
    End Sub

    Private Sub pdFiche_PrintPage(sender As Object, e As PrintPageEventArgs) Handles pdFiche.PrintPage
        Dim g As Graphics = e.Graphics
        Dim fontTitre As New Font("Arial", 14, FontStyle.Bold)
        Dim fontEntete As New Font("Arial", 10, FontStyle.Bold)
        Dim fontLabel As New Font("Arial", 9, FontStyle.Bold)
        Dim fontVal As New Font("Arial", 9)
        Dim x As Integer = 30 : Dim y As Integer = 30

        ' ── En-tête société ─────────────────────────────────────
        g.FillRectangle(New SolidBrush(Color.FromArgb(10, 50, 120)), x, y, 740, 60)
        g.DrawString(SocieteNom, New Font("Arial", 13, FontStyle.Bold), Brushes.White, x + 10, y + 5)
        g.DrawString(SocieteAdresse & "  Tél: " & SocieteTelephone, New Font("Arial", 8), Brushes.LightGray, x + 10, y + 28)
        g.DrawString("ICE: " & SocieteICE & "  RC: " & SocieteRC, New Font("Arial", 8), Brushes.LightGray, x + 10, y + 43)
        y += 70

        ' ── Titre fiche ─────────────────────────────────────────
        g.FillRectangle(New SolidBrush(Color.FromArgb(45, 100, 165)), x, y, 740, 30)
        g.DrawString("FICHE JOURNALIÈRE — " & _date.ToString("dddd dd MMMM yyyy").ToUpper(),
                     fontEntete, Brushes.White, x + 10, y + 7)
        y += 40

        ' ── Totaux ──────────────────────────────────────────────
        g.FillRectangle(New SolidBrush(Color.FromArgb(230, 235, 245)), x, y, 740, 28)
        g.DrawString("Total : " & lblTotalQte.Text.Replace("Total : ", "") &
                     "   Livré : " & lblTotalLivre.Text.Replace("Livré : ", "") &
                     "   Reste : " & lblTotalReste.Text.Replace("Reste : ", "") &
                     "   " & lblNbPlannings.Text,
                     fontLabel, Brushes.Black, x + 10, y + 7)
        y += 35

        ' ── En-têtes colonnes ───────────────────────────────────
        Dim cols = {"N°", "H.Fab", "H.Liv", "Client", "Chantier", "Formule", "Qté m³", "Livré", "Reste", "Statut", "Camion"}
        Dim largeurs = {30, 45, 45, 130, 130, 65, 55, 55, 55, 75, 85}
        Dim xCol As Integer = x

        g.FillRectangle(New SolidBrush(Color.FromArgb(45, 100, 165)), x, y, 740, 22)
        For i As Integer = 0 To cols.Length - 1
            g.DrawString(cols(i), New Font("Arial", 8, FontStyle.Bold), Brushes.White, xCol + 2, y + 4)
            xCol += largeurs(i)
        Next
        y += 22

        ' ── Lignes données ──────────────────────────────────────
        Dim dt As DataTable = TryCast(DataGridView1.DataSource, DataTable)
        If dt IsNot Nothing Then
            Dim pair As Boolean = False
            For Each row As DataRow In dt.Rows
                Dim bgColor As Color = If(pair, Color.FromArgb(245, 248, 255), Color.White)
                Select Case row("Statut").ToString().ToUpper()
                    Case "TERMINÉ", "TERMINE" : bgColor = Color.FromArgb(210, 255, 210)
                    Case "EN COURS" : bgColor = Color.FromArgb(255, 245, 180)
                End Select
                g.FillRectangle(New SolidBrush(bgColor), x, y, 740, 20)

                xCol = x
                Dim vals = {
                    row("N°").ToString(),
                    row("H.Fab").ToString(),
                    row("H.Liv").ToString(),
                    TronquerTexte(row("Client").ToString(), 18),
                    TronquerTexte(row("Chantier").ToString(), 18),
                    row("Formule").ToString(),
                    row("Qté m³").ToString(),
                    row("Livré").ToString(),
                    row("Reste").ToString(),
                    row("Statut").ToString(),
                    TronquerTexte(row("Camion").ToString(), 12)
                }
                For i As Integer = 0 To vals.Length - 1
                    g.DrawString(vals(i), fontVal, Brushes.Black, xCol + 2, y + 3)
                    xCol += largeurs(i)
                Next

                g.DrawLine(Pens.LightGray, x, y + 20, x + 740, y + 20)
                y += 20
                pair = Not pair

                If y > 1020 Then Exit For
            Next
        End If

        ' ── Pied de page ────────────────────────────────────────
        g.FillRectangle(New SolidBrush(Color.FromArgb(10, 50, 120)), x, 800, 740, 15)
        g.DrawString("Imprimé le " & DateTime.Now.ToString("dd/MM/yyyy HH:mm") &
                     "  |  " & SocieteNom & "  |  ICE: " & SocieteICE,
                     New Font("Arial", 7), Brushes.White, x + 5, 803)
    End Sub

    ' ═══════════════════════════════════════════════════════════════════
    '  EXPORT CSV
    ' ═══════════════════════════════════════════════════════════════════

    Private Sub btnExportCSV_Click(sender As Object, e As EventArgs) Handles btnExportCSV.Click
        Try
            Dim chemin As String = IO.Path.Combine(
                Environment.GetFolderPath(Environment.SpecialFolder.Desktop),
                "Fiche_Planning_" & _date.ToString("yyyyMMdd") & ".csv")

            Dim dt As DataTable = TryCast(DataGridView1.DataSource, DataTable)
            If dt Is Nothing OrElse dt.Rows.Count = 0 Then
                MessageBox.Show("Aucune donnée à exporter.")
                Exit Sub
            End If

            Using sw As New IO.StreamWriter(chemin, False, System.Text.Encoding.UTF8)
                ' En-têtes
                Dim entetes As New List(Of String)
                For Each col As DataColumn In dt.Columns
                    entetes.Add(col.ColumnName)
                Next
                sw.WriteLine(String.Join(";", entetes))

                ' Données
                For Each row As DataRow In dt.Rows
                    Dim vals As New List(Of String)
                    For Each col As DataColumn In dt.Columns
                        vals.Add(row(col).ToString().Replace(";", ","))
                    Next
                    sw.WriteLine(String.Join(";", vals))
                Next
            End Using

            MessageBox.Show("✅ Export réussi !" & vbCrLf & chemin,
                            "Export CSV", MessageBoxButtons.OK, MessageBoxIcon.Information)

        Catch ex As Exception
            MessageBox.Show("Erreur export : " & ex.Message)
        End Try
    End Sub

    ' ═══════════════════════════════════════════════════════════════════
    '  HELPER
    ' ═══════════════════════════════════════════════════════════════════

    Private Function TronquerTexte(txt As String, max As Integer) As String
        If txt.Length <= max Then Return txt
        Return txt.Substring(0, max - 1) & "…"
    End Function

End Class