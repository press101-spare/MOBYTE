using System.Collections;
using UnityEngine;

public class RouletteGame_HTY : MonoBehaviour
{
    [SerializeField] private RoulettePointer _pointer;
    [SerializeField] private float _minSpeed = 500f;
    [SerializeField] private float _maxSpeed = 800f;
    [SerializeField] private float _minDeceleration = 70f;
    [SerializeField] private float _maxDeceleration = 110f;

    public GameObject _startBT;

    private float _speed;
    private bool _isSpinning;

    private void OnEnable()
    {
        ResetRoulette();
    }

    public void StartGamble()
    {
        if (_isSpinning) return;
        StartCoroutine(Spin());
    }

    private IEnumerator Spin()
    {
        _isSpinning = true;

        _speed = Random.Range(_minSpeed, _maxSpeed);
        float deceleration = Random.Range(_minDeceleration, _maxDeceleration);

        while (_speed > 0f)
        {
            transform.Rotate(0f, 0f, _speed * Time.deltaTime);

            _speed -= deceleration * Time.deltaTime;

            if (_speed < 0f)
                _speed = 0f;

            yield return null;
        }

        yield return new WaitForFixedUpdate();

        RouletteSlot resultSlot = _pointer.CurrentSlot;

        if (resultSlot == null)
        {
            Debug.LogWarning("룰렛 결과 Collider를 찾지 못했습니다.");
            _isSpinning = false;
            yield break;
        }

        float coin = resultSlot._result;

        Debug.Log($"룰렛 결과 배수 : {coin}");

        GambleManager.instance.GambleEnd(coin);

        yield return new WaitForSeconds(2f);

        _isSpinning = false;

        transform.parent.parent.gameObject.SetActive(false);
    }

    public void ResetRoulette()
    {
        StopAllCoroutines();
        _speed = 0f;
        _isSpinning = false;
        transform.localRotation = Quaternion.identity;
        _startBT.SetActive(true);
    }
}