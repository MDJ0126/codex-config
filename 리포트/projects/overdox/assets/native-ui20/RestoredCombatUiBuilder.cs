using System;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Overdox.Editor
{
    public static partial class RestoredUiBuilder
    {
        [MenuItem("OVERDOX/Restore/Bind native combat HUD")]
        public static void BindCombat()
        {
            if (EditorApplication.isPlaying || SceneManager.GetActiveScene().isDirty)
            {
                throw new InvalidOperationException("현재 씬을 저장하고 재생을 종료하세요.");
            }
            if (SceneManager.GetActiveScene().name == "PreviewCombat") { CapturePreview(); }
            var setup = EditorSceneManager.GetSceneManagerSetup();
            var scene = EditorSceneManager.OpenScene("Assets/Scenes/OverdoxArena.unity", OpenSceneMode.Single);
            if (UnityEngine.Object.FindFirstObjectByType<RestoredCombatHud>() != null)
            {
                EditorSceneManager.RestoreSceneManagerSetup(setup);
                throw new InvalidOperationException("기존 전투 HUD 연결을 보존합니다.");
            }
            var session = UnityEngine.Object.FindFirstObjectByType<ArenaSession>();
            var root = (GameObject)PrefabUtility.InstantiatePrefab(AssetDatabase.LoadAssetAtPath<GameObject>(OutputFolder + "/OriginalGameRoomUI.v2.prefab"));
            PrefabUtility.UnpackPrefabInstance(root, PrefabUnpackMode.Completely, InteractionMode.AutomatedAction);
            var document = root.GetComponent<RestoredUiDocument>();
            var common = document.Find(6165);
            var battle = document.Find(7727);
            KeepChildren(common, "Anchor - TopLeft", "Anchor - Top", "DPad");
            KeepChildren(document.Find(6087), "HeroInfo");
            KeepChildren(document.Find(5432), "BattleTimer");
            KeepChildren(battle, "Anchor - TopRight");
            KeepChildren(document.Find(7885), "BottomToolBar");
            KeepChildren(document.Find(7298), "KillCounterUI");
            KeepChildren(document.Find(5972), "Root");
            var hero = document.Find(5146);
            KeepChildren(hero, "UserIconInfo", "HitPointTable", "100. Label - Stamina");
            document.Find(5563).gameObject.SetActive(true);
            foreach (var id in new[] { 5743, 5398, 6227, 6755 })
            {
                var button = document.Find(id);
                var action = button.gameObject.AddComponent<CombatButton>();
                action.action = id == 5743 ? CombatButtonAction.Attack : id == 5398 ? CombatButtonAction.Guard : id == 6227 ? CombatButtonAction.Dash : CombatButtonAction.Skill;
                var image = button.gameObject.AddComponent<Image>();
                image.color = Color.clear;
                image.raycastTarget = true;
                button.sizeDelta = new Vector2(140, 140);
                foreach (Transform child in button)
                {
                    if (child.name.Contains("Guide") || child.name == "StopButton") { child.gameObject.SetActive(false); }
                }
            }
            KeepChildren(document.Find(5743).parent, "AttackButton", "GuardButton", "DashButton", "SpecialAttackButton");
            // Preserve all fifty source health segments; update only their fill.
            var hud = root.AddComponent<RestoredCombatHud>();
            hud.health = document.Find(6363).GetComponent<Text>();
            hud.stamina = document.Find(5563).GetComponent<Text>();
            hud.timer = document.Find(6565).GetComponent<Text>();
            hud.alive = document.Find(7016).GetComponent<Text>();
            hud.total = document.Find(7651).GetComponent<Text>();
            hud.kills = document.Find(6829).GetComponent<Text>();
            hud.healthSegments = document.Find(6749).Cast<Transform>().Where(child => child.name.StartsWith("HitPointUnit"))
                .OrderBy(child => child.localPosition.x).Select(child => child.Find("2. Sprite - Foreground").GetComponent<Image>()).ToArray();
            foreach (var image in hud.healthSegments) { image.type = Image.Type.Filled; image.fillMethod = Image.FillMethod.Horizontal; }
            ConfigureCombatPresentation(hud, document);
            var canvas = root.GetComponent<Canvas>();
            canvas.renderMode = RenderMode.ScreenSpaceCamera;
            canvas.worldCamera = session.arenaCamera.GetComponent<Camera>();
            canvas.planeDistance = .5f;
            session.hud.nativeCombatPresentation = true;
            EditorSceneManager.SaveScene(scene);
            EditorSceneManager.RestoreSceneManagerSetup(setup);
            Debug.Log("OVERDOX_NATIVE_COMBAT_HUD_BOUND");
        }

        private static void KeepChildren(Transform parent, params string[] names)
        {
            foreach (Transform child in parent) { child.gameObject.SetActive(names.Contains(child.name)); }
        }

        [MenuItem("OVERDOX/Restore/Apply native combat presentation fixes")]
        public static void FixCombatPresentation()
        {
            if (EditorApplication.isPlaying || SceneManager.GetActiveScene().isDirty) { throw new InvalidOperationException("현재 씬을 저장하고 재생을 종료하세요."); }
            var setup = EditorSceneManager.GetSceneManagerSetup();
            var scene = EditorSceneManager.OpenScene("Assets/Scenes/OverdoxArena.unity", OpenSceneMode.Single);
            var hud = UnityEngine.Object.FindFirstObjectByType<RestoredCombatHud>();
            var document = hud.GetComponent<RestoredUiDocument>();
            ConfigureCombatPresentation(hud, document);
            EditorSceneManager.SaveScene(scene);
            EditorSceneManager.RestoreSceneManagerSetup(setup);
            Debug.Log("OVERDOX_NATIVE_COMBAT_PRESENTATION_FIXED hpPerUnit=30");
        }

        private static void ConfigureCombatPresentation(RestoredCombatHud hud, RestoredUiDocument document)
        {
            foreach (int id in new[] { 6483, 6603, 6675 }) { document.Find(id).gameObject.SetActive(false); }
            foreach (var image in hud.healthSegments)
            {
                image.transform.parent.Find("1. Sprite - EffectForeground").gameObject.SetActive(false);
            }
            hud.health.horizontalOverflow = HorizontalWrapMode.Overflow;
            hud.health.rectTransform.sizeDelta = new Vector2(180, hud.health.rectTransform.sizeDelta.y);
            hud.skillTime = document.Find(5968).GetComponent<Text>();
            hud.dashTime = document.Find(6048).GetComponent<Text>();
            hud.skillCharge = document.Find(6700).GetComponent<Image>();
            hud.dashCooldown = document.Find(5465).GetComponent<Image>();
            hud.skillCountRings = new[] { 5193, 5067, 5261, 6743 }.Select(id => document.Find(id).gameObject).ToArray();
            var stickRect = document.Find(5556);
            var stick = stickRect.GetComponent<RestoredMoveStick>() ?? stickRect.gameObject.AddComponent<RestoredMoveStick>();
            stick.knob = document.Find(6537);
            stick.arrow = document.Find(5852);
            stick.arrow.gameObject.SetActive(false);
            stickRect.sizeDelta = new Vector2(220, 220);
            var hit = stickRect.GetComponent<Image>() ?? stickRect.gameObject.AddComponent<Image>();
            hit.color = Color.clear;
            hit.raycastTarget = true;
        }

        [MenuItem("OVERDOX/Restore/Preview native combat UI")]
        public static void PreviewCombat()
        {
            if (EditorApplication.isPlaying || SceneManager.GetActiveScene().isDirty)
            {
                throw new InvalidOperationException("현재 씬을 저장하고 재생을 종료하세요.");
            }
            const string path = OutputFolder + "/PreviewCombat.unity";
            if (File.Exists(path))
            {
                EditorSceneManager.OpenScene(path, OpenSceneMode.Single);
                return;
            }
            var scene = EditorSceneManager.NewScene(NewSceneSetup.EmptyScene, NewSceneMode.Single);
            var prefab = AssetDatabase.LoadAssetAtPath<GameObject>(OutputFolder + "/OriginalGameRoomUI.v2.prefab");
            var root = (GameObject)PrefabUtility.InstantiatePrefab(prefab);
            var camera = new GameObject("Preview Camera").AddComponent<Camera>();
            camera.tag = "MainCamera";
            camera.clearFlags = CameraClearFlags.SolidColor;
            camera.backgroundColor = new Color(.035f, .05f, .075f);
            camera.orthographic = true;
            var canvas = root.GetComponent<Canvas>();
            canvas.renderMode = RenderMode.ScreenSpaceCamera;
            canvas.worldCamera = camera;
            canvas.planeDistance = 1;
            EditorSceneManager.SaveScene(scene, path);
            Debug.Log("OVERDOX_NATIVE_COMBAT_PREVIEW_READY");
        }
    }
}
