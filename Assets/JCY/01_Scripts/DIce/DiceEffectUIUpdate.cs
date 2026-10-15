using System;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class DiceEffectUIUpdate : MonoBehaviour
{
   public DiceEffectSlotUI slots;

    private void Start()
    {
        DiceDeckManager_JCY.Instance.diceEffectUIUpdate = this;
    }

  

    //public void ShowUi()
    //{ 
    //    if (gameObject.activeInHierarchy)
    //    {
    //        gameObject.SetActive(false);
    //    }
    //    else
    //    {
    //        gameObject.SetActive(true);
    //    }
    //}
}
