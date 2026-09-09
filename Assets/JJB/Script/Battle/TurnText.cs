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
            turnManager.OnPhaseChanged += UpdateText;
        }

        private void OnDisable()
        {
            turnManager.OnPhaseChanged -= UpdateText;
        }

        private void UpdateText(BattlePhase phase)
        {
            turnText.text = phase switch
            {
                BattlePhase.Start => "BATTLE START",
                BattlePhase.Draw => "DRAW",
                BattlePhase.HandSelect => "SELECT HAND",
                BattlePhase.Attack => "ATTACK",
                BattlePhase.Defense => "DEFENSE",
                BattlePhase.TurnEnd => "TURN END",
                BattlePhase.Enemy => "ENEMY TURN",
                BattlePhase.BattleEnd => "BATTLE END",

                _ => ""
            };
        }
    }
}