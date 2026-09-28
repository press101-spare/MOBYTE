using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.U2D.Animation;

namespace JJB.Script.Battle.Enemy
{
    [CreateAssetMenu(fileName = "EnemyData", menuName = "JJB/Enemy Data")]
    public class EnemyData : ScriptableObject
    {
        [SerializeField] private string enemyName;
        [SerializeField] private int maxHealth;
        [SerializeField] private int attackPower;
        [SerializeField] private int attackPower2;
        [SerializeField] private int attackPower3;
        [SerializeField] private SpriteLibraryAsset spriteLibraryAsset;

        [Header("Abilities")]
        [SerializeField] private EnemyAbility ability;

        public string EnemyName => enemyName;
        public int MaxHealth => maxHealth;
        public int AttackPower => attackPower;
        public int AttackPower2 => attackPower2;
        public int AttackPower3 => attackPower3;
        public EnemyAbility Ability => ability;
        public SpriteLibraryAsset SpriteLibraryAsset => spriteLibraryAsset;
    }
}