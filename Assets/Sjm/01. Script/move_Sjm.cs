using System;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class move_Sjm : MonoBehaviour
{
    private readonly int MoveY = Animator.StringToHash("MoveY");
    private readonly int MoveX = Animator.StringToHash("MoveX");
    private readonly int UpLeft = Animator.StringToHash("UpLeft");
    private readonly int UpRight = Animator.StringToHash("UpRight");
    private readonly int DownRIght = Animator.StringToHash("DownRight");
 
    private Animator _animator;
    private PlayerMovement _movement;
    public SpriteRenderer _spriteRenderer;
  
    private void Start()
    {
        _animator = GetComponent<Animator>();
        _movement = GetComponentInParent<PlayerMovement>();
        _spriteRenderer = GetComponentInParent<SpriteRenderer>();
    }
    private void FixedUpdate()
    {
        SetMoveXAnimation();
        SetMoveYAnimation();
        FlipX();
        SetUpLeftWalk();
        SetDownRIghtWalk();
    }

    private void SetMoveYAnimation()
    {
        _animator.SetFloat(MoveY, _movement._rb.linearVelocityY);
    }
    private void SetMoveXAnimation()
    {
        _animator.SetFloat(MoveX,Mathf.Abs(_movement._rb.linearVelocityX));
    }
    private void SetUpLeftWalk()
    {
    
        if(_movement._moveDir.x < -0.1 && _movement._moveDir.y > 0.1)
        {
           _animator.SetBool(UpLeft,true);
        }
       else  if (_movement._moveDir.x > 0.1 && _movement._moveDir.y > 0.1)
        {
            _animator.SetBool(UpLeft, true);
        }

        else
        {
            _animator.SetBool(UpLeft, false);
        }

    }
    private void SetDownRIghtWalk()
    {

        if (_movement._moveDir.x < -0.1 && _movement._moveDir.y < -0.1)
        {
            _animator.SetBool(DownRIght, true);
        }
        else if (_movement._moveDir.x > 0.1 && _movement._moveDir.y < -0.1)
        {
            _animator.SetBool(DownRIght, true);
        }

        else
        {
            _animator.SetBool(DownRIght, false);
        }

    }
    private void FlipX()
    {
        if (_movement._rb.linearVelocityX > 0)
        {
            _spriteRenderer.flipX = true;
        }
        else
        {
            _spriteRenderer.flipX = false;
        }
    }
  
}
