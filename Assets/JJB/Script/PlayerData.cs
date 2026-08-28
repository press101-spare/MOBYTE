using UnityEngine;

namespace JJB.Script
{
    [CreateAssetMenu(fileName = "PlayerData", menuName = "Game/Player Data")]
    public class PlayerData : ScriptableObject
    {
        [SerializeField] private int maxHealth = 100;
        [SerializeField] private int attackPower = 10;
        [SerializeField] private int defense = 0;
        
        public int MaxHealth => maxHealth;
        public int AttackPower => attackPower;
        public int Defense => defense;
    }
}