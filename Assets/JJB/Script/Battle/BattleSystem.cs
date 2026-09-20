using System.Collections;
using JJB.Script.Battle.Enemy;
using JJB.Script.Battle.Player;
using JJB.Script.Battle.Stage;
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
        private StageManager _stageManager;
        
        private void Awake()
        {
            _battleTurnManager = GetComponent<BattleTurnManager>();
            _playerTurnController = GetComponent<PlayerTurnController>();
            _diceBattleAdapter = GetComponent<DiceBattleAdapter>();

            FindPlayer();
            FindEnemy();
            FindUI();
            
            _stageManager = FindFirstObjectByType<StageManager>();
        }

        private IEnumerator Start()
        {
            yield return null;

            InitializeBattle();
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

        private void InitializeBattle()
        {
            if (!CanInitialize())
                return;
            
            EnemyData enemyData = _stageManager.PrepareCurrentStage();

            if (enemyData == null)
            {
                Debug.LogError("현재 스테이지의 EnemyData를 가져오지 못했습니다.");
                return;
            }

            _enemyHealthSetup.SetData(enemyData);
            _enemyHealthSetup.Initialize();

            _playerAttackController.Initialize(_enemyDamageReceiver);

            _playerDamageReceiver.Initialize(_diceBattleAdapter);
            _enemyTurnController.Initialize(_playerDamageReceiver);

            _playerTurnController.Initialize(_playerAttackController);

            if (_battleUIController != null)
                _battleUIController.Initialize(_battleTurnManager);

            _battleTurnManager.Initialize(_playerHealthSetup.Health, _enemyHealthSetup.Health, _enemyTurnController);
        }

        public void Attack()
        {
            _battleTurnManager.Attack();
        }

        public void TurnEnd()
        {
            _battleTurnManager.TurnEnd();
        }

        private bool CanInitialize()
        {
            if (_playerHealthSetup == null)
            {
                Debug.LogError("PlayerHealthSetup이 없습니다.");
                return false;
            }

            if (_playerAttackController == null)
            {
                Debug.LogError("PlayerAttackController가 없습니다.");
                return false;
            }

            if (_playerDamageReceiver == null)
            {
                Debug.LogError("PlayerDamageReceiver가 없습니다.");
                return false;
            }

            if (_enemyHealthSetup == null)
            {
                Debug.LogError("EnemyHealthSetup이 없습니다.");
                return false;
            }

            if (_enemyDamageReceiver == null)
            {
                Debug.LogError("EnemyDamageReceiver가 없습니다.");
                return false;
            }

            if (_enemyTurnController == null)
            {
                Debug.LogError("EnemyTurnController가 없습니다.");
                return false;
            }

            return true;
        }
    }
}