using System.Collections;
using DG.Tweening;
using UnityEngine;

namespace JJB.Script
{
    public class DicePhysics : MonoBehaviour
    {
        [Header("Throw")]
        [SerializeField] private float xForce = 5f;
        [SerializeField] private float yForce = 5f;
        [SerializeField] private float liftForce = 4f;
        [SerializeField] private float torqueForce = 3f;

        private Rigidbody _rb;

        private void Awake()
        {
            _rb = GetComponent<Rigidbody>();
            
            // 💡 유니티 기본 중력 사용
            _rb.useGravity = true;
            _rb.isKinematic = true; // 처음에는 가만히 있도록 설정
        }

        public void Throw()
        {
            _rb.isKinematic = false;
            _rb.linearVelocity = Vector3.zero;
            _rb.angularVelocity = Vector3.zero;

            Vector3 force = new Vector3(
                Random.Range(-xForce, xForce),
                Random.Range(-yForce, yForce),
                liftForce
            );

            _rb.AddForce(force, ForceMode.Impulse);
            _rb.AddTorque(Random.insideUnitSphere * torqueForce, ForceMode.Impulse);
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
            // 💡 화면(카메라)을 바라보는 기준 축 설정
            // 2D 게임처럼 카메라가 정면을 보고 있다면 Vector3.back
            // 3D 보드게임처럼 바닥에 닿고 '윗면'을 본다면 Vector3.up 으로 바꿔주세요!
            Vector3 viewDir = Vector3.back; 
    
            float maxDot = -2f;
            Vector3 bestRotation = rotations[0];

            foreach (var rot in rotations)
            {
                // 1. 해당 숫자 면이 정면을 볼 때의 기준 회전값
                Quaternion targetRot = Quaternion.Euler(rot);
        
                // 2. 그 상태일 때 '정면'을 향하는 주사위의 '진짜 로컬 방향' 역추적
                Vector3 localFaceDir = Quaternion.Inverse(targetRot) * viewDir;
        
                // 3. 현재 굴러가서 멈춘 주사위의 그 로컬 방향이 월드에서 어디를 향하고 있는지 계산
                Vector3 currentWorldFaceDir = transform.rotation * localFaceDir;
        
                // 4. 그 방향이 카메라(viewDir)를 얼마나 똑바로 쳐다보고 있는지 확인
                float dot = Vector3.Dot(currentWorldFaceDir, viewDir);

                // 내적(Dot)이 가장 큰(1에 가까운) 면이 카메라를 보고 있는 진짜 숫자!
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