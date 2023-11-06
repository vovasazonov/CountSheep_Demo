using System;
using Cysharp.Threading.Tasks;
using Project.CoreDomain;
using Project.CoreDomain.Services.Data;
using Project.CoreDomain.Services.Engine;
using Project.Scripts.CoreDomain.Services.Ads;

namespace Project.Scripts.CoreDomain.Services.AdsImplemantations.UnityAds
{
    public class UnityAdsService : ITaskAsyncInitializable, ITaskAsyncDisposable, IAdsService
    {
        private readonly IEngineService _engineService;
        private readonly IDataStorageService _dataStorageService;
        private const string _appId = "***SECRET***";

        public event Action HasNoAdsUpdated;

        public IBannerAdUnit Banner { get; }

        public IAdUnit Inter { get; }

        public IAdUnit Reward { get; }

        public bool HasNoAds
        {
            get => _dataStorageService.Contains("hasNoAds") && _dataStorageService.Get<PrimitiveData<bool>>("hasNoAds").Value;
            set
            {
                var data = _dataStorageService.Contains("hasNoAds")
                    ? _dataStorageService.Get<PrimitiveData<bool>>("hasNoAds")
                    : _dataStorageService.Create<PrimitiveData<bool>>("hasNoAds");
                
                data.Value = value;
                
                _dataStorageService.Save();

                HasNoAdsUpdated?.Invoke();
            }
        }

        public UnityAdsService(IEngineService engineService, IDataStorageService dataStorageService)
        {
            _engineService = engineService;
            _dataStorageService = dataStorageService;
            Inter = new UnityInterAd(engineService);
            Reward = new UnityRewardAd(engineService);
            Banner = new UnityBannerAd();
        }

        public UniTask InitializeAsync()
        {
            _engineService.Paused += OnPaused;
            _engineService.UnPaused += OnUnPaused;

            IronSourceEvents.onSdkInitializationCompletedEvent += OnSdkInitializeComplete;

            OsyaLogger.Log("Start initialize ADs");

            IronSource.Agent.setConsent(false); // GDRP
            IronSource.Agent.setMetaData("is_deviceid_optout", "true"); // it is for famility agrement
            IronSource.Agent.setMetaData("is_child_directed", "true"); // If you know how detect is child, set child true and for not child false, it is for famility agrement
            IronSource.Agent.init(_appId);
            IronSource.Agent.validateIntegration();

            Inter.IsEnable = true; //!HasNoAds;

            return UniTask.CompletedTask;
        }

        public UniTask DisposeAsync()
        {
            _engineService.Paused -= OnPaused;
            _engineService.UnPaused -= OnUnPaused;

            IronSourceEvents.onSdkInitializationCompletedEvent -= OnSdkInitializeComplete;

            return UniTask.CompletedTask;
        }

        private void OnSdkInitializeComplete()
        {
            OsyaLogger.Log("ADs Sdk Initialize Complete");
        }

        private void OnPaused()
        {
            IronSource.Agent.onApplicationPause(true);
        }

        private void OnUnPaused()
        {
            IronSource.Agent.onApplicationPause(false);
        }
    }
}