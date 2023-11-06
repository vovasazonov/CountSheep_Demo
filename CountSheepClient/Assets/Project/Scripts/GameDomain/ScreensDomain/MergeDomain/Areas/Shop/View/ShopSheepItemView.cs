using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Project.CoreDomain.Services.Analytics;
using Project.CoreDomain.Services.Audio;
using Project.CoreDomain.Services.Audio.Configs.AudioPlayer;
using Project.CoreDomain.Services.Content;
using Project.GameDomain.ScreensDomain.MainDomain;
using Project.GameDomain.ScreensDomain.MainDomain.Areas.Currency.Model;
using Project.GameDomain.ScreensDomain.MainDomain.Areas.Farm.Model;
using Project.GameDomain.ScreensDomain.MergeDomain.Areas.Shop.Config;
using Project.GameDomain.ScreensDomain.MergeDomain.Areas.Shop.Data;
using Project.Scripts.CoreDomain.Services.Ads;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace Project.GameDomain.ScreensDomain.MergeDomain.Areas.Shop.View
{
    public class ShopSheepItemView : MonoBehaviour
    {
        [SerializeField] private string _productionTextFormat;
        [SerializeField] private string _purchaseTextFormat;
        [SerializeField] private Text _purchasedText;
        [SerializeField] private Text _nameText;
        [SerializeField] private Text _productionText;
        [SerializeField] private Image _itemImage;
        [SerializeField] private Button _buyButton;
        [SerializeField] private Text _costText;
        [SerializeField] private Button _watchAdButton;
        [SerializeField] private AudioPlayerConfig _buySound;
        [SerializeField] private Text _secretNameText;
        private IContentService _contentService;
        private IFarmModel _farmModel;
        private ShopSheepConfig.ItemById _config;
        private IAudioService _audioService;
        private ShopSheepData _data;
        private ICurrencyModel _currencyModel;
        private IAdsService _adsService;
        private bool _isWatchAdVisible;
        private ThanksShopSheepWatchAdView _thanksShopSheepWatchAdView;
        private bool _isSecret;
        private bool _isInitialized;

        public event Action<FailBuyReason> BuyFailed;

        public bool IsVisible
        {
            set => gameObject.SetActive(value);
        }

        public bool IsWatchAdVisible
        {
            get => _isWatchAdVisible;
            set
            {
                _isWatchAdVisible = value;

                UpdateVisibilityWatchAds();
            }
        }

        public bool IsSecret
        {
            set
            {
                _isSecret = value;
                _buyButton.gameObject.SetActive(!value);
                _itemImage.color = value ? Color.black : Color.white;
                _nameText.gameObject.SetActive(!value);
                _secretNameText.gameObject.SetActive(value);
            }
        }

        public Action OnBuyOrWatchAd { get; set; }

        private void UpdateVisibilityWatchAds()
        {
            if (_isWatchAdVisible && _adsService.Reward.IsReady)
            {
                _watchAdButton.gameObject.SetActive(true);
            }
            else
            {
                _watchAdButton.gameObject.SetActive(false);
            }
        }

        [Inject]
        private void Constructor(
            IContentService contentService,
            IFarmModel farmModel,
            ICurrencyModel currencyModel,
            IAudioService audioService,
            IAdsService adsService
        )
        {
            _contentService = contentService;
            _farmModel = farmModel;
            _currencyModel = currencyModel;
            _audioService = audioService;
            _adsService = adsService;
        }

        public void Initialize(ShopSheepConfig.ItemById config, ShopSheepData shopSheepData, ThanksShopSheepWatchAdView thanksShopSheepWatchAdView)
        {
            _thanksShopSheepWatchAdView = thanksShopSheepWatchAdView;
            _data = shopSheepData;
            _config = config;
            _productionText.text = String.Format(_productionTextFormat, _farmModel.GetCoinsPerClick(config.Id)/20f);
            UpdatePurchaseText();
            _nameText.text = _farmModel.GetName(config.Id);
            UpdateCost();
            SetImageAsync();
            UpdateBuyable();
            UpdateVisibilityWatchAds();
            _isInitialized = true;
        }

        private async UniTask SetImageAsync()
        {
            _itemImage.sprite = (await _contentService.LoadAsync<Sprite>(string.Format(MainScreenContentIds.PlayerIdleSprite, _config.Id))).Value;
        }

        private void UpdateCost()
        {
            _costText.text = $"{GetCurrenCost():n0}";
        }

        private void UpdateBuyable()
        {
            _buyButton.interactable = GetCurrenCost() <= _currencyModel.Amount;
        }

        private void UpdatePurchaseText()
        {
            _purchasedText.text = String.Format(_purchaseTextFormat, _data.AmountPurchaseBySheepItem[_config.Id]);
        }

        private int GetCurrenCost()
        {
            return (_data.AmountPurchaseBySheepItem[_config.Id] + 1) * _config.Cost;
        }

        private void OnEnable()
        {
            _currencyModel.Updated += OnCurrencyUpdated;
            _buyButton.onClick.AddListener(OnBuyClicked);
            _watchAdButton.onClick.AddListener(OnWatchAdClicked);
            _adsService.Reward.ReadyUpdated += OnWatchAdUpdated;

            if (_isInitialized)
            {
                UpdateBuyable();
            }
        }

        private void OnDisable()
        {
            _currencyModel.Updated -= OnCurrencyUpdated;
            _buyButton.onClick.RemoveListener(OnBuyClicked);
            _watchAdButton.onClick.RemoveListener(OnWatchAdClicked);
            _adsService.Reward.ReadyUpdated -= OnWatchAdUpdated;
        }

        private void OnWatchAdUpdated()
        {
            UpdateVisibilityWatchAds();
        }

        private void OnCurrencyUpdated()
        {
            UpdateBuyable();
        }

        private void OnBuyClicked()
        {
            if (CheckAvailablePlace())
            {
                var currentCost = GetCurrenCost();
                _farmModel.Add(_config.Id);
                _currencyModel.Amount -= currentCost;
                _data.AmountPurchaseBySheepItem[_config.Id]++;
                _audioService.Sound.PlayImmediately(_buySound);
                UpdateBuyable();
                UpdateCost();
                UpdatePurchaseText();
                AnalyticService.Instance.Track("coins_spend", new Dictionary<string, object>()
                {
                    { "currentAmountCoin", _currencyModel.Amount },
                    { "itemName", $"ShopSheep: {_config.Id}" },
                    { "itemCost", currentCost }
                });
                OnBuyOrWatchAd?.Invoke();
            }
        }

        private void OnWatchAdClicked()
        {
            if (CheckAvailablePlace())
            {
                _adsService.Reward.Watch(() =>
                    {
                        AnalyticService.Instance.Track("video_reward_from_shop", new Dictionary<string, object>()
                        {
                            {
                                "itemName", _isSecret ? "shop_sheep_secret" : $"shop_sheep_{_config.Id}"
                            }
                        });

                        _farmModel.Add(_config.Id);
                        _data.AmountPurchaseBySheepItem[_config.Id]++;
                        IsVisible = false;

                        _thanksShopSheepWatchAdView.SetNameOfSheep(_farmModel.GetName(_config.Id));
                        _thanksShopSheepWatchAdView.Show();
                        OnBuyOrWatchAd?.Invoke();
                    },
                    null);
            }
        }

        private bool CheckAvailablePlace()
        {
            var hasPlace = _farmModel.HasPlaceInFarm();

            if (!hasPlace)
            {
                BuyFailed?.Invoke(FailBuyReason.NoPlaceForAnimalInFarm);
            }
            
            return hasPlace;
        }
    }
}