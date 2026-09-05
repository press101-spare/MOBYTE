using UnityEngine;
using UnityEngine.Serialization;

namespace JJB.Script.Battle
{
    public class JJBGameManager : MonoBehaviour
    {
        public static JJBGameManager Instance { get; private set; }

        [field: FormerlySerializedAs("<PlayerHealth>k__BackingField")]
        [field: SerializeField]
        public JJBHealth PlayerJjbHealth { get; private set; }

        [field: FormerlySerializedAs("<EnemyHealth>k__BackingField")]
        [field: SerializeField]
        public JJBHealth EnemyJjbHealth { get; private set; }

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