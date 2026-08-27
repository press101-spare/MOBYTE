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

        public event Action<int, int> OnHealthChanged;
        public event Action OnDied;

        public void Initialize(int maxHealth)
        {
            _maxHealth = maxHealth;
            _currentHealth = maxHealth;

            OnHealthChanged?.Invoke(
                _currentHealth,
                _maxHealth
            );
        }

        public void TakeDamage(int damage)
        {
            if (damage <= 0 || _currentHealth <= 0)
                return;

            _currentHealth = Mathf.Clamp(
                _currentHealth - damage,
                0,
                _maxHealth
            );

            OnHealthChanged?.Invoke(
                _currentHealth,
                _maxHealth
            );

            if (_currentHealth == 0)
                OnDied?.Invoke();
        }

        public void Heal(int amount)
        {
            if (amount <= 0 || _currentHealth <= 0)
                return;

            _currentHealth = Mathf.Clamp(
                _currentHealth + amount,
                0,
                _maxHealth
            );

            OnHealthChanged?.Invoke(
                _currentHealth,
                _maxHealth
            );
        }
    }
}