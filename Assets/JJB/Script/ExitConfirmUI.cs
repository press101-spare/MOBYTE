using UnityEngine;

namespace JJB.Script
{
    #if UNITY_EDITOR
        using UnityEditor;
    #endif
    public class ExitConfirmUI : MonoBehaviour
    {
        [SerializeField] private GameObject exitConfirmPanel;

        private void Awake()
        {
            exitConfirmPanel.SetActive(false);
        }

        public void Open()
        {
            exitConfirmPanel.SetActive(true);
        }

        public void Close()
        {
            exitConfirmPanel.SetActive(false);
        }

        public void QuitGame()
        {
            #if UNITY_EDITOR
                EditorApplication.isPlaying = false;
            #else
                Application.Quit();
            #endif
        }
    }
}