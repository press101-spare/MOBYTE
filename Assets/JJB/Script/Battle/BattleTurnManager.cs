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

        public BattlePhase CurrentPhase { get; private set; }

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

            if (_enemyHealth.IsDead)
            {
                EndBattle();
                return;
            }

            // 여기서 바로 Defense로 넘어가지 않음
            StartCoroutine(WaitForAttackFinished());
        }

        private IEnumerator WaitForAttackFinished()
        {
            // 일단 임시로 2초 기다림
            yield return new WaitForSeconds(2f);

            ChangePhase(BattlePhase.Defense);

            _playerTurnController.StartDefense(OnDefenseFinished);
        }

        private void OnDefenseFinished()
        {
            ChangePhase(BattlePhase.TurnEnd);
            StartCoroutine(DelayCoroutine());
        }

        private IEnumerator DelayCoroutine()
        {
            yield return new WaitForSeconds(2f);
        }

        public void TurnEnd()
        {
            if (CurrentPhase != BattlePhase.TurnEnd)
                return;

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
            ChangePhase(BattlePhase.BattleEnd);
        }

        private void ChangePhase(BattlePhase phase)
        {
            CurrentPhase = phase;
            OnPhaseChanged?.Invoke(phase);

            Debug.Log($"Battle Phase : {phase}");
        }
    }
}