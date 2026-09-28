using UnityEngine;
using UnityEngine.EventSystems;

namespace JJB.Script
{
    public class SimpleJoystick : MonoBehaviour, IPointerDownHandler, IDragHandler, IPointerUpHandler
    {
        [SerializeField] private RectTransform background;
        [SerializeField] private RectTransform handle;
        [SerializeField] private float radius = 80f;

        public Vector2 Direction { get; private set; }

        public void OnPointerDown(PointerEventData eventData)
        {
            UpdateJoystick(eventData);
        }

        public void OnDrag(PointerEventData eventData)
        {
            UpdateJoystick(eventData);
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            Direction = Vector2.zero;

            if (handle != null)
                handle.anchoredPosition = Vector2.zero;
        }

        private void UpdateJoystick(PointerEventData eventData)
        {
            if (background == null || handle == null)
                return;

            RectTransformUtility.ScreenPointToLocalPointInRectangle(
                background,
                eventData.position,
                eventData.pressEventCamera,
                out Vector2 localPoint
            );

            Direction = Vector2.ClampMagnitude(
                localPoint / radius,
                1f
            );

            handle.anchoredPosition =
                Direction * radius;
        }
    }
}