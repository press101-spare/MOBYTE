using TMPro;
using UnityEngine;

namespace JJB.Script.Battle.Enemy
{
    [RequireComponent(typeof(JJBHealth))]
    public class EnemyHealthSetup : MonoBehaviour
    {
        [SerializeField] private EnemyData data;
        [SerializeField] private TMP_Text enemyNameText;

        private JJBHealth _health;

        public EnemyData Data => data;
        public JJBHealth Health => _health;
        public EnemyAbility Ability { get; private set; }

        private void Awake()
        {
            _health = GetComponent<JJBHealth>();
        }
        
        public void SetData(EnemyData enemyData)
        {
            data = enemyData;
        }

        public void Initialize()
        {
            if (data == null)
            {
                Debug.LogError("EnemyData가 없습니다.", this);
                return;
            }

            _health.Initialize(data.MaxHealth);

            if (data.Ability != null)
                Ability = Instantiate(data.Ability);
        }
        
        private void OnValidate()
        {
            if (data == null || enemyNameText == null)
                return;

            enemyNameText.text = data.EnemyName;
        }
    }
}