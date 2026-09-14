using System;
using System.Collections;
using JJB.Script.Battle.Player;
using UnityEngine;

namespace JJB.Script.Battle.Enemy
{
    public class EnemyTurnController : MonoBehaviour
    {
        [SerializeField] private EnemyHealthSetup enemy;
        [SerializeField] private PlayerDamageReceiver playerDamageReceiver;
        [SerializeField] private float actionDelay = 0.7f;

        private EnemyAbilityController _abilityController;

        private void Awake()
        {
            _abilityController = GetComponent<EnemyAbilityController>();
        }
        
        public void Initialize(PlayerDamageReceiver _playerDamageReceiver)
        {
            playerDamageReceiver = _playerDamageReceiver;
        }


        public void ExecuteTurn(Action onFinished)
        {
            StartCoroutine(ExecuteRoutine(onFinished));
        }

        private IEnumerator ExecuteRoutine(Action onFinished)
        {
            if (enemy == null)
            {
                Debug.LogError("EnemyHealthSetup이 없습니다.");
                yield break;
            }

            if (enemy.Data == null)
            {
                Debug.LogError("EnemyData가 없습니다.");
                yield break;
            }

            if (playerDamageReceiver == null)
            {
                Debug.LogError("PlayerDamageReceiver가 초기화되지 않았습니다.");
                yield break;
            }

            if (_abilityController == null)
            {
                Debug.LogError("EnemyAbilityController가 없습니다.");
                yield break;
            }
            
            _abilityController.OnTurnStart();
            yield return new WaitForSeconds(actionDelay);
            
            int damage = enemy.Data.AttackPower;
            damage = _abilityController.ModifyAttackDamage(damage);
            playerDamageReceiver.TakeDamage(damage);
            yield return new WaitForSeconds(actionDelay);

            _abilityController.OnTurnEnd();
            onFinished?.Invoke();
        }
    }
}