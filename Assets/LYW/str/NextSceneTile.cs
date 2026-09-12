using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class NextSceneTile : MonoBehaviour
{
    [SerializeField] private string nextSceneName;

    private GameObject panel;

    private void Start()
    {
        CreateUI();
        panel.SetActive(false);
    }

    // 플레이어가 타일맵 콜라이더에 닿았을 때
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            panel.SetActive(true);
        }
    }

    // 타일맵 콜라이더에서 떨어졌을 때
    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            panel.SetActive(false);
        }
    }

    private void CreateUI()
    {
        // Canvas 생성
        GameObject canvasObj = new GameObject("MoveCanvas");

        Canvas canvas = canvasObj.AddComponent<Canvas>();
        canvas.renderMode = RenderMode.ScreenSpaceOverlay;

        CanvasScaler scaler = canvasObj.AddComponent<CanvasScaler>();
        scaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
        scaler.referenceResolution = new Vector2(1920, 1080);

        canvasObj.AddComponent<GraphicRaycaster>();


        // Panel 생성
        panel = new GameObject("MovePanel");
        panel.transform.SetParent(canvasObj.transform, false);

        Image panelImage = panel.AddComponent<Image>();
        panelImage.color = new Color(0.1f, 0.1f, 0.1f, 0.9f);

        RectTransform panelRect = panel.GetComponent<RectTransform>();
        panelRect.anchorMin = new Vector2(0.5f, 0.5f);
        panelRect.anchorMax = new Vector2(0.5f, 0.5f);
        panelRect.pivot = new Vector2(0.5f, 0.5f);
        panelRect.sizeDelta = new Vector2(500, 250);
        panelRect.anchoredPosition = Vector2.zero;


        // 안내 문구
        GameObject textObj = new GameObject("MessageText");
        textObj.transform.SetParent(panel.transform, false);

        Text text = textObj.AddComponent<Text>();

        text.text = "다음으로 이동하시겠습니까?";
        text.font =
            Resources.GetBuiltinResource<Font>("LegacyRuntime.ttf");

        text.fontSize = 28;
        text.alignment = TextAnchor.MiddleCenter;
        text.color = Color.white;

        RectTransform textRect =
            textObj.GetComponent<RectTransform>();

        textRect.anchorMin = new Vector2(0.5f, 0.5f);
        textRect.anchorMax = new Vector2(0.5f, 0.5f);
        textRect.pivot = new Vector2(0.5f, 0.5f);

        textRect.sizeDelta = new Vector2(450, 100);
        textRect.anchoredPosition = new Vector2(0, 50);


        // 이동 버튼
        CreateButton(
            "이동",
            new Vector2(-100, -60),
            YesButton
        );


        // 취소 버튼
        CreateButton(
            "취소",
            new Vector2(100, -60),
            NoButton
        );
    }


    private void CreateButton(
        string buttonText,
        Vector2 position,
        UnityEngine.Events.UnityAction action
    )
    {
        GameObject buttonObj =
            new GameObject(buttonText + "Button");

        buttonObj.transform.SetParent(panel.transform, false);


        Image image = buttonObj.AddComponent<Image>();

        image.color =
            new Color(0.25f, 0.25f, 0.25f, 1f);


        Button button =
            buttonObj.AddComponent<Button>();

        button.onClick.AddListener(action);


        RectTransform buttonRect =
            buttonObj.GetComponent<RectTransform>();

        buttonRect.anchorMin = new Vector2(0.5f, 0.5f);
        buttonRect.anchorMax = new Vector2(0.5f, 0.5f);
        buttonRect.pivot = new Vector2(0.5f, 0.5f);

        buttonRect.sizeDelta = new Vector2(150, 60);
        buttonRect.anchoredPosition = position;


        // 버튼 글자
        GameObject textObj = new GameObject("Text");

        textObj.transform.SetParent(buttonObj.transform, false);

        Text text = textObj.AddComponent<Text>();

        text.text = buttonText;

        text.font =
            Resources.GetBuiltinResource<Font>("LegacyRuntime.ttf");

        text.fontSize = 24;
        text.alignment = TextAnchor.MiddleCenter;
        text.color = Color.white;


        RectTransform textRect =
            textObj.GetComponent<RectTransform>();

        textRect.anchorMin = Vector2.zero;
        textRect.anchorMax = Vector2.one;

        textRect.offsetMin = Vector2.zero;
        textRect.offsetMax = Vector2.zero;
    }


    // 이동 버튼
    public void YesButton()
    {
        SceneManager.LoadScene(nextSceneName);
    }


    // 취소 버튼
    public void NoButton()
    {
        panel.SetActive(false);
    }
}