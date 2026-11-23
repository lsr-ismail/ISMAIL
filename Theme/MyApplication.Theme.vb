' Démarre le gestionnaire de thème au lancement de l'application (aucune modification des formulaires existants).
Namespace My
    Partial Friend Class MyApplication
        Private Sub MyApplication_Startup(sender As Object, e As ApplicationServices.StartupEventArgs) Handles Me.Startup
            ThemeManager.Demarrer()
        End Sub
    End Class
End Namespace
