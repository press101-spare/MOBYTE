using UnityEngine;

public class DiceType_HTY : MonoBehaviour
{
    public DiceSO_JCY _myDice;

    public void SetDice(DiceSO_JCY dice)
    {
        _myDice = dice;
    }

    public void BuyDice()
    {
        TotalManager.Instance.BuyItem(_myDice);
        DiceDeckManager_JCY.Instance.AddDice(_myDice);
        Debug.Log($"성공적으로{_myDice}구매했습니다zz");
    }
}
