using DG.Tweening;
using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class ShopManager_JCY : MonoBehaviour
{
    [Header("디스플레이 관련")]
    [SerializeField] private Image[] beckGround;
    [SerializeField] private TextMeshProUGUI[] nameText;
    [SerializeField] private TextMeshProUGUI[] costText;
    [SerializeField] private TextMeshProUGUI[] description;
    [SerializeField] private DiceType_HTY[] _btType;
    [SerializeField] private Image[] _diceImage;
    [SerializeField] private TextMeshProUGUI _reRollText;

    [SerializeField] private TextMeshProUGUI _notReRoll;

    [SerializeField] private RectTransform[] points;

    public GameObject _par;

    private int _reRoll;

    [Header("SO 관련")]
    private List<DiceSO_JCY> currentDiceSO = new List<DiceSO_JCY>();

    Sequence seq;

    private void Start()
    {
        _reRoll = Random.Range(1,4);
        _reRollText.text = $"남은 새로고침 수:{_reRoll}";
        _reRoll++;
        OnDisplay();

    }

    public void OnDisplay()
    {
        if (_reRoll <= 0)
        {
            NotReRoll();
            return;
        }
        currentDiceSO.Clear();
        for (int i = 0; i < nameText.Length; i++)
        {
            currentDiceSO.Add(DiceManager_JCY.Instance.allDiceSo[Random.Range(0, DiceManager_JCY.Instance.allDiceSo.Length)]);
            beckGround[i].color =  currentDiceSO[i].color;
            nameText[i].text = currentDiceSO[i].diceName;
            nameText[i].color  = currentDiceSO[i].color;
            costText[i].text = $"{currentDiceSO[i].cost.ToString()} 칩";
            description[i].text = currentDiceSO[i].diceDescription;
            _btType[i].SetDice(currentDiceSO[i]);
            _btType[i].SetRe();
            _diceImage[i].sprite = currentDiceSO[i].diceIcon;
        }

        _reRoll--;
        _reRollText.text = $"남은 새로고침 수:{_reRoll}";
    }

    public void NotReRoll()
    {
        seq = DOTween.Sequence();
        GameObject a = Instantiate(_notReRoll.gameObject,_par.transform);
        a.gameObject.SetActive(true);
        a.GetComponent<RectTransform>().position = points[0].position;
        seq.Append(a.GetComponent<RectTransform>().DOAnchorPos(new Vector2(points[0].anchoredPosition.x, points[0].anchoredPosition.y+270), 3f));
        seq.AppendCallback(()=> Destroy(a));
    }


    public void DiceDestroy()
    {

    }
}
