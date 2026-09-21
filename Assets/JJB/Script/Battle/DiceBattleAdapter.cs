using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace JJB.Script.Battle
{
    public class DiceBattleAdapter : MonoBehaviour
    {
        private DiceManager_JCY DiceManager => DiceManager_JCY.Instance;
        private DiceDeckManager_JCY DiceDeckManager => DiceDeckManager_JCY.Instance;

        public bool IsRolling
        {
            get
            {
                if (DiceManager == null)
                    return false;

                return DiceManager.isRolling;
            }
        }

        public int CurrentScore
        {
            get
            {
                if (DiceManager == null || DiceManager.diceTree == null)
                    return 0;

                return DiceManager.diceTree.CurrentScore;
            }
        }

        public int ShieldValue
        {
            get
            {
                if (DiceManager == null || DiceManager.shledDice == null)
                    return 0;

                return DiceManager.shledDice.shledValue;
            }
        }

        public int ReRollCount
        {
            get
            {
                if (DiceManager == null || DiceManager.reRollUI == null)
                    return 0;

                return DiceManager.reRollUI.reRollCount;
            }
        }

        public void DrawDice()
        {
            if (DiceDeckManager == null)
            {
                Debug.LogError("DiceDeckManager_JCY.Instance가 없습니다.");
                return;
            }

            DiceDeckManager.DrawDice();
        }

        public void StartDefenseDice()
        {
            if (DiceManager == null || DiceManager.shledDice == null)
                return;

            DiceManager.shledDice.shideDraw();
        }

        public void ClearDice()
        {
            if (DiceManager == null)
                return;

            DiceManager.ClearDice();
        }

        public void SetShieldMode(bool value)
        {
            if (DiceManager == null)
                return;

            DiceManager.isShled = value;
        }

        public int DamageWithShield(int damage)
        {
            if (DiceManager == null || DiceManager.shledDice == null)
                return damage;

            if (DiceManager.shledDice.shledValue <= 0)
                return damage;

            int absorbed = Mathf.Min(DiceManager.shledDice.shledValue, damage);

            DiceManager.shledDice.shledValue -= absorbed;

            return damage - absorbed;
        }
    }
}