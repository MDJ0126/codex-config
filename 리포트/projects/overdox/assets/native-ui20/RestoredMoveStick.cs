using UnityEngine;
using UnityEngine.EventSystems;

namespace Overdox
{
    public sealed class RestoredMoveStick : MonoBehaviour, IPointerDownHandler, IDragHandler, IPointerUpHandler
    {
        public RectTransform knob, arrow;
        public float radius = 60;
        private int _pointer = int.MinValue;
        public void OnPointerDown(PointerEventData data)
        {
            if (_pointer != int.MinValue) { return; }
            _pointer = data.pointerId;
            OnDrag(data);
        }
        public void OnDrag(PointerEventData data)
        {
            var session = ArenaSession.Instance;
            if (_pointer != data.pointerId || session == null || !session.IsFighting) { return; }
            if (!RectTransformUtility.ScreenPointToLocalPointInRectangle((RectTransform)transform, data.position, data.pressEventCamera, out var point)) { return; }
            var value = Vector2.ClampMagnitude(point / radius, 1);
            session.TouchMove = value;
            knob.anchoredPosition = value * radius;
            arrow.gameObject.SetActive(value.sqrMagnitude > .01f);
            arrow.localRotation = Quaternion.Euler(0, 0, Mathf.Atan2(value.y, value.x) * Mathf.Rad2Deg - 90);
        }
        public void OnPointerUp(PointerEventData data)
        {
            if (_pointer == data.pointerId) { Release(); }
        }
        private void OnDisable() { Release(); }
        private void Release()
        {
            _pointer = int.MinValue;
            if (ArenaSession.Instance != null) { ArenaSession.Instance.TouchMove = Vector2.zero; }
            if (knob != null) { knob.anchoredPosition = Vector2.zero; }
            if (arrow != null) { arrow.gameObject.SetActive(false); }
        }
    }
}
