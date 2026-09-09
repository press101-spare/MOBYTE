using System;
using System.Collections;
using UnityEngine;

namespace JJB.Script.Battle.Player
{
    public class PlayerTurnController : MonoBehaviour
    {
        [Header("Dice")]
        [SerializeField] private DiceManager_JCY diceManager;
        [SerializeField] private ShledDice_JCY shieldDice;

        [Header("Attack")]
        [SerializeField] private PlayerAttackController playerAttackController;


        // =========================
        // 공격 주사위 Draw
        // =========================

        public void DrawDice(Action onFinished)
        {
            if (diceManager.isRolling) return;

            DiceDeckManager_JCY.Instance.DrawDice();

            StartCoroutine(WaitForDraw(onFinished));
        }

        private IEnumerator WaitForDraw(Action onFinished)
        {
            yield return new WaitUntil(() => !diceManager.isRolling);
            onFinished?.Invoke();
        }


        // =========================
        // 공격
        // =========================

        public bool TryAttack()
        {
            if (diceManager.isRolling) return false;

            int score = diceManager.diceTree.CurrentScore;

            if (score <= 0)
            {
                return false;
            }

            playerAttackController.Attack(score);

            return true;
        }


        // =========================
        // 방어 주사위
        // =========================

        public void StartDefense(Action onFinished)
        {
            StartCoroutine(DefenseRoutine(onFinished));
        }

        private IEnumerator DefenseRoutine(Action onFinished)
        {
            shieldDice.shideDraw();
            
            if (shieldDice.shledDiceCount <= 0)
            {
                diceManager.isShled = false;

                onFinished?.Invoke();
                yield break;
            }

            yield return new WaitUntil(() => !diceManager.isRolling);

            diceManager.isShled = false;

            onFinished?.Invoke();
        }
    }
}