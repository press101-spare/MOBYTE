using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DeletePanel : MonoBehaviour
{
    [SerializeField] private GameObject _diceGroup;
    [SerializeField] private Button _diceBTTem;
    private List<GameObject> _selectDice = new();

    private void Start()
    {
        ShowDice();
    }

    public void ShowDice()
    {
        for (int i = 0; i < DiceDeckManager_JCY.Instance.diceCollection.Count; i++)
        {
            GameObject dice = Instantiate(_diceBTTem.gameObject, _diceGroup.transform);
            dice.gameObject.GetComponent<Image>().sprite = DiceDeckManager_JCY.Instance.diceCollection[i].diceIcon;
            dice.GetComponentInChildren<TextMeshProUGUI>().text = DiceDeckManager_JCY.Instance.diceCollection[i].diceName;
            dice.GetComponent<Button>().onClick.AddListener(() =>
            {
                dice.gameObject.GetComponent<Outline>().enabled = !dice.gameObject.GetComponent<Outline>().enabled;
                if(dice.gameObject.GetComponent<Outline>().enabled)
                {
                    _selectDice.Add(dice);
                }
                else
                {
                    _selectDice.Remove(dice);
                }
            });
        }
    }

}
