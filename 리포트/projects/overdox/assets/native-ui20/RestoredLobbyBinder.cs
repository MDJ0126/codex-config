using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEditor.Events;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Overdox.Editor
{
    public static partial class RestoredUiBuilder
    {
        [MenuItem("OVERDOX/Restore/Apply native lobby presentation fixes")]
        public static void FixLobbyPresentation()
        {
            if (EditorApplication.isPlaying || SceneManager.GetActiveScene().isDirty) { throw new InvalidOperationException("재생을 종료하고 현재 씬을 저장하세요."); }
            AssetDatabase.Refresh(ImportAssetOptions.ForceSynchronousImport);
            foreach (string name in new[] { "lobbybackground", "BG_LobbyGround", "BG_Lobby_Gradation" })
            {
                string originalPath = "Assets/Art/Original/Material/" + name + ".mat";
                AssetDatabase.ImportAsset(originalPath, ImportAssetOptions.ForceSynchronousImport | ImportAssetOptions.ForceUpdate);
                Debug.Log($"OVERDOX_STAGE_MATERIAL source={originalPath} guid={AssetDatabase.AssetPathToGUID(originalPath)} loaded={AssetDatabase.LoadAssetAtPath<Material>(originalPath) != null}");
            }
            var setup = EditorSceneManager.GetSceneManagerSetup();
            var scene = EditorSceneManager.OpenScene("Assets/Scenes/OverdoxLobby.unity", OpenSceneMode.Single);
            var controller = UnityEngine.Object.FindFirstObjectByType<RestoredLobbyController>();
            if (PrefabUtility.IsPartOfPrefabInstance(controller.gameObject))
            {
                // This is the editable playable scene, while the recovered
                // source prefabs remain intact. Bake static label/layer edits
                // instead of silently losing unrecorded prefab overrides.
                PrefabUtility.UnpackPrefabInstance(controller.gameObject, PrefabUnpackMode.Completely, InteractionMode.AutomatedAction);
            }
            var document = controller.GetComponent<RestoredUiDocument>();
            var session = UnityEngine.Object.FindFirstObjectByType<ArenaSession>();
            string[] familyNames = { "SwordAndShield", "HammerAndShield", "DualBlade", "Gsword", "Ghammer", "PaxeAndShield", "QuarterStaff", "LongSword" };
            for (int family = 0; family < familyNames.Length; family++)
            {
                session.restoredLibrary.animations[family].lobbyIdle = AssetDatabase.LoadAssetAtPath<AnimationClip>("Assets/Art/Restored/Animations/Equipment_Idle_" + familyNames[family] + "_01.anim");
            }
            EditorUtility.SetDirty(session.restoredLibrary);
            AssetDatabase.SaveAssets();
            session.arenaCamera.nativeLobbyPose = true;
            var canvas = controller.GetComponent<Canvas>();
            canvas.renderMode = RenderMode.ScreenSpaceCamera;
            canvas.worldCamera = session.arenaCamera.GetComponent<Camera>();
            canvas.planeDistance = .5f;
            ImportLobbyStage(scene, session);
            document.Find(55214).parent.gameObject.SetActive(true);
            var layout = JsonUtility.FromJson<Layout>(File.ReadAllText("Assets/GameData/RestoredUI/Lobby.json"));
            var atlases = layout.atlases.ToDictionary(atlas => atlas.name);
            var spriteCache = new Dictionary<string, Sprite>();
            Sprite LobbySprite(string name) => ResolveSprite(new Widget { atlas = "LobbyUI", sprite = name }, atlases, spriteCache);
            var frames = new[] { "Basic", "Normal", "Rare", "Epic", "Legend" }.Select(name => LobbySprite("CardFrame_" + name + "_02")).ToArray();
            var brands = Enumerable.Range(1, 6).Select(index => LobbySprite("armorBrand_icon_" + index)).ToArray();
            foreach (var card in controller.cards)
            {
                card.frame = card.transform.Find("equipmentFrame_2").GetComponent<Image>();
                card.brand = card.transform.Find("equipmentHead").GetComponent<Image>();
                card.rarityFrames = frames;
                card.brandIcons = brands;
                card.thumbnail.preserveAspect = true;
                // NGUI sorts widgets by depth, independently of Transform order.
                // Preserve each card's hierarchy while baking its visual layers.
                card.frame.transform.SetAsFirstSibling();
                card.thumbnail.transform.SetSiblingIndex(1);
                card.transform.Find("equipmentStatsGroup").SetAsLastSibling();
                card.title.transform.SetAsLastSibling();
                card.brand.transform.SetAsLastSibling();
                card.transform.Find("materials").SetAsLastSibling();
                card.statName = card.transform.Find("equipmentStatsGroup/equipmentStatsLabel").GetComponent<Text>();
                card.legendaryEffect = card.transform.Find("equipmentFrame_2/equipmentFrame_3(Legendary)").gameObject;
                card.stat.rectTransform.sizeDelta = new Vector2(110, card.stat.rectTransform.sizeDelta.y);
                card.stat.horizontalOverflow = HorizontalWrapMode.Overflow;
                card.title.resizeTextForBestFit = true;
                card.title.resizeTextMinSize = 12;
                card.title.resizeTextMaxSize = card.title.fontSize;
                EditorUtility.SetDirty(card);
            }
            controller.windowTitle.horizontalOverflow = HorizontalWrapMode.Overflow;
            controller.windowTitle.rectTransform.sizeDelta = new Vector2(250, 60);
            controller.windowTitle.alignment = TextAnchor.MiddleLeft;
            var actions = new UnityAction[] { controller.Head, controller.Body, controller.Cape, controller.Weapons };
            var slots = new[] { 32873, 39623, 35176, 34357 }.Select(id => document.Find(id)).OrderByDescending(rect => rect.localPosition.y).ToArray();
            for (int slot = 0; slot < slots.Length; slot++)
            {
                Wire(slots[slot], actions[slot]);
                foreach (var label in slots[slot].GetComponentsInChildren<Text>(true))
                {
                    label.text = label.text.Replace("999", "1");
                }
            }
            EditorSceneManager.SaveScene(scene);
            EditorSceneManager.RestoreSceneManagerSetup(setup);
            Debug.Log("OVERDOX_NATIVE_LOBBY_PRESENTATION_FIXED");
        }

        private static void ImportLobbyStage(Scene lobby, ArenaSession session)
        {
            var existing = lobby.GetRootGameObjects().FirstOrDefault(root => root.name == "Native Lobby Stage");
            if (existing != null)
            {
                var renderers = existing.GetComponentsInChildren<Renderer>(true);
                if (renderers.Length > 0 && renderers.All(renderer => renderer.sharedMaterials.All(material => material != null))) { return; }
                UnityEngine.Object.DestroyImmediate(existing);
                var previousBackground = session.arenaCamera.transform.Find("Native Lobby Background Camera");
                if (previousBackground != null) { UnityEngine.Object.DestroyImmediate(previousBackground.gameObject); }
            }
            var source = EditorSceneManager.OpenScene("Assets/Art/Restored/Scenes/OriginalLobbyStage.unity", OpenSceneMode.Additive);
            var stage = source.GetRootGameObjects().Single();
            stage.name = "Native Lobby Stage";
            // Native Hero_Position is (-1051,60,3984), scale 1100. The
            // reconstructed actor uses 1.1, so the scene is normalized by 1000.
            var container = new GameObject("Native Lobby Stage");
            SceneManager.MoveGameObjectToScene(container, lobby);
            container.transform.position = new Vector3(1.051f, .04f, -3.984f);
            container.transform.localScale = Vector3.one * .001f;
            SceneManager.MoveGameObjectToScene(stage, lobby);
            stage.transform.SetParent(container.transform, false);
            Directory.CreateDirectory(OutputFolder + "/StageMaterials");
            int missingMaterials = 0;
            foreach (var renderer in stage.GetComponentsInChildren<Renderer>(true))
            {
                renderer.gameObject.layer = 20;
                renderer.sharedMaterials = renderer.sharedMaterials.Select(original =>
                {
                    if (original == null) { missingMaterials++; return null; }
                    string path = OutputFolder + "/StageMaterials/" + original.name + ".mat";
                    var material = AssetDatabase.LoadAssetAtPath<Material>(path);
                    if (material != null) { return material; }
                    bool transparent = original.renderQueue >= 3000 || original.name.IndexOf("Fx", StringComparison.OrdinalIgnoreCase) >= 0;
                    bool gradient = original.name == "BG_Lobby_Gradation";
                    material = new Material(Shader.Find(gradient ? "OVERDOX/Restored Lobby Gradient" : transparent ? "Unlit/Transparent" : "Unlit/Texture"));
                    material.name = original.name;
                    if (gradient)
                    {
                        foreach (string property in new[] { "_Top_Color", "_Middle_Color", "_Bottom_Color" }) { material.SetColor(property, original.GetColor(property)); }
                        material.SetFloat("_Middle_Position", original.GetFloat("_Middle_Position"));
                    }
                    else
                    {
                        string textureProperty = original.HasProperty("_MainTex") ? "_MainTex" : original.HasProperty("_Main_Tex") ? "_Main_Tex" : null;
                        if (textureProperty != null)
                        {
                            material.mainTexture = original.GetTexture(textureProperty);
                            material.mainTextureScale = original.GetTextureScale(textureProperty);
                            material.mainTextureOffset = original.GetTextureOffset(textureProperty);
                        }
                    }
                    AssetDatabase.CreateAsset(material, path);
                    return material;
                }).ToArray();
            }
            if (missingMaterials != 0) { throw new InvalidDataException($"로비 무대 원본 재질 누락 {missingMaterials}개"); }
            EditorSceneManager.CloseScene(source, true);
            var actorCamera = session.arenaCamera.GetComponent<Camera>();
            actorCamera.clearFlags = CameraClearFlags.Depth;
            actorCamera.cullingMask &= ~(1 << 20);
            var background = new GameObject("Native Lobby Background Camera").AddComponent<Camera>();
            background.transform.SetParent(actorCamera.transform, false);
            background.clearFlags = CameraClearFlags.SolidColor;
            background.backgroundColor = new Color(0, 15f / 255, 28f / 255);
            background.fieldOfView = 35;
            background.cullingMask = 1 << 20;
            background.depth = actorCamera.depth - 1;
            background.nearClipPlane = .01f;
            AssetDatabase.SaveAssets();
        }

        [MenuItem("OVERDOX/Restore/Bind native lobby to game flow")]
        public static void BindLobby()
        {
            if (EditorApplication.isPlaying || SceneManager.GetActiveScene().isDirty) { throw new InvalidOperationException("현재 씬을 저장하고 재생을 종료하세요."); }
            const string path = "Assets/Scenes/OverdoxLobby.unity";
            var setup = EditorSceneManager.GetSceneManagerSetup();
            var scene = EditorSceneManager.OpenScene(path, OpenSceneMode.Single);
            var session = UnityEngine.Object.FindFirstObjectByType<ArenaSession>();
            if (UnityEngine.Object.FindFirstObjectByType<RestoredLobbyController>() != null) { throw new InvalidOperationException("기존 연결을 보존합니다. 중복 연결하지 않습니다."); }
            var prefab = AssetDatabase.LoadAssetAtPath<GameObject>(OutputFolder + "/OriginalLobbyUI.v2.prefab");
            var root = (GameObject)PrefabUtility.InstantiatePrefab(prefab);
            var document = root.GetComponent<RestoredUiDocument>();
            var controller = root.AddComponent<RestoredLobbyController>();
            controller.home = document.Find(30217).parent.gameObject;
            controller.equipment = document.Find(33679).parent.gameObject;
            controller.back = document.Find(30571).gameObject;
            controller.profile = document.Find(30571).parent.Find("myUserInfo").gameObject;
            controller.windowTitle = document.Find(30162).GetComponent<Text>();
            controller.nickname = controller.profile.GetComponentsInChildren<Text>(true).First(text => text.text == "Nickname");
            controller.health = document.Find(38567).GetComponent<Text>();
            controller.damage = document.Find(37562).GetComponent<Text>();
            controller.weight = document.Find(37636).GetComponent<Text>();
            Wire(document.Find(30832), controller.OpenEquipment);
            Wire(document.Find(30640), controller.Battle);
            Wire(document.Find(30571), controller.OpenHome);
            Wire(document.Find(31203), controller.OpenHome);
            Wire(document.Find(55332), controller.Gender);
            Wire(document.Find(34185), controller.Previous);
            Wire(document.Find(36471), controller.Next);
            var slots = new[] { 32873, 39623, 35176, 34357 }.Select(id => document.Find(id)).OrderByDescending(rect => rect.localPosition.y).ToArray();
            var slotActions = new UnityAction[] { controller.Head, controller.Body, controller.Cape, controller.Weapons };
            controller.equippedIcons = new Image[4];
            for (int slot = 0; slot < slots.Length; slot++)
            {
                Wire(slots[slot], slotActions[slot]);
                controller.equippedIcons[slot] = slots[slot].GetComponentsInChildren<Image>(true).First(image => image.name.Contains("Thumbnail", StringComparison.OrdinalIgnoreCase));
            }
            var list = document.Find(55214).parent;
            list.gameObject.SetActive(true);
            foreach (Transform sibling in list.parent) { if (sibling != list) { sibling.gameObject.SetActive(false); } }
            var cards = new List<RestoredGearCard>();
            foreach (Transform element in list.Cast<Transform>().OrderBy(element => element.localPosition.x))
            {
                var cardRoot = element.Find("EquipmentCard");
                var card = cardRoot.gameObject.AddComponent<RestoredGearCard>();
                card.title = cardRoot.Find("equipmentHead_name").GetComponent<Text>();
                card.level = cardRoot.Find("materials/equipmentLevelMark_10/equipmentLevelLabel").GetComponent<Text>();
                card.stat = cardRoot.Find("equipmentStatsGroup/equipmentStates").GetComponent<Text>();
                card.thumbnail = cardRoot.Find("equipmentThumbnail_5").GetComponent<Image>();
                card.selectedMark = cardRoot.Find("markGroup/MarkGrid/EquipMark").gameObject;
                foreach (var named in new[] { "markGroup/NewMark", "markGroup/Awakening1Mark", "markGroup/Awakening2Mark", "markGroup/MarkGrid/UpgradeMark", "materials/materialCountGroup", "materials/levelMax", "specialSkill" })
                {
                    var child = cardRoot.Find(named);
                    if (child != null) { child.gameObject.SetActive(false); }
                }
                Wire(element, card.Select);
                cards.Add(card);
            }
            controller.cards = cards.ToArray();
            var layout = JsonUtility.FromJson<Layout>(File.ReadAllText("Assets/GameData/RestoredUI/Lobby.json"));
            var atlases = layout.atlases.ToDictionary(atlas => atlas.name);
            var cache = new Dictionary<string, Sprite>();
            Sprite Icon(string name)
            {
                var sprite = ResolveSprite(new Widget { atlas = "EquipmentUI", sprite = name }, atlases, cache);
                if (sprite != null) { return sprite; }
                string output = OutputFolder + "/Sprites/Standalone_" + name + ".asset";
                sprite = AssetDatabase.LoadAssetAtPath<Sprite>(output);
                if (sprite != null) { return sprite; }
                var texture = AssetDatabase.LoadAssetAtPath<Texture2D>("Assets/Art/Original/Resources/texture/equipmentui/" + name + ".png");
                if (texture == null) { return null; }
                sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(.5f, .5f), 100);
                sprite.name = name;
                AssetDatabase.CreateAsset(sprite, output);
                return sprite;
            }
            controller.icons = session.catalog.Weapons.Concat(session.catalog.Armor).Select(row => new RestoredGearIcon
            {
                gearId = row.id,
                male = Icon(row.resourceName + (session.catalog.Armor.Any(armor => armor.id == row.id) ? "_" + new[] { "Head", "Body", "Cape" }[row.slot] + "_M" : "")),
                female = Icon(row.resourceName + (session.catalog.Armor.Any(armor => armor.id == row.id) ? "_" + new[] { "Head", "Body", "Cape" }[row.slot] + "_F" : ""))
            }).ToArray();
            int missing = controller.icons.Count(icon => icon.male == null || icon.female == null);
            if (missing != 0) { throw new InvalidDataException($"장비 아이콘 누락 {missing}개. 씬은 저장하지 않았습니다."); }
            controller.OpenHome();
            session.hud.gameObject.SetActive(false);
            EditorSceneManager.SaveScene(scene);
            AssetDatabase.SaveAssets();
            EditorSceneManager.RestoreSceneManagerSetup(setup);
            Debug.Log($"OVERDOX_NATIVE_LOBBY_BOUND cards={cards.Count} gearIcons={controller.icons.Length}");
        }

        private static void Wire(Transform target, UnityAction action)
        {
            var rect = (RectTransform)target;
            var graphic = target.GetComponent<Graphic>();
            if (graphic == null)
            {
                var hit = target.gameObject.AddComponent<Image>();
                hit.color = Color.clear;
                graphic = hit;
            }
            if (graphic is Image image && image.color.a == 0)
            {
                var child = target.GetComponentsInChildren<Graphic>(true).Where(item => item != graphic)
                    .OrderByDescending(item => item.rectTransform.rect.width * item.rectTransform.rect.height).FirstOrDefault();
                if (child != null) { rect.sizeDelta = child.rectTransform.rect.size; }
            }
            graphic.raycastTarget = true;
            var button = target.GetComponent<Button>() ?? target.gameObject.AddComponent<Button>();
            button.transition = Selectable.Transition.None;
            button.targetGraphic = graphic;
            while (button.onClick.GetPersistentEventCount() > 0) { UnityEventTools.RemovePersistentListener(button.onClick, 0); }
            UnityEventTools.AddPersistentListener(button.onClick, action);
        }
    }
}
