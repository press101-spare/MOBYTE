using JJB.Script.Battle.Player.Progression;
using UnityEngine;

/// <summary>
/// 현재 보유 금액에서 지정한 비율만큼 차감합니다.
/// Inspector에서 0.3은 30%, 0.5는 50%를 의미합니다.
/// </summary>
public sealed class EventMoneyLoss : EventEffect
{
    [SerializeField, Range(0f, 1f)]
    private float lossRate = 0.3f;

    public override void Apply()
    {
        // 이벤트 장면에 매니저가 없어도 공통 연결부가 프로필을 준비합니다.
        if (!EventExternalSystems.TryGetProfile(this, out PlayerProfile profile))
            return;

        // 소수점 금액이 생기지 않도록 내림 처리합니다.
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
