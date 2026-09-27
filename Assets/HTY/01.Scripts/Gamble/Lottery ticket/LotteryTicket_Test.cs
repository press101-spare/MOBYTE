using DG.Tweening;
using JJB.Script.Battle.Player.Progression;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LotteryTicket_Test : MonoBehaviour
{
    public GameObject lotteryButton;

    [SerializeField] private Button[] buttons;

    private List<int> nums = new List<int>();
    private List<int> ranNums = new List<int>();

    private Dictionary<Button, int> buttonDic = new Dictionary<Button, int>();
    private Dictionary<int, bool> clickDic = new Dictionary<int, bool>();

    [SerializeField] private int _buyChip = 500;
    [SerializeField] private TextMeshProUGUI _textLoto;

    private void Awake()
    {
        // Inspector에서 배열 크기를 설정하지 않아도 됨
        buttons = new Button[24];

        for (int i = 0; i < 24; i++)
        {
            buttons[i] = Instantiate(lotteryButton, transform)
                .GetComponent<Button>();
        }
    }

    private void Start()
    {
        for (int i = 0; i < buttons.Length; i++)
        {
            int number = i + 1;
            Button button = buttons[i];

            button.GetComponentInChildren<TextMeshProUGUI>().text
                = number.ToString();

            buttonDic[button] = number;
            clickDic[number] = false;

            button.onClick.AddListener(() =>
            {
                Lottery(number, button.gameObject);
            });
        }

        ResetLotto();
    }

    private void OnEnable()
    {
        // Start 이전에는 Dictionary 세팅이 안 되어 있으므로
        // Start 이후부터 초기화
        if (clickDic.Count > 0)
        {
            ResetLotto();
        }
    }

    public void Lottery(int number, GameObject buttonObject)
    {
        if (clickDic[number] == false)
        {
            // 최대 5개까지만 선택
            if (nums.Count >= 5)
            {
                Debug.Log("최대 5개까지만 선택할 수 있습니다.");
                return;
            }

            nums.Add(number);

            clickDic[number] = true;

            buttonObject.GetComponent<Outline>().enabled = true;
        }
        else
        {
            nums.Remove(number);

            clickDic[number] = false;

            buttonObject.GetComponent<Outline>().enabled = false;
        }
    }

    public void BuyLoto()
    {
        if (PlayerProfileManager.Instance.Profile.money < _buyChip)
        {
            Debug.Log("돈이 부족합니다.");
            return;
        }

        PlayerProfileManager.Instance.Profile.money -= _buyChip;
    }

    public void EndLoto()
    {
        if (nums.Count < 5)
        {
            Debug.Log("번호를 5개 선택해주세요.");
            return;
        }

        int count = 0;

        foreach (int selectedNumber in nums)
        {
            foreach (int randomNumber in ranNums)
            {
                if (selectedNumber == randomNumber)
                {
                    count++;
                    break;
                }
            }
        }

        Debug.Log($"맞은 개수 : {count}");

        if (count == 1)
        {
            PlayerProfileManager.Instance.Profile.money += 400;
        }
        else if (count == 2)
        {
            PlayerProfileManager.Instance.Profile.money += 600;
        }
        else if (count == 3)
        {
            PlayerProfileManager.Instance.Profile.money += 900;
        }
        else if (count == 4)
        {
            PlayerProfileManager.Instance.Profile.money += 1500;
        }
        else if (count == 5)
        {
            PlayerProfileManager.Instance.Profile.money += 3000;
        }

        StartCoroutine(Coll(count));
    }

    private IEnumerator Coll(int count)
    {
        _textLoto.gameObject.SetActive(true);

        // 이전 판에서 Fade 됐을 수 있으므로 알파값 복구
        _textLoto.alpha = 1f;

        _textLoto.text =
            $"{ranNums[0]}, {ranNums[1]}, {ranNums[2]}, {ranNums[3]}, {ranNums[4]}\n" +
            $"맞은 개수 {count}";

        yield return new WaitForSeconds(4f);

        // Fade 실행
        _textLoto.DOFade(0f, 1.2f);

        // DOFade는 기다리지 않기 때문에 직접 기다려줘야 함
        yield return new WaitForSeconds(1.2f);

        GambleManager.instance.selectCompo._canSelect = true;

        _textLoto.gameObject.SetActive(false);

        // 다음 판을 위한 초기화
        ResetLotto();

        transform.parent.parent.gameObject.SetActive(false);
    }

    private void ResetLotto()
    {
        // 플레이어가 선택한 번호 초기화
        nums.Clear();

        // 당첨 번호 초기화
        ranNums.Clear();

        // 모든 버튼 초기화
        for (int i = 0; i < buttons.Length; i++)
        {
            int number = i + 1;

            clickDic[number] = false;

            Outline outline = buttons[i].GetComponent<Outline>();

            if (outline != null)
            {
                outline.enabled = false;
            }
        }

        // 결과 Text 초기화
        _textLoto.DOKill();
        _textLoto.alpha = 1f;
        _textLoto.gameObject.SetActive(false);

        // 새로운 당첨 번호 5개 생성
        CreateRandomNumbers();
    }

    private void CreateRandomNumbers()
    {
        while (ranNums.Count < 5)
        {
            int randomNumber = Random.Range(1, 25);

            // 이미 있는 번호라면 추가하지 않음
            if (!ranNums.Contains(randomNumber))
            {
                ranNums.Add(randomNumber);
            }
        }
    }
}