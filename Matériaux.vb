Imports System.Drawing.Printing
Imports Microsoft.Data.SqlClient
Imports System.Data

Public Class Matériaux

    Private docImpression As New PrintDocument
    Private apercuImpression As New PrintPreviewDialog
    Private idMateriauSelectionne As Integer = 0
    Private modeEdition As Boolean = False

    ' ═══════════════════════════════════════════════════════════════════
    '  LOAD
    ' ═══════════════════════════════════════════════════════════════════

    Private Sub Matériaux_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        DataGridView1.RowHeadersVisible = False   ' NOUVEAU

        ' NOUVEAU : gestion des rôles
        Select Case RoleUtilisateur
            Case "Admin", "Super"
                btnAjouter.Visible = True : btnAjouter.Enabled = True
                btnModifier.Visible = True : btnModifier.Enabled = True
            Case "Operateur"
                btnAjouter.Visible = True : btnAjouter.Enabled = False
                btnModifier.Visible = True : btnModifier.Enabled = False
            Case Else
                btnAjouter.Visible = True : btnAjouter.Enabled = False
                btnModifier.Visible = True : btnModifier.Enabled = False
        End Select

        cmbTypeMateriau.Items.Clear()
        cmbTypeMateriau.Items.Add("GRANULAT")
        cmbTypeMateriau.Items.Add("LIANT")
        cmbTypeMateriau.Items.Add("EAU")
        cmbTypeMateriau.Items.Add("ADJUVANT")
        cmbTypeMateriau.Items.Add("ADDITION")
        cmbTypeMateriau.Items.Add("AJOUT")
        ChargerMateriaux()
        AddHandler docImpression.PrintPage, AddressOf docImpression_PrintPage
    End Sub

    ' ═══════════════════════════════════════════════════════════════════
    '  CHARGEMENT
    ' ═══════════════════════════════════════════════════════════════════

    Private Sub ChargerMateriaux()
        Try
            Dim dt As DataTable = ModuleDb.Lire(
            "SELECT IdMateriau, CodeMateriau, CleRecherche, NomMateriau, TypeMateriau, Stock, PrixRevient " &
            "FROM Materiaux ORDER BY IdMateriau DESC")

            DataGridView1.DataSource = dt
            DataGridView1.ReadOnly = True
            DataGridView1.SelectionMode = DataGridViewSelectionMode.FullRowSelect
            DataGridView1.AllowUserToAddRows = False
            DataGridView1.RowHeadersVisible = False   ' NOUVEAU

            ' NOUVEAU : largeurs fixes — seulement Code / Nom / Type
            For Each col As DataGridViewColumn In DataGridView1.Columns
                col.Visible = False
            Next
            DataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.None

            If DataGridView1.Columns.Contains("CodeMateriau") Then
                DataGridView1.Columns("CodeMateriau").Visible = True
                DataGridView1.Columns("CodeMateriau").HeaderText = "Code"
                DataGridView1.Columns("CodeMateriau").Width = 110
            End If
            If DataGridView1.Columns.Contains("NomMateriau") Then
                DataGridView1.Columns("NomMateriau").Visible = True
                DataGridView1.Columns("NomMateriau").HeaderText = "Nom"
                DataGridView1.Columns("NomMateriau").Width = 230
            End If
            If DataGridView1.Columns.Contains("TypeMateriau") Then
                DataGridView1.Columns("TypeMateriau").Visible = True
                DataGridView1.Columns("TypeMateriau").HeaderText = "Type"
                DataGridView1.Columns("TypeMateriau").Width = 130
            End If
             lblNombre.Text = "Total : " & dt.Rows.Count & " matériau(x)"   ' NOUVEAU
        Catch ex As Exception
            MessageBox.Show("Erreur chargement : " & ex.Message)
        End Try
    End Sub

    ' ═══════════════════════════════════════════════════════════════════
    '  CLIC GRILLE
    ' ═══════════════════════════════════════════════════════════════════

    Private Sub DataGridView1_CellClick(sender As Object, e As DataGridViewCellEventArgs) Handles DataGridView1.CellClick
        If e.RowIndex >= 0 Then
            idMateriauSelectionne = Convert.ToInt32(DataGridView1.Rows(e.RowIndex).Cells("IdMateriau").Value)
            ChargerDetailsMateriau(idMateriauSelectionne)
        End If
    End Sub

    Private Sub ChargerDetailsMateriau(id As Integer)
        Try
            ' ── Matériau principal ──────────────────────────────────
            Dim dt1 As DataTable = ModuleDb.Lire(
                "SELECT * FROM Materiaux WHERE IdMateriau=@Id",
                Sub(cmd) cmd.Parameters.Add("@Id", SqlDbType.Int).Value = id)

            If dt1.Rows.Count > 0 Then
                Dim dr As DataRow = dt1.Rows(0)
                txtCodeMateriau.Text = dr("CodeMateriau").ToString()
                txtCleRecherche.Text = dr("CleRecherche").ToString()
                txtNomMateriau.Text = dr("NomMateriau").ToString()
                cmbTypeMateriau.Text = dr("TypeMateriau").ToString()
                txtCodeMatierePremiere.Text = dr("CodeMatierePremiere").ToString()
                txtMateriauEquivalent.Text = dr("MateriauEquivalent").ToString()
                txtPrixRevient.Text = dr("PrixRevient").ToString()
                txtDensite.Text = dr("Densite").ToString()
                txtStock.Text = dr("Stock").ToString()
                txtDosageMax.Text = dr("DosageMaxM3").ToString()
                txtTolerance.Text = dr("ToleranceAutomate").ToString()
                txtTauxChlorure.Text = dr("TauxChlorure").ToString()
                txtTauxAlcalin.Text = dr("TauxAlcalin").ToString()
                txtGestionStock.Text = If(Convert.ToBoolean(dr("GestionStocks")), "1", "0")
                txtAbsorptionCapillaire.Text = dr("AbsorptionCapillaire").ToString()
                txtSable.Text = If(Convert.ToBoolean(dr("EstSable")), "1", "0")
                txtDosageMiniBL.Text = dr("DosageMiniBL").ToString()
                txtDosageMaxiBL.Text = dr("DosageMaxiBL").ToString()
            End If

            ' ── Facturation ────────────────────────────────────────
            Dim dt2 As DataTable = ModuleDb.Lire(
                "SELECT TOP 1 * FROM MateriauxFacturation WHERE IdMateriau=@Id",
                Sub(cmd) cmd.Parameters.Add("@Id", SqlDbType.Int).Value = id)

            If dt2.Rows.Count > 0 Then
                Dim dr2 As DataRow = dt2.Rows(0)
                txtCodeProduit.Text = dr2("CodeProduit").ToString()
                txtCleRechercheProduit.Text = dr2("CleProduit").ToString()
                txtNomProduit.Text = dr2("NomProduit").ToString()
                txtTVA.Text = dr2("TVA").ToString()
                txtTarif1.Text = dr2("Tarif1Montant").ToString()
                txtTarif2.Text = dr2("Tarif2Montant").ToString()
                txtUnite.Text = dr2("Tarif1Unite").ToString()
            Else
                txtCodeProduit.Text = "" : txtCleRechercheProduit.Text = "" : txtNomProduit.Text = ""
                txtTVA.Text = "" : txtTarif1.Text = "" : txtTarif2.Text = "" : txtUnite.Text = ""
            End If

        Catch ex As Exception
            MessageBox.Show("Erreur chargement détail : " & ex.Message)
        End Try
    End Sub

    ' ═══════════════════════════════════════════════════════════════════
    '  MODIFIER (activation du mode)
    ' ═══════════════════════════════════════════════════════════════════

    Private Sub btnModifier_Click(sender As Object, e As EventArgs) Handles btnModifier.Click
        If idMateriauSelectionne = 0 Then
            MessageBox.Show("Sélectionne d'abord une ligne.")
            Exit Sub
        End If
        modeEdition = True
        MessageBox.Show("Mode modification activé. Modifiez les champs puis cliquez sur AJOUTER pour enregistrer.")
    End Sub

    ' ═══════════════════════════════════════════════════════════════════
    '  AJOUTER / ENREGISTRER (INSERT ou UPDATE selon modeEdition)
    ' ═══════════════════════════════════════════════════════════════════

    Private Sub btnAjouter_Click(sender As Object, e As EventArgs) Handles btnAjouter.Click
        Try
            If Not modeEdition Then
                ' ── INSERT Matériau + récupère l'ID ─────────────────
                Dim idNouveauMateriau As Integer = ModuleDb.LireValeur(Of Integer)(
                    "INSERT INTO Materiaux " &
                    "(CodeMateriau, CleRecherche, NomMateriau, TypeMateriau, CodeMatierePremiere, LibelleSilo, MateriauEquivalent, " &
                    "PrixRevient, Stock, UniteDosage, UniteStockage, ToleranceAutomate, Densite, DosageMaxM3, " &
                    "TauxChlorure, TauxAlcalin, AbsorptionCapillaire, EstSable, DosageMiniBL, DosageMaxiBL, " &
                    "GestionStocks, DerniereModification) " &
                    "VALUES " &
                    "(@CodeMateriau, @CleRecherche, @NomMateriau, @TypeMateriau, @CodeMatierePremiere, @LibelleSilo, @MateriauEquivalent, " &
                    "@PrixRevient, @Stock, @UniteDosage, @UniteStockage, @ToleranceAutomate, @Densite, @DosageMaxM3, " &
                    "@TauxChlorure, @TauxAlcalin, @AbsorptionCapillaire, @EstSable, @DosageMiniBL, @DosageMaxiBL, " &
                    "@GestionStocks, GETDATE()); SELECT SCOPE_IDENTITY();",
                    Sub(cmd) AjouterParamsMateriau(cmd))

                ' ── INSERT Facturation ──────────────────────────────
                ModuleDb.Executer(
                    "INSERT INTO MateriauxFacturation " &
                    "(IdMateriau, CodeProduit, CleProduit, NomProduit, TVA, " &
                    "Tarif1Date, Tarif1Montant, Tarif1Unite, Tarif2Date, Tarif2Montant, Tarif2Unite) " &
                    "VALUES " &
                    "(@IdMateriau, @CodeProduit, @CleProduit, @NomProduit, @TVA, " &
                    "@Tarif1Date, @Tarif1Montant, @Tarif1Unite, @Tarif2Date, @Tarif2Montant, @Tarif2Unite)",
                    Sub(cmd) AjouterParamsFacturation(cmd, idNouveauMateriau))

                MessageBox.Show("Matériau ajouté avec succès.")

            Else
                ' ── UPDATE Matériau ─────────────────────────────────
                ModuleDb.Executer(
                    "UPDATE Materiaux SET " &
                    "CodeMateriau=@CodeMateriau, CleRecherche=@CleRecherche, NomMateriau=@NomMateriau, " &
                    "TypeMateriau=@TypeMateriau, CodeMatierePremiere=@CodeMatierePremiere, " &
                    "MateriauEquivalent=@MateriauEquivalent, PrixRevient=@PrixRevient, Stock=@Stock, " &
                    "UniteDosage=@UniteDosage, UniteStockage=@UniteStockage, ToleranceAutomate=@ToleranceAutomate, " &
                    "Densite=@Densite, DosageMaxM3=@DosageMaxM3, TauxChlorure=@TauxChlorure, " &
                    "TauxAlcalin=@TauxAlcalin, AbsorptionCapillaire=@AbsorptionCapillaire, " &
                    "EstSable=@EstSable, DosageMiniBL=@DosageMiniBL, DosageMaxiBL=@DosageMaxiBL, " &
                    "GestionStocks=@GestionStocks, DerniereModification=GETDATE() " &
                    "WHERE IdMateriau=@IdMateriau",
                    Sub(cmd)
                        AjouterParamsMateriau(cmd)
                        cmd.Parameters.Add("@IdMateriau", SqlDbType.Int).Value = idMateriauSelectionne
                    End Sub)

                ' ── UPDATE ou INSERT Facturation ────────────────────
                Dim nb As Integer = ModuleDb.LireValeur(Of Integer)(
                    "SELECT COUNT(*) FROM MateriauxFacturation WHERE IdMateriau=@Id",
                    Sub(cmd) cmd.Parameters.Add("@Id", SqlDbType.Int).Value = idMateriauSelectionne)

                If nb > 0 Then
                    ModuleDb.Executer(
                        "UPDATE MateriauxFacturation SET " &
                        "CodeProduit=@CodeProduit, CleProduit=@CleProduit, NomProduit=@NomProduit, TVA=@TVA, " &
                        "Tarif1Montant=@Tarif1Montant, Tarif1Unite=@Tarif1Unite, " &
                        "Tarif2Montant=@Tarif2Montant, Tarif2Unite=@Tarif2Unite " &
                        "WHERE IdMateriau=@IdMateriau",
                        Sub(cmd)
                            cmd.Parameters.Add("@CodeProduit", SqlDbType.NVarChar, 50).Value = txtCodeProduit.Text.Trim()
                            cmd.Parameters.Add("@CleProduit", SqlDbType.NVarChar, 50).Value = txtCleRechercheProduit.Text.Trim()
                            cmd.Parameters.Add("@NomProduit", SqlDbType.NVarChar, 200).Value = txtNomProduit.Text.Trim()
                            cmd.Parameters.Add("@TVA", SqlDbType.Decimal).Value = NombreValeur(txtTVA)
                            cmd.Parameters.Add("@Tarif1Montant", SqlDbType.Decimal).Value = NombreValeur(txtTarif1)
                            cmd.Parameters.Add("@Tarif1Unite", SqlDbType.NVarChar, 50).Value = txtUnite.Text.Trim()
                            cmd.Parameters.Add("@Tarif2Montant", SqlDbType.Decimal).Value = NombreValeur(txtTarif2)
                            cmd.Parameters.Add("@Tarif2Unite", SqlDbType.NVarChar, 50).Value = txtUnite.Text.Trim()
                            cmd.Parameters.Add("@IdMateriau", SqlDbType.Int).Value = idMateriauSelectionne
                        End Sub)
                Else
                    ModuleDb.Executer(
                        "INSERT INTO MateriauxFacturation " &
                        "(IdMateriau, CodeProduit, CleProduit, NomProduit, TVA, " &
                        "Tarif1Date, Tarif1Montant, Tarif1Unite, Tarif2Date, Tarif2Montant, Tarif2Unite) " &
                        "VALUES (@IdMateriau, @CodeProduit, @CleProduit, @NomProduit, @TVA, " &
                        "@Tarif1Date, @Tarif1Montant, @Tarif1Unite, @Tarif2Date, @Tarif2Montant, @Tarif2Unite)",
                        Sub(cmd) AjouterParamsFacturation(cmd, idMateriauSelectionne))
                End If

                MessageBox.Show("Matériau modifié avec succès.")
                modeEdition = False
            End If

            ChargerMateriaux()
            ViderChamps()
            idMateriauSelectionne = 0

        Catch ex As Exception
            MessageBox.Show("Erreur enregistrement : " & ex.Message)
        End Try
    End Sub

    ' ═══════════════════════════════════════════════════════════════════
    '  SUPPRIMER
    ' ═══════════════════════════════════════════════════════════════════

    Private Sub btnSupprimer_Click(sender As Object, e As EventArgs) Handles btnSupprimer.Click
        If idMateriauSelectionne = 0 Then
            MessageBox.Show("Sélectionne d'abord une ligne.")
            Exit Sub
        End If
        If MessageBox.Show("Voulez-vous supprimer ce matériau ?", "Confirmation",
                           MessageBoxButtons.YesNo, MessageBoxIcon.Question) = DialogResult.No Then Exit Sub
        Try
            ModuleDb.Executer(
                "DELETE FROM MateriauxFacturation WHERE IdMateriau=@Id",
                Sub(cmd) cmd.Parameters.Add("@Id", SqlDbType.Int).Value = idMateriauSelectionne)

            ModuleDb.Executer(
                "DELETE FROM Materiaux WHERE IdMateriau=@Id",
                Sub(cmd) cmd.Parameters.Add("@Id", SqlDbType.Int).Value = idMateriauSelectionne)

            MessageBox.Show("Matériau supprimé avec succès.")
            ChargerMateriaux()
            ViderChamps()
            idMateriauSelectionne = 0
            modeEdition = False

        Catch ex As Exception
            MessageBox.Show("Erreur suppression : " & ex.Message)
        End Try
    End Sub

    ' ═══════════════════════════════════════════════════════════════════
    '  RECHERCHE
    ' ═══════════════════════════════════════════════════════════════════

    Private Sub txtRECHERCHE_TextChanged(sender As Object, e As EventArgs) Handles txtRECHERCHE.TextChanged
        RECHERCHE.PerformClick()
    End Sub

    Private Sub RECHERCHE_Click(sender As Object, e As EventArgs) Handles RECHERCHE.Click
        Try
            Dim dt As DataTable = ModuleDb.Lire(
                "SELECT IdMateriau, CodeMateriau, CleRecherche, NomMateriau, TypeMateriau, Stock, PrixRevient " &
                "FROM Materiaux " &
                "WHERE CodeMateriau LIKE @rech OR NomMateriau LIKE @rech OR TypeMateriau LIKE @rech " &
                "ORDER BY IdMateriau DESC",
                Sub(cmd)
                    cmd.Parameters.Add("@rech", SqlDbType.NVarChar, 200).Value = "%" & txtRECHERCHE.Text.Trim() & "%"
                End Sub)

            DataGridView1.DataSource = dt
            If DataGridView1.Columns.Contains("IdMateriau") Then
                DataGridView1.Columns("IdMateriau").Visible = False
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
        idMateriauSelectionne = 0
        modeEdition = False
    End Sub

    Private Sub btnActualiser_Click(sender As Object, e As EventArgs) Handles btnActualiser.Click
        ChargerMateriaux()
        idMateriauSelectionne = 0
        modeEdition = False
    End Sub

    ' ═══════════════════════════════════════════════════════════════════
    '  IMPRESSION (inchangée)
    ' ═══════════════════════════════════════════════════════════════════

    Private Sub Imprimer_Click(sender As Object, e As EventArgs) Handles Imprimer.Click
        If idMateriauSelectionne = 0 Then
            MessageBox.Show("Sélectionne d'abord une ligne à imprimer.")
            Exit Sub
        End If
        apercuImpression.Document = docImpression
        apercuImpression.WindowState = FormWindowState.Maximized
        apercuImpression.ShowDialog()
    End Sub

    Private Sub docImpression_PrintPage(sender As Object, e As PrintPageEventArgs)
        Dim g As Graphics = e.Graphics
        Dim titreFont As New Font("Arial", 16, FontStyle.Bold)
        Dim normalFont As New Font("Arial", 11, FontStyle.Regular)
        Dim grasFont As New Font("Arial", 11, FontStyle.Bold)
        Dim x As Integer = 50 : Dim y As Integer = 50 : Dim ligne As Integer = 30

        g.DrawString("Fiche Matériau", titreFont, Brushes.Black, x, y) : y += 50
        g.DrawString("Date impression : " & DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss"), normalFont, Brushes.Black, x, y) : y += 40

        Dim champs = {("Code matériau :", txtCodeMateriau.Text), ("Clé recherche :", txtCleRecherche.Text),
                      ("Nom matériau :", txtNomMateriau.Text), ("Type matériau :", cmbTypeMateriau.Text),
                      ("Code matière première :", txtCodeMatierePremiere.Text), ("Matériau équivalent :", txtMateriauEquivalent.Text),
                      ("Prix revient :", txtPrixRevient.Text), ("Densité :", txtDensite.Text),
                      ("Stock :", txtStock.Text), ("Dosage max :", txtDosageMax.Text),
                      ("Tolérance :", txtTolerance.Text), ("Taux chlorure :", txtTauxChlorure.Text),
                      ("Taux alcalin :", txtTauxAlcalin.Text), ("Gestion stock :", txtGestionStock.Text),
                      ("Absorption capillaire :", txtAbsorptionCapillaire.Text), ("Sable :", txtSable.Text)}
        For Each c In champs
            g.DrawString(c.Item1, grasFont, Brushes.Black, x, y)
            g.DrawString(c.Item2, normalFont, Brushes.Black, x + 180, y)
            y += ligne
        Next

        g.DrawLine(Pens.Black, x, y + 10, 750, y + 10) : y += 30
        g.DrawString("Facturation", titreFont, Brushes.Black, x, y) : y += 40

        Dim champsF = {("Code produit :", txtCodeProduit.Text), ("Clé produit :", txtCleRechercheProduit.Text),
                       ("Nom produit :", txtNomProduit.Text), ("TVA :", txtTVA.Text),
                       ("Tarif 1 :", txtTarif1.Text), ("Tarif 2 :", txtTarif2.Text),
                       ("Montant unitaire :", txtMontantUnitaire.Text), ("Unité :", txtUnite.Text),
                       ("Dosage mini BL :", txtDosageMiniBL.Text), ("Dosage maxi BL :", txtDosageMaxiBL.Text)}
        For Each c In champsF
            g.DrawString(c.Item1, grasFont, Brushes.Black, x, y)
            g.DrawString(c.Item2, normalFont, Brushes.Black, x + 180, y)
            y += ligne
        Next
    End Sub

    ' ═══════════════════════════════════════════════════════════════════
    '  HELPERS
    ' ═══════════════════════════════════════════════════════════════════

    Private Sub AjouterParamsMateriau(cmd As SqlCommand)
        cmd.Parameters.Add("@CodeMateriau", SqlDbType.NVarChar, 50).Value = txtCodeMateriau.Text.Trim()
        cmd.Parameters.Add("@CleRecherche", SqlDbType.NVarChar, 50).Value = txtCleRecherche.Text.Trim()
        cmd.Parameters.Add("@NomMateriau", SqlDbType.NVarChar, 200).Value = txtNomMateriau.Text.Trim()
        cmd.Parameters.Add("@TypeMateriau", SqlDbType.NVarChar, 50).Value = cmbTypeMateriau.Text.Trim()
        cmd.Parameters.Add("@CodeMatierePremiere", SqlDbType.NVarChar, 50).Value = txtCodeMatierePremiere.Text.Trim()
        cmd.Parameters.Add("@LibelleSilo", SqlDbType.NVarChar, 50).Value = ""
        cmd.Parameters.Add("@MateriauEquivalent", SqlDbType.NVarChar, 50).Value = txtMateriauEquivalent.Text.Trim()
        cmd.Parameters.Add("@PrixRevient", SqlDbType.Decimal).Value = NombreValeur(txtPrixRevient)
        cmd.Parameters.Add("@Stock", SqlDbType.Decimal).Value = NombreValeur(txtStock)
        cmd.Parameters.Add("@UniteDosage", SqlDbType.NVarChar, 20).Value = txtUnite.Text.Trim()
        cmd.Parameters.Add("@UniteStockage", SqlDbType.NVarChar, 20).Value = txtUnite.Text.Trim()
        cmd.Parameters.Add("@ToleranceAutomate", SqlDbType.Decimal).Value = NombreValeur(txtTolerance)
        cmd.Parameters.Add("@Densite", SqlDbType.Decimal).Value = NombreValeur(txtDensite)
        cmd.Parameters.Add("@DosageMaxM3", SqlDbType.Decimal).Value = NombreValeur(txtDosageMax)
        cmd.Parameters.Add("@TauxChlorure", SqlDbType.Decimal).Value = NombreValeur(txtTauxChlorure)
        cmd.Parameters.Add("@TauxAlcalin", SqlDbType.Decimal).Value = NombreValeur(txtTauxAlcalin)
        cmd.Parameters.Add("@AbsorptionCapillaire", SqlDbType.Decimal).Value = NombreValeur(txtAbsorptionCapillaire)
        cmd.Parameters.Add("@EstSable", SqlDbType.Bit).Value = BitValeur(txtSable)
        cmd.Parameters.Add("@DosageMiniBL", SqlDbType.Decimal).Value = NombreValeur(txtDosageMiniBL)
        cmd.Parameters.Add("@DosageMaxiBL", SqlDbType.Decimal).Value = NombreValeur(txtDosageMaxiBL)
        cmd.Parameters.Add("@GestionStocks", SqlDbType.Bit).Value = BitValeur(txtGestionStock)
    End Sub

    Private Sub AjouterParamsFacturation(cmd As SqlCommand, idMateriau As Integer)
        cmd.Parameters.Add("@IdMateriau", SqlDbType.Int).Value = idMateriau
        cmd.Parameters.Add("@CodeProduit", SqlDbType.NVarChar, 50).Value = txtCodeProduit.Text.Trim()
        cmd.Parameters.Add("@CleProduit", SqlDbType.NVarChar, 50).Value = txtCleRechercheProduit.Text.Trim()
        cmd.Parameters.Add("@NomProduit", SqlDbType.NVarChar, 200).Value = txtNomProduit.Text.Trim()
        cmd.Parameters.Add("@TVA", SqlDbType.Decimal).Value = NombreValeur(txtTVA)
        cmd.Parameters.Add("@Tarif1Date", SqlDbType.Date).Value = Date.Today
        cmd.Parameters.Add("@Tarif1Montant", SqlDbType.Decimal).Value = NombreValeur(txtTarif1)
        cmd.Parameters.Add("@Tarif1Unite", SqlDbType.NVarChar, 50).Value = txtUnite.Text.Trim()
        cmd.Parameters.Add("@Tarif2Date", SqlDbType.Date).Value = Date.Today
        cmd.Parameters.Add("@Tarif2Montant", SqlDbType.Decimal).Value = NombreValeur(txtTarif2)
        cmd.Parameters.Add("@Tarif2Unite", SqlDbType.NVarChar, 50).Value = txtUnite.Text.Trim()
    End Sub

    Private Function NombreValeur(txt As TextBox) As Decimal
        If txt.Text.Trim() = "" Then Return 0
        Dim v As Decimal
        Decimal.TryParse(txt.Text.Replace(",", "."),
            Globalization.NumberStyles.Any,
            Globalization.CultureInfo.InvariantCulture, v)
        Return v
    End Function

    Private Function BitValeur(txt As TextBox) As Boolean
        Dim val As String = txt.Text.Trim().ToUpper()
        Return val = "1" OrElse val = "OUI" OrElse val = "TRUE"
    End Function

    Private Sub ViderChamps()
        txtCodeMateriau.Text = "" : txtCleRecherche.Text = "" : txtNomMateriau.Text = ""
        cmbTypeMateriau.SelectedIndex = -1 : txtCodeMatierePremiere.Text = "" : txtMateriauEquivalent.Text = ""
        txtPrixRevient.Text = "" : txtDensite.Text = "" : txtStock.Text = "" : txtDosageMax.Text = ""
        txtTolerance.Text = "" : txtTauxChlorure.Text = "" : txtTauxAlcalin.Text = ""
        txtGestionStock.Text = "" : txtAbsorptionCapillaire.Text = "" : txtSable.Text = ""
        txtCodeProduit.Text = "" : txtCleRechercheProduit.Text = "" : txtNomProduit.Text = ""
        txtTVA.Text = "" : txtTarif1.Text = "" : txtTarif2.Text = "" : txtMontantUnitaire.Text = ""
        txtUnite.Text = "" : txtDosageMiniBL.Text = "" : txtDosageMaxiBL.Text = ""
        txtCodeMateriau.Focus()
    End Sub

    Private Sub Label17_Click(sender As Object, e As EventArgs) Handles lblNombre.Click

    End Sub
End Class