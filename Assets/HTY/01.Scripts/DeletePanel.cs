using UnityEngine;
using UnityEngine.UI;

public class DeletePanel : MonoBehaviour
{
    [SerializeField] private GameObject _diceGroup;
    [SerializeField] private Button _diceBTTem;

    private void Start()
    {
        ShowDice();
    }

    public void ShowDice()
    {
        for (int i = 0; i < DiceManager_JCY.Instance.allDiceSo.Length; i++)
        {
            /*DiceDeckManager_JCY
            GameObject a = Instantiate(_diceBTTem.gameObject, _diceGroup.transform);
            a.gameObject.GetComponent<Image>().sprite=
                \a.GetComponent<Button>().onClick.AddListener(()=>);*/
        }
    }
}
