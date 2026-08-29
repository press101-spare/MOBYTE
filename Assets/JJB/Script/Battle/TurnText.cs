using TMPro;
using UnityEngine;

namespace JJB.Script.Battle
{
    public class TurnText : MonoBehaviour
    {
        [SerializeField] private BattleTurnManager turnManager;
        [SerializeField] private TMP_Text turnText;

        private void OnEnable()
        {
            turnManager.OnTurnChanged += UpdateUI;
        }

        private void OnDisable()
        {
            turnManager.OnTurnChanged -= UpdateUI;
        }

        private void UpdateUI(BattleTurn turn)
        {
            turnText.text = turn switch
            {
                BattleTurn.Player => "PLAYER TURN",
                BattleTurn.Enemy => "ENEMY TURN",
                _ => "BATTLE END"
            };
        }
    }
}