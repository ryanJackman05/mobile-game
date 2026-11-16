using UnityEngine;

// Best distance across runs, saved in PlayerPrefs (same approach as WalletSave).
public static class HighScore
{
    const string BestKey = "highscore.distance";

    public static float Best => PlayerPrefs.GetFloat(BestKey, 0f);

    // Returns true if this run beat the previous best (and saves it).
    public static bool Submit(float distance)
    {
        if (distance <= Best) return false;
        PlayerPrefs.SetFloat(BestKey, distance);
        PlayerPrefs.Save(); // flush to disk now, the player may close the app on the results screen
        return true;
    }
}
