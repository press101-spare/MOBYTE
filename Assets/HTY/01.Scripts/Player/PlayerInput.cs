using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInput : MonoBehaviour
{
    public Vector2 MoveDir { get; private set; }

    public void OnMove(InputValue value)
    {
        MoveDir = value.Get<Vector2>();
    }
}
