using UnityEngine;

namespace JJB.Script.Battle.Player
{
    [RequireComponent(typeof(JJBHealth))]
    public class PlayerHealthSetup : MonoBehaviour
    {
        [SerializeField] private int maxHealth = 70;

        private JJBHealth _health;

        public JJBHealth Health => _health;

        private void Awake()
        {
            _health = GetComponent<JJBHealth>();
            _health.Initialize(maxHealth);

            Debug.Log($"Player HP 초기화 : {_health.CurrentHealth}/{_health.MaxHealth}");
        }
    }
}