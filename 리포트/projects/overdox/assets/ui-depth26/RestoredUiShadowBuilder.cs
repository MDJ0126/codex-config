using System;
using System.Collections.Generic;
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
        /// <summary>
        /// 기존 씬과 프로젝트 소유 프리팹의 그림자와 직접 형제 위젯의 원작 depth 순서를 복구한다.
        /// </summary>
        [MenuItem("OVERDOX/Restore/Repair native UI depth")]
        public static void RepairSavedNativeShadows()
        {
            if (EditorApplication.isPlaying || SceneManager.GetActiveScene().isDirty)
            {
                throw new InvalidOperationException("현재 씬을 저장하고 재생을 종료하세요.");
            }
            var setup = EditorSceneManager.GetSceneManagerSetup();
            var records = new List<ShadowRepairRecord>();
            try
            {
                foreach (string sourceScene in new[] { "Lobby", "GameRoom" })
                {
                    var layout = JsonUtility.FromJson<Layout>(File.ReadAllText("Assets/GameData/RestoredUI/" + sourceScene + ".json"));
                    string prefabPath = OutputFolder + "/Original" + sourceScene + "UI.v2.prefab";
                    var prefab = PrefabUtility.LoadPrefabContents(prefabPath);
                    try
                    {
                        records.Add(RepairDocumentShadows(prefab.GetComponent<RestoredUiDocument>(), layout, prefabPath));
                        PrefabUtility.SaveAsPrefabAsset(prefab, prefabPath);
                    }
                    finally
                    {
                        PrefabUtility.UnloadPrefabContents(prefab);
                    }
                    string scenePath = sourceScene == "Lobby" ? "Assets/Scenes/OverdoxLobby.unity" : "Assets/Scenes/OverdoxArena.unity";
                    var scene = EditorSceneManager.OpenScene(scenePath, OpenSceneMode.Single);
                    var document = UnityEngine.Object.FindFirstObjectByType<RestoredUiDocument>();
                    records.Add(RepairDocumentShadows(document, layout, scenePath));
                    EditorSceneManager.SaveScene(scene);
                }
            }
            finally
            {
                EditorSceneManager.RestoreSceneManagerSetup(setup);
            }
            Directory.CreateDirectory("Docs/Verification/UiDepth");
            var result = new ShadowRepairResult { recordedAt = DateTime.UtcNow.ToString("O"), targets = records.ToArray() };
            File.WriteAllText("Docs/Verification/UiDepth/shadow-repair.json", JsonUtility.ToJson(result, true));
            Debug.Log("OVERDOX_UI_SHADOW_DEPTH_READY " + JsonUtility.ToJson(result));
        }

        /// <summary>
        /// 바인딩된 원본 Transform을 유지하면서 해당 문서의 그림자 수정을 적용한다.
        /// </summary>
        private static ShadowRepairRecord RepairDocumentShadows(RestoredUiDocument document, Layout layout, string target)
        {
            if (document == null)
            {
                throw new InvalidOperationException(target + ": 원본 UI 문서가 없습니다.");
            }
            var objects = document.bindings.Where(binding => binding.target != null).ToDictionary(binding => binding.sourceId, binding => binding.target);
            return new ShadowRepairRecord
            {
                target = target,
                parentIds = RestoreNativeShadows(layout, objects),
                siblingParentIds = RestoreSiblingDepth(layout, objects)
            };
        }

        /// <summary>
        /// 부모가 같은 원작 위젯만 depth 오름차순으로 정렬하고 비위젯 슬롯과 좌표를 유지한다.
        /// </summary>
        private static int[] RestoreSiblingDepth(Layout layout, Dictionary<int, RectTransform> objects)
        {
            var depthById = layout.widgets.ToDictionary(widget => widget.transformId, widget => widget.depth);
            var idByRect = objects.ToDictionary(pair => pair.Value, pair => pair.Key);
            var changedParents = new List<int>();
            foreach (var group in objects.Where(pair => depthById.ContainsKey(pair.Key) && pair.Value.parent != null).GroupBy(pair => pair.Value.parent))
            {
                var original = group.Key.Cast<Transform>().ToArray();
                var selected = group.Select(pair => (Transform)pair.Value).ToHashSet();
                var ordered = original.Where(selected.Contains).OrderBy(child => depthById[idByRect[(RectTransform)child]]).ThenBy(child => child.GetSiblingIndex()).ToArray();
                var desired = original.ToArray();
                int cursor = 0;
                for (int index = 0; index < desired.Length; index++)
                {
                    if (selected.Contains(desired[index]))
                    {
                        desired[index] = ordered[cursor++];
                    }
                }
                if (original.SequenceEqual(desired))
                {
                    continue;
                }
                // 원본 위젯끼리의 그리기 순서만 변경한다. 원본 ID·부모·Rect·비위젯 자식 슬롯은 유지한다.
                for (int index = 0; index < desired.Length; index++)
                {
                    desired[index].SetSiblingIndex(index);
                }
                changedParents.Add(idByRect.TryGetValue(group.Key as RectTransform, out int parentId) ? parentId : 0);
            }
            return changedParents.ToArray();
        }

        /// <summary>
        /// 크기·피벗·스프라이트·회전이 같은 낮은 depth 검정 자식만 원본 오프셋의 뒤쪽 메시로 바꾼다.
        /// </summary>
        private static int[] RestoreNativeShadows(Layout layout, Dictionary<int, RectTransform> objects)
        {
            var nodes = layout.transforms.ToDictionary(node => node.id);
            var widgets = layout.widgets.ToDictionary(widget => widget.transformId);
            var repaired = new List<int>();
            foreach (var shadow in layout.widgets)
            {
                var node = nodes[shadow.transformId];
                if (!widgets.TryGetValue(node.parentId, out var icon) || shadow.depth >= icon.depth ||
                    shadow.type != "UISprite" || icon.type != "UISprite" || shadow.sprite != icon.sprite || shadow.atlas != icon.atlas ||
                    shadow.width != icon.width || shadow.height != icon.height || shadow.pivot != icon.pivot || shadow.flip != icon.flip ||
                    shadow.color.r != 0 || shadow.color.g != 0 || shadow.color.b != 0 ||
                    node.scale != Vector3.one || Quaternion.Angle(node.rotation, Quaternion.identity) > .001f)
                {
                    continue;
                }
                if (!objects.TryGetValue(icon.transformId, out var iconRect) || !objects.TryGetValue(shadow.transformId, out var shadowRect))
                {
                    continue;
                }
                var iconImage = iconRect.GetComponent<Image>();
                var shadowImage = shadowRect.GetComponent<Image>();
                if (iconImage == null || shadowImage == null)
                {
                    throw new InvalidOperationException("원본 그림자 쌍의 Image가 없습니다: " + shadow.transformId);
                }
                // NGUI는 자식도 depth로 먼저 그릴 수 있지만 uGUI는 부모 Graphic 뒤에 자식을 그린다.
                // 동일 스프라이트 쌍은 Shadow 메시를 사용하면 계층·클리핑·동적 아이콘 참조를 유지할 수 있다.
                var effect = iconRect.GetComponent<RestoredNativeShadow>() ?? iconRect.gameObject.AddComponent<RestoredNativeShadow>();
                effect.effectColor = shadow.color;
                effect.effectDistance = new Vector2(node.position.x, node.position.y);
                effect.useGraphicAlpha = false;
                effect.enabled = node.active && shadow.enabled;
                shadowImage.enabled = false;
                EditorUtility.SetDirty(iconRect.gameObject);
                EditorUtility.SetDirty(shadowImage);
                repaired.Add(icon.transformId);
            }
            return repaired.ToArray();
        }

        /// <summary>
        /// 수정한 문서와 본 이미지 원본 ID를 검증 기록으로 보관한다.
        /// </summary>
        [Serializable]
        private sealed class ShadowRepairRecord
        {
            public string target;
            public int[] parentIds;
            public int[] siblingParentIds;
        }

        /// <summary>
        /// 저장한 대상과 UTC 실행 시각을 JSON으로 기록한다.
        /// </summary>
        [Serializable]
        private sealed class ShadowRepairResult
        {
            public string recordedAt;
            public ShadowRepairRecord[] targets;
        }
    }
}
