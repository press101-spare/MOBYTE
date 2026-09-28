using JJB.Script.Battle.Player.Progression;
using UnityEngine;

/// <summary>
/// 주사위 하나를 굴려 1~3이면 베팅금의 2배,
/// 4~6이면 베팅금의 3배를 돌려주는 이벤트입니다.
/// </summary>
public sealed class EventDiceMultiplierGamble : EventEffect
{
    // 실제 보유 금액이 더 적으면 가진 돈까지만 베팅합니다.
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

        // 먼저 베팅금을 차감한 뒤 결과 보상을 더합니다.
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
