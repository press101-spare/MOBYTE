using UnityEngine;

namespace JJB.Script.Battle
{
    public class PlayerAttackController : MonoBehaviour
    {
        [SerializeField] private Health enemyHealth;

        public void Attack(DiceTree_JCY.Trees treeType, int baseDamage)
        {
            if (enemyHealth == null)
            {
                Debug.LogError("Enemy Health가 연결되지 않았습니다.");
                return;
            }

            if (enemyHealth.IsDead)
                return;

            if (baseDamage <= 0)
                return;
            
            // 나중에 주사위 특수효과 계산할 위치

            int finalDamage = baseDamage;

            // 출혈 수치 계산
            // 흡혈 수치 계산
            
            // 실제 적에게 피해

            enemyHealth.TakeDamage(finalDamage);

            Debug.Log(
                $"{treeType} 공격 / 최종 피해 : {finalDamage}"
            );
            
            // 나중에 공격 후 효과

            // 출혈 부여
            // 흡혈 회복
            // 실드 획득
            // 재굴림 획득
        }
    }
}