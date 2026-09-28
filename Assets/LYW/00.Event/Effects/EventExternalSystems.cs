using System.Collections.Generic;
using JJB.Script.Battle.Player.Progression;
using UnityEngine;

/// <summary>
/// LYW 이벤트 코드와 프로젝트의 기존 돈/주사위 시스템 사이를 연결합니다.
/// 외부 팀 코드를 직접 수정하지 않도록 모든 접근을 이 클래스에 모았습니다.
/// </summary>
internal static class EventExternalSystems
{
    // 이벤트 장면에 덱이 없을 때 받은 주사위를 실행 중 잠시 보관합니다.
    private static readonly List<DiceSO_JCY> pendingDice = new();

    public static bool TryGetProfile(
        Object context,
        out PlayerProfile profile)
    {
        // 싱글턴을 먼저 사용하고, 장면에만 존재하는 경우도 다시 검색합니다.
        PlayerProfileManager manager = PlayerProfileManager.Instance;

        if (manager == null)
            manager = Object.FindFirstObjectByType<PlayerProfileManager>();

        if (manager == null)
        {
            // 이벤트 장면을 단독 실행해도 돈 이벤트가 동작하도록 자동 생성합니다.
            GameObject managerObject =
                new GameObject("PlayerProfileManager (LYW Event)");

            manager = managerObject.AddComponent<PlayerProfileManager>();
        }

        profile = manager != null ? manager.Profile : null;

        if (profile != null)
            return true;

        Debug.LogError("플레이어 프로필을 준비하지 못했습니다.", context);
        return false;
    }

    public static DiceDeckManager_JCY GetDiceDeck()
    {
        DiceDeckManager_JCY deck = DiceDeckManager_JCY.Instance != null
            ? DiceDeckManager_JCY.Instance
            : Object.FindFirstObjectByType<DiceDeckManager_JCY>();

        // 덱이 생겼다면 임시 보관했던 주사위를 실제 덱으로 옮깁니다.
        if (deck == null || pendingDice.Count == 0)
            return deck;

        foreach (DiceSO_JCY dice in pendingDice)
        {
            if (dice != null)
                deck.AddDice(dice);
        }

        pendingDice.Clear();
        return deck;
    }

    public static DiceSO_JCY[] GetGlobalDicePool()
    {
        DiceManager_JCY manager = DiceManager_JCY.Instance;

        if (manager == null)
            manager = Object.FindFirstObjectByType<DiceManager_JCY>();

        return manager != null ? manager.allDiceSo : null;
    }

    public static DiceSO_JCY GetRandomDice(IReadOnlyList<DiceSO_JCY> pool)
    {
        if (pool == null || pool.Count == 0)
            return null;

        // null 항목을 제외한 실제 주사위만 같은 확률로 추첨합니다.
        int validCount = 0;

        for (int i = 0; i < pool.Count; i++)
        {
            if (pool[i] != null)
                validCount++;
        }

        if (validCount == 0)
            return null;

        int target = Random.Range(0, validCount);

        for (int i = 0; i < pool.Count; i++)
        {
            if (pool[i] == null)
                continue;

            if (target-- == 0)
                return pool[i];
        }

        return null;
    }

    public static bool TryAddDice(DiceSO_JCY dice, Object context)
    {
        if (dice == null)
        {
            Debug.LogError("지급할 주사위가 설정되지 않았습니다.", context);
            return false;
        }

        DiceDeckManager_JCY deck = GetDiceDeck();

        if (deck != null)
        {
            deck.AddDice(dice);
            return true;
        }

        // 이벤트 장면에 덱이 없으면 나중에 전달하기 위해 임시 보관합니다.
        pendingDice.Add(dice);

        Debug.LogWarning(
            "DiceDeckManager가 없어 획득 주사위를 임시 보관했습니다. " +
            "덱이 생성되면 자동으로 전달합니다.",
            context
        );

        return true;
    }

    public static bool TryRemovePendingDice(out DiceSO_JCY lostDice)
    {
        // 실제 덱이 없을 때 소지품 손실 이벤트가 임시 목록에서 하나를 제거합니다.
        lostDice = null;

        if (pendingDice.Count == 0)
            return false;

        int index = Random.Range(0, pendingDice.Count);
        lostDice = pendingDice[index];
        pendingDice.RemoveAt(index);
        return true;
    }
}
