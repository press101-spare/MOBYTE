using UnityEngine;

namespace JJB.Script.Battle
{
    public class DiceBattleAdapter : MonoBehaviour
    {
        private DiceManager_JCY DiceManager => DiceManager_JCY.Instance;
        private DiceDeckManager_JCY DiceDeckManager => DiceDeckManager_JCY.Instance;

        public bool IsRolling => DiceManager != null && DiceManager.isRolling;

        public int CurrentScore
        {
            get
            {
                if (DiceManager == null || DiceManager.diceTree == null)
                    return 0;

                return DiceManager.diceTree.CurrentScore;
            }
        }

        public string CurrentTree
        {
            get
            {
                if (DiceManager == null || DiceManager.diceTree == null)
                    return "";

                return DiceManager.diceTree.CurrentTree;
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
    }
}