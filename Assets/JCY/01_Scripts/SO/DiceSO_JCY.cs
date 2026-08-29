using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "DiceSO_JCY", menuName = "Scriptable Objects/DiceSO_JCY")]
public class DiceSO_JCY : ScriptableObject
{
    [Header("주사위 정보")]
    public string diceName;          // 주사위 이름
    public string diceDescription;   // 주사위 설명
    public Sprite diceIcon;          // 대표 아이콘
    public int cost;          // 주사위 상점 가격
    public Color Color;
    public GameObject dicePrefab; // 각 주사위 전용 3D 프리팹 등록
    public DiceEffectType diceEffectType;

    public DiceSO_JCY[] shodice;
    

    // [Header("주사위 눈 설정")]
    // new Vector3(0f, 180f, 0f),   // Index 1 (숫자 1)
    // new Vector3(0f, 90f, 0f),    // Index 2 (숫자 2)
    // new Vector3(0f, 0f, 0f),     // Index 3 (숫자 3)
    // new Vector3(90f, 0f, 0f),    // Index 4 (숫자 4)
    // new Vector3(0f, 270f, 0f),   // Index 5 (숫자 5)
    // new Vector3(-90f, 0f, 0f)    // Index 6 (숫자 6)

    public int[] faceValues = { 1 , 2, 3, 4,5, 6};  //주사위 눈 값
    
    
    public enum DiceEffectType
    {
        None , Even , Odd , Blood , Shield , Allin , Vampire , Reroll , Joker , Debt
    }

    // 무작위로 눈 하나를 뽑아주는 함수
    public int GetRandomIndex()
    {
        List<int> availableIndexes = new List<int>();

        for (int i = 0; i < faceValues.Length; i++)
        {
            switch (diceEffectType)
            {
                case DiceEffectType.None:
                    availableIndexes.Add(i);
                    break;

                case DiceEffectType.Even:
                    if (faceValues[i] % 2 == 0)
                        availableIndexes.Add(i);
                    break;

                case DiceEffectType.Odd:
                    if (faceValues[i] % 2 != 0)
                        availableIndexes.Add(i);
                    break;
            }
        }

        int num = availableIndexes[Random.Range(0, availableIndexes.Count)];
        Debug.Log("나온 주사위 값:" + num);
        return num;
    }
}
