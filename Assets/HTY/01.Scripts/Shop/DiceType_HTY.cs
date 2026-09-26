using JJB.Script.Battle.Player.Progression;
using UnityEngine;
using UnityEngine.UI;

public class DiceType_HTY : MonoBehaviour
{
    public DiceSO_JCY _myDice;

    public void SetDice(DiceSO_JCY dice)
    {
        _myDice = dice;
    }

    public void BuyDice()
    {
        PlayerProfileManager.Instance.Profile.money-=_myDice.cost;
        DiceDeckManager_JCY.Instance.AddDice(_myDice);
        Debug.Log($"성공적으로{_myDice}구매했습니다zz");
        transform.GetChild(3).GetComponent<Image>().color = Color.gray;
        transform.GetComponentInChildren<Button>().interactable = false;
        GambleManager.instance._shopChipText.text = PlayerProfileManager.Instance.Profile.money.ToString();
    }

    public void SetRe()
    {
        transform.GetChild(3).GetComponent<Image>().color = Color.white;
        transform.GetComponentInChildren<Button>().interactable = true;
    }
}
