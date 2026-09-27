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
            Debug.Log($"돈이 부족합니다. 필요 금액: {price}칩", this);
            return;
        }

        if (!EventExternalSystems.TryAddDice(dice, this))
            return;

        profile.money -= price;

        Debug.Log(
            $"{dice.diceName}을(를) {price}칩에 구매했습니다. 남은 돈: {profile.money}",
            this
        );
    }
}
