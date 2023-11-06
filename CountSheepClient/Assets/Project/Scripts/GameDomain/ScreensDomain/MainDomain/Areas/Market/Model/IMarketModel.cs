using System;

namespace Project.GameDomain.ScreensDomain.MainDomain.Areas.Market.Model
{
    public interface IMarketModel
    {
        event Action VisibleUpdated;

        bool IsOpen { get; }
        
        IMarketItem RedBundle { get; }
        IMarketItem PurpleBundle { get; }
        IMarketItem MiniBundle1 { get; }
        IMarketItem MiniBundle2 { get; }
        IMarketItem MiniBundle3 { get; }
        IMarketItem NoAdsBundle { get; }

        void Open();
        void Close();
    }
}