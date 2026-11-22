' ================================================================
'  ModuleStock.vb — Le module STOCKS travaille depuis MATERIAUX
'  ----------------------------------------------------------------
'  PROBLÈME :
'   • Les formulaires Stocks / Entrées MP / Sorties MP listent
'     la table T_STOCK. Un matériau créé dans le formulaire
'     Matériaux n'y apparaît JAMAIS (aucun écran ne crée sa
'     fiche stock).
'   • Les entrées/sorties modifient T_STOCK mais pas
'     Materiaux.Stock → les deux stocks divergent.
'
'  SOLUTION (ce module) :
'   1. SynchroniserFichesStock() : à l'ouverture des formulaires,
'      crée automatiquement la fiche T_STOCK de chaque matériau
'      géré en stock. → la liste affiche TOUJOURS tous les
'      matériaux de la table Materiaux.
'   2. MettreAJourStockMateriau() : à chaque entrée/sortie,
'      répercute le mouvement sur Materiaux.Stock. → les deux
'      stocks restent identiques.
'
' ═══════════════════════════════════════════════════════════════
'  INSTALLATION — 1 fichier + 5 copier-coller
' ═══════════════════════════════════════════════════════════════
'
'  ÉTAPE 0 : Ajouter CE fichier au projet
'    (clic droit projet → Ajouter → Élément existant → ModuleStock.vb)
'
'  ── Formulaire STOCKS ─────────────────────────────────────────
'  ÉTAPE 1 : dans Stocks, méthode Load (ou équivalent), AVANT
'  l'appel à ChargerStocks(), coller :
'
'        ModuleStock.SynchroniserFichesStock()
'
'  ── Formulaire ENTRÉES de matières premières ──────────────────
'  ÉTAPE 2 : dans Entrées_Load, AVANT ChargerStocks(), coller :
'
'        ModuleStock.SynchroniserFichesStock()
'
'  ÉTAPE 3 : dans btnEnregistrer_Click, juste APRÈS le
'  ModuleDb.Executer("UPDATE dbo.T_STOCK SET ... End Sub)
'  et AVANT le MessageBox.Show("✅ Entrée..."), coller :
'
'        ' Répercuter l'entrée sur Materiaux.Stock
'        ModuleStock.MettreAJourStockMateriau(txtCodeMat.Text, qte)
'
'  ── Formulaire SORTIES de matières premières ──────────────────
'  ÉTAPE 4 : dans le Load, AVANT ChargerStocks(), coller :
'
'        ModuleStock.SynchroniserFichesStock()
'
'  ÉTAPE 5 : dans btnEnregistrer_Click, juste APRÈS le
'  ModuleDb.Executer("UPDATE dbo.T_STOCK SET ... End Sub)
'  et AVANT le MessageBox.Show("✅ Sortie..."), coller :
'
'        ' Répercuter la sortie sur Materiaux.Stock (négatif)
'        ModuleStock.MettreAJourStockMateriau(txtCodeMat.Text, -qte)
'
'  Compiler (F6) → 0 erreur → tester : créez un matériau dans
'  Matériaux, ouvrez Stocks → il apparaît immédiatement.
' ================================================================
Imports System.Data

Public Module ModuleStock

    ' ═══════════════════════════════════════════════════════════════
    '  Crée la fiche T_STOCK de chaque matériau qui n'en a pas
    '  (GestionStocks = 1). Idempotent, très rapide, appelé à
    '  chaque ouverture des formulaires de stock.
    '  MAT_Type = code automate : GRANULAT=0, LIANT=1, EAU=2,
    '  ADJUVANT=3, ADDITION=4
    ' ═══════════════════════════════════════════════════════════════
    Public Sub SynchroniserFichesStock()
        Try
            ModuleDb.Executer(
                "INSERT INTO dbo.T_STOCK " &
                " (STO_Date, MAT_Code, POF_Code, MAT_Nom, MAT_Type, " &
                "  STO_Stock, STO_Stock_Initial, STO_Somme_Entrees, " &
                "  STO_Somme_Sorties, STO_Unite, STO_DateModification, " &
                "  STO_Somme_Corrections) " &
                "SELECT GETDATE(), LEFT(m.CodeMateriau,15), '', " &
                "  LEFT(m.NomMateriau,50), " &
                "  CASE UPPER(m.TypeMateriau) " &
                "    WHEN 'GRANULAT' THEN 0 WHEN 'LIANT' THEN 1 " &
                "    WHEN 'EAU' THEN 2 WHEN 'ADJUVANT' THEN 3 " &
                "    WHEN 'ADDITION' THEN 4 ELSE 0 END, " &
                "  ISNULL(m.Stock,0), ISNULL(m.Stock,0), 0, 0, " &
                "  LEFT(ISNULL(m.UniteStockage,'kg'),3), GETDATE(), 0 " &
                "FROM dbo.Materiaux m " &
                "WHERE ISNULL(m.GestionStocks,1) = 1 " &
                "  AND NOT EXISTS (SELECT 1 FROM dbo.T_STOCK s " &
                "                  WHERE s.MAT_Code = LEFT(m.CodeMateriau,15))")

            ' Rafraîchir le nom si le matériau a été renommé
            ModuleDb.Executer(
                "UPDATE s SET s.MAT_Nom = LEFT(m.NomMateriau,50) " &
                "FROM dbo.T_STOCK s " &
                "JOIN dbo.Materiaux m ON s.MAT_Code = LEFT(m.CodeMateriau,15) " &
                "WHERE s.MAT_Nom <> LEFT(m.NomMateriau,50)")

        Catch ex As Exception
            Debug.WriteLine("SynchroniserFichesStock : " & ex.Message)
        End Try
    End Sub

    ' ═══════════════════════════════════════════════════════════════
    '  Répercute un mouvement manuel (entrée = +, sortie = −)
    '  sur Materiaux.Stock, pour que les deux stocks restent
    '  identiques. Le mouvement est déjà en unité de STOCKAGE
    '  (celle affichée dans le formulaire), donc pas de conversion.
    ' ═══════════════════════════════════════════════════════════════
    Public Sub MettreAJourStockMateriau(codeMateriau As String, delta As Decimal)
        Try
            ModuleDb.Executer(
                "UPDATE dbo.Materiaux SET " &
                "  Stock = ISNULL(Stock,0) + @d, " &
                "  DerniereModification = GETDATE() " &
                "WHERE LEFT(CodeMateriau,15) = @c",
                Sub(cmd)
                    cmd.Parameters.AddWithValue("@d", delta)
                    cmd.Parameters.AddWithValue("@c", codeMateriau.Trim())
                End Sub)
        Catch ex As Exception
            Debug.WriteLine("MettreAJourStockMateriau : " & ex.Message)
        End Try
    End Sub

End Module