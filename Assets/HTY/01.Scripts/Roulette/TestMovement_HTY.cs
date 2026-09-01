using UnityEngine;
using UnityEngine.InputSystem;
[RequireComponent(typeof(Rigidbody2D))]
public class TestMovement_HTY : MonoBehaviour
{
    private Rigidbody2D _rb;
    [SerializeField] private float _speed = 3f;
    private Vector2 _moveDir;
    private void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate()
    {
        _rb.linearVelocity = _moveDir * _speed;
    }

    public void OnMove(InputValue value)
    {
        _moveDir = value.Get<Vector2>();
    }
}
