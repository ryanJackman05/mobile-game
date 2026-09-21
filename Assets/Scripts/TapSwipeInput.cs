using UnityEngine;
using UnityEngine.InputSystem.EnhancedTouch;
using Touch = UnityEngine.InputSystem.EnhancedTouch.Touch;
using TouchPhase = UnityEngine.InputSystem.TouchPhase;

public class TapSwipeInput : MonoBehaviour
{
    public float swipeDp = 50f;     // distance in dp, not pixels
    public float tapMax = 0.3f;     // seconds
    public float holdMinTime = 0.1f; // Minimum time required for a long press

    void OnEnable() => EnhancedTouchSupport.Enable();
    void OnDisable() => EnhancedTouchSupport.Disable();

    void Update()
    {
        if (LifecycleGuard.IsPaused) return; // PAUSE MENU CHECK
        
        foreach (var t in Touch.activeTouches)
        {
            // Check for an ongoing hold
            if (t.phase != TouchPhase.Ended) 
            {
                // If it's been held long enough
                if (t.time - t.startTime >= holdMinTime)
                {
                    Debug.Log("Hold at " + t.screenPosition);
                }
                continue; // Keep skipping the rest of the loop for active touches
            }
            
            float px = swipeDp * Mathf.Max(Screen.dpi, 160f) / 160f;
            Vector2 d = t.screenPosition - t.startScreenPosition;
            
            if (d.magnitude >= px) Debug.Log("Swiped " + d.normalized); // TODO - do I need to remove swipe detection for end-of-hold? (Refer to Lab 2A)
            else if (t.time - t.startTime < tapMax) Debug.Log("Tap");
        }
    }
}