using System.Threading;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Bootstrap : MonoBehaviour
{
    void Awake()
    {
        Application.targetFrameRate = 60;
        DontDestroyOnLoad(gameObject);
        
        Telemetry.Log("session_start",
            ("app_version", Application.version),
            ("device_model", SystemInfo.deviceModel),
            ("android", SystemInfo.operatingSystem),
            ("version",Application.version),
            ("build", Application.buildGUID));
    }
    async Awaitable Start() => await GetComponent<SceneLoader>().Load("MainMenu",  Application.exitCancellationToken);
}

