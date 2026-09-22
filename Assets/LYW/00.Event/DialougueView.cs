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

    [Header("버튼")]
    [SerializeField] private Button nextButton;
    [SerializeField] private Button skipButton;

    [Header("선택지")]
    [SerializeField] private Transform choiceParent;
    [SerializeField] private Button choiceButtonPrefab;

    private readonly List<Button> choiceButtons = new();

    public event Action NextClicked;
    public event Action SkipClicked;

    public int CharacterCount
    {
        get
        {
            talkText.ForceMeshUpdate(true);
            return talkText.textInfo.characterCount;
        }
    }

    private void Awake()
    {
        nextButton.onClick.AddListener(() => NextClicked?.Invoke());
        skipButton.onClick.AddListener(() => SkipClicked?.Invoke());
    }

    public void ShowDialogue(string speaker, string text, Sprite image)
    {
        nameText.text = speaker;
        talkText.text = text;

        talkText.maxVisibleCharacters = 0;
        talkText.ForceMeshUpdate(true);

        if (image != null)
            eventImage.sprite = image;
    }

    public void ShowCharacters(int count)
    {
        talkText.maxVisibleCharacters = count;
    }

    public void ShowAllText()
    {
        talkText.maxVisibleCharacters = int.MaxValue;
    }

    public void ShowChoices(
        EventDialogueData.ChoiceData[] choices,
        Action<int> onSelected)
    {
        ClearChoices();

        if (choices == null)
            return;

        for (int i = 0; i < choices.Length; i++)
        {
            if (choices[i] == null)
                continue;

            int index = i;

            Button button = Instantiate(
                choiceButtonPrefab,
                choiceParent
            );

            TextMeshProUGUI text =
                button.GetComponentInChildren<TextMeshProUGUI>();

            if (text != null)
                text.text = choices[i].choiceText;

            button.onClick.AddListener(
                () => onSelected(index)
            );

            choiceButtons.Add(button);
        }
    }

    public void ClearChoices()
    {
        foreach (Button button in choiceButtons)
        {
            if (button != null)
                Destroy(button.gameObject);
        }

        choiceButtons.Clear();
    }

    public void Clear()
    {
        ClearChoices();

        nameText.text = "";
        talkText.text = "";
    }
}