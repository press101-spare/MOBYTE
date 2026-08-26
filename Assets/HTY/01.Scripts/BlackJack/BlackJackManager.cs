using DG.Tweening;
using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;

public class BlackJackManager : MonoBehaviour
{
    [Header("게임진행")]
    public int _playerSum;
    public int _dealerSum;

    [Header("UI오브젝트")]
    public GameObject _checkButton;
    public GameObject _moreButton;
    public TextMeshProUGUI _endingText;
    public TextMeshProUGUI _titleText;
    public Slider _timer;
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

    private void Update()
    {
        if (Keyboard.current.yKey.wasPressedThisFrame)
        {
            BlackJackGameManager();
        }
    }

    private void BlackJackGameManager()
    {
        StartCoroutine(BlackjackGame());
        StartCoroutine(MoreGame());


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

        
    }

    public IEnumerator MoreGame()
    {
        _moreButton.SetActive(true);
        _titleText.text = "Drow more Card?";
        _timer.value = 1;
        _timer.SetEnabled(true);
        while (true)
        {
            _timer.value -= 0.05f;
            yield return new WaitForSeconds(0.2f);
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
        _endRotate = new Vector3(0, 0, Random.Range(90, 210));//랜덤한 회전값
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
        int ran = Random.Range(0, _copyCard.Count);
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
