
using System;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class SelectTest_HTY : MonoBehaviour
{
    public GambleSoList _gameList;
    [SerializeField] private GambleTable_HTY[] _scripts;//나중에 수정 자동화에 어려움
    private List<GambleTable_HTY> a = new List<GambleTable_HTY>();
    private float _currnetMin;
    private GambleTable_HTY _currnetTable;

    public void Select()
    {
        _currnetMin = 10;
        foreach (var script in _scripts)
        {
            if(script._rangeToPlayer < 5)
            {
                if(script._rangeToPlayer<_currnetMin)
                {
                    _currnetMin= script._rangeToPlayer;
                    _currnetTable = script;
                }
            }
        }
        if (_currnetTable == null) return;

        Instantiate(_currnetTable._myGamble._panel);
        Instantiate(_currnetTable._myGamble._object);
        

    }
}
