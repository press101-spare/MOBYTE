using UnityEngine;
using UnityEngine.InputSystem;
[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(PlayerInput))]
public class PlayerMovement_HTY : MonoBehaviour
{
    public Vector2 _moveDir;
    public float _moveSpeed;
    private Rigidbody2D _rb;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
    }
    private void FixedUpdate()
    {
        _rb.linearVelocity = _moveDir * _moveSpeed;
    }
    public void OnMove(InputValue value)
    {
        _moveDir = value.Get<Vector2>();
    }
}
