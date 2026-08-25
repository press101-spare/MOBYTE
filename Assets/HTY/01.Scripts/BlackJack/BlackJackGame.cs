using DG.Tweening;
using System.Net.NetworkInformation;
using UnityEngine;
using UnityEngine.InputSystem;

public class BlackJackGame : MonoBehaviour
{
    public int _playerSum;
    public int _dealerSum;
    public Sprite[] Card1; //하트 
    public Sprite[] Card2; //다이아
    public Sprite[] Card3; //스페이드
    public Sprite[] Card4; //클로버

    [SerializeField] private Transform _fristVec;
    [SerializeField] private Transform _endVec;
    [SerializeField] private Vector3 _endRotate;
    public float _during = 2f;
    public GameObject _thisCard;

    private void Update()
    {
        if(Keyboard.current.yKey.wasPressedThisFrame)
        {
            BlackJack();
        }
    }
    public void BlackJack()
    {
        NewCard();
    }
    public void NewCard()
    {
        _endRotate = new Vector3(0,0,Random.Range(90,210));
        GameObject cardVi = Instantiate(_thisCard,_fristVec.position,Quaternion.identity);
        cardVi.transform.parent = gameObject.transform;
        cardVi.transform.DOMove(_endVec.position, _during);
        cardVi.transform.DORotate(_endRotate, _during-1f);
    }

    public Sprite CardSprite()
    {
        return _thisCard.GetComponent<Sprite>();
    }
}
