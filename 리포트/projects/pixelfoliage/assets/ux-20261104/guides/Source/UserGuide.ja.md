# Pixel Foliage ガイド

Pixora Studio の独立した植生ツールです。草、花、低木、木が風、雲の陰影、接触反応を共有します。

## はじめに

1. Tools → TurnRoom → Pixora Studio → Foliage を開きます。サンプルから09-MixedGardenを開き、Playを押すと訪問者が植物の間を移動します。シーン変更は明示的に行います。

2. パッチにワールドとプロファイルを指定します。プロファイルにはサブメッシュ1個とマテリアルが必要です。原点は根元、頂点アルファは曲がりの重みです。木は幹を固定し樹冠を動かせます。

3. Animation Fpsの0は連続、5/8/12は段階再生です。更新分散は植物ごとの更新をずらし、描画FPSを制限しません。手動時間で同じ瞬間を比較できます。

4. 表面レイヤー、レイ高さ、最大傾斜を選びベイクします。配置リストは置換され、元に戻すで復元します。レイが外れた場所とトリガーは除外します。

## オプション連携

Pixel Cameraを別途導入し、カメラとFull Cameraプリセットを選んで連携します。URPのRenderer Featureは別途設定します。サンプル11はバンドル用です。

Pixoraから透明テクスチャを書き出して選択します。新規マテリアルを作成し、元画像と作業文書は変更しません。スプライト植物はUV付きquadとCamera Facingを使います。

## サンプル

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

## 検証と制限

開発プレビュー0.1.0です。静的コンパイルはUnityの導入・Play検証とは異なります。画面操作保留中のため外観、性能、選択・Undo操作、全言語レイアウトは未検証です。Affinityロゴも未完了です。

Built-inとURP17のシェーダー経路を実装しました。HDRP、XR、モバイル性能、モーションベクトルは未確認です。独自の方向光と雲の陰影を使い、完全なPBRや追加光には対応しません。接触はパッチごとに近い16件です。

独立サンプル11個はCameraとPixora不要です。本体はサンプルを参照しません。導入でシーンやビルド設定を自動変更しません。

接触更新頻度の初期値は10Hzで、0は要求ごとの収集です。余韻の時間は通過後の復元時間で、0は履歴を無効にします。Animation Fps0は連続した風です。段階的な動きはGPU描画を省略しません。SoftGarden-Lightweightは既存のAutomatic条件で元画像を再利用し、元のジオメトリ描画解像度は下げません。画面操作の停止中につき、新しいUIとシーンのキャプチャは未検証です。

## 描画とモジュール設定

完全ビルボードはカメラの傾きと回転に追従し、通常のカメラ向きは植物を直立させます。UV曲げとUV遠近変形はUV付きテクスチャカードに使用します。三色の領域の強さ・スケール・境界で大きな色のまとまりを作ります。草と地面の例は同じ領域設定です。雲は二層のノイズを光の方向に沿って雲の高さへ投影し、水平光には安全な代替を使います。平行光源を参照でき、シーン照明を使うとBuilt-in/URPの主光源と影受信が有効になります。追加ライトと完全なPBRは対象外です。デバッグは通常・風・接触。08に平行光源、10に二つのビルボード、06には同じ向きの四つのFPS区画があります。外観・影バイアス・UIの実動作検証は未完了です。



<!-- prismatic-woodland -->

## Prismatic Woodland

Prismatic Woodland: Samples/Scenes/12-PrismaticWoodland.unityを開きます。ほぼ正面の平行投影カメラで8色の樹冠、低木、シダ、花、玄武岩の崖と垂れるつるを配置した参考研究です。樹木と下草は風を共有し、崖とつるは静的です。大気と柔らかな光はサンプルのメッシュ材質で、画像テクスチャや追加のポスト処理は不要です。任意の13-PixelCameraWoodlandは同じシーンに360pのFull Cameraを適用します。URPでは既存のRenderer Featureが必要です。Built-inの実行画像は撮影済みです。性能と参考画像との一致は未検証です。 改訂シーンは木48本、低木45個、シダ260個、花群136個です。前景を反射する川に変更しました。サンプルの平面反射とソフトBloomはBuilt-in専用でURP機能ではありません。反射は初期値512ピクセル幅・12Hzで、更新時に追加描画が発生します。任意のPixel CameraはBloom後に適用します。実機撮影で外観を確認し、他環境の性能は保証しません。

<!-- /prismatic-woodland -->

<!-- pastel-examples -->

暖かなパステル例：Cameraの既存カメラと効果の比較を維持し、柔らかな照明と庭の境界を追加しました。植生例は接触経路と同条件のフレーム比較を維持します。Prismatic Woodlandは木48本、低木45株、シダ260株、花群136個、苔むした守護者・フクロウ・ウサギの像6体です。像は静的で植物は風を共有します。描画負荷は増え、最適化機能ではありません。水面反射とBloomはBuilt-in専用サンプルです。

<!-- /pastel-examples -->









<!-- palette-guide-link -->

[植生パレット](../PaletteGuide.ja.html)

<!-- /palette-guide-link -->
