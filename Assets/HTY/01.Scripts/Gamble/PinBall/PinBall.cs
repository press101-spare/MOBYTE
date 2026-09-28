using System.Collections;
using TMPro;
using UnityEngine;

public class PinBall : MonoBehaviour
{
    public enum Difficulty
    {
        Easy,
        Normal,
        Hard
    }

    [Header("Reference")]
    [SerializeField] private GameObject _ball;
    [SerializeField] private Transform _point;
    [SerializeField] private TextMeshProUGUI _text;

    [Header("Difficulty")]
    [SerializeField] private Difficulty _difficulty = Difficulty.Normal;

    [Header("Random Setting")]
    [SerializeField] private float _easySpawnRange = 0.08f;
    [SerializeField] private float _normalSpawnRange = 0.18f;
    [SerializeField] private float _hardSpawnRange = 0.30f;

    [SerializeField] private float _easyHorizontalSpeed = 0.15f;
    [SerializeField] private float _normalHorizontalSpeed = 0.35f;
    [SerializeField] private float _hardHorizontalSpeed = 0.6f;

    [SerializeField] private float _rotationSpeed = 80f;

    private Rigidbody2D _ballRb;

    private float _coinX;
    private bool _isPlaying;


    private void Awake()
    {
        _ballRb = _ball.GetComponent<Rigidbody2D>();
    }


    private void OnEnable()
    {
        ResetPinBall();
    }


    public void PinBallStart()
    {
        if (_isPlaying)
        {
            return;
        }

        _isPlaying = true;

        // 난이도에 따라서 랜덤 범위 가져오기
        float spawnRange = GetSpawnRange();
        float horizontalSpeed = GetHorizontalSpeed();

        // 시작 위치를 좌우로 조금 랜덤하게 변경
        Vector2 startPosition = _point.position;

        startPosition.x += Random.Range(
            -spawnRange,
            spawnRange
        );

        _ballRb.position = startPosition;

        // 중력 시작
        _ballRb.gravityScale = 1f;

        // 좌우 시작 속도 추가
        float randomXSpeed = Random.Range(
            -horizontalSpeed,
            horizontalSpeed
        );

        _ballRb.linearVelocity = new Vector2(
            randomXSpeed,
            0f
        );

        // 회전도 랜덤
        _ballRb.angularVelocity = Random.Range(
            -_rotationSpeed,
            _rotationSpeed
        );

        _ballRb.WakeUp();
    }


    private float GetSpawnRange()
    {
        switch (_difficulty)
        {
            case Difficulty.Easy:
                return _easySpawnRange;

            case Difficulty.Normal:
                return _normalSpawnRange;

            case Difficulty.Hard:
                return _hardSpawnRange;
        }

        return _normalSpawnRange;
    }


    private float GetHorizontalSpeed()
    {
        switch (_difficulty)
        {
            case Difficulty.Easy:
                return _easyHorizontalSpeed;

            case Difficulty.Normal:
                return _normalHorizontalSpeed;

            case Difficulty.Hard:
                return _hardHorizontalSpeed;
        }

        return _normalHorizontalSpeed;
    }


    public void ResetPinBall()
    {
        _isPlaying = false;

        _ballRb.gravityScale = 0f;

        _ballRb.linearVelocity = Vector2.zero;
        _ballRb.angularVelocity = 0f;

        _ballRb.position = _point.position;
        _ballRb.rotation = 0f;

        _coinX = 0f;
        _text.text = "0";

        _ballRb.Sleep();
    }


    public void GetScore(float value)
    {
        if (!_isPlaying)
        {
            return;
        }

        _isPlaying = false;

        _coinX = value;

        _text.text = _coinX.ToString();

        Debug.Log($"PinBall Result : {value}X");

        StartCoroutine(Col(value));
    }


    private IEnumerator Col(float value)
    {
        GambleManager.instance._successEffect.PlayEffect($"{value}배!");

        yield return new WaitForSeconds(5f);

        GambleManager.instance.GambleEnd(value);

        ResetPinBall();

        transform.parent.gameObject.SetActive(false);
    }
}