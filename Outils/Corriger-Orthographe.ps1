<#
  Corriger-Orthographe.ps1 — corrige les fautes d'orthographe / d'accents dans les TEXTES AFFICHES de l'ERP.
  Securite : seuls sont modifies les litteraux de chaine situes dans un contexte d'affichage :
      .Text = "..."   .HeaderText = "..."   MessageBox.Show(...)   EnregistrerAction(...)   String.Format("...")   AfficherForcage(...)
  Jamais touches : requetes SQL (SELECT/INSERT/UPDATE/@param/dbo.), noms de controles, noms de colonnes, cles internes.
  Usage : sans option = simulation (liste des changements dans $env:TEMP\orthographe.csv) ; -Appliquer = ecrit les fichiers.
#>
param([switch]$Appliquer)
$ErrorActionPreference = "Stop"
$racine = Split-Path -Parent (Split-Path -Parent $MyInvocation.MyCommand.Path)

# --- phrases (appliquees avant le dictionnaire) ---
$phrases = @(
  ,@("puis clique sur AJOUTE pour",        "puis cliquez sur AJOUTER pour")
  ,@("Modifie les champs puis clique sur", "Modifiez les champs puis cliquez sur")
  ,@("Modifie les champs", "Modifiez les champs")
  ,@("Enregistre d'abord la formule.",     "Enregistrez d'abord la formule.")
  ,@("puis clique sur AJOUTE pour",        "puis cliquez sur AJOUTER pour")
  ,@("BL cloture automatiquement",         "BL clôturé automatiquement")
  ,@("Cloture BL manque",                  "Clôture du BL manquée")
  ,@("rien a arreter",                     "rien à arrêter")
  ,@("Fin de cycle operateur",             "Fin de cycle opérateur")
  ,@("ANNULE (914",                        "ANNULÉ (914")
  ,@("Arret en fin demande (914",          "Arrêt en fin demandé (914")
  ,@("ARRET URGENCE",                      "ARRÊT D'URGENCE")
  ,@("arret en fin",                       "arrêt en fin")
  ,@("Arret en fin",                       "Arrêt en fin")
)
# --- dictionnaire mot a mot (insensible a la casse, la casse d'origine est conservee) ---
$dico = @{
 etat="état"; etats="états"; derniere="dernière"; unite="unité"; unites="unités"; designation="désignation"; selectionne="sélectionné"
 selectionner="sélectionner"; selectionnez="sélectionnez"; quantite="quantité"; quantites="quantités"; vehicule="véhicule"; numero="numéro"
 duree="durée"; telephone="téléphone"; capacite="capacité"; equipement="équipement"; creation="création"; creer="créer"; hygromettre="hygromètre"
 hygrometre="hygromètre"; hygrometrie="hygrométrie"; general="général"; generale="générale"; tremie="trémie"; tremies="trémies"
 parametrage="paramétrage"; parametrable="paramétrable"; parametrables="paramétrables"; parametre="paramètre"; parametres="paramètres"
 gachee="gâchée"; gachees="gâchées"; seche="sèche"; defaut="défaut"; defauts="défauts"; temperature="température"; role="rôle"
 periode="période"; regle="réglé"; reglement="règlement"; echeance="échéance"; controle="contrôle"; matiere="matière"; matieres="matières"
 premiere="première"; premieres="premières"; ecart="écart"; ecarts="écarts"; entree="entrée"; entrees="entrées"; arret="arrêt"; donnees="données"
 deja="déjà"; apres="après"; envoye="envoyé"; envoyee="envoyée"; enregistre="enregistré"; enregistres="enregistrés"; enregistree="enregistrée"
 connecte="connecté"; confirme="confirmé"; confirmee="confirmée"; demandee="demandée"; termine="terminé"; terminee="terminée"; imprime="imprimé"
 archivee="archivée"; archivees="archivées"; affichee="affichée"; annulee="annulée"; echoue="échoué"; ecriture="écriture"
 initialisee="initialisée"; modifie="modifié"; desactive="désactivé"; desactives="désactivés"; forcage="forçage"; forcages="forçages"
 reponse="réponse"; redemarrage="redémarrage"; reduit="réduit"; etabli="établi"; precedente="précédente"; operateur="opérateur"; detail="détail"
 depart="départ"; beton="béton"; debut="début"; echec="échec"; rincage="rinçage"; reseau="réseau"; resultat="résultat"; securite="sécurité"
 ecran="écran"; element="élément"; elements="éléments"; fchier="fichier"; demrage="démarrage"; ntete="en-tête"; resistance="résistance"
 livree="livrée"; debit="débit"; stabilite="stabilité"; plasticite="plasticité"; durabilite="durabilité"; densite="densité"
 granularite="granularité"; tolerance="tolérance"; societe="société"; reference="référence"; operation="opération"; operations="opérations"
 supprimee="supprimée"; manque="manqué"; qte="qté"; egale="égale"; fabriquee="fabriquée"; reel="réel"; equipements="équipements"
}
# casse : TOUT MAJ -> MAJ ; Initiale -> Initiale ; sinon minuscule
function Cas([string]$orig, [string]$nouv) {
  if ($orig.Length -gt 1 -and $orig -ceq $orig.ToUpper()) { return $nouv.ToUpper() }
  if ($orig.Substring(0,1) -ceq $orig.Substring(0,1).ToUpper()) { return $nouv.Substring(0,1).ToUpper() + $nouv.Substring(1) }
  return $nouv
}
$rxMot = [regex]('(?i)\b(' + (($dico.Keys | Sort-Object { -$_.Length }) -join '|') + ')\b')
$sql   = [regex]'(?i)\b(SELECT|INSERT|UPDATE|DELETE|FROM|WHERE|JOIN|ORDER BY|ISNULL|ALTER TABLE|CREATE TABLE|GETDATE)\b|dbo\.|@\w+|_kg\b|\bAS\s+\['
$ctx   = [regex]'\.Text\s*&?=\s*"|\.HeaderText\s*=|MessageBox\.Show\(|EnregistrerAction\(|String\.Format\(\s*"|AfficherForcage\('

function Corriger-Litteral([string]$s) {
  if ($sql.IsMatch($s)) { return $s }
  foreach ($p in $phrases) { if ($s.Contains($p[0])) { $s = $s.Replace($p[0], $p[1]) } }
  $s = $rxMot.Replace($s, { param($m) Cas $m.Value $dico[$m.Value.ToLower()] })
  switch -CaseSensitive ($s) { "AJOUTE" { $s = "AJOUTER" } "Duplique" { $s = "Dupliquer" } "Annule" { $s = "Annuler" } }
  return $s
}

$journal = New-Object System.Collections.Generic.List[object]
$fichiers = Get-ChildItem -Path $racine -Filter *.vb -File
foreach ($f in $fichiers) {
  $texte = [IO.File]::ReadAllText($f.FullName)
  $lignes = [regex]::Split($texte, '(?<=\n)')
  $modif = $false; $suite = 0; $n = 0
  for ($i = 0; $i -lt $lignes.Length; $i++) {
    $l = $lignes[$i]; $n = $i + 1
    $t = $l.TrimStart()
    if ($t.StartsWith("'")) { $suite = 0; continue }
    $suiteAvant = $suite
    $dansCtx = $ctx.IsMatch($l) -or $suite -gt 0
    # continuation d'appel multi-ligne
    $fin = ([regex]::Replace($l, "\s'[^`"]*$", "")).TrimEnd()
    if ($ctx.IsMatch($l) -and ($fin.EndsWith("_") -or $fin.EndsWith("&") -or $fin.EndsWith(",") -or $fin.EndsWith("("))) { $suite = 3 }
    elseif ($suite -gt 0) { if ($fin.EndsWith("_") -or $fin.EndsWith("&") -or $fin.EndsWith(",")) { $suite-- } else { $suite = 0 } }
    if (-not $dansCtx) { continue }
    $premierEA = $l -match 'EnregistrerAction\('
    # seuls les litteraux situes APRES le declencheur (.Text = / MessageBox.Show( ...) sont corriges :
    # ex. Columns("Controle").HeaderText = "Controle" -> la cle "Controle" (avant) n'est JAMAIS modifiee.
    $debut = 0
    if (-not ($suiteAvant -gt 0)) { $mc = $ctx.Match($l); if ($mc.Success) { $debut = $mc.Index + $mc.Length - 1 } }
    $idx = 0
    $nouv = [regex]::Replace($l, '"((?:[^"]|"")*)"', {
      param($m)
      if ($m.Index -lt $debut) { return $m.Value }
      $script:idx++
      $s = $m.Groups[1].Value
      # EnregistrerAction(module, ACTION, DETAIL, adresse, valeur, TYPE) : seuls ACTION et DETAIL sont du texte ;
      # le code module (1er) et le TYPE (dernier, stocke en base et utilise comme cle) ne sont JAMAIS modifies.
      if ($premierEA -and ($script:idx -eq 1 -or $script:idx -gt 3)) { return $m.Value }
      $c = Corriger-Litteral $s
      if ($c -cne $s) { $journal.Add([pscustomobject]@{ Fichier=$f.Name; Ligne=$n; Avant=$s; Apres=$c }) }
      return '"' + $c + '"'
    })
    $script:idx = 0
    if ($nouv -cne $l) { $lignes[$i] = $nouv; $modif = $true }
  }
  if ($modif -and $Appliquer) {
    [IO.File]::WriteAllText($f.FullName, ($lignes -join ""), (New-Object System.Text.UTF8Encoding($true)))
  }
}
$csv = Join-Path $env:TEMP "orthographe.csv"
$journal | Export-Csv $csv -NoTypeInformation -Encoding UTF8
Write-Host ("{0} correction(s) {1}. Detail : {2}" -f $journal.Count, $(if ($Appliquer) { "APPLIQUEE(S)" } else { "proposee(s) (simulation)" }), $csv)
