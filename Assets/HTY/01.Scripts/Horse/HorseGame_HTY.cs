using DG.Tweening;
using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class HorseGame_HTY : MonoBehaviour
{
    private bool _notColl;
    public HorseEnd _End;
     
    public void Start()
    {
        StartHorse();
    }
    Sequence sequence;
    private void StartHorse()
    {
        sequence = DOTween.Sequence();
        Debug.Log(_End.End());
        while (!_End.End())
        {
            Vector2 a = new Vector2(transform.position.x+ UnityEngine.Random.Range(-1.5f, 1.5f), transform.position.y);
            sequence.Append(transform.DOMove(a, 2));
            sequence.AppendCallback(()=> sequence.AppendInterval(1.5f));
            
        }
    }
}
