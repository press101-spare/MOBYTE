using System;
using JJB.Script.Battle.Player.Progression;
using UnityEngine;

public class EventMoneyLoss : MonoBehaviour
{
    private PlayerProfile _playerProfile;
    private float _stolenMoney = 0.7f;

    private void Update()
    {
        _playerProfile = PlayerProfileManager.Instance.Profile;
        int lostmoney = Mathf.FloorToInt(_playerProfile.money * _stolenMoney);
    }
}
