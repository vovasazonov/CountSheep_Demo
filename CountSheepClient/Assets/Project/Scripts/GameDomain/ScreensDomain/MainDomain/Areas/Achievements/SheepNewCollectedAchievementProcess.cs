using Cysharp.Threading.Tasks;
using Project.CoreDomain;
using Project.CoreDomain.Services.Achievement;
using Project.GameDomain.ScreensDomain.MainDomain.Areas.Farm.Model;

namespace Project.GameDomain.ScreensDomain.MainDomain.Areas.Achievements
{
    public class SheepNewCollectedAchievementProcess : IDomainTaskAsyncInitializable, IDomainTaskAsyncDisposable
    {
        private readonly IFarmModel _farmModel;
        private readonly IAchievementService _achievementService;

        public SheepNewCollectedAchievementProcess(IFarmModel farmModel, IAchievementService achievementService)
        {
            _farmModel = farmModel;
            _achievementService = achievementService;
        }

        public UniTask InitializeAsync()
        {
            _farmModel.Added += OnSheepAdded;
            _farmModel.CollectionUpdated += OnCollectionUpdated;
            _farmModel.Updated += OnSheepUpdated;
            return UniTask.CompletedTask;
        }

        public UniTask DisposeAsync()
        {
            _farmModel.Removed -= OnSheepAdded;
            _farmModel.CollectionUpdated -= OnCollectionUpdated;
            _farmModel.Updated -= OnSheepUpdated;
            return UniTask.CompletedTask;
        }

        private void OnSheepUpdated(int sheepId)
        {
            _achievementService.Unlock(_farmModel.Animals[sheepId]);
        }

        private void OnCollectionUpdated(string id)
        {
            _achievementService.Unlock(id);
        }

        private void OnSheepAdded(int sheepId)
        {
            _achievementService.Unlock(_farmModel.Animals[sheepId]);
        }
    }
}