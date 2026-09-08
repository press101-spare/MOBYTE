using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace JJB.Script.Slot
{
    public class SlotMachineController : MonoBehaviour
    {
        [SerializeField]
        private SlotReel[] reels;

        [SerializeField]
        private Button spinButton;

        [SerializeField]
        private TMP_Text resultText;

        private bool _isSpinning;

        public void Spin()
        {
            if (_isSpinning)
                return;

            StartCoroutine(SpinRoutine());
        }

        private IEnumerator SpinRoutine()
        {
            _isSpinning = true;

            spinButton.interactable = false;

            if (resultText != null)
                resultText.text = "";
            
            Coroutine reel1 = StartCoroutine(reels[0].Spin(2.2f));
            Coroutine reel2 = StartCoroutine(reels[1].Spin(2.45f));
            Coroutine reel3 = StartCoroutine(reels[2].Spin(2.7f));
            
            yield return reel3;

            CheckResult();

            spinButton.interactable = true;

            _isSpinning = false;
        }

        private void CheckResult()
        {
            int a = reels[0].ResultIndex;
            int b = reels[1].ResultIndex;
            int c = reels[2].ResultIndex;

            if (a == b && b == c)
            {
                resultText.text = "JACKPOT";
                return;
            }

            if (a == b || b == c || a == c)
            {
                resultText.text = "PAIR";
                return;
            }

            resultText.text = "MISS";
        }
    }
}