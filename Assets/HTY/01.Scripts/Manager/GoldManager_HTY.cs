using TMPro;
using UnityEngine;

public class GoldManager_HTY : MonoBehaviour
{
    public static GoldManager_HTY instance;
    public int _myGold = 0;
    public TextMeshProUGUI _goldText;
    private void Awake()
    {
        instance = this;
    }

    public void AddGold(int addGold)
    {
        _myGold+= addGold;
        if (_goldText != null)
            _goldText.text=($"{_myGold}");
    }

    public void SpendGold(int spendGold)
    {
        _myGold-= spendGold;
        if (_goldText != null)
            _goldText.text = ($"{_myGold}");
    }
}
