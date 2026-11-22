Imports Microsoft.Data.SqlClient
Imports System.Data

' ================================================================
'  Configuration générale — 19/09/2026 (écran complété)
'  Société (Entete1-4 = nom / adresse / téléphone / fax, ICE)
'  → dbo.ConfigurationSociete + dbo.Societe (comme avant)
'  Imprimantes, copies, entêtes, formats, commentaires BL, options
'  OUI/NON, mode chantier → ModuleConfigImpression (colonnes
'  ajoutées automatiquement dans ConfigurationSociete).
' ================================================================
Public Class Configuration_générale

    Private Sub Configuration_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        ChargerImprimantes()
        ChargerConfiguration()
    End Sub

    ' ── Imprimantes installées ──────────────────────────────────
    Private Sub ChargerImprimantes()
        Try
            For Each cmb In {cmbImprimanteBL, cmbImprimantePeseesBPE, cmbImprimantePeseesPrefa, cmbImprimanteFacture}
                cmb.Items.Clear()
                cmb.Items.Add("")                       ' vide = boîte de dialogue / imprimante par défaut
                For Each imp As String In System.Drawing.Printing.PrinterSettings.InstalledPrinters
                    cmb.Items.Add(imp)
                Next
            Next
        Catch ex As Exception
            MessageBox.Show("Erreur chargement imprimantes : " & ex.Message)
        End Try
    End Sub

    ' ── Chargement ──────────────────────────────────────────────
    Private Sub ChargerConfiguration()
        Try
            ' Société
            Dim dt As DataTable = ModuleDb.Lire("SELECT TOP 1 * FROM dbo.ConfigurationSociete")
            If dt.Rows.Count > 0 Then
                Dim dr As DataRow = dt.Rows(0)
                txtEntete1.Text = LireTexte(dr, "NomSociete")
                txtEntete2.Text = LireTexte(dr, "Adresse")
                txtEntete3.Text = LireTexte(dr, "Telephone")
                txtEntete4.Text = LireTexte(dr, "Fax")
                txtICE.Text = LireTexte(dr, "NumeroICE")
            End If

            ' Impression / options
            ModuleConfigImpression.Charger()
            SetComboBox(cmbImprimanteBL, ModuleConfigImpression.ImprimanteBL)
            SetComboBox(cmbImprimantePeseesBPE, ModuleConfigImpression.ImprimantePeseesBPE)
            SetComboBox(cmbImprimantePeseesPrefa, ModuleConfigImpression.ImprimantePeseesPrefa)
            SetComboBox(cmbImprimanteFacture, ModuleConfigImpression.ImprimanteFacture)
            txtImprimantePrincipale.Text = ModuleConfigImpression.ImprimantePrincipale
            txtNbreCopiesBL.Text = ModuleConfigImpression.NbreCopiesBL.ToString()
            txtNbreCopiesFactures.Text = ModuleConfigImpression.NbreCopiesFactures.ToString()
            txtEnteteBL.Text = ModuleConfigImpression.EnteteBL
            txtEnteteTicket.Text = ModuleConfigImpression.EnteteTicket
            txtEnteteFacture.Text = ModuleConfigImpression.EnteteFacture
            txtEnteteListing.Text = ModuleConfigImpression.EnteteListing
            txtLargeurBL.Text = ModuleConfigImpression.LargeurBL.ToString()
            txtHauteurBL.Text = ModuleConfigImpression.HauteurBL.ToString()
            txtLargeurFacture.Text = ModuleConfigImpression.LargeurFacture.ToString()
            txtHauteurFacture.Text = ModuleConfigImpression.HauteurFacture.ToString()
            txtCommentairesBL.Text = ModuleConfigImpression.CommentairesBL
            txtOperateur.Text = ModuleConfigImpression.Operateur
            SetBoutonOuiNon(btnSansEntete, ModuleConfigImpression.EnteteActive)
            SetBoutonOuiNon(btnSansNorme, ModuleConfigImpression.NormeActive)
            SetBoutonOuiNon(btnDraft, ModuleConfigImpression.Draft)
            SetBoutonOuiNon(btnTemperature, ModuleConfigImpression.Temperature)
            SetBoutonOuiNon(btnPapierBL, ModuleConfigImpression.PapierBL)
            SetBoutonOuiNon(btnPapierFacture, ModuleConfigImpression.PapierFacture)
            Select Case ModuleConfigImpression.ModeChantier
                Case "PARAMETRE_CHANTIER" : rbParametreChantier.Checked = True
                Case "JAMAIS" : rbJamais.Checked = True
                Case Else : rbToujours.Checked = True
            End Select
            btnValider.Text = "Valider"
            btnAbandonner.Text = "Abandonner"
        Catch ex As Exception
            MessageBox.Show("Erreur chargement : " & ex.Message)
        End Try
    End Sub

    ' ── Boutons OUI / NON ───────────────────────────────────────
    Private Sub BoutonOuiNon_Click(sender As Object, e As EventArgs) _
        Handles btnSansEntete.Click, btnSansNorme.Click, btnDraft.Click,
                btnTemperature.Click, btnPapierBL.Click, btnPapierFacture.Click
        Dim btn As Button = CType(sender, Button)
        SetBoutonOuiNon(btn, btn.Text <> "OUI")
    End Sub

    ' ── Valider ─────────────────────────────────────────────────
    Private Sub btnValider_Click(sender As Object, e As EventArgs) Handles btnValider.Click
        Try
            ' 1. Société (comme avant)
            ModuleDb.Executer(
                "UPDATE dbo.ConfigurationSociete SET " &
                "NomSociete=@Nom, Adresse=@Adresse, Telephone=@Tel, Fax=@Fax, NumeroICE=@ICE",
                Sub(cmd)
                    cmd.Parameters.Add("@Nom", SqlDbType.NVarChar, 150).Value = txtEntete1.Text.Trim()
                    cmd.Parameters.Add("@Adresse", SqlDbType.NVarChar, 250).Value = txtEntete2.Text.Trim()
                    cmd.Parameters.Add("@Tel", SqlDbType.NVarChar, 50).Value = txtEntete3.Text.Trim()
                    cmd.Parameters.Add("@Fax", SqlDbType.NVarChar, 50).Value = txtEntete4.Text.Trim()
                    cmd.Parameters.Add("@ICE", SqlDbType.NVarChar, 50).Value = txtICE.Text.Trim()
                End Sub)
            MettreAJourSociete()

            ' 2. Impression / options
            ModuleConfigImpression.ImprimanteBL = cmbImprimanteBL.Text.Trim()
            ModuleConfigImpression.ImprimantePeseesBPE = cmbImprimantePeseesBPE.Text.Trim()
            ModuleConfigImpression.ImprimantePeseesPrefa = cmbImprimantePeseesPrefa.Text.Trim()
            ModuleConfigImpression.ImprimanteFacture = cmbImprimanteFacture.Text.Trim()
            ModuleConfigImpression.ImprimantePrincipale = txtImprimantePrincipale.Text.Trim()
            ModuleConfigImpression.NbreCopiesBL = Math.Max(1, ParseInt(txtNbreCopiesBL.Text))
            ModuleConfigImpression.NbreCopiesFactures = Math.Max(1, ParseInt(txtNbreCopiesFactures.Text))
            ModuleConfigImpression.EnteteBL = txtEnteteBL.Text.Trim()
            ModuleConfigImpression.EnteteTicket = txtEnteteTicket.Text.Trim()
            ModuleConfigImpression.EnteteFacture = txtEnteteFacture.Text.Trim()
            ModuleConfigImpression.EnteteListing = txtEnteteListing.Text.Trim()
            ModuleConfigImpression.LargeurBL = ParseInt(txtLargeurBL.Text)
            ModuleConfigImpression.HauteurBL = ParseInt(txtHauteurBL.Text)
            ModuleConfigImpression.LargeurFacture = ParseInt(txtLargeurFacture.Text)
            ModuleConfigImpression.HauteurFacture = ParseInt(txtHauteurFacture.Text)
            ModuleConfigImpression.CommentairesBL = txtCommentairesBL.Text.Trim()
            ModuleConfigImpression.Operateur = txtOperateur.Text.Trim()
            ModuleConfigImpression.EnteteActive = (btnSansEntete.Text = "OUI")
            ModuleConfigImpression.NormeActive = (btnSansNorme.Text = "OUI")
            ModuleConfigImpression.Draft = (btnDraft.Text = "OUI")
            ModuleConfigImpression.Temperature = (btnTemperature.Text = "OUI")
            ModuleConfigImpression.PapierBL = (btnPapierBL.Text = "OUI")
            ModuleConfigImpression.PapierFacture = (btnPapierFacture.Text = "OUI")
            ModuleConfigImpression.ModeChantier = If(rbParametreChantier.Checked, "PARAMETRE_CHANTIER", If(rbJamais.Checked, "JAMAIS", "TOUJOURS"))
            If Not ModuleConfigImpression.Enregistrer() Then Exit Sub

            txtNbreCopiesBL.Text = ModuleConfigImpression.NbreCopiesBL.ToString()
            txtNbreCopiesFactures.Text = ModuleConfigImpression.NbreCopiesFactures.ToString()
            MessageBox.Show("✅ Configuration sauvegardée avec succès.")
        Catch ex As Exception
            MessageBox.Show("Erreur sauvegarde : " & ex.Message)
        End Try
    End Sub

    ' Variables société en mémoire + table dbo.Societe (comme avant)
    Private Sub MettreAJourSociete()
        SocieteNom = txtEntete1.Text.Trim()
        SocieteAdresse = txtEntete2.Text.Trim()
        SocieteTelephone = txtEntete3.Text.Trim()
        SocieteFax = txtEntete4.Text.Trim()
        SocieteICE = txtICE.Text.Trim()
        Try
            ModuleDb.Executer(
                "UPDATE dbo.Societe SET NomSociete=@Nom, Adresse=@Adresse, Telephone=@Tel, Fax=@Fax, NumeroICE=@ICE WHERE IdSociete=1",
                Sub(cmd)
                    cmd.Parameters.Add("@Nom", SqlDbType.NVarChar, 150).Value = txtEntete1.Text.Trim()
                    cmd.Parameters.Add("@Adresse", SqlDbType.NVarChar, 250).Value = txtEntete2.Text.Trim()
                    cmd.Parameters.Add("@Tel", SqlDbType.NVarChar, 50).Value = txtEntete3.Text.Trim()
                    cmd.Parameters.Add("@Fax", SqlDbType.NVarChar, 50).Value = txtEntete4.Text.Trim()
                    cmd.Parameters.Add("@ICE", SqlDbType.NVarChar, 50).Value = txtICE.Text.Trim()
                End Sub)
        Catch
        End Try
    End Sub

    Private Sub btnAbandonner_Click(sender As Object, e As EventArgs) Handles btnAbandonner.Click
        Me.Close()
    End Sub

    ' ── Helpers ─────────────────────────────────────────────────
    Private Function LireTexte(dr As DataRow, col As String) As String
        If Not dr.Table.Columns.Contains(col) OrElse IsDBNull(dr(col)) Then Return ""
        Return dr(col).ToString()
    End Function

    Private Function ParseInt(txt As String) As Integer
        Dim v As Integer
        Integer.TryParse(txt.Trim(), v)
        Return v
    End Function

    Private Sub SetComboBox(cmb As ComboBox, valeur As String)
        Dim idx As Integer = cmb.FindStringExact(valeur)
        If idx >= 0 Then
            cmb.SelectedIndex = idx
        Else
            cmb.SelectedIndex = -1
            cmb.Text = valeur
        End If
    End Sub

    Private Sub SetBoutonOuiNon(btn As Button, valeur As Boolean)
        btn.Text = If(valeur, "OUI", "NON")
        btn.BackColor = If(valeur, Color.FromArgb(144, 238, 144), Color.FromArgb(255, 182, 193))
        btn.ForeColor = Color.Black
        btn.FlatStyle = FlatStyle.Flat
    End Sub

End Class
