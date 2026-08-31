using DG.Tweening;
using JetBrains.Annotations;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Threading.Tasks;
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
    public int _notMorePlayer;
    [Header("UI오브젝트")]
    public Slider _timer;
    public GameObject _checkButton;
    public GameObject _moreButton;
    public TextMeshProUGUI _endingText;
    public TextMeshProUGUI _titleText;
    [Header("카드덱")]
    public List<Sprite> _originCard = new List<Sprite>();//원본
    public List<Sprite> _copyCard = new List<Sprite>();//쓸거
    [Header("연출용")]
    [SerializeField] private Transform _fristVec;
    [SerializeField] private Transform _endVecPlayer;
    [SerializeField] private Transform _endVecDealer;
    [SerializeField] private Vector3 _endRotate;
    public float _during = 2f;
    public GameObject _thisCard;//프리팹








    private async Task BlackJackGameManager()
    {
        StartCoroutine(StartBlackjack());
        await CheckPlayerCard();
        WaitPlayer();
    }
    public void WaitPlayer()
    {
        _moreButton.SetActive(true);
        _titleText.text = "Drow more Card?";
        _timer.value = 1;
        _timer.gameObject.SetActive(true);

        while(_timer.value>0)
        {
            _timer.value -= 0.01f;
            if(_morePlay)
            {
                _timer.value = 1;
                _morePlay = false;
                _moreButton.SetActive(false);
                _timer.gameObject.SetActive(false);
                _titleText.text = "";
               
                NewCard(0);
            }
        }


    }
    private void Update()
    {
        if(_notMorePlayer>=2)
        {
            Ending();
        }
        if (Keyboard.current.yKey.wasPressedThisFrame)
        {
            
        }
    }
    
    public void CheckCardKey()
    {
        if(Keyboard.current.pKey.wasPressedThisFrame)
        {
            OpenCard();
            WhoWinandLose();
        }
    }


    public async Task CheckPlayerCard()
    {
        await CheckPlayerCard();
        if(Keyboard.current.iKey.wasPressedThisFrame)
        {
            OpenCard();
        }
    }

    private IEnumerator StartBlackjack()//시작 세팅
    {
        NewCard(0);
        yield return new WaitForSeconds(1.2f);
        NewCard(1);
        yield return new WaitForSeconds(1.2f);
        NewCard(0);
        yield return new WaitForSeconds(1.2f);
        NewCard(1);
        yield return new WaitForSeconds(3f);


    }
    public IEnumerator MoreGame()
    {
        yield return new WaitForSeconds(5f);
        


        while(_notMorePlayer <2)
        {
            while (_timer.value > 0|| !_morePlay)//플레이어의 moreTurn
            {
                _timer.value -= 0.01f;
                yield return new WaitForSeconds(0.01f);

                if (_morePlay)
                {
                    NewCard(0);
                    yield return new WaitForSeconds(1.5f);
                    _titleText.text = "dealerTurn";
                    yield return null;
                    break;
                }
            }



            if (!_morePlay)
            {
                if (_notMorePlayer == 1) _notMorePlayer = 2;
                else if (_notMorePlayer == 0) _notMorePlayer = 1;
            }
            else
            {
                _morePlay = false;
            }
            yield return new WaitForSeconds(1.5f);
            if (_dealerSum <= 16)
            {
                NewCard(1);
                yield return new WaitForSeconds(1.5f);
            }
            else
            {
                if (_notMorePlayer == 1) _notMorePlayer = 2;
                else if(_notMorePlayer==0)_notMorePlayer = 1;

            }
        }
        
    }

    public void TurnSelet()
    {
        _morePlay = true;
    }

    public void Ending()
    {
        Debug.Log("임시 테스트 끝");
    }

    public void WhoWinandLose()
    {
        if (_playerSum == 21 || _dealerSum == 21)
        {
            Ending();
        }
        else if (_playerSum > 21 || _dealerSum > 21)
        {
            Ending();
        }
    }


    public void OpenCard()
    {
        BlackJackCard[] a = transform.GetComponentsInChildren<BlackJackCard>();

        
        for (int i = 0; i < a.Length; i++)
        {
            if (a[i]._myId==0)
            {
                a[i].OpenCard();
            }
        }
    }
    #region 카드전용
    public void NewCard(int id)
    {
        Transform _endVec;
        if(id==0)
        {
            _endVec = _endVecPlayer;
        }
        else
        {
            _endVec = _endVecDealer;
        }
        GameObject moveCard = Instantiate(_thisCard, _fristVec.position, Quaternion.identity);//카드를 생성
        _endRotate = new Vector3(0, 0, UnityEngine.Random.Range(90, 210));//랜덤한 회전값
        moveCard.transform.parent = gameObject.transform;//부모를 지정
        moveCard.transform.DOMove(_endVec.position, _during);//목표된 위치까지 이동
        moveCard.transform.DORotate(_endRotate, _during - 1f);//랜덤한 각도로 회전함


        //각각의 카드들의 정보를 넣음
        BlackJackCard cardCompo = moveCard.GetComponent<BlackJackCard>();
        CardInfo(cardCompo, id);
    }//카드 드로우&세팅
    private void CardInfo(BlackJackCard Compo, int id)
    {
        Compo._myId = id;
        Sprite sprite = CheckSprite();
        Compo._myImage = sprite;
        Compo._myNumber = CheckNum(sprite.name);
        if(id==1) _dealerSum += CheckNum(sprite.name);
    }//카드 정보 입력 
    public Sprite CheckSprite()
    {
        int ran = UnityEngine.Random.Range(0, _copyCard.Count);
        Sprite sprite = _copyCard[ran];
        _copyCard.Remove(_copyCard[ran]);

        return sprite;
    }//스프라이트 정보값 결정
    public int CheckNum(string name)
    {
        switch (name.Split("_")[0])
        {
            case "2":
                {
                    return 2;
                }
            case "3":
                {
                    return 3;
                }
            case "4":
                {
                    return 4;
                }
            case "5":
                {
                    return 5;
                }
            case "6":
                {
                    return 6;
                }
            case "7":
                {
                    return 7;
                }
            case "8":
                {
                    return 8;
                }
            case "9":
                {
                    return 9;
                }
            case "ace":
                {
                    return 1;
                }
            case "jack":
            case "king":
            case "queen":
                {
                    return 10;
                }
        }
        return 0;
    }//카드 숫자 체크
    #endregion 
}
