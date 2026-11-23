Imports Microsoft.Data.SqlClient
Imports System.Data

Public Class Normes

    Dim idNormeSelectionne As Integer = 0
    Dim modeEditionNorme As Boolean = False

    ' ═══════════════════════════════════════════════════════════════════
    '  LOAD
    ' ═══════════════════════════════════════════════════════════════════

    Private Sub Normes_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        ChargerNormes()
    End Sub

    ' ═══════════════════════════════════════════════════════════════════
    '  CHARGEMENT
    ' ═══════════════════════════════════════════════════════════════════

    Private Sub ChargerNormes()
        Try
            Dim dt As DataTable = ModuleDb.Lire(
                "SELECT IdNorme, ClasseExposition, RapportECLimite, ClasseResistance, " &
                "TeneurMiniLiant, CoefD, CoefL, CoefS, CoefU, CoefV " &
                "FROM dbo.Normes ORDER BY IdNorme DESC")

            DataGridViewNormes.DataSource = dt
            DataGridViewNormes.ReadOnly = True
            DataGridViewNormes.SelectionMode = DataGridViewSelectionMode.FullRowSelect
            DataGridViewNormes.AllowUserToAddRows = False
            DataGridViewNormes.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill

            If DataGridViewNormes.Columns.Count > 0 Then
                DataGridViewNormes.Columns("IdNorme").Visible = False
                DataGridViewNormes.Columns("ClasseExposition").HeaderText = "Classe exposition"
                DataGridViewNormes.Columns("RapportECLimite").HeaderText = "Rapport E/C"
                DataGridViewNormes.Columns("ClasseResistance").HeaderText = "Résistance"
                DataGridViewNormes.Columns("TeneurMiniLiant").HeaderText = "Liant mini"
            End If

        Catch ex As Exception
            MessageBox.Show("Erreur chargement normes : " & ex.Message)
        End Try
    End Sub

    ' ═══════════════════════════════════════════════════════════════════
    '  CLIC GRILLE
    ' ═══════════════════════════════════════════════════════════════════

    Private Sub DataGridViewNormes_CellClick(sender As Object, e As DataGridViewCellEventArgs) Handles DataGridViewNormes.CellClick
        If e.RowIndex < 0 Then Exit Sub
        Dim row As DataGridViewRow = DataGridViewNormes.Rows(e.RowIndex)
        idNormeSelectionne = Convert.ToInt32(row.Cells("IdNorme").Value)

        txtClasseExposition.Text = LireTexte(row, "ClasseExposition")
        txtRapportECLimite.Text = LireTexte(row, "RapportECLimite")
        txtClasseResistance.Text = LireTexte(row, "ClasseResistance")
        txtTeneurMiniLiant.Text = LireTexte(row, "TeneurMiniLiant")
        txtCoefD.Text = LireTexte(row, "CoefD")
        txtCoefL.Text = LireTexte(row, "CoefL")
        txtCoefS.Text = LireTexte(row, "CoefS")
        txtCoefU.Text = LireTexte(row, "CoefU")
        txtCoefV.Text = LireTexte(row, "CoefV")
    End Sub

    ' ═══════════════════════════════════════════════════════════════════
    '  AJOUTER
    ' ═══════════════════════════════════════════════════════════════════

    Private Sub btnAjouteNorme_Click(sender As Object, e As EventArgs) Handles btnAjouteNorme.Click
        If txtClasseExposition.Text.Trim() = "" Then
            MessageBox.Show("La classe exposition est obligatoire.")
            Exit Sub
        End If
        Try
            ModuleDb.Executer(
                "INSERT INTO dbo.Normes " &
                "(ClasseExposition, RapportECLimite, ClasseResistance, TeneurMiniLiant, " &
                "CoefD, CoefL, CoefS, CoefU, CoefV) " &
                "VALUES (@ClasseExposition, @RapportECLimite, @ClasseResistance, @TeneurMiniLiant, " &
                "@CoefD, @CoefL, @CoefS, @CoefU, @CoefV)",
                AddressOf AjouterParametres)

            MessageBox.Show("Norme ajoutée avec succès.")
            ChargerNormes()
            ViderChampsNorme()

        Catch ex As Exception
            MessageBox.Show("Erreur ajout norme : " & ex.Message)
        End Try
    End Sub

    ' ═══════════════════════════════════════════════════════════════════
    '  ÉDITER (activation mode)
    ' ═══════════════════════════════════════════════════════════════════

    Private Sub btnEditeNorme_Click(sender As Object, e As EventArgs) Handles btnEditeNorme.Click
        If idNormeSelectionne = 0 Then
            MessageBox.Show("Sélectionne d'abord une norme.")
            Exit Sub
        End If
        modeEditionNorme = True
        MessageBox.Show("Modifiez les champs puis cliquez sur Actualiser l'enregistrement.")
    End Sub

    ' ═══════════════════════════════════════════════════════════════════
    '  ACTUALISER / MODIFIER
    ' ═══════════════════════════════════════════════════════════════════

    Private Sub btnActualiserNormeme_Click(sender As Object, e As EventArgs) Handles btnActualiserNormeme.Click
        If Not modeEditionNorme Then
            ChargerNormes()
            Exit Sub
        End If
        If idNormeSelectionne = 0 Then
            MessageBox.Show("Sélectionne d'abord une norme.")
            Exit Sub
        End If
        Try
            Executer(
                "UPDATE dbo.Normes SET " &
                "ClasseExposition=@ClasseExposition, RapportECLimite=@RapportECLimite, " &
                "ClasseResistance=@ClasseResistance, TeneurMiniLiant=@TeneurMiniLiant, " &
                "CoefD=@CoefD, CoefL=@CoefL, CoefS=@CoefS, CoefU=@CoefU, CoefV=@CoefV " &
                "WHERE IdNorme=@IdNorme",
                Sub(cmd)
                    cmd.Parameters.Add("@IdNorme", SqlDbType.Int).Value = idNormeSelectionne
                    AjouterParametres(cmd)
                End Sub)

            MessageBox.Show("Norme modifiée avec succès.")
            modeEditionNorme = False
            ChargerNormes()
            ViderChampsNorme()

        Catch ex As Exception
            MessageBox.Show("Erreur modification norme : " & ex.Message)
        End Try
    End Sub

    ' ═══════════════════════════════════════════════════════════════════
    '  SUPPRIMER
    ' ═══════════════════════════════════════════════════════════════════

    Private Sub btnSupprimeNorme_Click(sender As Object, e As EventArgs) Handles btnSupprimeNorme.Click
        If idNormeSelectionne = 0 Then
            MessageBox.Show("Sélectionne d'abord une norme.")
            Exit Sub
        End If
        If MessageBox.Show("Voulez-vous supprimer cette norme ?", "Confirmation",
                           MessageBoxButtons.YesNo, MessageBoxIcon.Question) = DialogResult.No Then Exit Sub
        Try
            ModuleDb.Executer(
                "DELETE FROM dbo.Normes WHERE IdNorme=@IdNorme",
                Sub(cmd)
                    cmd.Parameters.Add("@IdNorme", SqlDbType.Int).Value = idNormeSelectionne
                End Sub)

            MessageBox.Show("Norme supprimée.")
            ChargerNormes()
            ViderChampsNorme()

        Catch ex As Exception
            MessageBox.Show("Erreur suppression norme : " & ex.Message)
        End Try
    End Sub

    ' ═══════════════════════════════════════════════════════════════════
    '  RECHERCHE
    ' ═══════════════════════════════════════════════════════════════════

    Private Sub btnRechercheNorme_Click(sender As Object, e As EventArgs) Handles btnRechercheNorme.Click
        Try
            Dim dt As DataTable = ModuleDb.Lire(
                "SELECT IdNorme, ClasseExposition, RapportECLimite, ClasseResistance, " &
                "TeneurMiniLiant, CoefD, CoefL, CoefS, CoefU, CoefV " &
                "FROM dbo.Normes " &
                "WHERE ClasseExposition LIKE @rech OR ClasseResistance LIKE @rech " &
                "ORDER BY IdNorme DESC",
                Sub(cmd)
                    cmd.Parameters.Add("@rech", SqlDbType.NVarChar, 200).Value = "%" & txtClasseExposition.Text.Trim() & "%"
                End Sub)

            DataGridViewNormes.DataSource = dt
            If DataGridViewNormes.Columns.Contains("IdNorme") Then
                DataGridViewNormes.Columns("IdNorme").Visible = False
            End If

        Catch ex As Exception
            MessageBox.Show("Erreur recherche norme : " & ex.Message)
        End Try
    End Sub

    ' ═══════════════════════════════════════════════════════════════════
    '  HELPERS
    ' ═══════════════════════════════════════════════════════════════════

    Private Sub AjouterParametres(cmd As SqlCommand)
        cmd.Parameters.Add("@ClasseExposition", SqlDbType.NVarChar, 100).Value = txtClasseExposition.Text.Trim()
        cmd.Parameters.Add("@RapportECLimite", SqlDbType.Decimal).Value = NombreValeur(txtRapportECLimite)
        cmd.Parameters.Add("@ClasseResistance", SqlDbType.NVarChar, 50).Value = txtClasseResistance.Text.Trim()
        cmd.Parameters.Add("@TeneurMiniLiant", SqlDbType.Decimal).Value = NombreValeur(txtTeneurMiniLiant)
        cmd.Parameters.Add("@CoefD", SqlDbType.Decimal).Value = NombreValeur(txtCoefD)
        cmd.Parameters.Add("@CoefL", SqlDbType.Decimal).Value = NombreValeur(txtCoefL)
        cmd.Parameters.Add("@CoefS", SqlDbType.Decimal).Value = NombreValeur(txtCoefS)
        cmd.Parameters.Add("@CoefU", SqlDbType.Decimal).Value = NombreValeur(txtCoefU)
        cmd.Parameters.Add("@CoefV", SqlDbType.Decimal).Value = NombreValeur(txtCoefV)
    End Sub

    Private Function NombreValeur(txt As TextBox) As Decimal
        If txt.Text.Trim() = "" Then Return 0
        Dim v As Decimal
        Decimal.TryParse(txt.Text.Replace(",", "."),
            Globalization.NumberStyles.Any,
            Globalization.CultureInfo.InvariantCulture, v)
        Return v
    End Function

    Private Function LireTexte(row As DataGridViewRow, col As String) As String
        Return If(IsDBNull(row.Cells(col).Value), "", row.Cells(col).Value.ToString())
    End Function

    Private Sub ViderChampsNorme()
        txtClasseExposition.Text = "" : txtRapportECLimite.Text = "" : txtClasseResistance.Text = ""
        txtTeneurMiniLiant.Text = "" : txtCoefD.Text = "" : txtCoefL.Text = ""
        txtCoefS.Text = "" : txtCoefU.Text = "" : txtCoefV.Text = ""
        idNormeSelectionne = 0
        modeEditionNorme = False
    End Sub

End Class