using System;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

namespace Overdox.Editor
{
    public static partial class RestoredUiBuilder
    {
        /// <summary>
        /// 실제 재생 중 세트 제목 중심과 겹치는 활성 Graphic의 그리기 순서·원본 depth·재질을 기록한다.
        /// </summary>
        [MenuItem("OVERDOX/Restore/Inspect native equipment draw order")]
        public static void InspectEquipmentDrawOrder()
        {
            if (!EditorApplication.isPlaying)
            {
                throw new InvalidOperationException("로비를 재생하고 장비 화면을 여세요.");
            }
            var controller = UnityEngine.Object.FindFirstObjectByType<RestoredLobbyController>();
            if (controller == null || !controller.equipment.activeInHierarchy)
            {
                throw new InvalidOperationException("실제 장비 화면이 표시되어야 합니다.");
            }
            Canvas.ForceUpdateCanvases();
            var document = controller.GetComponent<RestoredUiDocument>();
            var layout = JsonUtility.FromJson<Layout>(File.ReadAllText("Assets/GameData/RestoredUI/Lobby.json"));
            var sourceIds = document.bindings.ToDictionary(binding => binding.target, binding => binding.sourceId);
            var sourceWidgets = layout.widgets.ToDictionary(widget => widget.transformId);
            var title = controller.equipmentPresentation.setTitle;
            var camera = title.canvas.worldCamera;
            var point = RectTransformUtility.WorldToScreenPoint(camera, title.rectTransform.TransformPoint(title.rectTransform.rect.center));
            var graphics = controller.GetComponentsInChildren<Graphic>().Where(graphic => graphic.enabled && graphic.color.a > 0 &&
                RectTransformUtility.RectangleContainsScreenPoint(graphic.rectTransform, point, camera)).OrderBy(graphic => graphic.depth).ToArray();
            var records = graphics.Select(graphic =>
            {
                sourceIds.TryGetValue(graphic.rectTransform, out int sourceId);
                sourceWidgets.TryGetValue(sourceId, out var widget);
                return new EquipmentDrawRecord
                {
                    sourceId = sourceId, sourceDepth = widget?.depth ?? -1, drawDepth = graphic.depth,
                    name = graphic.name, color = graphic.color,
                    text = graphic is Text text ? text.text : null,
                    sprite = graphic is Image image && image.sprite != null ? image.sprite.name : null,
                    shader = graphic.materialForRendering != null ? graphic.materialForRendering.shader.name : null
                };
            }).ToArray();
            Directory.CreateDirectory("Docs/Verification/EquipmentDisplay");
            File.WriteAllText("Docs/Verification/EquipmentDisplay/actual-draw-order.json", JsonUtility.ToJson(new EquipmentDrawResult
            {
                recordedAt = DateTime.UtcNow.ToString("O"), point = point, records = records
            }, true));
            Debug.Log($"OVERDOX_EQUIPMENT_DRAW_INSPECTED overlaps={records.Length} titleDepth={title.depth}");
        }

        /// <summary>
        /// 사각형 겹침 후보의 원본 ID와 실제 uGUI 상태를 보관한다. 알파 텍스처의 픽셀 가림은 별도 확인한다.
        /// </summary>
        [Serializable]
        private sealed class EquipmentDrawRecord
        {
            public int sourceId, sourceDepth, drawDepth;
            public string name, text, sprite, shader;
            public Color color;
        }

        /// <summary>
        /// 실제 진단 시각과 화면 좌표·후보를 JSON으로 보존한다.
        /// </summary>
        [Serializable]
        private sealed class EquipmentDrawResult
        {
            public string recordedAt;
            public Vector2 point;
            public EquipmentDrawRecord[] records;
        }
    }
}
