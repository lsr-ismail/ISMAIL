Imports System.Windows.Forms

' ================================================================
'  Form_TableDB71.vb — 16/09/2026
'  Affiche la table des gâchées DB71 du CPU comme la « page verte »
'  de WinCC : 40 emplacements × (compteur + pose / écart de chaque
'  matière), lue en direct, rafraîchie toutes les 3 s.
'  Bouton « Copier » : copie le texte brut (offsets + valeurs non
'  nulles) pour diagnostic.
'  Ouverture : Form_TableDB71.Afficher(Me)   (bouton du Synoptique)
' ================================================================
Public Class Form_TableDB71
    Inherits Form

    Private Shared _instance As Form_TableDB71 = Nothing

    Public Shared Sub Afficher(proprietaire As Form)
        If _instance Is Nothing OrElse _instance.IsDisposed Then _instance = New Form_TableDB71()
        _instance.Owner = proprietaire
        _instance.Show()
        _instance.BringToFront()
    End Sub

    Private WithEvents grid As New DataGridView()
    Private WithEvents timerMaj As New Timer()
    Private WithEvents btnActualiser As New Button()
    Private WithEvents btnCopier As New Button()
    Private WithEvents btnFermer As New Button()
    Private lblEtat As New Label()
    Private _lecture As Boolean = False

    ' Colonnes matières : (titre, offset pose, offset écart)
    Private ReadOnly _mat() As (String, Integer, Integer) = {
        ("G1", 0, 1600), ("SC", 160, 1760), ("SL", 320, 1920), ("D2", 480, 2080),
        ("CIM", 640, 3040), ("EAU", 800, 2880),
        ("ADJ1", 960, 2240), ("ADJ2", 1120, 2400), ("ADJ3", 1280, 2560), ("ADJ4", 1440, 2720)}

    Private Sub New()
        Me.Text = "Table des gâchées CPU — DB71"
        Me.Size = New Size(1180, 760)
        Me.StartPosition = FormStartPosition.CenterScreen
        Me.Font = New Font("Segoe UI", 9)
        Me.BackColor = Color.FromArgb(0, 128, 0)

        Dim pnl As New Panel With {.Dock = DockStyle.Top, .Height = 44, .BackColor = Color.FromArgb(11, 61, 92)}
        btnActualiser.Text = "Actualiser" : btnActualiser.Location = New Point(10, 7) : btnActualiser.Size = New Size(100, 30)
        btnCopier.Text = "Copier (diagnostic)" : btnCopier.Location = New Point(120, 7) : btnCopier.Size = New Size(150, 30)
        btnFermer.Text = "Fermer" : btnFermer.Location = New Point(280, 7) : btnFermer.Size = New Size(90, 30)
        For Each b In {btnActualiser, btnCopier, btnFermer}
            b.FlatStyle = FlatStyle.Flat : b.FlatAppearance.BorderSize = 0
            b.BackColor = Color.FromArgb(0, 120, 215) : b.ForeColor = Color.White
            b.Font = New Font("Segoe UI", 9, FontStyle.Bold) : b.Cursor = Cursors.Hand
        Next
        btnFermer.BackColor = Color.FromArgb(180, 50, 50)
        lblEtat.AutoSize = True : lblEtat.ForeColor = Color.White : lblEtat.Location = New Point(390, 13)
        pnl.Controls.AddRange({btnActualiser, btnCopier, btnFermer, lblEtat})

        grid.Dock = DockStyle.Fill
        grid.Font = New Font("Segoe UI", 9, FontStyle.Bold)
        grid.RowHeadersVisible = False
        grid.AllowUserToAddRows = False
        grid.AllowUserToDeleteRows = False
        grid.ReadOnly = True
        grid.SelectionMode = DataGridViewSelectionMode.FullRowSelect
        grid.MultiSelect = False
        grid.BackgroundColor = Color.FromArgb(0, 128, 0)
        grid.GridColor = Color.FromArgb(0, 100, 0)
        grid.BorderStyle = BorderStyle.None
        grid.EnableHeadersVisualStyles = False
        grid.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(0, 100, 0)
        grid.ColumnHeadersDefaultCellStyle.ForeColor = Color.White
        grid.ColumnHeadersDefaultCellStyle.Font = New Font("Segoe UI", 9, FontStyle.Bold)
        grid.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter
        grid.DefaultCellStyle.BackColor = Color.FromArgb(0, 128, 0)
        grid.DefaultCellStyle.ForeColor = Color.Black
        grid.DefaultCellStyle.SelectionBackColor = Color.FromArgb(0, 150, 0)
        grid.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter
        grid.DefaultCellStyle.WrapMode = DataGridViewTriState.True
        grid.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.None
        grid.RowTemplate.Height = 34
        grid.ScrollBars = ScrollBars.Both

        AjouterColonne("N°", 45)
        AjouterColonne("Compteur", 80)
        For Each m In _mat
            AjouterColonne(m.Item1 & vbLf & "pose / écart", 95)
        Next

        Me.Controls.Add(grid)
        Me.Controls.Add(pnl)
        grid.BringToFront()

        For k As Integer = 1 To ModuleGacheesDB71.NB_GACHEES
            Dim idx As Integer = grid.Rows.Add()
            grid.Rows(idx).Cells(0).Value = k
        Next
        grid.Rows(0).Cells(0).Style.BackColor = Color.FromArgb(0, 128, 0)

        timerMaj.Interval = 3000
    End Sub

    Private Sub AjouterColonne(titre As String, largeur As Integer)
        Dim c As New DataGridViewTextBoxColumn With {.HeaderText = titre, .Width = largeur, .SortMode = DataGridViewColumnSortMode.NotSortable}
        grid.Columns.Add(c)
    End Sub

    Private Sub Form_TableDB71_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Lire()
        timerMaj.Start()
    End Sub

    Private Sub Form_TableDB71_FormClosed(sender As Object, e As FormClosedEventArgs) Handles MyBase.FormClosed
        timerMaj.Stop()
    End Sub

    Private Sub timerMaj_Tick(sender As Object, e As EventArgs) Handles timerMaj.Tick
        Lire()
    End Sub

    Private Sub btnActualiser_Click(sender As Object, e As EventArgs) Handles btnActualiser.Click
        Lire()
    End Sub

    Private Sub btnFermer_Click(sender As Object, e As EventArgs) Handles btnFermer.Click
        Me.Close()
    End Sub

    ' ── Lecture DB71 hors thread écran, puis affichage ─────────
    Private Sub Lire()
        If _lecture Then Exit Sub
        _lecture = True
        Threading.ThreadPool.QueueUserWorkItem(
            Sub(state)
                Dim db() As Byte = Nothing
                Dim err As String = ""
                Try
                    db = ModulePLCDuplex.LireBytesDB(ModuleGacheesDB71.DB_GACHEES, 0, 3360)
                    If db Is Nothing Then err = PLCData.DerniereErreur
                Catch ex As Exception
                    err = ex.Message
                End Try
                Try
                    If Me.IsHandleCreated AndAlso Not Me.IsDisposed Then
                        Me.BeginInvoke(Sub() AfficherDonnees(db, err))
                    End If
                Catch
                End Try
                _lecture = False
            End Sub)
    End Sub

    Private Sub AfficherDonnees(db() As Byte, err As String)
        If db Is Nothing Then
            lblEtat.Text = "Lecture DB71 impossible : " & err
            Exit Sub
        End If
        grid.SuspendLayout()
        For k As Integer = 0 To ModuleGacheesDB71.NB_GACHEES - 1
            Dim r As DataGridViewRow = grid.Rows(k)
            Dim cpt As Long = Dint(db, 3200 + k * 4)
            r.Cells(1).Value = cpt
            For j As Integer = 0 To _mat.Length - 1
                Dim pose As Single = Reel(db, _mat(j).Item2 + k * 4)
                Dim ecart As Single = Reel(db, _mat(j).Item3 + k * 4)
                Dim fmt As String = If(j >= 6, "0.000", "0.0")
                r.Cells(2 + j).Value = pose.ToString(fmt) & vbLf & ecart.ToString(fmt)
                r.Cells(2 + j).Style.ForeColor = If(cpt > 0, Color.Black, Color.FromArgb(200, 255, 200))
            Next
            r.Cells(1).Style.ForeColor = If(cpt > 0, Color.Black, Color.FromArgb(200, 255, 200))
        Next
        grid.ResumeLayout()
        lblEtat.Text = "DB71 lue à " & Now.ToString("HH:mm:ss") & "   (cycle PLC " & PLCData.DureeCycleMs & " ms)"
    End Sub

    ' ── Copier le diagnostic brut (offsets + REAL non nuls) ─────
    Private Sub btnCopier_Click(sender As Object, e As EventArgs) Handles btnCopier.Click
        Try
            Dim txt As String = ModuleGacheesDB71.Diagnostic(5)
            Clipboard.SetText(txt)
            MessageBox.Show("Diagnostic copié dans le presse-papiers." & vbCrLf & "Collez-le (Ctrl+V) dans le chat.", "Copier", MessageBoxButtons.OK, MessageBoxIcon.Information)
        Catch ex As Exception
            MessageBox.Show("Erreur : " & ex.Message)
        End Try
    End Sub

    ' ── Helpers Big Endian ─────────────────────────────────────
    Private Function Reel(b() As Byte, off As Integer) As Single
        If b Is Nothing OrElse off + 3 >= b.Length Then Return 0
        Dim t() As Byte = {b(off + 3), b(off + 2), b(off + 1), b(off)}
        Dim v As Single = BitConverter.ToSingle(t, 0)
        If Single.IsNaN(v) OrElse Single.IsInfinity(v) Then Return 0
        Return v
    End Function

    Private Function Dint(b() As Byte, off As Integer) As Long
        If b Is Nothing OrElse off + 3 >= b.Length Then Return 0
        Return CLng(CInt(b(off)) << 24 Or CInt(b(off + 1)) << 16 Or CInt(b(off + 2)) << 8 Or CInt(b(off + 3)))
    End Function

End Class
