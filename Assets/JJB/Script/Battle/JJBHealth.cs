using System;
using UnityEngine;

namespace JJB.Script.Battle
{
    public class JJBHealth : MonoBehaviour
    {
        public int CurrentHealth { get; private set; }
        public int MaxHealth { get; private set; }

        public bool IsDead => CurrentHealth <= 0;

        public event Action<int, int> OnHealthChanged;
        public event Action OnDied;

        public void Initialize(int maxHealth)
        {
            MaxHealth = Mathf.Max(1, maxHealth);
            CurrentHealth = MaxHealth;

            OnHealthChanged?.Invoke(CurrentHealth, MaxHealth);
        }

        public void TakeDamage(int damage)
        {
            if (IsDead)
                return;

            damage = Mathf.Max(0, damage);

            CurrentHealth = Mathf.Max(0, CurrentHealth - damage);

            Debug.Log($"{gameObject.name} 데미지 {damage} / HP : {CurrentHealth}/{MaxHealth}");

            OnHealthChanged?.Invoke(CurrentHealth, MaxHealth);

            if (IsDead)
                OnDied?.Invoke();
        }

        public void Heal(int amount)
        {
            if (IsDead)
                return;

            amount = Mathf.Max(0, amount);

            CurrentHealth = Mathf.Min(MaxHealth, CurrentHealth + amount);

            OnHealthChanged?.Invoke(CurrentHealth, MaxHealth);
        }
    }
}