# Guide Pixel Foliage

Végétation indépendante de Pixora Studio. Herbe, fleurs, arbustes et arbres partagent vent, nuages et interaction.

## Démarrage

1. Ouvrez Tools → TurnRoom → Pixora Studio → Foliage. Trouvez les exemples, ouvrez09-MixedGarden puis Play. Le visiteur traverse les plantes. Le changement de scène reste explicite.

2. Attribuez World et Profile à une parcelle. Le profil nécessite un sous-maillage et un matériau. Le pivot est à la base ; l’alpha des sommets règle la flexion. Le tronc peut rester fixe et la couronne bouger.

3. Animation Fps0 est continu ;5/8/12 produit des étapes. Le décalage ne limite pas les FPS de rendu. Manual Time compare un instant fixe.

4. Choisissez calques, hauteur du rayon et pente maximale puis ancrez sur les colliders. La liste est remplacée ; Annuler la restaure. Rayons manqués et déclencheurs sont exclus.

## Connexions facultatives

Installez Pixel Camera séparément puis liez une caméra avec un préréglage Full Camera. Configurez sa Renderer Feature dans URP. La scène11 appartient au lot.

Exportez une texture transparente de Pixora. L’adaptateur crée un nouveau matériau sans modifier texture ou document. Utilisez un quad avec UV et Camera Facing pour les sprites.

## Exemples

- 01-MeadowBreeze.unity · MeadowGrass · 0 FPS
- 02-BlossomWalk.unity · IvoryDaisy · 0 FPS
- 03-ShrubGarden.unity · RoundedShrub · 0 FPS
- 04-YoungWoodland.unity · BranchingTree · 0 FPS
- 05-ContactTrail.unity · MeadowGrass · 0 FPS
- 06-SteppedRhythm.unity · MeadowGrass · 5 FPS
- 07-SharedGusts.unity · RoseBlossom · 0 FPS
- 08-CloudLight.unity · MeadowGrass · 0 FPS
- 09-MixedGarden.unity · MeadowGrass · 0 FPS
- 10-ExtensionGrove.unity · BranchingTree · 0 FPS
- 12-PrismaticWoodland.unity · Layered colorful woodland · 0 FPS

## Validation et limites

Aperçu0.1.0. Compiler ne valide pas l’import et Play dans Unity. Aspect, performances, sélecteurs, Annuler et langues restent à vérifier pendant la pause du bureau. Le logo Affinity reste à créer.

Chemins Built-in et URP17 implémentés. HDRP, XR, performances mobiles et vecteurs de mouvement non garantis. Lumière directionnelle personnalisée et nuages, sans PBR complet ni lumières supplémentaires.16 sources proches par parcelle.

Les11 exemples autonomes n’exigent ni Camera ni Pixora. L’import n’ouvre pas de scène et ne modifie pas les réglages de compilation.

La fréquence des interactions est10Hz;0 collecte à chaque demande. La durée de la trace contrôle la récupération;0 désactive l’historique. Animation Fps0 garde un vent continu. Les étapes ne suppriment pas le rendu GPU. SoftGarden-Lightweight réutilise l’image selon les conditions Automatic existantes; il ne réduit pas la résolution géométrique initiale. Les nouvelles captures UI et scènes restent à vérifier pendant la pause du contrôle du bureau.

## Options de rendu et modules

Le billboard complet suit l’inclinaison et la rotation de la caméra ; le mode normal garde les plantes droites. Courbure et perspective UV concernent les cartes texturées avec UV. Trois couleurs et les réglages d’intensité, d’échelle et de douceur créent de grandes régions, partagées par l’herbe et le sol. Deux couches de bruit sont projetées à la hauteur des nuages selon la lumière, avec un repli sûr à l’horizon. L’éclairage de scène active la lumière principale et la réception des ombres Built-in/URP, sans lumières supplémentaires ni PBR complet. Débogage : Normal, Vent, Contact. La scène08 inclut un soleil,10 compare les billboards et06 conserve la même orientation pour les quatre zones FPS. Aspect, biais des ombres et UI restent à vérifier dans Unity.



<!-- prismatic-woodland -->

## Prismatic Woodland

Prismatic Woodland : ouvrez Samples/Scenes/12-PrismaticWoodland.unity. Cette étude utilise une caméra orthographique presque frontale, huit palettes d’arbres, des arbustes, fougères, fleurs, une falaise de basalte et des lianes. Arbres et sous-bois partagent le vent ; rochers et lianes restent statiques. Atmosphère et halo utilisent des matériaux de maillage sans textures d’image ni post-traitement supplémentaire. La scène facultative13-PixelCameraWoodland ajoute Full Camera à360p. URP exige la Renderer Feature existante. L’aspect Built-in est capturé en QA ; performances et équivalence restent non vérifiées. Scène révisée :48arbres,45arbustes,260fougères et136groupes de fleurs, avec rivière réfléchissante. Réflexion plane et Bloom sont des exemples Built-in, pas des fonctions URP. Le reflet utilise512pixels de large à12Hz et ajoute un rendu lors des mises à jour. Pixel Camera reste facultatif après Bloom. Les captures natives montrent le résultat, sans garantie de performances ailleurs.

<!-- /prismatic-woodland -->

<!-- pastel-examples -->

Exemples pastel chaleureux : Camera conserve ses démonstrations avec un éclairage doux et des bordures végétales. Foliage préserve les chemins de contact et les comparaisons équivalentes. Prismatic Woodland comprend48 arbres,45 arbustes,260 fougères,136 bouquets et6 statues moussues de gardien, chouette et lièvre. Les statues restent statiques ; les plantes partagent le vent. La géométrie augmente le coût de rendu ; ce n’est pas une optimisation. Reflet du fleuve et bloom restent des effets d’exemple Built-in uniquement.

<!-- /pastel-examples -->








<!-- palette-guide-link -->

[Palette de végétation](../PaletteGuide.fr.html)

<!-- /palette-guide-link -->
