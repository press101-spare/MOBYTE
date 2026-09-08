using System;
using System.Collections.Generic;
using JJB.Script.Battle;
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

            
            case DiceSO_JCY.DiceEffectType.Vampire:
                break;
            
            case DiceSO_JCY.DiceEffectType.Reroll:
                DiceManager_JCY.Instance.reRollUI.UpdateReRollCount(1);
                break;
        }
        
        
    }
    // DiceEffect_JCY 또는 DiceManager_JCY 내의 계산 메서드 예시
    public int CalculateFinalDamage(IReadOnlyList<DiceObject_JCY> activeDice, int baseDamage)
    {
        float finalDamage = baseDamage;
        int allinStack = 0;
        int growStack = 0;

        foreach (DiceObject_JCY dice in activeDice)
        {
            // 1. 주사위 종류(EffectType) 가져오기
            DiceSO_JCY.DiceEffectType effectType = dice.currentDiceSO.diceEffectType;
        
            // 2. 주사위 결과 눈금 가져오기
            int rolledValue = dice.currentIndex;

            // 3. Allin 주사위 판별 예시
            if (effectType == DiceSO_JCY.DiceEffectType.Allin && allinStack < 2)
            {
                if (rolledValue == 6)
                {
                    finalDamage *= 1.5f ; // 6이 나왔을 때 대폭 증가
                    allinStack++;
                }
            }
            

            if (effectType == DiceSO_JCY.DiceEffectType.Grow && growStack < 2)
            {
                Debug.Log($"성장 {(float)JJBGameManager.Instance.BattleTurnManager.CurrentPhase / 2f}만큼");
                finalDamage += (float)JJBGameManager.Instance.BattleTurnManager.CurrentPhase / 2f;
                growStack++;
                Debug.Log(growStack);
            }
            
        }

        int returnDamage = (int)Math.Round(finalDamage);
        return returnDamage;
    }
}
