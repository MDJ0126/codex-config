# Guia do Pixel Foliage

Vegetação independente do Pixora Studio. Grama, flores, arbustos e árvores compartilham vento, nuvens e interação.

## Primeiros passos

1. Abra Tools → TurnRoom → Pixora Studio → Foliage. Encontre os exemplos, abra09-MixedGarden e pressione Play. O visitante percorre as plantas. A troca de cena é explícita.

2. Atribua World e Profile à área. O perfil exige uma submalha e material. A origem fica na base; alfa de vértice controla a curvatura. Tronco e raízes podem ficar fixos.

3. Animation Fps0 é contínuo;5/8/12 usa etapas. A distribuição não limita os FPS de renderização. Manual Time compara um instante fixo.

4. Escolha camadas, altura do raio e inclinação máxima e fixe nos colisores. A lista é substituída; Desfazer a restaura. Raios sem contato e gatilhos são excluídos.

## Conexões opcionais

Instale Pixel Camera separadamente e vincule uma câmera com uma predefinição Full Camera. Configure a Renderer Feature no URP. A cena11 pertence ao pacote conjunto.

Exporte uma textura transparente do Pixora. O adaptador cria material novo sem alterar a textura ou documento. Use quad com UV e Camera Facing para plantas sprite.

## Exemplos

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

## Validação e limites

Prévia0.1.0. Compilar não valida importação e Play no Unity. Aparência, desempenho, seletores, Desfazer e idiomas estão pendentes durante a pausa da tela. O logotipo Affinity também está pendente.

Caminhos Built-in e URP17 implementados. HDRP, XR, desempenho móvel e vetores de movimento não são garantidos. Luz direcional própria com nuvens, sem PBR completo ou luzes extras.16 fontes próximas por área.

Os11 exemplos independentes não exigem Camera nem Pixora. A importação não abre cenas nem altera configurações de compilação.

A frequência de interação é10Hz;0 coleta a cada solicitação. A duração do rastro controla a recuperação;0 desativa o histórico. Animation Fps0 mantém o vento contínuo. Os passos não pulam a renderização GPU. SoftGarden-Lightweight reutiliza a imagem nas condições Automatic existentes; não reduz a resolução geométrica original. As novas capturas da interface e das cenas estão pendentes durante a pausa do controle da área de trabalho.

## Opções de renderização e módulos

O billboard completo acompanha inclinação e rotação da câmera; o normal mantém as plantas verticais. Curvatura e perspectiva UV são para cartões com textura e UV. Três cores com intensidade, escala e suavidade formam grandes regiões; grama e chão compartilham ajustes. Duas camadas de ruído são projetadas na altura das nuvens pela direção da luz, com alternativa segura no horizonte. A iluminação da cena ativa luz principal e recepção de sombras em Built-in/URP, sem luzes adicionais ou PBR completo. Depuração: Normal, Vento, Contato. A cena08 inclui sol,10 compara billboards e06 mantém a mesma orientação nas quatro áreas FPS. Aparência, viés de sombras e interface ainda precisam de validação nativa.



<!-- prismatic-woodland -->

## Prismatic Woodland

Prismatic Woodland: abra Samples/Scenes/12-PrismaticWoodland.unity. Este estudo usa câmera ortográfica quase frontal, oito paletas de árvores, arbustos, samambaias, flores, penhasco de basalto e trepadeiras. Árvores e vegetação baixa compartilham vento; rochas e trepadeiras são estáticas. Atmosfera e brilho usam materiais de malha sem texturas de imagem nem pós-processamento adicional. A cena opcional13-PixelCameraWoodland adiciona Full Camera a360p. URP exige a Renderer Feature existente. A aparência Built-in foi capturada em QA; desempenho e equivalência permanecem sem verificação. Cena revisada:48árvores,45arbustos,260samambaias e136grupos de flores, com rio refletivo. Reflexão planar e Bloom suave são exemplos exclusivos de Built-in, não recursos URP. Reflexo com512pixels de largura e12Hz; cada atualização adiciona um render da cena. Pixel Camera é opcional e ocorre após Bloom. Capturas nativas mostram o resultado, sem garantir desempenho em outras plataformas.

<!-- /prismatic-woodland -->

<!-- pastel-examples -->

Exemplos em pastel quente: Camera mantém as demonstrações com luz suave e bordas de jardim. Foliage preserva caminhos de contato e comparações equivalentes. Prismatic Woodland tem48 árvores,45 arbustos,260 samambaias,136 grupos de flores e6 estátuas com musgo de guardião, coruja e lebre. As estátuas são estáticas; plantas compartilham vento. Mais geometria aumenta o custo; não é otimização. Reflexo do rio e bloom são exemplos exclusivos do Built-in.

<!-- /pastel-examples -->








<!-- palette-guide-link -->

[Paleta de vegetação](../PaletteGuide.pt-BR.html)

<!-- /palette-guide-link -->
