using UnityEngine;
using UnityEngine.Audio;

namespace JJB.Script
{
    public class AudioMixerControls : MonoBehaviour
    {
        [SerializeField] private AudioMixer audioMixer;

        private const string MasterKey = "MasterVolume";
        private const string BGMKey = "BGMVolume";
        private const string SFXKey = "SFXVolume";

        private void Start()
        {
            float masterVolume = PlayerPrefs.GetFloat(MasterKey, 0.3f);
            float bgmVolume = PlayerPrefs.GetFloat(BGMKey, 0.3f);
            float sfxVolume = PlayerPrefs.GetFloat(SFXKey, 0.3f);

            SetMasterVolume(masterVolume);
            SetBGMVolume(bgmVolume);
            SetSfxVolume(sfxVolume);
        }

        public void SetMasterVolume(float value)
        {
            value = Mathf.Clamp(value, 0.0001f, 1f);

            audioMixer.SetFloat(
                "MasterVolume",
                Mathf.Log10(value) * 20f
            );

            PlayerPrefs.SetFloat(MasterKey, value);
            PlayerPrefs.Save();
        }

        public void SetBGMVolume(float value)
        {
            value = Mathf.Clamp(value, 0.0001f, 1f);

            audioMixer.SetFloat(
                "BGMVolume",
                Mathf.Log10(value) * 20f
            );

            PlayerPrefs.SetFloat(BGMKey, value);
            PlayerPrefs.Save();
        }

        public void SetSfxVolume(float value)
        {
            value = Mathf.Clamp(value, 0.0001f, 1f);

            audioMixer.SetFloat(
                "SFXVolume",
                Mathf.Log10(value) * 20f
            );

            PlayerPrefs.SetFloat(SFXKey, value);
            PlayerPrefs.Save();
        }
    }
}