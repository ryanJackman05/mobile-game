using System.Threading;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Bootstrap : MonoBehaviour
{
    void Awake()
    {
        Application.targetFrameRate = 60;
        DontDestroyOnLoad(gameObject);
    }
    async Awaitable Start() => await GetComponent<SceneLoader>().Load("MainMenu", CancellationToken.None);
}

