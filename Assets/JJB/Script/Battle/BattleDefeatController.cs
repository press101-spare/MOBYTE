using DG.Tweening;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace JJB.Script.Battle
{
    public class BattleDefeatController : MonoBehaviour
    {
        [Header("Fade")]
        [SerializeField] private CanvasGroup fadePanel;
        [SerializeField] private float fadeDuration = 1.5f;

        [Header("Scene")]
        [SerializeField] private string titleSceneName = "Title";

        private bool _isPlaying;

        private void Awake()
        {
            if (fadePanel == null)
                return;

            fadePanel.alpha = 0f;
            fadePanel.blocksRaycasts = false;
            fadePanel.gameObject.SetActive(false);
        }

        public void PlayDefeat()
        {
            if (_isPlaying)
                return;

            _isPlaying = true;

            if (fadePanel == null)
            {
                DeleteSaveAndMoveTitle();
                return;
            }

            fadePanel.gameObject.SetActive(true);

            fadePanel.alpha = 0f;
            fadePanel.blocksRaycasts = true;

            fadePanel.DOKill();

            fadePanel
                .DOFade(1f, fadeDuration)
                .SetEase(Ease.Linear)
                .SetUpdate(true)
                .OnComplete(DeleteSaveAndMoveTitle);
        }

        private void DeleteSaveAndMoveTitle()
        {
            SaveManager saveManager =
                FindFirstObjectByType<SaveManager>();

            if (saveManager != null)
                saveManager.DeleteSaveFile();

            SceneManager.LoadScene(titleSceneName);
        }

        private void OnDestroy()
        {
            if (fadePanel != null)
                fadePanel.DOKill();
        }
    }
}