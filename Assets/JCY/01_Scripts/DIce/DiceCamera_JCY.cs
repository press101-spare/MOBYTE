using DG.Tweening;
using UnityEngine;

public class DiceCamera_JCY : MonoBehaviour
{
   [SerializeField] private Transform _camera;
   [SerializeField] private Transform _battlePoint;
   [SerializeField] private Transform _dicePoint;
   public float rotateDuration;
   public float moveDuration;

   public void BattleCameraMove()
   {
      Sequence seq = DOTween.Sequence(); 
      seq.Join(_camera.DOMoveZ(_battlePoint.position.z , moveDuration));
      seq.Join(_camera.DORotate(_battlePoint.rotation.eulerAngles, rotateDuration));
    //  seq.Append(_camera.DOMoveY(_battlePoint.position.y , moveDuration));
   }
   
   public void DiceCameraMove()
   {
      Sequence seq = DOTween.Sequence();
      seq.Join(_camera.DOMoveZ(_dicePoint.position.z , moveDuration));
      seq.Join(_camera.DORotate(_dicePoint.rotation.eulerAngles, rotateDuration));
   }
}
