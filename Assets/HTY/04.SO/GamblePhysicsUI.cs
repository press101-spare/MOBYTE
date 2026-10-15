using UnityEngine;

public class GamblePhysicsUI : MonoBehaviour
{
    [SerializeField]
    private GameObject physicsObject;

    private static Transform _physicsRoot;

    private void Awake()
    {
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

        if (physicsObject != null)
        {
            // 위치 그대로 유지한 채 Canvas 밖으로 이동
            physicsObject.transform.SetParent(_physicsRoot, true);
        }
    }

    private void OnEnable()
    {
        if (physicsObject != null)
        {
            physicsObject.SetActive(true);
        }
    }

    private void OnDisable()
    {
        if (physicsObject != null)
        {
            physicsObject.SetActive(false);
        }
    }
}