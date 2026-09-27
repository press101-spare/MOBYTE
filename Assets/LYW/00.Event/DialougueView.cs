using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

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

    private readonly List<Button> choiceButtons = new();
    private Button dialoguePanelButton;
    private bool ownsDialoguePanelButton;
    [SerializeField] TMP_FontAsset _font;

    public event Action NextClicked;
    public event Action SkipClicked;

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
        if (talkText != null)
            EventResultDisplay.SetFont(talkText.font);

        EnsureEventBackground();
        ConfigureDialoguePanel();
        ConfigureChoiceParent();

        if (nextButton != null)
        {
            nextButton.onClick.AddListener(OnNextClicked);

            // The dialogue panel is a large raycast target. Keep the actual
            // control buttons above it so their clicks are not swallowed.
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
                // Destroy is deferred until the end of the frame. Disable the
                // old choice immediately so it cannot be drawn or clicked
                // while the next choice screen is being created.
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

        ContentSizeFitter fitter =
            choiceParent.GetComponent<ContentSizeFitter>();

        if (fitter == null)
            fitter = choiceParent.gameObject.AddComponent<ContentSizeFitter>();

        fitter.horizontalFit = ContentSizeFitter.FitMode.Unconstrained;
        fitter.verticalFit = ContentSizeFitter.FitMode.PreferredSize;
    }
}
