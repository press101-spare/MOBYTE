using UnityEngine;

namespace JJB.Script.Battle.Enemy
{
    [RequireComponent(typeof(JJBHealth))]
    public class EnemyHealthSetup : MonoBehaviour
    {
        [SerializeField] private EnemyData enemyData;
        
        private JJBHealth _health;
        private EnemyAbilityController _abilityController;
        public EnemyData Data => enemyData;

        private void Awake()
        {
            _health = GetComponent<JJBHealth>();
            _abilityController = GetComponent<EnemyAbilityController>();
        }

        private void Start()
        {
            _health.Initialize(enemyData.MaxHealth);
            _abilityController.Initialize(enemyData);
        }
    }
}