using JJB.Script.Battle.Enemy;
using JJB.Script.Battle.Player;
using JJB.Script.Battle.Player.Progression;
using JJB.Script.Battle.Stage;
using UnityEngine;

namespace JJB.Script.Battle
{
    public class JJBGameManager : MonoBehaviour
    {
        public static JJBGameManager Instance { get; private set; }
        [field: SerializeField] public JJBHealth PlayerJjbHealth { get; private set; }
        [field: SerializeField] public JJBHealth EnemyJjbHealth { get; private set; }
        [field: SerializeField] public BattleTurnManager BattleTurnManager { get; private set; }

        [field: SerializeField] public PlayerDamageReceiver PlayerDamageReceiver { get; private set; }
        [field: SerializeField] public EnemyDamageReceiver EnemyDamageReceiver { get; private set; }

        [field: SerializeField] public StageManager StageManager { get; private set; }
        public bool isFighting;
        
        public int PlayerCurrentHealth => PlayerJjbHealth != null ? PlayerJjbHealth.CurrentHealth : 0;
        public int PlayerMaxHealth => PlayerJjbHealth != null ? PlayerJjbHealth.MaxHealth : 0;

        public int EnemyCurrentHealth => EnemyJjbHealth != null ? EnemyJjbHealth.CurrentHealth : 0;
        public int EnemyMaxHealth => EnemyJjbHealth != null ? EnemyJjbHealth.MaxHealth : 0;

        public int CurrentStageIndex => StageManager != null ? StageManager.CurrentStageIndex : 0;
        public bool IsFighting => isFighting;
        public EnemyData CurrentEnemy
        {
            get
            {
                if (StageManager == null)
                    return null;

                return StageManager.CurrentEnemy;
            }
        }

        public int CurrentMoney
        {
            get
            {
                if (PlayerProfileManager.Instance == null)
                    return 0;

                return PlayerProfileManager.Instance.Profile.money;
            }
        }

        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
                return;
            }

            Instance = this;
        }

        public void AddMoney(int amount)
        {
            if (amount <= 0)
                return;

            if (PlayerProfileManager.Instance == null)
                return;

            PlayerProfileManager.Instance.Profile.money += amount;
        }
    }
}