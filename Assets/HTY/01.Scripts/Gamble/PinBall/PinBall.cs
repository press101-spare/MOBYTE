using Unity.Mathematics;
using UnityEngine;

public class PinBall : MonoBehaviour
{
    [SerializeField] private GameObject _ball;
    public Transform _point;
    private int _coinX;

    private void Start()
    {
        PinBallStart();
    }
    public void PinBallStart()
    {
        Instantiate(_ball,_point.position,Quaternion.identity);
    }

    public void GetScore(int value)
    {
        _coinX = value;
    }
}
