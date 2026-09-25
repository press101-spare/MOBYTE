using UnityEngine;

[CreateAssetMenu(fileName = "EventRandom", menuName = "SO/Event/Event")]
public class EventRandom : ScriptableObject
{
    public string eventName;

    [TextArea]
    public string description;

    [Min(0)]
    public int weight = 10;

    public Sprite eventImage;
}