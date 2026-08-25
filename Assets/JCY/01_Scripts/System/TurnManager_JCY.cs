using UnityEngine;

public class TurnManager_JCY : MonoBehaviour
{
        public static TurnManager_JCY Instance;
        public CameraMove_JCY CM;
        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
                DontDestroyOnLoad(gameObject); // 씬이 넘어가도 파괴되지 않음
            }
            else
            {
                Destroy(gameObject); // 중복 생성 방지
            }
        }

        public void StartTurn()
        {
            DiceDeckManager_JCY.Instance.DrawDice();
            CM.HighAngleCamera();
        }
}
