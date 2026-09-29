using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using DG.Tweening;
using UnityEngine.SceneManagement;

public class SelectTest_HTY : MonoBehaviour
{
    public GambleSoList _gameList;
    [SerializeField] private List<GambleTable_HTY> _scripts = new List<GambleTable_HTY>();//나중에 수정 자동화에 어려움
    private float _currnetMin;
    private GambleTable_HTY _currnetTable;

    [SerializeField] private GameObject _bettingUI;
    private GambleUI_HTY[] _uiList;

    [SerializeField] private Transform _gambleCC;
    [SerializeField] private GameObject _selectBT;


    [SerializeField] private GameObject _selectPanel;



    [SerializeField] private GameObject _bacara;
    [SerializeField] private GameObject _loto;
    [SerializeField] private GameObject _slot;

    [Header("씬 이동")]
    [SerializeField] private CanvasGroup _fadePanel;
    [SerializeField] private string _elevatorSceneName = "Elevator_HTY";
    [SerializeField] private float _fadeDuration = 0.5f;
    
    private bool _isSceneMoving;



    private void Start()
    {
        
        StartCoroutine(Coll());
    }

    private IEnumerator Coll()
    {
        yield return new WaitForSeconds(0.5f);
        _scripts = FindObjectsByType<GambleTable_HTY>(FindObjectsSortMode.None).ToList();
        _uiList = _bettingUI.transform.GetComponentsInChildren<GambleUI_HTY>(true);
    }

    private void Update()
    {
        foreach (var script in _scripts)
        {
            if (script._rangeToPlayer < script._range)
            {
                _selectBT.SetActive(true);
                return;
            }
        }
        _selectBT.SetActive(false);
    }
    public void SelectBT()
    {
        _currnetMin = 10;
        _currnetTable = null;
        foreach (var script in _scripts)
        {
            if (script._rangeToPlayer < script._range)
            {
                if (script._rangeToPlayer<_currnetMin)
                {
                    _currnetMin= script._rangeToPlayer;
                    _currnetTable = script;
                }
            }
        }
        if (_currnetTable == null) return;

        switch (_currnetTable._myGamble._gambleName)
        {
            case GambleType.Shop:
                _selectPanel.SetActive(true);
                break;
            case GambleType.PinBall:
            case GambleType.BlackJack:
            case GambleType.Roulette:
            case GambleType.SellGame:
            case GambleType.Baccarat:
                _bettingUI.SetActive(true);
                _bettingUI.GetComponent<Betting_HTY>()._currentGamble = _currnetTable._myGamble;
                break;
            case GambleType.Loto:
                _loto.gameObject.SetActive(true);
                _loto.transform.GetChild(0).gameObject.SetActive(true);
                _loto.transform.GetChild(1).gameObject.SetActive(false);
                break;
            case GambleType.SlotGame:
                _slot.SetActive(true);
                break;
            case GambleType.Elevator:
                MoveToElevator();
                break;
        }
    }
    
    private void MoveToElevator()
    {
        if (_isSceneMoving)
            return;

        _isSceneMoving = true;

        if (_selectBT != null)
            _selectBT.SetActive(false);

        // 씬 이동 직전에 현재 데이터 저장
        DataManager dataManager =
            FindFirstObjectByType<DataManager>();

        if (dataManager != null)
            dataManager.SaveGame();

        // FadePanel이 없으면 바로 이동
        if (_fadePanel == null)
        {
            SceneManager.LoadScene(_elevatorSceneName);
            return;
        }

        _fadePanel.blocksRaycasts = true;
        _fadePanel.DOKill();

        _fadePanel
            .DOFade(1f, _fadeDuration)
            .SetEase(Ease.InOutSine)
            .OnComplete(() =>
            {
                SceneManager.LoadScene(_elevatorSceneName);
            });
    }

    private void OnDestroy()
    {
        if (_fadePanel != null)
            _fadePanel.DOKill();
    }
}
