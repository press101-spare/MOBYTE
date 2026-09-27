using System.Collections.Generic;
using JJB.Script.Battle.Player.Progression;
using UnityEngine;

internal static class EventExternalSystems
{
    private static readonly List<DiceSO_JCY> pendingDice = new();

    public static bool TryGetProfile(
        Object context,
        out PlayerProfile profile)
    {
        PlayerProfileManager manager = PlayerProfileManager.Instance;

        if (manager == null)
            manager = Object.FindFirstObjectByType<PlayerProfileManager>();

        if (manager == null)
        {
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
        lostDice = null;

        if (pendingDice.Count == 0)
            return false;

        int index = Random.Range(0, pendingDice.Count);
        lostDice = pendingDice[index];
        pendingDice.RemoveAt(index);
        return true;
    }
}
