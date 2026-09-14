using UnityEngine;

namespace JJB.Script.Battle.Enemy
{
    [RequireComponent(typeof(JJBHealth))]
    public class EnemyHealthSetup : MonoBehaviour
    {
        [SerializeField] private EnemyData data;

        private JJBHealth _health;

        public EnemyData Data => data;
        public JJBHealth Health => _health;

        private void Awake()
        {
            _health = GetComponent<JJBHealth>();

            if (data == null)
            {
                Debug.LogError("EnemyData가 없습니다.", this);
                return;
            }

            _health.Initialize(data.MaxHealth);
        }
    }
}