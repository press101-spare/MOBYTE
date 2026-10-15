using JJB.Script.Battle.Player.Progression;
using UnityEngine;

public class AddMoney : MonoBehaviour
{
   
    void Start()
    {
        PlayerProfileManager.Instance.AddMoney(100);
    }

   
}
