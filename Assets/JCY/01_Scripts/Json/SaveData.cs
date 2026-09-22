using System.Collections.Generic;

[System.Serializable]
public class SaveData
{
    // 1. 진행 상황 및 씬
    public int currentSceneNum;
    public int currentStage;

    // 2. 플레이어 및 적 체력
    public int playerCurrentHp;
    public int currntChip;

    // 3. 주사위 덱 및 상점 품목 (고유 이름/ID 리스트)
    public List<string> diceDeckIDs = new List<string>();
    public List<string> shopItemIDs = new List<string>();

    // 4. 도박 진행사항
    public int gambleProgressData;
}