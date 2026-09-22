using System;
using System.Collections;
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
        
        private EnemyDamageReceiver _enemyDamageReceiver;

        public BattlePhase CurrentPhase { get; private set; }

        public event Action<BattlePhase> OnPhaseChanged;

        private void Awake()
        {
            _playerTurnController = GetComponent<PlayerTurnController>();
        }

        public void Initialize(
            JJBHealth playerHealth,
            JJBHealth enemyHealth,
            EnemyTurnController enemyTurnController,
            EnemyDamageReceiver enemyDamageReceiver)
        {
            _playerHealth = playerHealth;
            _enemyHealth = enemyHealth;
            _enemyTurnController = enemyTurnController;
            _enemyDamageReceiver = enemyDamageReceiver;
            
            if (JJBGameManager.Instance != null)
                JJBGameManager.Instance.isFighting = true;

            StartPlayerTurn();
        }

        private void StartPlayerTurn()
        {
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

            if (!_playerTurnController.TryAttack())
                return;

            ChangePhase(BattlePhase.Attack);

            StartCoroutine(WaitForAttackFinished());
        }

        private IEnumerator WaitForAttackFinished()
        {
            yield return new WaitForSeconds(2f);

            if (_enemyHealth.IsDead)
            {
                EndBattle();
                yield break;
            }

            if (!_playerTurnController.HasDefenseDice())
            {
                StartEnemyTurn();
                yield break;
            }

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
            _enemyDamageReceiver?.TickPoison();

            if (_enemyHealth.IsDead)
            {
                EndBattle();
                return;
            }

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
            if (JJBGameManager.Instance != null)
                JJBGameManager.Instance.isFighting = false;
            
            ChangePhase(BattlePhase.BattleEnd);
        }

        private void ChangePhase(BattlePhase phase)
        {
            CurrentPhase = phase;
            OnPhaseChanged?.Invoke(phase);
        }
    }
}