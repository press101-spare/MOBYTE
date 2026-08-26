using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DIceRoller_JCY : MonoBehaviour
{
    [Header("컴포넌트")]
    [SerializeField] private Transform diceTransform;       // 주사위 3D 모델 Transform
    [SerializeField] private BoxCollider diceCollider;     // 주사위 충돌체 (없어도 되지만 배치용)
    [SerializeField] private DiceSO_JCY currentDice;        // 주사위 SO

    [Header("굴리기 설정")]
    [SerializeField] private float rollDuration = 1.5f;     // 전체 연출 시간 (초)
    [SerializeField] private float rollDistance = 5.0f;     // 주사위가 굴러가는 거리
    [SerializeField] private Vector3 diceStartPos;         // 주사위를 던지는 시작 위치 (인스펙터에서 설정 권장)

    private bool isRolling = false;

  

    public void RollDice()
    {
        if (!isRolling)
        {
            StartCoroutine(RollDiceOnBoardRoutine());
        }
    }

    private IEnumerator RollDiceOnBoardRoutine()
    {
        isRolling = true;

        // 1. 초기 위치 및 회전 설정
        // 바닥에 배치된 경우, 주사위의 중심(Pivot)이 정중앙에 있어야 합니다.
        diceTransform.position = diceStartPos;
        diceTransform.rotation = Quaternion.Euler(Random.Range(0, 360f), Random.Range(0, 360f), Random.Range(0, 360f)); // 랜덤하게 던지기 시작

        // 2. SO에서 무작위 면 '인덱스(0~5)' 추출 및 목표 각도 계산
        int targetIndex = currentDice.GetRandomIndex();
        Vector3 finalRotation = DiceManager_JCY.Instance.FaceRotations[targetIndex];
        int finalResultValue = currentDice.faceValues[targetIndex]; // 최종 결과값

        // 3. 연출 변수 계산
        Vector3 targetPos = diceStartPos + (diceTransform.forward * rollDistance); // 앞쪽으로 던지기

        // 공중에서 뱅글뱅글 도는 연출용 추가 회전 (360도 N번 추가)
        Vector3 randomSpin = new Vector3(
            360f * Random.Range(3, 5), // N바퀴 회전
            360f * Random.Range(3, 5),
            360f * Random.Range(3, 5)
        );

        Quaternion startRot = diceTransform.rotation;
        Quaternion spinRot = Quaternion.Euler(finalRotation + randomSpin); // 연출용 중간 회전 목표

        float elapsed = 0f;

        // 4. 감속 구르기 연출
        while (elapsed < rollDuration)
        {
            elapsed += Time.deltaTime;
            float t = elapsed / rollDuration;

            // Ease-Out 효과 (천천히 멈춤)
            float easeT = t * t * (3f - 2f * t);

            // 포지션 이동 (시작점 -> 목표점)
            diceTransform.position = Vector3.Lerp(diceStartPos, targetPos, easeT);

            // 회전 연출 (랜덤 시작각 -> 뱅글뱅글 도는 중간 연출각)
            diceTransform.rotation = Quaternion.Slerp(startRot, spinRot, easeT);

            // 주사위가 멈추기 전 천천히 멈추는 느낌을 주기 위해 
            // 마지막 20% 시간 동안은 최종 목표 각도로 수렴하도록 Slerp를 한 번 더 사용합니다.
            if (t > 0.8f)
            {
                float stopT = (t - 0.8f) / 0.2f; // 0~1 값으로 변환
                diceTransform.rotation = Quaternion.Slerp(spinRot, Quaternion.Euler(finalRotation), stopT);
            }

            yield return null;
        }

        // 5. 정확한 최종 목표 각도로 보정
        diceTransform.rotation = Quaternion.Euler(finalRotation);

        Debug.Log($"주사위가 굴러 Index: {targetIndex} (값: {finalResultValue})이(가) 나왔습니다.");
        isRolling = false;
    }
}
