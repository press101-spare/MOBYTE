using TMPro;
using UnityEngine;

public class GoldManager_HTY : MonoBehaviour
{
    public static GoldManager_HTY instance;
    public int _gold = 0;
    public TextMeshProUGUI _goldText;
    private void Awake()
    {
        instance = this;
    }

    public void AddGold(int addGold)
    {
        _gold+= addGold;
        _goldText.text=($"{_gold}");
    }

    public void SpendGold(int spendGold)
    {
        _gold-= spendGold;
        _goldText.text = ($"{_gold}");
    }
}
