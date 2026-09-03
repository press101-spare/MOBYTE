using UnityEngine;

public class DiceEffect : MonoBehaviour
{
    public void Effect(DiceSO_JCY diseSO)
    {
        DiceSO_JCY.DiceEffectType _effect = diseSO.diceEffectType;
        switch (_effect)
        {
            case DiceSO_JCY.DiceEffectType.Even:
            case DiceSO_JCY.DiceEffectType.None:
            case DiceSO_JCY.DiceEffectType.Odd:
            case DiceSO_JCY.DiceEffectType.Joker:
                break;
            
            case DiceSO_JCY.DiceEffectType.Blood:
                break;
            
            case DiceSO_JCY.DiceEffectType.Gamble:
                break;
            
            case DiceSO_JCY.DiceEffectType.Shield:
                break;
            
            case DiceSO_JCY.DiceEffectType.Debt:
                DiceManager_JCY.Instance.reRollUI.Debt = true;
                break;
            
            case DiceSO_JCY.DiceEffectType.Allin:
                break;
            
            case DiceSO_JCY.DiceEffectType.Vampire:
                break;
            
            case DiceSO_JCY.DiceEffectType.Reroll:
                DiceManager_JCY.Instance.reRollUI.UpdateReRollCount(1);
                break;
        }
    }
}
