using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace JJB.Script.Battle
{
    public class BattleEndPanelController : MonoBehaviour
    {
        [Header("Panel")]
        [SerializeField] private GameObject victoryPanel;
        [SerializeField] private CanvasGroup victoryCanvasGroup;

        [Header("Texts")]
        [SerializeField] private TMP_Text victoryText;
        [SerializeField] private TMP_Text expText;
        [SerializeField] private TMP_Text moneyText;

        [Header("Buttons")]
        [SerializeField] private RectTransform returnButton;
        [SerializeField] private RectTransform battleButton;

        [Header("Animation")]
        [SerializeField] private float panelFadeDuration = 0.3f;
        [SerializeField] private float victoryMoveDuration = 0.4f;
        [SerializeField] private float textFadeDuration = 0.25f;
        [SerializeField] private float buttonDuration = 0.25f;
        
        private Vector3 _returnButtonScale;
        private Vector3 _battleButtonScale;
        
        private const int VictoryExp = 5;
        private const int VictoryMoney = 200;

        private BattleTurnManager _battleTurnManager;
        private RectTransform _victoryTextRect;

        private Vector2 _victoryTextPosition;

        private Sequence _sequence;

        private void Awake()
        {
            if (returnButton != null)
                _returnButtonScale = returnButton.localScale;

            if (battleButton != null)
                _battleButtonScale = battleButton.localScale;

            if (victoryText != null)
            {
                _victoryTextRect = victoryText.GetComponent<RectTransform>();
                _victoryTextPosition = _victoryTextRect.anchoredPosition;
            }

            if (victoryPanel != null)
                victoryPanel.SetActive(false);
        }

        private void Start()
        {
            if (JJBGameManager.Instance == null)
                return;

            _battleTurnManager =
                JJBGameManager.Instance.BattleTurnManager;

            if (_battleTurnManager == null)
                return;

            _battleTurnManager.OnPhaseChanged += OnPhaseChanged;

            OnPhaseChanged(_battleTurnManager.CurrentPhase);
        }

        private void OnPhaseChanged(BattlePhase phase)
        {
            if (phase != BattlePhase.BattleEnd)
                return;

            ShowResult();
        }

        private void ShowResult()
        {
            JJBHealth playerHealth =
                JJBGameManager.Instance.PlayerJjbHealth;

            JJBHealth enemyHealth =
                JJBGameManager.Instance.EnemyJjbHealth;

            bool isVictory =
                enemyHealth != null &&
                enemyHealth.IsDead &&
                playerHealth != null &&
                !playerHealth.IsDead;

            if (!isVictory)
                return;

            if (expText != null)
                expText.text =
                    $"획득한 경험치 : {VictoryExp}";

            if (moneyText != null)
                moneyText.text =
                    $"획득한 칩 : {VictoryMoney}";

            PlayVictoryAnimation();
        }

        private void PlayVictoryAnimation()
        {
            if (victoryPanel == null)
                return;

            victoryPanel.SetActive(true);

            _sequence?.Kill();

            // 초기 상태
            if (victoryCanvasGroup != null)
                victoryCanvasGroup.alpha = 0f;

            if (victoryText != null)
            {
                victoryText.alpha = 0f;

                _victoryTextRect.anchoredPosition =
                    _victoryTextPosition + Vector2.up * 80f;
            }

            if (expText != null)
                expText.alpha = 0f;

            if (moneyText != null)
                moneyText.alpha = 0f;

            if (returnButton != null)
                returnButton.localScale = _returnButtonScale * 0.8f;

            if (battleButton != null)
                battleButton.localScale = _battleButtonScale * 0.8f;

            _sequence = DOTween.Sequence();

            // 패널 등장
            if (victoryCanvasGroup != null)
            {
                _sequence.Append(
                    victoryCanvasGroup
                        .DOFade(1f, panelFadeDuration)
                );
            }

            // VICTORY 등장
            if (victoryText != null)
            {
                _sequence.Append(
                    _victoryTextRect
                        .DOAnchorPos(
                            _victoryTextPosition,
                            victoryMoveDuration
                        )
                        .SetEase(Ease.OutBack)
                );

                _sequence.Join(
                    victoryText
                        .DOFade(1f, victoryMoveDuration)
                );
            }

            // 경험치
            if (expText != null)
            {
                _sequence.Append(
                    expText
                        .DOFade(1f, textFadeDuration)
                );
            }

            // 골드
            if (moneyText != null)
            {
                _sequence.Append(
                    moneyText
                        .DOFade(1f, textFadeDuration)
                );
            }

            if (returnButton != null)
            {
                _sequence.Append(
                    returnButton
                        .DOScale(_returnButtonScale, buttonDuration)
                        .SetEase(Ease.OutBack)
                );
            }

            if (battleButton != null)
            {
                _sequence.Join(
                    battleButton
                        .DOScale(_battleButtonScale, buttonDuration)
                        .SetEase(Ease.OutBack)
                );
            }
        }

        public void LoadScene(string sceneName)
        {
            SceneManager.LoadScene(sceneName);
        }

        private void OnDestroy()
        {
            if (_battleTurnManager != null)
                _battleTurnManager.OnPhaseChanged -= OnPhaseChanged;

            _sequence?.Kill();
        }
    }
}