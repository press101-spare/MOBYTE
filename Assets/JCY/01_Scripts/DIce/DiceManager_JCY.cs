using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using JJB.Script;
using JJB.Script.Battle;
using TMPro;
using UnityEngine;

public class DiceManager_JCY : MonoBehaviour
{
    public static DiceManager_JCY Instance { get; private set; }
    public DiceSO_JCY[] allDiceSo;
    
    [Header("주사위 리스트들")]
    [SerializeField] private List<GameObject> activeDiceObjects = new List<GameObject>();
    [SerializeField] private List<DiceObject_JCY> activeDiceScripts = new List<DiceObject_JCY>();
    [SerializeField] private List<JJB_DicePhysics> activeDicePhysicd = new List<JJB_DicePhysics>();
    public List<DiceSO_JCY> activeDiceSo = new List<DiceSO_JCY>();

    [Header("기타 수치")] [field: SerializeField]
    public int[] currentDiceValue = new int[5]; 
    [SerializeField] private float reRollUp= 1.5f;
    
    [Header("주사위 정렬 관련")]
    [SerializeField] private Transform[] spawnPositions; // 주사위 스폰 위치들
    [SerializeField] private Transform[] dicePositions; // 주사위 화면 위치들
    [SerializeField] private float sortTime = 1f;
    
    [Header("UI 관련")]
    [SerializeField] string sumUiTxt;
    [SerializeField] TextMeshProUGUI sumTxt;
    [SerializeField] GameObject backUiPannel;
    [SerializeField] private GameObject[] canvas;
    public bool isRolling;
    public bool isShled;

    [Header("공격 연출")]
    [SerializeField] private Transform endTrf;
    [SerializeField] private float attackDuration = 0.8f;
    [SerializeField] private float attackStagger = 0.2f;
    [SerializeField] private float attackHeight = 2f;
    [SerializeField] private float attackSide = 1.5f;
    
    [Header("관련 스크립트")] 
    public DiceTree_JCY diceTree;
    public DiceEffect_JCY diceEffect;
    public ReRollCount_JCY reRollUI;
    public ShledDice_JCY shledDice;
    public DiceCamera_JCY diceCamera;
    
    public IReadOnlyList<DiceObject_JCY> ActiveDiceScripts => activeDiceScripts;
    
    [Header("주사위 효과")] 
    public int glassStack;
    public int _xecution;
    public int _vamfire;


    // 0~5번 인덱스 면이 정면을 볼 때의 회전 각도 배열 (제시해주신 각도 데이터 적용)
    public readonly Vector3[] FaceRotations = new Vector3[]
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
            if(backUiPannel != null)
                 backUiPannel.SetActive(false);
      //      DontDestroyOnLoad(gameObject); // 씬이 넘어가도 파괴되지 않음
        }
        else
        {
            Destroy(gameObject); // 중복 생성 방지
        }
    }
    // 턴 시작 시 호출 (뽑힌 SO 리스트를 넘겨받음)
    public void StartTurn(List<DiceSO_JCY> drawnDiceSoList)
    {
        ClearDice();
        diceCamera.DiceCameraMove();
        activeDiceSo = drawnDiceSoList;
        for (int i = 0; i < drawnDiceSoList.Count; i++)
        {
            if (i >= spawnPositions.Length) break;
            DiceSO_JCY currentSO = drawnDiceSoList[i];

            // SO에 지정된 전용 프리팹 생성
            GameObject newDice = Instantiate(currentSO.dicePrefab, spawnPositions[i].position, Quaternion.identity);
            DiceObject_JCY diceScript = newDice.GetComponent<DiceObject_JCY>();
            JJB_DicePhysics jjbDicePhysicdScript = newDice.GetComponent<JJB_DicePhysics>();

            // 스크립트에 SO 데이터 전달
            diceScript.Setup(currentSO);
            diceEffect.Effect(currentSO);

            activeDiceObjects.Add(newDice);
            activeDiceScripts.Add(diceScript);
            activeDicePhysicd.Add(jjbDicePhysicdScript);
        }
        
        sumTxt.gameObject.SetActive(false);
        backUiPannel.SetActive(true);
        isRolling = true;
        RollAllDice();
    }

    #region Roll

    public void RollAllDice()
    {
        StartCoroutine(RollAllRoutine());
    }

    private IEnumerator RollAllRoutine()
    {
        int totalScore = 0;
        int completedCount = 0;
       // yield return new WaitForSeconds(diceCamera.moveDuration);
        // 1. 모든 주사위 물리 던지기 실행
        for (int i = 0; i < activeDicePhysicd.Count; i++)
        {
            activeDicePhysicd[i].Throw();
        }

        // 2. 주사위마다 개별적인 코루틴 실행
        for (int i = 0; i < activeDiceScripts.Count; i++)
        {
            JJB_DicePhysics physics = activeDicePhysicd[i];

            // 💡 수정된 부분: 불필요한 시간 변수들을 빼고 매개변수 2개만 전달
            StartCoroutine(RollSingleDiceRoutine(physics, (resultValue) =>
            {
                totalScore += resultValue;
                completedCount++;
            }));
        }

        yield return new WaitUntil(() => completedCount >= activeDiceScripts.Count);
        StartCoroutine(FaceDiceCO());
    }

// 💡 매개변수 구조 맞추기
    private IEnumerator RollSingleDiceRoutine(JJB_DicePhysics physics, System.Action<int> onResult)
    {
        if (physics == null) yield break;
        Rigidbody rb = physics.GetComponent<Rigidbody>();
        yield return new WaitForSeconds(0.5f);
        yield return new WaitUntil(() => physics == null || rb == null || (rb.linearVelocity.magnitude < 0.05f && rb.angularVelocity.magnitude < 0.05f));

        if (physics == null || rb == null) yield break;

        // 💡 강제로 각도를 돌리지 않고, 현재 멈춘 상태 그대로의 위쪽 면(Raycast 또는 윗면 벡터)을 계산해서 결과 도출
        // (또는 기존 방식대로 가장 가까운 면의 값을 바로 반환)
        Vector3 closestRot = physics.GetClosestRotation(FaceRotations);
        
        // 💡 4. 다른 주사위에 부딪혀서 틀어지는 걸 막기 위해 즉시 물리 고정!
        physics.LockDice();
        
        int targetIndex = System.Array.IndexOf(FaceRotations, closestRot);
        DiceObject_JCY currentDice = physics.GetComponent<DiceObject_JCY>();

        int resultValue = currentDice.currentDiceSO.faceValues[targetIndex];
        currentDice.currentIndex = resultValue;
        
        UpdateCurrentDiceValues();
        onResult?.Invoke(resultValue);
    }

  
    //나온 결괏값을 바탕으로 나온 인덱스의 오름차순으로 주사위 화면 
   // 주사위 정렬 및 이동 정보를 담을 임시 구조체
    private struct DiceSortData
    {
        public GameObject diceObject;
        public int resultValue;
        public Vector3 targetRotation;

        public DiceSortData(GameObject obj, int val, Vector3 rot)
        {
            diceObject = obj;
            resultValue = val;
            targetRotation.x = rot.x;
            targetRotation.y = rot.y;
            targetRotation.z = 0;
        }
    }

    // 나온 결괏값을 바탕으로 오름차순 정렬 후 화면 위치로 배치하는 코루틴
   

    #endregion
    

    #region ReRoll

    public void RerollSelectedDice()
    {
        if(reRollUI.reRollCount <= 0)
            return;
        StartCoroutine(RerollSelectedDiceRoutine());
    }
    
    private IEnumerator RerollSelectedDiceRoutine()
    {
        List<DiceObject_JCY> selectedDice = new List<DiceObject_JCY>();

        // 선택된 주사위 찾기
        for (int i = 0; i < activeDiceScripts.Count; i++)
        {
            if (activeDiceScripts[i].IsSelected)
            {
                selectedDice.Add(activeDiceScripts[i]);
            }
        }

        if (selectedDice.Count == 0)
        {
            Debug.Log("선택된 주사위가 없습니다.");
            yield break;
        }
        backUiPannel.SetActive(true);
        isRolling = true;
        reRollUI.UpdateReRollCount(-1);
        diceCamera.DiceCameraMove();

        int completedCount = 0;

        // 선택된 주사위만 다시 굴리기
        foreach (DiceObject_JCY dice in selectedDice)
        {
            dice.SetSelected(false);
            JJB_DicePhysics physics =
                dice.GetComponent<JJB_DicePhysics>();

            if (physics == null)
            {
                completedCount++;
                continue;
            }

            int diceIndex = activeDiceScripts.IndexOf(dice);

            Rigidbody rb = dice.GetComponent<Rigidbody>();

            if (rb != null)
            {
                if (!rb.isKinematic)
                {
                    rb.linearVelocity = Vector3.zero;
                    rb.angularVelocity = Vector3.zero;
                }

                rb.isKinematic = true;
            }

            if (diceIndex >= 0 && diceIndex < spawnPositions.Length)
            {
                Vector3 spawnPosition = spawnPositions[diceIndex].position;

                dice.transform.position = spawnPosition;

                if (rb != null)
                {
                    rb.position = spawnPosition;
                }
            }

            if (rb != null)
            {
                rb.isKinematic = false;
            }

            physics.Throw();

            StartCoroutine(
                RollSingleDiceRoutine(
                    physics,
                    (resultValue) =>
                    {
                        dice.currentIndex = resultValue;
                        completedCount++;
                    }
                )
            );
        }

        // 모든 재굴림 주사위가 멈출 때까지 기다림
        yield return new WaitUntil(() =>
            completedCount >= selectedDice.Count
        );

        // 재굴린 주사위만 원래 자리로 이동
        Sequence sequence = DOTween.Sequence();
        diceCamera.BattleCameraMove();

        foreach (DiceObject_JCY dice in selectedDice)
        {
            Transform targetPosition = dice.GetDicePosition();

            if (targetPosition == null)
                continue;

            Rigidbody rb = dice.GetComponent<Rigidbody>();

            if (rb != null)
            {
                rb.isKinematic = true;
            }

            int targetIndex = -1;

            for (int i = 0; i < dice.currentDiceSO.faceValues.Length; i++)
            {
                if (dice.currentDiceSO.faceValues[i] == dice.currentIndex)
                {
                    targetIndex = i;
                    break;
                }
            }

            if (targetIndex == -1)
                continue;

            Vector3 targetRotation = FaceRotations[targetIndex];

            sequence.Join(
                dice.transform.DOMove(
                    targetPosition.position,
                    0.5f
                ).SetEase(Ease.OutQuad)
            );

            sequence.Join(
                dice.transform.DORotate(
                    targetRotation,
                    0.5f
                ).SetEase(Ease.OutQuad)
            );
        }

        yield return sequence.WaitForCompletion();
        

        // 현재 결과 갱신
        UpdateCurrentDiceValues();
        
        backUiPannel.SetActive(false);
        isRolling = false;
    }
    
   

    #endregion

    public void ClearDice()
    {
        foreach (var diceObj in activeDiceObjects)
        {
            Destroy(diceObj);
        }
        activeDiceObjects.Clear();
        activeDiceScripts.Clear();
        activeDicePhysicd.Clear();
        
        for (int i = 0; i < currentDiceValue.Length; i++)
        {
            currentDiceValue[i] = 0;
        }
        diceTree.Reset();
        
    }

    
    public IEnumerator FaceDiceCO()
    {
        yield return new WaitForSeconds(sortTime);
        diceCamera.BattleCameraMove();
       // yield return new WaitForSeconds(diceCamera.moveDuration);
        // 1. 현재 생성되어 있는 주사위들의 데이터를 수집합니다.
        List<DiceSortData> sortList = new List<DiceSortData>();

        for (int i = 0; i < activeDiceObjects.Count; i++)
        {
            GameObject diceObj = activeDiceObjects[i];
            DiceObject_JCY diceScript = activeDiceScripts[i];

            int resultVal = diceScript.currentIndex; // 위에서 계산된 주사위 숫자
            
            // 해당 숫자가 faceRotations 배열에서 몇 번째 인덱스인지 찾고 회전값 가져오기
            // (주의: faceRotations와 faceValues 매칭 방식에 따라 다를 수 있으므로 현재 로직에 맞게 조절)
            int targetIndex = -1;
            for (int f = 0; f < diceScript.currentDiceSO.faceValues.Length; f++)
            {
                if (diceScript.currentDiceSO.faceValues[f] == resultVal)
                {
                    targetIndex = f;
                    break;
                }
            }

            Vector3 targetRot = (targetIndex != -1) ? FaceRotations[targetIndex] : diceObj.transform.eulerAngles;
            sortList.Add(new DiceSortData(diceObj, resultVal, targetRot));
        }

        // 2. 결과값(resultValue)을 기준으로 오름차순 정렬 (LINQ OrderBy 사용)
        sortList = sortList.OrderBy(x => x.resultValue).ToList();

        // 3. 정렬된 순서대로 dicePositions에 DOTween을 이용해 이동 및 회전
        Sequence moveSequence = DOTween.Sequence();

        for (int i = 0; i < sortList.Count; i++)
        {
            if (i >= dicePositions.Length) break;

            Transform diceTransform = sortList[i].diceObject.transform;
            Transform targetPos = dicePositions[i];
            Vector3 finalRotation = sortList[i].targetRotation;
            
            DiceObject_JCY diceScript = 
                sortList[i].diceObject.GetComponent<DiceObject_JCY>();

            diceScript.SetDicePosition(targetPos);

            Rigidbody rb = sortList[i].diceObject.GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.isKinematic = true;
            }

            // 위치 이동
            moveSequence.Join(diceTransform.DOMove(targetPos.position, 0.5f).SetEase(Ease.OutQuad));
            //Debug
    
            // 💡 수정 포인트: DORotate 대신 DOLocalRotate를 사용하거나, 
            // 부모의 회전을 고려한 로컬 회전값으로 지정해 보세요.
            moveSequence.Join(diceTransform.DOLocalRotate(finalRotation, 0.5f, RotateMode.FastBeyond360).SetEase(Ease.OutQuad));
        }

        yield return moveSequence.WaitForCompletion();

        sumTxt.gameObject.SetActive(true);
        backUiPannel.SetActive(false);
        isRolling = false;
    }
    private void UpdateCurrentDiceValues()
    {
        for (int i = 0; i < currentDiceValue.Length; i++)
        {
            currentDiceValue[i] = 0;
        }

        for (int i = 0; i < activeDiceScripts.Count; i++)
        {
            if (i >= currentDiceValue.Length)
                break;

            currentDiceValue[i] = activeDiceScripts[i].currentIndex;
        }
        
        SumDiceValue();
        if (isShled) return;
        // DiceTree에 주사위 값 배열 전달하여 UI 갱신
        if (diceTree != null)
        {
            diceTree.UpdateTreeStatus(currentDiceValue);
        }
    }
    public void SumDiceValue()
    {
        int totalsum = 0;

        for (int i = 0; i < currentDiceValue.Length; i++)
        {
            totalsum += currentDiceValue[i];
        }
        sumTxt.text = sumUiTxt + totalsum;
        if (isShled)
        {
            shledDice.ShledHP(totalsum);
        }
    }
  

    public void PlayAttackAnimation(int baseDamage, System.Action<int> onDiceHit)
    {
        StartCoroutine(DiceAttackRoutine(baseDamage, onDiceHit));
    }
    
    private IEnumerator DiceAttackRoutine(int baseDamage, System.Action<int> onDiceHit)
    {
        int diceCount = activeDiceObjects.Count;

        if (diceCount <= 0)
            yield break;

        // 최종 데미지 계산
        int finalDamage = diceEffect.CalculateFinalDamage(activeDiceScripts, baseDamage);

        // 주사위 개수만큼 데미지 분배
        int dividedDamage = finalDamage / diceCount;
        int remainder = finalDamage % diceCount;
        
        //애니메이션 시작 및 트레일 켜기
        TrialOn(true);
        for (int i = 0; i < diceCount; i++)
        {
            GameObject dice = activeDiceObjects[i];

            if (dice == null)
                continue;

            Rigidbody rb = dice.GetComponent<Rigidbody>();

            if (rb != null)
            {
                if (!rb.isKinematic)
                {
                    rb.linearVelocity = Vector3.zero;
                    rb.angularVelocity = Vector3.zero;
                }

                rb.isKinematic = true;
            }

            Vector3 start = dice.transform.position;
            Vector3 end = endTrf.position;

            float side = (i - (diceCount - 1) * 0.5f) * attackSide;

            Vector3 point1 = start + Vector3.up * attackHeight + Vector3.right * side;

            Vector3 point2 = Vector3.Lerp(start, end, 0.6f) + Vector3.up * attackHeight + Vector3.right * side;

            dice.transform.DOKill();

            Sequence attackSequence = DOTween.Sequence();
            
            attackSequence.Append(
                dice
                    .transform
                    .DOPath(
                    new Vector3[]
                    {
                        start,
                        point1,
                        point2,
                        end
                    },
                    attackDuration,
                    PathType.CatmullRom
                )
                .SetEase(Ease.InOutSine)
            );

            attackSequence.Join(
                dice.transform
                    .DORotate(
                        dice.transform.eulerAngles +
                        new Vector3(360f, 360f, 360f),
                        attackDuration,
                        RotateMode.FastBeyond360
                    )
                    .SetEase(Ease.Linear)
                );

            attackSequence.SetDelay(i * attackStagger);

            int hitDamage = dividedDamage;

            if (i < remainder)
                hitDamage++;

            attackSequence.OnComplete(() =>
            {
                // 1. 적중 데미지
                onDiceHit?.Invoke(hitDamage);

                if (_vamfire > 0)
                {
                    JJBGameManager.Instance.PlayerJjbHealth.Heal(hitDamage * ((_vamfire * 20) / 100));
                }

                //글라스 스택만큼 데미지
                for (int glass = 0; glass < glassStack; glass++)
                {
                    onDiceHit?.Invoke(10);
                }

                if (glassStack > 0)
                {
                    glassStack = 0;
                }

                if (_xecution > 0)
                {
                    if(JJBGameManager.Instance.EnemyJjbHealth.CurrentHealth < (JJBGameManager.Instance.EnemyJjbHealth.MaxHealth *((_xecution * 10) / 100)))
                    {
                        onDiceHit?.Invoke(999);
                    }
                }


                // 2. 주사위 삭제
                if (dice != null)
                {
                    Destroy(dice);
                }
            }); 
        }

        _xecution = 0;
        _vamfire = 0;
        
        yield return new WaitForSeconds(
            attackDuration +
            attackStagger * Mathf.Max(0, diceCount - 1)
        );
    }

    private void TrialOn(bool on)
    {
        foreach (var diceScript in ActiveDiceScripts)
        {
            diceScript.trailRenderer.enabled = on;
        }
    }

    public void DiceSet(bool amount)
    {
        // 1. 현재 켜져 있는 UI Canvas의 개수를 세어봅니다.
        int activeCanvasCount = 0;
        foreach (var canva in canvas)
        {
            if (canva != null && canva.activeInHierarchy)
            {
                activeCanvasCount++;
            }
        }

        // 2. 켜진 UI가 0개면 주사위 활성화(true), 1개 이상이면 비활성화(false)
        bool shouldShowDice = (activeCanvasCount == 0);

        // 3. 주사위 상태 적용
        foreach (var activeDiceObject in activeDiceObjects)
        {
            if (activeDiceObject != null)
            {
                activeDiceObject.SetActive(shouldShowDice);
            }
        }
    }

    public void DiceSlotSet()
    {
        DiceDeckManager_JCY.Instance.diceEffectUIUpdate.UpdateUI(activeDiceSo);
    }
}

