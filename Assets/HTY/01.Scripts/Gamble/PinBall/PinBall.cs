using Unity.Mathematics;
using UnityEngine;

public class PinBall : MonoBehaviour
{
    [SerializeField] private GameObject _ball;
    public Transform _point;
    private void Start()
    {
        PinBallStart();
    }
    public void PinBallStart()
    {
        Instantiate(_ball,_point.position,Quaternion.identity);
    }
}
