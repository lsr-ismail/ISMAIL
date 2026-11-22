Imports System.Windows.Forms
Imports Microsoft.Data.SqlClient
Imports System.Data

Public Class Frm_Config_Case
    Inherits Form

    Private _numeroCas As Integer
    Private _typeEquip As String
    Private _caseID As Integer = 0
    Private _nomCase As String = ""
    Private _dosageValide As Boolean = False
    Private _niveauKg As Decimal = 0
    Private _capaciteKg As Decimal = 80000
    Private _hygrometrie As Decimal = 0
    Private _seuilAlerteKg As Decimal = 0
    Private _actif As Boolean = True

    Private pnlTitre As Panel
    Private lblTitre As Label
    Private lblIcone As Label
    Private pnlCorps As Panel
    Private txtNomCase As TextBox
    Private chkDosageValide As CheckBox
    Private chkActif As CheckBox
    Private grpNiveaux As GroupBox
    Private lblNiveauVal As Label
    Private lblCapaciteVal As Label
    Private lblHygroVal As Label
    Private lblSeuilVal As Label
    Private progressNiveau As ProgressBar
    Private pnlBoutons As Panel
    Private btnValider As Button
    Private btnAbandonner As Button
    Private btnConfiguration As Button

    ' ═══════════════════════════════════════════════════════════════════
    '  CONSTRUCTEUR
    ' ═══════════════════════════════════════════════════════════════════

    Public Sub New(numeroCas As Integer, typeEquip As String)
        _numeroCas = numeroCas
        _typeEquip = typeEquip.ToUpper()
        InitialiserFormulaire()
        ChargerDonneesBDD()
        RemplirAffichage()
    End Sub

    ' ═══════════════════════════════════════════════════════════════════
    '  CONSTRUCTION UI
    ' ═══════════════════════════════════════════════════════════════════

    Private Sub InitialiserFormulaire()
        Me.Text = "Configuration Case"
        Me.Size = New Size(370, 420)
        Me.StartPosition = FormStartPosition.CenterParent
        Me.FormBorderStyle = FormBorderStyle.FixedDialog
        Me.MaximizeBox = False : Me.MinimizeBox = False
        Me.BackColor = Color.FromArgb(245, 245, 250)

        pnlTitre = New Panel() With {
            .Dock = DockStyle.Top, .Height = 52,
            .BackColor = If(_typeEquip = "SILO", Color.FromArgb(60, 100, 160), Color.FromArgb(60, 130, 90))}
        lblIcone = New Label() With {
            .Text = If(_typeEquip = "SILO", "🏭", "🏗️"),
            .Font = New Font("Segoe UI Emoji", 18), .ForeColor = Color.White,
            .Location = New Point(10, 8), .Size = New Size(40, 36),
            .TextAlign = ContentAlignment.MiddleCenter}
        pnlTitre.Controls.Add(lblIcone)
        lblTitre = New Label() With {
            .Font = New Font("Arial", 12, FontStyle.Bold), .ForeColor = Color.White,
            .Location = New Point(55, 12), .Size = New Size(270, 28)}
        pnlTitre.Controls.Add(lblTitre)
        Me.Controls.Add(pnlTitre)

        pnlCorps = New Panel() With {
            .Location = New Point(0, 52), .Size = New Size(354, 290),
            .BackColor = Color.Transparent}
        Me.Controls.Add(pnlCorps)

        AjouterLabel(pnlCorps, "Nom de la case :", 10, 14)
        txtNomCase = New TextBox() With {
            .Location = New Point(130, 11), .Size = New Size(200, 22),
            .Font = New Font("Arial", 9, FontStyle.Bold)}
        pnlCorps.Controls.Add(txtNomCase)

        chkDosageValide = New CheckBox() With {
            .Text = "Dosage validé", .Location = New Point(10, 42),
            .Size = New Size(140, 22), .Font = New Font("Arial", 9)}
        pnlCorps.Controls.Add(chkDosageValide)

        chkActif = New CheckBox() With {
            .Text = "Case active", .Location = New Point(165, 42),
            .Size = New Size(120, 22), .Font = New Font("Arial", 9)}
        pnlCorps.Controls.Add(chkActif)

        Dim sep As New Label() With {
            .BorderStyle = BorderStyle.Fixed3D,
            .Location = New Point(10, 70), .Size = New Size(330, 2)}
        pnlCorps.Controls.Add(sep)

        grpNiveaux = New GroupBox() With {
            .Text = " Niveaux & Capacités ", .Location = New Point(8, 78),
            .Size = New Size(338, 170), .Font = New Font("Arial", 8, FontStyle.Bold)}
        pnlCorps.Controls.Add(grpNiveaux)

        progressNiveau = New ProgressBar() With {
            .Location = New Point(10, 22), .Size = New Size(316, 18),
            .Minimum = 0, .Maximum = 100, .Value = 0}
        grpNiveaux.Controls.Add(progressNiveau)

        Dim yG As Integer = 46
        AjouterLigneInfo(grpNiveaux, "Niveau actuel :", 10, yG, Color.FromArgb(0, 120, 0))
        lblNiveauVal = AjouterValeur(grpNiveaux, "", 175, yG) : yG += 26
        AjouterLigneInfo(grpNiveaux, "Capacité maxi :", 10, yG, Color.DarkGray)
        lblCapaciteVal = AjouterValeur(grpNiveaux, "", 175, yG) : yG += 26
        AjouterLigneInfo(grpNiveaux, "Hygrométrie :", 10, yG, Color.FromArgb(0, 100, 180))
        lblHygroVal = AjouterValeur(grpNiveaux, "", 175, yG) : yG += 26
        AjouterLigneInfo(grpNiveaux, "Seuil alerte :", 10, yG, Color.DarkOrange)
        lblSeuilVal = AjouterValeur(grpNiveaux, "", 175, yG)

        pnlBoutons = New Panel() With {
            .Dock = DockStyle.Bottom, .Height = 50,
            .BackColor = Color.FromArgb(230, 230, 235)}
        Me.Controls.Add(pnlBoutons)

        btnAbandonner = New Button() With {
            .Text = "Abandonner", .Location = New Point(10, 10),
            .Size = New Size(100, 30), .BackColor = Color.FromArgb(220, 220, 220)}
        pnlBoutons.Controls.Add(btnAbandonner)

        btnValider = New Button() With {
            .Text = "✔ Valider", .Location = New Point(120, 10),
            .Size = New Size(100, 30), .BackColor = Color.FromArgb(180, 230, 180),
            .Font = New Font("Arial", 9, FontStyle.Bold)}
        pnlBoutons.Controls.Add(btnValider)

        btnConfiguration = New Button() With {
            .Text = "⚙ Configuration", .Location = New Point(232, 10),
            .Size = New Size(118, 30), .BackColor = Color.FromArgb(100, 160, 220),
            .ForeColor = Color.White, .Font = New Font("Arial", 9, FontStyle.Bold)}
        pnlBoutons.Controls.Add(btnConfiguration)

        AddHandler btnValider.Click, AddressOf BtnValider_Click
        AddHandler btnAbandonner.Click, AddressOf BtnAbandonner_Click
        AddHandler btnConfiguration.Click, AddressOf BtnConfiguration_Click
    End Sub

    ' ═══════════════════════════════════════════════════════════════════
    '  HELPERS UI
    ' ═══════════════════════════════════════════════════════════════════

    Private Sub AjouterLabel(parent As Control, texte As String, x As Integer, y As Integer)
        parent.Controls.Add(New Label() With {
            .Text = texte, .Location = New Point(x, y + 2),
            .Size = New Size(118, 18), .Font = New Font("Arial", 8),
            .ForeColor = Color.DimGray})
    End Sub

    Private Sub AjouterLigneInfo(parent As Control, texte As String,
                                  x As Integer, y As Integer, couleur As Color)
        parent.Controls.Add(New Label() With {
            .Text = texte, .Location = New Point(x, y),
            .Size = New Size(160, 20), .Font = New Font("Arial", 8, FontStyle.Bold),
            .ForeColor = couleur})
    End Sub

    Private Function AjouterValeur(parent As Control, texte As String,
                                    x As Integer, y As Integer) As Label
        Dim lbl As New Label() With {
            .Text = texte, .Location = New Point(x, y),
            .Size = New Size(150, 20), .Font = New Font("Arial", 9, FontStyle.Bold),
            .ForeColor = Color.Black}
        parent.Controls.Add(lbl)
        Return lbl
    End Function

    ' ═══════════════════════════════════════════════════════════════════
    '  LECTURE BDD
    ' ═══════════════════════════════════════════════════════════════════

    Private Sub ChargerDonneesBDD()
        Try
            Dim dt As DataTable = ModuleDb.Lire(
                "SELECT TOP 1 CaseID, NomCase, DosageValide, NiveauKg, " &
                "CapaciteKg, Hygrometrie, SeuilAlerteKg, Actif " &
                "FROM dbo.Cases " &
                "WHERE NumeroCase=@num AND TypeMateriau=@type",
                Sub(cmd)
                    cmd.Parameters.Add("@num", SqlDbType.Int).Value = _numeroCas
                    cmd.Parameters.Add("@type", SqlDbType.NVarChar, 50).Value = If(_typeEquip = "SILO", "CIMENT", "GRANULAT")
                End Sub)

            If dt.Rows.Count > 0 Then
                Dim r As DataRow = dt.Rows(0)
                _caseID = CInt(r("CaseID"))
                _nomCase = If(IsDBNull(r("NomCase")), "", r("NomCase").ToString())
                _dosageValide = Not IsDBNull(r("DosageValide")) AndAlso CBool(r("DosageValide"))
                _niveauKg = If(IsDBNull(r("NiveauKg")), 0D, CDec(r("NiveauKg")))
                _capaciteKg = If(IsDBNull(r("CapaciteKg")), 80000D, CDec(r("CapaciteKg")))
                _hygrometrie = If(IsDBNull(r("Hygrometrie")), 0D, CDec(r("Hygrometrie")))
                _seuilAlerteKg = If(IsDBNull(r("SeuilAlerteKg")), 0D, CDec(r("SeuilAlerteKg")))
                _actif = Not IsDBNull(r("Actif")) AndAlso CBool(r("Actif"))
            Else
                _nomCase = If(_typeEquip = "SILO", "Silo CIM " & _numeroCas, "Case " & _numeroCas)
            End If

        Catch ex As Exception
            Debug.WriteLine("Frm_Config_Case.ChargerDonneesBDD : " & ex.Message)
        End Try
    End Sub

    ' ═══════════════════════════════════════════════════════════════════
    '  AFFICHAGE
    ' ═══════════════════════════════════════════════════════════════════

    Private Sub RemplirAffichage()
        lblTitre.Text = If(_typeEquip = "SILO", "Silo Ciment " & _numeroCas, "Case / Trémie " & _numeroCas)
        txtNomCase.Text = _nomCase
        chkDosageValide.Checked = _dosageValide
        chkActif.Checked = _actif

        If _capaciteKg > 0 Then
            Dim pct As Integer = Math.Max(0, Math.Min(100, CInt((_niveauKg / _capaciteKg) * 100)))
            progressNiveau.Value = pct
            progressNiveau.ForeColor = If(pct < 20, Color.Red, If(pct < 40, Color.Orange, Color.Green))
        End If

        lblNiveauVal.Text = _niveauKg.ToString("N0") & " kg  (" &
                              If(_capaciteKg > 0, CInt((_niveauKg / _capaciteKg) * 100).ToString() & " %", "?") & ")"
        lblCapaciteVal.Text = _capaciteKg.ToString("N0") & " kg"
        lblHygroVal.Text = _hygrometrie.ToString("F1") & " %"
        lblSeuilVal.Text = _seuilAlerteKg.ToString("N0") & " kg"
    End Sub

    ' ═══════════════════════════════════════════════════════════════════
    '  BOUTONS
    ' ═══════════════════════════════════════════════════════════════════

    Private Sub BtnValider_Click(sender As Object, e As EventArgs)
        Try
            If _caseID = 0 Then
                ModuleDb.Executer(
                    "INSERT INTO dbo.Cases (NumeroCase, TypeMateriau, NomCase, DosageValide, Actif) " &
                    "VALUES (@num, @type, @nom, @valide, @actif)",
                    Sub(cmd)
                        cmd.Parameters.Add("@num", SqlDbType.Int).Value = _numeroCas
                        cmd.Parameters.Add("@type", SqlDbType.NVarChar, 50).Value = If(_typeEquip = "SILO", "CIMENT", "GRANULAT")
                        cmd.Parameters.Add("@nom", SqlDbType.NVarChar, 100).Value = txtNomCase.Text.Trim()
                        cmd.Parameters.Add("@valide", SqlDbType.Bit).Value = chkDosageValide.Checked
                        cmd.Parameters.Add("@actif", SqlDbType.Bit).Value = chkActif.Checked
                    End Sub)
            Else
                ModuleDb.Executer(
                    "UPDATE dbo.Cases SET NomCase=@nom, DosageValide=@valide, " &
                    "Actif=@actif, DateModif=GETDATE() WHERE CaseID=@id",
                    Sub(cmd)
                        cmd.Parameters.Add("@nom", SqlDbType.NVarChar, 100).Value = txtNomCase.Text.Trim()
                        cmd.Parameters.Add("@valide", SqlDbType.Bit).Value = chkDosageValide.Checked
                        cmd.Parameters.Add("@actif", SqlDbType.Bit).Value = chkActif.Checked
                        cmd.Parameters.Add("@id", SqlDbType.Int).Value = _caseID
                    End Sub)
            End If

            Me.DialogResult = DialogResult.OK
            Me.Close()

        Catch ex As Exception
            MessageBox.Show("Erreur lors de la sauvegarde :" & vbCrLf & ex.Message,
                            "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    Private Sub BtnAbandonner_Click(sender As Object, e As EventArgs)
        Me.DialogResult = DialogResult.Cancel
        Me.Close()
    End Sub

    Private Sub BtnConfiguration_Click(sender As Object, e As EventArgs)
        Dim fConfig As New Frm_Config_Automate(_numeroCas, _typeEquip)
        fConfig.ShowDialog(Me)
        ChargerDonneesBDD()
        RemplirAffichage()
    End Sub

    Private Sub Frm_Config_Case_Load(sender As Object, e As EventArgs) Handles MyBase.Load
    End Sub

End Class