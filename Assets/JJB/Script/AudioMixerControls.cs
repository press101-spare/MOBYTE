using System;
using UnityEngine;
using UnityEngine.Audio;

namespace JJB.Script
{
    public class AudioMixerControls : MonoBehaviour
    {
        [SerializeField] private AudioMixer audioMixer;

        private void Start()
        {
            audioMixer.SetFloat("MasterVolume", Mathf.Log10(0.3f) * 20f);
            audioMixer.SetFloat("BGMVolume", Mathf.Log10(0.3f) * 20f);
            audioMixer.SetFloat("SFXVolume", Mathf.Log10(0.3f) * 20f);
        }

        public void SetMasterVolume(float value)
        {
            audioMixer.SetFloat("MasterVolume", Mathf.Log10(value) * 20f);
        }

        public void SetBGMVolume(float value)
        {
            audioMixer.SetFloat(
                "BGMVolume",
                Mathf.Log10(value) * 20f
            );
        }

        public void SetSfxVolume(float value)
        {
            audioMixer.SetFloat(
                "SFXVolume",
                Mathf.Log10(value) * 20f
            );
        }
    }
}