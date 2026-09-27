using UnityEngine;
using UnityEngine.UI;

public class BT_SJM : MonoBehaviour
{
    public DiceSO_JCY dice;
    private Button _bt;
    private void Awake()
    {
        _bt = GetComponent<Button>();
    }
    private void OnValidate()
    {
        _bt.onClick.AddListener(()=>RE(dice));
    }
    public void RE(DiceSO_JCY dicedata)
    {
    }
}
