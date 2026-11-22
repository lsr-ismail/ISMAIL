Imports Microsoft.Data.SqlClient
Imports System.Configuration
Imports System.Data

Module ModuleDb

    ' ── Chaîne de connexion lue depuis App.config ──────────────────────
    Private ReadOnly Property ChaineConnexion As String
        Get
            Return ConfigurationManager.ConnectionStrings("CentraleBeton").ConnectionString
        End Get
    End Property

    ' ── Ouvre une connexion neuve (le pool ADO.NET la gère) ─────────────
    Private Function OuvrirNouvelle() As SqlConnection
        Dim cn As New SqlConnection(ChaineConnexion)
        cn.Open()
        Return cn
    End Function

    ' ═══════════════════════════════════════════════════════════════════
    ' LECTURE — retourne un DataTable (pour remplir un DataGridView)
    ' ═══════════════════════════════════════════════════════════════════
    Public Function Lire(sql As String,
                         Optional parametres As Action(Of SqlCommand) = Nothing) As DataTable
        Dim dt As New DataTable()
        Try
            Using cn As SqlConnection = OuvrirNouvelle()
                Using cmd As New SqlCommand(sql, cn)
                    If parametres IsNot Nothing Then parametres(cmd)
                    Using da As New SqlDataAdapter(cmd)
                        da.Fill(dt)
                    End Using
                End Using
            End Using
        Catch ex As Exception
            Journal("Lire", ex)
            Throw
        End Try
        Return dt
    End Function

    ' ═══════════════════════════════════════════════════════════════════
    ' ÉCRITURE — INSERT / UPDATE / DELETE
    ' ═══════════════════════════════════════════════════════════════════
    Public Function Executer(sql As String,
                             Optional parametres As Action(Of SqlCommand) = Nothing) As Integer
        Try
            Using cn As SqlConnection = OuvrirNouvelle()
                Using cmd As New SqlCommand(sql, cn)
                    If parametres IsNot Nothing Then parametres(cmd)
                    Return cmd.ExecuteNonQuery()
                End Using
            End Using
        Catch ex As Exception
            Journal("Executer", ex)
            Throw
        End Try
        Return 0
    End Function

    ' ═══════════════════════════════════════════════════════════════════
    ' VALEUR UNIQUE — COUNT, MAX, un seul champ…
    ' ═══════════════════════════════════════════════════════════════════
    Public Function LireValeur(Of T)(sql As String,
                                     Optional parametres As Action(Of SqlCommand) = Nothing) As T
        Try
            Using cn As SqlConnection = OuvrirNouvelle()
                Using cmd As New SqlCommand(sql, cn)
                    If parametres IsNot Nothing Then parametres(cmd)
                    Dim r = cmd.ExecuteScalar()
                    If r Is Nothing OrElse Convert.IsDBNull(r) Then Return Nothing
                    Return CType(Convert.ChangeType(r, GetType(T)), T)
                End Using
            End Using
        Catch ex As Exception
            Journal("LireValeur", ex)
            Throw
        End Try
        Return Nothing
    End Function

    ' ═══════════════════════════════════════════════════════════════════
    ' JOURNALISATION — écrit les erreurs dans un fichier log
    ' ═══════════════════════════════════════════════════════════════════
    Private Sub Journal(source As String, ex As Exception)
        Try
            Dim chemin As String = IO.Path.Combine(
                AppDomain.CurrentDomain.BaseDirectory, "erreurs.log")
            Dim ligne As String = $"[{DateTime.Now:yyyy-MM-dd HH:mm:ss}] [{source}] {ex.Message}"
            IO.File.AppendAllText(chemin, ligne & Environment.NewLine)
        Catch
            ' Si le log échoue, on ne bloque pas l'application
        End Try
    End Sub

End Module