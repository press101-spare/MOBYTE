using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.WSA;

public class TotalManager : MonoBehaviour
{
    public int Cost {  get; private set; }
    public static TotalManager Instance { get; private set; }
    private void Awake()
    {
        Cost = 90000;
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }
    public void BuyItem(DiceSO_JCY dice)
    {
        Cost -= dice.cost;
        Debug.Log(Cost+dice.diceName);
    }
    public void BuyItem(int cost)//혹시몰라서 회복물약의 쓸거면 쓰기
    {

    }


}
