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
    private Dictionary<Button,int> buttonDic = new Dictionary<Button,int>();
    private Dictionary<int,bool> clickDic = new Dictionary<int,bool>();


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
            button.onClick.AddListener(() => Lottery(buttonDic[button]));
            clickDic[a] = false;
            Debug.Log(a);
            
        }
    }
    public void Lottery(int a)
    {
        if (clickDic[a]==false)//눌린적 없으면
        {
            if (nums.Count < 3)
            {
                nums.Add(a);
                clickDic[a] = true;
                Debug.Log($"{a} {clickDic[a]}");
                Debug.Log($"현재 숫자수 {nums.Count}");
            }
            else
            {
                Debug.Log("실패");
            }
        }
        else if(clickDic[a]) 
        {
            nums.Remove(a);
            clickDic[a] = false;
            Debug.Log($"{a} {clickDic[a]}");
            Debug.Log($"현재 숫자수 {nums.Count}");
        }
    }
}
