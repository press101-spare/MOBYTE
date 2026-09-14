using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "GamebleSoData", menuName = "Gameble/GamebleSoData")]
public class GambleSoData : ScriptableObject
{
    public string _gambleName;//이름
    public Sprite _icon;//테이블 이미지
    public GameObject _gambleTable;//테이블
    public GameObject _gambleObject;//화면 녹화용
}
