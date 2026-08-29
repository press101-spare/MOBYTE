using System;
using UnityEngine;

namespace JJB.Script.Battle
{
    public class BattleTurnManager : MonoBehaviour
    {
        [SerializeField] private EnemyTurnController enemyTurnController;

        private BattleTurn _currentTurn;

        public BattleTurn CurrentTurn => _currentTurn;

        public event Action<BattleTurn> OnTurnChanged;

        private void Start()
        {
            StartPlayerTurn();
        }

        private void StartPlayerTurn()
        {
            _currentTurn = BattleTurn.Player;

            OnTurnChanged?.Invoke(_currentTurn);
        }

        public void EndPlayerTurn()
        {
            if (_currentTurn != BattleTurn.Player)
                return;

            StartEnemyTurn();
        }

        private void StartEnemyTurn()
        {
            _currentTurn = BattleTurn.Enemy;

            OnTurnChanged?.Invoke(_currentTurn);

            enemyTurnController.ExecuteTurn(
                EndEnemyTurn
            );
        }

        private void EndEnemyTurn()
        {
            StartPlayerTurn();
        }

        public void EndBattle()
        {
            _currentTurn = BattleTurn.BattleEnd;

            OnTurnChanged?.Invoke(_currentTurn);
        }
    }
}