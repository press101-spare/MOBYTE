using UnityEngine;
using UnityEngine.UI;

public class GambleTable_HTY : MonoBehaviour
{
    public GamebleSoData _myGamble;
    [Header("OutLine")]
    public Material _material;
    [SerializeField] private Transform _playerTrans;
    [SerializeField] private float _range;
    private Image _myImage;
    public float _rangeToPlayer;

    private void Awake()
    {
        _myImage = GetComponent<Image>();
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
            _myImage.material = null;
        }
    }
}
