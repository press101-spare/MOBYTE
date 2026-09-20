using UnityEngine;

namespace JJB.Script.Battle.Enemy
{
    [RequireComponent(typeof(EnemyHealthSetup))]
    public class EnemyDamageReceiver : MonoBehaviour
    {
        private EnemyHealthSetup _enemy;
        private EnemyHitFlash _hitFlash;

        private void Awake()
        {
            _enemy = GetComponent<EnemyHealthSetup>();
            _hitFlash = GetComponent<EnemyHitFlash>();
        }

        public void TakeDamage(int damage)
        {
            if (_enemy.Health.IsDead)
                return;

            if (_enemy.Ability != null)
                damage = _enemy.Ability.ModifyIncomingDamage(damage, _enemy.Health);

            _enemy.Health.TakeDamage(damage);
            
            _hitFlash?.Play();
        }
    }
}