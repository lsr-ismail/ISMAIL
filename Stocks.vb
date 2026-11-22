Imports System.Data
Imports System.Drawing.Printing
Imports Microsoft.Data.SqlClient

Public Class Stocks
    Private docImpression As New PrintDocument
    Private apercuImpression As New PrintPreviewDialog
    Private pageImpressionCourante As Integer = 0
    Private _idStockCourant As Integer = 0
    Private _codeMatCourant As String = ""

    ' ═══════════════════════════════════════════════════════════════════
    '  LOAD
    ' ═══════════════════════════════════════════════════════════════════

    Private Sub Stocks_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        ModuleStock.SynchroniserFichesStock()
        ChargerStocks()
        AddHandler docImpression.PrintPage, AddressOf docImpression_PrintPage
        txtMAT_Code.ReadOnly = True
        txtMAT_Nom.ReadOnly = True
        txtSTO_Unite.ReadOnly = True
        txtSTO_Stock_Initial.ReadOnly = True
        txtSTO_Somme_Entrees.ReadOnly = True
        txtSTO_Somme_Sorties.ReadOnly = True
        txtSTO_Somme_Corrections.ReadOnly = True
        txtSTO_Stock.ReadOnly = True
    End Sub

    ' ═══════════════════════════════════════════════════════════════════
    '  CHARGEMENT
    ' ═══════════════════════════════════════════════════════════════════

    Private Sub ChargerStocks()
        Try
            Dim dt As DataTable = ModuleDb.Lire(
                "SELECT STO_ID, MAT_Code, MAT_Nom, MAT_Type, STO_Unite, " &
                "STO_Stock_Initial, STO_Somme_Entrees, STO_Somme_Sorties, " &
                "STO_Somme_Corrections, STO_Stock, " &
                "STO_Date, STO_DateModification " &
                "FROM dbo.T_STOCK ORDER BY MAT_Nom")

            DataGridView1.DataSource = dt
            DataGridView1.ReadOnly = True
            DataGridView1.AllowUserToAddRows = False
            DataGridView1.SelectionMode = DataGridViewSelectionMode.FullRowSelect
            DataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill

            For Each col As DataGridViewColumn In DataGridView1.Columns
                col.Visible = False
            Next

            If DataGridView1.Columns.Contains("MAT_Code") Then
                DataGridView1.Columns("MAT_Code").Visible = True
                DataGridView1.Columns("MAT_Code").HeaderText = "Code"
                DataGridView1.Columns("MAT_Code").Width = 100
            End If
            If DataGridView1.Columns.Contains("MAT_Nom") Then
                DataGridView1.Columns("MAT_Nom").Visible = True
                DataGridView1.Columns("MAT_Nom").HeaderText = "Matériau"
                DataGridView1.Columns("MAT_Nom").Width = 200
            End If
            If DataGridView1.Columns.Contains("STO_Stock") Then
                DataGridView1.Columns("STO_Stock").Visible = True
                DataGridView1.Columns("STO_Stock").HeaderText = "Stock Actuel"
                DataGridView1.Columns("STO_Stock").Width = 120
            End If

            For Each row As DataGridViewRow In DataGridView1.Rows
                If Not row.IsNewRow Then
                    Dim stock As Decimal = 0
                    If Not IsDBNull(row.Cells("STO_Stock").Value) Then
                        stock = Convert.ToDecimal(row.Cells("STO_Stock").Value)
                    End If
                    If stock <= 0 Then
                        row.DefaultCellStyle.BackColor = Color.FromArgb(255, 180, 180)
                    ElseIf stock < 5000 Then
                        row.DefaultCellStyle.BackColor = Color.FromArgb(255, 255, 160)
                    Else
                        row.DefaultCellStyle.BackColor = Color.White
                    End If
                End If
            Next

            lblNombre.Text = "Total : " & dt.Rows.Count & " matériau(x)"

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
            _idStockCourant = CInt(row.Cells("STO_ID").Value)
            _codeMatCourant = LireTexte(row, "MAT_Code")

            txtMAT_Code.Text = LireTexte(row, "MAT_Code")
            txtMAT_Nom.Text = LireTexte(row, "MAT_Nom")
            txtSTO_Unite.Text = LireTexte(row, "STO_Unite")
            txtSTO_Stock_Initial.Text = FormatDecimal(row, "STO_Stock_Initial")
            txtSTO_Somme_Entrees.Text = FormatDecimal(row, "STO_Somme_Entrees")
            txtSTO_Somme_Sorties.Text = FormatDecimal(row, "STO_Somme_Sorties")
            txtSTO_Somme_Corrections.Text = FormatDecimal(row, "STO_Somme_Corrections")
            txtSTO_Stock.Text = FormatDecimal(row, "STO_Stock")

            ColorierStock(txtSTO_Stock.Text)
            txtQteEntree.Text = ""
            txtQteEntree.Focus()

        Catch ex As Exception
            MessageBox.Show("Erreur sélection : " & ex.Message)
        End Try
    End Sub

    ' ═══════════════════════════════════════════════════════════════════
    '  BOUTON ENTRÉE
    ' ═══════════════════════════════════════════════════════════════════

    Private Sub btnEntree_Click(sender As Object, e As EventArgs) Handles btnEntree.Click
        If _idStockCourant = 0 Then
            MessageBox.Show("Veuillez sélectionner un matériau.", "Sélection requise", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Exit Sub
        End If
        Dim qte As Decimal = 0
        If Not ParseQte(txtQteEntree.Text, qte, positifSeulement:=True) Then Exit Sub

        If MessageBox.Show(
            "Confirmer l'entrée de " & qte.ToString("0.00") & " " & txtSTO_Unite.Text &
            " pour " & txtMAT_Nom.Text & " ?",
            "Confirmation Entrée", MessageBoxButtons.YesNo, MessageBoxIcon.Question) = DialogResult.No Then Exit Sub
        Try
            Dim lignes As Integer = ModuleDb.Executer(
                "UPDATE dbo.T_STOCK SET " &
                "STO_Somme_Entrees = STO_Somme_Entrees + @Qte, " &
                "STO_Stock = STO_Stock + @Qte, " &
                "STO_DateModification = GETDATE() " &
                "WHERE STO_ID = @STO_ID",
                Sub(cmd)
                    cmd.Parameters.Add("@Qte", SqlDbType.Decimal).Value = qte
                    cmd.Parameters.Add("@STO_ID", SqlDbType.Int).Value = _idStockCourant
                End Sub)

            If lignes > 0 Then
                ' Enregistrement dans le journal (opérateur, date/heure automatiques)
                Synoptique.EnregistrerAction("STOCK", "Entrée stock (écran Stocks)",
                    txtMAT_Nom.Text & " : +" & qte.ToString("0.00") & " " & txtSTO_Unite.Text,
                    txtMAT_Code.Text, qte.ToString("0.00"), "ENTREE_STOCK")

                MessageBox.Show("✅ Entrée enregistrée!" & vbCrLf &
                    "Matériau : " & txtMAT_Nom.Text & vbCrLf &
                    "Quantité ajoutée : " & qte.ToString("0.00") & " " & txtSTO_Unite.Text,
                    "Succès", MessageBoxButtons.OK, MessageBoxIcon.Information)
            Else
                MessageBox.Show("Aucun enregistrement modifié.", "Attention", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            End If

            ChargerStocks()
            ReselectionnerLigne(_idStockCourant)
            txtQteEntree.Text = ""

        Catch ex As Exception
            MessageBox.Show("Erreur entrée stock : " & ex.Message, "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    ' ═══════════════════════════════════════════════════════════════════
    '  BOUTON CORRECTION
    ' ═══════════════════════════════════════════════════════════════════

    Private Sub btnCorrection_Click(sender As Object, e As EventArgs) Handles btnCorrection.Click
        If _idStockCourant = 0 Then
            MessageBox.Show("Veuillez sélectionner un matériau.", "Sélection requise", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Exit Sub
        End If
        Dim correction As Decimal = 0
        If Not ParseQte(txtQteEntree.Text, correction, positifSeulement:=False) Then Exit Sub

        Dim signe = If(correction >= 0, "+", "")
        If MessageBox.Show(
            "Confirmer la correction de " & signe & correction.ToString("0.00") & " " & txtSTO_Unite.Text &
            " pour " & txtMAT_Nom.Text & " ?",
            "Confirmation Correction", MessageBoxButtons.YesNo, MessageBoxIcon.Question) = DialogResult.No Then Exit Sub
        Try
            ModuleDb.Executer(
                "UPDATE dbo.T_STOCK SET " &
                "STO_Somme_Corrections = STO_Somme_Corrections + @Correction, " &
                "STO_Stock = STO_Stock + @Correction, " &
                "STO_DateModification = GETDATE() " &
                "WHERE STO_ID = @STO_ID",
                Sub(cmd)
                    cmd.Parameters.Add("@Correction", SqlDbType.Decimal).Value = correction
                    cmd.Parameters.Add("@STO_ID", SqlDbType.Int).Value = _idStockCourant
                End Sub)

            ' Enregistrement dans le journal (opérateur, date/heure automatiques)
            Synoptique.EnregistrerAction("STOCK", "Correction stock",
                txtMAT_Nom.Text & " : " & signe & correction.ToString("0.00") & " " & txtSTO_Unite.Text,
                txtMAT_Code.Text, correction.ToString("0.00"), "CORRECTION_STOCK")

            MessageBox.Show("✅ Correction appliquée!" & vbCrLf &
                "Matériau : " & txtMAT_Nom.Text & vbCrLf &
                "Correction : " & signe & correction.ToString("0.00") & " " & txtSTO_Unite.Text,
                "Succès", MessageBoxButtons.OK, MessageBoxIcon.Information)

            ChargerStocks()
            ReselectionnerLigne(_idStockCourant)
            txtQteEntree.Text = ""

        Catch ex As Exception
            MessageBox.Show("Erreur correction stock : " & ex.Message, "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    ' ═══════════════════════════════════════════════════════════════════
    '  BOUTON MISE À JOUR STOCK INITIAL
    ' ═══════════════════════════════════════════════════════════════════

    Private Sub btnMiseAJour_Click(sender As Object, e As EventArgs) Handles btnMiseAJour.Click
        If _idStockCourant = 0 Then
            MessageBox.Show("Veuillez sélectionner un matériau.", "Sélection requise", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Exit Sub
        End If
        Dim nvStock As Decimal = 0
        If Not ParseQte(txtQteEntree.Text, nvStock, positifSeulement:=False) Then Exit Sub

        If MessageBox.Show(
            "Définir le stock de " & txtMAT_Nom.Text &
            " à " & nvStock.ToString("0.00") & " " & txtSTO_Unite.Text & " ?",
            "Confirmation Mise à jour", MessageBoxButtons.YesNo, MessageBoxIcon.Question) = DialogResult.No Then Exit Sub
        Try
            Executer(
                "UPDATE dbo.T_STOCK SET " &
                "STO_Stock = @NvStock, STO_Stock_Initial = @NvStock, " &
                "STO_Somme_Entrees = 0, STO_Somme_Sorties = 0, STO_Somme_Corrections = 0, " &
                "STO_DateModification = GETDATE() " &
                "WHERE STO_ID = @STO_ID",
                Sub(cmd)
                    cmd.Parameters.Add("@NvStock", SqlDbType.Decimal).Value = nvStock
                    cmd.Parameters.Add("@STO_ID", SqlDbType.Int).Value = _idStockCourant
                End Sub)

            ' Enregistrement dans le journal (opérateur, date/heure automatiques)
            Synoptique.EnregistrerAction("STOCK", "Mise à jour stock initial",
                txtMAT_Nom.Text & " → nouveau stock : " & nvStock.ToString("0.00") & " " & txtSTO_Unite.Text,
                txtMAT_Code.Text, nvStock.ToString("0.00"), "MAJ_STOCK")

            MessageBox.Show("✅ Stock mis à jour avec succès.", "Succès", MessageBoxButtons.OK, MessageBoxIcon.Information)
            ChargerStocks
            ReselectionnerLigne(_idStockCourant)
            txtQteEntree.Text = ""

        Catch ex As Exception
            MessageBox.Show("Erreur mise à jour : " & ex.Message, "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    ' ═══════════════════════════════════════════════════════════════════
    '  RECHERCHE TEMPS RÉEL
    ' ═══════════════════════════════════════════════════════════════════

    Private Sub txtRECHERCHE_TextChanged(sender As Object, e As EventArgs) Handles txtRECHERCHE.TextChanged
        Try
            Dim dt As DataTable = ModuleDb.Lire(
                "SELECT STO_ID, MAT_Code, MAT_Nom, MAT_Type, STO_Unite, " &
                "STO_Stock_Initial, STO_Somme_Entrees, STO_Somme_Sorties, " &
                "STO_Somme_Corrections, STO_Stock, STO_Date, STO_DateModification " &
                "FROM dbo.T_STOCK WHERE MAT_Code LIKE @mot OR MAT_Nom LIKE @mot ORDER BY MAT_Nom",
                Sub(cmd)
                    cmd.Parameters.Add("@mot", SqlDbType.NVarChar, 200).Value = "%" & txtRECHERCHE.Text.Trim & "%"
                End Sub)

            DataGridView1.DataSource = dt
            If DataGridView1.Columns.Contains("STO_ID") Then
                DataGridView1.Columns("STO_ID").Visible = False
            End If
            lblNombre.Text = "Total : " & dt.Rows.Count & " matériau(x)"

        Catch ex As Exception
            MessageBox.Show("Erreur recherche : " & ex.Message)
        End Try
    End Sub

    ' ═══════════════════════════════════════════════════════════════════
    '  ACTUALISER
    ' ═══════════════════════════════════════════════════════════════════

    Private Sub btnActualiser_Click(sender As Object, e As EventArgs) Handles btnActualiser.Click
        ChargerStocks()
        ViderChamps()
    End Sub

    ' ═══════════════════════════════════════════════════════════════════
    '  RE-SÉLECTIONNER LIGNE APRÈS REFRESH
    ' ═══════════════════════════════════════════════════════════════════

    Private Sub ReselectionnerLigne(idStock As Integer)
        For Each row As DataGridViewRow In DataGridView1.Rows
            If Not row.IsNewRow AndAlso Not IsDBNull(row.Cells("STO_ID").Value) AndAlso
               CInt(row.Cells("STO_ID").Value) = idStock Then
                DataGridView1.ClearSelection()
                row.Selected = True
                DataGridView1.FirstDisplayedScrollingRowIndex = row.Index
                txtMAT_Code.Text = LireTexte(row, "MAT_Code")
                txtMAT_Nom.Text = LireTexte(row, "MAT_Nom")
                txtSTO_Unite.Text = LireTexte(row, "STO_Unite")
                txtSTO_Stock_Initial.Text = FormatDecimal(row, "STO_Stock_Initial")
                txtSTO_Somme_Entrees.Text = FormatDecimal(row, "STO_Somme_Entrees")
                txtSTO_Somme_Sorties.Text = FormatDecimal(row, "STO_Somme_Sorties")
                txtSTO_Somme_Corrections.Text = FormatDecimal(row, "STO_Somme_Corrections")
                txtSTO_Stock.Text = FormatDecimal(row, "STO_Stock")
                ColorierStock(txtSTO_Stock.Text)
                Exit For
            End If
        Next
    End Sub

    ' ═══════════════════════════════════════════════════════════════════
    '  HELPERS
    ' ═══════════════════════════════════════════════════════════════════

    Private Function LireTexte(row As DataGridViewRow, col As String) As String
        Return If(IsDBNull(row.Cells(col).Value), "", row.Cells(col).Value.ToString())
    End Function

    Private Function FormatDecimal(row As DataGridViewRow, col As String) As String
        If IsDBNull(row.Cells(col).Value) Then Return "0.00"
        Return Convert.ToDecimal(row.Cells(col).Value).ToString("0.00")
    End Function

    Private Function ParseQte(texte As String, ByRef valeur As Decimal, positifSeulement As Boolean) As Boolean
        If texte.Trim = "" Then
            MessageBox.Show("Veuillez saisir une quantité.", "Valeur requise", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            txtQteEntree.Focus()
            Return False
        End If
        If Not Decimal.TryParse(texte.Replace(",", "."),
            Globalization.NumberStyles.Any,
            Globalization.CultureInfo.InvariantCulture, valeur) Then
            MessageBox.Show("Valeur invalide. Exemples : 1500 ou -200", "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Error)
            txtQteEntree.Focus()
            Return False
        End If
        If positifSeulement AndAlso valeur <= 0 Then
            MessageBox.Show("La quantité doit être supérieure à zéro.", "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            txtQteEntree.Focus()
            Return False
        End If
        Return True
    End Function

    Private Sub ColorierStock(texte As String)
        Dim stock As Decimal = 0
        Decimal.TryParse(texte, Globalization.NumberStyles.Any,
            Globalization.CultureInfo.InvariantCulture, stock)
        If stock <= 0 Then
            txtSTO_Stock.BackColor = Color.FromArgb(255, 180, 180)
        ElseIf stock < 5000 Then
            txtSTO_Stock.BackColor = Color.FromArgb(255, 255, 160)
        Else
            txtSTO_Stock.BackColor = Color.FromArgb(180, 255, 180)
        End If
    End Sub

    Private Sub ViderChamps()
        _idStockCourant = 0 : _codeMatCourant = ""
        txtMAT_Code.Text = "" : txtMAT_Nom.Text = "" : txtSTO_Unite.Text = ""
        txtSTO_Stock_Initial.Text = "" : txtSTO_Somme_Entrees.Text = ""
        txtSTO_Somme_Sorties.Text = "" : txtSTO_Somme_Corrections.Text = ""
        txtSTO_Stock.Text = "" : txtSTO_Stock.BackColor = Color.White
        txtQteEntree.Text = ""
    End Sub


    ' ═══════════════════════════════════════════════════════════════════
    '  IMPRESSION DU TABLEAU COMPLET
    ' ═══════════════════════════════════════════════════════════════════

    Private Sub btnImprimer_Click(sender As Object, e As EventArgs) Handles btnImprimer.Click
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

        g.DrawString("ÉTAT DES STOCKS — MATIÈRES PREMIÈRES", titreFont, Brushes.Black, x, y)
        g.DrawString("Date d'impression : " & DateTime.Now.ToString("dd/MM/yyyy HH:mm"),
                 New Font("Arial", 8, FontStyle.Italic), Brushes.Gray, x, y + 22)
        y += 55

        ' ── En-tête colonnes ──
        g.DrawString("Code", enteteFont, Brushes.Black, x, y)
        g.DrawString("Matériau", enteteFont, Brushes.Black, x + 90, y)
        g.DrawString("Unité", enteteFont, Brushes.Black, x + 300, y)
        g.DrawString("Stock initial", enteteFont, Brushes.Black, x + 370, y)
        g.DrawString("Entrées", enteteFont, Brushes.Black, x + 470, y)
        g.DrawString("Sorties", enteteFont, Brushes.Black, x + 550, y)
        g.DrawString("Stock actuel", enteteFont, Brushes.Black, x + 630, y)
        y += ligneHauteur
        g.DrawLine(Pens.Black, x, y, x + 720, y)
        y += 6

        ' ── Lignes de données, avec pagination ──
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

            g.DrawString(LireTexte(row, "MAT_Code"), txtFont, Brushes.Black, x, y)
            g.DrawString(LireTexte(row, "MAT_Nom"), txtFont, Brushes.Black, x + 90, y)
            g.DrawString(LireTexte(row, "STO_Unite"), txtFont, Brushes.Black, x + 300, y)
            g.DrawString(FormatDecimal(row, "STO_Stock_Initial"), txtFont, Brushes.Black, x + 370, y)
            g.DrawString(FormatDecimal(row, "STO_Somme_Entrees"), txtFont, Brushes.Black, x + 470, y)
            g.DrawString(FormatDecimal(row, "STO_Somme_Sorties"), txtFont, Brushes.Black, x + 550, y)
            g.DrawString(FormatDecimal(row, "STO_Stock"), New Font("Arial", 9, FontStyle.Bold), Brushes.Black, x + 630, y)

            y += ligneHauteur
            i += 1
        End While

        pageImpressionCourante = 0
        e.HasMorePages = False
    End Sub
End Class