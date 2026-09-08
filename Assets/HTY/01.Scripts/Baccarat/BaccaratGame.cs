using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class BaccaratGame : MonoBehaviour
{
    public enum Batting {Player,Backer,Tie}
    public Batting _whoBatting;
    public void BattingBT(int a)
    {
        if (a == 0) _whoBatting = Batting.Player;
        else if (a == 1) _whoBatting = Batting.Backer;
        else if (a == 2) _whoBatting = Batting.Tie;
    }
    public Transform[] _settingPoint;


    [Header("게임진행")]
    public int _playerSum;
    public int _dealerSum;
    public bool _morePlay = false;
    public bool _notMorePlayer;
    public bool _notMoreDealer;
    public bool _checkCard;


    [Header("UI오브젝트")]
    public Slider _timer;
    
    public GameObject _checkButton;
    public TextMeshProUGUI _endingText;

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




    public int _thardCard;






    private void Update()
    {
        if(Keyboard.current.yKey.wasPressedThisFrame)
        {
            StartCoroutine(BlackjackGame());
        }
    }

    private IEnumerator BlackjackGame()
    {
        //리셋 만들기
        NewCard(0);
        yield return new WaitForSeconds(1.2f);

        NewCard(1);
        yield return new WaitForSeconds(1.2f);

        NewCard(0);
        yield return new WaitForSeconds(1.2f);

        NewCard(1);
        yield return new WaitForSeconds(3f);

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
            yield return new WaitForSeconds(5);
            BankerMore();
            yield return new WaitForSeconds(5);

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
                        if(_thardCard!=8) NewCard(1); break;
                    case 4:
                        if(_thardCard>=2&&_thardCard<=7) NewCard(1); break;
                    case 5:
                        if (_thardCard >= 4 && _thardCard <= 7) NewCard(1); break;
                    case 6:
                        if (_thardCard >= 6 && _thardCard <= 7) NewCard(1); break;
                    case 7:
                        break;
                        

                }
            }
        }
    }

    private void EndingGame()
    {
        Debug.Log($"EndingGame 실행 / Player: {_playerSum}, Banker: {_dealerSum}");

        if (_playerSum > _dealerSum)
        {
            _endingText.text = "플레이어의 승";
        }
        else if (_playerSum < _dealerSum)
        {
            _endingText.text = "밴커의 승";
        }
        else
        {
            _endingText.text = "타이 판정 무승부";
        }
    }

    public void Sum(int id,int a)
    {
        Debug.Log($"{id} {a}");
        if(id==0)
        {
            _playerSum = (_playerSum + a) % 10;
        }
        else if(id==1)
        {
            _dealerSum = (_dealerSum + a) % 10;
        }
    }
    public void OpenCard()
    {

    }
    

    public void BattingChip()
    {
        //연동할때 만들어진 스크립트 쓰기
    }
    public bool CheckNature()
    {
        if(_playerSum >=8)
        {
            return true;
        }
        if(_dealerSum >=8)
        {
            return true;
        }
        return false;
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

        GameObject moveCard = Instantiate(_thisCard, _fristVec.position, Quaternion.identity);

        _endRotate = new Vector3(0, 0, UnityEngine.Random.Range(90, 210));

        moveCard.transform.DOMove(endVec.position, _during);
        moveCard.transform.DORotate(_endRotate, _during - 1f);

        BlackJackCard cardCompo = moveCard.GetComponent<BlackJackCard>();

        CardInfo(cardCompo, id);
    }

    private void CardInfo(BlackJackCard compo, int id)
    {
        compo._myId = id;

        Sprite sprite = CheckSprite();

        compo._myImage = sprite;
        compo._myNumber = CheckNum(sprite.name);

        Sum(id,compo._myNumber);
        if(id==0)
        {
            _thardCard = compo._myNumber;
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
