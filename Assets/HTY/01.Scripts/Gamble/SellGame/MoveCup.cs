using System.Collections;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MoveCup : MonoBehaviour
{
    [Header("컵")]
    [SerializeField] private RectTransform[] _cups;
    [SerializeField] private Button[] _cupButtons;

    [Header("공")]
    [SerializeField] private RectTransform _ball;

    [Header("UI")]
    [SerializeField] private Button _startButton;
    [SerializeField] private Slider _timer;
    [SerializeField] private TextMeshProUGUI _resultText;

    [Header("컵 이동")]
    [SerializeField] private float _raiseHeight = 200f;
    [SerializeField] private float _during = 0.5f;

    [Header("셔플")]
    [SerializeField] private int _minMoveCount = 3;
    [SerializeField] private int _maxMoveCount = 6;
    [SerializeField] private float _shuffleDelay = 0.2f;

    [Header("선택")]
    [SerializeField] private float _selectTimeLimit = 5f;
    [SerializeField] private float _openWaitTime = 2f;

    private Vector2[] _groundPositions;
    private RectTransform _winCup;
    private bool _canSelect;
    private bool _isPlaying;
    private Coroutine _timerCoroutine;

    private void Start()
    {
        SellGame();
    }

    public void SellGame()
    {
        SaveCupPositions();
        ResetGame();
        _timer.gameObject.SetActive(false);
    }

    private void SaveCupPositions()
    {
        _groundPositions = new Vector2[_cups.Length];
        for (int i = 0; i < _cups.Length; i++) _groundPositions[i] = _cups[i].anchoredPosition;
    }

    private void ResetGame()
    {
        _canSelect = false;
        _isPlaying = false;
        _resultText.text = "";

        int winIndex = Random.Range(0, _cups.Length);
        _winCup = _cups[winIndex];
        _ball.anchoredPosition = _groundPositions[winIndex];

        for (int i = 0; i < _cups.Length; i++) _cups[i].anchoredPosition = _groundPositions[i] + Vector2.up * _raiseHeight;

        SetCupButtons(false);
        _startButton.interactable = true;

        if (_timer != null)
        {
            _timer.maxValue = _selectTimeLimit;
            _timer.value = _selectTimeLimit;
        }
    }

    public void StartGame()
    {
        if (_isPlaying) return;

        _isPlaying = true;
        _startButton.interactable = false;
        StartCoroutine(GameRoutine());
    }

    private IEnumerator GameRoutine()
    {
        for (int i = 0; i < _cups.Length; i++) _cups[i].DOAnchorPos(_groundPositions[i], _during);

        yield return new WaitForSeconds(_during);
        yield return StartCoroutine(Shuffle());

        _canSelect = true;
        SetCupButtons(true);

        _resultText.text = "컵을 선택하세요";
        _timer.gameObject.SetActive(true);
        _timerCoroutine = StartCoroutine(SelectTimer());
    }

    private IEnumerator Shuffle()
    {
        int moveCount = Random.Range(_minMoveCount, _maxMoveCount + 1);

        for (int i = 0; i < moveCount; i++)
        {
            int firstIndex = Random.Range(0, _cups.Length);
            int secondIndex = Random.Range(0, _cups.Length);

            while (firstIndex == secondIndex) secondIndex = Random.Range(0, _cups.Length);

            RectTransform firstCup = _cups[firstIndex];
            RectTransform secondCup = _cups[secondIndex];

            Vector2 firstPosition = firstCup.anchoredPosition;
            Vector2 secondPosition = secondCup.anchoredPosition;

            firstCup.DOAnchorPos(secondPosition, _during);
            secondCup.DOAnchorPos(firstPosition, _during);

            if (_winCup == firstCup) _ball.DOAnchorPos(secondPosition, _during);
            else if (_winCup == secondCup) _ball.DOAnchorPos(firstPosition, _during);

            yield return new WaitForSeconds(_during);

            _ball.anchoredPosition = _winCup.anchoredPosition;

            yield return new WaitForSeconds(_shuffleDelay);
        }
    }

    private IEnumerator SelectTimer()
    {
        float currentTime = _selectTimeLimit;
        _timer.maxValue = _selectTimeLimit;
        _timer.value = currentTime;

        while (currentTime > 0f)
        {
            if (!_canSelect) yield break;

            currentTime -= Time.deltaTime;
            _timer.value = Mathf.Max(currentTime, 0f);

            yield return null;
        }

        _canSelect = false;
        SetCupButtons(false);
        _resultText.text = "시간 초과!";
        _timer.gameObject.SetActive(false);

        yield return new WaitForSeconds(_openWaitTime);
        yield return StartCoroutine(RaiseAllCups());

        EndGame(0);
    }

    public void SelectCup(int index)
    {
        if (!_canSelect) return;
        if (index < 0 || index >= _cups.Length) return;

        _canSelect = false;
        SetCupButtons(false);

        if (_timerCoroutine != null)
        {
            StopCoroutine(_timerCoroutine);
            _timerCoroutine = null;
        }

        StartCoroutine(OpenSelectedCup(_cups[index]));
    }

    private IEnumerator OpenSelectedCup(RectTransform selectedCup)
    {
        selectedCup.DOAnchorPosY(selectedCup.anchoredPosition.y + _raiseHeight, _during);

        yield return new WaitForSeconds(_during);

        _resultText.text = selectedCup == _winCup ? "승리!" : "패배!";

        yield return new WaitForSeconds(_openWaitTime);

        for (int i = 0; i < _cups.Length; i++)
        {
            if (_cups[i] == selectedCup) continue;
            _cups[i].DOAnchorPosY(_cups[i].anchoredPosition.y + _raiseHeight, _during);
        }

        yield return new WaitForSeconds(_during);

        EndGame(selectedCup == _winCup ? 2 : 0);
    }

    private IEnumerator RaiseAllCups()
    {
        for (int i = 0; i < _cups.Length; i++) _cups[i].DOAnchorPosY(_cups[i].anchoredPosition.y + _raiseHeight, _during);
        yield return new WaitForSeconds(_during);
    }

    private void SetCupButtons(bool value)
    {
        for (int i = 0; i < _cupButtons.Length; i++) _cupButtons[i].interactable = value;
    }

    private void EndGame(int x)
    {
        _canSelect = false;
        _isPlaying = false;
        SetCupButtons(false);
        Debug.Log("야바위 게임 종료");
        GambleManager.instance.GambleEnd(x);
        gameObject.transform.parent.gameObject.SetActive(false);
        
    }
}