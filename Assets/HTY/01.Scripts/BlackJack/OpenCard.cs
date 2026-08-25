using UnityEngine;

public class OpenCard : MonoBehaviour
{
    private SpriteRenderer _spr;
    private Sprite _image;
    private BlackJackGame _blackJack;
    private void Awake()
    {
        _spr = GetComponent<SpriteRenderer>();
    }
    private void OnMouseDown()
    {
        _spr.sprite = _image;
    }
}
