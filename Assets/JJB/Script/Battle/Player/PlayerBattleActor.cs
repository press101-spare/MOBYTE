using UnityEngine;

namespace JJB.Script.Battle.Player
{
    [RequireComponent(typeof(JJBHealth))]
    [RequireComponent(typeof(PlayerAttackController))]
    [RequireComponent(typeof(PlayerDamageReceiver))]
    public class PlayerBattleActor : MonoBehaviour
    {
        public JJBHealth Health { get; private set; }
        public PlayerAttackController AttackController { get; private set; }
        public PlayerDamageReceiver DamageReceiver { get; private set; }

        private void Awake()
        {
            Health = GetComponent<JJBHealth>();
            AttackController = GetComponent<PlayerAttackController>();
            DamageReceiver = GetComponent<PlayerDamageReceiver>();
        }
    }
}