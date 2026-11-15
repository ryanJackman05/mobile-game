using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_WalletHUD : MonoBehaviour
{
    public TextMeshProUGUI coinsLabel, upgradeLabel;
    public Button upgradeButton;
    void OnEnable()  { Wallet.wallet.Changed += Refresh; Refresh(); }
    void OnDisable() { Wallet.wallet.Changed -= Refresh; }
    void Refresh()
    {
        // Each part is optional, so the Game HUD can show just coins and the main menu the upgrade button.
        if (coinsLabel != null) coinsLabel.SetText("{0}", Wallet.wallet.Coins);            // no string concat
        if (upgradeLabel != null) upgradeLabel.SetText("Max health +10 ({0})", Wallet.wallet.NextCost);
        if (upgradeButton != null) upgradeButton.interactable = Wallet.wallet.Coins >= Wallet.wallet.NextCost;
    }

    public void Upgrade()
    {
        Wallet.wallet.TryUpgrade();
    }
}
