using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class TouchSceneLoader : MonoBehaviour, IPointerClickHandler
{
    [SerializeField] private string nextSceneName = "MainGame";

    private bool _isLoading;

    public void OnPointerClick(PointerEventData eventData)
    {
        if (_isLoading)
        {
            return;
        }

        if (!Application.CanStreamedLevelBeLoaded(nextSceneName))
        {
            Debug.LogError(
                $"'{nextSceneName}' 씬을 찾을 수 없습니다. " +
                "Build Profiles와 씬 이름을 확인하세요."
            );

            return;
        }

        _isLoading = true;
        SceneManager.LoadScene(nextSceneName);
    }
}