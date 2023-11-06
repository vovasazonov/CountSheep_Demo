using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Project.CoreDomain;
using Project.CoreDomain.Services.Content;
using Project.CoreDomain.Services.View;
using Project.GameDomain.ScreensDomain.MainDomain.Areas.Farm.Model;

namespace Project.GameDomain.ScreensDomain.MergeDomain.Areas.Farm.Presenter
{
    public class FarmPresenter : IPresenter
    {
        private readonly IFarmModel _farmModel;
        private readonly IViewService _viewService;
        private readonly IContentService _contentService;
        private readonly Dictionary<int, IPresenter> _presenters = new();

        public FarmPresenter(IFarmModel farmModel, IViewService viewService, IContentService contentService)
        {
            _farmModel = farmModel;
            _viewService = viewService;
            _contentService = contentService;
        }

        public UniTask InitializeAsync()
        {
            foreach (var id in _farmModel.Animals.Keys)
            {
                CreateAnimal(id);
            }

            _farmModel.Removed += OnRemoved;
            _farmModel.Added += OnAdded;
            
            return UniTask.CompletedTask;
        }

        private void CreateAnimal(int id)
        {
            var presenter = new AnimalPresenter(id, _viewService, _farmModel, _contentService);
            _presenters.Add(id, presenter);
            presenter.InitializeAsync();
        }

        public async UniTask DisposeAsync()
        {
            foreach (var presenter in _presenters.Values)
            {
                await presenter.DisposeAsync();
            }

            _presenters.Clear();
            
            _farmModel.Removed -= OnRemoved;
            _farmModel.Added -= OnAdded;
        }

        private void OnAdded(int id)
        {
            CreateAnimal(id);
        }

        private void OnRemoved(int id)
        {
            _presenters[id].DisposeAsync();
            _presenters.Remove(id);
        }
    }
}