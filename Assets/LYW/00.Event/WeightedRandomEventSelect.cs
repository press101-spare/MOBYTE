using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(
    fileName = "WeightedRandomSelector",
    menuName = "SO/Event/WeightedRandomSelector"
)]
public class WeightedRandomEventSelector : EventSelectorSO
{
    public override EventRandom Select(
        IReadOnlyList<EventRandom> events
    )
    {
        if (events == null || events.Count == 0)
            return null;

        int totalWeight = 0;

        foreach (EventRandom eventData in events)
        {
            if (eventData == null)
                continue;

            totalWeight += eventData.weight;
        }

        if (totalWeight <= 0)
            return null;

        int randomValue =
            UnityEngine.Random.Range(0, totalWeight);

        foreach (EventRandom eventData in events)
        {
            if (eventData == null)
                continue;

            randomValue -= eventData.weight;

            if (randomValue < 0)
                return eventData;
        }

        return null;
    }
}