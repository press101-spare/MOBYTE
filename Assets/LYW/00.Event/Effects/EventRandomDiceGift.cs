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

        string message = $"주사위 획득!\n{selected.diceName}";
        Debug.Log(message, this);
        ShowResult(message);
    }
}
