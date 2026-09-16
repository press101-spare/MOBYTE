using System;

namespace JJB.Script.Battle.Player.Progression
{
    [Serializable]
    public class PlayerSettings
    {
        public float bgmVolume = 1f;
        public float sfxVolume = 1f;

        public string qualityLevel = "very High";
        public int frameRate = 60;
    }
}