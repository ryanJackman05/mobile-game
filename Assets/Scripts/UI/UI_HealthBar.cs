using TMPro;
using UnityEngine;
using UnityEngine.UI;

// Health bar for the Game HUD. Setup: a UI Image with Image Type = Filled (Horizontal) as "fill",
// optionally a TMP label. Listens to PlayerCarController.HealthChanged.
public class UI_HealthBar : MonoBehaviour
{
    [SerializeField] Image fill;
    [SerializeField] TextMeshProUGUI label;   // optional, e.g. "64/110"
    [SerializeField] Gradient colour = DefaultGradient();
    [SerializeField] float smoothing = 8f;    // how quickly the bar catches up to the real value

    PlayerCarController car;
    float target = 1f, shown = 1f;

    void Start()
    {
        car = GameManager.current.playerCar;
        car.HealthChanged += OnHealthChanged;
        OnHealthChanged(car.health, car.maxHealth);
        shown = target; // no slide-in on the first frame
        Apply();
    }

    void OnDestroy()
    {
        if (car != null) car.HealthChanged -= OnHealthChanged;
    }

    void OnHealthChanged(int health, int max)
    {
        target = max > 0 ? (float)health / max : 0f;
        if (label != null) label.SetText("${health}/${max}");
    }

    void Update()
    {
        if (Mathf.Approximately(shown, target)) return;
        shown = Mathf.MoveTowards(shown, target, smoothing * Time.unscaledDeltaTime * Mathf.Max(0.1f, Mathf.Abs(target - shown)));
        if (Mathf.Abs(shown - target) < 0.001f) shown = target;
        Apply();
    }

    void Apply()
    {
        fill.fillAmount = shown;
        fill.color = colour.Evaluate(shown);
    }

    static Gradient DefaultGradient()
    {
        var g = new Gradient();
        g.SetKeys(
            new[] { new GradientColorKey(new Color(0.85f, 0.15f, 0.1f), 0f),
                    new GradientColorKey(new Color(0.95f, 0.75f, 0.1f), 0.5f),
                    new GradientColorKey(new Color(0.3f, 0.85f, 0.25f), 1f) },
            new[] { new GradientAlphaKey(1f, 0f), new GradientAlphaKey(1f, 1f) });
        return g;
    }
}
