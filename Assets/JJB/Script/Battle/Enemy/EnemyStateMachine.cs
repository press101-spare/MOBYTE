using System.Collections;
using UnityEngine;

namespace JJB.Script.Battle.Enemy
{
    public class EnemyStateMachine : MonoBehaviour
    {
        private IEnemy _currentState;

        public void ChangeState(IEnemy newState)
        {
            _currentState?.Exit();

            _currentState = newState;

            _currentState.Enter();
        }

        public IEnumerator ExecuteCurrentState()
        {
            if (_currentState == null)
                yield break;

            yield return _currentState.Execute();
        }
    }
}