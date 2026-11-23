Imports System.Drawing
Imports System.IO
Imports System.Reflection
Imports System.Runtime.CompilerServices
Imports System.Runtime.InteropServices
Imports System.Text.RegularExpressions
Imports System.Windows.Forms

' ================================================================
'  ThemeManager.vb — charte graphique BETONIA : thème clair / sombre, boutons primaire / secondaire,
'  typographie unifiée (Segoe UI 9 / 10 / 12 / 14 pt).
'
'  PRINCIPES
'   • AUCUNE logique métier n'est modifiée : le thème ne fait que poser des couleurs, polices et styles de
'     boutons sur les contrôles existants, au moment où une fenêtre s'ouvre (balayage de Application.OpenForms).
'   • Les couleurs « sémantiques » posées par le code (marche = vert, arrêt = rouge, alarmes…) sont conservées :
'     un contrôle dont la couleur d'origine est vive n'est jamais repeint ; il reçoit seulement un effet de survol.
'   • Si le code change lui-même une couleur après le thème, le thème ne l'écrase pas.
'   • Le mode (clair / sombre) est mémorisé dans %APPDATA%\Betonia\theme.ini.
' ================================================================
Public NotInheritable Class ThemeManager

    Private Sub New()
    End Sub

    ' barre de titre Windows 10/11 : mode sombre + couleurs (ignoré sans erreur sur les systèmes plus anciens)
    <DllImport("dwmapi.dll")>
    Private Shared Function DwmSetWindowAttribute(hwnd As IntPtr, attr As Integer, ByRef valeur As Integer, taille As Integer) As Integer
    End Function

    Private Shared Sub ThemeBarreTitre(f As Form)
        Try
            If Not f.IsHandleCreated Then Return
            Dim sombre As Integer = If(_mode = ModeTheme.Sombre, 1, 0)
            DwmSetWindowAttribute(f.Handle, 20, sombre, 4)                        ' DWMWA_USE_IMMERSIVE_DARK_MODE
            Dim cap As Integer = _p.Surface.R Or (_p.Surface.G << 8) Or (_p.Surface.B << 16)
            Dim txt As Integer = _p.Texte.R Or (_p.Texte.G << 8) Or (_p.Texte.B << 16)
            DwmSetWindowAttribute(f.Handle, 35, cap, 4)                           ' DWMWA_CAPTION_COLOR (Windows 11)
            DwmSetWindowAttribute(f.Handle, 36, txt, 4)                           ' DWMWA_TEXT_COLOR   (Windows 11)
        Catch
        End Try
    End Sub

    ' ── état global ──────────────────────────────────────────
    Private Shared _mode As ModeTheme = ModeTheme.Clair
    Private Shared _p As ThemePalette = ThemePalette.Creer(ModeTheme.Clair)
    Private Shared _timer As Timer
    Private Shared _icone As Icon
    Private Shared ReadOnly _formes As New ConditionalWeakTable(Of Form, Object)()
    Private Shared ReadOnly _etat As New ConditionalWeakTable(Of Control, EtatControle)()
    Private Shared ReadOnly _cacheFonts As New Dictionary(Of String, Font)()
    Private Shared ReadOnly _peintres As New ConditionalWeakTable(Of TabControl, PeintreOnglets)()

    Public Shared Event ModeChange As EventHandler

    Private NotInheritable Class EtatControle
        Public Initialise As Boolean
        Public OrigFond As Color
        Public OrigTexte As Color
        Public Role As String = ""            ' Primaire | Secondaire | Semantique | Neutre | Ignore
        Public DernierFond As Color
        Public DernierTexte As Color
        Public FondPose As Boolean
        Public TextePose As Boolean
        Public Raccorde As Boolean
        Public FondAvantSurvol As Color
        Public FondSurvol As Color
        Public Survole As Boolean
        Public PoseEnCours As Boolean         ' true pendant nos propres affectations de couleur
    End Class

    Public Shared ReadOnly Property Mode As ModeTheme
        Get
            Return _mode
        End Get
    End Property

    Public Shared ReadOnly Property Palette As ThemePalette
        Get
            Return _p
        End Get
    End Property

    ' ═════════════ API publique ═════════════
    ''' <summary>À appeler une fois au démarrage : lit le mode mémorisé et surveille l'ouverture des fenêtres.</summary>
    Public Shared Sub Demarrer()
        If _timer IsNot Nothing Then Return
        Try
            _mode = LireModeMemorise()
            _p = ThemePalette.Creer(_mode)
            Try
                _icone = Icon.ExtractAssociatedIcon(Application.ExecutablePath)
            Catch
            End Try
            _timer = New Timer() With {.Interval = 200}
            AddHandler _timer.Tick, AddressOf Balayer
            _timer.Start()
        Catch ex As Exception
            Debug.WriteLine("ThemeManager.Demarrer : " & ex.Message)
        End Try
    End Sub

    Public Shared Sub DefinirMode(m As ModeTheme)
        If m = _mode Then Return
        _mode = m
        _p = ThemePalette.Creer(m)
        MemoriserMode()
        For Each f As Form In Application.OpenForms.Cast(Of Form).ToList()
            AppliquerFenetre(f)
        Next
        RaiseEvent ModeChange(Nothing, EventArgs.Empty)
    End Sub

    Public Shared Sub BasculerMode()
        DefinirMode(If(_mode = ModeTheme.Clair, ModeTheme.Sombre, ModeTheme.Clair))
    End Sub

    ''' <summary>Applique le thème à une fenêtre et à tous ses contrôles (utilisé aussi par les outils de capture).</summary>
    Public Shared Sub AppliquerFenetre(f As Form)
        Try
            If f Is Nothing OrElse f.IsDisposed Then Return
            If TypeOf f Is PrintPreviewDialog Then Return
            Dim marque As Object = Nothing
            If Not _formes.TryGetValue(f, marque) Then _formes.Add(f, New Object())
            If f.Icon Is Nothing OrElse f.Icon Is SystemIcons.Application Then
                If _icone IsNot Nothing Then f.Icon = _icone
            ElseIf _icone IsNot Nothing Then
                f.Icon = _icone
            End If
            f.SuspendLayout()
            AppliquerArbre(f)
            f.Text = TitreAffiche(f)
            HabillerConnexion(f)
            InstallerBasculeur(f)
            f.ResumeLayout(False)
            ThemeBarreTitre(f)
            f.Invalidate(True)
        Catch ex As Exception
            Debug.WriteLine("ThemeManager.AppliquerFenetre : " & ex.Message)
        End Try
    End Sub

    Public Shared Function ModeEnTexte() As String
        Return If(_mode = ModeTheme.Sombre, "Sombre", "Clair")
    End Function

    ' ═════════════ balayage ═════════════
    Private Shared Sub Balayer(sender As Object, e As EventArgs)
        Try
            For Each f As Form In Application.OpenForms.Cast(Of Form).ToList()
                Dim marque As Object = Nothing
                If Not _formes.TryGetValue(f, marque) Then AppliquerFenetre(f)
            Next
        Catch ex As Exception
            Debug.WriteLine("ThemeManager.Balayer : " & ex.Message)
        End Try
    End Sub

    ' ═════════════ mémorisation du mode ═════════════
    Private Shared Function CheminIni() As String
        Return Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "Betonia", "theme.ini")
    End Function

    Private Shared Function LireModeMemorise() As ModeTheme
        Try
            Dim ch As String = CheminIni()
            If File.Exists(ch) Then
                For Each l In File.ReadAllLines(ch)
                    If l.Trim().StartsWith("Mode=", StringComparison.OrdinalIgnoreCase) Then
                        Return If(l.Substring(5).Trim().Equals("Sombre", StringComparison.OrdinalIgnoreCase), ModeTheme.Sombre, ModeTheme.Clair)
                    End If
                Next
            End If
        Catch
        End Try
        Return ModeTheme.Clair
    End Function

    Private Shared Sub MemoriserMode()
        Try
            Dim ch As String = CheminIni()
            Directory.CreateDirectory(Path.GetDirectoryName(ch))
            File.WriteAllText(ch, "Mode=" & ModeEnTexte() & Environment.NewLine)
        Catch
        End Try
    End Sub

    ' ═════════════ bascule clair / sombre dans l'interface ═════════════
    Private Shared Sub InstallerBasculeur(f As Form)
        Try
            ' 1) menu « Affichage » si la fenêtre a une barre de menus (écran principal)
            Dim ms As MenuStrip = f.Controls.OfType(Of MenuStrip)().FirstOrDefault()
            If ms IsNot Nothing Then
                Dim existe As Boolean = ms.Items.OfType(Of ToolStripItem)().Any(Function(i) i.Name = "mnuBetoniaAffichage")
                If Not existe Then
                    Dim top As New ToolStripMenuItem("Affichage") With {.Name = "mnuBetoniaAffichage"}
                    Dim sub1 As New ToolStripMenuItem("Mode sombre") With {.Name = "mnuBetoniaModeSombre", .CheckOnClick = True}
                    sub1.Checked = (_mode = ModeTheme.Sombre)
                    AddHandler sub1.Click, Sub(s, e) DefinirMode(If(CType(s, ToolStripMenuItem).Checked, ModeTheme.Sombre, ModeTheme.Clair))
                    AddHandler ModeChange, Sub(s, e) sub1.Checked = (_mode = ModeTheme.Sombre)
                    top.DropDownItems.Add(sub1)
                    ms.Items.Add(top)
                End If
            End If
            ' 2) petit bouton sur l'écran de connexion
            If TypeOf f Is LoginForm Then
                Dim b As Button = f.Controls.OfType(Of Button)().FirstOrDefault(Function(x) x.Name = "btnBetoniaTheme")
                If b Is Nothing Then
                    b = New Button() With {.Name = "btnBetoniaTheme", .Size = New Size(150, 30),
                        .Location = New Point(10, Math.Max(0, f.ClientSize.Height - 40)), .Anchor = AnchorStyles.Bottom Or AnchorStyles.Left, .TabStop = True}
                    AddHandler b.Click, Sub(s, e) BasculerMode()
                    AddHandler ModeChange, Sub(s, e) b.Text = If(_mode = ModeTheme.Sombre, "Mode clair", "Mode sombre")
                    f.Controls.Add(b)
                    b.BringToFront()
                End If
                b.Text = If(_mode = ModeTheme.Sombre, "Mode clair", "Mode sombre")
            End If
        Catch ex As Exception
            Debug.WriteLine("ThemeManager.InstallerBasculeur : " & ex.Message)
        End Try
    End Sub

    ' ═════════════ titres de fenêtres : « BETONIA — Nom lisible » ═════════════
    Private Shared Function TitreAffiche(f As Form) As String
        Try
            If TypeOf f Is PanelMain Then Return "BETONIA"
            Dim t As String = f.Text
            If String.IsNullOrWhiteSpace(t) Then t = f.GetType().Name
            If t.StartsWith("BETONIA", StringComparison.OrdinalIgnoreCase) Then Return t
            ' noms techniques (Planning_journalier, Configuration_Automate, FormGraphiqueTendance...) -> texte lisible
            t = t.Replace("_", " ")
            t = Regex.Replace(t, "(?<=[a-zà-ÿ])(?=[A-Z])", " ")
            t = Regex.Replace(t, "^(Form|Frm)\s+", "")
            t = t.Trim()
            If t.Length = 0 Then Return "BETONIA"
            t = Char.ToUpper(t(0)) & t.Substring(1)
            Return "BETONIA — " & t
        Catch
            Return f.Text
        End Try
    End Function

    ' ═════════════ logo sur l'écran de connexion ═════════════
    Private Shared Sub HabillerConnexion(f As Form)
        Try
            If Not (TypeOf f Is LoginForm) Then Return
            If f.Controls.OfType(Of PictureBox)().Any(Function(x) x.Name = "picBetoniaLogo") Then Return
            Dim flux As Stream = Assembly.GetExecutingAssembly().GetManifestResourceStream("central.logo.png")
            If flux Is Nothing Then Return
            Dim img As Image = Image.FromStream(flux)
            Dim delta As Integer = 150
            For Each ctl As Control In f.Controls.Cast(Of Control).ToList()
                ctl.Top += delta
            Next
            f.Height += delta
            Dim pic As New PictureBox() With {.Name = "picBetoniaLogo", .Image = img, .SizeMode = PictureBoxSizeMode.Zoom,
                .Size = New Size(130, 128), .BackColor = Color.Transparent}
            pic.Location = New Point(Math.Max(0, (f.ClientSize.Width - pic.Width) \ 2), 10)
            f.Controls.Add(pic)
            ' le logo est noir sur fond transparent : pastille claire pour rester lisible en mode sombre
            pic.BackColor = Color.FromArgb(242, 243, 245)
        Catch ex As Exception
            Debug.WriteLine("ThemeManager.HabillerConnexion : " & ex.Message)
        End Try
    End Sub

    ' ═════════════ outils couleur ═════════════
    Private Shared Function EstNeutre(c As Color) As Boolean
        If c.IsEmpty Then Return True
        If c.A < 255 Then Return True
        If c.IsSystemColor Then Return True
        Dim mx As Integer = Math.Max(c.R, Math.Max(c.G, c.B))
        Dim mn As Integer = Math.Min(c.R, Math.Min(c.G, c.B))
        Return (mx - mn) <= 24
    End Function

    Private Shared Function FondEffectif(c As Control) As Color
        Dim cur As Control = c
        Do While cur IsNot Nothing
            Dim bc As Color = cur.BackColor
            If bc.A = 255 AndAlso Not bc.IsEmpty Then Return bc
            cur = cur.Parent
        Loop
        Return _p.Fond
    End Function

    Private Shared Function Eclaircir(c As Color, delta As Single) As Color
        ' delta > 0 éclaircit, delta < 0 assombrit (mélange vers blanc / noir)
        If delta >= 0 Then
            Return Color.FromArgb(255, CInt(c.R + (255 - c.R) * delta), CInt(c.G + (255 - c.G) * delta), CInt(c.B + (255 - c.B) * delta))
        Else
            Dim d As Single = -delta
            Return Color.FromArgb(255, CInt(c.R * (1 - d)), CInt(c.G * (1 - d)), CInt(c.B * (1 - d)))
        End If
    End Function

    Private Shared Function Capturer(c As Control) As EtatControle
        Dim st As EtatControle = _etat.GetValue(c, Function(k) New EtatControle())
        If Not st.Initialise Then
            st.Initialise = True
            st.OrigFond = c.BackColor
            st.OrigTexte = c.ForeColor
        End If
        Return st
    End Function

    ''' <summary>Pose un fond sauf si le code de l'application l'a modifié depuis notre dernier passage.</summary>
    Private Shared Sub PoserFond(c As Control, st As EtatControle, couleur As Color)
        If st.FondPose AndAlso c.BackColor.ToArgb() <> st.DernierFond.ToArgb() Then Return
        c.BackColor = couleur
        st.DernierFond = c.BackColor
        st.FondPose = True
    End Sub

    Private Shared Sub PoserTexte(c As Control, st As EtatControle, couleur As Color)
        If st.TextePose AndAlso c.ForeColor.ToArgb() <> st.DernierTexte.ToArgb() Then Return
        c.ForeColor = couleur
        st.DernierTexte = c.ForeColor
        st.TextePose = True
    End Sub

    ''' <summary>Couleur de texte : thème si la couleur d'origine est neutre ; sinon conservée si lisible.</summary>
    Private Shared Function TexteLisible(c As Control, st As EtatControle, fond As Color) As Color
        Dim cible As Color
        If EstNeutre(st.OrigTexte) Then
            cible = _p.Texte
            If ThemePalette.Contraste(cible, fond) < 4.5 Then cible = _p.MeilleurTexte(fond)
        Else
            cible = st.OrigTexte
            If ThemePalette.Contraste(cible, fond) < 3.0 Then cible = _p.MeilleurTexte(fond)
        End If
        Return cible
    End Function

    ' ═════════════ polices ═════════════
    Private Shared ReadOnly _famillesConservees As String() = {"Consolas", "Courier New", "Lucida Console", "Wingdings", "Webdings",
        "Marlett", "Segoe UI Emoji", "Segoe UI Symbol", "Segoe MDL2 Assets", "Symbol"}

    Private Shared Function PoliceUnifiee(f As Font) As Font
        If f Is Nothing Then Return Nothing
        If _famillesConservees.Contains(f.Name, StringComparer.OrdinalIgnoreCase) Then Return Nothing
        Dim t As Single = f.SizeInPoints
        Dim cible As Single
        If t < 9.5F Then
            cible = 9.0F
        ElseIf t < 11.0F Then
            cible = 10.0F
        ElseIf t < 12.5F Then
            cible = 12.0F
        ElseIf t < 14.5F Then
            cible = 14.0F
        Else
            cible = CSng(Math.Round(t))
        End If
        If f.Name = "Segoe UI" AndAlso Math.Abs(t - cible) < 0.01F Then Return Nothing
        Dim cle As String = cible.ToString("0.0") & "|" & CInt(f.Style)
        Dim res As Font = Nothing
        If Not _cacheFonts.TryGetValue(cle, res) Then
            res = New Font("Segoe UI", cible, f.Style, GraphicsUnit.Point)
            _cacheFonts(cle) = res
        End If
        Return res
    End Function

    Private Shared Sub UnifierPolice(c As Control)
        If TypeOf c Is Form OrElse TypeOf c Is PictureBox OrElse TypeOf c Is TabControl Then Return
        Dim nf As Font = PoliceUnifiee(c.Font)
        If nf IsNot Nothing Then c.Font = nf
    End Sub

    ' ═════════════ application récursive ═════════════
    Private Shared Sub AppliquerArbre(c As Control)
        AppliquerControle(c)
        For Each enfant As Control In c.Controls.Cast(Of Control).ToList()
            AppliquerArbre(enfant)
        Next
    End Sub

    Private Shared Sub SurAjoutControle(sender As Object, e As ControlEventArgs)
        Try
            AppliquerArbre(e.Control)
        Catch ex As Exception
            Debug.WriteLine("ThemeManager.SurAjoutControle : " & ex.Message)
        End Try
    End Sub

    Private Shared Sub AppliquerControle(c As Control)
        Dim st As EtatControle = Capturer(c)
        If Not st.Raccorde Then
            st.Raccorde = True
            AddHandler c.ControlAdded, AddressOf SurAjoutControle
        End If
        UnifierPolice(c)

        If TypeOf c Is Form Then
            PoserFond(c, st, _p.Fond) : PoserTexte(c, st, _p.Texte)
        ElseIf TypeOf c Is Button Then
            ThemeBouton(CType(c, Button), st)
        ElseIf TypeOf c Is DataGridView Then
            ThemeGrille(CType(c, DataGridView))
        ElseIf TypeOf c Is MenuStrip OrElse TypeOf c Is ToolStrip OrElse TypeOf c Is StatusStrip OrElse TypeOf c Is ContextMenuStrip Then
            ThemeBarre(CType(c, ToolStrip))
        ElseIf TypeOf c Is LinkLabel Then
            Dim ll As LinkLabel = CType(c, LinkLabel)
            ll.LinkColor = _p.Lien : ll.ActiveLinkColor = _p.Texte : ll.VisitedLinkColor = _p.Lien
            PoserTexte(c, st, _p.Texte)
        ElseIf TypeOf c Is Label Then
            ThemeEtiquette(c, st)
        ElseIf TypeOf c Is TabPage Then
            Dim tp As TabPage = CType(c, TabPage)
            tp.UseVisualStyleBackColor = False
            PoserFond(c, st, _p.Fond) : PoserTexte(c, st, _p.Texte)
        ElseIf TypeOf c Is TabControl Then
            Dim tc As TabControl = CType(c, TabControl)
            Dim pe As PeintreOnglets = Nothing
            If Not _peintres.TryGetValue(tc, pe) Then _peintres.Add(tc, New PeintreOnglets(tc))
            tc.Invalidate(True)
        ElseIf TypeOf c Is PictureBox OrElse TypeOf c Is ProgressBar Then
            ' images et jauges : jamais repeintes
        ElseIf TypeOf c Is TextBoxBase OrElse TypeOf c Is ComboBox OrElse TypeOf c Is NumericUpDown OrElse
               TypeOf c Is ListBox OrElse TypeOf c Is ListView OrElse TypeOf c Is TreeView OrElse
               TypeOf c Is DateTimePicker OrElse TypeOf c Is MonthCalendar OrElse TypeOf c Is MaskedTextBox Then
            ThemeChamp(c, st)
        ElseIf TypeOf c Is CheckBox OrElse TypeOf c Is RadioButton Then
            If EstNeutre(st.OrigFond) AndAlso st.OrigFond.A = 255 Then PoserFond(c, st, FondEffectifParent(c))
            PoserTexte(c, st, TexteLisible(c, st, FondEffectif(c)))
        Else
            ' panneaux, groupes, conteneurs divers
            If EstBandeauTitre(c, st) Then
                ThemeBandeau(c, st)
            Else
                If EstNeutre(st.OrigFond) AndAlso st.OrigFond.A = 255 Then PoserFond(c, st, _p.Fond)
                PoserTexte(c, st, TexteLisible(c, st, FondEffectif(c)))
            End If
        End If

        If c.ContextMenuStrip IsNot Nothing Then ThemeBarre(c.ContextMenuStrip)
    End Sub

    Private Shared Function FondEffectifParent(c As Control) As Color
        If c.Parent Is Nothing Then Return _p.Fond
        Return FondEffectif(c.Parent)
    End Function

    ' bandeau de titre : étiquette ou panneau collé en haut, peu haut, fond vif décoratif -> couleur de charte (graphite / gris acier)
    Private Shared Function EstBandeauTitre(c As Control, st As EtatControle) As Boolean
        If EstNeutre(st.OrigFond) Then Return False
        If EstEcranProcess(c) Then Return False
        Return c.Dock = DockStyle.Top AndAlso c.Height <= 64
    End Function

    Private Shared Sub ThemeBandeau(c As Control, st As EtatControle)
        Dim fond As Color = If(_mode = ModeTheme.Sombre, _p.EnteteGrille, _p.Primaire)
        PoserFond(c, st, fond)
        PoserTexte(c, st, _p.MeilleurTexte(fond))
        For Each enfant As Control In c.Controls
            If TypeOf enfant Is Label Then
                Dim ee As EtatControle = Capturer(enfant)
                If enfant.BackColor.A < 255 OrElse EstNeutre(ee.OrigFond) OrElse True Then
                    PoserFond(enfant, ee, fond)
                    PoserTexte(enfant, ee, _p.MeilleurTexte(fond))
                End If
            End If
        Next
    End Sub

    ' ── étiquettes ───────────────────────────────────────────
    Private Shared Sub ThemeEtiquette(c As Control, st As EtatControle)
        If EstBandeauTitre(c, st) Then
            ThemeBandeau(c, st)
            Return
        End If
        ' fond transparent conservé ; fond plein neutre -> fond du thème ; fond vif (voyant) conservé
        If st.OrigFond.A = 255 AndAlso EstNeutre(st.OrigFond) AndAlso Not st.OrigFond.IsEmpty Then
            PoserFond(c, st, FondEffectifParent(c))
        End If
        PoserTexte(c, st, TexteLisible(c, st, FondEffectif(c)))
    End Sub

    ' ── champs de saisie ─────────────────────────────────────
    Private Shared Sub ThemeChamp(c As Control, st As EtatControle)
        If EstNeutre(st.OrigFond) OrElse c.BackColor.IsSystemColor Then
            Dim lectureSeule As Boolean = (TypeOf c Is TextBoxBase AndAlso CType(c, TextBoxBase).ReadOnly)
            PoserFond(c, st, If(lectureSeule, _p.SurfaceAlt, _p.Champ))
        End If
        PoserTexte(c, st, TexteLisible(c, st, c.BackColor))
        If TypeOf c Is ComboBox Then
            Dim cb As ComboBox = CType(c, ComboBox)
            If _mode = ModeTheme.Sombre Then cb.FlatStyle = FlatStyle.Flat Else cb.FlatStyle = FlatStyle.Standard
            ' liste « DropDownList » : Windows ignore BackColor -> dessin manuel des éléments (aucun changement de comportement)
            If cb.DropDownStyle = ComboBoxStyle.DropDownList AndAlso cb.DrawMode = DrawMode.Normal Then
                cb.DrawMode = DrawMode.OwnerDrawFixed
                AddHandler cb.DrawItem, AddressOf ComboDessinElement
            End If
        End If
    End Sub

    Private Shared Sub ComboDessinElement(sender As Object, e As DrawItemEventArgs)
        Try
            Dim cb As ComboBox = CType(sender, ComboBox)
            Dim survol As Boolean = (e.State And DrawItemState.Selected) = DrawItemState.Selected
            Dim fond As Color = If(survol AndAlso (e.State And DrawItemState.ComboBoxEdit) = 0, _p.Selection, cb.BackColor)
            Dim texte As Color = If(survol AndAlso (e.State And DrawItemState.ComboBoxEdit) = 0, _p.SelectionTexte, cb.ForeColor)
            Using br As New SolidBrush(fond)
                e.Graphics.FillRectangle(br, e.Bounds)
            End Using
            If e.Index >= 0 Then
                TextRenderer.DrawText(e.Graphics, cb.GetItemText(cb.Items(e.Index)), cb.Font,
                                      New Rectangle(e.Bounds.X + 2, e.Bounds.Y, e.Bounds.Width - 2, e.Bounds.Height), texte,
                                      TextFormatFlags.Left Or TextFormatFlags.VerticalCenter Or TextFormatFlags.EndEllipsis)
            End If
        Catch
        End Try
    End Sub

    ' ── boutons : primaire / secondaire / sémantique ─────────
    Private Shared ReadOnly _rxPrimaire As New Regex("valid|enregistr|ajout|connecter|connexion|confirm|appliquer|activer|^ok$|envoyer|lancer|cr[ée]er", RegexOptions.IgnoreCase Or RegexOptions.Compiled)

    ' écrans de conduite de la centrale, dont les couleurs vives indiquent un état
    Private Shared ReadOnly _ecransProcess As String() = {"Synoptique", "Dosage_manuel", "Form_Test_Synoptique", "Assistance___PLC", "Frm_ParamEquipement"}

    Private Shared Function EstEcranProcess(c As Control) As Boolean
        Dim f As Form = c.FindForm()
        If f Is Nothing Then Return False
        Return _ecransProcess.Contains(f.GetType().Name, StringComparer.OrdinalIgnoreCase)
    End Function

    Private Shared Function EstPrimaire(b As Button) As Boolean
        Dim cle As String = (b.Name & " " & b.Text)
        Return _rxPrimaire.IsMatch(cle)
    End Function

    Private Shared Sub ThemeBouton(b As Button, st As EtatControle)
        If st.Role = "" Then
            If b.BackgroundImage IsNot Nothing OrElse b.Image IsNot Nothing Then
                st.Role = "Ignore"
            ElseIf BoutonsSemantiques.Noms.Contains(b.Name) Then
                st.Role = "Semantique"       ' couleur d'état pilotée par le code : jamais repeint
            ElseIf Not EstNeutre(st.OrigFond) AndAlso EstEcranProcess(b) Then
                st.Role = "Semantique"       ' écrans de conduite : les couleurs vives y portent un sens (AUTO / FORÇAGE, marche / arrêt)
            ElseIf EstPrimaire(b) Then
                st.Role = "Primaire"
            Else
                st.Role = "Secondaire"
            End If
            b.Cursor = Cursors.Hand
            AddHandler b.MouseEnter, AddressOf BoutonEntre
            AddHandler b.MouseLeave, AddressOf BoutonSorti
            AddHandler b.EnabledChanged, AddressOf BoutonActivation
            AddHandler b.BackColorChanged, AddressOf BoutonCouleurChangee
            AddHandler b.Paint, AddressOf BoutonDessinDesactive
        End If
        Select Case st.Role
            Case "Ignore"
                Return
            Case "Semantique"
                ' couleur vive posée par le concepteur ou par le code : conservée (effet de survol par événements)
                If ThemePalette.Contraste(b.ForeColor, b.BackColor) < 3.0 Then b.ForeColor = _p.MeilleurTexte(b.BackColor)
                Return
        End Select
        b.UseVisualStyleBackColor = False
        b.FlatStyle = FlatStyle.Flat
        b.FlatAppearance.BorderSize = 1
        AppliquerCouleursBouton(b, st)
    End Sub

    Private Shared Sub AppliquerCouleursBouton(b As Button, st As EtatControle)
        Dim fond, texte, bord, survol, actif As Color
        If Not b.Enabled Then
            fond = _p.Desactive : texte = _p.DesactiveTexte : bord = _p.Desactive : survol = _p.Desactive : actif = _p.Desactive
        ElseIf st.Role = "Primaire" Then
            fond = _p.Primaire : texte = _p.PrimaireTexte : bord = _p.Primaire : survol = _p.PrimaireSurvol : actif = _p.PrimaireActif
        Else
            fond = _p.SecondaireFond : texte = _p.SecondaireTexte : bord = _p.SecondaireBord : survol = _p.SecondaireSurvol : actif = _p.SecondaireActif
        End If
        st.PoseEnCours = True
        b.BackColor = fond : st.DernierFond = fond : st.FondPose = True
        b.ForeColor = texte : st.DernierTexte = texte : st.TextePose = True
        st.PoseEnCours = False
        b.FlatAppearance.BorderColor = bord
        b.FlatAppearance.MouseOverBackColor = survol
        b.FlatAppearance.MouseDownBackColor = actif
    End Sub

    ' si le code de l'application recolore lui-même un bouton « thémé », on le traite désormais comme bouton d'état
    Private Shared Sub BoutonCouleurChangee(sender As Object, e As EventArgs)
        Try
            Dim b As Button = CType(sender, Button)
            Dim st As EtatControle = Nothing
            If Not _etat.TryGetValue(b, st) Then Return
            If st.PoseEnCours OrElse st.Survole Then Return
            If (st.Role = "Primaire" OrElse st.Role = "Secondaire") AndAlso st.FondPose AndAlso b.BackColor.ToArgb() <> st.DernierFond.ToArgb() Then
                st.Role = "Semantique"
                b.FlatAppearance.MouseOverBackColor = Color.Empty
                b.FlatAppearance.MouseDownBackColor = Color.Empty
            End If
        Catch
        End Try
    End Sub

    ' Windows dessine le texte des boutons « plats » désactivés avec une teinte fixe, illisible sur fond sombre : on le redessine
    Private Shared Sub BoutonDessinDesactive(sender As Object, e As PaintEventArgs)
        Try
            Dim b As Button = CType(sender, Button)
            Dim st As EtatControle = Nothing
            If b.Enabled OrElse Not _etat.TryGetValue(b, st) Then Return
            If st.Role <> "Primaire" AndAlso st.Role <> "Secondaire" Then Return
            Dim r As Rectangle = b.ClientRectangle
            r.Inflate(-1, -1)
            Using br As New SolidBrush(_p.Desactive)
                e.Graphics.FillRectangle(br, r)
            End Using
            TextRenderer.DrawText(e.Graphics, b.Text, b.Font, r, _p.DesactiveTexte,
                                  TextFormatFlags.HorizontalCenter Or TextFormatFlags.VerticalCenter Or TextFormatFlags.WordBreak Or TextFormatFlags.EndEllipsis)
        Catch
        End Try
    End Sub

    Private Shared Sub BoutonActivation(sender As Object, e As EventArgs)
        Try
            Dim b As Button = CType(sender, Button)
            Dim st As EtatControle = Nothing
            If _etat.TryGetValue(b, st) AndAlso (st.Role = "Primaire" OrElse st.Role = "Secondaire") Then AppliquerCouleursBouton(b, st)
        Catch
        End Try
    End Sub

    ' survol des boutons « sémantiques » : on éclaircit / assombrit la couleur courante, puis on la restitue
    Private Shared Sub BoutonEntre(sender As Object, e As EventArgs)
        Try
            Dim b As Button = CType(sender, Button)
            Dim st As EtatControle = Nothing
            If Not _etat.TryGetValue(b, st) OrElse st.Role <> "Semantique" OrElse Not b.Enabled Then Return
            st.FondAvantSurvol = b.BackColor
            Dim lum As Double = ThemePalette.LuminanceRelative(b.BackColor)
            st.FondSurvol = Eclaircir(b.BackColor, If(lum > 0.35, -0.14F, 0.18F))
            st.Survole = True
            b.BackColor = st.FondSurvol
        Catch
        End Try
    End Sub

    Private Shared Sub BoutonSorti(sender As Object, e As EventArgs)
        Try
            Dim b As Button = CType(sender, Button)
            Dim st As EtatControle = Nothing
            If Not _etat.TryGetValue(b, st) OrElse st.Role <> "Semantique" OrElse Not st.Survole Then Return
            st.Survole = False
            ' on ne restitue que si le code n'a pas changé la couleur entre-temps
            If b.BackColor.ToArgb() = st.FondSurvol.ToArgb() Then b.BackColor = st.FondAvantSurvol
        Catch
        End Try
    End Sub

    ' ── grilles de données ───────────────────────────────────
    Private Shared Sub ThemeGrille(g As DataGridView)
        g.BackgroundColor = _p.Fond
        g.GridColor = _p.Bordure
        g.EnableHeadersVisualStyles = False
        g.BorderStyle = BorderStyle.FixedSingle
        With g.ColumnHeadersDefaultCellStyle
            .BackColor = _p.EnteteGrille : .ForeColor = _p.EnteteGrilleTexte
            .SelectionBackColor = _p.EnteteGrille : .SelectionForeColor = _p.EnteteGrilleTexte
            Dim f As Font = PoliceUnifiee(.Font) : If f IsNot Nothing Then .Font = New Font(f, FontStyle.Bold)
        End With
        With g.RowHeadersDefaultCellStyle
            .BackColor = _p.EnteteGrille : .ForeColor = _p.EnteteGrilleTexte
            .SelectionBackColor = _p.Selection : .SelectionForeColor = _p.SelectionTexte
        End With
        With g.DefaultCellStyle
            .BackColor = _p.Champ : .ForeColor = _p.Texte
            .SelectionBackColor = _p.Selection : .SelectionForeColor = _p.SelectionTexte
            Dim f As Font = PoliceUnifiee(.Font) : If f IsNot Nothing Then .Font = f
        End With
        With g.AlternatingRowsDefaultCellStyle
            .BackColor = _p.SurfaceAlt : .ForeColor = _p.Texte
            .SelectionBackColor = _p.Selection : .SelectionForeColor = _p.SelectionTexte
        End With
        ' garde-fou de lisibilité : le code pose parfois des fonds pastel par ligne ; en mode sombre le texte clair y serait illisible
        RemoveHandler g.CellFormatting, AddressOf GrilleCellFormatting
        AddHandler g.CellFormatting, AddressOf GrilleCellFormatting
    End Sub

    Private Shared Sub GrilleCellFormatting(sender As Object, e As DataGridViewCellFormattingEventArgs)
        Try
            Dim s As DataGridViewCellStyle = e.CellStyle
            If s Is Nothing OrElse s.BackColor.IsEmpty Then Return
            If ThemePalette.Contraste(s.ForeColor, s.BackColor) < 3.0 Then s.ForeColor = _p.MeilleurTexte(s.BackColor)
        Catch
        End Try
    End Sub

    ' ── onglets : repeints par-dessus le dessin Windows (aucun changement de comportement) ──
    Private NotInheritable Class PeintreOnglets
        Inherits NativeWindow
        Private ReadOnly _tc As TabControl

        Public Sub New(tc As TabControl)
            _tc = tc
            AddHandler tc.HandleCreated, Sub(s, e) Rattacher()
            AddHandler tc.HandleDestroyed, Sub(s, e) ReleaseHandle()
            Rattacher()
        End Sub

        Private Sub Rattacher()
            If _tc.IsHandleCreated AndAlso Handle <> _tc.Handle Then
                ReleaseHandle()
                AssignHandle(_tc.Handle)
            End If
        End Sub

        Protected Overrides Sub WndProc(ByRef m As Message)
            MyBase.WndProc(m)
            Try
                If m.Msg = &HF Then                                   ' WM_PAINT : affichage normal
                    Using g As Graphics = Graphics.FromHwnd(_tc.Handle)
                        Dessiner(g)
                    End Using
                ElseIf (m.Msg = &H317 OrElse m.Msg = &H318) AndAlso m.WParam <> IntPtr.Zero Then   ' WM_PRINT / WM_PRINTCLIENT : captures, impression
                    Using g As Graphics = Graphics.FromHdc(m.WParam)
                        Dessiner(g)
                    End Using
                End If
            Catch
            End Try
        End Sub

        Private Sub Dessiner(g As Graphics)
            If _tc.TabCount = 0 OrElse _tc.Alignment <> TabAlignment.Top OrElse _tc.Multiline Then Return
            If True Then
                Dim w As Integer = _tc.ClientSize.Width, h As Integer = _tc.ClientSize.Height
                Dim hOng As Integer = 0
                For i As Integer = 0 To _tc.TabCount - 1
                    hOng = Math.Max(hOng, _tc.GetTabRect(i).Bottom)
                Next
                Dim p As ThemePalette = _p
                Using brFond As New SolidBrush(p.Fond)
                    g.FillRectangle(brFond, 0, 0, w, hOng)                         ' bandeau des onglets
                    Dim disp As Rectangle = _tc.DisplayRectangle
                    g.SetClip(disp, Drawing2D.CombineMode.Exclude)
                    g.FillRectangle(brFond, 0, hOng, w, h - hOng)                   ' cadre autour de la page
                    g.ResetClip()
                End Using
                Using pen As New Pen(p.Bordure)
                    g.DrawRectangle(pen, 0, hOng, w - 1, h - hOng - 1)
                    For i As Integer = 0 To _tc.TabCount - 1
                        Dim r As Rectangle = _tc.GetTabRect(i)
                        Dim sel As Boolean = (i = _tc.SelectedIndex)
                        Using br As New SolidBrush(If(sel, p.Surface, p.Fond))
                            g.FillRectangle(br, r)
                        End Using
                        g.DrawRectangle(pen, r.X, r.Y, r.Width - 1, r.Height - 1)
                        If sel Then
                            Using brA As New SolidBrush(p.Primaire)
                                g.FillRectangle(brA, r.X + 1, r.Y, r.Width - 2, 3)
                            End Using
                            Using brS As New SolidBrush(p.Surface)
                                g.FillRectangle(brS, r.X + 1, r.Bottom - 1, r.Width - 2, 2)
                            End Using
                        End If
                        TextRenderer.DrawText(g, _tc.TabPages(i).Text, _tc.Font, r, If(sel, p.Texte, p.TexteSecondaire),
                                              TextFormatFlags.HorizontalCenter Or TextFormatFlags.VerticalCenter Or TextFormatFlags.SingleLine Or TextFormatFlags.EndEllipsis)
                    Next
                End Using
            End If
        End Sub
    End Class

    ' ── barres de menus / outils / état ──────────────────────
    Private Shared Sub ThemeBarre(t As ToolStrip)
        t.RenderMode = ToolStripRenderMode.Professional
        t.Renderer = New ToolStripProfessionalRenderer(New TableCouleursTheme(_p)) With {.RoundedEdges = False}
        t.BackColor = _p.Surface
        t.ForeColor = _p.Texte
        For Each it As ToolStripItem In t.Items
            ThemeItem(it)
        Next
    End Sub

    Private Shared Sub ThemeItem(it As ToolStripItem)
        it.ForeColor = _p.Texte
        it.BackColor = Color.Transparent
        Dim mi As ToolStripDropDownItem = TryCast(it, ToolStripDropDownItem)
        If mi IsNot Nothing Then
            mi.DropDown.BackColor = _p.Surface
            For Each sub1 As ToolStripItem In mi.DropDownItems
                ThemeItem(sub1)
            Next
        End If
    End Sub

    ''' <summary>Couleurs des menus déroulants et barres (rendu « Professional » recoloré).</summary>
    Private NotInheritable Class TableCouleursTheme
        Inherits ProfessionalColorTable
        Private ReadOnly _c As ThemePalette

        Public Sub New(p As ThemePalette)
            _c = p
            UseSystemColors = False
        End Sub

        Public Overrides ReadOnly Property MenuItemSelected As Color
            Get
                Return _c.SecondaireSurvol
            End Get
        End Property
        Public Overrides ReadOnly Property MenuItemSelectedGradientBegin As Color
            Get
                Return _c.SecondaireSurvol
            End Get
        End Property
        Public Overrides ReadOnly Property MenuItemSelectedGradientEnd As Color
            Get
                Return _c.SecondaireSurvol
            End Get
        End Property
        Public Overrides ReadOnly Property MenuItemPressedGradientBegin As Color
            Get
                Return _c.SecondaireActif
            End Get
        End Property
        Public Overrides ReadOnly Property MenuItemPressedGradientMiddle As Color
            Get
                Return _c.SecondaireActif
            End Get
        End Property
        Public Overrides ReadOnly Property MenuItemPressedGradientEnd As Color
            Get
                Return _c.SecondaireActif
            End Get
        End Property
        Public Overrides ReadOnly Property MenuItemBorder As Color
            Get
                Return _c.Bordure
            End Get
        End Property
        Public Overrides ReadOnly Property MenuBorder As Color
            Get
                Return _c.Bordure
            End Get
        End Property
        Public Overrides ReadOnly Property MenuStripGradientBegin As Color
            Get
                Return _c.Surface
            End Get
        End Property
        Public Overrides ReadOnly Property MenuStripGradientEnd As Color
            Get
                Return _c.Surface
            End Get
        End Property
        Public Overrides ReadOnly Property ToolStripDropDownBackground As Color
            Get
                Return _c.Surface
            End Get
        End Property
        Public Overrides ReadOnly Property ImageMarginGradientBegin As Color
            Get
                Return _c.Surface
            End Get
        End Property
        Public Overrides ReadOnly Property ImageMarginGradientMiddle As Color
            Get
                Return _c.Surface
            End Get
        End Property
        Public Overrides ReadOnly Property ImageMarginGradientEnd As Color
            Get
                Return _c.Surface
            End Get
        End Property
        Public Overrides ReadOnly Property SeparatorDark As Color
            Get
                Return _c.Bordure
            End Get
        End Property
        Public Overrides ReadOnly Property SeparatorLight As Color
            Get
                Return _c.Surface
            End Get
        End Property
        Public Overrides ReadOnly Property ToolStripGradientBegin As Color
            Get
                Return _c.Surface
            End Get
        End Property
        Public Overrides ReadOnly Property ToolStripGradientMiddle As Color
            Get
                Return _c.Surface
            End Get
        End Property
        Public Overrides ReadOnly Property ToolStripGradientEnd As Color
            Get
                Return _c.Surface
            End Get
        End Property
        Public Overrides ReadOnly Property ToolStripBorder As Color
            Get
                Return _c.Bordure
            End Get
        End Property
        Public Overrides ReadOnly Property ToolStripContentPanelGradientBegin As Color
            Get
                Return _c.Fond
            End Get
        End Property
        Public Overrides ReadOnly Property ToolStripContentPanelGradientEnd As Color
            Get
                Return _c.Fond
            End Get
        End Property
        Public Overrides ReadOnly Property StatusStripGradientBegin As Color
            Get
                Return _c.Surface
            End Get
        End Property
        Public Overrides ReadOnly Property StatusStripGradientEnd As Color
            Get
                Return _c.Surface
            End Get
        End Property
        Public Overrides ReadOnly Property ButtonSelectedGradientBegin As Color
            Get
                Return _c.SecondaireSurvol
            End Get
        End Property
        Public Overrides ReadOnly Property ButtonSelectedGradientMiddle As Color
            Get
                Return _c.SecondaireSurvol
            End Get
        End Property
        Public Overrides ReadOnly Property ButtonSelectedGradientEnd As Color
            Get
                Return _c.SecondaireSurvol
            End Get
        End Property
        Public Overrides ReadOnly Property ButtonPressedGradientBegin As Color
            Get
                Return _c.SecondaireActif
            End Get
        End Property
        Public Overrides ReadOnly Property ButtonPressedGradientMiddle As Color
            Get
                Return _c.SecondaireActif
            End Get
        End Property
        Public Overrides ReadOnly Property ButtonPressedGradientEnd As Color
            Get
                Return _c.SecondaireActif
            End Get
        End Property
        Public Overrides ReadOnly Property CheckBackground As Color
            Get
                Return _c.Selection
            End Get
        End Property
        Public Overrides ReadOnly Property CheckSelectedBackground As Color
            Get
                Return _c.Selection
            End Get
        End Property
    End Class

End Class
