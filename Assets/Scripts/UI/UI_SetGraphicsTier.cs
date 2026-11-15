using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_SetGraphicsTier : MonoBehaviour
{
    public void Start()
    {
        TMP_Dropdown drop = GetComponent<TMP_Dropdown>();
        drop.SetValueWithoutNotify(PlayerPrefs.GetInt("tier_override"));
    }
    public void SetTierOverride(int option)   // 0 = Auto, 1..3 = Low..High
    {
        if (option == 0) PlayerPrefs.DeleteKey("tier_override");
        else PlayerPrefs.SetInt("tier_override", option - 1);
        PlayerPrefs.Save();
        Debug.Log("Graphics Tier Level="+option);
    }
}
