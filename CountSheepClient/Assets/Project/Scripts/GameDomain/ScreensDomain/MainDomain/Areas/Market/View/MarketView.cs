using Project.Scripts.CoreDomain.Services.Ads;
using UnityEngine;
using Zenject;

namespace Project.GameDomain.ScreensDomain.MainDomain.Areas.Market.View
{
    public class MarketView : MonoBehaviour, IMarketView
    {
        [SerializeField] private MarketItemView _redBundle;
        [SerializeField] private MarketItemView _purpleBundle;
        [SerializeField] private MarketItemView _miniBundle1;
        [SerializeField] private MarketItemView _miniBundle2;
        [SerializeField] private MarketItemView _miniBundle3;
        [SerializeField] private MarketItemView _miniBundle4;
        private IAdsService _adsService;

        public IMarketItemView RedBundle => _redBundle;
        public IMarketItemView PurpleBundle => _purpleBundle;
        public IMarketItemView MiniBundle1 => _miniBundle1;
        public IMarketItemView MiniBundle2 => _miniBundle2;
        public IMarketItemView MiniBundle3 => _miniBundle3;
        public IMarketItemView MiniBundle4 => _miniBundle4;

        [Inject]
        private void Constructor(IAdsService adsService)
        {
            _adsService = adsService;
        }
        
        public void Open()
        {
            // _miniBundle4.gameObject.SetActive(!_adsService.HasNoAds);
            gameObject.SetActive(true);
        }

        public void Close()
        {
            gameObject.SetActive(false);
        }

        private void OnEnable()
        {
            _adsService.HasNoAdsUpdated += OnHasNoAdsUpdated;
        }

        private void OnDisable()
        {
            _adsService.HasNoAdsUpdated -= OnHasNoAdsUpdated;
        }

        private void OnHasNoAdsUpdated()
        {
            // _miniBundle4.gameObject.SetActive(!_adsService.HasNoAds);
        }
    }
}