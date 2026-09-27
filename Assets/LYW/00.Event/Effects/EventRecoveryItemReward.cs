using UnityEngine;

/// <summary>
/// 회복용 주사위 또는 포션을 지급하는 이벤트 효과입니다.
/// 직접 연결된 아이템이 없으면 전체 목록에서 회복 타입을 찾습니다.
/// </summary>
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

        string message = $"회복 아이템 획득!\n{item.diceName}";
        Debug.Log(message, this);
        ShowResult(message);
    }

    private static DiceSO_JCY FindRecoveryItem(DiceSO_JCY[] pool)
    {
        // Potion 또는 Health 타입 중 가장 먼저 발견한 아이템을 반환합니다.
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
