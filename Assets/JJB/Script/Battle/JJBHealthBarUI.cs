using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace JJB.Script.Battle
{
    public class JjbHealthBarUI : MonoBehaviour
    {
        [SerializeField] private JJBHealth health;
        [SerializeField] private Slider healthSlider;
        [SerializeField] private TMP_Text healthText;

        private void OnEnable()
        {
            if (health == null)
            {
                Debug.LogError("JJBHealth가 연결되지 않았습니다.", this);
                return;
            }

            health.OnHealthChanged += UpdateUI;

            UpdateUI(health.CurrentHealth, health.MaxHealth);
        }

        private void OnDisable()
        {
            if (health == null)
                return;

            health.OnHealthChanged -= UpdateUI;
        }

        private void UpdateUI(int currentHealth, int maxHealth)
        {
            if (healthSlider != null)
            {
                healthSlider.maxValue = maxHealth;
                healthSlider.value = currentHealth;

                if (healthSlider.fillRect != null)
                    healthSlider.fillRect.gameObject.SetActive(currentHealth > 0);
            }

            if (healthText != null)
                healthText.text = $"{currentHealth} / {maxHealth}";
        }
    }
}