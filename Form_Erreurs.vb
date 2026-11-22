Imports System.Windows.Forms
Imports Microsoft.Data.SqlClient

' ================================================================
'  Form_Erreurs.vb — 18/09/2026
'  Liste des erreurs du logiciel : lignes ALARME du JournalActions
'  + dernière erreur de communication CPU (PLCData.DerniereErreur).
'  Ouverture : Form_Erreurs.Afficher(Me)   (bouton « Erreurs » du Synoptique)
'  Form_Erreurs.NbNouvelles(depuis) : nombre d'ALARMES depuis une date
'  (utilisé par le Synoptique pour colorer le bouton en rouge).
' ================================================================
Public Class Form_Erreurs
    Inherits Form

    Private Shared _instance As Form_Erreurs = Nothing
    ' Date/heure du dernier « Effacer » : les alarmes plus anciennes ne colorent plus le bouton
    Public Shared DerniereConsultation As Date = Now

    Public Shared Sub Afficher(proprietaire As Form)
        If _instance Is Nothing OrElse _instance.IsDisposed Then _instance = New Form_Erreurs()
        _instance.Owner = proprietaire
        _instance.Show()
        _instance.BringToFront()
        _instance.Charger()
    End Sub

    ' Nombre d'alarmes SQL depuis la dernière consultation (appelé en arrière-plan)
    Public Shared Function NbNouvelles() As Integer
        Try
            Dim o As Object = ModuleDb.LireValeur(Of Object)(
                "SELECT COUNT(*) FROM dbo.JournalActions WHERE TypeAction='ALARME' AND DateHeure > @d",
                Sub(cmd) cmd.Parameters.AddWithValue("@d", DerniereConsultation))
            If o Is Nothing OrElse IsDBNull(o) Then Return 0
            Return CInt(o)
        Catch
            Return 0
        End Try
    End Function

    Private WithEvents grid As New DataGridView()
    Private WithEvents btnEffacer As New Button()
    Private WithEvents btnActualiser As New Button()
    Private WithEvents btnFermer As New Button()
    Private lblCPU As New Label()
    Private lblInfo As New Label()

    Private Sub New()
        Me.Text = "Erreurs du logiciel"
        Me.Size = New Size(1000, 600)
        Me.StartPosition = FormStartPosition.CenterScreen
        Me.Font = New Font("Segoe UI", 9)
        Me.BackColor = Color.White

        Dim pnl As New Panel With {.Dock = DockStyle.Top, .Height = 78, .BackColor = Color.FromArgb(11, 61, 92)}
        btnActualiser.Text = "Actualiser" : btnActualiser.Location = New Point(10, 8) : btnActualiser.Size = New Size(110, 30)
        btnEffacer.Text = "Effacer (vu)" : btnEffacer.Location = New Point(130, 8) : btnEffacer.Size = New Size(120, 30)
        btnFermer.Text = "Fermer" : btnFermer.Location = New Point(260, 8) : btnFermer.Size = New Size(90, 30)
        For Each b In {btnActualiser, btnEffacer, btnFermer}
            b.FlatStyle = FlatStyle.Flat : b.FlatAppearance.BorderSize = 0
            b.BackColor = Color.FromArgb(0, 120, 215) : b.ForeColor = Color.White
            b.Font = New Font("Segoe UI", 9, FontStyle.Bold) : b.Cursor = Cursors.Hand
        Next
        btnEffacer.BackColor = Color.FromArgb(0, 150, 100)
        btnFermer.BackColor = Color.FromArgb(180, 50, 50)
        lblInfo.AutoSize = True : lblInfo.ForeColor = Color.White : lblInfo.Location = New Point(370, 14)
        lblCPU.AutoSize = True : lblCPU.ForeColor = Color.Yellow : lblCPU.Location = New Point(10, 48)
        lblCPU.Font = New Font("Consolas", 9, FontStyle.Bold)
        pnl.Controls.AddRange({btnActualiser, btnEffacer, btnFermer, lblInfo, lblCPU})

        grid.Dock = DockStyle.Fill
        grid.ReadOnly = True
        grid.AllowUserToAddRows = False
        grid.AllowUserToDeleteRows = False
        grid.RowHeadersVisible = False
        grid.SelectionMode = DataGridViewSelectionMode.FullRowSelect
        grid.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill
        grid.BackgroundColor = Color.White
        grid.EnableHeadersVisualStyles = False
        grid.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(11, 61, 92)
        grid.ColumnHeadersDefaultCellStyle.ForeColor = Color.White
        grid.ColumnHeadersDefaultCellStyle.Font = New Font("Segoe UI", 9, FontStyle.Bold)
        grid.DefaultCellStyle.Font = New Font("Consolas", 9)
        grid.DefaultCellStyle.WrapMode = DataGridViewTriState.True
        grid.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells

        Me.Controls.Add(grid)
        Me.Controls.Add(pnl)
        grid.BringToFront()
    End Sub

    Private Sub Charger()
        Try
            lblCPU.Text = "CPU : " & If(PLCData.Connecte, "connecté", "NON CONNECTÉ") &
                          If(String.IsNullOrEmpty(PLCData.DerniereErreur), "", "   |   dernière erreur : " & PLCData.DerniereErreur)
            Dim dt As System.Data.DataTable = ModuleDb.Lire(
                "SELECT TOP 300 DateHeure AS [Date/heure], Module, Action, Detail AS Détail, Adresse_PLC AS Adresse, Operateur AS Opérateur " &
                "FROM dbo.JournalActions WHERE TypeAction='ALARME' ORDER BY DateHeure DESC")
            grid.DataSource = dt
            If grid.Columns.Contains("Date/heure") Then
                grid.Columns("Date/heure").DefaultCellStyle.Format = "dd/MM HH:mm:ss"
                grid.Columns("Date/heure").FillWeight = 12
                grid.Columns("Module").FillWeight = 10
                grid.Columns("Action").FillWeight = 25
                grid.Columns("Détail").FillWeight = 38
                grid.Columns("Adresse").FillWeight = 8
                grid.Columns("Opérateur").FillWeight = 7
            End If
            For Each r As DataGridViewRow In grid.Rows
                Dim d As Date = CDate(r.Cells(0).Value)
                If d > DerniereConsultation Then r.DefaultCellStyle.BackColor = Color.FromArgb(255, 225, 225)
            Next
            lblInfo.Text = dt.Rows.Count & " alarme(s) (300 dernières) — nouvelles depuis " & DerniereConsultation.ToString("dd/MM HH:mm") & " en rouge"
        Catch ex As Exception
            lblInfo.Text = "Erreur lecture journal : " & ex.Message
        End Try
    End Sub

    Private Sub btnActualiser_Click(sender As Object, e As EventArgs) Handles btnActualiser.Click
        Charger()
    End Sub

    Private Sub btnEffacer_Click(sender As Object, e As EventArgs) Handles btnEffacer.Click
        DerniereConsultation = Now
        Charger()
    End Sub

    Private Sub btnFermer_Click(sender As Object, e As EventArgs) Handles btnFermer.Click
        Me.Close()
    End Sub

End Class
