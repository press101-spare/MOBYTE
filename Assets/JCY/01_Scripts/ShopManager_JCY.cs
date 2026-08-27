using System;
using TMPro;
using UnityEngine;
using Random = UnityEngine.Random;

public class ShopManager_JCY : MonoBehaviour
{
    public TextMeshProUGUI txt;
    public DiceSO_JCY[] so;

    private void Start()
    {
        int i = Random.Range(0, so.Length);
        txt.text = so[i].diceName;
    }
}
