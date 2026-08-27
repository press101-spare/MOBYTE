using UnityEngine;
using UnityEngine.UI;

namespace JJB.Script
{
    public class HealthBarUI : MonoBehaviour
    {
        [SerializeField] private Image healthFill;

        private Health _health;

        public void Bind(Health health)
        {
            if (_health != null)
                _health.OnHealthChanged -= UpdateBar;

            _health = health;
            _health.OnHealthChanged += UpdateBar;

            UpdateBar(_health.CurrentHealth, _health.MaxHealth);
        }

        private void UpdateBar(int current, int max)
        {
            healthFill.fillAmount = (float)current / max;
        }
    }
}