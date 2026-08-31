using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace JJB.Script.Battle
{
    public class HealthBarUI : MonoBehaviour
    {
        [SerializeField] private Health health;

        [SerializeField] private TMP_Text healthText;
        [SerializeField] private Image fillImage;

        private void Start()
        {
            Refresh(health.CurrentHealth, health.MaxHealth);
        }

        private void OnEnable()
        {
            health.OnHealthChanged += Refresh;
        }

        private void OnDisable()
        {
            health.OnHealthChanged -= Refresh;
        }

        private void Refresh(int current, int max)
        {
            healthText.text =
                $"{current}/{max}";

            fillImage.fillAmount =
                (float)current / max;
        }
    }
}