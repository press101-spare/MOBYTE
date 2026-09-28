using System;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class FadeOut : MonoBehaviour
{
    private Image image;
    public int duration = 1;
    private void Awake()
    {
        image = GetComponent<Image>();
    }

    private void Start()
    {
        gameObject.SetActive(true);
        Sequence sequence = DOTween.Sequence();
        sequence.Append(image.DOFade(0, duration));
        sequence.OnComplete(() => gameObject.SetActive(false));
    }
}
