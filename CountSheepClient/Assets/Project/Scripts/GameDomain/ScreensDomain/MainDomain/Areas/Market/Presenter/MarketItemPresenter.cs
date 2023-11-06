using Cysharp.Threading.Tasks;
using Project.CoreDomain;
using Project.GameDomain.ScreensDomain.MainDomain.Areas.Market.Model;
using Project.GameDomain.ScreensDomain.MainDomain.Areas.Market.View;

namespace Project.GameDomain.ScreensDomain.MainDomain.Areas.Market.Presenter
{
    public class MarketItemPresenter : IPresenter
    {
        private readonly IMarketItemView _view;
        private readonly IMarketItem _model;

        public MarketItemPresenter(IMarketItemView view, IMarketItem model)
        {
            _view = view;
            _model = model;
        }

        public async UniTask InitializeAsync()
        {
            _view.BuyClicked += OnBuyClicked;
            _model.Updated += OnModelUpdated;
            RenderView();
        }

        private void RenderView()
        {
            _view.IsAvailable = _model.IsAvailable;
            if (_model.Coins != 0)
            {
                _view.AmountCoins = _model.Coins;
            }
            _view.IsIncludeAds = _model.IsIncludeAds;
            _view.Price = _model.Price;
        }

        public async UniTask DisposeAsync()
        {
            _view.BuyClicked -= OnBuyClicked;
            _model.Updated -= OnModelUpdated;
        }

        private void OnModelUpdated()
        {
            RenderView();
        }

        private void OnBuyClicked()
        {
            _model.Buy();
        }
    }
}