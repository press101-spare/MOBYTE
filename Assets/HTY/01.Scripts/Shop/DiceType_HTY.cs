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
        //여기에 인벤토리 추가 구현
    }
}
