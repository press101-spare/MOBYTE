using UnityEngine;

namespace JJB.Script
{
    public class Dice : MonoBehaviour
    {
        private int _value;
        private bool _isHeld;

        public int Value => _value;
        public bool IsHeld => _isHeld;

        public void SetValue(int value)
        {
            _value = value;
        }

        public void ToggleHold()
        {
            _isHeld = !_isHeld;
        }

        public void ResetDice()
        {
            _value = 0;
            _isHeld = false;
        }
    }
}