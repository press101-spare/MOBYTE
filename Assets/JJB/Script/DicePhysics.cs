using UnityEngine;

namespace JJB.Script
{
    public class DicePhysics : MonoBehaviour
    {
        private Rigidbody _rb;

        private void Awake()
        {
            _rb = GetComponent<Rigidbody>();
        }

        public void Throw()
        {
            _rb.linearVelocity = Vector3.zero;
            _rb.angularVelocity = Vector3.zero;

            _rb.AddForce(Vector3.up * 5f, ForceMode.Impulse);
            _rb.AddTorque(Random.insideUnitSphere * 10f, ForceMode.Impulse);
        }
    }
}