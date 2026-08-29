using UnityEngine;

namespace JJB.Script.Battle
{
    [CreateAssetMenu(fileName = "EnemyData", menuName = "Game/Enemy Data")]
    public class EnemyData : ScriptableObject
    {
        [SerializeField] private string enemyName;

        [Header("Stats")]
        [SerializeField] private int maxHealth = 50;
        [SerializeField] private int attackPower = 5;
        [SerializeField] private int defense = 0;

        public string EnemyName => enemyName;
        public int MaxHealth => maxHealth;
        public int AttackPower => attackPower;
        public int Defense => defense;
    }
}