using UnityEngine;
using UnityEngine.InputSystem;

namespace JJB.Script
{
    public class ElevatorPlayerMovement : MonoBehaviour
    {
        [SerializeField] private SimpleJoystick joystick;
        [SerializeField] private float moveSpeed = 5f;

        private Rigidbody2D _rb;

        private void Awake()
        {
            _rb = GetComponent<Rigidbody2D>();
        }

        private void FixedUpdate()
        {
            float moveX = joystick.Direction.x;

            _rb.linearVelocity = new Vector2(
                moveX * moveSpeed,
                _rb.linearVelocity.y
            );
        }
    }
}