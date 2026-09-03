using System;
using System.Collections;
using JJB.Script.Battle.Enemy;
using UnityEngine;
using UnityEngine.Serialization;

namespace JJB.Script.Battle
{
    public class EnemyTurnController : MonoBehaviour
    {
        [SerializeField] private EnemyHealthSetup enemy;
        [FormerlySerializedAs("playerHealth")] [SerializeField] private JJBHealth playerJjbHealth;

        [SerializeField] private float actionDelay = 0.7f;

        public void ExecuteTurn(Action onFinished)
        {
            StartCoroutine(ExecuteRoutine(onFinished));
        }

        private IEnumerator ExecuteRoutine(Action onFinished)
        {
            yield return new WaitForSeconds(actionDelay);

            playerJjbHealth.TakeDamage(enemy.Data.AttackPower);

            yield return new WaitForSeconds(actionDelay);

            onFinished?.Invoke();
        }
    }
}