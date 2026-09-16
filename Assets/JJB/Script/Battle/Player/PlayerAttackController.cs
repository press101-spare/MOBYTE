using JJB.Script.Battle.Enemy;
using JJB.Script.Battle.Player.Progression;
using UnityEngine;

namespace JJB.Script.Battle.Player
{
    public class PlayerAttackController : MonoBehaviour
    {
        private EnemyDamageReceiver _enemyDamageReceiver;

        public void Initialize(EnemyDamageReceiver enemyDamageReceiver)
        {
            _enemyDamageReceiver = enemyDamageReceiver;
        }

        public bool Attack(int damage)
        {
            if (_enemyDamageReceiver == null)
            {
                Debug.LogError("EnemyDamageReceiver가 연결되지 않았습니다.");
                return false;
            }

            if (damage <= 0)
                return false;

            int finalDamage = CalculateDamage(damage);

            _enemyDamageReceiver.TakeDamage(finalDamage);

            return true;
        }

        private int CalculateDamage(int damage)
        {
            if (PlayerProfileManager.Instance == null)
                return damage;

            int attackPower = PlayerProfileManager.Instance.Profile.stats.attackPower;
            
            return damage + attackPower;
        }
    }
}