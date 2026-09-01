using UnityEngine;

[CreateAssetMenu(fileName = "AudioClipDataSO", menuName = "Scriptable Objects/AudioClipDataSO")]
public class AudioClipDataSO : ScriptableObject
{
    [field: SerializeField] public string clipName { get; private set; }
    [field: SerializeField] public AudioClip clip;

    private int? _clipHash;

    public int ClipHash
    {
        get
        {
            if (_clipHash == null)
            {
                _clipHash = Animator.StringToHash(clipName);
            }
            return _clipHash.Value;
        }
    }
}