using UnityEngine;

namespace JJB.Script.Battle.Player
{
    public class PlayerAttackController : MonoBehaviour
    {
        [SerializeField] private JJBHealth enemyJjbHealth;

        public bool Attack(int baseDamage)
        {
            if (enemyJjbHealth == null)
            {
                Debug.LogError("Enemy Health가 연결되지 않았습니다.");
                return false;
            }

            if (enemyJjbHealth.IsDead) return false;

            if (baseDamage <= 0) return false;

            DiceManager_JCY diceManager = DiceManager_JCY.Instance;

            if (diceManager == null)
            {
                Debug.LogError("DiceManager_JCY가 없습니다.");
                return false;
            }
            
            int finalDamage = diceManager.diceEffect.CalculateFinalDamage(diceManager.ActiveDiceScripts, baseDamage);

            // 실제 적 피해
            enemyJjbHealth.TakeDamage(finalDamage);
            
            return true;
        }
    }
}