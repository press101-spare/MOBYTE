using System.Collections.Generic;
using UnityEngine;

public class DetectGamble_HTY : MonoBehaviour
{
    [SerializeField] private float _detectRange;
    [SerializeField] private GameObject _selectPanel;
    [SerializeField] private GameObject _cctvCam;


    public void DetectObject()
    {
        Collider2D[] collders = Physics2D.OverlapCircleAll(transform.position,_detectRange);

        float a = 10;
        GambleTable_HTY nearTable =null;
        foreach (var v in collders)
        {
            if(v.TryGetComponent<GambleTable_HTY>(out GambleTable_HTY table))
            {
                if (table._rangeToPlayer<a)
                {
                    a = table._rangeToPlayer;
                    nearTable = table;
                }
            }
        }
        if (nearTable == null) return;
        if (a > 5) return;

        if (nearTable._myGamble._gambleName == GambleType.Shop)
        {
            _selectPanel.SetActive(true);
        }
        else
        {
            Instantiate(nearTable._myGamble._gambleObject);
            _cctvCam.SetActive(false);
        }
    }
}
