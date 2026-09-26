using JJB.Script.Battle.Player.Progression;
using UnityEngine;

public class ShopChipUI : MonoBehaviour
{
    private void OnEnable()
    {
        GambleManager.instance._shopChipText.text = PlayerProfileManager.Instance.Profile.money.ToString();
    }
}
