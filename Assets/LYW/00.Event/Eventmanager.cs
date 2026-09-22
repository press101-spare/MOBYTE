using UnityEngine;

public class EventManager : MonoBehaviour
{
    [SerializeField]
    private EventSo eventSo;

    [SerializeField]
    private EventSelectorSO eventSelector;

    public EventRandom GetRandomEvent()
    {
        if (eventSo == null || eventSelector == null)
            return null;

        return eventSelector.Select(eventSo.Events);
    }

    public void StartRandomEvent()
    {
        EventRandom selectedEvent = GetRandomEvent();

        if (selectedEvent == null)
            return;

        Debug.Log(
            "선택된 이벤트 : " +
            selectedEvent.eventName
        );
    }
}