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
            Debug.Log("주사위에 걸 돈이 없습니다.", this);
            return;
        }

        profile.money -= actualStake;

        int roll = Random.Range(1, 7);
        int multiplier = roll <= 3 ? 2 : 3;
        int reward = actualStake * multiplier;

        profile.money += reward;

        Debug.Log(
            $"주사위 결과 {roll}! {multiplier}배인 {reward}칩을 받았습니다. " +
            $"현재 돈: {profile.money}",
            this
        );
    }
}
