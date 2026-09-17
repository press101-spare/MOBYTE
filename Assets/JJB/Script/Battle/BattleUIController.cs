using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace JJB.Script.Battle
{
    public class BattleUIController : MonoBehaviour
    {
        [SerializeField] private Button attackButton;
        [SerializeField] private Button turnEndButton;

        private BattleTurnManager _battleTurnManager;

        public void Initialize(BattleTurnManager battleTurnManager)
        {
            _battleTurnManager = battleTurnManager;

            _battleTurnManager.OnPhaseChanged += UpdateUI;

            UpdateUI(_battleTurnManager.CurrentPhase);
        }

        private void OnDestroy()
        {
            if (_battleTurnManager != null)
                _battleTurnManager.OnPhaseChanged -= UpdateUI;
        }

        private void UpdateUI(BattlePhase phase)
        {
            SetButtonState(attackButton, phase == BattlePhase.HandSelect, HexColor("#0089FF"));
            SetButtonState(turnEndButton, phase == BattlePhase.TurnEnd, Color.green);
        }
        
        private Color HexColor(string hex)
        {
            ColorUtility.TryParseHtmlString(hex, out Color color);
            return color;
        }
        
        private void SetButtonState(Button button, bool isActive, Color activeColor)
        {
            button.interactable = isActive;

            TMP_Text text = button.GetComponentInChildren<TMP_Text>(true);
            Outline outline = button.GetComponent<Outline>();

            if (text != null)
                text.color = isActive ? activeColor : Color.gray;

            if (outline != null)
            {
                outline.enabled = true;
                outline.effectColor = isActive ? activeColor : Color.gray;

                if (isActive)
                    outline.effectColor = activeColor;
            }
        }
    }
}