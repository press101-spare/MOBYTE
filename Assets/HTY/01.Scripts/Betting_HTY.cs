using JJB.Script.Battle.Player.Progression;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Betting_HTY : MonoBehaviour
{
    [SerializeField] private TMP_InputField coinBetting;
    private int _currentBettingChip = 0;

    public GambleSoData _currentGamble;

    private PlayerProfile profile;

    private void OnEnable()
    {
        _currentBettingChip = 0;
        coinBetting.text = "0";
    }

    private void Update()
    {
        profile = PlayerProfileManager.Instance.Profile;

        if(int.TryParse(coinBetting.text, out int chip))
        {
            chip = Mathf.Clamp(chip,0, profile.money);
            _currentBettingChip = chip;
            coinBetting.text = chip.ToString();
        }
    }

    public void BettingCoin()
    {
        if(int.TryParse(coinBetting.text,out int bettingChip))
        {
            if(_currentGamble._canBettingChip<=_currentBettingChip)
            {
                GambleManager.instance.GetSetting(_currentBettingChip, _currentGamble);
                PlayerProfileManager.Instance.Profile.money -= bettingChip;
                GambleManager.instance.GambleStart();
                gameObject.SetActive(false);
            }
            else
            {
                Debug.Log($"{_currentGamble._canBettingChip} 이상의 칩을 베팅해야 합니다");
            }
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
            chip = Mathf.Clamp(currentChip, 0, profile.money);
            coinBetting.text = currentChip.ToString();
        }
    }

    public void MaxChipAndZero(bool max)
    {
        if (int.TryParse(coinBetting.text, out int currentChip))
        {
            if (max)
            {
                currentChip = profile.money;
                coinBetting.text = currentChip.ToString();
            }
            else
            {
                currentChip = 0;//겜블이 요구하는 최소금액
                coinBetting.text = currentChip.ToString();
            }
        }
    }
}
