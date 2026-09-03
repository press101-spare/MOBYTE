using System;
using TMPro;
using UnityEngine;

public class ReRollCount_JCY : MonoBehaviour
{
    public int reRollCount;
    [SerializeField] private TextMeshProUGUI reRollCountUI;
    public bool Debt;

    public void ResetReRollCount(int value)
    {
        if (Debt)
        {
            Debt = false;
            value -= 1;
        }
        reRollCount = value;
        reRollCountUI.text = "Count " + reRollCount.ToString();
    }


    public void UpdateReRollCount(int value)
    {
        if(reRollCount >= 4 && value >= 1)
            return;
        reRollCount += value;
        reRollCountUI.text = "Count " + reRollCount.ToString();
    }
    
}
