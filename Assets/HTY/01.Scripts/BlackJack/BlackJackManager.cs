using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class BlackJackManager : MonoBehaviour
{
    [Header("게임진행")]
    public int _playerSum;
    public int _dealerSum;
    public bool _morePlay = false;
    public bool _notMorePlayer;
    public bool _notMoreDealer;
    public bool _checkCard;
    private bool _first = true;
    private bool _gameEnd;

    [Header("블랙잭 룰")]
    [SerializeField] private int _blackJackNumber = 21;
    [SerializeField] private int _dealerStopNumber = 17;
    [SerializeField] private float _choiceTime = 10f;

    [Header("UI오브젝트")]
    public Slider _timer;
    public GameObject _checkButton;
    public GameObject _moreButton;
    public GameObject _standButton;
    public TextMeshProUGUI _endingText;
    public TextMeshProUGUI _titleText;

    [Header("카드덱")]
    public List<Sprite> _originCard = new List<Sprite>();
    public List<Sprite> _copyCard = new List<Sprite>();

    [Header("연출용")]
    [SerializeField] private Transform _fristVec;
    [SerializeField] private Transform _endVecPlayer;
    [SerializeField] private Transform _endVecDealer;
    [SerializeField] private Vector3 _endRotate;
    public float _during = 2f;
    public GameObject _thisCard;

    private List<GameObject> _spawnedCards = new List<GameObject>();

    private void Update()
    {
        if (Keyboard.current != null && Keyboard.current.yKey.wasPressedThisFrame)
        {
            StartCoroutine(BlackjackGame());
        }
    }

    private IEnumerator BlackjackGame()
    {
        ResetGame();

        NewCard(0);
        yield return new WaitForSeconds(1.2f);

        NewCard(1);
        yield return new WaitForSeconds(1.2f);

        NewCard(0);
        yield return new WaitForSeconds(1.2f);

        NewCard(1);
        yield return new WaitForSeconds(3f);

        StartCoroutine(TimerCol());
    }

    private void ResetGame()
    {
        _playerSum = 0;
        _dealerSum = 0;

        _morePlay = false;
        _notMorePlayer = false;
        _notMoreDealer = false;
        _checkCard = false;
        _first = true;
        _gameEnd = false;

        _copyCard = new List<Sprite>(_originCard);

        for (int i = 0; i < _spawnedCards.Count; i++)
        {
            if (_spawnedCards[i] != null)
            {
                Destroy(_spawnedCards[i]);
            }
        }

        _spawnedCards.Clear();

        _timer.gameObject.SetActive(false);
        _checkButton.SetActive(false);
        _moreButton.SetActive(false);

        if (_standButton != null)
        {
            _standButton.SetActive(false);
        }

        _endingText.text = "";
        _titleText.text = "";
    }

    private IEnumerator TimerCol()
    {
        if (_gameEnd) yield break;

        _checkCard = false;

        _titleText.text = "카드를 확인하세요";
        _checkButton.SetActive(true);

        _timer.gameObject.SetActive(true);
        _timer.value = 1;

        float time = _choiceTime;

        while (time > 0)
        {
            time -= Time.deltaTime;
            _timer.value = time / _choiceTime;

            if (_checkCard)
            {
                break;
            }

            yield return null;
        }

        _timer.gameObject.SetActive(false);
        _checkButton.SetActive(false);
        _titleText.text = "";

        if (!_checkCard)
        {
            CheckPlayerCard();
        }
    }

    public void CheckPlayerCard()
    {
        if (_gameEnd) return;

        BlackJackCard[] cards = transform.GetComponentsInChildren<BlackJackCard>();

        for (int i = 0; i < cards.Length; i++)
        {
            if (!cards[i]._iamCheck)
            {
                cards[i].OpenCard();
                _playerSum += cards[i]._myNumber;
            }
        }

        _checkCard = true;

        if (_playerSum > _blackJackNumber)
        {
            FinishGame();
            return;
        }

        if (_playerSum == _blackJackNumber)
        {
            _notMorePlayer = true;
        }

        if (_first)
        {
            _first = false;
            StartCoroutine(MoreGame());
        }
    }

    public void MoreTurn()
    {
        if (_gameEnd) return;

        _morePlay = true;
    }

    public void StandTurn()
    {
        if (_gameEnd) return;

        _notMorePlayer = true;
        _morePlay = false;
    }

    public IEnumerator MoreGame()
    {
        yield return new WaitForSeconds(1f);

        while (!_notMorePlayer || !_notMoreDealer)
        {
            if (_gameEnd) yield break;

            if (!_notMorePlayer)
            {
                _morePlay = false;

                _moreButton.SetActive(true);

                if (_standButton != null)
                {
                    _standButton.SetActive(true);
                }

                _titleText.text = "카드를 더?";

                _timer.gameObject.SetActive(true);
                _timer.value = 1;

                float time = _choiceTime;

                while (time > 0)
                {
                    time -= Time.deltaTime;
                    _timer.value = time / _choiceTime;

                    if (_morePlay || _notMorePlayer)
                    {
                        break;
                    }

                    yield return null;
                }

                _timer.gameObject.SetActive(false);
                _moreButton.SetActive(false);

                if (_standButton != null)
                {
                    _standButton.SetActive(false);
                }

                _titleText.text = "";

                if (_morePlay)
                {
                    NewCard(0);
                    yield return new WaitForSeconds(_during);

                    yield return StartCoroutine(TimerCol());

                    if (_playerSum > _blackJackNumber)
                    {
                        FinishGame();
                        yield break;
                    }
                }
                else
                {
                    _notMorePlayer = true;
                }
            }

            if (!_notMoreDealer)
            {
                if (_dealerSum < _dealerStopNumber)
                {
                    _titleText.text = "딜러가 카드를 뽑습니다";

                    NewCard(1);
                    yield return new WaitForSeconds(_during);

                    _titleText.text = "";

                    if (_dealerSum > _blackJackNumber)
                    {
                        FinishGame();
                        yield break;
                    }
                }
                else
                {
                    _notMoreDealer = true;
                }
            }

            yield return new WaitForSeconds(0.5f);
        }

        FinishGame();
    }

    private void FinishGame()
    {
        if (_gameEnd) return;

        _gameEnd = true;

        StopAllCoroutines();

        _timer.gameObject.SetActive(false);
        _checkButton.SetActive(false);
        _moreButton.SetActive(false);

        if (_standButton != null)
        {
            _standButton.SetActive(false);
        }

        string result = "";

        if (_playerSum > _blackJackNumber)
        {
            result = "플레이어 버스트!\n딜러 승리";
        }
        else if (_dealerSum > _blackJackNumber)
        {
            result = "딜러 버스트!\n플레이어 승리";
        }
        else if (_playerSum > _dealerSum)
        {
            result = "플레이어 승리!";
        }
        else if (_playerSum < _dealerSum)
        {
            result = "딜러 승리!";
        }
        else
        {
            result = "무승부!";
        }

        _titleText.text = "게임 종료";

        _endingText.text =
            result +
            "\n\n플레이어: " + _playerSum +
            "\n딜러: " + _dealerSum;
    }

    #region 카드전용

    public void NewCard(int id)
    {
        if (_copyCard.Count <= 0)
        {
            FinishGame();
            return;
        }

        Transform endVec;

        if (id == 0)
        {
            endVec = _endVecPlayer;
        }
        else
        {
            endVec = _endVecDealer;
        }

        GameObject moveCard = Instantiate(_thisCard, _fristVec.position, Quaternion.identity);
        _spawnedCards.Add(moveCard);

        _endRotate = new Vector3(0, 0, UnityEngine.Random.Range(90, 210));

        moveCard.transform.DOMove(endVec.position, _during);
        moveCard.transform.DORotate(_endRotate, _during - 1f);

        BlackJackCard cardCompo = moveCard.GetComponent<BlackJackCard>();

        CardInfo(cardCompo, id);
    }

    private void CardInfo(BlackJackCard compo, int id)
    {
        compo._myId = id;

        if (id == 0)
        {
            compo.gameObject.transform.parent = gameObject.transform;
        }

        Sprite sprite = CheckSprite();

        compo._myImage = sprite;
        compo._myNumber = CheckNum(sprite.name);

        if (id == 1)
        {
            _dealerSum += compo._myNumber;
        }
    }

    public Sprite CheckSprite()
    {
        int ran = UnityEngine.Random.Range(0, _copyCard.Count);

        Sprite sprite = _copyCard[ran];

        _copyCard.RemoveAt(ran);

        return sprite;
    }

    public int CheckNum(string name)
    {
        switch (name.Split("_")[0])
        {
            case "2":
                return 2;

            case "3":
                return 3;

            case "4":
                return 4;

            case "5":
                return 5;

            case "6":
                return 6;

            case "7":
                return 7;

            case "8":
                return 8;

            case "9":
                return 9;

            case "10":
                return 10;

            case "ace":
                return 1;

            case "jack":
            case "queen":
            case "king":
                return 10;
        }

        return 0;
    }

    #endregion
}