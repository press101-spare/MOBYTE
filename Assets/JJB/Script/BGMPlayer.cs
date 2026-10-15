using UnityEngine;

public class BGMPlayer : MonoBehaviour
{
    [SerializeField] AudioClipDataSO audioClipDataSO;

    private void Start()
    {
        AudioManager.Instance.PlayBGM(audioClipDataSO.clipName);
    }
}
