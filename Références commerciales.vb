Imports Microsoft.Data.SqlClient
Imports System.Data

Public Class Références_commerciales

    Dim ligneSelectionnee As DataGridViewRow = Nothing

    ' ═══════════════════════════════════════════════════════════════════
    '  LOAD
    ' ═══════════════════════════════════════════════════════════════════

    Private Sub ReferencesCommerciales_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        ChargerReferencesCommerciales()
        txtIdReference.Visible = False
    End Sub

    ' ═══════════════════════════════════════════════════════════════════
    '  CHARGEMENT
    ' ═══════════════════════════════════════════════════════════════════

    Private Sub ChargerReferencesCommerciales()
        Try
            Dim dt As DataTable = ModuleDb.Lire(
                "SELECT * FROM ReferencesCommerciales ORDER BY IdReference DESC")

            DataGridView1.Columns.Clear()
            DataGridView1.AutoGenerateColumns = True
            DataGridView1.DataSource = dt
            FormaterGrilleReferences()

        Catch ex As Exception
            MessageBox.Show("Erreur chargement : " & ex.Message)
        End Try
    End Sub

    Private Sub FormaterGrilleReferences()
        DataGridView1.SelectionMode = DataGridViewSelectionMode.FullRowSelect
        DataGridView1.MultiSelect = False
        DataGridView1.AllowUserToAddRows = False
        DataGridView1.ReadOnly = True
        DataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill

        Dim colsMasquees = {"IdReference", "Produit", "DimensionMaximum", "ClasseResistance",
                            "ClasseCompactation", "ClasseExposition", "TeneurChlorures", "Additions",
                            "Adjuvants", "ProprietesSpeciales", "Ciment", "MasseVolumique", "Norme",
                            "ClasseDiffusion", "ClasseConsistance", "ClasseVebe", "MinimumTeneurAirBeton",
                            "Durabilite", "ChampLibre1", "ChampLibre2", "ChampLibre3", "ChampLibre4",
                            "Actif", "DerniereModification"}
        For Each col In colsMasquees
            If DataGridView1.Columns.Contains(col) Then
                DataGridView1.Columns(col).Visible = False
            End If
        Next

        If DataGridView1.Columns.Contains("CodeReference") Then DataGridView1.Columns("CodeReference").HeaderText = "CODE"
        If DataGridView1.Columns.Contains("NomReference") Then DataGridView1.Columns("NomReference").HeaderText = "NOM"
    End Sub

    ' ═══════════════════════════════════════════════════════════════════
    '  CLIC GRILLE
    ' ═══════════════════════════════════════════════════════════════════

    Private Sub DataGridView1_CellClick(sender As Object, e As DataGridViewCellEventArgs) Handles DataGridView1.CellClick
        If e.RowIndex < 0 Then Exit Sub
        Dim row As DataGridViewRow = DataGridView1.Rows(e.RowIndex)
        ligneSelectionnee = row

        txtIdReference.Text = LireCell(row, "IdReference")
        txtCodeReference.Text = LireCell(row, "CodeReference")
        txtNomReference.Text = LireCell(row, "NomReference")
        txtProduit.Text = LireCell(row, "Produit")
        txtDimensionMaximum.Text = LireCell(row, "DimensionMaximum")
        txtClasseResistance.Text = LireCell(row, "ClasseResistance")
        txtClasseCompactation.Text = LireCell(row, "ClasseCompactation")
        txtClasseExposition.Text = LireCell(row, "ClasseExposition")
        txtTeneurChlorures.Text = LireCell(row, "TeneurChlorures")
        txtAdditions.Text = LireCell(row, "Additions")
        txtAdjuvants.Text = LireCell(row, "Adjuvants")
        txtProprietesSpeciales.Text = LireCell(row, "ProprietesSpeciales")
        txtCiment.Text = LireCell(row, "Ciment")
        txtMasseVolumique.Text = LireCell(row, "MasseVolumique")
        txtNorme.Text = LireCell(row, "Norme")
        txtClasseDiffusion.Text = LireCell(row, "ClasseDiffusion")
        txtClasseConsistance.Text = LireCell(row, "ClasseConsistance")
        txtClasseVebe.Text = LireCell(row, "ClasseVebe")
        txtMinimumTeneurAirBeton.Text = LireCell(row, "MinimumTeneurAirBeton")
        txtDurabilite.Text = LireCell(row, "Durabilite")
        txtChampLibre1.Text = LireCell(row, "ChampLibre1")
        txtChampLibre2.Text = LireCell(row, "ChampLibre2")
        txtChampLibre3.Text = LireCell(row, "ChampLibre3")
        txtChampLibre4.Text = LireCell(row, "ChampLibre4")
        chkActif.Checked = Not IsDBNull(row.Cells("Actif").Value) AndAlso CBool(row.Cells("Actif").Value)
        txtDerniereModification.Text = If(IsDBNull(row.Cells("DerniereModification").Value), "",
            CDate(row.Cells("DerniereModification").Value).ToString("yyyy-MM-dd"))
    End Sub

    ' ═══════════════════════════════════════════════════════════════════
    '  AJOUTER
    ' ═══════════════════════════════════════════════════════════════════

    Private Sub btnAjouter_Click(sender As Object, e As EventArgs) Handles btnAjouter.Click
        If txtCodeReference.Text.Trim = "" OrElse txtNomReference.Text.Trim = "" Then
            MessageBox.Show("Code et Nom sont obligatoires.")
            Exit Sub
        End If
        Try
            ModuleDb.Executer(
                "INSERT INTO ReferencesCommerciales " &
                "(CodeReference, NomReference, Produit, DimensionMaximum, ClasseResistance, ClasseCompactation, " &
                "ClasseExposition, TeneurChlorures, Additions, Adjuvants, ProprietesSpeciales, Ciment, " &
                "MasseVolumique, Norme, ClasseDiffusion, ClasseConsistance, ClasseVebe, MinimumTeneurAirBeton, " &
                "Durabilite, ChampLibre1, ChampLibre2, ChampLibre3, ChampLibre4, Actif, DerniereModification) " &
                "VALUES " &
                "(@CodeReference, @NomReference, @Produit, @DimensionMaximum, @ClasseResistance, @ClasseCompactation, " &
                "@ClasseExposition, @TeneurChlorures, @Additions, @Adjuvants, @ProprietesSpeciales, @Ciment, " &
                "@MasseVolumique, @Norme, @ClasseDiffusion, @ClasseConsistance, @ClasseVebe, @MinimumTeneurAirBeton, " &
                "@Durabilite, @ChampLibre1, @ChampLibre2, @ChampLibre3, @ChampLibre4, @Actif, @DerniereModification)",
                AddressOf AjouterParametres)

            MessageBox.Show("Référence ajoutée avec succès.")
            ChargerReferencesCommerciales()
            ViderChampsReferences()

        Catch ex As Exception
            MessageBox.Show("Erreur ajout : " & ex.Message)
        End Try
    End Sub

    ' ═══════════════════════════════════════════════════════════════════
    '  MODIFIER
    ' ═══════════════════════════════════════════════════════════════════

    Private Sub btnModifier_Click(sender As Object, e As EventArgs) Handles btnModifier.Click
        If txtIdReference.Text = "" Then
            MessageBox.Show("Sélectionne une référence.")
            Exit Sub
        End If
        Try
            ModuleDb.Executer(
                "UPDATE ReferencesCommerciales SET " &
                "CodeReference=@CodeReference, NomReference=@NomReference, Produit=@Produit, " &
                "DimensionMaximum=@DimensionMaximum, ClasseResistance=@ClasseResistance, " &
                "ClasseCompactation=@ClasseCompactation, ClasseExposition=@ClasseExposition, " &
                "TeneurChlorures=@TeneurChlorures, Additions=@Additions, Adjuvants=@Adjuvants, " &
                "ProprietesSpeciales=@ProprietesSpeciales, Ciment=@Ciment, MasseVolumique=@MasseVolumique, " &
                "Norme=@Norme, ClasseDiffusion=@ClasseDiffusion, ClasseConsistance=@ClasseConsistance, " &
                "ClasseVebe=@ClasseVebe, MinimumTeneurAirBeton=@MinimumTeneurAirBeton, Durabilite=@Durabilite, " &
                "ChampLibre1=@ChampLibre1, ChampLibre2=@ChampLibre2, ChampLibre3=@ChampLibre3, ChampLibre4=@ChampLibre4, " &
                "Actif=@Actif, DerniereModification=@DerniereModification " &
                "WHERE IdReference=@IdReference",
                Sub(cmd)
                    cmd.Parameters.Add("@IdReference", SqlDbType.Int).Value = CInt(txtIdReference.Text)
                    AjouterParametres(cmd)
                End Sub)

            MessageBox.Show("Référence modifiée avec succès.")
            ChargerReferencesCommerciales()
            ViderChampsReferences()

        Catch ex As Exception
            MessageBox.Show("Erreur modification : " & ex.Message)
        End Try
    End Sub

    ' ═══════════════════════════════════════════════════════════════════
    '  SUPPRIMER
    ' ═══════════════════════════════════════════════════════════════════

    Private Sub btnSupprimer_Click(sender As Object, e As EventArgs) Handles btnSupprimer.Click
        If txtIdReference.Text = "" Then
            MessageBox.Show("Sélectionne une référence.")
            Exit Sub
        End If
        If MessageBox.Show("Voulez-vous supprimer cette référence ?", "Confirmation",
                           MessageBoxButtons.YesNo, MessageBoxIcon.Question) = DialogResult.No Then Exit Sub
        Try
            ModuleDb.Executer(
                "DELETE FROM ReferencesCommerciales WHERE IdReference=@IdReference",
                Sub(cmd)
                    cmd.Parameters.Add("@IdReference", SqlDbType.Int).Value = CInt(txtIdReference.Text)
                End Sub)

            MessageBox.Show("Référence supprimée avec succès.")
            ChargerReferencesCommerciales()
            ViderChampsReferences()

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
        If txtRECHERCHE.Text.Trim() = "" Then
            ChargerReferencesCommerciales()
            Exit Sub
        End If
        Try
            Dim dt As DataTable = ModuleDb.Lire(
                "SELECT * FROM ReferencesCommerciales " &
                "WHERE CodeReference LIKE @mot OR NomReference LIKE @mot " &
                "ORDER BY IdReference DESC",
                Sub(cmd)
                    cmd.Parameters.Add("@mot", SqlDbType.NVarChar, 200).Value = "%" & txtRECHERCHE.Text.Trim() & "%"
                End Sub)

            DataGridView1.Columns.Clear()
            DataGridView1.AutoGenerateColumns = True
            DataGridView1.DataSource = dt
            FormaterGrilleReferences()

        Catch ex As Exception
            MessageBox.Show("Erreur recherche : " & ex.Message)
        End Try
    End Sub

    ' ═══════════════════════════════════════════════════════════════════
    '  NOUVEAU / ACTUALISER
    ' ═══════════════════════════════════════════════════════════════════

    Private Sub btnNouveau_Click(sender As Object, e As EventArgs) Handles btnNouveau.Click
        ViderChampsReferences()
    End Sub

    Private Sub btnActualiser_Click(sender As Object, e As EventArgs) Handles btnActualiser.Click
        ChargerReferencesCommerciales()
    End Sub

    ' ═══════════════════════════════════════════════════════════════════
    '  IMPRESSION (inchangée, simplifiée)
    ' ═══════════════════════════════════════════════════════════════════

    Private Sub PrintDocument1_PrintPage(sender As Object, e As Printing.PrintPageEventArgs) Handles PrintDocument1.PrintPage
        Dim fontTitre As New Font("Arial", 14, FontStyle.Bold)
        Dim fontTexte As New Font("Arial", 10, FontStyle.Regular)
        Dim y As Integer = 40

        e.Graphics.DrawString("Référence commerciale sélectionnée", fontTitre, Brushes.Black, 40, y) : y += 35

        Dim champs = {
            ("Code : ", "CodeReference"),
            ("Nom : ", "NomReference"),
            ("Produit : ", "Produit"),
            ("Dimension maximum : ", "DimensionMaximum"),
            ("Classe résistance : ", "ClasseResistance"),
            ("Classe compactation : ", "ClasseCompactation"),
            ("Classe exposition : ", "ClasseExposition"),
            ("Teneur chlorures : ", "TeneurChlorures"),
            ("Additions : ", "Additions"),
            ("Adjuvants : ", "Adjuvants"),
            ("Propriétés spéciales : ", "ProprietesSpeciales"),
            ("Ciment : ", "Ciment"),
            ("Masse volumique : ", "MasseVolumique"),
            ("Norme : ", "Norme"),
            ("Classe diffusion : ", "ClasseDiffusion"),
            ("Classe consistance : ", "ClasseConsistance"),
            ("Classe Vébè : ", "ClasseVebe"),
            ("Minimum teneur air béton : ", "MinimumTeneurAirBeton"),
            ("Durabilité : ", "Durabilite"),
            ("Champ libre 1 : ", "ChampLibre1"),
            ("Champ libre 2 : ", "ChampLibre2"),
            ("Champ libre 3 : ", "ChampLibre3"),
            ("Champ libre 4 : ", "ChampLibre4"),
            ("Actif : ", "Actif"),
            ("Dernière modification : ", "DerniereModification")
        }
        For Each c In champs
            Dim val As String = If(ligneSelectionnee?.Cells(c.Item2)?.Value Is Nothing, "",
                                   ligneSelectionnee.Cells(c.Item2).Value.ToString())
            e.Graphics.DrawString(c.Item1 & val, fontTexte, Brushes.Black, 40, y)
            y += 25
        Next
    End Sub

    ' ═══════════════════════════════════════════════════════════════════
    '  HELPERS
    ' ═══════════════════════════════════════════════════════════════════

    Private Sub AjouterParametres(cmd As SqlCommand)
        cmd.Parameters.Add("@CodeReference", SqlDbType.NVarChar, 50).Value = txtCodeReference.Text.Trim()
        cmd.Parameters.Add("@NomReference", SqlDbType.NVarChar, 200).Value = txtNomReference.Text.Trim()
        cmd.Parameters.Add("@Produit", SqlDbType.NVarChar, 100).Value = txtProduit.Text.Trim()
        cmd.Parameters.Add("@DimensionMaximum", SqlDbType.NVarChar, 50).Value = txtDimensionMaximum.Text.Trim()
        cmd.Parameters.Add("@ClasseResistance", SqlDbType.NVarChar, 50).Value = txtClasseResistance.Text.Trim()
        cmd.Parameters.Add("@ClasseCompactation", SqlDbType.NVarChar, 50).Value = txtClasseCompactation.Text.Trim()
        cmd.Parameters.Add("@ClasseExposition", SqlDbType.NVarChar, 100).Value = txtClasseExposition.Text.Trim()
        cmd.Parameters.Add("@TeneurChlorures", SqlDbType.NVarChar, 50).Value = txtTeneurChlorures.Text.Trim()
        cmd.Parameters.Add("@Additions", SqlDbType.NVarChar, 200).Value = txtAdditions.Text.Trim()
        cmd.Parameters.Add("@Adjuvants", SqlDbType.NVarChar, 200).Value = txtAdjuvants.Text.Trim()
        cmd.Parameters.Add("@ProprietesSpeciales", SqlDbType.NVarChar, 300).Value = txtProprietesSpeciales.Text.Trim()
        cmd.Parameters.Add("@Ciment", SqlDbType.NVarChar, 100).Value = txtCiment.Text.Trim()
        cmd.Parameters.Add("@MasseVolumique", SqlDbType.NVarChar, 50).Value = txtMasseVolumique.Text.Trim()
        cmd.Parameters.Add("@Norme", SqlDbType.NVarChar, 100).Value = txtNorme.Text.Trim()
        cmd.Parameters.Add("@ClasseDiffusion", SqlDbType.NVarChar, 50).Value = txtClasseDiffusion.Text.Trim()
        cmd.Parameters.Add("@ClasseConsistance", SqlDbType.NVarChar, 50).Value = txtClasseConsistance.Text.Trim()
        cmd.Parameters.Add("@ClasseVebe", SqlDbType.NVarChar, 50).Value = txtClasseVebe.Text.Trim()
        cmd.Parameters.Add("@MinimumTeneurAirBeton", SqlDbType.NVarChar, 50).Value = txtMinimumTeneurAirBeton.Text.Trim()
        cmd.Parameters.Add("@Durabilite", SqlDbType.NVarChar, 200).Value = txtDurabilite.Text.Trim()
        cmd.Parameters.Add("@ChampLibre1", SqlDbType.NVarChar, 200).Value = txtChampLibre1.Text.Trim()
        cmd.Parameters.Add("@ChampLibre2", SqlDbType.NVarChar, 200).Value = txtChampLibre2.Text.Trim()
        cmd.Parameters.Add("@ChampLibre3", SqlDbType.NVarChar, 200).Value = txtChampLibre3.Text.Trim()
        cmd.Parameters.Add("@ChampLibre4", SqlDbType.NVarChar, 200).Value = txtChampLibre4.Text.Trim()
        cmd.Parameters.Add("@Actif", SqlDbType.Bit).Value = chkActif.Checked
        If txtDerniereModification.Text.Trim() = "" Then
            cmd.Parameters.Add("@DerniereModification", SqlDbType.DateTime).Value = DBNull.Value
        Else
            Dim d As Date
            cmd.Parameters.Add("@DerniereModification", SqlDbType.DateTime).Value =
                If(Date.TryParse(txtDerniereModification.Text.Trim(), d), CObj(d), DBNull.Value)
        End If
    End Sub

    Private Function LireCell(row As DataGridViewRow, col As String) As String
        Return If(IsDBNull(row.Cells(col).Value), "", row.Cells(col).Value.ToString())
    End Function

    Private Sub ViderChampsReferences()
        txtIdReference.Text = "" : txtCodeReference.Text = "" : txtNomReference.Text = ""
        txtProduit.Text = "" : txtDimensionMaximum.Text = "" : txtClasseResistance.Text = ""
        txtClasseCompactation.Text = "" : txtClasseExposition.Text = "" : txtTeneurChlorures.Text = ""
        txtAdditions.Text = "" : txtAdjuvants.Text = "" : txtProprietesSpeciales.Text = ""
        txtCiment.Text = "" : txtMasseVolumique.Text = "" : txtNorme.Text = ""
        txtClasseDiffusion.Text = "" : txtClasseConsistance.Text = "" : txtClasseVebe.Text = ""
        txtMinimumTeneurAirBeton.Text = "" : txtDurabilite.Text = ""
        txtChampLibre1.Text = "" : txtChampLibre2.Text = "" : txtChampLibre3.Text = "" : txtChampLibre4.Text = ""
        txtDerniereModification.Text = "" : txtRECHERCHE.Text = ""
        chkActif.Checked = False
        ligneSelectionnee = Nothing
    End Sub

End Class