using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadScene : MonoBehaviour
{
    public string sceneName;

    public void LoadThisScene()
    {
        if (SceneLoader.Instance != null)
            SceneLoader.Instance.Load(sceneName, Application.exitCancellationToken);
        else
            SceneManager.LoadSceneAsync(sceneName);
    }
}
