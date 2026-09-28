using System;
using System.Collections;
using JJB.Script.Battle.Player.Progression;
using UnityEngine;

namespace JJB.Script.Battle.Player
{
    [RequireComponent(typeof(JJBHealth))]
    public class PlayerHealthSetup : MonoBehaviour
    {
        [Header("Boss Stage")]
        [SerializeField] private int[] bossStageIndexes;
        
        private JJBHealth _health;

        public JJBHealth Health => _health;

        private void Awake()
        {
            _health = GetComponent<JJBHealth>();
        }

        private IEnumerator Start()
        {
            // DataManager.Start가 JSON을 적용할 시간을 한 프레임 기다림
            yield return null;

            int maxHealth =
                PlayerProfileManager.Instance.Profile.stats.maxHealth;

            // 보스 스테이지는 무조건 풀피
            if (IsBossStage())
            {
                _health.Initialize(maxHealth);
                yield break;
            }

            DataManager dataManager =
                FindFirstObjectByType<DataManager>();

            // 저장된 전투 체력이 있으면 그 체력으로 시작
            if (dataManager != null &&
                dataManager.CurrentBattleData != null)
            {
                int savedHp =
                    dataManager.CurrentBattleData.playerHp;

                _health.Initialize(
                    maxHealth,
                    savedHp
                );

                yield break;
            }

            // 저장된 전투 데이터가 없으면 풀피
            _health.Initialize(maxHealth);
        }

        private bool IsBossStage()
        {
            if (JJBGameManager.Instance == null ||
                JJBGameManager.Instance.StageManager == null)
            {
                return false;
            }

            int currentStage =
                JJBGameManager.Instance
                    .StageManager
                    .CurrentStageIndex;

            return Array.IndexOf(
                bossStageIndexes,
                currentStage
            ) >= 0;
        }
    }
}