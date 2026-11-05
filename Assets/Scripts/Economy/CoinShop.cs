using UnityEngine;

public class CoinShop : MonoBehaviour
{
    [SerializeField] Wallet wallet;
    IPurchaseProvider store = new FakePurchaseProvider();

    public async void OnBuyPack()   // Button handler: async void ok
    {
        if (await store.PurchaseAsync("coins_500")) wallet.Earn(500);
    }
}