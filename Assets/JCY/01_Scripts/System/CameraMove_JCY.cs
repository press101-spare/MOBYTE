using DG.Tweening;
using UnityEngine;

public class CameraMove_JCY : MonoBehaviour
{
   [SerializeField] private Vector3 highAngleTf;
   [SerializeField] private Vector3 highAngleRo;
   [SerializeField] private float moveTime = 0.2f;
   [SerializeField] private Vector3 currentCameraTf;
   [SerializeField] private Vector3 currentCameraRo;
   
   public void HighAngleCamera()
   {
      currentCameraTf = transform.position;
      currentCameraRo = transform.rotation.eulerAngles;
   }

   public void currenCamaerMove()
   {
      transform.DOMove(currentCameraTf , moveTime );
   }
}
