# Charte graphique BETONIA

Le logo BETONIA est en noir et blanc (silos, malaxeur, bandeau noir). La charte en est tirée : **graphite, gris béton, blanc**, avec une seule teinte d'appui (ambre) réservée à la sélection dans les tableaux. Aucune couleur vive décorative.

## Palette

| Rôle | Clair | Sombre |
|---|---|---|
| Fond des fenêtres | `#F2F3F5` | `#16181B` |
| Surface (menus, onglet actif) | `#FAFAFB` | `#1E2126` |
| Champs de saisie | `#FFFFFF` | `#262A30` |
| Texte | `#1F2328` | `#E8EAED` |
| Texte secondaire | `#4B5563` | `#A6ADB5` |
| Contour des composants | `#7D858E` | `#7F8892` |
| **Bouton principal** | fond `#1F2328`, texte blanc | fond `#E8EAED`, texte `#16181B` |
| Bouton principal (survol / appui) | `#3A414A` / `#0F1114` | `#FFFFFF` / `#C9CED3` |
| **Bouton secondaire** | fond `#FAFAFB`, contour `#1F2328` | fond `#1E2126`, contour `#A6ADB5` |
| Bouton secondaire (survol / appui) | `#E3E6EA` / `#D3D8DD` | `#2E343B` / `#3A414A` |
| Sélection dans les grilles | `#FFD98A` (texte `#1F2328`) | `#7A5C00` (texte blanc) |
| En-tête de grille / bandeau de titre | `#E3E6EA` / `#1F2328` | `#2E343B` |
| Bouton désactivé | fond `#D9DCE0`, texte `#5F6670` | fond `#2A2E34`, texte `#8B939C` |

Le bouton principal est celui de l'action centrale d'un écran (Valider, Enregistrer, Ajouter, Connecter, Confirmer…). Tous les autres sont secondaires. Au survol, la couleur change (voir tableau) et le curseur devient une main.

## Typographie (Segoe UI)

| Usage | Taille |
|---|---|
| Libellés secondaires, petits textes | 9 pt |
| Texte courant, champs, boutons | 10 pt |
| Sous-titres | 12 pt |
| Titres | 14 pt |
| Grands affichages (poids, compteurs) | taille d'origine (≥ 15 pt) |

Les polices à chasse fixe (Consolas, Courier) et les polices de symboles sont conservées.

## Normes respectées

- **WCAG 2.1 niveau AA** (référentiel repris par le RGAA 4 et la norme EN 301 549) : contraste ≥ 4,5:1 pour le texte, ≥ 3:1 pour les contours de composants. Tableau mesuré ci-dessous.
- Aucune information portée par la seule couleur : les états AUTO / FORÇAGE, marche / arrêt restent doublés d'un texte.
- Taille minimale de texte : 9 pt.

### Contrastes mesurés

| Paire | Clair | Sombre | Seuil |
|---|---|---|---|
| Texte / fond | 14.23 : 1 ✔ | 14.76 : 1 ✔ | 4.5 : 1 |
| Texte secondaire / fond | 6.81 : 1 ✔ | 7.85 : 1 ✔ | 4.5 : 1 |
| Texte / champ de saisie | 15.8 : 1 ✔ | 11.96 : 1 ✔ | 4.5 : 1 |
| Bouton principal (texte / fond) | 15.8 : 1 ✔ | 14.76 : 1 ✔ | 4.5 : 1 |
| Bouton principal survol | 10.32 : 1 ✔ | 17.79 : 1 ✔ | 4.5 : 1 |
| Bouton secondaire (texte / fond) | 15.14 : 1 ✔ | 13.39 : 1 ✔ | 4.5 : 1 |
| Bouton secondaire survol | 12.62 : 1 ✔ | 10.43 : 1 ✔ | 4.5 : 1 |
| Sélection grille (texte / fond) | 11.69 : 1 ✔ | 6.25 : 1 ✔ | 4.5 : 1 |
| En-tête de grille | 12.62 : 1 ✔ | 10.43 : 1 ✔ | 4.5 : 1 |
| Lien / fond | 5.75 : 1 ✔ | 8.44 : 1 ✔ | 4.5 : 1 |
| Contour des champs / fond (3:1) | 3.37 : 1 ✔ | 4.95 : 1 ✔ | 3 : 1 |


## Ce qui est conservé volontairement

Sur les écrans de conduite (Synoptique, Marche forcée, Assistance PLC, paramétrage équipement) et pour les boutons d'état listés dans `Theme/BoutonsSemantiques.vb`, les couleurs vives posées par le code (vert = actif, rouge = arrêt…) **ne sont pas repeintes** : elles portent un sens d'exploitation. Elles reçoivent seulement l'effet de survol.

## Utilisation

- Mode clair / sombre : menu **Affichage → Mode sombre** (écran principal) ou bouton en bas à gauche de l'écran de connexion. Le choix est mémorisé dans `%APPDATA%\Betonia\theme.ini`.
- Le thème s'applique automatiquement à toute fenêtre qui s'ouvre (`Theme/ThemeManager.vb`) : **aucun formulaire n'a besoin d'être modifié** pour en bénéficier.
- Nouveau bouton d'état dont le code change la couleur : ajouter son nom dans `Theme/BoutonsSemantiques.vb`.
- Aperçu : `Outils/CaptureEcrans` génère des captures PNG des écrans en mode clair et sombre.

## Limites connues

- Les images du synoptique et les graphiques dessinés par le code gardent leurs couleurs.
- Les cases à cocher, barres de défilement et bordures des champs texte restent dessinées par Windows (légèrement plus claires en mode sombre).
- Les boîtes de dialogue `MessageBox` sont gérées par Windows et ne suivent pas le thème.
- Les écrans qui colorent leurs lignes par type (ex. configuration des silos : trémies bleues, ciments orange…) gardent ce codage de couleurs, qui est une information métier.