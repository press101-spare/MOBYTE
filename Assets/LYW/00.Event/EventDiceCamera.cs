using DG.Tweening;
using UnityEngine;

public class EventDiceCamera : MonoBehaviour
{
    [SerializeField] private Transform _camera;
    [SerializeField] private Transform _battlePoint;
    [SerializeField] private Transform _dicePoint;
    public float rotateDuration;
    public float moveDuration;

    public void BattleCameraMove()
    {
        if (_camera == null || _battlePoint == null)
        {
            Debug.LogError("BattleCameraMove : Camera 또는 BattlePoint가 NULL임");
            return;
        }

        Sequence seq = DOTween.Sequence(); 
        seq.Join(_camera.DOMoveZ(_battlePoint.position.z , moveDuration));
        seq.Join(_camera.DORotate(_battlePoint.rotation.eulerAngles, rotateDuration));
        //  seq.Append(_camera.DOMoveY(_battlePoint.position.y , moveDuration));
    }
   
    public void DiceCameraMove()
    {
      
        if (_camera == null || _dicePoint == null)
        {
            Debug.LogError("DiceCameraMove : Camera 또는 DicePoint가 NULL임");
            return;
        }
        Sequence seq = DOTween.Sequence();
        seq.Join(_camera.DOMoveZ(_dicePoint.position.z , moveDuration));
        seq.Join(_camera.DORotate(_dicePoint.rotation.eulerAngles, rotateDuration));
    }
}
