using System;
using TMPro;
using UnityEngine;

public class ReRollUI_JCY : MonoBehaviour
{
    public int reRollCount;
    [SerializeField] private TextMeshProUGUI reRollCountUI;

    public void ResetReRollCount(int value)
    {
        reRollCount = value;
    }
    

    public void UpdateReRollCount(int value)
    {
        reRollCount += value;
        reRollCountUI.text = "Count " + reRollCount.ToString();
    }
    
}
