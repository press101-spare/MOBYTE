using System;
using JJB.Script.Battle.Enemy;
using JJB.Script.Battle.Player;
using UnityEngine;

namespace JJB.Script.Battle
{
    [RequireComponent(typeof(PlayerTurnController))]
    public class BattleTurnManager : MonoBehaviour
    {
        [Header("Turn")]
        private PlayerTurnController _playerTurnController;
        private EnemyTurnController _enemyTurnController;


        [Header("Health")]
        private JJBHealth _playerHealth;
        private JJBHealth _enemyHealth;
        
        public BattlePhase CurrentPhase { get; private set; }
        public event Action<BattlePhase> OnPhaseChanged;

        private void Awake()
        {
            _playerTurnController = GetComponent<PlayerTurnController>();
        }

        public void Initialize(
            JJBHealth playerHealth, 
            JJBHealth enemyHealth, 
            EnemyTurnController enemyTurnController)
        {
            _playerHealth = playerHealth;
            _enemyHealth = enemyHealth;
            _enemyTurnController = enemyTurnController;
        }

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
            _playerTurnController.DrawDice(OnDrawFinished);
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
            bool success = _playerTurnController.TryAttack();
            if (!success)
            {
                ChangePhase(BattlePhase.HandSelect);
                return;
            }
            if (_enemyHealth.IsDead)
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
            _playerTurnController.StartDefense(OnDefenseFinished);
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
            _enemyTurnController.ExecuteTurn(OnEnemyTurnFinished);
        }

        private void OnEnemyTurnFinished()
        {
            DiceManager_JCY.Instance.ClearDice();
            if (_playerHealth.IsDead)
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