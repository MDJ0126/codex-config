# Prototype GrowingCity

[English](../en/README.md) · [한국어](../ko/README.md) · [日本語](../ja/README.md) · [简体中文](../zh-Hans/README.md) · [繁體中文](../zh-Hant/README.md) · [Deutsch](../de/README.md) · [Français](../fr/README.md) · [Español](../es/README.md) · [Português](../pt/README.md)

Bombardez une ville qui grandit seule. Les bâtiments voisins évoluent ensemble et les zones détruites se reconstruisent.

Ouvrez Assets/Scenes/City Prototype.unity avec Unity 6000.0.83f1 et URP 17.0.4. Le projet est un prototype Unity ; son exécution dans un navigateur reste non vérifiée.

Tracez une direction sur la ville puis relâchez pour lancer la formation. Les avions traversent tout l’écran depuis l’extérieur. 1 : bombe, 2 : nucléaire, Espace : pause/reprise, Échap : annuler.

Chaque avion commence à une bombe par seconde. Améliorez cadence, capacité, rayon, dégâts, formation, vitesse, blindage et résistance. La croissance accélère les nouvelles constructions. Les améliorations concernent les prochains vols ou bâtiments.

Chaque avion arrivé ajoute exactement 50% de recharge normale. Deux arrivées donnent une utilisation, les surplus sont conservés. La recharge passive prend 10 secondes de jeu et remplit au maximum une utilisation.

Le nucléaire a un délai indépendant de 100 secondes dès acceptation d’un ordre valide, y compris en file. Disponible au départ, sans consommer de charge normale. Les arrivées et la vitesse 3x ne le réduisent pas. La pause arrête le compteur ; Nouvelle ville le réinitialise.

Les bâtiments tirent des missiles de niveau adapté au suivi limité. Les avions peuvent être endommagés et abattus. Les bâtiments évolués ont davantage de résistance et peuvent rétrécir. La destruction rapporte de l’or et réduit le développement urbain.

Au premier lancement, la langue suit le système. Le bouton de langue parcourt Système et neuf langues ; le choix est enregistré sur cet appareil. Une langue non prise en charge utilise l’anglais.

## Commandes et paramètres

Les originaux externes Assets/polyperfect/Low Poly Epic City/ restent intacts. Utilisez des ressources propres au projet et des adaptateurs. Ne redistribuez pas séparément les ressources du Store.

Limites : affichage Unity, changement de langue en jeu, déploiement Web et équilibrage non vérifiés ici. La compilation et les tests indépendants ne remplacent pas un test Play.

[HTML](index.html)
