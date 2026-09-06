using System;
using System.Collections;
using UnityEngine;

namespace JJB.Script.Battle
{
    public class BattleTurnManager : MonoBehaviour
    {
        [Header("Dice")]
        [SerializeField] private DiceManager_JCY diceManager;
        [SerializeField] private ShledDice_JCY shieldDice;

        [Header("Battle")]
        [SerializeField] private PlayerAttackController playerAttackController;
        [SerializeField] private EnemyTurnController enemyTurnController;

        public BattleTurn CurrentTurn { get; private set; }

        public event Action<BattleTurn> OnTurnChanged;
        
        private bool _canDraw;
        private bool _canAttack;

        private void Start()
        {
            StartCoroutine(StartPlayerTurn());
        }

        // ==============================
        // 플레이어 턴
        // ==============================

        private IEnumerator StartPlayerTurn()
        {
            CurrentTurn = BattleTurn.Player;
            OnTurnChanged?.Invoke(CurrentTurn);

            Debug.Log("플레이어 턴 시작 - Draw 버튼 대기");

            _canDraw = true;
            _canAttack = false;

            yield break;
        }
        
        public void DrawDice()
        {
            if (CurrentTurn != BattleTurn.Player)
                return;

            if (!_canDraw)
                return;

            if (diceManager.isRolling)
                return;

            _canDraw = false;

            DiceDeckManager_JCY.Instance.DrawDice();

            StartCoroutine(WaitForAttackDice());
        }

        private IEnumerator WaitForAttackDice()
        {
            yield return new WaitUntil(
                () => !diceManager.isRolling
            );

            Debug.Log("공격 주사위 완료 - 공격 가능");

            _canAttack = true;
        }
        
        public void Attack()
        {
            if (CurrentTurn != BattleTurn.Player)
                return;

            if (!_canAttack)
                return;

            if (diceManager.isRolling)
                return;

            int score = diceManager.diceTree.CurrentScore;

            if (score <= 0)
            {
                Debug.Log("족보를 먼저 선택해주세요.");
                return;
            }

            _canAttack = false;
            playerAttackController.Attack(score);
            
            StartCoroutine(DefensePhase());
        }

        // ==============================
        // 방어 주사위
        // ==============================

        private IEnumerator DefensePhase()
        {
            Debug.Log("방어 주사위 시작");

            shieldDice.shideDraw();

            if (shieldDice.shledDiceCount > 0)
            {
                yield return new WaitUntil(() => !diceManager.isRolling);
            }

            diceManager.isShled = false;

            Debug.Log($"방어 주사위 종료 / 쉴드: {shieldDice.shledValue}");
            
            yield return StartEnemyTurn();
        }

        // ==============================
        // 적 턴
        // ==============================

        private IEnumerator StartEnemyTurn()
        {
            CurrentTurn = BattleTurn.Enemy;
            OnTurnChanged?.Invoke(CurrentTurn);

            Debug.Log("적 턴 시작");

            bool enemyTurnFinished = false;

            enemyTurnController.ExecuteTurn(
                () =>
                {
                    enemyTurnFinished = true;
                }
            );
            
            yield return new WaitUntil(() => enemyTurnFinished);

            Debug.Log("적 턴 종료");
            
            yield return StartPlayerTurn();
        }

        // ==============================
        // 전투 종료
        // ==============================

        public void EndBattle()
        {
            CurrentTurn = BattleTurn.BattleEnd;
            
            OnTurnChanged?.Invoke(CurrentTurn);
            
            _canAttack = false;
            Debug.Log("전투 종료");
        }
    }
}