using JJB.Script.Battle.Player.Progression;
using UnityEngine;

public sealed class EventDiscountDicePurchase : EventEffect
{
    [SerializeField] private DiceSO_JCY dice;
    [SerializeField, Range(0f, 1f)] private float discountRate = 0.2f;

    public override void Apply()
    {
        if (dice == null ||
            !EventExternalSystems.TryGetProfile(this, out PlayerProfile profile))
            return;

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

        if (!EventExternalSystems.TryAddDice(dice, this))
            return;

        profile.money -= price;

        string message =
            $"{dice.diceName} 구매 완료!\n가격: {price}칩 / 남은 돈: {profile.money}칩";

        Debug.Log(message, this);
        ShowResult(message);
    }
}
