using System.Collections.Generic;

[System.Serializable]
public class SaveData
{
    // 1. 진행 상황 및 씬
    public int currentSceneNum;

    // 2. 현재 칩
    public int currntChip;

    // 3. 주사위 덱 및 상점 품목 (고유 이름/ID 리스트)
    public List<string> allDiceIDs = new List<string>();
    public List<string> diceDeckIDs = new List<string>();
    public List<string> shopItemIDs = new List<string>();


    public BattleSaveData BattleSaveData;
    public ShopSaveData ShopSaveData;

}