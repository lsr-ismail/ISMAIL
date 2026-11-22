Imports Microsoft.Data.SqlClient
Imports System.Data
Imports System.Drawing.Printing

Public Class Alarmes_machine

    Private docImpression As New PrintDocument
    Private apercuImpression As New PrintPreviewDialog
    Private pageImpressionCourante As Integer = 0

    ' ═══════════════════════════════════════════════════════════════════
    '  LOAD
    ' ═══════════════════════════════════════════════════════════════════

    Private Sub Alarmes_machine_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        DataGridView1.RowHeadersVisible = False
        AddHandler docImpression.PrintPage, AddressOf docImpression_PrintPage

        dtDebut.Value = Date.Today.AddDays(-7)   ' par défaut : 7 derniers jours
        dtFin.Value = Date.Today

        ChargerAlarmesParPeriode()
    End Sub

    ' ═══════════════════════════════════════════════════════════════════
    '  CHARGEMENT PAR PERIODE (entre les 2 dates)
    ' ═══════════════════════════════════════════════════════════════════

    Private Sub ChargerAlarmesParPeriode()
        Try
            Dim dt As DataTable = ModuleDb.Lire(
                "SELECT Id, DateHeure, CodeAlarme, Description, Etat, TypeAlarme, Operateur, DateResolution " &
                "FROM dbo.Alarmes " &
                "WHERE DateHeure >= @DateDebut AND DateHeure < @DateFin " &
                "ORDER BY DateHeure DESC",
                Sub(cmd)
                    cmd.Parameters.Add("@DateDebut", SqlDbType.DateTime).Value = dtDebut.Value.Date
                    cmd.Parameters.Add("@DateFin", SqlDbType.DateTime).Value = dtFin.Value.Date.AddDays(1)   ' inclut toute la journée de fin
                End Sub)

            AfficherDansGrille(dt)

        Catch ex As Exception
            MessageBox.Show("Erreur chargement : " & ex.Message)
        End Try
    End Sub

    ' ═══════════════════════════════════════════════════════════════════
    '  INSTANTANE — alarmes actives EN CE MOMENT (ignore les dates)
    ' ═══════════════════════════════════════════════════════════════════

    Private Sub btnInstantane_Click(sender As Object, e As EventArgs) Handles btnInstantane.Click
        Try
            Dim dt As DataTable = ModuleDb.Lire(
                "SELECT Id, DateHeure, CodeAlarme, Description, Etat, TypeAlarme, Operateur, DateResolution " &
                "FROM dbo.Alarmes " &
                "WHERE Etat = 'ACTIVE' " &
                "ORDER BY DateHeure DESC")

            AfficherDansGrille(dt)

        Catch ex As Exception
            MessageBox.Show("Erreur instantané : " & ex.Message)
        End Try
    End Sub

    ' ═══════════════════════════════════════════════════════════════════
    '  ACTUALISER — relance le filtre par période actuel
    ' ═══════════════════════════════════════════════════════════════════

    Private Sub btnActualiser_Click(sender As Object, e As EventArgs) Handles btnActualiser.Click
        ChargerAlarmesParPeriode()
    End Sub

    ' ═══════════════════════════════════════════════════════════════════
    '  AFFICHAGE COMMUN DE LA GRILLE
    ' ═══════════════════════════════════════════════════════════════════

    ' 22/09 : 3 colonnes Date / DB (adresse du bit) / Commentaire ; clic → commentaire dans txtedite
    Private Sub AfficherDansGrille(dt As DataTable)
        DataGridView1.DataSource = dt
        DataGridView1.ReadOnly = True
        DataGridView1.AllowUserToAddRows = False
        DataGridView1.SelectionMode = DataGridViewSelectionMode.FullRowSelect
        DataGridView1.MultiSelect = False

        For Each col As DataGridViewColumn In DataGridView1.Columns
            col.Visible = False
        Next
        DataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.None

        If DataGridView1.Columns.Contains("DateHeure") Then
            DataGridView1.Columns("DateHeure").Visible = True
            DataGridView1.Columns("DateHeure").HeaderText = "Date"
            DataGridView1.Columns("DateHeure").Width = 140
            DataGridView1.Columns("DateHeure").DefaultCellStyle.Format = "dd/MM/yyyy HH:mm:ss"
            DataGridView1.Columns("DateHeure").DisplayIndex = 0
        End If
        If DataGridView1.Columns.Contains("CodeAlarme") Then
            DataGridView1.Columns("CodeAlarme").Visible = True
            DataGridView1.Columns("CodeAlarme").HeaderText = "DB"
            DataGridView1.Columns("CodeAlarme").Width = 120
            DataGridView1.Columns("CodeAlarme").DisplayIndex = 1
        End If
        If DataGridView1.Columns.Contains("Description") Then
            DataGridView1.Columns("Description").Visible = True
            DataGridView1.Columns("Description").HeaderText = "Commentaire"
            DataGridView1.Columns("Description").Width = 420
            DataGridView1.Columns("Description").DisplayIndex = 2
        End If
        If DataGridView1.Columns.Contains("Etat") Then
            DataGridView1.Columns("Etat").Visible = True
            DataGridView1.Columns("Etat").HeaderText = "État"
            DataGridView1.Columns("Etat").Width = 80
            DataGridView1.Columns("Etat").DisplayIndex = 3
        End If
        ' Défauts encore actifs en rouge
        For Each r As DataGridViewRow In DataGridView1.Rows
            Dim et As String = LireTexte(r, "Etat").ToUpper()
            If et = "ACTIVE" Then r.DefaultCellStyle.ForeColor = Color.Red
        Next

        lblNombre.Text = "Total : " & dt.Rows.Count & " alarme(s)"
        txtedite.Text = ""
    End Sub

    ' ── Sélection d'une ligne → commentaire dans la case ────────
    Private Sub DataGridView1_SelectionChanged(sender As Object, e As EventArgs) Handles DataGridView1.SelectionChanged
        Try
            If DataGridView1.CurrentRow Is Nothing Then Exit Sub
            txtedite.Text = LireTexte(DataGridView1.CurrentRow, "Description")
        Catch
        End Try
    End Sub

    ' ── EDITE : enregistre le commentaire dans AlarmesConfig (+ historique) ──
    Private Sub btnModifier_Click(sender As Object, e As EventArgs) Handles btnModifier.Click
        If DataGridView1.CurrentRow Is Nothing Then
            MessageBox.Show("Sélectionnez d'abord une ligne.", "Edite", MessageBoxButtons.OK, MessageBoxIcon.Information)
            Exit Sub
        End If
        Dim adresse As String = LireTexte(DataGridView1.CurrentRow, "CodeAlarme")
        Dim texte As String = txtedite.Text.Trim()
        If adresse = "" OrElse texte = "" Then
            MessageBox.Show("Commentaire vide.", "Edite", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Exit Sub
        End If
        If ModuleDefautsCPU.EnregistrerCommentaire(adresse, texte) Then
            lblNombre.Text = "Commentaire enregistré pour " & adresse
            ChargerAlarmesParPeriode()
        End If
    End Sub

    ' ── EXPORT CSV de la grille affichée ─────────────────────────
    Private Sub btexpcsv_Click(sender As Object, e As EventArgs) Handles btexpcsv.Click
        If DataGridView1.Rows.Count = 0 Then
            MessageBox.Show("Aucune alarme à exporter.", "Export", MessageBoxButtons.OK, MessageBoxIcon.Information)
            Exit Sub
        End If
        Using sfd As New SaveFileDialog With {.Filter = "CSV (*.csv)|*.csv", .FileName = "Alarmes_" & Date.Now.ToString("yyyyMMdd_HHmm") & ".csv"}
            If sfd.ShowDialog(Me) <> DialogResult.OK Then Exit Sub
            Try
                Dim sb As New System.Text.StringBuilder()
                sb.AppendLine("Date;DB;Commentaire;Etat;Type;Resolution")
                For Each r As DataGridViewRow In DataGridView1.Rows
                    sb.AppendLine(String.Join(";", {LireTexte(r, "DateHeure"), LireTexte(r, "CodeAlarme"),
                                  LireTexte(r, "Description").Replace(";", ","), LireTexte(r, "Etat"),
                                  LireTexte(r, "TypeAlarme"), LireTexte(r, "DateResolution")}))
                Next
                IO.File.WriteAllText(sfd.FileName, sb.ToString(), System.Text.Encoding.UTF8)
                MessageBox.Show("Export terminé : " & sfd.FileName, "Export CSV", MessageBoxButtons.OK, MessageBoxIcon.Information)
            Catch ex As Exception
                MessageBox.Show("Erreur export : " & ex.Message)
            End Try
        End Using
    End Sub

    ' ═══════════════════════════════════════════════════════════════════
    '  IMPRESSION (liste complète, avec pagination)
    ' ═══════════════════════════════════════════════════════════════════

    Private Sub btnImprimerTout_Click(sender As Object, e As EventArgs) Handles btnImprimerTout.Click
        If DataGridView1.Rows.Count = 0 Then
            MessageBox.Show("Aucune donnée à imprimer.")
            Exit Sub
        End If
        pageImpressionCourante = 0
        apercuImpression.Document = docImpression
        apercuImpression.WindowState = FormWindowState.Maximized
        apercuImpression.ShowDialog()
    End Sub

    Private Sub docImpression_PrintPage(sender As Object, e As PrintPageEventArgs)
        Dim g As Graphics = e.Graphics
        Dim titreFont As New Font("Arial", 14, FontStyle.Bold)
        Dim enteteFont As New Font("Arial", 9, FontStyle.Bold)
        Dim txtFont As New Font("Arial", 9)
        Dim x As Integer = 40
        Dim y As Integer = 40
        Dim ligneHauteur As Integer = 20
        Dim limiteBas As Integer = e.MarginBounds.Bottom

        g.DrawString("JOURNAL DES ALARMES MACHINE", titreFont, Brushes.Black, x, y)
        g.DrawString("Période : " & dtDebut.Value.ToString("dd/MM/yyyy") & " au " & dtFin.Value.ToString("dd/MM/yyyy"),
                     New Font("Arial", 8, FontStyle.Italic), Brushes.Gray, x, y + 22)
        y += 55

        g.DrawString("Date/Heure", enteteFont, Brushes.Black, x, y)
        g.DrawString("Code", enteteFont, Brushes.Black, x + 130, y)
        g.DrawString("Description", enteteFont, Brushes.Black, x + 210, y)
        g.DrawString("Type", enteteFont, Brushes.Black, x + 470, y)
        g.DrawString("État", enteteFont, Brushes.Black, x + 570, y)
        y += ligneHauteur
        g.DrawLine(Pens.Black, x, y, x + 700, y)
        y += 6

        Dim i As Integer = pageImpressionCourante
        While i < DataGridView1.Rows.Count
            Dim row As DataGridViewRow = DataGridView1.Rows(i)
            If row.IsNewRow Then
                i += 1
                Continue While
            End If

            If y + ligneHauteur > limiteBas Then
                pageImpressionCourante = i
                e.HasMorePages = True
                Return
            End If

            Dim dateH As String = If(IsDBNull(row.Cells("DateHeure").Value), "", Convert.ToDateTime(row.Cells("DateHeure").Value).ToString("dd/MM/yyyy HH:mm"))
            g.DrawString(dateH, txtFont, Brushes.Black, x, y)
            g.DrawString(LireTexte(row, "CodeAlarme"), txtFont, Brushes.Black, x + 130, y)
            g.DrawString(LireTexte(row, "Description"), txtFont, Brushes.Black, x + 210, y)
            g.DrawString(LireTexte(row, "TypeAlarme"), txtFont, Brushes.Black, x + 470, y)
            g.DrawString(LireTexte(row, "Etat"), txtFont, Brushes.Black, x + 570, y)

            y += ligneHauteur
            i += 1
        End While

        pageImpressionCourante = 0
        e.HasMorePages = False
    End Sub

    Private Function LireTexte(row As DataGridViewRow, col As String) As String
        Try
            If Not row.DataGridView.Columns.Contains(col) Then Return ""
            Dim v As Object = row.Cells(col).Value
            If v Is Nothing OrElse IsDBNull(v) Then Return ""
            If TypeOf v Is Date Then Return CDate(v).ToString("dd/MM/yyyy HH:mm:ss")
            Return v.ToString()
        Catch
            Return ""
        End Try
    End Function
    ' ═══════════════════════════════════════════════════════════════════
    '  +/- SUR CHAQUE DATE INDEPENDAMMENT
    ' ═══════════════════════════════════════════════════════════════════

    Private Sub btnDebutMoins_Click(sender As Object, e As EventArgs) Handles btnDebutMoins.Click
        dtDebut.Value = dtDebut.Value.AddDays(-1)
        ChargerAlarmesParPeriode()
    End Sub

    Private Sub btnDebutPlus_Click(sender As Object, e As EventArgs) Handles btnDebutPlus.Click
        dtDebut.Value = dtDebut.Value.AddDays(1)
        ChargerAlarmesParPeriode()
    End Sub

    Private Sub btnFinMoins_Click(sender As Object, e As EventArgs) Handles btnFinMoins.Click
        dtFin.Value = dtFin.Value.AddDays(-1)
        ChargerAlarmesParPeriode()
    End Sub

    Private Sub btnFinPlus_Click(sender As Object, e As EventArgs) Handles btnFinPlus.Click
        dtFin.Value = dtFin.Value.AddDays(1)
        ChargerAlarmesParPeriode()
    End Sub



End Class