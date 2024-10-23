using System;
using TMPro;
using UnityEngine;
using UnityEngine.Purchasing;

public class IapManager : MonoBehaviour, IStoreListener
{
    UiManager uiManager;
    IStoreController storeController;

    [SerializeField] private EconomyManager economyManager;
    [SerializeField] private IapData iapData;

    //---consumable
    [SerializeField] private TMP_Text basicCardPriceText;
    [SerializeField] private TMP_Text valueCardPriceText;
    [SerializeField] private TMP_Text premiumCardPriceText;
    [SerializeField] private TMP_Text bestDealCardPriceText;

    //---subscription
    [SerializeField] private TMP_Text adsSubscriptionPriceText;

    private void Start()
    {
        SetUpBuilder();
        uiManager = UiManager.instance;
    }

    private void SetUpBuilder ()
    {
        var builder = ConfigurationBuilder.Instance(StandardPurchasingModule.Instance());

        builder.AddProduct(iapData.basicTicketCard.Id, ProductType.Consumable);
        builder.AddProduct(iapData.valueTicketCard.Id, ProductType.Consumable);
        builder.AddProduct(iapData.premiumTicketCard.Id, ProductType.Consumable);
        builder.AddProduct(iapData.bestDealTicketCard.Id, ProductType.Consumable);

        builder.AddProduct(iapData.adsSubscriptionItem.Id, ProductType.Subscription);
        //builder.AddProduct(adsSubscriptionItem.Id, ProductType.Subscription);

        UnityPurchasing.Initialize(this, builder);
    }

    public void OnInitialized(IStoreController controller, IExtensionProvider extensions)
    {
        Debug.Log("IAP initialized");
        storeController = controller;

        // --- consumable
        Product basicCard = storeController.products.WithID(iapData.basicTicketCard.Id);
        Product valueCard = storeController.products.WithID(iapData.valueTicketCard.Id);
        Product premiumCard = storeController.products.WithID(iapData.premiumTicketCard.Id);
        Product bestDealCard = storeController.products.WithID(iapData.bestDealTicketCard.Id);
        // --- subscription
        Product adsSubscription = storeController.products.WithID(iapData.adsSubscriptionItem.Id);

        // --- ui update
        basicCardPriceText.text = basicCard.metadata.localizedPriceString;
        valueCardPriceText.text = valueCard.metadata.localizedPriceString;
        premiumCardPriceText.text = premiumCard.metadata.localizedPriceString;
        bestDealCardPriceText.text = bestDealCard.metadata.localizedPriceString;

        adsSubscriptionPriceText.text = adsSubscription.metadata.localizedPriceString;
    }

    // ---BUTTON FUNCTIONS---
    public void BasicCard500()
    {
        uiManager.ConnetingPleaseWait(true);
        storeController.InitiatePurchase(iapData.basicTicketCard.Id);
    }

    public void ValueCard750()
    {
        uiManager.ConnetingPleaseWait(true);
        storeController.InitiatePurchase(iapData.valueTicketCard.Id);
    }

    public void PremiumCard1000()
    {
        uiManager.ConnetingPleaseWait(true);
        storeController.InitiatePurchase(iapData.premiumTicketCard.Id);
    }

    public void TicketBestDealCard1500()
    {
        uiManager.ConnetingPleaseWait(true);
        storeController.InitiatePurchase(iapData.bestDealTicketCard.Id);
    }

    public void AdsSubscriptionCard()
    {
        uiManager.ConnetingPleaseWait(true);
        storeController.InitiatePurchase(iapData.adsSubscriptionItem.Id);
    }

        // ---PROCESSING PRUCHASE---
        public PurchaseProcessingResult ProcessPurchase(PurchaseEventArgs purchaseEvent)
    {
        var product = purchaseEvent.purchasedProduct;
        string productID = product.definition.id;

        if (productID == iapData.basicTicketCard.Id)
        {
            economyManager.CreditTickets(500);

            uiManager.ConnetingPleaseWait(false);
            uiManager.ThankYouForPurchase(true);
        }
        else if (productID == iapData.valueTicketCard.Id)
        {
            economyManager.CreditTickets(750);

            uiManager.ConnetingPleaseWait(false);
            uiManager.ThankYouForPurchase(true);
        }
        else if (productID == iapData.premiumTicketCard.Id)
        {
            economyManager.CreditTickets(1000);

            uiManager.ConnetingPleaseWait(false);
            uiManager.ThankYouForPurchase(true);
        }
        else if (productID == iapData.bestDealTicketCard.Id)
        {
            economyManager.CreditTickets(1500);

            uiManager.ConnetingPleaseWait(false);
            uiManager.ThankYouForPurchase(true);
        }

        return PurchaseProcessingResult.Complete;
    }

    //--------------------------------------------------------------------------------
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



