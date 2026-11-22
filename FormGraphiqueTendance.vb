Imports System.Drawing.Drawing2D
Imports System.Drawing.Text

' ══════════════════════════════════════════════════════════════════
'  FORM GRAPHIQUE DE TENDANCE — Centrale à Béton
'  ✅ 4 courbes temps réel : GR / CIM / EAU / ADJ
'  ✅ Zoom molette souris
'  ✅ Défilement automatique (fenêtre glissante)
'  ✅ Grille + axes + légende
'  ✅ Sélection courbe active par clic
'  ✅ Affichage valeur sous le curseur
'  ✅ Boutons : Pause / RAZ / Export CSV
'  ✅ Style SCADA gris industriel cohérent avec Synoptique
' ══════════════════════════════════════════════════════════════════

Public Class FormGraphiqueTendance
    Inherits System.Windows.Forms.Form

    ' ── Timers ──────────────────────────────────────────────────
    Private _timerData As New Timer()     ' 1000ms — acquisition donnée
    Private _timerDraw As New Timer()     ' 100ms  — redessiner

    ' ── Canvas ──────────────────────────────────────────────────
    Private _canvas As PictureBox

    ' ── Paramètres graphique ────────────────────────────────────
    Private Const MAX_POINTS As Integer = 300       ' points max conservés
    Private Const FENETRE_VISIBLE As Integer = 60   ' points visibles par défaut
    Private _zoom As Double = 1.0                   ' 0.5 à 4.0
    Private _enPause As Boolean = False
    Private _cursorX As Integer = -1                ' position souris sur canvas
    Private _courbeActive As Integer = -1           ' -1 = toutes

    ' ── Données courbes ─────────────────────────────────────────
    '  0 = Granulats  1 = Ciment  2 = Eau  3 = Adjuvants
    Private _donnees(3)() As PointF
    Private _nbPts(3) As Integer
    Private _temps(3)() As DateTime

    Private _noms() As String = {"Granulats (kg)", "Ciment (kg)", "Eau (L)", "Adjuvants (kg)"}
    Private _couleurs() As Color = {
        Color.FromArgb(255, 180, 0),
        Color.FromArgb(100, 200, 255),
        Color.FromArgb(80, 220, 120),
        Color.FromArgb(255, 100, 160)
    }
    Private _epaisseurs() As Single = {2.5, 2.5, 2.5, 2.0}
    Private _visibles() As Boolean = {True, True, True, True}

    ' ── Consignes (lignes pointillées) ──────────────────────────
    Private _consignes() As Single = {1200, 350, 180, 5}

    ' ── Valeurs actuelles simulées ───────────────────────────────
    Private _vActuelles() As Single = {0, 0, 0, 0}
    Private _vMax() As Single = {1500, 500, 250, 10}
    Private _rng As New Random()
    Private _secondes As Integer = 0

    ' ── Palette couleurs ────────────────────────────────────────
    Private ReadOnly C_FOND As Color = Color.FromArgb(18, 22, 35)
    Private ReadOnly C_FOND_GRAPH As Color = Color.FromArgb(12, 15, 26)
    Private ReadOnly C_GRILLE As Color = Color.FromArgb(40, 50, 75)
    Private ReadOnly C_AXE As Color = Color.FromArgb(80, 95, 130)
    Private ReadOnly C_TEXTE As Color = Color.FromArgb(180, 195, 225)
    Private ReadOnly C_TEXTE2 As Color = Color.FromArgb(100, 120, 160)
    Private ReadOnly C_HEADER As Color = Color.FromArgb(35, 45, 80)
    Private ReadOnly C_BTN As Color = Color.FromArgb(50, 70, 130)
    Private ReadOnly C_BTN_HOVER As Color = Color.FromArgb(70, 100, 180)
    Private ReadOnly C_PANNEAU As Color = Color.FromArgb(25, 32, 55)

    ' ── Marges zone graphique ───────────────────────────────────
    Private _mG As Integer = 70   ' gauche (axe Y)
    Private _mD As Integer = 20   ' droite
    Private _mH As Integer = 50   ' haut
    Private _mB As Integer = 50   ' bas (axe X)
    Private _legendeH As Integer = 110  ' hauteur panneau légende bas

    ' ── Rectangles boutons ──────────────────────────────────────
    Private _btnPause As Rectangle
    Private _btnRaz As Rectangle
    Private _btnExport As Rectangle
    Private _btnZoomP As Rectangle
    Private _btnZoomM As Rectangle
    Private _btnFermer As Rectangle

    ' ════════════════════════════════════════════════════════════
    Public Sub New()
        ' Liaison avec Synoptique_V2 : passer les valeurs par propriétés publiques
        InitializeComponent_Manuel()
    End Sub

    Public Sub New(gr As Single, cim As Single, eau As Single, adj As Single)
        InitializeComponent_Manuel()
        ' Valeurs initiales depuis le synoptique
        _vActuelles(0) = gr
        _vActuelles(1) = cim
        _vActuelles(2) = eau
        _vActuelles(3) = adj
    End Sub

    ' ── Propriétés publiques pour mise à jour depuis Synoptique ─
    Public WriteOnly Property ValeurGranulats As Single
        Set(v As Single)
            _vActuelles(0) = v
        End Set
    End Property

    Public WriteOnly Property ValeurCiment As Single
        Set(v As Single)
            _vActuelles(1) = v
        End Set
    End Property

    Public WriteOnly Property ValeurEau As Single
        Set(v As Single)
            _vActuelles(2) = v
        End Set
    End Property

    Public WriteOnly Property ValeurAdjuvants As Single
        Set(v As Single)
            _vActuelles(3) = v
        End Set
    End Property

    Public WriteOnly Property ConsigneGranulats As Single
        Set(v As Single)
            _consignes(0) = v
        End Set
    End Property

    Public WriteOnly Property ConsigneCiment As Single
        Set(v As Single)
            _consignes(1) = v
        End Set
    End Property

    Public WriteOnly Property ConsigneEau As Single
        Set(v As Single)
            _consignes(2) = v
        End Set
    End Property

    ' ════════════════════════════════════════════════════════════
    Private Sub InitializeComponent_Manuel()
        Me.Text = "Graphiques de Tendance — Centrale à Béton"
        Me.Size = New Size(1200, 720)
        Me.MinimumSize = New Size(900, 550)
        Me.BackColor = C_FOND
        Me.StartPosition = FormStartPosition.CenterScreen

        ' Double-buffering
        Dim pi As System.Reflection.PropertyInfo =
            GetType(Form).GetProperty("DoubleBuffered",
                System.Reflection.BindingFlags.NonPublic Or
                System.Reflection.BindingFlags.Instance)
        pi.SetValue(Me, True, Nothing)

        ' Initialiser tableaux de données
        For i As Integer = 0 To 3
            ReDim _donnees(i)(MAX_POINTS - 1)
            ReDim _temps(i)(MAX_POINTS - 1)
            _nbPts(i) = 0
        Next

        ' Canvas
        _canvas = New PictureBox() With {.Dock = DockStyle.Fill, .BackColor = C_FOND}
        Dim pi2 As System.Reflection.PropertyInfo =
            GetType(Control).GetProperty("DoubleBuffered",
                System.Reflection.BindingFlags.NonPublic Or
                System.Reflection.BindingFlags.Instance)
        pi2.SetValue(_canvas, True, Nothing)

        AddHandler _canvas.Paint, AddressOf Dessiner
        AddHandler _canvas.MouseMove, AddressOf CanvasMouseMove
        AddHandler _canvas.MouseClick, AddressOf CanvasClick
        AddHandler _canvas.MouseWheel, AddressOf CanvasMouseWheel
        Me.Controls.Add(_canvas)

        ' Timer données (simule lecture PLC/SQL toutes les secondes)
        _timerData.Interval = 1000
        AddHandler _timerData.Tick, AddressOf AcquerirDonnee
        _timerData.Start()

        ' Timer dessin
        _timerDraw.Interval = 100
        AddHandler _timerDraw.Tick, Sub() _canvas.Invalidate()
        _timerDraw.Start()
    End Sub

    Private Sub FormGraphiqueTendance_FormClosing(sender As Object,
            e As FormClosingEventArgs) Handles MyBase.FormClosing
        _timerData.Stop()
        _timerDraw.Stop()
    End Sub

    ' ════════════════════════════════════════════════════════════
    '  ACQUISITION DONNÉES (simulation — remplacer par lecture SQL)
    ' ════════════════════════════════════════════════════════════
    Private Sub AcquerirDonnee(sender As Object, e As EventArgs)
        If _enPause Then Return
        _secondes += 1

        ' ── Simuler une gâchée réaliste ──────────────────────────
        Dim phase As Integer = _secondes Mod 90   ' cycle de 90 sec
        Dim grTarget As Single = If(phase < 30, phase * 40.0F,
                                    If(phase < 60, 1200.0F, 1200.0F - (phase - 60) * 40.0F))
        Dim cimTarget As Single = If(phase < 40, 0,
                                      If(phase < 60, (phase - 40) * 17.5F, 350.0F))
        Dim eauTarget As Single = If(phase < 50, 0,
                                      If(phase < 65, (phase - 50) * 12.0F, 180.0F))
        Dim adjTarget As Single = If(phase < 55, 0, If(phase < 65, (phase - 55) * 0.5F, 5.0F))

        ' Ajout bruit réaliste
        _vActuelles(0) = Math.Max(0, grTarget + CSng(_rng.Next(-15, 15)))
        _vActuelles(1) = Math.Max(0, cimTarget + CSng(_rng.Next(-5, 5)))
        _vActuelles(2) = Math.Max(0, eauTarget + CSng(_rng.Next(-3, 3)))
        _vActuelles(3) = Math.Max(0, adjTarget + CSng(_rng.NextDouble() * 0.4 - 0.2))

        ' ── Enregistrer le point ──────────────────────────────────
        For i As Integer = 0 To 3
            If _nbPts(i) < MAX_POINTS Then
                _donnees(i)(_nbPts(i)) = New PointF(_secondes, _vActuelles(i))
                _temps(i)(_nbPts(i)) = DateTime.Now
                _nbPts(i) += 1
            Else
                ' Décaler (buffer circulaire)
                Array.Copy(_donnees(i), 1, _donnees(i), 0, MAX_POINTS - 1)
                Array.Copy(_temps(i), 1, _temps(i), 0, MAX_POINTS - 1)
                _donnees(i)(MAX_POINTS - 1) = New PointF(_secondes, _vActuelles(i))
                _temps(i)(MAX_POINTS - 1) = DateTime.Now
            End If
        Next
    End Sub

    ' ════════════════════════════════════════════════════════════
    '  DESSIN PRINCIPAL
    ' ════════════════════════════════════════════════════════════
    Private Sub Dessiner(sender As Object, e As PaintEventArgs)
        Dim g As Graphics = e.Graphics
        g.SmoothingMode = SmoothingMode.AntiAlias
        g.TextRenderingHint = TextRenderingHint.ClearTypeGridFit

        Dim W As Integer = _canvas.Width
        Dim H As Integer = _canvas.Height

        g.Clear(C_FOND)

        ' Zone graphique
        Dim gX As Integer = _mG
        Dim gY As Integer = _mH + 40    ' +40 pour header
        Dim gW As Integer = W - _mG - _mD
        Dim gH As Integer = H - gY - _mB - _legendeH

        If gW < 100 OrElse gH < 50 Then Return

        ' ── HEADER ────────────────────────────────────────────────
        DessinerHeader(g, W)

        ' ── FOND GRAPHIQUE ────────────────────────────────────────
        g.FillRectangle(New SolidBrush(C_FOND_GRAPH), gX, gY, gW, gH)

        ' ── GRILLE + AXES ─────────────────────────────────────────
        DessinerGrille(g, gX, gY, gW, gH)

        ' ── COURBES ───────────────────────────────────────────────
        DessinerCourbes(g, gX, gY, gW, gH)

        ' ── CURSEUR SOURIS ────────────────────────────────────────
        If _cursorX >= gX AndAlso _cursorX <= gX + gW Then
            DessinerCurseur(g, gX, gY, gW, gH)
        End If

        ' ── CADRE GRAPHIQUE ───────────────────────────────────────
        g.DrawRectangle(New Pen(C_AXE, 1.5), gX, gY, gW, gH)

        ' ── LÉGENDE BAS ───────────────────────────────────────────
        DessinerLegende(g, W, H, gH + gY + _mB)

        ' ── BOUTONS ───────────────────────────────────────────────
        DessinerBoutons(g, W)

        ' ── VALEURS EN TEMPS RÉEL (droite) ────────────────────────
        DessinerValeursActuelles(g, W, gY)
    End Sub

    ' ════════════════════════════════════════════════════════════
    '  HEADER
    ' ════════════════════════════════════════════════════════════
    Private Sub DessinerHeader(g As Graphics, W As Integer)
        Using br As New LinearGradientBrush(New Rectangle(0, 0, W, 40),
            C_HEADER, Color.FromArgb(20, 28, 55), LinearGradientMode.Vertical)
            g.FillRectangle(br, 0, 0, W, 40)
        End Using
        g.DrawLine(New Pen(Color.FromArgb(60, 90, 160), 1.5), 0, 40, W, 40)

        Using fnt As New Font("Arial", 12, FontStyle.Bold)
            g.DrawString("📊  GRAPHIQUES DE TENDANCE — CENTRALE À BÉTON", fnt,
                New SolidBrush(Color.FromArgb(200, 215, 255)), 12.0F, 10.0F)
        End Using

        ' Heure + statut
        Using fnt As New Font("Arial", 8)
            Dim statut As String = If(_enPause, "⏸ PAUSE", "● ACQUISITION EN COURS")
            Dim coulStat As Color = If(_enPause, Color.FromArgb(255, 160, 0), Color.FromArgb(80, 220, 100))
            Dim sz As SizeF = g.MeasureString(statut, fnt)
            g.DrawString(statut, fnt, New SolidBrush(coulStat), W - sz.Width - 150, 14.0F)
            g.DrawString(DateTime.Now.ToString("ddd dd/MM/yyyy   HH:mm:ss"),
                fnt, New SolidBrush(C_TEXTE2), W - 145.0F, 14.0F)
        End Using
    End Sub

    ' ════════════════════════════════════════════════════════════
    '  GRILLE + AXES
    ' ════════════════════════════════════════════════════════════
    Private Sub DessinerGrille(g As Graphics, gX As Integer, gY As Integer,
                                gW As Integer, gH As Integer)

        Dim nbLignesY As Integer = 6
        Dim nbLignesX As Integer = 8

        ' Lignes horizontales
        Using pen As New Pen(C_GRILLE, 1)
            pen.DashStyle = DashStyle.Dot
            For i As Integer = 0 To nbLignesY
                Dim y As Integer = gY + CInt(i * gH / nbLignesY)
                g.DrawLine(pen, gX, y, gX + gW, y)

                ' Valeur Y (axe gauche — valeur max visible)
                Dim vMax As Single = ValeurYMax()
                Dim val As Single = vMax - (i / CSng(nbLignesY)) * vMax
                Using fnt As New Font("Arial", 7)
                    Dim txt As String = val.ToString("0")
                    Dim sz As SizeF = g.MeasureString(txt, fnt)
                    g.DrawString(txt, fnt, New SolidBrush(C_TEXTE2),
                        CSng(gX) - sz.Width - 5, CSng(y) - sz.Height / 2.0F)
                End Using
            Next
        End Using

        ' Lignes verticales (temps)
        Using pen As New Pen(C_GRILLE, 1)
            pen.DashStyle = DashStyle.Dot
            For i As Integer = 0 To nbLignesX
                Dim x As Integer = gX + CInt(i * gW / nbLignesX)
                g.DrawLine(pen, x, gY, x, gY + gH)

                ' Label temps
                Dim ptIdx As Integer = CInt(i * FenetreVisible() / nbLignesX)
                Dim tStr As String = "-" & (FenetreVisible() - ptIdx).ToString("0") & "s"
                Using fnt As New Font("Arial", 7)
                    Dim sz As SizeF = g.MeasureString(tStr, fnt)
                    g.DrawString(tStr, fnt, New SolidBrush(C_TEXTE2),
                        CSng(x) - sz.Width / 2.0F, CSng(gY + gH) + 4.0F)
                End Using
            Next
        End Using

        ' Label axe Y
        Using fnt As New Font("Arial", 8, FontStyle.Bold)
            Dim txt As String = "Valeur (kg / L)"
            Dim sz As SizeF = g.MeasureString(txt, fnt)
            ' Rotation texte axe Y
            g.TranslateTransform(12, CSng(gY + gH / 2))
            g.RotateTransform(-90)
            g.DrawString(txt, fnt, New SolidBrush(C_TEXTE2), -sz.Width / 2.0F, 0)
            g.ResetTransform()
        End Using

        ' Label axe X
        Using fnt As New Font("Arial", 8)
            Dim txt As String = "Temps (secondes)"
            Dim sz As SizeF = g.MeasureString(txt, fnt)
            g.DrawString(txt, fnt, New SolidBrush(C_TEXTE2),
                CSng(gX + gW / 2) - sz.Width / 2.0F, CSng(gY + gH + 32))
        End Using
    End Sub

    ' ════════════════════════════════════════════════════════════
    '  COURBES
    ' ════════════════════════════════════════════════════════════
    Private Sub DessinerCourbes(g As Graphics, gX As Integer, gY As Integer,
                                  gW As Integer, gH As Integer)

        Dim vMax As Single = ValeurYMax()
        Dim fenetre As Integer = FenetreVisible()

        For c As Integer = 0 To 3
            If Not _visibles(c) Then Continue For
            If _nbPts(c) < 2 Then Continue For

            ' Déterminer les points à afficher (derniers `fenetre` points)
            Dim debut As Integer = Math.Max(0, _nbPts(c) - fenetre)
            Dim nb As Integer = _nbPts(c) - debut

            ' ── Remplissage aire sous la courbe (semi-transparent) ──
            Dim ptsFill(nb + 1) As PointF
            For i As Integer = 0 To nb - 1
                Dim idx As Integer = debut + i
                Dim px As Single = CSng(gX) + CSng(i) * gW / Math.Max(fenetre - 1, 1)
                Dim py As Single = CSng(gY + gH) - (_donnees(c)(idx).Y / vMax) * gH
                ptsFill(i) = New PointF(px, py)
            Next
            ptsFill(nb) = New PointF(ptsFill(nb - 1).X, gY + gH)
            ptsFill(nb + 1) = New PointF(gX, gY + gH)

            If nb >= 2 Then
                Try
                    Using br As New SolidBrush(Color.FromArgb(25, _couleurs(c)))
                        g.FillPolygon(br, ptsFill)
                    End Using
                Catch : End Try
            End If

            ' ── Ligne consigne (pointillée) ──────────────────────────
            Dim consY As Single = CSng(gY + gH) - (_consignes(c) / vMax) * gH
            If consY >= gY AndAlso consY <= gY + gH Then
                Using pen As New Pen(Color.FromArgb(100, _couleurs(c)), 1)
                    pen.DashStyle = DashStyle.Dash
                    g.DrawLine(pen, gX, consY, gX + gW, consY)
                End Using
                Using fnt As New Font("Arial", 6)
                    g.DrawString("Cons. " & _consignes(c).ToString("0"),
                        fnt, New SolidBrush(Color.FromArgb(140, _couleurs(c))),
                        CSng(gX + gW - 60), consY - 10)
                End Using
            End If

            ' ── Courbe principale ─────────────────────────────────────
            Dim alpha As Integer = If(_courbeActive = -1 OrElse _courbeActive = c, 255, 60)
            Dim epaisseur As Single = If(_courbeActive = c, _epaisseurs(c) + 1.5F, _epaisseurs(c))
            Using pen As New Pen(Color.FromArgb(alpha, _couleurs(c)), epaisseur)
                pen.LineJoin = LineJoin.Round

                Dim pts(nb - 1) As PointF
                For i As Integer = 0 To nb - 1
                    Dim idx As Integer = debut + i
                    pts(i).X = CSng(gX) + CSng(i) * gW / Math.Max(fenetre - 1, 1)
                    pts(i).Y = CSng(gY + gH) - (_donnees(c)(idx).Y / vMax) * gH
                    ' Clamp dans la zone graphique
                    pts(i).Y = Math.Max(gY, Math.Min(gY + gH, pts(i).Y))
                Next

                If pts.Length >= 2 Then
                    Try
                        g.DrawLines(pen, pts)
                    Catch : End Try
                End If

                ' Point final (valeur actuelle)
                If pts.Length > 0 Then
                    Dim dernX As Single = pts(pts.Length - 1).X
                    Dim dernY As Single = pts(pts.Length - 1).Y
                    g.FillEllipse(New SolidBrush(Color.FromArgb(alpha, _couleurs(c))),
                        dernX - 5, dernY - 5, 10, 10)
                    g.DrawEllipse(New Pen(Color.FromArgb(alpha, Color.White), 1.5),
                        dernX - 5, dernY - 5, 10, 10)

                    ' Valeur actuelle à droite du point
                    Using fnt As New Font("Consolas", 8, FontStyle.Bold)
                        g.DrawString(_vActuelles(c).ToString("0.0"),
                            fnt, New SolidBrush(Color.FromArgb(alpha, _couleurs(c))),
                            dernX + 8, dernY - 8)
                    End Using
                End If
            End Using
        Next
    End Sub

    ' ════════════════════════════════════════════════════════════
    '  CURSEUR SOURIS (ligne verticale + tooltip valeurs)
    ' ════════════════════════════════════════════════════════════
    Private Sub DessinerCurseur(g As Graphics, gX As Integer, gY As Integer,
                                  gW As Integer, gH As Integer)

        ' Ligne verticale
        Using pen As New Pen(Color.FromArgb(120, Color.White), 1)
            pen.DashStyle = DashStyle.Dot
            g.DrawLine(pen, _cursorX, gY, _cursorX, gY + gH)
        End Using

        ' Tooltip
        Dim vMax As Single = ValeurYMax()
        Dim fenetre As Integer = FenetreVisible()
        Dim ratio As Double = (_cursorX - gX) / CDbl(gW)
        Dim idxCursor As Integer = CInt(ratio * fenetre)

        ' Fond tooltip
        Dim ttX As Integer = _cursorX + 10
        Dim ttY As Integer = gY + 10
        Dim ttW As Integer = 160
        Dim ttH As Integer = 15 + 4 * 18

        If ttX + ttW > gX + gW Then ttX = _cursorX - ttW - 10

        g.FillRectangle(New SolidBrush(Color.FromArgb(220, C_PANNEAU)), ttX, ttY, ttW, ttH)
        g.DrawRectangle(New Pen(C_AXE, 1), ttX, ttY, ttW, ttH)

        Using fntT As New Font("Consolas", 7, FontStyle.Bold)
            Using fntL As New Font("Arial", 7)
                ' Heure
                g.DrawString("t - " & (fenetre - idxCursor).ToString("0") & " sec",
                    fntL, New SolidBrush(C_TEXTE2), ttX + 6, ttY + 3)

                For c As Integer = 0 To 3
                    If Not _visibles(c) Then Continue For
                    Dim debut As Integer = Math.Max(0, _nbPts(c) - fenetre)
                    Dim localIdx As Integer = Math.Min(debut + idxCursor, _nbPts(c) - 1)
                    Dim valCursor As Single = If(localIdx >= 0 AndAlso localIdx < _nbPts(c),
                        _donnees(c)(localIdx).Y, 0)

                    ' Rond couleur
                    g.FillEllipse(New SolidBrush(_couleurs(c)),
                        ttX + 6, ttY + 16 + c * 18, 8, 8)
                    g.DrawString(_noms(c).Split("("c)(0).Trim() & " : " &
                        valCursor.ToString("0.0"),
                        fntT, New SolidBrush(Color.FromArgb(200, _couleurs(c))),
                        ttX + 20, ttY + 14 + c * 18)
                Next
            End Using
        End Using
    End Sub

    ' ════════════════════════════════════════════════════════════
    '  LÉGENDE
    ' ════════════════════════════════════════════════════════════
    Private Sub DessinerLegende(g As Graphics, W As Integer, H As Integer, legY As Integer)

        Dim legX As Integer = _mG
        Dim legW As Integer = W - _mG - _mD
        Dim legH As Integer = _legendeH - 10

        g.FillRectangle(New SolidBrush(C_PANNEAU), legX, legY, legW, legH)
        g.DrawRectangle(New Pen(C_AXE, 1), legX, legY, legW, legH)

        Dim colW As Integer = legW \ 4

        For c As Integer = 0 To 3
            Dim cx As Integer = legX + c * colW + 10
            Dim cy As Integer = legY + 8

            ' Bouton toggle visibilité
            Dim btnVis As New Rectangle(cx, cy, colW - 20, 22)
            Dim alpha As Integer = If(_visibles(c), 220, 60)

            Using br As New LinearGradientBrush(btnVis,
                Color.FromArgb(alpha, Color.FromArgb(40, 50, 80)),
                Color.FromArgb(alpha, Color.FromArgb(25, 32, 55)),
                LinearGradientMode.Vertical)
                g.FillRectangle(br, btnVis)
            End Using
            g.DrawRectangle(New Pen(Color.FromArgb(alpha, _couleurs(c)), 1.5), btnVis)

            ' Ligne démo
            Using pen As New Pen(Color.FromArgb(alpha, _couleurs(c)), 2)
                g.DrawLine(pen, cx + 6, cy + 11, cx + 24, cy + 11)
            End Using
            ' Point
            g.FillEllipse(New SolidBrush(Color.FromArgb(alpha, _couleurs(c))),
                cx + 12, cy + 7, 8, 8)

            Using fnt As New Font("Arial", 7, FontStyle.Bold)
                g.DrawString(_noms(c), fnt,
                    New SolidBrush(Color.FromArgb(alpha, C_TEXTE)),
                    cx + 28, cy + 4)
            End Using

            ' Valeur actuelle
            Dim vy As Integer = cy + 28
            Using fntV As New Font("Consolas", 11, FontStyle.Bold)
                g.DrawString(_vActuelles(c).ToString("0.0"),
                    fntV, New SolidBrush(Color.FromArgb(alpha, _couleurs(c))),
                    cx + 6, vy)
            End Using
            Using fntU As New Font("Arial", 7)
                Dim unite As String = If(c = 2, "L", "kg")
                g.DrawString(unite, fntU, New SolidBrush(C_TEXTE2), cx + 6 + 55, vy + 4)
            End Using

            ' Écart consigne
            Dim ecart As Single = _vActuelles(c) - _consignes(c)
            Dim coulEcart As Color = If(Math.Abs(ecart) < 5, Color.FromArgb(80, 220, 120),
                If(Math.Abs(ecart) < 20, Color.FromArgb(255, 200, 0), Color.FromArgb(255, 80, 80)))
            Using fntE As New Font("Arial", 7)
                Dim signe As String = If(ecart >= 0, "+", "")
                g.DrawString("Écart : " & signe & ecart.ToString("0.0"),
                    fntE, New SolidBrush(Color.FromArgb(alpha, coulEcart)),
                    cx + 6, vy + 22)
            End Using

            ' Consigne
            Using fntC As New Font("Arial", 6)
                g.DrawString("Cons. : " & _consignes(c).ToString("0"),
                    fntC, New SolidBrush(C_TEXTE2), cx + 6, vy + 38)
            End Using
        Next
    End Sub

    ' ════════════════════════════════════════════════════════════
    '  VALEURS TEMPS RÉEL (panneau droite)
    ' ════════════════════════════════════════════════════════════
    Private Sub DessinerValeursActuelles(g As Graphics, W As Integer, gY As Integer)
        Dim panW As Integer = 0    ' pas de panneau séparé — valeurs dans légende
        ' (Réservé pour extension future)
    End Sub

    ' ════════════════════════════════════════════════════════════
    '  BOUTONS
    ' ════════════════════════════════════════════════════════════
    Private Sub DessinerBoutons(g As Graphics, W As Integer)
        Dim btnY As Integer = 6
        Dim btnH As Integer = 28
        Dim btnW As Integer = 90

        ' Calculer positions
        Dim startX As Integer = W - 6 * (btnW + 6) - 10

        _btnPause = New Rectangle(startX, btnY, btnW, btnH)
        _btnRaz = New Rectangle(startX + btnW + 6, btnY, btnW, btnH)
        _btnExport = New Rectangle(startX + (btnW + 6) * 2, btnY, btnW + 20, btnH)
        _btnZoomP = New Rectangle(startX + (btnW + 6) * 3 + 20, btnY, 40, btnH)
        _btnZoomM = New Rectangle(startX + (btnW + 6) * 3 + 64, btnY, 40, btnH)
        _btnFermer = New Rectangle(startX + (btnW + 6) * 3 + 110, btnY, btnW, btnH)

        ' Dessiner boutons
        Dim btns() As (Rectangle, String, Color) = {
            (_btnPause, If(_enPause, "▶ REPRENDRE", "⏸ PAUSE"),
             If(_enPause, Color.FromArgb(200, 100, 0), Color.FromArgb(50, 80, 150))),
            (_btnRaz, "🗑 RAZ", Color.FromArgb(120, 40, 40)),
            (_btnExport, "💾 EXPORT CSV", Color.FromArgb(40, 100, 60)),
            (_btnZoomP, "🔍+", Color.FromArgb(60, 80, 130)),
            (_btnZoomM, "🔍-", Color.FromArgb(60, 80, 130)),
            (_btnFermer, "✖ FERMER", Color.FromArgb(100, 30, 30))
        }

        For Each btn In btns
            Using br As New LinearGradientBrush(btn.Item1,
                ControlPaint.Light(btn.Item3, 0.2),
                btn.Item3, LinearGradientMode.Vertical)
                g.FillRectangle(br, btn.Item1)
            End Using
            g.DrawRectangle(New Pen(Color.FromArgb(80, Color.White), 1), btn.Item1)
            g.DrawLine(New Pen(Color.FromArgb(60, Color.White), 1),
                btn.Item1.Left + 1, btn.Item1.Top + 1,
                btn.Item1.Right - 1, btn.Item1.Top + 1)
            Using fnt As New Font("Arial", 7, FontStyle.Bold)
                Dim sz As SizeF = g.MeasureString(btn.Item2, fnt)
                g.DrawString(btn.Item2, fnt, Brushes.White,
                    CSng(btn.Item1.X) + (btn.Item1.Width - sz.Width) / 2.0F,
                    CSng(btn.Item1.Y) + (btn.Item1.Height - sz.Height) / 2.0F)
            End Using
        Next

        ' Zoom niveau
        Using fnt As New Font("Arial", 7)
            g.DrawString("x" & _zoom.ToString("0.0"),
                fnt, New SolidBrush(C_TEXTE2),
                CSng(_btnZoomM.Right + 4), CSng(btnY + 8))
        End Using
    End Sub

    ' ════════════════════════════════════════════════════════════
    '  ÉVÉNEMENTS SOURIS
    ' ════════════════════════════════════════════════════════════
    Private Sub CanvasMouseMove(sender As Object, e As MouseEventArgs)
        _cursorX = e.X
        _canvas.Invalidate()
    End Sub

    Private Sub CanvasClick(sender As Object, e As MouseEventArgs)
        ' Boutons header
        If _btnPause.Contains(e.Location) Then
            _enPause = Not _enPause : Return
        End If
        If _btnRaz.Contains(e.Location) Then
            For i As Integer = 0 To 3 : _nbPts(i) = 0 : Next
            _secondes = 0 : Return
        End If
        If _btnExport.Contains(e.Location) Then
            ExporterCSV() : Return
        End If
        If _btnZoomP.Contains(e.Location) Then
            _zoom = Math.Min(4.0, _zoom + 0.25) : Return
        End If
        If _btnZoomM.Contains(e.Location) Then
            _zoom = Math.Max(0.25, _zoom - 0.25) : Return
        End If
        If _btnFermer.Contains(e.Location) Then
            Me.Close() : Return
        End If

        ' Clic sur légende — toggle visibilité courbe
        Dim gX As Integer = _mG
        Dim gW As Integer = _canvas.Width - _mG - _mD
        Dim gH As Integer = _canvas.Height - (_mH + 40) - _mB - _legendeH
        Dim legY As Integer = _mH + 40 + gH + _mB
        Dim colW As Integer = gW \ 4

        If e.Y >= legY AndAlso e.Y <= legY + _legendeH - 10 Then
            For c As Integer = 0 To 3
                Dim cx As Integer = gX + c * colW + 10
                Dim btnVis As New Rectangle(cx, legY + 8, colW - 20, 22)
                If btnVis.Contains(e.Location) Then
                    _visibles(c) = Not _visibles(c)
                    If e.Button = MouseButtons.Right Then _courbeActive = c
                    Return
                End If
            Next
        End If

        ' Clic droit → sélectionner courbe active
        If e.Button = MouseButtons.Right Then
            _courbeActive = If(_courbeActive = -1, 0, -1)
        End If
    End Sub

    Private Sub CanvasMouseWheel(sender As Object, e As MouseEventArgs)
        If e.Delta > 0 Then
            _zoom = Math.Min(4.0, _zoom + 0.25)
        Else
            _zoom = Math.Max(0.25, _zoom - 0.25)
        End If
    End Sub

    ' ════════════════════════════════════════════════════════════
    '  HELPERS
    ' ════════════════════════════════════════════════════════════

    ' Nombre de points visibles selon zoom
    Private Function FenetreVisible() As Integer
        Return CInt(FENETRE_VISIBLE / _zoom)
    End Function

    ' Valeur max affichée (auto-scale selon courbes visibles)
    Private Function ValeurYMax() As Single
        Dim vMax As Single = 100
        For c As Integer = 0 To 3
            If _visibles(c) Then
                vMax = Math.Max(vMax, _vMax(c))
            End If
        Next
        Return vMax
    End Function

    ' Export CSV
    Private Sub ExporterCSV()
        Try
            Dim chemin As String = System.IO.Path.Combine(
                Environment.GetFolderPath(Environment.SpecialFolder.Desktop),
                "tendance_" & DateTime.Now.ToString("yyyyMMdd_HHmmss") & ".csv")

            Using sw As New System.IO.StreamWriter(chemin, False, System.Text.Encoding.UTF8)
                sw.WriteLine("Heure;Granulats (kg);Ciment (kg);Eau (L);Adjuvants (kg)")
                Dim nb As Integer = _nbPts(0)
                For i As Integer = 0 To nb - 1
                    Dim ligne As String = _temps(0)(i).ToString("HH:mm:ss")
                    For c As Integer = 0 To 3
                        If i < _nbPts(c) Then
                            ligne &= ";" & _donnees(c)(i).Y.ToString("0.00")
                        Else
                            ligne &= ";0"
                        End If
                    Next
                    sw.WriteLine(ligne)
                Next
            End Using

            MessageBox.Show("Export réussi !" & vbCrLf & chemin,
                "Export CSV", MessageBoxButtons.OK, MessageBoxIcon.Information)
        Catch ex As Exception
            MessageBox.Show("Erreur export : " & ex.Message,
                "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

End Class