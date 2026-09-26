using TMPro;
using UnityEngine;
using UnityEngine.U2D.Animation;
using UnityEngine.UI;

namespace JJB.Script.Battle.Enemy
{
    [RequireComponent(typeof(JJBHealth))]
    public class EnemyHealthSetup : MonoBehaviour
    {
        [SerializeField] private SpriteLibrary spriteLibrary;
        [SerializeField] private Image enemyImage;
        
        [SerializeField] private EnemyData data;
        [SerializeField] private TMP_Text enemyNameText;

        private JJBHealth _health;

        public EnemyData Data => data;
        public JJBHealth Health => _health;
        public EnemyAbility Ability { get; private set; }

        private void Awake()
        {
            _health = GetComponent<JJBHealth>();
        }
        
        public void SetData(EnemyData enemyData)
        {
            data = enemyData;
            
            if (enemyNameText != null)
                enemyNameText.text = data.EnemyName;

            //UpdateEnemySprite();
        }
        
        /*private void UpdateEnemySprite()
        {
            if (data == null)
                return;

            if (spriteLibrary == null || enemyImage == null)
                return;

            Sprite sprite = spriteLibrary.GetSprite("Enemy", data.SpriteLabel);

            if (sprite != null)
                enemyImage.sprite = sprite;
        }*/

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
            if (data == null || enemyNameText == null)
                return;

            enemyNameText.text = data.EnemyName;
        }
    }
}