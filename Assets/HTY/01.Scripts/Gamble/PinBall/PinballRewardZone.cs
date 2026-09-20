using UnityEngine;

public class PinballRewardZone : MonoBehaviour
{
    [SerializeField] private int _multiplier = 1;
    [SerializeField] private PinBall _pinBall;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("PinballBall")) return;
        _pinBall.GetScore(_multiplier);
    }
}