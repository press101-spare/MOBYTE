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
            yield return null;

            int maxHealth = PlayerProfileManager.Instance.Profile.stats.maxHealth;

            // 보스 스테이지는 무조건 풀피
            if (IsBossStage())
            {
                _health.Initialize(maxHealth);
                yield break;
            }

            DataManager dataManager = FindFirstObjectByType<DataManager>();

            if (dataManager != null && dataManager.CurrentBattleData != null && dataManager.CurrentBattleData.playerHp > 0)
            {
                _health.Initialize(maxHealth, dataManager.CurrentBattleData.playerHp);
                yield break;
            }
            _health.Initialize(maxHealth);
        }

        private bool IsBossStage()
        {
            if (JJBGameManager.Instance == null || JJBGameManager.Instance.StageManager == null)
            {
                return false;
            }

            int currentStage = JJBGameManager.Instance.StageManager.CurrentStageIndex;

            return Array.IndexOf(bossStageIndexes, currentStage) >= 0;
        }
    }
}