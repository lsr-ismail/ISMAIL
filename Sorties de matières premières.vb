Imports Microsoft.Data.SqlClient
Imports System.Data

Public Class Sorties_de_matières_premières

    Private _idStockCourant As Integer = 0

    Private Sub Sorties_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        StyleGrille()
        ModuleStock.SynchroniserFichesStock()
        ChargerStocks()
    End Sub

    Private Sub StyleGrille()
        With DataGridView1
            .ReadOnly = True : .AllowUserToAddRows = False
            .SelectionMode = DataGridViewSelectionMode.FullRowSelect
            .MultiSelect = False : .RowHeadersVisible = False
            .EnableHeadersVisualStyles = False
            .AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill
            .ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(180, 30, 30)
            .ColumnHeadersDefaultCellStyle.ForeColor = Color.White
            .ColumnHeadersDefaultCellStyle.Font = New Font("Arial", 9, FontStyle.Bold)
            .DefaultCellStyle.SelectionBackColor = Color.RoyalBlue
            .RowTemplate.Height = 28
            .BackgroundColor = Color.White
            .AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(255, 245, 245)
        End With
    End Sub

    Private Sub ChargerStocks()
        Try
            Dim dt As DataTable = ModuleDb.Lire(
                "SELECT STO_ID, MAT_Code, MAT_Nom, STO_Unite, " &
                "STO_Stock_Initial, STO_Somme_Entrees, STO_Somme_Sorties, " &
                "STO_Somme_Corrections, STO_Stock " &
                "FROM dbo.T_STOCK ORDER BY MAT_Nom")

            DataGridView1.DataSource = dt

            If DataGridView1.Columns.Count > 0 Then
                DataGridView1.Columns("STO_ID").Visible = False
                DataGridView1.Columns("MAT_Code").HeaderText = "Code"
                DataGridView1.Columns("MAT_Nom").HeaderText = "Matériau"
                DataGridView1.Columns("STO_Unite").HeaderText = "Unité"
                DataGridView1.Columns("STO_Stock_Initial").HeaderText = "Stock Initial"
                DataGridView1.Columns("STO_Somme_Entrees").HeaderText = "Entrées"
                DataGridView1.Columns("STO_Somme_Sorties").HeaderText = "Sorties"
                DataGridView1.Columns("STO_Somme_Corrections").HeaderText = "Corrections"
                DataGridView1.Columns("STO_Stock").HeaderText = "Stock Actuel"
            End If

            ColorierStocks()
            lblNombre.Text = "Matériaux : " & dt.Rows.Count

        Catch ex As Exception
            MessageBox.Show("Erreur chargement : " & ex.Message)
        End Try
    End Sub

    Private Sub ColorierStocks()
        For Each row As DataGridViewRow In DataGridView1.Rows
            If row.IsNewRow Then Continue For
            Dim stock As Decimal = 0
            Decimal.TryParse(If(row.Cells("STO_Stock").Value, "0").ToString(), stock)
            row.DefaultCellStyle.BackColor = If(stock <= 0, Color.FromArgb(255, 180, 180),
                                            If(stock < 1000, Color.FromArgb(255, 245, 180),
                                            Color.White))
        Next
        DataGridView1.Refresh()
    End Sub

    Private Sub DataGridView1_CellClick(sender As Object, e As DataGridViewCellEventArgs) Handles DataGridView1.CellClick
        If e.RowIndex < 0 Then Exit Sub
        Dim row As DataGridViewRow = DataGridView1.Rows(e.RowIndex)
        _idStockCourant = CInt(row.Cells("STO_ID").Value)
        txtCodeMat.Text = row.Cells("MAT_Code").Value.ToString()
        txtNomMat.Text = row.Cells("MAT_Nom").Value.ToString()
        txtUnite.Text = row.Cells("STO_Unite").Value.ToString()
        txtStockActuel.Text = row.Cells("STO_Stock").Value.ToString()
        txtQuantite.Text = ""
        txtQuantite.Focus()
    End Sub

    Private Sub btnEnregistrer_Click(sender As Object, e As EventArgs) Handles btnEnregistrer.Click
        If _idStockCourant = 0 Then
            MessageBox.Show("Sélectionnez d'abord un matériau.") : Exit Sub
        End If
        Dim qte As Decimal = 0
        If Not Decimal.TryParse(txtQuantite.Text.Replace(",", "."),
            Globalization.NumberStyles.Any,
            Globalization.CultureInfo.InvariantCulture, qte) OrElse qte <= 0 Then
            MessageBox.Show("Quantité invalide — doit être > 0.") : Exit Sub
        End If

        ' Vérifier stock suffisant
        Dim stockActuel As Decimal = 0
        Decimal.TryParse(txtStockActuel.Text, stockActuel)
        If qte > stockActuel Then
            If MessageBox.Show("⚠️ Stock insuffisant (" & stockActuel.ToString("0.00") & " " & txtUnite.Text & ")" &
                               vbCrLf & "Continuer quand même ?",
                               "Attention", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) = DialogResult.No Then Exit Sub
        End If

        If MessageBox.Show(
            "Confirmer la sortie de " & qte.ToString("0.00") & " " & txtUnite.Text &
            " pour " & txtNomMat.Text & " ?",
            "Confirmation", MessageBoxButtons.YesNo, MessageBoxIcon.Question) = DialogResult.No Then Exit Sub
        Try
            ModuleDb.Executer(
                "UPDATE dbo.T_STOCK SET " &
                "STO_Somme_Sorties = STO_Somme_Sorties + @Qte, " &
                "STO_Stock = STO_Stock - @Qte, " &
                "STO_DateModification = GETDATE() " &
                "WHERE STO_ID = @Id",
                Sub(cmd)
                    cmd.Parameters.Add("@Qte", SqlDbType.Decimal).Value = qte
                    cmd.Parameters.Add("@Id", SqlDbType.Int).Value = _idStockCourant
                End Sub)

            ' Enregistrement dans le journal (opérateur, date/heure automatiques)
            Synoptique.EnregistrerAction("STOCK", "Sortie matière première",
                txtNomMat.Text & " : -" & qte.ToString("0.00") & " " & txtUnite.Text,
                txtCodeMat.Text, qte.ToString("0.00"), "SORTIE_MP")

            MessageBox.Show("✅ Sortie de " & qte.ToString("0.00") & " " & txtUnite.Text &
                            " enregistrée pour " & txtNomMat.Text)
            ChargerStocks()
            For Each row As DataGridViewRow In DataGridView1.Rows
                If Not row.IsNewRow AndAlso CInt(row.Cells("STO_ID").Value) = _idStockCourant Then
                    row.Selected = True
                    DataGridView1_CellClick(Nothing, New DataGridViewCellEventArgs(0, row.Index))
                    Exit For
                End If
            Next

        Catch ex As Exception
            MessageBox.Show("Erreur sortie : " & ex.Message)
        End Try
        ModuleStock.MettreAJourStockMateriau(txtCodeMat.Text, qte)
    End Sub

    Private Sub btnActualiser_Click(sender As Object, e As EventArgs) Handles btnActualiser.Click
        ChargerStocks()
    End Sub

    Private Sub txtRECHERCHE_TextChanged(sender As Object, e As EventArgs) Handles txtRECHERCHE.TextChanged
        If txtRECHERCHE.Text.Trim = "" Then ChargerStocks() : Exit Sub
        Try
            Dim dt As DataTable = ModuleDb.Lire(
                "SELECT STO_ID, MAT_Code, MAT_Nom, STO_Unite, " &
                "STO_Stock_Initial, STO_Somme_Entrees, STO_Somme_Sorties, " &
                "STO_Somme_Corrections, STO_Stock " &
                "FROM dbo.T_STOCK " &
                "WHERE MAT_Code LIKE @mot OR MAT_Nom LIKE @mot " &
                "ORDER BY MAT_Nom",
                Sub(cmd)
                    cmd.Parameters.Add("@mot", SqlDbType.NVarChar, 100).Value = "%" & txtRECHERCHE.Text.Trim() & "%"
                End Sub)
            DataGridView1.DataSource = dt
        Catch ex As Exception
            MessageBox.Show("Erreur recherche : " & ex.Message)
        End Try
    End Sub

End Class