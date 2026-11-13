using UnityEngine;
using UnityEngine.EventSystems;

namespace Overdox
{
    /// <summary>
    /// 하나의 포인터를 소유하여 원작 스틱 표시와 전투 이동 입력을 함께 갱신한다.
    /// </summary>
    public sealed class RestoredMoveStick : MonoBehaviour, IPointerDownHandler, IDragHandler, IPointerUpHandler
    {
        public RectTransform knob, arrow;
        public float radius = 60;
        private int _pointer = int.MinValue;

        /// <summary>
        /// 화면이나 오브젝트가 비활성화될 때 남아 있는 이동 입력을 해제한다.
        /// </summary>
        private void OnDisable()
        {
            Release();
        }

        /// <summary>
        /// 소유 중인 포인터가 없으면 누른 포인터를 연결하고 첫 이동값을 적용한다.
        /// </summary>
        public void OnPointerDown(PointerEventData data)
        {
            // 두 번째 손가락이 기존 이동 방향을 덮어쓰지 못하도록 소유권을 유지한다.
            if (_pointer != int.MinValue)
            {
                return;
            }
            _pointer = data.pointerId;
            OnDrag(data);
        }

        /// <summary>
        /// 연결된 포인터의 UI 로컬 좌표를 반경 안의 이동값과 방향 표시로 변환한다.
        /// </summary>
        public void OnDrag(PointerEventData data)
        {
            var session = ArenaSession.Instance;
            if (_pointer != data.pointerId || session == null || !session.IsFighting)
            {
                return;
            }
            if (!RectTransformUtility.ScreenPointToLocalPointInRectangle((RectTransform)transform, data.position, data.pressEventCamera, out var point))
            {
                return;
            }
            // 입력 크기를 1로 제한하여 스틱 밖을 끌어도 이동 속도가 증가하지 않게 한다.
            var value = Vector2.ClampMagnitude(point / radius, 1);
            session.TouchMove = value;
            knob.anchoredPosition = value * radius;
            arrow.gameObject.SetActive(value.sqrMagnitude > .01f);
            arrow.localRotation = Quaternion.Euler(0, 0, Mathf.Atan2(value.y, value.x) * Mathf.Rad2Deg - 90);
        }

        /// <summary>
        /// 소유한 포인터가 떼어졌을 때만 이동과 스틱 표시를 초기화한다.
        /// </summary>
        public void OnPointerUp(PointerEventData data)
        {
            if (_pointer == data.pointerId)
            {
                Release();
            }
        }

        /// <summary>
        /// 포인터 소유권을 반환하고 남은 이동값과 방향 표시를 제거한다.
        /// </summary>
        private void Release()
        {
            _pointer = int.MinValue;
            if (ArenaSession.Instance != null)
            {
                ArenaSession.Instance.TouchMove = Vector2.zero;
            }
            if (knob != null)
            {
                knob.anchoredPosition = Vector2.zero;
            }
            if (arrow != null)
            {
                arrow.gameObject.SetActive(false);
            }
        }
    }
}
