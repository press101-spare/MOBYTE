using UnityEngine;

public class DetachPhysicsFromUI : MonoBehaviour
{
    private static Transform _physicsRoot;

    private void Awake()
    {
        // 물리 오브젝트들을 모아놓을 부모를 자동 생성
        if (_physicsRoot == null)
        {
            GameObject rootObject = GameObject.Find("GamblePhysicsRoot");

            if (rootObject == null)
            {
                rootObject = new GameObject("GamblePhysicsRoot");
            }

            _physicsRoot = rootObject.transform;

            _physicsRoot.position = Vector3.zero;
            _physicsRoot.rotation = Quaternion.identity;
            _physicsRoot.localScale = Vector3.one;
        }

        // 현재 월드 위치/회전/크기를 유지하면서
        // UI 부모에서 빠져나옴
        transform.SetParent(_physicsRoot, true);
    }
}