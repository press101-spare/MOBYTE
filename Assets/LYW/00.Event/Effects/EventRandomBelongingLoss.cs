using JJB.Script.Battle.Player.Progression;
using UnityEngine;

public sealed class EventRandomBelongingLoss : EventEffect
{
    public override void Apply()
    {
        DiceDeckManager_JCY deck = EventExternalSystems.GetDiceDeck();

        if (deck != null &&
            deck.diceCollection != null &&
            deck.diceCollection.Count > 0)
        {
            int index = Random.Range(0, deck.diceCollection.Count);
            DiceSO_JCY lostDice = deck.diceCollection[index];
            int before = deck.diceCollection.Count;

            deck.RemoveDice(lostDice.diceEffectType);

            if (deck.diceCollection.Count == before)
                deck.diceCollection.RemoveAt(index);

            Debug.Log($"소지품을 잃었습니다: {lostDice.diceName}", this);
            return;
        }

        if (!EventExternalSystems.TryGetProfile(this, out PlayerProfile profile) ||
            profile.unlockedDice == null ||
            profile.unlockedDice.Count == 0)
        {
            Debug.Log("잃어버릴 소지품이 없습니다.", this);
            return;
        }

        int fallbackIndex = Random.Range(0, profile.unlockedDice.Count);
        string lostItem = profile.unlockedDice[fallbackIndex];
        profile.unlockedDice.RemoveAt(fallbackIndex);

        Debug.Log($"소지품을 잃었습니다: {lostItem}", this);
    }
}
