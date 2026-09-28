using DG.Tweening;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

namespace JJB.Script
{
    [RequireComponent(typeof(Animator))]
    [RequireComponent(typeof(Collider2D))]
    public class Elevator : MonoBehaviour
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

        private static readonly int IsOpenHash =
            Animator.StringToHash("Open");

        private Animator _animator;
        private Collider2D _collider;
        private Camera _camera;

        private bool _canInteract;
        private bool _isMoving;

        private void Awake()
        {
            _animator = GetComponent<Animator>();
            _collider = GetComponent<Collider2D>();
            _camera = Camera.main;

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
            CheckDistance();
            CheckInput();
        }

        private void CheckDistance()
        {
            if (player == null)
                return;

            float distance = Vector2.Distance(
                player.position,
                transform.position
            );

            _canInteract = distance <= interactDistance;

            if (outlineObject != null)
                outlineObject.SetActive(_canInteract && !_isMoving);
        }

        private void CheckInput()
        {
            if (!_canInteract || _isMoving)
                return;

            if (Pointer.current == null)
                return;

            if (!Pointer.current.press.wasPressedThisFrame)
                return;

            Vector2 screenPosition =
                Pointer.current.position.ReadValue();

            Vector2 worldPosition =
                _camera.ScreenToWorldPoint(screenPosition);

            if (!_collider.OverlapPoint(worldPosition))
                return;

            Interact();
        }

        private void Interact()
        {
            _isMoving = true;

            if (outlineObject != null)
                outlineObject.SetActive(false);

            Debug.Log("엘리베이터 클릭 성공");

            _animator.SetBool(IsOpenHash, true);
        }

        // Open 애니메이션 마지막 프레임 Animation Event
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