Imports Microsoft.Data.SqlClient
Imports System.Data

Public Class TVA

    Private _idTVACourant As Integer = 0

    ' ═══════════════════════════════════════════════════════════════════
    '  LOAD
    ' ═══════════════════════════════════════════════════════════════════

    Private Sub TVA_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        DataGridView1.RowHeadersVisible = False   ' NOUVEAU
        'txtTVA_ID.Visible = False

        ' NOUVEAU : gestion des rôles
        Select Case RoleUtilisateur
            Case "Admin", "Super"
                btnAjouter.Visible = True : btnAjouter.Enabled = True
                btnModifier.Visible = True : btnModifier.Enabled = True
                btnSupprimer.Visible = True : btnSupprimer.Enabled = True
            Case "Operateur"
                btnAjouter.Visible = True : btnAjouter.Enabled = False
                btnModifier.Visible = True : btnModifier.Enabled = False
                btnSupprimer.Visible = True : btnSupprimer.Enabled = False
            Case Else
                btnAjouter.Visible = True : btnAjouter.Enabled = False
                btnModifier.Visible = True : btnModifier.Enabled = False
                btnSupprimer.Visible = True : btnSupprimer.Enabled = False
        End Select

        ChargerTVA()
    End Sub

    ' ═══════════════════════════════════════════════════════════════════
    '  CHARGEMENT
    ' ═══════════════════════════════════════════════════════════════════

    Private Sub ChargerTVA()
        Try
            Dim dt As DataTable = ModuleDb.Lire(
            "SELECT TVA_ID, TVA_Code, TVA_Cle, TVA_Nom, " &
            "TVA_Montant1, TVA_Montant2, " &
            "TVA_DateTarif1, TVA_DateTarif2, " &
            "TVA_Flag_Interdiction, TVA_DateModification " &
            "FROM dbo.T_TVA ORDER BY TVA_Montant1")

            DataGridView1.DataSource = dt
            DataGridView1.ReadOnly = True
            DataGridView1.AllowUserToAddRows = False
            DataGridView1.SelectionMode = DataGridViewSelectionMode.FullRowSelect

            ' NOUVEAU : cacher toutes les colonnes puis ne montrer que 3
            For Each col As DataGridViewColumn In DataGridView1.Columns
                col.Visible = False
            Next
            DataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.None

            If DataGridView1.Columns.Contains("TVA_Code") Then
                DataGridView1.Columns("TVA_Code").Visible = True
                DataGridView1.Columns("TVA_Code").HeaderText = "Code"
                DataGridView1.Columns("TVA_Code").Width = 100
            End If
            If DataGridView1.Columns.Contains("TVA_Nom") Then
                DataGridView1.Columns("TVA_Nom").Visible = True
                DataGridView1.Columns("TVA_Nom").HeaderText = "Désignation"
                DataGridView1.Columns("TVA_Nom").Width = 250
            End If
            If DataGridView1.Columns.Contains("TVA_Montant1") Then
                DataGridView1.Columns("TVA_Montant1").Visible = True
                DataGridView1.Columns("TVA_Montant1").HeaderText = "Taux %"
                DataGridView1.Columns("TVA_Montant1").Width = 100
            End If

            lblNombre.Text = "Total : " & dt.Rows.Count & " taux TVA"

        Catch ex As Exception
            MessageBox.Show("Erreur chargement : " & ex.Message)
        End Try
    End Sub
    ' ═══════════════════════════════════════════════════════════════════
    '  CLIC GRILLE
    ' ═══════════════════════════════════════════════════════════════════

    Private Sub DataGridView1_CellClick(sender As Object, e As DataGridViewCellEventArgs) Handles DataGridView1.CellClick
        If e.RowIndex < 0 Then Exit Sub
        Try
            Dim row As DataGridViewRow = DataGridView1.Rows(e.RowIndex)
            _idTVACourant = CInt(row.Cells("TVA_ID").Value)

            txtTVA_ID.Text = _idTVACourant.ToString()
            txtTVA_Code.Text = LireTexte(row, "TVA_Code")
            txtTVA_Cle.Text = LireTexte(row, "TVA_Cle")
            txtTVA_Nom.Text = LireTexte(row, "TVA_Nom")
            txtTVA_Montant1.Text = LireTexte(row, "TVA_Montant1")
            txtTVA_Montant2.Text = LireTexte(row, "TVA_Montant2")

            If Not IsDBNull(row.Cells("TVA_DateTarif1").Value) Then
                DateTimePicker1.Value = Convert.ToDateTime(row.Cells("TVA_DateTarif1").Value)
            End If
            If Not IsDBNull(row.Cells("TVA_DateTarif2").Value) Then
                DateTimePicker2.Value = Convert.ToDateTime(row.Cells("TVA_DateTarif2").Value)
            End If

            chkInterdiction.Checked = Not IsDBNull(row.Cells("TVA_Flag_Interdiction").Value) AndAlso
                                      Convert.ToBoolean(row.Cells("TVA_Flag_Interdiction").Value)
        Catch ex As Exception
            MessageBox.Show("Erreur sélection : " & ex.Message)
        End Try
    End Sub

    ' ═══════════════════════════════════════════════════════════════════
    '  AJOUTER
    ' ═══════════════════════════════════════════════════════════════════

    Private Sub btnAjouter_Click(sender As Object, e As EventArgs) Handles btnAjouter.Click
        If txtTVA_Code.Text.Trim = "" OrElse txtTVA_Nom.Text.Trim = "" Then
            MessageBox.Show("Code et Désignation sont obligatoires.")
            Exit Sub
        End If
        Try
            ModuleDb.Executer(
                "INSERT INTO dbo.T_TVA " &
                "(TVA_Code, TVA_Cle, TVA_Nom, TVA_DateTarif1, TVA_DateTarif2, " &
                "TVA_Montant1, TVA_Montant2, TVA_Flag_Interdiction, TVA_DateModification) " &
                "VALUES (@TVA_Code, @TVA_Cle, @TVA_Nom, @TVA_DateTarif1, @TVA_DateTarif2, " &
                "@TVA_Montant1, @TVA_Montant2, @TVA_Flag_Interdiction, GETDATE())",
                AddressOf AjouterParametres)

            MessageBox.Show("TVA ajoutée avec succès.")
            ChargerTVA()
            ViderChamps()

        Catch ex As Exception
            MessageBox.Show("Erreur ajout : " & ex.Message)
        End Try
    End Sub

    ' ═══════════════════════════════════════════════════════════════════
    '  MODIFIER
    ' ═══════════════════════════════════════════════════════════════════

    Private Sub btnModifier_Click(sender As Object, e As EventArgs) Handles btnModifier.Click
        If _idTVACourant = 0 Then
            MessageBox.Show("Sélectionner une TVA à modifier.")
            Exit Sub
        End If
        Try
            ModuleDb.Executer(
                "UPDATE dbo.T_TVA SET " &
                "TVA_Code=@TVA_Code, TVA_Cle=@TVA_Cle, TVA_Nom=@TVA_Nom, " &
                "TVA_DateTarif1=@TVA_DateTarif1, TVA_DateTarif2=@TVA_DateTarif2, " &
                "TVA_Montant1=@TVA_Montant1, TVA_Montant2=@TVA_Montant2, " &
                "TVA_Flag_Interdiction=@TVA_Flag_Interdiction, " &
                "TVA_DateModification=GETDATE() " &
                "WHERE TVA_ID=@TVA_ID",
                Sub(cmd)
                    cmd.Parameters.Add("@TVA_ID", SqlDbType.Int).Value = _idTVACourant
                    AjouterParametres(cmd)
                End Sub)

            MessageBox.Show("TVA modifiée avec succès.")
            ChargerTVA()
            ViderChamps()

        Catch ex As Exception
            MessageBox.Show("Erreur modification : " & ex.Message)
        End Try
    End Sub

    ' ═══════════════════════════════════════════════════════════════════
    '  SUPPRIMER
    ' ═══════════════════════════════════════════════════════════════════

    Private Sub btnSupprimer_Click(sender As Object, e As EventArgs) Handles btnSupprimer.Click
        If _idTVACourant = 0 Then
            MessageBox.Show("Sélectionner une TVA à supprimer.")
            Exit Sub
        End If
        If MessageBox.Show("Voulez-vous supprimer cette TVA ?", "Confirmation",
                           MessageBoxButtons.YesNo, MessageBoxIcon.Question) = DialogResult.No Then Exit Sub
        Try
            ModuleDb.Executer(
                "DELETE FROM dbo.T_TVA WHERE TVA_ID=@TVA_ID",
                Sub(cmd)
                    cmd.Parameters.Add("@TVA_ID", SqlDbType.Int).Value = _idTVACourant
                End Sub)

            MessageBox.Show("TVA supprimée avec succès.")
            ChargerTVA()
            ViderChamps()

        Catch ex As Exception
            MessageBox.Show("Erreur suppression : " & ex.Message)
        End Try
    End Sub

    ' ═══════════════════════════════════════════════════════════════════
    '  RECHERCHE (temps réel)
    ' ═══════════════════════════════════════════════════════════════════

    Private Sub txtRECHERCHE_TextChanged(sender As Object, e As EventArgs) Handles txtRECHERCHE.TextChanged
        Try
            Dim dt As DataTable = ModuleDb.Lire(
                "SELECT TVA_ID, TVA_Code, TVA_Cle, TVA_Nom, " &
                "TVA_Montant1, TVA_Montant2, TVA_DateTarif1, TVA_DateTarif2, " &
                "TVA_Flag_Interdiction, TVA_DateModification " &
                "FROM dbo.T_TVA WHERE TVA_Code LIKE @mot " &
                "OR TVA_Nom LIKE @mot ORDER BY TVA_Montant1",
                Sub(cmd)
                    cmd.Parameters.Add("@mot", SqlDbType.NVarChar, 200).Value = "%" & txtRECHERCHE.Text.Trim & "%"
                End Sub)

            DataGridView1.DataSource = dt
            DataGridView1.DataSource = dt

            For Each col As DataGridViewColumn In DataGridView1.Columns
                col.Visible = False
            Next
            DataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.None

            If DataGridView1.Columns.Contains("TVA_Code") Then
                DataGridView1.Columns("TVA_Code").Visible = True
                DataGridView1.Columns("TVA_Code").HeaderText = "Code"
                DataGridView1.Columns("TVA_Code").Width = 100
            End If
            If DataGridView1.Columns.Contains("TVA_Nom") Then
                DataGridView1.Columns("TVA_Nom").Visible = True
                DataGridView1.Columns("TVA_Nom").HeaderText = "Désignation"
                DataGridView1.Columns("TVA_Nom").Width = 250
            End If
            If DataGridView1.Columns.Contains("TVA_Montant1") Then
                DataGridView1.Columns("TVA_Montant1").Visible = True
                DataGridView1.Columns("TVA_Montant1").HeaderText = "Taux %"
                DataGridView1.Columns("TVA_Montant1").Width = 100
            End If

            lblNombre.Text = "Total : " & dt.Rows.Count & " taux TVA"
            If DataGridView1.Columns.Contains("TVA_ID") Then
                DataGridView1.Columns("TVA_ID").Visible = False
            End If
            lblNombre.Text = "Total : " & dt.Rows.Count & " taux TVA"

        Catch ex As Exception
            MessageBox.Show("Erreur recherche : " & ex.Message)
        End Try
    End Sub

    ' ═══════════════════════════════════════════════════════════════════
    '  NOUVEAU / ACTUALISER
    ' ═══════════════════════════════════════════════════════════════════

    Private Sub btnNouveau_Click(sender As Object, e As EventArgs) Handles btnNouveau.Click
        ViderChamps()
    End Sub

    Private Sub btnActualiser_Click(sender As Object, e As EventArgs) Handles btnActualiser.Click
        ChargerTVA()
    End Sub

    ' ═══════════════════════════════════════════════════════════════════
    '  HELPERS
    ' ═══════════════════════════════════════════════════════════════════

    Private Sub AjouterParametres(cmd As SqlCommand)
        cmd.Parameters.Add("@TVA_Code", SqlDbType.NVarChar, 50).Value = ValTexte(txtTVA_Code.Text)
        cmd.Parameters.Add("@TVA_Cle", SqlDbType.NVarChar, 50).Value = ValTexte(txtTVA_Cle.Text)
        cmd.Parameters.Add("@TVA_Nom", SqlDbType.NVarChar, 200).Value = ValTexte(txtTVA_Nom.Text)
        cmd.Parameters.Add("@TVA_DateTarif1", SqlDbType.Date).Value = DateTimePicker1.Value.Date
        cmd.Parameters.Add("@TVA_DateTarif2", SqlDbType.Date).Value = DateTimePicker2.Value.Date
        cmd.Parameters.Add("@TVA_Montant1", SqlDbType.Decimal).Value = ParseDecimal(txtTVA_Montant1.Text)
        cmd.Parameters.Add("@TVA_Montant2", SqlDbType.Decimal).Value = ParseDecimal(txtTVA_Montant2.Text)
        cmd.Parameters.Add("@TVA_Flag_Interdiction", SqlDbType.Bit).Value = chkInterdiction.Checked
    End Sub

    Private Function LireTexte(row As DataGridViewRow, col As String) As String
        Return If(IsDBNull(row.Cells(col).Value), "", row.Cells(col).Value.ToString())
    End Function

    Private Function ValTexte(txt As String) As Object
        Return If(txt.Trim = "", DBNull.Value, CObj(txt.Trim))
    End Function

    Private Function ParseDecimal(txt As String) As Decimal
        Dim v As Decimal
        Decimal.TryParse(txt.Replace(",", "."),
            Globalization.NumberStyles.Any,
            Globalization.CultureInfo.InvariantCulture, v)
        Return v
    End Function

    Private Sub ViderChamps()
        _idTVACourant = 0
        txtTVA_ID.Text = "" : txtTVA_Code.Text = "" : txtTVA_Cle.Text = ""
        txtTVA_Nom.Text = "" : txtTVA_Montant1.Text = "0.00" : txtTVA_Montant2.Text = "0.00"
        DateTimePicker1.Value = Date.Today
        DateTimePicker2.Value = Date.Today
        chkInterdiction.Checked = False
        txtTVA_Code.Focus()
    End Sub



End Class