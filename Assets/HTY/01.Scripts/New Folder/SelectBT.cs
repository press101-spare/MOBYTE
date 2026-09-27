using UnityEngine;

public class SelectBT : MonoBehaviour
{
    public GameObject[] gambles;

    private void Update()
    {
        foreach (var gamble in gambles)
        {
            gameObject.SetActive(false);
        }
    }
}
