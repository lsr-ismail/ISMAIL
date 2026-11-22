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
```
Crée `CentraleBeton` (schéma **reconstruit depuis le code**, voir avertissement dans `Database/01_Schema.sql`) et des données de test.
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
Le port 102 est occupé sur ce PC par le service Siemens **S7DOS Help Service**. Le script l'arrête le temps de la session (fenêtre UAC) puis le relance à la fermeture.
Sans ce service, on peut aussi lancer directement `Simulateur\bin\Debug\net8.0-windows\SimulateurAutomate.exe --port 102 --autostart`.

## 5. Lancer l'ERP
`bin\Debug\net8.0-windows\central.exe`
- Licence : au 1er lancement, l'écran de licence affiche un code machine. Calculer l'activation :
  `powershell -File Database\Calculer-CodeActivation.ps1 -CodeAffiche <code affiché>`
- Se connecter (admin / admin). Le synoptique doit passer en « PLC connecté » ; le journal du simulateur montre les écritures de l'ERP.

## 6. Test automatique du simulateur (sans ERP)
```powershell
dotnet run --project Simulateur.Test
```
Utilise la bibliothèque S7netplus (port 1102) : connexion, lecture DB1/DB71, écritures, cycle de 2 gâchées, coupure de liaison.

## Limites connues (à lire)
- Le programme automate réel n'est pas disponible : la logique du simulateur (`Simulateur/CpuEngine.cs`) est **déduite de ce que l'ERP lit et écrit**. Les durées, débits et le calcul des gâchées sont des hypothèses réglables dans l'onglet « Réglages ».
- Le schéma SQL est reconstruit : comparer avec une vraie sauvegarde dès qu'elle est disponible.
- L'ERP utilise S7.Net 0.1.4, qui ne sait se connecter qu'au port 102.
