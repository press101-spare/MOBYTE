
using JJB.Script.Battle.Player.Progression;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using UnityEditor.Experimental.GraphView;
using UnityEditor.Rendering;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

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

    

    public bool _canSelect =true;


    private void Start()
    {
        _scripts = FindObjectsByType<GambleTable_HTY>(FindObjectsSortMode.None).ToList();
        _uiList = _bettingUI.transform.GetComponentsInChildren<GambleUI_HTY>(true);
    }
    private void Update()
    {
        if (!_canSelect)
        { 
            _selectBT.SetActive(false);
            return; 
        }

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
        if (!_canSelect) return;
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
                _canSelect = false;
                break;
            case GambleType.PinBall:
            case GambleType.BlackJack:
            case GambleType.Roulette:
            case GambleType.SellGame:
            case GambleType.Baccarat:
                _bettingUI.SetActive(true);
                _bettingUI.GetComponent<Betting_HTY>()._currentGamble = _currnetTable._myGamble;
                _canSelect = false;
                break;
            case GambleType.Loto:
                _loto.gameObject.SetActive(true);
                _loto.transform.GetChild(0).gameObject.SetActive(true);
                _loto.transform.GetChild(1).gameObject.SetActive(false);
                _canSelect = false;
                break;
            case GambleType.SlotGame:
                _slot.SetActive(true);
                _canSelect = false;
                break;
        }
    }

    public void ExitGame()
    {
        _canSelect = true;
    }
}
