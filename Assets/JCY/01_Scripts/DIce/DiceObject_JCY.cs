using System;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class DiceObject_JCY : MonoBehaviour
{
    [Header("주사위 정보")]
    public DiceSO_JCY currentDiceSO;
    public int currentIndex;

    [Header("굴리기 설정")]
    [SerializeField] private float rollDuration = 1.5f;
    [SerializeField] private float rollDistance = 5.0f;
    
   

    
    [Header("셀렉트 설정")]
    public MeshRenderer MeshCompo { get; private set; }
    [field:SerializeField] public Material OutLine { get; private set; }
    [field:SerializeField] public bool select { get; private set; }

    private void OnEnable()
    {
        select = true;
        MeshCompo = GetComponentInChildren<MeshRenderer>();
    }

    public void Setup(DiceSO_JCY diceSO)
    {
        currentDiceSO = diceSO;
    }

    public void OnMouseDown()
    {
        if (select)
        {
            select = false;
            Debug.Log(currentIndex);
            Material[] cureentMaterials = MeshCompo.materials;

            Material[] newMaterials = new Material[cureentMaterials.Length + 1];

            int index = 0;
            for (index = 0; index < cureentMaterials.Length; index++)
            {
                newMaterials[index] = cureentMaterials[index];
            }

            newMaterials[index] = OutLine;
            index = 0;
            MeshCompo.materials = newMaterials;
        }
        else
        {
            
        }
    }

    // public IEnumerator RollRoutine(System.Action<int> onComplete)
    // {
    //     if (currentDiceSO == null)
    //     {
    //         Debug.LogError("주사위 데이터(SO)가 주입되지 않았습니다!");
    //         onComplete?.Invoke(0);
    //         yield break;
    //     }
    //
    //     // --------------------------------
    //     // 1. 결과 결정
    //     // --------------------------------
    //
    //     int targetIndex = currentDiceSO.GetRandomIndex();
    //
    //     Vector3 finalRotation =
    //         DiceManager_JCY.Instance.faceRotations[targetIndex];
    //
    //     int finalResultValue =
    //         currentDiceSO.faceValues[targetIndex];
    //
    //
    //     // --------------------------------
    //     // 2. 시작 위치 / 회전
    //     // --------------------------------
    //
    //     diceTransform = gameObject.transform;
    //     
    //     Vector3 startPos = diceTransform.position;
    //
    //     diceTransform.rotation = Quaternion.Euler(
    //         Random.Range(0, 360f),
    //         Random.Range(0, 360f),
    //         Random.Range(0, 360f)
    //     );
    //
    //
    //     // --------------------------------
    //     // 3. 목표 위치
    //     // --------------------------------
    //
    //     Vector3 targetPos =
    //         startPos + diceTransform.forward * rollDistance;
    //
    //
    //     // --------------------------------
    //     // 4. 여러 바퀴 회전
    //     // --------------------------------
    //
    //     Vector3 randomSpin = new Vector3(
    //         360f * Random.Range(3, 5),
    //         360f * Random.Range(3, 5),
    //         360f * Random.Range(3, 5)
    //     );
    //
    //     Quaternion startRot = diceTransform.rotation;
    //
    //     Quaternion spinRot =
    //         Quaternion.Euler(finalRotation + randomSpin);
    //
    //
    //     // --------------------------------
    //     // 5. 굴러가는 연출
    //     // --------------------------------
    //
    //     float elapsed = 0f;
    //
    //     while (elapsed < rollDuration)
    //     {
    //         elapsed += Time.deltaTime;
    //
    //         float t = elapsed / rollDuration;
    //
    //         // Ease-Out
    //         float easeT = t * t * (3f - 2f * t);
    //
    //
    //         if (diceTransform == null) yield break;
    //         // 위치 이동
    //         diceTransform.position =
    //             Vector3.Lerp(startPos, targetPos, easeT);
    //
    //
    //         // 회전
    //         diceTransform.rotation =
    //             Quaternion.Slerp(startRot, spinRot, easeT);
    //
    //
    //         yield return null;
    //     }
    //
    //
    //     // --------------------------------
    //     // 6. 최종 위치 / 회전 고정
    //     // --------------------------------
    //
    //     diceTransform.position = targetPos;
    //
    //     diceTransform.rotation =
    //         Quaternion.Euler(finalRotation);
    //
    //
    //     // --------------------------------
    //     // 7. 결과 전달
    //     // --------------------------------
    //
    //     Debug.Log(
    //         $"주사위 결과: {finalResultValue}"
    //     );
    //
    //     onComplete?.Invoke(finalResultValue);
    // }
}