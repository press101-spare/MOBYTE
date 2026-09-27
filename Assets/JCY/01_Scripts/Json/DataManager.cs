using System;
using System.Collections.Generic;
using JJB.Script.Battle;
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
    public int playerCurrentHp;
    public int currentSceneIndex => SceneManager.GetActiveScene().buildIndex;
    public int currentStage => JJBGameManager.Instance != null && JJBGameManager.Instance.StageManager != null 
        ? JJBGameManager.Instance.StageManager.CurrentStageIndex 
        : 0;
    public int gambleProgressData = 0;

    [Header("현재 플레이어의 주사위 덱")] 
    public List<DiceSO_JCY> playerDeck => DiceDeckManager_JCY.Instance != null ? DiceDeckManager_JCY.Instance.diceCollection : null;
    
    [Header("모든 주사위 SO")] 
    public DiceSO_JCY[] allDiceSo => DiceManager_JCY.Instance != null ? DiceManager_JCY.Instance.allDiceSo : null;

    // 🟢 씬이 전환되어도 파괴되지 않고 세이브 데이터를 유지하는 정적(static) 대기열
    private static SaveData pendingSaveData = null;

    private void Awake()
    {
        saveManager = GetComponent<SaveManager>();
        diceDatabase = GetComponent<DiceDatabase>();
    }

    private void Start()
    {
        // 🟢 씬이 로드 완료되고 매니저들이 Awake()를 마친 직후(Start 시점) 대기 데이터가 있다면 복원!
        if (pendingSaveData != null)
        {
            ApplyLoadedData(pendingSaveData);
            pendingSaveData = null; // 대기 데이터 사용 완료 후 초기화
        }
    }

    private void Update()
    {
        if (Keyboard.current.sKey.wasPressedThisFrame)
        {
            SaveGame();
        }
        if (Keyboard.current.dKey.wasPressedThisFrame)
        {
            saveManager.DeleteSaveFile();
        }
        if (Keyboard.current.lKey.wasPressedThisFrame)
        {
            LoadGame();
        }
    }

    // ===================================================
    // 🔴 게임 저장 (Save)
    // ===================================================
    public void SaveGame()
    {
        SaveData data = new SaveData();

        data.currentSceneNum = currentSceneIndex;
        data.currentStage = currentStage;
        data.playerCurrentHp = playerCurrentHp;
        data.gambleProgressData = gambleProgressData;

        // 주사위 SO 덱 저장
        if (playerDeck != null)
        {
            foreach (DiceSO_JCY dice in playerDeck)
            {
                if (dice != null)
                {
                    data.diceDeckIDs.Add(dice.diceName);
                }
            }
        }
        
        // 모든 주사위 SO 배열 저장
        if (allDiceSo != null)
        {
            foreach (DiceSO_JCY dice in allDiceSo)
            {
                if (dice != null)
                {
                    data.allDiceIDs.Add(dice.diceName);
                }
            }
        }

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
        playerCurrentHp = data.playerCurrentHp;
        gambleProgressData = data.gambleProgressData;

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

        Debug.Log("[DataManager] 씬 이동 완료 및 세이브 데이터 최종 적용 성공!");
    }
}