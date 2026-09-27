using UnityEngine;

public sealed class EventDialogueData : MonoBehaviour
{
    [System.Serializable]
    public sealed class DialogueSequence
    {
        public string eventName;
        public Sprite background;
        public DialogueNode[] dialogues;
        public EventEffect[] endEffects;
    }

    [System.Serializable]
    public sealed class ChoiceData
    {
        public string choiceText;

        [Tooltip("-1 = 다음 대사, -2 = 종료, 0 이상 = 지정 대사")]
        public int nextIndex = -1;

        public EventEffect[] effects;
    }

    [System.Serializable]
    public sealed class DialogueNode
    {
        public string speaker;

        [TextArea(2, 5)]
        public string text;

        public Sprite background;

        [Tooltip("-1 = 다음 대사, -2 = 종료, 0 이상 = 지정 대사")]
        public int nextIndex = -1;

        public ChoiceData[] choices;
    }

    [SerializeField] private Sprite defaultBackground;
    [SerializeField] private DialogueNode[] dialogues;
    [SerializeField] private EventEffect[] endEffects;
    [Header("랜덤 대화 목록")]
    [Tooltip("목록이 비어 있지 않으면 시작할 때 이 중 하나를 무작위로 선택합니다.")]
    [SerializeField] private DialogueSequence[] randomDialogueList;

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
