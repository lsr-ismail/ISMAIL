Imports System.Drawing

' ================================================================
'  ThemePalette.vb — palette BETONIA : noir, gris et nuances de gris (choix du client)
'  Contrastes vérifiés selon WCAG 2.1 niveau AA (4,5:1 texte, 3:1 composants d'interface).
'  Voir docs/CHARTE_GRAPHIQUE.md pour le tableau de contrastes.
' ================================================================
Public Enum ModeTheme
    Clair = 0
    Sombre = 1
End Enum

Public NotInheritable Class ThemePalette
    ' Fonds
    Public Fond As Color              ' fond des fenêtres et panneaux
    Public Surface As Color           ' cartes, zones surélevées
    Public SurfaceAlt As Color        ' lignes alternées, champs en lecture seule
    Public Champ As Color             ' champs de saisie, listes, grilles
    ' Texte
    Public Texte As Color
    Public TexteSecondaire As Color
    Public Bordure As Color           ' contour des composants (≥ 3:1)
    Public Lien As Color
    ' Bouton principal
    Public Primaire As Color
    Public PrimaireTexte As Color
    Public PrimaireSurvol As Color
    Public PrimaireActif As Color
    ' Bouton secondaire
    Public SecondaireFond As Color
    Public SecondaireTexte As Color
    Public SecondaireBord As Color
    Public SecondaireSurvol As Color
    Public SecondaireActif As Color
    ' Sélection et grilles
    Public Selection As Color
    Public SelectionTexte As Color
    Public EnteteGrille As Color
    Public EnteteGrilleTexte As Color
    ' Identité : bandeaux de titre (bleu marine) et accent (orange)
    Public Bandeau As Color
    Public BandeauTexte As Color
    Public Accent As Color
    ' Désactivé
    Public Desactive As Color
    Public DesactiveTexte As Color

    Public Shared Function Creer(mode As ModeTheme) As ThemePalette
        Dim p As New ThemePalette()
        If mode = ModeTheme.Sombre Then
            p.Fond = H("15171A") : p.Surface = H("1D2024") : p.SurfaceAlt = H("191C20") : p.Champ = H("24282D")
            p.Texte = H("E8EAED") : p.TexteSecondaire = H("A6ADB5") : p.Bordure = H("7F8892") : p.Lien = H("C9CED3")
            p.Primaire = H("D5D9DF") : p.PrimaireTexte = H("15171A") : p.PrimaireSurvol = H("FFFFFF") : p.PrimaireActif = H("B7BDC5")
            p.SecondaireFond = H("1D2024") : p.SecondaireTexte = H("E8EAED") : p.SecondaireBord = H("A6ADB5")
            p.SecondaireSurvol = H("2D3238") : p.SecondaireActif = H("3A4047")
            p.Selection = H("4A525B") : p.SelectionTexte = H("FFFFFF")
            p.EnteteGrille = H("2D3238") : p.EnteteGrilleTexte = H("E8EAED")
            p.Bandeau = H("2D3238") : p.BandeauTexte = H("E8EAED") : p.Accent = H("D5D9DF")
            p.Desactive = H("2A2E34") : p.DesactiveTexte = H("8B939C")
        Else
            p.Fond = H("F2F3F5") : p.Surface = H("FFFFFF") : p.SurfaceAlt = H("EEEFF1") : p.Champ = H("FFFFFF")
            p.Texte = H("1F2328") : p.TexteSecondaire = H("4B5563") : p.Bordure = H("7D858E") : p.Lien = H("3A414A")
            p.Primaire = H("2B3036") : p.PrimaireTexte = H("FFFFFF") : p.PrimaireSurvol = H("4A525B") : p.PrimaireActif = H("15181C")
            p.SecondaireFond = H("FFFFFF") : p.SecondaireTexte = H("2B3036") : p.SecondaireBord = H("2B3036")
            p.SecondaireSurvol = H("E1E4E8") : p.SecondaireActif = H("CDD2D8")
            p.Selection = H("D5D9DF") : p.SelectionTexte = H("1F2328")
            p.EnteteGrille = H("E1E4E8") : p.EnteteGrilleTexte = H("1F2328")
            p.Bandeau = H("2B3036") : p.BandeauTexte = H("FFFFFF") : p.Accent = H("2B3036")
            p.Desactive = H("DDE0E4") : p.DesactiveTexte = H("5F6670")
        End If
        Return p
    End Function

    Private Shared Function H(hex As String) As Color
        Return Color.FromArgb(255, Convert.ToInt32(hex.Substring(0, 2), 16), Convert.ToInt32(hex.Substring(2, 2), 16), Convert.ToInt32(hex.Substring(4, 2), 16))
    End Function

    ' ── Contraste WCAG ─────────────────────────────────────────
    Public Shared Function LuminanceRelative(c As Color) As Double
        Dim f = Function(v As Integer) As Double
                    Dim s As Double = v / 255.0
                    Return If(s <= 0.03928, s / 12.92, Math.Pow((s + 0.055) / 1.055, 2.4))
                End Function
        Return 0.2126 * f(c.R) + 0.7152 * f(c.G) + 0.0722 * f(c.B)
    End Function

    Public Shared Function Contraste(a As Color, b As Color) As Double
        Dim la As Double = LuminanceRelative(a), lb As Double = LuminanceRelative(b)
        Dim hi As Double = Math.Max(la, lb), lo As Double = Math.Min(la, lb)
        Return (hi + 0.05) / (lo + 0.05)
    End Function

    ''' <summary>Blanc ou graphite, selon ce qui contraste le mieux avec le fond donné.</summary>
    Public Function MeilleurTexte(fond As Color) As Color
        Dim cl As Color = H("FFFFFF"), cs As Color = H("16181B")
        Return If(Contraste(cl, fond) >= Contraste(cs, fond), cl, cs)
    End Function
End Class
