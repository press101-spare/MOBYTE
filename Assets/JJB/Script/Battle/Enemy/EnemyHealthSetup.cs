using System.Collections;
using TMPro;
using UnityEngine;
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
        [SerializeField] private bool flipX = true;

        [Header("Animation")]
        [SerializeField] private int idleFrameCount = 5;
        [SerializeField] private int attackFrameCount = 5;
        [SerializeField] private int hurtFrameCount = 2;
        [SerializeField] private float frameDelay = 0.12f;

        private JJBHealth _health;
        private Coroutine _animationCoroutine;

        public EnemyData Data => data;
        public JJBHealth Health => _health;
        public EnemyAbility Ability { get; private set; }

        private void Awake()
        {
            _health = GetComponent<JJBHealth>();

            ApplyFlipX();
        }

        private void Start()
        {
            UpdateEnemySpriteLibrary();
            PlayIdle();
        }

        public void SetData(EnemyData enemyData)
        {
            data = enemyData;

            if (data == null)
                return;

            if (enemyNameText != null)
                enemyNameText.text = data.EnemyName;

            UpdateEnemySpriteLibrary();
            PlayIdle();
        }

        private void UpdateEnemySpriteLibrary()
        {
            if (data == null ||
                enemyImage == null ||
                data.SpriteLibraryAsset == null)
            {
                return;
            }

            Sprite sprite =
                data.SpriteLibraryAsset.GetSprite(
                    "Idle",
                    "Idle"
                );

            if (sprite != null)
                enemyImage.sprite = sprite;
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

        public void PlayIdle()
        {
            PlayAnimation(
                "Idle",
                idleFrameCount,
                true
            );
        }

        public void PlayAttack()
        {
            PlayAnimation(
                "Attack",
                attackFrameCount,
                false
            );
        }

        public void PlayHurt()
        {
            PlayAnimation(
                "Hurt",
                hurtFrameCount,
                false
            );
        }

        private void PlayAnimation(string category, int frameCount, bool loop)
        {
            if (data == null ||
                data.SpriteLibraryAsset == null ||
                enemyImage == null)
            {
                Debug.LogError(
                    $"애니메이션 실행 실패 : {category}",
                    this
                );

                return;
            }

            if (_animationCoroutine != null)
                StopCoroutine(_animationCoroutine);

            _animationCoroutine =
                StartCoroutine(
                    AnimationRoutine(
                        category,
                        frameCount,
                        loop
                    )
                );
        }

        private IEnumerator AnimationRoutine(string category, int frameCount, bool loop)
        {
            do
            {
                for (int i = 0; i < frameCount; i++)
                {
                    string label =
                        i == 0
                            ? category
                            : category + i;

                    Sprite sprite =
                        data.SpriteLibraryAsset.GetSprite(
                            category,
                            label
                        );

                    if (sprite != null)
                    {
                        enemyImage.sprite = sprite;
                    }
                    else
                    {
                        Debug.LogWarning(
                            $"Sprite 없음 : {category} / {label}",
                            this
                        );
                    }

                    yield return new WaitForSeconds(
                        frameDelay
                    );
                }
            }
            while (loop);

            _animationCoroutine = null;

            if (category != "Idle")
                PlayIdle();
        }

        private void ApplyFlipX()
        {
            if (enemyImage == null)
                return;

            Vector3 scale =
                enemyImage.rectTransform.localScale;

            scale.x =
                Mathf.Abs(scale.x) *
                (flipX ? -1f : 1f);

            enemyImage.rectTransform.localScale =
                scale;
        }

        private void OnValidate()
        {
            if (data == null)
                return;

            if (enemyNameText != null)
                enemyNameText.text = data.EnemyName;

            if (enemyImage != null &&
                data.SpriteLibraryAsset != null)
            {
                Sprite sprite =
                    data.SpriteLibraryAsset.GetSprite(
                        "Idle",
                        "Idle"
                    );

                if (sprite != null)
                    enemyImage.sprite = sprite;
            }

            ApplyFlipX();
        }
    }
}