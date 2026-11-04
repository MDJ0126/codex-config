# Pixel Foliage 指南

Pixora Studio 產品系列的獨立植被工具。草、花、灌木與樹木共享風場、雲影與接觸反應。

## 快速開始

1. 開啟 Tools → TurnRoom → Pixora Studio → Foliage。尋找範例並開啟09-MixedGarden，按Play後訪客會穿過植物。場景切換由使用者明確操作。

2. 為區域指定世界與設定檔。設定檔需要單一子網格與材質。網格原點位於底部，頂點透明度控制彎曲權重。樹幹可保持穩定，樹冠隨風移動。

3. 動畫幀率0表示連續，5/8/12表示分段。錯開更新不會限制繪製幀率。手動時間可比較相同時刻。

4. 選擇表面圖層、射線高度與最大坡度，然後烘焙至碰撞體。烘焙取代清單，復原可還原。忽略未命中射線與觸發器。

## 選用整合

另行安裝Pixel Camera，選擇場景攝影機與Full Camera預設後連接。URP Renderer Feature需另行設定。範例11屬於組合配置。

從Pixora匯出透明紋理並選擇。建立新材質不會修改紋理或工作文件。精靈植物使用有UV的四邊形與Camera Facing。

## 範例

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

## 驗證與限制

開發預覽0.1.0。靜態編譯不等於Unity匯入與Play驗證。由於暫停畫面操作，外觀、效能、選擇器、復原與全部語言配置尚未驗證。Affinity標誌仍待製作。

已實作Built-in與URP17著色器路徑。未確認HDRP、XR、行動效能與動作向量。照明為自訂方向光與雲影，並非完整PBR或額外光源支援。每區域使用最近16個互動來源。

11個獨立範例無需Camera或Pixora。本體不參照範例；匯入不自動開啟場景或修改建置設定。

互動更新頻率預設為10Hz，0代表每次請求都收集。軌跡持續時間控制角色通過後的恢復，0停用歷史。動畫FPS0保持連續風。分段動畫不會略過GPU繪製。SoftGarden-Lightweight在既有Automatic條件下重用原圖，不降低原始幾何渲染解析度。桌面控制已暫停，目前UI與改進場景的新截圖尚未驗證。

## 渲染與模組選項

完整攝影機朝向跟隨俯仰與旋轉，一般朝向保持植物直立。UV彎曲與透視變形用於有UV的紋理卡片。三種區域顏色、強度、縮放和邊界柔和度控制大塊色彩；草與地面範例共用設定。兩層雜訊沿光線方向投影至雲層高度，水平光使用安全回退。可參照方向光；場景光照啟用Built-in/URP主光與陰影接收，不包括額外光源或完整PBR。偵錯檢視為一般、風、接觸。08包含方向光，10比較兩種朝向，06的四個FPS區域保持相同朝向。外觀、陰影偏移與實際介面驗證仍待完成。



<!-- prismatic-woodland -->

## Prismatic Woodland

Prismatic Woodland：開啟 Samples/Scenes/12-PrismaticWoodland.unity。此參考研究以接近正面的正交攝影機呈現八種樹冠配色、灌木、蕨類、花卉、玄武岩懸崖及垂藤。樹木與地被共用風場，懸崖與垂藤為靜態。大氣及柔光使用範例網格材質，不需影像紋理或額外後製套件。選用13-PixelCameraWoodland將相同場景連結至360p Full Camera。URP仍需既有Renderer Feature。已在開發QA專案擷取Built-in實際外觀，效能及參考一致性尚未驗證。 修訂場景包含48棵樹、45個灌木、260個蕨類及136組花卉，前景改為有實際反射的河流。範例平面反射與柔和Bloom僅支援Built-in，不是URP功能。反射預設寬512像素、12Hz，每次更新增加一次場景繪製。選用Pixel Camera在Bloom後處理。外觀透過實際擷取檢查，不保證其他平台效能。

<!-- /prismatic-woodland -->

<!-- pastel-examples -->

溫暖粉彩範例：Camera保留原有攝影機與效果示範，加入柔和光照及花園邊界。植被範例保留接觸路徑與同條件幀率比較。Prismatic Woodland含48棵樹、45叢灌木、260叢蕨類、136組花與6座覆苔守護者、貓頭鷹、兔子雕像。雕像靜止，植物共用風場。幾何增加會提高繪製成本，並非效能最佳化。水面反射與Bloom僅為Built-in範例效果。

<!-- /pastel-examples -->









<!-- palette-guide-link -->

[植被調色盤](../PaletteGuide.zh-Hant.html)

<!-- /palette-guide-link -->
