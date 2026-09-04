using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class NextSceneTile : MonoBehaviour
{
    [Header("이동할 씬 이름")]
    [SerializeField] private string nextSceneName;

    private GameObject panel;
    private bool playerInside = false;

    private void Start()
    {
        CreateUI();

        // 처음에는 선택창 숨김
        panel.SetActive(false);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Player 태그를 가진 오브젝트가 들어왔을 때
        if (collision.CompareTag("Player"))
        {
            playerInside = true;

            panel.SetActive(true);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        // 플레이어가 타일에서 벗어났을 때
        if (collision.CompareTag("Player"))
        {
            playerInside = false;

            panel.SetActive(false);
        }
    }

    private void CreateUI()
    {

        GameObject canvasObj = new GameObject("MoveCanvas");

        Canvas canvas = canvasObj.AddComponent<Canvas>();
        canvas.renderMode = RenderMode.ScreenSpaceOverlay;

        CanvasScaler scaler = canvasObj.AddComponent<CanvasScaler>();
        scaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
        scaler.referenceResolution = new Vector2(1920, 1080);

        canvasObj.AddComponent<GraphicRaycaster>();
        

        panel = new GameObject("MovePanel");

        panel.transform.SetParent(canvasObj.transform, false);

        Image panelImage = panel.AddComponent<Image>();

        // 패널 색
        panelImage.color = new Color(
            0.1f,
            0.1f,
            0.1f,
            0.9f
        );

        RectTransform panelRect =
            panel.GetComponent<RectTransform>();

        panelRect.anchorMin =
            new Vector2(0.5f, 0.5f);

        panelRect.anchorMax =
            new Vector2(0.5f, 0.5f);

        panelRect.pivot =
            new Vector2(0.5f, 0.5f);

        panelRect.sizeDelta =
            new Vector2(550, 280);

        panelRect.anchoredPosition =
            Vector2.zero;


        

        GameObject textObj =
            new GameObject("MessageText");

        textObj.transform.SetParent(
            panel.transform,
            false
        );

        Text text =
            textObj.AddComponent<Text>();

        text.text =
            "다음으로 이동하시겠습니까?";

        text.font =
            Resources.GetBuiltinResource<Font>(
                "LegacyRuntime.ttf"
            );

        text.fontSize = 28;

        text.alignment =
            TextAnchor.MiddleCenter;

        text.color = Color.white;

        RectTransform textRect =
            textObj.GetComponent<RectTransform>();

        textRect.anchorMin =
            new Vector2(0.5f, 0.5f);

        textRect.anchorMax =
            new Vector2(0.5f, 0.5f);

        textRect.pivot =
            new Vector2(0.5f, 0.5f);

        textRect.sizeDelta =
            new Vector2(500, 100);

        textRect.anchoredPosition =
            new Vector2(0, 60);
        

        CreateButton(
            panel.transform,
            "YesButton",
            "이동",
            new Vector2(-110, -70),
            YesButton
        );
        

        CreateButton(
            panel.transform,
            "NoButton",
            "취소",
            new Vector2(110, -70),
            NoButton
        );
    }


    private void CreateButton(
        Transform parent,
        string objectName,
        string buttonText,
        Vector2 position,
        UnityEngine.Events.UnityAction action
    )
    {
        // 버튼 오브젝트
        GameObject buttonObj =
            new GameObject(objectName);

        buttonObj.transform.SetParent(
            parent,
            false
        );


        // 버튼 이미지
        Image image =
            buttonObj.AddComponent<Image>();

        image.color =
            new Color(
                0.25f,
                0.25f,
                0.25f,
                1f
            );


        // Button 컴포넌트
        Button button =
            buttonObj.AddComponent<Button>();

        button.onClick.AddListener(action);


        // 버튼 위치 / 크기
        RectTransform buttonRect =
            buttonObj.GetComponent<RectTransform>();

        buttonRect.anchorMin =
            new Vector2(0.5f, 0.5f);

        buttonRect.anchorMax =
            new Vector2(0.5f, 0.5f);

        buttonRect.pivot =
            new Vector2(0.5f, 0.5f);

        buttonRect.sizeDelta =
            new Vector2(170, 65);

        buttonRect.anchoredPosition =
            position;
        

        GameObject textObj =
            new GameObject("Text");

        textObj.transform.SetParent(
            buttonObj.transform,
            false
        );

        Text text =
            textObj.AddComponent<Text>();

        text.text = buttonText;

        text.font =
            Resources.GetBuiltinResource<Font>(
                "LegacyRuntime.ttf"
            );

        text.fontSize = 24;

        text.alignment =
            TextAnchor.MiddleCenter;

        text.color = Color.white;


        RectTransform textRect =
            textObj.GetComponent<RectTransform>();

        textRect.anchorMin =
            Vector2.zero;

        textRect.anchorMax =
            Vector2.one;

        textRect.offsetMin =
            Vector2.zero;

        textRect.offsetMax =
            Vector2.zero;
    }
    

    public void YesButton()
    {
        if (string.IsNullOrEmpty(nextSceneName))
        {
            Debug.LogError(
                "Next Scene Name이 설정되지 않았습니다."
            );

            return;
        }

        SceneManager.LoadScene(nextSceneName);
    }
    
    public void NoButton()
    {
        panel.SetActive(false);
    }
}