Imports System.Data
Imports System.Windows.Forms

' ================================================================
'  Frm_StockElement.vb — 26/09/2026
'  Fiche stock d'UN matériau (clic droit → Stock sur une trémie / un silo du Synoptique).
'  Affiche les données de T_STOCK (comme la partie gauche de l'écran Stocks, sans la table),
'  avec les boutons « Entrée stock », « Actualiser » et « Fermer ».
' ================================================================
Public Class Frm_StockElement
    Inherits Form

    Private ReadOnly _codeMat As String
    Private ReadOnly _siloId As Integer = 0        ' 26/09 : > 0 → fiche d'un SILO (stock par silo, Silos.StockKg)
    Private _idStock As Integer = 0
    Private _unite As String = ""
    Private _nomMat As String = ""

    Private ReadOnly _txt As New Dictionary(Of String, TextBox)
    Private _txtQte As TextBox
    Private _dtp As DateTimePicker
    Private _lblEtat As Label

    Private Shared ReadOnly CHAMPS() As (String, String) = {
        ("MAT_Code", "Code matériau"), ("MAT_Nom", "Matériau"), ("STO_Unite", "Unité"),
        ("STO_Stock_Initial", "Stock initial"), ("STO_Somme_Entrees", "Somme entrées"),
        ("STO_Somme_Sorties", "Somme sorties"), ("STO_Somme_Corrections", "Somme corrections"),
        ("STO_Stock", "Stock actuel")}

    Public Sub New(codeMateriau As String, titre As String)
        _codeMat = codeMateriau
        ConstruireInterface(titre)
    End Sub

    ' 26/09 : fiche d'un silo ciment — stock PAR SILO (kg), + total matériau (T_STOCK) si affecté
    Public Sub New(siloId As Integer, codeMateriau As String, titre As String)
        _siloId = siloId
        _codeMat = codeMateriau
        ConstruireInterface(titre)
    End Sub

    Private Sub ConstruireInterface(titre As String)
        Me.Text = "Stock — " & titre
        Me.FormBorderStyle = FormBorderStyle.FixedDialog
        Me.MaximizeBox = False : Me.MinimizeBox = False
        Me.StartPosition = FormStartPosition.CenterParent
        Me.Font = New Font("Segoe UI", 10)

        Dim lblTitre As New Label With {.Text = titre, .Font = New Font("Segoe UI", 11, FontStyle.Bold),
                                        .ForeColor = Color.DarkBlue, .AutoSize = True, .Location = New Point(12, 10)}
        Me.Controls.Add(lblTitre)

        Dim y As Integer = 45
        Dim champs() As (String, String) = CHAMPS
        If _siloId > 0 Then
            champs = {("NomSilo", "Silo"), ("MAT_Code", "Matériau"), ("CapaciteMax_kg", "Capacité (kg)"),
                      ("NiveauMini", "Niveau mini (%)"), ("STO_Stock", "Stock du silo (kg)")}
        End If
        For Each c In champs
            Me.Controls.Add(New Label With {.Text = c.Item2 & " :", .Location = New Point(15, y + 3), .AutoSize = True})
            Dim tb As New TextBox With {.Location = New Point(190, y), .Width = 200, .ReadOnly = True,
                                        .TextAlign = HorizontalAlignment.Right, .BackColor = Color.WhiteSmoke}
            _txt(c.Item1) = tb
            Me.Controls.Add(tb)
            y += 30
        Next
        _txt("STO_Stock").BackColor = Color.FromArgb(200, 255, 200)
        _txt("STO_Stock").Font = New Font("Segoe UI", 10, FontStyle.Bold)

        y += 8
        Me.Controls.Add(New Label With {.Text = "Quantité entrée :", .Location = New Point(15, y + 3), .AutoSize = True})
        _txtQte = New TextBox With {.Location = New Point(190, y), .Width = 200, .TextAlign = HorizontalAlignment.Right}
        Me.Controls.Add(_txtQte)
        y += 30
        Me.Controls.Add(New Label With {.Text = "Date :", .Location = New Point(15, y + 3), .AutoSize = True})
        _dtp = New DateTimePicker With {.Location = New Point(190, y), .Width = 200, .Format = DateTimePickerFormat.Long}
        Me.Controls.Add(_dtp)
        y += 36

        _lblEtat = New Label With {.Location = New Point(12, y), .AutoSize = True, .Font = New Font("Segoe UI", 8), .ForeColor = Color.Gray}
        Me.Controls.Add(_lblEtat)
        y += 24

        Dim btnEntree As New Button With {.Text = "Entrée stock", .Location = New Point(15, y), .Size = New Size(120, 30),
                                          .BackColor = Color.FromArgb(0, 130, 60), .ForeColor = Color.White, .FlatStyle = FlatStyle.Flat}
        Dim btnActualiser As New Button With {.Text = "Actualiser", .Location = New Point(145, y), .Size = New Size(110, 30)}
        Dim btnFermer As New Button With {.Text = "Fermer", .Location = New Point(300, y), .Size = New Size(90, 30)}
        AddHandler btnEntree.Click, Sub() EnregistrerEntree()
        AddHandler btnActualiser.Click, Sub() Charger()
        AddHandler btnFermer.Click, Sub() Me.Close()
        Me.Controls.AddRange({btnEntree, btnActualiser, btnFermer})
        Me.AcceptButton = btnEntree
        Me.CancelButton = btnFermer
        Me.ClientSize = New Size(410, y + 45)
    End Sub

    Private Sub Frm_StockElement_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Charger()
        _txtQte.Focus()
    End Sub

    Private Sub Charger()
        If _siloId > 0 Then ChargerSilo() : Exit Sub
        Try
            ModuleStock.SynchroniserFichesStock()
            Dim dt As DataTable = ModuleDb.Lire(
                "SELECT STO_ID, MAT_Code, MAT_Nom, STO_Unite, STO_Stock_Initial, STO_Somme_Entrees, " &
                "       STO_Somme_Sorties, STO_Somme_Corrections, STO_Stock, STO_DateModification " &
                "FROM dbo.T_STOCK WHERE MAT_Code = @c",
                Sub(cmd) cmd.Parameters.Add("@c", SqlDbType.VarChar, 15).Value = _codeMat)
            If dt.Rows.Count = 0 Then
                _idStock = 0
                For Each kv In _txt : kv.Value.Text = "" : Next
                _txt("MAT_Code").Text = _codeMat
                _lblEtat.Text = "Aucune fiche stock pour ce code (matériau non géré en stock ?)" : _lblEtat.ForeColor = Color.Red
                Exit Sub
            End If
            Dim r As DataRow = dt.Rows(0)
            _idStock = CInt(r("STO_ID"))
            _unite = r("STO_Unite").ToString().Trim()
            _nomMat = r("MAT_Nom").ToString()
            _txt("MAT_Code").Text = r("MAT_Code").ToString()
            _txt("MAT_Nom").Text = _nomMat
            _txt("STO_Unite").Text = _unite
            For Each col In {"STO_Stock_Initial", "STO_Somme_Entrees", "STO_Somme_Sorties", "STO_Somme_Corrections", "STO_Stock"}
                _txt(col).Text = If(IsDBNull(r(col)), "0,00", CDec(r(col)).ToString("N2")) & " " & _unite
            Next
            Dim stock As Decimal = If(IsDBNull(r("STO_Stock")), 0D, CDec(r("STO_Stock")))
            _txt("STO_Stock").BackColor = If(stock <= 0, Color.FromArgb(255, 180, 180), Color.FromArgb(200, 255, 200))
            _lblEtat.Text = "Dernière modification : " & If(IsDBNull(r("STO_DateModification")), "-", CDate(r("STO_DateModification")).ToString("dd/MM/yyyy HH:mm"))
            _lblEtat.ForeColor = Color.Gray
        Catch ex As Exception
            _lblEtat.Text = "Erreur : " & ex.Message : _lblEtat.ForeColor = Color.Red
        End Try
    End Sub

    Private Sub EnregistrerEntree()
        If _siloId > 0 Then EnregistrerEntreeSilo() : Exit Sub
        If _idStock = 0 Then
            MessageBox.Show("Aucune fiche stock pour ce matériau.", "Stock", MessageBoxButtons.OK, MessageBoxIcon.Warning) : Exit Sub
        End If
        Dim qte As Decimal
        If _txtQte.Text.Trim() = "" OrElse
           Not Decimal.TryParse(_txtQte.Text.Trim().Replace(",", "."), Globalization.NumberStyles.Any,
                                Globalization.CultureInfo.InvariantCulture, qte) OrElse qte <= 0 Then
            MessageBox.Show("Saisir une quantité supérieure à zéro (ex. 1500 ou 12,5).", "Stock", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            _txtQte.Focus() : _txtQte.SelectAll() : Exit Sub
        End If
        If MessageBox.Show("Confirmer l'entrée de " & qte.ToString("N2") & " " & _unite & " pour " & _nomMat & " ?",
                           "Entrée stock", MessageBoxButtons.YesNo, MessageBoxIcon.Question) <> DialogResult.Yes Then Exit Sub
        Try
            Dim lignes As Integer = ModuleDb.Executer(
                "UPDATE dbo.T_STOCK SET STO_Somme_Entrees = STO_Somme_Entrees + @Qte, " &
                "STO_Stock = STO_Stock + @Qte, STO_DateModification = @Dt WHERE STO_ID = @Id",
                Sub(cmd)
                    cmd.Parameters.Add("@Qte", SqlDbType.Decimal).Value = qte
                    cmd.Parameters.Add("@Dt", SqlDbType.DateTime).Value = _dtp.Value.Date.Add(Now.TimeOfDay)
                    cmd.Parameters.Add("@Id", SqlDbType.Int).Value = _idStock
                End Sub)
            If lignes > 0 Then
                ModuleStock.MettreAJourStockMateriau(_codeMat, qte)      ' Materiaux.Stock cohérent
                Synoptique.EnregistrerAction("STOCK", "Entrée stock (Synoptique)",
                    _nomMat & " : +" & qte.ToString("0.00") & " " & _unite & " le " & _dtp.Value.ToString("dd/MM/yyyy"),
                    _codeMat, qte.ToString("0.00"), "ENTREE_STOCK")
                _txtQte.Text = ""
                Charger()
                _lblEtat.Text = "Entrée enregistrée : +" & qte.ToString("N2") & " " & _unite : _lblEtat.ForeColor = Color.DarkGreen
            Else
                MessageBox.Show("Aucun enregistrement modifié.", "Stock", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            End If
        Catch ex As Exception
            MessageBox.Show("Erreur entrée stock : " & ex.Message, "Stock", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    ' ══════════════════ MODE SILO (26/09) ══════════════════
    Private Sub ChargerSilo()
        Try
            Dim dt As DataTable = ModuleDb.Lire(
                "SELECT s.NomSilo, ISNULL(s.CapaciteMax_kg,0) AS Cap, ISNULL(s.NiveauMini,0) AS Mini, " &
                "       ISNULL(s.StockKg,0) AS StockKg, s.DateModif, " &
                "       ISNULL(m.CodeMateriau,'') AS CodeMat, ISNULL(m.NomMateriau,'') AS NomMat " &
                "FROM dbo.Silos s LEFT JOIN dbo.Materiaux m ON m.IdMateriau = s.IdMateriau " &
                "WHERE s.SiloID = @id",
                Sub(cmd) cmd.Parameters.Add("@id", SqlDbType.Int).Value = _siloId)
            If dt.Rows.Count = 0 Then
                _lblEtat.Text = "Silo introuvable" : _lblEtat.ForeColor = Color.Red : Exit Sub
            End If
            Dim r As DataRow = dt.Rows(0)
            _unite = "kg"
            _nomMat = r("NomMat").ToString()
            _txt("NomSilo").Text = r("NomSilo").ToString()
            _txt("MAT_Code").Text = If(r("CodeMat").ToString() = "", "(non affecté)", r("CodeMat").ToString() & " - " & _nomMat)
            _txt("CapaciteMax_kg").Text = CDbl(r("Cap")).ToString("N0") & " kg"
            _txt("NiveauMini").Text = CDbl(r("Mini")).ToString("0") & " %"
            Dim stock As Double = CDbl(r("StockKg"))
            _txt("STO_Stock").Text = stock.ToString("N0") & " kg"
            Dim cap As Double = CDbl(r("Cap"))
            Dim pct As Double = If(cap > 0, stock / cap * 100, 0)
            _txt("STO_Stock").BackColor = If(stock <= 0 OrElse pct < CDbl(r("Mini")), Color.FromArgb(255, 180, 180), Color.FromArgb(200, 255, 200))
            _lblEtat.Text = "Dernière modification : " & If(IsDBNull(r("DateModif")), "-", CDate(r("DateModif")).ToString("dd/MM/yyyy HH:mm")) &
                            If(cap > 0, "   (" & pct.ToString("0.0") & " % de la capacité)", "")
            _lblEtat.ForeColor = Color.Gray
        Catch ex As Exception
            _lblEtat.Text = "Erreur : " & ex.Message : _lblEtat.ForeColor = Color.Red
        End Try
    End Sub

    Private Sub EnregistrerEntreeSilo()
        Dim qte As Decimal
        If _txtQte.Text.Trim() = "" OrElse
           Not Decimal.TryParse(_txtQte.Text.Trim().Replace(",", "."), Globalization.NumberStyles.Any,
                                Globalization.CultureInfo.InvariantCulture, qte) OrElse qte <= 0 Then
            MessageBox.Show("Saisir une quantité en kg supérieure à zéro (ex. 25000).", "Stock silo", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            _txtQte.Focus() : _txtQte.SelectAll() : Exit Sub
        End If
        Dim nomSilo As String = _txt("NomSilo").Text
        If MessageBox.Show("Confirmer l'entrée de " & qte.ToString("N0") & " kg dans " & nomSilo & " ?",
                           "Entrée stock silo", MessageBoxButtons.YesNo, MessageBoxIcon.Question) <> DialogResult.Yes Then Exit Sub
        Try
            ' 1) stock du silo
            ModuleArchivageGachees.Instance.MouvementStockSilo(_siloId, CDbl(qte))
            ' 2) total du matériau (T_STOCK + Materiaux.Stock), converti selon l'unité de la fiche
            If _codeMat <> "" Then
                ModuleDb.Executer(
                    "UPDATE dbo.T_STOCK SET " &
                    "  STO_Somme_Entrees = STO_Somme_Entrees + CASE WHEN UPPER(STO_Unite) IN ('T','M3') THEN @Qte / 1000.0 ELSE @Qte END, " &
                    "  STO_Stock = STO_Stock + CASE WHEN UPPER(STO_Unite) IN ('T','M3') THEN @Qte / 1000.0 ELSE @Qte END, " &
                    "  STO_DateModification = @Dt WHERE MAT_Code = @c",
                    Sub(cmd)
                        cmd.Parameters.Add("@Qte", SqlDbType.Decimal).Value = qte
                        cmd.Parameters.Add("@Dt", SqlDbType.DateTime).Value = _dtp.Value.Date.Add(Now.TimeOfDay)
                        cmd.Parameters.Add("@c", SqlDbType.VarChar, 15).Value = _codeMat
                    End Sub)
                Dim uniteMat As String = ""
                Try
                    uniteMat = ModuleDb.LireValeur(Of String)("SELECT ISNULL(UniteStockage,'kg') FROM dbo.Materiaux WHERE LEFT(CodeMateriau,15) = @c",
                                                             Sub(cmd) cmd.Parameters.AddWithValue("@c", _codeMat))
                Catch
                End Try
                Dim delta As Decimal = If(uniteMat.ToUpper() = "T" OrElse uniteMat.ToUpper() = "M3", qte / 1000D, qte)
                ModuleStock.MettreAJourStockMateriau(_codeMat, delta)
            End If
            Synoptique.EnregistrerAction("STOCK", "Entrée stock silo (Synoptique)",
                nomSilo & " : +" & qte.ToString("N0") & " kg" & If(_nomMat <> "", " (" & _nomMat & ")", "") & " le " & _dtp.Value.ToString("dd/MM/yyyy"),
                _codeMat, qte.ToString("0"), "ENTREE_STOCK")
            _txtQte.Text = ""
            ChargerSilo()
            _lblEtat.Text = "Entrée enregistrée : +" & qte.ToString("N0") & " kg" : _lblEtat.ForeColor = Color.DarkGreen
        Catch ex As Exception
            MessageBox.Show("Erreur entrée stock silo : " & ex.Message, "Stock silo", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

End Class
