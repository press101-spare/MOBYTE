using UnityEngine;
using UnityEngine.SceneManagement;

namespace JJB.Script.Battle
{
    public class BattleEndPanelController : MonoBehaviour
    {
        [SerializeField] private GameObject battleEndPanel;

        private BattleTurnManager _battleTurnManager;

        private void Start()
        {
            if (battleEndPanel != null)
                battleEndPanel.SetActive(false);

            if (JJBGameManager.Instance == null ||
                JJBGameManager.Instance.BattleTurnManager == null)
            {
                Debug.LogError("BattleTurnManager를 찾을 수 없습니다.", this);
                return;
            }

            _battleTurnManager =
                JJBGameManager.Instance.BattleTurnManager;

            _battleTurnManager.OnPhaseChanged += OnPhaseChanged;

            OnPhaseChanged(_battleTurnManager.CurrentPhase);
        }

        private void OnDestroy()
        {
            if (_battleTurnManager != null)
                _battleTurnManager.OnPhaseChanged -= OnPhaseChanged;
        }

        private void OnPhaseChanged(BattlePhase phase)
        {
            if (phase != BattlePhase.BattleEnd)
                return;

            if (battleEndPanel != null)
                battleEndPanel.SetActive(true);
        }

        public void LoadScene(string sceneName)
        {
            SceneManager.LoadScene(sceneName);
        }
    }
}