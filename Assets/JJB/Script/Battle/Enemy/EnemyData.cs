using UnityEngine;

namespace JJB.Script.Battle.Enemy
{
    [CreateAssetMenu(fileName = "EnemyData", menuName = "Game/Enemy Data")]
    public class EnemyData : ScriptableObject
    {
        [SerializeField] private string enemyName;
        [SerializeField] private int maxHealth;
        [SerializeField] private int attackPower;

        [Header("Abilities")]
        [SerializeField] private EnemyAbility[] abilities;

        public string EnemyName => enemyName;
        public int MaxHealth => maxHealth;
        public int AttackPower => attackPower;
        public EnemyAbility[] Abilities => abilities;
    }
}