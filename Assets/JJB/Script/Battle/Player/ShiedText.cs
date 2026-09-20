using TMPro;
using UnityEngine;

namespace JJB.Script.Battle.Player
{
    public class ShiedText : MonoBehaviour
    {
        private TMP_Text _shieldText;
        private DiceBattleAdapter _diceBattleAdapter;

        private int _lastShield = -1;

        private void Awake()
        {
            _shieldText = GetComponent<TMP_Text>();
            _diceBattleAdapter = FindFirstObjectByType<DiceBattleAdapter>();
        }

        private void Update()
        {
            if (_diceBattleAdapter == null)
                return;

            int currentShield = _diceBattleAdapter.ShieldValue;

            if (_lastShield == currentShield)
                return;

            _lastShield = currentShield;
            _shieldText.text = "+" + currentShield.ToString();
        }
    }
}