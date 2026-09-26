using UnityEngine;

public class PlayerAnimation : MonoBehaviour
{
    private Animator _animator;
    private readonly int _animatorY = Animator.StringToHash("MoveY");
    private readonly int _animatorX = Animator.StringToHash("MoveX");
    private readonly int _animatorIsMove = Animator.StringToHash("IsMove");

    private void Awake()
    {
        _animator = GetComponent<Animator>();
    }

    public void SetAnim(PlayerMovement_H moveCompo)
    {
        bool isMove = moveCompo.RbCompo.linearVelocity != Vector2.zero;
        _animator.SetBool(_animatorIsMove, isMove);
        

        if (isMove == false) return;

        _animator.SetFloat(_animatorX, moveCompo.RbCompo.linearVelocityX);
        _animator.SetFloat(_animatorY, moveCompo.RbCompo.linearVelocityY);        
    }
}
