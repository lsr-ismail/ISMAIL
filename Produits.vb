Imports System.Data
Imports System.Drawing.Printing
Imports Microsoft.Data.SqlClient

Public Class Produits
    Private docImpression As New PrintDocument
    Private apercuImpression As New PrintPreviewDialog
    ' ═══════════════════════════════════════════════════════════════════
    '  LOAD
    ' ═══════════════════════════════════════════════════════════════════

    Private Sub ProduitsServicesForm_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        DataGridViewTables.RowHeadersVisible = False   ' NOUVEAU
        AddHandler docImpression.PrintPage, AddressOf docImpression_PrintPage
        ' NOUVEAU : gestion des rôles
        Select Case RoleUtilisateur
            Case "Admin", "Super"
                btnAjouteTable.Visible = True : btnAjouteTable.Enabled = True
                btnEditeTable.Visible = True : btnEditeTable.Enabled = True
                btnSupprimeTable.Visible = True : btnSupprimeTable.Enabled = True
            Case "Operateur"
                btnAjouteTable.Visible = True : btnAjouteTable.Enabled = False
                btnEditeTable.Visible = True : btnEditeTable.Enabled = False
                btnSupprimeTable.Visible = True : btnSupprimeTable.Enabled = False
            Case Else
                btnAjouteTable.Visible = True : btnAjouteTable.Enabled = False
                btnEditeTable.Visible = True : btnEditeTable.Enabled = False
                btnSupprimeTable.Visible = True : btnSupprimeTable.Enabled = False
        End Select

        InitialiserForm()
        ChargerProduitsServices()
    End Sub
    Private Sub AfficherSeulement3Colonnes()
        If DataGridViewTables.Columns.Count = 0 Then Exit Sub
        For Each col As DataGridViewColumn In DataGridViewTables.Columns
            col.Visible = False
        Next
        DataGridViewTables.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.None   ' NOUVEAU

        If DataGridViewTables.Columns.Contains("CodeProduit") Then
            DataGridViewTables.Columns("CodeProduit").Visible = True
            DataGridViewTables.Columns("CodeProduit").HeaderText = "Code"
            DataGridViewTables.Columns("CodeProduit").Width = 110
        End If
        If DataGridViewTables.Columns.Contains("NomProduit") Then
            DataGridViewTables.Columns("NomProduit").Visible = True
            DataGridViewTables.Columns("NomProduit").HeaderText = "Nom"
            DataGridViewTables.Columns("NomProduit").Width = 280
        End If
        If DataGridViewTables.Columns.Contains("TypeProduit") Then
            DataGridViewTables.Columns("TypeProduit").Visible = True
            DataGridViewTables.Columns("TypeProduit").HeaderText = "Type"
            DataGridViewTables.Columns("TypeProduit").Width = 120
        End If
    End Sub
    Private Sub InitialiserForm()
        txtIdProduit.ReadOnly = True
        txtIdProduit.Text = ""

        ComboBox1.Items.Clear()
        ComboBox1.Items.Add("Service")
        ComboBox1.Items.Add("Transport")
        ComboBox1.Items.Add("Béton")

        cmbUnite1.Items.Clear()
        cmbUnite2.Items.Clear()
        For Each u In {"M3 livrés", "Forfait premier BL", "Forfait chaque BL", "Quantité saisie", "Heure"}
            cmbUnite1.Items.Add(u)
            cmbUnite2.Items.Add(u)
        Next

        dtpDateEntree1.Format = DateTimePickerFormat.Short
        dtpDateEntree2.Format = DateTimePickerFormat.Short
        chkServiceTransport.Checked = False
        chkActif.Checked = True
        ViderChamps()
    End Sub

    ' ═══════════════════════════════════════════════════════════════════
    '  CHARGEMENT GRILLE
    ' ═══════════════════════════════════════════════════════════════════

    Private Sub ChargerProduitsServices()
        Try
            Dim dt As DataTable = ModuleDb.Lire(
                "SELECT IdProduit, CodeProduit, CleProduit, NomProduit, TypeProduit, " &
                "CodeFacturation, TVA, NatureService, ServiceTransport, Actif " &
                "FROM ProduitsServices ORDER BY IdProduit DESC")

            DataGridViewTables.DataSource = dt
            AfficherSeulement3Colonnes()
            DataGridViewTables.ReadOnly = True
            DataGridViewTables.AllowUserToAddRows = False
            DataGridViewTables.SelectionMode = DataGridViewSelectionMode.FullRowSelect
            DataGridViewTables.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill

        Catch ex As Exception
            MessageBox.Show("Erreur chargement : " & ex.Message)
        End Try
    End Sub



    ' ═══════════════════════════════════════════════════════════════════
    '  CHARGEMENT TARIFS
    ' ═══════════════════════════════════════════════════════════════════

    Private Sub ChargerTarifsProduit(idProduit As Integer)
        Try
            Dim dt As DataTable = ModuleDb.Lire(
                "SELECT TOP 2 IdTarif, DateEntreeVigueur, MontantUnitaire, UniteTarif " &
                "FROM TarifsProduitsServices WHERE IdProduit=@IdProduit " &
                "ORDER BY DateEntreeVigueur DESC",
                Sub(cmd)
                    cmd.Parameters.Add("@IdProduit", SqlDbType.Int).Value = idProduit
                End Sub)

            dtpDateEntree1.Value = Date.Today : txtMontantUnitaire1.Text = "0.00" : cmbUnite1.SelectedIndex = -1
            dtpDateEntree2.Value = Date.Today : txtMontantUnitaire2.Text = "0.00" : cmbUnite2.SelectedIndex = -1

            If dt.Rows.Count >= 1 Then
                dtpDateEntree1.Value = Convert.ToDateTime(dt.Rows(0)("DateEntreeVigueur"))
                txtMontantUnitaire1.Text = Convert.ToDecimal(dt.Rows(0)("MontantUnitaire")).ToString("0.00")
                cmbUnite1.Text = dt.Rows(0)("UniteTarif").ToString()
            End If
            If dt.Rows.Count >= 2 Then
                dtpDateEntree2.Value = Convert.ToDateTime(dt.Rows(1)("DateEntreeVigueur"))
                txtMontantUnitaire2.Text = Convert.ToDecimal(dt.Rows(1)("MontantUnitaire")).ToString("0.00")
                cmbUnite2.Text = dt.Rows(1)("UniteTarif").ToString()
            End If

        Catch ex As Exception
            MessageBox.Show("Erreur chargement tarifs : " & ex.Message)
        End Try
    End Sub

    ' ═══════════════════════════════════════════════════════════════════
    '  CLIC GRILLE
    ' ═══════════════════════════════════════════════════════════════════

    Private Sub DataGridViewTables_CellClick(sender As Object, e As DataGridViewCellEventArgs) Handles DataGridViewTables.CellClick
        If e.RowIndex < 0 Then Exit Sub
        Dim row As DataGridViewRow = DataGridViewTables.Rows(e.RowIndex)
        If row.Cells("IdProduit").Value Is Nothing Then Exit Sub

        txtIdProduit.Text = row.Cells("IdProduit").Value.ToString()
        txtCodeProduit.Text = row.Cells("CodeProduit").Value.ToString()
        txtCleProduit.Text = row.Cells("CleProduit").Value.ToString()
        txtNomProduit.Text = row.Cells("NomProduit").Value.ToString()
        ComboBox1.Text = row.Cells("TypeProduit").Value.ToString()
        txtCodeFacturation.Text = If(IsDBNull(row.Cells("CodeFacturation").Value), "", row.Cells("CodeFacturation").Value.ToString())
        txtTVA.Text = If(IsDBNull(row.Cells("TVA").Value), "", row.Cells("TVA").Value.ToString())
        txtNatureService.Text = If(IsDBNull(row.Cells("NatureService").Value), "", row.Cells("NatureService").Value.ToString())
        chkServiceTransport.Checked = If(IsDBNull(row.Cells("ServiceTransport").Value), False, Convert.ToBoolean(row.Cells("ServiceTransport").Value))
        chkActif.Checked = If(IsDBNull(row.Cells("Actif").Value), False, Convert.ToBoolean(row.Cells("Actif").Value))

        ChargerTarifsProduit(CInt(row.Cells("IdProduit").Value))
    End Sub

    ' ═══════════════════════════════════════════════════════════════════
    '  AJOUTER (avec transaction)
    ' ═══════════════════════════════════════════════════════════════════

    Private Sub btnAjouteTable_Click(sender As Object, e As EventArgs) Handles btnAjouteTable.Click
        If txtCodeProduit.Text.Trim() = "" OrElse txtNomProduit.Text.Trim() = "" Then
            MessageBox.Show("Code et Nom obligatoires.")
            Exit Sub
        End If
        Try
            ' ── INSERT produit + récupère ID ─────────────────────────
            Dim idProduit As Integer = ModuleDb.LireValeur(Of Integer)(
                "INSERT INTO ProduitsServices " &
                "(CodeProduit, CleProduit, NomProduit, TypeProduit, CodeFacturation, TVA, " &
                "NatureService, ServiceTransport, Actif, DateCreation, DerniereModification) " &
                "VALUES " &
                "(@CodeProduit, @CleProduit, @NomProduit, @TypeProduit, @CodeFacturation, @TVA, " &
                "@NatureService, @ServiceTransport, @Actif, GETDATE(), GETDATE()); " &
                "SELECT SCOPE_IDENTITY();",
                AddressOf AjouterParamsProduit)

            ' ── INSERT tarif 1 ───────────────────────────────────────
            If cmbUnite1.Text.Trim() <> "" Then
                ModuleDb.Executer(
                    "INSERT INTO TarifsProduitsServices " &
                    "(IdProduit, DateEntreeVigueur, MontantUnitaire, UniteTarif) " &
                    "VALUES (@IdProduit, @DateEntreeVigueur, @MontantUnitaire, @UniteTarif)",
                    Sub(cmd) AjouterParamsTarif(cmd, idProduit, dtpDateEntree1, txtMontantUnitaire1, cmbUnite1))
            End If

            ' ── INSERT tarif 2 ───────────────────────────────────────
            If cmbUnite2.Text.Trim() <> "" Then
                ModuleDb.Executer(
                    "INSERT INTO TarifsProduitsServices " &
                    "(IdProduit, DateEntreeVigueur, MontantUnitaire, UniteTarif) " &
                    "VALUES (@IdProduit, @DateEntreeVigueur, @MontantUnitaire, @UniteTarif)",
                    Sub(cmd) AjouterParamsTarif(cmd, idProduit, dtpDateEntree2, txtMontantUnitaire2, cmbUnite2))
            End If

            MessageBox.Show("Produit ajouté avec succès.")
            ChargerProduitsServices()
            ViderChamps()

        Catch ex As Exception
            MessageBox.Show("Erreur ajout : " & ex.Message)
        End Try
    End Sub

    ' ═══════════════════════════════════════════════════════════════════
    '  MODIFIER
    ' ═══════════════════════════════════════════════════════════════════

    Private Sub btnEditeTable_Click(sender As Object, e As EventArgs) Handles btnEditeTable.Click
        If txtIdProduit.Text.Trim() = "" Then
            MessageBox.Show("Sélectionne un produit.")
            Exit Sub
        End If
        Try
            ModuleDb.Executer(
                "UPDATE ProduitsServices SET " &
                "CodeProduit=@CodeProduit, CleProduit=@CleProduit, NomProduit=@NomProduit, " &
                "TypeProduit=@TypeProduit, CodeFacturation=@CodeFacturation, TVA=@TVA, " &
                "NatureService=@NatureService, ServiceTransport=@ServiceTransport, Actif=@Actif, " &
                "DerniereModification=GETDATE() WHERE IdProduit=@IdProduit",
                Sub(cmd)
                    cmd.Parameters.Add("@IdProduit", SqlDbType.Int).Value = CInt(txtIdProduit.Text)
                    AjouterParamsProduit(cmd)
                End Sub)

            MessageBox.Show("Produit modifié avec succès.")
            ChargerProduitsServices()

        Catch ex As Exception
            MessageBox.Show("Erreur modification : " & ex.Message)
        End Try
    End Sub

    ' ═══════════════════════════════════════════════════════════════════
    '  SUPPRIMER (avec transaction)
    ' ═══════════════════════════════════════════════════════════════════

    Private Sub btnSupprimeTable_Click(sender As Object, e As EventArgs) Handles btnSupprimeTable.Click
        If txtIdProduit.Text.Trim() = "" Then
            MessageBox.Show("Sélectionne un produit.")
            Exit Sub
        End If
        If MessageBox.Show("Supprimer ce produit ?", "Confirmation",
                           MessageBoxButtons.YesNo, MessageBoxIcon.Question) = DialogResult.No Then Exit Sub
        Try
            Dim id As Integer = CInt(txtIdProduit.Text)

            ModuleDb.Executer(
                "DELETE FROM TarifsProduitsServices WHERE IdProduit=@Id",
                Sub(cmd) cmd.Parameters.Add("@Id", SqlDbType.Int).Value = id)

            ModuleDb.Executer(
                "DELETE FROM ProduitsServices WHERE IdProduit=@Id",
                Sub(cmd) cmd.Parameters.Add("@Id", SqlDbType.Int).Value = id)

            MessageBox.Show("Produit supprimé avec succès.")
            ChargerProduitsServices()
            ViderChamps()

        Catch ex As Exception
            MessageBox.Show("Erreur suppression : " & ex.Message)
        End Try
    End Sub

    ' ═══════════════════════════════════════════════════════════════════
    '  NOUVEAU / ACTUALISER
    ' ═══════════════════════════════════════════════════════════════════

    Private Sub btnNouveauTable_Click(sender As Object, e As EventArgs) Handles btnNouveauTable.Click
        ViderChamps()
    End Sub

    Private Sub btnActualiserTable_Click(sender As Object, e As EventArgs) Handles btnActualiserTable.Click
        ChargerProduitsServices()
    End Sub

    ' ═══════════════════════════════════════════════════════════════════
    '  HELPERS
    ' ═══════════════════════════════════════════════════════════════════

    Private Sub AjouterParamsProduit(cmd As SqlCommand)
        cmd.Parameters.Add("@CodeProduit", SqlDbType.NVarChar, 50).Value = txtCodeProduit.Text.Trim()
        cmd.Parameters.Add("@CleProduit", SqlDbType.NVarChar, 50).Value = txtCleProduit.Text.Trim()
        cmd.Parameters.Add("@NomProduit", SqlDbType.NVarChar, 200).Value = txtNomProduit.Text.Trim()
        cmd.Parameters.Add("@TypeProduit", SqlDbType.NVarChar, 50).Value = ComboBox1.Text.Trim()
        cmd.Parameters.Add("@CodeFacturation", SqlDbType.NVarChar, 50).Value = txtCodeFacturation.Text.Trim()
        cmd.Parameters.Add("@TVA", SqlDbType.NVarChar, 20).Value = txtTVA.Text.Trim()
        cmd.Parameters.Add("@NatureService", SqlDbType.NVarChar, 100).Value = txtNatureService.Text.Trim()
        cmd.Parameters.Add("@ServiceTransport", SqlDbType.Bit).Value = chkServiceTransport.Checked
        cmd.Parameters.Add("@Actif", SqlDbType.Bit).Value = chkActif.Checked
    End Sub

    Private Sub AjouterParamsTarif(cmd As SqlCommand, idProduit As Integer,
                                    dtp As DateTimePicker, txtMontant As TextBox, cmb As ComboBox)
        cmd.Parameters.Add("@IdProduit", SqlDbType.Int).Value = idProduit
        cmd.Parameters.Add("@DateEntreeVigueur", SqlDbType.Date).Value = dtp.Value.Date
        cmd.Parameters.Add("@MontantUnitaire", SqlDbType.Decimal).Value = ParseDecimal(txtMontant.Text)
        cmd.Parameters.Add("@UniteTarif", SqlDbType.NVarChar, 50).Value = cmb.Text.Trim()
    End Sub

    Private Function ParseDecimal(txt As String) As Decimal
        Dim v As Decimal
        Decimal.TryParse(txt.Replace(",", "."),
            Globalization.NumberStyles.Any,
            Globalization.CultureInfo.InvariantCulture, v)
        Return v
    End Function

    Private Sub ViderChamps()
        txtIdProduit.Text = "" : txtCodeProduit.Text = "" : txtCleProduit.Text = ""
        txtNomProduit.Text = "" : ComboBox1.SelectedIndex = -1
        txtCodeFacturation.Text = "" : txtTVA.Text = "" : txtNatureService.Text = ""
        chkServiceTransport.Checked = False : chkActif.Checked = True
        dtpDateEntree1.Value = Date.Today : txtMontantUnitaire1.Text = "0.00" : cmbUnite1.SelectedIndex = -1
        dtpDateEntree2.Value = Date.Today : txtMontantUnitaire2.Text = "0.00" : cmbUnite2.SelectedIndex = -1
    End Sub


    Private Sub txtRecherche_TextChanged(sender As Object, e As EventArgs) Handles txtRECHERCHE.TextChanged
        RechercherProduits()
    End Sub

    Private Sub RechercherProduits()
        Try
            Dim dt As DataTable = ModuleDb.Lire(
                "SELECT IdProduit, CodeProduit, CleProduit, NomProduit, TypeProduit, " &
                "CodeFacturation, TVA, NatureService, ServiceTransport, Actif " &
                "FROM ProduitsServices " &
                "WHERE CodeProduit LIKE @mot OR NomProduit LIKE @mot " &
                "ORDER BY IdProduit DESC",
                Sub(cmd)
                    cmd.Parameters.Add("@mot", SqlDbType.NVarChar, 200).Value = "%" & txtRECHERCHE.Text.Trim & "%"
                End Sub)

            DataGridViewTables.DataSource = dt
            AfficherSeulement3Colonnes()
            DataGridViewTables.ReadOnly = True
            DataGridViewTables.AllowUserToAddRows = False
            DataGridViewTables.SelectionMode = DataGridViewSelectionMode.FullRowSelect

        Catch ex As Exception
            MessageBox.Show("Erreur recherche : " & ex.Message)
        End Try
    End Sub





    ' ═══════════════════════════════════════════════════════════════════
    '  IMPRESSION
    ' ═══════════════════════════════════════════════════════════════════

    Private Sub Imprimer_Click(sender As Object, e As EventArgs) Handles Imprimer.Click
        If txtIdProduit.Text.Trim() = "" Then
            MessageBox.Show("Sélectionne d'abord un produit.")
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

        g.DrawString("FICHE PRODUIT / SERVICE", titreFont, Brushes.Black, x, y)
        g.DrawString("Date : " & DateTime.Now.ToString("dd/MM/yyyy HH:mm"),
                     New Font("Arial", 9, FontStyle.Italic), Brushes.Gray, x, y + 35)
        y += 80
        g.DrawLine(Pens.Black, x, y, 700, y)
        y += 15

        Dim champs = {
            ("Code :", txtCodeProduit.Text),
            ("Clé :", txtCleProduit.Text),
            ("Nom :", txtNomProduit.Text),
            ("Type :", ComboBox1.Text),
            ("Code facturation :", txtCodeFacturation.Text),
            ("TVA :", txtTVA.Text),
            ("Nature service :", txtNatureService.Text),
            ("Service transport :", If(chkServiceTransport.Checked, "Oui", "Non")),
            ("Actif :", If(chkActif.Checked, "Oui", "Non"))
        }
        For Each c In champs
            g.DrawString(c.Item1, labelFont, Brushes.Black, x, y)
            g.DrawString(c.Item2, txtFont, Brushes.Black, x + 180, y)
            y += esp
        Next

        y += 15
        g.DrawLine(Pens.Black, x, y, 700, y)
        y += 15
        g.DrawString("TARIFS", labelFont, Brushes.Black, x, y)
        y += esp

        g.DrawString("Tarif 1 : " & txtMontantUnitaire1.Text & " (" & cmbUnite1.Text & ") — dès le " & dtpDateEntree1.Value.ToString("dd/MM/yyyy"),
                     txtFont, Brushes.Black, x, y)
        y += esp
        g.DrawString("Tarif 2 : " & txtMontantUnitaire2.Text & " (" & cmbUnite2.Text & ") — dès le " & dtpDateEntree2.Value.ToString("dd/MM/yyyy"),
                     txtFont, Brushes.Black, x, y)
        y += esp + 20

        g.DrawString("Centrale à Béton — Gestion des produits & services",
                     New Font("Arial", 8, FontStyle.Italic), Brushes.Gray, x, y)
    End Sub
End Class