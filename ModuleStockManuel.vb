Imports System.Data

' ================================================================
'  ModuleStockManuel.vb — 26/09/2026
'  Suivi du stock en MODE MANUEL (forçage) : méthode événementielle.
'    • Remplissage : à l'APPUI sur un poussoir (trémie 1-4, vis 1/2, vanne eau, ADJ 1-4)
'      on mémorise le poids de la bascule ; au RELÂCHEMENT, les boutons du groupe sont
'      bloqués 0,5 s (stabilisation) puis on relit le poids : la différence est mise
'      dans le « panier » de l'équipement (→ matériau de la case / du silo / de la bascule).
'    • Vidange : à l'ouverture de la trappe (CIM / EAU / ADJ) ou à la marche du tapis
'      peseur (AGR) on mémorise le poids ; 0,5 s après le relâchement / l'arrêt on relit :
'      la quantité sortie est SOUSTRAITE DU STOCK des matériaux du panier (au prorata),
'      avec les mêmes fonctions et conversions qu'en automatique, et journalisée.
'    • Rien n'est lu en continu ; en automatique c'est DB71 qui fait foi (pas de double comptage).
' ================================================================
Public Module ModuleStockManuel

    Public Const DELAI_STABILISATION_MS As Integer = 500

    ' panier : bascule ("AGR","CIM","EAU","ADJ") → source ("TR1".."TR4","S1","S2","EAU","ADJ1".."ADJ4") → kg
    Private ReadOnly _panier As New Dictionary(Of String, Dictionary(Of String, Double))
    Private ReadOnly _poidsDebut As New Dictionary(Of String, Single)
    Private ReadOnly _verrou As New Object()

    ' ── Poids actuel d'une bascule (instantané DB1) ──
    Public Function PoidsBascule(bascule As String) As Single
        Select Case bascule
            Case "AGR" : Return PLCData.Poids_AGR
            Case "CIM" : Return PLCData.Poids_CIM
            Case "EAU" : Return PLCData.Poids_EAU
            Case "ADJ" : Return PLCData.Poids_ADJ
            Case Else : Return 0
        End Select
    End Function

    Public Function BasculeDe(source As String) As String
        If source.StartsWith("TR") Then Return "AGR"
        If source.StartsWith("S") Then Return "CIM"
        If source = "EAU" Then Return "EAU"
        If source.StartsWith("ADJ") Then Return "ADJ"
        Return ""
    End Function

    Private Function Seuil(bascule As String) As Single
        Return If(bascule = "ADJ", 0.005F, 0.5F)      ' bruit de pesée ignoré
    End Function

    ' ══════════════ REMPLISSAGE ══════════════
    Public Sub DebutRemplissage(source As String)
        SyncLock _verrou
            _poidsDebut(source) = PoidsBascule(BasculeDe(source))
        End SyncLock
    End Sub

    ' appelé 0,5 s après le relâchement du poussoir
    Public Sub FinRemplissage(source As String)
        Dim bascule As String = BasculeDe(source)
        If bascule = "" Then Exit Sub
        Dim avant As Single = 0
        SyncLock _verrou
            If Not _poidsDebut.TryGetValue(source, avant) Then Exit Sub
            _poidsDebut.Remove(source)
        End SyncLock
        Dim apres As Single = PoidsBascule(bascule)
        Dim delta As Double = apres - avant
        If delta < Seuil(bascule) Then Exit Sub
        SyncLock _verrou
            If Not _panier.ContainsKey(bascule) Then _panier(bascule) = New Dictionary(Of String, Double)
            If Not _panier(bascule).ContainsKey(source) Then _panier(bascule)(source) = 0
            _panier(bascule)(source) += delta
        End SyncLock
        Synoptique.EnregistrerAction("STOCK", "Remplissage manuel " & source,
            "+" & delta.ToString("0.0##") & " " & Unite(bascule) & " (bascule " & bascule & " : " & avant.ToString("0.0##") & " → " & apres.ToString("0.0##") & ")",
            "", "", "MANUEL")
    End Sub

    ' ══════════════ VIDANGE ══════════════
    Public Sub DebutVidange(bascule As String)
        SyncLock _verrou
            _poidsDebut("VID_" & bascule) = PoidsBascule(bascule)
        End SyncLock
    End Sub

    ' appelé 0,5 s après le relâchement de la trappe / l'arrêt du tapis
    Public Sub FinVidange(bascule As String)
        Dim avant As Single = 0
        SyncLock _verrou
            If Not _poidsDebut.TryGetValue("VID_" & bascule, avant) Then Exit Sub
            _poidsDebut.Remove("VID_" & bascule)
        End SyncLock
        Dim apres As Single = PoidsBascule(bascule)
        Dim sorti As Double = avant - apres
        If sorti < Seuil(bascule) Then Exit Sub

        Dim parts As New List(Of KeyValuePair(Of String, Double))
        SyncLock _verrou
            Dim total As Double = 0
            If _panier.ContainsKey(bascule) Then
                For Each kv In _panier(bascule) : total += kv.Value : Next
            End If
            If total <= 0 Then
                Synoptique.EnregistrerAction("ALARME", "Vidange manuelle " & bascule & " sans remplissage identifie",
                    sorti.ToString("0.0##") & " " & Unite(bascule) & " NON deduits du stock", "", "", "MANUEL")
                Exit Sub
            End If
            Dim ratio As Double = Math.Min(1.0, sorti / total)
            For Each source In New List(Of String)(_panier(bascule).Keys)
                Dim q As Double = _panier(bascule)(source) * ratio
                If q > 0 Then parts.Add(New KeyValuePair(Of String, Double)(source, q))
                _panier(bascule)(source) -= q
                If _panier(bascule)(source) < Seuil(bascule) Then _panier(bascule).Remove(source)
            Next
        End SyncLock

        For Each p In parts
            SortirStock(p.Key, p.Value, bascule)
        Next
    End Sub

    ' ══════════════ SORTIE DE STOCK ══════════════
    Private Sub SortirStock(source As String, kg As Double, bascule As String)
        Try
            Dim idMat As Integer = IdMateriauPour(source)
            Dim nomMat As String = ""
            ' 26/09 : silo ciment → stock DU SILO (Silos.StockKg) en plus du total matériau
            If source.StartsWith("S") AndAlso Not source.StartsWith("SILO") Then
                Dim sid As Integer = ModuleArchivageGachees.SiloIdCiment(CInt(source.Substring(1)))
                If sid > 0 Then ModuleArchivageGachees.Instance.SortieStockSilo(sid, kg)
            End If
            If idMat > 0 Then
                nomMat = ModuleDb.LireValeur(Of String)(
                    "SELECT ISNULL(NomMateriau,'') FROM dbo.Materiaux WHERE IdMateriau=@id",
                    Sub(cmd) cmd.Parameters.AddWithValue("@id", idMat))
                ModuleArchivageGachees.Instance.SortieStock(idMat, kg)
                Synoptique.EnregistrerAction("STOCK", "Sortie manuelle " & source,
                    kg.ToString("0.0##") & " " & Unite(bascule) & " → " & nomMat, "", "", "MANUEL")
            Else
                Synoptique.EnregistrerAction("ALARME", "Sortie manuelle " & source & " : aucun materiau affecte",
                    kg.ToString("0.0##") & " " & Unite(bascule) & " NON deduits du stock", "", "", "MANUEL")
            End If
        Catch ex As Exception
            Synoptique.EnregistrerAction("ALARME", "Sortie manuelle " & source & " : erreur", ex.Message, "", "", "MANUEL")
        End Try
    End Sub

    ' Même liaison équipement → matériau qu'en automatique (DecrementerStocks)
    Private Function IdMateriauPour(source As String) As Integer
        ' 26/09 : dbo.Silos = table unique de configuration (TypeElement TR / S / ADJ / EAU, NumeroElement)
        Try
            Dim typ As String, num As Integer
            If source.StartsWith("TR") Then
                typ = "TR" : num = CInt(source.Substring(2))
            ElseIf source.StartsWith("ADJ") Then
                typ = "ADJ" : num = CInt(source.Substring(3))
            ElseIf source.StartsWith("S") Then
                typ = "S" : num = CInt(source.Substring(1))
            ElseIf source = "EAU" Then
                typ = "EAU" : num = 0
            Else
                Return 0
            End If
            If num > 0 Then
                Return ModuleDb.LireValeur(Of Integer)(
                    "SELECT TOP 1 ISNULL(IdMateriau,0) FROM dbo.Silos WHERE EstActif = 1 AND TypeElement = @t AND NumeroElement = @n",
                    Sub(cmd)
                        cmd.Parameters.AddWithValue("@t", typ)
                        cmd.Parameters.AddWithValue("@n", num)
                    End Sub)
            Else
                Return ModuleDb.LireValeur(Of Integer)(
                    "SELECT TOP 1 ISNULL(IdMateriau,0) FROM dbo.Silos WHERE EstActif = 1 AND TypeElement = @t ORDER BY NumeroElement",
                    Sub(cmd) cmd.Parameters.AddWithValue("@t", typ))
            End If
        Catch
        End Try
        Return 0
    End Function

    Private Function Unite(bascule As String) As String
        Return If(bascule = "EAU", "L", "kg")
    End Function

    ' Contenu des paniers (pour affichage / diagnostic)
    Public Function ResumePaniers() As String
        Dim sb As New System.Text.StringBuilder()
        SyncLock _verrou
            For Each b In _panier
                For Each s In b.Value
                    If s.Value > 0 Then sb.Append(s.Key & "=" & s.Value.ToString("0.0") & " ")
                Next
            Next
        End SyncLock
        Return sb.ToString().Trim()
    End Function

End Module
