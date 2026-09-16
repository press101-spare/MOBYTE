using JJB.Script.Battle.Player.Progression;
using UnityEngine;

namespace JJB.Script.Battle.Enemy.EnemyAbilities
{
    [CreateAssetMenu(fileName = "StealMoneyAbility", menuName = "Game/Enemy Ability/Steal Money")]
    public class StealMoneyAbility : EnemyAbility
    {
        private const float StealChance = 0.4f;
        private const float StealRate = 0.2f;

        public override void AfterAttack()
        {
            if (Random.value >= StealChance)
            {
                Debug.Log("강탈 실패");
                return;
            }

            if (PlayerProfileManager.Instance == null)
            {
                Debug.LogError("PlayerProfileManager가 없습니다.");
                return;
            }

            PlayerProfile profile = PlayerProfileManager.Instance.Profile;

            int stolenMoney = Mathf.FloorToInt(profile.money * StealRate);

            profile.money -= stolenMoney;
        }
    }
}