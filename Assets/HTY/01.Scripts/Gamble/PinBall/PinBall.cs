using TMPro;
using UnityEngine;

public class PinBall : MonoBehaviour
{
    [SerializeField] private GameObject _ball;
    [SerializeField] private Transform _point;
    [SerializeField] private TextMeshProUGUI _text;

    private Rigidbody2D _ballRb;

    private int _coinX;
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
        // 시작 버튼 연타 방지
        if (_isPlaying)
        {
            return;
        }

        _isPlaying = true;

        _ballRb.gravityScale = 1f;

        // 정지 상태였다면 Physics 다시 활성화
        _ballRb.WakeUp();
    }


    public void ResetPinBall()
    {
        _isPlaying = false;

        // 중력 정지
        _ballRb.gravityScale = 0f;

        // 기존 속도 제거
        _ballRb.linearVelocity = Vector2.zero;
        _ballRb.angularVelocity = 0f;

        // 시작 위치로 이동
        _ballRb.position = _point.position;

        // 회전값도 초기화
        _ballRb.rotation = 0f;

        // 결과 초기화
        _coinX = 0;
        _text.text = "0";

        // 물리 계산 잠시 정지
        _ballRb.Sleep();
    }


    public void GetScore(int value)
    {
        if (!_isPlaying)
        {
            return;
        }

        _isPlaying = false;

        _coinX = value;

        _text.text = _coinX.ToString();
        Debug.Log(value);
        GambleManager.instance.GambleEnd((float)value);

        ResetPinBall();

        transform.parent.gameObject.SetActive(false);
    }
}