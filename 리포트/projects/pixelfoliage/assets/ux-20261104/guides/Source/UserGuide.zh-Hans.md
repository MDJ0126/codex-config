# Pixel Foliage 指南

Pixora Studio 产品系列的独立植被工具。草、花、灌木和树木共享风场、云影和接触反应。

## 快速开始

1. 打开 Tools → TurnRoom → Pixora Studio → Foliage。找到示例并打开09-MixedGarden，按Play后访客会穿过植物。场景切换始终由用户明确操作。

2. 为区域指定世界与配置。配置需要单个子网格和材质。网格原点位于底部，顶点透明度控制弯曲权重。树干可保持稳定，树冠随风移动。

3. 动画帧率0表示连续，5/8/12表示分段。错开更新不会限制渲染帧率。手动时间可比较相同时刻。

4. 选择表面层、射线高度和最大坡度，然后烘焙到碰撞体。烘焙替换列表，撤销可恢复。忽略未命中射线与触发器。

## 可选集成

单独安装Pixel Camera，选择场景相机和Full Camera预设后连接。URP Renderer Feature需另行设置。示例11属于捆绑配置。

从Pixora导出透明纹理并选择。创建新材质不会修改纹理或工作文档。精灵植物使用带UV的四边形和Camera Facing。

## 示例

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

## 验证与限制

开发预览0.1.0。静态编译不等于Unity导入和Play验证。由于暂停屏幕操作，外观、性能、选择器、撤销及全部语言布局尚未验证。Affinity标志仍待制作。

已实现Built-in与URP17着色器路径。未确认HDRP、XR、移动性能和运动矢量。照明为自定义方向光和云影，不是完整PBR或附加光源支持。每区域使用最近16个交互源。

11个独立示例无需Camera或Pixora。本体不引用示例；导入不自动打开场景或修改构建设置。

交互更新频率默认为10Hz，0表示每次请求都采集。轨迹持续时间控制角色经过后的恢复，0禁用历史。动画FPS0保持连续风。分段动画不会跳过GPU绘制。SoftGarden-Lightweight在现有Automatic条件下复用原图，不降低原始几何渲染分辨率。桌面控制已暂停，当前UI和改进场景的新截图尚未验证。

## 渲染与模块选项

完整相机朝向跟随相机俯仰与旋转，普通朝向保持植物直立。UV弯曲与透视变形用于有UV的纹理卡片。三种区域颜色、强度、缩放和边界柔和度控制大块色彩；草与地面示例共用设置。两层噪声沿光线方向投影至云层高度，水平光使用安全回退。可引用方向光；场景光照启用Built-in/URP主光与阴影接收，不包括额外光源或完整PBR。调试视图为正常、风、接触。08包含方向光，10比较两种朝向，06的四个FPS区域保持相同朝向。外观、阴影偏移和实际界面验证仍待完成。



<!-- prismatic-woodland -->

## Prismatic Woodland

Prismatic Woodland：打开 Samples/Scenes/12-PrismaticWoodland.unity。此参考研究以接近正面的正交相机展示八种树冠配色、灌木、蕨类、花卉、玄武岩悬崖及垂藤。树木与地被共享风场，悬崖和垂藤为静态。大气及柔光使用示例网格材质，无需图像纹理或额外后处理包。可选13-PixelCameraWoodland将相同场景连接至360p Full Camera。URP仍需现有Renderer Feature。已在开发QA项目捕获Built-in实际外观，性能及参考一致性尚未验证。 修订场景包含48棵树、45个灌木、260个蕨类及136组花卉，前景改为有真实反射的河流。示例平面反射和柔和Bloom仅支持Built-in，不是URP功能。反射默认宽512像素、12Hz，每次更新增加一次场景渲染。可选Pixel Camera在Bloom后处理。外观通过实际捕获检查，不保证其他平台性能。

<!-- /prismatic-woodland -->

<!-- pastel-examples -->

温暖的粉彩示例：Camera保留原有相机与效果演示，增加柔和光照及花园边界。植被示例保留接触路径与同条件帧率对比。Prismatic Woodland包含48棵树、45丛灌木、260丛蕨类、136组花和6座覆苔守护者、猫头鹰、兔子雕像。雕像静止，植物共享风。几何增加会提高渲染成本，并非性能优化。水面反射与Bloom仅为Built-in示例效果。

<!-- /pastel-examples -->









<!-- palette-guide-link -->

[植被调色板](../PaletteGuide.zh-Hans.html)

<!-- /palette-guide-link -->
