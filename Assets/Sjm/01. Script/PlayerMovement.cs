using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float _moveSpeed = 5f;
    public Vector2 _moveDir;
    public Rigidbody2D _rb;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate()
    {
        _rb.linearVelocity = _moveSpeed * _moveDir.normalized;
    }
    
    private void OnMove(InputValue value)
    {
        _moveDir = value.Get<Vector2>();
    }
}
