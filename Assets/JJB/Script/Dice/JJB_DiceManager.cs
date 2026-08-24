using System;
using UnityEngine;

namespace JJB.Script
{
    public class JJB_DiceManager : MonoBehaviour
    {
        [SerializeField] private JJB_Dice[] dices;
        [SerializeField] private JJB_DicePhysics[] dicePhysics;

        [SerializeField] private MonoBehaviour diceRollerSource;

        private IDiceRoller _diceRoller;

        private void Awake()
        {
            _diceRoller = diceRollerSource as IDiceRoller;

            if (_diceRoller == null)
            {
                Debug.LogError("Dice Roller Source가 IDiceRoller를 구현하지 않았습니다.", this);
            }
        }

        private void Start()
        {
            RollDices();
        }
        
        private void RollDices()
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

                Debug.Log($"Dice {i + 1} Value : {dices[i].Value}");
            }
        }
    }
}