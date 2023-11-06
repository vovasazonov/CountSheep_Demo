using System;
using Project.CoreDomain.Services.Engine;
using Project.Scripts.CoreDomain.Services.Ads;

namespace Project.Scripts.CoreDomain.Services.AdsImplemantations.UnityAds
{
    public class UnityRewardAd : IAdUnit
    {
        public event Action ReadyUpdated;
        
        private float _leftSeconds;
        private Action _onSuccess;
        private Action _onError;
        private const float _waitingSeconds = 30f;
        
        public bool IsEnable { get; set; } = true;

        public bool IsReady => IronSource.Agent.isRewardedVideoAvailable();

        public UnityRewardAd(IEngineService engineService)
        {
            IronSourceEvents.onSdkInitializationCompletedEvent += OnSdkInitializeComplete;

            IronSourceRewardedVideoEvents.onAdClosedEvent += OnAdClosedEvent;
            IronSourceRewardedVideoEvents.onAdOpenedEvent += OnAdOpenedEvent;
            IronSourceRewardedVideoEvents.onAdLoadFailedEvent += OnAdLoadFailedEvent;
            IronSourceRewardedVideoEvents.onAdShowFailedEvent += OnAdShowFailedEvent;
            IronSourceRewardedVideoEvents.onAdRewardedEvent += OnAdRewardedEvent;
            IronSourceRewardedVideoEvents.onAdReadyEvent += OnAdReadyEvent;

            engineService.Updating += OnUpdating;
        }

        private void OnAdRewardedEvent(IronSourcePlacement arg1, IronSourceAdInfo arg2)
        {
           _onSuccess?.Invoke();
           _onSuccess = null;
           _onError = null;
        }

        private void OnUpdating()
        {
            _leftSeconds += UnityEngine.Time.deltaTime;

            if (_leftSeconds > _waitingSeconds && !IronSource.Agent.isRewardedVideoAvailable())
            {
                _leftSeconds = 0;
            }
        }

        private void OnAdShowFailedEvent(IronSourceError arg1, IronSourceAdInfo arg2)
        {
            _onError?.Invoke();
            _onError = null;
            _onSuccess = null;
        }

        private void OnAdReadyEvent(IronSourceAdInfo obj)
        {
            if (IronSource.Agent.isRewardedVideoAvailable())
            {
                ReadyUpdated?.Invoke();
            }
        }

        private void OnAdLoadFailedEvent(IronSourceError obj)
        {
        }

        private void OnAdOpenedEvent(IronSourceAdInfo obj)
        {
        }

        private void OnAdClosedEvent(IronSourceAdInfo obj)
        {
        }

        private void OnSdkInitializeComplete()
        {
            IronSource.Agent.loadRewardedVideo();
        }

        public void Watch(Action onSuccess, Action onError)
        {
            _onSuccess = onSuccess;
            _onError = onError;
            Watch();
        }

        public void Watch()
        {
            if (IronSource.Agent.isRewardedVideoAvailable())
            {
                IronSource.Agent.showRewardedVideo();
            }
            else
            {
                _onError?.Invoke();
                _onError = null;
                _onSuccess = null;
            }
        }
    }
}