using System;
using Project.Scripts.CoreDomain.Services.Ads;

namespace Project.Scripts.CoreDomain.Services.AdsImplemantations.UnityAds
{
    public class UnityBannerAd : IBannerAdUnit
    {
        public event Action ReadyUpdated;

        private bool _isReady;
        private bool _isWantToWatch;

        public bool IsEnable { get; set; } = true;

        public bool IsReady
        {
            get => _isReady;
            private set
            {
                _isReady = value;
                ReadyUpdated?.Invoke();
            }
        }

        public UnityBannerAd()
        {
            IronSourceEvents.onSdkInitializationCompletedEvent += OnSdkInitializeComplete;

            IronSourceBannerEvents.onAdClickedEvent += OnAdClickedEvent;
            IronSourceBannerEvents.onAdLoadedEvent += OnAdLoadedEvent;
            IronSourceBannerEvents.onAdLeftApplicationEvent += OnAdLeftApplicationEvent;
            IronSourceBannerEvents.onAdLoadFailedEvent += OnAdLoadFailedEvent;
            IronSourceBannerEvents.onAdScreenDismissedEvent += OnAdScreenDismissedEvent;
            IronSourceBannerEvents.onAdScreenPresentedEvent += OnAdScreenPresentedEvent;
        }

        private void OnAdScreenPresentedEvent(IronSourceAdInfo obj)
        {
        }

        private void OnAdScreenDismissedEvent(IronSourceAdInfo obj)
        {
        }

        private void OnAdLoadFailedEvent(IronSourceError obj)
        {
            IsReady = false;
        }

        private void OnAdLeftApplicationEvent(IronSourceAdInfo obj)
        {
        }

        private void OnAdLoadedEvent(IronSourceAdInfo obj)
        {
            IsReady = true;

            if (_isWantToWatch)
            {
                Watch();
                _isWantToWatch = false;
            }
        }

        private void OnAdClickedEvent(IronSourceAdInfo obj)
        {
        }

        private void OnSdkInitializeComplete()
        {
            IronSource.Agent.loadBanner(IronSourceBannerSize.SMART, IronSourceBannerPosition.BOTTOM);
        }

        public void Watch(Action onSuccess, Action onError)
        {
            Watch();
        }

        public void Watch()
        {
            if (IsReady)
            {
                IronSource.Agent.displayBanner();
            }
            else
            {
                _isWantToWatch = true;
            }
        }

        public void UnWatch()
        {
            IronSource.Agent.hideBanner();
        }
    }
}