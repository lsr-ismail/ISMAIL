Imports System.Windows.Forms

' ================================================================
'  Frm_ParamEquipement.vb — 25/09/2026
'  Petite fenêtre « Paramètres » d'un équipement du Synoptique (clic droit → Paramètres).
'  Liste de paramètres fournie par l'appelant (Synoptique.ParametresEquipement) :
'    REAL (kg, %) ou TIME (DINT ms, affiché/saisi en secondes avec virgule, ex. 1,000).
'  Mêmes adresses que Configuration centrale (DB8 / DB19 / DB90 / DB1).
' ================================================================
Public Class Frm_ParamEquipement
    Inherits Form

    Public Structure ParamDef
        Public Libelle As String
        Public Db As Integer
        Public Offset As Integer
        Public EstTemps As Boolean      ' True = TIME (ms) ; False = REAL
        Public Unite As String          ' "kg", "%", "s"
        Public Sub New(libelleP As String, dbP As Integer, offsetP As Integer, estTempsP As Boolean, uniteP As String)
            Libelle = libelleP : Db = dbP : Offset = offsetP : EstTemps = estTempsP : Unite = uniteP
        End Sub
    End Structure

    Private ReadOnly _params As List(Of ParamDef)
    Private ReadOnly _txt As List(Of TextBox)
    Private _lus() As Double                   ' dernières valeurs lues (REAL ou ms)
    Private ReadOnly _lblEtat As New Label()
    Private _btnLire As Button, _btnEnregistrer As Button
    Private _occupe As Boolean = False

    Public Sub New(titre As String, params As List(Of ParamDef))
        _params = params
        _txt = New List(Of TextBox)
        ReDim _lus(params.Count - 1)
        ConstruireInterface(titre)
    End Sub

    Private Sub ConstruireInterface(titre As String)
        Me.Text = "Paramètres " & titre
        Me.FormBorderStyle = FormBorderStyle.FixedDialog
        Me.MaximizeBox = False : Me.MinimizeBox = False
        Me.StartPosition = FormStartPosition.CenterParent
        Me.Font = New Font("Segoe UI", 10)

        Dim lblTitre As New Label With {.Text = titre, .Font = New Font("Segoe UI", 11, FontStyle.Bold),
                                        .ForeColor = Color.DarkBlue, .AutoSize = True, .Location = New Point(12, 10)}
        Me.Controls.Add(lblTitre)

        Dim y As Integer = 45
        For k As Integer = 0 To _params.Count - 1
            Me.Controls.Add(New Label With {.Text = _params(k).Libelle & " :", .Location = New Point(15, y + 3), .AutoSize = True})
            Dim tb As New TextBox With {.Location = New Point(230, y), .Width = 110, .TextAlign = HorizontalAlignment.Right}
            _txt.Add(tb) : Me.Controls.Add(tb)
            Me.Controls.Add(New Label With {.Text = _params(k).Unite, .Location = New Point(348, y + 3), .AutoSize = True,
                                            .Font = New Font("Segoe UI", 10, FontStyle.Bold)})
            y += 32
        Next

        _lblEtat.Location = New Point(12, y + 4) : _lblEtat.AutoSize = True
        _lblEtat.Font = New Font("Segoe UI", 8) : _lblEtat.ForeColor = Color.Gray
        Me.Controls.Add(_lblEtat)
        y += 26

        _btnLire = New Button With {.Text = "Lire", .Location = New Point(15, y), .Size = New Size(90, 28)}
        _btnEnregistrer = New Button With {.Text = "Enregistrer", .Location = New Point(115, y), .Size = New Size(110, 28)}
        Dim btnFermer As New Button With {.Text = "Fermer", .Location = New Point(290, y), .Size = New Size(90, 28)}
        AddHandler _btnLire.Click, Sub() LireCPU()
        AddHandler _btnEnregistrer.Click, Sub() EnregistrerCPU()
        AddHandler btnFermer.Click, Sub() Me.Close()
        Me.Controls.AddRange({_btnLire, _btnEnregistrer, btnFermer})
        Me.CancelButton = btnFermer
        Me.ClientSize = New Size(395, y + 40)
    End Sub

    Private Sub Frm_ParamEquipement_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        LireCPU()
    End Sub

    Private Shared Function Formater(p As ParamDef, v As Double) As String
        If p.EstTemps Then
            Return (v / 1000).ToString("0.000", Globalization.CultureInfo.InvariantCulture).Replace(".", ",")
        End If
        Return v.ToString("0.###")
    End Function

    ' ── Lecture : une trame par DB (plage min → max) ──
    Private Sub LireCPU()
        If _occupe Then Exit Sub
        If Not PLCData.Connecte Then
            _lblEtat.Text = "CPU non connecté" : _lblEtat.ForeColor = Color.Red : Exit Sub
        End If
        _occupe = True : _btnLire.Enabled = False : _btnEnregistrer.Enabled = False
        _lblEtat.Text = "Lecture CPU..." : _lblEtat.ForeColor = Color.DarkOrange
        Dim liste As List(Of ParamDef) = _params
        Threading.ThreadPool.QueueUserWorkItem(
            Sub(state)
                Dim valeurs(liste.Count - 1) As Double
                Dim oks(liste.Count - 1) As Boolean
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
                        If buf IsNot Nothing AndAlso buf.Length >= taille Then
                            For Each i In idx
                                Dim rel As Integer = liste(i).Offset - minOff
                                Dim inv() As Byte = {buf(rel + 3), buf(rel + 2), buf(rel + 1), buf(rel)}
                                If liste(i).EstTemps Then
                                    valeurs(i) = BitConverter.ToInt32(inv, 0)
                                Else
                                    Dim v As Single = BitConverter.ToSingle(inv, 0)
                                    If Single.IsNaN(v) OrElse Single.IsInfinity(v) Then v = 0
                                    valeurs(i) = v
                                End If
                                oks(i) = True
                            Next
                        End If
                    Catch
                    End Try
                Next
                Try
                    Me.BeginInvoke(
                        Sub()
                            Dim nbErr As Integer = 0
                            For i As Integer = 0 To liste.Count - 1
                                If oks(i) Then
                                    _lus(i) = valeurs(i)
                                    _txt(i).Text = Formater(liste(i), valeurs(i))
                                    _txt(i).BackColor = Color.White
                                Else
                                    _txt(i).BackColor = Color.MistyRose : nbErr += 1
                                End If
                            Next
                            If nbErr = 0 Then
                                _lblEtat.Text = "Lu à " & Now.ToString("HH:mm:ss") : _lblEtat.ForeColor = Color.DarkGreen
                            Else
                                _lblEtat.Text = nbErr & " paramètre(s) non lu(s) : " & PLCData.DerniereErreur : _lblEtat.ForeColor = Color.Red
                            End If
                            _btnLire.Enabled = True : _btnEnregistrer.Enabled = True : _occupe = False
                        End Sub)
                Catch
                    _occupe = False
                End Try
            End Sub)
    End Sub

    ' ── Enregistrement : seulement les valeurs modifiées, puis relecture ──
    Private Sub EnregistrerCPU()
        If _occupe Then Exit Sub
        If Not PLCData.Connecte Then
            MessageBox.Show("CPU non connecté.", "Paramètres", MessageBoxButtons.OK, MessageBoxIcon.Warning) : Exit Sub
        End If
        Dim aEcrire As New List(Of Integer)
        Dim valeurs(_params.Count - 1) As Double
        For k As Integer = 0 To _params.Count - 1
            Dim txt As String = _txt(k).Text.Trim().Replace(",", ".")
            Dim v As Double
            If Not Double.TryParse(txt, Globalization.NumberStyles.Float, Globalization.CultureInfo.InvariantCulture, v) Then
                MessageBox.Show("Valeur invalide pour « " & _params(k).Libelle & " » : " & _txt(k).Text,
                                "Paramètres", MessageBoxButtons.OK, MessageBoxIcon.Stop)
                _txt(k).Focus() : _txt(k).SelectAll() : Exit Sub
            End If
            If _params(k).EstTemps Then v *= 1000              ' secondes → ms
            valeurs(k) = v
            Dim tol As Double = If(_params(k).EstTemps, 100.0, 0.001)
            If Math.Abs(v - _lus(k)) > tol Then aEcrire.Add(k)
        Next
        If aEcrire.Count = 0 Then
            MessageBox.Show("Aucune valeur modifiée.", "Paramètres", MessageBoxButtons.OK, MessageBoxIcon.Information) : Exit Sub
        End If
        If MessageBox.Show("Envoyer " & aEcrire.Count & " paramètre(s) au CPU ?", "Enregistrer CPU",
                           MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) <> DialogResult.Yes Then Exit Sub
        _occupe = True : _btnLire.Enabled = False : _btnEnregistrer.Enabled = False
        _lblEtat.Text = "Écriture CPU..." : _lblEtat.ForeColor = Color.DarkOrange
        Dim titre As String = Me.Text
        Dim liste As List(Of ParamDef) = _params
        Threading.ThreadPool.QueueUserWorkItem(
            Sub(state)
                Dim erreurs As New List(Of String)
                For Each k In aEcrire
                    Dim ok As Boolean
                    If liste(k).EstTemps Then
                        ok = ModulePLCDuplex.EcrireDInt(liste(k).Db, liste(k).Offset, CInt(Math.Round(valeurs(k))))
                    Else
                        ok = ModulePLCDuplex.EcrireReal(liste(k).Db, liste(k).Offset, CSng(valeurs(k)))
                    End If
                    If Not ok Then erreurs.Add(liste(k).Libelle & " (" & PLCData.DerniereErreur & ")")
                Next
                Threading.Thread.Sleep(300)
                Try
                    Me.BeginInvoke(
                        Sub()
                            _occupe = False
                            If erreurs.Count = 0 Then
                                _lblEtat.Text = "Enregistré à " & Now.ToString("HH:mm:ss") : _lblEtat.ForeColor = Color.DarkGreen
                                Synoptique.EnregistrerAction("PARAMETRES", titre, aEcrire.Count & " valeur(s)", "CPU", "", "PLC_WRITE")
                            Else
                                _lblEtat.Text = erreurs.Count & " erreur(s) d'écriture" : _lblEtat.ForeColor = Color.Red
                                MessageBox.Show(String.Join(vbCrLf, erreurs), "Erreurs d'écriture CPU", MessageBoxButtons.OK, MessageBoxIcon.Error)
                            End If
                            LireCPU()   ' relecture de contrôle
                        End Sub)
                Catch
                    _occupe = False
                End Try
            End Sub)
    End Sub

End Class
