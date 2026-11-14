using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem.EnhancedTouch;
using Touch = UnityEngine.InputSystem.EnhancedTouch.Touch;
using TouchPhase = UnityEngine.InputSystem.TouchPhase;

// Hides this panel when a new touch begins outside it AND outside all of its children.
// Used on Settings Panel and BuildLabel (child of Settings Panel), so menus chain correctly.
public class UI_TapOutsideToExit : MonoBehaviour
{
    private RectTransform rect;
    private Camera uiCam; // null for Screen Space - Overlay
    private static readonly List<RaycastResult> hits = new();

    void Awake()
    {
        rect = GetComponent<RectTransform>();
        var canvas = GetComponentInParent<Canvas>().rootCanvas;
        uiCam = canvas.renderMode == RenderMode.ScreenSpaceOverlay ? null : canvas.worldCamera;
    }

    void OnEnable() => EnhancedTouchSupport.Enable();
    void OnDisable() => EnhancedTouchSupport.Disable();

    void Update()
    {
        foreach (var t in Touch.activeTouches)
        {
            if (t.phase != TouchPhase.Began) continue; // only react to new taps

            if (!IsInside(t.screenPosition))
            {
                Show(false);
                return;
            }
        }
    }

    bool IsInside(Vector2 screenPos)
    {
        // 1. Inside this panel's own rectangle
        if (RectTransformUtility.RectangleContainsScreenPoint(rect, screenPos, uiCam))
            return true;

        // 2. On any UI element that belongs to this panel (children placed outside its bounds, nested panels)
        var es = EventSystem.current;
        if (es == null) return false;

        hits.Clear();
        es.RaycastAll(new PointerEventData(es) { position = screenPos }, hits);
        foreach (var h in hits)
            if (h.gameObject.transform.IsChildOf(transform)) // IsChildOf includes itself
                return true;

        return false;
    }

    public void Show(bool show) => gameObject.SetActive(show);
}
