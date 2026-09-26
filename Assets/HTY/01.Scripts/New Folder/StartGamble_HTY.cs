using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class StartGamble_HTY : MonoBehaviour, IPointerDownHandler
{
    [SerializeField] private TitleAnim ani;
    public void OnPointerDown(PointerEventData eventData)
    {
        if(ani._canNext)
        {
            GambleManager.instance._casino.SetActive(true);
            GambleManager.instance._title.SetActive(false);
        }
        
    }
}
