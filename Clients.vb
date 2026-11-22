Imports Microsoft.Data.SqlClient
Imports System.Data
Imports System.Drawing.Printing

Public Class Clients
    Private IdClientSelectionne As Integer = -1
    Private WithEvents pd As New PrintDocument

    ' ═══════════════════════════════════════════════════════════════════
    '  CHARGEMENT
    ' ═══════════════════════════════════════════════════════════════════

    Public Sub ChargerClients()
        Try
            Dim dt As DataTable = ModuleDb.Lire(
                "SELECT IdClient, CodeClient, NomClient, Adresse, CodePostal, Pays, " &
                "ICE, Telephone, Fax, Email, Zone, Radial, Contacts, Remarques, " &
                "ControleCredit, AutorisationDosage, Verrouille " &
                "FROM dbo.Clients ORDER BY NomClient")

            DataGridView1.DataSource = dt
            DataGridView1.ReadOnly = True
            DataGridView1.AllowUserToAddRows = False
            DataGridView1.SelectionMode = DataGridViewSelectionMode.FullRowSelect

            For Each col As DataGridViewColumn In DataGridView1.Columns
                col.Visible = False
            Next
            If DataGridView1.Columns.Contains("CodeClient") Then
                DataGridView1.Columns("CodeClient").Visible = True
                DataGridView1.Columns("CodeClient").HeaderText = "Code"
                DataGridView1.Columns("CodeClient").Width = 120
            End If
            If DataGridView1.Columns.Contains("NomClient") Then
                DataGridView1.Columns("NomClient").Visible = True
                DataGridView1.Columns("NomClient").HeaderText = "Nom"
                DataGridView1.Columns("NomClient").Width = 250
            End If
            If DataGridView1.Columns.Contains("Telephone") Then
                DataGridView1.Columns("Telephone").Visible = True
                DataGridView1.Columns("Telephone").HeaderText = "Téléphone"
                DataGridView1.Columns("Telephone").Width = 150
            End If
        Catch ex As Exception
            MessageBox.Show("Erreur chargement : " & ex.Message)
        End Try
    End Sub

    Private Sub Clients_Load(sender As Object, e As EventArgs) Handles MyBase.Load

        Select Case RoleUtilisateur
            Case "Admin", "Super"
                AJOUTE.Visible = True : AJOUTE.Enabled = True
                EDITE.Visible = True : EDITE.Enabled = True
                SUPRIME.Visible = True : SUPRIME.Enabled = True
                BTN_CHANTIER.Visible = True : BTN_CHANTIER.Enabled = True

            Case "Operateur"
                AJOUTE.Visible = True : AJOUTE.Enabled = False
                EDITE.Visible = True : EDITE.Enabled = False
                SUPRIME.Visible = True : SUPRIME.Enabled = False
                BTN_CHANTIER.Visible = True : BTN_CHANTIER.Enabled = True

            Case Else
                ' Rôle inconnu / non défini → accès le plus restreint par sécurité
                AJOUTE.Visible = True : AJOUTE.Enabled = False
                EDITE.Visible = True : EDITE.Enabled = False
                SUPRIME.Visible = True : SUPRIME.Enabled = False
                BTN_CHANTIER.Visible = True : BTN_CHANTIER.Enabled = False
        End Select


        ChargerClients()
    End Sub

    ' ═══════════════════════════════════════════════════════════════════
    '  VIDER CHAMPS
    ' ═══════════════════════════════════════════════════════════════════

    Public Sub ViderChamps()
        TextBoxId.Clear()
        TextBoxCode.Clear() : TextBoxNom.Clear() : TextBoxAdresse.Clear()
        TextBoxCodePostal.Clear() : TextBoxPays.Clear() : TextBoxTVA.Clear()
        TextBoxTelephone.Clear() : TextBoxFax.Clear() : TextBoxEmail.Clear()
        TextBoxZone.Clear() : TextBoxRadial.Clear() : TextBoxContacts.Clear()
        TextBoxRemarques.Clear()
        CheckBoxControleCredit.Checked = False
        CheckBoxAutorisationDosage.Checked = False
        CheckBoxVerrouille.Checked = False
        IdClientSelectionne = -1   ' NOUVEAU
    End Sub

    ' ═══════════════════════════════════════════════════════════════════
    '  CLIC GRILLE
    ' ═══════════════════════════════════════════════════════════════════

    Private Sub DataGridView1_CellClick(sender As Object, e As DataGridViewCellEventArgs) Handles DataGridView1.CellClick
        If e.RowIndex < 0 Then Exit Sub
        Try
            Dim row As DataGridViewRow = DataGridView1.Rows(e.RowIndex)
            TextBoxId.Text = If(IsDBNull(row.Cells("IdClient").Value), "", row.Cells("IdClient").Value.ToString())

            IdClientSelectionne = If(IsDBNull(row.Cells("IdClient").Value), -1, Convert.ToInt32(row.Cells("IdClient").Value))

            TextBoxCode.Text = If(IsDBNull(row.Cells("CodeClient").Value), "", row.Cells("CodeClient").Value.ToString())
            TextBoxNom.Text = If(IsDBNull(row.Cells("NomClient").Value), "", row.Cells("NomClient").Value.ToString())
            TextBoxAdresse.Text = If(IsDBNull(row.Cells("Adresse").Value), "", row.Cells("Adresse").Value.ToString())
            TextBoxCodePostal.Text = If(IsDBNull(row.Cells("CodePostal").Value), "", row.Cells("CodePostal").Value.ToString())
            TextBoxPays.Text = If(IsDBNull(row.Cells("Pays").Value), "", row.Cells("Pays").Value.ToString())
            TextBoxTVA.Text = If(IsDBNull(row.Cells("ICE").Value), "", row.Cells("ICE").Value.ToString())
            TextBoxTelephone.Text = If(IsDBNull(row.Cells("Telephone").Value), "", row.Cells("Telephone").Value.ToString())
            TextBoxFax.Text = If(IsDBNull(row.Cells("Fax").Value), "", row.Cells("Fax").Value.ToString())
            TextBoxEmail.Text = If(IsDBNull(row.Cells("Email").Value), "", row.Cells("Email").Value.ToString())
            TextBoxZone.Text = If(IsDBNull(row.Cells("Zone").Value), "", row.Cells("Zone").Value.ToString())
            TextBoxRadial.Text = If(IsDBNull(row.Cells("Radial").Value), "", row.Cells("Radial").Value.ToString())
            TextBoxContacts.Text = If(IsDBNull(row.Cells("Contacts").Value), "", row.Cells("Contacts").Value.ToString())
            TextBoxRemarques.Text = If(IsDBNull(row.Cells("Remarques").Value), "", row.Cells("Remarques").Value.ToString())
            CheckBoxControleCredit.Checked = Not IsDBNull(row.Cells("ControleCredit").Value) AndAlso Convert.ToBoolean(row.Cells("ControleCredit").Value)
            CheckBoxAutorisationDosage.Checked = Not IsDBNull(row.Cells("AutorisationDosage").Value) AndAlso Convert.ToBoolean(row.Cells("AutorisationDosage").Value)
            CheckBoxVerrouille.Checked = Not IsDBNull(row.Cells("Verrouille").Value) AndAlso Convert.ToBoolean(row.Cells("Verrouille").Value)
        Catch ex As Exception
            MessageBox.Show("Erreur sélection : " & ex.Message)
        End Try
    End Sub

    ' ═══════════════════════════════════════════════════════════════════
    '  AJOUTER
    ' ═══════════════════════════════════════════════════════════════════

    Private Sub AJOUTE_Click(sender As Object, e As EventArgs) Handles AJOUTE.Click
        If TextBoxCode.Text.Trim = "" OrElse TextBoxNom.Text.Trim = "" Then
            MessageBox.Show("Code et Nom sont obligatoires.")
            Exit Sub
        End If
        Try
            ModuleDb.Executer(
                "INSERT INTO dbo.Clients " &
                "(CodeClient, NomClient, Adresse, CodePostal, Pays, ICE, " &
                "Telephone, Fax, Email, Zone, Radial, Contacts, Remarques, " &
                "ControleCredit, AutorisationDosage, Verrouille, Actif) " &
                "VALUES (@CodeClient, @NomClient, @Adresse, @CodePostal, @Pays, @ICE, " &
                "@Telephone, @Fax, @Email, @Zone, @Radial, @Contacts, @Remarques, " &
                "@ControleCredit, @AutorisationDosage, @Verrouille, 1)",
                Sub(cmd)
                    cmd.Parameters.Add("@CodeClient", SqlDbType.NVarChar, 50).Value = TextBoxCode.Text.Trim
                    cmd.Parameters.Add("@NomClient", SqlDbType.NVarChar, 200).Value = TextBoxNom.Text.Trim
                    cmd.Parameters.Add("@Adresse", SqlDbType.NVarChar, 300).Value = If(TextBoxAdresse.Text.Trim = "", DBNull.Value, CObj(TextBoxAdresse.Text.Trim))
                    cmd.Parameters.Add("@CodePostal", SqlDbType.NVarChar, 20).Value = If(TextBoxCodePostal.Text.Trim = "", DBNull.Value, CObj(TextBoxCodePostal.Text.Trim))
                    cmd.Parameters.Add("@Pays", SqlDbType.NVarChar, 100).Value = If(TextBoxPays.Text.Trim = "", DBNull.Value, CObj(TextBoxPays.Text.Trim))
                    cmd.Parameters.Add("@ICE", SqlDbType.NVarChar, 50).Value = If(TextBoxTVA.Text.Trim = "", DBNull.Value, CObj(TextBoxTVA.Text.Trim))
                    cmd.Parameters.Add("@Telephone", SqlDbType.NVarChar, 30).Value = If(TextBoxTelephone.Text.Trim = "", DBNull.Value, CObj(TextBoxTelephone.Text.Trim))
                    cmd.Parameters.Add("@Fax", SqlDbType.NVarChar, 30).Value = If(TextBoxFax.Text.Trim = "", DBNull.Value, CObj(TextBoxFax.Text.Trim))
                    cmd.Parameters.Add("@Email", SqlDbType.NVarChar, 100).Value = If(TextBoxEmail.Text.Trim = "", DBNull.Value, CObj(TextBoxEmail.Text.Trim))
                    cmd.Parameters.Add("@Zone", SqlDbType.NVarChar, 100).Value = If(TextBoxZone.Text.Trim = "", DBNull.Value, CObj(TextBoxZone.Text.Trim))
                    cmd.Parameters.Add("@Radial", SqlDbType.NVarChar, 50).Value = If(TextBoxRadial.Text.Trim = "", DBNull.Value, CObj(TextBoxRadial.Text.Trim))

                    cmd.Parameters.Add("@Contacts", SqlDbType.NVarChar, 300).Value = If(TextBoxContacts.Text.Trim = "", DBNull.Value, CObj(TextBoxContacts.Text.Trim))
                    cmd.Parameters.Add("@Remarques", SqlDbType.NVarChar, 500).Value = If(TextBoxRemarques.Text.Trim = "", DBNull.Value, CObj(TextBoxRemarques.Text.Trim))
                    cmd.Parameters.Add("@ControleCredit", SqlDbType.Bit).Value = CheckBoxControleCredit.Checked
                    cmd.Parameters.Add("@AutorisationDosage", SqlDbType.Bit).Value = CheckBoxAutorisationDosage.Checked
                    cmd.Parameters.Add("@Verrouille", SqlDbType.Bit).Value = CheckBoxVerrouille.Checked
                End Sub)
            MessageBox.Show("Client ajouté avec succès.")
            ChargerClients() : ViderChamps()
        Catch ex As Exception
            MessageBox.Show("Erreur ajout : " & ex.Message)
        End Try
    End Sub

    ' ═══════════════════════════════════════════════════════════════════
    '  MODIFIER
    ' ═══════════════════════════════════════════════════════════════════

    Private Sub EDITE_Click(sender As Object, e As EventArgs) Handles EDITE.Click
        If TextBoxCode.Text.Trim = "" Then
            MessageBox.Show("Sélectionner un client à modifier.")
            Exit Sub
        End If
        Try
            ModuleDb.Executer(
                "UPDATE dbo.Clients SET " &
                "NomClient=@NomClient, Adresse=@Adresse, CodePostal=@CodePostal, " &
                "Pays=@Pays, ICE=@ICE, Telephone=@Telephone, Fax=@Fax, " &
                "Email=@Email, Zone=@Zone, Radial=@Radial, Contacts=@Contacts, " &
                "Remarques=@Remarques, ControleCredit=@ControleCredit, " &
                "AutorisationDosage=@AutorisationDosage, Verrouille=@Verrouille " &
                "WHERE CodeClient=@CodeClient",
                Sub(cmd)
                    cmd.Parameters.Add("@NomClient", SqlDbType.NVarChar, 200).Value = TextBoxNom.Text.Trim
                    cmd.Parameters.Add("@Adresse", SqlDbType.NVarChar, 300).Value = If(TextBoxAdresse.Text.Trim = "", DBNull.Value, CObj(TextBoxAdresse.Text.Trim))
                    cmd.Parameters.Add("@CodePostal", SqlDbType.NVarChar, 20).Value = If(TextBoxCodePostal.Text.Trim = "", DBNull.Value, CObj(TextBoxCodePostal.Text.Trim))
                    cmd.Parameters.Add("@Pays", SqlDbType.NVarChar, 100).Value = If(TextBoxPays.Text.Trim = "", DBNull.Value, CObj(TextBoxPays.Text.Trim))
                    cmd.Parameters.Add("@ICE", SqlDbType.NVarChar, 50).Value = If(TextBoxTVA.Text.Trim = "", DBNull.Value, CObj(TextBoxTVA.Text.Trim))
                    cmd.Parameters.Add("@Telephone", SqlDbType.NVarChar, 30).Value = If(TextBoxTelephone.Text.Trim = "", DBNull.Value, CObj(TextBoxTelephone.Text.Trim))
                    cmd.Parameters.Add("@Fax", SqlDbType.NVarChar, 30).Value = If(TextBoxFax.Text.Trim = "", DBNull.Value, CObj(TextBoxFax.Text.Trim))
                    cmd.Parameters.Add("@Email", SqlDbType.NVarChar, 100).Value = If(TextBoxEmail.Text.Trim = "", DBNull.Value, CObj(TextBoxEmail.Text.Trim))
                    cmd.Parameters.Add("@Zone", SqlDbType.NVarChar, 100).Value = If(TextBoxZone.Text.Trim = "", DBNull.Value, CObj(TextBoxZone.Text.Trim))

                    cmd.Parameters.Add("@Radial", SqlDbType.NVarChar, 50).Value = If(TextBoxRadial.Text.Trim = "", DBNull.Value, CObj(TextBoxRadial.Text.Trim))
                    cmd.Parameters.Add("@Contacts", SqlDbType.NVarChar, 300).Value = If(TextBoxContacts.Text.Trim = "", DBNull.Value, CObj(TextBoxContacts.Text.Trim))
                    cmd.Parameters.Add("@Remarques", SqlDbType.NVarChar, 500).Value = If(TextBoxRemarques.Text.Trim = "", DBNull.Value, CObj(TextBoxRemarques.Text.Trim))
                    cmd.Parameters.Add("@ControleCredit", SqlDbType.Bit).Value = CheckBoxControleCredit.Checked
                    cmd.Parameters.Add("@AutorisationDosage", SqlDbType.Bit).Value = CheckBoxAutorisationDosage.Checked
                    cmd.Parameters.Add("@Verrouille", SqlDbType.Bit).Value = CheckBoxVerrouille.Checked
                    cmd.Parameters.Add("@CodeClient", SqlDbType.NVarChar, 50).Value = TextBoxCode.Text.Trim
                End Sub)
            MessageBox.Show("Client modifié avec succès.")
            ChargerClients() : ViderChamps()
        Catch ex As Exception
            MessageBox.Show("Erreur modification : " & ex.Message)
        End Try
    End Sub

    ' ═══════════════════════════════════════════════════════════════════
    '  SUPPRIMER
    ' ═══════════════════════════════════════════════════════════════════

    Private Sub SUPRIME_Click(sender As Object, e As EventArgs) Handles SUPRIME.Click
        If TextBoxCode.Text.Trim = "" Then
            MessageBox.Show("Sélectionner un client à supprimer.")
            Exit Sub
        End If
        If MessageBox.Show("Voulez-vous supprimer ce client ?", "Confirmation",
                           MessageBoxButtons.YesNo, MessageBoxIcon.Question) = DialogResult.No Then
            Exit Sub
        End If
        Try
            ModuleDb.Executer(
                "DELETE FROM dbo.Clients WHERE CodeClient=@CodeClient",
                Sub(cmd)
                    cmd.Parameters.Add("@CodeClient", SqlDbType.NVarChar, 50).Value = TextBoxCode.Text.Trim
                End Sub)
            MessageBox.Show("Client supprimé avec succès.")
            ChargerClients() : ViderChamps()
        Catch ex As Exception
            MessageBox.Show("Erreur suppression : " & ex.Message)
        End Try
    End Sub

    ' ═══════════════════════════════════════════════════════════════════
    '  RECHERCHE
    ' ═══════════════════════════════════════════════════════════════════

    Private Sub RECHERCHE_Click(sender As Object, e As EventArgs) Handles RECHERCHE.Click
        Try
            Dim dt As DataTable

            If TextBoxCode.Text.Trim = "" AndAlso TextBoxNom.Text.Trim = "" Then
                dt = Lire(
                    "SELECT IdClient, CodeClient, NomClient, Adresse, CodePostal, Pays, " &
                    "ICE, Telephone, Fax, Email, Zone, Radial, Contacts, Remarques, " &
                    "ControleCredit, AutorisationDosage, Verrouille " &
                    "FROM dbo.Clients ORDER BY NomClient")
            ElseIf TextBoxCode.Text.Trim <> "" Then
                dt = Lire(
                    "SELECT IdClient, CodeClient, NomClient, Adresse, CodePostal, Pays, " &
                    "ICE, Telephone, Fax, Email, Zone, Radial, Contacts, Remarques, " &
                    "ControleCredit, AutorisationDosage, Verrouille " &
                    "FROM dbo.Clients WHERE CodeClient LIKE @Recherche ORDER BY NomClient",
                    Sub(cmd)
                        cmd.Parameters.Add("@Recherche", SqlDbType.NVarChar, 50).Value = "%" & TextBoxCode.Text.Trim & "%"
                    End Sub)
            Else
                dt = Lire(
                    "SELECT IdClient, CodeClient, NomClient, Adresse, CodePostal, Pays, " &
                    "ICE, Telephone, Fax, Email, Zone, Radial, Contacts, Remarques, " &
                    "ControleCredit, AutorisationDosage, Verrouille " &
                    "FROM dbo.Clients WHERE NomClient LIKE @Recherche ORDER BY NomClient",
                    Sub(cmd)
                        cmd.Parameters.Add("@Recherche", SqlDbType.NVarChar, 200).Value = "%" & TextBoxNom.Text.Trim & "%"
                    End Sub)
            End If

            DataGridView1.DataSource = dt
            For Each col As DataGridViewColumn In DataGridView1.Columns
                col.Visible = False
            Next
            If DataGridView1.Columns.Contains("CodeClient") Then
                DataGridView1.Columns("CodeClient").Visible = True
                DataGridView1.Columns("CodeClient").HeaderText = "Code"
                DataGridView1.Columns("CodeClient").Width = 120
            End If
            If DataGridView1.Columns.Contains("NomClient") Then
                DataGridView1.Columns("NomClient").Visible = True
                DataGridView1.Columns("NomClient").HeaderText = "Nom"
                DataGridView1.Columns("NomClient").Width = 250
            End If
            If DataGridView1.Columns.Contains("Telephone") Then
                DataGridView1.Columns("Telephone").Visible = True
                DataGridView1.Columns("Telephone").HeaderText = "Téléphone"
                DataGridView1.Columns("Telephone").Width = 150
            End If
            If dt.Rows.Count = 0 Then MessageBox.Show("Aucun résultat trouvé.")

        Catch ex As Exception
            MessageBox.Show("Erreur recherche : " & ex.Message)
        End Try
    End Sub

    ' ═══════════════════════════════════════════════════════════════════
    '  IMPRESSION (inchangée)
    ' ═══════════════════════════════════════════════════════════════════

    Private Sub pd_PrintPage(sender As Object, e As PrintPageEventArgs) Handles pd.PrintPage
        Dim titreFont As New Font("Arial", 18, FontStyle.Bold)
        Dim sousTitreFont As New Font("Arial", 10, FontStyle.Italic)
        Dim texteFont As New Font("Arial", 11, FontStyle.Regular)
        Dim texteBold As New Font("Arial", 11, FontStyle.Bold)
        Dim x As Integer = 40 : Dim y As Integer = 30
        e.Graphics.DrawString("FICHE CLIENT", titreFont, Brushes.Black, 250, y + 20)
        e.Graphics.DrawString("Date : " & DateTime.Now.ToString("dd/MM/yyyy HH:mm"), sousTitreFont, Brushes.Black, 250, y + 60)
        y += 130
        e.Graphics.DrawLine(Pens.Black, x, y, 760, y)
        y += 20
        e.Graphics.DrawRectangle(Pens.Black, x, y, 720, 420)
        Dim labelX As Integer = x + 20 : Dim valueX As Integer = x + 170
        Dim ligneY As Integer = y + 20 : Dim espace As Integer = 28
        Dim champsG = {("Code :", TextBoxCode.Text), ("Nom :", TextBoxNom.Text),
                       ("Adresse :", TextBoxAdresse.Text), ("Code Postal :", TextBoxCodePostal.Text),
                       ("Pays :", TextBoxPays.Text), ("TVA :", TextBoxTVA.Text),
                       ("Téléphone :", TextBoxTelephone.Text)}
        For Each c In champsG
            e.Graphics.DrawString(c.Item1, texteBold, Brushes.Black, labelX, ligneY)
            e.Graphics.DrawString(c.Item2, texteFont, Brushes.Black, valueX, ligneY)
            ligneY += espace
        Next
        Dim labelX2 As Integer = 420 : Dim valueX2 As Integer = 560 : Dim ligneY2 As Integer = y + 20
        Dim champsD = {("Fax :", TextBoxFax.Text), ("Email :", TextBoxEmail.Text),
                       ("Zone :", TextBoxZone.Text), ("Radial :", TextBoxRadial.Text),
                       ("Contacts :", TextBoxContacts.Text), ("Remarques :", TextBoxRemarques.Text),
                       ("Autorisation dosage :", If(CheckBoxAutorisationDosage.Checked, "Oui", "Non")),
                       ("Contrôle crédit :", If(CheckBoxControleCredit.Checked, "Oui", "Non")),
                       ("Verrouillé :", If(CheckBoxVerrouille.Checked, "Oui", "Non"))}
        For Each c In champsD
            e.Graphics.DrawString(c.Item1, texteBold, Brushes.Black, labelX2, ligneY2)
            e.Graphics.DrawString(c.Item2, texteFont, Brushes.Black, valueX2, ligneY2)
            ligneY2 += espace
        Next
        e.Graphics.DrawString("Application de gestion centrale à béton", sousTitreFont, Brushes.Gray, x, 780)
    End Sub

    Private Sub Imprimer_Click(sender As Object, e As EventArgs) Handles Imprimer.Click
        If TextBoxCode.Text.Trim = "" Then
            MessageBox.Show("Sélectionner un client à imprimer.")
            Exit Sub
        End If
        Dim apercu As New PrintPreviewDialog
        apercu.Document = pd
        apercu.WindowState = FormWindowState.Maximized
        apercu.ShowDialog()
    End Sub
    ' ═══════════════════════════════════════════════════════════════════
    '  CHANTIERS DU CLIENT
    ' ═══════════════════════════════════════════════════════════════════

    Private Sub BTN_CHANTIER_Click(sender As Object, e As EventArgs) Handles BTN_CHANTIER.Click
        If IdClientSelectionne = -1 Then
            MessageBox.Show("Veuillez sélectionner un client.")
            Exit Sub
        End If

        Dim frm As New Chantiers(IdClientSelectionne)
        frm.Show()
        Me.Close()
    End Sub

    Private Sub AfficherChantiersClient(idClient As Integer)
        Try
            Dim dt As DataTable = ModuleDb.Lire(
                "SELECT IdChantier, CodeChantier, NomChantier, Rue, CodePostal, Ville, " &
                "Pays, Telephone " &
                "FROM dbo.Chantier WHERE IdClient = @IdClient ORDER BY NomChantier",
                Sub(cmd)
                    cmd.Parameters.Add("@IdClient", SqlDbType.Int).Value = idClient
                End Sub)

            If dt.Rows.Count = 0 Then
                MessageBox.Show("Aucun chantier trouvé pour ce client.")
                Exit Sub
            End If

            ' Création d'une fenêtre popup avec une grille pour afficher les chantiers
            Dim frm As New Form()
            frm.Text = "Chantiers du client : " & TextBoxNom.Text
            frm.Width = 800
            frm.Height = 500
            frm.StartPosition = FormStartPosition.CenterParent

            Dim grid As New DataGridView()
            grid.Dock = DockStyle.Fill
            grid.DataSource = dt
            grid.ReadOnly = True
            grid.AllowUserToAddRows = False
            grid.SelectionMode = DataGridViewSelectionMode.FullRowSelect
            grid.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill

            If grid.Columns.Contains("IdChantier") Then grid.Columns("IdChantier").HeaderText = "Id"
            If grid.Columns.Contains("CodeChantier") Then grid.Columns("CodeChantier").HeaderText = "Code"
            If grid.Columns.Contains("NomChantier") Then grid.Columns("NomChantier").HeaderText = "Nom"
            If grid.Columns.Contains("Rue") Then grid.Columns("Rue").HeaderText = "Rue"
            If grid.Columns.Contains("CodePostal") Then grid.Columns("CodePostal").HeaderText = "Code Postal"
            If grid.Columns.Contains("Ville") Then grid.Columns("Ville").HeaderText = "Ville"
            If grid.Columns.Contains("Pays") Then grid.Columns("Pays").HeaderText = "Pays"
            If grid.Columns.Contains("Telephone") Then grid.Columns("Telephone").HeaderText = "Téléphone"

            frm.Controls.Add(grid)
            frm.ShowDialog()

        Catch ex As Exception
            MessageBox.Show("Erreur chargement chantiers : " & ex.Message)
        End Try
    End Sub

    Private Sub DataGridView1_CellContentClick(sender As Object, e As DataGridViewCellEventArgs) Handles DataGridView1.CellContentClick

    End Sub
End Class