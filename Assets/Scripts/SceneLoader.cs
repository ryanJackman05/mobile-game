using System.Threading; 
using UnityEngine;
using UnityEngine.SceneManagement; 
using UnityEngine.UI;
public class SceneLoader : MonoBehaviour   // persistent, see Awake
{
    [SerializeField] Slider bar;
    [SerializeField] GameObject loadingScreen;
    public static SceneLoader Instance;

    void Awake()
    {
        DontDestroyOnLoad(loadingScreen);
        Instance = this;
    }

    public async Awaitable Load(string scene, CancellationToken ct)
    {
        loadingScreen.SetActive(true);
        var op = SceneManager.LoadSceneAsync(scene);
        op.allowSceneActivation = false;  // progress now stops at 0.9
        while (op.progress < 0.9f)        // NOT while (!op.isDone)
        {
            bar.value = op.progress / 0.9f;
            await Awaitable.NextFrameAsync(ct);
        }
        op.allowSceneActivation = true;
        await op;                         // completes on activation
        loadingScreen.SetActive(false);
    }
    
    // TODO
    // 5. Optional but recommended: while the loader holds at 0.9, prewarm your pools (Lab A) so the first frame of `Game` does
    // no `Instantiate`. One way is to expose an `Awaitable Prewarm()` on the pool and await it before `allowSceneActivation = true`.
}
