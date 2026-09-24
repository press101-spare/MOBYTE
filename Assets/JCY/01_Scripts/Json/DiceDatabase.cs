using System.Collections.Generic;
using UnityEngine;

public class DiceDatabase : MonoBehaviour
{
    [Header("게임에 존재하는 모든 주사위 SO를 여기에 등록하세요")]
    public DiceSO_JCY[] allDiceArray; // => 구문 및 [SerializeField] 제거

    // 빠른 검색을 위한 사전 (Key: diceName, Value: DiceSO)
    private Dictionary<string, DiceSO_JCY> diceDictionary = new Dictionary<string, DiceSO_JCY>();

    private void Awake()
    {
        InitializeDatabase();
    }

    // 씬 시작 시 사전(Dictionary)을 빌드합니다.
    private void InitializeDatabase()
    {
        diceDictionary.Clear();

        foreach (DiceSO_JCY dice in allDiceArray)
        {
            if (dice != null && !string.IsNullOrEmpty(dice.diceName))
            {
                if (!diceDictionary.ContainsKey(dice.diceName))
                {
                    diceDictionary.Add(dice.diceName, dice);
                }
                else
                {
                    Debug.LogWarning($"[DiceDatabase] 중복된 주사위 이름이 있습니다: {dice.diceName}");
                }
            }
        }
    }

    // 이름(String)을 넘겨주면 일치하는 DiceSO를 찾아 반환합니다.
    public DiceSO_JCY GetDiceSOByName(string diceName)
    {
        if (diceDictionary.TryGetValue(diceName, out DiceSO_JCY foundDice))
        {
            return foundDice;
        }

        Debug.LogError($"[DiceDatabase] 해당 이름의 주사위를 찾을 수 없습니다: {diceName}");
        return null;
    }
}