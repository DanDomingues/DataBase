using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Purchasing;
using UnityEngine.Events;

public class IAPHandler : ReferenceSingleton<IAPHandler>, IStoreListener
{
    private static IStoreController m_StoreController;          // The Unity Purchasing system.
    private static IExtensionProvider m_StoreExtensionProvider; // The store-specific Purchasing subsystems.

    // General product identifiers for the consumable, non-consumable, and subscription products.
    // Use these handles in the code to reference which product to purchase. Also use these values 
    // when defining the Product Identifiers on the store. Except, for illustration purposes, the 
    // kProductIDSubscription - it has custom Apple and Google identifiers. We declare their store-
    // specific mapping to Unity Purchasing's AddProduct, below.
    public List<string> loggedProducts;

    public UnityAction productEffect;

    // Apple App Store-specific product identifier for the subscription product.
    const string kProductNameAppleSubscription = "com.unity3d.subscription.new";

    // Google Play Store-specific product identifier subscription product.
    const string kProductNameGooglePlaySubscription = "com.unity3d.subscription.original";
    const string ConsumableGooglePlayProductName = "com.unity3d.";
    ConfigurationBuilder builder;

    void Update()
    {
        //Debug.Log(IsInitialized());
    }

    /// <summary>
    /// Sets key arrays in it's respective categories for later usage
    /// </summary>
    /// <param name="consumableKeys"></param>
    /// <param name="nonConsumableKeys"></param>
    /// <param name="subscriptionKeys"></param>
    public void LogProductKeys(string[] consumableKeys)
    {
        loggedProducts.AddRange(consumableKeys);
        // If we haven't set up the Unity Purchasing reference
        if (m_StoreController == null)
        {
            // Begin to configure our connection to Purchasing
            InitializePurchasing();
        }
    }

    /// <summary>
    /// Sends a purchase request to the device's store
    /// </summary>
    /// <param name="productKey">Product's IAP key</param>
    /// <param name="productEffect">Action to be done if the purchase is sucessful</param>
    public void PurchaseRequest(string productKey, UnityAction productEffect)
    {
        this.productEffect = productEffect;
        BuyProductID(productKey);
    }

    public void InitializePurchasing()
    {
        // If we have already connected to Purchasing ...
        if (IsInitialized())
        {
            // ... we are done here.
            return;
        }

        // Create a builder, first passing in a suite of Unity provided stores.
        builder = ConfigurationBuilder.Instance(StandardPurchasingModule.Instance());
        builder.Configure<IGooglePlayConfiguration>().SetPublicKey("MIIBIjANBgkqhkiG9w0BAQEFAAOCAQ8AMIIBCgKCAQEAhmGoGEHliLOAjHiUemCsEIaa7oPlRKkMVSps/Vb0199tZFs2FxteTQpUAhv7bf6sw126cnsYDO2Tj24ZwDvxE5+X1+semBP6ZJqaJPewZSGd1v++e1TA6h6llM0aXo0lBCYnn2uIM0vc9Q4Yog2AJzkYmCGc51yLeqjLdZnt5WIEiiNPvP9zf9Vk7isW7MsMnErGcGd85tCXtJOpnE1xkFW76PkCxfZmnsI8ZOST17FcxSADqnMu0JgYNo+TOvQHjogAeE3p9mJrvQOWrPaO1ug5Z64l5/L3U+CDF10W6SrdcjYOUiGEGaFXcccf5HpeRcOE7Dq0WYuCgiQCfSW7YQIDAQAB");

        // Add a product to sell / restore by way of its identifier, associating the general identifier
        // with its store-specific identifiers.
        foreach(string key in loggedProducts)
        builder.AddProduct(key, ProductType.Consumable);


        // And finish adding the subscription product. Notice this uses store-specific IDs, illustrating
        // if the Product ID was configured differently between Apple and Google stores. Also note that
        // one uses the general kProductIDSubscription handle inside the game - the store-specific IDs 
        // must only be referenced here. 

        //foreach (string key in kProductIDSubscription)
        //{
        //    builder.AddProduct(key, ProductType.Subscription, new IDs()
        //    {
        //        { kProductNameAppleSubscription, AppleAppStore.Name },
        //        { kProductNameGooglePlaySubscription, GooglePlay.Name },
        //    });

        //}

        // Kick off the remainder of the set-up with an asynchrounous call, passing the configuration 
        // and this class' instance. Expect a response either in OnInitialized or OnInitializeFailed.
        UnityPurchasing.Initialize(this, builder);
    }

    private bool IsInitialized()
    {
        // Only say we are initialized if both the Purchasing references are set.
        return m_StoreController != null && m_StoreExtensionProvider != null;
    }


    void BuyProductID(string productId)
    {        
        // If Purchasing has been initialized ...
        if (IsInitialized())
        {
            // ... look up the Product reference with the general product identifier and the Purchasing 
            // system's products collection.

            Product product = m_StoreController.products.WithID(productId);
            foreach (var item in m_StoreController.products.all)
            {
                Debug.Log(item.definition.id);
            }

            // If the look up found a product for this device's store and that product is ready to be sold ... 
            if (product != null && product.availableToPurchase)
            {
                Debug.Log(string.Format("Purchasing product asychronously: '{0}'", product.definition.id));
                // ... buy the product. Expect a response either through ProcessPurchase or OnPurchaseFailed 
                // asynchronously.
                m_StoreController.InitiatePurchase(product);
            }
            // Otherwise ...
            else
            {
                Debug.Log(string.Format("{0} (local) {1} (builder)", 
                loggedProducts != null ? loggedProducts.Count.ToString() : "#",
                builder != null ? builder.products.Count.ToString() : "#"));

                // ... report the product look-up failure situation  
                Debug.Log(string.Format("\nBuyProductID: FAIL. Not purchasing product ({0}), either is not found or is not available for purchase", productId));
            }
        }
        // Otherwise ...
        else
        {
            // ... report the fact Purchasing has not succeeded initializing yet. Consider waiting longer or 
            // retrying initiailization.
                Debug.Log(string.Format("{0} (local) {1} (builder)", 
                loggedProducts != null ? loggedProducts.Count.ToString() : "#",
                builder != null ? builder.products.Count.ToString() : "#"));

            Debug.Log("\nBuyProductID FAIL. Not initialized.");
        }

    }


    // Restore purchases previously made by this customer. Some platforms automatically restore purchases, like Google. 
    // Apple currently requires explicit purchase restoration for IAP, conditionally displaying a password prompt.
    public void RestorePurchases()
    {
        // If Purchasing has not yet been set up ...
        if (!IsInitialized())
        {
            // ... report the situation and stop restoring. Consider either waiting longer, or retrying initialization.
            Debug.Log("RestorePurchases FAIL. Not initialized.");
            return;
        }

        // If we are running on an Apple device ... 
        if (Application.platform == RuntimePlatform.IPhonePlayer ||
            Application.platform == RuntimePlatform.OSXPlayer)
        {
            // ... begin restoring purchases
            Debug.Log("RestorePurchases started ...");

            // Fetch the Apple store-specific subsystem.
            var apple = m_StoreExtensionProvider.GetExtension<IAppleExtensions>();
            // Begin the asynchronous process of restoring purchases. Expect a confirmation response in 
            // the Action<bool> below, and ProcessPurchase if there are previously purchased products to restore.
            apple.RestoreTransactions((result) => {
                // The first phase of restoration. If no more responses are received on ProcessPurchase then 
                // no purchases are available to be restored.
                Debug.Log("RestorePurchases continuing: " + result + ". If no further messages, no purchases available to restore.");
            });
        }
        // Otherwise ...
        else
        {
            // We are not running on an Apple device. No work is necessary to restore purchases.
            Debug.Log("RestorePurchases FAIL. Not supported on this platform. Current = " + Application.platform);
        }
    }


    //  
    // --- IStoreListener
    //

    public void OnInitialized(IStoreController controller, IExtensionProvider extensions)
    {
        // Purchasing has succeeded initializing. Collect our Purchasing references.
        Debug.Log("OnInitialized: PASS");

        // Overall Purchasing system, configured with products for this application.
        m_StoreController = controller;
        // Store specific subsystem, for accessing device-specific store features.
        m_StoreExtensionProvider = extensions;
    }


    public void OnInitializeFailed(InitializationFailureReason error)
    {
        // Purchasing set-up has not succeeded. Check error for reason. Consider sharing this reason with the user.
        Debug.Log(("OnInitializeFailed InitializationFailureReason:" + error));
    }

    public PurchaseProcessingResult ProcessPurchase(PurchaseEventArgs args)
    {
        bool sucess = false;

        // A consumable product has been purchased by this user.

        if (!sucess) foreach (string key in loggedProducts)
        if (String.Equals(args.purchasedProduct.definition.id, key, StringComparison.Ordinal))
        {
            Debug.Log(string.Format("ProcessPurchase: PASS. Product: '{0}'", args.purchasedProduct.definition.id));
            productEffect();
            sucess = true;
        }
        /*
        // Or ... a non-consumable product has been purchased by this user.
        else if (String.Equals(args.purchasedProduct.definition.id, kProductIDNonConsumable, StringComparison.Ordinal))
        {
            Debug.Log(string.Format("ProcessPurchase: PASS. Product: '{0}'", args.purchasedProduct.definition.id));
            // TODO: The non-consumable item has been successfully purchased, grant this item to the player.
        }
        // Or ... a subscription product has been purchased by this user.
        else if (String.Equals(args.purchasedProduct.definition.id, kProductIDSubscription, StringComparison.Ordinal))
        {
            Debug.Log(string.Format("ProcessPurchase: PASS. Product: '{0}'", args.purchasedProduct.definition.id));
            // TODO: The subscription item has been successfully purchased, grant this to the player.
        }
        // Or ... an unknown product has been purchased by this user. Fill in additional products here....
        */
        else
        {
            Debug.Log(string.Format("ProcessPurchase: FAIL. Unrecognized product: '{0}'", args.purchasedProduct.definition.id));
        }

        // Return a flag indicating whether this product has completely been received, or if the application needs 
        // to be reminded of this purchase at next app launch. Use PurchaseProcessingResult.Pending when still 
        // saving purchased products to the cloud, and when that save is delayed. 
        return PurchaseProcessingResult.Complete;
    }


    public void OnPurchaseFailed(Product product, PurchaseFailureReason failureReason)
    {
        // A product purchase attempt did not succeed. Check failureReason for more detail. Consider sharing 
        // this reason with the user to guide their troubleshooting actions.
        Debug.Log(string.Format("OnPurchaseFailed: FAIL. Product: '{0}', PurchaseFailureReason: {1}", product.definition.storeSpecificId, failureReason));
    }
}
