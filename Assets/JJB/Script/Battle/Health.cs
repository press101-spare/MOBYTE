using System;
using UnityEngine;

namespace JJB.Script
{
    public class Health : MonoBehaviour
    {
        private int _maxHealth;
        private int _currentHealth;

        public int MaxHealth => _maxHealth;
        public int CurrentHealth => _currentHealth;
        public bool IsDead => _currentHealth <= 0;

        public event Action<int, int> OnHealthChanged;
        public event Action OnDied;

        public void Initialize(int maxHealth)
        {
            _maxHealth = Mathf.Max(1, maxHealth);
            _currentHealth = _maxHealth;
            OnHealthChanged?.Invoke(_currentHealth, _maxHealth);
        }

        public void TakeDamage(int damage)
        {
            if (damage <= 0 || IsDead)
                return;
            
            _currentHealth = Mathf.Max(0, _currentHealth - damage);
            OnHealthChanged?.Invoke(_currentHealth, _maxHealth);

            if (IsDead)
                OnDied?.Invoke();
        }

        public void Heal(int amount)
        {
            if (amount <= 0 || IsDead)
                return;

            _currentHealth = Mathf.Min(_maxHealth, _currentHealth + amount);
            OnHealthChanged?.Invoke(_currentHealth, _maxHealth);
        }
    }
}