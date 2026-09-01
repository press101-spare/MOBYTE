using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;

namespace JJB.Script.Battle.Enemy
{
    [CreateAssetMenu(fileName = "EnemyData", menuName = "Game/Enemy Data")]
    public class EnemyData : ScriptableObject
    {
        [SerializeField] private string enemyName;

        [Header("Stats")]
        [SerializeField] private int maxHealth = 50;
        [SerializeField] private int attackPower = 5;
        [SerializeField] private int defense = 0;

        public UnityEvent onEnemyability;

        public string EnemyName => enemyName;
        public int MaxHealth => maxHealth;
        public int AttackPower => attackPower;
        public int Defense => defense;
    }
}