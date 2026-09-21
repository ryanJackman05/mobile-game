using UnityEngine;
using UnityEngine.InputSystem;

public class PauseMenu : MonoBehaviour
{
    [SerializeField] GameObject panel;
    [SerializeField] LifecycleGuard guard;

    void OnEnable()  { LifecycleGuard.PausedChanged += Show; Show(LifecycleGuard.IsPaused); }  // at startup, add Show as a listener to PauseChanged
    // IMPORTANT NOTE: Events can only be linked like this when the subscribing method has the same argument signature/type as the Action calling it
    void OnDisable() { LifecycleGuard.PausedChanged -= Show; }
    void Show(bool paused) => panel.SetActive(paused);

    void Update()
    {
        var kb = Keyboard.current;
        if (kb == null || !kb.escapeKey.wasPressedThisFrame) return;
        guard.SetPaused(!LifecycleGuard.IsPaused);
    }

    public void OnResumePressed() => guard.SetPaused(false);
    
    // TODO - In menus outside play, back should go up one level; a MenuStack with Push and Pop is enough, and it is on your cuts list if you run out of time.
}