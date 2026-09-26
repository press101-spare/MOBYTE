using JJB.Script.Battle.Enemy.EnemyAbilities;
using UnityEngine;

namespace JJB.Script.Battle.Enemy
{
    [RequireComponent(typeof(EnemyHealthSetup))]
    public class EnemyDamageReceiver : MonoBehaviour
    {
        private EnemyHealthSetup _enemy;
        private EnemyHitFlash _hitFlash;
        
        private int _poison;

        public int Poison => _poison;

        private void Awake()
        {
            _enemy = GetComponent<EnemyHealthSetup>();
            _hitFlash = GetComponent<EnemyHitFlash>();
        }

        public void TakeDamage(int damage)
        {
            if (_enemy.Health.IsDead)
                return;
            
            int bloodStack = DiceManager_JCY.Instance.diceEffect.bloodStack;
            
            if (bloodStack > 0)
            {
                damage += damage * (10 * bloodStack / 100);
            }

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
        public void ApplyPoison(int amount)
        {
            if (amount <= 0)
                return;

            _poison += amount;
        }

        public void TickPoison()
        {
            if (_poison <= 0)
                return;

            if (_enemy == null || _enemy.Health == null)
                return;

            if (_enemy.Health.IsDead)
                return;

            int damage = _poison;

            _enemy.Health.TakeDamage(damage);

            _hitFlash?.Play(new Color(0.6f, 0f, 1f));

            _poison--;
        }
    }
}