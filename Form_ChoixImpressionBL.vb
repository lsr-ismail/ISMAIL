Imports System.Windows.Forms

' ================================================================
'  Form_ChoixImpressionBL.vb — 16/09/2026
'  Fenêtre affichée à la FIN DU CHARGEMENT (fin commande) :
'     OUI     → imprimer le BL AVEC le détail des gâchées
'     NON     → imprimer le BL SANS détail
'     QUITTER → ne pas imprimer
'  Utilisation :
'     Dim r = Form_ChoixImpressionBL.Demander(numeroBL, nbGachees, volume)
'     r = DialogResult.Yes / No / Cancel
' ================================================================
Public Class Form_ChoixImpressionBL
    Inherits Form

    Public Shared Function Demander(numeroBL As String, nbGachees As Integer, volumeM3 As Double) As DialogResult
        Using f As New Form_ChoixImpressionBL(numeroBL, nbGachees, volumeM3)
            Return f.ShowDialog()
        End Using
    End Function

    Private Sub New(numeroBL As String, nbGachees As Integer, volumeM3 As Double)
        Me.Text = "Fin de chargement — Impression du BL"
        Me.FormBorderStyle = FormBorderStyle.FixedDialog
        Me.StartPosition = FormStartPosition.CenterScreen
        Me.MaximizeBox = False : Me.MinimizeBox = False
        Me.ShowInTaskbar = False
        Me.TopMost = True
        Me.ClientSize = New Size(520, 250)
        Me.Font = New Font("Segoe UI", 10)
        Me.BackColor = Color.White

        Dim bandeau As New Panel With {.Dock = DockStyle.Top, .Height = 48, .BackColor = Color.FromArgb(11, 61, 92)}
        bandeau.Controls.Add(New Label With {
            .Text = "✅  LIVRAISON TERMINÉE", .ForeColor = Color.White, .AutoSize = True,
            .Font = New Font("Segoe UI", 13, FontStyle.Bold), .Location = New Point(16, 11)})
        Me.Controls.Add(bandeau)

        Dim info As New Label With {
            .Location = New Point(24, 62), .Size = New Size(480, 70), .Font = New Font("Segoe UI", 10.5F),
            .Text = "BL : " & numeroBL & vbCrLf &
                    "Gâchées : " & nbGachees & "        Volume : " & volumeM3.ToString("0.00") & " m³"}
        Me.Controls.Add(info)

        Dim question As New Label With {
            .Location = New Point(24, 130), .AutoSize = True, .Font = New Font("Segoe UI", 11, FontStyle.Bold),
            .Text = "Imprimer le bon de livraison ?"}
        Me.Controls.Add(question)

        Dim bOui As Button = Bouton("OUI" & vbCrLf & "avec détail", Color.FromArgb(0, 150, 100), DialogResult.Yes, 24)
        Dim bNon As Button = Bouton("NON" & vbCrLf & "sans détail", Color.FromArgb(0, 120, 215), DialogResult.No, 194)
        Dim bQuit As Button = Bouton("QUITTER" & vbCrLf & "ne pas imprimer", Color.FromArgb(150, 150, 150), DialogResult.Cancel, 364)
        Me.Controls.AddRange({bOui, bNon, bQuit})

        Me.AcceptButton = bOui
        Me.CancelButton = bQuit
    End Sub

    Private Function Bouton(texte As String, fond As Color, resultat As DialogResult, x As Integer) As Button
        Dim b As New Button With {
            .Text = texte, .Location = New Point(x, 170), .Size = New Size(150, 56),
            .BackColor = fond, .ForeColor = Color.White, .FlatStyle = FlatStyle.Flat,
            .Font = New Font("Segoe UI", 10, FontStyle.Bold), .Cursor = Cursors.Hand,
            .DialogResult = resultat}
        b.FlatAppearance.BorderSize = 0
        Return b
    End Function

End Class
