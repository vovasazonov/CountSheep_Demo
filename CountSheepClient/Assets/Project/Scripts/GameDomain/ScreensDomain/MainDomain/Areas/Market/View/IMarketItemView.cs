using System;

namespace Project.GameDomain.ScreensDomain.MainDomain.Areas.Market.View
{
    public interface IMarketItemView
    {
        event Action BuyClicked;
        
        bool IsIncludeAds { set; }
        int AmountCoins { set; }
        string Price { set; }
        bool IsAvailable { set; }
    }
}