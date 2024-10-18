using System;
using UnityEngine;
using UnityEngine.Purchasing;

public class IapManager : MonoBehaviour, IStoreListener
{
    UiManager uiManager;
    IStoreController storeController;

    [SerializeField] EconomyManager economyManager;
    [SerializeField] private ConsumableItem oilItem;
    [SerializeField] private ConsumableItem ticketItem;
    [SerializeField] private SubscriptionItem adsSubscriptionItem;

    private void Start()
    {
        SetUpBuilder();
        uiManager = UiManager.instance;
    }

    private void SetUpBuilder ()
    {
        var builder = ConfigurationBuilder.Instance(StandardPurchasingModule.Instance());

        builder.AddProduct(oilItem.Id, ProductType.Consumable);
        builder.AddProduct(ticketItem.Id, ProductType.Consumable);
        //builder.AddProduct(adsSubscriptionItem.Id, ProductType.Subscription);

        UnityPurchasing.Initialize(this, builder);
    }

    public void OnInitialized(IStoreController controller, IExtensionProvider extensions)
    {
        Debug.Log("IAP initialized");
        storeController = controller; 
    }

    public void TicketBestValueCard1500()
    {
        uiManager.ConnetingPleaseWait(true);
        storeController.InitiatePurchase(ticketItem.Id);
    }

    public void OilBestValueCard1500()
    {
        uiManager.ConnetingPleaseWait(true);
        storeController.InitiatePurchase(oilItem.Id);
    }

    public PurchaseProcessingResult ProcessPurchase(PurchaseEventArgs purchaseEvent)
    {
        var product = purchaseEvent.purchasedProduct;
        string productID = product.definition.id;

        if (productID == oilItem.Id)
        {
            economyManager.CreditOil(100000);

            uiManager.ConnetingPleaseWait(false);
            uiManager.ThankYouForPurchase(true);
        }
        else if (productID == ticketItem.Id)
        {
            economyManager.CreditTickets(1500);

            uiManager.ConnetingPleaseWait(false);
            uiManager.ThankYouForPurchase(true);

        }

        return PurchaseProcessingResult.Complete;
    }













    public void OnInitializeFailed(InitializationFailureReason error)
    {
        throw new System.NotImplementedException();
    }

    public void OnInitializeFailed(InitializationFailureReason error, string message)
    {
        throw new System.NotImplementedException();
    }

    public void OnPurchaseFailed(Product product, PurchaseFailureReason failureReason)
    {
        uiManager.ConnetingPleaseWait(false);
        uiManager.UnableToPurchase(true);
    }

    
}

[Serializable]
public class ConsumableItem
{
    public string Name;
    public string Id;
    public string Description;
    public string Price;
}

[Serializable]
public class NonConsumableItem
{
    public string Name;
    public string Id;
    public string Description;
    public string Price;
}

[Serializable]
public class SubscriptionItem
{
    public string Name;
    public string Id;
    public string Description;
    public string Price;
    public int timeDuration;
}
