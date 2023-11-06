using System;
using Project.CoreDomain.Services.Data;
using Project.CoreDomain.Services.Engine;
using Project.Scripts.CoreDomain.Services.Ads;

namespace Project.Scripts.CoreDomain.Services.AdsImplemantations.UnityAds
{
    public class UnityInterAd : IAdUnit
    {
        public event Action ReadyUpdated;
        
        private bool _isWantToWatch;
        private float _leftSeconds;
        private Action _onSuccess;
        private Action _onError;
        private const float _waitingSeconds = 30f;
        
        public bool IsEnable { get; set; } = true;

        public bool IsReady => IronSource.Agent.isInterstitialReady();

        public UnityInterAd(IEngineService engineService)
        {
            IronSourceEvents.onSdkInitializationCompletedEvent += OnSdkInitializeComplete;

            IronSourceInterstitialEvents.onAdClickedEvent += OnAdClickedEvent;
            IronSourceInterstitialEvents.onAdClosedEvent += OnAdClosedEvent;
            IronSourceInterstitialEvents.onAdOpenedEvent += OnAdOpenedEvent;
            IronSourceInterstitialEvents.onAdLoadFailedEvent += OnAdLoadFailedEvent;
            IronSourceInterstitialEvents.onAdShowFailedEvent += OnAdShowFailedEvent;
            IronSourceInterstitialEvents.onAdShowSucceededEvent += OnAdShowSucceededEvent;
            IronSourceInterstitialEvents.onAdReadyEvent += OnAdReadyEvent;

            engineService.Updating += OnUpdating;
        }

        private void OnUpdating()
        {
            _leftSeconds += UnityEngine.Time.deltaTime;

            if (_leftSeconds > _waitingSeconds && !IronSource.Agent.isInterstitialReady())
            {
                _leftSeconds = 0;
                OsyaLogger.Log("Start load inter AD");
                IronSource.Agent.loadInterstitial();
            }
        }

        private void OnAdShowFailedEvent(IronSourceError arg1, IronSourceAdInfo arg2)
        {
            _onError?.Invoke();
            _onError = null;
            _onSuccess = null;
            OsyaLogger.Log("Show failed inter AD");
        }

        private void OnAdReadyEvent(IronSourceAdInfo obj)
        {
            if (IronSource.Agent.isInterstitialReady())
            {
                OsyaLogger.Log("Inter AD ready");
                ReadyUpdated?.Invoke();
            }
        }

        private void OnAdShowSucceededEvent(IronSourceAdInfo obj)
        {
            _onSuccess?.Invoke();
            _onError = null;
            _onSuccess = null;
            OsyaLogger.Log("Show succeed inter AD");
        }

        private void OnAdLoadFailedEvent(IronSourceError obj)
        {
            OsyaLogger.Log("Load failed inter AD");
        }

        private void OnAdOpenedEvent(IronSourceAdInfo obj)
        {
        }

        private void OnAdClosedEvent(IronSourceAdInfo obj)
        {
        }

        private void OnAdClickedEvent(IronSourceAdInfo obj)
        {
        }

        private void OnSdkInitializeComplete()
        {
            OsyaLogger.Log("Start load inter AD");
            IronSource.Agent.loadInterstitial();
        }

        public void Watch(Action onSuccess, Action onError)
        {
            _onSuccess = onSuccess;
            _onError = onError;
            Watch();
        }

        public void Watch()
        {
            if (IronSource.Agent.isInterstitialReady() && IsEnable)
            {
                OsyaLogger.Log("Watch inter AD");
                IronSource.Agent.showInterstitial();
            }
            else
            {
                _onError?.Invoke();
                _onError = null;
                _onSuccess = null;

                if (IsEnable)
                {
                    OsyaLogger.Log("Inter not ready - No Watch inter AD");
                }
                else
                {
                    OsyaLogger.Log("Inter disabled");
                }
            }
        }
    }
}