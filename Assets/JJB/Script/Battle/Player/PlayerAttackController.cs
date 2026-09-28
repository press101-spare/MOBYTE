using JJB.Script.Battle.Enemy;
using JJB.Script.Battle.Player.Progression;
using UnityEngine;

namespace JJB.Script.Battle.Player
{
    public class PlayerAttackController : MonoBehaviour
    {
        private EnemyDamageReceiver _enemyDamageReceiver;
        private EnemyHealthSetup _enemy;

        private bool _attackEvaded;

        public void Initialize(EnemyDamageReceiver enemyDamageReceiver)
        {
            _enemyDamageReceiver = enemyDamageReceiver;

            if (_enemyDamageReceiver != null)
                _enemy = _enemyDamageReceiver.GetComponent<EnemyHealthSetup>();
        }

        public bool Attack(int damage)
        {
            if (_enemyDamageReceiver == null)
            {
                Debug.LogError("EnemyDamageReceiver가 연결되지 않았습니다.");
                return false;
            }

            if (damage <= 0)
                return false;

            FinalBossAbility finalBossAbility =
                _enemy?.Ability as FinalBossAbility;

            string currentHand =
                DiceManager_JCY.Instance
                    .diceTree
                    .CurrentTree
                    .ToString();

            // 같은 족보 연속 사용 불가
            if (finalBossAbility != null && !finalBossAbility.CanUseHand(currentHand))
            {
                Debug.Log("직전에 사용한 족보는 다시 사용할 수 없습니다.");
                return false;
            }

            // 여기까지 왔으면 공격 확정
            finalBossAbility?.RegisterHand(currentHand);

            // 공격 전체에 대해 회피 판정은 한 번만
            _attackEvaded = false;

            if (finalBossAbility != null &&
                finalBossAbility.TryEvade())
            {
                _attackEvaded = true;

                Debug.Log("최종 보스가 공격을 회피했습니다.");
            }

            int attackDamage = CalculateDamage(damage);

            DiceManager_JCY.Instance.PlayAttackAnimation(
                attackDamage,
                ApplyDiceHit
            );

            return true;
        }

        private int CalculateDamage(int damage)
        {
            if (PlayerProfileManager.Instance == null)
                return damage;

            int attackPower =
                PlayerProfileManager.Instance
                    .Profile
                    .stats
                    .attackPower;

            return DiceManager_JCY.Instance
                .diceTree
                .TreeEffect(damage + attackPower);
        }

        private void ApplyDiceHit(int damage)
        {
            // 이번 공격이 회피된 경우 모든 타격 무효
            if (_attackEvaded)
                return;

            _enemyDamageReceiver.TakeDamage(damage);
        }
    }
}