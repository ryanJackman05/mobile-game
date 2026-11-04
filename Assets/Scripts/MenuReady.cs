using UnityEngine;

public class MenuReady : MonoBehaviour
{
    private bool sed;
    
    private void Update()
    {
        if (!sed)
        {
            Debug.Log($"[perf] interactive {Time.realtimeSinceStartup:F2}s");
            sed = true;
        }
    }
}
