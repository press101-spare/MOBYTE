using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Betting_HTY : MonoBehaviour
{
    [SerializeField] private TMP_InputField coinBetting;
    private int _currentBettingChip = 0;

    public GambleSoData _currentGamble;

    private void Start()
    {
        _currentBettingChip =0;
        coinBetting.text = "0";//이것도 나중에 자신의 최소금액으로
    }

    private void Update()
    {
        if(int.TryParse(coinBetting.text, out int chip))
        {
            chip = Mathf.Clamp(chip,0,100);//테스트용 나중에 금액의 최소와 자신이 가진 금액 만으로 하게 만듦
            _currentBettingChip = chip;
            coinBetting.text = chip.ToString();
        }
    }

    public void BettingCoin()
    {
        if(int.TryParse(coinBetting.text,out int bettingChip))
        {
            Debug.Log(_currentBettingChip);
            Debug.Log(_currentGamble);
            GambleManager.instance.GetSetting(_currentBettingChip,_currentGamble);
            GambleManager.instance.GambleStart();
            gameObject.SetActive(false);
        }
        else
        {
            Debug.Log("베팅불가");
        }
    }

    public void BettingPM(int chip)
    {
        if (int.TryParse(coinBetting.text, out int currentChip))
        {
            currentChip += chip;
            chip = Mathf.Clamp(currentChip, 0, 100);
            coinBetting.text = currentChip.ToString();

            Debug.Log(chip);
            Debug.Log(_currentBettingChip);
        }
    }

    public void MaxChipAndZero(bool max)
    {
        if (int.TryParse(coinBetting.text, out int currentChip))
        {
            if (max)
            {
                currentChip = 100;//자신이 가진 최대 골드로 바꾸기
                coinBetting.text = currentChip.ToString();
            }
            else
            {
                currentChip = 0;//자신이 가진 최대 골드로 바꾸기
                coinBetting.text = currentChip.ToString();
            }
            
        }
    }
}
