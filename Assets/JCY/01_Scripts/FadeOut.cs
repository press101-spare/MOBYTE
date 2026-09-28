using System;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class FadeOut : MonoBehaviour
{
    private Image image;
    private void Awake()
    {
        image = GetComponent<Image>();
    }

    private void Start()
    {
        Sequence sequence = DOTween.Sequence();
        sequence.Append(image.DOFade(0, 1));
        sequence.OnComplete(() => gameObject.SetActive(false));
    }
}
