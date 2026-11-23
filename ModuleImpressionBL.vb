' ================================================================
'  ModuleImpressionBL.vb
'  Modele d'impression unique des Bons de Livraison.
'  Reproduit la mise en page en grille (format KMKA BETON).
'
'  Deux versions :
'     ModuleImpressionBL.ImprimerBL("27201")                  ' SANS detail
'     ModuleImpressionBL.ImprimerBL("27201", avecDetail:=True) ' AVEC detail dosage
'     ModuleImpressionBL.ImprimerBL("27201", apercu:=False)    ' impression directe
'
'  Lit dans dbo.BonsLivraison (+ dbo.ReferencesCommerciales pour
'  les classes, + dbo.GacheesDetails pour le detail dosage).
'  Si une colonne n'existe pas, la case reste vide (aucun plantage).
' ================================================================

Imports System.Data
Imports System.Drawing
Imports System.Drawing.Printing
Imports System.Drawing.Text
Imports System.IO
Imports System.Windows.Forms
Imports Microsoft.Data.SqlClient

Module ModuleImpressionBL

    ' ── Numero d'agrement imprime en haut a droite (a adapter) ──
    Private Const NUMERO_AGREMENT As String = "2023RCNM019-02-01"

    ' ── Texte legal du pied de page (norme marocaine) ───────────
    Private Const LEGAL1 As String = "* Béton à propriété spécifique (BPS) ou à composition prescrite (BCP) conformes à la norme NM10.1.008"
    Private Const LEGAL2 As String = "Il est strictement interdit à notre chauffeur de rajouter de l'eau ou d'autres ingrédients."
    Private Const LEGAL3 As String = "La durée de mise en œuvre du béton ne dépasse pas 120 min."

    ' ── Donnees courantes ───────────────────────────────────────
    Private _bl As DataRow = Nothing
    Private _ref As DataRow = Nothing
    Private _client As DataRow = Nothing
    Private _gachees As DataTable = Nothing
    Private _avecDetail As Boolean = False

    ' ── Geometrie de la grille (coord. en 1/100 pouce) ──────────
    Private ReadOnly XS() As Integer = {40, 190, 340, 490, 640}   ' x gauche des 5 colonnes
    Private Const COLW As Integer = 150
    Private Const XR As Integer = 790                              ' bord droit

    ' ═══════════════════════════════════════════════════════════
    '  POINT D'ENTREE
    ' ═══════════════════════════════════════════════════════════
    Public Sub ImprimerBL(numeroBL As String,
                          Optional avecDetail As Boolean = False,
                          Optional apercu As Boolean = True)
        Try
            If numeroBL Is Nothing OrElse numeroBL.Trim = "" Then
                MessageBox.Show("Numéro de BL vide.") : Return
            End If
            numeroBL = numeroBL.Trim()

            ' ── BL ───────────────────────────────────────────
            Dim dt As DataTable = ModuleDb.Lire(
                "SELECT TOP 1 * FROM dbo.BonsLivraison WHERE NumeroBL=@bl",
                Sub(cmd) cmd.Parameters.Add("@bl", SqlDbType.NVarChar, 50).Value = numeroBL)
            If dt.Rows.Count = 0 Then
                MessageBox.Show("BL introuvable : " & numeroBL) : Return
            End If
            _bl = dt.Rows(0)
            _avecDetail = avecDetail

            ' ── Client (adresse, ICE) pour l'en-tête ─────────
            _client = Nothing
            Try
                Dim codeC As String = Txt(_bl, "CodeClient")
                If codeC <> "" Then
                    Dim dtc As DataTable = ModuleDb.Lire(
                        "SELECT TOP 1 * FROM dbo.Clients WHERE CodeClient=@c",
                        Sub(cmd) cmd.Parameters.Add("@c", SqlDbType.NVarChar, 50).Value = codeC)
                    If dtc.Rows.Count > 0 Then _client = dtc.Rows(0)
                End If
            Catch
            End Try

            ' ── Reference commerciale (classes beton) ─────────
            _ref = Nothing
            Try
                Dim codeF As String = Txt(_bl, "CodeFormule")
                If codeF <> "" Then
                    Dim dtr As DataTable = ModuleDb.Lire(
                        "SELECT TOP 1 * FROM dbo.ReferencesCommerciales WHERE CodeReference=@c",
                        Sub(cmd) cmd.Parameters.Add("@c", SqlDbType.NVarChar, 50).Value = codeF)
                    If dtr.Rows.Count > 0 Then _ref = dtr.Rows(0)
                End If
            Catch
            End Try

            ' ── Detail gachees ───────────────────────────────
            _gachees = Nothing
            If avecDetail Then
                Try
                    _gachees = ModuleDb.Lire(
                        "SELECT * FROM dbo.GacheesDetails WHERE NumeroBL=@bl ORDER BY NumeroGachee, DateHeure",
                        Sub(cmd) cmd.Parameters.Add("@bl", SqlDbType.NVarChar, 50).Value = numeroBL)
                Catch
                End Try
            End If

            ' ── Document ─────────────────────────────────────
            Dim doc As New PrintDocument()
            doc.DocumentName = "BL " & numeroBL
            doc.DefaultPageSettings.Margins = New Margins(30, 30, 25, 25)
            AddHandler doc.PrintPage, AddressOf DessinerBL

            ' 19/09 : réglages « Configuration générale » : imprimante BL + nombre de copies
            Try
                ModuleConfigImpression.Charger()
                If ModuleConfigImpression.ImprimanteBL <> "" Then
                    doc.PrinterSettings.PrinterName = ModuleConfigImpression.ImprimanteBL
                    If Not doc.PrinterSettings.IsValid Then
                        Synoptique.EnregistrerAction("ALARME", "Imprimante BL introuvable",
                            ModuleConfigImpression.ImprimanteBL, "", "", "ALARME")
                        doc.PrinterSettings = New PrinterSettings()
                    End If
                End If
                doc.PrinterSettings.Copies = CShort(Math.Max(1, ModuleConfigImpression.NbreCopiesBL))
            Catch
            End Try

            If apercu Then
                Dim ppd As New PrintPreviewDialog() With {.Document = doc, .WindowState = FormWindowState.Maximized}
                ppd.ShowDialog()
            Else
                doc.Print()
            End If

        Catch ex As Exception
            MessageBox.Show("Erreur impression BL : " & ex.Message)
        End Try
    End Sub

    ' ═══════════════════════════════════════════════════════════
    '  DESSIN DE LA PAGE
    ' ═══════════════════════════════════════════════════════════
    Private Sub DessinerBL(sender As Object, e As PrintPageEventArgs)
        If _bl Is Nothing Then Return
        Dim g As Graphics = e.Graphics
        g.TextRenderingHint = TextRenderingHint.AntiAlias

        Dim p As New Pen(Color.Black, 0.6F)
        Dim fLib As New Font("Arial", 6.5F, FontStyle.Bold)     ' libelles des cases
        Dim fVal As New Font("Arial", 10.5F, FontStyle.Bold)    ' valeurs (lisibles)
        Dim fInfo As New Font("Arial", 9.0F)
        Dim fInfoB As New Font("Arial", 9.0F, FontStyle.Bold)
        Dim fTitre As New Font("Arial", 15.0F, FontStyle.Bold)
        Dim fSoc As New Font("Arial", 11.0F, FontStyle.Bold)
        Dim fSocP As New Font("Arial", 8.0F)
        Dim fLegal As New Font("Arial", 7.5F, FontStyle.Italic)

        ' ── EN-TETE ──────────────────────────────────────────
        Dim y As Integer = 30
        ' 19/09 (spéc. utilisateur) : GAUCHE = logo puis, en dessous, les données société ;
        '   CENTRE = titre ; DROITE = date, n° BL, puis client (nom, adresse, ICE) et chantier.
        Dim yG As Integer = y
        Dim yDonnees As Integer = y            ' niveau des coordonnées société ET du bloc client
        If ModuleConfigImpression.EnteteActive Then
            If CheminLogo() <> "" Then
                DessinerLogo(g, 40, y, 180, 80)
                yG = y + 86
                yDonnees = yG
            End If
            g.DrawString(SocieteNom, fSoc, Brushes.Black, 40, yG) : yG += 17
            If SocieteAdresse <> "" Then g.DrawString(SocieteAdresse, fSocP, Brushes.Black, 40, yG) : yG += 13
            If SocieteVille <> "" Then g.DrawString(SocieteVille, fSocP, Brushes.Black, 40, yG) : yG += 13
            g.DrawString("Tel : " & SocieteTelephone & If(SocieteFax <> "", "   Fax : " & SocieteFax, ""), fSocP, Brushes.Black, 40, yG) : yG += 13
            If SocieteICE <> "" Then g.DrawString("ICE : " & SocieteICE, fSocP, Brushes.Black, 40, yG) : yG += 13
            g.DrawString("N°: " & NUMERO_AGREMENT, fSocP, Brushes.Black, 40, yG) : yG += 13
            If ModuleConfigImpression.EnteteBL <> "" Then g.DrawString(ModuleConfigImpression.EnteteBL, fSocP, Brushes.Black, 40, yG) : yG += 13
        End If

        DessinerCentre(g, "BON DE LIVRAISON", fTitre, Brushes.Black, 300, y + 30, 220)
        If String.Equals(Txt(_bl, "Statut"), "LIVRE", StringComparison.OrdinalIgnoreCase) Then
            DessinerCentre(g, "DUPLICATA", New Font("Arial", 8, FontStyle.Italic), Brushes.Black, 300, y + 54, 220)
        End If

        ' Bloc droit : date, n° BL, client (nom, adresse, ICE), chantier
        Dim xC As Integer = 530
        Dim yC As Integer = yDonnees          ' même niveau que les coordonnées société
        g.DrawString("DATE : " & DateBL(), fInfo, Brushes.Black, xC, yC) : yC += 18
        g.DrawString("BON N° : " & Txt(_bl, "NumeroBL"), fInfoB, Brushes.Black, xC, yC) : yC += 22
        g.DrawString("CLIENT : " & Txt(_bl, "NomClient") & "   (" & Txt(_bl, "CodeClient") & ")", fInfoB, Brushes.Black, xC, yC) : yC += 16
        If _client IsNot Nothing Then
            Dim adrC As String = Txt(_client, "Adresse")
            Dim villeC As String = (Txt(_client, "CodePostal") & " " & Txt(_client, "Pays")).Trim()
            If adrC <> "" Then g.DrawString(adrC, fSocP, Brushes.Black, xC, yC) : yC += 13
            If villeC <> "" Then g.DrawString(villeC, fSocP, Brushes.Black, xC, yC) : yC += 13
            If Txt(_client, "ICE") <> "" Then g.DrawString("ICE : " & Txt(_client, "ICE"), fSocP, Brushes.Black, xC, yC) : yC += 13
        End If
        yC += 4
        g.DrawString("CHANTIER : " & Txt(_bl, "CodeChantier") & "  " & Txt(_bl, "NomChantier"), fInfoB, Brushes.Black, xC, yC) : yC += 18

        y = Math.Max(yG, yC) + 8
        g.DrawLine(p, 40, y, XR, y)
        y += 10

        ' ── LIGNE 5 CASES : quantites + code beton ────────────
        Dim hC As Integer = 50
        Boite(g, p, XS(0), y, COLW, hC, "QUANTITE LIVREE", Txt(_bl, "VolumeM3") & " M3", fLib, fVal)
        Boite(g, p, XS(1), y, COLW, hC, "N° DE COMMANDE", Txt(_bl, "NumeroCommande"), fLib, fVal)
        Boite(g, p, XS(2), y, COLW, hC, "QTE COMMANDEE", Txt(_bl, "QteCommandee"), fLib, fVal)
        Boite(g, p, XS(3), y, COLW, hC, "QTE CUMULEE", Txt(_bl, "QteCumulee"), fLib, fVal)
        Boite(g, p, XS(4), y, XR - XS(4), hC, "CODE BETON", Txt(_bl, "CodeFormule"), fLib, fVal)
        y += hC

        ' ── DESIGNATION ──────────────────────────────────────
        Dim hD As Integer = 24
        g.DrawRectangle(p, 40, y, XR - 40, hD)
        g.DrawString("DESIGNATION :", fLib, Brushes.Black, 46, y + 6)
        g.DrawString(Designation(), fInfoB, Brushes.Black, 160, y + 5)
        y += hD

        ' ── GRILLE A : classes ───────────────────────────────
        Dim hG As Integer = 62
        Boite(g, p, XS(0), y, COLW, hG, "CLASSE DE RESISTANCE", RefTxt("ClasseResistance"), fLib, fVal)
        Boite(g, p, XS(1), y, COLW, hG, "CLASSE D'EXPOSITION", RefTxt("ClasseExposition"), fLib, fVal)
        Boite(g, p, XS(2), y, COLW, hG, "CLASSE DE CONSISTANCE", RefTxt("ClasseConsistance"), fLib, fVal)
        Boite(g, p, XS(3), y, COLW, hG, "CLASSE DE TENEUR EN CHLORURE", RefTxt("TeneurChlorures"), fLib, fVal)
        Boite(g, p, XS(4), y, XR - XS(4), hG, "D max", RefTxt("DimensionMaximum"), fLib, fVal)
        y += hG

        ' ── GRILLE B : ciment / adjuvant / E-C / air / temp ──
        Boite(g, p, XS(0), y, COLW, hG, "TYPE ET CLASSE DE CIMENT", RefTxt("Ciment"), fLib, fVal)
        Boite(g, p, XS(1), y, COLW, hG, "TYPE D'ADJUVANT", RefTxt("Adjuvants"), fLib, fVal)
        Boite(g, p, XS(2), y, COLW, hG, "RAPPORT E/C", Txt(_bl, "RapportEC"), fLib, fVal)
        Boite(g, p, XS(3), y, COLW, hG, "TENEUR EN AIR", Txt(_bl, "TeneurAir"), fLib, fVal)
        Boite(g, p, XS(4), y, XR - XS(4), hG, "TEMPERATURE DU BETON", Txt(_bl, "Temperature"), fLib, fVal)
        y += hG

        ' ── LIGNE teneur ciment / masse volumique ────────────
        Dim hT As Integer = 24
        g.DrawRectangle(p, 40, y, 440, hT)
        g.DrawString("TENEUR EN CIMENT", fLib, Brushes.Black, 46, y + 7)
        g.DrawString(RefTxt("TeneurCiment"), fInfoB, Brushes.Black, 250, y + 6)
        g.DrawRectangle(p, 490, y, XR - 490, hT)
        g.DrawString("MASSE VOLUMIQUE", fLib, Brushes.Black, 496, y + 7)
        g.DrawString(RefTxt("MasseVolumique"), fInfoB, Brushes.Black, 660, y + 6)
        y += hT

        ' ── GRILLE C : transport / camion / chauffeur ────────
        Dim hCh As Integer = 68
        Boite(g, p, XS(0), y, COLW, hCh, "TRANSPORTEUR", Txt(_bl, "Transporteur"), fLib, fVal)
        Boite(g, p, XS(1), y, COLW, hCh, "MATRICULE DU CAMION", Txt(_bl, "CodeCamion"), fLib, fVal)
        Boite(g, p, XS(2), y, COLW, hCh, "NOM DU CHAUFFEUR", Txt(_bl, "NomChauffeur"), fLib, fVal)
        Boite(g, p, XS(3), y, COLW, hCh, "SERVICES", "", fLib, fVal)
        Boite(g, p, XS(4), y, XR - XS(4), hCh, "REFERENCE", Txt(_bl, "ReferenceClient"), fLib, fVal)
        y += hCh

        ' ── GRILLE D : heures ────────────────────────────────
        Dim hH As Integer = 54
        Boite(g, p, XS(0), y, COLW, hH, "H. 1ERE", Txt(_bl, "Heure1ere"), fLib, fVal)
        Boite(g, p, XS(1), y, COLW, hH, "H. ARRIVEE", Txt(_bl, "HeureArrivee"), fLib, fVal)
        Boite(g, p, XS(2), y, COLW, hH, "H. DEBUT", Txt(_bl, "HeureDebut"), fLib, fVal)
        Boite(g, p, XS(3), y, COLW, hH, "H. FIN", Txt(_bl, "HeureFin"), fLib, fVal)
        Boite(g, p, XS(4), y, XR - XS(4), hH, "RETOUR CENTRALE", Txt(_bl, "RetourCentrale"), fLib, fVal)
        y += hH

        ' ── NOTES / RECEPTION ────────────────────────────────
        Dim hN As Integer = 130
        g.DrawRectangle(p, 40, y, 440, hN)
        DessinerCentre(g, "NOTES :", fLib, Brushes.Black, 40, y + 6, 440)
        g.DrawString(Txt(_bl, "Remarques"), fInfo, Brushes.Black, 50, y + 26)
        g.DrawRectangle(p, 490, y, XR - 490, hN)
        DessinerCentre(g, "RECEPTION", fLib, Brushes.Black, 490, y + 6, XR - 490)
        DessinerCentre(g, "VISA & CACHET :", fLib, Brushes.Black, 490, y + 22, XR - 490)
        y += hN + 10

        ' ── PIED LEGAL ───────────────────────────────────────
        If ModuleConfigImpression.NormeActive Then
            g.DrawString(LEGAL1, fLegal, Brushes.Black, 45, y) : y += 14
            g.DrawString(LEGAL2, fLegal, Brushes.Black, 45, y) : y += 14
            g.DrawString(LEGAL3, fLegal, Brushes.Black, 45, y) : y += 18
        End If
        ' Commentaires BL (Configuration générale)
        If ModuleConfigImpression.CommentairesBL <> "" Then
            g.DrawString(ModuleConfigImpression.CommentairesBL, New Font("Arial", 8, FontStyle.Italic), Brushes.Black, 45, y) : y += 16
        End If

        ' ── DETAIL DOSAGE (optionnel) ────────────────────────
        If _avecDetail AndAlso _gachees IsNot Nothing AndAlso _gachees.Rows.Count > 0 Then
            g.DrawLine(p, 40, y, XR, y) : y += 6
            DessinerDetailDosage(g, y)
        End If
    End Sub

    ' ═══════════════════════════════════════════════════════════
    '  DETAIL DOSAGE — reproduit le listing reel (format RSAI)
    '  Colonnes : SD  SC  G1  G2  CPJ55  EAU  VISCOFL
    '  ⚠ MAPPING A CONFIRMER selon l'affectation de tes tremies/silos :
    '     SD=Tr1  SC=Tr2  G1=Tr3  G2=Tr4  CPJ55=S1(+S2)  EAU=EAU  VISCOFL=ADJ1
    '  Bloc gauche = poids matieres (kg). Bloc droit = Heure / AjEau / Qte (m3).
    ' ═══════════════════════════════════════════════════════════
    ' ── Noms des 10 éléments depuis la configuration (16/09) ─────
    '    0..3 TR1..TR4 : matière liée à la case (sinon nom / code de la case)
    '    4    Ciment : silo 1 actif → Materiaux.CodeMateriau (sinon NomSilo)
    '    5    EAU
    '    6..9 ADJ1..ADJ4 : dbo.Bascules type ADJUVANT → Materiaux.CodeMateriau (sinon NomBascule)
    Private Function NomsElements() As String()
        ' 26/09 : noms CONFIGURÉS (écran Configuration → dbo.Silos, table unique : TypeElement TR/S/EAU/ADJ,
        '         NumeroElement, NomSilo), comme sur le Synoptique ; repli = code matériau, puis TRn/CIM/EAU/ADJn.
        Dim noms() As String = {"TR1", "TR2", "TR3", "TR4", "CIM", "EAU", "ADJ1", "ADJ2", "ADJ3", "ADJ4"}
        Try
            Dim dt As DataTable = ModuleDb.Lire(
                "SELECT s.TypeElement, s.NumeroElement, " &
                "       COALESCE(NULLIF(LTRIM(RTRIM(s.NomSilo)),''), NULLIF(LTRIM(RTRIM(m.CodeMateriau)),''), '') AS Nom " &
                "FROM dbo.Silos s LEFT JOIN dbo.Materiaux m ON m.IdMateriau = s.IdMateriau " &
                "WHERE s.EstActif = 1 ORDER BY s.TypeElement, s.NumeroElement, s.SiloID")
            Dim kAdj As Integer = 0
            For Each r As DataRow In dt.Rows
                Dim typ As String = r("TypeElement").ToString().Trim().ToUpper()
                Dim num As Integer = If(IsDBNull(r("NumeroElement")), 0, CInt(r("NumeroElement")))
                Dim nom As String = r("Nom").ToString().Trim()
                If nom = "" Then Continue For
                Select Case typ
                    Case "TR" : If num >= 1 AndAlso num <= 4 Then noms(num - 1) = nom
                    Case "S" : If num = 1 OrElse noms(4) = "CIM" Then noms(4) = nom
                    Case "EAU" : noms(5) = nom
                    Case "ADJ"
                        kAdj += 1
                        Dim idx As Integer = If(num >= 1 AndAlso num <= 4, num, kAdj)
                        If idx >= 1 AndAlso idx <= 4 Then noms(5 + idx) = nom
                End Select
            Next
        Catch
        End Try
        For i As Integer = 0 To noms.Length - 1
            noms(i) = noms(i).ToUpper()
            If noms(i).Length > 7 Then noms(i) = noms(i).Substring(0, 7)
        Next
        Return noms
    End Function

    ' ── Formule (kg ou L par m3) pour les 10 colonnes ───────────
    '    Liaison par IdMateriau, EXACTEMENT comme l'envoi des consignes au CPU
    '    (Form_ConfirmationBL) : Cases 1..4, silo 'S' actif, bascule EAU, bascules ADJUVANT.
    '    Renvoie Nothing si la formule est introuvable.
    Private _codesFormule() As String = Nothing   ' codes matière de la formule, par colonne (rempli par FormuleParM3)

    Private Function FormuleParM3(noms() As String) As Double()
        Try
            _codesFormule = Nothing
            Dim codeF As String = Txt(_bl, "CodeFormule")
            If codeF = "" Then Return Nothing
            Dim dt As DataTable = ModuleDb.Lire(
                "SELECT ISNULL(fc.IdMateriau,0) AS IdMat, ISNULL(fc.Quantite,0) AS Qte, ISNULL(fc.Phase,0) AS Phase, " &
                "       ISNULL(UPPER(fc.TypeMateriau),'') AS Typ, " &
                "       COALESCE(NULLIF(LTRIM(RTRIM(m.CodeMateriau)),''), NULLIF(LTRIM(RTRIM(fc.CodeMateriau)),''), NULLIF(LTRIM(RTRIM(m.NomMateriau)),''), '') AS Code " &
                "FROM dbo.FormuleComposition fc JOIN dbo.Formules f ON f.IdFormule = fc.IdFormule " &
                "LEFT JOIN dbo.Materiaux m ON m.IdMateriau = fc.IdMateriau " &
                "WHERE f.CodeFormule = @c",
                Sub(cmd) cmd.Parameters.Add("@c", SqlDbType.NVarChar, 30).Value = codeF)
            If dt.Rows.Count = 0 Then Return Nothing

            ' Colonne de chaque matière d'après la configuration
            Dim colParMat As New Dictionary(Of Integer, Integer)
            Try
                For Each r As DataRow In ModuleDb.Lire("SELECT NumeroCase, IdMateriau FROM dbo.Cases WHERE IdMateriau IS NOT NULL AND NumeroCase BETWEEN 1 AND 4").Rows
                    colParMat(CInt(r("IdMateriau"))) = CInt(r("NumeroCase")) - 1
                Next
            Catch
            End Try
            Try
                For Each r As DataRow In ModuleDb.Lire("SELECT IdMateriau FROM dbo.Silos WHERE IdMateriau IS NOT NULL AND EstActif = 1 AND TypeElement = 'S'").Rows
                    If Not colParMat.ContainsKey(CInt(r("IdMateriau"))) Then colParMat(CInt(r("IdMateriau"))) = 4
                Next
            Catch
            End Try
            Try
                For Each r As DataRow In ModuleDb.Lire("SELECT b.IdMateriau FROM dbo.Bascules b JOIN dbo.TypesBascule t ON b.TypeID = t.TypeID WHERE t.NomType LIKE '%EAU%' AND b.IdMateriau IS NOT NULL").Rows
                    If Not colParMat.ContainsKey(CInt(r("IdMateriau"))) Then colParMat(CInt(r("IdMateriau"))) = 5
                Next
            Catch
            End Try
            Try
                For Each r As DataRow In ModuleDb.Lire("SELECT b.IdMateriau, ROW_NUMBER() OVER (ORDER BY b.Ordre, b.BasculeID) AS Rang FROM dbo.Bascules b JOIN dbo.TypesBascule t ON b.TypeID = t.TypeID WHERE t.NomType LIKE '%ADJUVANT%' AND b.IdMateriau IS NOT NULL").Rows
                    Dim rang As Integer = CInt(r("Rang"))
                    If rang >= 1 AndAlso rang <= 4 AndAlso Not colParMat.ContainsKey(CInt(r("IdMateriau"))) Then colParMat(CInt(r("IdMateriau"))) = 5 + rang
                Next
            Catch
            End Try

            Dim q(9) As Double
            Dim codes(9) As String
            Dim trouve As Boolean = False
            For Each r As DataRow In dt.Rows
                Dim idm As Integer = CInt(r("IdMat"))
                Dim qte As Double = CDbl(r("Qte"))
                Dim idx As Integer = -1
                If idm > 0 AndAlso colParMat.ContainsKey(idm) Then idx = colParMat(idm)
                ' Secours eau : phase 3 / type EAU
                If idx < 0 AndAlso (CInt(r("Phase")) = 3 OrElse r("Typ").ToString().Contains("EAU")) Then idx = 5
                If idx >= 0 Then
                    q(idx) += qte : trouve = True
                    Dim c As String = r("Code").ToString().Trim().ToUpper()
                    If c <> "" AndAlso String.IsNullOrEmpty(codes(idx)) Then codes(idx) = If(c.Length > 7, c.Substring(0, 7), c)
                End If
            Next
            If trouve Then _codesFormule = codes
            Return If(trouve, q, Nothing)
        Catch
            Return Nothing
        End Try
    End Function

    ' Volume malaxeur (m3) : planning du BL (VolumeRotationMaxi), sinon CPU (DB1.DBD916)
    Private Function VolumeMalaxeur() As Double
        Dim v As Double = 0
        Try
            Dim idP As Double = Num(_bl, "IdPlanning")
            If idP > 0 Then
                Dim o As Object = ModuleDb.LireValeur(Of Object)(
                    "SELECT TOP 1 ISNULL(VolumeRotationMaxi,0) FROM dbo.Planning_journalier WHERE IdPlanning=@id",
                    Sub(cmd) cmd.Parameters.Add("@id", SqlDbType.Int).Value = CInt(idP))
                If o IsNot Nothing AndAlso Not IsDBNull(o) Then v = CDbl(o)
            End If
        Catch
        End Try
        If v <= 0 Then v = PLCData.CapMalaxeur
        Return v
    End Function

    ' Temps de malaxage (s) de la formule du BL — dbo.Formules.TempsMalaxage
    Private Function TempsMalaxageFormule() As Double
        Try
            Dim codeF As String = Txt(_bl, "CodeFormule")
            If codeF = "" Then Return Num(_bl, "TempsMalaxageS")
            Dim v As Object = ModuleDb.LireValeur(Of Object)(
                "SELECT TOP 1 ISNULL(TempsMalaxage,0) FROM dbo.Formules WHERE CodeFormule=@c",
                Sub(cmd) cmd.Parameters.Add("@c", SqlDbType.NVarChar, 30).Value = codeF)
            If v Is Nothing OrElse IsDBNull(v) Then Return Num(_bl, "TempsMalaxageS")
            Return CDbl(v)
        Catch
            Return Num(_bl, "TempsMalaxageS")
        End Try
    End Function

    Private Sub DessinerDetailDosage(g As Graphics, yDep As Integer)
        Dim fM As New Font("Courier New", 7.0F)
        Dim fMB As New Font("Courier New", 7.0F, FontStyle.Bold)
        Dim x As Integer = 45      ' bloc gauche (matieres)
        Dim xD As Integer = 600    ' bloc droit (heures / qte)
        Dim y As Integer = yDep
        Const NC As Integer = 10

        Dim noms() As String = NomsElements()
        Dim champsPose() As String = {"PoseTr1", "PoseTr2", "PoseTr3", "PoseTr4", "PoseS1", "PoseEAU", "PoseADJ1", "PoseADJ2", "PoseADJ3", "PoseADJ4"}
        Dim champsCons() As String = {"ConsTr1", "ConsTr2", "ConsTr3", "ConsTr4", "ConsS1", "ConsEAU", "ConsADJ1", "ConsADJ2", "ConsADJ3", "ConsADJ4"}

        ' ── En-tete ──────────────────────────────────────────
        g.DrawString("Bon No " & Txt(_bl, "NumeroBL") & "    " & DateBL() &
                     " Formule " & Txt(_bl, "CodeFormule") &
                     "    Cubage : " & Txt(_bl, "VolumeM3") & " m3", fMB, Brushes.Black, x, y)
        y += 12
        g.DrawString("          " & Designation(), fM, Brushes.Black, x, y) : y += 11
        Dim tMalax As Double = TempsMalaxageFormule()
        Dim nbGach As Integer = If(_gachees IsNot Nothing, _gachees.Rows.Count, 0)
        g.DrawString("          Malaxage " & F(tMalax) & " s   x " & nbGach & " gachees = " & F(tMalax * nbGach) & " s", fM, Brushes.Black, x, y) : y += 16

        ' ── Ligne 1 : NOMS des matieres d'apres la FORMULE (sinon configuration) ──
        Dim c0 As DataRow = _gachees.Rows(0)
        Dim fq() As Double = FormuleParM3(noms)
        Dim nomsLigne(NC - 1) As String
        For i As Integer = 0 To NC - 1
            nomsLigne(i) = noms(i)      ' 26/09 : noms configurés (plus les codes de la formule)
        Next
        g.DrawString(LN(nomsLigne), fM, Brushes.Black, x, y)
        g.DrawString("Materiaux", fM, Brushes.Black, xD, y) : y += 11

        ' ── Ligne 2 : FORMULE kg/m3 telle que TRANSFEREE AU CPU
        '    lue dans DB1 : DBD538/542/546/550 (TR1-4), 530 (CIM), 586 (EAU), 594/598/602/606 (ADJ1-4)
        '    Si CPU non connecte : formule SQL (kg/m3).
        Dim offsetsCPU() As Integer = {538, 542, 546, 550, 530, 586, 594, 598, 602, 606}
        Dim lf(NC - 1) As String
        Dim cpuOk As Boolean = PLCData.Connecte
        For i As Integer = 0 To NC - 1
            Dim v As Double = 0
            If cpuOk Then v = ModulePLCDuplex.LireDBDReal(1, offsetsCPU(i))
            If Not cpuOk AndAlso fq IsNot Nothing Then v = fq(i)
            lf(i) = If(i >= 6, F2(v), F(v))
        Next
        g.DrawString(LN(lf), fM, Brushes.Black, x, y)
        g.DrawString("Consignes Formules", fM, Brushes.Black, xD, y) : y += 11
        Dim lu(NC - 1) As String
        For i As Integer = 0 To NC - 1 : lu(i) = If(i = 5, "l", "kg") : Next
        g.DrawString(LN(lu), fM, Brushes.Black, x, y)
        g.DrawString("Heure AjEau Qte M   Matricule", fM, Brushes.Black, xD, y) : y += 11
        g.DrawString("              l     m3", fM, Brushes.Black, xD, y) : y += 13

        ' ── Volume theorique de chaque gachee (regle du CPU, 18/09) ──
        '    premieres gachees = volume malaxeur ; le reste est reparti a parts
        '    egales sur les DEUX DERNIERES gachees (19/09).
        '    Secours (valeur impossible) : repartition au prorata du poids.
        Dim volBL As Double = Num(_bl, "VolumeM3")
        Dim nbGv As Integer = _gachees.Rows.Count
        Dim vMax As Double = VolumeMalaxeur()
        Dim volG(nbGv - 1) As Double
        Dim regleOk As Boolean = False
        If nbGv >= 1 AndAlso vMax > 0 AndAlso volBL > 0 Then
            If nbGv = 1 Then
                volG(0) = volBL : regleOk = True
            ElseIf Math.Abs(volBL - nbGv * vMax) < 0.0015 Then
                ' 21/09 : volume = gachees x malaxeur (fin de cycle / commande pleine) → chaque gachee = malaxeur
                For i As Integer = 0 To nbGv - 1 : volG(i) = vMax : Next
                regleOk = True
            Else
                Dim bout As Double = (volBL - (nbGv - 2) * vMax) / 2.0
                If bout > 0.0001 AndAlso bout <= vMax + 0.0001 Then
                    For i As Integer = 0 To nbGv - 1 : volG(i) = vMax : Next
                    volG(nbGv - 2) = bout : volG(nbGv - 1) = bout
                    regleOk = True
                End If
            End If
        End If
        If Not regleOk Then
            Dim poidsTotal As Double = 0
            Dim poidsG As New List(Of Double)
            For Each r As DataRow In _gachees.Rows
                Dim w As Double = Num(r, "TotalPoseTr") + Num(r, "PoseS1") + Num(r, "PoseS2") + Num(r, "PoseEAU")
                poidsG.Add(w) : poidsTotal += w
            Next
            For i As Integer = 0 To nbGv - 1
                volG(i) = If(poidsTotal > 0, volBL * poidsG(i) / poidsTotal, 0)
            Next
        End If

        ' ── Une ligne par gachee + bloc droit ────────────────
        Dim yD As Integer = y
        Dim sR(NC - 1) As Double, sT(NC - 1) As Double
        Dim volReel As Double = 0
        Dim idx As Integer = 0
        For Each r As DataRow In _gachees.Rows
            Dim lp(NC - 1) As String
            For i As Integer = 0 To NC - 1
                Dim pose As Double = Num(r, champsPose(i))
                Dim cons As Double = Num(r, champsCons(i))
                sR(i) += pose : sT(i) += cons
                lp(i) = If(i >= 6, F2(pose), F(pose))
            Next
            g.DrawString(LN(lp), fM, Brushes.Black, x, y)
            y += 12

            Dim vol As Double = volG(idx)
            volReel += vol
            Dim ajEau As Double = Num(r, "AjoutEau")   ' 22/09 : DB71.DBW3360… (Int, via GacheesDetails.AjoutEau)
            g.DrawString(HeureG(r).PadRight(6) & ajEau.ToString("0", Globalization.CultureInfo.InvariantCulture).PadLeft(4) &
                         "  " & vol.ToString("0.000", Globalization.CultureInfo.InvariantCulture) &
                         "   " & MatriculeG(r), fM, Brushes.Black, xD, yD)
            yD += 12
            idx += 1
        Next

        ' ── 3 dernieres lignes : cumul reel / theorique / ecart % ──
        '    Lues dans le CPU a la fin de commande (DB65 reel / DB101 theorique) et
        '    enregistrees avec le BL (BonsLivraison.CpuReel* / CpuTheo*).
        '    A defaut (anciens BL) : sommes des gachees SQL.
        Dim nomsCpu() As String = {"TR1", "TR2", "TR3", "TR4", "CIM", "EAU", "ADJ1", "ADJ2", "ADJ3", "ADJ4"}
        Dim cpuDispo As Boolean = False
        For i As Integer = 0 To NC - 1
            If Num(_bl, "CpuTheo" & nomsCpu(i)) > 0.001 OrElse Num(_bl, "CpuReel" & nomsCpu(i)) > 0.001 Then cpuDispo = True : Exit For
        Next
        If cpuDispo Then
            For i As Integer = 0 To NC - 1
                sR(i) = Num(_bl, "CpuReel" & nomsCpu(i))
                sT(i) = Num(_bl, "CpuTheo" & nomsCpu(i))
            Next
        End If
        y += 4
        Dim lr(NC - 1) As String, lt(NC - 1) As String, le(NC - 1) As String
        For i As Integer = 0 To NC - 1
            lr(i) = If(i >= 6, F2(sR(i)), F(sR(i)))
            lt(i) = If(i >= 6, F2(sT(i)), F(sT(i)))
            If Math.Abs(sT(i)) < 0.01 Then
                le(i) = "0.00"
            Else
                le(i) = F2((sR(i) - sT(i)) / sT(i) * 100)
            End If
        Next
        g.DrawString(LN(lr), fMB, Brushes.Black, x, y)
        g.DrawString("Cumuls Reels " & volReel.ToString("0.000", Globalization.CultureInfo.InvariantCulture), fM, Brushes.Black, xD, Math.Max(y, yD + 8)) : y += 12
        g.DrawString(LN(lt), fM, Brushes.Black, x, y)
        g.DrawString("Cumuls Theo  " & Txt(_bl, "VolumeM3"), fM, Brushes.Black, xD, Math.Max(y, yD + 20)) : y += 12
        g.DrawString(LN(le), fM, Brushes.Black, x, y)
        g.DrawString("Ecarts %", fM, Brushes.Black, xD, Math.Max(y, yD + 32))
    End Sub

    ' ── 10 colonnes a largeur fixe (monospace, 8 caracteres) ────
    Private Function LN(c() As String) As String
        Dim sb As New System.Text.StringBuilder()
        For Each v As String In c
            sb.Append(v.PadLeft(8))
        Next
        Return sb.ToString()
    End Function



    Private Function F(d As Double) As String
        Return d.ToString("0", Globalization.CultureInfo.InvariantCulture)
    End Function
    Private Function F1(d As Double) As String
        Return d.ToString("0.0", Globalization.CultureInfo.InvariantCulture)
    End Function
    Private Function F2(d As Double) As String
        If Double.IsNaN(d) OrElse Double.IsInfinity(d) Then Return "0.00"
        Return d.ToString("0.00", Globalization.CultureInfo.InvariantCulture)
    End Function
    ' Matricule CPU de la gachee (colonne MatriculeGachee, ajoutee 16/09) — vide si absente
    Private Function MatriculeG(r As DataRow) As String
        If Not r.Table.Columns.Contains("MatriculeGachee") OrElse IsDBNull(r("MatriculeGachee")) Then Return ""
        Return Convert.ToInt64(r("MatriculeGachee")).ToString()
    End Function

    Private Function HeureG(r As DataRow) As String
        If Not r.Table.Columns.Contains("DateHeure") OrElse IsDBNull(r("DateHeure")) Then Return ""
        Return Convert.ToDateTime(r("DateHeure")).ToString("HH:mm")
    End Function

    ' ═══════════════════════════════════════════════════════════
    '  HELPERS DE DESSIN
    ' ═══════════════════════════════════════════════════════════
    Private Sub Boite(g As Graphics, p As Pen, x As Integer, y As Integer, w As Integer, h As Integer,
                      libelle As String, valeur As String, fLib As Font, fVal As Font)
        g.DrawRectangle(p, x, y, w, h)
        Dim sfHaut As New StringFormat() With {.Alignment = StringAlignment.Center, .LineAlignment = StringAlignment.Near}
        g.DrawString(libelle, fLib, Brushes.Black, New RectangleF(x + 2, y + 3, w - 4, 26), sfHaut)
        Dim sfMil As New StringFormat() With {.Alignment = StringAlignment.Center, .LineAlignment = StringAlignment.Center}
        g.DrawString(valeur, fVal, Brushes.Black, New RectangleF(x + 2, y + 22, w - 4, h - 24), sfMil)
    End Sub

    Private Sub DessinerCentre(g As Graphics, txt As String, font As Font, br As Brush,
                               x As Integer, y As Integer, w As Integer)
        Dim sz As SizeF = g.MeasureString(txt, font)
        g.DrawString(txt, font, br, x + (w - sz.Width) / 2, y)
    End Sub

    ' Chemin du logo : ConfigurationSociete.LogoPath, sinon images\Synoptique\LogoPath.png (dossier de l'application)
    Private Function CheminLogo() As String
        Try
            If SocieteLogoPath <> "" AndAlso File.Exists(SocieteLogoPath) Then Return SocieteLogoPath
            Dim defaut As String = Path.Combine(Application.StartupPath, "images", "Synoptique", "LogoPath.png")
            If File.Exists(defaut) Then Return defaut
        Catch
        End Try
        Return ""
    End Function

    Private Sub DessinerLogo(g As Graphics, x As Integer, y As Integer, w As Integer, h As Integer)
        Try
            Dim chemin As String = CheminLogo()
            If chemin <> "" Then
                Using img As Image = Image.FromFile(chemin)
                    ' Conserver les proportions dans le cadre (w × h)
                    Dim ratio As Double = Math.Min(w / img.Width, h / img.Height)
                    Dim lw As Integer = CInt(img.Width * ratio), lh As Integer = CInt(img.Height * ratio)
                    g.DrawImage(img, x, y + (h - lh) \ 2, lw, lh)
                End Using
                Return
            End If
        Catch
        End Try
        g.DrawString(SocieteNom, New Font("Arial", 13, FontStyle.Bold), Brushes.Black, x, y + 20)
    End Sub

    ' ═══════════════════════════════════════════════════════════
    '  HELPERS DE LECTURE (vide / 0 si colonne absente)
    ' ═══════════════════════════════════════════════════════════
    Private Function Txt(r As DataRow, col As String) As String
        If r Is Nothing OrElse Not r.Table.Columns.Contains(col) OrElse IsDBNull(r(col)) Then Return ""
        Return r(col).ToString().Trim()
    End Function

    Private Function RefTxt(col As String) As String
        Return Txt(_ref, col)
    End Function

    Private Function DateBL() As String
        If _bl Is Nothing OrElse Not _bl.Table.Columns.Contains("DateHeure") OrElse IsDBNull(_bl("DateHeure")) Then Return ""
        Return Convert.ToDateTime(_bl("DateHeure")).ToString("dd/MM/yyyy HH:mm")
    End Function

    Private Function Designation() As String
        Dim d As String = RefTxt("NomReference")

        If d = "" Then d = Txt(_bl, "NomFormule")
        Return d
    End Function

    Private Function Num(r As DataRow, col As String) As Double
        If r Is Nothing OrElse Not r.Table.Columns.Contains(col) OrElse IsDBNull(r(col)) Then Return 0
        Dim v As Double
        Return If(Double.TryParse(r(col).ToString(), v), v, 0)
    End Function

    Private Function NumTxt(r As DataRow, col As String) As String
        If r Is Nothing OrElse Not r.Table.Columns.Contains(col) OrElse IsDBNull(r(col)) Then Return ""
        Return r(col).ToString()
    End Function

End Module