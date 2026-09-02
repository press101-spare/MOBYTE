using UnityEngine;

namespace JJB.Script.Battle.Enemy
{
    public abstract class EnemyAbility : ScriptableObject
    {
        public abstract void Execute(
            EnemyData enemyData,
            Health selfHealth,
            Health targetHealth
        );
    }
}