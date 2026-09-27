using System.Collections;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace JJB.Script.Battle
{
    public class BattleUIController : MonoBehaviour
    {
        [Header("Battle Button")]
        [SerializeField] private Button attackButton;
        [SerializeField] private Button turnEndButton;

        [Header("Stage Intro")]
        [SerializeField] private RectTransform stagePanel;
        [SerializeField] private Image stagePanelImage;
        [SerializeField] private TMP_Text stageText;

        [SerializeField] private RectTransform targetPosition;
        [SerializeField] private float fadeDuration = 0.5f;
        [SerializeField] private float moveDuration = 0.7f;
        [SerializeField] private float targetScale = 0.67f;

        private Vector2 _startPosition;
        private Vector3 _startScale;

        private BattleTurnManager _battleTurnManager;
        
        private void Awake()
        {
            _startPosition = stagePanel.anchoredPosition;
            _startScale = stagePanel.localScale;

            stagePanel.gameObject.SetActive(false);
        }

        public void Initialize(BattleTurnManager battleTurnManager)
        {
            _battleTurnManager = battleTurnManager;

            _battleTurnManager.OnPhaseChanged += UpdateUI;

            UpdateUI(_battleTurnManager.CurrentPhase);
        }

        public IEnumerator PlayStageIntro(string stageName)
        {
            stageText.text = stageName;

            stagePanel.DOKill();
            stagePanelImage.DOKill();
            stageText.DOKill();

            stagePanel.gameObject.SetActive(true);

            stagePanel.anchoredPosition = _startPosition;
            stagePanel.localScale = _startScale;

            Color panelColor = stagePanelImage.color;
            panelColor.a = 0f;
            stagePanelImage.color = panelColor;

            Color textColor = stageText.color;
            textColor.a = 0f;
            stageText.color = textColor;

            Sequence sequence = DOTween.Sequence();

            // 1. Fade In
            sequence.Append(
                stagePanelImage.DOFade(1f, fadeDuration)
            );

            sequence.Join(
                stageText.DOFade(1f, fadeDuration)
            );
            
            sequence.AppendInterval(0.2f);

            // 2. Fade가 완전히 끝난 다음 이동 시작
            sequence.Append(
                stagePanel.DOAnchorPos(
                    targetPosition.anchoredPosition,
                    moveDuration
                ).SetEase(Ease.OutCubic)
            );

            // 3. 이동과 크기 축소는 동시에
            sequence.Join(
                stagePanel.DOScale(
                    _startScale * targetScale,
                    moveDuration
                ).SetEase(Ease.OutCubic)
            );

            yield return sequence.WaitForCompletion();
        }

        private void OnDestroy()
        {
            if (_battleTurnManager != null)
                _battleTurnManager.OnPhaseChanged -= UpdateUI;
            
            if (stagePanel != null)
                stagePanel.DOKill();

            if (stagePanelImage != null)
                stagePanelImage.DOKill();

            if (stageText != null)
                stageText.DOKill();
        }

        private void UpdateUI(BattlePhase phase)
        {
            SetButtonState(attackButton, phase == BattlePhase.HandSelect, HexColor("#0089FF"));
            SetButtonState(turnEndButton, phase == BattlePhase.TurnEnd, Color.green);
        }
        
        private Color HexColor(string hex)
        {
            ColorUtility.TryParseHtmlString(hex, out Color color);
            return color;
        }
        
        private void SetButtonState(Button button, bool isActive, Color activeColor)
        {
            button.interactable = isActive;

            TMP_Text text = button.GetComponentInChildren<TMP_Text>(true);
            Outline outline = button.GetComponent<Outline>();

            if (text != null)
                text.color = isActive ? activeColor : Color.gray;

            if (outline != null)
            {
                outline.enabled = true;
                outline.effectColor = isActive ? activeColor : Color.gray;

                if (isActive)
                    outline.effectColor = activeColor;
            }
        }
    }
}