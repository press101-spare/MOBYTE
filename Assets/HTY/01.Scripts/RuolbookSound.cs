using UnityEngine;

public class RuolbookSound : MonoBehaviour
{
    private void OnEnable()
    {
        AudioManager.Instance.PlayClipSFX("SFX_BOOK");
    }
    private void OnDisable()
    {
        AudioManager.Instance.PlayClipSFX("SFX_BOOK");
    }
}
