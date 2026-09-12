using UnityEngine;
using UnityEngine.Serialization;

namespace JJB.Script.Battle
{
    public class JJBGameManager : MonoBehaviour
    {
        public static JJBGameManager Instance { get; private set; }
        
        [field: SerializeField] public JJBHealth PlayerJjbHealth { get; private set; }
        [field: SerializeField] public JJBHealth EnemyJjbHealth { get; private set; }
        [field: SerializeField] public BattleTurnManager BattleTurnManager { get; private set; }

        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
                return;
            }

            Instance = this;
        }
    }
}