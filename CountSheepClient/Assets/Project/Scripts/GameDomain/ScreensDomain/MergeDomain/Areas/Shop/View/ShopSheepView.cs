using System;
using System.Collections.Generic;
using System.Linq;
using Project.CoreDomain.Services.Analytics;
using Project.CoreDomain.Services.Data;
using Project.CoreDomain.Services.View;
using Project.GameDomain.ScreensDomain.MainDomain.Areas.Currency.Model;
using Project.GameDomain.ScreensDomain.MainDomain.Areas.Farm.Model;
using Project.GameDomain.ScreensDomain.MainDomain.Areas.Tutorials;
using Project.GameDomain.ScreensDomain.MergeDomain.Areas.Shop.Config;
using Project.GameDomain.ScreensDomain.MergeDomain.Areas.Shop.Data;
using Project.Scripts.CoreDomain.Services.Ads;
using TMPro;
using UnityEngine;
using Zenject;

namespace Project.GameDomain.ScreensDomain.MergeDomain.Areas.Shop.View
{
    public class ShopSheepView : MonoBehaviour
    {
        [SerializeField] private ShopSheepItemView shopSheepItemPrefab;
        [SerializeField] private Transform _itemsContainer;
        [SerializeField] private ShopSheepConfig _config;
        [SerializeField] private ThanksShopSheepWatchAdView _thanksShopSheepWatchAdView;
        [SerializeField] private GameObject _areaFull;
        [SerializeField] private TextMeshProUGUI _amountAnimalsText;
        [SerializeField] private TextMeshProUGUI _infoAreaFullText;
        [SerializeField] private GameObject _importantIcon;
        private readonly Dictionary<string, IDisposableView<ShopSheepItemView>> _views = new();
        private IDataStorageService _dataStorageService;
        private IViewService _viewService;
        private ShopSheepData _data;
        private bool _isInitialized;
        private IFarmModel _farmModel;
        private ShopSheepItemView _secretItem;
        private IAdsService _adsService;
        private ICurrencyModel _currencyModel;

        [Inject]
        private void Constructor(
            IAdsService adsService,
            IViewService viewService,
            IDataStorageService dataStorageService,
            IFarmModel farmModel,
            ICurrencyModel currencyModel
        )
        {
            _adsService = adsService;
            _dataStorageService = dataStorageService;
            _viewService = viewService;
            _farmModel = farmModel;
            _currencyModel = currencyModel;
        }

        private void OnEnable()
        {
            AnalyticService.Instance.Track("shop_open", new Dictionary<string, object>()
            {
                {"currentAmountCoin", _currencyModel.Amount}
            });
            
            if (!_isInitialized)
            {
                _isInitialized = true;
                var dataKey = "shop_sheep";
                _data = _dataStorageService.Contains(dataKey) ? _dataStorageService.Get<ShopSheepData>(dataKey) : _dataStorageService.Create<ShopSheepData>(dataKey);

                var itemsConfigList = _config.Items.ToList();
                itemsConfigList.Reverse();
                foreach (var config in itemsConfigList)
                {
                    if (!_data.AmountPurchaseBySheepItem.ContainsKey(config.Id))
                    {
                        _data.AmountPurchaseBySheepItem.Add(config.Id, 0);
                    }

                    var itemView = _viewService.Create(shopSheepItemPrefab);
                    _views.Add(config.Id, itemView);
                    itemView.Value.Initialize(config, _data, _thanksShopSheepWatchAdView);
                    itemView.Value.transform.SetParent(_itemsContainer, false);
                    AddItemListeners(itemView.Value);
                }

                _farmModel.CollectionUpdated += OnCollectionUpdated;
            }
            
            int previousPurchase = 10;
            string lastActive = "";
            string secretActive = _config.Items.FirstOrDefault(i => !_farmModel.ContainsInCollection(i.Id)).Id;
            
            foreach (var configItem in _config.Items)
            {
                var isActive = previousPurchase > 9 && _farmModel.ContainsInCollection(configItem.Id);

                if (isActive)
                {
                    lastActive = configItem.Id;
                }
                
                _views[configItem.Id].Value.IsVisible = isActive;
                previousPurchase = _data.AmountPurchaseBySheepItem[configItem.Id];
            }

            var isTutorialPassed = _dataStorageService.Contains(BuyInShopTutorialView.TutorialDataKey) && _dataStorageService.Get<PrimitiveData<bool>>(BuyInShopTutorialView.TutorialDataKey).Value;

            var ticksData = GetTicksData();
            var previousData = new DateTime(ticksData.Value);
            TimeSpan diff = DateTime.Now - previousData;
            var isLeftMoreThanHourBetweenSecrets = diff.TotalMinutes > 60;
            
            foreach (var configItem in _config.Items)
            {
                _views[configItem.Id].Value.IsWatchAdVisible = configItem.Id == lastActive && isTutorialPassed;
                _views[configItem.Id].Value.IsSecret = false;
                _views[configItem.Id].Value.OnBuyOrWatchAd = null;

                if (isTutorialPassed && !string.IsNullOrEmpty(secretActive) && secretActive == configItem.Id && _adsService.Reward.IsReady && isLeftMoreThanHourBetweenSecrets)
                {
                    _views[configItem.Id].Value.IsVisible = true;
                    _views[configItem.Id].Value.IsWatchAdVisible = true;
                    _views[configItem.Id].Value.IsSecret = true;
                    _views[configItem.Id].Value.OnBuyOrWatchAd = () =>
                    {
                        ticksData.Value = DateTime.Now.Ticks;
                        _dataStorageService.Save();
                        _views[configItem.Id].Value.OnBuyOrWatchAd = null;
                    };
                }
            }
        }

        private PrimitiveData<long> GetTicksData()
        {
            var dataKey = "secrets_interval_ticks";
            PrimitiveData<long> _ticks;

            if (_dataStorageService.Contains(dataKey))
            {
                _ticks = _dataStorageService.Get<PrimitiveData<long>>(dataKey);
            }
            else
            {
                _ticks = _dataStorageService.Create<PrimitiveData<long>>(dataKey);
                _ticks.Value = DateTime.Now.Ticks;
            }

            return _ticks;
        }

        private void OnDisable()
        {
            AnalyticService.Instance.Track("shop_closed", new Dictionary<string, object>()
            {
                {"currentAmountCoin", _currencyModel.Amount}
            });
        }

        private void OnCollectionUpdated(string obj)
        {
            _importantIcon.SetActive(true);
        }

        private void AddItemListeners(ShopSheepItemView item)
        {
            item.BuyFailed += OnBuyFailed;
        }

        private void RemoveItemListeners(ShopSheepItemView item)
        {
            item.BuyFailed -= OnBuyFailed;
        }

        private void OnBuyFailed(FailBuyReason reason)
        {
            switch (reason)
            {
                case FailBuyReason.NoPlaceForAnimalInFarm:
                    _amountAnimalsText.text = $"{_farmModel.Animals.Count}/{_farmModel.GetMaxAnimalInFarm()}";
                    _infoAreaFullText.text = $"You can only have up to {_farmModel.GetMaxAnimalInFarm()} Sheep.";
                    _areaFull.SetActive(true);
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(reason), reason, null);
            }
        }
    }
}