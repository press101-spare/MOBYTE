using System.Collections;
using TMPro;
using UnityEngine;

public class BlinkText : MonoBehaviour
{
    [Header("페이드 설정")]
    [SerializeField] private float  fadeDuration = 1.5f;
    [SerializeField] private float stayDuration = 0.5f;
    
    private CanvasGroup _canvasGroup;
    

    private void Awake()
    {
        _canvasGroup = GetComponent<CanvasGroup>();
        if (_canvasGroup == null)
        {
            _canvasGroup = gameObject.AddComponent<CanvasGroup>();
        }
    }

    private IEnumerator Start()
    {
        while (true)
        {
            // 천천히 사라짐
            yield return Fade(1f, 0f);

            // 사라진 상태로 잠깐 대기
            yield return new WaitForSecondsRealtime(stayDuration);

            // 천천히 나타남
            yield return Fade(0f, 1f);

            // 나타난 상태로 잠깐 대기
            yield return new WaitForSecondsRealtime(stayDuration);
        }
        
    }

    private IEnumerator Fade(float startAlpha, float endAlpha)
    {
        if (_canvasGroup == null)
        {
            _canvasGroup = GetComponent<CanvasGroup>();
            if (_canvasGroup == null)
            {
                yield break;
            }
        }

        float time = 0f;
        while (time < fadeDuration)
        {
            time += Time.unscaledDeltaTime;
            float progress = Mathf.Clamp01(time / fadeDuration);
            float smoothProgress = Mathf.SmoothStep(0f,1f,progress);
            
            _canvasGroup.alpha = Mathf.Lerp(startAlpha, endAlpha, smoothProgress);
            
            yield return null;
        }
        _canvasGroup.alpha = endAlpha;
    }
}
