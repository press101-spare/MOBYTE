using UnityEngine;

namespace JJB.Script.SettingUI
{
    public class TabCarouselVisual : MonoBehaviour
    {
        [SerializeField] private RectTransform viewport;
        [SerializeField] private TabCarouselItem[] items;

        [Header("Scale")]
        [SerializeField] private float centerScale = 1f;
        [SerializeField] private float sideScale = 0.8f;

        [Header("Alpha")]
        [SerializeField] private float centerAlpha = 1f;
        [SerializeField] private float sideAlpha = 0.4f;

        [SerializeField] private float effectDistance = 80f;

        public void Refresh(Vector2 _)
        {
            foreach (TabCarouselItem item in items)
            {
                float distance = item.GetDistance(viewport);

                float t = Mathf.Clamp01(
                    distance / effectDistance
                );

                item.SetVisual(
                    Mathf.Lerp(centerScale, sideScale, t),
                    Mathf.Lerp(centerAlpha, sideAlpha, t)
                );
            }
        }
    }
}