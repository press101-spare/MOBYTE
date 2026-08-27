using UnityEngine;

namespace JJB.Script
{
    [RequireComponent(typeof(Health))]
    public class Enemy : MonoBehaviour
    {
        [SerializeField] private EnemyData enemyData;

        private Health _health;

        public EnemyData Data => enemyData;
        public Health Health => _health;

        private void Awake()
        {
            _health = GetComponent<Health>();

            _health.Initialize(enemyData.MaxHealth);
        }
    }
}