using UnityEngine;

namespace JJB.Script.Battle
{
    [RequireComponent(typeof(Health))]
    public class EnemyHealthSetup : MonoBehaviour
    {
        [SerializeField] private EnemyData enemyData;

        public EnemyData Data => enemyData;

        private void Awake()
        {
            GetComponent<Health>().Initialize(enemyData.MaxHealth);
        }
    }
}