using UnityEngine;

public sealed class EventDialogueData : MonoBehaviour
{
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

    public Sprite DefaultBackground => defaultBackground;
    public EventEffect[] EndEffects => endEffects;

    public bool TryGetNode(int index, out DialogueNode node)
    {
        node = null;

        if (dialogues == null)
            return false;

        if (index < 0 || index >= dialogues.Length)
            return false;

        node = dialogues[index];

        return node != null;
    }
}