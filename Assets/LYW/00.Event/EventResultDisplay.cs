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
    // 완전히 표시되는 시간과 사라지는 애니메이션 시간입니다.
    private const float VisibleDuration = 3f;
    private const float FadeDuration = 0.35f;

    // 장면 안에 결과 패널을 하나만 유지합니다.
    private static EventResultDisplay instance;
    private static TMP_FontAsset dialogueFont;

    private CanvasGroup canvasGroup;
    private TextMeshProUGUI resultText;
    private Coroutine hideCoroutine;

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
    private static void ResetStatics()
    {
        // Enter Play Mode 옵션과 관계없이 새 실행마다 정적 참조를 초기화합니다.
        instance = null;
        dialogueFont = null;
    }

    public static void SetFont(TMP_FontAsset font)
    {
        // DialogueView가 가진 한글 폰트를 결과 텍스트에서도 사용합니다.
        if (font == null)
            return;

        dialogueFont = font;

        if (instance != null && instance.resultText != null)
            instance.resultText.font = font;
    }

    public static void Show(string message, Object context = null)
    {
        // 효과 코드에서는 이 함수 하나만 호출하면 결과가 화면에 나타납니다.
        if (string.IsNullOrWhiteSpace(message))
            return;

        EventResultDisplay display = GetOrCreate(context);

        if (display != null)
            display.Present(message);
    }

    private static EventResultDisplay GetOrCreate(Object context)
    {
        // 이미 생성된 패널이 있으면 다시 만들지 않습니다.
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

        // Canvas 아래에 배경 패널, 투명도 제어기와 표시 스크립트를 만듭니다.
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

        // 패널 안쪽 여백을 가진 결과 텍스트를 만듭니다.
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
        // 새 결과가 오면 이전 사라짐 예약을 취소하고 표시 시간을 다시 시작합니다.
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
        // 게임 일시 정지 중에도 결과 UI가 정상적으로 사라지도록 실시간을 사용합니다.
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
