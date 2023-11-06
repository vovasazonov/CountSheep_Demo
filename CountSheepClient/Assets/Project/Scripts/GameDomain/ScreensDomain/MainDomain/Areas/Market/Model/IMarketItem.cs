using System;

namespace Project.GameDomain.ScreensDomain.MainDomain.Areas.Market.Model
{
    public interface IMarketItem
    {
        event Action Updated;
        
        string Id { get; }
        bool IsIncludeAds { get; }
        int Coins { get; }
        string Price { get; }
        bool IsAvailable { get; }

        void Buy();
        void CheckReceipt();
    }
}