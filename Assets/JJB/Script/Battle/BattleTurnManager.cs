using System;
using JJB.Script.Battle.Enemy;
using JJB.Script.Battle.Player;
using UnityEngine;

namespace JJB.Script.Battle
{
    [RequireComponent(typeof(PlayerTurnController))]
    public class BattleTurnManager : MonoBehaviour
    {
        private PlayerTurnController _playerTurnController;

        private EnemyTurnController _enemyTurnController;
        private JJBHealth _playerHealth;
        private JJBHealth _enemyHealth;

        public BattlePhase CurrentPhase { get; private set; }
        public int TurnCount { get; private set; }

        public event Action<BattlePhase> OnPhaseChanged;

        private void Awake()
        {
            _playerTurnController = GetComponent<PlayerTurnController>();
        }

        public void Initialize(JJBHealth playerHealth, JJBHealth enemyHealth, EnemyTurnController enemyTurnController)
        {
            _playerHealth = playerHealth;
            _enemyHealth = enemyHealth;
            _enemyTurnController = enemyTurnController;

            TurnCount = 0;

            ChangePhase(BattlePhase.Start);
        }

        public void StartBattle()
        {
            if (_playerHealth ==null || _enemyHealth == null)
            {
                Debug.LogError("BattleTurnManager 없음");
                return;
            }
            
            StartPlayerTurn();
        }

        private void StartPlayerTurn()
        {
            TurnCount++;
            
            ChangePhase(BattlePhase.Draw);

            _playerTurnController.DrawDice(OnDrawFinished);
        }

        private void OnDrawFinished()
        {
            ChangePhase(BattlePhase.HandSelect);
        }

        public void Attack()
        {
            if (CurrentPhase != BattlePhase.HandSelect)
                return;

            ChangePhase(BattlePhase.Attack);

            bool attacked = _playerTurnController.TryAttack();

            if (!attacked)
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
        
        private void StartDefense()
        {
            ChangePhase(BattlePhase.Defense);

            _playerTurnController.StartDefense(OnDefenseFinished);
        }

        private void OnDefenseFinished()
        {
            ChangePhase(BattlePhase.TurnEnd);
        }

        public void TurnEnd()
        {
            if (CurrentPhase != BattlePhase.TurnEnd)
                return;
            
            StartEnemyTurn();
        }
        
        private void StartEnemyTurn()
        {
            ChangePhase(BattlePhase.Enemy);

            _enemyTurnController.ExecuteTurn(OnEnemyTurnFinished);
        }

        private void OnEnemyTurnFinished()
        {
            _playerTurnController.ClearDice();

            if (_playerHealth.IsDead)
            {
                EndBattle();
                return;
            }

            StartPlayerTurn();
        }

        private void EndBattle()
        {
            _playerTurnController.ClearDice();

            ChangePhase(BattlePhase.BattleEnd);
        }

        private void ChangePhase(BattlePhase phase)
        {
            CurrentPhase = phase;
            OnPhaseChanged?.Invoke(phase);
        }
    }
}