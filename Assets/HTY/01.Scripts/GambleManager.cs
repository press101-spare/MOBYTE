using System.Drawing;
using TMPro;
using UnityEngine;

public class GambleManager : MonoBehaviour
{
    public GameObject _gambleUI;
    public TextMeshProUGUI _endText;
    public TextMeshProUGUI _endChipText;
    private int _bettingChip;
    private GambleSoData _gambleData;
    [SerializeField] private Transform point;

    public static GambleManager instance;

    private void Awake()
    {
        if(instance==null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
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
    
    public void GambleEnd()
    {

    }


}
