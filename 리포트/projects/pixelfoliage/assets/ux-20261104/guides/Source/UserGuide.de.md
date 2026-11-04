# Pixel Foliage-Anleitung

Eigenständige Vegetation aus Pixora Studio. Gras, Blumen, Sträucher und Bäume teilen Wind, Wolkenlicht und Interaktion.

## Erste Schritte

1. Öffnen Sie Tools → TurnRoom → Pixora Studio → Foliage. Öffnen Sie09-MixedGarden unter den Beispielen und drücken Sie Play. Der Besucher bewegt sich durch die Pflanzen. Szenenwechsel erfolgen ausdrücklich.

2. Weisen Sie einer Fläche World und Profile zu. Ein Profil benötigt ein Submesh und Material. Der Ursprung liegt an der Basis; Vertex-Alpha steuert die Biegung. Stamm und Wurzeln können fest bleiben.

3. Animation Fps0 bedeutet kontinuierlich,5/8/12 stufenweise. Versetzte Aktualisierungen begrenzen nicht die Render-FPS. Manual Time vergleicht einen festen Zeitpunkt.

4. Wählen Sie Ebenen, Strahlhöhe und maximale Neigung und backen Sie auf Collider. Die Liste wird ersetzt; Rückgängig stellt sie wieder her. Fehlende Treffer und Trigger werden ausgelassen.

## Optionale Verbindungen

Installieren Sie Pixel Camera separat und verbinden Sie eine Kamera mit einer Full Camera-Vorgabe. Konfigurieren Sie die Renderer Feature in URP. Szene11 gehört zum Bündel.

Exportieren Sie eine transparente Textur aus Pixora. Der Adapter erstellt ein neues Material ohne Textur oder Arbeitsdatei zu ändern. Für Sprites sind UV-Quad und Camera Facing vorgesehen.

## Beispiele

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

## Prüfung und Grenzen

Entwicklungsvorschau0.1.0. Kompilierung ersetzt weder Unity-Import noch Play. Darstellung, Leistung, Auswahl, Rückgängig und Sprachlayouts bleiben während der Desktop-Pause ungeprüft. Das Affinity-Logo steht ebenfalls aus.

Built-in und URP17 sind implementiert. HDRP, XR, mobile Leistung und Bewegungsvektoren werden nicht zugesichert. Eigene Richtungsbeleuchtung mit Wolken statt vollständigem PBR oder Zusatzlichtern.16 nächste Quellen pro Fläche.

Die11 eigenständigen Beispiele benötigen weder Camera noch Pixora. Der Import öffnet keine Szene und verändert keine Build-Einstellungen.

Die Interaktionsrate ist10Hz;0 erfasst jede Anfrage. Die Spurdauer steuert die Erholung;0 deaktiviert den Verlauf. Animation Fps0 hält den Wind kontinuierlich. Stufen überspringen kein GPU-Rendering. SoftGarden-Lightweight verwendet das Quellbild unter den vorhandenen Automatic-Bedingungen wieder; die ursprüngliche Geometrieauflösung sinkt nicht. Neue UI- und Szenenaufnahmen bleiben bei pausierter Desktopsteuerung ungeprüft.

## Rendering- und Moduloptionen

Das vollständige Billboard folgt Kameraneigung und Rollwinkel; der normale Modus hält Pflanzen aufrecht. UV-Biegung und Perspektive sind für texturierte Karten mit UVs vorgesehen. Drei Farben sowie Stärke, Skalierung und weiche Übergänge erzeugen große Farbbereiche; Gras und Boden teilen die Einstellungen. Zwei Rauschschichten werden entlang der Lichtrichtung auf die Wolkenhöhe projiziert, mit sicherem Horizont-Fallback. Szenenbeleuchtung aktiviert Hauptlicht und Schattenempfang für Built-in/URP, ohne zusätzliche Lichter oder vollständiges PBR. Debug: Normal, Wind, Kontakt. Szene08 enthält Sonnenlicht,10 vergleicht Billboards und06 behält gleiche Ausrichtung in allen vier FPS-Beeten. Darstellung, Schatten-Bias und UI benötigen native Prüfung.



<!-- prismatic-woodland -->

## Prismatic Woodland

Prismatic Woodland: Samples/Scenes/12-PrismaticWoodland.unity öffnen. Die Studie zeigt acht Baumfarbpaletten, Sträucher, Farne, Blumen, eine Basaltklippe und Ranken mit fast frontaler orthografischer Kamera. Bäume und Unterwuchs teilen Wind; Klippe und Ranken sind statisch. Atmosphäre und Lichtschein verwenden Mesh-Materialien ohne Bildtexturen oder zusätzliche Nachbearbeitung. Optional ergänzt13-PixelCameraWoodland dieselbe Szene um Full Camera mit360p. URP benötigt die vorhandene Renderer Feature. Das Built-in-Aussehen wurde in QA aufgenommen; Leistung und Gleichwertigkeit sind ungeprüft. Überarbeitet:48Bäume,45Sträucher,260Farne und136Blumengruppen mit spiegelndem Fluss. Planare Reflexion und sanftes Bloom sind Built-in-Beispiele, keine URP-Funktionen. Reflexion:512Pixel breit,12Hz; jede Aktualisierung benötigt einen zusätzlichen Szenenrender. Optional folgt Pixel Camera auf Bloom. Native Aufnahmen zeigen das Ergebnis; andere Plattformleistungen sind ungeprüft.

<!-- /prismatic-woodland -->

<!-- pastel-examples -->

Warme Pastellbeispiele: Camera behält Kamera- und Effektdemos mit weicher Beleuchtung und Gartenrändern. Foliage bewahrt Kontaktwege und gleiche Vergleichsbedingungen. Prismatic Woodland enthält48 Bäume,45 Sträucher,260 Farne,136 Blumengruppen und6 bemooste Wächter-, Eulen- und Hasenstatuen. Statuen sind statisch, Pflanzen teilen Wind. Mehr Geometrie erhöht Renderkosten; keine Leistungsoptimierung. Flussreflexion und Bloom sind ausschließlich Built-in-Beispiele.

<!-- /pastel-examples -->









<!-- palette-guide-link -->

[Vegetationspalette](../PaletteGuide.de.html)

<!-- /palette-guide-link -->
