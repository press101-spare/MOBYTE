using TMPro;
using UnityEngine;

public class PinballManager : MonoBehaviour
{
    public static PinballManager Instance;

    [SerializeField] private Rigidbody2D _ball;
    [SerializeField] private Transform _startPoint;
    [SerializeField] private TextMeshProUGUI _resultText;

    private bool _isPlaying;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        ResetBall();
    }

    public void StartGame()
    {
        if (_isPlaying) return;
        _isPlaying = true;
        _resultText.text = "";
        _ball.transform.position = _startPoint.position;
        _ball.linearVelocity = Vector2.zero;
        _ball.angularVelocity = 0f;
        _ball.simulated = true;
    }

    public void FinishGame(float multiplier)
    {
        if (!_isPlaying) return;
        _isPlaying = false;
        _ball.simulated = false;
        _resultText.text = $"x{multiplier}";
        Debug.Log($"최종 배수 : x{multiplier}");
    }

    private void ResetBall()
    {
        _ball.transform.position = _startPoint.position;
        _ball.linearVelocity = Vector2.zero;
        _ball.angularVelocity = 0f;
        _ball.simulated = false;
    }
}