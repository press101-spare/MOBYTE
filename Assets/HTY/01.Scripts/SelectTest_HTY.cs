
using System;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class SelectTest_HTY : MonoBehaviour
{
    public GameObject _bt;
    public enum Gamble {gam,ble }
    public Transform _playerTrans;
    private Image[] _image;
    public Material shader;
    public Dictionary<Gamble, GameObject> _gambleDic;
    public Dictionary<Gamble, GameObject> _panelDic;
    private Gamble currentEnum;

    private void Update()
    {
        foreach (var image in _gambleDic.Keys)
        {
            float a = Vector2.Distance(_gambleDic[image].gameObject.transform.position, _playerTrans.position);
            if(a<5)
            {
                _gambleDic[image].gameObject.GetComponent<Image>().material = shader;
                currentEnum = image;
                _bt.SetActive(true);
                return;
            }
            else
            {
                _gambleDic[image].gameObject.GetComponent<Image>().material = null;
            }
        }
        _bt.SetActive(false);

    }

    public void Select()
    {
        if (_bt)
        {
            _panelDic[currentEnum].SetActive(true);
        }
    }
}
