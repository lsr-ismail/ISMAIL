Imports System.Data
Imports System.Linq
Imports Microsoft.Data.SqlClient

' ================================================================
'  ModuleDefautsCPU.vb — 22/09/2026
'  Défauts CPU (structures det / DFT TIMEOUT / dFT disj, octets 24 à 51)
'  • Textes ("commentaires") : dbo.AlarmesConfig (VariableDeclenchement =
'    adresse "DBx.DBXoo.b", Texte, Classe DET/TIMEOUT/DISJ). Remplie
'    automatiquement avec la liste ci-dessous si elle est vide.
'  • Historique : dbo.Alarmes (DateHeure, CodeAlarme = adresse,
'    Description = commentaire, Etat ACTIVE/RESOLUE, TypeAlarme = classe,
'    DateResolution) — lu par l'écran « Alarmes machine ».
'  • Scruter() : à appeler chaque seconde (Synoptique) ; Actifs = liste
'    des défauts présents (date d'apparition, adresse, commentaire).
' ================================================================
Module ModuleDefautsCPU

    ' ★ Numéro du DB contenant la structure des défauts (octets 24..51)
    Public Const DB_DEFAUTS As Integer = 1
    ' 22/09 : le groupe DET (24.0-30.2) = état des détecteurs, PAS des défauts →
    '         ignoré par la scrutation (seuls TIMEOUT 32.0-42.3 et DISJ 44.0-51.5 comptent)
    Public Const SURVEILLER_DET As Boolean = False
    Private Const OCTET_DEBUT As Integer = 24
    Private Const NB_OCTETS As Integer = 28

    Public Class Defaut
        Public Numero As Integer
        Public Octet As Integer
        Public Bit As Integer
        Public Classe As String
        Public Texte As String
        Public Actif As Boolean
        Public DateApparition As Date
        Public ReadOnly Property Adresse As String
            Get
                Return "DB" & DB_DEFAUTS & ".DBX" & Octet & "." & Bit
            End Get
        End Property
    End Class

    Private ReadOnly _liste As New List(Of Defaut)
    Private _initialise As Boolean = False
    Private _precedent() As Byte = Nothing
    Private ReadOnly _verrou As New Object()

    Private Sub AjoutDefaut(num As Integer, octet As Integer, bit As Integer, classe As String, texte As String)
        _liste.Add(New Defaut With {.Numero = num, .Octet = octet, .Bit = bit, .Classe = classe, .Texte = texte})
    End Sub

    ' ── Liste des défauts (adresses TIA) ─────────────────────────
    Private Sub ConstruireListe()
        _liste.Clear()
        AjoutDefaut(1, 24, 0, "DET", "Détecteur agrégat 1")
        AjoutDefaut(2, 24, 1, "DET", "Détecteur agrégat 2")
        AjoutDefaut(3, 24, 2, "DET", "Détecteur agrégat 3")
        AjoutDefaut(4, 24, 3, "DET", "Détecteur agrégat 4")
        AjoutDefaut(5, 24, 4, "DET", "Détecteur agrégat 5")
        AjoutDefaut(6, 24, 5, "DET", "Détecteur agrégat 6")
        AjoutDefaut(7, 24, 6, "DET", "Détecteur agrégat 7")
        AjoutDefaut(8, 24, 7, "DET", "Détecteur agrégat 8")
        AjoutDefaut(9, 25, 0, "DET", "Détecteur agrégat 9")
        AjoutDefaut(10, 25, 1, "DET", "Détecteur agrégat 10")
        AjoutDefaut(11, 25, 2, "DET", "Détecteur agrégat 11")
        AjoutDefaut(12, 25, 3, "DET", "Détecteur agrégat 12")
        AjoutDefaut(13, 25, 4, "DET", "Détecteur sécurité haut skip")
        AjoutDefaut(14, 25, 5, "DET", "Détecteur haut skip")
        AjoutDefaut(15, 25, 6, "DET", "Détecteur attente skip")
        AjoutDefaut(16, 25, 7, "DET", "Détecteur bas skip")
        AjoutDefaut(17, 26, 0, "DET", "Détecteur sécurité bas skip")
        AjoutDefaut(18, 26, 1, "DET", "Détecteur ouverture trémie tampon")
        AjoutDefaut(19, 26, 2, "DET", "Détecteur fermeture trémie tampon")
        AjoutDefaut(20, 26, 3, "DET", "Détecteur silo haut 1")
        AjoutDefaut(21, 26, 4, "DET", "Détecteur silo haut 2")
        AjoutDefaut(22, 26, 5, "DET", "Détecteur silo haut 3")
        AjoutDefaut(23, 26, 6, "DET", "Détecteur silo haut 4")
        AjoutDefaut(24, 26, 7, "DET", "Détecteur silo haut 5")
        AjoutDefaut(25, 27, 0, "DET", "Détecteur silo haut 6")
        AjoutDefaut(26, 27, 1, "DET", "Détecteur silo bas 1")
        AjoutDefaut(27, 27, 2, "DET", "Détecteur silo bas 2")
        AjoutDefaut(28, 27, 3, "DET", "Détecteur silo bas 3")
        AjoutDefaut(29, 27, 4, "DET", "Détecteur silo bas 4")
        AjoutDefaut(30, 27, 5, "DET", "Détecteur silo bas 5")
        AjoutDefaut(31, 27, 6, "DET", "Détecteur silo bas 6")
        AjoutDefaut(32, 27, 7, "DET", "Détecteur ouverture bascule ciment 1")
        AjoutDefaut(33, 28, 0, "DET", "Détecteur ouverture bascule ciment 2")
        AjoutDefaut(34, 28, 1, "DET", "Détecteur ouverture bascule ciment 3")
        AjoutDefaut(35, 28, 2, "DET", "Détecteur ouverture bascule ciment 4")
        AjoutDefaut(36, 28, 3, "DET", "Détecteur fermeture bascule ciment 1")
        AjoutDefaut(37, 28, 4, "DET", "Détecteur compteur adjuvant 1")
        AjoutDefaut(38, 28, 5, "DET", "Détecteur compteur adjuvant 2")
        AjoutDefaut(39, 28, 6, "DET", "Détecteur compteur adjuvant 3")
        AjoutDefaut(40, 28, 7, "DET", "Détecteur compteur adjuvant 4")
        AjoutDefaut(41, 29, 0, "DET", "Détecteur compteur adjuvant 5")
        AjoutDefaut(42, 29, 1, "DET", "Détecteur compteur adjuvant 6")
        AjoutDefaut(43, 29, 2, "DET", "Détecteur compteur adjuvant 7")
        AjoutDefaut(44, 29, 3, "DET", "Détecteur compteur adjuvant 8")
        AjoutDefaut(45, 29, 4, "DET", "Détecteur fermeture trémie adjuvant")
        AjoutDefaut(46, 29, 5, "DET", "Détecteur compteur eau")
        AjoutDefaut(47, 29, 6, "DET", "Détecteur fermeture bascule eau")
        AjoutDefaut(48, 29, 7, "DET", "Détecteur ouverture trappe malaxeur 1")
        AjoutDefaut(49, 30, 0, "DET", "Détecteur ouverture trappe malaxeur 2")
        AjoutDefaut(50, 30, 1, "DET", "Détecteur fermeture trappe malaxeur 1")
        AjoutDefaut(51, 30, 2, "DET", "Détecteur fermeture trappe malaxeur 2")
        AjoutDefaut(52, 32, 0, "TIMEOUT", "Défaut tolérance agrégat 1")
        AjoutDefaut(53, 32, 1, "TIMEOUT", "Défaut tolérance agrégat 2")
        AjoutDefaut(54, 32, 2, "TIMEOUT", "Défaut tolérance agrégat 3")
        AjoutDefaut(55, 32, 3, "TIMEOUT", "Défaut tolérance agrégat 4")
        AjoutDefaut(56, 32, 4, "TIMEOUT", "Défaut tolérance agrégat 5")
        AjoutDefaut(57, 32, 5, "TIMEOUT", "Défaut tolérance agrégat 6")
        AjoutDefaut(58, 32, 6, "TIMEOUT", "Défaut tolérance agrégat 7")
        AjoutDefaut(59, 32, 7, "TIMEOUT", "Défaut tolérance agrégat 8")
        AjoutDefaut(60, 33, 0, "TIMEOUT", "Défaut tolérance agrégat 9")
        AjoutDefaut(61, 33, 1, "TIMEOUT", "Défaut tolérance agrégat 10")
        AjoutDefaut(62, 33, 2, "TIMEOUT", "Défaut tolérance agrégat 11")
        AjoutDefaut(63, 33, 3, "TIMEOUT", "Défaut tolérance agrégat 12")
        AjoutDefaut(64, 33, 4, "TIMEOUT", "Time-out agrégat 1")
        AjoutDefaut(65, 33, 5, "TIMEOUT", "Time-out agrégat 2")
        AjoutDefaut(66, 33, 6, "TIMEOUT", "Time-out agrégat 3")
        AjoutDefaut(67, 33, 7, "TIMEOUT", "Time-out agrégat 4")
        AjoutDefaut(68, 34, 0, "TIMEOUT", "Time-out agrégat 5")
        AjoutDefaut(69, 34, 1, "TIMEOUT", "Time-out agrégat 6")
        AjoutDefaut(70, 34, 2, "TIMEOUT", "Time-out agrégat 7")
        AjoutDefaut(71, 34, 3, "TIMEOUT", "Time-out agrégat 8")
        AjoutDefaut(72, 34, 4, "TIMEOUT", "Time-out agrégat 9")
        AjoutDefaut(73, 34, 5, "TIMEOUT", "Time-out agrégat 10")
        AjoutDefaut(74, 34, 6, "TIMEOUT", "Time-out agrégat 11")
        AjoutDefaut(75, 34, 7, "TIMEOUT", "Time-out agrégat 12")
        AjoutDefaut(76, 35, 0, "TIMEOUT", "Time-out tapis peseur 1")
        AjoutDefaut(77, 35, 1, "TIMEOUT", "Time-out tapis peseur 2")
        AjoutDefaut(78, 35, 2, "TIMEOUT", "Time-out tapis peseur 3")
        AjoutDefaut(79, 35, 3, "TIMEOUT", "Time-out tapis reprise 1")
        AjoutDefaut(80, 35, 4, "TIMEOUT", "Time-out tapis reprise 2")
        AjoutDefaut(81, 35, 5, "TIMEOUT", "Time-out montée skip")
        AjoutDefaut(82, 35, 6, "TIMEOUT", "Time-out descente skip")
        AjoutDefaut(83, 35, 7, "TIMEOUT", "Défaut skip haut")
        AjoutDefaut(84, 36, 0, "TIMEOUT", "Défaut skip attente")
        AjoutDefaut(85, 36, 1, "TIMEOUT", "Défaut skip bas")
        AjoutDefaut(86, 36, 2, "TIMEOUT", "Défaut fins de course skip haut/bas")
        AjoutDefaut(87, 36, 3, "TIMEOUT", "Time-out ouverture trémie tampon")
        AjoutDefaut(88, 36, 4, "TIMEOUT", "Time-out fermeture trémie tampon")
        AjoutDefaut(89, 36, 5, "TIMEOUT", "Défaut tolérance vis 1")
        AjoutDefaut(90, 36, 6, "TIMEOUT", "Défaut tolérance vis 2")
        AjoutDefaut(91, 36, 7, "TIMEOUT", "Défaut tolérance vis 3")
        AjoutDefaut(92, 37, 0, "TIMEOUT", "Défaut tolérance vis 4")
        AjoutDefaut(93, 37, 1, "TIMEOUT", "Défaut tolérance vis 5")
        AjoutDefaut(94, 37, 2, "TIMEOUT", "Défaut tolérance vis 6")
        AjoutDefaut(95, 37, 3, "TIMEOUT", "Time-out vis ciment 1")
        AjoutDefaut(96, 37, 4, "TIMEOUT", "Time-out vis ciment 2")
        AjoutDefaut(97, 37, 5, "TIMEOUT", "Time-out vis ciment 3")
        AjoutDefaut(98, 37, 6, "TIMEOUT", "Time-out vis ciment 4")
        AjoutDefaut(99, 37, 7, "TIMEOUT", "Time-out vis ciment 5")
        AjoutDefaut(100, 38, 0, "TIMEOUT", "Time-out vis ciment 6")
        AjoutDefaut(101, 38, 1, "TIMEOUT", "Time-out ouverture bascule ciment 1")
        AjoutDefaut(102, 38, 2, "TIMEOUT", "Time-out ouverture bascule ciment 2")
        AjoutDefaut(103, 38, 3, "TIMEOUT", "Time-out ouverture bascule ciment 3")
        AjoutDefaut(104, 38, 4, "TIMEOUT", "Time-out ouverture bascule ciment 4")
        AjoutDefaut(105, 38, 5, "TIMEOUT", "Time-out fermeture bascule ciment 1")
        AjoutDefaut(106, 38, 6, "TIMEOUT", "Time-out fermeture bascule ciment 2")
        AjoutDefaut(107, 38, 7, "TIMEOUT", "Time-out fermeture bascule ciment 3")
        AjoutDefaut(108, 39, 0, "TIMEOUT", "Time-out fermeture bascule ciment 4")
        AjoutDefaut(109, 39, 1, "TIMEOUT", "Défaut tolérance adjuvant 1")
        AjoutDefaut(110, 39, 2, "TIMEOUT", "Défaut tolérance adjuvant 2")
        AjoutDefaut(111, 39, 3, "TIMEOUT", "Défaut tolérance adjuvant 3")
        AjoutDefaut(112, 39, 4, "TIMEOUT", "Défaut tolérance adjuvant 4")
        AjoutDefaut(113, 39, 5, "TIMEOUT", "Défaut tolérance adjuvant 5")
        AjoutDefaut(114, 39, 6, "TIMEOUT", "Défaut tolérance adjuvant 6")
        AjoutDefaut(115, 39, 7, "TIMEOUT", "Défaut tolérance adjuvant 7")
        AjoutDefaut(116, 40, 0, "TIMEOUT", "Défaut tolérance adjuvant 8")
        AjoutDefaut(117, 40, 1, "TIMEOUT", "Time-out adjuvant 1")
        AjoutDefaut(118, 40, 2, "TIMEOUT", "Time-out adjuvant 2")
        AjoutDefaut(119, 40, 3, "TIMEOUT", "Time-out adjuvant 3")
        AjoutDefaut(120, 40, 4, "TIMEOUT", "Time-out adjuvant 4")
        AjoutDefaut(121, 40, 5, "TIMEOUT", "Time-out adjuvant 5")
        AjoutDefaut(122, 40, 6, "TIMEOUT", "Time-out adjuvant 6")
        AjoutDefaut(123, 40, 7, "TIMEOUT", "Time-out adjuvant 7")
        AjoutDefaut(124, 41, 0, "TIMEOUT", "Time-out adjuvant 8")
        AjoutDefaut(125, 41, 1, "TIMEOUT", "Time-out vidange adjuvant")
        AjoutDefaut(126, 41, 2, "TIMEOUT", "Défaut tolérance dosage eau dans bascule")
        AjoutDefaut(127, 41, 3, "TIMEOUT", "Time-out dosage eau dans bascule")
        AjoutDefaut(128, 41, 4, "TIMEOUT", "Défaut tolérance dosage eau compteur")
        AjoutDefaut(129, 41, 5, "TIMEOUT", "Time-out dosage eau compteur")
        AjoutDefaut(130, 41, 6, "TIMEOUT", "Time-out vidange eau")
        AjoutDefaut(131, 41, 7, "TIMEOUT", "Défaut démarrage malaxeur")
        AjoutDefaut(132, 42, 0, "TIMEOUT", "Time-out ouverture trappe malaxeur 1")
        AjoutDefaut(133, 42, 1, "TIMEOUT", "Time-out ouverture trappe malaxeur 2")
        AjoutDefaut(134, 42, 2, "TIMEOUT", "Time-out fermeture trappe malaxeur 1")
        AjoutDefaut(135, 42, 3, "TIMEOUT", "Time-out fermeture trappe malaxeur 2")
        AjoutDefaut(136, 44, 0, "DISJ", "Défaut disjoncteur agrégat 1")
        AjoutDefaut(137, 44, 1, "DISJ", "Défaut disjoncteur agrégat 2")
        AjoutDefaut(138, 44, 2, "DISJ", "Défaut disjoncteur agrégat 3")
        AjoutDefaut(139, 44, 3, "DISJ", "Défaut disjoncteur agrégat 4")
        AjoutDefaut(140, 44, 4, "DISJ", "Défaut disjoncteur agrégat 5")
        AjoutDefaut(141, 44, 5, "DISJ", "Défaut disjoncteur agrégat 6")
        AjoutDefaut(142, 44, 6, "DISJ", "Défaut disjoncteur agrégat 7")
        AjoutDefaut(143, 44, 7, "DISJ", "Défaut disjoncteur agrégat 8")
        AjoutDefaut(144, 45, 0, "DISJ", "Défaut disjoncteur agrégat 9")
        AjoutDefaut(145, 45, 1, "DISJ", "Défaut disjoncteur agrégat 10")
        AjoutDefaut(146, 45, 2, "DISJ", "Défaut disjoncteur agrégat 11")
        AjoutDefaut(147, 45, 3, "DISJ", "Défaut disjoncteur agrégat 12")
        AjoutDefaut(148, 45, 4, "DISJ", "Défaut disjoncteur vibreur agrégat 1")
        AjoutDefaut(149, 45, 5, "DISJ", "Défaut disjoncteur vibreur agrégat 2")
        AjoutDefaut(150, 45, 6, "DISJ", "Défaut disjoncteur vibreur agrégat 3")
        AjoutDefaut(151, 45, 7, "DISJ", "Défaut disjoncteur vibreur agrégat 4")
        AjoutDefaut(152, 46, 0, "DISJ", "Défaut disjoncteur vibreur agrégat 5")
        AjoutDefaut(153, 46, 1, "DISJ", "Défaut disjoncteur vibreur agrégat 6")
        AjoutDefaut(154, 46, 2, "DISJ", "Défaut disjoncteur vibreur agrégat 7")
        AjoutDefaut(155, 46, 3, "DISJ", "Défaut disjoncteur vibreur agrégat 8")
        AjoutDefaut(156, 46, 4, "DISJ", "Défaut disjoncteur vibreur agrégat 9")
        AjoutDefaut(157, 46, 5, "DISJ", "Défaut disjoncteur vibreur agrégat 10")
        AjoutDefaut(158, 46, 6, "DISJ", "Défaut disjoncteur vibreur agrégat 11")
        AjoutDefaut(159, 46, 7, "DISJ", "Défaut disjoncteur vibreur agrégat 12")
        AjoutDefaut(160, 47, 0, "DISJ", "Défaut disjoncteur tapis peseur 1")
        AjoutDefaut(161, 47, 1, "DISJ", "Défaut disjoncteur tapis peseur 2")
        AjoutDefaut(162, 47, 2, "DISJ", "Défaut disjoncteur tapis peseur 3")
        AjoutDefaut(163, 47, 3, "DISJ", "Défaut disjoncteur vibreur tapis peseur 1")
        AjoutDefaut(164, 47, 4, "DISJ", "Défaut disjoncteur vibreur tapis peseur 2")
        AjoutDefaut(165, 47, 5, "DISJ", "Défaut disjoncteur vibreur tapis peseur 3")
        AjoutDefaut(166, 47, 6, "DISJ", "Défaut disjoncteur tapis reprise 1")
        AjoutDefaut(167, 47, 7, "DISJ", "Défaut disjoncteur tapis reprise 2")
        AjoutDefaut(168, 48, 0, "DISJ", "Défaut disjoncteur skip")
        AjoutDefaut(169, 48, 1, "DISJ", "Défaut disjoncteur trémie tampon")
        AjoutDefaut(170, 48, 2, "DISJ", "Défaut disjoncteur vis 1")
        AjoutDefaut(171, 48, 3, "DISJ", "Défaut disjoncteur vis 2")
        AjoutDefaut(172, 48, 4, "DISJ", "Défaut disjoncteur vis 3")
        AjoutDefaut(173, 48, 5, "DISJ", "Défaut disjoncteur vis 4")
        AjoutDefaut(174, 48, 6, "DISJ", "Défaut disjoncteur vis 5")
        AjoutDefaut(175, 48, 7, "DISJ", "Défaut disjoncteur vis 6")
        AjoutDefaut(176, 49, 0, "DISJ", "Défaut disjoncteur vibreur vis ciment 1")
        AjoutDefaut(177, 49, 1, "DISJ", "Défaut disjoncteur vibreur vis ciment 2")
        AjoutDefaut(178, 49, 2, "DISJ", "Défaut disjoncteur vibreur vis ciment 3")
        AjoutDefaut(179, 49, 3, "DISJ", "Défaut disjoncteur vibreur vis ciment 4")
        AjoutDefaut(180, 49, 4, "DISJ", "Défaut disjoncteur vibreur vis ciment 5")
        AjoutDefaut(181, 49, 5, "DISJ", "Défaut disjoncteur vibreur vis ciment 6")
        AjoutDefaut(182, 49, 6, "DISJ", "Défaut disjoncteur vibreur bascule ciment 1")
        AjoutDefaut(183, 49, 7, "DISJ", "Défaut disjoncteur vibreur bascule ciment 2")
        AjoutDefaut(184, 50, 0, "DISJ", "Défaut disjoncteur vibreur bascule ciment 3")
        AjoutDefaut(185, 50, 1, "DISJ", "Défaut disjoncteur vibreur bascule ciment 4")
        AjoutDefaut(186, 50, 2, "DISJ", "Défaut disjoncteur adjuvant 1")
        AjoutDefaut(187, 50, 3, "DISJ", "Défaut disjoncteur adjuvant 2")
        AjoutDefaut(188, 50, 4, "DISJ", "Défaut disjoncteur adjuvant 3")
        AjoutDefaut(189, 50, 5, "DISJ", "Défaut disjoncteur adjuvant 4")
        AjoutDefaut(190, 50, 6, "DISJ", "Défaut disjoncteur adjuvant 5")
        AjoutDefaut(191, 50, 7, "DISJ", "Défaut disjoncteur adjuvant 6")
        AjoutDefaut(192, 51, 0, "DISJ", "Défaut disjoncteur adjuvant 7")
        AjoutDefaut(193, 51, 1, "DISJ", "Défaut disjoncteur adjuvant 8")
        AjoutDefaut(194, 51, 2, "DISJ", "Défaut disjoncteur pompe eau")
        AjoutDefaut(195, 51, 3, "DISJ", "Défaut disjoncteur malaxeur")
        AjoutDefaut(196, 51, 4, "DISJ", "Défaut disjoncteur pompe trappe 1 malaxeur")
        AjoutDefaut(197, 51, 5, "DISJ", "Défaut disjoncteur pompe trappe 2 malaxeur")
    End Sub

    Public ReadOnly Property Liste As List(Of Defaut)
        Get
            If Not _initialise Then Initialiser()
            Return _liste
        End Get
    End Property

    ' ── Initialisation : liste + table AlarmesConfig ───────────
    Public Sub Initialiser()
        If _initialise Then Exit Sub
        _initialise = True
        ConstruireListe()
        Try
            VerifierColonnesAlarmes()
            ' AlarmesConfig vide → remplissage avec la liste
            Dim n As Object = ModuleDb.LireValeur(Of Object)("SELECT COUNT(*) FROM dbo.AlarmesConfig")
            If n IsNot Nothing AndAlso Not IsDBNull(n) AndAlso CInt(n) = 0 Then
                For Each d In _liste
                    Dim dd = d
                    ModuleDb.Executer(
                        "INSERT INTO dbo.AlarmesConfig (Numero, Texte, Classe, VariableDeclenchement, NumeroBit, AdresseDeclenchement) " &
                        "VALUES (@n, @t, @c, @v, @b, @a)",
                        Sub(cmd)
                            cmd.Parameters.AddWithValue("@n", dd.Numero)
                            cmd.Parameters.AddWithValue("@t", dd.Texte)
                            cmd.Parameters.AddWithValue("@c", dd.Classe)
                            cmd.Parameters.AddWithValue("@v", dd.Adresse)
                            cmd.Parameters.AddWithValue("@b", dd.Bit)
                            cmd.Parameters.AddWithValue("@a", "DB" & DB_DEFAUTS & ".DBB" & dd.Octet)
                        End Sub)
                Next
                Synoptique.EnregistrerAction("DEFAUTS", "AlarmesConfig initialisée", _liste.Count & " défauts", "", "", "AUTO")
            End If
            ChargerCommentaires()
            ' Défauts restés ACTIVE dans la base au démarrage → RESOLUE (le CPU est relu)
            ModuleDb.Executer("UPDATE dbo.Alarmes SET Etat='RESOLUE', DateResolution=GETDATE() WHERE Etat='ACTIVE' AND DateResolution IS NULL")
        Catch ex As Exception
            Debug.WriteLine("ModuleDefautsCPU.Initialiser : " & ex.Message)
        End Try
    End Sub

    Private Sub VerifierColonnesAlarmes()
        Try
            For Each def In {"DateHeure DATETIME NULL", "CodeAlarme NVARCHAR(50) NULL", "Description NVARCHAR(255) NULL",
                             "Etat NVARCHAR(50) NULL", "TypeAlarme NVARCHAR(30) NULL", "Operateur NVARCHAR(50) NULL", "DateResolution DATETIME NULL"}
                Dim nom As String = def.Split(" "c)(0)
                ModuleDb.Executer("IF COL_LENGTH('dbo.Alarmes','" & nom & "') IS NULL ALTER TABLE dbo.Alarmes ADD " & def)
            Next
        Catch
        End Try
    End Sub

    ' Commentaires depuis AlarmesConfig (clé = VariableDeclenchement)
    Public Sub ChargerCommentaires()
        Try
            Dim dt As DataTable = ModuleDb.Lire("SELECT VariableDeclenchement, Texte FROM dbo.AlarmesConfig")
            If dt Is Nothing Then Exit Sub
            Dim dico As New Dictionary(Of String, String)(StringComparer.OrdinalIgnoreCase)
            For Each r As DataRow In dt.Rows
                If IsDBNull(r("VariableDeclenchement")) Then Continue For
                dico(r("VariableDeclenchement").ToString().Trim()) = If(IsDBNull(r("Texte")), "", r("Texte").ToString())
            Next
            SyncLock _verrou
                For Each d In _liste
                    Dim t As String = Nothing
                    If dico.TryGetValue(d.Adresse, t) AndAlso t <> "" Then d.Texte = t
                Next
            End SyncLock
        Catch ex As Exception
            Debug.WriteLine("ChargerCommentaires : " & ex.Message)
        End Try
    End Sub

    ' Modification d'un commentaire (écran Alarmes machine, bouton EDITE)
    Public Function EnregistrerCommentaire(adresse As String, texte As String) As Boolean
        Try
            adresse = adresse.Trim() : texte = texte.Trim()
            Dim n As Object = ModuleDb.LireValeur(Of Object)(
                "SELECT COUNT(*) FROM dbo.AlarmesConfig WHERE VariableDeclenchement=@v",
                Sub(cmd) cmd.Parameters.AddWithValue("@v", adresse))
            If n IsNot Nothing AndAlso Not IsDBNull(n) AndAlso CInt(n) > 0 Then
                ModuleDb.Executer("UPDATE dbo.AlarmesConfig SET Texte=@t WHERE VariableDeclenchement=@v",
                    Sub(cmd)
                        cmd.Parameters.AddWithValue("@t", texte)
                        cmd.Parameters.AddWithValue("@v", adresse)
                    End Sub)
            Else
                ModuleDb.Executer(
                    "INSERT INTO dbo.AlarmesConfig (Numero, Texte, Classe, VariableDeclenchement, NumeroBit, AdresseDeclenchement) " &
                    "VALUES ((SELECT ISNULL(MAX(Numero),0)+1 FROM dbo.AlarmesConfig), @t, 'AUTRE', @v, 0, '')",
                    Sub(cmd)
                        cmd.Parameters.AddWithValue("@t", texte)
                        cmd.Parameters.AddWithValue("@v", adresse)
                    End Sub)
            End If
            ' Historique : même commentaire pour toutes les lignes de cette adresse
            ModuleDb.Executer("UPDATE dbo.Alarmes SET Description=@t WHERE CodeAlarme=@v",
                Sub(cmd)
                    cmd.Parameters.AddWithValue("@t", texte)
                    cmd.Parameters.AddWithValue("@v", adresse)
                End Sub)
            SyncLock _verrou
                For Each d In _liste
                    If String.Equals(d.Adresse, adresse, StringComparison.OrdinalIgnoreCase) Then d.Texte = texte
                Next
            End SyncLock
            Synoptique.EnregistrerAction("DEFAUTS", "Commentaire modifié", adresse & " = " & texte, adresse, "", "CLIC")
            Return True
        Catch ex As Exception
            MessageBox.Show("Erreur enregistrement commentaire : " & ex.Message)
            Return False
        End Try
    End Function

    ' ── Scrutation (1 lecture de 28 octets) ─────────────────────
    Public Sub Scruter()
        If Not _initialise Then Initialiser()
        If Not PLCData.Connecte Then Exit Sub
        Dim b() As Byte
        Try
            ' 22/09 : DB1 → lecture dans l'instantané (0 réseau) ; autre DB → lecture réseau
            If DB_DEFAUTS = 1 Then
                b = ModulePLCDuplex.LireInstantaneDB1(OCTET_DEBUT, NB_OCTETS)
            Else
                b = ModulePLCDuplex.LireBytesDB(DB_DEFAUTS, OCTET_DEBUT, NB_OCTETS)
            End If
        Catch
            Exit Sub
        End Try
        If b Is Nothing OrElse b.Length < NB_OCTETS Then Exit Sub
        Dim maintenant As Date = Now
        SyncLock _verrou
            For Each d In _liste
                If Not SURVEILLER_DET AndAlso d.Classe = "DET" Then Continue For
                Dim idx As Integer = d.Octet - OCTET_DEBUT
                If idx < 0 OrElse idx >= NB_OCTETS Then Continue For
                Dim etat As Boolean = ((b(idx) >> d.Bit) And 1) = 1
                If etat AndAlso Not d.Actif Then
                    d.Actif = True : d.DateApparition = maintenant
                    Dim dd = d
                    System.Threading.ThreadPool.QueueUserWorkItem(Sub(o) Apparition(dd, maintenant))
                ElseIf Not etat AndAlso d.Actif Then
                    d.Actif = False
                    Dim dd = d
                    System.Threading.ThreadPool.QueueUserWorkItem(Sub(o) Disparition(dd))
                End If
            Next
        End SyncLock
        _precedent = b
    End Sub

    Private Sub Apparition(d As Defaut, quand As Date)
        Try
            ModuleDb.Executer(
                "INSERT INTO dbo.Alarmes (DateHeure, CodeAlarme, Description, Etat, TypeAlarme, Operateur) " &
                "VALUES (@d, @c, @t, 'ACTIVE', @ty, @op)",
                Sub(cmd)
                    cmd.Parameters.AddWithValue("@d", quand)
                    cmd.Parameters.AddWithValue("@c", d.Adresse)
                    cmd.Parameters.AddWithValue("@t", d.Texte)
                    cmd.Parameters.AddWithValue("@ty", d.Classe)
                    cmd.Parameters.AddWithValue("@op", If(String.IsNullOrEmpty(SessionUtilisateur.NomUtilisateur), "", SessionUtilisateur.NomUtilisateur))
                End Sub)
            Synoptique.EnregistrerAction("ALARME", "Défaut CPU : " & d.Texte, d.Classe & " n°" & d.Numero, d.Adresse, "1", "ALARME")
        Catch ex As Exception
            Debug.WriteLine("Apparition defaut : " & ex.Message)
        End Try
    End Sub

    Private Sub Disparition(d As Defaut)
        Try
            ModuleDb.Executer(
                "UPDATE dbo.Alarmes SET Etat='RESOLUE', DateResolution=GETDATE() WHERE CodeAlarme=@c AND Etat='ACTIVE'",
                Sub(cmd) cmd.Parameters.AddWithValue("@c", d.Adresse))
            Synoptique.EnregistrerAction("DEFAUTS", "Défaut CPU disparu : " & d.Texte, d.Classe, d.Adresse, "0", "AUTO")
        Catch ex As Exception
            Debug.WriteLine("Disparition defaut : " & ex.Message)
        End Try
    End Sub

    ' Défauts actifs (copie, triés par date d'apparition)
    Public Function Actifs() As List(Of Defaut)
        Dim res As New List(Of Defaut)
        SyncLock _verrou
            For Each d In _liste
                If d.Actif Then res.Add(d)
            Next
        End SyncLock
        res.Sort(Function(x, y) x.DateApparition.CompareTo(y.DateApparition))
        Return res
    End Function

    Public Function NbActifs() As Integer
        SyncLock _verrou
            Return _liste.Where(Function(x) x.Actif).Count()
        End SyncLock
    End Function

End Module
