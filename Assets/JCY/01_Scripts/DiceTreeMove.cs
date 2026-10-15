using DG.Tweening;
using UnityEngine;

public class DiceTreeMove : MonoBehaviour
{
    private RectTransform _rectTfm;
    [SerializeField] private RectTransform _moveTfm;
    [SerializeField] RectTransform _originTfm;
    public float moveSpeed;
    [SerializeField] private bool _isDisplay = false;
    
    void Start()
    {
        _rectTfm= GetComponent<RectTransform>();
    }

    public void Move()
    {
        if (_isDisplay)
        {
            _isDisplay = false;

            _rectTfm.DOAnchorPos(
                _originTfm.anchoredPosition,
                moveSpeed
            ).SetEase(Ease.OutBack);
        }
        else
        {
            _isDisplay = true;

            _rectTfm.DOAnchorPos(
                _moveTfm.anchoredPosition,
                moveSpeed
            ).SetEase(Ease.OutBack);
        }
    }

    
}
