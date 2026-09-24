using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "EventSo", menuName = "SO/Event/EventList")]
public class EventSo : ScriptableObject
{
    [SerializeField]
    private List<EventRandom> events;

    public IReadOnlyList<EventRandom> Events => events;
}