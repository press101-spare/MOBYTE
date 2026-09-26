using JJB.Script.Battle;
using UnityEngine;

namespace JJB.Script
{
    public class Background : MonoBehaviour
    {
        private bool _bgmStopped;
        private void Start()
        {
            AudioManager.Instance.PlayBGM("BGM_Map3");
        }

        private void Update()
        {
            if (_bgmStopped)
                return;

            if (JJBGameManager.Instance.BattleTurnManager.CurrentPhase == BattlePhase.BattleEnd)
            {
                AudioManager.Instance.StopBGM();
                _bgmStopped = true;
            }
        }
    }
}
