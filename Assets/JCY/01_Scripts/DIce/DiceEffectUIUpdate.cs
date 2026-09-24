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

        foreach (var dice in diceCounts)
        {
            slots[slotIndex].SetData(dice.Key, dice.Value);
            slots[slotIndex].gameObject.SetActive(true);

            slotIndex++;
        }
    }
}
