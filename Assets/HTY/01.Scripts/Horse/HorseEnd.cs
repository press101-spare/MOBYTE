using UnityEngine;

public class HorseEnd : MonoBehaviour
{
    public bool _end= false;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.TryGetComponent<HorseGame_HTY>(out HorseGame_HTY horse))
        {
            Debug.Log("겜끝");
            _end = true;
        }
    }

    public bool End()
    {
        return _end;
    }
}
