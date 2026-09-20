using UnityEngine;

namespace JJB.Script.Battle.Enemy.EnemyAbilities
{
    [CreateAssetMenu(fileName = "PaybackAbility", menuName = "JJB/Enemy Ability/Payback")]
    public class PaybackAbility : EnemyAbility
    {
        private int _storedPlayerDamage;
        private string _storedTree;

        public void RecordTree(string tree)
        {
            _storedTree = tree;
        }

        public override int ModifyIncomingDamage(int damage, JJBHealth health)
        {
            _storedPlayerDamage += damage;

            return damage;
        }

        public override int ModifyAttackDamage(int damage)
        {
            if (_storedPlayerDamage <= 0)
                return damage;
            
            return _storedPlayerDamage;
        }

        public override void AfterAttack()
        {
            _storedPlayerDamage = 0;
            _storedTree = null;
        }
    }
}