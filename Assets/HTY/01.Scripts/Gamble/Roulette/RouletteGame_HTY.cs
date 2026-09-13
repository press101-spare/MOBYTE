using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class RouletteGame_HTY : MonoBehaviour
{
    [SerializeField] private float _speed;
    [SerializeField] private bool _isSpin = false;

    private void Update()
    {
        if(Keyboard.current.aKey.wasPressedThisFrame)
        {
            _isSpin = !_isSpin;
            _speed = Random.Range(8,14);
            if(_isSpin)
            {
                StartCoroutine(Spin());
            }
        }
    }
    private IEnumerator Spin()
    {
        while(_isSpin)
        {
            yield return null;
            gameObject.transform.Rotate(0, 0, _speed);
            _speed -= 0.01f;
            if(_speed<0)
            {
                _isSpin=false;
            }
            yield return null;
        }
        
    }
}
