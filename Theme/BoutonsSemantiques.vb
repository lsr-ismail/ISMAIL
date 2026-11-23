' GENERE par analyse du code : boutons dont la couleur est pilotée par le code (état marche / arrêt, forçages...).
' Le thème ne repeint JAMAIS ces boutons (il ajoute seulement l'effet de survol). Ajouter ici tout nouveau bouton d'état.
Friend NotInheritable Class BoutonsSemantiques
    Friend Shared ReadOnly Noms As New HashSet(Of String)(StringComparer.OrdinalIgnoreCase) From {
        "btnabbondomennt",
        "btnDepartCycle",
        "btnFinChargement",
        "btnModeAuto",
        "btnModeDemarrage",
        "btnRAZ",
        "btnRAZDefaut",
        "btnVidangeBeton",
        "Buttonadj1",
        "Buttonadj2",
        "Buttonadj3",
        "Buttonadj4",
        "Buttonadjtr",
        "Buttoneau1",
        "Buttoneau2",
        "Buttoneautr",
        "Buttonmxa",
        "Buttonmxfr",
        "Buttonmxm",
        "Buttonmxov",
        "Buttonska",
        "Buttonskb",
        "Buttonskh",
        "Buttonsl1",
        "Buttonsl2",
        "Buttonsltr",
        "Buttontpa",
        "Buttontpm",
        "Buttontr1",
        "Buttontr2",
        "Buttontr3",
        "Buttontr4"
    }
End Class
