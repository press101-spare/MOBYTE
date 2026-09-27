using UnityEngine;

public class RoulettePointer : MonoBehaviour
{
    public RouletteSlot CurrentSlot { get; private set; }

    private void OnTriggerStay2D(Collider2D other)
    {
        RouletteSlot slot = other.GetComponent<RouletteSlot>();

        if (slot != null)
        {
            CurrentSlot = slot;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        RouletteSlot slot = other.GetComponent<RouletteSlot>();

        if (slot == CurrentSlot)
        {
            CurrentSlot = null;
        }
    }
}