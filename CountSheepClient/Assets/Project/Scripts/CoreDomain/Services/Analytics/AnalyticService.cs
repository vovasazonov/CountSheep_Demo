using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Unity.Services.Analytics;
using Unity.Services.Core;

namespace Project.CoreDomain.Services.Analytics
{
    public class AnalyticService : IAnalyticService, ITaskAsyncInitializable
    {
        private bool _isInitialized;

        public static IAnalyticService Instance { get; private set; }
        
        public async UniTask InitializeAsync()
        {
            Instance = this;

            Initialize();
        }

        private async UniTask Initialize()
        {
            await UnityServices.InitializeAsync();
            AnalyticsService.Instance.StartDataCollection();
            _isInitialized = true;
        }

        public void Track(string name, Dictionary<string, object> properties = null)
        {
            if (!_isInitialized)
            {
                OsyaLogger.LogError("Analytics not initializied but you try track events");
                return;
            }
            try
            {
                if (properties == null)
                {
                    AnalyticsService.Instance.CustomData(name);
                }
                else
                {
                    AnalyticsService.Instance.CustomData(name, properties);
                }
            }
            catch (Exception e)
            {
                OsyaLogger.LogError(e.Message);
            }
        }
    }
}