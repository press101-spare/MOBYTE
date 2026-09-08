using System;
using JJB.Script.Battle.Player;
using UnityEngine;

namespace JJB.Script.Battle
{
    public class BattleTurnManager : MonoBehaviour
    {
        [Header("Turn")]
        [SerializeField] private PlayerTurnController playerTurnController;
        [SerializeField] private EnemyTurnController enemyTurnController;


        [Header("Health")]
        [SerializeField] private JJBHealth playerHealth;
        [SerializeField] private JJBHealth enemyHealth;
        
        public BattlePhase CurrentPhase { get; private set; }
        public event Action<BattlePhase> OnPhaseChanged;

        private void Start()
        {
            ChangePhase(BattlePhase.Start);
            StartPlayerTurn();
        }


        // =========================
        // 새로운 플레이어 턴
        // =========================

        private void StartPlayerTurn()
        {
            ChangePhase(BattlePhase.Draw);
        }


        // =========================
        // DRAW 버튼
        // =========================

        public void Draw()
        {
            if (CurrentPhase != BattlePhase.Draw)
            {
                return;
            }

            playerTurnController.DrawDice(OnDrawFinished);
        }

        private void OnDrawFinished()
        {
            ChangePhase(BattlePhase.HandSelect);
        }

        // =========================
        // ATTACK 버튼
        // =========================

        public void Attack()
        {
            if (CurrentPhase != BattlePhase.HandSelect)
            {
                return;
            }
            ChangePhase(BattlePhase.Attack);
            bool success = playerTurnController.TryAttack();
            if (!success)
            {
                ChangePhase(BattlePhase.HandSelect);
                return;
            }
            if (enemyHealth.IsDead)
            {
                EndBattle();
                return;
            }
            StartDefense();
        }


        // =========================
        // 방어
        // =========================

        private void StartDefense()
        {
            ChangePhase(BattlePhase.Defense);
            playerTurnController.StartDefense(OnDefenseFinished);
        }

        private void OnDefenseFinished()
        {
            ChangePhase(BattlePhase.TurnEnd);
        }


        // =========================
        // TURN END 버튼
        // =========================

        public void TurnEnd()
        {
            if (CurrentPhase != BattlePhase.TurnEnd)
            {
                return;
            }
            StartEnemyTurn();
        }


        // =========================
        // 적 턴
        // =========================

        private void StartEnemyTurn()
        {
            ChangePhase(BattlePhase.Enemy);
            enemyTurnController.ExecuteTurn(OnEnemyTurnFinished);
        }

        private void OnEnemyTurnFinished()
        {
            if (playerHealth.IsDead)
            {
                EndBattle();
                return;
            }
            StartPlayerTurn();
        }


        // =========================
        // 전투 종료
        // =========================

        private void EndBattle()
        {
            ChangePhase(BattlePhase.BattleEnd);
        }


        // =========================
        // Phase 변경
        // =========================

        private void ChangePhase(BattlePhase phase)
        {
            CurrentPhase = phase;
            OnPhaseChanged?.Invoke(phase);
        }
    }
}