using JJB.Script.Battle.Player.Progression;
using TMPro;
using UnityEngine;

public class MoneyText : MonoBehaviour
{
    private TMP_Text _moneyText;
    private int _lastMoney = -1;

    private void Awake()
    {
        _moneyText = GetComponent<TMP_Text>();
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
