using System.Collections;
using UnityEngine;

namespace JJB.Script.Battle.Enemy
{
    public class EnemyHitFlash : MonoBehaviour
    {
        [SerializeField] private float duration = 1f;

        private SpriteRenderer[] _renderers;
        private Color[] _originalColors;
        private Coroutine _coroutine;

        private void Awake()
        {
            _renderers = GetComponentsInChildren<SpriteRenderer>();
            _originalColors = new Color[_renderers.Length];

            for (int i = 0; i < _renderers.Length; i++)
                _originalColors[i] = _renderers[i].color;
        }

        public void Play()
        {
            if (_coroutine != null)
                StopCoroutine(_coroutine);

            _coroutine = StartCoroutine(FlashRoutine());
        }

        private IEnumerator FlashRoutine()
        {
            for (int i = 0; i < _renderers.Length; i++)
                _renderers[i].color = Color.red;

            yield return new WaitForSeconds(duration);

            for (int i = 0; i < _renderers.Length; i++)
                _renderers[i].color = _originalColors[i];

            _coroutine = null;
        }
    }
}