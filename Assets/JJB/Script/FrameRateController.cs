using TMPro;
using UnityEngine;

namespace JJB.Script
{
    public class FrameRateController : MonoBehaviour
    {
        private const string FrameRateKey = "FrameRate";

        [SerializeField] private TMP_Dropdown frameRateDropdown;

        private readonly int[] _frameRates =
        {
            30,
            60,
            120
        };

        private void Start()
        {
            int savedIndex = PlayerPrefs.GetInt(FrameRateKey, 1);

            savedIndex = Mathf.Clamp(
                savedIndex,
                0,
                _frameRates.Length - 1
            );

            ApplyFrameRate(savedIndex);

            frameRateDropdown.SetValueWithoutNotify(
                savedIndex
            );
        }

        public void SetFrameRate(int index)
        {
            if (index < 0 || index >= _frameRates.Length)
                return;

            ApplyFrameRate(index);

            PlayerPrefs.SetInt(
                FrameRateKey,
                index
            );

            PlayerPrefs.Save();
        }

        private void ApplyFrameRate(int index)
        {
            Application.targetFrameRate =
                _frameRates[index];
        }
    }
}