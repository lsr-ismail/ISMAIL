Imports S7.Net

' ================================================================
'  ModuleGacheesDB71.vb — LECTURE DE LA TABLE DES GÂCHÉES DU CPU
'  16/09/2026
'  ─────────────────────────────────────────────────────────────
'  Le CPU mémorise les NB_GACHEES dernières gâchées dans DB71 :
'   • chaque colonne est un bloc de 160 octets = 40 REAL (4 octets)
'   • la gâchée n°k (1..40) est à  base + 4 × (k − 1)
'   • le COMPTEUR (DBD 3200…) est le MATRICULE de la gâchée :
'     numéro unique, jamais remis à zéro (comme un matricule).
'
'  Blocs (offset de base) — tableau automaticien, corrigé le 16/09 :
'     Poses  : TR1 0   TR2 160  TR3 320  TR4 480  SILO1 640  EAU 800
'              ADJ1 960 ADJ2 1120 ADJ3 1280 ADJ4 1440
'     Écarts : TR1 1600 TR2 1760 TR3 1920 TR4 2080 ADJ1 2240
'              ADJ2 2400 ADJ3 2560 ADJ4 2720 EAU 2880 SILO1 3040
'     Compteur : 3200
'
'  Utilisation (ModuleArchivageGachees) :
'     Dim nouvelles = ModuleGacheesDB71.LireNouvelles(dernierMatriculeArchive)
'     → liste triée par matricule croissant des gâchées pas encore en SQL
' ================================================================
Public Module ModuleGacheesDB71

    Public Const DB_GACHEES As Integer = 71
    Public Const NB_GACHEES As Integer = 40
    Private Const TAILLE_DB71 As Integer = 3440      ' 3200 + 160 + 80 (ajout d'eau, 22/09)
    Private Const AJOUT_EAU As Integer = 3360        ' 22/09 : Int (2 octets) par emplacement : 3360 + 2×k

    ' Le compteur est-il un REAL (True) ou un entier DINT (False) ?
    ' → à basculer si les matricules lus sont absurdes.
    Public COMPTEUR_EST_REAL As Boolean = False

    ' Offsets de base
    Private Const P_TR1 As Integer = 0, P_TR2 As Integer = 160, P_TR3 As Integer = 320, P_TR4 As Integer = 480
    Private Const P_S1 As Integer = 640, P_EAU As Integer = 800
    Private Const P_ADJ1 As Integer = 960, P_ADJ2 As Integer = 1120, P_ADJ3 As Integer = 1280, P_ADJ4 As Integer = 1440
    Private Const E_TR1 As Integer = 1600, E_TR2 As Integer = 1760, E_TR3 As Integer = 1920, E_TR4 As Integer = 2080
    Private Const E_ADJ1 As Integer = 2240, E_ADJ2 As Integer = 2400, E_ADJ3 As Integer = 2560, E_ADJ4 As Integer = 2720
    Private Const E_EAU As Integer = 2880, E_S1 As Integer = 3040
    Private Const COMPTEUR As Integer = 3200

    Public Class GacheeCPU
        Public Emplacement As Integer      ' 1..40
        Public Matricule As Long
        Public PoseTR1, PoseTR2, PoseTR3, PoseTR4 As Single
        Public PoseS1, PoseEAU As Single
        Public PoseADJ1, PoseADJ2, PoseADJ3, PoseADJ4 As Single
        Public EcartTR1, EcartTR2, EcartTR3, EcartTR4 As Single
        Public EcartS1, EcartEAU As Single
        Public EcartADJ1, EcartADJ2, EcartADJ3, EcartADJ4 As Single
        Public AjoutEau As Integer          ' 22/09 : DB71.DBW3360… (Int) ajout d'eau manuel de la gâchée
    End Class

    ' ── Dernière erreur de lecture DB71 (pour diagnostic) ────────
    Public DerniereErreur As String = ""

    ' ═══════════════════════════════════════════════════════════
    '  Lit seulement le bloc COMPTEUR (160 octets, 1 trame) et
    '  renvoie les matricules des 40 emplacements. Nothing si erreur.
    ' ═══════════════════════════════════════════════════════════
    Public Function LireMatricules() As Long()
        Dim b() As Byte = ModulePLCDuplex.LireBytesDB(DB_GACHEES, COMPTEUR, NB_GACHEES * 4)
        If b Is Nothing Then
            DerniereErreur = PLCData.DerniereErreur
            Return Nothing
        End If
        Dim m(NB_GACHEES - 1) As Long
        For k As Integer = 0 To NB_GACHEES - 1
            m(k) = LireCompteur(b, k * 4)
        Next
        Return m
    End Function

    ' ═══════════════════════════════════════════════════════════
    '  Renvoie les gâchées dont le matricule est > dernierMatricule,
    '  triées par matricule croissant. Lit toute DB71 (17 trames)
    '  UNIQUEMENT s'il y a du nouveau.
    ' ═══════════════════════════════════════════════════════════
    Public Function LireNouvelles(dernierMatricule As Long) As List(Of GacheeCPU)
        Dim res As New List(Of GacheeCPU)
        Dim mats() As Long = LireMatricules()
        If mats Is Nothing Then Return res

        Dim aDuNouveau As Boolean = False
        For k As Integer = 0 To NB_GACHEES - 1
            If mats(k) > dernierMatricule AndAlso mats(k) > 0 Then aDuNouveau = True : Exit For
        Next
        If Not aDuNouveau Then Return res

        Dim db() As Byte = ModulePLCDuplex.LireBytesDB(DB_GACHEES, 0, TAILLE_DB71)
        If db Is Nothing Then
            DerniereErreur = PLCData.DerniereErreur
            Return res
        End If

        For k As Integer = 0 To NB_GACHEES - 1
            Dim mat As Long = LireCompteur(db, COMPTEUR + k * 4)
            If mat <= dernierMatricule OrElse mat <= 0 Then Continue For
            Dim o As Integer = k * 4
            Dim g As New GacheeCPU With {
                .Emplacement = k + 1, .Matricule = mat,
                .PoseTR1 = R(db, P_TR1 + o), .PoseTR2 = R(db, P_TR2 + o), .PoseTR3 = R(db, P_TR3 + o), .PoseTR4 = R(db, P_TR4 + o),
                .PoseS1 = R(db, P_S1 + o), .PoseEAU = R(db, P_EAU + o),
                .PoseADJ1 = R(db, P_ADJ1 + o), .PoseADJ2 = R(db, P_ADJ2 + o), .PoseADJ3 = R(db, P_ADJ3 + o), .PoseADJ4 = R(db, P_ADJ4 + o),
                .EcartTR1 = R(db, E_TR1 + o), .EcartTR2 = R(db, E_TR2 + o), .EcartTR3 = R(db, E_TR3 + o), .EcartTR4 = R(db, E_TR4 + o),
                .EcartS1 = R(db, E_S1 + o), .EcartEAU = R(db, E_EAU + o),
                .EcartADJ1 = R(db, E_ADJ1 + o), .EcartADJ2 = R(db, E_ADJ2 + o), .EcartADJ3 = R(db, E_ADJ3 + o), .EcartADJ4 = R(db, E_ADJ4 + o),
                .AjoutEau = I16(db, AJOUT_EAU + k * 2)}
            res.Add(g)
        Next
        res.Sort(Function(a, b) a.Matricule.CompareTo(b.Matricule))
        Return res
    End Function

    ' ═══════════════════════════════════════════════════════════
    '  DÉTECTION PAR CHANGEMENT D'EMPLACEMENT (résiste à la remise
    '  à zéro du compteur CPU) : renvoie les gâchées dont le compteur
    '  de l'emplacement k est DIFFÉRENT de precedent(k). Le tableau
    '  precedent n'est PAS modifié ici : l'appelant met à jour
    '  precedent(Emplacement-1) après archivage réussi.
    ' ═══════════════════════════════════════════════════════════
    Public Function LireChangements(precedent() As Long) As List(Of GacheeCPU)
        Dim res As New List(Of GacheeCPU)
        If precedent Is Nothing OrElse precedent.Length < NB_GACHEES Then Return res
        Dim mats() As Long = LireMatricules()
        If mats Is Nothing Then Return res

        Dim change As Boolean = False
        For k As Integer = 0 To NB_GACHEES - 1
            If mats(k) <> precedent(k) AndAlso mats(k) > 0 Then change = True : Exit For
        Next
        If Not change Then Return res

        Dim db() As Byte = ModulePLCDuplex.LireBytesDB(DB_GACHEES, 0, TAILLE_DB71)
        If db Is Nothing Then
            DerniereErreur = PLCData.DerniereErreur
            Return res
        End If

        For k As Integer = 0 To NB_GACHEES - 1
            Dim mat As Long = LireCompteur(db, COMPTEUR + k * 4)
            If mat = precedent(k) OrElse mat <= 0 Then Continue For
            res.Add(Extraire(db, k, mat))
        Next
        res.Sort(Function(a, b) a.Matricule.CompareTo(b.Matricule))
        Return res
    End Function

    Private Function Extraire(db() As Byte, k As Integer, mat As Long) As GacheeCPU
        Dim o As Integer = k * 4
        Return New GacheeCPU With {
            .Emplacement = k + 1, .Matricule = mat,
            .PoseTR1 = R(db, P_TR1 + o), .PoseTR2 = R(db, P_TR2 + o), .PoseTR3 = R(db, P_TR3 + o), .PoseTR4 = R(db, P_TR4 + o),
            .PoseS1 = R(db, P_S1 + o), .PoseEAU = R(db, P_EAU + o),
            .PoseADJ1 = R(db, P_ADJ1 + o), .PoseADJ2 = R(db, P_ADJ2 + o), .PoseADJ3 = R(db, P_ADJ3 + o), .PoseADJ4 = R(db, P_ADJ4 + o),
            .EcartTR1 = R(db, E_TR1 + o), .EcartTR2 = R(db, E_TR2 + o), .EcartTR3 = R(db, E_TR3 + o), .EcartTR4 = R(db, E_TR4 + o),
            .EcartS1 = R(db, E_S1 + o), .EcartEAU = R(db, E_EAU + o),
            .EcartADJ1 = R(db, E_ADJ1 + o), .EcartADJ2 = R(db, E_ADJ2 + o), .EcartADJ3 = R(db, E_ADJ3 + o), .EcartADJ4 = R(db, E_ADJ4 + o),
            .AjoutEau = I16(db, AJOUT_EAU + k * 2)}
    End Function

    ' ═══════════════════════════════════════════════════════════
    '  DIAGNOSTIC : valeurs brutes des 21 blocs pour les emplacements
    '  1..nbEmpl (pour vérifier les offsets avec l'écran WinCC).
    ' ═══════════════════════════════════════════════════════════
    Public Function Diagnostic(Optional nbEmpl As Integer = 3) As String
        Dim db() As Byte = ModulePLCDuplex.LireBytesDB(DB_GACHEES, 0, TAILLE_DB71)
        If db Is Nothing Then Return "Lecture DB71 impossible : " & PLCData.DerniereErreur
        Dim blocs() As (String, Integer) = {
            ("Pose TR1", P_TR1), ("Pose TR2", P_TR2), ("Pose TR3", P_TR3), ("Pose TR4", P_TR4),
            ("Pose S1", P_S1), ("Pose EAU", P_EAU),
            ("Pose ADJ1", P_ADJ1), ("Pose ADJ2", P_ADJ2), ("Pose ADJ3", P_ADJ3), ("Pose ADJ4", P_ADJ4),
            ("Ecart TR1", E_TR1), ("Ecart TR2", E_TR2), ("Ecart TR3", E_TR3), ("Ecart TR4", E_TR4),
            ("Ecart ADJ1", E_ADJ1), ("Ecart ADJ2", E_ADJ2), ("Ecart ADJ3", E_ADJ3), ("Ecart ADJ4", E_ADJ4),
            ("Ecart EAU", E_EAU), ("Ecart S1", E_S1)}
        Dim sb As New System.Text.StringBuilder()
        sb.Append("Bloc (offset)".PadRight(22))
        For k As Integer = 1 To nbEmpl
            sb.Append(("Empl." & k).PadLeft(12))
        Next
        sb.AppendLine()
        sb.Append("Compteur (3200)".PadRight(22))
        For k As Integer = 1 To nbEmpl
            sb.Append(LireCompteur(db, COMPTEUR + (k - 1) * 4).ToString().PadLeft(12))
        Next
        sb.AppendLine()
        For Each b In blocs
            sb.Append((b.Item1 & " (" & b.Item2 & ")").PadRight(22))
            For k As Integer = 1 To nbEmpl
                sb.Append(R(db, b.Item2 + (k - 1) * 4).ToString("0.000", Globalization.CultureInfo.InvariantCulture).PadLeft(12))
            Next
            sb.AppendLine()
        Next
        ' Balayage complet : tous les REAL non nuls de DB71 (pour retrouver les valeurs "perdues")
        sb.AppendLine()
        sb.AppendLine("REAL non nuls dans DB71 (offset = valeur) :")
        Dim n As Integer = 0
        For off As Integer = 0 To COMPTEUR - 4 Step 4
            Dim v As Single = R(db, off)
            If Math.Abs(v) > 0.0001F AndAlso Not Single.IsNaN(v) AndAlso Math.Abs(v) < 1000000.0F Then
                sb.Append("DBD" & off & "=" & v.ToString("0.###", Globalization.CultureInfo.InvariantCulture) & "   ")
                n += 1
                If n Mod 6 = 0 Then sb.AppendLine()
            End If
        Next
        Return sb.ToString()
    End Function

    ' Plus grand matricule présent dans le CPU (0 si illisible)
    Public Function MatriculeMaxCPU() As Long
        Dim mats() As Long = LireMatricules()
        If mats Is Nothing Then Return 0
        Dim mx As Long = 0
        For Each m In mats
            If m > mx Then mx = m
        Next
        Return mx
    End Function

    ' ── Helpers Big Endian ───────────────────────────────────────
    Private Function LireCompteur(b() As Byte, off As Integer) As Long
        If b Is Nothing OrElse off + 3 >= b.Length Then Return 0
        If COMPTEUR_EST_REAL Then
            Return CLng(Math.Round(R(b, off)))
        End If
        Return CLng(CInt(b(off)) << 24 Or CInt(b(off + 1)) << 16 Or CInt(b(off + 2)) << 8 Or CInt(b(off + 3)))
    End Function

    ' Int S7 (16 bits signé, big endian)
    Private Function I16(b() As Byte, off As Integer) As Integer
        If b Is Nothing OrElse off + 1 >= b.Length Then Return 0
        Return BitConverter.ToInt16({b(off + 1), b(off)}, 0)
    End Function

    Private Function R(b() As Byte, off As Integer) As Single
        If b Is Nothing OrElse off + 3 >= b.Length Then Return 0
        Dim t() As Byte = {b(off + 3), b(off + 2), b(off + 1), b(off)}
        Return BitConverter.ToSingle(t, 0)
    End Function

End Module
