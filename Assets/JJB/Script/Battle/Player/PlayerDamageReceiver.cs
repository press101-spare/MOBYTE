using UnityEngine;

namespace JJB.Script.Battle.Player
{
    public class PlayerDamageReceiver : MonoBehaviour
    {
        [SerializeField] private JJBHealth playerHealth;
        [SerializeField] private ShledDice_JCY shieldDice;

        public void TakeDamage(int damage)
        {
            if (damage <= 0) return;
            
            int shield = shieldDice.shledValue;
            int absorbedDamage = Mathf.Min(shield, damage);
            
            shieldDice.shledValue -= absorbedDamage;
            
            int remainingDamage = damage - absorbedDamage;
            
            if (remainingDamage > 0)
            {
                playerHealth.TakeDamage(remainingDamage);
            }
        }
    }
}