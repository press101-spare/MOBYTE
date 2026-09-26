using JJB.Script.Battle.Enemy;
using UnityEngine;

namespace JJB.Script.Battle.Player
{
    public class PlayerDamageReceiver : MonoBehaviour
    {
        private JJBHealth _health;
        private DiceBattleAdapter _diceBattleAdapter;
        private EnemyDamageReceiver _enemyDamageReceiver;
        
        private bool _reflectionEnabled;
        private float _reflectionRate;
        private void Awake()
        {
            _health = GetComponent<JJBHealth>();
        }

        public void Initialize(DiceBattleAdapter diceBattleAdapter, EnemyDamageReceiver enemyDamageReceiver)
        {
            _diceBattleAdapter = diceBattleAdapter;
            _enemyDamageReceiver = enemyDamageReceiver;
        }

        public void TakeDamage(int damage)
        {
            if (_health.IsDead)
                return;

            if (_diceBattleAdapter != null)
                damage = _diceBattleAdapter.DamageWithShield(damage);

            if (damage <= 0)
                return;

            _health.TakeDamage(damage);
            
            if (_reflectionEnabled && _enemyDamageReceiver != null)
            {
                int reflectDamage = Mathf.CeilToInt(damage * _reflectionRate);

                _enemyDamageReceiver.TakeDamage(reflectDamage);
            }
        }
        
        public void EnableReflection(float rate)
        {
            _reflectionEnabled = true;
            _reflectionRate = Mathf.Max(0f, rate);
        }

        public void DisableReflection()
        {
            _reflectionEnabled = false;
            _reflectionRate = 0f;
        }
    }
}