using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneChange : MonoBehaviour
{
    public void ChangeSceneTo3()
    {
        SceneManager.LoadScene(3);

    }
    public void ChangeSceneTo2()
    {
        SceneManager.LoadScene(2);
    } 
    public void ChangeSceneTo4()
    {
        SceneManager.LoadScene(4);
    }
    public void ChangeSceneToboss1()
    {
        SceneManager.LoadScene(5);
    }
    public void ChangeSceneToboss2()
    {
        SceneManager.LoadScene(6);
    }
    public void ChangeSceneToboss3()
    {
        SceneManager.LoadScene(7);
    }
}
