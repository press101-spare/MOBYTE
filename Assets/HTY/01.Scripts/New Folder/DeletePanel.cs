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
            dice.GetComponentInChildren<TextMeshProUGUI>().text = 
                DiceDeckManager_JCY.Instance.diceCollection[i].diceName;

            Debug.Log(i);
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


    public void DeleteBT()
    {
        int currentDiceCount = DiceDeckManager_JCY.Instance.diceCollection.Count;
        int deleteDiceCount = _selectDice.Count;

        // 삭제 후 15개 미만이 되는지 검사
        if (currentDiceCount - deleteDiceCount < 15)
        {
            Debug.Log("주사위는 최소 15개 이상 보유해야 합니다.");
            return;
        }

        foreach (GameObject dice in _selectDice)
        {
            string diceName =
                dice.GetComponentInChildren<TextMeshProUGUI>().text;

            // 이름이 같은 주사위 하나 찾기
            DiceSO_JCY targetDice =
                DiceDeckManager_JCY.Instance.diceCollection.Find(
                    diceData => diceData.diceName == diceName
                );

            // 찾았다면 딱 하나만 삭제
            if (targetDice != null)
            {
                DiceDeckManager_JCY.Instance.diceCollection.Remove(targetDice);
            }

            // UI에서도 삭제
            Destroy(dice);
        }

        _selectDice.Clear();
    }

}
