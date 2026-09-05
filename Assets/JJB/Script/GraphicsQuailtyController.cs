using TMPro;
using UnityEngine;

namespace JJB.Script
{
    public class GraphicsQuailtyController : MonoBehaviour
    {
        private const string QualityKey = "GraphicsQuality";

        [SerializeField] private TMP_Dropdown qualityDropdown;

        private void Start()
        {
            int savedQuality = PlayerPrefs.GetInt(
                QualityKey,
                2
            );

            savedQuality = Mathf.Clamp(savedQuality, 0, 4);

            QualitySettings.SetQualityLevel(
                savedQuality,
                true
            );

            qualityDropdown.SetValueWithoutNotify(
                savedQuality
            );
        }

        public void SetQuality(int qualityIndex)
        {
            qualityIndex = Mathf.Clamp(
                qualityIndex,
                0,
                4
            );

            QualitySettings.SetQualityLevel(
                qualityIndex,
                true
            );

            PlayerPrefs.SetInt(
                QualityKey,
                qualityIndex
            );

            PlayerPrefs.Save();
        }
    }
}