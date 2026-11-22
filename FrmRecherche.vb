Imports Microsoft.Data.SqlClient
Imports System.Data

Public Class FrmRecherche

    Public ValeurSelectionnee As String = ""
    Public IdSelectionne As Integer = 0

    Private _sqlBase As String = ""
    Private _colonneId As String = ""
    Private _colonneCode As String = ""
    Private _colonneNom As String = ""

    ' ═══════════════════════════════════════════════════════════════════
    '  INITIALISER
    ' ═══════════════════════════════════════════════════════════════════

    Public Sub Initialiser(sqlBase As String, colonneId As String,
                           colonneCode As String, colonneNom As String)
        _sqlBase = sqlBase
        _colonneId = colonneId
        _colonneCode = colonneCode
        _colonneNom = colonneNom
    End Sub

    ' ═══════════════════════════════════════════════════════════════════
    '  LOAD
    ' ═══════════════════════════════════════════════════════════════════

    Private Sub FrmRecherche_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        txtRechercheCode.Text = ""
        txtRechercheNom.Text = ""
        ChargerDonnees("", "")
    End Sub

    ' ═══════════════════════════════════════════════════════════════════
    '  CHARGEMENT AVEC FILTRE
    ' ═══════════════════════════════════════════════════════════════════

    Private Sub ChargerDonnees(filtreCode As String, filtreNom As String)
        Try
            Dim sql As String = _sqlBase
            Dim conditions As New List(Of String)
            If filtreCode.Trim <> "" Then conditions.Add(_colonneCode & " LIKE @FiltreCode")
            If filtreNom.Trim <> "" Then conditions.Add(_colonneNom & " LIKE @FiltreNom")
            If conditions.Count > 0 Then
                sql &= " AND (" & String.Join(" OR ", conditions) & ")"
            End If

            Dim dt As DataTable = ModuleDb.Lire(sql,
                Sub(cmd)
                    If filtreCode.Trim <> "" Then
                        cmd.Parameters.Add("@FiltreCode", SqlDbType.NVarChar, 200).Value = "%" & filtreCode & "%"
                    End If
                    If filtreNom.Trim <> "" Then
                        cmd.Parameters.Add("@FiltreNom", SqlDbType.NVarChar, 200).Value = "%" & filtreNom & "%"
                    End If
                End Sub)

            DataGridView1.DataSource = dt
            DataGridView1.ReadOnly = True
            DataGridView1.AllowUserToAddRows = False
            DataGridView1.SelectionMode = DataGridViewSelectionMode.FullRowSelect
            DataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill
            lblNombre.Text = dt.Rows.Count.ToString() & " enregistrement(s)"

        Catch ex As Exception
            MessageBox.Show("Erreur : " & ex.Message)
        End Try
    End Sub

    ' ═══════════════════════════════════════════════════════════════════
    '  RECHERCHE EN TEMPS RÉEL
    ' ═══════════════════════════════════════════════════════════════════

    Private Sub txtRechercheCode_TextChanged(sender As Object, e As EventArgs) Handles txtRechercheCode.TextChanged
        ChargerDonnees(txtRechercheCode.Text.Trim, txtRechercheNom.Text.Trim)
    End Sub

    Private Sub txtRechercheNom_TextChanged(sender As Object, e As EventArgs) Handles txtRechercheNom.TextChanged
        ChargerDonnees(txtRechercheCode.Text.Trim, txtRechercheNom.Text.Trim)
    End Sub

    ' ═══════════════════════════════════════════════════════════════════
    '  SÉLECTION
    ' ═══════════════════════════════════════════════════════════════════

    Private Sub DataGridView1_CellDoubleClick(sender As Object, e As DataGridViewCellEventArgs) Handles DataGridView1.CellDoubleClick
        If e.RowIndex >= 0 Then SelectionnerLigne()
    End Sub

    Private Sub btnSelectionner_Click(sender As Object, e As EventArgs) Handles btnSelectionner.Click
        If DataGridView1.CurrentRow Is Nothing Then
            MessageBox.Show("Sélectionner une ligne.")
            Exit Sub
        End If
        SelectionnerLigne()
    End Sub

    Private Sub SelectionnerLigne()
        Try
            Dim row As DataGridViewRow = DataGridView1.CurrentRow
            IdSelectionne = CInt(row.Cells(_colonneId).Value)
            ValeurSelectionnee = row.Cells(_colonneCode).Value.ToString()
            Me.DialogResult = DialogResult.OK
            Me.Close()
        Catch ex As Exception
            MessageBox.Show("Erreur sélection : " & ex.Message)
        End Try
    End Sub

    ' ═══════════════════════════════════════════════════════════════════
    '  ANNULER / ENTER
    ' ═══════════════════════════════════════════════════════════════════

    Private Sub btnAnnuler_Click(sender As Object, e As EventArgs) Handles btnAnnuler.Click
        Me.DialogResult = DialogResult.Cancel
        Me.Close()
    End Sub

    Private Sub txtRechercheCode_KeyPress(sender As Object, e As KeyPressEventArgs) Handles txtRechercheCode.KeyPress, txtRechercheNom.KeyPress
        If e.KeyChar = Chr(13) AndAlso DataGridView1.CurrentRow IsNot Nothing Then
            SelectionnerLigne()
        End If
    End Sub

End Class