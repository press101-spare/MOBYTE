using UnityEngine;

namespace JJB.Script
{
    public class DiceManager : MonoBehaviour
    {
        [SerializeField] private Dice[] dices;
        [SerializeField] private DicePhysics[] dicePhysics;

        [SerializeField] private MonoBehaviour diceRollerSource;

        private IDiceRoller _diceRoller;

        private void Awake()
        {
            _diceRoller = diceRollerSource as IDiceRoller;

            if (_diceRoller == null)
            {
                Debug.LogError(
                    "Dice Roller Source가 IDiceRoller를 구현하지 않았습니다.",
                    this
                );
            }
        }

        public void RollDices()
        {
            if (_diceRoller == null)
                return;

            for (int i = 0; i < dices.Length; i++)
            {
                if (dices[i].IsHeld)
                    continue;

                int value = _diceRoller.Roll();

                dices[i].SetValue(value);

                dicePhysics[i].Throw();

                Debug.Log($"Dice {i + 1} : {value}");
            }
        }
    }
}