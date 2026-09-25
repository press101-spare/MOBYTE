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
            Play(Color.red);
        }

        public void Play(Color flashColor)
        {
            if (visualImage == null)
                return;

            if (_coroutine != null)
                StopCoroutine(_coroutine);

            _coroutine = StartCoroutine(FlashRoutine(flashColor));
        }

        private IEnumerator FlashRoutine(Color flashColor)
        {
            flashColor.a = _originalColor.a;
            visualImage.color = flashColor;

            yield return new WaitForSeconds(duration);

            visualImage.color = _originalColor;
            _coroutine = null;
        }
    }
}