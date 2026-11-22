Imports Microsoft.Data.SqlClient
Imports System.Drawing.Printing
Imports System.Data

Public Class Bons_de_livraison

    Private _idLivraisonCourant As Integer = 0
    Private _idPlanningCourant As Integer = 0

    ' ═══════════════════════════════════════════════════════════════════
    '  LOAD
    ' ═══════════════════════════════════════════════════════════════════

    Private Sub Bons_de_livraison_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        dtpDateBL.Value = Date.Today
        StyleGrille()
        ChargerBonsLivraison()
    End Sub

    Private Sub StyleGrille()
        With DataGridView1
            .ReadOnly = True
            .AllowUserToAddRows = False
            .SelectionMode = DataGridViewSelectionMode.FullRowSelect
            .MultiSelect = False
            .AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.None
            .RowHeadersVisible = False
            .EnableHeadersVisualStyles = False
            .ColumnHeadersDefaultCellStyle.BackColor = Color.LightGray
            .ColumnHeadersDefaultCellStyle.ForeColor = Color.Black
            .DefaultCellStyle.SelectionBackColor = Color.RoyalBlue
            .DefaultCellStyle.SelectionForeColor = Color.White
        End With
    End Sub

    ' ═══════════════════════════════════════════════════════════════════
    '  CHARGEMENT
    ' ═══════════════════════════════════════════════════════════════════

    Private Sub ChargerBonsLivraison()
        Try
            Dim dt As DataTable = ModuleDb.Lire(
                "SELECT bl.IdLivraison, bl.NumeroBL, bl.NumeroTour, " &
                "bl.HeureFabrication, bl.HeureLivraison, " &
                "cl.NomClient AS CLIENT, ch.NomChantier AS CHANTIER, " &
                "f.CodeFormule AS FORMULE, bl.Vehicule, " &
                "bl.QuantiteM3, bl.CumulM3, bl.Operateur, " &
                "bl.Observation, p.DatePlanning " &
                "FROM dbo.PlanningLivraisons bl " &
                "JOIN dbo.Planning_journalier p ON p.IdPlanning = bl.IdPlanning " &
                "JOIN dbo.Clients cl ON cl.IdClient = p.IdClient " &
                "JOIN dbo.Chantier ch ON ch.IdChantier = p.IdChantier " &
                "JOIN dbo.Formules f ON f.IdFormule = p.IdFormule " &
                "WHERE p.DatePlanning = @DateBL " &
                "ORDER BY bl.HeureFabrication, bl.NumeroTour",
                Sub(cmd)
                    cmd.Parameters.Add("@DateBL", SqlDbType.Date).Value = dtpDateBL.Value.Date
                End Sub)

            DataGridView1.DataSource = dt

            For Each col As String In {"IdLivraison", "DatePlanning"}
                If DataGridView1.Columns.Contains(col) Then
                    DataGridView1.Columns(col).Visible = False
                End If
            Next

            Dim entetes = New Dictionary(Of String, String) From {
                {"NumeroBL", "N° BL"}, {"NumeroTour", "Tour"},
                {"HeureFabrication", "H.Fab"}, {"HeureLivraison", "H.Liv"},
                {"CLIENT", "Client"}, {"CHANTIER", "Chantier"},
                {"FORMULE", "Formule"}, {"Vehicule", "Véhicule"},
                {"QuantiteM3", "M³"}, {"CumulM3", "Cumul M³"},
                {"Operateur", "Opérateur"}, {"Observation", "Observation"}
            }
            For Each kv In entetes
                If DataGridView1.Columns.Contains(kv.Key) Then
                    DataGridView1.Columns(kv.Key).HeaderText = kv.Value
                End If
            Next

            Dim largeurs = New Dictionary(Of String, Integer) From {
                {"NumeroBL", 80}, {"NumeroTour", 50},
                {"HeureFabrication", 70}, {"HeureLivraison", 70},
                {"CLIENT", 150}, {"CHANTIER", 150},
                {"FORMULE", 80}, {"Vehicule", 100},
                {"QuantiteM3", 70}, {"CumulM3", 80},
                {"Operateur", 100}, {"Observation", 150}
            }
            For Each kv In largeurs
                If DataGridView1.Columns.Contains(kv.Key) Then
                    DataGridView1.Columns(kv.Key).Width = kv.Value
                End If
            Next

            Dim totalM3 As Decimal = 0
            For Each row As DataRow In dt.Rows
                If Not IsDBNull(row("QuantiteM3")) Then
                    totalM3 += Convert.ToDecimal(row("QuantiteM3"))
                End If
            Next
            lblNombre.Text = "Total : " & dt.Rows.Count & " BL"
            lblTotalM3.Text = "Total M³ : " & totalM3.ToString("0.00")

        Catch ex As Exception
            MessageBox.Show("Erreur chargement BL : " & ex.Message)
        End Try
    End Sub

    ' ═══════════════════════════════════════════════════════════════════
    '  CLIC GRILLE
    ' ═══════════════════════════════════════════════════════════════════

    Private Sub DataGridView1_CellClick(sender As Object, e As DataGridViewCellEventArgs) Handles DataGridView1.CellClick
        If e.RowIndex < 0 Then Exit Sub
        Try
            Dim row As DataGridViewRow = DataGridView1.Rows(e.RowIndex)
            _idLivraisonCourant = CInt(row.Cells("IdLivraison").Value)
            txtNumeroBL.Text = LireTexte(row, "NumeroBL")
            txtNumeroTour.Text = LireTexte(row, "NumeroTour")
            txtVehicule.Text = LireTexte(row, "Vehicule")
            txtQuantiteM3.Text = LireTexte(row, "QuantiteM3")
            txtCumulM3.Text = LireTexte(row, "CumulM3")
            txtOperateur.Text = LireTexte(row, "Operateur")
            txtObservation.Text = LireTexte(row, "Observation")
        Catch ex As Exception
            MessageBox.Show("Erreur sélection : " & ex.Message)
        End Try
    End Sub

    ' ═══════════════════════════════════════════════════════════════════
    '  AJOUTER
    ' ═══════════════════════════════════════════════════════════════════

    Private Sub btnAjouter_Click(sender As Object, e As EventArgs) Handles btnAjouter.Click
        If _idPlanningCourant = 0 Then
            MessageBox.Show("Sélectionner un planning d'abord.")
            Exit Sub
        End If
        If txtQuantiteM3.Text.Trim = "" Then
            MessageBox.Show("Quantité M³ obligatoire.")
            txtQuantiteM3.Focus()
            Exit Sub
        End If
        Try
            Dim numeroTour As Integer = ModuleDb.LireValeur(Of Integer)(
                "SELECT ISNULL(MAX(NumeroTour), 0) + 1 FROM dbo.PlanningLivraisons WHERE IdPlanning=@Id",
                Sub(cmd) cmd.Parameters.Add("@Id", SqlDbType.Int).Value = _idPlanningCourant)

            Dim cumul As Decimal = ModuleDb.LireValeur(Of Decimal)(
                "SELECT ISNULL(SUM(QuantiteM3), 0) FROM dbo.PlanningLivraisons WHERE IdPlanning=@Id",
                Sub(cmd) cmd.Parameters.Add("@Id", SqlDbType.Int).Value = _idPlanningCourant)
            cumul += ParseDecimal(txtQuantiteM3.Text)

            Dim numeroBL As String = "BL-" & Date.Today.ToString("yyyyMMdd") & "-" & numeroTour.ToString("D3")

            ModuleDb.Executer(
                "INSERT INTO dbo.PlanningLivraisons " &
                "(IdPlanning, NumeroTour, NumeroBL, Vehicule, QuantiteM3, " &
                "HeureFabrication, HeureLivraison, CumulM3, Operateur, Observation) " &
                "VALUES (@IdPlanning, @NumeroTour, @NumeroBL, @Vehicule, @QuantiteM3, " &
                "@HeureFabrication, @HeureLivraison, @CumulM3, @Operateur, @Observation)",
                Sub(cmd)
                    cmd.Parameters.Add("@IdPlanning", SqlDbType.Int).Value = _idPlanningCourant
                    cmd.Parameters.Add("@NumeroTour", SqlDbType.Int).Value = numeroTour
                    cmd.Parameters.Add("@NumeroBL", SqlDbType.NVarChar, 50).Value = numeroBL
                    cmd.Parameters.Add("@Vehicule", SqlDbType.NVarChar, 100).Value = If(txtVehicule.Text.Trim = "", DBNull.Value, CObj(txtVehicule.Text.Trim()))
                    cmd.Parameters.Add("@QuantiteM3", SqlDbType.Decimal).Value = ParseDecimal(txtQuantiteM3.Text)
                    cmd.Parameters.Add("@HeureFabrication", SqlDbType.Time).Value = TimeSpan.FromTicks(Date.Now.TimeOfDay.Ticks)
                    cmd.Parameters.Add("@HeureLivraison", SqlDbType.Time).Value = TimeSpan.FromTicks(Date.Now.TimeOfDay.Ticks)
                    cmd.Parameters.Add("@CumulM3", SqlDbType.Decimal).Value = cumul
                    cmd.Parameters.Add("@Operateur", SqlDbType.NVarChar, 100).Value = If(txtOperateur.Text.Trim = "", CObj(NomUtilisateur), CObj(txtOperateur.Text.Trim()))
                    cmd.Parameters.Add("@Observation", SqlDbType.NVarChar, 300).Value = If(txtObservation.Text.Trim = "", DBNull.Value, CObj(txtObservation.Text.Trim()))
                End Sub)

            ModuleDb.Executer(
                "UPDATE dbo.Planning_journalier SET QuantiteLivree=@Cumul WHERE IdPlanning=@Id",
                Sub(cmd)
                    cmd.Parameters.Add("@Cumul", SqlDbType.Decimal).Value = cumul
                    cmd.Parameters.Add("@Id", SqlDbType.Int).Value = _idPlanningCourant
                End Sub)
            Synoptique.EnregistrerAction("LIVRAISON", "BL créé", "BL " & numeroBL & " — Véhicule=" & txtVehicule.Text.Trim() & " Vol=" & ParseDecimal(txtQuantiteM3.Text).ToString("0.00") & "m3", numeroBL, txtQuantiteM3.Text, "CREATION_BL")
            MessageBox.Show("BL N° " & numeroBL & " enregistré avec succès.")
            ChargerBonsLivraison()
            ViderChamps()

        Catch ex As Exception
            MessageBox.Show("Erreur ajout BL : " & ex.Message)
        End Try
    End Sub

    ' ═══════════════════════════════════════════════════════════════════
    '  MODIFIER
    ' ═══════════════════════════════════════════════════════════════════

    Private Sub btnModifier_Click(sender As Object, e As EventArgs) Handles btnModifier.Click
        If _idLivraisonCourant = 0 Then
            MessageBox.Show("Sélectionner un BL à modifier.")
            Exit Sub
        End If
        Try
            ModuleDb.Executer(
                "UPDATE dbo.PlanningLivraisons SET " &
                "Vehicule=@Vehicule, QuantiteM3=@QuantiteM3, " &
                "Operateur=@Operateur, Observation=@Observation " &
                "WHERE IdLivraison=@IdLivraison",
                Sub(cmd)
                    cmd.Parameters.Add("@Vehicule", SqlDbType.NVarChar, 100).Value = If(txtVehicule.Text.Trim = "", DBNull.Value, CObj(txtVehicule.Text.Trim()))
                    cmd.Parameters.Add("@QuantiteM3", SqlDbType.Decimal).Value = ParseDecimal(txtQuantiteM3.Text)
                    cmd.Parameters.Add("@Operateur", SqlDbType.NVarChar, 100).Value = If(txtOperateur.Text.Trim = "", DBNull.Value, CObj(txtOperateur.Text.Trim()))
                    cmd.Parameters.Add("@Observation", SqlDbType.NVarChar, 300).Value = If(txtObservation.Text.Trim = "", DBNull.Value, CObj(txtObservation.Text.Trim()))
                    cmd.Parameters.Add("@IdLivraison", SqlDbType.Int).Value = _idLivraisonCourant
                End Sub)
            Synoptique.EnregistrerAction("LIVRAISON", "BL modifié", "BL ID=" & _idLivraisonCourant & " — Véhicule=" & txtVehicule.Text.Trim() & " Vol=" & ParseDecimal(txtQuantiteM3.Text).ToString("0.00") & "m3", txtNumeroBL.Text, txtQuantiteM3.Text, "MODIF_BL")
            MessageBox.Show("BL modifié avec succès.")
            ChargerBonsLivraison()
            ViderChamps()

        Catch ex As Exception
            MessageBox.Show("Erreur modification : " & ex.Message)
        End Try
    End Sub

    ' ═══════════════════════════════════════════════════════════════════
    '  SUPPRIMER
    ' ═══════════════════════════════════════════════════════════════════

    Private Sub btnSupprimer_Click(sender As Object, e As EventArgs) Handles btnSupprimer.Click
        If _idLivraisonCourant = 0 Then
            MessageBox.Show("Sélectionner un BL à supprimer.")
            Exit Sub
        End If
        If MessageBox.Show("Supprimer ce BL ?", "Confirmation",
                           MessageBoxButtons.YesNo, MessageBoxIcon.Question) = DialogResult.No Then Exit Sub
        Try
            ModuleDb.Executer(
                "DELETE FROM dbo.PlanningLivraisons WHERE IdLivraison=@IdLivraison",
                Sub(cmd)
                    cmd.Parameters.Add("@IdLivraison", SqlDbType.Int).Value = _idLivraisonCourant
                End Sub)
            Synoptique.EnregistrerAction("LIVRAISON", "BL supprimé", "BL " & txtNumeroBL.Text & " — ID=" & _idLivraisonCourant, txtNumeroBL.Text, "", "SUPPRESSION_BL")
            MessageBox.Show("BL supprimé avec succès.")
            ChargerBonsLivraison()
            ViderChamps()

        Catch ex As Exception
            MessageBox.Show("Erreur suppression : " & ex.Message)
        End Try
    End Sub

    ' ═══════════════════════════════════════════════════════════════════
    '  CHOISIR PLANNING
    ' ═══════════════════════════════════════════════════════════════════

    Private Sub btnChoisirPlanning_Click(sender As Object, e As EventArgs) Handles btnChoisirPlanning.Click
        Dim f As New Planning_journalier
        f.ShowDialog()
        If f.IdPlanningEnCours > 0 Then
            _idPlanningCourant = f.IdPlanningEnCours
            txtPlanningSelectionne.Text = "Planning N° " & _idPlanningCourant
        End If
    End Sub

    ' ═══════════════════════════════════════════════════════════════════
    '  IMPRESSION
    ' ═══════════════════════════════════════════════════════════════════

    Private WithEvents pdBL As New PrintDocument
    Private _blImprimer As DataGridViewRow = Nothing

    Private Sub btnImprimer_Click(sender As Object, e As EventArgs) Handles btnImprimer.Click
        If _idLivraisonCourant = 0 Then
            MessageBox.Show("Sélectionner un BL à imprimer.")
            Exit Sub
        End If
        _blImprimer = DataGridView1.CurrentRow
        Dim apercu As New PrintPreviewDialog
        apercu.Document = pdBL
        apercu.WindowState = FormWindowState.Maximized
        apercu.ShowDialog()
    End Sub

    Private Sub pdBL_PrintPage(sender As Object, e As PrintPageEventArgs) Handles pdBL.PrintPage
        Dim g As Graphics = e.Graphics
        Dim fontTitre As New Font("Arial", 16, FontStyle.Bold)
        Dim fontLabel As New Font("Arial", 10, FontStyle.Bold)
        Dim fontValeur As New Font("Arial", 10)
        Dim x As Integer = 40 : Dim y As Integer = 30
        Dim col As Integer = 200 : Dim esp As Integer = 28

        g.FillRectangle(New SolidBrush(Color.FromArgb(10, 50, 120)), x, y, 720, 60)
        g.DrawString(SocieteNom, New Font("Arial", 13, FontStyle.Bold), Brushes.White, x + 10, y + 5)
        g.DrawString(SocieteAdresse & "  Tél: " & SocieteTelephone, New Font("Arial", 8), Brushes.LightGray, x + 10, y + 30)
        g.DrawString("ICE: " & SocieteICE & "  RC: " & SocieteRC, New Font("Arial", 8), Brushes.LightGray, x + 400, y + 30)
        y += 75

        g.DrawString("BON DE LIVRAISON", fontTitre, Brushes.DarkBlue, 220, y) : y += 35
        g.DrawLine(New Pen(Color.DarkBlue, 2), x, y, 760, y) : y += 15

        If _blImprimer Is Nothing Then Exit Sub

        Dim champs = {
            ("N° BL :", LireTexte(_blImprimer, "NumeroBL")),
            ("Tour N° :", LireTexte(_blImprimer, "NumeroTour")),
            ("Client :", LireTexte(_blImprimer, "CLIENT")),
            ("Chantier :", LireTexte(_blImprimer, "CHANTIER")),
            ("Formule :", LireTexte(_blImprimer, "FORMULE")),
            ("Véhicule :", LireTexte(_blImprimer, "Vehicule")),
            ("Quantité M³ :", LireTexte(_blImprimer, "QuantiteM3")),
            ("Cumul M³ :", LireTexte(_blImprimer, "CumulM3")),
            ("H. Fabrication :", LireTexte(_blImprimer, "HeureFabrication")),
            ("H. Livraison :", LireTexte(_blImprimer, "HeureLivraison")),
            ("Opérateur :", LireTexte(_blImprimer, "Operateur")),
            ("Observation :", LireTexte(_blImprimer, "Observation"))
        }
        For Each c In champs
            g.DrawString(c.Item1, fontLabel, Brushes.Black, x, y)
            g.DrawString(c.Item2, fontValeur, Brushes.Black, col, y)
            y += esp
        Next

        y += 30
        g.DrawLine(Pens.LightGray, x, y, 760, y) : y += 20
        g.DrawString("Chauffeur : ___________________", fontLabel, Brushes.Black, x, y)
        g.DrawString("Responsable : ___________________", fontLabel, Brushes.Black, 430, y)
        g.FillRectangle(New SolidBrush(Color.FromArgb(10, 50, 120)), x, 790, 720, 15)
        g.DrawString(SocieteNom & " | " & SocieteAdresse & " | ICE: " & SocieteICE,
                     New Font("Arial", 7), Brushes.White, x + 5, 793)
    End Sub

    ' ═══════════════════════════════════════════════════════════════════
    '  NAVIGATION DATE
    ' ═══════════════════════════════════════════════════════════════════

    Private Sub btnJourPlus_Click(sender As Object, e As EventArgs) Handles btnJourPlus.Click
        dtpDateBL.Value = dtpDateBL.Value.AddDays(1)
        ChargerBonsLivraison()
    End Sub

    Private Sub btnJourMoins_Click(sender As Object, e As EventArgs) Handles btnJourMoins.Click
        dtpDateBL.Value = dtpDateBL.Value.AddDays(-1)
        ChargerBonsLivraison()
    End Sub

    Private Sub btnActualiser_Click(sender As Object, e As EventArgs) Handles btnActualiser.Click
        ChargerBonsLivraison()
    End Sub

    Private Sub dtpDateBL_ValueChanged(sender As Object, e As EventArgs) Handles dtpDateBL.ValueChanged
        ChargerBonsLivraison()
    End Sub

    ' ═══════════════════════════════════════════════════════════════════
    '  RECHERCHE
    ' ═══════════════════════════════════════════════════════════════════

    Private Sub txtRECHERCHE_TextChanged(sender As Object, e As EventArgs) Handles txtRECHERCHE.TextChanged
        If txtRECHERCHE.Text.Trim = "" Then
            ChargerBonsLivraison()
            Exit Sub
        End If
        Try
            Dim dt As DataTable = ModuleDb.Lire(
                "SELECT bl.IdLivraison, bl.NumeroBL, bl.NumeroTour, " &
                "bl.HeureFabrication, bl.HeureLivraison, " &
                "cl.NomClient AS CLIENT, ch.NomChantier AS CHANTIER, " &
                "f.CodeFormule AS FORMULE, bl.Vehicule, " &
                "bl.QuantiteM3, bl.CumulM3, bl.Operateur, " &
                "bl.Observation, p.DatePlanning " &
                "FROM dbo.PlanningLivraisons bl " &
                "JOIN dbo.Planning_journalier p ON p.IdPlanning = bl.IdPlanning " &
                "JOIN dbo.Clients cl ON cl.IdClient = p.IdClient " &
                "JOIN dbo.Chantier ch ON ch.IdChantier = p.IdChantier " &
                "JOIN dbo.Formules f ON f.IdFormule = p.IdFormule " &
                "WHERE bl.NumeroBL LIKE @mot OR cl.NomClient LIKE @mot " &
                "OR ch.NomChantier LIKE @mot OR bl.Vehicule LIKE @mot " &
                "ORDER BY bl.HeureFabrication",
                Sub(cmd)
                    cmd.Parameters.Add("@mot", SqlDbType.NVarChar, 200).Value = "%" & txtRECHERCHE.Text.Trim() & "%"
                End Sub)

            DataGridView1.DataSource = dt
            If DataGridView1.Columns.Contains("IdLivraison") Then
                DataGridView1.Columns("IdLivraison").Visible = False
            End If

        Catch ex As Exception
            MessageBox.Show("Erreur recherche : " & ex.Message)
        End Try
    End Sub

    ' ═══════════════════════════════════════════════════════════════════
    '  HELPERS
    ' ═══════════════════════════════════════════════════════════════════

    Private Function LireTexte(row As DataGridViewRow, col As String) As String
        Return If(IsDBNull(row.Cells(col).Value), "", row.Cells(col).Value.ToString())
    End Function

    Private Function ParseDecimal(txt As String) As Decimal
        Dim v As Decimal
        Decimal.TryParse(txt.Replace(",", "."),
            Globalization.NumberStyles.Any,
            Globalization.CultureInfo.InvariantCulture, v)
        Return v
    End Function

    Private Sub ViderChamps()
        _idLivraisonCourant = 0
        txtNumeroBL.Text = ""
        txtNumeroTour.Text = ""
        txtVehicule.Text = ""
        txtQuantiteM3.Text = ""
        txtCumulM3.Text = ""
        txtOperateur.Text = NomUtilisateur
        txtObservation.Text = ""
    End Sub

End Class