using System;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class DiceObject_JCY : MonoBehaviour
{
    [Header("주사위 정보")]
    public DiceSO_JCY currentDiceSO;
    public int currentIndex;
    public TrailRenderer trailRenderer;
    
    [Header("셀렉트 설정")]
    public MeshRenderer MeshCompo { get; private set; }
    [field:SerializeField] public Material OutLine { get; private set; }
    [field: SerializeField] public bool IsSelected { get; private set; }
    private Transform myDicePosition;

    [SerializeField] private Material glassSelectMaterial;

    private void OnEnable()
    {
        IsSelected = false;
        MeshCompo = GetComponentInChildren<MeshRenderer>();
        trailRenderer = GetComponent<TrailRenderer>();
        trailRenderer.enabled = false;
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

        if (currentDiceSO.diceEffectType == DiceSO_JCY.DiceEffectType.Allin ||
         (currentDiceSO.diceEffectType == DiceSO_JCY.DiceEffectType.Glass ||
         (currentDiceSO.diceEffectType == DiceSO_JCY.DiceEffectType.Potion)))
        {
            ChageMeterial(currentDiceSO);
            return;
        }
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

    public void ChageMeterial(DiceSO_JCY So)
    {
        if (So.diceEffectType == DiceSO_JCY.DiceEffectType.Allin)
        {
            Material[] mats = MeshCompo.materials;

            Material temp = mats[0];
            mats[0] = mats[1];
            mats[1] = temp;

            // 변경된 배열을 다시 할당
            MeshCompo.materials = mats;
            return;
        }

        if (So.diceEffectType == DiceSO_JCY.DiceEffectType.Glass)
        {
            Material[] mats = MeshCompo.materials;

            Material temp = mats[1];
            mats[1] = glassSelectMaterial;
            glassSelectMaterial = temp;

            // 변경된 배열을 다시 할당
            MeshCompo.materials = mats;
            return;
        }

        if (So.diceEffectType == DiceSO_JCY.DiceEffectType.Potion)
        {
            Material[] mats = MeshCompo.materials;

            Material temp = mats[0];
            mats[0] = glassSelectMaterial;
            glassSelectMaterial = temp;

            // 변경된 배열을 다시 할당
            MeshCompo.materials = mats;
            return;
        }



}
    
}