using JJB.Script.Battle.Player.Progression;
using TMPro;
using UnityEngine;

public class MoneyText : MonoBehaviour
{
    private TextMeshProUGUI _moneyText;
    private int _lastMoney = -1;

    private void Awake()
    {
        _moneyText = GetComponent<TextMeshProUGUI>();
    }

    private void Update()
    {
        if (PlayerProfileManager.Instance == null)
            return;

        int currentMoney = PlayerProfileManager.Instance.Profile.money;

        if (_lastMoney == currentMoney)
            return;

        _lastMoney = currentMoney;
        _moneyText.text = currentMoney.ToString();
    }
}
