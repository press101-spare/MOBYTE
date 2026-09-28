using JJB.Script.Battle.Player.Progression;
using UnityEngine;

/// <summary>
/// 연결된 주사위를 원래 가격보다 할인하여 구매하는 선택지 효과입니다.
/// discountRate 0.2는 20% 할인을 뜻합니다.
/// </summary>
public sealed class EventDiscountDicePurchase : EventEffect
{
    [SerializeField] private DiceSO_JCY dice;
    [SerializeField, Range(0f, 1f)] private float discountRate = 0.2f;

    public override void Apply()
    {
        if (dice == null ||
            !EventExternalSystems.TryGetProfile(this, out PlayerProfile profile))
            return;

        // 가격이 소수가 되면 올림하고 음수 가격은 허용하지 않습니다.
        int price = Mathf.Max(
            0,
            Mathf.CeilToInt(dice.cost * (1f - discountRate))
        );

        if (profile.money < price)
        {
            string noMoney = $"돈이 부족합니다.\n필요 금액: {price}칩";
            Debug.Log(noMoney, this);
            ShowResult(noMoney);
            return;
        }

        // 지급이 성공한 경우에만 돈을 차감합니다.
        if (!EventExternalSystems.TryAddDice(dice, this))
            return;

        profile.money -= price;

        string message =
            $"{dice.diceName} 구매 완료!\n가격: {price}칩 / 남은 돈: {profile.money}칩";

        Debug.Log(message, this);
        ShowResult(message);
    }
}
