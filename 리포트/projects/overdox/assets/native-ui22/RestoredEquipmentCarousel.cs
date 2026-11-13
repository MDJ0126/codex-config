using UnityEngine;
using UnityEngine.EventSystems;

namespace Overdox
{
    /// <summary>
    /// 고정 카드 풀을 사용해 장비 목록의 드래그·중앙 정렬·주변 카드 축소를 처리한다.
    /// </summary>
    public sealed class RestoredEquipmentCarousel : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler, IScrollHandler
    {
        public RestoredLobbyController owner;
        public RectTransform[] items;
        // 원작 UIGrid와 UICenterOnChild에서 읽은 값을 씬에 저장한다.
        public float cellWidth = 210;
        public float springStrength = 16;
        // 주변 축소율은 공개 화면 대조값이며 원작 코드 상수로 확인한 수치는 아니다.
        public float neighborScale = .65f;
        private int _focus, _count;
        private int _pointer = int.MinValue;
        private float _position, _dragStartPosition;
        private Vector2 _dragStartPoint;
        private bool _isInitialized;

        /// <summary>
        /// 드래그하지 않을 때 선택한 카드의 중심으로 시간 배율과 독립적으로 수렴한다.
        /// </summary>
        private void LateUpdate()
        {
            if (!_isInitialized)
            {
                return;
            }
            if (_pointer == int.MinValue)
            {
                _position = Mathf.Lerp(_position, _focus, 1 - Mathf.Exp(-springStrength * Time.unscaledDeltaTime));
            }
            UpdateLayout();
        }

        /// <summary>
        /// 창이 닫히면 포인터 소유권을 해제하여 다음 진입에 드래그가 남지 않게 한다.
        /// </summary>
        private void OnDisable()
        {
            _pointer = int.MinValue;
        }

        /// <summary>
        /// 데이터가 갱신된 카드 풀의 중심 인덱스와 목록 범위를 연결한다.
        /// </summary>
        public void Bind(int focus, int count)
        {
            _focus = focus;
            _count = count;
            if (!_isInitialized)
            {
                ResetTo(focus);
            }
            UpdateLayout();
        }

        /// <summary>
        /// 슬롯 변경처럼 다른 목록으로 진입할 때 이전 이동 잔여량을 제거한다.
        /// </summary>
        public void ResetTo(int focus)
        {
            _focus = focus;
            _position = focus;
            _pointer = int.MinValue;
            _isInitialized = true;
            UpdateLayout();
        }

        /// <summary>
        /// 첫 포인터의 시작 위치를 기록하고 카드 클릭 대신 가로 드래그를 시작한다.
        /// </summary>
        public void OnBeginDrag(PointerEventData data)
        {
            if (_pointer != int.MinValue || _count <= 0
                || !RectTransformUtility.ScreenPointToLocalPointInRectangle((RectTransform)transform, data.pressPosition, data.pressEventCamera, out _dragStartPoint))
            {
                return;
            }
            _pointer = data.pointerId;
            _dragStartPosition = _position;
            data.eligibleForClick = false;
            Debug.Log($"OVERDOX_CAROUSEL_DRAG_BEGIN focus={_focus} position={_position:F3}");
        }

        /// <summary>
        /// 소유한 포인터의 가로 이동을 목록 인덱스로 바꾸고 가까운 카드 데이터를 연결한다.
        /// </summary>
        public void OnDrag(PointerEventData data)
        {
            if (_pointer != data.pointerId || !TryGetPoint(data, out var point))
            {
                return;
            }
            // UI 로컬 좌표를 사용하므로 해상도나 Canvas 배율이 달라도 같은 카드 간격을 유지한다.
            _position = Mathf.Clamp(_dragStartPosition - (point.x - _dragStartPoint.x) / cellWidth, 0, _count - 1);
            owner.Focus(Mathf.RoundToInt(_position));
            UpdateLayout();
        }

        /// <summary>
        /// 소유 포인터를 놓으면 가장 가까운 카드로 중앙 정렬을 요청한다.
        /// </summary>
        public void OnEndDrag(PointerEventData data)
        {
            if (_pointer != data.pointerId)
            {
                return;
            }
            _pointer = int.MinValue;
            owner.Focus(Mathf.RoundToInt(_position));
            Debug.Log($"OVERDOX_CAROUSEL_DRAG_END focus={_focus} position={_position:F3}");
        }

        /// <summary>
        /// 마우스 휠 한 입력을 장비 목록의 한 카드 이동으로 처리한다.
        /// </summary>
        public void OnScroll(PointerEventData data)
        {
            if (_count <= 0 || Mathf.Approximately(data.scrollDelta.y, 0))
            {
                return;
            }
            owner.Focus(Mathf.Clamp(_focus - (int)Mathf.Sign(data.scrollDelta.y), 0, _count - 1));
        }

        /// <summary>
        /// 포인터의 화면 위치를 캐러셀의 로컬 좌표로 변환한다.
        /// </summary>
        private bool TryGetPoint(PointerEventData data, out Vector2 point)
        {
            return RectTransformUtility.ScreenPointToLocalPointInRectangle((RectTransform)transform, data.position, data.pressEventCamera, out point);
        }

        /// <summary>
        /// 중심 카드와의 거리로 풀의 위치·크기를 계산하고 데이터 범위 밖 카드는 숨긴다.
        /// </summary>
        private void UpdateLayout()
        {
            if (items == null)
            {
                return;
            }
            int center = items.Length / 2;
            for (int index = 0; index < items.Length; index++)
            {
                int dataIndex = _focus + index - center;
                var item = items[index];
                item.gameObject.SetActive(dataIndex >= 0 && dataIndex < _count);
                float distance = dataIndex - _position;
                item.anchoredPosition = new Vector2(distance * cellWidth, 0);
                item.localScale = Vector3.one * Mathf.Lerp(1, neighborScale, Mathf.Clamp01(Mathf.Abs(distance)));
            }
        }
    }
}
