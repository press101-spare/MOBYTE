using JJB.Script.Battle.Enemy;
using JJB.Script.Battle.Player;
using UnityEngine;

namespace JJB.Script.Battle
{
    [RequireComponent(typeof(BattleTurnManager))]
    [RequireComponent(typeof(PlayerTurnController))]
    [RequireComponent(typeof(DiceBattleAdapter))]
    public class BattleSystem : MonoBehaviour
    {
        private BattleTurnManager _battleTurnManager;
        private PlayerTurnController _playerTurnController;
        private DiceBattleAdapter _diceBattleAdapter;

        private PlayerHealthSetup _playerHealthSetup;
        private PlayerAttackController _playerAttackController;
        private PlayerDamageReceiver _playerDamageReceiver;

        private EnemyHealthSetup _enemyHealthSetup;
        private EnemyDamageReceiver _enemyDamageReceiver;
        private EnemyTurnController _enemyTurnController;

        private BattleUIController _battleUIController;

        private void Awake()
        {
            _battleTurnManager = GetComponent<BattleTurnManager>();
            _playerTurnController = GetComponent<PlayerTurnController>();
            _diceBattleAdapter = GetComponent<DiceBattleAdapter>();

            FindPlayer();
            FindEnemy();
            FindUI();
        }

        private void Start()
        {
            StartBattle();
        }

        private void FindPlayer()
        {
            _playerHealthSetup = FindFirstObjectByType<PlayerHealthSetup>();
            _playerAttackController = FindFirstObjectByType<PlayerAttackController>();
            _playerDamageReceiver = FindFirstObjectByType<PlayerDamageReceiver>();
        }

        private void FindEnemy()
        {
            _enemyHealthSetup = FindFirstObjectByType<EnemyHealthSetup>();
            _enemyDamageReceiver = FindFirstObjectByType<EnemyDamageReceiver>();
            _enemyTurnController = FindFirstObjectByType<EnemyTurnController>();
        }

        private void FindUI()
        {
            _battleUIController = FindFirstObjectByType<BattleUIController>();
        }

        private void StartBattle()
        {
            if (!CanStartBattle())
                return;

            _enemyHealthSetup.Initialize();

            _playerAttackController.Initialize(_enemyDamageReceiver);
            _playerDamageReceiver.Initialize(_diceBattleAdapter);

            _enemyTurnController.Initialize(_playerDamageReceiver);
            _playerTurnController.Initialize(_playerAttackController);

            _battleTurnManager.Initialize(
                _playerHealthSetup.Health,
                _enemyHealthSetup.Health,
                _enemyTurnController
            );

            if (_battleUIController != null)
                _battleUIController.Initialize(_battleTurnManager);

            _battleTurnManager.StartBattle();
        }

        public void Attack()
        {
            _battleTurnManager.Attack();
        }

        public void TurnEnd()
        {
            _battleTurnManager.TurnEnd();
        }

        private bool CanStartBattle()
        {
            if (_playerHealthSetup == null)
            {
                Debug.LogError("PlayerHealthSetup을 찾을 수 없습니다.");
                return false;
            }

            if (_playerAttackController == null)
            {
                Debug.LogError("PlayerAttackController를 찾을 수 없습니다.");
                return false;
            }

            if (_playerDamageReceiver == null)
            {
                Debug.LogError("PlayerDamageReceiver를 찾을 수 없습니다.");
                return false;
            }

            if (_enemyHealthSetup == null)
            {
                Debug.LogError("EnemyHealthSetup을 찾을 수 없습니다.");
                return false;
            }

            if (_enemyDamageReceiver == null)
            {
                Debug.LogError("EnemyDamageReceiver를 찾을 수 없습니다.");
                return false;
            }

            if (_enemyTurnController == null)
            {
                Debug.LogError("EnemyTurnController를 찾을 수 없습니다.");
                return false;
            }

            return true;
        }
    }
}