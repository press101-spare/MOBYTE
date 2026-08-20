using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class DiceDeckManager : MonoBehaviour
{
    // 내가 보유한 전체 주사위
    [SerializeField] private List<DiceSO_JCY> diceCollection;
    
    // 기본 덱에 들어가는 주사위
    [SerializeField] private DiceSO_JCY defaultDice;

    // 현재 덱에 들어있는 주사위
    [SerializeField] private List<DiceSO_JCY> diceDeck;

    // 이번 턴에 뽑힌 주사위
    [SerializeField] private List<DiceSO_JCY> drawnDice;
    
    //버려진 주사위들
    [SerializeField] private List<DiceSO_JCY> discardPile;

    // 한 턴에 뽑을 주사위 개수
    [SerializeField] private int drawCount = 6;

    public static DiceDeckManager Instance { get; private set; }
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
    
    private void Start()
    {
        InitializeDeck();
    }

    private void InitializeDeck()
    {
        diceDeck.Clear();

        for (int i = 0; i < 10; i++)
        {
            diceDeck.Add(defaultDice);
        }

        ShuffleDeck();
    }

    //기본 카드 섞기
    private void ShuffleDeck()
    {
        for (int i = 0; i < diceDeck.Count; i++)
        {
            int randomIndex = Random.Range(i, diceDeck.Count);

            DiceSO_JCY temp = diceDeck[i];
            diceDeck[i] = diceDeck[randomIndex];
            diceDeck[randomIndex] = temp;
        }
    }

    //덱에서 주사위 뽑아서 전달
    public void DrawDice()
    {
        if(diceDeck.Count < drawCount)
            ReshuffleDeck();
        DiscardDice(); 
        for (int i = 0; i < drawCount; i++)
        {
            int randomIndex = Random.Range(0, diceDeck.Count);

            // 2. 뽑은 주사위를 drawnDice에 넣기
            DiceSO_JCY selectedDice = diceDeck[randomIndex];

            drawnDice.Add(selectedDice);
            
            diceDeck.RemoveAt(randomIndex);
        }

        // 3. DiceManager에게 전달
        DiceManager_JCY.Instance.StartTurn(drawnDice);
    }

    //다이스 버린거 버리는 덱에 넣고 드로우한 다이스 초기화
    public void DiscardDice()
    {
        discardPile.AddRange(drawnDice);
        drawnDice.Clear();
    }
    
    //덱에 있는 다이스가 6개 이하면 보충해주는 함수
    public void ReshuffleDeck()
    {
        diceDeck.AddRange(discardPile);
        discardPile.Clear();

        // 여기서 랜덤하게 섞기
        for (int i = 0; i < diceDeck.Count; i++)
        {
            int randomIndex = Random.Range(i, diceDeck.Count);

            DiceSO_JCY temp = diceDeck[i];
            diceDeck[i] = diceDeck[randomIndex];
            diceDeck[randomIndex] = temp;
        }
    }
}
