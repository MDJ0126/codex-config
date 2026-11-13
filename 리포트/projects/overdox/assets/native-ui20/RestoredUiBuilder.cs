using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Overdox.Editor
{
    /// <summary>회수한 NGUI 배치와 아틀라스를 편집 가능한 uGUI 프리팹으로 저장한다.</summary>
    public static partial class RestoredUiBuilder
    {
        [Serializable] private sealed class Layout
        {
            public string scene;
            public Node[] transforms;
            public Widget[] widgets;
            public Atlas[] atlases;
            public Panel[] panels;
            public string[] missing;
        }
        [Serializable] private sealed class Panel
        {
            public int transformId;
            public Vector4 clip;
            public Vector2 softness;
        }
        [Serializable] private sealed class Node
        {
            public int id, parentId, order;
            public string name;
            public bool active;
            public Vector3 position, scale;
            public Quaternion rotation;
        }
        [Serializable] private sealed class Widget
        {
            public int id, transformId, width, height, pivot, depth, imageType, fillDirection, flip;
            public int fontSize, fontStyle, alignment, effectStyle;
            public string type, name, atlas, sprite, text, fontPath;
            public bool enabled, invert;
            public Color color, effectColor;
            public Vector2 effectDistance;
            public float fillAmount;
        }
        [Serializable] private sealed class Atlas
        {
            public string name, texturePath;
            public SpriteRect[] sprites;
        }
        [Serializable] private sealed class SpriteRect
        {
            public string name;
            public int x, y, width, height, borderLeft, borderRight, borderTop, borderBottom;
        }
        private const string OutputFolder = "Assets/Art/Restored/UI";
        private static readonly Vector2[] Pivots =
        {
            new(0, 1), new(.5f, 1), new(1, 1), new(0, .5f), new(.5f, .5f),
            new(1, .5f), new(0, 0), new(.5f, 0), new(1, 0)
        };

        [MenuItem("OVERDOX/Restore/Preview native equipment UI")]
        public static void PreviewEquipment()
        {
            const string path = OutputFolder + "/PreviewEquipment.v2.unity";
            if (File.Exists(path))
            {
                if (SceneManager.GetActiveScene().isDirty) { throw new InvalidOperationException("현재 씬을 먼저 저장하세요."); }
                EditorSceneManager.OpenScene(path, OpenSceneMode.Single);
                return;
            }
            PreviewLobby();
            var document = UnityEngine.Object.FindFirstObjectByType<RestoredUiDocument>();
            document.Find(30217).parent.gameObject.SetActive(false);
            document.Find(33679).parent.gameObject.SetActive(true);
            EditorSceneManager.SaveScene(SceneManager.GetActiveScene(), path);
        }

        [MenuItem("OVERDOX/Restore/Capture native UI preview")]
        public static void CapturePreview()
        {
            var camera = Camera.main;
            if (camera == null) { throw new InvalidOperationException("미리보기 카메라가 없습니다."); }
            Directory.CreateDirectory("Docs/Verification/NativeUI");
            var target = RenderTexture.GetTemporary(1600, 900, 24);
            var previous = camera.targetTexture;
            var active = RenderTexture.active;
            try
            {
                camera.targetTexture = target;
                Canvas.ForceUpdateCanvases();
                camera.Render();
                RenderTexture.active = target;
                var pixels = new Texture2D(1600, 900, TextureFormat.RGB24, false);
                pixels.ReadPixels(new Rect(0, 0, 1600, 900), 0, 0);
                pixels.Apply();
                string output = "Docs/Verification/NativeUI/" + SceneManager.GetActiveScene().name + "-" + DateTime.UtcNow.ToString("yyyyMMdd-HHmmss") + ".png";
                File.WriteAllBytes(output, pixels.EncodeToPNG());
                UnityEngine.Object.DestroyImmediate(pixels);
                Debug.Log("OVERDOX_NATIVE_UI_CAPTURE " + output);
            }
            finally
            {
                camera.targetTexture = previous;
                RenderTexture.active = active;
                RenderTexture.ReleaseTemporary(target);
            }
        }

        [MenuItem("OVERDOX/Restore/Preview native lobby UI")]
        public static void PreviewLobby()
        {
            if (EditorApplication.isPlaying || SceneManager.GetActiveScene().isDirty)
            {
                throw new InvalidOperationException("재생을 종료하고 현재 씬을 저장한 뒤 미리보기를 여세요.");
            }
            const string scenePath = OutputFolder + "/PreviewLobby.v2.unity";
            if (File.Exists(scenePath))
            {
                EditorSceneManager.OpenScene(scenePath, OpenSceneMode.Single);
                return;
            }
            var scene = EditorSceneManager.NewScene(NewSceneSetup.EmptyScene, NewSceneMode.Single);
            var prefab = AssetDatabase.LoadAssetAtPath<GameObject>(OutputFolder + "/OriginalLobbyUI.v2.prefab");
            var root = (GameObject)PrefabUtility.InstantiatePrefab(prefab);
            var camera = new GameObject("Preview Camera").AddComponent<Camera>();
            camera.tag = "MainCamera";
            camera.clearFlags = CameraClearFlags.SolidColor;
            camera.backgroundColor = new Color(.035f, .05f, .075f);
            camera.orthographic = true;
            camera.orthographicSize = 5;
            var canvas = root.GetComponent<Canvas>();
            canvas.renderMode = RenderMode.ScreenSpaceCamera;
            canvas.worldCamera = camera;
            canvas.planeDistance = 1;
            new GameObject("EventSystem", typeof(EventSystem), typeof(StandaloneInputModule));
            EditorSceneManager.SaveScene(scene, scenePath);
            Debug.Log("OVERDOX_NATIVE_LOBBY_PREVIEW_READY");
        }

        [MenuItem("OVERDOX/Restore/Create missing native UI prefabs")]
        public static void CreateMissingPrefabs()
        {
            Directory.CreateDirectory(OutputFolder);
            Directory.CreateDirectory(OutputFolder + "/Sprites");
            AssetDatabase.Refresh();
            foreach (string scene in new[] { "Lobby", "GameRoom" })
            {
                string output = OutputFolder + "/Original" + scene + "UI.v2.prefab";
                if (File.Exists(output)) { continue; }
                var layout = JsonUtility.FromJson<Layout>(File.ReadAllText("Assets/GameData/RestoredUI/" + scene + ".json"));
                if (layout.missing.Length != 0) { throw new InvalidDataException(string.Join("\n", layout.missing)); }
                var root = Convert(layout);
                PrefabUtility.SaveAsPrefabAsset(root, output);
                UnityEngine.Object.DestroyImmediate(root);
                Debug.Log($"OVERDOX_NATIVE_UI_PREFAB scene={scene} widgets={layout.widgets.Length} transforms={layout.transforms.Length}");
            }
            AssetDatabase.SaveAssets();
        }

        private static GameObject Convert(Layout layout)
        {
            var root = new GameObject("Original " + layout.scene + " UI", typeof(RectTransform), typeof(Canvas), typeof(CanvasScaler), typeof(GraphicRaycaster));
            var canvas = root.GetComponent<Canvas>();
            canvas.renderMode = RenderMode.ScreenSpaceOverlay;
            var scaler = root.GetComponent<CanvasScaler>();
            scaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
            scaler.referenceResolution = new Vector2(1280, 720);
            scaler.matchWidthOrHeight = .5f;
            var document = root.AddComponent<RestoredUiDocument>();
            var nodes = layout.transforms.ToDictionary(node => node.id);
            var objects = new Dictionary<int, RectTransform>();
            foreach (var node in layout.transforms)
            {
                objects.Add(node.id, node.parentId == 0 ? root.GetComponent<RectTransform>() : new GameObject(node.name, typeof(RectTransform)).GetComponent<RectTransform>());
            }
            foreach (var node in layout.transforms.OrderBy(node => node.order))
            {
                if (node.parentId == 0) { continue; }
                var rect = objects[node.id];
                rect.SetParent(objects[node.parentId], false);
                rect.localPosition = new Vector3(node.position.x, node.position.y, 0);
                rect.localRotation = node.rotation;
                rect.localScale = node.scale;
                rect.gameObject.SetActive(node.active);
            }
            var atlases = layout.atlases.ToDictionary(atlas => atlas.name);
            var sprites = new Dictionary<string, Sprite>();
            foreach (var widget in layout.widgets)
            {
                var rect = objects[widget.transformId];
                rect.sizeDelta = new Vector2(widget.width, widget.height);
                rect.pivot = Pivots[Mathf.Clamp(widget.pivot, 0, 8)];
                if (widget.type == "UISprite")
                {
                    var image = rect.gameObject.AddComponent<Image>();
                    image.raycastTarget = false;
                    image.color = widget.color;
                    image.enabled = widget.enabled;
                    image.sprite = ResolveSprite(widget, atlases, sprites);
                    image.type = widget.imageType == 4 ? Image.Type.Simple : (Image.Type)Mathf.Clamp(widget.imageType, 0, 3);
                    image.fillMethod = (Image.FillMethod)Mathf.Clamp(widget.fillDirection, 0, 4);
                    image.fillAmount = widget.fillAmount;
                    image.fillClockwise = !widget.invert;
                    image.fillOrigin = widget.invert && widget.fillDirection < 2 ? 1 : 0;
                    if (image.sprite == null) { image.enabled = false; }
                }
                else
                {
                    var text = rect.gameObject.AddComponent<Text>();
                    text.raycastTarget = false;
                    text.font = AssetDatabase.LoadAssetAtPath<Font>(widget.fontPath);
                    text.fontSize = widget.fontSize;
                    text.fontStyle = (FontStyle)Mathf.Clamp(widget.fontStyle, 0, 3);
                    text.color = widget.color;
                    text.enabled = widget.enabled;
                    text.supportRichText = true;
                    text.horizontalOverflow = HorizontalWrapMode.Wrap;
                    text.verticalOverflow = VerticalWrapMode.Overflow;
                    int horizontal = widget.alignment == 0 ? widget.pivot % 3 : Mathf.Clamp(widget.alignment - 1, 0, 2);
                    text.alignment = (TextAnchor)(widget.pivot / 3 * 3 + horizontal);
                    text.text = Regex.Replace(widget.text ?? "", @"\[([0-9A-Fa-f]{6}(?:[0-9A-Fa-f]{2})?)\]", "<color=#$1>").Replace("[-]", "</color>");
                    if (widget.effectStyle > 0)
                    {
                        Shadow shadow = widget.effectStyle == 1 ? text.gameObject.AddComponent<Shadow>() : text.gameObject.AddComponent<Outline>();
                        shadow.effectColor = widget.effectColor;
                        shadow.effectDistance = widget.effectDistance;
                    }
                }
            }
            foreach (var panel in layout.panels ?? Array.Empty<Panel>())
            {
                if (!objects.TryGetValue(panel.transformId, out var rect)) { continue; }
                // NGUI clips in panel-local coordinates. A separate saved viewport
                // preserves the original child positions when the clip has an offset.
                var viewport = new GameObject("Native clip viewport", typeof(RectTransform), typeof(RectMask2D)).GetComponent<RectTransform>();
                viewport.SetParent(rect, false);
                viewport.anchoredPosition = new Vector2(panel.clip.x, panel.clip.y);
                viewport.sizeDelta = new Vector2(panel.clip.z, panel.clip.w);
                viewport.GetComponent<RectMask2D>().softness = new Vector2Int(Mathf.RoundToInt(panel.softness.x), Mathf.RoundToInt(panel.softness.y));
                foreach (var child in rect.Cast<Transform>().Where(child => child != viewport).ToArray())
                {
                    child.SetParent(viewport, false);
                    child.localPosition -= new Vector3(panel.clip.x, panel.clip.y, 0);
                }
            }
            document.bindings = objects.Select(pair => new RestoredUiBinding { sourceId = pair.Key, target = pair.Value }).ToArray();
            return root;
        }

        private static Sprite ResolveSprite(Widget widget, Dictionary<string, Atlas> atlases, Dictionary<string, Sprite> cache)
        {
            string key = widget.atlas + ":" + widget.sprite;
            if (cache.TryGetValue(key, out var cached)) { return cached; }
            if (!atlases.TryGetValue(widget.atlas ?? "", out var atlas)) { return null; }
            var source = atlas.sprites.FirstOrDefault(sprite => sprite.name == widget.sprite);
            if (source == null) { return null; }
            string name = Regex.Replace(widget.atlas + "_" + widget.sprite, @"[^\w.-]", "_");
            string path = OutputFolder + "/Sprites/" + name + ".asset";
            var result = AssetDatabase.LoadAssetAtPath<Sprite>(path);
            if (result == null)
            {
                var texture = AssetDatabase.LoadAssetAtPath<Texture2D>(atlas.texturePath);
                var bounds = new Rect(source.x, texture.height - source.y - source.height, source.width, source.height);
                var border = new Vector4(source.borderLeft, source.borderBottom, source.borderRight, source.borderTop);
                result = Sprite.Create(texture, bounds, new Vector2(.5f, .5f), 100, 0, SpriteMeshType.FullRect, border);
                result.name = widget.sprite;
                AssetDatabase.CreateAsset(result, path);
            }
            cache.Add(key, result);
            return result;
        }
    }
}
