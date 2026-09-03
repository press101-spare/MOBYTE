using System;
using System.Collections.Generic;
using UnityEngine;

public class ShledDice_JCY : MonoBehaviour
{
    private ShledDice_JCY Instance;
    public List<DiceSO_JCY> shideDiceList = new List<DiceSO_JCY>();
    public DiceSO_JCY shideDiceSO;
    public int shledValue;
    public int shledDiceCount;

    public int ShledAddValue;
    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }

    public void shideDraw()
    {
        shledDiceCount = DiceManager_JCY.Instance.reRollUI.reRollCount + ShledAddValue;
        ShledeReset();
        for (int i = 0; i <shledDiceCount ; i++)
        {
            shideDiceList.Add(shideDiceSO);
        }

        DiceManager_JCY.Instance.reRollUI.ResetReRollCount(0);
        DiceManager_JCY.Instance.isShled = true;
        DiceManager_JCY.Instance.StartTurn(shideDiceList);
    }

    public void ShledeHP(int value)
    {
        shledValue = value;
        Debug.Log(shledValue + "만큼 쉴드 생성");
    }

    public void ShledeReset()
    {
        shideDiceList.Clear();
        ShledAddValue = 0;
        DiceManager_JCY.Instance.isShled = true;
    }

    public void ShledAdd(int value)
    {
        if (value + ShledAddValue > 2)
        {
            return;
        }

        ShledAddValue += value;
    }
    
}
