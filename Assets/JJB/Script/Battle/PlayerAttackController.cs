using UnityEngine;
using UnityEngine.Serialization;

namespace JJB.Script.Battle
{
    public class PlayerAttackController : MonoBehaviour
    {
        [FormerlySerializedAs("enemyHealth")] [SerializeField] private JJBHealth enemyJjbHealth;

        public void Attack(DiceTree_JCY.Trees treeType, int baseDamage)
        {
            if (enemyJjbHealth == null)
            {
                Debug.LogError("Enemy Health가 연결되지 않았습니다.");
                return;
            }

            if (enemyJjbHealth.IsDead)
                return;

            if (baseDamage <= 0)
                return;

            int finalDamage = baseDamage;

            enemyJjbHealth.TakeDamage(finalDamage);

            Debug.Log(
                $"{treeType} 공격 / 최종 피해 : {finalDamage}"
            );
        }
        
        public void Attack(int baseDamage)
        {
            if (enemyJjbHealth == null)
            {
                Debug.LogError("Enemy Health가 연결되지 않았습니다.");
                return;
            }

            if (enemyJjbHealth.IsDead)
                return;

            if (baseDamage <= 0)
                return;

            int finalDamage = baseDamage;

            // 나중에 여기서
            // 출혈
            // 흡혈
            // 최종 데미지 증가
            // 주사위 특수효과 계산

            enemyJjbHealth.TakeDamage(finalDamage);

            Debug.Log($"플레이어 공격 / 최종 피해 : {finalDamage}");
        }
    }
}