using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace JJB.Script.Battle
{
    public class RerollHoldController : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IPointerExitHandler
    {
        [Header("Long Press")]
        [SerializeField] private float holdTime = 0.6f;
        [SerializeField] private float reRollUp = 5f;
        [SerializeField] private float changeDelay = 0.2f;
        [SerializeField] private float rollDuration = 0.8f;

        [Header("Hold Gauge")]
        [SerializeField] private Image holdGauge;
        [SerializeField] private int gaugeTextureSize = 128;
        [SerializeField] private float gaugeThickness = 10f;

        private Coroutine _holdCoroutine;
        private bool _isLongPress;

        private Texture2D _holdGaugeTexture;
        private Sprite _holdGaugeSprite;

        private void Awake()
        {
            CreateHoldGauge();

            if (holdGauge != null)
                holdGauge.fillAmount = 0f;
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            _isLongPress = false;

            if (DiceManager_JCY.Instance == null || DiceManager_JCY.Instance.isRolling)
                return;

            if (holdGauge != null)
                holdGauge.fillAmount = 0f;

            _holdCoroutine = StartCoroutine(HoldRoutine());
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            if (_holdCoroutine != null)
                StopCoroutine(_holdCoroutine);

            _holdCoroutine = null;

            ResetGauge();

            if (!_isLongPress && DiceManager_JCY.Instance != null)
                DiceManager_JCY.Instance.RerollSelectedDice();
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            if (_holdCoroutine != null)
                StopCoroutine(_holdCoroutine);

            _holdCoroutine = null;

            ResetGauge();
        }

        private IEnumerator HoldRoutine()
        {
            float elapsedTime = 0f;

            while (elapsedTime < holdTime)
            {
                elapsedTime += Time.unscaledDeltaTime;

                if (holdGauge != null)
                {
                    holdGauge.fillAmount =
                        Mathf.Clamp01(elapsedTime / holdTime);
                }

                yield return null;
            }

            if (holdGauge != null)
                holdGauge.fillAmount = 1f;

            _isLongPress = true;
            _holdCoroutine = null;

            yield return ChangeSelectedDiceTypeRoutine();
        }

        private IEnumerator ChangeSelectedDiceTypeRoutine()
        {
            DiceManager_JCY manager = DiceManager_JCY.Instance;

            if (manager == null ||
                manager.isRolling ||
                manager.reRollUI.reRollCount <= 0 ||
                manager.allDiceSo == null ||
                manager.allDiceSo.Length == 0)
            {
                yield break;
            }

            List<DiceObject_JCY> selectedDice = new List<DiceObject_JCY>();

            foreach (DiceObject_JCY dice in manager.ActiveDiceScripts)
            {
                if (dice.IsSelected)
                    selectedDice.Add(dice);
            }

            if (selectedDice.Count == 0)
                yield break;

            manager.isRolling = true;
            manager.reRollUI.UpdateReRollCount(-1);

            foreach (DiceObject_JCY dice in selectedDice)
            {
                StartCoroutine(
                    ChangeDiceRoutine(
                        dice,
                        DiceDeckManager_JCY.Instance.diceCollection
                    )
                );
            }

            yield return new WaitForSeconds(rollDuration);

            DiceManager_JCY.Instance.DiceSlotSet();

            manager.isRolling = false;
        }

        private IEnumerator ChangeDiceRoutine(DiceObject_JCY dice, List<DiceSO_JCY> allDiceSo)
        {
            List<DiceSO_JCY> candidates = new List<DiceSO_JCY>();

            foreach (DiceSO_JCY diceSO in allDiceSo)
            {
                if (diceSO != null && diceSO != dice.currentDiceSO)
                    candidates.Add(diceSO);
            }

            if (candidates.Count == 0)
                yield break;

            DiceSO_JCY randomSO =
                candidates[Random.Range(0, candidates.Count)];

            int currentValue = dice.currentIndex;

            Vector3 originalPosition = dice.transform.position;
            Quaternion originalRotation = dice.transform.rotation;

            dice.SetSelected(false);

            JJB_DicePhysics physics =
                dice.GetComponent<JJB_DicePhysics>();

            if (physics == null)
                yield break;

            physics.RerollThrow(reRollUp);

            yield return new WaitForSeconds(changeDelay);

            // activeDiceSo 리스트에서 기존 currentDiceSO 위치 찾기
            var list = DiceManager_JCY.Instance.activeDiceSo;

            int index = list.IndexOf(dice.currentDiceSO);

            // 기존 SO를 새 SO로 교체
            if (index != -1)
            {
                list[index] = randomSO;
            }

            ApplyDiceVisual(dice, randomSO);

            dice.Setup(randomSO);

            // 기존 눈 값 유지
            dice.currentIndex = currentValue;

            Debug.Log($"주사위 종류 변경 : {randomSO.name}");

            yield return new WaitForSeconds(
                Mathf.Max(0f, rollDuration - changeDelay)
            );

            Rigidbody rb = dice.GetComponent<Rigidbody>();

            if (rb != null)
            {
                if (!rb.isKinematic)
                {
                    rb.linearVelocity = Vector3.zero;
                    rb.angularVelocity = Vector3.zero;
                }

                rb.isKinematic = true;
            }

            dice.transform.position = originalPosition;
            dice.transform.rotation = originalRotation;

            dice.currentIndex = currentValue;
        }

        private void ApplyDiceVisual(DiceObject_JCY dice, DiceSO_JCY diceSO)
        {
            if (diceSO.dicePrefab == null)
                return;

            MeshFilter sourceMesh =
                diceSO.dicePrefab.GetComponentInChildren<MeshFilter>(true);

            MeshRenderer sourceRenderer =
                diceSO.dicePrefab.GetComponentInChildren<MeshRenderer>(true);

            MeshFilter targetMesh =
                dice.GetComponentInChildren<MeshFilter>();

            if (sourceMesh != null && targetMesh != null)
                targetMesh.sharedMesh = sourceMesh.sharedMesh;

            if (sourceRenderer != null && dice.MeshCompo != null)
                dice.MeshCompo.sharedMaterials = sourceRenderer.sharedMaterials;
        }

        private void CreateHoldGauge()
        {
            if (holdGauge == null)
                return;

            int size = Mathf.Max(16, gaugeTextureSize);

            float outerRadius = size * 0.5f - 1f;
            float innerRadius =
                Mathf.Max(0f, outerRadius - gaugeThickness);

            _holdGaugeTexture =
                new Texture2D(
                    size,
                    size,
                    TextureFormat.RGBA32,
                    false
                );

            _holdGaugeTexture.filterMode = FilterMode.Bilinear;

            Color[] pixels = new Color[size * size];

            Vector2 center = new Vector2(
                (size - 1) * 0.5f,
                (size - 1) * 0.5f
            );

            for (int y = 0; y < size; y++)
            {
                for (int x = 0; x < size; x++)
                {
                    float distance = Vector2.Distance(
                        new Vector2(x, y),
                        center
                    );

                    if (distance <= outerRadius &&
                        distance >= innerRadius)
                    {
                        pixels[y * size + x] = Color.white;
                    }
                    else
                    {
                        pixels[y * size + x] = Color.clear;
                    }
                }
            }

            _holdGaugeTexture.SetPixels(pixels);
            _holdGaugeTexture.Apply();

            _holdGaugeSprite = Sprite.Create(
                _holdGaugeTexture,
                new Rect(0f, 0f, size, size),
                new Vector2(0.5f, 0.5f),
                100f
            );

            holdGauge.sprite = _holdGaugeSprite;

            holdGauge.type = Image.Type.Filled;
            holdGauge.fillMethod = Image.FillMethod.Radial360;

            // 위쪽에서 시작
            holdGauge.fillOrigin = 2;

            holdGauge.fillClockwise = true;
            holdGauge.fillAmount = 0f;
            holdGauge.raycastTarget = false;
            holdGauge.preserveAspect = true;
        }

        private void ResetGauge()
        {
            if (holdGauge != null)
                holdGauge.fillAmount = 0f;
        }

        private void OnDisable()
        {
            if (_holdCoroutine != null)
                StopCoroutine(_holdCoroutine);

            _holdCoroutine = null;
            _isLongPress = false;

            ResetGauge();
        }

        private void OnDestroy()
        {
            if (_holdGaugeSprite != null)
                Destroy(_holdGaugeSprite);

            if (_holdGaugeTexture != null)
                Destroy(_holdGaugeTexture);
        }
    }
}