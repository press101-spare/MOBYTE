using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DiceManager_JCY : MonoBehaviour
{
    public static DiceManager_JCY Instance { get; private set; }
    [SerializeField] private Transform[] spawnPositions; // 주사위 스폰 위치들

    private List<GameObject> activeDiceObjects = new List<GameObject>();
    private List<DiceObject> activeDiceScripts = new List<DiceObject>();


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
            GameObject newDice = Instantiate(currentSO.dicePrefab, spawnPositions[i].position, spawnPositions[i].rotation);
            DiceObject diceScript = newDice.GetComponent<DiceObject>();

            // 스크립트에 SO 데이터 전달
            diceScript.Setup(currentSO);

            activeDiceObjects.Add(newDice);
            activeDiceScripts.Add(diceScript);
        }
    }

    public void RollAllDice()
    {
        StartCoroutine(RollAllRoutine());
    }

    private IEnumerator RollAllRoutine()
    {
        int totalScore = 0;
        int completedCount = 0;

        foreach (var dice in activeDiceScripts)
        {
            StartCoroutine(dice.RollRoutine((result) =>
            {
                totalScore += result;
                completedCount++;
            }));
        }

        yield return new WaitUntil(() => completedCount >= activeDiceScripts.Count);
        Debug.Log($"총합: {totalScore}");
    }

    public void ClearDice()
    {
        foreach (var diceObj in activeDiceObjects)
        {
            Destroy(diceObj);
        }
        activeDiceObjects.Clear();
        activeDiceScripts.Clear();
    }
}

