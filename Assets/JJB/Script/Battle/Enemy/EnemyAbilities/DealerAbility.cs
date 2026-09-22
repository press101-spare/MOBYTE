using UnityEngine;

namespace JJB.Script.Battle.Enemy.EnemyAbilities
{
    [CreateAssetMenu(fileName = "DealerAbility", menuName = "JJB/Enemy Ability/Dealer")]
    public class DealerAbility : EnemyAbility
    {
        [Header("경험")]
        [SerializeField] private float experienceChance = 0.2f;
        [SerializeField] private float experienceMultiplier = 1.3f;

        [Header("보험")]
        [SerializeField] private int insuranceThreshold = 40;
        [SerializeField] private float insuranceMultiplier = 0.5f;

        private int _damageThisTurn;

        public override int ModifyAttackDamage(int damage)
        {
            if (Random.value >= experienceChance)
                return damage;

            int boostedDamage = Mathf.CeilToInt(damage * experienceMultiplier);

            return boostedDamage;
        }

        public override int ModifyIncomingDamage(int damage, JJBHealth health)
        {
            if (damage <= 0)
                return damage;

            int remainingNormalDamage =
                Mathf.Max(0, insuranceThreshold - _damageThisTurn);

            int normalDamage =
                Mathf.Min(damage, remainingNormalDamage);

            int insuranceDamage =
                damage - normalDamage;

            int reducedInsuranceDamage =
                Mathf.CeilToInt(insuranceDamage * insuranceMultiplier);

            int finalDamage =
                normalDamage + reducedInsuranceDamage;

            _damageThisTurn += damage;

            if (insuranceDamage > 0)
            {
                Debug.Log(
                    $"보험 발동 : {damage} → {finalDamage} " +
                    $"/ 이번 턴 누적 {_damageThisTurn}"
                );
            }

            return finalDamage;
        }

        public override void AfterAttack()
        {
            _damageThisTurn = 0;
        }
    }
}