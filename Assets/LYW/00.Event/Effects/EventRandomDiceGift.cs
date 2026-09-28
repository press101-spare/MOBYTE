using UnityEngine;

/// <summary>
/// Inspector의 Dice Pool에서 유효한 주사위 하나를 무작위로 지급합니다.
/// Pool이 비어 있으면 프로젝트의 전체 주사위 목록을 사용합니다.
/// </summary>
public sealed class EventRandomDiceGift : EventEffect
{
    [SerializeField] private DiceSO_JCY[] dicePool;

    public override void Apply()
    {
        // 이벤트 전용 목록을 우선하고 없을 때만 전역 목록을 사용합니다.
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
