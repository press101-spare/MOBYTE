using TMPro;
using UnityEngine;

namespace JJB.Script
{
    public class DiceValueView : MonoBehaviour
    {
        [SerializeField] private Dice dice;
        [SerializeField] private TMP_Text valueText;

        private void OnEnable()
        {
            dice.OnValueChanged += UpdateValue;
        }

        private void OnDisable()
        {
            dice.OnValueChanged -= UpdateValue;
        }

        private void UpdateValue(int value)
        {
            valueText.text = value == 0
                ? "-"
                : value.ToString();
        }
    }
}