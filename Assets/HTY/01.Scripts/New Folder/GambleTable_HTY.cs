using UnityEngine;
using UnityEngine.UI;

public class GambleTable_HTY : MonoBehaviour
{
    public GambleSoData _myGamble;
    [Header("OutLine")]
    public Material _materialO;
    public Material _material2;
    private Transform _playerTrans;
    public float _range = 3f;
    private SpriteRenderer _myImage;
    public float _rangeToPlayer;

    private void Awake()
    {
        _myImage = GetComponent<SpriteRenderer>();
        _playerTrans = FindAnyObjectByType<PlayerMovement_H>().transform;
    }
    private void Update()
    {
        _rangeToPlayer = Vector2.Distance(transform.position, _playerTrans.position);
        if (_rangeToPlayer < _range)
        {
            _myImage.material = _materialO;
        }
        else
        {
            _myImage.material = _material2;
        }
    }
}
