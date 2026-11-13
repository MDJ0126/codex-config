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
        [Serializable]
        private sealed class EquipmentHitAreaResult
        {
            public string utc;
            public Vector2 previousSize, clipSize, hitSize;
            public bool sharesClipRect, raycastTarget;
        }

        [Serializable]
        private sealed class EquipmentInputRoute
        {
            public Vector2 localPoint;
            public string firstHit, dragHandler;
            public bool routesToCarousel;
        }

        [Serializable]
        private sealed class EquipmentInputRoutes
        {
            public string utc;
            public EquipmentInputRoute[] samples;
        }

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

            var clip = carousel.transform.Find("Native clip viewport") as RectTransform;
            var offsets = new[] { Vector2.zero, new Vector2(-.4f, .35f), new Vector2(.4f, .35f), new Vector2(-.4f, -.35f), new Vector2(.4f, -.35f) };
            var samples = new List<EquipmentInputRoute>();
            foreach (var offset in offsets)
            {
                var localPoint = Vector2.Scale(offset, clip.rect.size);
                data.position = RectTransformUtility.WorldToScreenPoint(camera, clip.TransformPoint(localPoint));
                hits.Clear();
                EventSystem.current.RaycastAll(data, hits);
                var target = hits.FirstOrDefault().gameObject;
                var dragHandler = target != null ? ExecuteEvents.GetEventHandler<IDragHandler>(target) : null;
                samples.Add(new EquipmentInputRoute
                {
                    localPoint = localPoint,
                    firstHit = target != null ? target.name : "none",
                    dragHandler = dragHandler != null ? dragHandler.name : "none",
                    routesToCarousel = dragHandler == carousel.gameObject
                });
            }
            // 실제 레이캐스트 증거다. OS 마우스 버튼·드래그 입력 성공 여부와는 별도로 기록한다.
            Directory.CreateDirectory("Docs/Verification/EquipmentCarousel");
            File.WriteAllText("Docs/Verification/EquipmentCarousel/input-routes-with-clip.json",
                JsonUtility.ToJson(new EquipmentInputRoutes { utc = DateTime.UtcNow.ToString("o"), samples = samples.ToArray() }, true));
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

            // 원본 UIPanel의 부모 Rect는 기본 크기다. 실제 원작 클리핑 크기는 별도 자식에 저장된다.
            var clipViewport = viewport.Find("Native clip viewport") as RectTransform;
            if (clipViewport == null || clipViewport.GetComponent<RectMask2D>() == null)
            {
                throw new InvalidOperationException("장비 목록의 원작 클리핑 영역을 먼저 복원하세요.");
            }
            var previousSize = viewport.rect.size;
            var previousHitArea = viewport.GetComponent<Image>();
            if (previousHitArea != null)
            {
                UnityEngine.Object.DestroyImmediate(previousHitArea);
            }

            // 투명 Graphic을 마스크와 같은 Rect에 둔다. 빈 곳도 입력을 받되 자식 카드가 우선한다.
            var hitArea = clipViewport.GetComponent<Image>() ?? clipViewport.gameObject.AddComponent<Image>();
            hitArea.color = Color.clear;
            hitArea.raycastTarget = true;
            Wire(document.Find(36204), controller.EquipFocused);
            document.Find(34159).gameObject.SetActive(false);
            document.Find(36204).gameObject.SetActive(true);
            EditorSceneManager.SaveScene(scene);
            var hitAreaResult = new EquipmentHitAreaResult
            {
                utc = DateTime.UtcNow.ToString("o"),
                previousSize = previousSize,
                clipSize = clipViewport.rect.size,
                hitSize = hitArea.rectTransform.rect.size,
                sharesClipRect = hitArea.transform == clipViewport,
                raycastTarget = hitArea.raycastTarget
            };
            Directory.CreateDirectory("Docs/Verification/EquipmentCarousel");
            File.WriteAllText("Docs/Verification/EquipmentCarousel/hit-area.json", JsonUtility.ToJson(hitAreaResult, true));
            EditorSceneManager.RestoreSceneManagerSetup(setup);
            Debug.Log("OVERDOX_EQUIPMENT_CAROUSEL_READY cards=10 spacing=210 spring=16");
        }
    }
}
