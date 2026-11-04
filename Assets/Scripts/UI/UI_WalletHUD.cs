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
        coinsLabel.SetText("{0}", Wallet.wallet.Coins);            // no string concat
        upgradeLabel.SetText("Upgrade ({0})", Wallet.wallet.NextCost);
        upgradeButton.interactable = Wallet.wallet.Coins >= Wallet.wallet.NextCost;
    }
}
