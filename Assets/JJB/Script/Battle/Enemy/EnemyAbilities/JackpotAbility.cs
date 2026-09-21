using UnityEngine;

namespace JJB.Script.Battle.Enemy.EnemyAbilities
{
    [CreateAssetMenu(fileName = "JackpotAbility", menuName = "JJB/Enemy Ability/Jackpot")]
    public class JackpotAbility : EnemyAbility
    {
        private int _attackCount;
        private int _storedPlayerDamage;

        public override int ModifyIncomingDamage(int damage, JJBHealth health)
        {
            _storedPlayerDamage += damage;

            return damage;
        }

        public override int ModifyAttackDamage(int damage)
        {
            _attackCount++;

            if (_attackCount < 3)
                return damage;

            int jackpotDamage = _storedPlayerDamage;

            _attackCount = 0;
            _storedPlayerDamage = 0;

            return jackpotDamage;
        }
    }
}