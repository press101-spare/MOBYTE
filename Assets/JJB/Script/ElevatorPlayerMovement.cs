using UnityEngine;
using UnityEngine.InputSystem;

namespace JJB.Script
{
    public class ElevatorPlayerMovement : MonoBehaviour
    {
        [SerializeField] private float speed;
        private Vector2 _dir;
        private Rigidbody2D _rb;

        private void Awake()
        {
            _rb = GetComponent<Rigidbody2D>();
        }

        private void FixedUpdate()
        {
            _rb.linearVelocityX =  _dir.x * speed;
        }

        private void OnMove(InputValue value)
        {
            _dir = value.Get<Vector2>();
        }
    }
}