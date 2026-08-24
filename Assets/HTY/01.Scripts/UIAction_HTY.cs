using DG.Tweening;
using UnityEngine;
using static UnityEngine.Rendering.DebugUI;

public class UIAction_HTY : MonoBehaviour
{
    private RectTransform _myRect;
    [SerializeField] private float moveDistance = 500f;
    [SerializeField] private float duration = 0.3f;
    private Vector2 _originalPos;

    private void Awake()
    {
        _myRect = GetComponent<RectTransform>();
    }

    public void UiOn()
    {
        gameObject.SetActive(true);

        _myRect.anchoredPosition =
            _originalPos + Vector2.right * moveDistance;

        _myRect.DOAnchorPos(_originalPos, duration)
            .SetEase(Ease.OutCubic);

    }
    public void UiOff()
    {
        _myRect.DOAnchorPos(
                _originalPos + Vector2.right * moveDistance,
                duration
            )
            .SetEase(Ease.InCubic)
            .OnComplete(() => gameObject.SetActive(false));
    }
    

    
}
