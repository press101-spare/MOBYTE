using System.Collections.Generic;
using UnityEngine;

public class DiceDeckManager : MonoBehaviour
{
    // 내가 보유한 전체 주사위
    [SerializeField] private List<DiceSO_JCY> diceCollection;

    // 현재 덱에 들어있는 주사위
    [SerializeField] private List<DiceSO_JCY> diceDeck;

    // 이번 턴에 뽑힌 주사위
    [SerializeField] private List<DiceSO_JCY> drawnDice;

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
    
    public void DrawDiceAndStartTurn()
    {
        // 1. 덱에서 주사위 뽑기
        
        drawnDice.Clear();
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

    public void ClearDeck()
    {
        
    }
}
