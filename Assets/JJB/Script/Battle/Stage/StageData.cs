using JJB.Script.Battle.Enemy;
using UnityEngine;

namespace JJB.Script.Battle.Stage
{
    [CreateAssetMenu(fileName = "StageData", menuName = "Game/Stage Data")]
    public class StageData : ScriptableObject
    {
        [SerializeField] private EnemyData[] enemies;

        public EnemyData GetRandomEnemy()
        {
            if (enemies == null || enemies.Length == 0)
            {
                Debug.LogError($"{name}에 적이 없습니다.");
                return null;
            }

            return enemies[Random.Range(0, enemies.Length)];
        }
    }
}