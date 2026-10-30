using UnityEngine;

public class UI_ResultsMenu : MonoBehaviour
{
    [SerializeField] GameObject panel;

    void OnEnable()  { GameManager.current.StateChanged += Show; Show(GameManager.current.state); }  // at startup, add Show as a listener to PauseChanged
    // IMPORTANT NOTE: Events can only be linked like this when the subscribing method has the same argument signature/type as the Action calling it
    void OnDisable() { GameManager.current.StateChanged -= Show; }

    void Show(GameManager.GameState state)
    {
        if(state == GameManager.GameState.GameOver)
            panel.SetActive(true);
        else
            panel.SetActive(false);
    }
    
    public void OnRestartPressed() => GameManager.current.NewGame();
}
