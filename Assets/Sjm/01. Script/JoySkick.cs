using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem.OnScreen;
using UnityEngine.UI;

public class JoySkick : MonoBehaviour,IPointerDownHandler, IDragHandler, IPointerUpHandler
{
    [SerializeField] private Image joystickBackground;
    [SerializeField] private Image joystickHandler;
    [SerializeField] private OnScreenStick joystickComPo;

    private RectTransform rectTransform;
    private int stik = int .MinValue;

    public void OnDrag(PointerEventData eventData)
    {
        if(eventData.pointerId != stik)
        {
            return;
        }
        joystickComPo.OnDrag(eventData);
    }

    public void OnPointerDown(PointerEventData eventData)
    {
       if(stik != int.MinValue)
        {
            return;
        }
        stik = eventData.pointerId;
        joystickBackground.enabled = true;
        joystickHandler.enabled = true;
       RectTransformUtility.ScreenPointToLocalPointInRectangle(rectTransform, eventData.position, eventData.pressEventCamera, out Vector2 touchPosition);
       joystickBackground.rectTransform.anchoredPosition = touchPosition;
       joystickComPo.OnPointerDown(eventData);

    }

    public void OnPointerUp(PointerEventData eventData)
    {
        if (eventData.pointerId != stik)
        {
            return;
        }
        joystickComPo.OnPointerUp(eventData);
        stik = int.MinValue;
        joystickBackground.enabled = false;
        joystickHandler.enabled = false;
    }

    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        joystickBackground.enabled = false;
        joystickHandler.enabled = false;
    }







}
