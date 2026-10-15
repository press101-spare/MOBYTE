using UnityEngine;

public class PhysiceObj : MonoBehaviour
{
    [SerializeField] private GameObject _physiObj;
    private void OnEnable()
    {
        _physiObj.SetActive(true);
    }
    private void OnDisable()
    {
        _physiObj.SetActive(false);
    }
}
