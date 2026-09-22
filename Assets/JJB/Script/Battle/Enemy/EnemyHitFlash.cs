using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace JJB.Script.Battle.Enemy
{
    public class EnemyHitFlash : MonoBehaviour
    {
        [SerializeField] private Image visualImage;
        [SerializeField] private float duration = 1f;

        private Color _originalColor;
        private Coroutine _coroutine;

        private void Awake()
        {
            if (visualImage != null)
                _originalColor = visualImage.color;
        }

        public void Play()
        {
            if (visualImage == null)
                return;

            if (_coroutine != null)
                StopCoroutine(_coroutine);

            _coroutine = StartCoroutine(FlashRoutine());
        }

        private IEnumerator FlashRoutine()
        {
            visualImage.color = new Color(
                1f,
                0f,
                0f,
                _originalColor.a
            );

            yield return new WaitForSeconds(duration);

            visualImage.color = _originalColor;

            _coroutine = null;
        }
    }
}