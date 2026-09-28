using UnityEngine;
using UnityEngine.SceneManagement;

namespace JJB.Script
{
    public class SceneMove : MonoBehaviour
    {
        [SerializeField] private string sceneName;

        public void MoveScene()
        {
            SceneManager.LoadScene(sceneName);
        }
    }
}