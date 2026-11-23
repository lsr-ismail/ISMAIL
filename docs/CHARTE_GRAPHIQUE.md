# Charte graphique BETONIA

Charte **en gris et nuances de gris** (choix du client) : noir graphite, gris béton, blanc. Aucune couleur vive décorative : ni rouge, ni orange, ni bleu. Les seules couleurs conservées sont celles que le **code** pose pour indiquer un état d'exploitation (voir plus bas).

## Palette

| Rôle | Clair | Sombre |
|---|---|---|
| Fond des fenêtres | `#F2F3F5` | `#15171A` |
| Surface (menus, onglet actif) | `#FFFFFF` | `#1D2024` |
| Champs de saisie | `#FFFFFF` | `#24282D` |
| Texte | `#1F2328` | `#E8EAED` |
| Texte secondaire | `#4B5563` | `#A6ADB5` |
| Contour des composants | `#7D858E` | `#7F8892` |
| **Bouton principal** | fond `#2B3036`, texte blanc | fond `#D5D9DF`, texte `#15171A` |
| Bouton principal (survol / appui) | `#4A525B` / `#15181C` | `#FFFFFF` / `#B7BDC5` |
| **Bouton secondaire** | fond blanc, contour `#2B3036` | fond `#1D2024`, contour `#A6ADB5` |
| Bouton secondaire (survol / appui) | `#E1E4E8` / `#CDD2D8` | `#2D3238` / `#3A4047` |
| Sélection dans les grilles | `#D5D9DF` (texte `#1F2328`) | `#4A525B` (texte blanc) |
| En-tête de grille | `#E1E4E8` | `#2D3238` |
| Bandeau de titre | `#2B3036` (texte blanc) | `#2D3238` |
| Bouton désactivé | fond `#DDE0E4`, texte `#5F6670` | fond `#2A2E34`, texte `#8B939C` |

## Boutons

- **Arrondis** (rayon 9 px, contour lissé), **une seule hauteur : 36 px** (52 px pour un texte sur deux lignes). Largeur proportionnée au texte (110 px minimum) ; elle n'est jamais agrandie pour ne rien chevaucher.
- **Principal** = action centrale de l'écran (Valider, Enregistrer, Ajouter, Connecter, Confirmer…) ; tous les autres sont **secondaires**. Au survol, la couleur change et le curseur devient une main.
- Les petits boutons (flèches, + / −) gardent leur largeur ; les boutons avec image ne sont pas redessinés.

## Typographie (Segoe UI)

9 pt (petits textes) · 10 pt (courant, champs, boutons) · 12 pt (sous-titres) · 14 pt (titres). Les tailles ne sont **jamais agrandies** (11 à 14,9 pt deviennent 12 pt) pour éviter les débordements. Les polices à chasse fixe et de symboles sont conservées.

## Mise en page

- Les libellés sont centrés verticalement sur le champ de leur ligne et alignés à gauche par colonne.
- Un grand composant (tableau, onglets, groupe) cède la place aux champs voisins qui le chevauchent.
- **Adaptation à l'écran** : les fenêtres principales s'ouvrent agrandies ; les tableaux, onglets et groupes suivent les bords de la fenêtre, les boutons d'angle droit suivent le bord droit. Les petites boîtes de dialogue et l'écran de connexion gardent leur taille.
- Écrans exclus de ces règles de mise en page (leurs positions suivent le dessin du synoptique) : Synoptique, écran principal, Marche forcée, Assistance PLC, paramétrage équipement.

## Normes respectées

- **WCAG 2.1 niveau AA** (repris par le RGAA 4 et la norme EN 301 549) : contraste ≥ 4,5:1 pour le texte, ≥ 3:1 pour les contours. Mesuré ci-dessous : **tout est conforme**.
- Aucune information portée par la seule couleur ; taille de texte minimale : 9 pt.

### Contrastes mesurés

| Paire | Clair | Sombre | Seuil |
|---|---|---|---|
| Texte / fond | 14.23 : 1 ✔ | 14.9 : 1 ✔ | 4.5 : 1 |
| Texte secondaire / fond | 6.81 : 1 ✔ | 7.92 : 1 ✔ | 4.5 : 1 |
| Texte / champ de saisie | 15.8 : 1 ✔ | 12.3 : 1 ✔ | 4.5 : 1 |
| Bouton principal | 13.3 : 1 ✔ | 12.67 : 1 ✔ | 4.5 : 1 |
| Bouton principal (survol) | 7.93 : 1 ✔ | 17.96 : 1 ✔ | 4.5 : 1 |
| Bouton secondaire | 13.3 : 1 ✔ | 13.57 : 1 ✔ | 4.5 : 1 |
| Bouton secondaire (survol) | 10.43 : 1 ✔ | 10.72 : 1 ✔ | 4.5 : 1 |
| Sélection dans les grilles | 11.15 : 1 ✔ | 7.93 : 1 ✔ | 4.5 : 1 |
| En-tête de grille | 12.39 : 1 ✔ | 10.72 : 1 ✔ | 4.5 : 1 |
| Bandeau de titre | 13.3 : 1 ✔ | 10.72 : 1 ✔ | 4.5 : 1 |
| Lien | 9.29 : 1 ✔ | 11.33 : 1 ✔ | 4.5 : 1 |
| Contour des champs (seuil 3:1) | 3.37 : 1 ✔ | 4.99 : 1 ✔ | 3 : 1 |

## Couleurs d'état conservées

Les boutons d'état de la liste `Theme/BoutonsSemantiques.vb` (Synoptique : marche / arrêt, forçages) et tout bouton que le **code** recolore pendant l'exploitation (par exemple AUTO → FORÇAGE actif) gardent la couleur posée par le code : elle signale un état. Les boutons statiques sont, eux, en gris. Dans les tableaux, les fonds colorés posés par le code sont convertis en gris de même clarté et les textes colorés reprennent la couleur du thème.

## Utilisation

- **Affichage → Mode sombre** (écran principal) ou bouton en bas à gauche de l'écran de connexion ; choix mémorisé dans `%APPDATA%\Betonia\theme.ini`.
- Le thème s'applique à toute fenêtre qui s'ouvre (`Theme/ThemeManager.vb`) : aucun formulaire n'a à être modifié.
- Nouveau bouton d'état recoloré par le code : ajouter son nom dans `Theme/BoutonsSemantiques.vb`.
- `Outils/CaptureEcrans` produit des captures PNG des écrans (clair / sombre) et la liste des chevauchements.

## Limites connues

- Images du synoptique et graphiques dessinés par le code : couleurs d'origine.
- Cases à cocher, barres de défilement, sélecteur de date et contours des champs texte restent dessinés par Windows (plus clairs en mode sombre).
- Les boîtes `MessageBox` sont gérées par Windows et ne suivent pas le thème.
- Quelques chevauchements d'origine subsistent (titres derrière des boutons, étiquettes longues dans Configuration centrale / Automate) : ils sont listés par l'outil de capture.