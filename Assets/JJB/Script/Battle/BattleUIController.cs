using UnityEngine;
using UnityEngine.UI;

namespace JJB.Script.Battle
{
    public class BattleUIController : MonoBehaviour
    {
        [SerializeField] private BattleTurnManager turnManager;

        [Header("Buttons")]
        [SerializeField] private Button drawButton;
        [SerializeField] private Button attackButton;
        [SerializeField] private Button turnEndButton;
        
        private void OnEnable()
        {
            turnManager.OnPhaseChanged += UpdateButtons;
        }

        private void OnDisable()
        {
            turnManager.OnPhaseChanged -= UpdateButtons;
        }

        private void UpdateButtons(BattlePhase phase)
        {
            drawButton.interactable = phase == BattlePhase.Draw;
            attackButton.interactable = phase == BattlePhase.HandSelect;
            turnEndButton.interactable = phase == BattlePhase.TurnEnd;
        }
    }
}