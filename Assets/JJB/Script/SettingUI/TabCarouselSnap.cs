using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using UnityEngine.Events;
using UnityEngine.EventSystems;

namespace JJB.Script.SettingUI
{
    public class TabCarouselSnap : MonoBehaviour
    {
        [SerializeField] private ScrollRect scrollRect;
        [SerializeField] private RectTransform viewport;
        [SerializeField] private RectTransform content;
        [SerializeField] private TabCarouselItem[] items;

        [SerializeField] private float snapDuration = 0.2f;

        private Tween _snapTween;

        public void EndDrag(BaseEventData _)
        {
            SnapClosest();
        }

        private void SnapClosest()
        {
            TabCarouselItem closest = null;
            float minDistance = float.MaxValue;

            foreach (TabCarouselItem item in items)
            {
                float distance = item.GetDistance(viewport);

                if (distance >= minDistance)
                    continue;

                minDistance = distance;
                closest = item;
            }

            if (closest != null)
                SnapTo(closest);
        }

        private void SnapTo(TabCarouselItem item)
        {
            scrollRect.StopMovement();
            _snapTween?.Kill();

            Vector3 itemPosition =
                viewport.InverseTransformPoint(
                    item.Rect.TransformPoint(
                        item.Rect.rect.center
                    )
                );

            float difference =
                viewport.rect.center.y - itemPosition.y;

            float targetY =
                content.anchoredPosition.y + difference;

            _snapTween = content
                .DOAnchorPosY(targetY, snapDuration)
                .SetEase(Ease.OutCubic);
        }

        private void OnDisable()
        {
            _snapTween?.Kill();
        }
    }
}