using JJB.Script.Battle.Player.Progression;
using UnityEngine;

public sealed class EventRouletteGamble : EventEffect
{
    [SerializeField, Min(1)] private int stake = 100;
    [SerializeField, Range(0f, 1f)] private float successRate = 0.5f;
    [SerializeField, Min(1f)] private float rewardMultiplier = 2.5f;

    public override void Apply()
    {
        if (!EventExternalSystems.TryGetProfile(this, out PlayerProfile profile))
            return;

        int actualStake = Mathf.Min(stake, profile.money);

        if (actualStake <= 0)
        {
            const string noMoney = "도박에 걸 돈이 없습니다.";
            Debug.Log(noMoney, this);
            ShowResult(noMoney);
            return;
        }

        profile.money -= actualStake;
        bool success = Random.value < successRate;

        if (!success)
        {
            string failure =
                $"룰렛 실패!\n{actualStake}칩을 잃었습니다. 현재 돈: {profile.money}칩";
            Debug.Log(failure, this);
            ShowResult(failure);
            return;
        }

        int reward = Mathf.RoundToInt(actualStake * rewardMultiplier);
        profile.money += reward;

        string message =
            $"룰렛 성공!\n{reward}칩 획득! 현재 돈: {profile.money}칩";

        Debug.Log(message, this);
        ShowResult(message);
    }
}
