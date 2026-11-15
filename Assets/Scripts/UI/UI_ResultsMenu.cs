using TMPro;
using UnityEngine;

public class UI_ResultsMenu : MonoBehaviour
{
    [SerializeField] GameObject panel;
    [SerializeField] TextMeshProUGUI causeLabel;    // optional: "Overrun by zombies"
    [SerializeField] TextMeshProUGUI distanceLabel; // optional: "Distance 1234"
    [SerializeField] float showDelay = 1.2f;        // let the crash/stall play out before the panel covers it
    // TODO death feels off in play: either add feedback during this delay (sound, camera shake, cause flash) or set it to 0. See docs/priorities.md P1.

    float showTimer = -1f; // counts down to showing the panel; < 0 = nothing pending

    void Start()  { GameManager.current.StateChanged += Show; Show(GameManager.GameState.Playing); }  // at startup, add Show as a listener to PauseChanged
    // IMPORTANT NOTE: Events can only be linked like this when the subscribing method has the same argument signature/type as the Action calling it
    void OnDisable() { GameManager.current.StateChanged -= Show; }

    void Show(GameManager.GameState state)
    {
        if(state == GameManager.GameState.GameOver)
            showTimer = showDelay;
        else
        {
            showTimer = -1f; // restarted before the panel appeared
            panel.SetActive(false);
        }
    }

    void Update()
    {
        if (showTimer < 0f) return;
        showTimer -= Time.unscaledDeltaTime; // unscaled: still counts if the game is paused meanwhile
        if (showTimer > 0f) return;

        showTimer = -1f;
        if (causeLabel != null) causeLabel.SetText(CauseText(GameManager.current.LastCause));
        if (distanceLabel != null) distanceLabel.SetText("Distance {0:0}", (float)GameManager.current.Distance);
        panel.SetActive(true);
    }

    static string CauseText(GameManager.DeathCause cause) => cause switch
    {
        GameManager.DeathCause.Crashed     => "Crashed",
        GameManager.DeathCause.Flipped     => "Flipped",
        GameManager.DeathCause.OutOfBounds => "Off the road",
        GameManager.DeathCause.Overrun     => "Overrun by zombies",
        _                                  => "Game over",
    };

    public void OnRestartPressed() => GameManager.current.NewGame();
}
