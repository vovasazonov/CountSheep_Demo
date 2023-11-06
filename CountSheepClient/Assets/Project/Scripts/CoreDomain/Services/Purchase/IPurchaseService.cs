using System;
using System.Collections;
using System.Collections.Generic;

namespace Project.CoreDomain.Services.Purchase
{
    public interface IPurchaseService
    {
        event Action Initialized;
        bool IsInitialized { get; }
        IEnumerable<PurchaseProduct> AvailableProducts { get; }
        void Purchase(string productId, Action<bool> onPurchased);
        bool HasReceipt(string productId);
    }
}