using UnityEngine;

namespace JJB.Script
{
    public class PanelController : MonoBehaviour
    {
        [SerializeField] private GameObject[] panels;

        public void OpenPanel(GameObject targetPanel)
        {
            foreach (GameObject panel in panels)
            {
                panel.SetActive(panel == targetPanel);
            }
        }
    }
}