using System;
using UnityEngine;

namespace JJB.Script.Battle
{
    public class JJBHealth : MonoBehaviour
    {
        private bool _oneHpReviveEnabled;
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

            CurrentHealth -= damage;
            
            if (CurrentHealth <= 0 && _oneHpReviveEnabled)
            {
                _oneHpReviveEnabled = false;
                CurrentHealth = 1;

                OnHealthChanged?.Invoke(CurrentHealth, MaxHealth);

                return;
            }
            
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
        
        public void IncreaseMaxHealth(int amount)
        {
            if (amount <= 0 || IsDead)
                return;

            MaxHealth += amount;
            CurrentHealth += amount;

            OnHealthChanged?.Invoke(CurrentHealth, MaxHealth);
        }
        
        public void EnableOneHpRevive()
        {
            _oneHpReviveEnabled = true;
        }
    }
}