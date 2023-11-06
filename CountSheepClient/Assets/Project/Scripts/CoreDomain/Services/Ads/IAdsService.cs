using System;

namespace Project.Scripts.CoreDomain.Services.Ads
{
    public interface IAdsService
    {
        event Action HasNoAdsUpdated;
        
        IBannerAdUnit Banner { get; }
        IAdUnit Inter { get; }
        IAdUnit Reward { get; }
        
        bool HasNoAds { get; set; }
    }
}