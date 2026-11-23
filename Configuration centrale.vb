Imports System.Windows.Forms

' ================================================================
'  Configuration_centrale.vb — 18/09/2026
'  Écran « Paramètres » de la centrale (copie de l'écran WinCC).
'  Les TextBox / onglets sont dessinés dans le Designer.
'  REAL (Petite début kg, Tolérance %, Zéro bascule kg) et
'  TIME (temps en millisecondes, DINT) lus/écrits directement dans les DB
'  (DB8, DB19, DB63, DB90 : adresses WinCC ; zéros bascules : DB1).
'  Boutons btnLire / btnEnregistrer / btnFermer et lblEtat : Designer.
' ================================================================
Public Class Configuration_centrale

    ' Type de la variable CPU : REAL (kg, %) ou TIME (DINT en millisecondes)
    Private Enum TypeVar
        Reel
        Temps
    End Enum

    ' Un paramètre = (TextBox, DB, offset, type, libellé)
    Private Structure Param
        Public Champ As TextBox
        Public Db As Integer
        Public Offset As Integer
        Public Genre As TypeVar
        Public Libelle As String
        Public Sub New(c As TextBox, db As Integer, off As Integer, t As TypeVar, libelleParam As String)
            Champ = c : Me.Db = db : Offset = off : Genre = t : Libelle = libelleParam
        End Sub
    End Structure
    Private Const VREAL As TypeVar = TypeVar.Reel
    Private Const VTIME As TypeVar = TypeVar.Temps

    Private _params As New List(Of Param)
    Private _occupe As Boolean = False
    Private _dernieresValeurs As Double() = Nothing

    ' ── Lecture groupée : une seule trame par DB pour toute une liste de paramètres ──
    Private Sub LireGroupe(liste As List(Of Param), valeurs() As Double, oks() As Boolean)
        Dim indexParDb As New Dictionary(Of Integer, List(Of Integer))
        For i As Integer = 0 To liste.Count - 1
            If Not indexParDb.ContainsKey(liste(i).Db) Then indexParDb(liste(i).Db) = New List(Of Integer)
            indexParDb(liste(i).Db).Add(i)
        Next
        For Each db In indexParDb.Keys
            Dim idx As List(Of Integer) = indexParDb(db)
            Dim minOff As Integer = idx.Min(Function(i) liste(i).Offset)
            Dim taille As Integer = idx.Max(Function(i) liste(i).Offset) + 4 - minOff
            Try
                Dim buf() As Byte = ModulePLCDuplex.LireBytesDB(db, minOff, taille)
                If buf Is Nothing OrElse buf.Length < taille Then
                    For Each i In idx : oks(i) = False : Next
                Else
                    For Each i In idx
                        Dim rel As Integer = liste(i).Offset - minOff
                        Dim inv() As Byte = {buf(rel + 3), buf(rel + 2), buf(rel + 1), buf(rel)}
                        oks(i) = True
                        If liste(i).Genre = TypeVar.Temps Then
                            valeurs(i) = BitConverter.ToInt32(inv, 0)
                        Else
                            Dim v As Single = BitConverter.ToSingle(inv, 0)
                            If Single.IsNaN(v) OrElse Single.IsInfinity(v) Then v = 0
                            valeurs(i) = v
                        End If
                    Next
                End If
            Catch
                For Each i In idx : oks(i) = False : Next
            End Try
        Next
    End Sub


    ' ── Table des adresses (WinCC « Paramètre » + « Time out ») ──
    Private Sub ConstruireTable()
        _params.Clear()
        ' Vis ciment 1
        _params.Add(New Param(txtVis1_PetiteDebut, 8, 88, VREAL, "Petite début vis ciment 1"))
        _params.Add(New Param(txtVis1_Tolerance, 8, 40, VREAL, "Tolérance vis ciment 1"))
        _params.Add(New Param(txtVis1_TempsOn, 8, 158, VTIME, "Temps On vis ciment 1"))
        _params.Add(New Param(txtVis1_TempsOff, 8, 182, VTIME, "Temps Off vis ciment 1"))
        ' Vis ciment 2 (petite début / tolérance / temps off : même adresse que vis 1 dans WinCC)
        '  _params.Add(New Param(txtVis2_PetiteDebut, 8, 88, VREAL, "Petite début vis ciment 2"))
        ' _params.Add(New Param(txtVis2_Tolerance, 8, 40, VREAL, "Tolérance vis ciment 2"))
        _params.Add(New Param(txtVis2_TempsOn, 8, 154, VTIME, "Temps On vis ciment 2"))
        _params.Add(New Param(txtVis2_TempsOff, 8, 178, VTIME, "Temps Off vis ciment 2"))
        ' Granulats G1 / SC / SL / D2 (= TR1..TR4)
        _params.Add(New Param(txtG1_PetiteDebut, 8, 72, VREAL, "Petite début G1"))
        _params.Add(New Param(txtG1_Tolerance, 8, 24, VREAL, "Tolérance G1"))
        _params.Add(New Param(txtG1_TempsOn, 8, 138, VTIME, "Temps On G1"))
        _params.Add(New Param(txtG1_TempsOff, 8, 162, VTIME, "Temps Off G1"))
        _params.Add(New Param(txtSC_PetiteDebut, 8, 76, VREAL, "Petite début SC"))
        _params.Add(New Param(txtSC_Tolerance, 8, 28, VREAL, "Tolérance SC"))
        _params.Add(New Param(txtSC_TempsOn, 8, 142, VTIME, "Temps On SC"))
        _params.Add(New Param(txtSC_TempsOff, 8, 166, VTIME, "Temps Off SC"))
        _params.Add(New Param(txtSL_PetiteDebut, 8, 80, VREAL, "Petite début SL"))
        _params.Add(New Param(txtSL_Tolerance, 8, 32, VREAL, "Tolérance SL"))
        _params.Add(New Param(txtSL_TempsOn, 8, 146, VTIME, "Temps On SL"))
        _params.Add(New Param(txtSL_TempsOff, 8, 170, VTIME, "Temps Off SL"))
        _params.Add(New Param(txtD2_PetiteDebut, 8, 84, VREAL, "Petite début D2"))
        _params.Add(New Param(txtD2_Tolerance, 8, 36, VREAL, "Tolérance D2"))
        _params.Add(New Param(txtD2_TempsOn, 8, 150, VTIME, "Temps On D2"))
        _params.Add(New Param(txtD2_TempsOff, 8, 174, VTIME, "Temps Off D2"))
        ' Temps
        _params.Add(New Param(txtT_RetardVidangeCIM, 8, 248, VTIME, "Temps retard vidange CIM"))
        _params.Add(New Param(txtT_RetardVidangeEAU, 8, 252, VTIME, "Temps retard vidange EAU"))
        _params.Add(New Param(txtT_MalaxageIMD, 8, 256, VTIME, "Temps malaxage IMD"))
        _params.Add(New Param(txtT_VidangeMalaxeur, 8, 260, VTIME, "Temps vidange malaxeur"))
        _params.Add(New Param(txtT_VidangeADJ, 90, 28, VTIME, "Temps vidange bascule ADJ"))
        _params.Add(New Param(txtT_VidangeSkip, 8, 264, VTIME, "Temps vidange skip"))
        _params.Add(New Param(txtT_PositionAttenteSkip, 19, 106, VTIME, "Position attente skip"))
        ' Tolérances
        _params.Add(New Param(txtTol_EAU, 8, 44, VREAL, "Tolérance eau"))
        _params.Add(New Param(txtTol_ADJ1, 8, 334, VREAL, "Tolérance adj 1"))
        _params.Add(New Param(txtTol_ADJ2, 8, 338, VREAL, "Tolérance adj 2"))
        _params.Add(New Param(txtTol_ADJ3, 8, 342, VREAL, "Tolérance adj 3"))
        _params.Add(New Param(txtTol_ADJ4, 8, 346, VREAL, "Tolérance adj 4"))
        ' Zéros bascules (déjà dans DB1)
        _params.Add(New Param(txtZero_AGR, 1, 1052, VREAL, "Zéro bascule AGR"))
        _params.Add(New Param(txtZero_CIM, 1, 1056, VREAL, "Zéro bascule CIM"))
        _params.Add(New Param(txtZero_EAU, 1, 1060, VREAL, "Zéro bascule EAU"))
        _params.Add(New Param(txtZero_ADJ, 1, 1064, VREAL, "Zéro bascule ADJ"))
        ' ADJ
        ' Time out dosage
        _params.Add(New Param(txtTO_TR1, 63, 0, VTIME, "Time out dosage TR1"))
        _params.Add(New Param(txtTO_TR2, 63, 4, VTIME, "Time out dosage TR2"))
        _params.Add(New Param(txtTO_TR3, 63, 8, VTIME, "Time out dosage TR3"))
        _params.Add(New Param(txtTO_TR4, 63, 12, VTIME, "Time out dosage TR4"))
        _params.Add(New Param(txtTO_CIM, 63, 16, VTIME, "Time out dosage CIM"))
        _params.Add(New Param(txtTO_EAU, 63, 20, VTIME, "Time out dosage EAU"))
        _params.Add(New Param(txtTO_ADJ1, 63, 24, VTIME, "Time out dosage ADJ1"))
        _params.Add(New Param(txtTO_ADJ2, 63, 28, VTIME, "Time out dosage ADJ2"))
        _params.Add(New Param(txtTO_ADJ3, 63, 32, VTIME, "Time out dosage ADJ3"))
        _params.Add(New Param(txtTO_ADJ4, 63, 36, VTIME, "Time out dosage ADJ4"))
    End Sub

    Private Sub Configuration_centrale_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        ConstruireTable()
        For Each p In _params
            p.Champ.TextAlign = HorizontalAlignment.Right
        Next
        LireCPU()
    End Sub

    Private Sub btnLire_Click(sender As Object, e As EventArgs) Handles btnLire.Click
        LireCPU()
    End Sub

    Private Sub btnFermer_Click(sender As Object, e As EventArgs) Handles btnFermer.Click
        Me.Close()
    End Sub

    ' ── Lecture d'une valeur (REAL ou TIME = DINT ms) dans n'importe quel DB (Big Endian)
    Private Function LireValeur(p As Param, ByRef ok As Boolean) As Double
        ok = False
        Try
            Dim b() As Byte = ModulePLCDuplex.LireBytesDB(p.Db, p.Offset, 4)
            If b Is Nothing OrElse b.Length < 4 Then Return 0
            Dim inv() As Byte = {b(3), b(2), b(1), b(0)}
            ok = True
            If p.Genre = TypeVar.Temps Then
                Return BitConverter.ToInt32(inv, 0)          ' millisecondes
            Else
                Dim v As Single = BitConverter.ToSingle(inv, 0)
                If Single.IsNaN(v) OrElse Single.IsInfinity(v) Then v = 0
                Return v
            End If
        Catch
            Return 0
        End Try
    End Function

    ' ── Écriture d'une valeur (REAL ou TIME) ────────────────────────
    Private Function EcrireValeur(p As Param, valeur As Double) As Boolean
        If p.Genre = TypeVar.Temps Then
            Return ModulePLCDuplex.EcrireDInt(p.Db, p.Offset, CInt(Math.Round(valeur)))
        Else
            Return ModulePLCDuplex.EcrireReal(p.Db, p.Offset, CSng(valeur))
        End If
    End Function

    Private Function Formater(p As Param, v As Double) As String
        If p.Genre = TypeVar.Temps Then
            Return (v / 1000).ToString("0.000", Globalization.CultureInfo.InvariantCulture).Replace(".", ",")
        End If
        Return v.ToString("0.###")
    End Function

    ' ── LIRE CPU (en arrière-plan) ────────────────────────────────
    Private Sub LireCPU()
        If _occupe Then Exit Sub
        If Not PLCData.Connecte Then
            lblEtat.Text = "CPU non connecté : " & PLCData.DerniereErreur
            lblEtat.ForeColor = Color.Red
            AfficherModeDemarrage()          ' ← AJOUT
            Exit Sub
        End If
        _occupe = True
        lblEtat.Text = "Lecture CPU..." : lblEtat.ForeColor = Color.DarkOrange
        btnLire.Enabled = False : btnEnregistrer.Enabled = False
        Dim liste As List(Of Param) = _params
        Threading.ThreadPool.QueueUserWorkItem(
            Sub(state)
                Dim valeurs(liste.Count - 1) As Double
                Dim oks(liste.Count - 1) As Boolean
                LireGroupe(liste, valeurs, oks)
                Try
                    Me.BeginInvoke(
                        Sub()
                            Dim nbErr As Integer = 0
                            For i As Integer = 0 To liste.Count - 1
                                If oks(i) Then
                                    liste(i).Champ.Text = Formater(liste(i), valeurs(i))
                                    liste(i).Champ.BackColor = Color.White
                                Else
                                    liste(i).Champ.BackColor = Color.MistyRose
                                    nbErr += 1
                                End If
                            Next
                            If nbErr = 0 Then
                                lblEtat.Text = "Paramètres lus à " & Now.ToString("HH:mm:ss")
                                lblEtat.ForeColor = Color.DarkGreen
                            Else
                                lblEtat.Text = nbErr & " paramètre(s) non lu(s) (en rose) : " & PLCData.DerniereErreur
                                lblEtat.ForeColor = Color.Red
                            End If
                            _dernieresValeurs = valeurs
                            btnLire.Enabled = True : btnEnregistrer.Enabled = True
                            AfficherModeDemarrage()
                            _occupe = False
                        End Sub)
                Catch
                    _occupe = False
                End Try
            End Sub)
    End Sub

    ' ── ENREGISTRER CPU ───────────────────────────────────────────
    Private Sub btnEnregistrer_Click(sender As Object, e As EventArgs) Handles btnEnregistrer.Click
        If _occupe Then Exit Sub
        If Not PLCData.Connecte Then
            MessageBox.Show("CPU non connecté.", "Paramètres", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Exit Sub
        End If
        ' 1. Contrôle de saisie
        Dim aEcrire As New List(Of Tuple(Of Param, Double))
        For i As Integer = 0 To _params.Count - 1
            Dim p As Param = _params(i)
            Dim txt As String = p.Champ.Text.Trim().Replace(",", ".")
            Dim v As Double
            If Not Double.TryParse(txt, Globalization.NumberStyles.Float, Globalization.CultureInfo.InvariantCulture, v) Then
                MessageBox.Show("Valeur invalide pour « " & p.Libelle & " » : " & p.Champ.Text,
                                "Paramètres", MessageBoxButtons.OK, MessageBoxIcon.Stop)
                p.Champ.Focus() : p.Champ.SelectAll()
                Exit Sub
            End If
            If p.Genre = TypeVar.Temps Then v *= 1000   ' saisie en secondes -> ms pour le CPU
            Dim tol As Double = If(p.Genre = TypeVar.Temps, 100.0, 0.001)
            Dim inchange As Boolean = (_dernieresValeurs IsNot Nothing) AndAlso (i < _dernieresValeurs.Length) AndAlso (Math.Abs(v - _dernieresValeurs(i)) <= tol)
            If Not inchange Then aEcrire.Add(Tuple.Create(p, v))
        Next
        If aEcrire.Count = 0 Then
            MessageBox.Show("Aucune valeur modifiée.", "Paramètres", MessageBoxButtons.OK, MessageBoxIcon.Information)
            Exit Sub
        End If
        If MessageBox.Show("Envoyer les " & aEcrire.Count & " paramètres au CPU ?", "Enregistrer CPU",
                           MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) <> DialogResult.Yes Then
            Exit Sub
        End If
        ' 2. Écriture en arrière-plan puis relecture de contrôle
        _occupe = True
        lblEtat.Text = "Écriture CPU..." : lblEtat.ForeColor = Color.DarkOrange
        btnLire.Enabled = False : btnEnregistrer.Enabled = False
        Threading.ThreadPool.QueueUserWorkItem(
            Sub(state)
                Dim erreurs As New List(Of String)
                For Each t In aEcrire
                    If Not EcrireValeur(t.Item1, t.Item2) Then
                        erreurs.Add(t.Item1.Libelle & " (" & PLCData.DerniereErreur & ")")
                    End If
                Next
                Threading.Thread.Sleep(300)
                ' Relecture de contrôle (groupée par DB)
                Dim listeRelue As New List(Of Param)
                For Each t In aEcrire : listeRelue.Add(t.Item1) : Next
                Dim valeursLues(listeRelue.Count - 1) As Double
                Dim oksLues(listeRelue.Count - 1) As Boolean
                LireGroupe(listeRelue, valeursLues, oksLues)
                For i As Integer = 0 To aEcrire.Count - 1
                    Dim tol As Double = If(aEcrire(i).Item1.Genre = TypeVar.Temps, 100.0, 0.001)
                    If oksLues(i) AndAlso Math.Abs(valeursLues(i) - aEcrire(i).Item2) > tol Then
                        erreurs.Add(aEcrire(i).Item1.Libelle & " : envoyé " & aEcrire(i).Item2 & ", lu " & valeursLues(i))
                    End If
                Next
                Try
                    Me.BeginInvoke(
                        Sub()
                            _occupe = False
                            btnLire.Enabled = True : btnEnregistrer.Enabled = True
                            If erreurs.Count = 0 Then
                                lblEtat.Text = "Paramètres enregistrés dans le CPU à " & Now.ToString("HH:mm:ss")
                                lblEtat.ForeColor = Color.DarkGreen
                                Synoptique.EnregistrerAction("PARAMETRES", "Paramètres CPU enregistrés",
                                    aEcrire.Count & " valeurs", "DB8/DB19/DB63/DB90/DB1", "", "PLC_WRITE")
                            Else
                                lblEtat.Text = erreurs.Count & " erreur(s) d'écriture"
                                lblEtat.ForeColor = Color.Red
                                MessageBox.Show(String.Join(vbCrLf, erreurs), "Erreurs d'écriture CPU",
                                                MessageBoxButtons.OK, MessageBoxIcon.Error)
                                Synoptique.EnregistrerAction("ALARME", "Paramètres CPU : erreurs d'écriture",
                                    String.Join(" | ", erreurs), "", "", "ALARME")
                            End If
                            LireCPU()
                        End Sub)
                Catch
                    _occupe = False
                End Try
            End Sub)
    End Sub

    Private Sub txtT_VidangeADJ_TextChanged(sender As Object, e As EventArgs) Handles txtT_VidangeADJ.TextChanged

    End Sub
    ' ═══════════════════════════════════════════════════════════════
    '  MODE DÉMARRAGE MALAXEUR  (DÉMARREUR / CONTACTEUR)
    '  Bit CPU DB1.DBX1088.2 : 0 = DÉMARREUR, 1 = CONTACTEUR
    ' ═══════════════════════════════════════════════════════════════
    Private Const ADR_MODE_DEMARREUR As String = "DB1.DBX1088.2"
    Private Const BIT_DEMARREUR As Boolean = False   ' valeur du bit en mode DÉMARREUR

    ' Lit le CPU et renvoie True si mode DÉMARREUR, False si CONTACTEUR
    Private Function LireModeDemarreur(ByRef ok As Boolean) As Boolean
        ok = False
        Dim v As Object = ModulePLCDuplex.LireAdresse(ADR_MODE_DEMARREUR)
        If v Is Nothing Then Return False
        ok = True
        Return (CBool(v) = BIT_DEMARREUR)
    End Function

    Private Sub AfficherModeDemarrage()
        Try
            If Not PLCData.Connecte Then
                btnModeDemarrage.Text = "CPU non connecté"
                btnModeDemarrage.BackColor = Color.LightPink
                Exit Sub
            End If
            Dim ok As Boolean
            Dim demarreur As Boolean = LireModeDemarreur(ok)
            If Not ok Then
                btnModeDemarrage.Text = "Erreur lecture"
                btnModeDemarrage.BackColor = Color.LightPink
                Exit Sub
            End If
            btnModeDemarrage.Text = If(demarreur, "DÉMARREUR", "CONTACTEUR")
            btnModeDemarrage.BackColor = If(demarreur, Color.FromArgb(100, 220, 100), Color.FromArgb(255, 200, 120))
            btnModeDemarrage.UseVisualStyleBackColor = False
        Catch ex As Exception
            btnModeDemarrage.Text = "Erreur lecture"
            btnModeDemarrage.BackColor = Color.LightPink
        End Try
    End Sub

    Private Async Sub btnModeDemarrage_Click(sender As Object, e As EventArgs) Handles btnModeDemarrage.Click
        If Not PLCData.Connecte Then
            MessageBox.Show("CPU non connecté.", "Mode démarrage", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Exit Sub
        End If

        Dim okLu As Boolean
        Dim demarreurActuel As Boolean = LireModeDemarreur(okLu)
        If Not okLu Then
            MessageBox.Show("Lecture de " & ADR_MODE_DEMARREUR & " impossible.", "Mode démarrage",
                            MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Exit Sub
        End If
        Dim nouveauDemarreur As Boolean = Not demarreurActuel
        Dim libelle As String = If(nouveauDemarreur, "DÉMARREUR", "CONTACTEUR")

        If MessageBox.Show("Passer le malaxeur en mode " & libelle & " ?",
                           "Mode démarrage", MessageBoxButtons.YesNo,
                           MessageBoxIcon.Question) <> DialogResult.Yes Then Exit Sub

        Dim bitAEcrire As Boolean = If(nouveauDemarreur, BIT_DEMARREUR, Not BIT_DEMARREUR)
        If Not ModulePLCDuplex.EcrireBit(ADR_MODE_DEMARREUR, bitAEcrire) Then
            MessageBox.Show("Commande non envoyée au CPU (" & ADR_MODE_DEMARREUR & ").",
                            "Mode démarrage", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Exit Sub
        End If

        ' L'écriture passe par la file du thread PLC → on attend puis on relit
        btnModeDemarrage.Enabled = False
        Await Task.Delay(800)
        btnModeDemarrage.Enabled = True

        Dim okRelu As Boolean
        Dim demarreurRelu As Boolean = LireModeDemarreur(okRelu)
        Dim ok As Boolean = okRelu AndAlso (demarreurRelu = nouveauDemarreur)
        AfficherModeDemarrage()
        lblEtat.Text = If(ok, "Mode démarrage : " & libelle & " (OK)",
                              "Mode démarrage : le CPU n'a pas pris la valeur !")
        lblEtat.ForeColor = If(ok, Color.DarkGreen, Color.Red)
    End Sub
End Class
