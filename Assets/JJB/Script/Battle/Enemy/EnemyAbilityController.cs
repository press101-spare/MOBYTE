using System.Collections.Generic;
using UnityEngine;

namespace JJB.Script.Battle.Enemy
{
    public class EnemyAbilityController : MonoBehaviour
    {
        private readonly List<EnemyAbility> _abilities = new();

        private EnemyHealthSetup _enemy;

        private void Awake()
        {
            _enemy = GetComponent<EnemyHealthSetup>();
        }

        private void Start()
        {
            LoadAbilities();
        }

        private void LoadAbilities()
        {
            _abilities.Clear();

            if (_enemy == null)
            {
                Debug.LogError("EnemyHealthSetup이 없습니다.");
                return;
            }

            if (_enemy.Data == null)
            {
                Debug.LogError("EnemyData가 없습니다.");
                return;
            }

            if (_enemy.Data.Abilities == null)
                return;

            foreach (EnemyAbility ability in _enemy.Data.Abilities)
            {
                if (ability == null)
                    continue;

                _abilities.Add(Instantiate(ability));
            }

            Debug.Log($"적 능력 {_abilities.Count}개 로드");
        }

        public void OnTurnStart()
        {
            foreach (EnemyAbility ability in _abilities)
            {
                if (ability is IEnemyTurnStartAbility startAbility)
                    startAbility.OnTurnStart();
            }
        }

        public void OnTurnEnd()
        {
            foreach (EnemyAbility ability in _abilities)
            {
                if (ability is IEnemyTurnEndAbility endAbility)
                    endAbility.OnTurnEnd();
            }
        }

        public int ModifyAttackDamage(int damage)
        {
            int result = damage;

            foreach (EnemyAbility ability in _abilities)
            {
                if (ability is IEnemyAttackModifier modifier)
                    result = modifier.ModifyAttackDamage(result);
            }

            return result;
        }
    }
}