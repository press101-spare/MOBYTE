using TMPro;
using UnityEngine;
using UnityEngine.U2D.Animation;
using UnityEngine.UI;

namespace JJB.Script.Battle.Enemy
{
    [RequireComponent(typeof(JJBHealth))]
    public class EnemyHealthSetup : MonoBehaviour
    {
        [SerializeField] private EnemyData data;
        [SerializeField] private TMP_Text enemyNameText;

        [Header("Visual")]
        [SerializeField] private Image enemyImage;
        [SerializeField] private SpriteLibrary spriteLibrary;

        private JJBHealth _health;
        private SpriteLibrary _spriteLibrary;

        public EnemyData Data => data;
        public JJBHealth Health => _health;
        public EnemyAbility Ability { get; private set; }

        private void Awake()
        {
            _health = GetComponent<JJBHealth>();

            if (enemyImage != null)
                _spriteLibrary = enemyImage.GetComponent<SpriteLibrary>();
        }

        public void SetData(EnemyData enemyData)
        {
            data = enemyData;

            if (data == null)
                return;

            if (enemyNameText != null)
                enemyNameText.text = data.EnemyName;

            UpdateEnemySprite();
        }

        private void UpdateEnemySprite()
        {
            if (data == null || spriteLibrary == null)
                return;

            spriteLibrary.spriteLibraryAsset =
                data.SpriteLibraryAsset;

            Sprite idleSprite =
                spriteLibrary.GetSprite("Enemy", "Idle");

            if (idleSprite != null && enemyImage != null)
                enemyImage.sprite = idleSprite;
        }

        public void Initialize()
        {
            if (data == null)
            {
                Debug.LogError("EnemyData가 없습니다.", this);
                return;
            }

            _health.Initialize(data.MaxHealth);

            if (data.Ability != null)
                Ability = Instantiate(data.Ability);
        }

        private void OnValidate()
        {
            if (data == null)
                return;

            if (enemyNameText != null)
                enemyNameText.text = data.EnemyName;

            UpdateEnemySprite();
        }
    }
}