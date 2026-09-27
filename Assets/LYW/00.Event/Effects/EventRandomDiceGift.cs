using UnityEngine;

public sealed class EventRandomDiceGift : EventEffect
{
    [SerializeField] private DiceSO_JCY[] dicePool;

    public override void Apply()
    {
        DiceSO_JCY[] pool =
            dicePool != null && dicePool.Length > 0
                ? dicePool
                : EventExternalSystems.GetGlobalDicePool();

        DiceSO_JCY selected = EventExternalSystems.GetRandomDice(pool);

        if (!EventExternalSystems.TryAddDice(selected, this))
            return;

        Debug.Log($"주사위 증정: {selected.diceName}", this);
    }
}
