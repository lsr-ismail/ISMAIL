Imports System.Drawing.Printing
Imports Microsoft.Data.SqlClient
Imports System.Data

Public Class camions

    Private _idCamionCourant As Integer = 0

    ' ═══════════════════════════════════════════════════════════════════
    '  LOAD
    ' ═══════════════════════════════════════════════════════════════════

    Private Sub camions_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        ChargerCamions()
        DataGridView1.SelectionMode = DataGridViewSelectionMode.FullRowSelect
        DataGridView1.MultiSelect = False
        DataGridView1.ReadOnly = True
        DataGridView1.AllowUserToAddRows = False
    End Sub

    ' ═══════════════════════════════════════════════════════════════════
    '  CHARGEMENT
    ' ═══════════════════════════════════════════════════════════════════

    Private Sub ChargerCamions()
        Try
            Dim dt As DataTable = ModuleDb.Lire(
    "SELECT IdCamion, CodeCamion, CleCamion, Matricule, NomCamion, Fournisseur, " &
    "Famille, Equipement, Chauffeur, Capacite, ChargeUtile, " &
    "CodeRadio, Telephone, DerniereModification " &
    "FROM dbo.Camions ORDER BY NomCamion")

            DataGridView1.DataSource = dt
            DataGridView1.ReadOnly = True
            DataGridView1.AllowUserToAddRows = False
            DataGridView1.SelectionMode = DataGridViewSelectionMode.FullRowSelect

            ' NOUVEAU : cacher toutes les colonnes, puis ne montrer que Code / Matricule / Nom
            For Each col As DataGridViewColumn In DataGridView1.Columns
                col.Visible = False
            Next
            DataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.None

            If DataGridView1.Columns.Contains("CodeCamion") Then
                DataGridView1.Columns("CodeCamion").Visible = True
                DataGridView1.Columns("CodeCamion").HeaderText = "Code"
                DataGridView1.Columns("CodeCamion").Width = 95
            End If
            If DataGridView1.Columns.Contains("Matricule") Then
                DataGridView1.Columns("Matricule").Visible = True
                DataGridView1.Columns("Matricule").HeaderText = "Matricule"
                DataGridView1.Columns("Matricule").Width = 95
            End If

            If DataGridView1.Columns.Contains("NomCamion") Then
                DataGridView1.Columns("NomCamion").Visible = True
                DataGridView1.Columns("NomCamion").HeaderText = "Camion"
                DataGridView1.Columns("NomCamion").Width = 255
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
            _idCamionCourant = CInt(row.Cells("IdCamion").Value)

            txtCodeCamion.Text = LireTexte(row, "CodeCamion")
            txtCleCamion.Text = LireTexte(row, "CleCamion")
            txtNomCamion.Text = LireTexte(row, "NomCamion")
            txtMatricule.Text = LireTexte(row, "Matricule")
            txtFournisseur.Text = LireTexte(row, "Fournisseur")
            txtFamille.Text = LireTexte(row, "Famille")
            txtEquipement.Text = LireTexte(row, "Equipement")
            txtChauffeur.Text = LireTexte(row, "Chauffeur")
            txtCapacite.Text = LireTexte(row, "Capacite")
            txtChargeUtile.Text = LireTexte(row, "ChargeUtile")
            txtCodeRadio.Text = LireTexte(row, "CodeRadio")
            txtTelephone.Text = LireTexte(row, "Telephone")

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
        If txtCodeCamion.Text.Trim = "" OrElse txtNomCamion.Text.Trim = "" Then
            MessageBox.Show("Code et Nom sont obligatoires.")
            Exit Sub
        End If
        Try
            ModuleDb.Executer(
    "INSERT INTO dbo.Camions " &
    "(CodeCamion, CleCamion, Matricule, NomCamion, Fournisseur, Famille, Equipement, " &
    "Chauffeur, Capacite, ChargeUtile, CodeRadio, Telephone, DerniereModification) " &
    "VALUES (@CodeCamion, @CleCamion, @Matricule, @NomCamion, @Fournisseur, @Famille, @Equipement, " &
    "@Chauffeur, @Capacite, @ChargeUtile, @CodeRadio, @Telephone, @DerniereModification)",
    AddressOf AjouterParametres)


            MessageBox.Show("Camion ajouté avec succès.")
            ChargerCamions()
            ViderChamps()

        Catch ex As Exception
            MessageBox.Show("Erreur ajout : " & ex.Message)
        End Try
    End Sub

    ' ═══════════════════════════════════════════════════════════════════
    '  MODIFIER
    ' ═══════════════════════════════════════════════════════════════════

    Private Sub btnModifier_Click(sender As Object, e As EventArgs) Handles btnModifier.Click
        If _idCamionCourant = 0 Then
            MessageBox.Show("Sélectionner un camion à modifier.")
            Exit Sub
        End If
        Try
            ModuleDb.Executer(
    "UPDATE dbo.Camions SET " &
    "CodeCamion=@CodeCamion, CleCamion=@CleCamion, Matricule=@Matricule, NomCamion=@NomCamion, " &
    "Fournisseur=@Fournisseur, Famille=@Famille, Equipement=@Equipement, " &
    "Chauffeur=@Chauffeur, Capacite=@Capacite, ChargeUtile=@ChargeUtile, " &
    "CodeRadio=@CodeRadio, Telephone=@Telephone, DerniereModification=@DerniereModification " &
    "WHERE IdCamion=@IdCamion",
    Sub(cmd)
        cmd.Parameters.Add("@IdCamion", SqlDbType.Int).Value = _idCamionCourant
        AjouterParametres(cmd)
    End Sub)

            MessageBox.Show("Camion modifié avec succès.")
            ChargerCamions()
            ViderChamps()

        Catch ex As Exception
            MessageBox.Show("Erreur modification : " & ex.Message)
        End Try
    End Sub

    ' ═══════════════════════════════════════════════════════════════════
    '  SUPPRIMER
    ' ═══════════════════════════════════════════════════════════════════

    Private Sub btnSupprimer_Click(sender As Object, e As EventArgs) Handles btnSupprimer.Click
        If _idCamionCourant = 0 Then
            MessageBox.Show("Sélectionner un camion à supprimer.")
            Exit Sub
        End If
        If MessageBox.Show("Voulez-vous supprimer ce camion ?", "Confirmation",
                           MessageBoxButtons.YesNo, MessageBoxIcon.Question) = DialogResult.No Then
            Exit Sub
        End If
        Try
            ModuleDb.Executer(
                "DELETE FROM dbo.Camions WHERE IdCamion=@IdCamion",
                Sub(cmd)
                    cmd.Parameters.Add("@IdCamion", SqlDbType.Int).Value = _idCamionCourant
                End Sub)

            MessageBox.Show("Camion supprimé avec succès.")
            ChargerCamions()
            ViderChamps()

        Catch ex As Exception
            MessageBox.Show("Erreur suppression : " & ex.Message)
        End Try
    End Sub

    ' ═══════════════════════════════════════════════════════════════════
    '  RECHERCHE (temps réel)
    ' ═══════════════════════════════════════════════════════════════════

    Private Sub txtRECHERCHE_TextChanged(sender As Object, e As EventArgs) Handles txtRECHERCHE.TextChanged
        RECHERCHE.PerformClick()
    End Sub

    Private Sub RECHERCHE_Click(sender As Object, e As EventArgs) Handles RECHERCHE.Click
        Try
            Dim dt As DataTable = ModuleDb.Lire(
                "SELECT IdCamion, CodeCamion, CleCamion, Matricule, NomCamion, Fournisseur, " &
"Famille, Equipement, Chauffeur, Capacite, ChargeUtile, " &
"CodeRadio, Telephone, DerniereModification " &
"FROM dbo.Camions WHERE CodeCamion LIKE @mot OR NomCamion LIKE @mot " &
"ORDER BY NomCamion",
                Sub(cmd)
                    cmd.Parameters.Add("@mot", SqlDbType.NVarChar, 200).Value = "%" & txtRECHERCHE.Text.Trim & "%"
                End Sub)

            DataGridView1.DataSource = dt
            If DataGridView1.Columns.Contains("IdCamion") Then
                DataGridView1.Columns("IdCamion").Visible = False
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
        ChargerCamions()
    End Sub

    ' ═══════════════════════════════════════════════════════════════════
    '  IMPRESSION (inchangée)
    ' ═══════════════════════════════════════════════════════════════════

    Private Sub Imprimer_Click(sender As Object, e As EventArgs) Handles Imprimer.Click
        If _idCamionCourant = 0 Then
            MessageBox.Show("Sélectionner un camion à imprimer.")
            Exit Sub
        End If
        PrintPreviewDialog1.Document = PrintDocument1
        PrintPreviewDialog1.ShowDialog()
    End Sub

    Private Sub PrintDocument1_PrintPage(sender As Object, e As PrintPageEventArgs) Handles PrintDocument1.PrintPage
        Dim fTitre As New Font("Arial", 16, FontStyle.Bold)
        Dim fLabel As New Font("Arial", 11, FontStyle.Bold)
        Dim fTxt As New Font("Arial", 11)
        Dim x As Integer = 50
        Dim y As Integer = 50
        Dim esp As Integer = 28

        e.Graphics.DrawString("FICHE CAMION", fTitre, Brushes.Black, x, y)
        e.Graphics.DrawString("Date : " & DateTime.Now.ToString("dd/MM/yyyy HH:mm"),
                              New Font("Arial", 9, FontStyle.Italic), Brushes.Gray, x, y + 35)
        y += 80
        e.Graphics.DrawLine(Pens.Black, x, y, 700, y)
        y += 15

        Dim champs = {
            ("Code Camion :", txtCodeCamion.Text),
            ("Clé Camion :", txtCleCamion.Text),
            ("Nom Camion :", txtNomCamion.Text),
            ("Matricule :", txtMatricule.Text),
            ("Fournisseur :", txtFournisseur.Text),
            ("Famille :", txtFamille.Text),
            ("Equipement :", txtEquipement.Text),
            ("Chauffeur :", txtChauffeur.Text),
            ("Capacité (m³) :", txtCapacite.Text),
            ("Charge Utile :", txtChargeUtile.Text),
            ("Code Radio :", txtCodeRadio.Text),
            ("Téléphone :", txtTelephone.Text)
        }
        For Each c In champs
            e.Graphics.DrawString(c.Item1, fLabel, Brushes.Black, x, y)
            e.Graphics.DrawString(c.Item2, fTxt, Brushes.Black, x + 160, y)
            y += esp
        Next

        e.Graphics.DrawLine(Pens.LightGray, x, y + 10, 700, y + 10)
        e.Graphics.DrawString("Centrale à Béton — Gestion des camions",
                              New Font("Arial", 8, FontStyle.Italic), Brushes.Gray, x, y + 20)
    End Sub

    ' ═══════════════════════════════════════════════════════════════════
    '  HELPERS
    ' ═══════════════════════════════════════════════════════════════════

    Private Sub AjouterParametres(cmd As SqlCommand)
        cmd.Parameters.Add("@CodeCamion", SqlDbType.NVarChar, 50).Value = ValTexte(txtCodeCamion.Text)
        cmd.Parameters.Add("@CleCamion", SqlDbType.NVarChar, 50).Value = ValTexte(txtCleCamion.Text)

        cmd.Parameters.Add("@Matricule", SqlDbType.NVarChar, 20).Value = ValTexte(txtMatricule.Text)
        cmd.Parameters.Add("@NomCamion", SqlDbType.NVarChar, 200).Value = ValTexte(txtNomCamion.Text)
        cmd.Parameters.Add("@Fournisseur", SqlDbType.NVarChar, 100).Value = ValTexte(txtFournisseur.Text)
        cmd.Parameters.Add("@Famille", SqlDbType.NVarChar, 100).Value = ValTexte(txtFamille.Text)
        cmd.Parameters.Add("@Equipement", SqlDbType.NVarChar, 100).Value = ValTexte(txtEquipement.Text)
        cmd.Parameters.Add("@Chauffeur", SqlDbType.NVarChar, 100).Value = ValTexte(txtChauffeur.Text)
        cmd.Parameters.Add("@Capacite", SqlDbType.Decimal).Value = ParseDecimal(txtCapacite.Text)
        cmd.Parameters.Add("@ChargeUtile", SqlDbType.Decimal).Value = ParseDecimal(txtChargeUtile.Text)
        cmd.Parameters.Add("@CodeRadio", SqlDbType.NVarChar, 50).Value = ValTexte(txtCodeRadio.Text)
        cmd.Parameters.Add("@Telephone", SqlDbType.NVarChar, 30).Value = ValTexte(txtTelephone.Text)
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
        _idCamionCourant = 0
        txtCodeCamion.Text = "" : txtCleCamion.Text = "" : txtNomCamion.Text = ""
        txtMatricule.Text = ""
        txtFournisseur.Text = "" : txtFamille.Text = "" : txtEquipement.Text = ""
        txtChauffeur.Text = "" : txtCapacite.Text = "" : txtChargeUtile.Text = ""
        txtCodeRadio.Text = "" : txtTelephone.Text = ""
        dtpDerniereModification.Text = Date.Today.ToString("dd/MM/yyyy")
        txtCodeCamion.Focus()
    End Sub

    Private Sub dtpDerniereModification_TextChanged(sender As Object, e As EventArgs) Handles dtpDerniereModification.TextChanged
    End Sub

End Class