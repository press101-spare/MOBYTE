using DG.Tweening;
using JJB.Script.Battle.Player.Progression;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class LotteryTicket_Test : MonoBehaviour
{
    public GameObject lotteryButton;
    [SerializeField]private Button[] buttons;
    private List<int> nums = new List<int>();
    private List<int> ranNums = new List<int>();
    private Dictionary<Button,int> buttonDic = new Dictionary<Button,int>();
    private Dictionary<int,bool> clickDic = new Dictionary<int,bool>();

    [SerializeField] private int _buyChip=500;


    [SerializeField] private TextMeshProUGUI _textLoto;




    private void Awake()
    {
        for (int i = 0; i < 24; i++)
        {
            
            buttons[i] = Instantiate(lotteryButton, transform).GetComponent<Button>();
        }
    }
    private void Start()
    {
        
        int a=0;
        foreach (var button in buttons)
        {
            a++;
            button.gameObject.GetComponentInChildren<TextMeshProUGUI>().text = $"{a}";
            buttonDic[button] = a;
            button.onClick.AddListener(() => Lottery(buttonDic[button],button.gameObject));
            clickDic[a] = false;
            Debug.Log(a);
            
        }
    }

    private void OnEnable()
    {
        ranNums.Clear();
        for(int i = 0; i<5;i++)
        {
            ranNums.Add(Random.Range(1, 25)); 
        }
    }

    public void Lottery(int a,GameObject bt)
    {
        if (clickDic[a]==false)
        {
            if (nums.Count < 5)
            {
                nums.Add(a);
                clickDic[a] = true;
                bt.GetComponent<Outline>().enabled = true;
            }
            else
            {
                Debug.Log("실패");
                bt.GetComponent<Outline>().enabled = false;
            }
        }
        else if(clickDic[a]) 
        {
            nums.Remove(a);
            clickDic[a] = false;
            bt.GetComponent<Outline>().enabled = false;
        }
    }

    public void BuyLoto()
    {
        if (PlayerProfileManager.Instance.Profile.money >= _buyChip)
        {
            PlayerProfileManager.Instance.Profile.money -= _buyChip;
        }

    }
    public void EndLoto()
    {
        
        if(nums.Count < 5) 
        {
            //메세지 있음 좋을듯
            return;
        }
        int count = 0;
        foreach(var v in nums)
        {
            foreach(int i in ranNums)
            {
                if (v==i)
                {
                    count++;
                    continue;
                }
            }
        }

        Debug.Log(count);
        if (count==0)
        {
        }
        else if (count == 1)
        {
            PlayerProfileManager.Instance.Profile.money += 400;
        }
        else if(count ==2)
        {
            PlayerProfileManager.Instance.Profile.money += 600;
        }
        else if(count== 3)
        {
            PlayerProfileManager.Instance.Profile.money += 900;
        }
        else if(count == 4)
        {
            PlayerProfileManager.Instance.Profile.money += 1500;
        }
        else if(count == 5)
        {
            PlayerProfileManager.Instance.Profile.money += 3000;
        }

        StartCoroutine(Coll(count));
    }


    private IEnumerator Coll(int count)
    {
        _textLoto.gameObject.SetActive(true);
        _textLoto.text = $"{ranNums[0]},{ranNums[1]},{ranNums[2]},{ranNums[3]},{ranNums[4]}\n 맞은 개수 {count}";
        yield return new WaitForSeconds(4);
        _textLoto.DOFade(0,1.2f);
        _textLoto.gameObject.SetActive(false);
        gameObject.transform.parent.parent.gameObject.SetActive(false);
    }
}
