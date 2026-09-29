using JJB.Script.Battle.Player.Progression;
using System.Drawing;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GambleManager : MonoBehaviour
{
    public GameObject _gambleUI;
    public GameObject _endPanel;
    private int _bettingChip;
    private GambleSoData _gambleData;
    [SerializeField] private Transform point;
    public TextMeshProUGUI _shopChipText;

    public static GambleManager instance;

    public SelectTest_HTY selectCompo;
    public GambleSuccessEffect_HTY _successEffect;

    

    private void Awake()
    {
        
        if(instance==null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Update()
    {
        if (_shopChipText)
        {
            _shopChipText.text = PlayerProfileManager.Instance.Profile.money.ToString();
        }
    }

    public void GetSetting(int chip,GambleSoData data)
    {
        _bettingChip = chip;
        _gambleData = data;
    }

    public void GambleStart()
    {
        _gambleUI.SetActive(true);
        GambleUI_HTY[] uiTypes= _gambleUI.GetComponentsInChildren<GambleUI_HTY>(true);
        foreach (var v in uiTypes)
        {
            if(v._myGamble==_gambleData)
            {
                v.gameObject.SetActive(true);
                return;
            }
        }
    }

    public void GambleEnd(float coin)
    {
        _endPanel.SetActive(true);

        TextMeshProUGUI resultText =
            _endPanel.GetComponentInChildren<TextMeshProUGUI>();
        
        resultText.text =
            $"{_gambleData._gambleName}의 게임 결과: \n" +
            $"획득배수: {coin}x \n" +
            $"얻은 칩: {coin * _bettingChip}칩 \n" +
            $"현재 보유 칩: {PlayerProfileManager.Instance.Profile.money + coin * _bettingChip}칩";

        PlayerProfileManager.Instance.Profile.money
            += Mathf.CeilToInt(coin * _bettingChip);
        AudioManager.Instance.PlayClipSFX("SFX_BUY2");

        ResetGamble();
        
    }

    public void ResetGamble()
    {
        _bettingChip = 0;
        _gambleData = null;
        selectCompo._canSelect = true;
    }


}
