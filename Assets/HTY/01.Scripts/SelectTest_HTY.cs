
using System;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UI;

public class SelectTest_HTY : MonoBehaviour
{
    public GamebleSoList data;
    public Transform _playerTrans;
    private Image[] _image;
    public Material shader;
    //public Dictionary<GamebleSoData,GameObject>

    /*private void Update()
    {
        foreach (var image in data.gameble)
        {
            float a = Vector2.Distance(image.transform.position, _playerTrans.position);
            if(a<5)
            {
                image.material = shader;
            }
            else
            {
                image.material = null;
            }
        }
    }*/

    public void Select()
    {

    }
}
