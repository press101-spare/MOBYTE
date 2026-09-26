using UnityEngine;

public class Player_H : MonoBehaviour
{
    public PlayerInput InputCompo { get; private set; }
    public PlayerMovement_H MoveCompo { get; private set; }
    public PlayerAnimation AnimCompo { get; private set; }

    private void Awake()
    {
        InputCompo = GetComponent<PlayerInput>();
        MoveCompo = GetComponent<PlayerMovement_H>();

        AnimCompo = GetComponentInChildren<PlayerAnimation>();
    }

    private void Update()
    {
        MoveCompo.SetDir(InputCompo.MoveDir);
        AnimCompo.SetAnim(MoveCompo);
    }
}
