using JJB.Script.Battle.Player.Progression;
using UnityEngine;

public sealed class EventMoneyLoss : EventEffect
{
    [SerializeField, Range(0f, 1f)]
    private float lossRate = 0.3f;

    public override void Apply()
    {
        PlayerProfile profile =
            PlayerProfileManager.Instance.Profile;

        int lostMoney =
            Mathf.FloorToInt(
                profile.money * lossRate
            );

        profile.money -= lostMoney;
    }
}