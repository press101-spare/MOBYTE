using System;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class DiceObject_JCY : MonoBehaviour
{
    [Header("주사위 정보")]
    public DiceSO_JCY currentDiceSO;
    public int currentIndex;
    
    [Header("셀렉트 설정")]
    public MeshRenderer MeshCompo { get; private set; }
    [field:SerializeField] public Material OutLine { get; private set; }
    [field: SerializeField] public bool IsSelected { get; private set; }
    private Transform myDicePosition;

    private void OnEnable()
    {
        IsSelected = false;
        MeshCompo = GetComponentInChildren<MeshRenderer>();
    }

    public void Setup(DiceSO_JCY diceSO)
    {
        currentDiceSO = diceSO;
    }

    public void OnMouseDown()
    {
        if (DiceManager_JCY.Instance.isRolling)
            return;
        SetSelected(!IsSelected);
    }
    
    public void SetSelected(bool value)
    {
        if (IsSelected == value)
            return;

        IsSelected = value;

        if (IsSelected)
        {
            AddOutline();
        }
        else
        {
            RemoveOutline();
        }
    }
    
    private void AddOutline()
    {
        if(currentDiceSO.diceEffectType == DiceSO_JCY.DiceEffectType.Rock ||
           currentDiceSO.diceEffectType == DiceSO_JCY.DiceEffectType.ShieldTurn)
            return;
        Material[] currentMaterials = MeshCompo.materials;

        Material[] newMaterials = new Material[currentMaterials.Length + 1];

        for (int i = 0; i < currentMaterials.Length; i++)
        {
            newMaterials[i] = currentMaterials[i];
        }

        newMaterials[currentMaterials.Length] = OutLine;

        MeshCompo.materials = newMaterials;
    }
    
    public void RemoveOutline()
    {
        Material[] currentMaterials = MeshCompo.materials;

        if (currentMaterials.Length <= 1)
            return;

        Material[] newMaterials = new Material[currentMaterials.Length - 1];

        for (int i = 0; i < newMaterials.Length; i++)
        {
            newMaterials[i] = currentMaterials[i];
        }

        MeshCompo.materials = newMaterials;
    }
    
    public void SetDicePosition(Transform position)
    {
        myDicePosition = position;
    }

    public Transform GetDicePosition()
    {
        return myDicePosition;
    }
    
}