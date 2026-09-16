using UnityEngine;

namespace JJB.Script.Battle.Player
{
    public class PlayerDamageReceiver : MonoBehaviour
    {
        private JJBHealth _health;
        private DiceBattleAdapter _diceBattleAdapter;

        private void Awake()
        {
            _health = GetComponent<JJBHealth>();
        }

        public void Initialize(DiceBattleAdapter diceBattleAdapter)
        {
            _diceBattleAdapter = diceBattleAdapter;
        }

        public void TakeDamage(int damage)
        {
            if (_health.IsDead)
                return;

            if (damage <= 0)
                return;

            if (_diceBattleAdapter != null)
                damage = _diceBattleAdapter.DamageWithShield(damage);

            if (damage <= 0)
                return;

            _health.TakeDamage(damage);
        }
    }
}