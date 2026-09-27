using System.IO;
using UnityEngine;

public class SaveManager : MonoBehaviour
{
    // PC / 모바일 공통으로 가장 안전한 세이브 파일 경로
    private string SavePath => Path.Combine(Application.persistentDataPath, "game_save.json");

    // [1] 데이터 저장하기
    public void SaveGame(SaveData data)
    {
        // 1. SaveData 객체를 JSON 문자열로 변환 (true = 보기 좋게 들여쓰기)
        string jsonString = JsonUtility.ToJson(data, true);

        // 2. 파일에 JSON 문자열 쓰기
        File.WriteAllText(SavePath, jsonString);

        Debug.Log($"[SaveManager] 게임 저장 완료! 경로: {SavePath}");
    }

    // [2] 데이터 불러오기
    public SaveData LoadGame()
    {
        
        if (File.Exists(SavePath))
        {
            // 1. 파일에서 JSON 문자열 읽어오기
            string jsonString = File.ReadAllText(SavePath);

            // 2. JSON 문자열을 SaveData 객체로 역직렬화
            SaveData data = JsonUtility.FromJson<SaveData>(jsonString);

            Debug.Log("[SaveManager] 세이브 파일을 성공적으로 불러왔습니다.");
            return data;
        }
        else
        {
            Debug.LogWarning("[SaveManager] 세이브 파일이 존재하지 않습니다.");
            return null;
        }
    }

    // [3] 세이브 파일 삭제 (초기화용)
    public void DeleteSaveFile()
    {
        if (File.Exists(SavePath))
        {
            File.Delete(SavePath);
            Debug.Log("[SaveManager] 세이브 파일이 삭제되었습니다.");
        }
    }
}