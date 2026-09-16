using UnityEngine;

namespace JJB.Script.Battle.Enemy
{
    public abstract class EnemyAbility : ScriptableObject
    {
        public virtual int ModifyAttackDamage(int damage)
        {
            return damage;
        }
        public virtual int ModifyIncomingDamage(int damage, JJBHealth health)
        {
            return damage;
        }

        public virtual void AfterAttack()
        {
            
        }
    }
}