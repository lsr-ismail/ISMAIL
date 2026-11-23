Imports Microsoft.Data.SqlClient
Imports System.Data

Public Class Tables

    Dim chargementEnCours As Boolean = False
    Dim idTableSelectionne As Integer = 0
    Dim modeEditionTable As Boolean = False

    ' ═══════════════════════════════════════════════════════════════════
    '  LOAD
    ' ═══════════════════════════════════════════════════════════════════

    Private Sub Tables_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        ChargerTables()
        cmbNomTable.Items.Clear()
        For Each item In {"ADDITION", "ATTESTATION", "CERTIFICATION", "CHAUFFEUR",
                          "CLASSE_EXPOSITION", "CLASSE_CHLORURE", "RESISTANCE",
                          "CLIENT_COMPTANT", "CONSISTANCE", "CONTROLE", "DESIGNATION",
                          "DEVISE", "EQUIPEMENT", "ESSAI_PERFORMANCE", "FAMILLE_BETON",
                          "FAMILLE_CAMION", "FAMILLE_MATERIAU", "GRANULARITE",
                          "MODE_REGLEMENT", "MOTIF_ANNULATION", "NATURE_CIMENT", "UNITE_TARIF"}
            cmbNomTable.Items.Add(item)
        Next
        GererAffichageChampsTable()
    End Sub

    ' ═══════════════════════════════════════════════════════════════════
    '  CHARGEMENT
    ' ═══════════════════════════════════════════════════════════════════

    Private Sub ChargerTables()
        Try
            Dim dt As DataTable = ModuleDb.Lire(
                "SELECT IdTable, NomTable, CodeElement, NomElement, ServiceAssocie1, ServiceAssocie2, " &
                "ServiceAssocie3, CodeExport, Normee, Impression " &
                "FROM dbo.Tables ORDER BY NomTable, CodeElement")

            DataGridViewTables.DataSource = dt
            AfficherSeulement3Colonnes()

            If DataGridViewTables.Columns.Count > 0 Then
                DataGridViewTables.Columns("IdTable").Visible = False
                DataGridViewTables.Columns("NomTable").HeaderText = "Table"
                DataGridViewTables.Columns("CodeElement").HeaderText = "Code"
                DataGridViewTables.Columns("NomElement").HeaderText = "Nom"
            End If

            DataGridViewTables.ReadOnly = True
            DataGridViewTables.SelectionMode = DataGridViewSelectionMode.FullRowSelect
            DataGridViewTables.AllowUserToAddRows = False
            DataGridViewTables.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill

        Catch ex As Exception
            MessageBox.Show("Erreur chargement tables : " & ex.Message)
        End Try
    End Sub

    ' ═══════════════════════════════════════════════════════════════════
    '  CLIC GRILLE
    ' ═══════════════════════════════════════════════════════════════════

    Private Sub DataGridViewTables_CellClick(sender As Object, e As DataGridViewCellEventArgs) Handles DataGridViewTables.CellClick
        If e.RowIndex < 0 Then Exit Sub
        Try
            chargementEnCours = True
            Dim row As DataGridViewRow = DataGridViewTables.Rows(e.RowIndex)
            cmbNomTable.Text = row.Cells(1).Value.ToString()
            txtCodeElement.Text = row.Cells(2).Value.ToString()
            txtNomElement.Text = row.Cells(3).Value.ToString()
            chargementEnCours = False
            GererAffichageChampsTable()

            Dim dt As DataTable = ModuleDb.Lire(
                "SELECT * FROM dbo.Tables WHERE IdTable=@IdTable",
                Sub(cmd) cmd.Parameters.Add("@IdTable", SqlDbType.Int).Value = Convert.ToInt32(row.Cells(0).Value))

            If dt.Rows.Count > 0 Then
                Dim dr As DataRow = dt.Rows(0)
                idTableSelectionne = Convert.ToInt32(dr("IdTable"))
                txtServiceAssocie1.Text = If(IsDBNull(dr("ServiceAssocie1")), "", dr("ServiceAssocie1").ToString())
                txtServiceAssocie2.Text = If(IsDBNull(dr("ServiceAssocie2")), "", dr("ServiceAssocie2").ToString())
                txtServiceAssocie3.Text = If(IsDBNull(dr("ServiceAssocie3")), "", dr("ServiceAssocie3").ToString())
                txtCodeExport.Text = If(IsDBNull(dr("CodeExport")), "", dr("CodeExport").ToString())
                chkNormee.Checked = Not IsDBNull(dr("Normee")) AndAlso Convert.ToBoolean(dr("Normee"))
                txtImpression.Text = If(IsDBNull(dr("Impression")), "", dr("Impression").ToString())
            End If

        Catch ex As Exception
            MessageBox.Show("Erreur sélection ligne : " & ex.Message)
        Finally
            chargementEnCours = False
        End Try
    End Sub

    ' ═══════════════════════════════════════════════════════════════════
    '  AJOUTER
    ' ═══════════════════════════════════════════════════════════════════

    Private Sub btnAjouteTable_Click(sender As Object, e As EventArgs) Handles btnAjouteTable.Click
        If cmbNomTable.Text.Trim() = "" Then MessageBox.Show("Le nom de table est obligatoire.") : Exit Sub
        If txtCodeElement.Text.Trim() = "" Then MessageBox.Show("Le code élément est obligatoire.") : Exit Sub
        If txtNomElement.Text.Trim() = "" Then MessageBox.Show("Le nom élément est obligatoire.") : Exit Sub
        Try
            Dim nb As Integer = ModuleDb.LireValeur(Of Integer)(
                "SELECT COUNT(*) FROM dbo.Tables WHERE NomTable=@NomTable AND CodeElement=@CodeElement",
                Sub(cmd)
                    cmd.Parameters.Add("@NomTable", SqlDbType.NVarChar, 100).Value = cmbNomTable.Text.Trim()
                    cmd.Parameters.Add("@CodeElement", SqlDbType.NVarChar, 50).Value = txtCodeElement.Text.Trim()
                End Sub)

            If nb > 0 Then
                MessageBox.Show("Cet élément existe déjà dans cette table.")
                Exit Sub
            End If

            ModuleDb.Executer(
                "INSERT INTO dbo.Tables " &
                "(NomTable, CodeElement, NomElement, ServiceAssocie1, ServiceAssocie2, ServiceAssocie3, " &
                "CodeExport, Normee, Impression, DerniereModification) " &
                "VALUES (@NomTable, @CodeElement, @NomElement, @ServiceAssocie1, @ServiceAssocie2, @ServiceAssocie3, " &
                "@CodeExport, @Normee, @Impression, GETDATE())",
                AddressOf AjouterParametres)

            MessageBox.Show("Élément ajouté avec succès.")
            ChargerTables()
            ViderChampsTable()

        Catch ex As Exception
            MessageBox.Show("Erreur ajout table : " & ex.Message)
        End Try
    End Sub

    ' ═══════════════════════════════════════════════════════════════════
    '  ÉDITER (activation mode)
    ' ═══════════════════════════════════════════════════════════════════

    Private Sub btnEditeTable_Click(sender As Object, e As EventArgs) Handles btnEditeTable.Click
        If idTableSelectionne = 0 Then
            MessageBox.Show("Sélectionne d'abord une ligne.")
            Exit Sub
        End If
        modeEditionTable = True
        MessageBox.Show("Modifiez les champs puis cliquez sur Actualiser.")
    End Sub

    ' ═══════════════════════════════════════════════════════════════════
    '  ACTUALISER / MODIFIER
    ' ═══════════════════════════════════════════════════════════════════

    Private Sub btnActualiserTable_Click(sender As Object, e As EventArgs) Handles btnActualiserTable.Click
        If Not modeEditionTable Then
            ChargerTables()
            Exit Sub
        End If
        If idTableSelectionne = 0 Then
            MessageBox.Show("Sélectionne d'abord une ligne.")
            Exit Sub
        End If
        Try
            ModuleDb.Executer(
                "UPDATE dbo.Tables SET " &
                "NomTable=@NomTable, CodeElement=@CodeElement, NomElement=@NomElement, " &
                "ServiceAssocie1=@ServiceAssocie1, ServiceAssocie2=@ServiceAssocie2, ServiceAssocie3=@ServiceAssocie3, " &
                "CodeExport=@CodeExport, Normee=@Normee, Impression=@Impression, DerniereModification=GETDATE() " &
                "WHERE IdTable=@IdTable",
                Sub(cmd)
                    cmd.Parameters.Add("@IdTable", SqlDbType.Int).Value = idTableSelectionne
                    AjouterParametres(cmd)
                End Sub)

            MessageBox.Show("Élément modifié avec succès.")
            modeEditionTable = False
            ChargerTables()
            ViderChampsTable()

        Catch ex As Exception
            MessageBox.Show("Erreur modification table : " & ex.Message)
        End Try
    End Sub

    ' ═══════════════════════════════════════════════════════════════════
    '  SUPPRIMER
    ' ═══════════════════════════════════════════════════════════════════

    Private Sub btnSupprimeTable_Click(sender As Object, e As EventArgs) Handles btnSupprimeTable.Click
        If idTableSelectionne = 0 Then
            MessageBox.Show("Sélectionne d'abord une ligne.")
            Exit Sub
        End If
        If MessageBox.Show("Voulez-vous supprimer cet élément ?", "Confirmation",
                           MessageBoxButtons.YesNo, MessageBoxIcon.Question) = DialogResult.No Then Exit Sub
        Try
            ModuleDb.Executer(
                "DELETE FROM dbo.Tables WHERE IdTable=@IdTable",
                Sub(cmd) cmd.Parameters.Add("@IdTable", SqlDbType.Int).Value = idTableSelectionne)

            MessageBox.Show("Élément supprimé.")
            ChargerTables()
            ViderChampsTable()

        Catch ex As Exception
            MessageBox.Show("Erreur suppression table : " & ex.Message)
        End Try
    End Sub

    ' ═══════════════════════════════════════════════════════════════════
    '  RECHERCHE
    ' ═══════════════════════════════════════════════════════════════════

    Private Sub btnRechercheTable_Click(sender As Object, e As EventArgs) Handles btnRechercheTable.Click
        Try
            Dim dt As DataTable = ModuleDb.Lire(
                "SELECT IdTable, NomTable, CodeElement, NomElement, ServiceAssocie1, ServiceAssocie2, " &
                "ServiceAssocie3, CodeExport, Normee, Impression " &
                "FROM dbo.Tables " &
                "WHERE NomTable LIKE @rech OR CodeElement LIKE @rech OR NomElement LIKE @rech " &
                "ORDER BY NomTable, CodeElement",
                Sub(cmd)
                    cmd.Parameters.Add("@rech", SqlDbType.NVarChar, 200).Value = "%" & txtNomElement.Text.Trim() & "%"
                End Sub)

            DataGridViewTables.DataSource = dt

        Catch ex As Exception
            MessageBox.Show("Erreur recherche table : " & ex.Message)
        End Try
    End Sub

    ' ═══════════════════════════════════════════════════════════════════
    '  COMBO CHANGEMENT
    ' ═══════════════════════════════════════════════════════════════════

    Private Sub cmbNomTable_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cmbNomTable.SelectedIndexChanged
        If chargementEnCours Then Exit Sub
        GererAffichageChampsTable()
        ChargerTablesParType()
    End Sub

    Private Sub ChargerTablesParType()
        Try
            Dim dt As DataTable = ModuleDb.Lire(
                "SELECT IdTable, NomTable, CodeElement, NomElement, ServiceAssocie1, ServiceAssocie2, " &
                "ServiceAssocie3, CodeExport, Normee, Impression " &
                "FROM dbo.Tables WHERE NomTable=@NomTable ORDER BY CodeElement",
                Sub(cmd)
                    cmd.Parameters.Add("@NomTable", SqlDbType.NVarChar, 100).Value = cmbNomTable.Text.Trim()
                End Sub)

            DataGridViewTables.DataSource = dt
            AfficherSeulement3Colonnes()

        Catch ex As Exception
            MessageBox.Show("Erreur chargement table : " & ex.Message)
        End Try
    End Sub

    ' ═══════════════════════════════════════════════════════════════════
    '  NOUVEAU / IMPRIMER
    ' ═══════════════════════════════════════════════════════════════════

    Private Sub btnNouveauTable_Click(sender As Object, e As EventArgs) Handles btnNouveauTable.Click
        ViderChampsTable()
        cmbNomTable.Focus()
    End Sub

    Private Sub Imprimer_Click(sender As Object, e As EventArgs) Handles Imprimer.Click
        MessageBox.Show("Impression en cours de préparation.")
    End Sub

    ' ═══════════════════════════════════════════════════════════════════
    '  AFFICHAGE COLONNES / CHAMPS
    ' ═══════════════════════════════════════════════════════════════════

    Private Sub AfficherSeulement3Colonnes()
        If DataGridViewTables.Columns.Count = 0 Then Exit Sub
        For Each col As DataGridViewColumn In DataGridViewTables.Columns
            col.Visible = False
        Next
        If DataGridViewTables.Columns.Contains("NomTable") Then
            DataGridViewTables.Columns("NomTable").Visible = True
            DataGridViewTables.Columns("NomTable").HeaderText = "Table"
        End If
        If DataGridViewTables.Columns.Contains("CodeElement") Then
            DataGridViewTables.Columns("CodeElement").Visible = True
            DataGridViewTables.Columns("CodeElement").HeaderText = "Code"
        End If
        If DataGridViewTables.Columns.Contains("NomElement") Then
            DataGridViewTables.Columns("NomElement").Visible = True
            DataGridViewTables.Columns("NomElement").HeaderText = "Nom"
        End If
    End Sub

    Private Sub GererAffichageChampsTable()
        txtServiceAssocie1.Visible = False : txtServiceAssocie2.Visible = False : txtServiceAssocie3.Visible = False
        txtCodeExport.Visible = False : chkNormee.Visible = False : txtImpression.Visible = False
        lblServiceAssocie1.Visible = False : lblServiceAssocie2.Visible = False : lblServiceAssocie3.Visible = False
        lblCodeExport.Visible = False : lblNormee.Visible = False : lblImpression.Visible = False

        Select Case cmbNomTable.Text.Trim().ToUpper()
            Case "DESIGNATION"
                chkNormee.Visible = True : lblNormee.Visible = True
                chkNormee.Location = New Point(320, 163)
                lblNormee.Location = New Point(38, 163)
            Case "UNITE_TARIF"
                txtImpression.Visible = True : lblImpression.Visible = True
                txtImpression.Location = New Point(175, 163)
                lblImpression.Location = New Point(38, 163)
            Case "CLIENT_COMPTANT"
                txtCodeExport.Visible = True : lblCodeExport.Visible = True
                txtCodeExport.Location = New Point(175, 163)
                lblCodeExport.Location = New Point(38, 163)
            Case "EQUIPEMENT", "FAMILLE_CAMION"
                txtServiceAssocie1.Visible = True : txtServiceAssocie2.Visible = True : txtServiceAssocie3.Visible = True
                lblServiceAssocie1.Visible = True : lblServiceAssocie2.Visible = True : lblServiceAssocie3.Visible = True
        End Select
    End Sub

    ' ═══════════════════════════════════════════════════════════════════
    '  HELPERS
    ' ═══════════════════════════════════════════════════════════════════

    Private Sub AjouterParametres(cmd As SqlCommand)
        cmd.Parameters.Add("@NomTable", SqlDbType.NVarChar, 100).Value = cmbNomTable.Text.Trim()
        cmd.Parameters.Add("@CodeElement", SqlDbType.NVarChar, 50).Value = txtCodeElement.Text.Trim()
        cmd.Parameters.Add("@NomElement", SqlDbType.NVarChar, 200).Value = txtNomElement.Text.Trim()
        cmd.Parameters.Add("@ServiceAssocie1", SqlDbType.NVarChar, 100).Value = txtServiceAssocie1.Text.Trim()
        cmd.Parameters.Add("@ServiceAssocie2", SqlDbType.NVarChar, 100).Value = txtServiceAssocie2.Text.Trim()
        cmd.Parameters.Add("@ServiceAssocie3", SqlDbType.NVarChar, 100).Value = txtServiceAssocie3.Text.Trim()
        cmd.Parameters.Add("@CodeExport", SqlDbType.NVarChar, 50).Value = txtCodeExport.Text.Trim()
        cmd.Parameters.Add("@Normee", SqlDbType.Bit).Value = chkNormee.Checked
        cmd.Parameters.Add("@Impression", SqlDbType.NVarChar, 100).Value = txtImpression.Text.Trim()
    End Sub

    Private Sub ViderChampsTable()
        cmbNomTable.Text = "" : txtCodeElement.Text = "" : txtNomElement.Text = ""
        txtServiceAssocie1.Text = "" : txtServiceAssocie2.Text = "" : txtServiceAssocie3.Text = ""
        txtCodeExport.Text = "" : chkNormee.Checked = False : txtImpression.Text = ""
        idTableSelectionne = 0 : modeEditionTable = False
    End Sub

    Private Sub DataGridViewTables_CellContentClick(sender As Object, e As DataGridViewCellEventArgs) Handles DataGridViewTables.CellContentClick
    End Sub

End Class