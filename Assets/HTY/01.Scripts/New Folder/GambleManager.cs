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

    public bool _isFirst = true;

    public GameObject _title;
    public GameObject _casino;

    public SelectTest_HTY selectCompo;

    

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

    private void Start()
    {
        if (_isFirst)
        {
            _title.SetActive(true);
            _casino.SetActive(false);
        }
        else
        {
            _title.SetActive(false);
            _casino.SetActive(true);
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

        ResetGamble();
    }

    public void ResetGamble()
    {
        _bettingChip = 0;
        _gambleData = null;
        selectCompo._canSelect = true;
    }


}
