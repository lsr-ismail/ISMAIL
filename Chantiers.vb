Imports Microsoft.Data.SqlClient
Imports System.Data

Public Class Chantiers

    Private chargementClient As Boolean = False
    Public Property IdClientFiltre As Integer = -1

    ' Constructeur par défaut (utilisé par PanelMain et le Designer)
    Public Sub New()
        InitializeComponent()
    End Sub

    ' Constructeur utilisé depuis la fiche Client (avec filtre)
    Public Sub New(idClient As Integer)
        InitializeComponent()
        IdClientFiltre = idClient
    End Sub
    ' ═══════════════════════════════════════════════════════════════════
    '  LOAD
    ' ═══════════════════════════════════════════════════════════════════

    Private Sub FrmChantier_Load(sender As Object, e As EventArgs) Handles MyBase.Load

        Select Case RoleUtilisateur
            Case "Admin", "Super"
                btnAjouter.Visible = True : btnAjouter.Enabled = True
                btnModifier.Visible = True : btnModifier.Enabled = True
                btnSupprimer.Visible = True : btnSupprimer.Enabled = True
                btnNouveau.Visible = True : btnNouveau.Enabled = True

            Case "Operateur"
                btnAjouter.Visible = True : btnAjouter.Enabled = False
                btnModifier.Visible = True : btnModifier.Enabled = False
                btnSupprimer.Visible = True : btnSupprimer.Enabled = False
                btnNouveau.Visible = True : btnNouveau.Enabled = True

            Case Else
                ' Rôle inconnu / non défini → accès le plus restreint par sécurité
                btnAjouter.Visible = True : btnAjouter.Enabled = False
                btnModifier.Visible = True : btnModifier.Enabled = False
                btnSupprimer.Visible = True : btnSupprimer.Enabled = False
                btnNouveau.Visible = True : btnNouveau.Enabled = False
        End Select
        ChargerClientsRecherche()
        ChargerChantiers()
        If IdClientFiltre <> -1 Then
            cmdIdClient.SelectedValue = IdClientFiltre
        End If
    End Sub

    ' ═══════════════════════════════════════════════════════════════════
    '  CHARGEMENT GRILLE
    ' ═══════════════════════════════════════════════════════════════════

    Private Sub ChargerChantiers()
        Try
            Dim sql As String =
            "SELECT C.IdChantier, C.CodeChantier, C.NomChantier, " &
            "CL.CodeClient, CL.NomClient " &
            "FROM dbo.Chantier C " &
            "LEFT JOIN dbo.Clients CL ON C.IdClient = CL.IdClient "

            Dim dt As DataTable

            If IdClientFiltre <> -1 Then
                sql &= "WHERE C.IdClient = @IdClient ORDER BY C.IdChantier DESC"
                dt = ModuleDb.Lire(sql,
                Sub(cmd)
                    cmd.Parameters.Add("@IdClient", SqlDbType.Int).Value = IdClientFiltre
                End Sub)
            Else
                sql &= "ORDER BY C.IdChantier DESC"
                dt = ModuleDb.Lire(sql)
            End If

            DataGridView1.DataSource = dt
            DataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill
            DataGridView1.SelectionMode = DataGridViewSelectionMode.FullRowSelect
            DataGridView1.MultiSelect = False
            DataGridView1.AllowUserToAddRows = False
            DataGridView1.ReadOnly = True

            If DataGridView1.Columns.Count > 0 Then
                DataGridView1.Columns("IdChantier").HeaderText = "Id Chantier"
                DataGridView1.Columns("CodeChantier").HeaderText = "Code Chantier"
                DataGridView1.Columns("NomChantier").HeaderText = "Nom Chantier"
                DataGridView1.Columns("CodeClient").HeaderText = "Code Client"
                DataGridView1.Columns("NomClient").HeaderText = "Nom Client"
            End If

        Catch ex As Exception
            MessageBox.Show("Erreur chargement : " & ex.Message)
        End Try
    End Sub

    ' ═══════════════════════════════════════════════════════════════════
    '  CHARGEMENT CLIENTS COMBOBOX
    ' ═══════════════════════════════════════════════════════════════════

    Private Sub ChargerClientsRecherche()
        Try
            Dim dt As DataTable = ModuleDb.Lire(
                "SELECT IdClient, CodeClient, NomClient FROM dbo.Clients ORDER BY NomClient")

            cmdIdClient.DataSource = dt.Copy()
            cmdIdClient.DisplayMember = "IdClient"
            cmdIdClient.ValueMember = "IdClient"
            cmdIdClient.SelectedIndex = -1

            cmdCodeClient.DataSource = dt.Copy()
            cmdCodeClient.DisplayMember = "CodeClient"
            cmdCodeClient.ValueMember = "IdClient"
            cmdCodeClient.SelectedIndex = -1

            cmdNomClient.DataSource = dt.Copy()
            cmdNomClient.DisplayMember = "NomClient"
            cmdNomClient.ValueMember = "IdClient"
            cmdNomClient.SelectedIndex = -1

        Catch ex As Exception
            MessageBox.Show("Erreur chargement clients : " & ex.Message)
        End Try
    End Sub

    ' ═══════════════════════════════════════════════════════════════════
    '  SYNC COMBOBOX CLIENT
    ' ═══════════════════════════════════════════════════════════════════

    Private Sub cmdIdClient_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cmdIdClient.SelectedIndexChanged
        Try
            If chargementClient Then Exit Sub
            If cmdIdClient.SelectedIndex = -1 OrElse cmdIdClient.SelectedValue Is Nothing Then Exit Sub
            chargementClient = True
            Dim id As Integer = CInt(cmdIdClient.SelectedValue)
            cmdCodeClient.SelectedValue = id
            cmdNomClient.SelectedValue = id
        Catch ex As Exception
        Finally
            chargementClient = False
        End Try
    End Sub

    Private Sub cmdCodeClient_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cmdCodeClient.SelectedIndexChanged
        Try
            If chargementClient Then Exit Sub
            If cmdCodeClient.SelectedIndex = -1 OrElse cmdCodeClient.SelectedValue Is Nothing Then Exit Sub
            chargementClient = True
            Dim id As Integer = CInt(cmdCodeClient.SelectedValue)
            cmdIdClient.SelectedValue = id
            cmdNomClient.SelectedValue = id
        Catch ex As Exception
        Finally
            chargementClient = False
        End Try
    End Sub

    Private Sub cmdNomClient_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cmdNomClient.SelectedIndexChanged
        Try
            If chargementClient Then Exit Sub
            If cmdNomClient.SelectedIndex = -1 OrElse cmdNomClient.SelectedValue Is Nothing Then Exit Sub
            chargementClient = True
            Dim id As Integer = CInt(cmdNomClient.SelectedValue)
            cmdIdClient.SelectedValue = id
            cmdCodeClient.SelectedValue = id
        Catch ex As Exception
        Finally
            chargementClient = False
        End Try
    End Sub

    ' ═══════════════════════════════════════════════════════════════════
    '  CLIC GRILLE
    ' ═══════════════════════════════════════════════════════════════════

    Private Sub DataGridView1_CellClick(sender As Object, e As DataGridViewCellEventArgs) Handles DataGridView1.CellClick
        If e.RowIndex < 0 Then Exit Sub
        Dim idChantier As Integer = CInt(DataGridView1.Rows(e.RowIndex).Cells("IdChantier").Value)
        ChargerDetailsChantier(idChantier)
    End Sub

    Private Sub ChargerDetailsChantier(idChantier As Integer)
        Try
            Dim dt As DataTable = ModuleDb.Lire(
                "SELECT C.*, CL.CodeClient, CL.NomClient " &
                "FROM dbo.Chantier C " &
                "LEFT JOIN dbo.Clients CL ON C.IdClient = CL.IdClient " &
                "WHERE C.IdChantier = @IdChantier",
                Sub(cmd)
                    cmd.Parameters.Add("@IdChantier", SqlDbType.Int).Value = idChantier
                End Sub)

            If dt.Rows.Count = 0 Then Exit Sub
            Dim dr As DataRow = dt.Rows(0)

            txtIdChantier.Text = dr("IdChantier").ToString()
            txtCodeChantier.Text = LireTexte(dr, "CodeChantier")
            txtCleChantier.Text = LireTexte(dr, "CleChantier")
            txtNomChantier.Text = LireTexte(dr, "NomChantier")
            txtRue.Text = LireTexte(dr, "Rue")
            txtCodePostal.Text = LireTexte(dr, "CodePostal")
            txtVille.Text = LireTexte(dr, "Ville")
            txtPays.Text = LireTexte(dr, "Pays")
            txtTelephone.Text = LireTexte(dr, "Telephone")
            txtZoneFacturation.Text = LireTexte(dr, "ZoneFacturation")
            txtZoneTransport.Text = LireTexte(dr, "ZoneTransport")
            txtPlusValue.Text = LireTexte(dr, "PlusValue")
            txtNatureChantier.Text = LireTexte(dr, "NatureChantier")
            txtTypeChantier.Text = LireTexte(dr, "TypeChantier")
            txtMarcheWattmetre.Text = LireTexte(dr, "MarcheWattmetre")
            txtImpressionPeses.Text = LireTexte(dr, "ImpressionPeses")
            txtCreationChantierBL.Text = LireTexte(dr, "CreationChantierBL")
            txtCommandeObligatoire.Text = LireTexte(dr, "CommandeObligatoire")
            txtCommande.Text = LireTexte(dr, "Commande")
            txtDerniereModification.Text = If(IsDBNull(dr("DerniereModification")), "",
                CDate(dr("DerniereModification")).ToString("yyyy-MM-dd"))

            If Not IsDBNull(dr("IdClient")) Then
                Dim idClient As Integer = CInt(dr("IdClient"))
                chargementClient = True
                cmdIdClient.SelectedValue = idClient
                cmdCodeClient.SelectedValue = idClient
                cmdNomClient.SelectedValue = idClient
                chargementClient = False
            Else
                cmdIdClient.SelectedIndex = -1
                cmdCodeClient.SelectedIndex = -1
                cmdNomClient.SelectedIndex = -1
            End If

        Catch ex As Exception
            MessageBox.Show("Erreur chargement détails : " & ex.Message)
        End Try
    End Sub

    ' ═══════════════════════════════════════════════════════════════════
    '  AJOUTER
    ' ═══════════════════════════════════════════════════════════════════

    Private Sub btnAjouter_Click(sender As Object, e As EventArgs) Handles btnAjouter.Click
        If cmdIdClient.SelectedIndex = -1 Then
            MessageBox.Show("Sélectionner un client.")
            Exit Sub
        End If
        If txtNomChantier.Text.Trim = "" Then
            MessageBox.Show("Le nom du chantier est obligatoire.")
            Exit Sub
        End If
        Try
            ModuleDb.Executer(
                "INSERT INTO dbo.Chantier " &
                "(CodeChantier, CleChantier, NomChantier, Rue, CodePostal, Ville, Pays, " &
                "Telephone, ZoneFacturation, ZoneTransport, PlusValue, NatureChantier, " &
                "TypeChantier, MarcheWattmetre, ImpressionPeses, CreationChantierBL, " &
                "CommandeObligatoire, Commande, DerniereModification, IdClient) " &
                "VALUES (@CodeChantier, @CleChantier, @NomChantier, @Rue, @CodePostal, @Ville, @Pays, " &
                "@Telephone, @ZoneFacturation, @ZoneTransport, @PlusValue, @NatureChantier, " &
                "@TypeChantier, @MarcheWattmetre, @ImpressionPeses, @CreationChantierBL, " &
                "@CommandeObligatoire, @Commande, @DerniereModification, @IdClient)",
                AddressOf AjouterParametres)

            MessageBox.Show("Chantier ajouté avec succès.")
            ChargerChantiers()
            ViderChamps()

        Catch ex As Exception
            MessageBox.Show("Erreur ajout : " & ex.Message)
        End Try
    End Sub

    ' ═══════════════════════════════════════════════════════════════════
    '  MODIFIER
    ' ═══════════════════════════════════════════════════════════════════

    Private Sub btnModifier_Click(sender As Object, e As EventArgs) Handles btnModifier.Click
        If txtIdChantier.Text.Trim = "" Then
            MessageBox.Show("Sélectionner un chantier.")
            Exit Sub
        End If
        If cmdIdClient.SelectedIndex = -1 Then
            MessageBox.Show("Sélectionner un client.")
            Exit Sub
        End If
        Try
            ModuleDb.Executer(
                "UPDATE dbo.Chantier SET " &
                "CodeChantier=@CodeChantier, CleChantier=@CleChantier, NomChantier=@NomChantier, " &
                "Rue=@Rue, CodePostal=@CodePostal, Ville=@Ville, Pays=@Pays, " &
                "Telephone=@Telephone, ZoneFacturation=@ZoneFacturation, ZoneTransport=@ZoneTransport, " &
                "PlusValue=@PlusValue, NatureChantier=@NatureChantier, TypeChantier=@TypeChantier, " &
                "MarcheWattmetre=@MarcheWattmetre, ImpressionPeses=@ImpressionPeses, " &
                "CreationChantierBL=@CreationChantierBL, CommandeObligatoire=@CommandeObligatoire, " &
                "Commande=@Commande, DerniereModification=@DerniereModification, IdClient=@IdClient " &
                "WHERE IdChantier=@IdChantier",
                Sub(cmd)
                    cmd.Parameters.Add("@IdChantier", SqlDbType.Int).Value = CInt(txtIdChantier.Text)
                    AjouterParametres(cmd)
                End Sub)

            MessageBox.Show("Chantier modifié avec succès.")
            ChargerChantiers()
            ViderChamps()

        Catch ex As Exception
            MessageBox.Show("Erreur modification : " & ex.Message)
        End Try
    End Sub

    ' ═══════════════════════════════════════════════════════════════════
    '  SUPPRIMER
    ' ═══════════════════════════════════════════════════════════════════

    Private Sub btnSupprimer_Click(sender As Object, e As EventArgs) Handles btnSupprimer.Click
        If txtIdChantier.Text.Trim = "" Then
            MessageBox.Show("Sélectionner un chantier.")
            Exit Sub
        End If
        If MessageBox.Show("Voulez-vous supprimer ce chantier ?", "Confirmation",
                           MessageBoxButtons.YesNo, MessageBoxIcon.Question) = DialogResult.No Then
            Exit Sub
        End If
        Try
            ModuleDb.Executer(
                "DELETE FROM dbo.Chantier WHERE IdChantier=@IdChantier",
                Sub(cmd)
                    cmd.Parameters.Add("@IdChantier", SqlDbType.Int).Value = CInt(txtIdChantier.Text)
                End Sub)

            MessageBox.Show("Chantier supprimé avec succès.")
            ChargerChantiers()
            ViderChamps()

        Catch ex As Exception
            MessageBox.Show("Erreur suppression : " & ex.Message)
        End Try
    End Sub

    ' ═══════════════════════════════════════════════════════════════════
    '  ACTUALISER / NOUVEAU
    ' ═══════════════════════════════════════════════════════════════════

    Private Sub btnActualiser_Click(sender As Object, e As EventArgs) Handles btnActualiser.Click

        IdClientFiltre = -1      ' NOUVEAU : on enlève le filtre
        ChargerChantiers()
    End Sub


    Private Sub btnNouveau_Click(sender As Object, e As EventArgs) Handles btnNouveau.Click
        ViderChamps()
    End Sub

    ' ═══════════════════════════════════════════════════════════════════
    '  HELPERS
    ' ═══════════════════════════════════════════════════════════════════

    Private Sub AjouterParametres(cmd As SqlCommand)
        cmd.Parameters.Add("@CodeChantier", SqlDbType.NVarChar, 50).Value = ValTexte(txtCodeChantier.Text)
        cmd.Parameters.Add("@CleChantier", SqlDbType.NVarChar, 50).Value = ValTexte(txtCleChantier.Text)
        cmd.Parameters.Add("@NomChantier", SqlDbType.NVarChar, 200).Value = ValTexte(txtNomChantier.Text)
        cmd.Parameters.Add("@Rue", SqlDbType.NVarChar, 200).Value = ValTexte(txtRue.Text)
        cmd.Parameters.Add("@CodePostal", SqlDbType.NVarChar, 20).Value = ValTexte(txtCodePostal.Text)
        cmd.Parameters.Add("@Ville", SqlDbType.NVarChar, 100).Value = ValTexte(txtVille.Text)
        cmd.Parameters.Add("@Pays", SqlDbType.NVarChar, 100).Value = ValTexte(txtPays.Text)
        cmd.Parameters.Add("@Telephone", SqlDbType.NVarChar, 30).Value = ValTexte(txtTelephone.Text)
        cmd.Parameters.Add("@ZoneFacturation", SqlDbType.NVarChar, 100).Value = ValTexte(txtZoneFacturation.Text)
        cmd.Parameters.Add("@ZoneTransport", SqlDbType.NVarChar, 100).Value = ValTexte(txtZoneTransport.Text)
        Dim pv As Double = 0
        Double.TryParse(txtPlusValue.Text.Replace(",", "."), pv)
        cmd.Parameters.Add("@PlusValue", SqlDbType.Float).Value = pv
        cmd.Parameters.Add("@NatureChantier", SqlDbType.NVarChar, 100).Value = ValTexte(txtNatureChantier.Text)
        cmd.Parameters.Add("@TypeChantier", SqlDbType.NVarChar, 100).Value = ValTexte(txtTypeChantier.Text)
        cmd.Parameters.Add("@MarcheWattmetre", SqlDbType.NVarChar, 50).Value = ValTexte(txtMarcheWattmetre.Text)
        cmd.Parameters.Add("@ImpressionPeses", SqlDbType.NVarChar, 50).Value = ValTexte(txtImpressionPeses.Text)
        cmd.Parameters.Add("@CreationChantierBL", SqlDbType.NVarChar, 50).Value = ValTexte(txtCreationChantierBL.Text)
        cmd.Parameters.Add("@CommandeObligatoire", SqlDbType.NVarChar, 50).Value = ValTexte(txtCommandeObligatoire.Text)
        cmd.Parameters.Add("@Commande", SqlDbType.NVarChar, 200).Value = ValTexte(txtCommande.Text)
        If txtDerniereModification.Text.Trim = "" Then
            cmd.Parameters.Add("@DerniereModification", SqlDbType.DateTime).Value = DBNull.Value
        Else
            Dim d As Date
            cmd.Parameters.Add("@DerniereModification", SqlDbType.DateTime).Value =
                If(Date.TryParse(txtDerniereModification.Text.Trim, d), CObj(d), DBNull.Value)
        End If
        cmd.Parameters.Add("@IdClient", SqlDbType.Int).Value = CInt(cmdIdClient.SelectedValue)
    End Sub

    Private Function ValTexte(txt As String) As Object
        Return If(txt.Trim = "", DBNull.Value, CObj(txt.Trim))
    End Function

    Private Function LireTexte(dr As DataRow, col As String) As String
        Return If(IsDBNull(dr(col)), "", dr(col).ToString())
    End Function

    Private Sub ViderChamps()
        For Each txt As TextBox In {txtIdChantier, txtCodeChantier, txtCleChantier,
                                     txtNomChantier, txtRue, txtCodePostal, txtVille,
                                     txtPays, txtTelephone, txtZoneFacturation, txtZoneTransport,
                                     txtPlusValue, txtNatureChantier, txtTypeChantier,
                                     txtMarcheWattmetre, txtImpressionPeses, txtCreationChantierBL,
                                     txtCommandeObligatoire, txtCommande, txtDerniereModification}
            txt.Text = ""
        Next
        cmdIdClient.SelectedIndex = -1
        cmdCodeClient.SelectedIndex = -1
        cmdNomClient.SelectedIndex = -1
    End Sub

    Private Sub txtNomChantier_TextChanged(sender As Object, e As EventArgs) Handles txtNomChantier.TextChanged

    End Sub
End Class