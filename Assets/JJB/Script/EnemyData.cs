using UnityEngine;

namespace JJB.Script
{
    [CreateAssetMenu(fileName = "EnemyData", menuName = "Game/Enemy Data")]
    public class EnemyData : ScriptableObject
    {
        [SerializeField] private string enemyName;

        [SerializeField] private int maxHealth = 100;
        [SerializeField] private int attackPower = 10;
        [SerializeField] private int defense = 0;

        public string EnemyName => enemyName;
        public int MaxHealth => maxHealth;
        public int AttackPower => attackPower;
        public int Defense => defense;
    }
}