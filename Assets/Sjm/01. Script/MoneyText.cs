using TMPro;
using UnityEngine;

public class MoneyText : MonoBehaviour
{
    private TextMeshProUGUI moneytext;
    private void Awake()
    {
        moneytext = GetComponent<TextMeshProUGUI>();
    }
    private void Update()
    {
        moneytext.text = TotalManager.Instance.Cost.ToString();
    }
}
