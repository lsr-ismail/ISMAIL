<#
  Lancer-Simulateur.ps1 — demarre le simulateur d'automate sur le port 102 (celui qu'utilise l'ERP).

  Probleme : sur ce PC, le service Siemens « S7DOS Help Service » (s7oiehsx64) occupe deja le port 102.
  Ce script l'arrete le temps de la session (droits administrateur requis -> une fenetre UAC s'ouvre),
  lance le simulateur, puis REDEMARRE le service quand vous fermez le simulateur.
  Aucun fichier Siemens n'est modifie. Si le service n'existe pas / le port est libre, rien n'est arrete.
#>
param([string]$Exe = "")

$ErrorActionPreference = "Stop"
$dir = Split-Path -Parent $MyInvocation.MyCommand.Path

# 1) trouver l'executable (build Debug ou Release du simulateur, ou dossier publie)
if (-not $Exe) {
    $cands = @(
        (Join-Path $dir "SimulateurAutomate.exe"),
        (Join-Path $dir "bin\Release\net8.0-windows\SimulateurAutomate.exe"),
        (Join-Path $dir "bin\Debug\net8.0-windows\SimulateurAutomate.exe"))
    $Exe = $cands | Where-Object { Test-Path $_ } | Select-Object -First 1
}
if (-not $Exe) { Write-Host "SimulateurAutomate.exe introuvable. Compilez d'abord :  dotnet build Simulateur\Simulateur.csproj" -ForegroundColor Red; Read-Host "Entree pour quitter"; exit 1 }

# 2) le port 102 est-il pris par un service Siemens ?
$ecoute = Get-NetTCPConnection -LocalPort 102 -State Listen -ErrorAction SilentlyContinue | Select-Object -First 1
$serviceArrete = $null
if ($ecoute) {
    $svc = Get-CimInstance Win32_Service | Where-Object { $_.ProcessId -eq $ecoute.OwningProcess } | Select-Object -First 1
    if (-not $svc) { Write-Host "Le port 102 est occupe par le processus $($ecoute.OwningProcess) (pas un service). Fermez-le puis relancez." -ForegroundColor Red; Read-Host "Entree"; exit 1 }

    $admin = ([Security.Principal.WindowsPrincipal][Security.Principal.WindowsIdentity]::GetCurrent()).IsInRole([Security.Principal.WindowsBuiltInRole]::Administrator)
    if (-not $admin) {
        Write-Host "Relance en administrateur pour liberer le port 102 ($($svc.DisplayName))..." -ForegroundColor Yellow
        Start-Process powershell -Verb RunAs -ArgumentList "-NoProfile -ExecutionPolicy Bypass -File `"$($MyInvocation.MyCommand.Path)`" -Exe `"$Exe`""
        exit 0
    }
    Write-Host "Arret du service '$($svc.DisplayName)' pour liberer le port 102..." -ForegroundColor Yellow
    Stop-Service -Name $svc.Name -Force
    $serviceArrete = $svc.Name
}

# 3) lancer le simulateur et attendre sa fermeture
Write-Host "Demarrage du simulateur : $Exe" -ForegroundColor Green
$p = Start-Process -FilePath $Exe -ArgumentList "--port 102 --autostart" -PassThru
$p.WaitForExit()

# 4) remettre le service Siemens en l'etat
if ($serviceArrete) {
    Write-Host "Redemarrage du service '$serviceArrete'..." -ForegroundColor Yellow
    try { Start-Service -Name $serviceArrete } catch { Write-Host "Impossible de redemarrer le service : $($_.Exception.Message)" -ForegroundColor Red }
}
