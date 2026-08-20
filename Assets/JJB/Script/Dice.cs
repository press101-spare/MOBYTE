using System;
using UnityEngine;

namespace JJB.Script
{
    public class Dice : MonoBehaviour
    {
        private int _value;
        private bool _isHeld;

        public int Value => _value;
        public bool IsHeld => _isHeld;

        public event Action<int> OnValueChanged;

        public void SetValue(int value)
        {
            _value = value;

            OnValueChanged?.Invoke(_value);
        }

        public void ToggleHold()
        {
            _isHeld = !_isHeld;
        }

        public void ResetDice()
        {
            _value = 0;
            _isHeld = false;

            OnValueChanged?.Invoke(_value);
        }
    }
}