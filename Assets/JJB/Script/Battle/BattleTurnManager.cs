using System;
using System.Collections;
using JJB.Script.Battle.Enemy;
using JJB.Script.Battle.Player;
using JJB.Script.Battle.Player.Progression;
using JJB.Script.Battle.Stage;
using UnityEngine;

namespace JJB.Script.Battle
{
    [RequireComponent(typeof(PlayerTurnController))]
    public class BattleTurnManager : MonoBehaviour
    {
        public DiceEffectUIUpdate diceEffectUIUpdate;
        [SerializeField] private BattleDefeatController battleDefeatController;
        
        private PlayerTurnController _playerTurnController;

        private EnemyTurnController _enemyTurnController;
        private JJBHealth _playerHealth;
        private JJBHealth _enemyHealth;
        
        private EnemyDamageReceiver _enemyDamageReceiver;
        
        private bool _rewardReceived;

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
            
            _rewardReceived = false;
            
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
            JJBGameManager.Instance.isFighting = false;

            if (_playerHealth.IsDead)
            {
                battleDefeatController.PlayDefeat();
                return;
            }

            ChangePhase(BattlePhase.BattleEnd);

            if (_enemyHealth.IsDead && !_playerHealth.IsDead)
            {
                StageManager stageManager = JJBGameManager.Instance.StageManager;

                if (!_rewardReceived)
                {
                    _rewardReceived = true;

                    // 경험치 +5
                    JJBGameManager.Instance.PlayerProgression.AddExp(5);

                    // 현재 클리어한 스테이지 기준 보상
                    // StageIndex가 0부터 시작하므로 +1
                    int rewardMoney = (stageManager.CurrentStageIndex + 1) * 15;

                    JJBGameManager.Instance.AddMoney(rewardMoney);

                    Debug.Log(
                        $"전투 보상 +{rewardMoney} / " +
                        $"현재 돈 : {PlayerProfileManager.Instance.Profile.money}"
                    );

                    // 다음 스테이지로 증가
                    stageManager.SetStageIndex(stageManager.CurrentStageIndex + 1);

                    // 돈, 체력, 스테이지 등을 JSON에 저장
                    DataManager dataManager = FindFirstObjectByType<DataManager>();

                    if (dataManager != null)
                    {
                        dataManager.SaveGame();
                    }
                    else
                    {
                        Debug.LogError("DataManager를 찾지 못했습니다.");
                    }
                }
            }
        }

        private void ChangePhase(BattlePhase phase)
        {
            CurrentPhase = phase;
            OnPhaseChanged?.Invoke(phase);
        }
    }
}