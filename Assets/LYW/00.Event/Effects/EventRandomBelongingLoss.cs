using UnityEngine;

/// <summary>
/// 현재 보유한 주사위 또는 임시 보관 중인 주사위 하나를 무작위로 잃습니다.
/// 외부 덱의 최소 개수 제한 때문에 제거가 거부되면 선택한 항목을 직접 제거합니다.
/// </summary>
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

            // 기존 덱 API를 우선 사용하고 실제로 줄지 않았으면 직접 제거합니다.
            int before = deck.diceCollection.Count;

            deck.RemoveDice(lostDice.diceEffectType);

            if (deck.diceCollection.Count == before)
                deck.diceCollection.RemoveAt(index);

            string message = $"소지품을 잃었습니다.\n{lostDice.diceName}";
            Debug.Log(message, this);
            ShowResult(message);
            return;
        }

        // 실제 덱이 없는 이벤트 단독 장면에서는 임시 보관 목록을 확인합니다.
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
