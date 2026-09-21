using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using System.Collections.Generic;

public sealed class Talk : MonoBehaviour
{
    [System.Serializable]
    private sealed class Choice
    {
        [TextArea(1, 3)] public string choiceText;
        // 이전 씬의 직접 연결 버튼은 보존하되 숨깁니다.
        [HideInInspector] public Button button;
        [Tooltip("-1: 다음 대사, -2: 종료, 0 이상: Talks 배열 번호")]
        public int nextIndex = -1;
        public UnityEvent onSelected = new UnityEvent();
    }

    [System.Serializable]
    private sealed class Route
    {
        [Min(0)] public int dialogueIndex;
        [Tooltip("선택지가 없는 대사의 이동: -1 다음 대사, -2 종료")]
        public int nextIndex = -1;
        public Choice[] choices;
    }

    [Header("패널 클릭 / 선택지 분기")]
    [SerializeField] private Button dialoguePanelButton;
    [Tooltip("Button이 없는 대화 배경 Image도 연결할 수 있습니다.")]
    [SerializeField] private Graphic dialoguePanel;
    private IDialogueAdvanceInput advanceInput;
    private IDialogueChoiceView choiceView;
    [Header("선택지 자동 생성")]
    [SerializeField] private Transform choiceParent;
    [SerializeField] private Button choiceButtonPrefab;
    [SerializeField, Min(1f)] private float choiceButtonHeight = 50f;
    [SerializeField, Min(0f)] private float choiceSpacing = 8f;
    [SerializeField, Min(0)] private int choicePadding = 8;
    [Header("대사별 선택지 / 분기")]
    [SerializeField] private Route[] routes;
    private bool choosing;
    private bool running;
    private bool started;
    private int revision;
    private int inputFrame = -1;

    [Header("대화 UI")]
    [SerializeField] private TextMeshProUGUI nameText;
    [SerializeField] private TextMeshProUGUI talkText;

    [Header("대화 내용")]
    [SerializeField] private string[] names;
    [SerializeField, TextArea(2, 5)] private string[] talks;

    [Header("타자 효과")]
    [SerializeField, Min(0f)] private float speed = 0.05f;

    [Header("대화 이벤트")]
    [SerializeField] private UnityEvent onDialogueEnded = new UnityEvent();

    private int talkIndex;
    private int visibleCharacterCount;
    private int totalCharacterCount;
    private float typingTimer;
    private bool isTyping;

    private void Start()
    {
        if (nameText == null || talkText == null)
        {
            Debug.LogError("Talk의 Name Text와 Talk Text를 연결해 주세요.", this);
            enabled = false;
            return;
        }

        advanceInput = new UnityDialogueAdvanceInput(dialoguePanelButton, dialoguePanel, talkText, nameText);
        advanceInput.Requested += Next;
        if (!started) StartDialogue();
    }

    private void Update()
    {
        if (!isTyping)
        {
            return;
        }

        if (speed <= 0f)
        {
            CompleteCurrentTalk();
            return;
        }

        typingTimer += Time.unscaledDeltaTime;
        int charactersToReveal = Mathf.FloorToInt(typingTimer / speed);

        if (charactersToReveal <= 0)
        {
            return;
        }

        typingTimer -= charactersToReveal * speed;
        visibleCharacterCount = Mathf.Min(
            visibleCharacterCount + charactersToReveal,
            totalCharacterCount
        );
        talkText.maxVisibleCharacters = visibleCharacterCount;

        if (visibleCharacterCount >= totalCharacterCount)
        {
            CompleteCurrentTalk();
        }
    }

    public void StartDialogue()
    {
        StartDialogueAt(0);
    }

    public void StartDialogueAt(int index)
    {
        if (nameText == null || talkText == null)
        {
            Debug.LogError("Talk의 Name Text와 Talk Text를 연결해 주세요.", this);
            return;
        }
        started = true;
        EnsureChoiceView();
        running = true;
        talkIndex = index;
        ShowCurrentTalk();
    }

    // 스킵 버튼: 현재 경로의 다음 선택지까지 건너뜁니다.
    public void Skip()
    {
        if (!isActiveAndEnabled || !running || choosing || !AcceptInput()) return;
        var visited = new HashSet<int>();
        while (HasTalk(talkIndex))
        {
            Route route = FindRoute(talkIndex);
            if (HasChoices(route) || !visited.Add(talkIndex))
            {
                ShowCurrentTalk();
                CompleteCurrentTalk();
                return;
            }
            talkIndex = ResolveNext(route?.nextIndex ?? -1);
        }
        EndDialogue();
    }

    public void SkipDialogue() => Skip();

    // 패널 클릭: 문장 완성 / 다음 대사. 선택 중에는 진행하지 않습니다.
    public void Next()
    {
        if (!isActiveAndEnabled || !running || choosing || !AcceptInput())
        {
            return;
        }

        if (isTyping)
        {
            CompleteCurrentTalk();
            return;
        }

        talkIndex = ResolveNext(FindRoute(talkIndex)?.nextIndex ?? -1);
        ShowCurrentTalk();
    }

    private bool AcceptInput()
    {
        if (inputFrame == Time.frameCount) return false;
        inputFrame = Time.frameCount;
        return true;
    }

    private void ShowCurrentTalk()
    {
        revision++;
        HideChoices();
        if (!HasTalk(talkIndex))
        {
            EndDialogue();
            return;
        }

        nameText.text = GetName(talkIndex);
        talkText.text = talks[talkIndex] ?? string.Empty;
        talkText.maxVisibleCharacters = 0;
        talkText.ForceMeshUpdate(true);

        visibleCharacterCount = 0;
        totalCharacterCount = talkText.textInfo.characterCount;
        typingTimer = 0f;
        isTyping = true;

        if (totalCharacterCount == 0 || speed <= 0f)
        {
            CompleteCurrentTalk();
        }
    }

    private void CompleteCurrentTalk()
    {
        if (!isTyping) return;
        visibleCharacterCount = totalCharacterCount;
        talkText.maxVisibleCharacters = int.MaxValue;
        typingTimer = 0f;
        isTyping = false;
        Route route = FindRoute(talkIndex);
        if (!HasChoices(route)) return;
        choosing = true;
        EnsureChoiceView();
        var choices = new List<Choice>();
        var labels = new List<string>();
        foreach (Choice choice in route.choices)
        {
            if (choice == null) continue;
            choices.Add(choice);
            labels.Add(choice.choiceText);
        }
        choiceView?.Show(labels, index => SelectChoice(choices[index]));
    }

    public void EndDialogue()
    {
        bool notify = running;
        running = false;
        revision++;
        HideChoices();
        isTyping = false;
        typingTimer = 0f;

        if (nameText != null)
        {
            nameText.text = string.Empty;
        }

        if (talkText != null)
        {
            talkText.text = string.Empty;
            talkText.maxVisibleCharacters = int.MaxValue;
        }

        if (notify) onDialogueEnded?.Invoke();
    }

    private void SelectChoice(Choice choice)
    {
        if (!choosing || !running) return;
        inputFrame = Time.frameCount;
        int next = ResolveNext(choice.nextIndex);
        HideChoices();
        int previousRevision = revision;
        choice.onSelected?.Invoke();
        if (this == null || !isActiveAndEnabled || revision != previousRevision) return;
        talkIndex = next;
        ShowCurrentTalk();
    }

    private int ResolveNext(int target) => target == -1 ? talkIndex + 1 : target;

    private Route FindRoute(int index)
    {
        if (routes != null)
            foreach (Route route in routes)
                if (route != null && route.dialogueIndex == index) return route;
        return null;
    }

    private static bool HasChoices(Route route)
    {
        if (route?.choices != null)
            foreach (Choice choice in route.choices)
                if (choice != null) return true;
        return false;
    }

    private void EnsureChoiceView()
    {
        if (choiceView != null) return;
        if (choiceParent != null &&
            (transform.IsChildOf(choiceParent) ||
             (talkText != null && talkText.transform.IsChildOf(choiceParent)) ||
             (nameText != null && nameText.transform.IsChildOf(choiceParent)) ||
             (dialoguePanel != null && dialoguePanel.transform.IsChildOf(choiceParent))))
        {
            Debug.LogError("Choice Parent에는 대화 본문이 아닌 선택지 전용 컨테이너를 연결해 주세요.", this);
            return;
        }
        choiceView = new UnityDialogueChoiceView(choiceParent, choiceButtonPrefab,
            choiceButtonHeight, choiceSpacing, choicePadding);
    }

    private void HideChoices()
    {
        choosing = false;
        choiceView?.Hide();
        if (routes == null) return;
        foreach (Route route in routes)
        {
            if (route?.choices == null) continue;
            foreach (Choice choice in route.choices)
                if (choice?.button != null && choice.button != choiceButtonPrefab)
                    choice.button.gameObject.SetActive(false);
        }
    }

    private void Awake() => HideChoices();

    private void OnDestroy()
    {
        advanceInput?.Dispose();
        choiceView?.Dispose();
    }

    private bool HasTalk(int index)
    {
        return talks != null && index >= 0 && index < talks.Length;
    }

    private string GetName(int index)
    {
        if (names == null || index < 0 || index >= names.Length)
        {
            return string.Empty;
        }

        return names[index] ?? string.Empty;
    }
}
