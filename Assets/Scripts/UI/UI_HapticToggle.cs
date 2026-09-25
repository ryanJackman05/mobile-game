using TMPro;
using UnityEngine;

public class UI_HapticToggle : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI buttonText;
    public void Toggle()
    {
        Haptics.Enabled = !Haptics.Enabled;

        buttonText.text = "HAPTICS: "+(Haptics.Enabled?"ON":"OFF");
    }
}
