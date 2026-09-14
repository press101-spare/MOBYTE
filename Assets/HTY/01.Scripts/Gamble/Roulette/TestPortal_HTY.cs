using UnityEngine;
using UnityEngine.SceneManagement;

public class TestPortal_HTY : MonoBehaviour
{
    
    [SerializeField] private string _wantScene;

    [ContextMenu("포탈 기능")]
    public void NextScene()
    {
        if (_wantScene != null)
        {
            SceneManager.LoadScene(_wantScene);
        }
    }
}
