using DG.Tweening;
using System.Collections;
using UnityEngine;

public class MoveCup : MonoBehaviour
{
    [SerializeField] private Transform[] _cupPoint;
    private int _randomMove;
    public int _maxMoveCount=5;

    private Transform _oneTrans;//서로 바꿀 위치
    private Transform _twoTrans;//서로 바꿀 위치
    private Transform _temTrans;//임시 저장

    private Transform _winPoint;//최종으로 고른 곳의 위치가 이것과 같다면 승리


    public float _during = 3f;


    private void Start()
    {

        StartCoroutine(MovePoint());
    }

    public IEnumerator MovePoint()
    {
        _randomMove = Random.Range(1, _maxMoveCount + 1);
        for (int index=0; index < _randomMove;index++)
        {
            _oneTrans = _cupPoint[0];
            _twoTrans = _cupPoint[0];
            _temTrans = _cupPoint[0];
            while(_oneTrans ==_twoTrans)
            {
                _oneTrans = _cupPoint[Random.Range(0, _cupPoint.Length)];
                _twoTrans = _cupPoint[Random.Range(0, _cupPoint.Length)];
            }
            _temTrans = _oneTrans;
            _oneTrans.DOMove(_twoTrans.position, _during);
            _twoTrans.DOMove(_temTrans.position, _during);
            yield return new WaitForSeconds(_during+1);
            yield return new WaitForSeconds(1.5f);
        }
    }

}
