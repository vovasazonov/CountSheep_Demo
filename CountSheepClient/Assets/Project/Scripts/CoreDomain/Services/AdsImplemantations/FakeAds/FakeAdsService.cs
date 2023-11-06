using System;
using Project.Scripts.CoreDomain.Services.Ads;

namespace Project.Scripts.CoreDomain.Services.AdsImplemantations.FakeAds
{
    public class FakeAdsService : IAdsService
    {
        private readonly FakeAdUnit _fakeAdUnit = new FakeAdUnit();

        public bool IsAudioMuted { get; set; }
        public event Action HasNoAdsUpdated;
        public IBannerAdUnit Banner => _fakeAdUnit;
        public IAdUnit Inter => _fakeAdUnit;
        public IAdUnit Reward => _fakeAdUnit;
        public bool HasNoAds { get; set; }

        public void Initialize()
        {
        }
    }
}