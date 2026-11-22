Imports S7.Net
Imports System.Threading
Imports System.Text.RegularExpressions
Imports System.Collections.Concurrent
Imports System.Diagnostics

' ================================================================
'  ModulePLCDuplex.vb — Communication "full duplex" avec le CPU
'  ─────────────────────────────────────────────────────────────
'  v7 — 15/09/2026 — NOUVEL ADRESSAGE DB1 (tableau automaticien)
'   • Lectures : DB1 en UNE seule lecture de 1084 octets.
'   • Écritures : ECRIRE_DANS_DB1 = True → toutes les commandes vont
'     aux nouvelles adresses DB1 (les formulaires utilisent déjà
'     "DB1.DBX0.5", etc.). Mettre False pour revenir aux adresses M/DB
'     d'origine si le CPU ne lit pas ses commandes dans DB1.
'   • Aucun appel réseau sur le thread UI : file d'écriture + thread
'     dédié ; lectures ponctuelles depuis l'instantané DB1.
'   • PLCData.DerniereErreur : dernière erreur PLC, à afficher.
'  ─────────────────────────────────────────────────────────────
'  16/09 : M14.7→DB1.DBX1088.1, M14.2→DB1.DBX1088.2, M31.1→DB1.DBX6.5 (DB1 ≥ 1090 octets)
'  16/09 (soir) : ADRESSES SUPPRIMÉES DU CPU (n'existent plus) : M30.4, M1.1, M0.0, M0.1, M0.2, M1.0, M2.0, DB18.DBD20
'    → plus aucune lecture zone M ni DB18 ; EcrireBit/LireAdresse les ignorent (aucune erreur).
'    Cons_EAU2 est maintenant lue en DB1.DBD166 (ex DB8.DBD20, consigne eau).
' ================================================================

Public Class PLCData

    Public Shared PoidsTapis As Single = 0
    Public Shared Poids_AGR As Single = 0     ' DB1 DBD86
    Public Shared Poids_CIM As Single = 0     ' DB1 DBD78
    Public Shared Poids_EAU As Single = 0     ' DB1 DBD82
    Public Shared Poids_ADJ As Single = 0     ' DB1 DBD90
    Public Shared Ecar_ADJ1 As Single = 0     ' DB1 DBD402
    Public Shared Ecar_ADJ2 As Single = 0     ' DB1 DBD406
    Public Shared Ecar_ADJ3 As Single = 0     ' DB1 DBD410
    Public Shared Ecar_ADJ4 As Single = 0     ' DB1 DBD414

    Public Shared Cons_G1 As Single = 0       ' DB1 DBD118
    Public Shared Cons_SC As Single = 0       ' DB1 DBD122
    Public Shared Cons_SL As Single = 0       ' DB1 DBD126
    Public Shared Cons_D2 As Single = 0       ' DB1 DBD130
    Public Shared Cons_CIM As Single = 0      ' DB1 DBD94
    Public Shared ConsigneTapis As Single = 0 ' DB1 DBD1052
    Public Shared Cons_ADJ1 As Single = 0     ' DB1 DBD174
    Public Shared Cons_ADJ2 As Single = 0     ' DB1 DBD178
    Public Shared ComptCIMEAU As Single = 0   ' DB1 DBD1068
    Public Shared CompGachee As Single = 0    ' DB1 DBD1072
    Public Shared VisActif As Integer = 0     ' DB1 DBW52

    Public Shared Cons_ADJ3 As Single = 0     ' DB1 DBD182
    Public Shared Cons_ADJ4 As Single = 0     ' DB1 DBD186

    Public Shared NGachee As Integer = 0      ' DB1 DBW1076
    Public Shared Pose_G1 As Single = 0       ' DB1 DBD230
    Public Shared Pose_SC As Single = 0       ' DB1 DBD234
    Public Shared Pose_SL As Single = 0       ' DB1 DBD238
    Public Shared Pose_D2 As Single = 0       ' DB1 DBD242
    Public Shared Pose_CIM_S1 As Single = 0   ' DB1 DBD206
    Public Shared Cons_EAU2 As Single = 0     ' DB1 DBD166 (ex DB8.DBD20 ; DB18 supprimé)
    Public Shared Pose_EAU2 As Single = 0     ' DB1 DBD278
    Public Shared Ecar_G1 As Single = 0       ' DB1 DBD346
    Public Shared Ecar_SC As Single = 0       ' DB1 DBD350
    Public Shared Ecar_SL As Single = 0       ' DB1 DBD354
    Public Shared Ecar_D2 As Single = 0       ' DB1 DBD358
    Public Shared Ecar_CIM As Single = 0      ' DB1 DBD318
    Public Shared Ecar_EAU2 As Single = 0     ' DB1 DBD394
    Public Shared Pose_CIM_S2 As Single = 0   ' DB1 DBD210
    Public Shared Pose_ADJ1 As Single = 0     ' DB1 DBD286
    Public Shared Pose_ADJ2 As Single = 0     ' DB1 DBD290
    Public Shared Pose_ADJ3 As Single = 0     ' DB1 DBD294
    Public Shared Pose_ADJ4 As Single = 0     ' DB1 DBD298

    Public Shared CapMalaxeur As Single = 0   ' DB1 DBD916
    ' 22/09 : lus dans l'instantané (plus de lecture réseau depuis le Synoptique)
    Public Shared CorrEau As Integer = 0          ' DB1.DBW1194 (Int) correction eau
    Public Shared NbGacheeCalc As Single = 0      ' DB1.DBD1220 Real
    Public Shared CptGacheeAgr As Integer = 0     ' DB1.DBD1224 DInt
    Public Shared CptGacheeCim As Integer = 0     ' DB1.DBD1228 DInt
    Public Shared ResteGachee As Single = 0       ' DB1.DBD1232 Real
    Public Shared CptTremie(4) As Integer            ' 25/09 : DB1.DBW1196 / 1198 / 1200 / 1202 Int (compteurs trémies 1-4)
    Public Shared CapCamion As Single = 0     ' DB1 DBD920
    Public Shared NGacheeTotal As Integer = 0 ' DB1 DBD1078

    Public Shared Th_G1 As Single = 0         ' DB1 DBD1008
    Public Shared Th_SC As Single = 0         ' DB1 DBD1012
    Public Shared Th_SL As Single = 0         ' DB1 DBD1016
    Public Shared Th_D2 As Single = 0         ' DB1 DBD1020
    Public Shared Th_CIM As Single = 0        ' DB1 DBD1024
    Public Shared Th_EAU As Single = 0        ' DB1 DBD1028
    Public Shared Th_ADJ1 As Single = 0       ' DB1 DBD1032
    Public Shared Th_ADJ2 As Single = 0       ' DB1 DBD1036
    Public Shared Th_ADJ3 As Single = 0       ' DB1 DBD1040
    Public Shared Th_ADJ4 As Single = 0       ' DB1 DBD1044
    Public Shared NGacheeReste As Integer = 0 ' DB1 DBD1048

    Public Shared Reel_G1 As Single = 0       ' DB1 DBD924
    Public Shared Reel_SC As Single = 0       ' DB1 DBD928
    Public Shared Reel_SL As Single = 0       ' DB1 DBD932
    Public Shared Reel_D2 As Single = 0       ' DB1 DBD936
    Public Shared Reel_CIM As Single = 0      ' DB1 DBD940
    Public Shared Reel_EAU As Single = 0      ' DB1 DBD948
    Public Shared Reel_ADJ1 As Single = 0     ' DB1 DBD952
    Public Shared Reel_ADJ2 As Single = 0     ' DB1 DBD956
    Public Shared Reel_ADJ3 As Single = 0     ' DB1 DBD960
    Public Shared Reel_ADJ4 As Single = 0     ' DB1 DBD964

    Public Shared PctEcar_G1 As Single = 0    ' DB1 DBD968
    Public Shared PctEcar_SC As Single = 0    ' DB1 DBD972
    Public Shared PctEcar_SL As Single = 0    ' DB1 DBD976
    Public Shared PctEcar_D2 As Single = 0    ' DB1 DBD980
    Public Shared PctEcar_CIM As Single = 0   ' DB1 DBD984
    Public Shared PctEcar_EAU As Single = 0   ' DB1 DBD988
    Public Shared PctEcar_ADJ1 As Single = 0  ' DB1 DBD992
    Public Shared PctEcar_ADJ2 As Single = 0  ' DB1 DBD996
    Public Shared PctEcar_ADJ3 As Single = 0  ' DB1 DBD1000
    Public Shared PctEcar_ADJ4 As Single = 0  ' DB1 DBD1004

    Public Shared BitTr1 As Boolean = False       ' DB1.DBX12.0
    Public Shared BitTr2 As Boolean = False       ' DB1.DBX12.2
    Public Shared BitTr3 As Boolean = False       ' DB1.DBX12.4
    Public Shared BitTr4 As Boolean = False       ' DB1.DBX12.6
    Public Shared TapisMarche As Boolean = False  ' DB1.DBX16.4
    Public Shared BitMx As Boolean = False        ' DB1.DBX23.0
    Public Shared BitMxVert As Boolean = False    ' DB1.DBX30.1  → flèche verte malaxeur (19/09)
    Public Shared BitMxRouge As Boolean = False   ' DB1.DBX914.6 → flèche rouge malaxeur (19/09)
    Public Shared BitS1 As Boolean = False        ' DB1.DBX18.0
    Public Shared BitS2 As Boolean = False        ' DB1.DBX18.1
    Public Shared BitSl As Boolean = False        ' DB1.DBX19.4
    Public Shared SkipMonte As Boolean = False    ' DB1.DBX17.4
    Public Shared SkipDesc As Boolean = False     ' DB1.DBX17.5
    Public Shared BitEa1 As Boolean = False       ' DB1.DBX22.2
    Public Shared BitEau As Boolean = False       ' DB1.DBX22.4
    Public Shared BitAdj1 As Boolean = False      ' DB1.DBX21.0
    Public Shared BitAdj2 As Boolean = False      ' DB1.DBX21.1
    Public Shared BitAdj3 As Boolean = False      ' DB1.DBX21.2
    Public Shared BitAdj4 As Boolean = False      ' DB1.DBX21.3
    Public Shared CycleEnCours As Boolean = False ' DB1.DBX1088.1
    Public Shared BitEaadjj As Boolean = False    ' DB1.DBX22.1
    Public Shared MalaxeurON As Boolean = False   ' DB1.DBX22.5
    Public Shared MalaxeurPlein As Boolean = False ' DB1.DBX1082.2
    Public Shared ArretUrg As Boolean = False     ' DB1.DBX1082.1
    Public Shared SkipPlein As Boolean = False    ' DB1.DBX1082.3
    Public Shared SkipHaut As Boolean = False     ' DB1.DBX25.5
    Public Shared SkipAttente As Boolean = False  ' DB1.DBX25.6
    Public Shared SkipBas As Boolean = False      ' DB1.DBX25.7
    Public Shared ModeAuto As Boolean = False     ' DB1.DBX0.0
    Public Shared DefautActif As Boolean = False  ' DB1.DBX1082.0
    Public Shared FinChargement As Boolean = False ' DB1.DBX914.1
    Public Shared FinGachee As Boolean = False    ' DB1.DBX1082.4
    Public Shared VidangeMlx As Boolean = False   ' DB1.DBX914.5

    Public Shared ForceTapis As Boolean = False     ' M30.4 supprimé du CPU → reste False
    Public Shared ForceTrappeCim As Boolean = False ' M31.1
    Public Shared ForceMalaxeur As Boolean = False  ' M14.2

    Public Shared BitDens(12) As Boolean
    Public Shared NonIdentifies As New Dictionary(Of String, Single)

    Public Shared Connecte As Boolean = False
    Public Shared DerniereMAJ As DateTime = DateTime.MinValue
    Public Shared DerniereErreur As String = ""
    Public Shared DerniereErreurQuand As DateTime = DateTime.MinValue
    Public Shared EcrituresEnAttente As Integer = 0
    ' Durée de la dernière lecture complète (DB1 + zone M), en ms
    Public Shared DureeCycleMs As Integer = 0
End Class


Module ModulePLCDuplex

    ' True  : les commandes sont écrites dans DB1 (nouvelles adresses).
    ' False : les commandes sont écrites aux adresses d'origine (M / DB101…).
    Public ECRIRE_DANS_DB1 As Boolean = True

    Private ReadOnly _lockPLC As New Object()

    Private _threadLecture As Thread = Nothing
    Private _threadEcriture As Thread = Nothing
    Private _actif As Boolean = False
    Private _cycleCount As Long = 0

    ' Pause entre deux cycles : quasi nulle → cycle = temps réseau seul
    Public CadenceRapideMs As Integer = 50
    ' Erreurs de lecture consécutives avant reconnexion propre
    Private _erreursConsecutives As Integer = 0
    Private Const ERREURS_AVANT_RECONNEXION As Integer = 2
    ' Zone lente (530→1084 : théorique/réel camion, capacités) lue 1 cycle sur N
    Private Const CYCLES_ZONE_LENTE As Integer = 20
    ' Zone M (4 bits de forçage non migrés) lue 1 cycle sur N
    Private Const CYCLES_ZONE_M As Integer = 3
    Private Const FIN_ZONE_RAPIDE As Integer = 420
    Private Const AGE_MAX_INSTANTANE_MS As Integer = 2000
    Private Const TAILLE_DB1 As Integer = 1236   ' 22/09 : 1090 → 1240 ; 24/09 : 1236 = fin réelle du DB1 (dernier DBD1232).
                                                 ' Ne JAMAIS dépasser la taille du DB1 dans le CPU : S7.Net renvoie alors un tableau
                                                 ' tronqué sans erreur et tout ce qui est au-delà se lit à 0.
    ' Zone M : il ne reste que M30.4 (tapis) → 1 octet (MB30)
    Private Const M_DEBUT As Integer = 30
    Private Const M_TAILLE As Integer = 1

    Private _db1Snap() As Byte = Nothing
    Private _mbSnap() As Byte = Nothing

    Public Event NouvellesValeurs()

    Private Sub NoterErreur(msg As String)
        PLCData.DerniereErreur = msg
        PLCData.DerniereErreurQuand = DateTime.Now
        Debug.WriteLine("[PLC] " & msg)
    End Sub

    Private Function InstantaneFrais() As Boolean
        Return _db1Snap IsNot Nothing AndAlso
               (DateTime.Now - PLCData.DerniereMAJ).TotalMilliseconds < AGE_MAX_INSTANTANE_MS
    End Function

    Public Sub DemarrerDuplex()
        If _actif Then Exit Sub
        _actif = True

        _threadLecture = New Thread(AddressOf BoucleLecture)
        _threadLecture.IsBackground = True
        _threadLecture.Name = "ThreadPLCLecture"
        _threadLecture.Start()

        _threadEcriture = New Thread(AddressOf BoucleEcriture)
        _threadEcriture.IsBackground = True
        _threadEcriture.Name = "ThreadPLCEcriture"
        _threadEcriture.Start()
    End Sub

    Public Sub ArreterDuplex()
        _actif = False
        _signalEcriture.Set()
    End Sub

    Private Sub BoucleLecture()
        Do While _actif
            Try
                If Not PLCPret() Then
                    PLCData.Connecte = False
                    Try
                        SyncLock _lockPLC
                            FermerConnexion()
                            ConnecterPLCGlobal()
                        End SyncLock
                    Catch ex As Exception
                        NoterErreur("Connexion : " & ex.Message)
                    End Try
                    If Not PLCPret() Then
                        Thread.Sleep(1500)
                        Continue Do
                    End If
                    _erreursConsecutives = 0
                End If

                Dim plc As Plc = Module1.PLCInstance
                Dim chrono As Stopwatch = Stopwatch.StartNew()

                SyncLock _lockPLC
                    LireBlocDB1(plc)
                End SyncLock
                ' Zone M et DB18 supprimées du CPU (16/09) : plus de lecture complémentaire.

                PLCData.DureeCycleMs = CInt(chrono.ElapsedMilliseconds)
                _erreursConsecutives = 0
                PLCData.Connecte = True
                PLCData.DerniereMAJ = DateTime.Now
                _cycleCount += 1

                RaiseEvent NouvellesValeurs()

            Catch ex As Exception
                _erreursConsecutives += 1
                NoterErreur("Lecture DB1 (" & TAILLE_DB1 & " octets) : " & ex.Message &
                            "  [erreur " & _erreursConsecutives & "]  → si persistant : DB1 ≥ " & TAILLE_DB1 &
                            " octets ? « accès optimisé » décoché ? PUT/GET activé ? câble/switch ?")
                If _erreursConsecutives >= ERREURS_AVANT_RECONNEXION Then
                    ' Trame perdue une fois : on garde l'affichage. Deux fois : la socket
                    ' est probablement morte → fermer et rouvrir proprement.
                    PLCData.Connecte = False
                    SyncLock _lockPLC
                        FermerConnexion()
                    End SyncLock
                End If
                Thread.Sleep(300)
            End Try

            Thread.Sleep(CadenceRapideMs)
        Loop
    End Sub

    Private Sub LireBlocDB1(plc As Plc)

        ' Zone RAPIDE (0→420 : bits, poids, consignes, poses, écarts) : chaque cycle (2 trames)
        ' Zone RAPIDE bis (914→1084) : chaque cycle (1 trame)
        ' Zone LENTE  (530→610) : 1 cycle sur CYCLES_ZONE_LENTE (1 trame)
        Dim snap() As Byte = _db1Snap
        If snap Is Nothing OrElse snap.Length <> TAILLE_DB1 Then
            snap = plc.ReadBytes(DataType.DataBlock, 1, 0, TAILLE_DB1)   ' 1re fois : tout
            If snap Is Nothing OrElse snap.Length <> TAILLE_DB1 Then
                ' 24/09 : S7.Net renvoie un tableau tronqué (sans exception) quand on lit au-delà du DB1
                Throw New Exception("DB1 trop court : " & If(snap Is Nothing, 0, snap.Length) & " octets lus sur " & TAILLE_DB1 &
                                    " → le DB1 du CPU fait moins de " & TAILLE_DB1 & " octets (réduire TAILLE_DB1)")
            End If
        Else
            snap = CType(snap.Clone(), Byte())
            Dim rapide() As Byte = plc.ReadBytes(DataType.DataBlock, 1, 0, FIN_ZONE_RAPIDE)
            Array.Copy(rapide, 0, snap, 0, rapide.Length)
            ' 914→1084 : bits commandes (914.x), défaut/arrêt urgence/fin gâchée (1082.x),
            ' capacités, réel camion → aussi à chaque cycle (1 trame)
            Dim rapideB() As Byte = plc.ReadBytes(DataType.DataBlock, 1, 914, TAILLE_DB1 - 914)
            Array.Copy(rapideB, 0, snap, 914, rapideB.Length)
            If rapideB.Length <> TAILLE_DB1 - 914 Then
                Throw New Exception("DB1 trop court : " & (914 + rapideB.Length) & " octets lus sur " & TAILLE_DB1 &
                                    " → le DB1 du CPU fait moins de " & TAILLE_DB1 & " octets (réduire TAILLE_DB1)")
            End If
            If _cycleCount Mod CYCLES_ZONE_LENTE = 0 Then
                ' 530→610 : consignes formule camion (changent seulement à l'envoi d'un BL)
                Dim lentA() As Byte = plc.ReadBytes(DataType.DataBlock, 1, 530, 80)
                Array.Copy(lentA, 0, snap, 530, lentA.Length)
            End If
        End If
        _db1Snap = snap
        Dim db1() As Byte = snap

        PLCData.ModeAuto = ToBit(db1, 0, 0)
        PLCData.BitTr1 = ToBit(db1, 12, 0)
        PLCData.BitTr2 = ToBit(db1, 12, 2)
        PLCData.BitTr3 = ToBit(db1, 12, 4)
        PLCData.BitTr4 = ToBit(db1, 12, 6)
        PLCData.TapisMarche = ToBit(db1, 16, 4)
        PLCData.SkipMonte = ToBit(db1, 17, 4)
        PLCData.SkipDesc = ToBit(db1, 17, 5)
        PLCData.BitS1 = ToBit(db1, 18, 0)
        PLCData.BitS2 = ToBit(db1, 18, 1)
        PLCData.BitSl = ToBit(db1, 19, 4)
        PLCData.BitAdj1 = ToBit(db1, 21, 0)
        PLCData.BitAdj2 = ToBit(db1, 21, 1)
        PLCData.BitAdj3 = ToBit(db1, 21, 2)
        PLCData.BitAdj4 = ToBit(db1, 21, 3)
        PLCData.BitEaadjj = ToBit(db1, 22, 1)
        PLCData.BitEa1 = ToBit(db1, 22, 2)
        PLCData.BitEau = ToBit(db1, 22, 4)
        PLCData.MalaxeurON = ToBit(db1, 22, 5)
        PLCData.BitMx = ToBit(db1, 23, 0)
        PLCData.BitMxVert = Not ToBit(db1, 30, 1)   ' 19/09 : état inversé (0 = flèche verte)
        PLCData.BitMxRouge = ToBit(db1, 914, 6)
        PLCData.SkipHaut = ToBit(db1, 25, 5)
        PLCData.SkipAttente = ToBit(db1, 25, 6)
        PLCData.SkipBas = ToBit(db1, 25, 7)
        PLCData.FinChargement = ToBit(db1, 914, 1)
        PLCData.VidangeMlx = ToBit(db1, 914, 5)

        PLCData.DefautActif = ToBit(db1, 1082, 0)
        PLCData.ArretUrg = ToBit(db1, 1082, 1)
        PLCData.MalaxeurPlein = ToBit(db1, 1082, 2)
        PLCData.SkipPlein = ToBit(db1, 1082, 3)
        PLCData.FinGachee = ToBit(db1, 1082, 4)
        PLCData.CycleEnCours = ToBit(db1, 1088, 1)    ' ex M14.7
        PLCData.ForceMalaxeur = ToBit(db1, 1088, 2)   ' ex M14.2
        PLCData.ForceTrappeCim = ToBit(db1, 6, 5)     ' ex M31.1

        PLCData.BitDens(1) = ToBit(db1, 24, 0)
        PLCData.BitDens(2) = ToBit(db1, 24, 1)
        PLCData.BitDens(3) = ToBit(db1, 24, 2)
        PLCData.BitDens(4) = ToBit(db1, 24, 3)
        PLCData.BitDens(5) = ToBit(db1, 26, 0)
        PLCData.BitDens(6) = ToBit(db1, 25, 7)
        PLCData.BitDens(7) = ToBit(db1, 25, 6)
        PLCData.BitDens(8) = ToBit(db1, 25, 5)
        PLCData.BitDens(9) = ToBit(db1, 25, 4)
        PLCData.BitDens(10) = ToBit(db1, 29, 7)
        PLCData.BitDens(11) = ToBit(db1, 30, 1)
        PLCData.BitDens(12) = ToBit(db1, 28, 3)

        PLCData.Poids_CIM = ToFloat(db1, 78)
        PLCData.Poids_EAU = ToFloat(db1, 82)
        PLCData.Poids_AGR = ToFloat(db1, 86)
        PLCData.PoidsTapis = ToFloat(db1, 86)
        PLCData.Poids_ADJ = ToFloat(db1, 90)

        PLCData.Cons_CIM = ToFloat(db1, 94)
        PLCData.VisActif = ToInt16(db1, 52)
        PLCData.Cons_G1 = ToFloat(db1, 118)
        PLCData.Cons_SC = ToFloat(db1, 122)
        PLCData.Cons_SL = ToFloat(db1, 126)
        PLCData.Cons_D2 = ToFloat(db1, 130)
        PLCData.Cons_ADJ1 = ToFloat(db1, 174)
        PLCData.Cons_ADJ2 = ToFloat(db1, 178)

        PLCData.Cons_ADJ3 = ToFloat(db1, 182)
        PLCData.Cons_ADJ4 = ToFloat(db1, 186)

        PLCData.Pose_CIM_S1 = ToFloat(db1, 206)
        PLCData.Pose_CIM_S2 = ToFloat(db1, 210)
        PLCData.Pose_G1 = ToFloat(db1, 230)
        PLCData.Pose_SC = ToFloat(db1, 234)
        PLCData.Pose_SL = ToFloat(db1, 238)
        PLCData.Pose_D2 = ToFloat(db1, 242)
        PLCData.Pose_EAU2 = ToFloat(db1, 278)
        PLCData.Pose_ADJ1 = ToFloat(db1, 286)
        PLCData.Pose_ADJ2 = ToFloat(db1, 290)
        PLCData.Pose_ADJ3 = ToFloat(db1, 294)
        PLCData.Pose_ADJ4 = ToFloat(db1, 298)
        PLCData.Ecar_CIM = ToFloat(db1, 318)
        PLCData.Ecar_G1 = ToFloat(db1, 346)
        PLCData.Ecar_SC = ToFloat(db1, 350)
        PLCData.Ecar_SL = ToFloat(db1, 354)
        PLCData.Ecar_D2 = ToFloat(db1, 358)
        PLCData.Ecar_EAU2 = ToFloat(db1, 394)

        PLCData.Ecar_ADJ1 = ToFloat(db1, 402)
        PLCData.Ecar_ADJ2 = ToFloat(db1, 406)
        PLCData.Ecar_ADJ3 = ToFloat(db1, 410)
        PLCData.Ecar_ADJ4 = ToFloat(db1, 414)

        PLCData.CapMalaxeur = ToFloat(db1, 916)
        PLCData.CapCamion = ToFloat(db1, 920)
        PLCData.NGacheeTotal = CInt(ToFloat(db1, 1078))

        PLCData.Reel_G1 = ToFloat(db1, 924)
        PLCData.Reel_SC = ToFloat(db1, 928)
        PLCData.Reel_SL = ToFloat(db1, 932)
        PLCData.Reel_D2 = ToFloat(db1, 936)
        PLCData.Reel_CIM = ToFloat(db1, 940)
        PLCData.Reel_EAU = ToFloat(db1, 948)
        PLCData.Reel_ADJ1 = ToFloat(db1, 952)
        PLCData.Reel_ADJ2 = ToFloat(db1, 956)
        PLCData.Reel_ADJ3 = ToFloat(db1, 960)
        PLCData.Reel_ADJ4 = ToFloat(db1, 964)

        PLCData.PctEcar_G1 = ToFloat(db1, 968)
        PLCData.PctEcar_SC = ToFloat(db1, 972)
        PLCData.PctEcar_SL = ToFloat(db1, 976)
        PLCData.PctEcar_D2 = ToFloat(db1, 980)
        PLCData.PctEcar_CIM = ToFloat(db1, 984)
        PLCData.PctEcar_EAU = ToFloat(db1, 988)
        PLCData.PctEcar_ADJ1 = ToFloat(db1, 992)
        PLCData.PctEcar_ADJ2 = ToFloat(db1, 996)
        PLCData.PctEcar_ADJ3 = ToFloat(db1, 1000)
        PLCData.PctEcar_ADJ4 = ToFloat(db1, 1004)

        PLCData.Th_G1 = ToFloat(db1, 1008)
        PLCData.Th_SC = ToFloat(db1, 1012)
        PLCData.Th_SL = ToFloat(db1, 1016)
        PLCData.Th_D2 = ToFloat(db1, 1020)
        PLCData.Th_CIM = ToFloat(db1, 1024)
        PLCData.Th_EAU = ToFloat(db1, 1028)
        PLCData.Th_ADJ1 = ToFloat(db1, 1032)
        PLCData.Th_ADJ2 = ToFloat(db1, 1036)
        PLCData.Th_ADJ3 = ToFloat(db1, 1040)
        PLCData.Th_ADJ4 = ToFloat(db1, 1044)
        PLCData.NGacheeReste = CInt(ToFloat(db1, 1048))

        PLCData.ConsigneTapis = ToFloat(db1, 1052)
        PLCData.ComptCIMEAU = ToFloat(db1, 1068)
        PLCData.CompGachee = ToFloat(db1, 1072)

        PLCData.NGachee = ToInt16(db1, 1076)

        PLCData.Cons_EAU2 = ToFloat(db1, 166)        ' ex DB8.DBD20 (remplace DB18.DBD20 supprimé)
        ' 22/09 : correction eau + compteurs gâchées
        PLCData.CorrEau = ToInt16Signe(db1, 1194)
        PLCData.NbGacheeCalc = ToFloat(db1, 1220)
        PLCData.CptGacheeAgr = ToInt32(db1, 1224)
        PLCData.CptGacheeCim = ToInt32(db1, 1228)
        PLCData.ResteGachee = ToFloat(db1, 1232)
        For k As Integer = 1 To 4                              ' 25/09 : compteurs trémies
            PLCData.CptTremie(k) = ToInt16Signe(db1, 1196 + 2 * (k - 1))
        Next
        PLCData.NonIdentifies("DB1.DBD538 (ex DB101.DBD0 TR1)") = ToFloat(db1, 538)
        PLCData.NonIdentifies("DB1.DBD542 (ex DB101.DBD4 TR2)") = ToFloat(db1, 542)
        PLCData.NonIdentifies("DB1.DBD546 (ex DB101.DBD8 TR3)") = ToFloat(db1, 546)
        PLCData.NonIdentifies("DB1.DBD550 (ex DB101.DBD12 TR4)") = ToFloat(db1, 550)
        PLCData.NonIdentifies("DB1.DBD530 (ex DB101.DBD16 CIM)") = ToFloat(db1, 530)
        PLCData.NonIdentifies("DB1.DBD586 (ex DB101.DBD20 EAU)") = ToFloat(db1, 586)
        PLCData.NonIdentifies("DB1.DBD594 (ex DB101.DBD24 ADJ1)") = ToFloat(db1, 594)
        PLCData.NonIdentifies("DB1.DBD598 (ex DB101.DBD28 ADJ2)") = ToFloat(db1, 598)
        PLCData.NonIdentifies("DB1.DBD602 (ex DB101.DBD32 ADJ3)") = ToFloat(db1, 602)
        PLCData.NonIdentifies("DB1.DBD606 (ex DB101.DBD36 ADJ4)") = ToFloat(db1, 606)

    End Sub

    Private Sub LireMerkersNonMigres(plc As Plc)
        Dim mb() As Byte = plc.ReadBytes(DataType.Memory, 0, M_DEBUT, M_TAILLE)
        _mbSnap = mb
        PLCData.ForceTapis = ToBit(mb, 30 - M_DEBUT, 4)     ' M30.4 (en attente d'une position DB1 sans conflit)
    End Sub

    Private Sub LireBlocsComplement(plc As Plc)
        Try
            Dim db18_20() As Byte = plc.ReadBytes(DataType.DataBlock, 18, 20, 4)
            PLCData.Cons_EAU2 = ToFloat(db18_20, 0)
        Catch ex As Exception
            NoterErreur("DB18.DBD20 (Cons_EAU2) illisible : " & ex.Message)
        End Try
    End Sub

    ' ── Tables de traduction ancien → DB1 (aucun commentaire dans From { }) ──
    Private Structure BitLoc
        Public Octet As Integer
        Public Bit As Integer
        Public Sub New(o As Integer, b As Integer)
            Octet = o : Bit = b
        End Sub
    End Structure

    Private ReadOnly _mapBitsDB1 As New Dictionary(Of String, BitLoc)(StringComparer.OrdinalIgnoreCase) From {
        {"M9.1", New BitLoc(0, 0)}, {"M9.0", New BitLoc(0, 1)}, {"M4.5", New BitLoc(0, 4)},
        {"M30.0", New BitLoc(0, 5)}, {"M30.1", New BitLoc(0, 6)}, {"M30.2", New BitLoc(0, 7)},
        {"M30.3", New BitLoc(3, 5)}, {"M30.5", New BitLoc(4, 5)}, {"M30.6", New BitLoc(4, 6)},
        {"M30.7", New BitLoc(5, 1)}, {"M31.0", New BitLoc(5, 2)}, {"M1.6", New BitLoc(6, 5)},
        {"M31.4", New BitLoc(8, 1)}, {"M31.5", New BitLoc(8, 2)}, {"M31.6", New BitLoc(8, 3)},
        {"M31.7", New BitLoc(8, 4)}, {"M80.0", New BitLoc(9, 1)}, {"M32.0", New BitLoc(9, 2)},
        {"M31.2", New BitLoc(9, 3)}, {"M31.3", New BitLoc(9, 5)}, {"M32.1", New BitLoc(10, 0)},
        {"M32.2", New BitLoc(10, 2)}, {"M12.0", New BitLoc(12, 0)}, {"M12.1", New BitLoc(12, 2)},
        {"M12.2", New BitLoc(12, 4)}, {"M12.3", New BitLoc(12, 6)}, {"M12.4", New BitLoc(16, 4)},
        {"M13.5", New BitLoc(17, 4)}, {"M13.6", New BitLoc(17, 5)}, {"M13.2", New BitLoc(18, 0)},
        {"M13.3", New BitLoc(18, 1)}, {"M13.4", New BitLoc(19, 4)}, {"M14.3", New BitLoc(21, 0)},
        {"M14.4", New BitLoc(21, 1)}, {"M14.5", New BitLoc(21, 2)}, {"M14.6", New BitLoc(21, 3)},
        {"M19.3", New BitLoc(22, 0)}, {"M19.6", New BitLoc(22, 1)}, {"M13.7", New BitLoc(22, 2)},
        {"M14.1", New BitLoc(22, 4)}, {"M7.1", New BitLoc(22, 5)}, {"M15.0", New BitLoc(22, 6)},
        {"M15.1", New BitLoc(23, 0)}, {"M26.0", New BitLoc(24, 0)}, {"M26.1", New BitLoc(24, 1)},
        {"M26.2", New BitLoc(24, 2)}, {"M26.3", New BitLoc(24, 3)}, {"M25.3", New BitLoc(25, 4)},
        {"M25.0", New BitLoc(25, 5)}, {"M25.1", New BitLoc(25, 6)}, {"M25.2", New BitLoc(25, 7)},
        {"M25.4", New BitLoc(26, 0)}, {"M5.3", New BitLoc(28, 3)}, {"M6.7", New BitLoc(29, 7)},
        {"M7.0", New BitLoc(30, 1)}, {"M23.0", New BitLoc(32, 0)}, {"M23.1", New BitLoc(32, 1)},
        {"M23.2", New BitLoc(32, 2)}, {"M23.3", New BitLoc(32, 3)}, {"M21.4", New BitLoc(33, 4)},
        {"M21.5", New BitLoc(33, 5)}, {"M21.6", New BitLoc(33, 6)}, {"M21.7", New BitLoc(33, 7)},
        {"M23.4", New BitLoc(36, 5)}, {"M22.0", New BitLoc(37, 3)}, {"M22.2", New BitLoc(40, 1)},
        {"M22.3", New BitLoc(40, 2)}, {"M22.5", New BitLoc(40, 3)}, {"M22.6", New BitLoc(40, 4)},
        {"M23.5", New BitLoc(41, 2)}, {"M22.1", New BitLoc(41, 3)}, {"M23.7", New BitLoc(914, 0)},
        {"M50.3", New BitLoc(914, 1)}, {"M50.4", New BitLoc(914, 2)}, {"M51.0", New BitLoc(914, 3)},
        {"M8.2", New BitLoc(914, 4)}, {"M58.3", New BitLoc(914, 5)},
        {"M5.0", New BitLoc(1082, 0)}, {"M20.2", New BitLoc(1082, 1)}, {"M57.0", New BitLoc(1082, 2)},
        {"M57.1", New BitLoc(1082, 3)}, {"M64.6", New BitLoc(1082, 4)},
        {"M14.7", New BitLoc(1088, 1)}, {"M14.2", New BitLoc(1088, 2)}, {"M31.1", New BitLoc(6, 5)}
    }

    Private ReadOnly _mapRealDB1 As New Dictionary(Of String, Integer)(StringComparer.OrdinalIgnoreCase) From {
        {"4.0", 86}, {"4.4", 78}, {"4.8", 82}, {"4.98", 402}, {"4.102", 406}, {"4.114", 410}, {"4.118", 414},
        {"8.0", 118}, {"8.4", 122}, {"8.8", 126}, {"8.12", 130}, {"8.16", 94}, {"8.20", 166}, {"8.216", 174}, {"8.228", 178},
        {"8.126", 1052}, {"8.240", 1068}, {"8.330", 1072},
        {"78.0", 182}, {"78.4", 186},
        {"18.2", 230}, {"18.6", 234}, {"18.10", 238}, {"18.14", 242}, {"18.18", 206}, {"18.22", 278},
        {"18.26", 346}, {"18.30", 350}, {"18.34", 354}, {"18.38", 358}, {"18.42", 318}, {"18.46", 394},
        {"18.58", 210}, {"18.62", 286}, {"18.66", 290}, {"18.70", 294}, {"18.74", 298},
        {"90.4", 90},
        {"19.58", 916}, {"19.66", 920}, {"19.70", 1078},
        {"65.0", 924}, {"65.8", 928}, {"65.16", 932}, {"65.24", 936}, {"65.32", 940},
        {"65.48", 948}, {"65.56", 952}, {"65.64", 956}, {"65.80", 960}, {"65.88", 964},
        {"101.0", 538}, {"101.4", 542}, {"101.8", 546}, {"101.12", 550}, {"101.16", 530},
        {"101.20", 586}, {"101.24", 594}, {"101.28", 598}, {"101.32", 602}, {"101.36", 606},
        {"101.80", 1008}, {"101.84", 1012}, {"101.88", 1016}, {"101.92", 1020},
        {"101.96", 1024}, {"101.100", 1028}, {"101.104", 1032}, {"101.108", 1036},
        {"101.112", 1040}, {"101.116", 1044}, {"101.120", 1048},
        {"114.0", 968}, {"114.4", 972}, {"114.8", 976}, {"114.12", 980}, {"114.16", 984},
        {"114.20", 988}, {"114.24", 992}, {"114.28", 996}, {"114.32", 1000}, {"114.36", 1004}
    }

    Private ReadOnly _mapWordDB1 As New Dictionary(Of String, String)(StringComparer.OrdinalIgnoreCase) From {
        {"DB8.DBW324", "DB1.DBW52"},
        {"DB18.DBW0", "DB1.DBW1076"},
        {"DB19.DBW58", "DB1.DBW916"},
        {"DB19.DBW66", "DB1.DBW920"}
    }

    ' ── Résolution d'adresse bit ─────────────────────────────────
    Private Structure BitCible
        Public Zone As DataType
        Public Db As Integer
        Public Octet As Integer
        Public Bit As Integer
    End Structure

    Private ReadOnly _rxDBX As New Regex("^DB(\d+)\.DBX(\d+)\.([0-7])$", RegexOptions.IgnoreCase)
    Private ReadOnly _rxM As New Regex("^M(\d+)\.([0-7])$", RegexOptions.IgnoreCase)

    Private Function ResoudreBitLecture(adresse As String, ByRef cible As BitCible) As Boolean
        If String.IsNullOrWhiteSpace(adresse) Then Return False
        Dim a As String = adresse.Trim().Replace(" ", "")
        Dim loc As BitLoc
        If _mapBitsDB1.TryGetValue(a, loc) Then
            cible = New BitCible With {.Zone = DataType.DataBlock, .Db = 1, .Octet = loc.Octet, .Bit = loc.Bit}
            Return True
        End If
        Return ResoudreBitBrut(a, cible)
    End Function

    Private Function ResoudreBitEcriture(adresse As String, ByRef cible As BitCible) As Boolean
        If ECRIRE_DANS_DB1 Then Return ResoudreBitLecture(adresse, cible)
        If String.IsNullOrWhiteSpace(adresse) Then Return False
        Return ResoudreBitBrut(adresse.Trim().Replace(" ", ""), cible)
    End Function

    Private Function ResoudreBitBrut(a As String, ByRef cible As BitCible) As Boolean
        Dim m As Match = _rxDBX.Match(a)
        If m.Success Then
            cible = New BitCible With {.Zone = DataType.DataBlock, .Db = CInt(m.Groups(1).Value),
                                       .Octet = CInt(m.Groups(2).Value), .Bit = CInt(m.Groups(3).Value)}
            Return True
        End If
        m = _rxM.Match(a)
        If m.Success Then
            cible = New BitCible With {.Zone = DataType.Memory, .Db = 0,
                                       .Octet = CInt(m.Groups(1).Value), .Bit = CInt(m.Groups(2).Value)}
            Return True
        End If
        Return False
    End Function

    Private Function AdresseS7(c As BitCible) As String
        If c.Zone = DataType.DataBlock Then Return "DB" & c.Db & ".DBX" & c.Octet & "." & c.Bit
        Return "M" & c.Octet & "." & c.Bit
    End Function

    Public Function AdresseWordReelle(adresse As String) As String
        Dim a As String = adresse.Trim().Replace(" ", "")
        Dim cible As String = Nothing
        If _mapWordDB1.TryGetValue(a, cible) Then Return cible
        Return a
    End Function

    Public Function EstMigree(adresse As String) As Boolean
        Dim c As BitCible
        Return ResoudreBitLecture(adresse, c) AndAlso c.Zone = DataType.DataBlock
    End Function

    Public Function AdresseReelle(adresse As String) As String
        Dim e As BitCible, l As BitCible
        If Not ResoudreBitEcriture(adresse, e) Then Return adresse & " (?)"
        Dim s As String = AdresseS7(e)
        If ResoudreBitLecture(adresse, l) AndAlso AdresseS7(l) <> s Then
            s &= " (lu " & AdresseS7(l) & ")"
        End If
        Return s
    End Function

    ' ── File d'écriture (thread dédié) ───────────────────────────
    Private Structure DemandeBit
        Public CibleEcriture As BitCible
        Public CibleLecture As BitCible
        Public ALecture As Boolean
        Public Valeur As Boolean
        Public Libelle As String
    End Structure

    Private ReadOnly _fileBits As New ConcurrentQueue(Of DemandeBit)
    Private ReadOnly _signalEcriture As New AutoResetEvent(False)

    Private Sub BoucleEcriture()
        Do While _actif
            _signalEcriture.WaitOne(500)
            Dim d As DemandeBit
            Do While _fileBits.TryDequeue(d)
                PLCData.EcrituresEnAttente = _fileBits.Count
                ExecuterEcritureBit(d)
            Loop
        Loop
    End Sub

    Private Sub ExecuterEcritureBit(d As DemandeBit)
        Dim c As BitCible = d.CibleEcriture
        Try
            If Not PLCPret() Then
                NoterErreur("EcrireBit " & d.Libelle & " : PLC non connecté")
                Exit Sub
            End If
            SyncLock _lockPLC
                Dim plc As Plc = Module1.PLCInstance
                Dim b() As Byte = plc.ReadBytes(c.Zone, c.Db, c.Octet, 1)
                If b Is Nothing OrElse b.Length < 1 Then
                    NoterErreur("EcrireBit " & AdresseS7(c) & " : lecture de l'octet impossible")
                    Exit Sub
                End If
                Dim masque As Byte = CByte(1 << c.Bit)
                If d.Valeur Then
                    b(0) = CByte(b(0) Or masque)
                Else
                    b(0) = CByte(b(0) And (Not masque))
                End If
                plc.WriteBytes(c.Zone, c.Db, c.Octet, b)
            End SyncLock

            MettreAJourInstantane(c, d.Valeur)
            If d.ALecture Then MettreAJourInstantane(d.CibleLecture, d.Valeur)

        Catch ex As Exception
            NoterErreur("EcrireBit " & AdresseS7(c) & " : " & ex.Message)
        End Try
    End Sub

    Private Sub MettreAJourInstantane(c As BitCible, valeur As Boolean)
        Try
            Dim masque As Byte = CByte(1 << c.Bit)
            If c.Zone = DataType.DataBlock AndAlso c.Db = 1 Then
                Dim s() As Byte = _db1Snap
                If s IsNot Nothing AndAlso c.Octet < s.Length Then
                    s(c.Octet) = If(valeur, CByte(s(c.Octet) Or masque), CByte(s(c.Octet) And (Not masque)))
                End If
            ElseIf c.Zone = DataType.Memory Then
                Dim s() As Byte = _mbSnap
                Dim idx As Integer = c.Octet - M_DEBUT
                If s IsNot Nothing AndAlso idx >= 0 AndAlso idx < s.Length Then
                    s(idx) = If(valeur, CByte(s(idx) Or masque), CByte(s(idx) And (Not masque)))
                End If
            End If
        Catch
        End Try
    End Sub

    ' ── Adresses SUPPRIMÉES du CPU (16/09) : ignorées partout ──────
    Private ReadOnly _adressesSupprimees As New HashSet(Of String)(StringComparer.OrdinalIgnoreCase) From {"M30.4", "M1.1", "M0.0", "M0.1", "M0.2", "M1.0", "M2.0"}

    Public Function EstSupprimee(adresse As String) As Boolean
        If String.IsNullOrEmpty(adresse) Then Return False
        Return _adressesSupprimees.Contains(adresse.Trim())
    End Function

    ' ── Écritures (jamais bloquantes pour l'UI) ─────────────────
    Public Function EcrireBit(adresse As String, valeur As Boolean) As Boolean
        Dim ce As BitCible, cl As BitCible
        If EstSupprimee(adresse) Then Return False   ' adresse absente du CPU : on ignore sans erreur
        If Not PLCPret() Then
            NoterErreur("EcrireBit " & adresse & " : PLC non connecté")
            Return False
        End If
        If Not ResoudreBitEcriture(adresse, ce) Then
            NoterErreur("EcrireBit : adresse invalide « " & adresse & " »")
            Return False
        End If
        Dim aLecture As Boolean = ResoudreBitLecture(adresse, cl)
        _fileBits.Enqueue(New DemandeBit With {
            .CibleEcriture = ce, .CibleLecture = cl, .ALecture = aLecture,
            .Valeur = valeur, .Libelle = adresse})
        PLCData.EcrituresEnAttente = _fileBits.Count
        _signalEcriture.Set()
        Return True
    End Function

    Public Function EcrireInt(adresse As String, valeur As Integer) As Boolean
        Dim adrEcrite As String = If(ECRIRE_DANS_DB1, AdresseWordReelle(adresse), adresse.Trim())
        Try
            If Not PLCPret() Then Return False
            SyncLock _lockPLC
                Module1.PLCInstance.Write(adrEcrite, CShort(valeur))
            End SyncLock
            Return True
        Catch ex As Exception
            NoterErreur("EcrireInt " & adrEcrite & " : " & ex.Message)
            Return False
        End Try
    End Function

    Public Function EcrireReal(db As Integer, offset As Integer,
                               valeur As Single) As Boolean
        Dim dbCible As Integer = db
        Dim offsetCible As Integer = offset
        Try
            If Not PLCPret() Then Return False

            If ECRIRE_DANS_DB1 Then
                Dim offsetDB1 As Integer
                If _mapRealDB1.TryGetValue(db & "." & offset, offsetDB1) Then
                    dbCible = 1
                    offsetCible = offsetDB1
                End If
            End If

            Dim b() As Byte = BitConverter.GetBytes(valeur)
            Dim octets() As Byte = {b(3), b(2), b(1), b(0)}
            SyncLock _lockPLC
                Module1.PLCInstance.WriteBytes(
                    DataType.DataBlock, dbCible, offsetCible, octets)
            End SyncLock
            Return True
        Catch ex As Exception
            NoterErreur("EcrireReal DB" & dbCible & ".DBD" & offsetCible & " : " & ex.Message)
            Return False
        End Try
    End Function

    ' 18/09 : écriture d'un DINT (ex. variables TIME en ms) dans un DB quelconque
    Public Function EcrireDInt(db As Integer, offset As Integer, valeur As Integer) As Boolean
        Try
            If Not PLCPret() Then Return False
            Dim b() As Byte = BitConverter.GetBytes(valeur)
            Dim octets() As Byte = {b(3), b(2), b(1), b(0)}
            SyncLock _lockPLC
                Module1.PLCInstance.WriteBytes(DataType.DataBlock, db, offset, octets)
            End SyncLock
            Return True
        Catch ex As Exception
            NoterErreur("EcrireDInt DB" & db & ".DBD" & offset & " : " & ex.Message)
            Return False
        End Try
    End Function

    Public Sub EcrireImpulsion(adresse As String,
                               Optional dureeMs As Integer = 200)
        If EstSupprimee(adresse) Then Exit Sub
        Dim t As New Thread(Sub()
                                EcrireBit(adresse, True)
                                Thread.Sleep(dureeMs)
                                EcrireBit(adresse, False)
                            End Sub)
        t.IsBackground = True
        t.Start()
    End Sub

    ' ── Lectures ponctuelles (instantané DB1 → 0 réseau) ────────
    Public Function LireAdresse(adresse As String) As Object
        Try
            If EstSupprimee(adresse) Then Return False
            If Not PLCPret() Then Return Nothing

            Dim c As BitCible
            If ResoudreBitLecture(adresse, c) Then
                If InstantaneFrais() Then
                    If c.Zone = DataType.DataBlock AndAlso c.Db = 1 Then
                        Dim s() As Byte = _db1Snap
                        If s IsNot Nothing AndAlso c.Octet < s.Length Then Return ToBit(s, c.Octet, c.Bit)
                    ElseIf c.Zone = DataType.Memory Then
                        Dim s() As Byte = _mbSnap
                        Dim idx As Integer = c.Octet - M_DEBUT
                        If s IsNot Nothing AndAlso idx >= 0 AndAlso idx < s.Length Then Return ToBit(s, idx, c.Bit)
                    End If
                End If
                SyncLock _lockPLC
                    Dim b() As Byte = Module1.PLCInstance.ReadBytes(c.Zone, c.Db, c.Octet, 1)
                    Return ToBit(b, 0, c.Bit)
                End SyncLock
            End If

            SyncLock _lockPLC
                Return Module1.PLCInstance.Read(AdresseWordReelle(adresse))
            End SyncLock
        Catch ex As Exception
            NoterErreur("LireAdresse " & adresse & " : " & ex.Message)
            Return Nothing
        End Try
    End Function

    Public Function LireDBDReal(db As Integer, offset As Integer) As Single
        Try
            If Not PLCPret() Then Return 0.0F

            Dim offsetDB1 As Integer = offset
            Dim dbLu As Integer = db
            If db = 1 Then
                offsetDB1 = offset
            ElseIf _mapRealDB1.TryGetValue(db & "." & offset, offsetDB1) Then
                dbLu = 1
            End If

            If dbLu = 1 AndAlso InstantaneFrais() Then
                Dim s() As Byte = _db1Snap
                If s IsNot Nothing AndAlso offsetDB1 + 3 < s.Length Then Return ToFloat(s, offsetDB1)
            End If

            SyncLock _lockPLC
                Dim b() As Byte = Module1.PLCInstance.ReadBytes(DataType.DataBlock, dbLu, If(dbLu = 1, offsetDB1, offset), 4)
                Return ToFloat(b, 0)
            End SyncLock
        Catch ex As Exception
            NoterErreur("LireDBDReal DB" & db & "." & offset & " : " & ex.Message)
            Return 0.0F
        End Try
    End Function

    ' Ferme la connexion partagée (elle sera recréée par ConnecterPLCGlobal)
    Private Sub FermerConnexion()
        Try
            If Module1.PLCInstance IsNot Nothing Then Module1.PLCInstance.Close()
        Catch
        End Try
        Module1.PLCInstance = Nothing
    End Sub

    ' Lecture brute d'un bloc de DB (autre que DB1), sous verrou.
    ' Utilisé par ModuleGacheesDB71. Nothing si erreur (voir DerniereErreur).
    Public Function LireBytesDB(db As Integer, debut As Integer, nb As Integer) As Byte()
        Try
            If Not PLCPret() Then Return Nothing
            SyncLock _lockPLC
                Return Module1.PLCInstance.ReadBytes(DataType.DataBlock, db, debut, nb)
            End SyncLock
        Catch ex As Exception
            NoterErreur("LireBytesDB DB" & db & "." & debut & " (" & nb & ") : " & ex.Message)
            Return Nothing
        End Try
    End Function

    Private Function PLCPret() As Boolean
        Return Module1.PLCInstance IsNot Nothing AndAlso
               Module1.PLCInstance.IsConnected
    End Function

    Private Function ToFloat(data() As Byte, offset As Integer) As Single
        If data Is Nothing OrElse offset + 3 >= data.Length Then Return 0
        Dim b() As Byte = {data(offset + 3), data(offset + 2),
                           data(offset + 1), data(offset)}
        Return BitConverter.ToSingle(b, 0)
    End Function

    Private Function ToInt16Signe(data() As Byte, offset As Integer) As Integer
        If data Is Nothing OrElse offset + 1 >= data.Length Then Return 0
        Return BitConverter.ToInt16({data(offset + 1), data(offset)}, 0)
    End Function

    Private Function ToInt32(data() As Byte, offset As Integer) As Integer
        If data Is Nothing OrElse offset + 3 >= data.Length Then Return 0
        Return BitConverter.ToInt32({data(offset + 3), data(offset + 2), data(offset + 1), data(offset)}, 0)
    End Function

    ' 22/09 : copie d'une zone de l'instantané DB1 (aucun accès réseau, aucun verrou)
    Public Function LireInstantaneDB1(debut As Integer, nb As Integer) As Byte()
        Dim s() As Byte = _db1Snap
        If s Is Nothing OrElse debut < 0 OrElse debut + nb > s.Length Then Return Nothing
        Dim r(nb - 1) As Byte
        Array.Copy(s, debut, r, 0, nb)
        Return r
    End Function

    Private Function ToInt16(data() As Byte, offset As Integer) As Integer
        If data Is Nothing OrElse offset + 1 >= data.Length Then Return 0
        Return CInt(data(offset)) * 256 + CInt(data(offset + 1))
    End Function

    Private Function ToBit(data() As Byte, byteOff As Integer,
                           bitOff As Integer) As Boolean
        If data Is Nothing OrElse byteOff < 0 OrElse byteOff >= data.Length Then Return False
        Return (data(byteOff) And (1 << bitOff)) <> 0
    End Function

End Module
