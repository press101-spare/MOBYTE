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
        [SerializeField] private RectTransform stageIntroUI;
        [SerializeField] private RectTransform stageIntroTarget;
        [SerializeField] private CanvasGroup stageIntroCanvasGroup;
        [SerializeField] private TMP_Text stageNameText;

        [SerializeField] private float fadeDuration = 0.5f;
        [SerializeField] private float moveDuration = 0.7f;
        [SerializeField] private float endScale = 0.67f;

        private Vector2 _stageIntroStartPosition;
        private Vector3 _stageIntroStartScale;

        private BattleTurnManager _battleTurnManager;
        
        private void Awake()
        {
            if (stageIntroUI == null)
                return;

            _stageIntroStartPosition = stageIntroUI.anchoredPosition;
            _stageIntroStartScale = stageIntroUI.localScale;

            stageIntroUI.gameObject.SetActive(false);
        }

        public void Initialize(BattleTurnManager battleTurnManager)
        {
            _battleTurnManager = battleTurnManager;

            _battleTurnManager.OnPhaseChanged += UpdateUI;

            UpdateUI(_battleTurnManager.CurrentPhase);
        }

        public IEnumerator PlayStageIntro(string stageName)
        {
            if (stageIntroUI == null ||
                stageIntroTarget == null ||
                stageIntroCanvasGroup == null ||
                stageNameText == null)
                yield break;

            stageNameText.text = stageName;

            stageIntroUI.DOKill();
            stageIntroCanvasGroup.DOKill();

            stageIntroUI.gameObject.SetActive(true);

            // 처음 상태
            stageIntroUI.anchoredPosition = _stageIntroStartPosition;
            stageIntroUI.localScale = _stageIntroStartScale;
            stageIntroCanvasGroup.alpha = 0f;

            Sequence sequence = DOTween.Sequence();

            // 1. 투명 → 불투명
            sequence.Append(
                stageIntroCanvasGroup
                    .DOFade(1f, fadeDuration)
                    .SetEase(Ease.OutQuad)
            );

            // 2. 지정 위치로 이동하면서 동시에 작아짐
            sequence.Append(
                stageIntroUI
                    .DOAnchorPos(stageIntroTarget.anchoredPosition, moveDuration)
                    .SetEase(Ease.OutCubic)
            );

            sequence.Join(
                stageIntroUI
                    .DOScale(_stageIntroStartScale * endScale, moveDuration)
                    .SetEase(Ease.OutCubic)
            );

            yield return sequence.WaitForCompletion();
        }

        private void OnDestroy()
        {
            if (_battleTurnManager != null)
                _battleTurnManager.OnPhaseChanged -= UpdateUI;
            if (stageIntroUI != null)
                stageIntroUI.DOKill();
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