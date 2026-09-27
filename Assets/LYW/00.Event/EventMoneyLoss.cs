using JJB.Script.Battle.Player.Progression;
using UnityEngine;

public sealed class EventMoneyLoss : EventEffect
{
    [SerializeField, Range(0f, 1f)]
    private float lossRate = 0.3f;

    public override void Apply()
    {
        if (PlayerProfileManager.Instance == null)
        {
            Debug.LogError(
                "PlayerProfileManager.Instance가 없습니다.",
                this
            );

            return;
        }

        PlayerProfile profile =
            PlayerProfileManager.Instance.Profile;

        if (profile == null)
        {
            Debug.LogError(
                "PlayerProfile이 생성되지 않았습니다.",
                this
            );

            return;
        }

        int lostMoney =
            Mathf.FloorToInt(
                profile.money * lossRate
            );

        profile.money -= lostMoney;

        Debug.Log(
            $"이벤트로 {lostMoney}원을 잃었습니다. 현재 돈: {profile.money}"
        );
    }
}