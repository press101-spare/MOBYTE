using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Betting_HTY : MonoBehaviour
{
    [SerializeField] private TMP_InputField coinBetting;
    public void BettingCoin()
    {
        if(int.TryParse(coinBetting.text,out int a)&& a > 0)
        {
            if(!(a>GoldManager_HTY.instance._myGold))
            {
                GoldManager_HTY.instance.SpendGold(a);
                Debug.Log($"{a}");
            }
            else
            {
                //잘못된 값
            }
            
        }
    }
}
