using System;
using System.Collections.Generic;
using System.Xml.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AudioManager : MonoBehaviour
{
    [SerializeField] private List<AudioClipDataSO> clipData;
    [SerializeField] private AudioSource sfxSource;
    [SerializeField] private AudioSource bgmSource;

    public static bool IsNullInstacnce => Instance != null;
    public static AudioManager Instance;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(this);
    }

    private void Start()
    {
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;

        switch (currentSceneIndex)
        {
            case 0:
                PlayBGM(SoundStringContainer.BGM_MAP_1);
                break;

            case 1:
                PlayBGM(SoundStringContainer.BGM_MAP_2);
                break;

            case 2:
                PlayBGM(SoundStringContainer.BGM_MAP_3);
                break;

            case 3:
                PlayBGM(SoundStringContainer.BGM_MAP_2);
                break;
        }
    }

    public void PlayClipSFX(string sfxName)
    {
        int inputHash = Animator.StringToHash(sfxName);
        foreach (AudioClipDataSO clipData in clipData)
        {
            if (clipData.ClipHash == inputHash)
            {
                sfxSource.PlayOneShot(clipData.clip);
                return;
            }

        }
    }

    public void PlayBGM(string bgmName)
    {

        int inputHash = Animator.StringToHash(bgmName);

        foreach (AudioClipDataSO clip in clipData)
        {
            if (clip.ClipHash == inputHash)
            {
                bgmSource.clip = clip.clip;
                bgmSource.Play();
                return;
            }

        }
    }

    public void StopBGM()
    {
        bgmSource.Stop();
    }
}