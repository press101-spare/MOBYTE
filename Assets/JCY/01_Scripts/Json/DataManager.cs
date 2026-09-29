using System;
using System.Collections.Generic;
using JJB.Script.Battle;
using JJB.Script.Battle.Player.Progression;
using JJB.Script.Battle.Stage;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class DataManager : MonoBehaviour
{
    [Header("필수 매니저 참조")]
    private SaveManager saveManager;
    private DiceDatabase diceDatabase;

    [Header("현재 게임 플레이 데이터")]
    public int currentSceneIndex => SceneManager.GetActiveScene().buildIndex;
    public int currentStage => JJBGameManager.Instance != null && JJBGameManager.Instance.StageManager != null 
        ? JJBGameManager.Instance.StageManager.CurrentStageIndex 
        : 0;

    public int currentChip => PlayerProfileManager.Instance != null && PlayerProfileManager.Instance.Profile != null
        ? PlayerProfileManager.Instance.Profile.money
        : 0;

    [Header("현재 플레이어의 주사위 덱")] 
    public List<DiceSO_JCY> playerDeck => DiceDeckManager_JCY.Instance != null ? DiceDeckManager_JCY.Instance.diceCollection : null;
    
    [Header("모든 주사위 SO")] 
    public DiceSO_JCY[] allDiceSo => DiceManager_JCY.Instance != null ? DiceManager_JCY.Instance.allDiceSo : null;

    // 🟢 씬이 전환되어도 파괴되지 않고 세이브 데이터를 유지하는 정적(static) 대기열
    private static SaveData pendingSaveData = null;
    
    public BattleSaveData CurrentBattleData { get; private set; }

    private void Awake()
    {
        saveManager = GetComponent<SaveManager>();
        diceDatabase = GetComponent<DiceDatabase>();
    }

    private void Start()
    {
        if (pendingSaveData != null)
        {
            ApplyLoadedData(pendingSaveData);
            pendingSaveData = null;
        }
    }

    // ===================================================
    // 🔴 게임 저장 (Save)
    // ===================================================
    public void SaveGame()
    {
        SaveData data = saveManager.LoadGame();

        if (data == null)
            data = new SaveData();

        data.currentSceneNum = currentSceneIndex;

        // 돈 저장
        if (PlayerProfileManager.Instance != null &&
            PlayerProfileManager.Instance.Profile != null)
        {
            data.currntChip =
                PlayerProfileManager.Instance.Profile.money;
        }

        switch (data.currentSceneNum)
        {
            case 0:
                if (data.ShopSaveData == null)
                    data.ShopSaveData = new ShopSaveData();

                break;

            case 2:
                if (data.BattleSaveData == null)
                    data.BattleSaveData = new BattleSaveData();

                if (JJBGameManager.Instance != null &&
                    JJBGameManager.Instance.PlayerJjbHealth != null)
                {
                    data.BattleSaveData.playerHp =
                        JJBGameManager.Instance.PlayerJjbHealth.CurrentHealth;
                }

                data.BattleSaveData.currentStage =
                    currentStage;

                break;

            case 3:
                break;
        }

        // 주사위 덱 갱신
        data.diceDeckIDs.Clear();

        if (playerDeck != null)
        {
            foreach (DiceSO_JCY dice in playerDeck)
            {
                if (dice != null)
                    data.diceDeckIDs.Add(dice.diceName);
            }
        }

        // 모든 주사위 갱신
        data.allDiceIDs.Clear();

        if (allDiceSo != null)
        {
            foreach (DiceSO_JCY dice in allDiceSo)
            {
                if (dice != null)
                    data.allDiceIDs.Add(dice.diceName);
            }
        }

        Debug.Log($"저장되는 돈 : {data.currntChip}");

        saveManager.SaveGame(data);
    }

    // ===================================================
    // 🟢 게임 불러오기 (Load) - 1단계: 씬 불러오기 요청
    // ===================================================
    public void LoadGame()
    {
        SaveData data = saveManager.LoadGame();
        if (data == null)
        {
            Debug.Log("[DataManager] 불러올 세이브 데이터가 없습니다.");
            return;
        }

        // 1. 세이브 데이터를 static 보관함에 임시 보관
        pendingSaveData = data;

        // 2. 저장되어 있던 씬으로 이동 (이 순간 기존 씬 파괴 및 새 씬 로드 시작)
        SceneManager.LoadScene(data.currentSceneNum);
    }

    // ===================================================
    // 🟢 게임 불러오기 (Load) - 2단계: 새 씬에서 실제 데이터 복원
    // ===================================================
    private void ApplyLoadedData(SaveData data)
    {
        // 1. 기본 상태 데이터 복원
        CurrentBattleData = data.BattleSaveData;
        
        // 저장된 스테이지 복원
        if (CurrentBattleData != null &&
            JJBGameManager.Instance != null &&
            JJBGameManager.Instance.StageManager != null)
        {
            JJBGameManager.Instance.StageManager.SetStageIndex(
                CurrentBattleData.currentStage
            );
        }
        
        if (PlayerProfileManager.Instance != null && PlayerProfileManager.Instance.Profile != null)
        {
            PlayerProfileManager.Instance.Profile.money = data.currntChip;
        }

        // 2. 주사위 덱 복원
        if (playerDeck != null)
        {
            playerDeck.Clear();
            foreach (string diceName in data.diceDeckIDs)
            {
                DiceSO_JCY foundDice = diceDatabase.GetDiceSOByName(diceName);
                if (foundDice != null)
                {
                    playerDeck.Add(foundDice);
                }
            }
        }
        
        // 3. 모든 주사위 SO 복원
        if (DiceManager_JCY.Instance != null)
        {
            List<DiceSO_JCY> loadedDiceList = new List<DiceSO_JCY>();

            foreach (string diceName in data.allDiceIDs)
            {
                DiceSO_JCY foundDice = diceDatabase.GetDiceSOByName(diceName);
                if (foundDice != null)
                {
                    loadedDiceList.Add(foundDice);
                }
            }

            DiceManager_JCY.Instance.allDiceSo = loadedDiceList.ToArray();
        }
    }
}