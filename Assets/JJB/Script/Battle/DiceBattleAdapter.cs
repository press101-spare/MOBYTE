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
                if (DiceManager_JCY.Instance == null || DiceManager_JCY.Instance.reRollUI == null)
                    return 0;

                return DiceManager_JCY.Instance.reRollUI.reRollCount;
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
            {
                Debug.LogError("ShledDice_JCY를 찾을 수 없습니다.");
                return;
            }

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
            if (DiceManager_JCY.Instance == null)
                return damage;

            ShledDice_JCY shledDice = DiceManager_JCY.Instance.shledDice;

            if (shledDice == null || shledDice.shledValue <= 0)
                return damage;

            int absorbedDamage = Mathf.Min(shledDice.shledValue, damage);

            shledDice.shledValue -= absorbedDamage;
            damage -= absorbedDamage;

            return damage;
        }
    }
}