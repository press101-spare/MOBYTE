using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// 대화의 진행 상태를 관리하는 컨트롤러입니다.
/// 타이핑, 다음 대사, 전체 스킵, 선택지 대기와 효과 실행을 담당합니다.
/// 화면 표시는 DialogueView, 데이터 보관은 EventDialogueData가 담당합니다.
/// </summary>
public sealed class Talk : MonoBehaviour
{
    [Header("대화")]
    [SerializeField] private EventDialogueData dialogueData;
    [SerializeField] private DialogueView view;

    [Header("설정")]
    [SerializeField, Min(0f)] private float typingSpeed = 0.05f;

    [Header("종료")]
    [SerializeField] private UnityEvent onDialogueEnded;

    // 현재 출력 중인 대사의 배열 번호입니다.
    private int index;

    // 대화/타이핑/선택지 상태를 분리하여 잘못된 중복 입력을 막습니다.
    private bool running;
    private bool typing;
    private bool choosing;

    private Coroutine typingCoroutine;

    private void OnEnable()
    {
        // View의 버튼 입력을 대화 진행 함수와 연결합니다.
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

        // 등록된 이벤트 중 유효한 대화 하나를 무작위로 선택합니다.
        dialogueData.SelectRandomDialogue();

        Show();
    }

    private void Next()
    {
        // 선택지 화면에서는 패널/NEXT 클릭으로 넘어가지 못하게 합니다.
        if (!running || choosing)
            return;

        // 타이핑 중 클릭하면 다음 대사 대신 현재 문장을 즉시 완성합니다.
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
        // 선택지를 고르기 전에는 스킵할 수 없습니다.
        if (!running || choosing)
            return;

        if (typing)
            FinishTyping();

        if (choosing)
            return;

        // 잘못 연결된 대사 순환으로 무한 반복되는 것을 방지합니다.
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

            // 선택지가 없는 대사는 계속 건너뛰고 선택지가 나오면 멈춥니다.
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

        // 대사별 배경이 없으면 선택된 이벤트의 기본 배경을 사용합니다.
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
        // 실제 시간 기준으로 한 글자씩 공개합니다(Time.timeScale의 영향을 받지 않음).
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

        // 버튼 번호를 다시 실제 ChoiceData로 변환해 선택 결과를 실행합니다.
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

        // 하나의 선택지에 여러 효과를 순서대로 연결할 수 있습니다.
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
        // -1은 다음 대사, -2는 종료, 0 이상은 지정된 대사 번호입니다.
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

        // 선택된 이벤트 전체가 끝났을 때 실행할 공통 효과입니다.
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
