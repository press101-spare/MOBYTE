using System.Collections;
using System.Collections.Generic;
using JJB.Script;
using UnityEngine;

public class DiceManager_JCY : MonoBehaviour
{
    public static DiceManager_JCY Instance { get; private set; }
    [SerializeField] private Transform[] spawnPositions; // 주사위 스폰 위치들

    [Header("주사위 리스트들")]
    [SerializeField] private List<GameObject> activeDiceObjects = new List<GameObject>();
    [SerializeField] private List<DiceObject_JCY> activeDiceScripts = new List<DiceObject_JCY>();
    [SerializeField] private List<DicePhysics> activeDicePhysicd = new List<DicePhysics>();

    [Header("기타 수치")] 
    [SerializeField] private float minTime = 0.3f;
    [SerializeField] private float maxTime = 1.5f;


    // 0~5번 인덱스 면이 정면을 볼 때의 회전 각도 배열 (제시해주신 각도 데이터 적용)
    public readonly Vector3[] faceRotations = new Vector3[]
    {
        new Vector3(0f, 180f, 0f),   // Index 1 (숫자 1)
        new Vector3(0f, 90f, 0f),    // Index 2 (숫자 2)
        new Vector3(0f, 0f, 0f),     // Index 3 (숫자 3)
        new Vector3(90f, 0f, 0f),    // Index 4 (숫자 4)
        new Vector3(0f, 270f, 0f),   // Index 5 (숫자 5)
        new Vector3(-90f, 0f, 0f)    // Index 6 (숫자 6)
    };
    
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // 씬이 넘어가도 파괴되지 않음
        }
        else
        {
            Destroy(gameObject); // 중복 생성 방지
        }
    }
    // 턴 시작 시 호출 (뽑힌 SO 리스트를 넘겨받음)
    public void StartTurn(List<DiceSO_JCY> drawnDiceSOList)
    {
        ClearDice();

        for (int i = 0; i < drawnDiceSOList.Count; i++)
        {
            if (i >= spawnPositions.Length) break;

            DiceSO_JCY currentSO = drawnDiceSOList[i];

            // SO에 지정된 전용 프리팹 생성
            GameObject newDice = Instantiate(currentSO.dicePrefab, spawnPositions[i].position, spawnPositions[i].rotation ,spawnPositions[i] );
            DiceObject_JCY diceScript = newDice.GetComponent<DiceObject_JCY>();
            DicePhysics dicePhysicdScript = newDice.GetComponent<DicePhysics>();

            // 스크립트에 SO 데이터 전달
            diceScript.Setup(currentSO);

            activeDiceObjects.Add(newDice);
            activeDiceScripts.Add(diceScript);
            activeDicePhysicd.Add(dicePhysicdScript);
        }
        RollAllDice();
    }

    public void RollAllDice()
    {
        StartCoroutine(RollAllRoutine());
    }

    private IEnumerator RollAllRoutine()
    {
        int totalScore = 0;
        int completedCount = 0;

        // 1. 모든 주사위 물리 던지기 실행
        for (int i = 0; i < activeDicePhysicd.Count; i++)
        {
            activeDicePhysicd[i].Throw();
        }

        // 2. 주사위마다 개별적인 코루틴 실행
        for (int i = 0; i < activeDiceScripts.Count; i++)
        {
            DicePhysics physics = activeDicePhysicd[i];

            // 💡 수정된 부분: 불필요한 시간 변수들을 빼고 매개변수 2개만 전달
            StartCoroutine(RollSingleDiceRoutine(physics, (resultValue) =>
            {
                totalScore += resultValue;
                completedCount++;
            }));
        }

        yield return new WaitUntil(() => completedCount >= activeDiceScripts.Count);
        Debug.Log($"총합: {totalScore}");
    }

// 💡 매개변수 구조 맞추기
    private IEnumerator RollSingleDiceRoutine(DicePhysics physics, System.Action<int> onResult)
    {
        if (physics == null) yield break;
        Rigidbody rb = physics.GetComponent<Rigidbody>();
        if (rb == null) yield break;

        yield return new WaitForSeconds(0.5f);
        yield return new WaitUntil(() => physics == null || rb == null || (rb.linearVelocity.magnitude < 0.05f && rb.angularVelocity.magnitude < 0.05f));

        if (physics == null || rb == null) yield break;

        // 💡 강제로 각도를 돌리지 않고, 현재 멈춘 상태 그대로의 위쪽 면(Raycast 또는 윗면 벡터)을 계산해서 결과 도출
        // (또는 기존 방식대로 가장 가까운 면의 값을 바로 반환)
        Vector3 closestRot = physics.GetClosestRotation(faceRotations);
        
        // 💡 4. 다른 주사위에 부딪혀서 틀어지는 걸 막기 위해 즉시 물리 고정!
        physics.LockDice();
        
        int targetIndex = System.Array.IndexOf(faceRotations, closestRot);
        DiceObject_JCY currentDice = physics.GetComponent<DiceObject_JCY>();

        int resultValue = currentDice.currentDiceSO.faceValues[targetIndex];
        currentDice.currentIndex = resultValue;
        // 각도를 억지로 돌리는 DOTween 코드를 호출하지 않고 바로 결과 전달!
        Debug.Log("주사위 결과:"+ resultValue);
        onResult?.Invoke(resultValue);
    }

    public void ClearDice()
    {
        foreach (var diceObj in activeDiceObjects)
        {
            Destroy(diceObj);
        }
        activeDiceObjects.Clear();
        activeDiceScripts.Clear();
        activeDicePhysicd.Clear();
    }
}

