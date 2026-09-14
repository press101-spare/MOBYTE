using System;
using System.Collections;
using UnityEngine;

namespace JJB.Script.Battle.Player
{
    [RequireComponent(typeof(DiceBattleAdapter))]
    public class PlayerTurnController : MonoBehaviour
    {
        private DiceBattleAdapter _diceBattleAdapter;
        private PlayerAttackController _playerAttackController;

        private void Awake()
        {
            _diceBattleAdapter = GetComponent<DiceBattleAdapter>();
        }

        public void Initialize(PlayerAttackController playerAttackController)
        {
            _playerAttackController = playerAttackController;
        }

        public void DrawDice(Action onFinished)
        {
            if (_diceBattleAdapter.IsRolling)
                return;

            _diceBattleAdapter.DrawDice();

            StartCoroutine(WaitForDraw(onFinished));
        }

        private IEnumerator WaitForDraw(Action onFinished)
        {
            yield return new WaitUntil(() => !_diceBattleAdapter.IsRolling);

            onFinished?.Invoke();
        }

        public bool TryAttack()
        {
            if (_diceBattleAdapter.IsRolling)
                return false;

            int score = _diceBattleAdapter.CurrentScore;

            if (score <= 0)
            {
                Debug.Log("족보를 먼저 선택해주세요.");
                return false;
            }

            if (_playerAttackController == null)
            {
                Debug.LogError("PlayerAttackController가 연결되지 않았습니다.");
                return false;
            }

            return _playerAttackController.Attack(score);
        }

        public void StartDefense(Action onFinished)
        {
            StartCoroutine(DefenseRoutine(onFinished));
        }

        private IEnumerator DefenseRoutine(Action onFinished)
        {
            _diceBattleAdapter.StartDefenseDice();

            yield return new WaitUntil(() => !_diceBattleAdapter.IsRolling);

            _diceBattleAdapter.SetShieldMode(false);

            Debug.Log($"방어 완료 / Shield : {_diceBattleAdapter.ShieldValue}");

            onFinished?.Invoke();
        }

        public void ClearDice()
        {
            _diceBattleAdapter.ClearDice();
        }
    }
}