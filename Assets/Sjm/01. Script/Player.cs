using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    public UnityEvent<Vector2> onMovement;

    public void OnMove(InputValue value)
    {
        onMovement?.Invoke(value.Get<Vector2>());
    }
}
