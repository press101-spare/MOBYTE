using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DiceEffectSlotUI : MonoBehaviour
{
    [SerializeField] private Image diceImage;
    [SerializeField] private TextMeshProUGUI nameText;
    [SerializeField] private TextMeshProUGUI descriptionText;
    [SerializeField] private DiceSO_JCY defaultDice;

    [SerializeField] Image myImage;

    private void Start()
    {
        SetData(defaultDice);
    }
    public void SetData(DiceSO_JCY diceSO)
    {
        diceImage.sprite = diceSO.diceIcon;
        myImage.color = diceSO.color;
        nameText.text = diceSO.diceName;
        descriptionText.text = diceSO.diceDescription;
        
    }
}
