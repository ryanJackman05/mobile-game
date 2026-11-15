using UnityEngine;

public class Haptics : MonoBehaviour
{
    const string Key = "haptics";

    public static bool Enabled
    {
        get => PlayerPrefs.GetInt(Key, 1) == 1;
        set { PlayerPrefs.SetInt(Key, value ? 1 : 0); PlayerPrefs.Save(); } // value (bool): passed into Enabled setter when called as a method
    }

    public static void Pulse() // used on: zombie landing/hit/scraped off, car scrape, crash, repair pickup. (Jumps are on the cut list.)
    {
        if (!Enabled) return;
        Handheld.Vibrate();   // one fixed pulse; no duration or intensity control
    }
}