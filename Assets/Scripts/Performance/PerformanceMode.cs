using UnityEngine;
using UnityEngine.Rendering.Universal;

public enum PerfMode {Balanced = 0, HighFPS = 1, LowFPS = 2};
public class PerformanceMode : MonoBehaviour
{
    const string Key = "perf_mode";
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start() => Apply((PerfMode)PlayerPrefs.GetInt(Key, 0));

    // Update is called once per frame
    public void Apply(PerfMode mode) // wire to a UI toggle
    {
        bool high = mode == PerfMode.HighFPS;
        Application.targetFrameRate = high ? 120 : 60;
        var urp = UniversalRenderPipeline.asset;
        if (urp != null) urp.renderScale = high ? .75f : 1f;
        
        PlayerPrefs.SetInt(Key, (int)mode);
        PlayerPrefs.Save();
        Debug.Log($"perf_mode={mode}, high={high}");
    }
    
    public void SetHighFps(bool on) =>
        Apply(on ? PerfMode.HighFPS : PerfMode.Balanced);

}
