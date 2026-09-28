using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GambleSuccessEffect_HTY : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _successText;
    [SerializeField] private RectTransform _particleArea;
    [SerializeField] private Image _particlePrefab;
    [SerializeField] private int _particleCount = 30;

    private void Start()
    {
        _successText.gameObject.SetActive(false);
    }

    public void PlayEffect(string text)
    {
        _successText.gameObject.SetActive(true);

        _successText.text = text;
        _successText.alpha = 1f;
        _successText.transform.localScale = Vector3.zero;

        _successText.transform
            .DOScale(1.2f, 0.3f)
            .SetEase(Ease.OutBack)
            .OnComplete(() =>
            {
                _successText.transform.DOScale(1f, 0.15f);
            });
        AudioManager.Instance.PlayClipSFX("SFX_CHEER");
        PlayParticles();

        _successText
            .DOFade(0f, 0.5f)
            .SetDelay(1.5f)
            .OnComplete(() =>
            {
                _successText.gameObject.SetActive(false);
            });
    }

    private void PlayParticles()
    {
        for (int i = 0; i < _particleCount; i++)
        {
            Image particle = Instantiate(_particlePrefab, _particleArea);

            particle.gameObject.SetActive(true);
            particle.color = new Color(
                particle.color.r,
                particle.color.g,
                particle.color.b,
                1f
            );

            RectTransform rect = particle.rectTransform;

            rect.anchoredPosition = Vector2.zero;

            float size = Random.Range(15f, 35f);
            rect.sizeDelta = new Vector2(size, size);

            Vector2 direction = Random.insideUnitCircle.normalized;
            float distance = Random.Range(150f, 400f);
            float duration = Random.Range(0.6f, 1.1f);

            rect
                .DOAnchorPos(direction * distance, duration)
                .SetEase(Ease.OutQuad);

            rect
                .DORotate(
                    new Vector3(0, 0, Random.Range(-360f, 360f)),
                    duration,
                    RotateMode.FastBeyond360
                );

            particle
                .DOFade(0f, 0.4f)
                .SetDelay(duration - 0.4f)
                .OnComplete(() =>
                {
                    Destroy(particle.gameObject);
                });
        }
    }
}