Imports System.Drawing.Printing
Imports Microsoft.Data.SqlClient
Imports System.Data

Public Class Transporteurs

    Private _idTransporteurCourant As Integer = 0
    Private WithEvents pd As New PrintDocument
    Private ppd As New PrintPreviewDialog

    ' ═══════════════════════════════════════════════════════════════════
    '  LOAD
    ' ═══════════════════════════════════════════════════════════════════

    Private Sub Transporteurs_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        DataGridView1.RowHeadersVisible = False   ' NOUVEAU
        ' txtIdTransporteurs.Visible = False

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

        ChargerTransporteurs()
    End Sub

    ' ═══════════════════════════════════════════════════════════════════
    '  CHARGEMENT
    ' ═══════════════════════════════════════════════════════════════════

    Private Sub ChargerTransporteurs()
        Try
            Dim dt As DataTable = ModuleDb.Lire(
            "SELECT IdTransporteurs, CodeTransporteurs, CleTransporteurs, NomTransporteurs, " &
            "Famille, Equipement, Service1, Service2, Service3, " &
            "Valeur1, Valeur2, Valeur3, DerniereModification " &
            "FROM dbo.Transporteurs ORDER BY NomTransporteurs")

            DataGridView1.DataSource = dt
            DataGridView1.SelectionMode = DataGridViewSelectionMode.FullRowSelect
            DataGridView1.MultiSelect = False
            DataGridView1.AllowUserToAddRows = False
            DataGridView1.ReadOnly = True

            ' NOUVEAU : cacher toutes les colonnes puis ne montrer que l'essentiel
            For Each col As DataGridViewColumn In DataGridView1.Columns
                col.Visible = False
            Next
            DataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.None

            If DataGridView1.Columns.Contains("CodeTransporteurs") Then
                DataGridView1.Columns("CodeTransporteurs").Visible = True
                DataGridView1.Columns("CodeTransporteurs").HeaderText = "Code"
                DataGridView1.Columns("CodeTransporteurs").Width = 100
            End If
            If DataGridView1.Columns.Contains("NomTransporteurs") Then
                DataGridView1.Columns("NomTransporteurs").Visible = True
                DataGridView1.Columns("NomTransporteurs").HeaderText = "Nom"
                DataGridView1.Columns("NomTransporteurs").Width = 250
            End If
            If DataGridView1.Columns.Contains("Famille") Then
                DataGridView1.Columns("Famille").Visible = True
                DataGridView1.Columns("Famille").HeaderText = "Famille"
                DataGridView1.Columns("Famille").Width = 100
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
            _idTransporteurCourant = CInt(row.Cells("IdTransporteurs").Value)

            txtIdTransporteurs.Text = _idTransporteurCourant.ToString()
            txtCodeTransporteurs.Text = LireTexte(row, "CodeTransporteurs")
            txtCleTransporteurs.Text = LireTexte(row, "CleTransporteurs")
            txtNomTransporteurs.Text = LireTexte(row, "NomTransporteurs")
            txtFamille.Text = LireTexte(row, "Famille")
            txtEquipement.Text = LireTexte(row, "Equipement")
            txtService1.Text = LireTexte(row, "Service1")
            txtService2.Text = LireTexte(row, "Service2")
            txtService3.Text = LireTexte(row, "Service3")
            txtValeur1.Text = LireTexte(row, "Valeur1")
            txtValeur2.Text = LireTexte(row, "Valeur2")
            txtValeur3.Text = LireTexte(row, "Valeur3")
            txtDerniereModification.Text = If(IsDBNull(row.Cells("DerniereModification").Value), "",
                Convert.ToDateTime(row.Cells("DerniereModification").Value).ToString("dd/MM/yyyy"))

        Catch ex As Exception
            MessageBox.Show("Erreur sélection : " & ex.Message)
        End Try
    End Sub

    ' ═══════════════════════════════════════════════════════════════════
    '  AJOUTER
    ' ═══════════════════════════════════════════════════════════════════

    Private Sub btnAjouter_Click(sender As Object, e As EventArgs) Handles btnAjouter.Click
        If txtCodeTransporteurs.Text.Trim = "" OrElse txtNomTransporteurs.Text.Trim = "" Then
            MessageBox.Show("Code et Nom sont obligatoires.")
            Exit Sub
        End If
        Try
            ModuleDb.Executer(
                "INSERT INTO dbo.Transporteurs " &
                "(CodeTransporteurs, CleTransporteurs, NomTransporteurs, Famille, Equipement, " &
                "Service1, Service2, Service3, Valeur1, Valeur2, Valeur3, DerniereModification) " &
                "VALUES (@CodeTransporteurs, @CleTransporteurs, @NomTransporteurs, @Famille, @Equipement, " &
                "@Service1, @Service2, @Service3, @Valeur1, @Valeur2, @Valeur3, @DerniereModification)",
                AddressOf AjouterParametres)

            MessageBox.Show("Transporteur ajouté avec succès.")
            ChargerTransporteurs()
            ViderChamps()

        Catch ex As Exception
            MessageBox.Show("Erreur ajout : " & ex.Message)
        End Try
    End Sub

    ' ═══════════════════════════════════════════════════════════════════
    '  MODIFIER
    ' ═══════════════════════════════════════════════════════════════════

    Private Sub btnModifier_Click(sender As Object, e As EventArgs) Handles btnModifier.Click
        If _idTransporteurCourant = 0 Then
            MessageBox.Show("Sélectionner un transporteur à modifier.")
            Exit Sub
        End If
        Try
            Executer(
                "UPDATE dbo.Transporteurs SET " &
                "CodeTransporteurs=@CodeTransporteurs, CleTransporteurs=@CleTransporteurs, " &
                "NomTransporteurs=@NomTransporteurs, Famille=@Famille, Equipement=@Equipement, " &
                "Service1=@Service1, Service2=@Service2, Service3=@Service3, " &
                "Valeur1=@Valeur1, Valeur2=@Valeur2, Valeur3=@Valeur3, " &
                "DerniereModification=@DerniereModification " &
                "WHERE IdTransporteurs=@IdTransporteurs",
                Sub(cmd)
                    cmd.Parameters.Add("@IdTransporteurs", SqlDbType.Int).Value = _idTransporteurCourant
                    AjouterParametres(cmd)
                End Sub)

            MessageBox.Show("Transporteur modifié avec succès.")
            ChargerTransporteurs()
            ViderChamps()

        Catch ex As Exception
            MessageBox.Show("Erreur modification : " & ex.Message)
        End Try
    End Sub

    ' ═══════════════════════════════════════════════════════════════════
    '  SUPPRIMER
    ' ═══════════════════════════════════════════════════════════════════

    Private Sub btnSupprimer_Click(sender As Object, e As EventArgs) Handles btnSupprimer.Click
        If _idTransporteurCourant = 0 Then
            MessageBox.Show("Sélectionner un transporteur à supprimer.")
            Exit Sub
        End If
        If MessageBox.Show("Voulez-vous supprimer ce transporteur ?", "Confirmation",
                           MessageBoxButtons.YesNo, MessageBoxIcon.Question) = DialogResult.No Then
            Exit Sub
        End If
        Try
            Executer(
                "DELETE FROM dbo.Transporteurs WHERE IdTransporteurs=@IdTransporteurs",
                Sub(cmd)
                    cmd.Parameters.Add("@IdTransporteurs", SqlDbType.Int).Value = _idTransporteurCourant
                End Sub)

            MessageBox.Show("Transporteur supprimé avec succès.")
            ChargerTransporteurs()
            ViderChamps()

        Catch ex As Exception
            MessageBox.Show("Erreur suppression : " & ex.Message)
        End Try
    End Sub

    ' ═══════════════════════════════════════════════════════════════════
    '  RECHERCHE
    ' ═══════════════════════════════════════════════════════════════════

    Private Sub RECHERCHE_Click(sender As Object, e As EventArgs) Handles RECHERCHE.Click
        Dim mot = txtNomTransporteurs.Text.Trim
        If mot = "" Then
            ChargerTransporteurs()
            Exit Sub
        End If
        Try
            Dim dt = Lire(
                "SELECT IdTransporteurs, CodeTransporteurs, CleTransporteurs, NomTransporteurs, " &
                "Famille, Equipement, Service1, Service2, Service3, " &
                "Valeur1, Valeur2, Valeur3, DerniereModification " &
                "FROM dbo.Transporteurs WHERE CodeTransporteurs LIKE @mot " &
                "OR NomTransporteurs LIKE @mot ORDER BY NomTransporteurs",
                Sub(cmd)
                    cmd.Parameters.Add("@mot", SqlDbType.NVarChar, 200).Value = "%" & mot & "%"
                End Sub)

            DataGridView1.DataSource = dt

            For Each col As DataGridViewColumn In DataGridView1.Columns
                col.Visible = False
            Next
            DataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.None

            If DataGridView1.Columns.Contains("CodeTransporteurs") Then
                DataGridView1.Columns("CodeTransporteurs").Visible = True
                DataGridView1.Columns("CodeTransporteurs").HeaderText = "Code"
                DataGridView1.Columns("CodeTransporteurs").Width = 100
            End If
            If DataGridView1.Columns.Contains("NomTransporteurs") Then
                DataGridView1.Columns("NomTransporteurs").Visible = True
                DataGridView1.Columns("NomTransporteurs").HeaderText = "Nom"
                DataGridView1.Columns("NomTransporteurs").Width = 250
            End If
            If DataGridView1.Columns.Contains("Famille") Then
                DataGridView1.Columns("Famille").Visible = True
                DataGridView1.Columns("Famille").HeaderText = "Famille"
                DataGridView1.Columns("Famille").Width = 100
            End If
            If DataGridView1.Columns.Contains("IdTransporteurs") Then
                DataGridView1.Columns("IdTransporteurs").Visible = False
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
        ChargerTransporteurs()
    End Sub

    ' ═══════════════════════════════════════════════════════════════════
    '  IMPRESSION (inchangée)
    ' ═══════════════════════════════════════════════════════════════════

    Private Sub Imprimer_Click(sender As Object, e As EventArgs) Handles Imprimer.Click
        ppd.Document = pd
        ppd.WindowState = FormWindowState.Maximized
        ppd.ShowDialog()
    End Sub

    Private Sub pd_PrintPage(sender As Object, e As PrintPageEventArgs) Handles pd.PrintPage
        Dim fTitre As New Font("Arial", 14, FontStyle.Bold)
        Dim fEntete As New Font("Arial", 9, FontStyle.Bold)
        Dim fTexte As New Font("Arial", 9)
        Dim x As Integer = 40
        Dim y As Integer = 40
        Dim colW As Integer = 120

        e.Graphics.DrawString("LISTE DES TRANSPORTEURS", fTitre, Brushes.Black, x, y)
        e.Graphics.DrawString("Date : " & DateTime.Now.ToString("dd/MM/yyyy"),
                              New Font("Arial", 8), Brushes.Gray, x, y + 25)
        y += 55

        Dim colonnes = {"Code", "Nom", "Famille", "Service 1", "Valeur 1"}
        Dim nomsCols = {"CodeTransporteurs", "NomTransporteurs", "Famille", "Service1", "Valeur1"}

        Dim px As Integer = x
        For Each h In colonnes
            e.Graphics.FillRectangle(Brushes.LightGray, px, y, colW, 20)
            e.Graphics.DrawRectangle(Pens.Black, px, y, colW, 20)
            e.Graphics.DrawString(h, fEntete, Brushes.Black, px + 2, y + 3)
            px += colW
        Next
        y += 20

        For r As Integer = 0 To DataGridView1.Rows.Count - 1
            Dim row As DataGridViewRow = DataGridView1.Rows(r)
            If Not row.IsNewRow Then
                px = x
                For Each col In nomsCols
                    Dim val As String = If(DataGridView1.Columns.Contains(col) AndAlso
                        Not IsDBNull(row.Cells(col).Value), row.Cells(col).Value.ToString(), "")
                    e.Graphics.DrawRectangle(Pens.LightGray, px, y, colW, 20)
                    e.Graphics.DrawString(val, fTexte, Brushes.Black, px + 2, y + 3)
                    px += colW
                Next
                y += 20
                If y > e.MarginBounds.Bottom - 40 Then
                    e.HasMorePages = True
                    Exit Sub
                End If
            End If
        Next
        e.HasMorePages = False
    End Sub

    ' ═══════════════════════════════════════════════════════════════════
    '  HELPERS
    ' ═══════════════════════════════════════════════════════════════════

    Private Sub AjouterParametres(cmd As SqlCommand)
        cmd.Parameters.Add("@CodeTransporteurs", SqlDbType.NVarChar, 50).Value = ValTexte(txtCodeTransporteurs.Text)
        cmd.Parameters.Add("@CleTransporteurs", SqlDbType.NVarChar, 50).Value = ValTexte(txtCleTransporteurs.Text)
        cmd.Parameters.Add("@NomTransporteurs", SqlDbType.NVarChar, 200).Value = ValTexte(txtNomTransporteurs.Text)
        cmd.Parameters.Add("@Famille", SqlDbType.NVarChar, 100).Value = ValTexte(txtFamille.Text)
        cmd.Parameters.Add("@Equipement", SqlDbType.NVarChar, 100).Value = ValTexte(txtEquipement.Text)
        cmd.Parameters.Add("@Service1", SqlDbType.NVarChar, 100).Value = ValTexte(txtService1.Text)
        cmd.Parameters.Add("@Service2", SqlDbType.NVarChar, 100).Value = ValTexte(txtService2.Text)
        cmd.Parameters.Add("@Service3", SqlDbType.NVarChar, 100).Value = ValTexte(txtService3.Text)
        cmd.Parameters.Add("@Valeur1", SqlDbType.Decimal).Value = ParseDecimal(txtValeur1.Text)
        cmd.Parameters.Add("@Valeur2", SqlDbType.Decimal).Value = ParseDecimal(txtValeur2.Text)
        cmd.Parameters.Add("@Valeur3", SqlDbType.Decimal).Value = ParseDecimal(txtValeur3.Text)
        cmd.Parameters.Add("@DerniereModification", SqlDbType.Date).Value = Date.Today
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
        _idTransporteurCourant = 0
        txtIdTransporteurs.Text = "" : txtCodeTransporteurs.Text = "" : txtCleTransporteurs.Text = ""
        txtNomTransporteurs.Text = "" : txtFamille.Text = "" : txtEquipement.Text = ""
        txtService1.Text = "" : txtService2.Text = "" : txtService3.Text = ""
        txtValeur1.Text = "" : txtValeur2.Text = "" : txtValeur3.Text = ""
        txtDerniereModification.Text = Date.Today.ToString("dd/MM/yyyy")
        txtCodeTransporteurs.Focus()
    End Sub

    Private Sub DataGridView1_CellContentClick(sender As Object, e As DataGridViewCellEventArgs) Handles DataGridView1.CellContentClick

    End Sub
End Class