using JJB.Script.Battle.Player.Progression;
using UnityEngine;

public sealed class EventMoneyLoss : EventEffect
{
    [SerializeField, Range(0f, 1f)]
    private float lossRate = 0.3f;

    public override void Apply()
    {
        if (!EventExternalSystems.TryGetProfile(this, out PlayerProfile profile))
            return;

        int lostMoney =
            Mathf.FloorToInt(
                profile.money * lossRate
            );

        profile.money -= lostMoney;

        string message =
            $"돈 {lostMoney}칩을 잃었습니다.\n현재 돈: {profile.money}칩";

        Debug.Log(message, this);
        ShowResult(message);
    }
}
