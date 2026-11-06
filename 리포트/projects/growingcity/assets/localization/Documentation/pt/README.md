# Protótipo GrowingCity

[English](../en/README.md) · [한국어](../ko/README.md) · [日本語](../ja/README.md) · [简体中文](../zh-Hans/README.md) · [繁體中文](../zh-Hant/README.md) · [Deutsch](../de/README.md) · [Français](../fr/README.md) · [Español](../es/README.md) · [Português](../pt/README.md)

Bombardeie uma cidade que cresce sozinha. Edifícios vizinhos se combinam e evoluem; áreas destruídas são reconstruídas.

Abra Assets/Scenes/City Prototype.unity no Unity 6000.0.83f1 com URP 17.0.4. Este é um protótipo Unity; a execução no navegador não foi verificada.

Arraste sobre a cidade e solte para enviar a formação nessa direção. Os aviões entram e saem fora da tela. 1: bomba, 2: nuclear, Espaço: pausar/continuar, Esc: cancelar.

Cada avião começa com uma bomba por segundo. Melhore cadência, capacidade, raio, poder, formação, velocidade, blindagem e vida para o próximo voo. O crescimento urbano não é mais comprado.

O crescimento acelera com o tempo de jogo: 1x no início, 2x aos 60 segundos e máximo de 8x aos 420 segundos, mostrado no HUD. Só edifícios de nível 3 ou superior disparam mísseis. Cada décima segunda construção é um depósito de combustível amarelo, munições vermelho ou central ciano. Destruí-los causa dano próximo e reações em cadeia. O ouro é pago uma vez pela cadeia inteira; o desenvolvimento cai e as ruínas podem ser reconstruídas.

Cada avião que chega adiciona 50% fixos de carga normal. Duas chegadas dão um uso; o excedente fica guardado. A recarga passiva leva 10 segundos de jogo e preenche no máximo um uso.

O nuclear tem recarga independente de 100 segundos desde a aceitação de uma ordem válida, incluindo ordens na fila. Começa disponível e não consome carga normal. Chegadas e velocidade 3x não reduzem o tempo. Pausar interrompe o contador; Nova cidade o reinicia.

Os edifícios disparam mísseis por nível com perseguição limitada. Os aviões podem sofrer dano e ser abatidos. Edifícios evoluídos têm mais vida e podem diminuir ao sofrer dano. A destruição rende ouro e reduz o desenvolvimento urbano.

O primeiro início segue o idioma do sistema. O botão alterna Sistema e nove idiomas; a escolha fica salva neste dispositivo. Idiomas não suportados usam inglês.

## Controles e configurações

Os originais externos Assets/polyperfect/Low Poly Epic City/ não devem ser modificados. Use recursos próprios e adaptadores. Não redistribua os recursos da loja separadamente.

Limites: renderização real no Unity, mudança de idioma durante o jogo, publicação web e equilíbrio não verificados nesta tarefa. Compilação e testes independentes não substituem Play.

[HTML](index.html)

Os edifícios explosivos têm o corpo azul vivo; as bases amarelas, vermelhas e ciano indicam o tipo. A destruição combina queda lateral, torção e elevação explosiva. Seções da fachada e pequenos destroços usam física, alguns acima dos telhados ou fora da tela. Até 36 seções compartilham o orçamento existente. Os novos efeitos e o shader ainda precisam de verificação real no Play.
