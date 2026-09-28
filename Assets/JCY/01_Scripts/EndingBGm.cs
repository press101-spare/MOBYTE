using UnityEngine;

public class EndingBGm : MonoBehaviour
{
    void Start()
    {
        AudioManager.Instance.PlayClipSFX("BGM_Map1");
    }
}
