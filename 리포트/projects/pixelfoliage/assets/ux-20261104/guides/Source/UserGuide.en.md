# Pixel Foliage guide

Independent vegetation in the Pixora Studio family. Grass, flowers, shrubs and trees share wind, cloud light and contact response.

## Quick start

1. Open Tools → TurnRoom → Pixora Studio → Foliage. Find examples, open 09-MixedGarden and press Play. The visitor moves through the plants. Scene changes are always explicit.

2. Assign a World and Profile to a Patch. A profile needs one-submesh geometry and a material. Mesh roots must be at the base; vertex alpha is bend weight. Grass bends more than shrubs. Tree roots and trunks can stay stiff while the canopy moves.

3. Set Animation Fps to0 for continuous motion or5/8/12 for stepped motion. Stagger Phases offsets plant updates; this does not limit rendering FPS. Manual Time lets you compare a fixed instant.

4. For terrain, choose Surface layers, Ray height and Maximum slope, then Bake on colliders. Baking replaces the placement list; Undo restores it. Missed rays and trigger colliders are excluded.

## Optional connections

Install Pixel Camera separately, choose a scene camera and a Full Camera preset, then Link Pixel Camera. URP needs its Renderer Feature configured separately. The optional 11-PixelCameraGarden scene belongs to the bundle.

Export a transparent texture from Pixora and choose Exported texture. Create material from texture saves a new material without modifying the texture or work document. Use a UV-mapped quad and Camera Facing for sprite plants.

## Examples

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

## Validation and limits

Development preview0.1.0. C# and HLSL static validation are separate from Unity import and Play. Native visual quality, performance, picker/Undo round trips and all language layouts are pending while desktop interaction is paused. Required Affinity logo is also pending.

Built-in and URP17 shader paths are implemented. HDRP, XR, mobile performance and motion vectors are not claimed. Lighting is a custom directional style plus cloud modulation, not full PBR or additional-light support. Contact supports16 nearest sources per patch.

Eleven standalone scenes require no Camera or Pixora. Samples and optional integrations can be removed without the core referencing them. Import does not open scenes or change build settings.

Interaction Update Rate defaults to10Hz;0 collects on every request. Trail Duration controls temporary recovery after a unit passes;0 disables history. Animation Fps0 keeps wind continuous. Stepped animation is a visual choice and does not skip GPU rendering. The optional SoftGarden-Lightweight camera preset reuses the source image under the existing Automatic-mode conditions; it does not lower the original geometry-render resolution. Current UI and revised scene screenshots remain pending while desktop control is paused.

## Rendering and module options

Full Camera Facing follows camera pitch and roll; Camera Facing keeps plants upright. Use UV Bend and UV Perspective only on textured UV-mapped cards. Region Strength, Scale, Softness and three region colours control broad colour patches; grass and ground examples use the same region settings. Project Clouds projects two noise layers to Cloud Height along the light direction, with a finite horizon fallback. Directional Light can track a scene light. Use Scene Lighting enables the main-light and shadow-receiving path in Built-in/URP; additional lights and full PBR are outside this shader. Debug View selects Normal, Wind or Contact. Scene08 contains a native directional sun; Scene10 compares upright and full billboard grass. Scene06 keeps identical facing in all four FPS beds. Current visuals, shadow bias and all UI layouts still need native QA.



<!-- prismatic-woodland -->

## Prismatic Woodland

Prismatic Woodland: open Samples/Scenes/12-PrismaticWoodland.unity. This reference study combines eight tree palettes, shrubs, ferns, flowers, a basalt cliff and trailing vines under a nearly frontal orthographic camera. Wind affects trees and undergrowth; the cliff and hanging vines are static. The atmosphere and soft glow use sample mesh materials, without image textures or a post-processing dependency. Optional 13-PixelCameraWoodland uses the same scene with Full Camera at 360p. URP still needs the existing renderer feature. Built-in appearance has been captured in the development QA project; frame rate and reference equivalence remain unverified. Revised scene:48trees,45shrubs,260ferns and136flower clumps. A reflective river replaces the exposed foreground cliff. Sample-only planar reflection and soft camera bloom require the Built-in pipeline; they are not a URP feature. The reflector is512pixels wide and updates at12Hz by default, adding another scene render when refreshed. Pixel Camera remains optional and follows the bloom effect. Appearance is reviewed in the native capture report; no cross-platform performance claim is made.

<!-- /prismatic-woodland -->

<!-- pastel-examples -->

Warm pastel examples: Camera showcase and legacy scenes retain their original camera/effect demonstrations, with softer lighting and additional garden borders. Foliage examples add trees, shrubs and flower borders while retaining contact paths and matched rhythm beds. Prismatic Woodland now has48 trees,45 shrubs,260 ferns,136 flower clumps and6 moss-covered guardian, owl and hare statues. Stone is static; plants share wind. More geometry costs rendering work; this is not a performance optimization. Planar river reflection and camera bloom remain Built-in-only sample effects.

<!-- /pastel-examples -->









<!-- palette-guide-link -->

[Vegetation palette](../PaletteGuide.en.html)

<!-- /palette-guide-link -->
