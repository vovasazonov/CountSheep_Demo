using System;
using System.Collections.Generic;
using System.Linq;
using Cysharp.Threading.Tasks;
using Project.CoreDomain.Services.Logger;
using Project.CoreDomain.Services.Serialization;
using Unity.Services.Core;
using Unity.Services.Core.Environments;
using UnityEngine;
using UnityEngine.Purchasing;
using UnityEngine.Purchasing.Extension;

namespace Project.CoreDomain.Services.Purchase
{
    public class PurchaseService : IPurchaseService, IDetailedStoreListener, ITaskAsyncInitializable
    {
        public event Action Initialized;

        private readonly ISerializerService _serializerService;
        private bool _isInitialized;
        private IStoreController _storeController;
        private Action<bool> _onPurchased;

        public bool IsInitialized
        {
            get => _isInitialized;
            private set
            {
                _isInitialized = value;

                if (_isInitialized)
                {
                    Initialized?.Invoke();
                }
            }
        }

        public IEnumerable<PurchaseProduct> AvailableProducts { get; private set; }

        public PurchaseService(ISerializerService serializerService)
        {
            _serializerService = serializerService;
        }

        public async UniTask InitializeAsync()
        {
            Initialize();
        }

        private async UniTask Initialize()
        {
            var options = new InitializationOptions().
#if UNITY_EDITOR || DEVELOPMENT_BUILD
                SetEnvironmentName("test");
#else
                SetEnvironmentName("production");
#endif
            await UnityServices.InitializeAsync(options);
            var operation = Resources.LoadAsync<TextAsset>("IAPProductCatalog");
            operation.completed += i =>
            {
                var request = i as ResourceRequest;
                OsyaLogger.Log($"Loaded asset: {request.asset}");
                var catalog = ProductCatalog.Deserialize((request.asset as TextAsset).text);
                OsyaLogger.Log($"Loaded catalog with {catalog.allProducts.Count} items");


#if UNITY_EDITOR
                StandardPurchasingModule.Instance().useFakeStoreUIMode = FakeStoreUIMode.StandardUser;
                StandardPurchasingModule.Instance().useFakeStoreAlways = true;
#endif
                
                var builder
#if UNITY_ANDROID
                    = ConfigurationBuilder.Instance(StandardPurchasingModule.Instance(AppStore.GooglePlay));
#elif UNITY_IOS
                    = ConfigurationBuilder.Instance(StandardPurchasingModule.Instance(AppStore.AppleAppStore));
#else
                    = ConfigurationBuilder.Instance(StandardPurchasingModule.Instance(AppStore.NotSpecified));
#endif

                foreach (var item in catalog.allProducts)
                {
                    builder.AddProduct(item.id, item.type);
                }

                UnityPurchasing.Initialize(this, builder);
            };
        }


        public void OnInitializeFailed(InitializationFailureReason error)
        {
            IsInitialized = false;
            OsyaLogger.LogError($"Error initializing IAP because of {error}. Show a message to the player depending on the error.");
        }

        public void OnInitializeFailed(InitializationFailureReason error, string message)
        {
            IsInitialized = false;
            OsyaLogger.LogError($"Error initializing IAP because of {error} {message}. Show a message to the player depending on the error.");
        }

        public void OnInitialized(IStoreController controller, IExtensionProvider extensions)
        {
            _storeController = controller;

            AvailableProducts = controller.products.all.Select(i => new PurchaseProduct()
            {
                Id = i.definition.id,
                Price = i.metadata.localizedPrice,
                IsoCode = i.metadata.isoCurrencyCode,
            }).ToArray();

            IsInitialized = true;
        }

        public PurchaseProcessingResult ProcessPurchase(PurchaseEventArgs purchaseEvent)
        {
            OsyaLogger.Log($"Successfully purchased {purchaseEvent.purchasedProduct.definition.id}");
            _onPurchased?.Invoke(true);
            _onPurchased = null;

            return PurchaseProcessingResult.Complete;
        }

        public void OnPurchaseFailed(Product product, PurchaseFailureReason failureReason)
        {
            OsyaLogger.Log($"Failed to purchase {product.definition.id} because {failureReason}");
            _onPurchased?.Invoke(false);
            _onPurchased = null;
        }

        public void OnPurchaseFailed(Product product, PurchaseFailureDescription failureDescription)
        {
            OsyaLogger.Log($"Failed to purchase {product.definition.id} because {failureDescription.reason} - {failureDescription.message}");
            _onPurchased?.Invoke(false);
            _onPurchased = null;
        }

        public void Purchase(string productId, Action<bool> onPurchased)
        {
            if (_onPurchased == null)
            {
                var product = _storeController.products.WithID(productId);
                _storeController.InitiatePurchase(product);
                _onPurchased = onPurchased;
            }
            else
            {
                onPurchased?.Invoke(false);
            }
        }

        public bool HasReceipt(string productId)
        {
            return _storeController.products.WithID(productId).hasReceipt;
        }
    }
}