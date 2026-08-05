using UnityEngine;

[CreateAssetMenu(fileName = "DiceSO_JCY", menuName = "Scriptable Objects/DiceSO_JCY")]
public class DiceSO_JCY : ScriptableObject
{
    [Header("주사위 정보")]
    public string diceName;          // 주사위 이름
    public string diceDescription;   // 주사위 설명
    public Sprite diceIcon;          // 대표 아이콘
    public GameObject dicePrefab; // 각 주사위 전용 3D 프리팹 등록

    [Header("주사위 눈 설정")]
    //Vector3(0f, 0f, 0f),     // Index 0 (기본 모델링의 3번 면)
    //Vector3(0f, 90f, 0f),    // Index 1 (기본 모델링의 2번 면)
    //Vector3(0f, 180f, 0f),   // Index 2 (기본 모델링의 1번 면)
    //Vector3(0f, 270f, 0f),   // Index 3 (기본 모델링의 5번 면)
    //Vector3(90f, 0f, 0f),    // Index 4 (기본 모델링의 4번 면)
    //Vector3(-90f, 0f, 0f)    // Index 5 (기본 모델링의 6번 면)
    public int[] faceValues = new int[6];  //주사위 눈 값

    // 무작위로 눈 하나를 뽑아주는 함수
    public int GetRandomIndex()
    {
        return Random.Range(0, faceValues.Length);
    }
}
