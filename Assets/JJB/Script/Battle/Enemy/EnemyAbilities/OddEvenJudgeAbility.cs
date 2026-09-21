using UnityEngine;

namespace JJB.Script.Battle.Enemy.EnemyAbilities
{
    [CreateAssetMenu(fileName = "OddEvenJudgeAbility", menuName = "JJB/Enemy Ability/Odd Even Judge")]
    public class OddEvenJudgeAbility : EnemyAbility
    {
        [SerializeField] private int oddDamage = 10;
        [SerializeField] private int evenDamage = 30;

        private bool _isOdd;

        public override int ModifyAttackDamage(int damage)
        {
            int randomNumber = Random.Range(1, 101);

            _isOdd = randomNumber % 2 != 0;
            
            Debug.Log($"{randomNumber} → {(_isOdd ? "홀수" : "짝수")}");

            if (_isOdd)
            {
                return oddDamage;
            }
            return evenDamage;
        }

        public override void AfterAttack()
        {
            if (!_isOdd)
                return;

            ClearPlayerEffects();
        }

        private void ClearPlayerEffects()
        {
            if (DiceManager_JCY.Instance == null)
                return;

            if (DiceManager_JCY.Instance.shledDice != null)
                DiceManager_JCY.Instance.shledDice.shledValue = 0;
        }
    }
}