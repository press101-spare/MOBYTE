using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static System.Net.Mime.MediaTypeNames;

public class BaccaratGame : MonoBehaviour
{
    public enum Batting { Player, Backer, Tie }

    public Batting _whoBatting;
    public Batting _winer;

    [SerializeField] private Outline[] _battingOutlines;
    public Transform[] _settingPoint;

    [Header("게임진행")]
    public int _playerSum;
    public int _dealerSum;
    public bool _morePlay = false;
    public bool _notMorePlayer;
    public bool _notMoreDealer;
    public bool _checkCard;

    [Header("UI오브젝트")]
    public TextMeshProUGUI _endingText;

    [Header("카드 확인 UI")]
    [SerializeField] private Transform _playerCardGroup;
    [SerializeField] private Transform _bankerCardGroup;
    [SerializeField] private GameObject _cardImagePrefab;
    [SerializeField] private float _endingDelay = 10f;
    private List<GameObject> _spawnedCardUI = new List<GameObject>();
    private bool _cardsRevealed;

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
    [SerializeField] private TextMeshProUGUI _bettingText;

    public int _thardCard;

    private int _playerCardCount;
    private bool _isPlaying;
    private List<GameObject> _spawnCards = new List<GameObject>();

    public GameObject _btBetting;

    private void OnEnable()
    {
        _btBetting.SetActive(true);
    }

    public void BattingBT(int a)
    {
        if (_isPlaying) return;

        if (a == 0) _whoBatting = Batting.Player;
        else if (a == 1) _whoBatting = Batting.Backer;
        else if (a == 2) _whoBatting = Batting.Tie;
        else return;

        for (int i = 0; i < _battingOutlines.Length; i++)
        {
            _battingOutlines[i].enabled = i == a;
        }

        _bettingText.text = $"현재 베팅:{_whoBatting}";

    }

    public void StartGamble()
    {
        if (_isPlaying) return;

        _isPlaying = true;
        _btBetting.SetActive(false);
        StartCoroutine(BlackjackGame());
    }

    private IEnumerator BlackjackGame()
    {
        NewCard(0);
        yield return new WaitForSeconds(1.2f);

        NewCard(1);
        yield return new WaitForSeconds(1.2f);

        NewCard(0);
        yield return new WaitForSeconds(1.2f);

        NewCard(1);
        yield return new WaitForSeconds(3f);

        // 처음 4장 모두 UI에 공개
        _cardsRevealed = true;
        ShowAllCards();

        Debug.Log(CheckNature());

        if (CheckNature())
        {
            EndingGame();
        }
        else
        {
            if (_playerSum <= 5)
            {
                NewCard(0);
                _notMorePlayer = false;
            }
            else
            {
                _notMorePlayer = true;
            }

            yield return new WaitForSeconds(5f);

            BankerMore();

            yield return new WaitForSeconds(5f);

            EndingGame();
        }
    }

    public void BankerMore()
    {
        if (_notMorePlayer)
        {
            if (_dealerSum <= 5)
            {
                NewCard(1);
            }
        }
        else
        {
            if (_dealerSum <= 2)
            {
                NewCard(1);
            }
            else
            {
                switch (_dealerSum)
                {
                    case 3:
                        if (_thardCard != 8) NewCard(1);
                        break;
                    case 4:
                        if (_thardCard >= 2 && _thardCard <= 7) NewCard(1);
                        break;
                    case 5:
                        if (_thardCard >= 4 && _thardCard <= 7) NewCard(1);
                        break;
                    case 6:
                        if (_thardCard >= 6 && _thardCard <= 7) NewCard(1);
                        break;
                    case 7:
                        break;
                }
            }
        }
    }

    private void EndingGame()
    {
        if (!_isPlaying) return;

        Debug.Log($"EndingGame 실행 / Player: {_playerSum}, Banker: {_dealerSum}");

        if (_playerSum > _dealerSum)
        {
            _winer = Batting.Player;
        }
        else if (_playerSum < _dealerSum)
        {
            _winer = Batting.Backer;
        }
        else
        {
            _winer = Batting.Tie;
        }

        bool isWin = _whoBatting == _winer;

        string winnerText = "";

        if (_winer == Batting.Player)
        {
            winnerText = "플레이어 승";
        }
        else if (_winer == Batting.Backer)
        {
            winnerText = "뱅커 승";
        }
        else
        {
            winnerText = "타이";
        }

        string bettingText = isWin ? "베팅 성공!" : "베팅 실패!";

        GambleManager.instance._successEffect.PlayEffect(bettingText = isWin ? "베팅 성공!" : "베팅 실패!");

        _endingText.text =
            winnerText +
            "\n" +
            bettingText +
            "\n\nPlayer : " + _playerSum +
            "\nBanker : " + _dealerSum;

        StartCoroutine(EndingDelay(isWin));
    }

    private IEnumerator EndingDelay(bool isWin)
    {
        yield return new WaitForSeconds(_endingDelay);
        ResetGame();
        if (isWin)
        {
            GambleManager.instance.GambleEnd(2);
        }
        else
        {
            GambleManager.instance.GambleEnd(0);
        }

        _isPlaying = false;
        gameObject.transform.parent.gameObject.SetActive(false);
    }

    public void Sum(int id, int a)
    {
        Debug.Log($"{id} {a}");

        if (id == 0)
        {
            _playerSum = (_playerSum + a) % 10;
        }
        else if (id == 1)
        {
            _dealerSum = (_dealerSum + a) % 10;
        }
    }

    public bool CheckNature()
    {
        if (_playerSum >= 8) return true;
        if (_dealerSum >= 8) return true;

        return false;
    }

    public void ResetGame()
    {
        _playerSum = 0;
        _dealerSum = 0;
        _morePlay = false;
        _notMorePlayer = false;
        _notMoreDealer = false;
        _checkCard = false;
        _thardCard = 0;
        _playerCardCount = 0;
        _isPlaying = false;
        _cardsRevealed = false;

        _whoBatting = Batting.Player;
        _winer = Batting.Player;

        _endingText.text = "";

        _copyCard.Clear();
        _copyCard.AddRange(_originCard);

        for (int i = 0; i < _spawnCards.Count; i++)
        {
            if (_spawnCards[i] != null)
            {
                Destroy(_spawnCards[i]);
            }
        }

        _spawnCards.Clear();

        for (int i = 0; i < _spawnedCardUI.Count; i++)
        {
            if (_spawnedCardUI[i] != null)
            {
                Destroy(_spawnedCardUI[i]);
            }
        }

        _spawnedCardUI.Clear();

        for (int i = 0; i < _battingOutlines.Length; i++)
        {
            _battingOutlines[i].enabled = i == 0;
        }
    }

    private void ShowAllCards()
    {
        for (int i = 0; i < _spawnCards.Count; i++)
        {
            if (_spawnCards[i] == null) continue;

            BlackJackCard card = _spawnCards[i].GetComponent<BlackJackCard>();
            AudioManager.Instance.PlayClipSFX("SFX_CARD");

            if (card == null) continue;

            if (card._myId == 0)
            {
                CreateCardImage(card, _playerCardGroup);
            }
            else
            {
                CreateCardImage(card, _bankerCardGroup);
            }
        }
    }

    private void CreateCardImage(BlackJackCard card, Transform group)
    {
        GameObject cardUI = Instantiate(_cardImagePrefab, group);

        UnityEngine.UI.Image image = cardUI.GetComponent<UnityEngine.UI.Image>();

        if (image != null)
        {
            image.sprite = card._myImage;
        }

        cardUI.SetActive(true);
        _spawnedCardUI.Add(cardUI);
    }

    #region 카드전용

    public void NewCard(int id)
    {
        Transform endVec;

        if (id == 0)
        {
            endVec = _endVecPlayer;
        }
        else
        {
            endVec = _endVecDealer;
        }

        GameObject moveCard = Instantiate(
            _thisCard,
            _fristVec.position,
            Quaternion.identity
        );

        _spawnCards.Add(moveCard);

        _endRotate = new Vector3(
            0,
            0,
            UnityEngine.Random.Range(90, 210)
        );

        moveCard.transform.DOMove(
            endVec.position,
            _during
        );

        moveCard.transform.DORotate(
            _endRotate,
            _during - 1f
        );
        AudioManager.Instance.PlayClipSFX("SFX_CARD");
        BlackJackCard cardCompo =
            moveCard.GetComponent<BlackJackCard>();

        CardInfo(cardCompo, id);

        // 최초 공개 이후 추가된 3번째 카드는 바로 UI에 추가
        if (_cardsRevealed)
        {
            if (id == 0)
            {
                CreateCardImage(cardCompo, _playerCardGroup);
            }
            else
            {
                CreateCardImage(cardCompo, _bankerCardGroup);
            }
        }
    }

    private void CardInfo(BlackJackCard compo, int id)
    {
        compo._myId = id;

        Sprite sprite = CheckSprite();

        compo._myImage = sprite;
        compo._myNumber = CheckNum(sprite.name);

        Sum(id, compo._myNumber);

        if (id == 0)
        {
            _playerCardCount++;

            if (_playerCardCount == 3)
            {
                _thardCard = compo._myNumber;
            }
        }
    }

    public Sprite CheckSprite()
    {
        int ran = UnityEngine.Random.Range(
            0,
            _copyCard.Count
        );

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
            case "ace":
                return 1;
            case "10":
            case "jack":
            case "queen":
            case "king":
                return 0;
        }

        return 0;
    }

    #endregion
}