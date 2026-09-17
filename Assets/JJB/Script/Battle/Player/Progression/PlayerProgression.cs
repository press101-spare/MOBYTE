using JJB.Script.Battle.Player.Progression;
using UnityEngine;

namespace JJB.Script.Battle.Player
{
    public class PlayerProgression : MonoBehaviour
    {
        private PlayerProfile Profile => PlayerProfileManager.Instance.Profile;

        public int Level => Profile.level;
        public int CurrentExp => Profile.currentExp;

        public void AddExp(int amount)
        {
            if (amount <= 0)
                return;

            Profile.currentExp += amount;

            CheckLevelUp();
        }

        private void CheckLevelUp()
        {
            int requiredExp = GetRequiredExp();

            while (Profile.currentExp >= requiredExp)
            {
                Profile.currentExp -= requiredExp;
                Profile.level++;

                requiredExp = GetRequiredExp();
            }
        }

        public int GetRequiredExp()
        {
            return Profile.level * 100;
        }
    }
}