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

            if (lostDice == null)
            {
                deck.diceCollection.RemoveAt(index);
                const string emptySlot = "비어 있는 소지품 칸을 정리했습니다.";
                Debug.Log(emptySlot, this);
                ShowResult(emptySlot);
                return;
            }

            int before = deck.diceCollection.Count;

            deck.RemoveDice(lostDice.diceEffectType);

            if (deck.diceCollection.Count == before)
                deck.diceCollection.RemoveAt(index);

            string message = $"소지품을 잃었습니다.\n{lostDice.diceName}";
            Debug.Log(message, this);
            ShowResult(message);
            return;
        }

        if (!EventExternalSystems.TryRemovePendingDice(out DiceSO_JCY pendingLost))
        {
            const string noItem = "잃어버릴 소지품이 없습니다.";
            Debug.Log(noItem, this);
            ShowResult(noItem);
            return;
        }

        string itemName = pendingLost != null
            ? pendingLost.diceName
            : "알 수 없는 소지품";

        string pendingMessage = $"소지품을 잃었습니다.\n{itemName}";
        Debug.Log(pendingMessage, this);
        ShowResult(pendingMessage);
    }
}
