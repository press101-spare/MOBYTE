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
            attackButton.interactable = phase == BattlePhase.HandSelect;
            turnEndButton.interactable = phase == BattlePhase.TurnEnd;
        }
    }
}