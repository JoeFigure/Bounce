using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Purchasing;


// Deriving the Purchaser class from IStoreListener enables it to receive messages from Unity Purchasing.
public class Purchaser : MonoBehaviour, IStoreListener
{
	private static IStoreController m_StoreController;
	// The Unity Purchasing system.
	private static IExtensionProvider m_StoreExtensionProvider;
	// The store-specific Purchasing subsystems.

		
	public static string kProductIDNonConsumable = "nonconsumable";
	public static string kProductIDSubscription = "subscription";

	public static string kProductIDConsumable = "TenZoins";
	public static string productA_IDConsumable = "25Zoins";
	public static string productB_IDConsumable = "50Zoins";
	public static string productC_IDConsumable = "100Zoins";
	public static string productD_IDConsumable = "500Zoins";

	// Apple App Store-specific product identifier for the subscription product.
		
	//private static string kProductNameAppleConsumable = "TenZoins";
	//private static string productAAppleConsumable = "25Zoins";
	//private static string productBAppleConsumable = "50Zoins";
	//private static string productCAppleConsumable = "100Zoins";
	//private static string productDAppleConsumable = "500Zoins";


	// Google Play Store-specific product identifier subscription product.
	//private static string kProductNameGooglePlaySubscription =  "com.unity3d.subscription.original";

	void Start (){
		// If we haven't set up the Unity Purchasing reference
		if (m_StoreController == null) {
			// Begin to configure our connection to Purchasing
			InitializePurchasing ();
		}
	}

	public void InitializePurchasing (){
		// If we have already connected to Purchasing ...
		if (IsInitialized ()) {
			// ... we are done here.
			return;
		}

		// Create a builder, first passing in a suite of Unity provided stores.
		var builder = ConfigurationBuilder.Instance (StandardPurchasingModule.Instance ());

		builder.AddProduct (kProductIDConsumable, ProductType.Consumable);
		builder.AddProduct (productA_IDConsumable, ProductType.Consumable);
		builder.AddProduct (productB_IDConsumable, ProductType.Consumable);
		builder.AddProduct (productC_IDConsumable, ProductType.Consumable);
		builder.AddProduct (productD_IDConsumable, ProductType.Consumable);
		//builder.AddProducts(
		//builder.AddProduct(kProductIDConsumable, ProductType.Consumable, new IDs(){ kProductNameAppleConsumable, AppleAppStore.Name });
			

		// Kick off the remainder of the set-up with an asynchrounous call, passing the configuration 
		// and this class' instance. Expect a response either in OnInitialized or OnInitializeFailed.
		UnityPurchasing.Initialize (this, builder);
	}


	private bool IsInitialized (){
		// Only say we are initialized if both the Purchasing references are set.
		return m_StoreController != null && m_StoreExtensionProvider != null;
	}

	//Buy consumable
	public void Buy10Zoins (){
		BuyProductID (kProductIDConsumable);
	}

	public void Buy25Zoins (){
		BuyProductID (productA_IDConsumable);
	}

	public void Buy50Zoins (){
		BuyProductID (productB_IDConsumable);
	}

	public void Buy100Zoins (){
		BuyProductID (productC_IDConsumable);
	}

	public void Buy500Zoins (){
		BuyProductID (productD_IDConsumable);
	}



	public void BuyNonConsumable (){
		BuyProductID (kProductIDNonConsumable);
	}


	void BuyProductID (string productId){
		// If Purchasing has been initialized ...
		if (IsInitialized ()) {
			// ... look up the Product reference with the general product identifier and the Purchasing 
			// system's products collection.
			Product product = m_StoreController.products.WithID (productId);

			// If the look up found a product for this device's store and that product is ready to be sold ... 
			if (product != null && product.availableToPurchase) {
				Debug.Log (string.Format ("Purchasing product asychronously: '{0}'", product.definition.id));
				// ... buy the product. Expect a response either through ProcessPurchase or OnPurchaseFailed 
				// asynchronously.
				m_StoreController.InitiatePurchase (product);
			}
				// Otherwise ...
				else {
				// ... report the product look-up failure situation  
				Debug.Log ("BuyProductID: FAIL. Not purchasing product, either is not found or is not available for purchase");
			}
		}
			// Otherwise ...
			else {
			// ... report the fact Purchasing has not succeeded initializing yet. Consider waiting longer or 
			// retrying initiailization.
			Debug.Log ("BuyProductID FAIL. Not initialized.");
		}
	}


	// Restore purchases previously made by this customer. Some platforms automatically restore purchases, like Google.
	// Apple currently requires explicit purchase restoration for IAP, conditionally displaying a password prompt.
	public void RestorePurchases (){
		// If Purchasing has not yet been set up ...
		if (!IsInitialized ()) {
			// ... report the situation and stop restoring. Consider either waiting longer, or retrying initialization.
			Debug.Log ("RestorePurchases FAIL. Not initialized.");
			return;
		}

		// If we are running on an Apple device ... 
		if (Application.platform == RuntimePlatform.IPhonePlayer ||
		     Application.platform == RuntimePlatform.OSXPlayer) {
			// ... begin restoring purchases
			Debug.Log ("RestorePurchases started ...");

			// Fetch the Apple store-specific subsystem.
			var apple = m_StoreExtensionProvider.GetExtension<IAppleExtensions> ();
			// Begin the asynchronous process of restoring purchases. Expect a confirmation response in 
			// the Action<bool> below, and ProcessPurchase if there are previously purchased products to restore.
			apple.RestoreTransactions ((result) => {
				// The first phase of restoration. If no more responses are received on ProcessPurchase then 
				// no purchases are available to be restored.
				Debug.Log ("RestorePurchases continuing: " + result + ". If no further messages, no purchases available to restore.");
			});
		}
			// Otherwise ...
			else {
			// We are not running on an Apple device. No work is necessary to restore purchases.
			Debug.Log ("RestorePurchases FAIL. Not supported on this platform. Current = " + Application.platform);
		}
	}


	//
	// --- IStoreListener
	//

	public void OnInitialized (IStoreController controller, IExtensionProvider extensions){
		// Purchasing has succeeded initializing. Collect our Purchasing references.
		Debug.Log ("OnInitialized: PASS");

		// Overall Purchasing system, configured with products for this application.
		m_StoreController = controller;
		// Store specific subsystem, for accessing device-specific store features.
		m_StoreExtensionProvider = extensions;
	}


	public void OnInitializeFailed (InitializationFailureReason error){
		// Purchasing set-up has not succeeded. Check error for reason. Consider sharing this reason with the user.
		Debug.Log ("OnInitializeFailed InitializationFailureReason:" + error);
	}


	public PurchaseProcessingResult ProcessPurchase (PurchaseEventArgs args){
		// A consumable product has been purchased by this user.
		if (String.Equals (args.purchasedProduct.definition.id, kProductIDConsumable, StringComparison.Ordinal)) {
			//print ("10 Zoins");
			GameSparksManager.AddZoin (10);
			UIManager.instance.MainMenuUI ();
		}
		if (String.Equals (args.purchasedProduct.definition.id, productA_IDConsumable, StringComparison.Ordinal)) {
			//Debug.Log ("25 Zoins");
			GameSparksManager.AddZoin (25);
			UIManager.instance.MainMenuUI ();
		}
		if (String.Equals (args.purchasedProduct.definition.id, productB_IDConsumable, StringComparison.Ordinal)) {
			//Debug.Log ("50 Zoins");
			GameSparksManager.AddZoin (50);
			UIManager.instance.MainMenuUI ();
		}
		if (String.Equals (args.purchasedProduct.definition.id, productC_IDConsumable, StringComparison.Ordinal)) {
			//Debug.Log ("100 Zoins");
			GameSparksManager.AddZoin (100);
			UIManager.instance.MainMenuUI ();
		}
		if (String.Equals (args.purchasedProduct.definition.id, productD_IDConsumable, StringComparison.Ordinal)) {
			//Debug.Log ("500 Zoins");
			GameSparksManager.AddZoin (500);
			UIManager.instance.MainMenuUI ();
		}
			// Or ... a non-consumable product has been purchased by this user.
			else if (String.Equals (args.purchasedProduct.definition.id, kProductIDNonConsumable, StringComparison.Ordinal)) {
				Debug.Log (string.Format ("ProcessPurchase: PASS. Product: '{0}'", args.purchasedProduct.definition.id));
				// TODO: The non-consumable item has been successfully purchased, grant this item to the player.
			}
			
			// Or ... an unknown product has been purchased by this user. Fill in additional products here....
			else {
				Debug.Log (string.Format ("ProcessPurchase: FAIL. Unrecognized product: '{0}'", args.purchasedProduct.definition.id));
			}
			
		return PurchaseProcessingResult.Complete;

	}


	public void OnPurchaseFailed (Product product, PurchaseFailureReason failureReason){

		UIManager.instance.MainMenuUI ();
		Debug.Log (string.Format ("OnPurchaseFailed: FAIL. Product: '{0}', PurchaseFailureReason: {1}", product.definition.storeSpecificId, failureReason));

	}
}
