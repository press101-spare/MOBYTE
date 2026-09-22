using TMPro;
using Unity.Mathematics;
using UnityEngine;

public class PinBall : MonoBehaviour
{
    [SerializeField] private GameObject _ball;
    public Transform _point;
    [SerializeField] private TextMeshProUGUI _text;
    private int _coinX;

    private void OnEnable()
    {
        _ball.transform.position = _point.position;
        _ball.GetComponent<Rigidbody2D>().gravityScale = 0;
    }

    public void PinBallStart()
    {
        _ball.GetComponent<Rigidbody2D>().gravityScale =1;
    }

    public void GetScore(int value)
    {
        _coinX = value;
        _text.text = _coinX.ToString();
        GambleManager.instance.GambleEnd(value);
        gameObject.transform.parent.gameObject.SetActive(false);
        
    }
}
