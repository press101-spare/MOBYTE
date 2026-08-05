using TMPro;
using UnityEngine;

public class ThisNumber : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _textt;

    public void Re(int _num)
    {
        _textt.text = ("this num :" + _num);
    }
}
