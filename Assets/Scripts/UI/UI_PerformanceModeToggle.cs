using UnityEngine;
using UnityEngine.UI;

public class UI_PerformanceModeToggle : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    PerformanceMode pMode;
    [SerializeField] Toggle toggle;
    void Start()
    {
        pMode = GameObject.Find("App").GetComponent<PerformanceMode>();
        toggle.SetIsOnWithoutNotify(PlayerPrefs.GetInt("perf_mode", 0) == 1);
    }

    // Update is called once per frame
    public void Toggle(bool on)
    {
        if (pMode == null){
            pMode = GameObject.Find("App").GetComponent<PerformanceMode>();
        }
        if (pMode != null){
            pMode.SetHighFps(on);
        }
        else{
            // TODO feedback?
        }
    }
}
