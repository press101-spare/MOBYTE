using UnityEngine;

namespace JJB.Script
{
    public class JJB_DicePhysics : MonoBehaviour
    {
        [Header("Throw")]
        [SerializeField] private float xForce = 2f;
        [SerializeField] private float yForce = 4f;
        [SerializeField] private float liftForce = 4f;
        [SerializeField] private float torqueForce = 10f;

        [Header("Gravity")]
        [SerializeField] private float gravityForce = 9.81f;

        private Rigidbody _rb;

        private void Awake()
        {
            _rb = GetComponent<Rigidbody>();
            
            _rb.useGravity = false;
        }

        private void FixedUpdate()
        {
            _rb.AddForce(
                Vector3.forward * gravityForce,
                ForceMode.Acceleration
            );
        }

        public void Throw()
        {
            _rb.linearVelocity = Vector3.zero;
            _rb.angularVelocity = Vector3.zero;

            Vector3 force = new Vector3(
                Random.Range(-xForce, xForce),
                Random.Range(-yForce, yForce),
                liftForce
            );

            _rb.AddForce(force, ForceMode.Impulse);

            _rb.AddTorque(
                Random.insideUnitSphere * torqueForce,
                ForceMode.Impulse
            );
        }
    }
}