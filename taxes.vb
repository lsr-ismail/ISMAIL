Imports Microsoft.Data.SqlClient
Imports System.Data
Imports System.Drawing.Printing

Public Class taxes

    Private _idTaxeCourant As Integer = 0
    Private docImpression As New PrintDocument
    Private apercuImpression As New PrintPreviewDialog

    ' ═══════════════════════════════════════════════════════════════════
    '  LOAD
    ' ═══════════════════════════════════════════════════════════════════

    Private Sub taxes_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        DataGridView1.RowHeadersVisible = False
        AddHandler docImpression.PrintPage, AddressOf docImpression_PrintPage

        ' Gestion des rôles
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

        ChargerTaxes()
    End Sub

    ' ═══════════════════════════════════════════════════════════════════
    '  CHARGEMENT
    ' ═══════════════════════════════════════════════════════════════════

    Private Sub ChargerTaxes()
        Try
            Dim dt As DataTable = ModuleDb.Lire(
                "SELECT IdTaxe, CodeTaxe, NomTaxe, Taux, CompteComptable, Actif, DerniereModification " &
                "FROM dbo.Taxes ORDER BY NomTaxe")

            DataGridView1.DataSource = dt
            DataGridView1.ReadOnly = True
            DataGridView1.AllowUserToAddRows = False
            DataGridView1.SelectionMode = DataGridViewSelectionMode.FullRowSelect
            DataGridView1.MultiSelect = False

            For Each col As DataGridViewColumn In DataGridView1.Columns
                col.Visible = False
            Next
            DataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.None

            If DataGridView1.Columns.Contains("CodeTaxe") Then
                DataGridView1.Columns("CodeTaxe").Visible = True
                DataGridView1.Columns("CodeTaxe").HeaderText = "Code"
                DataGridView1.Columns("CodeTaxe").Width = 90
            End If
            If DataGridView1.Columns.Contains("NomTaxe") Then
                DataGridView1.Columns("NomTaxe").Visible = True
                DataGridView1.Columns("NomTaxe").HeaderText = "Nom"
                DataGridView1.Columns("NomTaxe").Width = 220
            End If
            If DataGridView1.Columns.Contains("Taux") Then
                DataGridView1.Columns("Taux").Visible = True
                DataGridView1.Columns("Taux").HeaderText = "Taux %"
                DataGridView1.Columns("Taux").Width = 80
            End If
            If DataGridView1.Columns.Contains("Actif") Then
                DataGridView1.Columns("Actif").Visible = True
                DataGridView1.Columns("Actif").HeaderText = "Actif"
                DataGridView1.Columns("Actif").Width = 60
            End If

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
            _idTaxeCourant = CInt(row.Cells("IdTaxe").Value)

            txtCodeTaxe.Text = LireTexte(row, "CodeTaxe")
            txtNomTaxe.Text = LireTexte(row, "NomTaxe")
            txtTaux.Text = LireTexte(row, "Taux")
            txtCompteComptable.Text = LireTexte(row, "CompteComptable")
            chkActif.Checked = Not IsDBNull(row.Cells("Actif").Value) AndAlso Convert.ToBoolean(row.Cells("Actif").Value)

            dtpDerniereModification.Text = If(Not IsDBNull(row.Cells("DerniereModification").Value),
                Convert.ToDateTime(row.Cells("DerniereModification").Value).ToString("dd/MM/yyyy"),
                Date.Today.ToString("dd/MM/yyyy"))

        Catch ex As Exception
            MessageBox.Show("Erreur sélection : " & ex.Message)
        End Try
    End Sub

    ' ═══════════════════════════════════════════════════════════════════
    '  AJOUTER
    ' ═══════════════════════════════════════════════════════════════════

    Private Sub btnAjouter_Click(sender As Object, e As EventArgs) Handles btnAjouter.Click
        If txtCodeTaxe.Text.Trim = "" OrElse txtNomTaxe.Text.Trim = "" Then
            MessageBox.Show("Code et Nom sont obligatoires.")
            Exit Sub
        End If
        Try
            ModuleDb.Executer(
                "INSERT INTO dbo.Taxes (CodeTaxe, NomTaxe, Taux, CompteComptable, Actif, DerniereModification) " &
                "VALUES (@CodeTaxe, @NomTaxe, @Taux, @CompteComptable, @Actif, GETDATE())",
                AddressOf AjouterParametres)

            MessageBox.Show("Taxe ajoutée avec succès.")
            ChargerTaxes()
            ViderChamps()

        Catch ex As Exception
            MessageBox.Show("Erreur ajout : " & ex.Message)
        End Try
    End Sub

    ' ═══════════════════════════════════════════════════════════════════
    '  MODIFIER
    ' ═══════════════════════════════════════════════════════════════════

    Private Sub btnModifier_Click(sender As Object, e As EventArgs) Handles btnModifier.Click
        If _idTaxeCourant = 0 Then
            MessageBox.Show("Sélectionner une taxe à modifier.")
            Exit Sub
        End If
        Try
            ModuleDb.Executer(
                "UPDATE dbo.Taxes SET CodeTaxe=@CodeTaxe, NomTaxe=@NomTaxe, Taux=@Taux, " &
                "CompteComptable=@CompteComptable, Actif=@Actif, DerniereModification=GETDATE() " &
                "WHERE IdTaxe=@IdTaxe",
                Sub(cmd)
                    cmd.Parameters.Add("@IdTaxe", SqlDbType.Int).Value = _idTaxeCourant
                    AjouterParametres(cmd)
                End Sub)

            MessageBox.Show("Taxe modifiée avec succès.")
            ChargerTaxes()
            ViderChamps()

        Catch ex As Exception
            MessageBox.Show("Erreur modification : " & ex.Message)
        End Try
    End Sub

    ' ═══════════════════════════════════════════════════════════════════
    '  SUPPRIMER
    ' ═══════════════════════════════════════════════════════════════════

    Private Sub btnSupprimer_Click(sender As Object, e As EventArgs) Handles btnSupprimer.Click
        If _idTaxeCourant = 0 Then
            MessageBox.Show("Sélectionner une taxe à supprimer.")
            Exit Sub
        End If
        If MessageBox.Show("Voulez-vous supprimer cette taxe ?", "Confirmation",
                           MessageBoxButtons.YesNo, MessageBoxIcon.Question) = DialogResult.No Then Exit Sub
        Try
            ModuleDb.Executer(
                "DELETE FROM dbo.Taxes WHERE IdTaxe=@IdTaxe",
                Sub(cmd) cmd.Parameters.Add("@IdTaxe", SqlDbType.Int).Value = _idTaxeCourant)

            MessageBox.Show("Taxe supprimée avec succès.")
            ChargerTaxes()
            ViderChamps()

        Catch ex As Exception
            MessageBox.Show("Erreur suppression : " & ex.Message)
        End Try
    End Sub

    ' ═══════════════════════════════════════════════════════════════════
    '  RECHERCHE (temps réel)
    ' ═══════════════════════════════════════════════════════════════════

    Private Sub txtRECHERCHE_TextChanged(sender As Object, e As EventArgs)
        RECHERCHE.PerformClick()
    End Sub

    Private Sub RECHERCHE_Click(sender As Object, e As EventArgs) Handles RECHERCHE.Click
        Try
            Dim dt As DataTable = ModuleDb.Lire(
                "SELECT IdTaxe, CodeTaxe, NomTaxe, Taux, CompteComptable, Actif, DerniereModification " &
                "FROM dbo.Taxes WHERE CodeTaxe LIKE @mot OR NomTaxe LIKE @mot ORDER BY NomTaxe",
                Sub(cmd)
                    cmd.Parameters.Add("@mot", SqlDbType.NVarChar, 200).Value = "%" & txtRECHERCHE.Text.Trim & "%"
                End Sub)

            DataGridView1.DataSource = dt

            For Each col As DataGridViewColumn In DataGridView1.Columns
                col.Visible = False
            Next
            DataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.None

            If DataGridView1.Columns.Contains("CodeTaxe") Then
                DataGridView1.Columns("CodeTaxe").Visible = True
                DataGridView1.Columns("CodeTaxe").HeaderText = "Code"
                DataGridView1.Columns("CodeTaxe").Width = 90
            End If
            If DataGridView1.Columns.Contains("NomTaxe") Then
                DataGridView1.Columns("NomTaxe").Visible = True
                DataGridView1.Columns("NomTaxe").HeaderText = "Nom"
                DataGridView1.Columns("NomTaxe").Width = 220
            End If
            If DataGridView1.Columns.Contains("Taux") Then
                DataGridView1.Columns("Taux").Visible = True
                DataGridView1.Columns("Taux").HeaderText = "Taux %"
                DataGridView1.Columns("Taux").Width = 80
            End If
            If DataGridView1.Columns.Contains("Actif") Then
                DataGridView1.Columns("Actif").Visible = True
                DataGridView1.Columns("Actif").HeaderText = "Actif"
                DataGridView1.Columns("Actif").Width = 60
            End If

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
        ChargerTaxes()
    End Sub

    ' ═══════════════════════════════════════════════════════════════════
    '  IMPRESSION
    ' ═══════════════════════════════════════════════════════════════════

    Private Sub Imprimer_Click(sender As Object, e As EventArgs) Handles Imprimer.Click
        If _idTaxeCourant = 0 Then
            MessageBox.Show("Sélectionner une taxe à imprimer.")
            Exit Sub
        End If
        apercuImpression.Document = docImpression
        apercuImpression.WindowState = FormWindowState.Maximized
        apercuImpression.ShowDialog()
    End Sub

    Private Sub docImpression_PrintPage(sender As Object, e As PrintPageEventArgs)
        Dim g As Graphics = e.Graphics
        Dim titreFont As New Font("Arial", 16, FontStyle.Bold)
        Dim labelFont As New Font("Arial", 11, FontStyle.Bold)
        Dim txtFont As New Font("Arial", 11)
        Dim x As Integer = 50
        Dim y As Integer = 50
        Dim esp As Integer = 28

        g.DrawString("FICHE TAXE", titreFont, Brushes.Black, x, y)
        g.DrawString("Date : " & DateTime.Now.ToString("dd/MM/yyyy HH:mm"),
                     New Font("Arial", 9, FontStyle.Italic), Brushes.Gray, x, y + 35)
        y += 80
        g.DrawLine(Pens.Black, x, y, 700, y)
        y += 15

        Dim champs = {
            ("Code :", txtCodeTaxe.Text),
            ("Nom :", txtNomTaxe.Text),
            ("Taux :", txtTaux.Text & " %"),
            ("Compte comptable :", txtCompteComptable.Text),
            ("Actif :", If(chkActif.Checked, "Oui", "Non"))
        }
        For Each c In champs
            g.DrawString(c.Item1, labelFont, Brushes.Black, x, y)
            g.DrawString(c.Item2, txtFont, Brushes.Black, x + 200, y)
            y += esp
        Next

        y += 15
        g.DrawString("Centrale à Béton — Gestion des taxes",
                     New Font("Arial", 8, FontStyle.Italic), Brushes.Gray, x, y)
    End Sub

    ' ═══════════════════════════════════════════════════════════════════
    '  HELPERS
    ' ═══════════════════════════════════════════════════════════════════

    Private Sub AjouterParametres(cmd As SqlCommand)
        cmd.Parameters.Add("@CodeTaxe", SqlDbType.NVarChar, 20).Value = txtCodeTaxe.Text.Trim
        cmd.Parameters.Add("@NomTaxe", SqlDbType.NVarChar, 100).Value = txtNomTaxe.Text.Trim
        cmd.Parameters.Add("@Taux", SqlDbType.Decimal).Value = ParseDecimal(txtTaux.Text)
        cmd.Parameters.Add("@CompteComptable", SqlDbType.NVarChar, 30).Value = ValTexte(txtCompteComptable.Text)
        cmd.Parameters.Add("@Actif", SqlDbType.Bit).Value = chkActif.Checked
    End Sub

    Private Function ParseDecimal(txt As String) As Decimal
        Dim v As Decimal
        Decimal.TryParse(txt.Replace(",", "."),
            Globalization.NumberStyles.Any,
            Globalization.CultureInfo.InvariantCulture, v)
        Return v
    End Function

    Private Function LireTexte(row As DataGridViewRow, col As String) As String
        Return If(IsDBNull(row.Cells(col).Value), "", row.Cells(col).Value.ToString())
    End Function

    Private Function ValTexte(txt As String) As Object
        Return If(txt.Trim = "", DBNull.Value, CObj(txt.Trim))
    End Function

    Private Sub ViderChamps()
        _idTaxeCourant = 0
        txtCodeTaxe.Text = "" : txtNomTaxe.Text = "" : txtTaux.Text = ""
        txtCompteComptable.Text = ""
        chkActif.Checked = True
        dtpDerniereModification.Text = Date.Today.ToString("dd/MM/yyyy")
        txtCodeTaxe.Focus()
    End Sub

End Class