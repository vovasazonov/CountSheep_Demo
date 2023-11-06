using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Project.CoreDomain;
using Project.CoreDomain.Services.Content;
using Project.CoreDomain.Services.View;
using Project.GameDomain.ScreensDomain.MainDomain;
using Project.GameDomain.ScreensDomain.MainDomain.Areas.Farm.Model;
using Project.GameDomain.ScreensDomain.MergeDomain.Areas.Farm.View;
using UnityEngine.U2D.Animation;

namespace Project.GameDomain.ScreensDomain.MergeDomain.Areas.Farm.Presenter
{
    public class AnimalPresenter : IPresenter
    {
        private readonly int _id;
        private readonly IViewService _viewService;
        private readonly IFarmModel _farmModel;
        private readonly IContentService _contentService;
        private IDisposableView<AnimalView> _view;
        private bool _isInitialized;
        private IContentKeeper<SpriteLibraryAsset> _spriteLibraryKeeper;

        public AnimalPresenter(int id, IViewService viewService, IFarmModel farmModel, IContentService contentService)
        {
            _id = id;
            _viewService = viewService;
            _farmModel = farmModel;
            _contentService = contentService;
        }

        public async UniTask InitializeAsync()
        {
            _view = await _viewService.CreateAsync<AnimalView>(MergeScreenContentIds.AnimalPrefab);
            _view.Value.IsVisible = false;
            _view.Value.Id = _id;
            _isInitialized = true;
            _farmModel.Updated += OnUpdate;
            _farmModel.CollectionUpdated += OnCollectionUpdated;
            _view.Value.Contacted += OnViewsContacted;
            await LoadSpriteAtlasAssets();
            _view.Value.IsVisible = true;
            UpdateSpriteAtlasAssets();
        }

        public async UniTask DisposeAsync()
        {
            while (!_isInitialized)
            {
                await UniTask.DelayFrame(1);
            }

            _spriteLibraryKeeper?.Dispose();
            _spriteLibraryKeeper = null;
            _farmModel.Updated -= OnUpdate;
            _farmModel.CollectionUpdated -= OnCollectionUpdated;
            _view.Value.Contacted -= OnViewsContacted;
            _view?.Dispose();
            _view = null;
        }

        private void OnCollectionUpdated(string id)
        {
            if (id == _farmModel.Animals[_id])
            {
                _view.Value.PlayNewCollectionEffect();
            }
        }

        private void OnViewsContacted(List<int> secondId)
        {
            _farmModel.TryMerge(_id, secondId);
        }

        private async void OnUpdate(int id)
        {
            if (id == _id)
            {
                _view.Value.MergeEffect();
                await LoadSpriteAtlasAssets();
                UpdateSpriteAtlasAssets();
            }
        }

        private void UpdateSpriteAtlasAssets()
        {
            _view.Value.SetSpriteLibrary(_spriteLibraryKeeper.Value);
            _view.Value.IsMax = _farmModel.IsAnimalMaxLevel(_id);
        }

        private async UniTask LoadSpriteAtlasAssets()
        {
            _spriteLibraryKeeper?.Dispose();
            _spriteLibraryKeeper = await _contentService.LoadAsync<SpriteLibraryAsset>(string.Format(MainScreenContentIds.PlayerSpriteLibrary, _farmModel.Animals[_id]));
        }
    }
}