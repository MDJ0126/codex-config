# Guía de Pixel Foliage

Vegetación independiente de Pixora Studio. Hierba, flores, arbustos y árboles comparten viento, nubes e interacción.

## Inicio rápido

1. Abra Tools → TurnRoom → Pixora Studio → Foliage. Busque los ejemplos, abra09-MixedGarden y pulse Play. El visitante recorre las plantas. Los cambios de escena son explícitos.

2. Asigne World y Profile a una parcela. El perfil necesita una submalla y material. El origen está en la base; el alfa de vértice controla la flexión. El tronco puede quedar fijo y la copa moverse.

3. Animation Fps0 es continuo;5/8/12 es escalonado. El desfase no limita los FPS de renderizado. Manual Time permite comparar el mismo instante.

4. Elija capas, altura del rayo y pendiente máxima; fije sobre colisionadores. Se sustituye la lista y Deshacer la restaura. Se excluyen rayos fallidos y disparadores.

## Conexiones opcionales

Instale Pixel Camera por separado y vincule una cámara con un preajuste Full Camera. Configure su Renderer Feature en URP. La escena11 pertenece al paquete conjunto.

Exporte una textura transparente de Pixora. El adaptador crea un material nuevo sin cambiar la textura ni el documento. Use una malla quad con UV y Camera Facing para plantas sprite.

## Ejemplos

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

## Validación y límites

Vista previa0.1.0. Compilar no equivale a importar y ejecutar en Unity. Apariencia, rendimiento, selectores, Deshacer y los idiomas siguen pendientes mientras se pausa el escritorio. También falta el logotipo de Affinity.

Se implementaron rutas Built-in y URP17. No se garantiza HDRP, XR, rendimiento móvil ni vectores de movimiento. La luz es direccional propia con nubes, no PBR completo ni luces adicionales. Hay16 fuentes cercanas por parcela.

Los11 ejemplos independientes no requieren Camera ni Pixora. Importar no abre escenas ni cambia ajustes de compilación.

La frecuencia de interacción es10Hz;0 recoge cada solicitud. La duración del rastro controla la recuperación;0 desactiva el historial. Animation Fps0 mantiene el viento continuo. Los pasos no omiten el renderizado GPU. SoftGarden-Lightweight reutiliza la imagen con las condiciones Automatic existentes; no reduce la resolución geométrica original. Las nuevas capturas de UI y escenas están pendientes mientras el control del escritorio está pausado.

## Opciones de renderizado y módulos

El billboard completo sigue la inclinación y rotación de la cámara; el normal mantiene las plantas verticales. La curvatura y perspectiva UV son para tarjetas texturizadas con UV. Tres colores y controles de intensidad, escala y suavidad crean grandes regiones; hierba y suelo comparten ajustes. Dos capas de ruido se proyectan a la altura de las nubes según la luz, con una alternativa segura en el horizonte. La iluminación de escena activa la luz principal y recepción de sombras en Built-in/URP, sin luces adicionales ni PBR completo. La depuración muestra Normal, Viento o Contacto. Escena08 incluye un sol, 10 compara billboards y06 mantiene la misma orientación en las cuatro parcelas FPS. Aspecto, sesgo de sombras y UI siguen pendientes de validación nativa.



<!-- prismatic-woodland -->

## Prismatic Woodland

Prismatic Woodland: abra Samples/Scenes/12-PrismaticWoodland.unity. Este estudio usa cámara ortográfica casi frontal, ocho paletas de árboles, arbustos, helechos, flores, un acantilado de basalto y enredaderas. Árboles y sotobosque comparten viento; roca y enredaderas son estáticas. La atmósfera y el brillo usan materiales de malla sin texturas de imagen ni posprocesado adicional. La escena opcional 13-PixelCameraWoodland añade Full Camera a360p. URP necesita la Renderer Feature existente. El aspecto Built-in se ha capturado en QA; rendimiento y equivalencia siguen sin verificar. Escena revisada:48árboles,45arbustos,260helechos y136grupos de flores; río reflectante en primer plano. La reflexión planar y Bloom son ejemplos exclusivos de Built-in, no funciones URP. El reflejo usa512p de ancho y12Hz; cada actualización añade un render de escena. Pixel Camera es opcional y se aplica después de Bloom. La captura nativa valida el aspecto, sin garantizar rendimiento en otras plataformas.

<!-- /prismatic-woodland -->

<!-- pastel-examples -->

Ejemplos pastel cálidos: Camera conserva las demostraciones de cámara y efectos con luz suave y bordes de jardín. Foliage mantiene rutas de contacto y comparaciones equivalentes. Prismatic Woodland contiene48 árboles,45 arbustos,260 helechos,136 grupos florales y6 estatuas con musgo de guardián, búho y liebre. Las estatuas son estáticas y las plantas comparten viento. La geometría adicional aumenta el coste; no es una optimización. Reflejos del río y bloom son ejemplos exclusivos de Built-in.

<!-- /pastel-examples -->









<!-- palette-guide-link -->

[Paleta de vegetación](../PaletteGuide.es.html)

<!-- /palette-guide-link -->
