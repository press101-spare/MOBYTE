using JJB.Script.Battle.Player.Progression;
using UnityEngine;

public sealed class EventDiceMultiplierGamble : EventEffect
{
    [SerializeField, Min(1)] private int stake = 100;

    public override void Apply()
    {
        if (!EventExternalSystems.TryGetProfile(this, out PlayerProfile profile))
            return;

        int actualStake = Mathf.Min(stake, profile.money);

        if (actualStake <= 0)
        {
            const string noMoney = "주사위에 걸 돈이 없습니다.";
            Debug.Log(noMoney, this);
            ShowResult(noMoney);
            return;
        }

        profile.money -= actualStake;

        int roll = Random.Range(1, 7);
        int multiplier = roll <= 3 ? 2 : 3;
        int reward = actualStake * multiplier;

        profile.money += reward;

        string message =
            $"주사위 결과: {roll}\n{multiplier}배 보상 {reward}칩 획득! " +
            $"현재 돈: {profile.money}칩";

        Debug.Log(message, this);
        ShowResult(message);
    }
}
