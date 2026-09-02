using JetBrains.Annotations;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class BlackJackCard : MonoBehaviour
{
    public int _myNumber;
    public Sprite _myImage;
    private SpriteRenderer _myRenderer;
    public int _myId;
    public bool _iamCheck;

    private void Awake()
    {
        _myRenderer = GetComponent<SpriteRenderer>();
    }
    public void OpenCard()
    {
        _myRenderer.sprite=_myImage;
        Debug.Log("내숫자" +_myNumber);
        _iamCheck = true;
    }
}
