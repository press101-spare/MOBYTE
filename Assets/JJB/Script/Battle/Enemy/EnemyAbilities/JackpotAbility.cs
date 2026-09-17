using UnityEngine;

namespace JJB.Script.Battle.Enemy.EnemyAbilities
{
    [CreateAssetMenu(fileName = "JackpotAbility", menuName = "Game/Enemy Ability/Jackpot")]
    public class JackpotAbility : EnemyAbility
    {
        private int _attackCount;
        private int _storedDamage;

        public override int ModifyAttackDamage(int damage)
        {
            _attackCount++;
            _storedDamage += damage;

            if (_attackCount < 3)
                return damage;

            int jackpotDamage = _storedDamage;

            _attackCount = 0;
            _storedDamage = 0;

            return jackpotDamage;
        }
    }
}