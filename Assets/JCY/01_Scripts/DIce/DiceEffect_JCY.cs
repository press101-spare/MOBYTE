using System;
using System.Collections.Generic;
using JJB.Script.Battle;
using JJB.Script.Battle.Enemy;
using JJB.Script.Battle.Player;
using JJB.Script.Battle.Player.Progression;
using Unity.Mathematics;
using UnityEngine;
using Random = UnityEngine.Random;

public class DiceEffect_JCY : MonoBehaviour
{
    public DiceEffect_JCY Instance { get; set; }
    [SerializeField] private PlayerDamageReceiver playerDamageReceiver;
    [SerializeField] private EnemyDamageReceiver enemyDamageReceiver;
    
    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }



    //드로우 할때 발동
    public void Effect(DiceSO_JCY diseSO)
    {
        DiceSO_JCY.DiceEffectType _effect = diseSO.diceEffectType;
        switch (_effect)
        {
            case DiceSO_JCY.DiceEffectType.Reroll:
                DiceManager_JCY.Instance.reRollUI.UpdateReRollCount(1);
                break;
            default:
                return;
        }
        
        
    }
    
    // DiceEffect_JCY 또는 DiceManager_JCY 내의 계산 메서드 예시
    //공격시 발동
    public int CalculateFinalDamage(IReadOnlyList<DiceObject_JCY> activeDice, int baseDamage)
    {
        float finalDamage = baseDamage;

        foreach (DiceObject_JCY dice in activeDice)
        {
            // 1. 주사위 종류(EffectType) 가져오기
            DiceSO_JCY.DiceEffectType effectType = dice.currentDiceSO.diceEffectType;
        
            // 2. 주사위 결과 눈금 가져오기
            int rolledValue = dice.currentIndex;

            // 3. Allin 주사위 판별 예시
            if (effectType == DiceSO_JCY.DiceEffectType.Allin)
            {
                if (rolledValue == 6)
                {
                    Debug.Log("올인 터짐");
                    finalDamage *= 1.5f ; // 6이 나왔을 때 대폭 증가
                }
                continue;
            }
            
            //처형
            if (effectType == DiceSO_JCY.DiceEffectType.ExecutionDice)
            {
                DiceManager_JCY.Instance._xecution++;
                continue;
            }
            
            //흡혈
            if (effectType == DiceSO_JCY.DiceEffectType.Vampire)
            {
                DiceManager_JCY.Instance._vamfire++;
                continue;
            }
                
            //핵폭탄
            if (effectType == DiceSO_JCY.DiceEffectType.Hack)
            {
                playerDamageReceiver.TakeDamage((JJBGameManager.Instance.PlayerJjbHealth.CurrentHealth * 20) / 100);
                finalDamage *= 1.5f;
                continue;
            }
            
            //방어막
            if (effectType == DiceSO_JCY.DiceEffectType.Shield)
            {
                DiceManager_JCY.Instance.shledDice.ShledAdd(1);
                continue;
            }
            
            //회복
            if (effectType == DiceSO_JCY.DiceEffectType.Health)
            {
                JJBGameManager.Instance.PlayerJjbHealth.Heal(10);
                continue;
            }
            
            //유리
            if (effectType == DiceSO_JCY.DiceEffectType.Glass)
            {
                Debug.Log("유리 와자창");
                DiceDeckManager_JCY.Instance.RemoveDice(DiceSO_JCY.DiceEffectType.Glass);
                DiceManager_JCY.Instance.glassStack++;
                continue;
            }
            
            //포션
            if (effectType == DiceSO_JCY.DiceEffectType.Potion)
            {
                DiceDeckManager_JCY.Instance.RemoveDice(DiceSO_JCY.DiceEffectType.Potion);
                JJBGameManager.Instance.PlayerJjbHealth.Heal(30);
                continue;
            }
        
            //성장
            if (effectType == DiceSO_JCY.DiceEffectType.Grow)
            {
                Debug.Log($"성장 {(float)JJBGameManager.Instance.BattleTurnManager.CurrentPhase / 2f}만큼");
                finalDamage += (float)JJBGameManager.Instance.BattleTurnManager.CurrentPhase / 2f;
                continue;
            }
            
            if (effectType == DiceSO_JCY.DiceEffectType.Phoenix)
            {
                JJBGameManager.Instance.PlayerJjbHealth.EnableOneHpRevive();
                continue;
            }
            
            //칩
            if (effectType == DiceSO_JCY.DiceEffectType.Chip)
            {
                PlayerProfileManager.Instance.AddMoney(Random.Range(100, 500));
                continue;
            }

            if (effectType == DiceSO_JCY.DiceEffectType.Mirror)
            {
                playerDamageReceiver.EnableReflection(0.34f);
            }

            if (effectType == DiceSO_JCY.DiceEffectType.Poison)
            {
                enemyDamageReceiver.ApplyPoison(5);
            }
            
            //거대
            if (effectType == DiceSO_JCY.DiceEffectType.Giant)
            {
                if (effectType == DiceSO_JCY.DiceEffectType.Giant)
                {
                    int value = Mathf.RoundToInt(JJBGameManager.Instance.PlayerJjbHealth.CurrentHealth * 0.1f);
                    finalDamage += value;
                }
            }
        }

        int returnDamage = (int)Math.Round(finalDamage);
        return returnDamage;
    }
}
