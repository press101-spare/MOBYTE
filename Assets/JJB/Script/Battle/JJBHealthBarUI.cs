using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace JJB.Script.Battle
{
    public class JJBHealthBarUI : MonoBehaviour
    {
        [FormerlySerializedAs("health")] [SerializeField] private JJBHealth jjbHealth;

        [SerializeField] private TMP_Text healthText;
        [SerializeField] private Image fillImage;

        private void Start()
        {
            Refresh(jjbHealth.CurrentHealth, jjbHealth.MaxHealth);
        }

        private void OnEnable()
        {
            jjbHealth.OnHealthChanged += Refresh;
        }

        private void OnDisable()
        {
            jjbHealth.OnHealthChanged -= Refresh;
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