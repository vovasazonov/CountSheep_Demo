using Cysharp.Threading.Tasks;
using Project.CoreDomain;
using Project.CoreDomain.Services.Content;
using Project.GameDomain.ScreensDomain.MainDomain.Areas.Collection.View;
using Project.GameDomain.ScreensDomain.MainDomain.Areas.Farm.Config;
using Project.GameDomain.ScreensDomain.MainDomain.Areas.Farm.Model;
using UnityEngine;

namespace Project.GameDomain.ScreensDomain.MainDomain.Areas.Collection.Presenter
{
    public class CardPresenter : IPresenter
    {
        private readonly ICardView _cardView;
        private readonly AnimalConfig _animalConfig;
        private readonly IFarmModel _farmModel;
        private readonly IContentService _contentService;

        public CardPresenter(
            ICardView cardView,
            AnimalConfig animalConfig,
            IFarmModel farmModel,
            IContentService contentService
        )
        {
            _cardView = cardView;
            _animalConfig = animalConfig;
            _farmModel = farmModel;
            _contentService = contentService;
        }

        public UniTask InitializeAsync()
        {
            _farmModel.CollectionUpdated += OnCollectionUpdated;
            _cardView.Name = _farmModel.GetName(_animalConfig.Id);
            _cardView.IsVisible = _farmModel.ContainsInCollection(_animalConfig.Id);
            SetImageAsync();
            return  UniTask.CompletedTask;
        }

        public UniTask DisposeAsync()
        {
            _farmModel.CollectionUpdated -= OnCollectionUpdated;
            return  UniTask.CompletedTask;
        }

        private void OnCollectionUpdated(string id)
        {
            if (_animalConfig.Id == id)
            {
                _cardView.IsVisible = true;
            }
        }

        private async UniTask SetImageAsync()
        {
            _cardView.Sprite = (await _contentService.LoadAsync<Sprite>(string.Format(MainScreenContentIds.PlayerIdleSprite, _animalConfig.Id))).Value;
        }
    }
}