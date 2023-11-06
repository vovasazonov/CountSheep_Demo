using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Project.CoreDomain;
using Project.GameDomain.ScreensDomain.MainDomain.Areas.Market.Model;
using Project.GameDomain.ScreensDomain.MainDomain.Areas.Market.View;

namespace Project.GameDomain.ScreensDomain.MainDomain.Areas.Market.Presenter
{
    public class MarketPresenter : IPresenter
    {
        private readonly IMarketView _view;
        private readonly IMarketModel _model;
        private readonly List<IPresenter> _presenters = new();

        public MarketPresenter(IMarketView view, IMarketModel model)
        {
            _view = view;
            _model = model;
        }

        public async UniTask InitializeAsync()
        {
            _presenters.Add(new MarketItemPresenter(_view.RedBundle, _model.RedBundle));
            _presenters.Add(new MarketItemPresenter(_view.PurpleBundle, _model.PurpleBundle));
            _presenters.Add(new MarketItemPresenter(_view.MiniBundle1, _model.MiniBundle1));
            _presenters.Add(new MarketItemPresenter(_view.MiniBundle2, _model.MiniBundle2));
            _presenters.Add(new MarketItemPresenter(_view.MiniBundle3, _model.MiniBundle3));
            _presenters.Add(new MarketItemPresenter(_view.MiniBundle4, _model.NoAdsBundle));

            foreach (var presenter in _presenters)
            {
                await presenter.InitializeAsync();
            }

            _model.VisibleUpdated += OnVisibleUpdated;
        }

        public async UniTask DisposeAsync()
        {
            foreach (var presenter in _presenters)
            {
                await presenter.DisposeAsync();
            }
            _presenters.Clear();
            
            _model.VisibleUpdated -= OnVisibleUpdated;
        }

        private void OnVisibleUpdated()
        {
            if (_model.IsOpen)
            {
                _view.Open();
            }
            else
            {
                _view.Close();
            }
        }
    }
}