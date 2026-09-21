using JJB.Script.Battle.Enemy;
using UnityEngine;

namespace JJB.Script.Battle.Stage
{
    public class StageManager : MonoBehaviour
    {
        [SerializeField] private StageData[] stages;
        [SerializeField] private int currentStageIndex;

        private EnemyData _currentEnemy;

        public int CurrentStageIndex => currentStageIndex;
        public EnemyData CurrentEnemy => _currentEnemy;

        public EnemyData PrepareCurrentStage()
        {
            if (stages == null || stages.Length == 0)
            {
                Debug.LogError("StageData가 없습니다.");
                return null;
            }

            if (currentStageIndex < 0 || currentStageIndex >= stages.Length)
            {
                Debug.LogError("스테이지 인덱스가 범위를 벗어났습니다.");
                return null;
            }

            if (_currentEnemy == null)
                _currentEnemy = stages[currentStageIndex].GetRandomEnemy();

            return _currentEnemy;
        }

        public void NextStage()
        {
            currentStageIndex++;
            _currentEnemy = null;
        }
    }
}