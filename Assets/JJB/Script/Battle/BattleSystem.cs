using JJB.Script.Battle.Enemy;
using JJB.Script.Battle.Player;
using UnityEngine;

namespace JJB.Script.Battle
{
    [RequireComponent(typeof(BattleTurnManager))]
    [RequireComponent(typeof(PlayerTurnController))]
    public class BattleSystem : MonoBehaviour
    {
        private BattleTurnManager _turnManager;
        private PlayerTurnController _playerTurnController;

        private void Awake()
        {
            _turnManager =
                GetComponent<BattleTurnManager>();

            _playerTurnController =
                GetComponent<PlayerTurnController>();
        }

        public void Initialize(PlayerBattleActor player, EnemyBattleActor enemy)
        {
            _turnManager.Initialize(player.Health, enemy.Health, enemy.TurnController);
            _playerTurnController.Initialize(player.AttackController);
        }
    }
}