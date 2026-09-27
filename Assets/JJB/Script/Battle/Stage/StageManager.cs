using JJB.Script.Battle.Enemy;
using UnityEngine;

namespace JJB.Script.Battle.Stage
{
    public class StageManager : MonoBehaviour
    {
        public static StageManager Instance { get; private set; }

        [SerializeField] private StageData[] stages;
        [SerializeField] private int currentStageIndex;

        private EnemyData _currentEnemy;

        public int CurrentStageIndex => currentStageIndex;
        public EnemyData CurrentEnemy => _currentEnemy;
        
        public string CurrentStageName
        {
            get
            {
                if (stages == null || stages.Length == 0)
                    return "";

                if (currentStageIndex < 0 || currentStageIndex >= stages.Length)
                    return "";

                return stages[currentStageIndex].name;
            }
        }

        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
                return;
            }

            Instance = this;
            DontDestroyOnLoad(gameObject);
        }

        public void SetStageIndex(int index)
        {
            if (stages == null || stages.Length == 0)
                return;

            if (index < 0 || index >= stages.Length)
                return;

            currentStageIndex = index;

            RefreshEnemy();
        }

        public EnemyData PrepareCurrentStage()
        {
            if (stages == null || stages.Length == 0)
            {
                Debug.LogError("StageData가 없습니다.");
                return null;
            }

            if (currentStageIndex < 0 || currentStageIndex >= stages.Length)
            {
                Debug.LogError($"잘못된 Stage Index : {currentStageIndex}");
                return null;
            }

            if (_currentEnemy == null)
                _currentEnemy = stages[currentStageIndex].GetRandomEnemy();

            return _currentEnemy;
        }

        public void NextStage()
        {
            SetStageIndex(currentStageIndex + 1);
        }

        private void RefreshEnemy()
        {
            _currentEnemy = null;

            EnemyData enemyData = PrepareCurrentStage();

            if (enemyData == null)
                return;

            EnemyHealthSetup enemyHealthSetup =
                FindFirstObjectByType<EnemyHealthSetup>();

            if (enemyHealthSetup == null)
                return;

            enemyHealthSetup.SetData(enemyData);
        }

#if UNITY_EDITOR
        private void OnValidate()
        {
            if (stages == null || stages.Length == 0)
                return;

            currentStageIndex = Mathf.Clamp(
                currentStageIndex,
                0,
                stages.Length - 1
            );

            _currentEnemy = null;

            if (!Application.isPlaying)
                return;

            RefreshEnemy();
        }
#endif
    }
}