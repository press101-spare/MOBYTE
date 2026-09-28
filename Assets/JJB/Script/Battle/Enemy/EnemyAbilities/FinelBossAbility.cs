using UnityEngine;

namespace JJB.Script.Battle.Enemy
{
    [CreateAssetMenu(fileName = "FinalBossAbility", menuName = "JJB/Enemy Ability/Final Boss")]
    public class FinalBossAbility : EnemyAbility
    {
        [Header("Phase 1")]
        [SerializeField] private float evadeChance = 0.2f;

        [Header("Phase 2")]
        [SerializeField] private int strongAttackDamage = 27;
        [SerializeField] private float shieldPenetrationRate = 0.5f;

        private bool _isPhase2;
        private bool _strongAttackQueued;

        private int _attackBonus;
        private int _phase2ActionCount;

        private string _lastHandName;

        public bool IsPhase2 => _isPhase2;
        public float ShieldPenetrationRate => shieldPenetrationRate;

        public bool TryEvade()
        {
            if (_isPhase2)
                return false;

            return Random.value < evadeChance;
        }

        public bool CanUseHand(string handName)
        {
            if (string.IsNullOrEmpty(_lastHandName))
                return true;

            return _lastHandName != handName;
        }

        public void RegisterHand(string handName)
        {
            _lastHandName = handName;
        }

        public override int ModifyIncomingDamage(int damage, JJBHealth health)
        {
            int afterHealth = health.CurrentHealth - damage;

            if (!_isPhase2 &&
                afterHealth <= health.MaxHealth * 0.5f)
            {
                EnterPhase2();
            }

            return damage;
        }

        public override int ModifyAttackDamage(int damage)
        {
            // 2페이즈 진입 직후 공격은 27
            if (_strongAttackQueued)
                return strongAttackDamage;

            // 1페이즈는 공격할 때마다 +1 누적
            if (!_isPhase2)
                return damage + _attackBonus;

            return damage;
        }

        public override void AfterAttack()
        {
            if (!_isPhase2)
            {
                _attackBonus++;
                return;
            }

            if (_strongAttackQueued)
                _strongAttackQueued = false;

            _phase2ActionCount++;
        }

        public bool IsShieldPiercingTurn()
        {
            if (!_isPhase2)
                return false;

            return (_phase2ActionCount + 1) % 3 == 0;
        }

        private void EnterPhase2()
        {
            _isPhase2 = true;
            _strongAttackQueued = true;
            _phase2ActionCount = 0;
        }
    }
}