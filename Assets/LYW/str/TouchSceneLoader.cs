using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class TouchSceneLoader : MonoBehaviour
{
    [SerializeField] private string nextScene;
    
    private bool isLoading;

    private void OnMouseDown()
    {
        if (isLoading)
            return;
        isLoading = true;
        SceneManager.LoadScene(nextScene);
    }
}