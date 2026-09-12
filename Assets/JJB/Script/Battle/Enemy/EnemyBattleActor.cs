using UnityEngine;

namespace JJB.Script.Battle.Enemy
{
    [RequireComponent(typeof(JJBHealth))]
    [RequireComponent(typeof(EnemyTurnController))]
    public class EnemyBattleActor : MonoBehaviour
    {
        public JJBHealth Health { get; private set; }
        public EnemyTurnController TurnController { get; private set; }

        private void Awake()
        {
            Health = GetComponent<JJBHealth>();
            TurnController = GetComponent<EnemyTurnController>();
        }
    }
}