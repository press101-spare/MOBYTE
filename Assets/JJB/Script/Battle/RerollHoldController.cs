using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace JJB.Script.Battle
{
    public class RerollHoldController : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IPointerExitHandler
    {
        [SerializeField] private float holdTime = 0.6f;
        [SerializeField] private float reRollUp = 5f;
        [SerializeField] private float changeDelay = 0.2f;
        [SerializeField] private float rollDuration = 0.8f;

        private Coroutine _holdCoroutine;
        private bool _isLongPress;

        public void OnPointerDown(PointerEventData eventData)
        {
            _isLongPress = false;

            if (DiceManager_JCY.Instance == null || DiceManager_JCY.Instance.isRolling)
                return;

            _holdCoroutine = StartCoroutine(HoldRoutine());
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            if (_holdCoroutine != null)
                StopCoroutine(_holdCoroutine);

            _holdCoroutine = null;

            if (!_isLongPress && DiceManager_JCY.Instance != null)
                DiceManager_JCY.Instance.RerollSelectedDice();
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            if (_holdCoroutine != null)
                StopCoroutine(_holdCoroutine);

            _holdCoroutine = null;
        }

        private IEnumerator HoldRoutine()
        {
            yield return new WaitForSecondsRealtime(holdTime);

            _isLongPress = true;
            _holdCoroutine = null;

            yield return ChangeSelectedDiceTypeRoutine();
        }

        private IEnumerator ChangeSelectedDiceTypeRoutine()
        {
            DiceManager_JCY manager = DiceManager_JCY.Instance;

            if (manager == null || manager.isRolling || manager.reRollUI.reRollCount <= 0 ||
                manager.allDiceSo == null || manager.allDiceSo.Length == 0)
                yield break;

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
                StartCoroutine(ChangeDiceRoutine(dice, DiceDeckManager_JCY.Instance.diceCollection));
               
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

            DiceSO_JCY randomSO = candidates[Random.Range(0, candidates.Count)];

            int currentValue = dice.currentIndex;
            Vector3 originalPosition = dice.transform.position;
            Quaternion originalRotation = dice.transform.rotation;

            dice.SetSelected(false);

            JJB_DicePhysics physics = dice.GetComponent<JJB_DicePhysics>();

            if (physics == null)
                yield break;

            physics.RerollThrow(reRollUp);

            yield return new WaitForSeconds(changeDelay);

            ApplyDiceVisual(dice, randomSO);
            dice.Setup(randomSO);
            dice.currentIndex = currentValue;
            
            // activeDiceSo 리스트에서 기존 currentDiceSO의 위치를 찾습니다.
            var list = DiceManager_JCY.Instance.activeDiceSo;
            int index = list.IndexOf(dice.currentDiceSO);

            // 리스트에 존재하면(-1이 아니면) 해당 위치의 SO를 randomSO로 교체합니다.
            if (index != -1)
            {
                list[index] = randomSO;
            }

            Debug.Log($"주사위 종류 변경 : {randomSO.name}");

            yield return new WaitForSeconds(Mathf.Max(0f, rollDuration - changeDelay));

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

            MeshFilter sourceMesh = diceSO.dicePrefab.GetComponentInChildren<MeshFilter>(true);
            MeshRenderer sourceRenderer = diceSO.dicePrefab.GetComponentInChildren<MeshRenderer>(true);
            MeshFilter targetMesh = dice.GetComponentInChildren<MeshFilter>();

            if (sourceMesh != null && targetMesh != null)
                targetMesh.sharedMesh = sourceMesh.sharedMesh;

            if (sourceRenderer != null && dice.MeshCompo != null)
                dice.MeshCompo.sharedMaterials = sourceRenderer.sharedMaterials;
        }

        private void OnDisable()
        {
            if (_holdCoroutine != null)
                StopCoroutine(_holdCoroutine);

            _holdCoroutine = null;
            _isLongPress = false;
        }
    }
}