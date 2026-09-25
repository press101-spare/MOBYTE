using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class RouletteGame_HTY : MonoBehaviour
{
    [SerializeField] private float _speed;
    [SerializeField] private bool _canSpin = false;

    public void StartGamble()
    {
        _canSpin = false;
        _speed = Random.Range(8, 14);
        StartCoroutine(Spin());
    }
    private IEnumerator Spin()
    {
        float _disSpeed = 0.01f;
        while(_canSpin)
        {
            yield return null;
            gameObject.transform.Rotate(0, 0, _speed);
            _speed -= _disSpeed;
            _disSpeed += 0.001f;
            if(_speed<0)
            {
                _canSpin=false;
            }
            yield return null;
        }
        gameObject.transform.parent.parent.gameObject.SetActive(false);
        GambleManager.instance.GambleEnd(2);
    }
}
