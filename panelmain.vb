Imports Microsoft.Data.SqlClient
Imports S7.Net

Public Class PanelMain

    Private plc As Plc



    ' ── MENU PRINCIPAL ───────────────────────────────────────────────
    Private Sub ClientsToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles ClientsToolStripMenuItem.Click
        FermerAutresFormulaires()   ' NOUVEAU
        Clients.Owner = Me
        Clients.Show()
    End Sub
    Private Sub ChantiersToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles ChantiersToolStripMenuItem.Click
        FermerAutresFormulaires()   ' NOUVEAU
        Chantiers.Owner = Me
        Chantiers.Show()
    End Sub
    Private Sub VoituresToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles VoituresToolStripMenuItem.Click
        FermerAutresFormulaires()   ' NOUVEAU
        camions.Owner = Me
        camions.Show()
    End Sub
    Private Sub ChauffeursToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles ChauffeursToolStripMenuItem.Click
        FermerAutresFormulaires()   ' NOUVEAU
        Chauffeurs.Owner = Me
        Chauffeurs.Show()
    End Sub
    Private Sub TransporteursToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles TransporteursToolStripMenuItem.Click
        FermerAutresFormulaires()   ' NOUVEAU
        Transporteurs.Owner = Me
        Transporteurs.Show()
    End Sub
    Private Sub ZonesToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles ZonesToolStripMenuItem.Click
        FermerAutresFormulaires()   ' NOUVEAU
        Zones.Owner = Me
        Zones.Show()
    End Sub
    Private Sub FournisseursToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles FournisseursToolStripMenuItem.Click
        FermerAutresFormulaires()   ' NOUVEAU
        Fournisseurs.Owner = Me
        Fournisseurs.Show()
    End Sub
    Private Sub FormulesToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles FormulesToolStripMenuItem.Click
        FermerAutresFormulaires()   ' NOUVEAU
        Formules.Owner = Me
        Formules.Show()
    End Sub
    Private Sub ProduitsToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles ProduitsToolStripMenuItem.Click
        FermerAutresFormulaires()   ' NOUVEAU
        Produits.Owner = Me
        Produits.Show()
    End Sub
    Private Sub StocksToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles StocksToolStripMenuItem.Click
        FermerAutresFormulaires()   ' NOUVEAU
        Stocks.Owner = Me
        Stocks.Show()
    End Sub
    Private Sub MatériauxToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles MatériauxToolStripMenuItem.Click
        FermerAutresFormulaires()   ' NOUVEAU
        Matériaux.Owner = Me
        Matériaux.Show()
    End Sub
    Private Sub TaxesToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles TaxesToolStripMenuItem.Click
        FermerAutresFormulaires()   ' NOUVEAU
        taxes.Owner = Me
        taxes.Show()
    End Sub
    Private Sub TVAToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles TVAToolStripMenuItem.Click
        FermerAutresFormulaires()   ' NOUVEAU
        TVA.Owner = Me
        TVA.Show()
    End Sub
    Private Sub PostesToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles PostesToolStripMenuItem.Click
        FermerAutresFormulaires()   ' NOUVEAU
        Postes.Owner = Me
        Postes.Show()
    End Sub
    Private Sub TablesToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles TablesToolStripMenuItem.Click
        FermerAutresFormulaires()   ' NOUVEAU
        Tables.Owner = Me
        Tables.Show()
    End Sub
    Private Sub NormesToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles NormesToolStripMenuItem.Click
        FermerAutresFormulaires()   ' NOUVEAU
        Normes.Owner = Me
        Normes.Show()
    End Sub
    Private Sub PistesToolStripMenuItem_Click(sender As Object, e As EventArgs)
        FermerAutresFormulaires   ' NOUVEAU
        Pistes.Owner = Me
        Pistes.Show
    End Sub
    Private Sub PiècesToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles PiècesToolStripMenuItem.Click
        FermerAutresFormulaires()   ' NOUVEAU
        Pièces.Owner = Me
        Pièces.Show()
    End Sub
    Private Sub DosageManuelToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles DosageManuelToolStripMenuItem.Click
        FermerAutresFormulaires()   ' NOUVEAU
        Dosage_manuel.Owner = Me
        Dosage_manuel.Show()
    End Sub
    Private Sub QuitterToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles QuitterToolStripMenuItem.Click
        If MessageBox.Show("Voulez-vous quitter ?", "Quitter",
            MessageBoxButtons.YesNo, MessageBoxIcon.Question) = DialogResult.Yes Then
            Application.Exit()
        End If
    End Sub

    ' ── MENU LIVRAISONS ──────────────────────────────────────────────
    Private Sub NouvelleProductionToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles NouvelleProductionToolStripMenuItem.Click
        FermerAutresFormulaires()   ' NOUVEAU
        Demandes_de_Dosage_de_Béton.Owner = Me
        Demandes_de_Dosage_de_Béton.Show()
    End Sub
    Private Sub HistoriqueProductionToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles HistoriqueProductionToolStripMenuItem.Click
        FermerAutresFormulaires()   ' NOUVEAU
        Bons_de_livraison.Owner = Me
        Bons_de_livraison.Show()
    End Sub
    Private Sub BonsDePompageToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles BonsDePompageToolStripMenuItem.Click
        FermerAutresFormulaires()   ' NOUVEAU
        Bons_de_Pompage.Owner = Me
        Bons_de_Pompage.Show()
    End Sub
    Private Sub FacturesToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles FacturesToolStripMenuItem.Click
        FermerAutresFormulaires()   ' NOUVEAU
        Factures.Owner = Me
        Factures.Show()
    End Sub
    Private Sub DevisToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles DevisToolStripMenuItem.Click
        FermerAutresFormulaires()   ' NOUVEAU
        Devis.Owner = Me
        Devis.Show()
    End Sub
    Private Sub ListeDeFacturesToolStripMenuItem1_Click(sender As Object, e As EventArgs) Handles ListeDeFacturesToolStripMenuItem1.Click
        FermerAutresFormulaires()   ' NOUVEAU
        ListeDeFactures.Owner = Me
        ListeDeFactures.Show()
    End Sub
    Private Sub ListeDeFacturesToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles ListeDeFacturesToolStripMenuItem.Click
        FermerAutresFormulaires()   ' NOUVEAU
        ListedeBL.Owner = Me
        ListedeBL.Show()
    End Sub
    Private Sub ListeDeFacturesToolStripMenuItem2_Click(sender As Object, e As EventArgs) Handles ListeDeFacturesToolStripMenuItem2.Click
        FermerAutresFormulaires()   ' NOUVEAU
        Liste_de_Devis.Owner = Me
        Liste_de_Devis.Show()
    End Sub

    ' ── MENU PLANIFICATION ───────────────────────────────────────────
    ' ⚠️ PlanificationToolStripMenuItem1 = "Planification" dans le sous-menu
    Private Sub PlanificationToolStripMenuItem1_Click(sender As Object, e As EventArgs) Handles PlanificationToolStripMenuItem1.Click
        FermerAutresFormulaires()   ' NOUVEAU

        Planification_Demands.Owner = Me
        Planification_Demands.Show()
    End Sub
    Private Sub ProductionToolStripMenuItem1_Click(sender As Object, e As EventArgs) Handles ProductionToolStripMenuItem1.Click
        FermerAutresFormulaires()   ' NOUVEAU
        Planification_Production.Owner = Me
        Planification_Production.Show()
    End Sub
    Private Sub PlanningJournalierToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles PlanningJournalierToolStripMenuItem.Click
        FermerAutresFormulaires()   ' NOUVEAU
        Planning_journalier.Owner = Me
        Planning_journalier.Show()
    End Sub
    Private Sub PlanningCamionsToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles PlanningCamionsToolStripMenuItem.Click
        FermerAutresFormulaires()   ' NOUVEAU
        Planning_camions.Owner = Me
        Planning_camions.Show()
    End Sub

    ' ── MENU BASE DE DONNÉES ─────────────────────────────────────────
    Private Sub ArticlesToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles ArticlesToolStripMenuItem.Click
        FermerAutresFormulaires()   ' NOUVEAU
        Articles.Owner = Me
        Articles.Show()
    End Sub
    Private Sub CommentairesToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles CommentairesToolStripMenuItem.Click
        FermerAutresFormulaires()   ' NOUVEAU
        Commentaires.Owner = Me
        Commentaires.Show()
    End Sub
    Private Sub RéférencesCommercialesToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles RéférencesCommercialesToolStripMenuItem.Click
        FermerAutresFormulaires()   ' NOUVEAU
        Références_commerciales.Owner = Me
        Références_commerciales.Show()
    End Sub
    Private Sub ÉlémentsÀBétonnerToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles ÉlémentsÀBétonnerToolStripMenuItem.Click
        FermerAutresFormulaires()   ' NOUVEAU
        Éléments_à_bétonner.Owner = Me
        Éléments_à_bétonner.Show()
    End Sub
    Private Sub EntréesDeMatièresPremièresToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles EntréesDeMatièresPremièresToolStripMenuItem.Click
        FermerAutresFormulaires()   ' NOUVEAU
        Entrées_de_matières_premières.Owner = Me
        Entrées_de_matières_premières.Show()
    End Sub
    Private Sub SortiesDeMatièresPremièresToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles SortiesDeMatièresPremièresToolStripMenuItem.Click
        FermerAutresFormulaires()   ' NOUVEAU
        Sorties_de_matières_premières.Owner = Me
        Sorties_de_matières_premières.Show()
    End Sub
    Private Sub AjustementDeStockToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles AjustementDeStockToolStripMenuItem.Click
        FermerAutresFormulaires()   ' NOUVEAU
        Ajustement_de_stock.Owner = Me
        Ajustement_de_stock.Show()
    End Sub

    ' ── MENU STATISTIQUES ────────────────────────────────────────────
    ' ⚠️ Noms exacts avec suffixe "1" dans le Designer
    Private Sub StatistiquesProductionsToolStripMenuItem1_Click(sender As Object, e As EventArgs) Handles StatistiquesProductionsToolStripMenuItem1.Click
        FermerAutresFormulaires()   ' NOUVEAU
        Statistiques___Productions.Owner = Me
        Statistiques___Productions.Show()
    End Sub
    Private Sub StatistiquesGraphiquesToolStripMenuItem1_Click(sender As Object, e As EventArgs) Handles StatistiquesGraphiquesToolStripMenuItem1.Click
        FermerAutresFormulaires()   ' NOUVEAU
        Statistiques___Graphiques.Owner = Me
        Statistiques___Graphiques.Show()
    End Sub
    Private Sub StatistiquesLaboratoireToolStripMenuItem1_Click(sender As Object, e As EventArgs) Handles StatistiquesLaboratoireToolStripMenuItem1.Click
        FermerAutresFormulaires()   ' NOUVEAU
        Statistiques___Laboratoire.Owner = Me
        Statistiques___Laboratoire.Show()
    End Sub
    Private Sub StatistiquesCartesToolStripMenuItem1_Click(sender As Object, e As EventArgs) Handles StatistiquesCartesToolStripMenuItem1.Click
        FermerAutresFormulaires()   ' NOUVEAU
        Statistiques___Cartes.Owner = Me
        Statistiques___Cartes.Show()
    End Sub
    Private Sub StatistiquesCourbeDuMalaxeurToolStripMenuItem1_Click(sender As Object, e As EventArgs) Handles StatistiquesCourbeDuMalaxeurToolStripMenuItem1.Click
        Statistiques___Courbe_du_malaxeur.Owner = Me
        Statistiques___Courbe_du_malaxeur.Show()
    End Sub
    Private Sub StatistiquesExportationToolStripMenuItem1_Click(sender As Object, e As EventArgs) Handles StatistiquesExportationToolStripMenuItem1.Click
        FermerAutresFormulaires()   ' NOUVEAU
        Statistiques___Exportation.Owner = Me
        Statistiques___Exportation.Show()
    End Sub

    ' ── MENU CONFIGURATION ───────────────────────────────────────────
    Private Sub ConfigurationGénéraleToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles ConfigurationGénéraleToolStripMenuItem.Click
        FermerAutresFormulaires()   ' NOUVEAU
        Configuration_générale.Owner = Me
        Configuration_générale.Show()
    End Sub
    Private Sub ConfigurationCentraleToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles ConfigurationCentraleToolStripMenuItem.Click
        FermerAutresFormulaires()   ' NOUVEAU
        Configuration_centrale.Owner = Me
        Configuration_centrale.Show()
    End Sub
    Private Sub ConfigurationSilosToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles ConfigurationSilosToolStripMenuItem.Click
        Configuration_silos.Owner = Me
        Configuration_silos.Show()
    End Sub
    Private Sub AutorisationsUtilisateursToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles AutorisationsUtilisateursToolStripMenuItem.Click
        Autorisations_utilisateurs.Owner = Me
        Autorisations_utilisateurs.Show()
    End Sub
    Private Sub AutorisationsGroupesToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles AutorisationsGroupesToolStripMenuItem.Click
        FermerAutresFormulaires()   ' NOUVEAU
        Autorisations_groupes.Owner = Me
        Autorisations_groupes.Show()
    End Sub
    Private Sub AutorisationsTypeMachineToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles AutorisationsTypeMachineToolStripMenuItem.Click
        FermerAutresFormulaires()   ' NOUVEAU
        Autorisations_type_machine.Owner = Me
        Autorisations_type_machine.Show()
    End Sub
    Private Sub RapportsToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles RapportsToolStripMenuItem.Click
        FermerAutresFormulaires()   ' NOUVEAU
        Rapports.Owner = Me
        Rapports.Show()
    End Sub
    Private Sub RaccourcisToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles RaccourcisToolStripMenuItem.Click
        FermerAutresFormulaires()   ' NOUVEAU
        Raccourcis.Owner = Me
        Raccourcis.Show()
    End Sub
    Private Sub RaisonsToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles RaisonsToolStripMenuItem.Click
        FermerAutresFormulaires()   ' NOUVEAU
        Raisons.Owner = Me
        Raisons.Show()
    End Sub
    Private Sub MaintenanceOpérationsToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles MaintenanceOpérationsToolStripMenuItem.Click
        FermerAutresFormulaires()   ' NOUVEAU
        Maintenance___opérations.Owner = Me
        Maintenance___opérations.Show()
    End Sub
    Private Sub MaintenancePréventiveDesMachinesToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles MaintenancePréventiveDesMachinesToolStripMenuItem.Click
        FermerAutresFormulaires()   ' NOUVEAU
        Maintenance_préventive_des_machines.Owner = Me
        Maintenance_préventive_des_machines.Show()
    End Sub

    ' ── MENU UTILITAIRES ─────────────────────────────────────────────
    Private Sub SauvegardeToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles SauvegardeToolStripMenuItem.Click
        FermerAutresFormulaires()   ' NOUVEAU
        Sauvegarde.Owner = Me
        Sauvegarde.Show()
    End Sub
    Private Sub AlarmesMachineToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles AlarmesMachineToolStripMenuItem.Click
        FermerAutresFormulaires()   ' NOUVEAU
        Alarmes_machine.Owner = Me
        Alarmes_machine.Show()
    End Sub
    Private Sub AssistancePLCToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles AssistancePLCToolStripMenuItem.Click
        FermerAutresFormulaires()   ' NOUVEAU
        Assistance___PLC.Owner = Me
        Assistance___PLC.Show()
    End Sub
    Private Sub AssistanceRegistreDesOccurrencesToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles AssistanceRegistreDesOccurrencesToolStripMenuItem.Click
        FermerAutresFormulaires()   ' NOUVEAU
        Assistance___registre_des_occurrences.Owner = Me
        Assistance___registre_des_occurrences.Show()
    End Sub
    Private Sub AssistanceConfigurationEntréessortiesToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles AssistanceConfigurationEntréessortiesToolStripMenuItem.Click
        FermerAutresFormulaires()   ' NOUVEAU
        Assistance___configuration_entrées_sorties.Owner = Me
        Assistance___configuration_entrées_sorties.Show()
    End Sub

    ' ── MENU DELTABETON ──────────────────────────────────────────────
    Private Sub DELTABETONToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles DELTABETONToolStripMenuItem.Click
        FermerAutresFormulaires()   ' NOUVEAU
        DELTABETON.Owner = Me
        DELTABETON.Show()
    End Sub

    ' ── ROUTEUR — DÉCONNEXION ────────────────────────────────────────
    Private Sub ROUTEURToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles ROUTEURToolStripMenuItem.Click
        RoleUtilisateur = ""
        LoginForm.utilisateur.SelectedIndex = -1   ' ✅ corrigé
        LoginForm.motdepasse.Clear()
        LoginForm.Show()
        Me.Close()
    End Sub

    Private Sub JToolStripMenuItem1_Click(sender As Object, e As EventArgs) Handles JToolStripMenuItem1.Click
        FermerAutresFormulaires()   ' NOUVEAU
        Configuration_Automate.Owner = Me
        Configuration_Automate.Show()
    End Sub

    Private Sub ConfigurationAutomateToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles ConfigurationAutomateToolStripMenuItem.Click
        FermerAutresFormulaires()   ' NOUVEAU
        Configuration_Automate.Owner = Me
        Configuration_Automate.Show()
    End Sub



    Private Sub PlanningFchierToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles PlanningFchierToolStripMenuItem.Click
        FermerAutresFormulaires()   ' NOUVEAU
        Fiche_planning.Owner = Me
        Fiche_planning.Show()
    End Sub

    Private Sub BonsDePompageToolStripMenuItem_Click_1(sender As Object, e As EventArgs) Handles BonsDePompageToolStripMenuItem.Click
        FermerAutresFormulaires()   ' NOUVEAU
        Bons_de_Pompage.Owner = Me
        Bons_de_Pompage.Show()
    End Sub

    ' ── Référence directe au Synoptique ouvert ──
    Private _synoptique As Synoptique = Nothing

    Public Sub OuvrirFormDansPanel(frm As Form)
        For Each ctrl As Control In PanelMai.Controls
            If TypeOf ctrl Is Form Then CType(ctrl, Form).Close()
        Next
        PanelMai.Controls.Clear()
        frm.TopLevel = False
        frm.FormBorderStyle = FormBorderStyle.None
        frm.Dock = DockStyle.Fill
        PanelMai.Controls.Add(frm)
        frm.Show()
    End Sub

    ' ── Historique des gâchées (SQL) ─────────────────────────────────
    Private _histGachees As Historique_Gachees = Nothing
    Private Sub HistoriqueGachees_Click(sender As Object, e As EventArgs)
        FermerAutresFormulaires()
        If _histGachees Is Nothing OrElse _histGachees.IsDisposed Then _histGachees = New Historique_Gachees()
        _histGachees.Owner = Me
        _histGachees.Show()
        _histGachees.BringToFront()
    End Sub

    Private Sub PanelMain_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        ' ── 16/09 : menu "Historique gâchées" (table SQL GacheesDetails) ──
        Try
            Dim itHist As New ToolStripMenuItem("Historique gâchées")
            itHist.Name = "HistoriqueGacheesToolStripMenuItem"
            AddHandler itHist.Click, AddressOf HistoriqueGachees_Click
            Dim idx As Integer = ProductionToolStripMenuItem.DropDownItems.IndexOf(HistoriqueProductionToolStripMenuItem)
            ProductionToolStripMenuItem.DropDownItems.Insert(idx + 1, itHist)
        Catch
        End Try

        ' Rôle utilisateur
        Select Case RoleUtilisateur
            Case "Admin" : DELTABETONToolStripMenuItem.Visible = True

            Case Else : DELTABETONToolStripMenuItem.Visible = False
        End Select
        PlanningFchierToolStripMenuItem.Visible = False
        PlanificationToolStripMenuItem1.Visible = False
        RapportsToolStripMenuItem.Visible = False
        PostesToolStripMenuItem.Visible = False
        TaxesToolStripMenuItem.Visible = False
        TVAToolStripMenuItem.Visible = False
        PostesToolStripMenuItem.Visible = False
        DELTABETONToolStripMenuItem.Visible = False
        JToolStripMenuItem1.Visible = False
        SauvegardeToolStripMenuItem.Visible = False

        AssistancePLCToolStripMenuItem.Visible = False
        AssistanceConfigurationEntréessortiesToolStripMenuItem.Visible = False
        AssistanceRegistreDesOccurrencesToolStripMenuItem.Visible = True

        AutorisationsUtilisateursToolStripMenuItem.Visible = False
        AutorisationsGroupesToolStripMenuItem.Visible = False
        AutorisationsTypeMachineToolStripMenuItem.Visible = False
        AutorisationsTypeMachineToolStripMenuItem.Visible = False
        RaccourcisToolStripMenuItem.Visible = False
        RaisonsToolStripMenuItem.Visible = False
        MaintenanceOpérationsToolStripMenuItem.Visible = False
        MaintenancePréventiveDesMachinesToolStripMenuItem.Visible = False
        ConfigurationAutomateToolStripMenuItem.Visible = False
        AssistanceRegistreDesOccurrencesToolStripMenuItem1.Visible = False
        AjustementDeStockToolStripMenuItem.Visible = False
        ÉlémentsÀBétonnerToolStripMenuItem.Visible = False
        CommentairesToolStripMenuItem.Visible = False
        RéférencesCommercialesToolStripMenuItem.Visible = False
        ArticlesToolStripMenuItem.Visible = False
        PlanificationToolStripMenuItem1.Visible = False
        FacturesToolStripMenuItem.Visible = False
        HistoriqueProductionToolStripMenuItem.Visible = False
        DevisToolStripMenuItem.Visible = False
        ListeDeFacturesToolStripMenuItem1.Visible = False
        ListeDeFacturesToolStripMenuItem2.Visible = False
        BonsDePompageToolStripMenuItem.Visible = False
        TaxesToolStripMenuItem.Visible = False
        TVAToolStripMenuItem.Visible = False
        PostesToolStripMenuItem.Visible = False
        TablesToolStripMenuItem.Visible = False
        NormesToolStripMenuItem.Visible = False
        PiècesToolStripMenuItem.Visible = False






        ' Ouvrir Synoptique et garder référence
        _synoptique = New Synoptique
        OuvrirFormDansPanel(_synoptique)

        Timer1.Start()
    End Sub

    Private Sub Timer1_Tick(sender As Object, e As EventArgs) Handles Timer1.Tick
        ' ── Heure ──
        Label1.Text = Format(Now, "dd-MM-yyyy   HH:mm:ss")

        ' ── Etat PLC — utiliser Module1 directement ──
        Dim plcConnecte As Boolean = (Module1.PLCInstance IsNot Nothing AndAlso
                                       Module1.PLCInstance.IsConnected)

        ' ── Image selon état ──
        Dim dossier As String = Application.StartupPath & "\images\Synoptique\"
        If plcConnecte Then
            If picEtatPLC.Tag?.ToString() <> "vert" Then
                Try
                    picEtatPLC.Image = Image.FromFile(dossier & "reseau_vert.png")
                    picEtatPLC.Tag = "vert"
                Catch
                    ' Image non trouvée → cercle vert dessiné
                    picEtatPLC.Image = DessinerCercle(Color.LimeGreen)
                    picEtatPLC.Tag = "vert"
                End Try
            End If
        Else
            ' Clignotement rouge
            Static blink As Boolean = False
            blink = Not blink
            Try
                picEtatPLC.Image = Image.FromFile(dossier & "reseau_rouge.png")
                picEtatPLC.Tag = "rouge"
            Catch
                ' Image non trouvée → cercle rouge dessiné
                picEtatPLC.Image = DessinerCercle(
                    If(blink, Color.Red, Color.FromArgb(150, 0, 0)))
                picEtatPLC.Tag = ""
            End Try
        End If
    End Sub

    ' ── Dessiner cercle si image non trouvée ──
    Private Function DessinerCercle(couleur As Color) As Image
        Dim bmp As New Bitmap(40, 40)
        Using g As Graphics = Graphics.FromImage(bmp)
            g.SmoothingMode = Drawing2D.SmoothingMode.AntiAlias
            g.Clear(Color.Transparent)
            Using b As New SolidBrush(Color.FromArgb(60, 0, 0, 0))
                g.FillEllipse(b, 4, 6, 32, 32)
            End Using
            Using b As New SolidBrush(couleur)
                g.FillEllipse(b, 2, 2, 34, 34)
            End Using
            Using p As New Pen(Color.White, 1.5F)
                g.DrawEllipse(p, 2, 2, 34, 34)
            End Using
            Using b As New SolidBrush(Color.FromArgb(80, 255, 255, 255))
                g.FillEllipse(b, 8, 5, 14, 9)
            End Using
        End Using
        Return bmp
    End Function

    Private Sub PanelMain_FormClosing(sender As Object, e As FormClosingEventArgs) Handles Me.FormClosing
        ' Fermer proprement le PLC via Module1
        DeconnecterPLCGlobal()
    End Sub

    Private Sub BaseDeDonneesToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles BaseDeDonneesToolStripMenuItem.Click

    End Sub

    Private Sub MenuPrincipalToolStripMenuItem1_Click(sender As Object, e As EventArgs) Handles MenuPrincipalToolStripMenuItem1.Click

    End Sub

    Private Sub PanelMai_Paint(sender As Object, e As PaintEventArgs) Handles PanelMai.Paint

    End Sub

    Private Sub ConfigurationToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles ConfigurationToolStripMenuItem.Click

    End Sub

    Private Sub UtilitairesToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles UtilitairesToolStripMenuItem.Click

    End Sub

    Private Sub AssistanceRegistreDesOccurrencesToolStripMenuItem1_Click(sender As Object, e As EventArgs) Handles AssistanceRegistreDesOccurrencesToolStripMenuItem1.Click
        FermerAutresFormulaires()   ' NOUVEAU
        Assistance___configuration_entrées_sorties.Owner = Me
        Assistance___configuration_entrées_sorties.Show()
    End Sub

    Private Sub AlarmesMachineToolStripMenuItem1_Click(sender As Object, e As EventArgs) Handles AlarmesMachineToolStripMenuItem1.Click
        FermerAutresFormulaires()   ' NOUVEAU
        Alarmes_machine.Owner = Me
        Alarmes_machine.Show()
    End Sub

    Private Sub MenuStrip1_ItemClicked(sender As Object, e As ToolStripItemClickedEventArgs) Handles MenuStrip1.ItemClicked

    End Sub
End Class
