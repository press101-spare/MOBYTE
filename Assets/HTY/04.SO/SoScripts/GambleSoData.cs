using UnityEngine;
using UnityEngine.UI;

public enum GambleType
{
    Baccarat,
    BlackJack,
    Loto,
    PinBall,
    Roulette,
    SellGame,
    SlotGame,
    Shop
}

[CreateAssetMenu(fileName = "GamebleSoData", menuName = "Gameble/GamebleSoData")]
public class GambleSoData : ScriptableObject
{
    public GambleType _gambleName;//이름
    public GameObject _gambleTable;//테이블맵
    [TextArea] public string _chipXtext;
    public int _canBettingChip;
}
