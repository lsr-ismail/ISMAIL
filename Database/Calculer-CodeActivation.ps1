<#
  Calculer-CodeActivation.ps1 — calcule le code d'activation de la licence (ModuleLicence.vb) pour le DEVELOPPEMENT.
  L'ecran de licence affiche un "code machine" (chiffres inverses). Entrez-le ici :
      powershell -File Database\Calculer-CodeActivation.ps1 -CodeAffiche 123456
  Algorithme (ModuleLicence.vb l.9-11 et l.52-57) : n = code affiche lu a l'envers ; activation = (n*18 + 6) mod 19941985, sur 6 chiffres.
#>
param([Parameter(Mandatory=$true)][string]$CodeAffiche)
$chars = $CodeAffiche.Trim().ToCharArray(); [array]::Reverse($chars)
$n = [int64](-join $chars)
$r = ($n * 18 + 6) % 19941985
"Code d'activation : {0:D6}" -f $r
