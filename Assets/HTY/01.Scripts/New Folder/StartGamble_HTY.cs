using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class StartGamble_HTY : MonoBehaviour, IPointerDownHandler
{
    [SerializeField] private TitleAnim ani;
    [SerializeField] private string sceneName;
    public void OnPointerDown(PointerEventData eventData)
    {
        if (ani._canNext)
        {
            SceneManager.LoadScene(sceneName);
        }
    }
}
