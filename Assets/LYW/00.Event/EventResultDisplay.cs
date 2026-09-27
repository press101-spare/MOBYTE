using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 이벤트 결과를 화면에 표시하는 자동 생성 UI입니다.
/// 장면에 별도의 오브젝트를 배치하지 않아도 됩니다.
/// </summary>
public sealed class EventResultDisplay : MonoBehaviour
{
    private const float VisibleDuration = 3f;
    private const float FadeDuration = 0.35f;

    private static EventResultDisplay instance;
    private static TMP_FontAsset dialogueFont;

    private CanvasGroup canvasGroup;
    private TextMeshProUGUI resultText;
    private Coroutine hideCoroutine;

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
    private static void ResetStatics()
    {
        instance = null;
        dialogueFont = null;
    }

    public static void SetFont(TMP_FontAsset font)
    {
        if (font == null)
            return;

        dialogueFont = font;

        if (instance != null && instance.resultText != null)
            instance.resultText.font = font;
    }

    public static void Show(string message, Object context = null)
    {
        if (string.IsNullOrWhiteSpace(message))
            return;

        EventResultDisplay display = GetOrCreate(context);

        if (display != null)
            display.Present(message);
    }

    private static EventResultDisplay GetOrCreate(Object context)
    {
        if (instance != null)
            return instance;

        instance = FindFirstObjectByType<EventResultDisplay>();

        if (instance != null)
            return instance;

        Canvas canvas = FindFirstObjectByType<Canvas>();

        if (canvas == null)
        {
            Debug.LogError("이벤트 결과를 표시할 Canvas가 없습니다.", context);
            return null;
        }

        GameObject panelObject = new(
            "Event Result Display",
            typeof(RectTransform),
            typeof(CanvasRenderer),
            typeof(Image),
            typeof(CanvasGroup),
            typeof(EventResultDisplay)
        );

        RectTransform panelRect = panelObject.GetComponent<RectTransform>();
        panelRect.SetParent(canvas.transform, false);
        panelRect.anchorMin = new Vector2(0.5f, 0.68f);
        panelRect.anchorMax = new Vector2(0.5f, 0.68f);
        panelRect.pivot = new Vector2(0.5f, 0.5f);
        panelRect.anchoredPosition = Vector2.zero;
        panelRect.sizeDelta = new Vector2(900f, 170f);
        panelRect.SetAsLastSibling();

        Image background = panelObject.GetComponent<Image>();
        background.color = new Color(0.03f, 0.05f, 0.09f, 0.9f);
        background.raycastTarget = false;

        CanvasGroup group = panelObject.GetComponent<CanvasGroup>();
        group.interactable = false;
        group.blocksRaycasts = false;

        GameObject textObject = new(
            "Result Text",
            typeof(RectTransform),
            typeof(CanvasRenderer),
            typeof(TextMeshProUGUI)
        );

        RectTransform textRect = textObject.GetComponent<RectTransform>();
        textRect.SetParent(panelRect, false);
        textRect.anchorMin = Vector2.zero;
        textRect.anchorMax = Vector2.one;
        textRect.offsetMin = new Vector2(30f, 18f);
        textRect.offsetMax = new Vector2(-30f, -18f);

        TextMeshProUGUI text = textObject.GetComponent<TextMeshProUGUI>();
        text.alignment = TextAlignmentOptions.Center;
        text.color = Color.white;
        text.fontSize = 40f;
        text.enableAutoSizing = true;
        text.fontSizeMin = 24f;
        text.fontSizeMax = 40f;
        text.textWrappingMode = TextWrappingModes.Normal;
        text.raycastTarget = false;

        if (dialogueFont != null)
            text.font = dialogueFont;

        instance = panelObject.GetComponent<EventResultDisplay>();
        instance.canvasGroup = group;
        instance.resultText = text;
        panelObject.SetActive(false);

        return instance;
    }

    private void Present(string message)
    {
        gameObject.SetActive(true);
        transform.SetAsLastSibling();

        canvasGroup.alpha = 1f;
        resultText.text = message;

        if (hideCoroutine != null)
            StopCoroutine(hideCoroutine);

        hideCoroutine = StartCoroutine(HideAfterDelay());
    }

    private IEnumerator HideAfterDelay()
    {
        yield return new WaitForSecondsRealtime(VisibleDuration);

        float elapsed = 0f;

        while (elapsed < FadeDuration)
        {
            elapsed += Time.unscaledDeltaTime;
            canvasGroup.alpha = 1f - Mathf.Clamp01(elapsed / FadeDuration);
            yield return null;
        }

        hideCoroutine = null;
        gameObject.SetActive(false);
    }
}
