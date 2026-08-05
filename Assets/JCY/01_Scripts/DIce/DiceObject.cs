using System.Collections;
using UnityEngine;

public class DiceObject : MonoBehaviour
{
    private Transform diceTransform;
    private DiceSO_JCY currentDiceSO;

    // 주사위의 6면(Index 0~5)이 각각 정면을 볼 때의 기준 회전 각도
    private readonly Vector3[] faceRotations = new Vector3[]
    {
        new Vector3(0f, 0f, 0f),     // Index 0
        new Vector3(0f, 90f, 0f),    // Index 1
        new Vector3(0f, 180f, 0f),   // Index 2
        new Vector3(0f, 270f, 0f),   // Index 3
        new Vector3(90f, 0f, 0f),    // Index 4
        new Vector3(-90f, 0f, 0f)    // Index 5
    };

    private void Awake()
    {
        // 최상위 Transform을 가져오거나, 주사위 메쉬만 회전시킬 타겟을 별도로 지정해도 됩니다.
        diceTransform = transform;
    }

    // 매니저에서 턴 시작 시(Instantiate 직후) 호출하여 SO 데이터 주입
    public void Setup(DiceSO_JCY diceSO)
    {
        currentDiceSO = diceSO;
    }

    // 매니저에서 전체 굴리기를 실행할 때 호출됨
    public IEnumerator RollRoutine(System.Action<int> onComplete)
    {
        if (currentDiceSO == null)
        {
            Debug.LogError("주사위 데이터(SO)가 주입되지 않았습니다!");
            onComplete?.Invoke(0);
            yield break;
        }

        // 1. 결과 인덱스와 최종 값 결정
        int targetIndex = currentDiceSO.GetRandomIndex();
        Vector3 targetRotation = faceRotations[targetIndex];
        int resultValue = currentDiceSO.faceValues[targetIndex];

        // 2. 공중에서 여러 바퀴 도는 연출용 회전값 생성
        Vector3 randomSpin = new Vector3(
            360f * Random.Range(3, 5),
            360f * Random.Range(3, 5),
            360f * Random.Range(3, 5)
        );

        Quaternion startRot = diceTransform.rotation;
        Quaternion endRot = Quaternion.Euler(targetRotation + randomSpin);

        float duration = 1.2f;
        float elapsed = 0f;

        // 3. 서서히 멈추는 Ease-Out 회전 연출
        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float t = elapsed / duration;

            // Ease-Out 공식
            t = t * t * (3f - 2f * t);

            diceTransform.rotation = Quaternion.Slerp(startRot, endRot, t);
            yield return null;
        }

        // 4. 오차 없이 정확한 최종 목표 각도로 고정
        diceTransform.rotation = Quaternion.Euler(targetRotation);

        // 5. 최종 눈금 값을 매니저로 전달
        onComplete?.Invoke(resultValue);
    }
}
