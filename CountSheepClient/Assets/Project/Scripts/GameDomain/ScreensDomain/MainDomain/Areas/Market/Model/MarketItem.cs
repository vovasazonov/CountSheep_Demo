using System;
using System.Linq;
using Project.CoreDomain.Services.Purchase;
using Project.Scripts.CoreDomain.Services.Ads;

namespace Project.GameDomain.ScreensDomain.MainDomain.Areas.Market.Model
{
    public class MarketItem : IMarketItem
    {
        public event Action Updated;

        private readonly bool _isIncludeAds;
        private readonly Action _onBuy;
        private readonly IAdsService _adsService;
        private readonly IPurchaseService _purchaseService;
        private readonly bool _isNonConsumable;

        public string Id { get; }
        public bool IsIncludeAds => _isIncludeAds && !_adsService.HasNoAds;
        public int Coins { get; }
        public string Price => IsAvailable ? $"{_purchaseService.AvailableProducts.First(i => i.Id == Id).Price} {_purchaseService.AvailableProducts.First(i => i.Id == Id).IsoCode}" : "";
        public bool IsAvailable => _purchaseService.IsInitialized && _purchaseService.AvailableProducts != null && _purchaseService.AvailableProducts.Any(i => i.Id == Id);

        public MarketItem(string id, bool isIncludeAds, int coins, Action onBuy, IAdsService adsService, IPurchaseService purchaseService, bool isNonConsumable)
        {
            Id = id;
            _isIncludeAds = isIncludeAds;
            _onBuy = onBuy;
            _adsService = adsService;
            _purchaseService = purchaseService;
            _isNonConsumable = isNonConsumable;
            Coins = coins;
            purchaseService.Initialized += OnPurchaseInitialized;
        }

        private void OnPurchaseInitialized()
        {
            if (_isNonConsumable)
            {
                _onBuy?.Invoke();
            }
            else
            {
                Updated?.Invoke();
            }
        }

        public void Buy()
        {
            _purchaseService.Purchase(Id, isSuccess =>
            {
                if (isSuccess)
                {
                    _onBuy?.Invoke();
                    Updated?.Invoke();
                }
            });
        }

        public void CheckReceipt()
        {
            if (_isNonConsumable && _purchaseService.IsInitialized && _purchaseService.HasReceipt(Id))
            {
                // _onBuy?.Invoke(); // TODO: Has Receipt looks like not works. Check it
            }
        }
    }
}