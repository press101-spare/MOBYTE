using JJB.Script.Battle.Player.Progression;
using System.Drawing;
using TMPro;
using UnityEngine;

public class GambleManager : MonoBehaviour
{
    public GameObject _gambleUI;
    public GameObject _endPanel;
    private int _bettingChip;
    private GambleSoData _gambleData;
    [SerializeField] private Transform point;

    public static GambleManager instance;

    public bool _isFirst = true;

    public GameObject _title;
    public GameObject _casino;

    

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
    
    public void GambleEnd(float x)
    {
        _endPanel.SetActive(true);
        _endPanel.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text
            = $"{_gambleData._gambleName}의 게임 결과: \n 획득배수:{x}x \n 얻은 칩:{x*_bettingChip}칩 \n 현재 보유 칩:{PlayerProfileManager.Instance.Profile.money+ x * _bettingChip} 칩";
        PlayerProfileManager.Instance.Profile.money += Mathf.CeilToInt(x * _bettingChip);

        ResetGamble();
    }

    public void ResetGamble()
    {
        _bettingChip = 0;
        _gambleData = null;
    }


}
