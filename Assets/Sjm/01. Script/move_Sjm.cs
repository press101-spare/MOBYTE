using UnityEngine;
using UnityEngine.InputSystem;

public class move_Sjm : MonoBehaviour
{
    public float _speed = 5f;
    private Vector2 _moveDir;
    private Rigidbody2D _rb;
    private Animator _animator;
    public SpriteRenderer _spriteRenderer;
    private void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
        _animator = GetComponent<Animator>();
    }

    private void FixedUpdate()
    {
        _rb.linearVelocity = _speed * _moveDir;
    }

    private void OnMove(InputValue value)
    {
       _moveDir = value.Get<Vector2>();
    }
    private void Update()
    {
        if (_moveDir == new Vector2(-1, 1))
        {
            _animator.SetBool("upleft", true);
            _animator.SetBool("downright", false);
        }
        else if (_moveDir == new Vector2(-1,-1))
        {
            _animator.SetBool("upleft", false);
            _animator.SetBool("downright", true);
        }
        else if (_moveDir.y < 0)
        {
            _animator.SetBool("upleft", false);
         
            _animator.SetFloat("MoveY", -0.2f);
            _animator.SetBool("side", false);
            _animator.SetBool("idle", false);
        }
        else if (_moveDir.y > 0)
        {
            _animator.SetBool("upleft", false);
         
            _animator.SetFloat("MoveY", 0.2f);
            _animator.SetBool("side", false);
            _animator.SetBool("idle", false);
        }
        else if (_moveDir.x < 0)
        {
            _animator.SetBool("upleft", false);
            _animator.SetBool("upside", false);
            _animator.SetFloat("MoveY", 0f);
            _animator.SetBool("side", true);
            _spriteRenderer.flipX = false;
            _animator.SetBool("idle", false);
        }
        else if (_moveDir.x > 0)
        {
            _animator.SetBool("upleft", false);
            _animator.SetFloat("MoveY", 0f);
            _animator.SetBool("side", true);
            _spriteRenderer.flipX = true;
            _animator.SetBool("idle", false);
        }
        else
        {
            _animator.SetBool("upleft", false);
            _animator.SetFloat("MoveY", 0f);
            _animator.SetBool("idle", true);
            _animator.SetBool("side", false);
        }


    }
}
