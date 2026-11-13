using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Overdox.Editor
{
    public static partial class RestoredUiBuilder
    {
        /// <summary>
        /// 실제 장비 창의 이전·다음 버튼 중심을 가리는 레이캐스트와 클릭 수신자를 읽기 전용으로 기록한다.
        /// </summary>
        [MenuItem("OVERDOX/Restore/Inspect native equipment input")]
        public static void InspectEquipmentInput()
        {
            var controller = UnityEngine.Object.FindFirstObjectByType<RestoredLobbyController>();
            if (!EditorApplication.isPlaying || controller == null || !controller.equipment.activeInHierarchy || EventSystem.current == null)
            {
                throw new InvalidOperationException("로비를 재생하고 실제 장비 창을 여세요.");
            }
            Canvas.ForceUpdateCanvases();
            var document = controller.GetComponent<RestoredUiDocument>();
            var records = new List<EquipmentInputRecord>();
            foreach (int sourceId in new[] { 34185, 36471 })
            {
                var rect = document.Find(sourceId);
                var button = rect.GetComponent<Button>();
                var canvas = rect.GetComponentInParent<Canvas>();
                var point = RectTransformUtility.WorldToScreenPoint(canvas.worldCamera, rect.TransformPoint(rect.rect.center));
                var hits = new List<RaycastResult>();
                EventSystem.current.RaycastAll(new PointerEventData(EventSystem.current) { position = point }, hits);
                records.Add(new EquipmentInputRecord
                {
                    sourceId = sourceId, point = point, size = rect.rect.size,
                    isInteractable = button != null && button.IsInteractable(),
                    clickTargets = hits.Select(hit =>
                    {
                        var receiver = ExecuteEvents.GetEventHandler<IPointerClickHandler>(hit.gameObject);
                        return PathFor(hit.gameObject.transform) + " -> " + (receiver != null ? PathFor(receiver.transform) : "none");
                    }).ToArray()
                });
            }
            Directory.CreateDirectory("Docs/Verification/FlowInput");
            File.WriteAllText("Docs/Verification/FlowInput/equipment-input.json", JsonUtility.ToJson(new EquipmentInputResult
            {
                recordedAt = DateTime.UtcNow.ToString("O"), width = Screen.width, height = Screen.height, records = records.ToArray()
            }, true));
            Debug.Log("OVERDOX_EQUIPMENT_INPUT_INSPECTED sourceIds=34185,36471");
        }

        /// <summary>
        /// 이름이 같은 원본 UI 요소를 구분하도록 전체 계층 경로를 반환한다.
        /// </summary>
        private static string PathFor(Transform target)
        {
            return target.parent != null ? PathFor(target.parent) + "/" + target.name : target.name;
        }

        /// <summary>
        /// 버튼 중심의 실제 입력 후보를 레이캐스트 순서대로 보관한다.
        /// </summary>
        [Serializable]
        private sealed class EquipmentInputRecord
        {
            public int sourceId;
            public Vector2 point, size;
            public bool isInteractable;
            public string[] clickTargets;
        }

        /// <summary>
        /// 진단 시각과 게임 화면 해상도를 입력 후보와 함께 저장한다.
        /// </summary>
        [Serializable]
        private sealed class EquipmentInputResult
        {
            public string recordedAt;
            public int width, height;
            public EquipmentInputRecord[] records;
        }
    }
}
