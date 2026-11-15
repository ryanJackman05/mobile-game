using UnityEngine;
using UnityEngine.UI;

// Add next to any Button (or Toggle) to play a click. No OnClick wiring needed.
public class UI_ButtonSound : MonoBehaviour
{
    [SerializeField] Sfx sound = Sfx.UiClick;

    void Awake()
    {
        if (TryGetComponent(out Button b)) b.onClick.AddListener(PlaySound);
        else if (TryGetComponent(out Toggle t)) t.onValueChanged.AddListener(_ => PlaySound());
    }

    void PlaySound() => AudioManager.Play(sound);
}
