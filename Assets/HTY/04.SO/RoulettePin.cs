using UnityEngine;

public class RoulettePin : MonoBehaviour
{
    private void OnCollisionEnter2D(Collision2D collision)
    {
        AudioManager.Instance.PlayClipSFX("SFX_RoulPin");
    }
}
