using JJB.Script.Battle.Player.Progression;
using UnityEngine;

namespace JJB.Script.Battle.Player
{
    [RequireComponent(typeof(JJBHealth))]
    public class PlayerHealthSetup : MonoBehaviour
    {
        private JJBHealth _health;

        public JJBHealth Health => _health;

        private void Awake()
        {
            _health = GetComponent<JJBHealth>();
        }

        private void Start()
        {
            int maxHealth = PlayerProfileManager.Instance.Profile.stats.maxHealth;

            _health.Initialize(maxHealth);
        }
    }
}