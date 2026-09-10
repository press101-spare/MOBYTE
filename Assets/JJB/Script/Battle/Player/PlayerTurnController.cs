using System;
using System.Collections;
using UnityEngine;

namespace JJB.Script.Battle.Player
{
    public class PlayerTurnController : MonoBehaviour
    {
        private PlayerAttackController _playerAttackController;

        private DiceManager_JCY DiceManager => DiceManager_JCY.Instance;
        private ShledDice_JCY ShieldDice => DiceManager_JCY.Instance.shledDice;
        
        public void Initialize(PlayerAttackController playerAttackController)
        {
            _playerAttackController = playerAttackController;
        }
        
        // =========================
        // 공격 주사위 Draw
        // =========================

        public void DrawDice(Action onFinished)
        {
            if (DiceManager.isRolling) return;

            DiceDeckManager_JCY.Instance.DrawDice();

            StartCoroutine(WaitForDraw(onFinished));
        }

        private IEnumerator WaitForDraw(Action onFinished)
        {
            yield return new WaitUntil(() => !DiceManager.isRolling);
            onFinished?.Invoke();
        }


        // =========================
        // 공격
        // =========================

        public bool TryAttack()
        {
            if (DiceManager.isRolling) return false;

            int score = DiceManager.diceTree.CurrentScore;

            if (score <= 0)
            {
                return false;
            }
            
            if (_playerAttackController == null)
            {
                Debug.LogError("PlayerAttackController가 초기화되지 않았습니다.");
                return false;
            }
            _playerAttackController.Attack(score);

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
            ShieldDice.shideDraw();
            
            if (ShieldDice.shledDiceCount <= 0)
            {
                DiceManager.isShled = false;

                onFinished?.Invoke();
                yield break;
            }

            yield return new WaitUntil(() => !DiceManager.isRolling);

            DiceManager.isShled = false;

            onFinished?.Invoke();
        }
    }
}