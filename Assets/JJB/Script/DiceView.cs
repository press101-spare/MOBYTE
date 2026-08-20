using UnityEngine;
using UnityEngine.UI;

namespace JJB.Script
{
    public class DiceView : MonoBehaviour
    {
        [SerializeField] private Image diceImage;
        [SerializeField] private Sprite[] diceSprites;

        public void UpdateView(int value)
        {
            if (value < 1 || value > diceSprites.Length)
                return;

            diceImage.sprite = diceSprites[value - 1];
        }
    }
}