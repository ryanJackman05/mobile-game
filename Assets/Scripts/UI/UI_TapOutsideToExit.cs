using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.EnhancedTouch;
using Touch = UnityEngine.InputSystem.EnhancedTouch.Touch;
using TouchPhase = UnityEngine.InputSystem.TouchPhase;

// placed on Setting Panel object.
// TODO should appear, and should disappear when Show is called with false, OR when the player clicks somewhere that ISNT in the bounds of this object
// perhaps also should be usable on BuildStamp
public class UI_TapOutsideToExit : MonoBehaviour
{
    private RectTransform rect;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rect = GetComponent<RectTransform>();
    }
    
    void OnEnable() => EnhancedTouchSupport.Enable();
    void OnDisable() => EnhancedTouchSupport.Disable();

    // Update is called once per frame
    void Update()
    {
        foreach (var t in Touch.activeTouches)
        {
            // Check for tap
            if (t.phase != TouchPhase.Ended) 
            {
                Rect r = rect.rect;
                r.position = rect.position;
                if (!r.Contains(t.screenPosition)){ // if any tap outside of menu, disable
                    Show(false);
                    return;
                }
                // Keep skipping the rest of the loop for active touches
            }
        }
    }

    public void Show(bool show)
    {
        gameObject.SetActive(show);
    }
}
