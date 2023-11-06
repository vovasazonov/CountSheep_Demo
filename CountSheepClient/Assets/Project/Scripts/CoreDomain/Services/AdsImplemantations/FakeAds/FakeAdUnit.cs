using System;
using Project.Scripts.CoreDomain.Services.Ads;

namespace Project.Scripts.CoreDomain.Services.AdsImplemantations.FakeAds
{
    public class FakeAdUnit : IBannerAdUnit, IAdUnit
    {
        public event Action ReadyUpdated;

        public bool IsEnable { get; set; } = true;
        public bool IsReady { get; } = true;
        public void Watch(Action onSuccess, Action onError)
        {
            
        }

        public void Watch()
        {
        }

        public void UnWatch()
        {
        }
    }
}