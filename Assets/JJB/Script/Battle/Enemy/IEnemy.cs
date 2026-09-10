using System.Collections;

namespace JJB.Script.Battle.Enemy
{
    public interface IEnemy
    {
        void Enter();
        IEnumerator Execute();
        void Exit();
    }
}