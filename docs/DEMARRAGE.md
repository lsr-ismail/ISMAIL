# DÃ©marrer l'ERP avec le simulateur (sans automate)

## 1. PrÃ©requis (une seule fois)
| Ã‰lÃ©ment | DÃ©tail |
|---|---|
| .NET SDK | 8 ou supÃ©rieur (9 / 11 dÃ©jÃ  prÃ©sents sur ce PC) |
| SQL Server Express | instance `.\SQLEXPRESS` (celle attendue par `app.config`) |
| Visual Studio 2022 | optionnel (ouvrir `..\central.sln`) ; sinon `dotnet build` |

## 2. CrÃ©er la base de donnÃ©es
```powershell
powershell -ExecutionPolicy Bypass -File Database\Installer-Base.ps1
```
CrÃ©e `CentraleBeton` (schÃ©ma **reconstruit depuis le code**, voir avertissement dans `Database/01_Schema.sql`) et des donnÃ©es de test.
Connexion ERP : **admin / admin**. La machine `Simulateur S7-1200` pointe sur `127.0.0.1`.

## 3. Compiler
```powershell
dotnet build central.vbproj -c Debug
dotnet build Simulateur\Simulateur.csproj -c Debug
```

## 4. Lancer le simulateur
```powershell
powershell -ExecutionPolicy Bypass -File Simulateur\Lancer-Simulateur.ps1
```
Le port 102 est occupÃ© sur ce PC par le service Siemens **S7DOS Help Service**. Le script l'arrÃªte le temps de la session (fenÃªtre UAC) puis le relance Ã  la fermeture.
Sans ce service, on peut aussi lancer directement `Simulateur\bin\Debug\net8.0-windows\SimulateurAutomate.exe --port 102 --autostart`.

## 5. Lancer l'ERP
`bin\Debug\net8.0-windows\central.exe`
- Licence : au 1er lancement, l'Ã©cran de licence affiche un code machine. Calculer l'activation :
  `powershell -File Database\Calculer-CodeActivation.ps1 -CodeAffiche <code affichÃ©>`
- Se connecter (admin / admin). Le synoptique doit passer en Â« PLC connectÃ© Â» ; le journal du simulateur montre les Ã©critures de l'ERP.

## 6. Test automatique du simulateur (sans ERP)
```powershell
dotnet run --project Simulateur.Test
```
Utilise la bibliothÃ¨que S7netplus (port 1102) : connexion, lecture DB1/DB71, Ã©critures, cycle de 2 gÃ¢chÃ©es, coupure de liaison.

## Limites connues (Ã  lire)
- Le programme automate rÃ©el n'est pas disponible : la logique du simulateur (`Simulateur/CpuEngine.cs`) est **dÃ©duite de ce que l'ERP lit et Ã©crit**. Les durÃ©es, dÃ©bits et le calcul des gÃ¢chÃ©es sont des hypothÃ¨ses rÃ©glables dans l'onglet Â« RÃ©glages Â».
- Le schÃ©ma SQL est reconstruit : comparer avec une vraie sauvegarde dÃ¨s qu'elle est disponible.
- L'ERP utilise S7.Net 0.1.4, qui ne sait se connecter qu'au port 102.

## Alternative sans toucher au service Siemens (branche Git)
La branche `feature/s7net-port-configurable` met l'ERP à jour vers S7netplus 0.20.0 et lit le port dans `dbo.Machines.Adresse_IP_PLC` (format `127.0.0.1:1102`). Avec elle, le simulateur peut tourner sur le port 1102 sans arrêter le service Siemens. **Non validée sur un vrai S7-1200** : à retester avant toute livraison client.
