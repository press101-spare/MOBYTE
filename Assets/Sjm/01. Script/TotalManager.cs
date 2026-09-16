using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.WSA;

public class TotalManager : MonoBehaviour
{
    [SerializeField] private DiceSO_JCY diceSO;
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
    public void BuyItem()
    {
        Cost -= diceSO.cost;
        Debug.Log(Cost);
    }


}
