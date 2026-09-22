using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "EventSelector", menuName = "SO/EventSelector")]
public abstract class EventSelectorSO : ScriptableObject
{
    public abstract EventRandom Select(
        IReadOnlyList<EventRandom> events
    );
}
