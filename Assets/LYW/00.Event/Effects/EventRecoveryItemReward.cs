using UnityEngine;

public sealed class EventRecoveryItemReward : EventEffect
{
    [SerializeField] private DiceSO_JCY recoveryItem;

    public override void Apply()
    {
        DiceSO_JCY item = recoveryItem;

        if (item == null)
            item = FindRecoveryItem(EventExternalSystems.GetGlobalDicePool());

        if (!EventExternalSystems.TryAddDice(item, this))
            return;

        Debug.Log($"회복 아이템 획득: {item.diceName}", this);
    }

    private static DiceSO_JCY FindRecoveryItem(DiceSO_JCY[] pool)
    {
        if (pool == null)
            return null;

        foreach (DiceSO_JCY dice in pool)
        {
            if (dice == null)
                continue;

            if (dice.diceEffectType == DiceSO_JCY.DiceEffectType.Potion ||
                dice.diceEffectType == DiceSO_JCY.DiceEffectType.Health)
                return dice;
        }

        return null;
    }
}
