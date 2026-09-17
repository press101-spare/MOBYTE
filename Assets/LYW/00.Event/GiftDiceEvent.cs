using UnityEngine;

public class GiftDiceEvent : MonoBehaviour
{
    public void GiftDice()
    {
        DiceSO_JCY a = DiceManager_JCY.Instance.allDiceSo
            [Random.Range(0, DiceManager_JCY.Instance.allDiceSo.Length)];
        DiceDeckManager_JCY.Instance.AddDice(a);
    }
}
