<#
  Installer-Base.ps1 — cree / met a jour la base CentraleBeton (schema reconstruit + donnees de dev).
  Usage :  powershell -ExecutionPolicy Bypass -File Database\Installer-Base.ps1 [-Serveur ".\SQLEXPRESS"] [-SansDonneesDev]
  Idempotent : peut etre relance. N'efface JAMAIS de donnees existantes.
  Utilise System.Data.SqlClient (present dans Windows PowerShell 5.1) : aucun outil a installer.
#>
param(
    [string]$Serveur = ".\SQLEXPRESS",
    [switch]$SansDonneesDev
)
$ErrorActionPreference = "Stop"
$dir = Split-Path -Parent $MyInvocation.MyCommand.Path
$fichiers = @("01_Schema.sql", "03_Configuration.sql")
if (-not $SansDonneesDev) { $fichiers += "02_SeedDev.sql" }

function Executer-Fichier([string]$chemin, [string]$base) {
    $cs = "Server=$Serveur;Database=$base;Integrated Security=True;TrustServerCertificate=True;Connect Timeout=15"
    $cn = New-Object System.Data.SqlClient.SqlConnection $cs
    $cn.Open()
    try {
        $sql = Get-Content $chemin -Raw -Encoding UTF8
        $lots = [regex]::Split($sql, '(?im)^\s*GO\s*$') | Where-Object { $_.Trim() -ne "" }
        foreach ($lot in $lots) {
            if ($lot -match '(?im)^\s*USE\s+CentraleBeton\s*;?\s*$') { $cn.ChangeDatabase("CentraleBeton"); $lot = [regex]::Replace($lot,'(?im)^\s*USE\s+CentraleBeton\s*;?\s*$','') }
            if ($lot.Trim() -eq "") { continue }
            $cmd = $cn.CreateCommand(); $cmd.CommandText = $lot; $cmd.CommandTimeout = 120
            [void]$cmd.ExecuteNonQuery()
        }
    } finally { $cn.Close() }
}

Write-Host "Serveur : $Serveur" -ForegroundColor Cyan
foreach ($f in $fichiers) {
    Write-Host " -> $f" -NoNewline
    # le 1er script cree la base ; on se connecte a master, puis le script fait USE CentraleBeton
    $base = "master"
    if ($f -ne "01_Schema.sql") { $base = "CentraleBeton" }
    Executer-Fichier (Join-Path $dir $f) $base
    Write-Host "  OK" -ForegroundColor Green
}
Write-Host "Base CentraleBeton prete. Connexion ERP : admin / admin" -ForegroundColor Green
