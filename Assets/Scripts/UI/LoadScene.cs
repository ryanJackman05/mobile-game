using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadScene : MonoBehaviour
{
    public string sceneName;

    public void LoadThisScene()
    {
        SceneManager.LoadSceneAsync(sceneName);
    }
}
