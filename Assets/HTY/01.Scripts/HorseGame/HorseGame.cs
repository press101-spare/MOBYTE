using DG.Tweening;
using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class HorseGame : MonoBehaviour
{
    public float _ranMove;
    private Sequence _sequence;
    private bool _isPlaying;
    private void Update()
    {
        if(Keyboard.current.aKey.wasPressedThisFrame)
        {
            _isPlaying = true;
            HorseGameStart();
        }
        if (Keyboard.current.tKey.wasPressedThisFrame)
        {
            _isPlaying = false;
        }


    }

    private void HorseGameStart()
    {
        _sequence = DOTween.Sequence();
        while(_isPlaying)
        {
            _sequence.Append(transform.DOMoveX(_ranMove,1f));
            _sequence.AppendCallback(() => { _ranMove = UnityEngine.Random.Range(1f, 5f); } );
        }
    }
}
