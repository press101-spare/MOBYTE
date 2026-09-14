using UnityEngine;
using UnityEngine.UI;

public class GambleTable_HTY : MonoBehaviour
{
    public GambleSoData _myGamble;
    [Header("OutLine")]
    public Material _material;
    public Material _material2;
    private Transform _playerTrans;
    public float _range = 3f;
    private SpriteRenderer _myImage;
    public float _rangeToPlayer;

    private void Awake()
    {
        _myImage = GetComponent<SpriteRenderer>();
        _playerTrans = FindAnyObjectByType<PlayerMovement_HTY>().transform;//나중에 사용하는 이동스크립트로 바꾸기
    }
    private void Start()
    {
        _myImage.sprite = _myGamble._icon;
    }
    private void Update()
    {
        _rangeToPlayer = Vector2.Distance(transform.position, _playerTrans.position);
        if (_rangeToPlayer < _range)
        {
            _myImage.material = _material;
        }
        else
        {
            _myImage.material = _material2;
        }
    }
}
