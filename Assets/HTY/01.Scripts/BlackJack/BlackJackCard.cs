using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.InputSystem;

public class BlackJackCard : MonoBehaviour
{
    public int _myNumber;
    public Sprite _myImage;
    private SpriteRenderer _myRenderer;
    public int _myId;

    private void Awake()
    {
        _myRenderer = GetComponent<SpriteRenderer>();
    }
    public void OpenCard()
    {

    }
}
