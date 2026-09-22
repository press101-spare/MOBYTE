using System.Collections.Generic;
using JJB.Script.Battle.Stage;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DataManager: MonoBehaviour
{
    [Header("필수 매니저 참조")]
    public SaveManager saveManager;
    public DiceDatabase diceDatabase;
    public StageManager stageManager;

    [Header("현재 게임 플레이 데이터")]
    public int playerCurrentHp;
    public int currentSceneIndex => SceneManager.GetActiveScene().buildIndex;
    public int currentStage => stageManager.CurrentStageIndex;
    public int gambleProgressData = 0;

    [Header("현재 플레이어의 주사위 덱")] 
    public List<DiceSO_JCY> playerDeck => DiceDeckManager_JCY.Instance.diceCollection;

    // ===================================================
    // 🔴 게임 저장 (Save)
    // ===================================================
    public void SaveGame()
    {
        // 1. 새로운 SaveData 상자 생성
        SaveData data = new SaveData();

        // 2. 현재 정보 복사
        data.currentSceneNum = currentSceneIndex;
        data.currentStage = currentStage;

        data.playerCurrentHp = playerCurrentHp;

        data.gambleProgressData = gambleProgressData;

        // 3. 주사위 SO 덱을 문자열(String) 이름 리스트로 변환하여 저장
        data.diceDeckIDs.Clear();
        foreach (DiceSO_JCY dice in playerDeck)
        {
            if (dice != null)
            {
                data.diceDeckIDs.Add(dice.diceName);
            }
        }

        // 4. SaveManager를 통해 파일로 출력
        saveManager.SaveGame(data);
    }

    // ===================================================
    // 🟢 게임 불러오기 (Load)
    // ===================================================
    public void LoadGame()
    {
        // 1. SaveManager를 통해 파일 읽어오기
        SaveData data = saveManager.LoadGame();
        if (data == null)
        {
            Debug.Log("[GameManager] 불러올 세이브 데이터가 없습니다.");
            return;
        }

        // 2. 기본 상태 데이터 복원
        playerCurrentHp = data.playerCurrentHp;

        gambleProgressData = data.gambleProgressData;

        // 3. 문자열(String) 주사위 이름 리스트를 실제 DiceSO 덱으로 복원
        playerDeck.Clear();
        foreach (string diceName in data.diceDeckIDs)
        {
            DiceSO_JCY foundDice = diceDatabase.GetDiceSOByName(diceName);
            if (foundDice != null)
            {
                playerDeck.Add(foundDice);
            }
        }

        Debug.Log("[GameManager] 게임 불러오기 및 덱 복원 완료!");

        // 4. (필요 시) 저장되어 있던 씬으로 이동
        SceneManager.LoadScene(data.currentSceneNum);
    }
}