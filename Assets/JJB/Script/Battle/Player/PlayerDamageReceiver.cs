using JJB.Script.Battle.Enemy;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

namespace JJB.Script.Battle.Player
{
    public class PlayerDamageReceiver : MonoBehaviour
    {
        [Header("Hit Effect")]
        [SerializeField] private Image playerImage;
        [SerializeField] private RectTransform playerVisual;
        [SerializeField] private float hitTilt = 15f;
        [SerializeField] private float hitDuration = 0.1f;

        private Color _originalColor;
        private Vector3 _originalRotation;
        
        private JJBHealth _health;
        private DiceBattleAdapter _diceBattleAdapter;
        private EnemyDamageReceiver _enemyDamageReceiver;

        private bool _reflectionEnabled;
        private float _reflectionRate;

        private void Awake()
        {
            _health = GetComponent<JJBHealth>();
            
            if (playerImage != null)
                _originalColor = playerImage.color;

            if (playerVisual != null)
                _originalRotation = playerVisual.localEulerAngles;
        }

        public void Initialize(
            DiceBattleAdapter diceBattleAdapter,
            EnemyDamageReceiver enemyDamageReceiver
        )
        {
            _diceBattleAdapter = diceBattleAdapter;
            _enemyDamageReceiver = enemyDamageReceiver;
        }

        public void TakeDamage(int damage)
        {
            TakeDamage(damage, 0f);
        }

        public void TakeDamage(
            int damage,
            float shieldPenetrationRate
        )
        {
            int rockStack =
                DiceManager_JCY.Instance
                    .diceEffect
                    .rockstack;

            if (_health.IsDead)
                return;

            shieldPenetrationRate =
                Mathf.Clamp01(shieldPenetrationRate);

            if (_diceBattleAdapter != null)
            {
                // 방어막을 무시하고 들어가는 피해
                int penetrationDamage =
                    Mathf.CeilToInt(
                        damage * shieldPenetrationRate
                    );

                // 방어막이 막을 수 있는 피해
                int shieldDamage =
                    damage - penetrationDamage;

                shieldDamage =
                    _diceBattleAdapter.DamageWithShield(
                        shieldDamage
                    );

                damage =
                    penetrationDamage + shieldDamage;
            }

            if (damage <= 0)
                return;

            if (rockStack > 0)
            {
                damage -=
                    damage * (10 * rockStack / 100);
            }

            _health.TakeDamage(damage);
            
            PlayHitEffect();
            
            AudioManager.Instance.PlayClipSFX("SFX_Hurt");

            if (_reflectionEnabled &&
                _enemyDamageReceiver != null)
            {
                int reflectDamage =
                    Mathf.CeilToInt(
                        damage * _reflectionRate
                    );

                _enemyDamageReceiver.TakeDamage(
                    reflectDamage
                );
            }
        }

        public void EnableReflection(float rate)
        {
            _reflectionEnabled = true;
            _reflectionRate = Mathf.Max(0f, rate);
        }

        public void DisableReflection()
        {
            _reflectionEnabled = false;
            _reflectionRate = 0f;
        }
        
        private void PlayHitEffect()
        {
            if (playerImage == null || playerVisual == null)
                return;

            playerImage.DOKill();
            playerVisual.DOKill();

            playerImage.color = _originalColor;
            playerVisual.localEulerAngles = _originalRotation;

            Sequence sequence = DOTween.Sequence();

            sequence.Join(
                playerImage.DOColor(Color.red, hitDuration)
            );

            sequence.Join(
                playerVisual.DOLocalRotate(
                    _originalRotation + new Vector3(0f, 0f, hitTilt),
                    hitDuration
                )
            );

            sequence.Append(
                playerImage.DOColor(
                    _originalColor,
                    hitDuration
                )
            );

            sequence.Join(
                playerVisual.DOLocalRotate(
                    _originalRotation,
                    hitDuration
                )
            );
        }
    }
}