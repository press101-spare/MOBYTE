using UnityEngine;
using UnityEngine.UI;

namespace JJB.Script.Battle
{
    public class PlayerActionUI : MonoBehaviour
    {
        [SerializeField] private BattleTurnManager turnManager;
        [SerializeField] private Button[] actionButtons;

        private void OnEnable()
        {
            turnManager.OnTurnChanged += UpdateButtons;
        }

        private void OnDisable()
        {
            turnManager.OnTurnChanged -= UpdateButtons;
        }

        private void UpdateButtons(BattleTurn turn)
        {
            bool interactable = turn == BattleTurn.Player;

            foreach (Button button in actionButtons)
            {
                button.interactable = interactable;
            }
        }
    }
}