using UnityEngine;
using UnityEngine.UI;

public class DeletePanel : MonoBehaviour
{
    [SerializeField] private GameObject _diceGroup;
    [SerializeField] private GameObject _diceBTTem;//

    private void Start()
    {
        ShowDice();
    }

    public void ShowDice()
    {
        for (int i = 0; i < DiceManager_JCY.Instance.allDiceSo.Length; i++)
        {
            GameObject a = Instantiate(_diceBTTem, _diceGroup.transform);
            //a.GetComponent<Button>().onClick.AddListener(()=>);
        }
    }
}
