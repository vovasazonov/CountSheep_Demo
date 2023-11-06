namespace Project.GameDomain.ScreensDomain.MainDomain.Areas.Market.View
{
    public interface IMarketView
    {
        IMarketItemView RedBundle { get; }
        IMarketItemView PurpleBundle { get; }
        IMarketItemView MiniBundle1 { get; }
        IMarketItemView MiniBundle2 { get; }
        IMarketItemView MiniBundle3 { get; }
        IMarketItemView MiniBundle4 { get; }
        
        void Open();
        void Close();
    }
}