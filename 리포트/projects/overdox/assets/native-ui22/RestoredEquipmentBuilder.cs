using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Overdox.Editor
{
    public static partial class RestoredUiBuilder
    {
        /// <summary>
        /// 실제 카드 레이캐스트를 따라 Unity 드래그 이벤트를 재생하고 한 칸 이동을 확인한다.
        /// </summary>
        [MenuItem("OVERDOX/Restore/Verify equipment drag events")]
        public static void VerifyEquipmentDragEvents()
        {
            var owner = UnityEngine.Object.FindFirstObjectByType<RestoredLobbyController>();
            if (!EditorApplication.isPlaying || owner == null || owner.carousel == null)
            {
                throw new InvalidOperationException("로비 장비 화면을 실행한 뒤 검증하세요.");
            }
            int previous = owner.FocusedIndex;
            var carousel = owner.carousel;
            try
            {
                owner.Focus(0);
                carousel.ResetTo(0);
                var camera = owner.GetComponent<Canvas>().worldCamera;
                var center = carousel.items[carousel.items.Length / 2];
                Vector2 start = RectTransformUtility.WorldToScreenPoint(camera, center.position);
                Vector2 end = RectTransformUtility.WorldToScreenPoint(camera, center.position - carousel.transform.right * carousel.cellWidth * carousel.transform.lossyScale.x);
                var data = new PointerEventData(EventSystem.current) { pointerId = -1, position = start, pressPosition = start };
                var hits = new List<RaycastResult>();
                EventSystem.current.RaycastAll(data, hits);
                var first = hits.First();
                data.pointerPressRaycast = first;
                var handler = ExecuteEvents.GetEventHandler<IDragHandler>(first.gameObject);
                ExecuteEvents.Execute(handler, data, ExecuteEvents.beginDragHandler);
                data.position = end;
                ExecuteEvents.Execute(handler, data, ExecuteEvents.dragHandler);
                ExecuteEvents.Execute(handler, data, ExecuteEvents.endDragHandler);
                if (owner.FocusedIndex != 1)
                {
                    throw new InvalidOperationException($"가로 한 칸 드래그 결과 불일치: {owner.FocusedIndex}");
                }
                Directory.CreateDirectory("Docs/Verification/EquipmentCarousel");
                // OS 마우스 검증과 구분해 실제 EventSystem 좌표·경로를 사용한 검사임을 기록한다.
                File.WriteAllText("Docs/Verification/EquipmentCarousel/drag-events.json", $"{{\"utc\":\"{DateTime.UtcNow:o}\",\"scope\":\"Unity EventSystem replay\",\"startIndex\":0,\"endIndex\":{owner.FocusedIndex},\"passed\":true}}");
                Debug.Log("OVERDOX_EQUIPMENT_DRAG_EVENTS_VERIFIED index=0->1");
            }
            finally
            {
                owner.Focus(previous);
                carousel.ResetTo(previous);
            }
        }

        /// <summary>
        /// 실행 중 중앙 카드의 실제 레이캐스트와 드래그 수신 경로를 조회한다.
        /// </summary>
        [MenuItem("OVERDOX/Restore/Inspect equipment input routing")]
        public static void InspectEquipmentInputRouting()
        {
            var owner = UnityEngine.Object.FindFirstObjectByType<RestoredLobbyController>();
            if (!EditorApplication.isPlaying || owner == null || owner.carousel == null)
            {
                throw new InvalidOperationException("로비 장비 화면을 실행한 뒤 입력 경로를 확인하세요.");
            }
            var carousel = owner.carousel;
            var camera = owner.GetComponent<Canvas>().worldCamera;
            var point = RectTransformUtility.WorldToScreenPoint(camera, carousel.items[carousel.items.Length / 2].position);
            var data = new PointerEventData(EventSystem.current) { position = point, pressPosition = point };
            var hits = new List<RaycastResult>();
            EventSystem.current.RaycastAll(data, hits);
            var first = hits.FirstOrDefault();
            var handler = first.gameObject != null ? ExecuteEvents.GetEventHandler<IDragHandler>(first.gameObject) : null;
            bool planeHit = RectTransformUtility.ScreenPointToLocalPointInRectangle((RectTransform)carousel.transform, point, camera, out var local);
            Debug.Log($"OVERDOX_EQUIPMENT_INPUT hits={hits.Count} first={first.gameObject?.name} dragHandler={handler?.name} active={carousel.isActiveAndEnabled} planeHit={planeHit} local={local}");
        }

        /// <summary>
        /// 원작 카드 간격·클리핑 영역을 유지하며 저장된 로비에 캐러셀 입력과 장착 명령을 연결한다.
        /// </summary>
        [MenuItem("OVERDOX/Restore/Restore equipment carousel")]
        public static void RestoreEquipmentCarousel()
        {
            if (EditorApplication.isPlaying || SceneManager.GetActiveScene().isDirty)
            {
                throw new InvalidOperationException("현재 씬을 저장하고 재생을 종료하세요.");
            }
            var setup = EditorSceneManager.GetSceneManagerSetup();
            var scene = EditorSceneManager.OpenScene("Assets/Scenes/OverdoxLobby.unity", OpenSceneMode.Single);
            var controller = UnityEngine.Object.FindFirstObjectByType<RestoredLobbyController>();
            var document = controller.GetComponent<RestoredUiDocument>();
            var viewport = document.Find(37814);
            var carousel = viewport.GetComponent<RestoredEquipmentCarousel>() ?? viewport.gameObject.AddComponent<RestoredEquipmentCarousel>();
            carousel.owner = controller;
            carousel.items = controller.cards.Select(card => (RectTransform)card.transform.parent).ToArray();
            carousel.cellWidth = 210;
            carousel.springStrength = 16;
            carousel.neighborScale = .65f;
            controller.carousel = carousel;

            // 빈 영역에서도 드래그를 시작할 수 있게 하고 원작 RectMask2D 범위를 그대로 사용한다.
            var hitArea = viewport.GetComponent<Image>() ?? viewport.gameObject.AddComponent<Image>();
            hitArea.color = Color.clear;
            hitArea.raycastTarget = true;
            Wire(document.Find(36204), controller.EquipFocused);
            document.Find(34159).gameObject.SetActive(false);
            document.Find(36204).gameObject.SetActive(true);
            EditorSceneManager.SaveScene(scene);
            EditorSceneManager.RestoreSceneManagerSetup(setup);
            Debug.Log("OVERDOX_EQUIPMENT_CAROUSEL_READY cards=10 spacing=210 spring=16");
        }
    }
}
