using UnityEngine;

public class PinballRewardZone : MonoBehaviour
{
    [SerializeField] private float _multiplier = 1f;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("PinballBall")) return;
        PinballManager.Instance.FinishGame(_multiplier);
    }
}