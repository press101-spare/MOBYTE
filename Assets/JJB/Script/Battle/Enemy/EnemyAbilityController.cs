using System.Collections.Generic;
using UnityEngine;

namespace JJB.Script.Battle.Enemy
{
    public class EnemyAbilityController : MonoBehaviour
    {
        private readonly List<EnemyAbility> _abilities = new();

        public void Initialize(EnemyData data)
        {
            _abilities.Clear();

            foreach (EnemyAbility ability in data.Abilities)
            {
                if (ability == null)
                    continue;

                _abilities.Add(Instantiate(ability));
            }
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

        public int ModifyPlayerDamage(int damage)
        {
            int result = damage;

            foreach (EnemyAbility ability in _abilities)
            {
                if (ability is IPlayerAttackModifier modifier)
                    result = modifier.ModifyPlayerDamage(result);
            }

            return result;
        }

        public int ModifyRerollCost(int cost)
        {
            int result = cost;

            foreach (EnemyAbility ability in _abilities)
            {
                if (ability is IRerollModifier modifier)
                    result = modifier.ModifyRerollCost(result);
            }

            return result;
        }
    }
}