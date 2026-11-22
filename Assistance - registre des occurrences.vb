Imports System.Data
Imports System.Drawing

Public Class Assistance___registre_des_occurrences

    Private Sub Assistance___registre_des_occurrences_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        StyleGrille()
        dtpDebut.Value = DateTime.Today
        dtpFin.Value = DateTime.Today
        ChargerJournal()
    End Sub

    ' ═══════════════════════════════════════════════════════════════
    '  STYLE DU TABLEAU
    ' ═══════════════════════════════════════════════════════════════
    Private Sub StyleGrille()
        With DataGridView1
            .ReadOnly = True
            .AllowUserToAddRows = False
            .AllowUserToDeleteRows = False
            .SelectionMode = DataGridViewSelectionMode.FullRowSelect
            .MultiSelect = False
            .RowHeadersVisible = False
            .EnableHeadersVisualStyles = False
            .AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill
            .ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(11, 61, 92)
            .ColumnHeadersDefaultCellStyle.ForeColor = Color.White
            .ColumnHeadersDefaultCellStyle.Font = New Font("Segoe UI", 9, FontStyle.Bold)
            .DefaultCellStyle.SelectionBackColor = Color.RoyalBlue
            .RowTemplate.Height = 26
            .BackgroundColor = Color.White
            .Font = New Font("Segoe UI", 8.5)
            .AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(240, 245, 250)
        End With
    End Sub

    ' ═══════════════════════════════════════════════════════════════
    '  CHARGEMENT DU JOURNAL (periode Debut->Fin + recherche libre)
    ' ═══════════════════════════════════════════════════════════════
    Private Sub ChargerJournal()
        Try
            If dtpDebut.Value.Date > dtpFin.Value.Date Then
                MessageBox.Show("La date de début doit être avant la date de fin.")
                Exit Sub
            End If

            Dim sql As String =
                "SELECT " &
                "  CONVERT(VARCHAR(19), DateHeure, 120) AS DateHeure," &
                "  ISNULL(Operateur,'-')   AS Operateur," &
                "  ISNULL(Module,'-')      AS Module," &
                "  ISNULL(Action,'-')      AS Action," &
                "  ISNULL(Detail,'')       AS Detail," &
                "  ISNULL(Adresse_PLC,'')  AS [Adr. PLC]," &
                "  ISNULL(Valeur_PLC,'')   AS [Val. PLC]," &
                "  ISNULL(TypeAction,'')   AS Type " &
                "FROM dbo.JournalActions " &
                "WHERE CAST(DateHeure AS DATE) BETWEEN @debut AND @fin "

            If txtRECHERCHE.Text.Trim() <> "" Then
                sql &= " AND (Operateur LIKE @mot OR Action LIKE @mot OR Detail LIKE @mot OR Module LIKE @mot) "
            End If

            sql &= "ORDER BY DateHeure DESC"

            Dim dt As DataTable = ModuleDb.Lire(sql,
                Sub(cmd)
                    cmd.Parameters.Add("@debut", SqlDbType.Date).Value = dtpDebut.Value.Date
                    cmd.Parameters.Add("@fin", SqlDbType.Date).Value = dtpFin.Value.Date
                    If txtRECHERCHE.Text.Trim() <> "" Then
                        cmd.Parameters.Add("@mot", SqlDbType.NVarChar, 200).Value = "%" & txtRECHERCHE.Text.Trim() & "%"
                    End If
                End Sub)

            DataGridView1.DataSource = dt
            DataGridView1.Columns(0).FillWeight = 170
            Me.Text = "Registre des occurrences (" & dt.Rows.Count & " ligne(s))"

        Catch ex As Exception
            MessageBox.Show("Erreur chargement journal : " & ex.Message)
        End Try
    End Sub

    ' ═══════════════════════════════════════════════════════════════
    '  RECHERCHE
    ' ═══════════════════════════════════════════════════════════════
    Private Sub RECHERCHE_Click(sender As Object, e As EventArgs) Handles RECHERCHE.Click
        ChargerJournal()
    End Sub

    Private Sub txtRECHERCHE_KeyDown(sender As Object, e As KeyEventArgs) Handles txtRECHERCHE.KeyDown
        If e.KeyCode = Keys.Enter Then
            ChargerJournal()
            e.SuppressKeyPress = True
        End If
    End Sub

    ' ═══════════════════════════════════════════════════════════════
    '  BOUTONS +/- SUR LES DATES (Debut / Fin)
    ' ═══════════════════════════════════════════════════════════════
    Private Sub btnDebutMoins_Click(sender As Object, e As EventArgs) Handles btnDebutMoins.Click
        dtpDebut.Value = dtpDebut.Value.AddDays(-1)
        ChargerJournal()
    End Sub

    Private Sub btnDebutPlus_Click(sender As Object, e As EventArgs) Handles btnDebutPlus.Click
        dtpDebut.Value = dtpDebut.Value.AddDays(1)
        ChargerJournal()
    End Sub

    Private Sub btnFinMoins_Click(sender As Object, e As EventArgs) Handles btnFinMoins.Click
        dtpFin.Value = dtpFin.Value.AddDays(-1)
        ChargerJournal()
    End Sub

    Private Sub btnFinPlus_Click(sender As Object, e As EventArgs) Handles btnfinPlus.Click
        dtpFin.Value = dtpFin.Value.AddDays(1)
        ChargerJournal()
    End Sub

    Private Sub dtpDebut_ValueChanged(sender As Object, e As EventArgs) Handles dtpDebut.ValueChanged
        ChargerJournal()
    End Sub

    Private Sub dtpFin_ValueChanged(sender As Object, e As EventArgs) Handles dtpFin.ValueChanged
        ChargerJournal()
    End Sub

End Class