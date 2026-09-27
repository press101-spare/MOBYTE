using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public sealed class Talk : MonoBehaviour
{
    [Header("대화")]
    [SerializeField] private EventDialogueData dialogueData;
    [SerializeField] private DialogueView view;

    [Header("설정")]
    [SerializeField, Min(0f)] private float typingSpeed = 0.05f;

    [Header("종료")]
    [SerializeField] private UnityEvent onDialogueEnded;

    private int index;

    private bool running;
    private bool typing;
    private bool choosing;

    private Coroutine typingCoroutine;

    private void OnEnable()
    {
        if (view == null)
            return;

        view.NextClicked += Next;
        view.SkipClicked += Skip;
    }

    private void OnDisable()
    {
        if (view == null)
            return;

        view.NextClicked -= Next;
        view.SkipClicked -= Skip;
    }

    private void Start()
    {
        StartDialogue();
    }

    public void StartDialogue()
    {
        if (dialogueData == null || view == null)
        {
            Debug.LogError(
                "DialogueData 또는 DialogueView를 연결해주세요.",
                this
            );

            return;
        }

        index = 0;

        running = true;
        choosing = false;

        dialogueData.SelectRandomDialogue();

        Show();
    }

    private void Next()
    {
        if (!running || choosing)
            return;

        if (typing)
        {
            FinishTyping();
            return;
        }

        if (!GetNode(out var node))
        {
            EndDialogue();
            return;
        }

        Move(node.nextIndex);
    }

    private void Skip()
    {
        if (!running || choosing)
            return;

        if (typing)
            FinishTyping();

        if (choosing)
            return;

        HashSet<int> visited = new();

        while (visited.Add(index))
        {
            if (!GetNode(out var node))
            {
                EndDialogue();
                return;
            }

            int next = GetNext(node.nextIndex);

            if (next == -2 ||
                !dialogueData.TryGetNode(next, out var nextNode))
            {
                EndDialogue();
                return;
            }

            index = next;

            if (!HasChoices(nextNode))
                continue;

            Show();
            FinishTyping();

            return;
        }

        EndDialogue();
    }

    private void Show()
    {
        StopTyping();

        choosing = false;

        view.ClearChoices();

        if (!GetNode(out var node))
        {
            EndDialogue();
            return;
        }

        Sprite image = node.background != null
            ? node.background
            : dialogueData.DefaultBackground;

        view.ShowDialogue(
            node.speaker,
            node.text,
            image
        );

        int length = view.CharacterCount;

        if (typingSpeed <= 0f || length <= 0)
        {
            typing = true;
            FinishTyping();
            return;
        }

        typing = true;

        typingCoroutine =
            StartCoroutine(TypeText(length));
    }

    private IEnumerator TypeText(int length)
    {
        for (int i = 1; i <= length; i++)
        {
            yield return new WaitForSecondsRealtime(
                typingSpeed
            );

            if (!typing)
                yield break;

            view.ShowCharacters(i);
        }

        typing = false;
        typingCoroutine = null;

        ShowChoices();
    }

    private void FinishTyping()
    {
        if (!typing)
            return;

        StopTyping();

        view.ShowAllText();

        ShowChoices();
    }

    private void ShowChoices()
    {
        if (!GetNode(out var node) ||
            !HasChoices(node))
            return;

        choosing = view.ShowChoices(
            node.choices,
            i => SelectChoice(node.choices[i])
        );

        if (!choosing)
        {
            Debug.LogError(
                $"대사 {index}의 선택지를 표시하지 못했습니다.",
                this
            );
        }
    }

    private void SelectChoice(
        EventDialogueData.ChoiceData choice)
    {
        if (!choosing || choice == null)
            return;

        choosing = false;

        view.ClearChoices();

        if (choice.effects != null)
        {
            Array.ForEach(
                choice.effects,
                effect => effect?.Apply()
            );
        }

        if (this == null || !isActiveAndEnabled)
            return;

        Move(choice.nextIndex);
    }

    private void Move(int target)
    {
        int next = GetNext(target);

        if (next == -2 ||
            !dialogueData.TryGetNode(next, out _))
        {
            EndDialogue();
            return;
        }

        index = next;

        Show();
    }

    private int GetNext(int target)
    {
        return target == -1
            ? index + 1
            : target;
    }

    private bool GetNode(
        out EventDialogueData.DialogueNode node)
    {
        return dialogueData.TryGetNode(
            index,
            out node
        );
    }

    private static bool HasChoices(
        EventDialogueData.DialogueNode node)
    {
        return node?.choices != null &&
               Array.Exists(
                   node.choices,
                   choice => choice != null
               );
    }

    private void StopTyping()
    {
        typing = false;

        if (typingCoroutine == null)
            return;

        StopCoroutine(typingCoroutine);

        typingCoroutine = null;
    }

    private void EndDialogue()
    {
        if (!running)
            return;

        running = false;
        choosing = false;

        StopTyping();

        view.Clear();

        if (dialogueData.EndEffects != null)
        {
            Array.ForEach(
                dialogueData.EndEffects,
                effect => effect?.Apply()
            );
        }

        onDialogueEnded?.Invoke();
    }
}
