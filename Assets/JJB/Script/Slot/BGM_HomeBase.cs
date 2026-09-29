using UnityEngine;

public class BGM_HomeBase : MonoBehaviour
{
    private void Start()
    {
        AudioManager.Instance.PlayBGM("BGM_Map1");
    }
}
