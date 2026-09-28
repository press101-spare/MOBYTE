using UnityEngine;

public class PinBallPin : MonoBehaviour
{
    private void OnCollisionEnter2D(Collision2D collision)
    {
        AudioManager.Instance.PlayClipSFX("SFX_Ma");
    }
}
