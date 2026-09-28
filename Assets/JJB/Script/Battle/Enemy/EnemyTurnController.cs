using System;
using System.Collections;
using JJB.Script.Battle.Player;
using UnityEngine;
using Random = UnityEngine.Random;

namespace JJB.Script.Battle.Enemy
{
    [RequireComponent(typeof(EnemyHealthSetup))]
    public class EnemyTurnController : MonoBehaviour
    {
        private EnemyHealthSetup _enemy;
        private PlayerDamageReceiver _playerDamageReceiver;

        private const float ActionDelay = 0.7f;

        private void Awake()
        {
            _enemy = GetComponent<EnemyHealthSetup>();
        }

        public void Initialize(PlayerDamageReceiver playerDamageReceiver)
        {
            _playerDamageReceiver = playerDamageReceiver;
        }

        public void ExecuteTurn(Action onFinished)
        {
            StartCoroutine(ExecuteRoutine(onFinished));
        }

        private IEnumerator ExecuteRoutine(Action onFinished)
        {
            yield return new WaitForSeconds(ActionDelay);
            
            _enemy.PlayAttack();

            int damage = SelectAttackDamage();

            if (_enemy.Ability != null)
            {
                damage = _enemy.Ability.ModifyAttackDamage(damage);
            }

            int bloodStack = DiceManager_JCY.Instance.diceEffect.bloodStack;

            if (bloodStack > 0)
            {
                damage -= damage * (10 * bloodStack / 100);
            }

            FinalBossAbility finalBossAbility = _enemy.Ability as FinalBossAbility;

            // 최종보스 2페이즈 3번째 행동마다
            // 방어막 50% 관통
            if (finalBossAbility != null && finalBossAbility.IsShieldPiercingTurn())
            {
                _playerDamageReceiver.TakeDamage(damage, finalBossAbility.ShieldPenetrationRate);
            }
            else
            {
                _playerDamageReceiver.TakeDamage(damage);
            }

            _enemy.Ability?.AfterAttack();

            yield return new WaitForSeconds(ActionDelay);

            onFinished?.Invoke();
        }

        private int SelectAttackDamage()
        {
            int randomValue =
                Random.Range(0, 100);

            if (randomValue < 20)
                return _enemy.Data.AttackPower;

            if (randomValue < 70)
                return _enemy.Data.AttackPower2;

            return _enemy.Data.AttackPower3;
        }
    }
}