using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DiceEffectSlotUI : MonoBehaviour
{
    [SerializeField] private Image diceImage;
    [SerializeField] private TextMeshProUGUI nameText;
    [SerializeField] private TextMeshProUGUI descriptionText;
    [SerializeField] private TMP_Text diceCount;

    public void SetData(DiceSO_JCY diceSO , int count)
    {
        diceImage.sprite = diceSO.diceIcon;
        nameText.text = diceSO.diceName;
        descriptionText.text = diceSO.diceDescription;
        
        diceCount.text = count > 1 ? $"x{count}" : "";
    }
}
