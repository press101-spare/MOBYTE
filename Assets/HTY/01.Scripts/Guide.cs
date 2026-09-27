using TMPro;
using UnityEngine;

public class Guide : MonoBehaviour
{
    private TextMeshProUGUI _gambleName;
    private TextMeshProUGUI _explanationName;

    private void Start()
    {
        _gambleName.text = $"";
    }
}
