
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

    [SerializeField] private GameObject _gambleUICanvas;
    private GambleUI_HTY[] _uiList;

    [SerializeField] private Transform _gambleCC;
    [SerializeField] private GameObject _selectBT;


    [SerializeField] private GameObject _selectPanel;


    private void Start()
    {
        _scripts = FindObjectsByType<GambleTable_HTY>(FindObjectsSortMode.None).ToList();
        _uiList = _gambleUICanvas.transform.GetComponentsInChildren<GambleUI_HTY>(true);
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
                Debug.Log(script._rangeToPlayer);
                if (script._rangeToPlayer<_currnetMin)
                {
                    _currnetMin= script._rangeToPlayer;
                    _currnetTable = script;
                }
            }
        }
        if (_currnetTable == null) return;

        if (_currnetTable._myGamble._gambleName == "Shop")
        {
            _selectPanel.SetActive(true);
        }




        _gambleUICanvas.SetActive(true);

        foreach (var v in _uiList)
        {
            if(v._myGamble==_currnetTable._myGamble)
            {
                v.gameObject.SetActive(true);
                GameObject a = Instantiate(v._myGamble._gambleObject);
                a.SetActive(true);
                a.transform.position = _gambleCC.position;
            }
        }
    }
}
