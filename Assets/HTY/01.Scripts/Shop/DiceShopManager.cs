using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;
public class DiceShopManager : MonoBehaviour
{
    [Header("디스플레이 관련")]
    [SerializeField] private GameObject _temp;
    [SerializeField] private Image[] beckGround;
    [SerializeField] private TextMeshProUGUI[] nameText;
    [SerializeField] private TextMeshProUGUI[] costText;
    [SerializeField] private TextMeshProUGUI[] description;

    [Header("SO 관련")]
    public DiceSO_JCY[] currentDiceSO = new DiceSO_JCY[10];


    private void Start()
    {
        OnDisplay();
    }

    public void OnDisplay()
    {
        for (int i = 0; i < nameText.Length; i++)
        {
            GameObject a =  Instantiate(_temp,transform);
            currentDiceSO[i] =
                DiceManager_JCY.Instance.allDiceSo[Random.Range(0, DiceManager_JCY.Instance.allDiceSo.Length)];
           //a.transform.GetChild(3).gameObject.GetComponent<Button>().onClick
            //    .AddListener();
            beckGround[i].color = currentDiceSO[i].color;
            nameText[i].text = currentDiceSO[i].diceName;
            nameText[i].color = currentDiceSO[i].color;
            costText[i].text = currentDiceSO[i].cost.ToString();
            description[i].text = currentDiceSO[i].diceDescription;
        }
    }

    private void BuyDice()
    {

    }
}
