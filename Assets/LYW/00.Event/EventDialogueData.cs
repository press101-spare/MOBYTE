using UnityEngine;

/// <summary>
/// 이벤트 대사, 선택지, 배경과 효과 연결 정보를 보관합니다.
/// 실행 로직은 Talk에 두고 이 클래스는 데이터 선택과 조회만 담당합니다.
/// </summary>
public sealed class EventDialogueData : MonoBehaviour
{
    [System.Serializable]
    public sealed class DialogueSequence
    {
        // 랜덤 목록의 이벤트 한 묶음입니다.
        public string eventName;
        public Sprite background;
        public DialogueNode[] dialogues;
        public EventEffect[] endEffects;
    }

    [System.Serializable]
    public sealed class ChoiceData
    {
        // 선택지에 표시할 문구와 선택 후 이동/실행할 효과입니다.
        public string choiceText;

        [Tooltip("-1 = 다음 대사, -2 = 종료, 0 이상 = 지정 대사")]
        public int nextIndex = -1;

        public EventEffect[] effects;
    }

    [System.Serializable]
    public sealed class DialogueNode
    {
        // 한 줄의 대사 데이터입니다. background가 비면 이벤트 기본 배경을 씁니다.
        public string speaker;

        [TextArea(2, 5)]
        public string text;

        public Sprite background;

        [Tooltip("-1 = 다음 대사, -2 = 종료, 0 이상 = 지정 대사")]
        public int nextIndex = -1;

        public ChoiceData[] choices;
    }

    // 랜덤 목록이 비었을 때 사용할 단일 대화용 기본 데이터입니다.
    [SerializeField] private Sprite defaultBackground;
    [SerializeField] private DialogueNode[] dialogues;
    [SerializeField] private EventEffect[] endEffects;
    [Header("랜덤 대화 목록")]
    [Tooltip("목록이 비어 있지 않으면 시작할 때 이 중 하나를 무작위로 선택합니다.")]
    [SerializeField] private DialogueSequence[] randomDialogueList;

    // 이번 실행에서 무작위로 선택된 이벤트입니다.
    private DialogueSequence selectedSequence;

    public Sprite DefaultBackground =>
        selectedSequence != null && selectedSequence.background != null
            ? selectedSequence.background
            : defaultBackground;

    public EventEffect[] EndEffects =>
        selectedSequence != null
            ? selectedSequence.endEffects
            : endEffects;

    public string SelectedEventName =>
        selectedSequence?.eventName ?? string.Empty;

    public void SelectRandomDialogue()
    {
        selectedSequence = null;

        if (randomDialogueList == null || randomDialogueList.Length == 0)
            return;

        // 대사가 하나도 없는 빈 항목은 추첨 대상에서 제외합니다.
        int validCount = 0;

        foreach (DialogueSequence sequence in randomDialogueList)
        {
            if (HasDialogue(sequence))
                validCount++;
        }

        if (validCount == 0)
            return;

        int target = Random.Range(0, validCount);

        foreach (DialogueSequence sequence in randomDialogueList)
        {
            if (!HasDialogue(sequence))
                continue;

            if (target-- != 0)
                continue;

            selectedSequence = sequence;
            Debug.Log($"선택된 랜덤 이벤트: {sequence.eventName}", this);
            return;
        }
    }

    public bool TryGetNode(int index, out DialogueNode node)
    {
        // 선택된 랜덤 이벤트가 있으면 그 대사를, 없으면 기본 대사를 조회합니다.
        node = null;

        DialogueNode[] activeDialogues =
            selectedSequence != null
                ? selectedSequence.dialogues
                : dialogues;

        if (activeDialogues == null)
            return false;

        if (index < 0 || index >= activeDialogues.Length)
            return false;

        node = activeDialogues[index];

        return node != null;
    }

    private static bool HasDialogue(DialogueSequence sequence)
    {
        return sequence?.dialogues != null &&
               sequence.dialogues.Length > 0;
    }
}
