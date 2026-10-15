using UnityEngine;

public class GamblePanelPhysicsLink : MonoBehaviour
{
    [SerializeField]
    private GameObject physicsObject;

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