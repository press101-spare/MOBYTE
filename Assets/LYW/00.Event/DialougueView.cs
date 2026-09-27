using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 대화 화면의 표시만 담당합니다.
/// 이름/대사/배경을 갱신하고 선택지 버튼을 생성하며,
/// 패널·NEXT·SKIP 입력을 Talk에 이벤트로 전달합니다.
/// </summary>
public sealed class DialogueView : MonoBehaviour
{
    [Header("대화 UI")]
    [SerializeField] private TextMeshProUGUI nameText;
    [SerializeField] private TextMeshProUGUI talkText;
    [SerializeField] private Image eventImage;
    [Tooltip("대화창 배경 이미지를 연결하면 배경 전체를 눌러 다음 대사로 진행합니다.")]
    [SerializeField] private Graphic dialoguePanel;

    [Header("버튼")]
    [SerializeField] private Button nextButton;
    [SerializeField] private Button skipButton;

    [Header("선택지")]
    [SerializeField] private Transform choiceParent;
    [SerializeField] private Button choiceButtonPrefab;
    [SerializeField, Min(1f)] private float choiceWidth = 720f;
    [SerializeField, Min(1f)] private float choiceButtonHeight = 64f;
    [SerializeField, Min(0f)] private float choiceSpacing = 12f;
    [SerializeField, Min(0)] private int choicePadding = 12;

    // 현재 화면에 생성한 선택지 버튼입니다. 다음 대사로 갈 때 모두 제거합니다.
    private readonly List<Button> choiceButtons = new();

    // dialoguePanel에 Button이 없으면 실행 중 추가하고 소유 여부를 기록합니다.
    private Button dialoguePanelButton;
    private bool ownsDialoguePanelButton;
    [SerializeField] TMP_FontAsset _font;

    // Talk가 구독하는 UI 입력 이벤트입니다.
    public event Action NextClicked;
    public event Action SkipClicked;

    // 타이핑 효과가 몇 글자까지 진행되어야 하는지 알려줍니다.
    public int CharacterCount
    {
        get
        {
            if (talkText == null)
                return 0;

            talkText.ForceMeshUpdate(true);
            return talkText.textInfo.characterCount;
        }
    }

    private void Awake()
    {
        // 결과 알림도 대화창과 같은 한글 폰트를 사용합니다.
        if (talkText != null)
            EventResultDisplay.SetFont(talkText.font);

        // Inspector 연결이 없어도 필요한 UI를 실행 중 준비합니다.
        EnsureEventBackground();
        ConfigureDialoguePanel();
        ConfigureChoiceParent();

        if (nextButton != null)
        {
            nextButton.onClick.AddListener(OnNextClicked);

            // 큰 대화 패널이 NEXT 버튼의 클릭을 가로채지 않도록 맨 위에 둡니다.
            nextButton.transform.SetAsLastSibling();
        }

        if (skipButton != null)
        {
            skipButton.onClick.AddListener(OnSkipClicked);

            skipButton.transform.SetAsLastSibling();
        }

        ClearChoices();
    }

    private void OnDestroy()
    {
        if (nextButton != null)
            nextButton.onClick.RemoveListener(OnNextClicked);

        if (skipButton != null)
            skipButton.onClick.RemoveListener(OnSkipClicked);

        if (dialoguePanelButton != null)
            dialoguePanelButton.onClick.RemoveListener(OnNextClicked);

        if (ownsDialoguePanelButton && dialoguePanelButton != null)
            Destroy(dialoguePanelButton);
    }

    private void OnNextClicked()
    {
        NextClicked?.Invoke();
    }

    private void OnSkipClicked()
    {
        SkipClicked?.Invoke();
    }

    public void ShowDialogue(string speaker, string text, Sprite image)
    {
        // 새 대사를 넣고 처음에는 글자를 모두 숨겨 타이핑 효과를 준비합니다.
        if (nameText != null)
            nameText.text = speaker;

        if (talkText != null)
        {
            talkText.text = text ?? "";
            talkText.maxVisibleCharacters = 0;
            talkText.ForceMeshUpdate(true);
        }

        if (eventImage != null)
        {
            eventImage.sprite = image;
            eventImage.enabled = image != null;
        }
    }

    public void ShowCharacters(int count)
    {
        if (talkText != null)
            talkText.maxVisibleCharacters = count;
    }

    public void ShowAllText()
    {
        if (talkText != null)
            talkText.maxVisibleCharacters = int.MaxValue;
    }

    public bool ShowChoices(
        EventDialogueData.ChoiceData[] choices,
        Action<int> onSelected)
    {
        // 이전 선택지를 제거한 뒤 데이터 개수만큼 버튼을 동적으로 만듭니다.
        ClearChoices();

        if (choices == null ||
            choiceParent == null ||
            choiceButtonPrefab == null)
        {
            Debug.LogError(
                "DialogueView의 Choice Parent와 Choice Button Prefab을 연결해주세요.",
                this
            );
            return false;
        }

        choiceParent.gameObject.SetActive(true);
        choiceParent.SetAsLastSibling();

        for (int i = 0; i < choices.Length; i++)
        {
            if (choices[i] == null)
                continue;

            int choiceIndex = i;

            // 프리팹 원본은 유지하고 Choice Parent 아래에 복사본을 만듭니다.
            Button button = Instantiate(
                choiceButtonPrefab,
                choiceParent,
                false
            );

            button.onClick.RemoveAllListeners();

            TMP_Text buttonText =
                button.GetComponentInChildren<TMP_Text>(true);

            buttonText.font = _font;
            
            if (buttonText != null)
                buttonText.text = choices[i].choiceText ?? string.Empty;

            LayoutElement layoutElement =
                button.GetComponent<LayoutElement>();

            if (layoutElement == null)
                layoutElement = button.gameObject.AddComponent<LayoutElement>();

            layoutElement.minHeight = choiceButtonHeight;
            layoutElement.preferredHeight = choiceButtonHeight;
            button.interactable = true;

            button.onClick.AddListener(
                () => onSelected?.Invoke(choiceIndex)
            );

            choiceButtons.Add(button);
        }

        if (choiceButtons.Count == 0)
        {
            choiceParent.gameObject.SetActive(false);
            return false;
        }

        Canvas.ForceUpdateCanvases();

        if (choiceParent is RectTransform rectTransform)
            LayoutRebuilder.ForceRebuildLayoutImmediate(rectTransform);

        return true;
    }

    public void ClearChoices()
    {
        foreach (Button button in choiceButtons)
        {
            if (button != null)
            {
                // Destroy는 프레임 끝에 실행되므로 먼저 꺼서 중복 클릭을 방지합니다.
                button.gameObject.SetActive(false);
                Destroy(button.gameObject);
            }
        }

        choiceButtons.Clear();

        if (choiceParent != null)
            choiceParent.gameObject.SetActive(false);
    }

    public void Clear()
    {
        ClearChoices();

        if (nameText != null)
            nameText.text = "";

        if (talkText != null)
            talkText.text = "";
    }

    private void ConfigureDialoguePanel()
    {
        if (dialoguePanel == null)
            return;

        dialoguePanel.raycastTarget = true;
        dialoguePanelButton = dialoguePanel.GetComponent<Button>();

        if (dialoguePanelButton == null)
        {
            // 대화 패널 자체를 클릭해 다음 대사로 갈 수 있도록 Button을 자동 추가합니다.
            dialoguePanelButton = dialoguePanel.gameObject.AddComponent<Button>();
            ownsDialoguePanelButton = true;
        }

        dialoguePanelButton.targetGraphic = dialoguePanel;
        dialoguePanelButton.transition = Selectable.Transition.None;
        dialoguePanelButton.navigation = new Navigation
        {
            mode = Navigation.Mode.None
        };
        dialoguePanelButton.onClick.AddListener(OnNextClicked);

        if (nameText != null)
            nameText.raycastTarget = false;

        if (talkText != null)
            talkText.raycastTarget = false;
    }

    private void EnsureEventBackground()
    {
        // Event Image를 Inspector에서 연결했다면 그대로 사용합니다.
        if (eventImage != null)
            return;

        Canvas canvas = GetComponentInParent<Canvas>();

        if (canvas == null)
            canvas = FindFirstObjectByType<Canvas>();

        if (canvas == null)
        {
            Debug.LogError("이벤트 배경을 생성할 Canvas가 없습니다.", this);
            return;
        }

        // 연결된 이미지가 없으면 Canvas 전체를 덮는 배경 Image를 자동 생성합니다.
        GameObject backgroundObject = new GameObject(
            "Event Background",
            typeof(RectTransform),
            typeof(CanvasRenderer),
            typeof(Image)
        );

        RectTransform rectTransform =
            backgroundObject.GetComponent<RectTransform>();

        rectTransform.SetParent(canvas.transform, false);
        rectTransform.anchorMin = Vector2.zero;
        rectTransform.anchorMax = Vector2.one;
        rectTransform.offsetMin = Vector2.zero;
        rectTransform.offsetMax = Vector2.zero;
        // 대화창과 버튼보다 뒤에 그려지도록 첫 번째 자식으로 이동합니다.
        rectTransform.SetAsFirstSibling();

        eventImage = backgroundObject.GetComponent<Image>();
        eventImage.raycastTarget = false;
        eventImage.preserveAspect = false;
        eventImage.enabled = false;
    }

    private void ConfigureChoiceParent()
    {
        if (!(choiceParent is RectTransform rectTransform))
            return;

        rectTransform.anchorMin = new Vector2(0.5f, 0.5f);
        rectTransform.anchorMax = new Vector2(0.5f, 0.5f);
        rectTransform.anchoredPosition = Vector2.zero;
        rectTransform.sizeDelta = new Vector2(choiceWidth, 0f);
        rectTransform.pivot = new Vector2(0.5f, 0.5f);

        Graphic background = choiceParent.GetComponent<Graphic>();
        if (background != null)
            background.raycastTarget = true;

        // 선택지 개수에 맞춰 세로로 자동 배치합니다.
        VerticalLayoutGroup layout =
            choiceParent.GetComponent<VerticalLayoutGroup>();

        if (layout == null)
            layout = choiceParent.gameObject.AddComponent<VerticalLayoutGroup>();

        int padding = Mathf.Max(0, choicePadding);
        layout.padding = new RectOffset(padding, padding, padding, padding);
        layout.spacing = Mathf.Max(0f, choiceSpacing);
        layout.childAlignment = TextAnchor.MiddleCenter;
        layout.childControlWidth = true;
        layout.childControlHeight = true;
        layout.childForceExpandWidth = true;
        layout.childForceExpandHeight = false;

        // 선택지 버튼 수에 따라 부모 높이가 자동으로 변하게 합니다.
        ContentSizeFitter fitter =
            choiceParent.GetComponent<ContentSizeFitter>();

        if (fitter == null)
            fitter = choiceParent.gameObject.AddComponent<ContentSizeFitter>();

        fitter.horizontalFit = ContentSizeFitter.FitMode.Unconstrained;
        fitter.verticalFit = ContentSizeFitter.FitMode.PreferredSize;
    }
}
