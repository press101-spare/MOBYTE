using System;
using System.Collections;
using JJB.Script.Battle.Enemy;
using JJB.Script.Battle.Player;
using UnityEngine;

namespace JJB.Script.Battle
{
    public class EnemyTurnController : MonoBehaviour
    {
        [SerializeField]
        private EnemyHealthSetup enemy;

        [SerializeField]
        private PlayerDamageReceiver playerDamageReceiver;

        [SerializeField]
        private float actionDelay = 0.7f;

        public void ExecuteTurn(Action onFinished)
        {
            StartCoroutine(ExecuteRoutine(onFinished));
        }

        private IEnumerator ExecuteRoutine(Action onFinished)
        {
            yield return new WaitForSeconds(actionDelay);
            playerDamageReceiver.TakeDamage(enemy.Data.AttackPower);
            yield return new WaitForSeconds(actionDelay);

            onFinished?.Invoke();
        }
    }
}