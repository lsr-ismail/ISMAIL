# Démarrer l'ERP avec le simulateur (sans automate)

## 1. Prérequis (une seule fois)
| Élément | Détail |
|---|---|
| .NET SDK | 8 ou supérieur (9 / 11 déjà présents sur ce PC) |
| SQL Server Express | instance `.\SQLEXPRESS` (celle attendue par `app.config`) |
| Visual Studio 2022 | optionnel (ouvrir `..\central.sln`) ; sinon `dotnet build` |

## 2. Créer la base de données
```powershell
powershell -ExecutionPolicy Bypass -File Database\Installer-Base.ps1
powershell -ExecutionPolicy Bypass -File Database\Activer-Licence-Dev.ps1
```
Crée `CentraleBeton` (schéma **reconstruit depuis le code**, voir l'avertissement dans `Database/01_Schema.sql`) et des données de test, puis active la licence sur ce poste de développement.
Connexion ERP : **admin / admin**. La machine `Simulateur S7-1200` pointe sur `127.0.0.1`.

Sans le script d'activation : l'écran de licence affiche un code machine ; le code d'activation se calcule avec
`powershell -File Database\Calculer-CodeActivation.ps1 -CodeAffiche <code affiché>`.

## 3. Compiler
```powershell
dotnet build central.vbproj -c Debug
dotnet build Simulateur\Simulateur.csproj -c Debug
```

## 4. Lancer le simulateur
```powershell
powershell -ExecutionPolicy Bypass -File Simulateur\Lancer-Simulateur.ps1
```
Le port 102 est occupé sur ce PC par le service Siemens **S7DOS Help Service**. Le script l'arrête le temps de la session (fenêtre UAC) puis le relance à la fermeture.
Sans ce service, on peut aussi lancer directement `Simulateur\bin\Debug\net8.0-windows\SimulateurAutomate.exe --port 102 --autostart`.

## 5. Lancer l'ERP
`bin\Debug\net8.0-windows\central.exe`
- Se connecter (admin / admin). Le synoptique doit passer en « PLC connecté » ; le journal du simulateur montre les écritures de l'ERP.

## 6. Test automatique du simulateur (sans ERP)
```powershell
dotnet run --project Simulateur.Test
```
Utilise la bibliothèque S7netplus (port 1102) : connexion, lecture DB1/DB71, écritures, cycle de 2 gâchées, coupure de liaison.

## Alternative sans toucher au service Siemens (branche Git)
La branche `feature/s7net-port-configurable` met l'ERP à jour vers S7netplus 0.20.0 et lit le port dans `dbo.Machines.Adresse_IP_PLC` (format `127.0.0.1:1102`). Avec elle, le simulateur peut tourner sur le port 1102 sans arrêter le service Siemens. **Non validée sur un vrai S7-1200** : à retester avant toute livraison client.

## Limites connues (à lire)
- Le programme automate réel n'est pas disponible : la logique du simulateur (`Simulateur/CpuEngine.cs`) est **déduite de ce que l'ERP lit et écrit**. Les durées, débits et le calcul des gâchées sont des hypothèses réglables dans l'onglet « Réglages du process ».
- Le schéma SQL est reconstruit : à comparer avec une vraie sauvegarde dès qu'elle est disponible.
- La compatibilité du simulateur avec S7.Net 0.1.4 (la bibliothèque de l'ERP actuel) est déduite du protocole ; elle n'a été testée qu'avec S7netplus 0.20.0 (le port 102 étant occupé, voir §4).
