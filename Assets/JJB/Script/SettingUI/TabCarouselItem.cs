using System;
using UnityEngine;
using UnityEngine.UI;

namespace JJB.Script.SettingUI
{
    public class TabCarouselItem : MonoBehaviour
    {
        private RectTransform _rect;
        private CanvasGroup _canvasGroup;

        public RectTransform Rect => _rect;

        private void Awake()
        {
            _rect = GetComponent<RectTransform>();
            _canvasGroup = GetComponent<CanvasGroup>();
        }

        public void SetVisual(float scale, float alpha)
        {
            _rect.localScale = Vector3.one * scale;
            _canvasGroup.alpha = alpha;
        }

        public float GetDistance(RectTransform viewport)
        {
            Vector3 position = viewport.InverseTransformPoint(
                _rect.TransformPoint(_rect.rect.center)
            );

            return Mathf.Abs(
                position.y - viewport.rect.center.y
            );
        }
    }
}