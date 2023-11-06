using System;
using Cysharp.Threading.Tasks;
using Project.CoreDomain;
using Project.CoreDomain.Services.Data;
using Project.CoreDomain.Services.Purchase;
using Project.GameDomain.ScreensDomain.MainDomain.Areas.Currency.Model;
using Project.Scripts.CoreDomain.Services.Ads;

namespace Project.GameDomain.ScreensDomain.MainDomain.Areas.Market.Model
{
    public class MarketModel : IMarketModel, IDomainTaskAsyncInitializable
    {
        private readonly ICurrencyModel _currencyModel;
        private readonly IAdsService _adsService;
        private readonly IPurchaseService _purchaseService;
        private readonly IDataStorageService _dataStorageService;

        public event Action VisibleUpdated;

        public bool IsOpen { get; private set; }

        public IMarketItem RedBundle { get; private set; }
        public IMarketItem PurpleBundle { get; private set; }
        public IMarketItem MiniBundle1 { get; private set; }
        public IMarketItem MiniBundle2 { get; private set; }
        public IMarketItem MiniBundle3 { get; private set; }
        public IMarketItem NoAdsBundle { get; private set; }

        public MarketModel(ICurrencyModel currencyModel, IAdsService adsService, IPurchaseService purchaseService, IDataStorageService dataStorageService)
        {
            _currencyModel = currencyModel;
            _adsService = adsService;
            _purchaseService = purchaseService;
            _dataStorageService = dataStorageService;
        }

        public UniTask InitializeAsync()
        {
            RedBundle = new MarketItem("red_bundle", false, 900000000, () =>
            {
                _currencyModel.Amount += 900000000;
                _dataStorageService.Save();
            }, _adsService, _purchaseService, false);
            PurpleBundle = new MarketItem("purple_bundle", false, 50000000, () =>
            {
                _currencyModel.Amount += 50000000;
                _dataStorageService.Save();
            }, _adsService, _purchaseService, false);
            MiniBundle1 = new MarketItem("mini_bundle_1", false, 900000, () =>
            {
                _currencyModel.Amount += 900000;
                _dataStorageService.Save();
            }, _adsService, _purchaseService, false);
            MiniBundle2 = new MarketItem("mini_bundle_2", false, 50000, () =>
            {
                _currencyModel.Amount += 50000;
                _dataStorageService.Save();
            }, _adsService, _purchaseService, false);
            MiniBundle3 = new MarketItem("mini_bundle_3", false, 10000, () =>
            {
                _currencyModel.Amount += 10000;
                _dataStorageService.Save();
            }, _adsService, _purchaseService, false);
            NoAdsBundle = new MarketItem("no_ads", true, 0, () =>
            {
                _adsService.HasNoAds = true;
                _dataStorageService.Save();
            }, _adsService, _purchaseService, true);

            NoAdsBundle.CheckReceipt();
            
            return UniTask.CompletedTask;
        }

        public void Open()
        {
            IsOpen = true;
            VisibleUpdated?.Invoke();
        }

        public void Close()
        {
            IsOpen = false;
            VisibleUpdated?.Invoke();
        }
    }
}