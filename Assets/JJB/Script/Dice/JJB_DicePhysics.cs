using System.Collections;
using DG.Tweening;
using UnityEngine;

namespace JJB.Script
{
    public class JJB_DicePhysics : MonoBehaviour
    {
        [Header("Throw")]
        [SerializeField] private float xForce = 8f;
        [SerializeField] private float yForce = 8f;
        [SerializeField] private float liftForce = 4f;
        [SerializeField] private float torqueForce = 5f;

        private Rigidbody _rb;

        private void Awake()
        {
            _rb = GetComponent<Rigidbody>();
            
            // 💡 유니티 기본 중력 사용
            _rb.useGravity = true;
            _rb.isKinematic = true; // 처음에는 가만히 있도록 설정
        }

        public void Throw(float forceMultiplier = 1f)
        {
            _rb.isKinematic = false;
            _rb.linearVelocity = Vector3.zero;
            _rb.angularVelocity = Vector3.zero;

            Vector3 force = new Vector3(
                Random.Range(-xForce, xForce),
                Random.Range(-yForce, yForce),
                liftForce
            ) * forceMultiplier;

            _rb.AddForce(force, ForceMode.Impulse);

            _rb.AddTorque(
                Random.insideUnitSphere * torqueForce * forceMultiplier,
                ForceMode.Impulse
            );
        }
        
        public void RerollThrow(float value)
        {
            _rb.isKinematic = false;
            _rb.linearVelocity = Vector3.zero;
            _rb.angularVelocity = Vector3.zero;

            // 위쪽으로 튀어오르는 힘
            Vector3 force = Vector3.up * value;

            _rb.AddForce(force, ForceMode.Impulse);

            // 랜덤하게 회전
            _rb.AddTorque(
                Random.insideUnitSphere * torqueForce,
                ForceMode.Impulse
            );
        }

        public void SmoothRotateToTarget(Vector3 targetEulerRotation, float duration, System.Action onComplete)
        {
            if (this == null || transform == null) return;
        
            _rb.isKinematic = true;
            _rb.linearVelocity = Vector3.zero;
            _rb.angularVelocity = Vector3.zero;
        
            transform.DORotate(targetEulerRotation, duration, RotateMode.Fast)
                .SetEase(Ease.OutSine)
                .OnComplete(() => { onComplete?.Invoke(); });
        }

        public Vector3 GetClosestRotation(Vector3[] rotations)
        {
            // 1. faceRotations 데이터가 생성될 때 기준이었던 로컬 방향 (기존에 잘 작동하던 Back)
            Vector3 baseDir = Vector3.back; 

            // 2. 판정 대상이 되는 방향 (카메라 시선 반대 방향 = 주사위 윗면이 바라보는 방향)
            // 메인 카메라가 있다면 카메라 시선 기준으로 자동 판정하고, 없으면 World Up(Vector3.up) 적용
            Vector3 targetWorldDir = Vector3.up;
            if (Camera.main != null)
            {
                targetWorldDir = -Camera.main.transform.forward;
            }

            float maxDot = -2f;
            Vector3 bestRotation = rotations[0];

            foreach (var rot in rotations)
            {
                Quaternion targetRot = Quaternion.Euler(rot);

                // 로컬 면 방향 계산 시에는 회전 데이터 기준축(baseDir) 사용
                Vector3 localFaceDir = Quaternion.Inverse(targetRot) * baseDir;

                // 주사위의 현재 회전을 적용한 월드 면 방향
                Vector3 currentWorldFaceDir = transform.rotation * localFaceDir;

                // 타겟 방향(카메라/위쪽)과 내적하여 최댓값 비교
                float dot = Vector3.Dot(currentWorldFaceDir, targetWorldDir);

                if (dot > maxDot)
                {
                    maxDot = dot;
                    bestRotation = rot;
                }
            }

            return bestRotation;
        }
        // 주사위의 물리적 움직임을 완전히 멈추고 고정하는 함수
        public void LockDice()
        {
            if (_rb != null)
            {
                _rb.linearVelocity = Vector3.zero;
                _rb.angularVelocity = Vector3.zero;
                _rb.isKinematic = true; // 물리 연산을 꺼버려서 더 이상 다른 주사위에 밀려 움직이지 않게 고정
            }
        }
    }
}