using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace JJB.Script.Slot
{
    public class SlotReel : MonoBehaviour
    {
        [Header("UI")]
        [SerializeField] private RectTransform content;
        [SerializeField] private Image symbolPrefab;

        [Header("심볼")]
        [SerializeField] private Sprite[] symbols;

        [Header("릴 설정")]
        [SerializeField] private float symbolHeight = 100f;
        [SerializeField] private int symbolCount = 35;
        
        [SerializeField] private int startCenterIndex = 1;

        public int ResultIndex { get; private set; }

        private readonly List<Image> _symbolImages = new List<Image>();

        private Tween _spinTween;

        private void Awake()
        {
            CreateSymbols();
        }

        private void CreateSymbols()
        {
            for (int i = 0; i < symbolCount; i++)
            {
                Image newSymbol = Instantiate(symbolPrefab, content);

                RectTransform rect = newSymbol.rectTransform;

                rect.anchorMin = new Vector2(0.5f, 0.5f);
                rect.anchorMax = new Vector2(0.5f, 0.5f);
                rect.pivot = new Vector2(0.5f, 0.5f);
                rect.anchoredPosition = new Vector2(0f, (i - startCenterIndex) * symbolHeight);

                newSymbol.sprite = GetRandomSymbol();

                _symbolImages.Add(newSymbol);
            }

            ResultIndex = Random.Range(0, symbols.Length);

            _symbolImages[startCenterIndex].sprite = symbols[ResultIndex];
        }

        public IEnumerator Spin(float duration)
        {
            _spinTween?.Kill();

            content.anchoredPosition = Vector2.zero;

            RandomizeSymbols();

            _symbolImages[startCenterIndex].sprite = symbols[ResultIndex];

            int newResult = Random.Range(0, symbols.Length);

            int targetIndex = symbolCount - 3;

            _symbolImages[targetIndex].sprite = symbols[newResult];

            float targetSymbolY = (targetIndex - startCenterIndex) * symbolHeight;
            float targetContentY = -targetSymbolY;
            float stopDuration = 0.16f;
            float beforeStopY = targetContentY + symbolHeight;

            Sequence sequence = DOTween.Sequence();

            sequence.Append(content.DOAnchorPosY(beforeStopY, duration - stopDuration).SetEase(Ease.Linear));
            sequence.Append(content.DOAnchorPosY(targetContentY, stopDuration).SetEase(Ease.OutCubic));

            _spinTween = sequence;

            yield return _spinTween.WaitForCompletion();

            ResultIndex = newResult;
        }

        private void RandomizeSymbols()
        {
            foreach (Image image in _symbolImages) 
            {
                image.sprite = GetRandomSymbol();
            }
        }

        private Sprite GetRandomSymbol() 
        {
            int index = Random.Range(0, symbols.Length);
            return symbols[index];
        }

        private void OnDisable() 
        { 
            _spinTween?.Kill();
        }
    }
}
