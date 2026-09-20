using JJB.Script.Battle.Enemy.EnemyAbilities;
using UnityEngine;

namespace JJB.Script.Battle.Enemy
{
    [RequireComponent(typeof(EnemyHealthSetup))]
    public class EnemyDamageReceiver : MonoBehaviour
    {
        private EnemyHealthSetup _enemy;
        private EnemyHitFlash _hitFlash;

        private void Awake()
        {
            _enemy = GetComponent<EnemyHealthSetup>();
            _hitFlash = GetComponent<EnemyHitFlash>();
        }

        public void TakeDamage(int damage)
        {
            if (_enemy.Health.IsDead)
                return;
            
            RecordPlayerTree();

            if (_enemy.Ability != null)
                damage = _enemy.Ability.ModifyIncomingDamage(damage, _enemy.Health);

            _enemy.Health.TakeDamage(damage);
            
            _hitFlash?.Play();
        }
        
        private void RecordPlayerTree()
        {
            if (_enemy.Ability is not PaybackAbility paybackAbility)
                return;

            if (DiceManager_JCY.Instance == null || DiceManager_JCY.Instance.diceTree == null)
                return;

            string currentTree = DiceManager_JCY.Instance.diceTree.CurrentTree;

            paybackAbility.RecordTree(currentTree);
        }
    }
}