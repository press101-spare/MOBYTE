using UnityEngine;

namespace JJB.Script.Battle.Enemy
{
    [RequireComponent(typeof(JJBHealth))]
    public class EnemyHealthSetup : MonoBehaviour
    {
        [SerializeField] private EnemyData enemyData;

        public EnemyData Data => enemyData;

        private void Awake()
        {
            GetComponent<JJBHealth>().Initialize(enemyData.MaxHealth);
        }
    }
}