using UnityEngine;
using UnityEngine.UI;

// Put on a Slider (0..1) in a settings menu. Saves to PlayerPrefs through AudioManager.
[RequireComponent(typeof(Slider))]
public class UI_VolumeSlider : MonoBehaviour
{
    enum Channel { Music, Sfx }
    [SerializeField] Channel channel;

    Slider slider;

    void Awake()
    {
        slider = GetComponent<Slider>();
        slider.minValue = 0f;
        slider.maxValue = 1f;
    }

    void OnEnable()
    {
        slider.SetValueWithoutNotify(channel == Channel.Music ? AudioManager.MusicVolume : AudioManager.SfxVolume);
        slider.onValueChanged.AddListener(OnChanged);
    }

    void OnDisable()
    {
        slider.onValueChanged.RemoveListener(OnChanged);
        PlayerPrefs.Save(); // once when the menu closes, instead of a disk write per drag step
    }

    void OnChanged(float v)
    {
        if (channel == Channel.Music) AudioManager.MusicVolume = v;
        else AudioManager.SfxVolume = v;
        AudioManager.RefreshVolumes();
    }
}
