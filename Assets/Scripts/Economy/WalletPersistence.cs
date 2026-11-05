using UnityEngine;

public class WalletPersistence : MonoBehaviour
{
    [SerializeField] Wallet wallet;
    void Awake()
    {
        WalletSave.Load(wallet);
        // save wallet after each update
        wallet.Changed += () => { WalletSave.Save(wallet); };
    }
    
    void OnApplicationPause(bool paused) { if (paused) WalletSave.Save(wallet); }
}
