using System;
using UnityEngine;

public class DiceEffect_JCY : MonoBehaviour
{
    public DiceEffect_JCY Instance { get; set; }
    
    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }



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
                DiceManager_JCY.Instance.shledDice.ShledAdd(1);
                break;
            
            case DiceSO_JCY.DiceEffectType.ShieldTurn:
                Debug.Log("쉴드 턴!");
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
