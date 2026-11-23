Imports System.Drawing

' ================================================================
'  ThemePalette.vb — palette BETONIA (tirée du logo : noir graphite, blanc, gris béton)
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
    ' Désactivé
    Public Desactive As Color
    Public DesactiveTexte As Color

    Public Shared Function Creer(mode As ModeTheme) As ThemePalette
        Dim p As New ThemePalette()
        If mode = ModeTheme.Sombre Then
            p.Fond = H("16181B") : p.Surface = H("1E2126") : p.SurfaceAlt = H("1B1E22") : p.Champ = H("262A30")
            p.Texte = H("E8EAED") : p.TexteSecondaire = H("A6ADB5") : p.Bordure = H("7F8892") : p.Lien = H("8AB4F8")
            p.Primaire = H("E8EAED") : p.PrimaireTexte = H("16181B") : p.PrimaireSurvol = H("FFFFFF") : p.PrimaireActif = H("C9CED3")
            p.SecondaireFond = H("1E2126") : p.SecondaireTexte = H("E8EAED") : p.SecondaireBord = H("A6ADB5")
            p.SecondaireSurvol = H("2E343B") : p.SecondaireActif = H("3A414A")
            p.Selection = H("7A5C00") : p.SelectionTexte = H("FFFFFF")
            p.EnteteGrille = H("2E343B") : p.EnteteGrilleTexte = H("E8EAED")
            p.Desactive = H("2A2E34") : p.DesactiveTexte = H("8B939C")
        Else
            p.Fond = H("F2F3F5") : p.Surface = H("FAFAFB") : p.SurfaceAlt = H("F6F7F8") : p.Champ = H("FFFFFF")
            p.Texte = H("1F2328") : p.TexteSecondaire = H("4B5563") : p.Bordure = H("7D858E") : p.Lien = H("0B57D0")
            p.Primaire = H("1F2328") : p.PrimaireTexte = H("FFFFFF") : p.PrimaireSurvol = H("3A414A") : p.PrimaireActif = H("0F1114")
            p.SecondaireFond = H("FAFAFB") : p.SecondaireTexte = H("1F2328") : p.SecondaireBord = H("1F2328")
            p.SecondaireSurvol = H("E3E6EA") : p.SecondaireActif = H("D3D8DD")
            p.Selection = H("FFD98A") : p.SelectionTexte = H("1F2328")
            p.EnteteGrille = H("E3E6EA") : p.EnteteGrilleTexte = H("1F2328")
            p.Desactive = H("D9DCE0") : p.DesactiveTexte = H("5F6670")
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
