using UnityEngine;

public class PlayerMovement_H : MonoBehaviour
{
    public Rigidbody2D RbCompo { get; private set; }
    [SerializeField] private float _speed = 5f;

    private Vector2 _moveDir;

    private void Awake()
    {
        RbCompo = GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate()
    {
        Move();
    }

    private void Move()
    {
        RbCompo.linearVelocity = _moveDir * _speed;
    }

    public void SetDir(Vector2 inputVector)
    {
        _moveDir = inputVector;
    }
}
