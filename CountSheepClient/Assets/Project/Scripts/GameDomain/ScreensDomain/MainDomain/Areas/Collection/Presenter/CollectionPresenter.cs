using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Project.CoreDomain;
using Project.CoreDomain.Services.Content;
using Project.GameDomain.ScreensDomain.MainDomain.Areas.Collection.View;
using Project.GameDomain.ScreensDomain.MainDomain.Areas.Farm.Config;
using Project.GameDomain.ScreensDomain.MainDomain.Areas.Farm.Model;

namespace Project.GameDomain.ScreensDomain.MainDomain.Areas.Collection.Presenter
{
    public class CollectionPresenter : IPresenter
    {
        private readonly ICollectionView _view;
        private readonly IFarmModel _farmModel;
        private readonly IFarmConfigKeeper _farmConfigKeeper;
        private readonly IContentService _contentService;
        private readonly List<IPresenter> _presenters = new();

        public CollectionPresenter(ICollectionView view, IFarmModel farmModel, IFarmConfigKeeper farmConfigKeeper, IContentService contentService)
        {
            _view = view;
            _farmModel = farmModel;
            _farmConfigKeeper = farmConfigKeeper;
            _contentService = contentService;
        }

        public async UniTask InitializeAsync()
        {
            foreach (var animalConfig in _farmConfigKeeper.FarmConfig.Animals)
            {
                var view = _view.Create();
                var cardPresenter = new CardPresenter(view, animalConfig, _farmModel, _contentService);
                await cardPresenter.InitializeAsync();
                _presenters.Add(cardPresenter);
            }
        }

        public async UniTask DisposeAsync()
        {
            foreach (var presenter in _presenters)
            {
                await presenter.DisposeAsync();
            }
            _presenters.Clear();
        }
    }
}