<#
  Activer-Licence-Dev.ps1 — active la licence de l'ERP sur CE poste de developpement (sans saisir de code).
  Reproduit ModuleLicence.ObtenirIdentifiantMachine() : SHA-256 (Base64) de "NumSerieCarteMere|IdProcesseur"
  puis marque la licence comme conforme. A utiliser uniquement sur votre propre machine de dev.
#>
param([string]$Serveur = ".\SQLEXPRESS")

function Nettoyer([string]$v) {
    if ($null -eq $v) { return "" }
    $t = $v.Trim().ToUpperInvariant().Replace(" ", "")
    if (@("", "NONE", "DEFAULTSTRING", "TOBEFILLEDBYO.E.M.", "SYSTEMSERIALNUMBER", "0", "NOTAPPLICABLE", "N/A") -contains $t) { return "" }
    return $t
}
$serie = Nettoyer ((Get-CimInstance Win32_BaseBoard | Select-Object -First 1).SerialNumber)
$proc  = Nettoyer ((Get-CimInstance Win32_Processor | Select-Object -First 1).ProcessorId)
if ($serie -eq "") { $serie = Nettoyer ((Get-CimInstance Win32_BIOS | Select-Object -First 1).SerialNumber) }
if ($serie -eq "") { $serie = "NA" }
if ($proc -eq "")  { $proc = "NA" }
$sha = [System.Security.Cryptography.SHA256]::Create()
$id = [Convert]::ToBase64String($sha.ComputeHash([Text.Encoding]::UTF8.GetBytes("$serie|$proc")))

$cn = New-Object System.Data.SqlClient.SqlConnection "Server=$Serveur;Database=CentraleBeton;Integrated Security=True;TrustServerCertificate=True"
$cn.Open()
try {
    $c = $cn.CreateCommand()
    $c.CommandText = "IF NOT EXISTS (SELECT 1 FROM dbo.GranulatsCertification) INSERT INTO dbo.GranulatsCertification (CodeGranulat, Designation, DateControle, Granulometrie, Disponible, Conforme) VALUES ('CLI-DEV','Poste de developpement',DATEADD(year,10,GETDATE()),5,1,0);
                     UPDATE dbo.GranulatsCertification SET CertificatQualite=@id, Conforme=1 WHERE IdCertification=(SELECT TOP 1 IdCertification FROM dbo.GranulatsCertification WHERE Disponible=1 ORDER BY DateControle DESC, IdCertification DESC)"
    [void]$c.Parameters.AddWithValue("@id", $id)
    [void]$c.ExecuteNonQuery()
    Write-Host "Licence activee pour ce poste." -ForegroundColor Green
} finally { $cn.Close() }
