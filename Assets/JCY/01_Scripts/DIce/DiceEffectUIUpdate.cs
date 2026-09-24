using System;
using System.Collections.Generic;
using UnityEngine;

public class DiceEffectUIUpdate : MonoBehaviour
{
    [SerializeField] private DiceEffectSlotUI[] slots;

    private void Start()
    {
        DiceDeckManager_JCY.Instance.diceEffectUIUpdate = this;
    }

    public void UpdateUI(List<DiceSO_JCY> drawnDiceSO)
    {
        
        // 모든 슬롯 비활성화
        for (int i = 0; i < slots.Length; i++)
        {
            slots[i].gameObject.SetActive(false);
            Debug.Log("되는데 비활성화");
            
        }

        // 중복 주사위와 개수 정리
        Dictionary<DiceSO_JCY, int> diceCounts = new Dictionary<DiceSO_JCY, int>();

        foreach (DiceSO_JCY diceSO in drawnDiceSO)
        {
            if (diceCounts.ContainsKey(diceSO))
                diceCounts[diceSO]++;
            else
                diceCounts.Add(diceSO, 1);
        }

        // 슬롯에 표시
        int slotIndex = 0;
        Debug.Log($"drawnDiceSO 개수: {drawnDiceSO.Count}");
        Debug.Log($"diceCounts 개수: {diceCounts.Count}");


        foreach (var dice in diceCounts)
        {
            slots[slotIndex].SetData(dice.Key, dice.Value);
            slots[slotIndex].gameObject.SetActive(true);

            slotIndex++;
            Debug.Log("되는데 활성화");
        }
    }
    
    public void ShowUi()
    { 
        if (gameObject.activeInHierarchy) 
        {
                gameObject.SetActive(false); 
        }
        else
        {
                gameObject.SetActive(true);
            
        }
    }
}
