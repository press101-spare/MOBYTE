using UnityEngine;

public class ShowUI : MonoBehaviour
{
    public void ShowUi()
    { 
        if (gameObject.activeInHierarchy)
        {
            gameObject.SetActive(false);
        }
        else
        {
            gameObject.SetActive(true);
        }
    }
}
