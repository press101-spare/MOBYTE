using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

namespace JJB.Script
{
    public class Elevator : MonoBehaviour, IPointerDownHandler
    {
        [Header("Player")] 
        [SerializeField] private Transform player;
        [SerializeField] private float interactDistance = 2f;

        [Header("Outline")] 
        [SerializeField] private GameObject outlineObject;

        [Header("Scene Move")] 
        [SerializeField] private string sceneName;

        [SerializeField] private CanvasGroup fadePanel;
        [SerializeField] private float fadeDuration = 0.5f;
        
        private Animator _animator;

        private static readonly int IsOpen = Animator.StringToHash("Open");

        private bool _canInteract;
        private bool _isMoving;

        private void Awake()
        {
            _animator = GetComponent<Animator>();
            
            if (outlineObject != null)
                outlineObject.SetActive(false);

            if (fadePanel != null)
            {
                fadePanel.alpha = 0f;
                fadePanel.blocksRaycasts = false;
            }
        }

        private void Update()
        {
            if (player == null)
                return;

            float distance = Vector2.Distance(
                player.position,
                transform.position
            );

            _canInteract = distance <= interactDistance;

            if (outlineObject != null)
            {
                outlineObject.SetActive(
                    _canInteract && !_isMoving
                );
            }
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            if (!_canInteract)
                return;

            if (_isMoving)
                return;

            _isMoving = true;

            if (outlineObject != null)
                outlineObject.SetActive(false);

            if (_animator != null)
                _animator.SetBool(IsOpen, true);
        }

        // Elevator Open 애니메이션 마지막 프레임에서
        // Animation Event로 호출
        public void OnElevatorOpenFinished()
        {
            FadeAndMoveScene();
        }

        private void FadeAndMoveScene()
        {
            if (fadePanel == null)
            {
                SceneManager.LoadScene(sceneName);
                return;
            }

            fadePanel.blocksRaycasts = true;

            fadePanel.DOKill();

            fadePanel
                .DOFade(1f, fadeDuration)
                .SetEase(Ease.InOutSine)
                .OnComplete(() => { SceneManager.LoadScene(sceneName); });
        }

        private void OnDestroy()
        {
            if (fadePanel != null)
                fadePanel.DOKill();
        }
    }
}